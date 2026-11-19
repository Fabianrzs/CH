using MockIntegrations.Api.Configuration;
using MockIntegrations.Api.Infrastructure.Catalog;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.Smtp;

/// <summary>
/// E11 — SMTP. El protocolo SMTP no es HTTP: GA2 habla con <see cref="SmtpCaptureServer"/> por TCP
/// (<c>SMTP:Host</c>/<c>SMTP:Puerto</c>). Estos endpoints HTTP sólo sirven para inspeccionar el buzón
/// de captura en las pruebas; no forman parte del contrato de GA2. Contrato: docs/providers/E11-smtp.md.
/// </summary>
public static class SmtpMock
{
    public const string Provider = "Smtp";

    public static void Map(WebApplication app)
    {
        var port = app.Services.GetRequiredService<IOptions<MockOptions>>().Value.Smtp.Port;
        var catalog = app.Services.GetRequiredService<MockCatalog>();
        catalog.Add(new MockOperation
        {
            Provider = Provider,
            Name = "EnviarNotificacion",
            Protocol = MockProtocol.Smtp,
            Method = "SMTP",
            Route = $"smtp://<host-del-mock>:{port}",
            Usage = Ga2Usage.Business,
            Summary = "Correos de crédito (aprobación, rechazo) y reportes (preaprobados), HTML con PDF adjunto. Sin TLS ni AUTH.",
            Ga2ConfigKey = "SMTP:Host, SMTP:Puerto, SMTP:HabilitarSsl=false",
            Ga2Source = "GA2.Infraestructure/Repositories/EmailNotificacion/EmailNotificacionRepository.cs",
            Scenarios = [new("destinatario-rechazado", "Agregue el correo a MockIntegrations:Smtp:RejectRecipients: RCPT TO responde 550 y GA2 registra la auditoría ERROR.")],
        });
        catalog.Add(new MockOperation
        {
            Provider = Provider,
            Name = "RecuperarContrasena",
            Protocol = MockProtocol.Smtp,
            Method = "SMTP",
            Route = "smtp-mail.outlook.com:587 (fijo en código)",
            Usage = Ga2Usage.Business,
            Summary = "Identity API: restablecer contraseña. Host fijo en código con STARTTLS: no se puede redirigir por configuración.",
            Ga2Source = "GA2.Domain/Core/Login/LoginBusinessLogic.cs (RecuperaContrasena)",
            Certainty = ContractCertainty.NotImplemented,
            Notes = "Requiere redirigir el DNS de smtp-mail.outlook.com y un servidor con STARTTLS y certificado de confianza (no incluido).",
        });

        var group = app.MapGroup("/smtp/messages").WithTags(Provider).WithGroupName(Provider.ToLowerInvariant());

        group.MapGet("", (SmtpMailbox mailbox) => mailbox.All().Select(m => m with { Raw = "" }))
            .WithName("Smtp_ListMessages")
            .WithSummary("Lista los correos capturados (sin el cuerpo)")
            .WithDescription("Inspección del buzón de captura. El campo `raw` se omite; use `GET /smtp/messages/{id}`. No es un endpoint de GA2.")
            .Produces<IEnumerable<CapturedMessage>>();

        group.MapGet("/{id:long}", (long id, SmtpMailbox mailbox) => mailbox.Find(id) is { } m ? Results.Ok(m) : Results.NotFound())
            .WithName("Smtp_GetMessage")
            .WithSummary("Detalle de un correo capturado, con el mensaje RFC 5322 completo")
            .Produces<CapturedMessage>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:long}/eml", (long id, SmtpMailbox mailbox) =>
                mailbox.Find(id) is { } m ? Results.File(System.Text.Encoding.UTF8.GetBytes(m.Raw), "message/rfc822", $"mensaje-{id}.eml") : Results.NotFound())
            .WithName("Smtp_DownloadMessage")
            .WithSummary("Descarga el correo como .eml")
            .Produces(StatusCodes.Status200OK, contentType: "message/rfc822")
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("", (SmtpMailbox mailbox) =>
            {
                mailbox.Clear();
                return Results.NoContent();
            })
            .WithName("Smtp_ClearMessages")
            .WithSummary("Vacía el buzón de captura")
            .Produces(StatusCodes.Status204NoContent);
    }
}
