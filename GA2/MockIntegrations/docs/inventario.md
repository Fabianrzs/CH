# Inventario de integraciones, proveedores y operaciones

Fuente: análisis del código de GA2 (rama `develop`) documentado en [docs/integraciones/INVENTARIO_INTEGRACIONES.md](../../docs/integraciones/INVENTARIO_INTEGRACIONES.md) (análisis completo con evidencias) y en las fichas de contrato [docs/integraciones/mocks/](../../docs/integraciones/mocks/README.md). En esta fase se volvieron a verificar contra el código los puntos de mayor riesgo (rutas de TusDatos, `Nodes[1]` de Fénix/Haberes, `ProviderRequestFactory`, `ValidacionesTU`, DTO de Dodo, Service Bus) y se comprobó el contrato **ejecutando** los proxies y clientes reales de GA2 contra el mock.

## 1. Stack de GA2 (resumen)

C# / **.NET 5** (ASP.NET Core 5, Ardalis.ApiEndpoints, Autofac), Dapper sobre SQL Server con procedimientos almacenados, WCF Core 4.9 para SOAP, `HttpClient` para REST, `Azure.Messaging.ServiceBus` 7.9, Serilog, Hangfire. Hosts: API GA2, API BUA, API Identity y Worker. Las URLs salen de `appsettings` (`SvcUtils`, `Integrationes`, `CHKeys`, `SMTP`, `EventBusConnection`), salvo las excepciones de la §4.

## 2. Inventario general (integraciones confirmadas)

| Id | Proveedor | Propósito | Consumidor → destino | Protocolo | Operaciones (negocio / total) | Redirigible | Mock | Certeza del contrato |
|---|---|---|---|---|---|---|---|---|
| E1 | Seven ERP (Digital Ware) | Contabilidad, CDP, cuentas por pagar, terceros | API GA2 → 7 ASMX | SOAP 1.1 | 6 / 29 | Sí | ✔ [doc](providers/E01-seven.md) | Estructura validada; raíz del XML de `...Result` sin confirmar |
| E2 | WorkManager / DODO | Gestor documental y BPM | API GA2, BUA, Service Bus → WCF | SOAP 1.1 | 6 / 29 | Sí | ✔ [doc](providers/E02-workmanager.md) | Validada |
| E3 | BUA (Caja Honor) | Base Única de Afiliados | API BUA (ws.02), API GA2 (ws.01) → WCF | SOAP 1.1 | 1 / 1 (2 proxies) | ws.02 sí, **ws.01 no** | ✔ [doc](providers/E03-bua.md) | Validada con ambos proxies |
| E4 | Vigía | Listas LA/FT (SARLAFT) | API GA2 → PHP | SOAP 1.1 RPC/encoded | 1 / 1 | Sí | ✔ [doc](providers/E04-vigia.md) | Estructura validada; dominio de valores sin confirmar |
| E5 | Fénix | Biometría | API GA2 → ASMX (DataSet) | SOAP 1.1 | 1 / 2 | Sí | ✔ [doc](providers/E05-fenix.md) | Validada (con la lectura de GA2) |
| E6 | Haberes (MCD) | Certificados de haberes | API GA2 → ASMX | SOAP 1.1 | 3 / 8 | Sí | ✔ [doc](providers/E06-haberes.md) | Validada |
| E7 | TransUnion DecisionCentre | IDVision/OTP, score AQM, historial | API GA2 → WCF (XML en texto) | SOAP 1.1 | 1 método, 6 pasos / 33 | **No** (URL fija) | ✔ [doc](providers/E07-transunion.md) | Validada con el cliente de GA2; contenido real sin muestras |
| E8 | TusDatos (API Gateway) | PEP, riesgo, Vigía LA/FT | API GA2 → gateway | REST/JSON + `x-api-key` | 3 / 3 | Sí | ✔ [doc](providers/E08-tusdatos.md) | Validada (DTO de GA2); dominios sin confirmar |
| E9 | Dodo REST | Nueva API del gestor documental | API GA2 (pass-through) → REST | REST/JSON | 0 / 29 (pass-through) | Sí | ✔ [doc](providers/E09-dodo-rest.md) | Validada (DTO de GA2) |
| E10 | Pasarela ktrionApi (Caja Honor) | Intermediario a WorkManager y Haberes | API GA2 → REST | REST/JSON + Bearer | 5 / 11 | Sí | ✔ [doc](providers/E10-ktrionapi.md) | Validada (Newtonsoft + tipos de GA2) |
| E11 | SMTP (relay interno; Outlook) | Notificaciones por correo | API GA2 (relay); Identity (Outlook) | SMTP | 1 emisor activo configurable | Relay sí; Outlook no | ✔ [doc](providers/E11-smtp.md) | Validada con `SmtpClient` |
| E12 | Azure Service Bus | Eventos hacia WorkManager | Productor externo → API GA2 (consumidor) | AMQP | 5 eventos | Sí (emulador + proxy TLS) | ✔ emulador + publicador [doc](providers/E12-servicebus.md) | Estructura validada; productor real desconocido |

## 3. Inventario de operaciones por proveedor

El inventario completo y siempre actualizado (153 operaciones con ruta, SOAPAction, verbo, uso en GA2, clave de configuración, código de GA2 que construye el request y lee el response, certeza y escenarios) es el **catálogo del mock**:

- `GET http://localhost:5080/_mock/catalog` (JSON), y
- la [matriz de cobertura](matriz-cobertura.md), generada a partir de él.

Clasificación de las 153 operaciones:

| Clasificación | Cantidad | Tratamiento en el mock |
|---|---|---|
| Usadas por la lógica de negocio de GA2 | 34 | Respuesta con contrato validado y escenarios de error |
| Expuestas por GA2 como *pass-through* (Seven, Dodo, `Form_GetData`) | 53 | Respuesta con contrato validado |
| En el contrato/cliente pero sin llamadas en GA2 | 66 | 5 de WorkManager con respuesta validada; el resto, **SOAP Fault o HTTP 501 explícito** (sin contrato de respuesta validado) |

Especificación de request y response de cada operación: fichas [E01–E12](../../docs/integraciones/mocks/README.md) y ejemplos por operación en Swagger.

## 4. Configuraciones sin uso demostrado y comportamiento no determinable

| Elemento | Estado | Tratamiento |
|---|---|---|
| OTP WsUtilidades (`SvcUtils:ServiceOtpSvc`) | Registrado; sin consumidores (el OTP real va por TransUnion) | No simulado |
| Vigía ASMX: `F_CARGATERCERO`, `F_CARGAPRODUCTOTERCERO`, `F_CARGATRANSACCION`, `F_VERIFICATERCEROV3` | Registrados; sin llamadas o `NotImplementedException` | No simulados |
| Pasarela para BUA/Vigía/Fénix (`IntegracionesGA2*`), `ProviderRequestG2`, `GestorDocumentalDodo`, `EmailRepository`, `AzureStorageProvider` | Inyectados o configurados, nunca invocados | No simulados (rutas de la pasarela sin uso → 501) |
| `SvcUtils:TransUnion` / `TrasnUnion`, `IdVision:estado`, `Aqm:estado` | Claves que el código no lee | — |
| Semántica de negocio de `GetCuentaContable`, `GetCuentaContableTO`, `GetAporte` (Seven); dominios de `Encontrado`/`Listas` (Vigía), `statusJobTusDatos`, `riskLAFT` (TusDatos); contenido real de los `DCResponse` (TransUnion) | **No determinable con el código** (GA2 sólo los reenvía o no los interpreta) | Valores de ejemplo configurables; marcados `StructureOnly` en el catálogo ([pendientes](pendientes.md)) |
| Productor de los eventos de Service Bus | **Fuera del repositorio** | El mock actúa de productor con el formato del código |

## 5. Autenticación, timeouts y políticas de conexión que afectan a los mocks

| Integración | Autenticación que exige GA2 / el mock | Timeout en GA2 |
|---|---|---|
| SOAP (Seven, WorkManager, BUA, Vigía, Fénix, Haberes) | Ninguna HTTP; WorkManager lleva `Header{User,Token}` en el DTO (el mock no lo valida) | `TimeSpan.MaxValue` (sin límite); BUA ws.01 1 min, ws.02 5 min |
| TransUnion | Usuario y clave dentro del `DCRequest` (el mock no los valida) | Sin límite |
| TusDatos | `x-api-key` (obligatorio en el mock) | 15 s |
| Dodo | `header{user,token}` en el body de Forms | 15 s |
| ktrionApi | Token de `api/autenticacion` como Bearer (validado por el mock) | 100 s; GA2 **acepta cualquier certificado TLS** |
| SMTP | Sin TLS ni AUTH (el mock acepta AUTH si se configura) | 30 s |
| Service Bus | SAS (clave pública del emulador) | Reintentos del SDK |

Reintentos: GA2 sólo los tiene en el SDK de Service Bus y en el reintento funcional de BUA (`tipo_Id` 1 → 4 → 5).
