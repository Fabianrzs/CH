using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Rest;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Providers.Haberes;
using MockIntegrations.Api.Providers.KtrionApi.Models;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.KtrionApi;

/// <summary>
/// E10 — Pasarela REST de Caja Honor "ktrionApi" (<c>CHKeys:UrlCajaHonor</c>). GA2 pide un token en
/// <c>api/autenticacion</c> antes de <b>cada</b> llamada y lo envía como <c>Authorization: Bearer</c>.
/// Rutas activas: <c>workflowattachfiles</c> (siempre) y <c>forminsert</c>/<c>formUpdate</c>/<c>generarreportehaberes</c>
/// (sólo con GA2 compilado en DEBUG). Contrato: docs/providers/E10-ktrionapi.md.
/// </summary>
public static partial class KtrionApiMock
{
    public const string Provider = "KtrionApi";
    public const string Base = "/REST/KriterionDev/ktrionApi/";
    private const string ConfigKey = "CHKeys:UrlCajaHonor";
    private const string Source = "GA2.Application/Main/ProviderRequest/ProviderRequestFactory.cs ← GA2.Domain/Core/IntegracionesGA2/*";

    private static readonly ScenarioInfo Unauthorized = new("unauthorized", "HTTP 401: GA2 lanza Exception(body) → HTTP 409 al frontend.");
    private static readonly ScenarioInfo Fallido = new("fallido", "HTTP 200 con el resultado WorkManager Success=false.");

    private static readonly RestOperationSpec Auth = Spec("Autenticacion", "Obtiene el token de la pasarela", Ga2Usage.Business,
        "Credenciales de `AppMain:*` (el mock no valida la contraseña). GA2 usa `Data.Token` sin validarlo; si `Data` es nulo hay NullReferenceException.",
        [Unauthorized, new("sin-token", "IsSuccess=true con Data=null: reproduce el NullReferenceException de GA2.")]);

    private static readonly RestOperationSpec FormInsert = Spec("FormInsert", "Radica en WorkManager a través de la pasarela (sólo DEBUG)", Ga2Usage.Business,
        "Cuerpo: proxy `Form_InsertRequest` serializado. GA2 lee `Data.Form_InsertResult.BarCode`.", [Fallido, Unauthorized]);

    private static readonly RestOperationSpec FormUpdate = Spec("FormUpdate", "Actualiza un formulario WorkManager a través de la pasarela (sólo DEBUG)", Ga2Usage.Business,
        "Cuerpo: proxy `Form_UpdateRequest` serializado. Ruta con mayúscula: `api/formUpdate`.", [Fallido, Unauthorized]);

    private static readonly RestOperationSpec WorkflowAttach = Spec("WorkflowAttachFiles", "Adjunta archivos al workflow de crédito (siempre)", Ga2Usage.Business,
        "`GestorDocumentalBusinessLogic.AttachFiles` la usa en Debug y en Release.", [Fallido, Unauthorized]);

    private static readonly RestOperationSpec Haberes = Spec("GenerarReporteHaberes", "Certificado de haberes en PDF Base64 (sólo DEBUG)", Ga2Usage.Business,
        "`Data` = PDF en Base64 (el mismo documento de prueba que el SOAP de Haberes).",
        [Unauthorized, new("sin-reporte", "IsSuccess=false con Data=null (GA2 falla al guardar en Blob).")]);

    private static readonly string[] UnusedRoutes =
        ["api/formdelete", "api/formgetdata", "api/formgetfilecodes", "api/formgetbase64stringfile", "api/listgetitems", "api/obtenerafiliadobua"];

    public static void Map(WebApplication app)
    {
        app.MapMock("POST", Base + "api/autenticacion", Auth, async (HttpContext http, LoginIntegracionesRequest request, KtrionApiTokens tokens) =>
            {
                var decision = await RestMock.BeginAsync(http, Auth, RestMock.Lookup(http, request));
                return RestMock.BuiltIn(decision, Auth) ?? decision.Name switch
                {
                    "default" => Json(new GatewayResponse<TokenData> { ReturnMessage = "ok", Data = new TokenData { Token = tokens.Issue() } }),
                    "sin-token" => Json(new GatewayResponse<TokenData> { ReturnMessage = "ok", Data = null }),
                    "unauthorized" => UnauthorizedText(),
                    _ => RestMock.UnknownScenario(decision, Auth),
                };
            })
            .Accepts<LoginIntegracionesRequest>("application/json")
            .Produces<GatewayResponse<TokenData>>()
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain");

        app.MapMock("POST", Base + "api/forminsert", FormInsert, async (HttpContext http, FormInsertGatewayRequest request, SequenceStore seq, KtrionApiTokens tokens, IOptionsMonitor<MockOptions> options) =>
            {
                var decision = await RestMock.BeginAsync(http, FormInsert, RestMock.Lookup(http, request));
                if (Guard(http, decision, FormInsert, tokens, options) is { } early)
                {
                    return early;
                }

                var formCode = string.IsNullOrEmpty(request.Request?.FormCode) ? "83" : request.Request!.FormCode;
                return Json(new GatewayResponse<FormInsertGatewayResult>
                {
                    ReturnMessage = "ok",
                    Data = new() { FormInsertResult = InsertResult(seq, decision, $"{formCode}-{seq.Next("ktrion.barcode", 123):D6}", request.Request?.Files?.Count ?? 0, request.Request?.CheckWorkflow == true ? (int)seq.Next("ktrion.workflowid", 9981) : 0) },
                });
            })
            .Accepts<FormInsertGatewayRequest>("application/json")
            .Produces<GatewayResponse<FormInsertGatewayResult>>()
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain");

        app.MapMock("POST", Base + "api/formUpdate", FormUpdate, async (HttpContext http, FormUpdateGatewayRequest request, KtrionApiTokens tokens, IOptionsMonitor<MockOptions> options) =>
            {
                var decision = await RestMock.BeginAsync(http, FormUpdate, RestMock.Lookup(http, request));
                if (Guard(http, decision, FormUpdate, tokens, options) is { } early)
                {
                    return early;
                }

                var ok = decision.Name == BuiltInScenarios.Default;
                return Json(new GatewayResponse<FormUpdateGatewayResult>
                {
                    ReturnMessage = "ok",
                    Data = new()
                    {
                        FormUpdateResult = new ResponseDto
                        {
                            Data = request.Request?.Data ?? [],
                            Message = ok ? "Registro actualizado" : "Error funcional simulado por MockIntegrations (formUpdate)",
                            Rows = ok ? 1 : 0,
                            Success = ok,
                        },
                    },
                });
            })
            .Accepts<FormUpdateGatewayRequest>("application/json")
            .Produces<GatewayResponse<FormUpdateGatewayResult>>()
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain");

        app.MapMock("POST", Base + "api/workflowattachfiles", WorkflowAttach, async (HttpContext http, WorkflowAttachFilesGatewayRequest request, SequenceStore seq, KtrionApiTokens tokens, IOptionsMonitor<MockOptions> options) =>
            {
                var decision = await RestMock.BeginAsync(http, WorkflowAttach, RestMock.Lookup(http, request));
                if (Guard(http, decision, WorkflowAttach, tokens, options) is { } early)
                {
                    return early;
                }

                return Json(new GatewayResponse<WorkflowAttachFilesGatewayResult>
                {
                    ReturnMessage = "ok",
                    Data = new() { WorkflowAttachFilesResult = InsertResult(seq, decision, null, request.Request?.Files?.Count ?? 0, (int)(request.Request?.WorkflowId ?? 0)) },
                });
            })
            .Accepts<WorkflowAttachFilesGatewayRequest>("application/json")
            .Produces<GatewayResponse<WorkflowAttachFilesGatewayResult>>()
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain");

        app.MapMock("POST", Base + "api/generarreportehaberes", Haberes, async (HttpContext http, HaberesGatewayRequest request, FixtureStore fixtures, KtrionApiTokens tokens, IOptionsMonitor<MockOptions> options) =>
            {
                var decision = await RestMock.BeginAsync(http, Haberes, RestMock.Lookup(http, request));
                if (Guard(http, decision, Haberes, tokens, options, allowed: ["sin-reporte"]) is { } early)
                {
                    return early;
                }

                return decision.Name == "sin-reporte"
                    ? Json(new GatewayResponse<string> { IsSuccess = false, ReturnMessage = "No se encontró información para el reporte", Data = null })
                    : Json(new GatewayResponse<string> { ReturnMessage = "ok", Data = SamplePdf(fixtures) });
            })
            .Accepts<HaberesGatewayRequest>("application/json")
            .Produces<GatewayResponse<string>>()
            .Produces<string>(StatusCodes.Status401Unauthorized, "text/plain");

        foreach (var route in UnusedRoutes)
        {
            var name = route["api/".Length..];
            var spec = Spec(name, $"Ruta definida en GA2 sin consumidores ({route})", Ga2Usage.NotUsed,
                "Ningún flujo de GA2 la invoca y su contrato no se validó: el mock responde 501 de forma explícita.", []) with { Certainty = ContractCertainty.NotImplemented };
            app.MapMock("POST", Base + route, spec, async (HttpContext http) =>
                {
                    await RestMock.BeginAsync(http, spec, RestMock.Lookup(http));
                    return Results.Text($"MockIntegrations: {route} no está implementada (sin consumidores en GA2; ver docs/pendientes.md).", "text/plain", Encoding.UTF8, StatusCodes.Status501NotImplemented);
                })
                .Produces<string>(StatusCodes.Status501NotImplemented, "text/plain");
        }
    }

    private static RestOperationSpec Spec(string name, string summary, Ga2Usage usage, string description, IReadOnlyList<ScenarioInfo> scenarios) => new()
    {
        Provider = Provider,
        Name = name,
        Usage = usage,
        Summary = summary,
        Description = description + " Headers: `Authorization: Bearer <token>` (salvo en autenticación), `Content-Type: application/json; charset=utf-8`. JSON PascalCase (Newtonsoft por defecto).",
        Scenarios = scenarios,
        Ga2ConfigKey = ConfigKey,
        Ga2Source = Source,
    };

    /// <summary>Escenarios técnicos, 401 y Bearer emitido por <c>api/autenticacion</c>; <c>null</c> si la llamada continúa.</summary>
    private static IResult? Guard(HttpContext http, ScenarioDecision decision, RestOperationSpec spec, KtrionApiTokens tokens, IOptionsMonitor<MockOptions> options, string[]? allowed = null)
    {
        if (RestMock.BuiltIn(decision, spec) is { } builtIn)
        {
            return builtIn;
        }

        var header = http.Request.Headers.Authorization.ToString();
        var token = header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? header[7..].Trim() : null;
        if (decision.Name == "unauthorized" || token is null || (options.CurrentValue.KtrionApi.RequireIssuedToken && !tokens.IsValid(token)))
        {
            return UnauthorizedText();
        }

        var known = allowed ?? ["fallido"];
        return decision.Name == BuiltInScenarios.Default || known.Contains(decision.Name) ? null : RestMock.UnknownScenario(decision, spec);
    }

    private static InsertResponseDto InsertResult(SequenceStore seq, ScenarioDecision decision, string? barCode, int files, int workflowId)
    {
        var ok = decision.Name == BuiltInScenarios.Default;
        return new InsertResponseDto
        {
            BarCode = ok ? barCode : null,
            FileCodes = ok ? Enumerable.Range(0, files).Select(_ => $"FC-{seq.Next("ktrion.filecode"):D6}").ToList() : [],
            Message = ok ? "Operación exitosa (MockIntegrations)" : "Error funcional simulado por MockIntegrations",
            RecordId = ok ? (int)seq.Next("ktrion.recordid", 4521) : 0,
            Success = ok,
            WorkflowId = workflowId,
        };
    }

    // Mismo PDF de prueba que el fixture SOAP de Haberes (una única fuente).
    private static string SamplePdf(FixtureStore fixtures) =>
        ReportRegex().Match(fixtures.Get(HaberesMock.Provider, "GenerarReporte.default.xml") ?? "").Groups[1].Value;

    [GeneratedRegex("<Report>([^<]*)</Report>")]
    private static partial Regex ReportRegex();

    private static IResult Json<T>(T value) => Results.Json(value, contentType: "application/json; charset=utf-8");

    private static IResult UnauthorizedText() =>
        Results.Text("Authorization has been denied for this request. (MockIntegrations: token ausente o no emitido por api/autenticacion)", "text/plain", Encoding.UTF8, StatusCodes.Status401Unauthorized);
}

/// <summary>Tokens emitidos por <c>api/autenticacion</c> (en memoria).</summary>
public sealed class KtrionApiTokens
{
    private readonly ConcurrentDictionary<string, DateTimeOffset> _tokens = new();

    public string Issue()
    {
        var token = $"mock-ktrion-{Guid.NewGuid():N}";
        _tokens[token] = DateTimeOffset.UtcNow;
        return token;
    }

    public bool IsValid(string token) => _tokens.ContainsKey(token);

    public void Reset() => _tokens.Clear();
}
