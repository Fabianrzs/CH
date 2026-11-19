# E8 — TusDatos (vía API Gateway de Caja Honor `kiosko-dev`) — REST/JSON

> Documento para implementar un **mock compatible** con el API Gateway que expone TusDatos (validación de identidad, listas PEP y riesgo LA/FT). GA2 lo consume con `TusDatosClient` (`ReferenciasSvc/TusDatos/TusDatosClient.cs`), que hereda de `CommonIntegracionCliente`.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | REST, JSON, `GET` |
| Base URL | `Integrationes:TusDatos:BaseUrl` = `https://apigateway.cajahonor.gov.co/kiosko-dev/` (incluye la `/` final; las rutas son relativas) |
| Prefijo | `{Version}/{Controller}` = `api/v1/tusdatos` (`Integrationes:TusDatos:Version` y `:Controller`) |
| Autenticación | Header **`x-api-key: {Integrationes:TusDatos:ApiKey}`** en todas las llamadas |
| Otros headers | `Accept: application/json` |
| Timeout | `Integrationes:TusDatos:TimeoutSeconds` = **15 s** (`HttpClient.Timeout`) |
| Toggle en BD | `TusDatos` / `ESTADO` (`EstadoIntegracionRepository.IntegracionActiva`, sin distinguir mayúsculas). **Si está inactivo, `ValidateIntegrationStatus` lanza `InvalidOperationException` fuera del `try`**: la excepción llega al `ErrorValidationMiddleware` y el frontend recibe **HTTP 409** |
| Auditoría | Cada llamada se registra con `InsertReportIntegracionesRest` (endpoint, verbo, status, response, duración) |
| Deserialización | `System.Text.Json`, `PropertyNameCaseInsensitive = true` |
| Host | API GA2 (la API BUA lo registra, pero no tiene la sección de configuración ni consumidores) |

## 2. Manejo de códigos HTTP (común a las tres operaciones)

| Respuesta del mock | Resultado en GA2 (`Response<T>`) |
|---|---|
| `200`–`299` con JSON | `IsSuccess=true`, `ReturnMessage="Consulta exitosa"`, `Data` = JSON deserializado |
| `404` | `IsSuccess=true`, `ReturnMessage="Resource not found"`, `Data=null` (**no se trata como error**) |
| Cualquier otro código (400, 401, 403, 500…) | `IsSuccess=false`, `ReturnMessage` = **cuerpo crudo** de la respuesta, `Data=null` |
| Timeout (> 15 s), DNS, TLS o JSON inválido | `IsSuccess=false`, `ReturnMessage` = mensaje de la excepción |

Los endpoints de GA2 devuelven ese `Response<T>` tal cual (HTTP 200) al frontend.

## 3. Operaciones

### 3.1 `ConsultarAsync` — consulta de cliente (inicia un job)

| Propiedad | Valor |
|---|---|
| Método y ruta | `GET {BaseUrl}api/v1/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{checkHistory}` |
| Plantilla en config | `Integrationes:TusDatos:Endpoints:ConsultarCliente` |
| Parámetros de ruta | `identification` (string), `documentType` (string, p. ej. `CC`), `checkHistory` (`true`/`false`, **en minúsculas**) |
| Body | ninguno |
| Entradas en GA2 | `GET api/terceros/validacion/riesgos/tusdatos` (con `[Authorize]`) y `GET api/integrations/tusdatos/customer/{identification}/document-type/{documentType}?checkHistory=` (**sin `[Authorize]`**) |

Ejemplo de request:

```http
GET /kiosko-dev/api/v1/tusdatos/customer/1020304050/document-type/CC/check-history/true HTTP/1.1
Host: apigateway.cajahonor.gov.co
Accept: application/json
x-api-key: <ApiKey>
```

Response `200` (`DatosClienteResponse`):

| Propiedad | Tipo | Significado |
|---|---|---|
| `jobId` | string, nulable | Identificador del job asíncrono para sondear con §3.2 |
| `isFromHistory` | boolean | `true` si el resultado sale del histórico y no hace falta sondear |
| `historyDataCustomer.isPEPS` | boolean | Persona expuesta políticamente |
| `historyDataCustomer.detectionRisk` | string, nulable | Nivel de riesgo detectado |

```json
{
  "jobId": "6f1c2a9e-5b7d-4c11-9d1e-0a3b2c4d5e6f",
  "isFromHistory": false,
  "historyDataCustomer": null
}
```

Variante con histórico:

```json
{
  "jobId": null,
  "isFromHistory": true,
  "historyDataCustomer": { "isPEPS": false, "detectionRisk": "BAJO" }
}
```

### 3.2 `ConsultarJobAsync` — sondeo del job

| Propiedad | Valor |
|---|---|
| Método y ruta | `GET {BaseUrl}api/v1/tusdatos/jobid/{jobId}/customer/{identification}` |
| Plantilla | `Integrationes:TusDatos:Endpoints:ConsultarJob` |
| Entradas en GA2 | `GET api/terceros/validacion/riesgos/tusdatos/polling` (`[Authorize]`) y `GET api/integrations/tusdatos/job/{jobId}/customer/{identification}` |

Response `200` (`DatosClienteJobResponse`):

```json
{
  "statusJobTusDatos": "finalizado",
  "historyDataCustomer": { "isPEPS": false, "detectionRisk": "BAJO" }
}
```

> El dominio de `statusJobTusDatos` **no está en el código** (GA2 lo pasa al frontend sin interpretarlo). Hay que confirmarlo con el gateway (p. ej. `pendiente`/`procesando`/`finalizado`/`error`). El mock debería pasar de un estado "pendiente" a "finalizado" tras N sondeos.

### 3.3 `ConsultarVigiaAsync` — riesgo LA/FT (Vigía vía gateway)

| Propiedad | Valor |
|---|---|
| Método y ruta | `GET {BaseUrl}api/v1/vigia/customer/{identification}/document-type/{documentType}/CheckRiskLAFT` |
| Construcción | Plantilla `Endpoints:ConsultarVigia`, con la parte `api/v1/tusdatos` reemplazada por `api/v1` (**no lleva el segmento `tusdatos`**) |
| Entradas en GA2 | `GET api/terceros/validacion/actualizacion/vigia` (`[Authorize]`) y `GET api/integrations/tusdatos/vigia/customer/{identification}/document-type/{documentType}` |

Response `200` (`VigiaResponse`):

```json
{
  "identification": "1020304050",
  "firstName": "JUAN CARLOS",
  "lastName": "PEREZ GOMEZ",
  "riskLAFT": "SIN RIESGO",
  "documentType": "CC"
}
```

> El dominio de `riskLAFT` tampoco se interpreta en GA2 (se devuelve al frontend). Confirmarlo con el gateway.

## 4. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Consulta que inicia un job | `200` con `jobId`, `isFromHistory=false` |
| 2 | Job pendiente → finalizado | Sondeos sucesivos que cambian `statusJobTusDatos` |
| 3 | Resultado del histórico | `200` con `isFromHistory=true` y `historyDataCustomer` |
| 4 | Persona PEP | `isPEPS=true` |
| 5 | No encontrado | `404` (GA2: éxito con `Data=null`) |
| 6 | API key inválida | `401` con un cuerpo de texto (GA2 lo muestra como `ReturnMessage`) |
| 7 | Lentitud | Retardo > 15 s (GA2: `IsSuccess=false`) |
