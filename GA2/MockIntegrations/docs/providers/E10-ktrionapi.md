# E10 · Pasarela REST "ktrionApi" de Caja Honor — mock REST + Bearer

**Especificación:** [docs/integraciones/mocks/E10-pasarela-ktrionapi.md](../../../docs/integraciones/mocks/E10-pasarela-ktrionapi.md).
**Código:** [`Providers/KtrionApi/`](../../src/MockIntegrations.Api/Providers/KtrionApi/).

**Base:** `CHKeys:UrlCajaHonor = http://<mock>:5080/REST/KriterionDev/ktrionApi/`.

| Ruta | Uso en GA2 | Respuesta |
|---|---|---|
| `POST api/autenticacion` | Antes de **cada** llamada | `{IsSuccess, ReturnMessage, Data: {Token}}`; el token queda registrado en el mock |
| `POST api/forminsert` | Radicación de crédito (sólo DEBUG) | `Data.Form_InsertResult {BarCode "{FormCode}-{n}", FileCodes, Message, RecordId, Success, workflowId}` |
| `POST api/formUpdate` | Actualización (sólo DEBUG) | `Data.Form_UpdateResult {Data, Message, Rows, Success}` |
| `POST api/workflowattachfiles` | Adjuntos del workflow de crédito (**siempre**) | `Data.Workflow_AttachFilesResult` |
| `POST api/generarreportehaberes` | Certificado de haberes (sólo DEBUG) | `Data` = PDF en Base64 (el mismo de E6) |
| `formdelete`, `formgetdata`, `formgetfilecodes`, `formgetbase64stringfile`, `listgetitems`, `obtenerafiliadobua` | Rutas definidas sin consumidores | **501** explícito (contrato no validado) |

- El cuerpo es el **proxy WCF de WorkManager serializado con Newtonsoft** (`{"request": {...}}`, nombres de los `DataMember`). El mock responde PascalCase; los nombres se fijan con `[JsonPropertyName]`, así que el JSON y el esquema OpenAPI coinciden. Las pruebas deserializan con la configuración de `ProviderRequestFactory` y los tipos de GA2.
- **Autorización:** las rutas, salvo `autenticacion`, exigen `Authorization: Bearer` con un token emitido por el mock (`MockIntegrations:KtrionApi:RequireIssuedToken`). Si no → 401 (GA2: `Exception(body)` → HTTP 409).

| Escenario | Respuesta |
|---|---|
| `unauthorized` | 401 en cualquier ruta |
| `sin-token` (autenticacion) | `Data=null`: reproduce el `NullReferenceException` de GA2 |
| `fallido` | Resultado WorkManager con `Success=false` |
| `sin-reporte` (haberes) | `IsSuccess=false`, `Data=null` |
