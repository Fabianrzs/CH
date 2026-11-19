using Microsoft.OpenApi;
using MockIntegrations.Api.Providers;

namespace MockIntegrations.Api.Infrastructure.OpenApi;

/// <summary>Un documento OpenAPI por proveedor (<c>/openapi/{proveedor}.json</c>) y uno con todo (<c>/openapi/all.json</c>).</summary>
public static class OpenApiSetup
{
    public const string AllDocument = "all";

    public static IServiceCollection AddMockOpenApi(this IServiceCollection services)
    {
        foreach (var provider in ProviderRegistry.All)
        {
            services.AddOpenApi(provider.Id, options =>
            {
                options.ShouldInclude = description => description.GroupName == provider.Id;
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = $"MockIntegrations · {provider.Title}",
                        Version = "1.0.0",
                        Description = $"{provider.Description}\n\n**Protocolo:** {provider.Protocol}.\n\n" + CommonDescription,
                    };
                    document.Tags = new HashSet<OpenApiTag> { new() { Name = provider.Key, Description = $"{provider.Title} — {provider.Protocol}" } };
                    return Task.CompletedTask;
                });
            });
        }

        services.AddOpenApi(AllDocument, options =>
        {
            options.ShouldInclude = _ => true;
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "MockIntegrations · Todos los proveedores",
                    Version = "1.0.0",
                    Description = "Mocks de las integraciones de GA2 agrupados por proveedor (una etiqueta por proveedor).\n\n" + CommonDescription,
                };
                document.Tags = ProviderRegistry.All
                    .Select(p => new OpenApiTag { Name = p.Key, Description = $"{p.Title} — {p.Protocol}" })
                    .ToHashSet();
                return Task.CompletedTask;
            });
        });

        return services;
    }

    private const string CommonDescription =
        "Los mocks reproducen el contrato que GA2 consume (fuente: código de GA2 y docs/integraciones/mocks). "
        + "Las operaciones SOAP se documentan por endpoint: cada `.asmx`/`.svc` es un `POST` que despacha por el header `SOAPAction`; "
        + "los ejemplos de request/response están por operación. "
        + "Escenarios: header `X-Mock-Scenario` (pruebas manuales), `PUT /_mock/scenarios/{proveedor}/{operación}`, "
        + "o `MockIntegrations:Scenarios` en configuración.";
}
