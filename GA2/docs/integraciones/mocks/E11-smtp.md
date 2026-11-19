# E11 — SMTP (notificaciones por correo)

> Documento para implementar un **servidor SMTP mock** compatible con los envíos de correo de GA2. No es una API con operaciones: hay **cuatro emisores** con configuraciones distintas. Para cada uno se documenta qué se envía y cómo reacciona GA2 a los errores.

## 1. Resumen de emisores

| # | Emisor | Destino SMTP | Configurable | Estado |
|---|---|---|---|---|
| 1 | `EmailNotificacionRepository.EnviarAsync` (`GA2.Infraestructure/Repositories/EmailNotificacion`) | Sección `SMTP`: `cpvmp-01v5:25`, sin SSL | **Sí** | **Activo**: crédito (aprobación/rechazo) y reportes (preaprobados) |
| 2 | `LoginBusinessLogic.RecuperaContrasena` (Identity API) | `smtp-mail.outlook.com:587`, `EnableSsl=true`, credenciales de la sección `Email` (que **no existe** en ningún appsettings) | Host/puerto **fijos en código** | Activo, pero falla por falta de credenciales |
| 3 | `ParametrizacionBusinessLogic.SendNotification` (GA2 API) | Sondeo TCP a `smtp-mail.outlook.com:465` + envío por `:587` con **credenciales fijas en el código** | No | **Sin uso**: la única llamada (`SincronizacionCuentas`, ≈ línea 778) está comentada |
| 4 | `SmtpTestController` (`POST api/test/smtp/send`, sin `[Authorize]`) | `cpvmp-01v5:25` fijo | No | Diagnóstico |
| — | `EmailRepository` | Lee claves de configuración inexistentes | — | **Roto y sin uso** |

> **Mock recomendado:** un servidor SMTP de captura (smtp4dev, MailHog, Papercut, GreenMail) escuchando en el puerto configurado. Para el emisor 1 basta cambiar `SMTP:Host`/`SMTP:Puerto`. El emisor 2 requiere redirigir el DNS de `smtp-mail.outlook.com` hacia el mock con **STARTTLS** en el 587 (y un certificado de confianza) o cambiar el código.

## 2. Emisor 1 — `EmailNotificacionRepository.EnviarAsync` (el importante)

### 2.1 Configuración (`appsettings` → `SMTP`)

| Clave | Valor actual | Uso |
|---|---|---|
| `SMTP:Host` | `cpvmp-01v5` | Servidor |
| `SMTP:Puerto` | `25` | Puerto (`int.Parse`) |
| `SMTP:HabilitarSsl` | `false` | `EnableSsl` (STARTTLS) |
| `SMTP:UsarCredencialesPorDefecto` | `false` | `UseDefaultCredentials` |
| `SMTP:Usuario` | `notificaciones@cajahonor.gov.co` | Remitente (`From`) y usuario de autenticación |
| `SMTP:Clave` | vacío | Sólo si `Usuario` **y** `Clave` tienen valor se envía `AUTH` |
| `SMTP:NombreRemitente` | `CajaHonor` | Nombre visible del remitente |
| `SMTP:Timeout` | `30000` | ms |

Con la configuración actual la sesión es **SMTP sin TLS ni AUTH** en el puerto 25: el mock debe aceptar `EHLO`, `MAIL FROM`, `RCPT TO` y `DATA` sin autenticación.

### 2.2 Mensaje que se envía

| Parte | Contenido |
|---|---|
| `From` | `"CajaHonor" <notificaciones@cajahonor.gov.co>` |
| `To` | `request.Destinatarios` (uno o varios) |
| `Subject` | Según el flujo (tabla 2.3) |
| Cuerpo | **HTML** (`IsBodyHtml=true`), plantilla leída de `{CarpetaBlogStorage:CarpetaServidorStorage}\template-email\{Modulo}\{RutaHtml}` con los marcadores `{{Clave}}` sustituidos; `{{ImagenEncabezado}}` se reemplaza por una imagen PNG **embebida como `data:image/png;base64,...`** |
| Adjuntos | Si `EnvioBase64=true`: un PDF en memoria `{NombreDocumentoAdjunto}.pdf` (`application/pdf`). Si no: archivos de disco (`Adjuntos[]`) que existan |

> **Dependencia de archivos:** antes de conectarse al SMTP, GA2 lee la plantilla HTML y la imagen del disco. Si faltan, falla con `FileNotFoundException` **sin intentar el envío**. En un entorno con mock hay que copiar la carpeta `template-email` o apuntar `CarpetaServidorStorage` a una copia.

### 2.3 Correos que genera GA2 hoy

| Flujo (código) | `Modulo` / plantilla | `Subject` | Marcadores | Adjunto |
|---|---|---|---|---|
| Aprobación de crédito (`CreditoBusinessLogic` ≈423) | `credito` / `TemplateAprobacionCredito.html` | `Comunicado Oficial - aprobación de crédito` | `NombreCompleto` | PDF *"Comunicación oficial de avalúo con concepto favorable"* |
| Rechazo de crédito (≈526, ≈10574, ≈10726) | `credito` / `TemplateRechazoCredito.html` | `Comunicado Oficial - Rechazo` | `NombreCompleto` | — |
| Preaprobado leasing (`ReportesBusinessLogic` ≈4102) | `credito` / `TemplatePreaprobadoHipotecario.html` | `Comunicación oficial de Preaprobado de crédito leasing` | `NombreCompleto` | PDF *"Comunicación oficial de preaprobado"* |
| Preaprobado hipotecario (≈4455) | `credito` / `TemplatePreaprobadoHipotecario.html` | `Comunicación oficial de Preaprobado de crédito hipotecario` | `NombreCompleto` | PDF *"Comunicación oficial de preaprobado"* |

Imagen de encabezado: `encabezadoRechazoCredito.png` en todos los casos.

### 2.4 Respuestas SMTP y efecto en GA2

| Comportamiento del mock | Efecto |
|---|---|
| `250` en todo el diálogo | Envío OK; se registra la auditoría `ENVIADO` (`AuditoriaEmailPeticion`) |
| Rechazo (`5xx` en `RCPT`/`DATA`), conexión rechazada, timeout (30 s) | `SmtpException`: se registra la auditoría `ERROR` y **se relanza**; el flujo de negocio que la invocó falla (→ HTTP 409 vía `ErrorValidationMiddleware`, salvo que el llamador la capture) |

## 3. Emisor 2 — recuperación de contraseña (Identity API)

`From`: `Email:UserName` ("CajaHonor"), `To`: el correo del usuario, `Subject`: `Restablecer contraseña`, cuerpo HTML con el enlace de restablecimiento. `smtp-mail.outlook.com:587`, `EnableSsl=true` (STARTTLS) y credenciales `Email:UserName/Password`. Envío **síncrono** (`smtp.Send`).

## 4. Emisor 3 — `ParametrizacionBusinessLogic.SendNotification` (sin uso; se documenta por si se reactiva)

1. Abre una conexión TCP a `smtp-mail.outlook.com:465` (sondeo de conectividad).
2. Envía por `smtp-mail.outlook.com:587` con `EnableSsl=true` y credenciales fijas en el código, **desactivando la validación de certificados** (`ServicePointManager.ServerCertificateValidationCallback => true`).
3. `From` = cuenta institucional fija; `To` = una dirección fija de desarrollo (`desarrollok11@…`); `Subject` = `Restablecer contraseña`; cuerpo HTML.

> Con destinatario y credenciales fijos en el código, este emisor parece código de prueba. Hoy no se ejecuta, pero las credenciales siguen en el repositorio.

## 5. Emisor 4 — `POST api/test/smtp/send`

Abre un `SmtpClient("cpvmp-01v5", 25)` y envía un correo de prueba. Útil para comprobar el mock una vez cambiado el DNS/host (el host está fijo en el código).

## 6. Escenarios mínimos recomendados

| # | Escenario | Mock |
|---|---|---|
| 1 | Envío exitoso con adjunto PDF | Capturar y verificar `Subject`, destinatarios y adjunto |
| 2 | Destinatario rechazado | `550` en `RCPT TO` |
| 3 | Servidor caído | Puerto cerrado (GA2 falla tras el timeout) |
| 4 | Lentitud | Retardo > 30 s |
