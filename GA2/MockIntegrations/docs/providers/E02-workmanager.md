# E2 · WorkManager / DODO — mock SOAP (WCF)

**Especificación de request/response:** [docs/integraciones/mocks/E02-workmanager-dodo-soap.md](../../../docs/integraciones/mocks/E02-workmanager-dodo-soap.md).
**Código:** [`Providers/WorkManager/WorkManagerMock.cs`](../../src/MockIntegrations.Api/Providers/WorkManager/WorkManagerMock.cs) · [`Fixtures/`](../../src/MockIntegrations.Api/Providers/WorkManager/Fixtures/).

## Endpoint y configuración

| Clave | Ruta en el mock | Contrato |
|---|---|---|
| `SvcUtils:WorkManagerUrlSvc` (GA2 y BUA) | `POST /APIV2_0005/WorkManagerService.svc` | `IWorkManagerServices` (`SOAPAction: "http://tempuri.org/IWorkManagerServices/<Op>"`) |

`SvcUtils:DodoUrlSvc` **no es una llamada**: GA2 lo usa para armar el enlace web que devuelve al frontend.

## Operaciones

| Operación | Uso en GA2 | Comportamiento del mock |
|---|---|---|
| `Form_Insert` | Radicación (solicitudes, reintegros, embargos, postulación; crédito sólo en Release) | `BarCode = "{FormCode}-{consecutivo:000000}"` (desde 000123), `RecordId` desde 4521, `workflowId` desde 9981 |
| `Form_Update` | Actualización de radicados | `Success=true`, `Rows=1` |
| `Form_AttachFiles` | Adjuntos (≥18 puntos) | `Success=true`, `FileCodes=["FC-000001"…]` |
| `Workflow_AttachFiles` | Adjuntos al workflow | Igual; devuelve el `WorkflowId` recibido |
| `Form_GetParameters` | Enlace DODO (`ObtenerEnlaceDodo`) | `Form.IdFormulario=83`, `Form.NombreCampoSticker="RADICADO"`, `Form.Codigo={FormCode}` |
| `Form_GetData` | Pass-through `api/credito/getdataforms` | Datos de ejemplo validados |
| `Form_Delete` | Manejador de Service Bus | `Success=true` |
| `Form_GetFileCodes`, `Form_GetBase64StringFile`, `Workflow_Start`, `Workflow_RunTask`, `List_GetItems` | Envueltas sin consumidores | Respuesta validada (fixture) |
| 17 operaciones del contrato sin cliente en GA2 (`ExternalUser_*`, `CC_*`, `Workflow_GetCases`…) | Sin uso | **SOAP Fault explícito** |

El `Header {User, Token}` viaja dentro del DTO; el mock acepta cualquier valor. Los DTO respetan el **orden alfabético** de `DataContractSerializer` (un elemento fuera de orden se ignora sin error).

## Escenarios

| Escenario | Respuesta | Efecto en GA2 |
|---|---|---|
| `fallido` | `Success=false` con `Message` | "El formulario se ha encolado para su procesamiento" |
| `resultado-nulo` | `<{Op}Result i:nil="true"/>` | `NullReferenceException` en `WorkManagerService` → 409 |
| `sin-formulario` (Form_GetParameters) | `Form` nulo | `NullReferenceException` en `ObtenerEnlaceDodo` |
| `fault` / `http-500` / `slow` | Técnicos | `FaultException` → 409 |

Regla de configuración incluida: `FormCode = "999"` → `Form_Insert` en `fallido`.

## Certeza y limitaciones

- Estructura validada con el proxy `ReferenciasSvc/WorkManager/Reference.cs` (pruebas `SoapProvidersCompatibilityTests`).
- Formato real del consecutivo del `BarCode` no confirmado (GA2 sólo exige el prefijo `{FormCode}-`).
- Con GA2 en **DEBUG**, la radicación de crédito no usa este servicio sino la pasarela [E10](E10-ktrionapi.md).
