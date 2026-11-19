# Inventario técnico de integraciones — GA2 (Kriterion / Caja Honor)

> **Fase 1: sólo análisis.** El documento sale de leer el código de la rama `develop` (snapshot del 2026-10-09). No se modificó la lógica del proyecto ni se implementaron mocks.
> Para afirmar que una integración está **activa** se exigió algo más que una URL o un paquete: un cliente registrado en DI **y** al menos una ruta de llamada sin comentar que llegue desde un endpoint, un job o un *hosted service*. El uso se comprobó sobre una copia del código con los comentarios eliminados (`//` y `/* */`), porque el repositorio tiene muchísimo código comentado.
> **Alcance:** el documento cubre únicamente las integraciones de la tabla del §0 (E1–E12 y las registradas sin uso, L*).
> **Contratos detallados para mocks:** cada integración tiene su documento técnico en [`mocks/`](mocks/README.md).
> **Ningún secreto se reproduce aquí.** Donde hay credenciales en el repositorio sólo se indica el archivo (ver §6).

---

## 0. Resumen ejecutivo

| # | Integración | Tipo | Protocolo | Estado real | Toggle en BD | Prioridad para mock |
|---|---|---|---|---|---|---|
| E1 | **Seven ERP** (7 servicios ASMX) | Externa (ERP contable) | SOAP 1.1 / ASMX | **Activa** (núcleo contable: créditos, cesantías, reintegros, cartera, embargos) | No | Alta |
| E2 | **WorkManager / DODO** (gestor documental + BPM) | Externa | SOAP WCF (`basicHttp`) | **Activa** | `DODO.ESTADO` (parcial) | Alta |
| E3 | **BUA SOAP** (Base Única de Afiliados) – `Kriterion.ws.01` y `ws.02` | Externa (sistema Caja Honor) | SOAP WCF | **Activa** (dos clientes distintos) | `Bua` (sólo en un flujo) | Alta |
| E4 | **Vigía** – verificación de terceros (listas LA/FT) | Externa | SOAP RPC/encoded (PHP) | **Activa** | `VIGIA.ESTADO` | Alta |
| E5 | **Fénix** – biometría | Externa | SOAP ASMX (DataSet) | **Activa** | `FENIX.ESTADO` | Media |
| E6 | **Haberes** – Modelo Canónico (MCD) | Externa | SOAP ASMX | **Activa** (Release); en DEBUG una operación usa la pasarela E10 | No | Media |
| E7 | **TransUnion DecisionCentre** (IDVision / AQM / scoring) | Externa (tercero) | SOAP WCF (payload XML en string) | **Activa** | No (los flags `estado` del config **no se leen**) | Alta |
| E8 | **TusDatos** (vía API Gateway de Caja Honor `kiosko-dev`) | Externa (tercero detrás de gateway) | REST JSON + `x-api-key` | **Activa** (endpoints nuevos) | `TusDatos.ESTADO` | Media |
| E9 | **Dodo REST** (`dodo-docs-test:10081`) | Externa | REST JSON | **Activa sólo como pass-through** (`DodoController`) | `Dodo.ESTADO` | Baja/Media |
| E10 | **Pasarela Caja Honor "ktrionApi"** (`/REST/KriterionDev/ktrionApi/`) | Externa (gateway propio) | REST JSON + Bearer | **Parcial**: sólo WorkManager y Haberes, y **sólo en compilación DEBUG** | No | Media (entornos dev) |
| E11 | **SMTP** (relay interno `cpvmp-01v5` y Outlook 365) | Externa | SMTP | **Activa** (notificaciones, recuperación de contraseña) | No | Media |
| E12 | **Azure Service Bus** (topic `chonorintegraciones`) | Externa (nube) | AMQP | **Activa sólo como consumidor**; el productor está fuera del repo | No | Alta (bloquea el arranque) |
| L* | OTP WsUtilidades, Vigía ASMX (carga tercero/producto/transacción, verificación v3), pasarela para BUA/Vigía/Fénix, `ProviderRequestG2`, `GestorDocumentalDodo`, `EmailRepository`, proveedor Azure del `StorageFactory` | — | — | **Registradas o configuradas, sin uso real** (ver §4) | — | No hace falta mock |

---

## 1. Reconocimiento del proyecto

### 1.1 Stack tecnológico

| Aspecto | Detalle |
|---|---|
| Lenguaje / runtime | C# sobre **.NET 5** (`global.json` → SDK 5.0.401; todos los `.csproj` en `net5.0`) |
| Web | ASP.NET Core 5 + **Ardalis.ApiEndpoints** (un endpoint por clase) + algunos `ControllerBase` clásicos |
| DI | `Microsoft.Extensions.DependencyInjection` + **Autofac** (sólo GA2 API, que lo necesita el Event Bus) |
| Acceso a datos | **Dapper** sobre `System.Data.SqlClient`, casi todo con procedimientos almacenados. El nombre del SP se deriva del nombre del método (`HelperDBParameters.BuilderFunction` con `[CallerMemberName]` → `dbo.<Método>`). EF Core 5 aparece referenciado pero apenas se usa |
| Clientes SOAP | `System.ServiceModel.*` 4.9 (WCF Core). Proxies generados (`Reference.cs`) con `dotnet-svcutil` / Connected Services |
| Clientes HTTP | `HttpClient` "a mano" (`ProviderRequestFactory`) y, en código nuevo, `IHttpClientFactory` tipado (`AddHttpClient<>`) |
| Mensajería | `Azure.Messaging.ServiceBus` 7.9 (bloque `EventBusServiceBus`, estilo eShopOnContainers) |
| Logging | **Serilog** (consola y archivo `C:\inetpub\Logs`) |
| Reportes / documentos | RDLC (`ReportViewerCore`, `AspNetCore.Reporting`), IronPdf, iTextSharp, PdfSharp, NReco, Select.HtmlToPdf, wkhtmltopdf, EPPlus, ClosedXML, Aspose.Cells, IronOcr. **Son librerías locales, no integraciones** |
| Auth | JWT HS256 con clave simétrica compartida entre las APIs (`JwtOptions:Key`) |
| Pruebas | `GA2Tests` (MSTest) con **un solo archivo**. No hay pruebas de integración ni dobles de servicios externos |

### 1.2 Estructura y componentes

```
GA2.sln
└── src/
    ├── GA2.Apis/
    │   ├── GA2/        (Web API principal: ~1 100 endpoints; crédito, cartera, cesantías, reportes, integraciones)
    │   ├── BUA/        (Web API "Base Única de Afiliados": 86 endpoints; clientes, cuentas, cierres)
    │   ├── Identity/   (Web API de autenticación: 33 endpoints; login GA2/UEJ/PAA, menús, roles)
    │   ├── Worker/     (host de BackgroundService: cargues de nómina; no usa integraciones del alcance)
    │   └── GA2Tests/   (MSTest, mínimo)
    ├── GA2.Application/ (Dto, Main → ProviderRequest/* = pasarela Caja Honor, TransUnion, Tokens)
    ├── GA2.Domain/      (Core → lógica de negocio + ReferenciasSvc/* = clientes de integración + ServiciosSOA/* + Connected Services/*)
    ├── GA2.Infraestructure/ (Data, Repositories → Dapper, Email, IntegracionesConfig)
    ├── GA2.BuildingBlocks/  (EventBus, EventBusServiceBus)
    └── GA2.Transversals/    (Commons → opciones de configuración y constantes; Mappers → AutoMapper)
```

**Hosts y puntos de entrada**

| Host | Entrada | Arranque relevante para integraciones |
|---|---|---|
| GA2 API | `src/GA2.Apis/GA2/Program.cs` / `Startup.cs` | Registra clientes SOAP/REST (`Extensions/DIManager.cs`, `Extensions/HttpClientExtensions.cs`) y el **Event Bus** (`Extensions/EventBusExtensions.cs`). En `Configure` **resuelve `IEventBus` y se suscribe a 5 eventos** (abre la conexión a Service Bus al arrancar) |
| BUA API | `src/GA2.Apis/BUA/Startup.cs` | `Extensions/DIManager.cs` y `Extensions/HttpClientExtensions.cs` (registra TusDatos/Dodo, aunque su `appsettings` **no tiene** la sección `Integrationes`) |
| Identity API | `src/GA2.Apis/Identity/Startup.cs` | `Extensions/DIManager.cs` (SMTP Outlook en recuperación de contraseña) |

### 1.3 Configuración

* Secciones relevantes: `SvcUtils` (URLs SOAP → `ConfiguracionesSvc`), `CHKeys` (pasarela Caja Honor + usuario/token de WorkManager/Dodo), `AppMain` (identidad de la app ante la pasarela), `Integrationes` (TusDatos, Dodo), `SMTP`, `IdVision`, `Aqm`, `EventBusConnection`, `SubscriptionClientName`.
* `appsettings.Desarrollo.json` es una **plantilla de despliegue** con *placeholders* `{{...}}` (`{{ApiKeyTusDatos}}`, `{{claveIDVision}}`, `{{claveAqm}}`, …). Los valores reales se inyectan en el pipeline, fuera del repositorio.
* No hay `.env`, Dockerfile, IaC ni pipelines en el repositorio. Los nombres de host (`cpvmp-*`) son servidores *on-premise* de Caja Honor.
* **Inconsistencias en la configuración:**
  * La clave de TransUnion aparece como `TransUnion` en algunos archivos y como `TrasnUnion` (con errata) en otros. **No tiene importancia práctica**: el código no lee ninguna de las dos, la URL está fija en `ValidacionesTU.cs:16`.
  * `VigiaVerificarTerceroUrlSvc` apunta al endpoint PHP `https://vigia.cajahonor.gov.co:443/WebSvc/SOAP_VerificaTercero.php` en la API GA2, pero al ASMX `http://cpvmp-22fw:8080/...` en la API BUA. Dos contratos distintos comparten la misma clave (ver E4).
  * `appsettings.json` de BUA tiene una coma final (`"AllowedHosts": "*",}`), lo que no es JSON estricto.

---

## 2. Mecanismos transversales (importantes para diseñar los mocks)

### 2.1 Interruptores de integración en base de datos
El SP `dbo.ObtenerParametrosIntegraciones` devuelve filas `{PARAM_ID, INTEGRACION, PARAMETRO, VALOR}`. Se consultan así:

| Consumidor | Clave | Comportamiento con valor `false` |
|---|---|---|
| `VigiaBusinessLogic.cs:54` | `VIGIA` / `ESTADO` | No llama a Vigía; devuelve la respuesta fija del SP `ConsultarResponseVigiaDesactivado` (columnas `VIG_ENCONTRADO`, `VIG_LISTAS`, `VIG_METODO`) |
| `FenixBusinessLogic.cs:29`, `:103`; `ProspeccionBusinessLogic.cs:10547`, `:10596` | `FENIX` / `ESTADO` (y Vigía en prospección) | Omite la biometría |
| `GestorDocumentSolicitudes.cs:54` y siguientes; `GestorDocumentalReintegrosBL`, `GestorDocumentalBusinessLogicFs` | `DODO` / (primer registro) con `VALOR == "true"` | No radica en WorkManager |
| `ClientsBusinessLogic.cs:501` (BUA API, **vía HTTP a la GA2 API**) | `Bua` | Con `"false"` usa sólo la BD local; con otro valor consulta además el SOAP BUA `ws.02` |
| `CommonIntegracionCliente.ValidateIntegrationStatus` (TusDatos, Dodo) | `TusDatos` / `ESTADO`, `Dodo` / `ESTADO` | Lanza `InvalidOperationException` **fuera del `try`** del cliente: la excepción llega al `ErrorValidationMiddleware` y el frontend recibe **HTTP 409** (no `IsSuccess=false`) |

> **Para los mocks:** estos interruptores ya permiten "apagar" Vigía, Fénix, WorkManager y TusDatos/Dodo sin código nuevo. Pero apagar no es simular: con el interruptor en `false` el flujo que se ejecuta es otro. Hay dos implementaciones de `IEstadoIntegracionProvider`: `EstadoIntegracionRepository` (GA2, va directo a la BD y compara sin distinguir mayúsculas) y `EstadoIntegracionClient` (BUA, llama a `GA2 /api/parametrizacion/ObtenerParametrosIntegraciones` y compara `ToUpper()` contra el nombre del enum `"TusDatos"`/`"Dodo"`, así que **nunca coincide**: `First()` lanza la excepción. Es un defecto latente, aunque hoy BUA no consume esos clientes).

### 2.2 Auditoría de llamadas
* SOAP: `IAuditoriaServicesBusinessLogic.EjecutarServicio<TReq,TRes>(delegate, request, ReferenciaSvc.X)` envuelve las llamadas de Fénix, Haberes, Vigía, Seven, WorkManager y OTP y deja registro en BD. El enum `ReferenciaSvc` (`AuditoriaServices/IAuditoriaServicesBusinessLogic.cs:17`) sirve de catálogo oficial: WorkManagent‑DODO, Seven, BUA, Fenix, Haberes, OTP, Vigia, TusDatos, Dodo, Internal.
* REST: `CommonIntegracionCliente` guarda `ReportIntegracion` (endpoint, verbo, request, response, status, duración) con `IAuditoriaRepository.InsertReportIntegracionesRest`.

### 2.3 Rutas distintas por configuración de compilación (`#if DEBUG`)
| Archivo | DEBUG | Release |
|---|---|---|
| `Credito/WorkManager/GestorDocumentalBusinessLogic.cs:95`, `:300` | Pasarela ktrionApi (E10): `api/formUpdate`, `api/forminsert` | SOAP WorkManager directo (`FormUpdateAsyncCH`, `FormInsertAsyncCH`) |
| `Credito/Haberes/HaberesBusinessLogic.cs:100` | Pasarela ktrionApi: `api/generarreportehaberes` | SOAP Haberes `GenerarReporte` |

> **Para los mocks:** el mismo flujo de negocio sale a sistemas distintos según se compile en Debug o en Release. Un entorno de pruebas tiene que fijar la configuración de compilación o simular ambas rutas.

### 2.4 Patrones de cliente
* **SOAP:** `BasicHttpBinding` con `SecurityMode = None` si la URL es `http` y `Transport` si es `https`. Timeouts en `TimeSpan.MaxValue` (las llamadas nunca expiran), `MaxReceivedMessageSize` entre 1 MB y `int.MaxValue`, sin credenciales de mensaje. **Para apuntar a un mock basta cambiar la URL en `SvcUtils`**, salvo en las excepciones señaladas (BUA `ws.01` y TransUnion, que ignoran la configuración).
* **`ProviderRequestFactory` (base de la pasarela E10):** JSON con Newtonsoft (camelCase); la pasarela envía el Bearer obtenido en `api/autenticacion`. **Acepta cualquier certificado TLS** (`ServerCertificateCustomValidationCallback => true`, líneas 292 y 311). Un estado 401/403 → `Exception`; cualquier otro error → `HttpRequestException`.
* **REST nuevo (`CommonIntegracionCliente`):** `HttpClient` tipado, System.Text.Json sin distinguir mayúsculas. **El 404 se trata como éxito con `Data = null`**, cualquier otro error HTTP como `IsSuccess = false`, con toggle y auditoría.

---

## 3. Integraciones externas (fichas)

### E1. Seven ERP (contabilidad, presupuesto, terceros, facturación)
* **Sistema:** ERP Seven (Digital Ware), expuesto como servicios ASMX en `http://cpvmp-306x/Seven/...` (los `Reference.cs` traen por defecto `cpvmp-34xp`).
* **Namespace SOAP:** `http://seven/`. `SOAPAction` = `http://seven/<Operación>`.
* **Clientes:** `src/GA2.Domain/Core/ReferenciasSvc/Seven/*/ConsumoXxx.cs` (interfaces `IConsumoSCnMcont`, `IConsumoWSAportes`, `IConsumoCDP`, `IConsumoWSFactura`, `IConsumoWSProveedor`, `IConsumoWSTercero`, `IConsumoWSWintipro`), registrados en la API GA2. Hay un segundo proxy de `scnmcont.asmx` en `Connected Services/ServiceSeven/MovimientoContableService.cs` (con validador y mapeo NIF propios) que usa `CarteraBusinessLogic.cs:12609`.

| Servicio (clave `SvcUtils`) | Endpoint | Operaciones | Uso real desde negocio (además del pass-through) |
|---|---|---|---|
| `SevenWSMovContable` | `/Seven/WsMovContable/scnmcont.asmx` | InsertarCnMcont, InsertarCnDmcon, AplicarCnMcont, RevertirCnMcont, AnularCnMcont, **InsertarMovContable**, SetContabToper | `InsertarMovContable`: Cartera, Cesantías, Crédito (≈8 puntos), Reintegros. `AnularCnMcont`: Cartera |
| `SevenWSCdp` | `/Seven/WsCdp/WPgOblig.asmx` | InsertarCDP, InsertarDetCDP, AplicarCDP, AnularCDP, **CrearCDP** | `CrearCDP`: Cesantías, Crédito (primer y segundo pago), Hipoteca, PagosEmbargos, Reintegros |
| `SevenWSFactura` | `/Seven/Wsfactura/spofactu.asmx` | **InsertarFacturaP**, AnularFactura, InsertarPoFactu, AnularPoFactu, InsertarPoFactuP | `InsertarFacturaP`: Cesantías, Crédito, Hipoteca, PagosEmbargos, Reintegros |
| `SevenWSProveedor` | `/Seven/Wsproveedor/SIeWssec.asmx` | SyncGnTerce, SyncFaClien, SyncFaClienComplex, **SyncPoPvdor**, SyncFaConta, SyncEmpleadoPvdor, ChangeNit, ActModalidadCupo | `SyncPoPvdor`: Cesantías, Crédito, Hipoteca, PagosEmbargos, Reintegros |
| `SevenWSTercero` | `/Seven/Wstercero/wgnterce.asmx` | **InsertarGnTerce** | Cesantías, Crédito, Hipoteca, PagosEmbargos, Reintegros |
| `SevenWintipro` | `/Seven/Wintipro/WInTipro.asmx` | GetCuentaContable, GetCuentaContableTO | Sólo pass-through |
| `SevenWSAportes` | `/Seven/WSaportes/ValidarAportes.asmx` | GetAporte | Sólo pass-through |

* **Endpoints GA2 que exponen Seven tal cual** (`Endpoints/SevenEndpoints`, todos `POST api/credito/<Operación>`): InsertarPoFactuP, AnularPoFactu, SyncFaConta, ChangeNit, InsertarGnTerce, AnularCnMcont, InsertarPoFactu, SyncFaClien, InsertarMovContable, SyncFaClienComplex, InsertarDetCDP, InsertarCnMcont, RevertirCnMcont, AplicarCDP, SyncEmpleadoPvdor, ActModalidadCupo, GetCuentaContable, AnularCDP, InsertarCnDmcon, InsertarCDP, AplicarCnMcont, GetAporte, InsertarFacturaP, crearCDP, AnularFactura, GetCuentaContableTO.
* **Flujos de negocio que los disparan:** `api/credito/CrearTerceroSevenCredito` → `CrearProveedorSevenCredito` → `CrearCDPSevenCreditoPrimerPago`/`SegundoPago` → `CrearFacturaSevenPrimerPago`/`SegundoPago`; cierres diarios, mensuales y anuales de cartera (`CierreDiarioCarteraSeven`, `MovContableSevenCierreDiario`, …); `api/cesantias/*Seven`; `api/Reintegros/*Seven`; `api/cartera/GenerarComprobanteSevenPorProceso`; pagos de embargos.
* **Patrón de respuesta:** las operaciones devuelven en su mayoría un `string` `...Result` (código o mensaje del ERP). Los requests son objetos tipo DTO de Seven (p. ej. `InsertarMovContable(pContable: TSCnMcont)`).
* **Autenticación:** ninguna a nivel SOAP.
* **Incertidumbres:** la semántica de éxito/error de cada `...Result` hay que confirmarla con muestras reales (el código la interpreta en cada BL). Los `Reference.cs` apuntan a `cpvmp-34xp` y el config a `cpvmp-306x`; manda la configuración.

### E2. WorkManager / "DODO" (gestor documental y BPM)
* **Endpoint:** `SvcUtils:WorkManagerUrlSvc` = `http://cpvmp-118p/APIV2_0005/WorkManagerService.svc` (WCF, contrato `IWorkManagerServices`, `SOAPAction` = `http://tempuri.org/IWorkManagerServices/<Op>`).
* **Clientes:** `ReferenciasSvc/WorkManager/ConsumoWorkManager.cs` → `ServiciosSOA/WorkManager/WorkManagerService.cs` (métodos `*AsyncCH`).
* **Operaciones del contrato (29):** Form_Insert, Form_AttachFiles, Form_Update, Form_Delete, Form_GetData, Form_GetFileCodes, Form_GetBase64StringFile, Workflow_Start, Form_GetFilesDirectories, Workflow_AttachFiles, Workflow_RunTask, List_GetItems, ExternalUser_* (5), Form_GetParameters, File_Delete, Workflow_GetCases, Workflow_GetTasks, Field_GetValues, CC_* (6), Workflow_CreateLink.
* **Envueltas por `IConsumoWorkManager` (12):** Form_Insert, Form_Update, Form_Delete, Form_GetData, Form_GetFileCodes, Form_GetBase64StringFile, Form_AttachFiles, List_GetItems, Workflow_AttachFiles, Workflow_RunTask, Workflow_Start, Form_GetParameters.
* **Uso real:**
  * `Form_AttachFiles`: Crédito (≥6 puntos), Reportes (≥7), Solicitud (3), Entidades, vía `IWorkManagerService`.
  * `Form_Insert` y `Form_Update`: radicación de crédito (`GestorDocumentalBusinessLogic`, sólo en Release), solicitudes, reintegros, embargos, postulación.
  * `Form_GetParameters`: `GestorDocumentSolicitudes.cs:412`, que además construye un enlace al formulario web con `SvcUtils:DodoUrlSvc` (`http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?...`). Ese enlace **no es una llamada**; se devuelve al frontend.
  * Endpoints directos: `POST api/credito/insertForm|updateForm|attachFiles|getdataforms`, `api/solicitud/ObtenerFlujoDodo`, `api/solicitud/ObtenerEnlaceDodo`, `api/CargarDocumentosActividadCreditoDODO`, `/api/prospeccion/CargarDocumentosProspeccionDODO`.
  * **Manejadores de Service Bus** (E12): Insert, Update, Delete, AttachFiles, WorkflowAttachFiles → `IConsumoWorkManager`.
* **Contrato de datos clave:**
  * `InsertRequestDto {Header{User,Token}, FormCode, OfficeCode, OperationUser, CheckWorkflow, ProcessId?, nextStepUser, Data: DictionaryDto[]{Field,Key,Value}, Files: FileDto[]{Base64String, Description, DirectoryCode, DocumentTypeCode, Ext, GUID}}`
  * `InsertResponseDto {Success, Message, BarCode, RecordId, FileCodes[], workflowId}`. El `BarCode` se guarda como número de radicado.
  * `UpdateRequestDto {Header, FormCode, OperationUser, Data[], FilterParameters}`
  * `ResponseDto {Success, Message, Rows, Data[]}`
* **Autenticación:** a nivel de payload (`Header.User`/`Header.Token` salen de `CHKeys:User`/`CHKeys:Token`).
* **Toggle:** `DODO` (en solicitudes, reintegros y postulación; el flujo de crédito **no** lo consulta).

### E3. BUA SOAP (Base Única de Afiliados de Caja Honor)
Hay **dos clientes del mismo contrato** (`IBUA_API.ObtenerBUA`, `SOAPAction http://tempuri.org/IBUA_API/ObtenerBUA`):

| Cliente | URL efectiva | Consumidores activos |
|---|---|---|
| `ReferenciasSvc/Bua/ConsumoBua.cs` → `ServiciosSOA/Bua/BuaService.ObtenerAfiliadoBuaCH` | **`http://cpvmp-09x/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc` fija en `Reference.cs`**. `ConsumoBua.cs:38` usa `new BUA_APIClient()` sin parámetros, así que **ignora `SvcUtils:BuaUrlSvc` y el binding que construye** | `CarteraBusinessLogic.cs:4537`, `VerificacionDerechosBusinessLogic.cs:5345`, `CuentasConceptosBusinessLogic.cs:542`, `ClientsBusinessLogic.cs:1339` (restaurar integración de cliente) |
| `Connected Services/ServiceReference1/BuaService2BusinessLogic.cs` | `SvcUtils:BuaUrlServiceApi` = `.../Kriterion.ws.02/...` (respeta la configuración; timeouts de 5 min; maneja Fault/Timeout) | `ClientsBusinessLogic.cs:1375` (`ObtenerClienteIntegracionGA2New`), que se usa en la consulta de cliente por tipo y número cuando el toggle `Bua` ≠ `"false"` |

* **Request:** `BUAConsultaBE {identificacion: string, tipo_Id: int}`. El flujo reintenta con `tipo_Id` 1 → 4 → 5 cuando no encuentra al afiliado.
* **Response:** `BUA` con ~45 campos: datos personales (`primerNombre`, `primerApellido`, `identificacion`, `fechaNacimiento`, `sexo`, `estadoCivil`, …), `grado`, `direccion` (`ResidenciaBEL`), `conyuge`, `Dependiente[]`, `Cuentas[]`, `AportesCategoria[]`, `EmbargosAfiliado[]`, `Solicitudes[]`, `CausalesBloqueoAfiliado[]`, `Anotaciones[]`, `informacionFinanciera`, `estadoActualCli`, etc.
* **Host:** principalmente la API BUA (y la API GA2 a través de Cartera, Verificación de derechos y Cuentas).
* **Para el mock:** el de `ws.01` **no se puede redirigir por configuración**. Hay que cambiar el código (fuera del alcance de esta fase) o redirigir el DNS/hosts de `cpvmp-09x`.

### E4. Vigía — verificación de terceros (SARLAFT)
* **Activo:** `Connected Services/ServiceVigiaVerificacionTercero/VerificacionTerceroVigiaBusinessLogic.cs`. Contrato `ApplicationServicesPortType`, namespace `https://vigia.cajahonor.gov.co/WebSvc`, **estilo RPC/encoded**, operación `F_VERIFICATERCERO(param1, param2, param3)` (`SOAPAction .../SOAP_VerificaTercero.php/F_VERIFICATERCERO`). La URL sale de `SvcUtils:VigiaVerificarTerceroUrlSvc` (en GA2: `https://vigia.cajahonor.gov.co:443/WebSvc/SOAP_VerificaTercero.php`).
  * Parámetros: `param1 = Pe_Dato` (documento), `param2 = Pe_TipoVerificacion`, `param3 = Pe_Porcentaje` (cadena vacía por defecto). `Pe_Origen = AppMain:ApplicationId`.
  * Respuesta: `Resultado {Encontrado: string, Listas: string, Metodo: string}`.
  * Consumidores: `VigiaBusinessLogic.ServiceVertificacionTerceroAsync` (endpoint `POST api/credito/verificacionTercero`), `ProspeccionBusinessLogic.cs:10601`, `SolicitudBusinessLogic.cs:1326`.
  * Toggle: `VIGIA.ESTADO`. Con `false` responde lo que devuelva el SP `ConsultarResponseVigiaDesactivado`.
* **Registrados pero sin uso** (contratos ASMX `http://cpvmp-22fw:8080/WebSvc_asmx/*`): `F_CARGATERCERO`, `F_CARGAPRODUCTOTERCERO`, `F_CARGATRANSACCION` y `F_VERIFICATERCEROV3`. `VigiaBusinessLogic` expone `CargarTerceroAsync` y `CargaProductoTerceroAsync`, pero **nadie los llama**; `CargarTransanccionAsync` y `VertificacionTerceroAsync` lanzan `NotImplementedException`.
* **Ruta alterna nueva:** `TusDatosClient.ConsultarVigiaAsync` (E8) → `GET api/v1/vigia/customer/{id}/document-type/{type}/CheckRiskLAFT` en el API Gateway (endpoint `GET api/terceros/validacion/actualizacion/vigia`).

### E5. Fénix — validación biométrica
* **Endpoint:** `SvcUtils:FenixServiceSvc` = `http://cpvmp-09xc/WsFenixBiometria/WebServicesBiometrias.asmx?WSDL`. Ojo: la URL incluye `?WSDL` y se usa tal cual como dirección del endpoint.
* **Cliente:** `ReferenciasSvc/Fenix/ConsumoFenix.cs` → `ServiciosSOA/Fenix/FenixService` → `Credito/Fenix/FenixBusinessLogic`.
* **Operación:** `ValidarBiometria(identificacion: int)` (`SOAPAction http://tempuri.org/ValidarBiometria`). Existe también `xxValidarBiometria`, que no se usa.
* **Respuesta:** `ValidarBiometriaResult: ArrayOfXElement`, es decir **un DataSet .NET serializado** (esquema + `diffgram`). El código toma `Nodes[1]` y lo deserializa como `diffgram`. **El mock tiene que reproducir exactamente esa estructura.**
* **Entrada:** `GET api/credito/validarBiometria?request=<id>` y el flujo de prospección.
* **Toggle:** `FENIX.ESTADO`.

### E6. Haberes — Modelo Canónico de Datos (MCD)
* **Endpoint:** `SvcUtils:HaberesUrlSvc` = `http://cpvmp-92x/MCDIntegrationServices/wsModeloCanonico.asmx`. Contrato `wsModeloCanonicoDatosSoap`, `SOAPAction http://tempuri.org/<Op>`.
* **Operaciones del contrato:** CustomerAccount, DetalleMovimiento, DetalleBloqueos, ReporteHaberes, ExtractoGeneral, DeclaracionRenta, GenerarReporte, GenerarReporteV2.
* **Usadas:**
  * `GenerarReporte(sReporte, customerIdentification, accountId)` → `ServiceResponse {Status, Message, Report: byte[]}`. **Sólo en Release**; en DEBUG va por la pasarela E10.
  * `GenerarReporteV2(+ month, year)`.
  * `ReporteHaberes(customerIdentification, accountId, sUserQuery)` → `ArrayOfXElement` (DataSet).
* **Entradas:** `POST api/credito/generarReporteHaberes`, `generarReporteHaberesv2`, `ReporteHaberes`; `api/Reportes/ReporteCertificadoHaberes`. El PDF resultante se guarda en el contenedor Blob `credito`.

### E7. TransUnion DecisionCentre (IDVision, OTP, AQM/score, historial)
* **Endpoint:** **fijo en el código**, `ValidacionesTU.cs:16`: `https://www.transuniondecisioncentre.com.mx/TU.IDS.ExternalServices_latam_prod/SolutionExecution/ExternalSolutionExecution.svc` (producción). El `Reference.cs` trae por defecto la URL de UAT. Las claves de configuración `TransUnion`/`TrasnUnion` no se leen.
* **Cliente:** `GA2.Application/Main/TransUnion/ValidacionesTU.cs`, que usa `ExternalSolutionExecutionClient.ExecuteXMLString(string)` (`SOAPAction http://tempuri.org/IExternalSolutionExecution/ExecuteXMLString`). El cuerpo es un **`DCRequest` XML serializado como string**; `ConsumirTUAqm` le inyecta el namespace `http://transunion.com/dc/extsvc`. La respuesta `DCResponse` (XML string) se deserializa.
* **Credenciales:** `IdVision:{usuarioIDVision, claveIDVision, solucionId}` y `Aqm:{usuarioAqm, claveAqm, solucionIdAqm}` viajan dentro del XML (`Authentication {UserId, Password, Type="OnDemand"}`, `RequestInfo {SolutionSetId, ExecutionMode="NewWithContext"|"Send", ApplicationId, QueueName="DocVerify"}`).
* **Flujos activos:**
  * `GestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion` (`POST api/credito/ProspeccionCredito`). Si ya hay una consulta de menos de 30 días, **reutiliza el XML guardado en Blob `trans-union`** en vez de llamar. Si no, consulta y, cuando `TraerHistorialCrediticio = "Y"`, hace una segunda llamada `ExecutionMode = "Send"`.
  * `GestorIdVisionBusinessLogic`: `validarCliente`, `EnvioCodigoIDVision`, `EnvioCodigoOTP`, `reEnvioCodigoOTP`, `validacionCodigoOTPAsync` (OTP de identidad), `obtenerScoreCliente` y `obtenerHistorialCliente` (AQM). Endpoints: `api/prospeccion/ValidacionCodigoOtpIdVision`, `api/credito/ObtenerOtpCliente`, `ReenvioCodigoOtp`, `validacionotptemporal`, etc.
* **Campos de respuesta que lee el código:** `Status`, `Authentication.Status` (`Success`/`FirstTimeLogIn`), `ResponseInfo.ApplicationId`, `ContextData.Field[Key="DecisionHomologada"|"ApplicationData"|...]`, `Applicants.Applicant.CriteriaPostBureauCOLData.PostBureauReasons`.
* **Toggle:** **no existe**. `IdVision.estado` y `Aqm.estado` están en el config pero el código no los consulta.
* **Para el mock:** es el más delicado. La URL no se puede configurar y el contrato es "XML dentro de SOAP" con varios estados encadenados (autenticación → OTP → validación → score/historial). Hacen falta ejemplos reales de `DCRequest`/`DCResponse` para cada paso; las tablas de log `RegistroLogTUSimulador` y `VLL_VALIDACION_LOG_OTP` probablemente los tengan.

### E8. TusDatos (a través del API Gateway de Caja Honor)
* **Base:** `Integrationes:TusDatos:BaseUrl` = `https://apigateway.cajahonor.gov.co/kiosko-dev/`. Header `x-api-key`, timeout de 15 s.
* **Cliente:** `ReferenciasSvc/TusDatos/TusDatosClient.cs` (hereda de `CommonIntegracionCliente`), registrado en `GA2/Extensions/HttpClientExtensions.cs`.

| Método | Ruta efectiva | Respuesta |
|---|---|---|
| `ConsultarAsync` | `GET api/v1/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{true\|false}` | `DatosClienteResponse {JobId, IsFromHistory, HistoryDataCustomer{IsPEPS, DetectionRisk}}` |
| `ConsultarJobAsync` | `GET api/v1/tusdatos/jobid/{jobId}/customer/{identification}` | `DatosClienteJobResponse {StatusJobTusDatos, HistoryDataCustomer}` |
| `ConsultarVigiaAsync` | `GET api/v1/vigia/customer/{identification}/document-type/{documentType}/CheckRiskLAFT` | `VigiaResponse {Identification, FirstName, LastName, RiskLAFT, DocumentType}` |

* **Entradas (GA2):** `GET api/terceros/validacion/riesgos/tusdatos`, `.../tusdatos/polling`, `.../actualizacion/vigia` (con `[Authorize]`) y `TusDatosController` en `api/integrations/tusdatos/...` (**sin `[Authorize]`**).
* **Patrón asíncrono:** consulta → `JobId` → sondeo por job. El mock tiene que simular estados de job.
* **Toggle:** `TusDatos.ESTADO`.

### E9. Dodo REST (nueva API del gestor documental)
* **Base:** `Integrationes:Dodo:BaseUrl` = `https://dodo-docs-test:10081/`, timeout de 15 s. Clientes en `ReferenciasSvc/Dodo/*/Services/*.cs`. Autenticación: en `Forms` se agrega `Header{User,Token}` al body (`PostDodoAsync`); el resto de servicios no añade credenciales.
* **Operaciones (todas `POST`, salvo `version`):** `api/version` (GET), `api/Token/GetToken`, `api/User/User_Info`, `api/Offices/GetOffices`, `api/Notification/SendEmail`, `api/Field/Field_GetValues`, `api/Lists/List_GetItems`, `api/File/File_CopyToEP|File_CopyToFather|File_Delete`, `api/Forms/Form_GetParameters|Form_Insert|Form_Update|Form_Delete|Form_GetData|Form_AttachFiles|Form_GetFileCodes|Form_GetFilesDirectories|Form_GetBase64StringFile`, `api/Workflow/Workflow_Start|Workflow_RunTask|Workflow_ReturnTask|Workflow_AbortProcess|Workflow_AttachFiles|Workflow_GetCases|Workflow_GetTasks|Workflow_GetFileCodes|Workflow_StepsForReturn|Process_Parameters`.
* **Uso real:** sólo a través de `DodoController` (`api/integrations/dodo/...`, 31 rutas, **sin `[Authorize]`**), que reenvía 1:1. `ReportesBusinessLogic` inyecta `IDodoFormsService` pero **no lo usa**. `GestorDocumentalDodo` (middleware) no está registrado en DI.
* **Interpretación:** es la migración en curso de WorkManager SOAP (E2) a REST; el negocio todavía no la usa.

### E10. Pasarela REST de Caja Honor "ktrionApi"
* **Base:** `CHKeys:UrlCajaHonor` = `https://aplicacion.cajahonor.gov.co/REST/KriterionDev/ktrionApi/`.
* **Autenticación:** `POST {base}api/autenticacion` con `{AplicacionId, Contrasena, NombreAplicacion, Id(correlation), UserId}` tomados de `AppMain`. Devuelve `Response<TokenDto>`, y luego se envía `Authorization: Bearer <token>`. **Se pide un token nuevo en cada llamada.**
* **Rutas que el código conoce:** `api/obtenerafiliadobua`, `api/forminsert`, `api/formUpdate`, `api/formdelete`, `api/formgetdata`, `api/formgetfilecodes`, `api/formgetbase64stringfile`, `api/workflowattachfiles`, `api/listgetitems`, `api/generarreportehaberes`.
* **Uso real:**
  * Sólo en DEBUG: `forminsert` y `formUpdate` (radicación de crédito) y `generarreportehaberes`.
  * **Siempre:** `workflowattachfiles` (`GestorDocumentalBusinessLogic.cs:369`, adjuntar archivos al workflow de crédito), en Debug y en Release.
  * `obtenerafiliadobua` no se usa (`ClientsBusinessLogic` inyecta `IIntegracionesGa2BuaBusinessLogic` pero no lo llama). Las variantes Vigía y Fénix por pasarela no tienen consumidores.
* **Variante legada:** `ProviderRequestG2` (`CHKeys:LoginCajaHonor` + token cifrado MD5/AES con `PhraseKey`/`HashAplicacion` y `CHGUID`). Está registrada en BUA e Identity, pero **nadie la consume**.

### E11. SMTP
| Componente | Destino | Estado |
|---|---|---|
| `Infraestructure/Repositories/EmailNotificacion/EmailNotificacionRepository.cs` | Sección `SMTP` (`cpvmp-01v5:25`, sin SSL, remitente `notificaciones@…`). Plantillas en `CarpetaServidorStorage\template-email` | **Activo**: `CreditoBusinessLogic` (4 puntos), `ReportesBusinessLogic` (2) |
| `Login/LoginBusinessLogic.cs:355` (`RecuperaContrasena`) | `smtp-mail.outlook.com:587`, credenciales de `Email:*`, **sección que no existe en ningún appsettings** | Activo pero probablemente roto en estos entornos |
| `Parametrization/ParametrizacionBusinessLogic.cs:811`, `:842` | `smtp-mail.outlook.com` (sondeo TCP 465 + envío por 587) con **credenciales fijas en el código** | **Sin uso**: la única llamada (`SincronizacionCuentas`, ≈ línea 778) está comentada |
| `Integraciones/SmtpTestController.cs:25` | `cpvmp-01v5:25` fijo (`POST api/test/smtp/send`, sin `[Authorize]`) | Endpoint de diagnóstico |
| `Repositories/Email/EmailRepository.cs:45` | Usa como *claves* de configuración valores literales (`_configuration["smtp-mail.outlook.com"]`), así que siempre lee `null` | **Roto y sin uso** (inyectado, nunca invocado) |
| `DodoNotificationService` (E9) | `api/Notification/SendEmail` | Sólo pass-through |

* **Para el mock:** basta un servidor SMTP de captura (smtp4dev/MailHog/Papercut) y cambiar `SMTP:Host`. Los destinos Outlook están fijos en el código.

### E12. Azure Service Bus
* **Conexión:** `EventBusConnection` (namespace `cpvmpdsb1.servicebus.windows.net`, `EntityPath=chonorintegraciones`). **Topic** `chonorintegraciones` fijo (`EventBusServiceBus.cs:21`), **suscripción** `SubscriptionClientName = dodosubscription`.
* **Rol:** **sólo consumidor.** No hay ninguna llamada a `IEventBus.Publish` (la referencia en `WorkManagerService` está comentada). Los mensajes los publica otro sistema.
* **Defecto verificado:** los manejadores deserializan con `System.Text.Json` por defecto, que ignora campos públicos; el payload `Form_*Request.request` (campo) **siempre llega nulo**, así que este canal no puede transportar datos reales (detalle en `mocks/E12-azure-service-bus.md`).
* **Eventos** (filtro `CorrelationRuleFilter` por `Subject` = nombre del evento sin el sufijo `IntegrationEvent`):

| Subject | Payload | Manejador → acción |
|---|---|---|
| `WorkManagerInsertRequest` | `{ InsertRequest: Form_InsertRequest }` | `IConsumoWorkManager.Form_InsertAsync` |
| `WorkManagerUpdateRequest` | `{ UpdateRequest: Form_UpdateRequest }` | `Form_UpdateAsync` |
| `WorkManagerDeleteRequest` | `{ DeleteRequest: Form_DeleteRequest }` | `Form_DeleteAsync` |
| `WorkManagerWorkflowAttachFilesRequest` | `{ WorkflowAttachFilesRequest }` | `Workflow_AttachFilesAsync` |
| `WorkManagerAttachRequest` | `{ AttachRequest: Form_AttachFilesRequest }` | `Form_AttachFilesAsync` |

* **Efecto en el arranque:** al iniciar, la GA2 API borra la regla `$Default`, crea las reglas y arranca el `ServiceBusProcessor`. **Sin conectividad con Service Bus la API GA2 no arranca** (o arranca con errores, según el fallo). Es el bloqueo principal para levantar GA2 aislada.
* **Para el mock:** el emulador oficial de Azure Service Bus (contenedor), o una implementación en memoria de `IEventBus`. Ojo: el código usa `ServiceBusAdministrationClient`, que el emulador soporta sólo parcialmente; hay que verificarlo.

---

## 4. Integraciones configuradas o registradas sin uso real (legadas o deshabilitadas)

| Elemento | Evidencia |
|---|---|
| **OTP WsUtilidades** (`ServiceOtpSvc`, `ReferenciasSvc/ServicesOtp`) | `IConsumoServiceOtp` registrado; su único consumidor, `OtpService`, tiene el registro DI comentado. `GenerarOtpAsync` y `ValidarOtpAsync` no tienen llamadas. El OTP real va por TransUnion IDVision |
| **Vigía ASMX** `SOAP_CargaTercero`, `SOAP_CargaProductosTercero`, `SOAP_CargaTransaccion`, `SOAP_VerificaTercero` v3 | Registrados; los métodos de negocio que los envuelven no tienen llamadas o lanzan `NotImplementedException` |
| **Pasarela ktrionApi** para BUA, Vigía y Fénix (`IntegracionesGA2*`) | Interfaces de negocio sin consumidores (sólo inyección) |
| **`ProviderRequestG2`** (login cifrado CH + CHGUID) | Registrado en BUA e Identity, sin consumidores |
| **`GestorDocumentalDodo`** (middleware REST) | No registrado en DI; sin consumidores |
| **`IDodoFormsService` en Reportes** | Inyectado, nunca invocado |
| **`EmailRepository`** | Inyectado en Parametrización, nunca invocado; configuración rota |
| **`AzureStorageProvider` del `StorageFactory`** | Sólo se usa si `ProviderStorage = "Azure"`; `AzureStorageConfig` sin configurar |
| **`IdVision.estado` / `Aqm.estado`, `SvcUtils:TransUnion`** | Claves que el código no lee (todo está fijo en el código) |

---

## 5. Qué hace falta para construir los mocks

### 5.1 Puntos de redirección (sin tocar código)
| Integración | ¿Se redirige por config? | Clave |
|---|---|---|
| Seven, WorkManager, Vigía PHP, Fénix, Haberes, BUA ws.02 | **Sí** | `SvcUtils:*` |
| BUA ws.01 | **No** (fijo en `Reference.cs`) | Requiere hosts/DNS o un cambio de código |
| TransUnion | **No** (fijo en `ValidacionesTU.cs:16`, HTTPS) | Requiere un cambio de código, o DNS más un certificado de confianza |
| TusDatos, Dodo | **Sí** | `Integrationes:*:BaseUrl` |
| Pasarela ktrionApi | **Sí** | `CHKeys:UrlCajaHonor` |
| Service Bus | **Sí** | `EventBusConnection` → emulador |
| SMTP notificaciones | **Sí** | `SMTP:Host/Puerto` |
| SMTP Outlook (login, parametrización) | **No** (fijo en el código) | Requiere un cambio de código o una red aislada |

### 5.2 Insumos por integración
1. **WSDL/XSD:** se pueden regenerar desde los `Reference.cs` o descargar de los endpoints internos (`?wsdl`). Se recomienda obtener los WSDL originales, sobre todo de Seven y Vigía (RPC/encoded).
2. **Catálogo de operaciones y SOAPAction:** ver §3 y el Anexo A.
3. **Muestras reales de request/response.** Las tablas de auditoría de la BD son la mejor fuente: `EjecutarServicio` → auditoría SOAP; `InsertReportIntegracionesRest`; `RegistroLogTUSimulador`; `VLL_VALIDACION_LOG_OTP`; logs de Serilog.
4. **Escenarios mínimos por mock:**
   * **Seven:** éxito y error de negocio en `InsertarGnTerce`, `SyncPoPvdor`, `CrearCDP`, `InsertarFacturaP`, `InsertarMovContable` y `AnularCnMcont`.
   * **WorkManager:** `Form_Insert` con `BarCode`, `Form_Update`, `Form_AttachFiles` (con `FileCodes`), `Form_GetParameters`, `Workflow_AttachFiles`, y fallo con `Success = false`.
   * **BUA:** afiliado encontrado (con cuentas, embargos y beneficiarios), no encontrado (`null`) y la búsqueda por `tipo_Id` 1/4/5.
   * **Vigía:** `Encontrado = "S"/"N"` con `Listas`.
   * **Fénix:** DataSet válido e inválido.
   * **Haberes:** PDF en `Report` y `Status = false`.
   * **TransUnion:** autenticación OK / `FirstTimeLogIn`, envío y validación de OTP, decisión homologada (aprobado/rechazado), score, historial y error.
   * **TusDatos:** job pendiente → completado, PEP / sin riesgo, `404 → null`.
   * **Service Bus:** un mensaje por cada `Subject`.
5. **Datos semilla de BD:** `ObtenerParametrosIntegraciones` (VIGIA, FENIX, DODO, Bua, TusDatos, Dodo), `ConsultarResponseVigiaDesactivado`.

### 5.3 Incertidumbres abiertas (a confirmar con el equipo)
1. ¿Qué sistema publica en el topic `chonorintegraciones` y con qué frecuencia? (no está en el repo).
2. ¿La radicación de crédito en producción usa WorkManager SOAP directo (Release) y el resto de entornos la pasarela (DEBUG)? Hay que confirmar qué configuración de compilación se despliega en cada entorno.
3. ¿Dodo REST (E9) reemplazará a WorkManager SOAP (E2)? Hoy sólo se expone como pass-through.
4. Semántica de los `...Result` de Seven (códigos de éxito y error).
5. Valores reales de `Pe_TipoVerificacion` para Vigía y la estructura de `Listas`.
6. ¿Fénix y TransUnion en producción apuntan a los mismos hosts que en desarrollo? (TransUnion apunta a **producción** fija en el código, incluso desde desarrollo).

---

## 6. Observaciones de seguridad y riesgo encontradas en el análisis
> No forman parte del alcance de mocks, pero afectan a cualquier entorno de pruebas y conviene escalarlas.

1. **Secretos en el repositorio:** la SAS de Service Bus, la API key de TusDatos, credenciales de IDVision/AQM, `AppMain:Password` y `CHKeys:Token` en `src/GA2.Apis/*/appsettings.json` y `appsettings.Development.json`. Además, **credenciales SMTP fijas en el código** en `ParametrizacionBusinessLogic.cs:844` (y la misma cadena en `EmailRepository.cs:50`). Se recomienda rotarlas y pasarlas a un almacén de secretos.
2. **Validación TLS desactivada** en las llamadas de la pasarela ktrionApi (`ProviderRequestFactory.cs:292`, `:311`).
3. **Endpoints sin `[Authorize]`** y sin política de autorización global (no hay `FallbackPolicy`): `DodoController` (`api/integrations/dodo/*`), `TusDatosController` y `SmtpTestController` (envío de correo). Quedan expuestos como proxies anónimos hacia sistemas internos.
4. **Timeouts infinitos** (`TimeSpan.MaxValue`) en los clientes SOAP: un mock lento o caído puede bloquear hilos indefinidamente.
5. **Llamadas a TransUnion de producción fijas en el código**, incluso desde entornos de desarrollo (posible coste o uso indebido de datos reales).

---

## Anexo A — SOAPAction por contrato (extraído de los `Reference.cs`)

| Contrato | Namespace / prefijo de Action | Operaciones |
|---|---|---|
| Seven `SCNMCONTSoap` (×2 proxies) | `http://seven/` | InsertarCnMcont, InsertarCnDmcon, AplicarCnMcont, RevertirCnMcont, AnularCnMcont, InsertarMovContable, SetContabToper |
| Seven `SPGOBLIGSoap` | `http://seven/` | InsertarCDP, InsertarDetCDP, AplicarCDP, AnularCDP, CrearCDP |
| Seven `SPOFACTUSoap` | `http://seven/` | InsertarFacturaP, AnularFactura, InsertarPoFactu, AnularPoFactu, InsertarPoFactuP |
| Seven `SIEWSSECSoap` | `http://seven/` | SyncGnTerce, SyncFaClien, SyncFaClienComplex, SyncPoPvdor, SyncFaConta, SyncEmpleadoPvdor, ChangeNit, ActModalidadCupo |
| Seven `SGNTERCESoap` | `http://seven/` | InsertarGnTerce |
| Seven `SINTIPROSoap` | `http://seven/` | GetCuentaContable, GetCuentaContableTO |
| Seven `ValidarAportesSoap` | `http://seven/` | GetAporte |
| `IWorkManagerServices` | `http://tempuri.org/IWorkManagerServices/` | 29 operaciones (ver E2) |
| `IBUA_API` (×2 proxies) | `http://tempuri.org/IBUA_API/` | ObtenerBUA |
| Vigía `ApplicationServicesPortType` (PHP, RPC/encoded) | `https://vigia.cajahonor.gov.co/WebSvc/SOAP_VerificaTercero.php/` | F_VERIFICATERCERO |
| Vigía ASMX (sin uso) | `http://tempuri.org/` | F_CARGATERCERO, F_CARGAPRODUCTOTERCERO, F_CARGATRANSACCION, F_VERIFICATERCEROV3 |
| Fénix `WebServicesBiometriasSoap` | `http://tempuri.org/` | ValidarBiometria, xxValidarBiometria |
| Haberes `wsModeloCanonicoDatosSoap` | `http://tempuri.org/` | CustomerAccount, DetalleMovimiento, DetalleBloqueos, ReporteHaberes, ExtractoGeneral, DeclaracionRenta, GenerarReporte, GenerarReporteV2 |
| OTP `IServicesOTP` (sin uso) | `http://tempuri.org/IServicesOTP/` | DoWork, CrearOTP, ValidaOTP, CrearOTPEmail, ValidaOTPEmail |
| TransUnion `IExternalSolutionExecution` | `http://tempuri.org/IExternalSolutionExecution/` | 33 operaciones; **se usa sólo `ExecuteXMLString`** |

## Anexo B — Archivos clave

| Tema | Ruta |
|---|---|
| Registro DI de integraciones (GA2) | `src/GA2.Apis/GA2/Extensions/DIManager.cs`, `HttpClientExtensions.cs`, `EventBusExtensions.cs`, `GlobalAppSettinsExtension.cs` |
| Registro DI (BUA / Identity) | `src/GA2.Apis/{BUA,Identity}/Extensions/DIManager.cs` |
| Opciones de configuración | `src/GA2.Transversals/Commons/Configuraciones/ConfiguracionesGlobales.cs`, `ConfiguracionesServiciosSoa.cs` |
| Clientes SOAP | `src/GA2.Domain/Core/ReferenciasSvc/**`, `src/GA2.Domain/Core/Connected Services/**`, `src/GA2.Application/Main/TransUnionService/Reference.cs` |
| Fachadas SOA | `src/GA2.Domain/Core/ServiciosSOA/**` |
| Pasarela ktrionApi | `src/GA2.Application/Main/ProviderRequest/IntegracionesGA2/**`, `src/GA2.Domain/Core/IntegracionesGA2/**` |
| Clientes REST nuevos | `src/GA2.Domain/Core/ReferenciasSvc/{Common,TusDatos,Dodo,IntegracionesConfig}/**` |
| Event Bus | `src/GA2.BuildingBlocks/**`, `src/GA2.Domain/Core/IntegrationEvents/**` |
| Toggles | `src/GA2.Infraestructure/Repositories/IntegracionesConfig/EstadoIntegracionRepository.cs`, `CreditoRepository.ObtenerParametrosIntegraciones` |
| TransUnion | `src/GA2.Application/Main/TransUnion/ValidacionesTU.cs`, `src/GA2.Domain/Core/Credito/TransUnion/*` |
| Correo | `src/GA2.Infraestructure/Repositories/EmailNotificacion/EmailNotificacionRepository.cs` |
