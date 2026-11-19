using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Providers;
using MockIntegrations.Tests.Infrastructure;

namespace MockIntegrations.Tests.Contract;

/// <summary>
/// Registro de endpoints, cobertura frente a los proxies de GA2 y documentación OpenAPI.
/// </summary>
[Collection(MockServerCollection.Name)]
public sealed partial class CatalogAndOpenApiTests(MockServer server) : IAsyncLifetime
{
    private static readonly string Ga2Src = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../src"));

    /// <summary>Proxies WCF de GA2 en el alcance → proveedor del mock.</summary>
    public static TheoryData<string, string> Ga2Proxies() => new()
    {
        { "GA2.Domain/Core/ReferenciasSvc/Seven/SCnMcont/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSCDP/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSFactura/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSProveedor/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSTercero/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSWintipro/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/Seven/WSAportes/Reference.cs", "Seven" },
        { "GA2.Domain/Core/Connected Services/ServiceSeven/Reference.cs", "Seven" },
        { "GA2.Domain/Core/ReferenciasSvc/WorkManager/Reference.cs", "WorkManager" },
        { "GA2.Domain/Core/ReferenciasSvc/Bua/Reference.cs", "Bua" },
        { "GA2.Domain/Core/Connected Services/ServiceReference1/Reference.cs", "Bua" },
        { "GA2.Domain/Core/Connected Services/ServiceVigiaVerificacionTercero/Reference.cs", "Vigia" },
        { "GA2.Domain/Core/ReferenciasSvc/Fenix/Reference.cs", "Fenix" },
        { "GA2.Domain/Core/ReferenciasSvc/Haberes/Reference.cs", "Haberes" },
        { "GA2.Application/Main/TransUnionService/Reference.cs", "TransUnion" },
    };

    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Theory]
    [MemberData(nameof(Ga2Proxies))]
    public void Every_SOAPAction_of_the_GA2_proxy_is_served_by_the_mock(string referenceFile, string provider)
    {
        var source = File.ReadAllText(Path.Combine(Ga2Src, referenceFile));
        var actions = ActionRegex().Matches(source).Select(m => m.Groups[1].Value).Distinct().ToList();
        Assert.NotEmpty(actions);

        var served = server.GetService<MockCatalog>().Operations
            .Where(o => o.Provider == provider)
            .Select(o => o.SoapAction)
            .ToHashSet(StringComparer.Ordinal);

        Assert.All(actions, action => Assert.Contains(action, served));
    }

    [Fact]
    public void Catalog_contains_every_provider_of_the_inventory()
    {
        var providers = server.GetService<MockCatalog>().Providers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        string[] expected = ["Seven", "WorkManager", "Bua", "Vigia", "Fenix", "Haberes", "TransUnion", "TusDatos", "Dodo", "KtrionApi", "Smtp", "ServiceBus"];
        Assert.All(expected, p => Assert.Contains(p, providers));
    }

    [Theory]
    [InlineData("GET", "/kiosko-dev/api/v1/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{checkHistory}")]
    [InlineData("GET", "/kiosko-dev/api/v1/tusdatos/jobid/{jobId}/customer/{identification}")]
    [InlineData("GET", "/kiosko-dev/api/v1/vigia/customer/{identification}/document-type/{documentType}/CheckRiskLAFT")]
    [InlineData("GET", "/dodo/api/version")]
    [InlineData("POST", "/dodo/api/Forms/Form_Insert")]
    [InlineData("POST", "/REST/KriterionDev/ktrionApi/api/autenticacion")]
    [InlineData("POST", "/REST/KriterionDev/ktrionApi/api/forminsert")]
    [InlineData("POST", "/REST/KriterionDev/ktrionApi/api/formUpdate")]
    [InlineData("POST", "/REST/KriterionDev/ktrionApi/api/workflowattachfiles")]
    [InlineData("POST", "/REST/KriterionDev/ktrionApi/api/generarreportehaberes")]
    public void Rest_routes_and_verbs_match_what_GA2_calls(string method, string route)
    {
        Assert.Contains(server.GetService<MockCatalog>().Operations, o => o.Method == method && o.Route == route);
    }

    [Fact]
    public void Dodo_routes_match_the_GA2_service_classes()
    {
        var routes = Directory.GetFiles(Path.Combine(Ga2Src, "GA2.Domain/Core/ReferenciasSvc/Dodo"), "*Service.cs", SearchOption.AllDirectories)
            .SelectMany(f => DodoRouteRegex().Matches(File.ReadAllText(f)).Select(m => "/dodo/" + m.Groups[1].Value))
            .ToHashSet();
        var served = server.GetService<MockCatalog>().Operations.Where(o => o.Provider == "Dodo").Select(o => o.Route).ToHashSet();

        Assert.Equal(29, routes.Count);
        Assert.Equal(routes, served);
    }

    [Fact]
    public async Task Unknown_SOAPAction_answers_a_fault()
    {
        using var content = new StringContent("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><Nada xmlns=\"http://seven/\"/></s:Body></s:Envelope>", Encoding.UTF8, "text/xml");
        content.Headers.Add("SOAPAction", "\"http://seven/NoExiste\"");
        using var response = await server.Http.PostAsync("Seven/WsCdp/WPgOblig.asmx", content);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("<s:Fault>", await response.Content.ReadAsStringAsync());
    }

    // ─── OpenAPI ────────────────────────────────────────────────────────────

    public static TheoryData<string> Documents() => new(ProviderRegistry.All.Select(p => p.Id).Append("all"));

    [Theory]
    [MemberData(nameof(Documents))]
    public async Task OpenApi_document_is_generated_and_every_operation_is_described(string document)
    {
        var doc = JsonNode.Parse(await server.Http.GetStringAsync($"openapi/{document}.json"))!;

        Assert.StartsWith("3.", doc["openapi"]!.GetValue<string>());
        var operations = doc["paths"]!.AsObject().SelectMany(p => p.Value!.AsObject().Select(m => (Path: p.Key, Method: m.Key, Op: m.Value!))).ToList();
        Assert.NotEmpty(operations);
        Assert.All(operations, o =>
        {
            Assert.False(string.IsNullOrWhiteSpace(o.Op["operationId"]?.GetValue<string>()), $"{o.Method} {o.Path} sin operationId");
            Assert.False(string.IsNullOrWhiteSpace(o.Op["summary"]?.GetValue<string>()), $"{o.Method} {o.Path} sin summary");
            Assert.NotEmpty(o.Op["responses"]!.AsObject());
            Assert.NotEmpty(o.Op["tags"]!.AsArray());
        });

        var ids = operations.Select(o => o.Op["operationId"]!.GetValue<string>()).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public async Task OpenApi_documents_every_catalog_operation()
    {
        var doc = JsonNode.Parse(await server.Http.GetStringAsync("openapi/all.json"))!;
        var paths = doc["paths"]!.AsObject();

        foreach (var op in server.GetService<MockCatalog>().Operations.Where(o => o.Protocol is MockProtocol.Soap11 or MockProtocol.Rest))
        {
            Assert.True(paths.ContainsKey(op.Route), $"{op.Provider}/{op.Name}: falta {op.Route}");
            var operation = paths[op.Route]![op.Method.ToLowerInvariant()];
            Assert.NotNull(operation);
            if (op.Protocol == MockProtocol.Soap11)
            {
                // SOAP: la acción está en el enum del header y hay ejemplo de request si GA2 la usa.
                var soapAction = operation!["parameters"]!.AsArray().First(p => p!["name"]!.GetValue<string>() == "SOAPAction")!;
                Assert.Contains(op.SoapAction, soapAction["schema"]!["enum"]!.AsArray().Select(v => v!.GetValue<string>()));
                if (op.Usage != Ga2Usage.NotUsed)
                {
                    Assert.NotNull(operation["requestBody"]!["content"]!["text/xml"]!["examples"]![op.Name]);
                    Assert.NotNull(operation["responses"]!["200"]!["content"]!["text/xml"]!["examples"]![op.Name]);
                }
            }
        }
    }

    [Fact]
    public async Task OpenApi_rest_schemas_use_the_names_GA2_serializes()
    {
        var ktrion = JsonNode.Parse(await server.Http.GetStringAsync("openapi/ktrionapi.json"))!;
        var insert = ktrion["components"]!["schemas"]!["InsertResponseDto"]!["properties"]!.AsObject();
        Assert.True(insert.ContainsKey("BarCode"));
        Assert.True(insert.ContainsKey("workflowId"));

        var tusdatos = JsonNode.Parse(await server.Http.GetStringAsync("openapi/tusdatos.json"))!;
        var vigia = tusdatos["components"]!["schemas"]!["VigiaResponse"]!["properties"]!.AsObject();
        Assert.True(vigia.ContainsKey("riskLAFT"));
    }

    [Fact]
    public async Task Swagger_ui_is_served()
    {
        using var response = await server.Http.GetAsync("swagger/index.html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [GeneratedRegex("OperationContractAttribute\\(Action\\s*=\\s*\"([^\"]+)\"")]
    private static partial Regex ActionRegex();

    [GeneratedRegex("\"(api/[A-Za-z]+(?:/[A-Za-z0-9_]+)?)\"")]
    private static partial Regex DodoRouteRegex();
}
