using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Linq;
using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Infrastructure.Soap;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.TransUnion;

/// <summary>
/// E7 — TransUnion DecisionCentre (<c>ExternalSolutionExecution.svc</c>). Un único método SOAP,
/// <c>ExecuteXMLString</c>, transporta un <c>DCRequest</c> como texto y devuelve un <c>DCResponse</c>
/// como texto. El mock despacha por <c>RequestInfo</c> y guarda el estado por <c>ApplicationId</c>
/// (IDVision → OTP → validación; AQM score → historial). Contrato: docs/providers/E07-transunion.md.
/// </summary>
public static class TransUnionMock
{
    public const string Provider = "TransUnion";
    public const string Operation = "ExecuteXMLString";
    private const string ActionPrefix = "http://tempuri.org/IExternalSolutionExecution/";

    public static readonly IReadOnlyList<ScenarioInfo> Scenarios =
    [
        new("auth-fallida", "Authentication/Status=Failure y Status=Failure en cualquier paso."),
        new("muerte-titular", "Inicio IDVision con Applicant/DocumentState='MUERTE POR TITULAR': GA2 rechaza con ese mensaje."),
        new("fuera-de-cola", "Envío de OTP con ErrorInfo 'Application does not exists in Queue PhoneSelection.' (codigoRechazo=1)."),
        new("sin-otp", "Envío de OTP exitoso pero sin los campos OTPCode/Delay (GA2: NullReferenceException capturado → codigoRechazo=1)."),
        new("pin-incorrecto", "Validación del PIN con código 302 aunque el PIN sea el válido."),
        new("pendiente", "AQM/prospección con DecisionHomologada=PENDIENTE."),
        new("no-viable", "AQM/prospección con DecisionHomologada='NO VIABLE' (GA2: tipoRechazo='Politicas')."),
        new("sin-applicationdata", "AQM sin Field ApplicationData (GA2: 'Error en la Conexión con Buro.')."),
    ];

    private static readonly string[] UnusedOperations =
    [
        "AuthenticateXMLString", "ChangePasswordXMLString", "DownloadDocument", "ExecuteXMLBinary", "GetFinalTrace",
        "GetQueueAppCountXMLString", "GetAppCountByField", "GetQueueAppCount", "GetSolutionQueueNames", "LockApplicationXMLString",
        "LogoutSessionXMLString", "ReadApplicationAllFieldsById", "ReadApplicationDataById", "ReadApplicationDataByIdAndQueue",
        "ReadApplicationFieldsById", "ReadApplicationFieldsByIdAndQueue", "ReassignApplicationXMLString", "RemoveDocument",
        "ResetPasswordByUserXMLString", "RetrieveAppsByRangeXMLString", "RetrieveDocumentMetaDataXMLString",
        "RetrieveSecretQuestionXMLString", "RetrieveSSXMLString", "RetrieveXMLString", "SaveXMLString", "SearchApplication",
        "SearchApplicationXMLString", "UnlockApplicationXMLString", "UploadDocumentXMLString", "GetDvCaseStatusHistory",
        "ViewApplicationHistoryXMLString", "ViewApplicationXMLString",
    ];

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "TransUnion_IExternalSolutionExecution",
        Contract = "IExternalSolutionExecution",
        Paths =
        [
            "/TU.IDS.ExternalServices_latam_prod/SolutionExecution/ExternalSolutionExecution.svc",
            "/TU.IDS.ExternalServices_latam_uat/SolutionExecution/ExternalSolutionExecution.svc",
        ],
        Description = "Verificación de identidad (IDVision + OTP por SMS), score/capacidad de endeudamiento (AQM) e historial crediticio. "
            + "Pasos lógicos sobre ExecuteXMLString: (1) inicio IDVision `NewWithContext` + SolutionSetId 142; (2) envío OTP `Send`/PhoneSelection; "
            + "(3) reenvío `Send`/PinVerification_OTPInput + Action=ResendOTP; (4) validación `Send`/PinVerification_OTPInput + PinNumber; "
            + "(5) AQM/prospección `NewWithContext` + SolutionSetId 360; (6) historial `Send`/DocVerify + GenerateReport=Y.",
        Ga2ConfigKey = "No configurable: URL fija en GA2.Application/Main/TransUnion/ValidacionesTU.cs:16 (HTTPS, producción)",
        Operations =
        [
            new SoapOperation
            {
                Name = Operation,
                SoapAction = ActionPrefix + Operation,
                Usage = Ga2Usage.Business,
                Summary = "DCRequest (texto) → DCResponse (texto). Estado por ApplicationId; PIN válido configurable (MockIntegrations:TransUnion:ValidPin).",
                Ga2Source = "GA2.Application/Main/TransUnion/ValidacionesTU.cs ← GestorIdVisionBusinessLogic, GestorTransUnionBusinessLogic",
                Scenarios = Scenarios,
                Certainty = ContractCertainty.StructureOnly,
                Notes = "Respuestas validadas con los deserializadores de GA2; el contenido real de cada DCResponse debe confirmarse con muestras (tablas RegistroLogTUSimulador / VLL_VALIDACION_LOG_OTP).",
                Handler = HandleAsync,
            },
            .. UnusedOperations.Select(name => new SoapOperation
            {
                Name = name,
                SoapAction = ActionPrefix + name,
                Usage = Ga2Usage.NotUsed,
                Summary = "En el contrato, sin llamadas en GA2. Responde SOAP Fault.",
                Certainty = ContractCertainty.NotImplemented,
            }),
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);

    private static Task<SoapReply> HandleAsync(SoapContext context)
    {
        var services = context.Http.RequestServices;
        var state = services.GetRequiredService<TransUnionState>();
        var options = services.GetRequiredService<IOptionsMonitor<MockOptions>>().CurrentValue.TransUnion;

        DcRequest request;
        try
        {
            request = DcRequest.Parse(context.Value("request") ?? "");
        }
        catch (XmlException ex)
        {
            return Task.FromResult(SoapReply.Xml(Envelope(DcResponse.Failure(null, $"MockIntegrations: DCRequest no es XML válido ({ex.Message})"))));
        }

        var session = request.ApplicationId is { } id ? state.Find(id) : null;

        // Las reglas de escenario pueden usar campos del DCRequest (IdNumber, QueueName, PinNumber...)
        // y, en los pasos siguientes, los datos guardados del inicio de la transacción.
        var scenario = context.Scenario;
        if (scenario.Source is ScenarioSource.Default or ScenarioSource.ConfiguredDefault or ScenarioSource.Rule)
        {
            var resolver = services.GetRequiredService<ScenarioResolver>();
            var decision = resolver.Resolve(context.Http, Provider, Operation, key => request.Lookup(key) ?? session?.Lookup(key));
            if (decision.Source == ScenarioSource.Default && session?.Scenario is { } inherited)
            {
                decision = new ScenarioDecision(inherited, ScenarioSource.Rule, "heredado del inicio de la transacción");
            }

            scenario = decision;
        }

        MockCallInfo.Set(context.Http, Provider, $"{Operation}:{request.Step}", scenario.Name, scenario.Source.ToString());

        var name = scenario.Name;
        if (name == BuiltInScenarios.Fault)
        {
            return Task.FromResult(SoapReply.Fault($"Error técnico simulado por MockIntegrations en {Provider}/{Operation}."));
        }

        if (name == BuiltInScenarios.Http500)
        {
            return Task.FromResult(SoapReply.Html500());
        }

        string response;
        if (name == "auth-fallida")
        {
            response = DcResponse.AuthFailure(request.ApplicationId);
        }
        else
        {
            response = request.Step switch
            {
                TuStep.IdVisionStart => IdVisionStart(state, options, request, name),
                TuStep.SendOtp => session is null || name == "fuera-de-cola"
                    ? DcResponse.Failure(request.ApplicationId, "Application does not exists in Queue PhoneSelection.")
                    : DcResponse.Success(session.ApplicationId, "PinVerification_OTPInput",
                        name == "sin-otp" ? [] : [("OTPCode", "SENT"), ("Delay", "60")]),
                TuStep.ResendOtp => session is null
                    ? DcResponse.Failure(request.ApplicationId, "Application does not exists in Queue PinVerification_OTPInput.")
                    : DcResponse.Success(session.ApplicationId, "PinVerification_OTPInput", [("OTPCode", "SENT"), ("Delay", "60")]),
                TuStep.ValidatePin => session is null
                    ? DcResponse.Failure(request.ApplicationId, "Application does not exists in Queue PinVerification_OTPInput.")
                    : ValidatePin(session, request, options, name),
                TuStep.AqmStart => AqmStart(state, options, request, name),
                TuStep.History => session is null
                    ? DcResponse.Failure(request.ApplicationId, "Application does not exists in Queue DocVerify.")
                    : History(session, name),
                _ => DcResponse.Failure(request.ApplicationId, $"MockIntegrations: combinación no soportada (ExecutionMode={request.ExecutionMode}, QueueName={request.QueueName}, SolutionSetId={request.SolutionSetId})."),
            };
        }

        return Task.FromResult(SoapReply.Xml(Envelope(response)));
    }

    private static string IdVisionStart(TransUnionState state, TransUnionOptions options, DcRequest request, string scenario)
    {
        var session = state.Start(options.FirstApplicationId, request, scenario);
        var documentState = scenario == "muerte-titular" ? "MUERTE POR TITULAR" : "VIGENTE";
        var applicants = new XElement("Applicants", new XElement("Applicant",
            new XElement("IdType", "0"),
            new XElement("IdNumber", request.Applicant("IdNumber") ?? ""),
            new XElement("DocumentState", documentState)));
        return DcResponse.Success(session.ApplicationId, "PhoneSelection", [("HasError", "False"), ("Applicants", Xml(applicants))]);
    }

    private static string ValidatePin(TuSession session, DcRequest request, TransUnionOptions options, string scenario)
    {
        var valid = scenario != "pin-incorrecto" && request.Field("PinNumber") == options.ValidPin;
        var (code, description) = valid ? ("300", "OTP valido") : ("302", "OTP invalido");
        var data = new XElement("ApplicationData", new XElement("OTPReasons", new XElement("Reason",
            new XElement("Code", code), new XElement("Type", "OTP"), new XElement("Description", description))));
        return DcResponse.Success(session.ApplicationId, "End", [("ApplicationData", Xml(data))]);
    }

    private static string AqmStart(TransUnionState state, TransUnionOptions options, DcRequest request, string scenario)
    {
        var session = state.Start(options.FirstApplicationId, request, scenario);
        if (scenario == "sin-applicationdata")
        {
            return DcResponse.Success(session.ApplicationId, "End", []);
        }

        var decision = scenario switch
        {
            "no-viable" => "NO VIABLE",
            "pendiente" => "PENDIENTE",
            _ => "VIABLE",
        };
        var data = new XElement("ApplicationData", new XElement("Product", request.ApplicationDataProduct ?? "CONSUMO"), new XElement("DecisionHomologada", decision));
        if (decision == "NO VIABLE")
        {
            return DcResponse.Success(session.ApplicationId, "End", [("ApplicationData", Xml(data))]);
        }

        return DcResponse.Success(session.ApplicationId, "DocVerify", [("ApplicationData", Xml(data)), ("Applicants", Xml(ScoreApplicants(withHistory: false)))]);
    }

    private static string History(TuSession session, string scenario)
    {
        var decision = scenario == "no-viable" ? "NO VIABLE" : "VIABLE";
        return DcResponse.Success(session.ApplicationId, "End", [("Applicants", Xml(ScoreApplicants(withHistory: true))), ("DecisionHomologada", decision)]);
    }

    // Punto decimal y sin separador de miles: GA2 hace decimal.Parse en cultura en-US.
    private static XElement ScoreApplicants(bool withHistory) =>
        new("Applicants", new XElement("Applicant",
            new XElement("IdType", "0"),
            new XElement("Score", "750"),
            new XElement("CapacidadEndeudamiento", "1500000.50"),
            new XElement("PorcEndFinal", "35.5"),
            withHistory ? new XElement("DSOnlineBureauCOLData") : null));

    private static string Xml(XElement element) => element.ToString(SaveOptions.DisableFormatting);

    private static string Envelope(string dcResponse)
    {
        XNamespace s = SoapEnvelope.Soap11;
        XNamespace t = "http://tempuri.org/";
        var envelope = new XElement(s + "Envelope", new XAttribute(XNamespace.Xmlns + "s", s),
            new XElement(s + "Body",
                new XElement(t + "ExecuteXMLStringResponse", new XAttribute("xmlns", t.NamespaceName),
                    new XElement(t + "ExecuteXMLStringResult", dcResponse))));
        return envelope.ToString(SaveOptions.DisableFormatting);
    }
}

public enum TuStep
{
    Unknown,
    IdVisionStart,
    SendOtp,
    ResendOtp,
    ValidatePin,
    AqmStart,
    History,
}

/// <summary><c>DCRequest</c> leído del texto de <c>&lt;request&gt;</c> (acepta Field con CDATA o con XML escapado).</summary>
public sealed class DcRequest
{
    private readonly XElement _root;

    private DcRequest(XElement root) => _root = root;

    public static DcRequest Parse(string xml) => new(XElement.Parse(xml));

    public string? SolutionSetId => Info("SolutionSetId");

    public string? ExecutionMode => Info("ExecutionMode");

    public string? ApplicationId => Info("ApplicationId");

    public string? QueueName => Info("QueueName");

    public string? ApplicationDataProduct => Inner("ApplicationData")?.Elements().FirstOrDefault(e => e.Name.LocalName == "Product")?.Value;

    public TuStep Step => (ExecutionMode, QueueName) switch
    {
        ("NewWithContext", _) when SolutionSetId == "142" => TuStep.IdVisionStart,
        ("NewWithContext", _) => TuStep.AqmStart,
        ("Send", "PhoneSelection") => TuStep.SendOtp,
        ("Send", "PinVerification_OTPInput") when Field("Action") == "ResendOTP" => TuStep.ResendOtp,
        ("Send", "PinVerification_OTPInput") => TuStep.ValidatePin,
        ("Send", "DocVerify") => TuStep.History,
        _ => TuStep.Unknown,
    };

    public string? Field(string key) =>
        _root.Descendants().FirstOrDefault(e => e.Name.LocalName == "Field" && (string?)e.Attribute("key") == key)?.Value;

    public string? Applicant(string name) =>
        Inner("PrimaryApplicant")?.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value;

    /// <summary>Busca en RequestInfo, en los Field por clave y en el Applicant (para las reglas de escenario).</summary>
    public string? Lookup(string key) => Info(key) ?? Field(key) ?? Applicant(key);

    private string? Info(string name) =>
        _root.Elements().FirstOrDefault(e => e.Name.LocalName == "RequestInfo")?.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value;

    private XElement? Inner(string fieldKey)
    {
        var text = Field(fieldKey);
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        try
        {
            return XElement.Parse(text);
        }
        catch (XmlException)
        {
            return null;
        }
    }
}

public static class DcResponse
{
    public static string Success(string applicationId, string currentQueue, IEnumerable<(string Key, string Value)> fields) =>
        Build("Success", "Success", applicationId, currentQueue, fields, null);

    public static string Failure(string? applicationId, string message) =>
        Build("Failure", "Success", applicationId, null, [], message);

    public static string AuthFailure(string? applicationId) =>
        Build("Failure", "Failure", applicationId, null, [], "Authentication failed (MockIntegrations).");

    private static string Build(string status, string authStatus, string? applicationId, string? currentQueue, IEnumerable<(string Key, string Value)> fields, string? error)
    {
        var context = new XElement("ContextData", fields.Select(f => new XElement("Field", new XAttribute("key", f.Key), f.Value)));
        var root = new XElement("DCResponse",
            new XElement("Status", status),
            new XElement("Authentication", new XElement("Status", authStatus), new XElement("Token", "token-tu-mock")),
            new XElement("ResponseInfo",
                new XElement("ApplicationId", applicationId ?? ""),
                new XElement("SolutionSetInstanceId", "mock-" + (applicationId ?? "0")),
                currentQueue is null ? null : new XElement("CurrentQueue", currentQueue)),
            context,
            error is null ? null : new XElement("ErrorInfo", new XElement("Message", error)));
        return root.ToString(SaveOptions.DisableFormatting);
    }
}

public sealed record TuSession(string ApplicationId, string? IdNumber, string? Phone, string? Scenario)
{
    public string? Lookup(string key) => key.ToUpperInvariant() switch
    {
        "IDNUMBER" => IdNumber,
        "RECENTPHONENUMBER" => Phone,
        _ => null,
    };
}

/// <summary>Transacciones TransUnion abiertas, por ApplicationId (en memoria).</summary>
public sealed class TransUnionState(SequenceStore sequences)
{
    private readonly ConcurrentDictionary<string, TuSession> _sessions = new();

    public TuSession Start(long firstApplicationId, DcRequest request, string scenario)
    {
        var id = sequences.Next("transunion.applicationid", firstApplicationId).ToString(System.Globalization.CultureInfo.InvariantCulture);
        var session = new TuSession(id, request.Applicant("IdNumber"), request.Applicant("RecentPhoneNumber"),
            scenario == BuiltInScenarios.Default ? null : scenario);
        _sessions[id] = session;
        return session;
    }

    public TuSession? Find(string applicationId) => _sessions.TryGetValue(applicationId, out var s) ? s : null;

    public void Reset() => _sessions.Clear();
}
