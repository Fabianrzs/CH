# E10 — Pasarela REST de Caja Honor "ktrionApi"

> Documento para implementar un **mock compatible** con la pasarela `https://aplicacion.cajahonor.gov.co/REST/KriterionDev/ktrionApi/`. GA2 la usa como **intermediario** hacia WorkManager y Haberes: el cuerpo JSON es el mismo objeto del proxy SOAP serializado con Newtonsoft. Los JSON de este documento se generaron con los proxies reales y `JsonConvert` con la misma configuración que GA2; las respuestas se validaron con la misma secuencia de deserialización.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | REST, JSON, `POST` |
| Base URL | `CHKeys:UrlCajaHonor` = `https://aplicacion.cajahonor.gov.co/REST/KriterionDev/ktrionApi/` |
| Autenticación | `POST {base}api/autenticacion` → token → header `Authorization: Bearer <token>` en cada llamada. **Se pide un token nuevo antes de cada llamada** |
| TLS | GA2 **acepta cualquier certificado** (`ServerCertificateCustomValidationCallback => true`): sirve un mock HTTPS autofirmado |
| Cliente .NET | `GA2.Application/Main/ProviderRequest/IntegracionesGA2/*` (`ProviderRequestFactory.PostExternalAsync`) ← `GA2.Domain/Core/IntegracionesGA2/*BusinessLogic` |
| Serialización del request | `JsonConvert.SerializeObject(obj)` **por defecto** (PascalCase; respeta `[DataContract]/[DataMember]` de los proxies) |
| Deserialización | Newtonsoft (camelCase resolver; la coincidencia de nombres no distingue mayúsculas) |
| Toggle | No hay. Las rutas WorkManager y Haberes se activan **sólo en compilación DEBUG**, salvo `workflowattachfiles`, que se usa siempre |
| Timeout | El `HttpClient` por defecto (100 s) |

## 2. Manejo de códigos HTTP (`ProviderRequestFactory.HandleResponse`)

| Respuesta del mock | Efecto en GA2 |
|---|---|
| `2xx` | Deserializa el cuerpo |
| `401` / `403` | `throw new Exception(body)` → sube hasta el `ErrorValidationMiddleware` → **HTTP 409** al frontend |
| Otro no exitoso | `throw new HttpRequestException(body)` → igual (409) |
| Timeout (100 s) | `TaskCanceledException` → 409 |

## 3. Operaciones

### 3.1 `POST api/autenticacion` — obtener token

**Request** (`LoginIntegracionesDto`, PascalCase):

```json
{
  "Id": "3f2c8a1e-0000-0000-0000-000000000000",
  "UserId": "3f2c8a1e-0000-0000-0000-000000000000",
  "NombreAplicacion": "IntegracionesKriterion",
  "Contrasena": "<AppMain:Password>",
  "AplicacionId": "<AppMain:ApplicationId>"
}
```

`Id` y `UserId` son el `CorrelationId` interno de la petición (el mismo GUID en ambos). Los valores salen de `AppMain:ApplicationName`, `AppMain:Password` y `AppMain:ApplicationId`. Headers: `Content-Type: application/json; charset=utf-8`, sin `Authorization`.

**Response esperada** (`Response<TokenDto>`):

```json
{ "IsSuccess": true, "ReturnMessage": "ok", "Data": { "Token": "eyJhbGciOi..." } }
```

GA2 usa `Data.Token` sin validarlo. **Si `Data` es nulo, se produce `NullReferenceException`** (→ 409).

### 3.2 `POST api/forminsert` — radicar en WorkManager (sólo DEBUG)

Invocada por `GestorDocumentalBusinessLogic` (radicación de crédito, ≈ línea 300) cuando GA2 está compilado en **DEBUG**. Headers: `Authorization: Bearer <token>`, `Content-Type: application/json; charset=utf-8`.

**Request** (proxy `Form_InsertRequest` serializado; los nombres respetan `[DataMember]`):

```json
{
  "request": {
    "CheckWorkflow": true,
    "Data": [
      {
        "Field": "Field",
        "Key": "Key",
        "Value": "Value"
      }
    ],
    "Files": [
      {
        "Base64String": "Base64String",
        "Description": "Description",
        "DirectoryCode": "DirectoryCode",
        "DocumentTypeCode": "DocumentTypeCode",
        "Ext": "Ext",
        "GUID": "GUID"
      }
    ],
    "FormCode": "FormCode",
    "Header": {
      "Token": "Token",
      "User": "User"
    },
    "OfficeCode": "OfficeCode",
    "OperationUser": "OperationUser",
    "ProcessId": 1,
    "nextStepUser": "nextStepUser"
  }
}
```

**Response que espera GA2** (`Response<Form_InsertResponse>`; validada con la secuencia `JObject → JsonConvert.DeserializeObject<Response<Form_InsertResponse>>`):

```json
{
  "IsSuccess": true,
  "ReturnMessage": "ok",
  "Data": {
    "Form_InsertResult": {
      "BarCode": "83-000123",
      "FileCodes": [
        "FC-001"
      ],
      "Message": "Message",
      "RecordId": 1,
      "Success": true,
      "workflowId": 1
    }
  }
}
```

GA2 lee `Data.Form_InsertResult.BarCode` (número de radicado; vacío → `"RADICADO NO DISPONIBLE"`). Las mismas reglas que en E2 §3.1.

### 3.3 `POST api/formUpdate` — actualizar formulario (sólo DEBUG)

**Request:**

```json
{
  "request": {
    "Data": [
      {
        "Field": "Field",
        "Key": "Key",
        "Value": "Value"
      }
    ],
    "FilterParameters": {
      "Field": "Field",
      "Operator": "Operator",
      "Value": "Value"
    },
    "FormCode": "FormCode",
    "Header": {
      "Token": "Token",
      "User": "User"
    },
    "OperationUser": "OperationUser"
  }
}
```

**Response que espera GA2** (`Response<Form_UpdateResponse>`):

```json
{
  "IsSuccess": true,
  "ReturnMessage": "ok",
  "Data": {
    "Form_UpdateResult": {
      "Data": [
        {
          "Field": "Field",
          "Key": "Key",
          "Value": "Value"
        }
      ],
      "Message": "Message",
      "Rows": 1,
      "Success": true
    }
  }
}
```

### 3.4 `POST api/workflowattachfiles` — adjuntar archivos al workflow (**siempre**)

`GestorDocumentalBusinessLogic.AttachFiles` (≈ línea 369) la usa en todas las compilaciones.

**Request:**

```json
{
  "request": {
    "Files": [
      {
        "Base64String": "Base64String",
        "Description": "Description",
        "DirectoryCode": "DirectoryCode",
        "DocumentTypeCode": "DocumentTypeCode",
        "Ext": "Ext",
        "GUID": "GUID"
      }
    ],
    "Header": {
      "Token": "Token",
      "User": "User"
    },
    "OperationUser": "OperationUser",
    "WorkflowId": 1
  }
}
```

**Response que espera GA2** (`Response<Workflow_AttachFilesResponse>`):

```json
{
  "IsSuccess": true,
  "ReturnMessage": "ok",
  "Data": {
    "Workflow_AttachFilesResult": {
      "BarCode": "BarCode",
      "FileCodes": [
        "FileCodes"
      ],
      "Message": "Message",
      "RecordId": 1,
      "Success": true,
      "workflowId": 1
    }
  }
}
```

### 3.5 `POST api/generarreportehaberes` — certificado de haberes (sólo DEBUG)

`HaberesBusinessLogic.GenerarReporteHaberes` en DEBUG (en Release va por E6).

**Request** (`HaberesRequestDto`):

```json
{ "Reporte": "CERTIFICADO", "ClienteIdentificacion": "1020304050", "CuentaId": 1234, "Mes": null, "Anio": null }
```

**Response que espera GA2** (`Response<string>`):

```json
{ "IsSuccess": true, "ReturnMessage": "ok", "Data": "JVBERi0xLjQK..." }
```

`Data` debe ser el **PDF en Base64**: GA2 lo guarda en el Blob `credito` y lo devuelve como archivo.

## 4. Rutas definidas en el código sin uso activo

`api/formdelete`, `api/formgetdata`, `api/formgetfilecodes`, `api/formgetbase64stringfile`, `api/listgetitems` (WorkManager) y `api/obtenerafiliadobua` (BUA) tienen cliente, pero ningún flujo los invoca. Las variantes Vigía y Fénix reciben la URL como parámetro y tampoco tienen consumidores. El cuerpo seguiría el mismo patrón: proxy SOAP serializado con Newtonsoft y respuesta `Response<T>`.

> **Variante legada `ProviderRequestG2`** (`CHKeys:LoginCajaHonor` + token cifrado MD5/AES + `CHGUID`): registrada en BUA e Identity, sin consumidores. No requiere mock.

## 5. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Token OK + radicación OK | `autenticacion` 200 + `forminsert` 200 con `BarCode` |
| 2 | Token rechazado | `autenticacion` 401 → GA2 responde 409 |
| 3 | Radicación con `Success=false` | `Data.Form_InsertResult.Success=false` |
| 4 | Adjuntos | `workflowattachfiles` 200 |
| 5 | Haberes | `generarreportehaberes` con PDF Base64 |
