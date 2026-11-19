# E5 · Fénix (biometría) — mock SOAP ASMX con DataSet

**Especificación:** [docs/integraciones/mocks/E05-fenix-biometria.md](../../../docs/integraciones/mocks/E05-fenix-biometria.md).
**Código:** [`Providers/Fenix/FenixMock.cs`](../../src/MockIntegrations.Api/Providers/Fenix/FenixMock.cs).

| Clave | Ruta en el mock | Operación |
|---|---|---|
| `SvcUtils:FenixServiceSvc` | `POST /WsFenixBiometria/WebServicesBiometrias.asmx` (acepta el `?WSDL` que GA2 deja en la URL) | `ValidarBiometria(identificacion: xs:int)` |

La respuesta es un **DataSet .NET** (`xs:schema` + `diffgr:diffgram`). GA2 toma `Nodes[1]` y lo deserializa como `diffgram`; las pruebas ejecutan esa misma lectura con el DTO de GA2. `A001NUM_IDENTIFICACION` = la identificación recibida.

| Escenario | Respuesta | Efecto en GA2 | Regla |
|---|---|---|---|
| `default` | `A008CEDULA/ROSTRO/HUELLA/FIRMA = 1` | "Biometria verificada" | — |
| `parcial` | `A008HUELLA = 0` | Éxito; el frontend interpreta | `identificacion = 1000000002` |
| `sin-datos` | diffgram sin `NewDataSet` | "No hay información en Fénix…" + notificación `ASERA_T` en BD | `identificacion = 1000000404` |
| `dato-invalido` | `A008HUELLA = X` | "Usuario sin datos en Fenix" | — |
| `fault` | SOAP Fault | Sin captura → HTTP 409 | `identificacion = 1000000500` |

`xxValidarBiometria` (sin uso) responde SOAP Fault. Probado de extremo a extremo a través de GA2: `GET api/credito/validarBiometria`.
