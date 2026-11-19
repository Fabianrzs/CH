using System.Text.Json.Serialization;
using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Api.Infrastructure.OpenApi;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Providers;
using MockIntegrations.Api.Providers.KtrionApi;
using MockIntegrations.Api.Providers.Smtp;
using MockIntegrations.Api.Providers.TransUnion;
using MockIntegrations.Api.Providers.TusDatos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MockOptions>(builder.Configuration.GetSection(MockOptions.Section));

builder.Services.AddSingleton<MockCatalog>();
builder.Services.AddSingleton<FixtureStore>();
builder.Services.AddSingleton<SequenceStore>();
builder.Services.AddSingleton<TemplateRenderer>();
builder.Services.AddSingleton<ScenarioOverrideStore>();
builder.Services.AddSingleton<ScenarioResolver>();
builder.Services.AddSingleton<MockDelays>();
builder.Services.AddSingleton<RequestJournal>();
builder.Services.AddSingleton<TransUnionState>();
builder.Services.AddSingleton<TusDatosState>();
builder.Services.AddSingleton<KtrionApiTokens>();
builder.Services.AddSingleton<SmtpMailbox>();
builder.Services.AddSingleton<SmtpCaptureServer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<SmtpCaptureServer>());

// Enumeraciones como texto en la API de administración (los modelos de los proveedores no usan enums).
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddMockOpenApi();

var app = builder.Build();

app.UseMiddleware<RequestJournalMiddleware>();

app.MapOpenApi("/openapi/{documentName}.json");
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "MockIntegrations · GA2";
    options.SwaggerEndpoint($"/openapi/{OpenApiSetup.AllDocument}.json", "Todos los proveedores");
    foreach (var provider in ProviderRegistry.All)
    {
        options.SwaggerEndpoint($"/openapi/{provider.Id}.json", provider.Title);
    }

    options.DisplayOperationId();
    options.EnableTryItOutByDefault();
});

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapGet("/health", () => Results.Ok(new { status = "ok" })).ExcludeFromDescription();

foreach (var provider in ProviderRegistry.All)
{
    provider.Map(app);
}

app.Run();

/// <summary>Punto de entrada (expuesto para WebApplicationFactory en las pruebas).</summary>
public partial class Program;
