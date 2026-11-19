# E7 · TransUnion DecisionCentre — mock SOAP con estado

**Especificación:** [docs/integraciones/mocks/E07-transunion-decisioncentre.md](../../../docs/integraciones/mocks/E07-transunion-decisioncentre.md).
**Código:** [`Providers/TransUnion/TransUnionMock.cs`](../../src/MockIntegrations.Api/Providers/TransUnion/TransUnionMock.cs).

## Endpoint

`POST /TU.IDS.ExternalServices_latam_prod/SolutionExecution/ExternalSolutionExecution.svc` (y la variante `_uat`). Operación única `ExecuteXMLString`: el request lleva un `DCRequest` **como texto** y la respuesta un `DCResponse` **como texto** (ambos escapados dentro del sobre SOAP).

> **No configurable en GA2:** la URL de producción está fija en `GA2.Application/Main/TransUnion/ValidacionesTU.cs:16` (HTTPS). Ver [guía de configuración §6](../guia-configuracion-ga2.md#6-integraciones-que-no-se-pueden-redirigir-por-configuración).

## Despacho por `RequestInfo` y estado por `ApplicationId`

| Paso | Cómo lo reconoce el mock | Respuesta |
|---|---|---|
| 1. Inicio IDVision | `ExecutionMode=NewWithContext`, `SolutionSetId=142` | Nuevo `ApplicationId` (desde 9876543), cola `PhoneSelection`, `Applicants` con `DocumentState=VIGENTE` |
| 2. Envío de OTP | `Send` + `QueueName=PhoneSelection` | `OTPCode=SENT`, `Delay=60`; `ApplicationId` desconocido → `ErrorInfo "Application does not exists in Queue PhoneSelection."` |
| 3. Reenvío | `Send` + `PinVerification_OTPInput` + `Action=ResendOTP` | `Status=Success` |
| 4. Validación del PIN | `Send` + `PinVerification_OTPInput` + `PinNumber` | `OTPReasons/Reason/Code = 300` si el PIN es `MockIntegrations:TransUnion:ValidPin` (por defecto `123456`); si no, `302` |
| 5. AQM / prospección | `NewWithContext` + otro `SolutionSetId` (360) | `DecisionHomologada=VIABLE`, `Score=750`, `CapacidadEndeudamiento=1500000.50`, `PorcEndFinal=35.5` (punto decimal, en-US) |
| 6. Historial | `Send` + `QueueName=DocVerify` | `Applicants` con `DSOnlineBureauCOLData` y `DecisionHomologada` |

Acepta los `Field` con CDATA (`ConsumirTU`) y con XML escapado (`ConsumirTUAqm`). Las pruebas ejecutan **el cliente real de GA2** (`ValidacionesTU`) y sus deserializadores (`Serialization.DeserializeDC/RC/Apps`).

## Escenarios

`auth-fallida`, `muerte-titular`, `fuera-de-cola`, `sin-otp`, `pin-incorrecto`, `pendiente`, `no-viable`, `sin-applicationdata` y los integrados. Las reglas por documento (`IdNumber`) se heredan en los pasos siguientes de la misma transacción:

| `IdNumber` | Escenario |
|---|---|
| 1000000666 | `no-viable` |
| 1000000777 | `pendiente` |
| 1000000999 | `muerte-titular` |
| 1000000500 | `fault` |

## Limitaciones

- Contenido validado contra los deserializadores de GA2, pero **no contra muestras reales** de TransUnion: deben confirmarse con las tablas `RegistroLogTUSimulador` / `VLL_VALIDACION_LOG_OTP`.
- Las 32 operaciones restantes del contrato no se usan en GA2 y responden SOAP Fault.
