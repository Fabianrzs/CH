# E1 · Seven ERP — mock SOAP (7 servicios ASMX)

**Especificación de request/response por operación:** [docs/integraciones/mocks/E01-seven-erp.md](../../../docs/integraciones/mocks/E01-seven-erp.md) (sobres capturados con los proxies WCF de GA2; respuestas validadas por ida y vuelta).
**Código del mock:** [`Providers/Seven/SevenMock.cs`](../../src/MockIntegrations.Api/Providers/Seven/SevenMock.cs) · fixtures en [`Providers/Seven/Fixtures/`](../../src/MockIntegrations.Api/Providers/Seven/Fixtures/).

## Endpoints y configuración en GA2

| Clave `SvcUtils` (GA2 y BUA) | Ruta en el mock | Contrato | Operaciones |
|---|---|---|---|
| `SevenWSMovContable` | `POST /Seven/WsMovContable/scnmcont.asmx` | `SCNMCONTSoap` | InsertarCnMcont, InsertarCnDmcon, AplicarCnMcont, RevertirCnMcont, AnularCnMcont, **InsertarMovContable**, SetContabToper |
| `SevenWSCdp` | `POST /Seven/WsCdp/WPgOblig.asmx` | `SPGOBLIGSoap` | InsertarCDP, InsertarDetCDP, AplicarCDP, AnularCDP, **CrearCDP** |
| `SevenWSFactura` | `POST /Seven/Wsfactura/spofactu.asmx` | `SPOFACTUSoap` | **InsertarFacturaP**, AnularFactura, InsertarPoFactu, AnularPoFactu, InsertarPoFactuP |
| `SevenWSProveedor` | `POST /Seven/Wsproveedor/SIeWssec.asmx` | `SIEWSSECSoap` | SyncGnTerce, SyncFaClien, SyncFaClienComplex, **SyncPoPvdor**, SyncFaConta, SyncEmpleadoPvdor, ChangeNit, ActModalidadCupo |
| `SevenWSTercero` | `POST /Seven/Wstercero/wgnterce.asmx` | `SGNTERCESoap` | **InsertarGnTerce** |
| `SevenWintipro` | `POST /Seven/Wintipro/WInTipro.asmx` | `SINTIPROSoap` | GetCuentaContable, GetCuentaContableTO |
| `SevenWSAportes` | `POST /Seven/WSaportes/ValidarAportes.asmx` | `ValidarAportesSoap` | GetAporte |

En negrita, las 6 operaciones con lógica de negocio en GA2; las 23 restantes se exponen como *pass-through* (`POST api/credito/<Operación>`). Transporte: SOAP 1.1, `Content-Type: text/xml; charset=utf-8`, `SOAPAction: "http://seven/<Operación>"`, sin autenticación.

## Cómo responde el mock

- Cada `...Result` de negocio es un **XML escapado** con `RETORNO` (0 = éxito, 1 = error) y `TXTERROR`. GA2 lo carga con `XmlDocument.LoadXml`.
- **Valores dinámicos** (contadores por proceso, reiniciables con `POST /_mock/reset`): `MCO_CONT` desde 551200, `MCO_NUME` desde 3301, `MPR_CONT` desde 45871, `FAC_CONT` desde 77812, `FAC_NUME` desde 1203, `PVD_CODD` desde 900123. Dos llamadas seguidas devuelven consecutivos distintos, como el ERP.
- `InsertarGnTerce` responde con raíz `GN_TERCE` (GA2 usa la ruta `//GN_TERCE/RETORNO`).

## Escenarios

| Escenario | Operaciones | Respuesta | Efecto en GA2 |
|---|---|---|---|
| `default` | todas | `RETORNO=0` con consecutivos | Continúa el flujo de pago |
| `error-negocio` | todas las que devuelven `RETORNO` | `RETORNO=1`, `TXTERROR` descriptivo, sin consecutivos | Corta el flujo y muestra `TXTERROR` |
| `tercero-existente` | InsertarGnTerce | `RETORNO=1` "El tercero ya se encuentra registrado" | El flujo continúa |
| `sin-identificadores` | InsertarFacturaP | `RETORNO=0` sin `FAC_CONT`/`FAC_NUME` | Reproduce el `NullReferenceException` de GA2 |
| `fault` / `http-500` / `slow` | todas | SOAP Fault 500 / HTML 500 / retardo | `FaultException` / `CommunicationException` → HTTP 409 en GA2 |

## Certeza y limitaciones

- **Estructura validada** con los proxies de GA2 (`ReferenciasSvc/Seven/*/Reference.cs` y `Connected Services/ServiceSeven`), pruebas `SevenCompatibilityTests`.
- **No confirmado con Seven:** el nombre del elemento raíz de cada `...Result` (salvo `GN_TERCE`; GA2 no lo valida) y el contenido de `GetCuentaContable`, `GetCuentaContableTO` y `GetAporte` (texto de ejemplo; GA2 sólo los reenvía).
- Probado de extremo a extremo a través de GA2: `POST api/credito/InsertarGnTerce` (prueba de humo).
