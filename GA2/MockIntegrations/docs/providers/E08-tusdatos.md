# E8 · TusDatos (API Gateway de Caja Honor) — mock REST

**Especificación:** [docs/integraciones/mocks/E08-tusdatos.md](../../../docs/integraciones/mocks/E08-tusdatos.md).
**Código:** [`Providers/TusDatos/TusDatosMock.cs`](../../src/MockIntegrations.Api/Providers/TusDatos/TusDatosMock.cs) · modelos copiados de GA2 en [`Providers/TusDatos/Models/`](../../src/MockIntegrations.Api/Providers/TusDatos/Models/).

| Operación | Ruta en el mock (`Integrationes:TusDatos:BaseUrl = http://<mock>:5080/kiosko-dev/`) | Respuesta |
|---|---|---|
| ConsultarCliente | `GET /kiosko-dev/api/v1/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{checkHistory}` | `DatosClienteResponse {jobId, isFromHistory, historyDataCustomer}` |
| ConsultarJob | `GET /kiosko-dev/api/v1/tusdatos/jobid/{jobId}/customer/{identification}` | `DatosClienteJobResponse {statusJobTusDatos, historyDataCustomer}` |
| ConsultarVigia | `GET /kiosko-dev/api/v1/vigia/customer/{identification}/document-type/{documentType}/CheckRiskLAFT` | `VigiaResponse {identification, firstName, lastName, riskLAFT, documentType}` |

- **Autenticación:** header `x-api-key` obligatorio (cualquier valor, o el de `MockIntegrations:TusDatos:ApiKey`). Sin él → **401** con cuerpo de texto (GA2: `IsSuccess=false` con ese texto).
- **Job asíncrono con estado:** la consulta crea `mock-job-NNNNNN`; los primeros `PollsUntilFinished` sondeos (por defecto 1) devuelven `PendingStatus` (`pendiente`) y después `FinishedStatus` (`finalizado`) con el resultado. Job desconocido → 404.
- JSON camelCase (GA2 deserializa sin distinguir mayúsculas).

| Escenario | Respuesta | Regla (`identification`) |
|---|---|---|
| `default` | Job nuevo, sin riesgo | — |
| `pep` | Job que termina con `isPEPS=true`, `detectionRisk=ALTO` | 1000000666 (ConsultarCliente) |
| `historial` | `isFromHistory=true`, `jobId=null` | 1000000100 |
| `riesgo` (Vigía) | `riskLAFT="RIESGO ALTO"` | 1000000666 (ConsultarVigia) |
| `no-encontrado` | **404** → GA2: éxito con `Data=null` | 1000000404 (todas) |
| `unauthorized` | 401 | — |

**No confirmado con el gateway:** el dominio de `statusJobTusDatos` y de `riskLAFT` (configurables). Probado de extremo a extremo a través de GA2 (`api/integrations/tusdatos/*`).
