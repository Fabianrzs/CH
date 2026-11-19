# E6 · Haberes (Modelo Canónico de Datos) — mock SOAP ASMX

**Especificación:** [docs/integraciones/mocks/E06-haberes-mcd.md](../../../docs/integraciones/mocks/E06-haberes-mcd.md).
**Código:** [`Providers/Haberes/HaberesMock.cs`](../../src/MockIntegrations.Api/Providers/Haberes/HaberesMock.cs).

| Clave | Ruta en el mock |
|---|---|
| `SvcUtils:HaberesUrlSvc` | `POST /MCDIntegrationServices/wsModeloCanonico.asmx` (`SOAPAction: "http://tempuri.org/<Op>"`) |

| Operación | Respuesta del mock |
|---|---|
| `GenerarReporte`, `GenerarReporteV2` | `ServiceResponse {Message, Report, Status=true}`; `Report` es un **PDF 1.4 válido** de una página (documento de prueba) |
| `ReporteHaberes` | DataSet `REPORTE_HABERES` **minificado** (GA2 lee `Nodes[1].FirstNode` y falla si hay espacios); `CLI_IDENTIFICACION` = el documento recibido |
| `CustomerAccount`, `DetalleMovimiento`, `DetalleBloqueos`, `ExtractoGeneral`, `DeclaracionRenta` | Sin uso en GA2: SOAP Fault explícito |

| Escenario | Respuesta | Regla |
|---|---|---|
| `sin-reporte` (GenerarReporte/V2) | `Status=false` sin `Report` (GA2 deja el Base64 nulo) | `customerIdentification = 1000000404` |
| `vacio` (ReporteHaberes) | Sólo `xs:schema` (`Nodes.Count = 1`): GA2 no arma el reporte | `customerIdentification = 1000000404` |

**Importante:** con GA2 compilado en **DEBUG**, `GenerarReporte` no llama a este servicio, sino a la pasarela [E10](E10-ktrionapi.md) (`api/generarreportehaberes`), que también está simulada y devuelve el mismo PDF.
