using System.ServiceModel;
using System.Xml;
using MockIntegrations.Api.Providers.Seven;
using MockIntegrations.Tests.Infrastructure;
using Cdp = GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using Factura = GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using MovContable = GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using Proveedor = GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using Tercero = GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;

namespace MockIntegrations.Tests.Ga2Compatibility;

/// <summary>E1 Seven: el mock se consume con los proxies WCF reales de GA2 y se interpreta como lo hace GA2.</summary>
[Collection(MockServerCollection.Name)]
public sealed class SevenCompatibilityTests(MockServer server) : IAsyncLifetime
{
    private static readonly Dictionary<string, Type> Clients = new()
    {
        ["SCNMCONTSoap"] = typeof(MovContable.SCNMCONTSoapClient),
        ["SPGOBLIGSoap"] = typeof(Cdp.SPGOBLIGSoapClient),
        ["SPOFACTUSoap"] = typeof(Factura.SPOFACTUSoapClient),
        ["SIEWSSECSoap"] = typeof(Proveedor.SIEWSSECSoapClient),
        ["SGNTERCESoap"] = typeof(Tercero.SGNTERCESoapClient),
        ["SINTIPROSoap"] = typeof(GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro.SINTIPROSoapClient),
        ["ValidarAportesSoap"] = typeof(GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes.ValidarAportesSoapClient),
    };

    public static TheoryData<string, string> AllOperations() => Operations(_ => true);

    public static TheoryData<string, string> OperationsWithBusinessError() =>
        Operations(op => op.Scenarios.Any(s => s.Name == "error-negocio"));

    private static TheoryData<string, string> Operations(Func<Api.Infrastructure.Soap.SoapOperation, bool> filter)
    {
        var data = new TheoryData<string, string>();
        foreach (var service in SevenMock.Services)
        {
            foreach (var op in service.Operations.Where(filter))
            {
                data.Add(service.Contract, op.Name);
            }
        }

        return data;
    }

    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Theory]
    [MemberData(nameof(AllOperations))]
    public async Task Each_operation_is_answered_and_deserialized_by_the_GA2_proxy(string contract, string operation)
    {
        var result = await CallAsync(contract, operation);

        Assert.NotNull(result);
        if (result is string text && text.StartsWith('<'))
        {
            // GA2 carga el ...Result con XmlDocument.LoadXml y lee RETORNO.
            var document = new XmlDocument();
            document.LoadXml(text);
            Assert.Equal("0", document.GetElementsByTagName("RETORNO")[0]?.InnerText);
        }
    }

    [Theory]
    [MemberData(nameof(OperationsWithBusinessError))]
    public async Task Business_error_scenario_returns_RETORNO_1_with_TXTERROR(string contract, string operation)
    {
        await server.SetScenarioAsync(SevenMock.Provider, operation, "error-negocio");
        var document = new XmlDocument();
        document.LoadXml((string)(await CallAsync(contract, operation))!);

        Assert.Equal("1", document.GetElementsByTagName("RETORNO")[0]!.InnerText);
        Assert.False(string.IsNullOrWhiteSpace(document.GetElementsByTagName("TXTERROR")[0]!.InnerText));
    }

    [Fact]
    public async Task Payment_flow_returns_the_identifiers_GA2_stores()
    {
        // Orden del pago: InsertarGnTerce → SyncPoPvdor → CrearCDP → InsertarFacturaP → InsertarMovContable.
        var tercero = Load((string)(await CallAsync("SGNTERCESoap", "InsertarGnTerce"))!);
        Assert.Equal("0", tercero.SelectSingleNode("//GN_TERCE/RETORNO")!.InnerText);

        var proveedor = Load((string)(await CallAsync("SIEWSSECSoap", "SyncPoPvdor"))!);
        Assert.True(int.TryParse(proveedor.GetElementsByTagName("PVD_CODD")[0]!.InnerText, out _));

        var cdp1 = Load((string)(await CallAsync("SPGOBLIGSoap", "CrearCDP"))!);
        var cdp2 = Load((string)(await CallAsync("SPGOBLIGSoap", "CrearCDP"))!);
        var mpr1 = int.Parse(cdp1.GetElementsByTagName("MPR_CONT")[0]!.InnerText);
        Assert.Equal(mpr1 + 1, int.Parse(cdp2.GetElementsByTagName("MPR_CONT")[0]!.InnerText));

        var factura = Load((string)(await CallAsync("SPOFACTUSoap", "InsertarFacturaP"))!);
        Assert.True(int.TryParse(factura.GetElementsByTagName("FAC_CONT")[0]!.InnerText, out _));
        Assert.True(int.TryParse(factura.GetElementsByTagName("FAC_NUME")[0]!.InnerText, out _));

        var movimiento = Load((string)(await CallAsync("SCNMCONTSoap", "InsertarMovContable"))!);
        Assert.True(int.TryParse(movimiento.GetElementsByTagName("MCO_CONT")[0]!.InnerText, out _));
        Assert.True(int.TryParse(movimiento.GetElementsByTagName("MCO_NUME")[0]!.InnerText, out _));
    }

    [Fact]
    public async Task Existing_third_party_keeps_the_GN_TERCE_root()
    {
        await server.SetScenarioAsync(SevenMock.Provider, "InsertarGnTerce", "tercero-existente");
        var tercero = Load((string)(await CallAsync("SGNTERCESoap", "InsertarGnTerce"))!);

        Assert.Equal("1", tercero.SelectSingleNode("//GN_TERCE/RETORNO")!.InnerText);
        Assert.Equal("El tercero ya se encuentra registrado", tercero.SelectSingleNode("//GN_TERCE/TXTERROR")!.InnerText);
    }

    [Fact]
    public async Task Invoice_without_identifiers_reproduces_the_GA2_robustness_case()
    {
        await server.SetScenarioAsync(SevenMock.Provider, "InsertarFacturaP", "sin-identificadores");
        var factura = Load((string)(await CallAsync("SPOFACTUSoap", "InsertarFacturaP"))!);

        Assert.Equal("0", factura.GetElementsByTagName("RETORNO")[0]!.InnerText);
        Assert.Empty(factura.GetElementsByTagName("FAC_CONT"));
    }

    [Fact]
    public async Task The_second_GA2_proxy_of_scnmcont_reads_the_same_response()
    {
        // Connected Services/ServiceSeven: XmlSerializer y parámetros planos (MovimientoContableService).
        var client = SoapClients.Create<ServiceSeven.SCNMCONTSoapClient>(server, "/Seven/WsMovContable/scnmcont.asmx");
        var result = await client.InsertarMovContableAsync(new ServiceSeven.TSCnMcont { Emp_codi = 1, Mco_desc = "Prueba", vDetalle = [] });
        var anulacion = await client.AnularCnMcontAsync(1, 551200);

        Assert.Equal("0", Load(result).GetElementsByTagName("RETORNO")[0]!.InnerText);
        Assert.Matches("<RETORNO>(.*?)</RETORNO>", anulacion);
    }

    [Fact]
    public async Task Technical_errors_surface_as_WCF_exceptions()
    {
        await server.SetScenarioAsync(SevenMock.Provider, "CrearCDP", "fault");
        await Assert.ThrowsAsync<FaultException>(() => CallAsync("SPGOBLIGSoap", "CrearCDP"));

        await server.SetScenarioAsync(SevenMock.Provider, "CrearCDP", "http-500");
        var error = await Assert.ThrowsAnyAsync<CommunicationException>(() => CallAsync("SPGOBLIGSoap", "CrearCDP"));
        Assert.IsNotType<FaultException>(error);
    }

    private async Task<object?> CallAsync(string contract, string operation)
    {
        var path = SevenMock.Services.Single(s => s.Contract == contract).Paths[0];
        var client = (ICommunicationObject)Activator.CreateInstance(Clients[contract], SoapClients.Binding(), new EndpointAddress(new Uri(server.BaseUri, path.TrimStart('/'))))!;
        try
        {
            return SoapClients.Result(await SoapClients.InvokeEmptyAsync(client, operation), operation);
        }
        finally
        {
            client.Abort();
        }
    }

    private static XmlDocument Load(string xml)
    {
        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }
}
