using System.Net;
using System.Text;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Tests.Infrastructure;

namespace MockIntegrations.Tests.Contract;

/// <summary>Selección determinista de escenarios y registro de llamadas.</summary>
[Collection(MockServerCollection.Name)]
public sealed class ScenarioSelectionTests(MockServer server) : IAsyncLifetime
{
    private const string Route = "kiosko-dev/api/v1/vigia/customer/{0}/document-type/CC/CheckRiskLAFT";

    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Fact]
    public async Task Priority_is_header_then_override_then_rule_then_default()
    {
        Assert.Equal(("default", "Default"), await CallAsync("1020304050"));
        Assert.Equal(("riesgo", "Rule"), await CallAsync("1000000666"));

        await server.SetScenarioAsync("TusDatos", "ConsultarVigia", "no-encontrado");
        Assert.Equal(("no-encontrado", "RuntimeOverride"), await CallAsync("1000000666"));

        Assert.Equal(("riesgo", "Header"), await CallAsync("1000000666", header: "riesgo"));

        using var cleared = await server.Http.DeleteAsync("_mock/scenarios/TusDatos/ConsultarVigia");
        Assert.Equal(("riesgo", "Rule"), await CallAsync("1000000666"));
    }

    [Fact]
    public async Task Unknown_scenario_is_reported_instead_of_returning_an_approximate_contract()
    {
        var (status, _) = await RawAsync("1020304050", "no-existe");
        Assert.Equal(HttpStatusCode.BadRequest, status);
    }

    [Fact]
    public async Task Slow_scenario_delays_the_response()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var (status, _) = await RawAsync("1020304050", "slow");
        Assert.Equal(HttpStatusCode.OK, status);
        Assert.True(watch.ElapsedMilliseconds >= 250, $"Retardo {watch.ElapsedMilliseconds} ms");
    }

    [Fact]
    public async Task Overriding_an_unknown_operation_is_rejected()
    {
        using var response = await server.Http.PutAsJsonAsync("_mock/scenarios/TusDatos/NoExiste", new { scenario = "fault" });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Journal_records_calls_with_masked_credentials()
    {
        await CallAsync("1020304050");
        using var soap = new StringContent("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><ValidarBiometria xmlns=\"http://tempuri.org/\"><identificacion>7</identificacion></ValidarBiometria></s:Body></s:Envelope>", Encoding.UTF8, "text/xml");
        soap.Headers.Add("SOAPAction", "\"http://tempuri.org/ValidarBiometria\"");
        (await server.Http.PostAsync("WsFenixBiometria/WebServicesBiometrias.asmx?WSDL", soap)).Dispose();

        var entries = await server.Http.GetFromJsonAsync<List<JournalEntry>>("_mock/journal?take=10");
        var vigia = entries!.Single(e => e.Operation == "ConsultarVigia");
        Assert.Equal("***", vigia.Headers["x-api-key"]);
        var fenix = entries.Single(e => e.Provider == "Fenix");
        Assert.Equal("ValidarBiometria", fenix.Operation);
        Assert.Contains("<identificacion>7</identificacion>", fenix.RequestBody);
        Assert.Equal("?WSDL", fenix.Query);
    }

    [Fact]
    public async Task Scenario_catalog_lists_builtin_fixture_and_rule_scenarios()
    {
        var scenarios = await server.Http.GetFromJsonAsync<List<Api.Infrastructure.Admin.AdminEndpoints.OperationScenarios>>("_mock/scenarios");
        var createCdp = scenarios!.Single(s => s.Provider == "Seven" && s.Operation == "CrearCDP");
        Assert.Contains("error-negocio", createCdp.Available);
        Assert.Contains("fault", createCdp.Available);

        var bua = scenarios.Single(s => s.Provider == "Bua");
        Assert.Contains(bua.Rules, r => r.EqualsValue == "1000000404" && r.Scenario == "no-encontrado");
    }

    [Fact]
    public void Configured_rules_point_to_scenarios_that_exist_for_every_targeted_operation()
    {
        var options = server.GetService<Microsoft.Extensions.Options.IOptions<Api.Configuration.MockOptions>>().Value;
        var catalog = server.GetService<Api.Infrastructure.Catalog.MockCatalog>();
        var fixtures = server.GetService<Api.Infrastructure.Fixtures.FixtureStore>();

        Assert.NotEmpty(options.Scenarios.Rules);
        foreach (var rule in options.Scenarios.Rules)
        {
            var targets = catalog.Operations.Where(o => o.Provider == rule.Provider
                && (string.IsNullOrEmpty(rule.Operation) || rule.Operation == "*" || o.Name == rule.Operation)).ToList();
            Assert.True(targets.Count > 0, $"La regla {rule.Provider}/{rule.Operation} no apunta a ninguna operación");
            foreach (var op in targets)
            {
                var available = Api.Infrastructure.Scenarios.BuiltInScenarios.All.Select(s => s.Name)
                    .Concat(fixtures.ScenariosFor(op.Provider, op.Name))
                    .Concat(op.Scenarios.Select(s => s.Name));
                Assert.True(available.Contains(rule.Scenario), $"{op.Provider}/{op.Name} no tiene el escenario '{rule.Scenario}' (regla {rule.Field}={rule.EqualsValue})");
            }
        }
    }

    private async Task<(string Scenario, string Source)> CallAsync(string identification, string? header = null)
    {
        using var request = Request(identification, header);
        using var response = await server.Http.SendAsync(request);
        return (response.Headers.GetValues("X-Mock-Scenario-Applied").Single(), response.Headers.GetValues("X-Mock-Scenario-Source").Single());
    }

    private async Task<(HttpStatusCode Status, string Body)> RawAsync(string identification, string header)
    {
        using var request = Request(identification, header);
        using var response = await server.Http.SendAsync(request);
        return (response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    private static HttpRequestMessage Request(string identification, string? header)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, string.Format(Route, identification));
        request.Headers.Add("x-api-key", "clave");
        if (header is not null)
        {
            request.Headers.Add("X-Mock-Scenario", header);
        }

        return request;
    }
}
