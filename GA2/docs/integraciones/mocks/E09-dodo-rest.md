# E9 — Dodo REST (nueva API del gestor documental) — REST/JSON

> Documento para implementar un **mock compatible** con la API REST de Dodo (`dodo-docs-test:10081`). En GA2 **sólo la usa** `DodoController`, que la expone tal cual (pass-through) en `api/integrations/dodo/*`, sin `[Authorize]`. Es la migración en curso de WorkManager SOAP (E2) a REST: el negocio todavía la consume.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | REST, JSON; todas las operaciones son `POST` salvo `api/version` (`GET`) |
| Base URL | `Integrationes:Dodo:BaseUrl` = `https://dodo-docs-test:10081/` |
| Headers enviados | `Accept: application/json`; en POST, `Content-Type: application/json; charset=utf-8` |
| Autenticación | Sin header. En las operaciones de **Forms** (`PostDodoAsync`), GA2 inyecta en el body `"header": {"user": CHKeys:User, "token": CHKeys:Token}`, que sobrescribe lo que envíe el llamador. En el resto, el `header` llega tal cual desde el body que recibe `DodoController` (el llamador debe incluirlo) |
| Timeout | `Integrationes:Dodo:TimeoutSeconds` = 15 s |
| Serialización del request | `System.Text.Json`, **camelCase**, **omite propiedades nulas** |
| Deserialización de la respuesta | `System.Text.Json`, sin distinguir mayúsculas |
| Toggle en BD | `Dodo` / `ESTADO`. Si está inactivo se lanza `InvalidOperationException` **fuera del `try`** → HTTP 409 (Conflict) desde `ErrorValidationMiddleware` |
| Auditoría | `InsertReportIntegracionesRest` por llamada |
| Respuesta de GA2 al frontend | `Response<T> {isSuccess, returnMessage, data}` con HTTP 200 (siempre `Ok(result)`) |

## 2. Manejo de códigos HTTP

| Respuesta del mock | Resultado en GA2 |
|---|---|
| `2xx` + JSON | `IsSuccess=true`, `ReturnMessage="Proceso exitoso"` (POST) o `"Consulta exitosa"` (GET), `Data` deserializado |
| `404` en el `GET api/version` | `IsSuccess=true`, `Data=null` |
| Otro código no exitoso | `IsSuccess=false`, `ReturnMessage` = cuerpo crudo |
| Timeout / red / JSON inválido | `IsSuccess=false`, `ReturnMessage` = mensaje de la excepción |

> `GET api/version` se deserializa como `string` JSON: el mock debe responder **una cadena JSON entre comillas** (p. ej. `"1.4.2"`), no texto plano. Con texto plano GA2 devuelve `IsSuccess=false`.

## 3. Tipos comunes

`DodoHeader`, `DodoDataField`, `DodoFileItem`, `DodoFilterParameters`, `DodoFileProperty`, `DodoFileOperationResponse`:


#### `DodoHeader`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `user` | string | no |  |
| `token` | string | no |  |
| `connectionName` | string | sí |  |

#### `DodoDataField`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `field` | string | no |  |
| `value` | string | no |  |
| `key` | string | sí |  |

#### `DodoFileItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `base64String` | string | sí |  |
| `ext` | string | no |  |
| `description` | string | no |  |
| `documentTypeCode` | string | no | `"00"` |
| `directoryCode` | string | no | `"00"` |
| `guid` | string | sí | `"00000000-0000-0000-0000-000000000000"` |
| `version` | string | sí | `"1"` |
| `length` | number (int) | sí |  |
| `mainFileId` | number (int) | sí |  |
| `publishDate` | string (ISO-8601) | sí | `"0001-01-01T00:00:00"` |

#### `DodoFilterParameters`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `field` | string | no |  |
| `operator` | string | no |  |
| `value` | string | no |  |

#### `DodoFileProperty`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `fileId` | number (int) | no | `0` |
| `code` | string | no |  |
| `description` | string | no |  |
| `ext` | string | no |  |
| `size` | number (long) | no | `0` |
| `docDate` | string (ISO-8601) | sí |  |
| `publishDate` | string (ISO-8601) | sí |  |
| `documentTypeCode` | string | sí |  |
| `version` | string | sí |  |

#### `DodoFileOperationResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `barCode` | string | sí |  |
| `recordId` | number (int) | no | `0` |
| `workflowId` | number (int) | no | `0` |
| `fileCodes` | array<string> | no | `[]` |
| `fileIdList` | array<number (int)> | no | `[]` |
| `destinationPath` | array<string> | no | `[]` |


> `DodoFileItem` lleva valores por defecto que **siempre** se envían (no son nulos): `documentTypeCode="00"`, `directoryCode="00"`, `guid="00000000-0000-0000-0000-000000000000"`, `version="1"`, `publishDate="0001-01-01T00:00:00"`. El mock debe tolerarlos.

## 4. Operaciones

### 4.1 `POST api/Field/Field_GetValues`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFieldService.GetFieldValuesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/field/values` |
| Request | `FieldGetValuesRequest` |
| Response | `FieldGetValuesResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  },
  "formCode": "FormCode",
  "columnName": "ColumnName",
  "fatherItemId": 1
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "items": [
    {
      "id": 1,
      "value": "Value",
      "text": "Text",
      "toolTipText": "ToolTipText"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FieldGetValuesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `header` | objeto `DodoHeader` | no |  |
| `formCode` | string | no |  |
| `columnName` | string | no |  |
| `fatherItemId` | number (int) | sí |  |
#### `FieldGetValuesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `items` | array<objeto `FieldValueItem`> | no | `[]` |
| `rows` | number (int) | no | `0` |

#### `FieldValueItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `id` | number (int) | no | `0` |
| `value` | string | no |  |
| `text` | string | no |  |
| `toolTipText` | string | sí |  |

</details>

### 4.2 `POST api/Lists/List_GetItems`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFieldService.GetListItemsAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/lists/items` |
| Request | `ListGetItemsRequest` |
| Response | `ListGetItemsResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "listId": 1,
  "fatherItemId": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "data": [
    {
      "idItem": 1,
      "idLista": 1,
      "codigo": "Codigo",
      "nombre": "Nombre",
      "idItemPadre": 1,
      "activo": true,
      "usuario": "Usuario",
      "fecha": "2026-01-15T00:00:00",
      "host": "Host",
      "toolTipText": "ToolTipText"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `ListGetItemsRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `listId` | number (int) | no | `0` |
| `fatherItemId` | number (int) | sí |  |
| `header` | objeto `DodoHeader` | no |  |
#### `ListGetItemsResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `data` | array<objeto `ListItem`> | no | `[]` |
| `rows` | number (int) | no | `0` |

</details>

### 4.3 `POST api/File/File_CopyToEP`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFileService.CopyToEPAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/file/copy-to-ep` |
| Request | `FileCopyToEPRequest` |
| Response | `FileSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "fileCode": "FileCode",
  "directoryCodeEP": "DirectoryCodeEP",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `FileCopyToEPRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `fileCode` | string | no |  |
| `directoryCodeEP` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `FileSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.4 `POST api/File/File_CopyToFather`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFileService.CopyToFatherAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/file/copy-to-father` |
| Request | `FileCopyToFatherRequest` |
| Response | `FileSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "barCodeSon": "BarCodeSon",
  "fileCode": "FileCode",
  "directoryCodeFather": "DirectoryCodeFather",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `FileCopyToFatherRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `barCodeSon` | string | no |  |
| `fileCode` | string | no |  |
| `directoryCodeFather` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `FileSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.5 `POST api/File/File_Delete`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFileService.DeleteFileAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/file/delete` |
| Request | `FileDeleteRequest` |
| Response | `FileSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "fileCode": "FileCode",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `FileDeleteRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `fileCode` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `FileSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.6 `POST api/Forms/Form_GetParameters`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.GetParametersAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/parameters` |
| Request | `FormGetParametersRequest` |
| Response | `FormGetParametersResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "formCode": "FormCode",
  "formId": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "form": {
    "idFormulario": 1,
    "tabla": "Tabla",
    "nombre": "Nombre",
    "descripcion": "Descripcion",
    "idPadre": 1,
    "estado": 1,
    "codigo": "Codigo",
    "usuario": "Usuario",
    "fecha": "2026-01-15T00:00:00",
    "host": "Host",
    "archivoRequerido": true,
    "workflowUnico": true
  }
}
```

<details><summary>Tablas de campos</summary>

#### `FormGetParametersRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `formCode` | string | no |  |
| `formId` | number (int) | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormGetParametersResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `form` | objeto `FormDefinition` | sí |  |

#### `FormDefinition`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `idFormulario` | number (int) | no | `0` |
| `tabla` | string | no |  |
| `nombre` | string | no |  |
| `descripcion` | string | sí |  |
| `idPadre` | number (int) | sí |  |
| `estado` | number (int) | no | `0` |
| `codigo` | string | no |  |
| `usuario` | string | sí |  |
| `fecha` | string (ISO-8601) | sí |  |
| `host` | string | sí |  |
| `archivoRequerido` | boolean | no | `false` |
| `workflowUnico` | boolean | no | `false` |

</details>

### 4.7 `POST api/Forms/Form_Insert`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.InsertAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/insert` |
| Request | `FormInsertRequest` |
| Response | `DodoFileOperationResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "formCode": "FormCode",
  "officeCode": "OfficeCode",
  "nextStepUser": "NextStepUser",
  "processId": 1,
  "data": [
    {
      "field": "Field",
      "value": "Value",
      "key": "Key"
    }
  ],
  "files": [
    {
      "base64String": "Base64String",
      "ext": "Ext",
      "description": "Description",
      "documentTypeCode": "DocumentTypeCode",
      "directoryCode": "DirectoryCode",
      "guid": "GUID",
      "version": "Version",
      "length": 1,
      "mainFileId": 1,
      "publishDate": "2026-01-15T00:00:00"
    }
  ],
  "checkWorkflow": true,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "barCode": "BarCode",
  "recordId": 1,
  "workflowId": 1,
  "fileCodes": [
    "FileCodes"
  ],
  "fileIdList": [
    1
  ],
  "destinationPath": [
    "DestinationPath"
  ]
}
```

<details><summary>Tablas de campos</summary>

#### `FormInsertRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `formCode` | string | no |  |
| `officeCode` | string | no |  |
| `nextStepUser` | string | sí |  |
| `processId` | number (int) | sí |  |
| `data` | array<objeto `DodoDataField`> | no | `[]` |
| `files` | array<objeto `DodoFileItem`> | sí |  |
| `checkWorkflow` | boolean | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |

</details>

### 4.8 `POST api/Forms/Form_Update`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.UpdateAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/update` |
| Request | `FormUpdateRequest` |
| Response | `FormUpdateResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "formCode": "FormCode",
  "data": [
    {
      "field": "Field",
      "value": "Value",
      "key": "Key"
    }
  ],
  "filterParameters": {
    "field": "Field",
    "operator": "Operator",
    "value": "Value"
  },
  "filter": "Filter",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FormUpdateRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `formCode` | string | no |  |
| `data` | array<objeto `DodoDataField`> | no | `[]` |
| `filterParameters` | objeto `DodoFilterParameters` | no |  |
| `filter` | string | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormUpdateResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `rows` | number (int) | no | `0` |

</details>

### 4.9 `POST api/Forms/Form_Delete`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.DeleteAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/delete` |
| Request | `FormDeleteRequest` |
| Response | `FormDeleteResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "formCode": "FormCode",
  "filterParameters": {
    "field": "Field",
    "operator": "Operator",
    "value": "Value"
  },
  "filter": "Filter",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FormDeleteRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `formCode` | string | no |  |
| `filterParameters` | objeto `DodoFilterParameters` | no |  |
| `filter` | string | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormDeleteResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `rows` | number (int) | no | `0` |

</details>

### 4.10 `POST api/Forms/Form_GetData`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.GetDataAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/data` |
| Request | `FormGetDataRequest` |
| Response | `FormGetDataResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "formCode": "FormCode",
  "filterParameters": {
    "field": "Field",
    "operator": "Operator",
    "value": "Value"
  },
  "filter": "Filter",
  "orderColumn": "OrderColumn",
  "orderType": "OrderType",
  "top": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "data": [
    {
      "field": "Field",
      "value": "Value",
      "key": "Key"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FormGetDataRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `formCode` | string | no |  |
| `filterParameters` | objeto `DodoFilterParameters` | no |  |
| `filter` | string | sí |  |
| `orderColumn` | string | sí |  |
| `orderType` | string | sí |  |
| `top` | number (int) | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormGetDataResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `data` | array<objeto `DodoDataField`> | no | `[]` |
| `rows` | number (int) | no | `0` |

</details>

### 4.11 `POST api/Forms/Form_AttachFiles`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.AttachFilesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/attach-files` |
| Request | `FormAttachFilesRequest` |
| Response | `DodoFileOperationResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "barCode": "BarCode",
  "files": [
    {
      "base64String": "Base64String",
      "ext": "Ext",
      "description": "Description",
      "documentTypeCode": "DocumentTypeCode",
      "directoryCode": "DirectoryCode",
      "guid": "GUID",
      "version": "Version",
      "length": 1,
      "mainFileId": 1,
      "publishDate": "2026-01-15T00:00:00"
    }
  ],
  "checkWorkflow": true,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "barCode": "BarCode",
  "recordId": 1,
  "workflowId": 1,
  "fileCodes": [
    "FileCodes"
  ],
  "fileIdList": [
    1
  ],
  "destinationPath": [
    "DestinationPath"
  ]
}
```

<details><summary>Tablas de campos</summary>

#### `FormAttachFilesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `barCode` | string | no |  |
| `files` | array<objeto `DodoFileItem`> | no | `[]` |
| `checkWorkflow` | boolean | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |

</details>

### 4.12 `POST api/Forms/Form_GetFileCodes`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.GetFileCodesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/file-codes` |
| Request | `FormGetFileCodesRequest` |
| Response | `FormGetFileCodesResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "barCode": "BarCode",
  "directoryCode": "DirectoryCode",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "fileProperties": [
    {
      "fileId": 1,
      "code": "Code",
      "description": "Description",
      "ext": "Ext",
      "size": 1,
      "docDate": "2026-01-15T00:00:00",
      "publishDate": "2026-01-15T00:00:00",
      "documentTypeCode": "DocumentTypeCode",
      "version": "Version"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FormGetFileCodesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `barCode` | string | no |  |
| `directoryCode` | string | sí |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormGetFileCodesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `fileProperties` | array<objeto `DodoFileProperty`> | no | `[]` |
| `rows` | number (int) | no | `0` |

</details>

### 4.13 `POST api/Forms/Form_GetFilesDirectories`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.GetFilesDirectoriesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/directories` |
| Request | `FormGetFilesDirectoriesRequest` |
| Response | `FormGetFilesDirectoriesResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "formCode": "FormCode",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "data": [
    {
      "idDirectorio": 1,
      "idFormulario": 1,
      "codigo": "Codigo",
      "nombre": "Nombre",
      "idEstado": 1
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `FormGetFilesDirectoriesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `formCode` | string | no |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormGetFilesDirectoriesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `data` | array<objeto `FormDirectoryItem`> | no | `[]` |
| `rows` | number (int) | no | `0` |

#### `FormDirectoryItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `idDirectorio` | number (int) | no | `0` |
| `idFormulario` | number (int) | no | `0` |
| `codigo` | string | no |  |
| `nombre` | string | no |  |
| `idEstado` | number (int) | no | `0` |

</details>

### 4.14 `POST api/Forms/Form_GetBase64StringFile`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoFormsService.GetBase64StringFileAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/forms/file-base64` |
| Request | `FormGetBase64StringFileRequest` |
| Response | `FormGetBase64StringFileResponse` |
| Inyección de `header` | sí (`CHKeys:User/Token`) |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "fileCode": "FileCode",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "file": {
    "fileId": 1,
    "code": "Code",
    "description": "Description",
    "ext": "Ext",
    "size": 1,
    "docDate": "2026-01-15T00:00:00",
    "publishDate": "2026-01-15T00:00:00",
    "documentTypeCode": "DocumentTypeCode",
    "version": "Version"
  }
}
```

<details><summary>Tablas de campos</summary>

#### `FormGetBase64StringFileRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `fileCode` | string | no |  |
| `header` | objeto `DodoHeader` | no | instancia vacía |
#### `FormGetBase64StringFileResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `file` | objeto `DodoFileProperty` | sí |  |

</details>

### 4.15 `POST api/Notification/SendEmail`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoNotificationService.SendEmailAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/notification/send-email` |
| Request | `SendEmailRequest` |
| Response | `SendEmailResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "sender": "Sender",
  "receiver": "Receiver",
  "subject": "Subject",
  "message": "Message",
  "isHtml": true,
  "user": "User",
  "host": "Host",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `SendEmailRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `sender` | string | no |  |
| `receiver` | string | no |  |
| `subject` | string | no |  |
| `message` | string | no |  |
| `isHtml` | boolean | no | `false` |
| `user` | string | no |  |
| `host` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `SendEmailResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.16 `POST api/Offices/GetOffices`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoOfficesService.GetOfficesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/offices/list` |
| Request | `GetOfficesRequest` |
| Response | `GetOfficesResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "officeList": [
    {
      "officeId": 1,
      "code": "Code",
      "name": "Name"
    }
  ],
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `GetOfficesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `header` | objeto `DodoHeader` | no |  |
#### `GetOfficesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `officeList` | array<objeto `OfficeItem`> | no | `[]` |
| `success` | boolean | no | `false` |
| `message` | string | no |  |

#### `OfficeItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `officeId` | number (int) | no | `0` |
| `code` | string | no |  |
| `name` | string | no |  |

</details>

### 4.17 `POST api/Token/GetToken`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoTokenService.GetTokenAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/token/get` |
| Request | `GetTokenRequest` |
| Response | `GetTokenResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "user": "User",
  "password": "Password",
  "expirationTimeSeconds": 1.5
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "user": "User",
  "token": "Token",
  "tokenDate": "TokenDate",
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `GetTokenRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `user` | string | no |  |
| `password` | string | no |  |
| `expirationTimeSeconds` | number | sí |  |
#### `GetTokenResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `user` | string | no |  |
| `token` | string | no |  |
| `tokenDate` | string | no |  |
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.18 `POST api/User/User_Info`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoUserService.GetUserInfoAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/user/info` |
| Request | `UserInfoRequest` |
| Response | `UserInfoResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "userName": "UserName",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "userInfo": {
    "userName": "UserName",
    "fullName": "FullName",
    "email": "Email"
  },
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `UserInfoRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `userName` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `UserInfoResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `userInfo` | objeto `UserInfoData` | no |  |
| `success` | boolean | no | `false` |
| `message` | string | no |  |

#### `UserInfoData`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `userName` | string | no |  |
| `fullName` | string | no |  |
| `email` | string | no |  |

</details>

### 4.19 `GET api/version`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoVersionService.GetVersionInfoAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/version/get` |
| Request | sin body |
| Response | `string` |
| Inyección de `header` | no |

**Response que espera GA2:** una cadena JSON, p. ej. `"1.4.2"`.

### 4.20 `POST api/Workflow/Workflow_Start`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.StartAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/start` |
| Request | `WorkflowStartRequest` |
| Response | `WorkflowStartResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "nextStepUser": "NextStepUser",
  "barCode": "BarCode",
  "processId": 1,
  "files": [
    {
      "base64String": "Base64String",
      "ext": "Ext",
      "description": "Description",
      "documentTypeCode": "DocumentTypeCode",
      "directoryCode": "DirectoryCode",
      "guid": "GUID",
      "version": "Version",
      "length": 1,
      "mainFileId": 1,
      "publishDate": "2026-01-15T00:00:00"
    }
  ],
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "workflowId": 1,
  "fileCodes": [
    "FileCodes"
  ]
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowStartRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `nextStepUser` | string | sí |  |
| `barCode` | string | no |  |
| `processId` | number (int) | no | `0` |
| `files` | array<objeto `DodoFileItem`> | sí |  |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowStartResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `workflowId` | number (int) | no | `0` |
| `fileCodes` | array<string> | no | `[]` |

</details>

### 4.21 `POST api/Workflow/Workflow_RunTask`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.RunTaskAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/run-task` |
| Request | `WorkflowRunTaskRequest` |
| Response | `WorkflowSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "barCode": "BarCode",
  "processId": 1,
  "workflowId": 1,
  "step": 1,
  "comment": "Comment",
  "nextStepUser": "NextStepUser",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowRunTaskRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `barCode` | string | sí |  |
| `processId` | number (int) | sí |  |
| `workflowId` | number (int) | sí |  |
| `step` | number (int) | no | `0` |
| `comment` | string | sí |  |
| `nextStepUser` | string | sí |  |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.22 `POST api/Workflow/Workflow_ReturnTask`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.ReturnTaskAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/return-task` |
| Request | `WorkflowReturnTaskRequest` |
| Response | `WorkflowSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  },
  "workflowId": 1,
  "sealMail": true,
  "returnStep": 1,
  "operationUser": "OperationUser",
  "comment": "Comment"
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowReturnTaskRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `header` | objeto `DodoHeader` | no |  |
| `workflowId` | number (int) | no | `0` |
| `sealMail` | boolean | no | `false` |
| `returnStep` | number (int) | no | `0` |
| `operationUser` | string | no |  |
| `comment` | string | no |  |
#### `WorkflowSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.23 `POST api/Workflow/Workflow_AbortProcess`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.AbortProcessAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/abort` |
| Request | `WorkflowAbortProcessRequest` |
| Response | `WorkflowSimpleResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "workflowId": 1,
  "operationUser": "OperationUser",
  "comment": "Comment",
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message"
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowAbortProcessRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `workflowId` | number (int) | no | `0` |
| `operationUser` | string | no |  |
| `comment` | string | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowSimpleResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |

</details>

### 4.24 `POST api/Workflow/Workflow_AttachFiles`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.AttachFilesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/attach-files` |
| Request | `WorkflowAttachFilesRequest` |
| Response | `DodoFileOperationResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "operationUser": "OperationUser",
  "workflowId": 1,
  "copyToForm": true,
  "files": [
    {
      "base64String": "Base64String",
      "ext": "Ext",
      "description": "Description",
      "documentTypeCode": "DocumentTypeCode",
      "directoryCode": "DirectoryCode",
      "guid": "GUID",
      "version": "Version",
      "length": 1,
      "mainFileId": 1,
      "publishDate": "2026-01-15T00:00:00"
    }
  ],
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "barCode": "BarCode",
  "recordId": 1,
  "workflowId": 1,
  "fileCodes": [
    "FileCodes"
  ],
  "fileIdList": [
    1
  ],
  "destinationPath": [
    "DestinationPath"
  ]
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowAttachFilesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `operationUser` | string | no |  |
| `workflowId` | number (int) | no | `0` |
| `copyToForm` | boolean | sí |  |
| `files` | array<objeto `DodoFileItem`> | no | `[]` |
| `header` | objeto `DodoHeader` | no |  |

</details>

### 4.25 `POST api/Workflow/Workflow_GetCases`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.GetCasesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/cases` |
| Request | `WorkflowGetCasesRequest` |
| Response | `WorkflowGetCasesResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "formCode": "FormCode",
  "processId": 1,
  "filterParameters": {
    "field": "Field",
    "operator": "Operator",
    "value": "Value"
  },
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "workflowCases": [
    {
      "workflowId": 1,
      "state": "State",
      "createDate": "2026-01-15T00:00:00",
      "finishDate": "2026-01-15T00:00:00",
      "processId": 1,
      "recordId": 1
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowGetCasesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `formCode` | string | no |  |
| `processId` | number (int) | no | `0` |
| `filterParameters` | objeto `DodoFilterParameters` | no |  |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowGetCasesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `workflowCases` | array<objeto `WorkflowCaseItem`> | no | `[]` |
| `rows` | number (int) | no | `0` |

#### `WorkflowCaseItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `workflowId` | number (int) | no | `0` |
| `state` | string | no |  |
| `createDate` | string (ISO-8601) | no | `"0001-01-01T00:00:00"` |
| `finishDate` | string (ISO-8601) | sí |  |
| `processId` | number (int) | no | `0` |
| `recordId` | number (int) | no | `0` |

</details>

### 4.26 `POST api/Workflow/Workflow_GetTasks`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.GetTasksAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/tasks` |
| Request | `WorkflowGetTasksRequest` |
| Response | `WorkflowGetTasksResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "workflowId": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "tasks": [
    {
      "step": 1,
      "description": "Description",
      "state": "State",
      "priority": "Priority",
      "ownerUser": "OwnerUser",
      "createDate": "2026-01-15T00:00:00",
      "expirationDate": "2026-01-15T00:00:00"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowGetTasksRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `workflowId` | number (int) | no | `0` |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowGetTasksResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `tasks` | array<objeto `WorkflowTaskItem`> | no | `[]` |
| `rows` | number (int) | no | `0` |

#### `WorkflowTaskItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `step` | number (int) | no | `0` |
| `description` | string | no |  |
| `state` | string | no |  |
| `priority` | string | sí |  |
| `ownerUser` | string | sí |  |
| `createDate` | string (ISO-8601) | sí |  |
| `expirationDate` | string (ISO-8601) | sí |  |

</details>

### 4.27 `POST api/Workflow/Workflow_GetFileCodes`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.GetFileCodesAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/file-codes` |
| Request | `WorkflowGetFileCodesRequest` |
| Response | `WorkflowGetFileCodesResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "workflowId": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "fileProperties": [
    {
      "fileId": 1,
      "code": "Code",
      "description": "Description",
      "ext": "Ext",
      "size": 1,
      "docDate": "2026-01-15T00:00:00",
      "publishDate": "2026-01-15T00:00:00",
      "documentTypeCode": "DocumentTypeCode",
      "version": "Version"
    }
  ],
  "rows": 1
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowGetFileCodesRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `workflowId` | number (int) | no | `0` |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowGetFileCodesResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `fileProperties` | array<objeto `DodoFileProperty`> | no | `[]` |
| `rows` | number (int) | no | `0` |

</details>

### 4.28 `POST api/Workflow/Workflow_StepsForReturn`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.GetStepsForReturnAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/steps-for-return` |
| Request | `WorkflowStepsForReturnRequest` |
| Response | `WorkflowStepsForReturnResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "workflowId": 1,
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  }
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "stepList": [
    {
      "paso": 1,
      "descripcion": "Descripcion",
      "idProceso": 1
    }
  ]
}
```

<details><summary>Tablas de campos</summary>

#### `WorkflowStepsForReturnRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `workflowId` | number (int) | no | `0` |
| `header` | objeto `DodoHeader` | no |  |
#### `WorkflowStepsForReturnResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `stepList` | array<objeto `WorkflowStepItem`> | no | `[]` |

#### `WorkflowStepItem`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `paso` | number (int) | no | `0` |
| `descripcion` | string | no |  |
| `idProceso` | number (int) | no | `0` |

</details>

### 4.29 `POST api/Workflow/Process_Parameters`

| Propiedad | Valor |
|---|---|
| Método del cliente | `DodoWorkflowService.GetProcessParametersAsync` |
| Endpoint GA2 que la expone | `POST api/integrations/dodo/workflow/process-parameters` |
| Request | `ProcessParametersRequest` |
| Response | `ProcessParametersResponse` |
| Inyección de `header` | no |

**Request de ejemplo** (generado con los DTO reales y las mismas opciones de serialización de GA2; valores de relleno):

```json
{
  "header": {
    "user": "User",
    "token": "Token",
    "connectionName": "ConnectionName"
  },
  "processId": 1
}
```

**Response que espera GA2** (estructura; el mock puede devolver estas propiedades en camelCase o PascalCase):

```json
{
  "success": true,
  "message": "Message",
  "parameters": {
    "idProceso": 1,
    "proceso": "Proceso",
    "descripcion": "Descripcion",
    "estado": 1,
    "version": "Version",
    "usuario": "Usuario",
    "fecha": "2026-01-15T00:00:00",
    "host": "Host",
    "idFormulario": 1
  }
}
```

<details><summary>Tablas de campos</summary>

#### `ProcessParametersRequest`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `header` | objeto `DodoHeader` | no |  |
| `processId` | number (int) | no | `0` |
#### `ProcessParametersResponse`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `success` | boolean | no | `false` |
| `message` | string | no |  |
| `parameters` | objeto `ProcessDefinition` | sí |  |

#### `ProcessDefinition`

| Propiedad JSON | Tipo | Nulable | Valor por defecto en GA2 |
|---|---|---|---|
| `idProceso` | number (int) | no | `0` |
| `proceso` | string | no |  |
| `descripcion` | string | sí |  |
| `estado` | number (int) | no | `0` |
| `version` | string | sí |  |
| `usuario` | string | sí |  |
| `fecha` | string (ISO-8601) | sí |  |
| `host` | string | sí |  |
| `idFormulario` | number (int) | no | `0` |

</details>


## 5. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | `forms/insert` exitoso | `DodoFileOperationResponse {success:true, barCode:"361-000045", recordId:45, workflowId:120}` |
| 2 | `forms/insert` con error de negocio | `success:false, message:"..."` (HTTP 200) |
| 3 | Error técnico | HTTP 500 con texto (GA2: `IsSuccess=false`, `ReturnMessage` = texto) |
| 4 | Versión | `GET api/version` → `"1.0.0"` |
| 5 | Integración apagada | Toggle `Dodo.ESTADO=false` → GA2 responde 409 sin llamar |

## 6. Código fuera de uso relacionado

- `ReferenciasSvc/Dodo/Middleware/GestorDocumental/GestorDocumentalDodo.cs` (orquestación de formularios 361/83/133/219/02) no está registrado en DI ni tiene consumidores.
- `ReportesBusinessLogic` inyecta `IDodoFormsService` pero no lo usa.
- Las constantes de `Forms/Constants/FormsConstans.cs` documentan los códigos de formulario (`02`, `83`, `133`, `219`, `361`) y los campos (`C0088`, `C0010`, …) previstos para esta API.
