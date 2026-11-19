using System.Text.Json;
using System.Text.Json.Nodes;
using Azure.Messaging.ServiceBus;
using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.ServiceBus;

/// <summary>
/// E12 — Azure Service Bus (topic <c>chonorintegraciones</c>, suscripción <c>dodosubscription</c>). GA2 sólo
/// consume: el broker lo provee el emulador oficial de Azure Service Bus (docker-compose del repositorio) y
/// este mock actúa como el productor externo, publicando los 5 eventos que GA2 escucha.
/// AMQP no se representa en OpenAPI: el endpoint HTTP de publicación es una herramienta de prueba.
/// Contrato: docs/providers/E12-servicebus.md.
/// </summary>
public static class ServiceBusMock
{
    public const string Provider = "ServiceBus";

    /// <summary>Subject (nombre del evento sin el sufijo <c>IntegrationEvent</c>) → propiedad con el payload.</summary>
    public static readonly IReadOnlyDictionary<string, (string PayloadProperty, string Handler)> Events = new Dictionary<string, (string, string)>
    {
        ["WorkManagerInsertRequest"] = ("InsertRequest", "IConsumoWorkManager.Form_InsertAsync"),
        ["WorkManagerUpdateRequest"] = ("UpdateRequest", "IConsumoWorkManager.Form_UpdateAsync"),
        ["WorkManagerDeleteRequest"] = ("DeleteRequest", "IConsumoWorkManager.Form_DeleteAsync"),
        ["WorkManagerWorkflowAttachFilesRequest"] = ("WorkflowAttachFilesRequest", "IConsumoWorkManager.Workflow_AttachFilesAsync"),
        ["WorkManagerAttachRequest"] = ("AttachRequest", "IConsumoWorkManager.Form_AttachFilesAsync"),
    };

    public static void Map(WebApplication app)
    {
        var catalog = app.Services.GetRequiredService<MockCatalog>();
        foreach (var (subject, (property, handler)) in Events)
        {
            catalog.Add(new MockOperation
            {
                Provider = Provider,
                Name = subject,
                Protocol = MockProtocol.ServiceBus,
                Method = "AMQP",
                Route = $"topic chonorintegraciones / subscription dodosubscription / Subject={subject}",
                Usage = Ga2Usage.Business,
                Summary = $"GA2 (consumidor) deserializa {{ {property}, Id, CreatedOn }} y llama a {handler}.",
                Ga2ConfigKey = "EventBusConnection, SubscriptionClientName",
                Ga2Source = "GA2.BuildingBlocks/EventBusServiceBus/EventBusServiceBus.cs; GA2.Domain/Core/IntegrationEvents/**",
                Certainty = ContractCertainty.StructureOnly,
                Notes = "Defecto de GA2: System.Text.Json ignora el campo público 'request' de los proxies, así que el payload llega nulo (ver docs).",
            });
        }

        app.MapPost("/_mock/servicebus/publish/{subject}", PublishAsync)
            .WithName("ServiceBus_Publish")
            .WithTags(Provider)
            .WithGroupName(Provider.ToLowerInvariant())
            .WithSummary("Publica un evento en el topic que consume GA2")
            .WithDescription("Herramienta de prueba (GA2 no la llama). Publica en `MockIntegrations:ServiceBus:Topic` con `Subject = {subject}` y `ContentType = application/json`, "
                + "usando `MockIntegrations:ServiceBus:ConnectionString` (emulador). Si no envía cuerpo, se publica el ejemplo de la documentación: "
                + "`{ <Payload>: { request: {...} }, Id, CreatedOn }` en PascalCase. Subjects válidos: " + string.Join(", ", Events.Keys.Select(k => $"`{k}`")) + ".")
            .Accepts<JsonObject>("application/json")
            .Produces<PublishResult>()
            .Produces<string>(StatusCodes.Status400BadRequest, "text/plain")
            .Produces<string>(StatusCodes.Status503ServiceUnavailable, "text/plain");

        app.MapGet("/_mock/servicebus/events/{subject}/sample", (string subject) =>
                Events.ContainsKey(subject) ? Results.Content(Sample(subject).ToJsonString(new JsonSerializerOptions { WriteIndented = true }), "application/json") : Results.NotFound())
            .WithName("ServiceBus_Sample")
            .WithTags(Provider)
            .WithGroupName(Provider.ToLowerInvariant())
            .WithSummary("Cuerpo de ejemplo de un evento")
            .Produces<JsonObject>()
            .Produces(StatusCodes.Status404NotFound);
    }

    public sealed record PublishResult(string Topic, string Subject, string MessageId, int BodyBytes);

    private static async Task<IResult> PublishAsync(string subject, HttpRequest request, IOptionsMonitor<MockOptions> options, CancellationToken ct)
    {
        if (!Events.ContainsKey(subject))
        {
            return Results.Text($"Subject desconocido. Válidos: {string.Join(", ", Events.Keys)}", "text/plain", null, StatusCodes.Status400BadRequest);
        }

        var settings = options.CurrentValue.ServiceBus;
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            return Results.Text("Publicación deshabilitada: configure MockIntegrations:ServiceBus:ConnectionString (emulador de Azure Service Bus).", "text/plain", null, StatusCodes.Status503ServiceUnavailable);
        }

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync(ct);
        if (string.IsNullOrWhiteSpace(body))
        {
            body = Sample(subject).ToJsonString();
        }

        await using var client = new ServiceBusClient(settings.ConnectionString);
        await using var sender = client.CreateSender(settings.Topic);
        var message = new ServiceBusMessage(BinaryData.FromString(body))
        {
            Subject = subject,
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString(),
        };
        await sender.SendMessageAsync(message, ct);
        return Results.Ok(new PublishResult(settings.Topic, subject, message.MessageId, System.Text.Encoding.UTF8.GetByteCount(body)));
    }

    private static JsonObject SampleFile() => new()
    {
        ["Base64String"] = "JVBERi0xLjQK",
        ["Description"] = "Documento de prueba",
        ["DirectoryCode"] = "00",
        ["DocumentTypeCode"] = "00",
        ["Ext"] = "pdf",
        ["GUID"] = Guid.Empty.ToString(),
    };

    /// <summary>Ejemplo de E12 §3.3: formato que corresponde a la intención del código (DataMember dentro de 'request').</summary>
    public static JsonObject Sample(string subject)
    {
        var header = new JsonObject { ["Token"] = "<CHKeys:Token>", ["User"] = "<CHKeys:User>" };
        JsonObject inner = subject switch
        {
            "WorkManagerInsertRequest" => new()
            {
                ["CheckWorkflow"] = true,
                ["Data"] = new JsonArray(new JsonObject { ["Field"] = "C0089", ["Key"] = "NumeroDocumento", ["Value"] = "1020304050" }),
                ["Files"] = new JsonArray(),
                ["FormCode"] = "83",
                ["Header"] = header,
                ["OfficeCode"] = "001",
                ["OperationUser"] = "mock",
                ["ProcessId"] = 1,
                ["nextStepUser"] = "mock",
            },
            "WorkManagerUpdateRequest" => new()
            {
                ["Data"] = new JsonArray(new JsonObject { ["Field"] = "C0098", ["Key"] = "Estado", ["Value"] = "APROBADO" }),
                ["FilterParameters"] = new JsonObject { ["Field"] = "C0001", ["Operator"] = "=", ["Value"] = "83-000123" },
                ["FormCode"] = "83",
                ["Header"] = header,
                ["OperationUser"] = "mock",
            },
            "WorkManagerDeleteRequest" => new()
            {
                ["FilterParameters"] = new JsonObject { ["Field"] = "C0001", ["Operator"] = "=", ["Value"] = "83-000123" },
                ["FormCode"] = "83",
                ["Header"] = header,
                ["OperationUser"] = "mock",
            },
            "WorkManagerAttachRequest" => new()
            {
                ["BarCode"] = "83-000123",
                ["CheckWorkflow"] = false,
                ["Files"] = new JsonArray(SampleFile()),
                ["Header"] = header,
                ["OperationUser"] = "mock",
            },
            _ => new()
            {
                ["Files"] = new JsonArray(SampleFile()),
                ["Header"] = header,
                ["OperationUser"] = "mock",
                ["WorkflowId"] = 9981,
            },
        };

        return new JsonObject
        {
            [Events[subject].PayloadProperty] = new JsonObject { ["request"] = inner },
            ["Id"] = Guid.NewGuid().ToString(),
            ["CreatedOn"] = DateTime.UtcNow.ToString("O"),
        };
    }
}
