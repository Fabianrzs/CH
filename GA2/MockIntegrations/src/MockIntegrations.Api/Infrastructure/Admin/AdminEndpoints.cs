using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Providers.KtrionApi;
using MockIntegrations.Api.Providers.Smtp;
using MockIntegrations.Api.Providers.TransUnion;
using MockIntegrations.Api.Providers.TusDatos;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Infrastructure.Admin;

/// <summary>API de administración del mock (<c>/_mock</c>): catálogo, escenarios, registro de llamadas y reinicio.</summary>
public static class AdminEndpoints
{
    public const string Provider = "Admin";

    public sealed record ScenarioOverride(string Scenario);

    public sealed record OperationScenarios(
        string Provider,
        string Operation,
        string Protocol,
        string Usage,
        IReadOnlyList<string> Available,
        string? Override,
        IReadOnlyList<ScenarioRule> Rules,
        string? ConfiguredDefault);

    public static void Map(WebApplication app)
    {
        var admin = app.MapGroup("/_mock").WithTags(Provider).WithGroupName(Provider.ToLowerInvariant());

        admin.MapGet("/catalog", (MockCatalog catalog) => catalog.Operations)
            .WithName("Admin_Catalog")
            .WithSummary("Inventario de operaciones simuladas")
            .WithDescription("Proveedor, operación, protocolo, ruta, SOAPAction, uso en GA2, clave de configuración, código fuente de GA2 y certeza del contrato.")
            .Produces<IReadOnlyCollection<MockOperation>>();

        admin.MapGet("/scenarios", (MockCatalog catalog, FixtureStore fixtures, ScenarioOverrideStore overrides, IOptionsMonitor<MockOptions> options) =>
                catalog.Operations.Select(op => new OperationScenarios(
                    op.Provider,
                    op.Name,
                    op.Protocol.ToString(),
                    op.Usage.ToString(),
                    BuiltInScenarios.All.Select(s => s.Name)
                        .Concat(fixtures.ScenariosFor(op.Provider, op.Name))
                        .Concat(op.Scenarios.Select(s => s.Name))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList(),
                    overrides.Get(op.Provider, op.Name),
                    options.CurrentValue.Scenarios.Rules
                        .Where(r => string.Equals(r.Provider, op.Provider, StringComparison.OrdinalIgnoreCase)
                            && (string.IsNullOrEmpty(r.Operation) || r.Operation == "*" || string.Equals(r.Operation, op.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList(),
                    ScenarioResolver.FindIgnoreCase(options.CurrentValue.Scenarios.Defaults, op.Provider) is { } perOp
                        ? ScenarioResolver.FindIgnoreCase(perOp, op.Name) ?? ScenarioResolver.FindIgnoreCase(perOp, "*")
                        : null)))
            .WithName("Admin_ListScenarios")
            .WithSummary("Escenarios disponibles y activos por operación")
            .Produces<IEnumerable<OperationScenarios>>();

        admin.MapPut("/scenarios/{provider}/{operation}", (string provider, string operation, ScenarioOverride body, MockCatalog catalog, ScenarioOverrideStore overrides) =>
            {
                if (operation != "*" && catalog.Find(provider, operation) is null)
                {
                    return Results.NotFound($"No existe la operación {provider}/{operation}. Consulte GET /_mock/catalog.");
                }

                overrides.Set(provider, operation, body.Scenario);
                return Results.NoContent();
            })
            .WithName("Admin_SetScenario")
            .WithSummary("Fija en caliente el escenario de una operación (o de todo el proveedor con '*')")
            .WithDescription("Tiene prioridad sobre las reglas y los defaults de configuración; el header `X-Mock-Scenario` sigue teniendo prioridad sobre este override.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status404NotFound);

        admin.MapDelete("/scenarios/{provider}/{operation}", (string provider, string operation, ScenarioOverrideStore overrides) =>
                overrides.Remove(provider, operation) ? Results.NoContent() : Results.NotFound())
            .WithName("Admin_ClearScenario")
            .WithSummary("Quita el override de una operación")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        admin.MapDelete("/scenarios", (ScenarioOverrideStore overrides) =>
            {
                overrides.Clear();
                return Results.NoContent();
            })
            .WithName("Admin_ClearScenarios")
            .WithSummary("Quita todos los overrides")
            .Produces(StatusCodes.Status204NoContent);

        admin.MapGet("/journal", (RequestJournal journal, string? provider, string? operation, int? take) => journal.Query(provider, operation, take ?? 50))
            .WithName("Admin_Journal")
            .WithSummary("Últimas llamadas recibidas por los mocks (más reciente primero)")
            .WithDescription("Incluye headers (Authorization y x-api-key enmascarados), cuerpo, escenario aplicado y código HTTP. Útil para aserciones en pruebas de integración con GA2.")
            .Produces<IReadOnlyList<JournalEntry>>();

        admin.MapDelete("/journal", (RequestJournal journal) =>
            {
                journal.Clear();
                return Results.NoContent();
            })
            .WithName("Admin_ClearJournal")
            .WithSummary("Vacía el registro de llamadas")
            .Produces(StatusCodes.Status204NoContent);

        admin.MapPost("/reset", (IServiceProvider services) =>
            {
                services.GetRequiredService<ScenarioOverrideStore>().Clear();
                services.GetRequiredService<RequestJournal>().Clear();
                services.GetRequiredService<SequenceStore>().Reset();
                services.GetRequiredService<TransUnionState>().Reset();
                services.GetRequiredService<TusDatosState>().Reset();
                services.GetRequiredService<KtrionApiTokens>().Reset();
                services.GetRequiredService<SmtpMailbox>().Clear();
                return Results.NoContent();
            })
            .WithName("Admin_Reset")
            .WithSummary("Reinicia el estado: overrides, registro, contadores, sesiones TransUnion, jobs TusDatos, tokens y buzón SMTP")
            .Produces(StatusCodes.Status204NoContent);
    }
}
