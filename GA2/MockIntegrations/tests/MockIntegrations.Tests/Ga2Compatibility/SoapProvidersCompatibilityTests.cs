using System.ServiceModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using MockIntegrations.Api.Providers.Bua;
using MockIntegrations.Api.Providers.Fenix;
using MockIntegrations.Api.Providers.Haberes;
using MockIntegrations.Api.Providers.Vigia;
using MockIntegrations.Api.Providers.WorkManager;
using MockIntegrations.Tests.Infrastructure;
using Fenix = GA2.Domain.Core.ReferenciasSvc.Fenix;
using Haberes = GA2.Domain.Core.ReferenciasSvc.Haberes;
using Wm = GA2.Domain.Core.ReferenciasSvc.WorkManager;

namespace MockIntegrations.Tests.Ga2Compatibility;

/// <summary>E2–E6: WorkManager, BUA (ws.01 y ws.02), Vigía, Fénix y Haberes con los proxies y la lógica de lectura de GA2.</summary>
[Collection(MockServerCollection.Name)]
public sealed class SoapProvidersCompatibilityTests(MockServer server) : IAsyncLifetime
{
    private const string WorkManagerPath = "/APIV2_0005/WorkManagerService.svc";

    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ─── E2 WorkManager ─────────────────────────────────────────────────────

    public static TheoryData<string> WorkManagerOperationsWithContract() =>
        new(WorkManagerMock.Service.Operations.Where(o => o.Certainty != Api.Infrastructure.Catalog.ContractCertainty.NotImplemented).Select(o => o.Name));

    public static TheoryData<string> WorkManagerOperationsWithoutContract() =>
        new(WorkManagerMock.Service.Operations.Where(o => o.Certainty == Api.Infrastructure.Catalog.ContractCertainty.NotImplemented).Select(o => o.Name));

    [Theory]
    [MemberData(nameof(WorkManagerOperationsWithContract))]
    public async Task WorkManager_wrapped_operations_return_a_successful_result(string operation)
    {
        var client = SoapClients.Create<Wm.WorkManagerServicesClient>(server, WorkManagerPath);
        var result = SoapClients.Result(await SoapClients.InvokeEmptyAsync(client, operation), operation);

        Assert.NotNull(result);
        Assert.True((bool)result.GetType().GetProperty("Success")!.GetValue(result)!);
    }

    [Theory]
    [MemberData(nameof(WorkManagerOperationsWithoutContract))]
    public async Task WorkManager_operations_unused_by_GA2_answer_an_explicit_fault(string operation)
    {
        var client = SoapClients.Create<Wm.WorkManagerServicesClient>(server, WorkManagerPath);
        var fault = await Assert.ThrowsAsync<FaultException>(() => SoapClients.InvokeEmptyAsync(client, operation));
        Assert.Contains("GA2 no la invoca", fault.Message);
    }

    [Fact]
    public async Task Form_Insert_returns_a_radicado_with_the_FormCode_prefix()
    {
        var client = SoapClients.Create<Wm.WorkManagerServicesClient>(server, WorkManagerPath);
        var request = new Wm.InsertRequestDto { FormCode = "219", Header = new Wm.HeaderDto { User = "u", Token = "t" }, CheckWorkflow = true, Data = [], Files = [] };

        var first = (await client.Form_InsertAsync(new Wm.Form_InsertRequest(request))).Form_InsertResult;
        var second = (await client.Form_InsertAsync(new Wm.Form_InsertRequest(request))).Form_InsertResult;

        Assert.True(first.Success);
        Assert.Equal("219", first.BarCode.Split("-")[0]); // GA2: numeroRadicado.Split("-")[0]
        Assert.NotEqual(first.BarCode, second.BarCode);
        Assert.True(first.RecordId > 0);
    }

    [Fact]
    public async Task Form_Insert_failure_and_null_result_scenarios()
    {
        var client = SoapClients.Create<Wm.WorkManagerServicesClient>(server, WorkManagerPath);
        var request = new Wm.Form_InsertRequest(new Wm.InsertRequestDto { FormCode = "999" });

        // Regla de configuración: FormCode 999 → fallido.
        var failed = (await client.Form_InsertAsync(request)).Form_InsertResult;
        Assert.False(failed.Success);

        await server.SetScenarioAsync(WorkManagerMock.Provider, "Form_Insert", "resultado-nulo");
        Assert.Null((await client.Form_InsertAsync(request)).Form_InsertResult);
    }

    [Fact]
    public async Task Form_GetParameters_gives_GA2_what_it_needs_to_build_the_DODO_link()
    {
        var client = SoapClients.Create<Wm.WorkManagerServicesClient>(server, WorkManagerPath);
        var response = await client.Form_GetParametersAsync(new Wm.Form_GetParametersRequest(new Wm.FormParametersRequestDto { FormCode = "83" }));

        var form = response.Form_GetParametersResult.Form;
        Assert.Equal(83, form.IdFormulario);
        Assert.False(string.IsNullOrEmpty(form.NombreCampoSticker));

        await server.SetScenarioAsync(WorkManagerMock.Provider, "Form_GetParameters", "sin-formulario");
        var withoutForm = await client.Form_GetParametersAsync(new Wm.Form_GetParametersRequest(new Wm.FormParametersRequestDto { FormCode = "83" }));
        Assert.Null(withoutForm.Form_GetParametersResult.Form);
    }

    // ─── E3 BUA ─────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc")]
    [InlineData("/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc")]
    public async Task ObtenerBUA_is_read_by_both_GA2_proxies(string path)
    {
        var ws01 = SoapClients.Create<GA2.Domain.Core.BuaServiceSvc.BUA_APIClient>(server, path);
        var ws02 = SoapClients.Create<ServiceReference1.BUA_APIClient>(server, path);

        var a = await ws01.ObtenerBUAAsync(new GA2.Domain.Core.BuaServiceSvc.BUAConsultaBE { identificacion = "1020304050", tipo_Id = 1 });
        var b = await ws02.ObtenerBUAAsync(new ServiceReference1.BUAConsultaBE { identificacion = "1020304050", tipo_Id = 1 });

        Assert.Equal("1020304050", a.identificacion);
        Assert.Equal("JUAN", a.primerNombre);
        Assert.Equal("PEREZ", b.primerApellido);
        Assert.NotEmpty(a.Cuentas);
        Assert.NotEmpty(b.Cuentas);
        Assert.Equal(new DateTime(1985, 3, 12), b.fechaNacimiento);
    }

    [Fact]
    public async Task ObtenerBUA_not_found_and_retry_by_document_type()
    {
        var client = SoapClients.Create<ServiceReference1.BUA_APIClient>(server, BuaMock.Service.Paths[0]);

        Assert.Null(await client.ObtenerBUAAsync(new ServiceReference1.BUAConsultaBE { identificacion = "1000000404", tipo_Id = 1 }));

        // GA2 reintenta 1 → 4 → 5 cuando el resultado es nulo.
        Assert.Null(await client.ObtenerBUAAsync(new ServiceReference1.BUAConsultaBE { identificacion = "1000000004", tipo_Id = 1 }));
        Assert.NotNull(await client.ObtenerBUAAsync(new ServiceReference1.BUAConsultaBE { identificacion = "1000000004", tipo_Id = 4 }));
    }

    // ─── E4 Vigía (RPC/encoded) ─────────────────────────────────────────────

    [Fact]
    public async Task F_VERIFICATERCERO_rpc_encoded_response_is_read_by_the_GA2_proxy()
    {
        var client = SoapClients.Create<ServiceVigiaVerificacionTercero.ApplicationServicesPortTypeClient>(server, VigiaMock.Service.Paths[0]);

        var clean = await client.F_VERIFICATERCEROAsync("1020304050", "1", "");
        var reported = await client.F_VERIFICATERCEROAsync("1000000666", "1", "");

        Assert.Equal("N", clean.Encontrado);
        Assert.Equal("S", reported.Encontrado);
        Assert.Contains("OFAC", reported.Listas);
        Assert.Equal("DOCUMENTO", reported.Metodo);
    }

    // ─── E5 Fénix ───────────────────────────────────────────────────────────

    [Theory]
    [InlineData(1020304050, true, "1")]
    [InlineData(1000000002, true, "0")]
    [InlineData(1000000404, false, null)]
    public async Task ValidarBiometria_dataset_is_deserialized_like_FenixBusinessLogic(int identificacion, bool hasData, string? huella)
    {
        // La URL configurada en GA2 termina en ?WSDL y se usa como dirección.
        var client = SoapClients.Create<Fenix.WebServicesBiometriasSoapClient>(server, FenixMock.Service.Paths[0] + "?WSDL");
        var response = await client.ValidarBiometriaAsync(new Fenix.ValidarBiometriaRequest { identificacion = identificacion });

        var diffgram = (GA2.Application.Dto.diffgram)new XmlSerializer(typeof(GA2.Application.Dto.diffgram))
            .Deserialize(new StringReader(response.ValidarBiometriaResult.Nodes[1].ToString()))!;

        Assert.Equal(hasData, diffgram.NewDataSet is not null);
        if (hasData)
        {
            Assert.Equal((uint)identificacion, diffgram.NewDataSet!.Table1.A001NUM_IDENTIFICACION);
            Assert.Equal(huella, diffgram.NewDataSet.Table1.A008HUELLA.ToString());
        }
    }

    [Fact]
    public async Task ValidarBiometria_invalid_value_makes_the_GA2_deserialization_fail()
    {
        await server.SetScenarioAsync(FenixMock.Provider, "ValidarBiometria", "dato-invalido");
        var client = SoapClients.Create<Fenix.WebServicesBiometriasSoapClient>(server, FenixMock.Service.Paths[0]);
        var response = await client.ValidarBiometriaAsync(new Fenix.ValidarBiometriaRequest { identificacion = 1 });

        Assert.Throws<InvalidOperationException>(() =>
            new XmlSerializer(typeof(GA2.Application.Dto.diffgram)).Deserialize(new StringReader(response.ValidarBiometriaResult.Nodes[1].ToString())));
    }

    // ─── E6 Haberes ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerarReporte_and_V2_return_a_real_pdf()
    {
        var client = SoapClients.Create<Haberes.wsModeloCanonicoDatosSoapClient>(server, HaberesMock.Service.Paths[0]);

        var v1 = (await client.GenerarReporteAsync(new Haberes.GenerarReporteRequest { sReporte = "CERTIFICADO", customerIdentification = "1020304050", accountId = 1 })).GenerarReporteResult;
        var v2 = (await client.GenerarReporteV2Async(new Haberes.GenerarReporteV2Request { sReporte = "CERTIFICADO", customerIdentification = "1020304050", accountId = 1, month = "08", year = "2026" })).GenerarReporteV2Result;

        Assert.True(v1.Status);
        Assert.True(v2.Status);
        Assert.Equal("%PDF-1.4", System.Text.Encoding.ASCII.GetString(v1.Report, 0, 8));
        Assert.EndsWith("%%EOF\n", System.Text.Encoding.ASCII.GetString(v1.Report));

        var noReport = (await client.GenerarReporteAsync(new Haberes.GenerarReporteRequest { customerIdentification = "1000000404" })).GenerarReporteResult;
        Assert.False(noReport.Status);
        Assert.Null(noReport.Report);
    }

    [Fact]
    public async Task ReporteHaberes_dataset_is_parsed_like_HaberesBusinessLogic()
    {
        var client = SoapClients.Create<Haberes.wsModeloCanonicoDatosSoapClient>(server, HaberesMock.Service.Paths[0]);
        var result = (await client.ReporteHaberesAsync(new Haberes.ReporteHaberesRequest { customerIdentification = "1020304050", accountId = 7, sUserQuery = "" })).ReporteHaberesResult;

        Assert.True(result.Nodes.Count > 1);
        var document = XDocument.Parse(result.Nodes[1].FirstNode!.ToString());
        Assert.Equal("1020304050", document.Descendants("CLI_IDENTIFICACION").First().Value);
        Assert.Equal("JUAN CARLOS PEREZ GOMEZ", document.Descendants("CLI_NOMBRE_COMPLETO").First().Value);
        Assert.NotNull((DateTime?)document.Descendants("APC_FECHA_ULTIMA_CUOTA").FirstOrDefault());

        await server.SetScenarioAsync(HaberesMock.Provider, "ReporteHaberes", "vacio");
        var empty = (await client.ReporteHaberesAsync(new Haberes.ReporteHaberesRequest { customerIdentification = "1" })).ReporteHaberesResult;
        Assert.Single(empty.Nodes);
    }

    [Fact]
    public async Task Unused_Haberes_operations_answer_an_explicit_fault()
    {
        var client = SoapClients.Create<Haberes.wsModeloCanonicoDatosSoapClient>(server, HaberesMock.Service.Paths[0]);
        await Assert.ThrowsAsync<FaultException>(() => client.CustomerAccountAsync(new Haberes.CustomerAccountRequest()));
    }
}
