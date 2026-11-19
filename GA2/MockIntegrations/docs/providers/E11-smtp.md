# E11 · SMTP — servidor de captura (protocolo SMTP, no HTTP)

**Especificación:** [docs/integraciones/mocks/E11-smtp.md](../../../docs/integraciones/mocks/E11-smtp.md).
**Código:** [`Providers/Smtp/SmtpCaptureServer.cs`](../../src/MockIntegrations.Api/Providers/Smtp/SmtpCaptureServer.cs), [`SmtpMock.cs`](../../src/MockIntegrations.Api/Providers/Smtp/SmtpMock.cs).

MockIntegrations levanta un **servidor SMTP propio** (TCP, puerto `MockIntegrations:Smtp:Port`, por defecto **2525**). No necesita herramientas externas.

| Emisor en GA2 | Configuración | ¿Usa el mock? |
|---|---|---|
| `EmailNotificacionRepository` (aprobación/rechazo de crédito, preaprobados) | `SMTP:Host = localhost`, `SMTP:Puerto = 2525`, `SMTP:HabilitarSsl = false` | **Sí** |
| `LoginBusinessLogic.RecuperaContrasena` (Identity API) | `smtp-mail.outlook.com:587` + STARTTLS **fijos en código** | No (ver [pendientes](../pendientes.md)) |
| `SmtpTestController` (`POST api/test/smtp/send`) | `cpvmp-01v5:25` fijo | Sólo con DNS/hosts hacia el mock y el puerto 25 |

## Protocolo soportado

`EHLO`/`HELO`, `AUTH LOGIN` (con o sin respuesta inicial) y `AUTH PLAIN` con cualquier credencial, `MAIL FROM`, `RCPT TO`, `DATA`, `RSET`, `NOOP`, `QUIT`. Sin TLS (como la configuración de GA2). Verificado con `System.Net.Mail.SmtpClient` (el mismo cliente que usa GA2): HTML, asunto UTF-8 y PDF adjunto.

## Inspección (HTTP, documento OpenAPI `smtp`)

| Endpoint | Uso |
|---|---|
| `GET /smtp/messages` | Correos capturados: remitente, destinatarios, asunto decodificado, adjuntos |
| `GET /smtp/messages/{id}` | Detalle con el mensaje RFC 5322 completo |
| `GET /smtp/messages/{id}/eml` | Descarga `.eml` |
| `DELETE /smtp/messages` | Vacía el buzón |

## Escenarios

- **Destinatario rechazado:** las direcciones de `MockIntegrations:Smtp:RejectRecipients` (por defecto `rechazado@mock.test`) reciben `550` en `RCPT TO`, y GA2 registra la auditoría `ERROR` y relanza `SmtpFailedRecipientException`.
- **Servidor caído:** `MockIntegrations:Smtp:Enabled = false` (puerto cerrado).

**Dependencia de GA2:** antes de enviar, GA2 lee las plantillas HTML de `CarpetaBlogStorage:CarpetaServidorStorage\template-email`. Deben existir en el entorno de pruebas.
