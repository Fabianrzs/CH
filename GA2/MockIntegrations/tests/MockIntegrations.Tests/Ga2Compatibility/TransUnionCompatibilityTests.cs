using System.Globalization;
using System.ServiceModel;
using GA2.Application.Dto;
using GA2.Application.Main.TU;
using MockIntegrations.Api.Providers.TransUnion;
using MockIntegrations.Tests.Infrastructure;

namespace MockIntegrations.Tests.Ga2Compatibility;

/// <summary>
/// E7 TransUnion: se ejecuta el cliente real de GA2 (<c>ValidacionesTU.ConsumirTU</c> / <c>ConsumirTUAqm</c>),
/// que construye el DCRequest (CDATA o XML escapado) y deserializa el DCResponse con <c>Serialization.DeserializeDC</c>.
/// La URL es fija en GA2: la prueba sustituye <c>endpointAddress</c> y el binding por reflexión.
/// </summary>
[Collection(MockServerCollection.Name)]
public sealed class TransUnionCompatibilityTests(MockServer server) : IAsyncLifetime
{
    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Fact]
    public void IdVision_start_send_and_validate_OTP_as_GA2_does()
    {
        var tu = Client();

        var start = tu.ConsumirTU(StartIdVision("1020304050"), out var rawRequest, out _);
        Assert.Contains("<![CDATA[<Applicant>", rawRequest); // transformación de ConsumirTU
        Assert.Equal("Success", start.Status);
        Assert.Equal("Success", start.Authentication.Status);
        var applicationId = start.ResponseInfo.ApplicationId;
        Assert.False(string.IsNullOrEmpty(applicationId));
        var applicants = Serialization.DeserializeApps(Field(start, "Applicants"));
        Assert.Equal("1020304050", applicants.Applicant.IdNumber);

        var otp = tu.ConsumirTU(Send(applicationId, "PhoneSelection", ("ValidationMethod", "SMS"), ("PhoneType", "Mobile"), ("SelectedPhoneNumber", "3001234567")), out _, out _);
        Assert.Equal("Success", otp.Status);
        Assert.Contains(otp.ContextData.Field, f => f.Key.Contains("OTPCode"));
        Assert.Contains(otp.ContextData.Field, f => f.Key.Contains("Delay"));

        var resend = tu.ConsumirTU(Send(applicationId, "PinVerification_OTPInput", ("Action", "ResendOTP")), out _, out _);
        Assert.Equal("Success", resend.Status);

        var wrong = tu.ConsumirTU(Send(applicationId, "PinVerification_OTPInput", ("PinNumber", "000000"), ("Action", "Continue")), out _, out _);
        Assert.Equal(302, Convert.ToInt32(Serialization.DeserializeRC(Field(wrong, "ApplicationData")).OTPReasons.Reason.Code));

        var valid = tu.ConsumirTU(Send(applicationId, "PinVerification_OTPInput", ("PinNumber", "123456"), ("Action", "Continue")), out _, out _);
        Assert.Equal(300, Convert.ToInt32(Serialization.DeserializeRC(Field(valid, "ApplicationData")).OTPReasons.Reason.Code));
    }

    [Fact]
    public void Sending_OTP_for_an_unknown_application_is_rejected_with_the_message_GA2_expects()
    {
        var response = Client().ConsumirTU(Send("424242", "PhoneSelection", ("ValidationMethod", "SMS")), out _, out _);

        Assert.Equal("Application does not exists in Queue PhoneSelection.", response.ErrorInfo.Message);
    }

    [Fact]
    public void Aqm_score_and_history_use_en_US_decimals()
    {
        var tu = Client();

        var score = tu.ConsumirTUAqm(StartAqm("1020304050"), out var rawRequest, out _);
        Assert.Contains("&lt;Applicant&gt;", rawRequest); // ConsumirTUAqm: XML interno escapado, sin CDATA
        var data = Serialization.DeserializeRC(Field(score, "ApplicationData"));
        Assert.Equal("VIABLE", data.DecisionHomologada);
        var applicant = Serialization.DeserializeApps(Field(score, "Applicants")).Applicant;
        Assert.Equal(1500000.50m, decimal.Parse(applicant.CapacidadEndeudamiento, new CultureInfo("en-US")));
        Assert.Equal(35.5m, decimal.Parse(applicant.PorcEndFinal, new CultureInfo("en-US")));

        var history = tu.ConsumirTUAqm(Send(score.ResponseInfo.ApplicationId, "DocVerify", ("GenerateReport", "Y")), out _, out _);
        Assert.Equal("Success", history.Status);
        Assert.NotNull(Serialization.DeserializeApps(Field(history, "Applicants")).Applicant.DSOnlineBureauCOLData);
    }

    [Fact]
    public void Prospeccion_request_with_CDATA_application_data_gets_a_decision()
    {
        var request = StartAqm("1020304050");
        var response = Client().ConsumirTU(request, out var rawRequest, out _);

        Assert.Contains("<![CDATA[<ApplicationData>", rawRequest);
        Assert.Equal("VIABLE", Serialization.DeserializeRC(Field(response, "ApplicationData")).DecisionHomologada);
    }

    [Theory]
    [InlineData("1000000666", "NO VIABLE")]
    [InlineData("1000000777", "PENDIENTE")]
    public void Decision_scenarios_are_selected_by_document_number(string idNumber, string decision)
    {
        var response = Client().ConsumirTUAqm(StartAqm(idNumber), out _, out _);
        Assert.Equal(decision, Serialization.DeserializeRC(Field(response, "ApplicationData")).DecisionHomologada);
    }

    [Fact]
    public void Deceased_holder_is_reported_in_DocumentState()
    {
        var response = Client().ConsumirTU(StartIdVision("1000000999"), out _, out var raw);
        Assert.Contains("MUERTE POR TITULAR", Field(response, "Applicants"));
        Assert.Contains("MUERTE POR TITULAR", raw);
    }

    [Fact]
    public void Technical_error_becomes_a_FaultException_in_ConsumirTU()
    {
        Assert.Throws<FaultException>(() => Client().ConsumirTU(StartIdVision("1000000500"), out _, out _));
    }

    [Fact]
    public async Task Unused_operations_answer_an_explicit_fault()
    {
        var client = SoapClients.Create<TransUnionService.ExternalSolutionExecutionClient>(server, TransUnionMock.Service.Paths[0]);
        await Assert.ThrowsAsync<FaultException>(() => client.AuthenticateXMLStringAsync("<x/>"));
    }

    private ValidacionesTU Client()
    {
        var tu = new ValidacionesTU();
        var address = new EndpointAddress(new Uri(server.BaseUri, TransUnionMock.Service.Paths[0].TrimStart('/')));
        typeof(ValidacionesTU).GetField(nameof(ValidacionesTU.endpointAddress))!.SetValue(tu, address);
        typeof(ValidacionesTU).GetField(nameof(ValidacionesTU.basicHttpBinding))!.SetValue(tu, (BasicHttpBinding)SoapClients.Binding());
        return tu;
    }

    private static DCRequest StartIdVision(string idNumber) => new()
    {
        Authentication = new Authentication { UserId = "USUARIO_TU", Password = "CLAVE_TU", Type = "OnDemand" },
        RequestInfo = new RequestInfo { SolutionSetId = "142", ExecuteLatestVersion = "true", ExecutionMode = "NewWithContext" },
        Fields = new Fields
        {
            Field =
            [
                new Field
                {
                    Key = "PrimaryApplicant",
                    Text = new Applicant { IdType = 1, IdNumber = idNumber, LastName = "PEREZ", RecentPhoneNumber = "3001234567", IdExpeditionDate = "15/01/2010" }.Serialize(),
                },
            ],
        },
    };

    private static DCRequest StartAqm(string idNumber) => new()
    {
        Authentication = new Authentication { UserId = "USUARIO_AQM", Password = "CLAVE_AQM", Type = "OnDemand" },
        RequestInfo = new RequestInfo { SolutionSetId = "360", ExecuteLatestVersion = "true", ExecutionMode = "NewWithContext" },
        Fields = new Fields
        {
            Field =
            [
                new Field
                {
                    Key = "PrimaryApplicant",
                    Text = new Applicant { IdType = 1, IdNumber = idNumber, LastName = "PEREZ", RecentPhoneNumber = "3001234567", IdExpeditionDate = "15/01/2010", Occupation = "EMPLEADO", MonthlyIncome = "3500000" }.Serialize(),
                },
                new Field { Key = "ApplicationData", Text = "<ApplicationData><Product>CONSUMO</Product></ApplicationData>" },
            ],
        },
    };

    private static DCRequest Send(string applicationId, string queue, params (string Key, string Value)[] fields) => new()
    {
        Authentication = new Authentication { UserId = "USUARIO_TU", Password = "CLAVE_TU", Type = "OnDemand" },
        RequestInfo = new RequestInfo { ExecutionMode = "Send", ApplicationId = applicationId, QueueName = queue },
        Fields = new Fields { Field = fields.Select(f => new Field { Key = f.Key, Text = f.Value }).ToList() },
    };

    private static string Field(DCResponse response, string key) =>
        response.ContextData.Field.First(f => f.Key.Contains(key)).Text;
}
