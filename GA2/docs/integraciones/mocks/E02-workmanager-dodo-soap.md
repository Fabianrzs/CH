# E2 — WorkManager / DODO (gestor documental y BPM) — servicio WCF SOAP

> Documento para implementar un **mock compatible** con `WorkManagerService.svc`. Los sobres de request se capturaron con el proxy WCF real del repositorio y los de response se validaron por ida y vuelta contra ese mismo proxy.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Sistema | WorkManager / DODO: gestor documental y motor de flujos (radicación de solicitudes, adjuntos, formularios) |
| Protocolo | SOAP 1.1, WCF `basicHttpBinding`, `DataContractSerializer` (document/literal wrapped) |
| URL | `SvcUtils:WorkManagerUrlSvc` = `http://cpvmp-118p/APIV2_0005/WorkManagerService.svc` |
| Contrato | `IWorkManagerServices` — namespace `http://tempuri.org/`; `SOAPAction = "http://tempuri.org/IWorkManagerServices/<Op>"` |
| Namespace de los DTO | `http://schemas.datacontract.org/2004/07/WorkManager.Entities` (prefijo `a:` en los ejemplos) |
| Autenticación | `Header{User, Token}` dentro del DTO de request (valores `CHKeys:User` / `CHKeys:Token`) |
| Binding cliente | `BasicHttpBinding` sin seguridad (`http`), timeouts `TimeSpan.MaxValue` |
| Cliente .NET | `ReferenciasSvc/WorkManager/ConsumoWorkManager.cs` → `ServiciosSOA/WorkManager/WorkManagerService.cs` |
| Toggle en BD | `DODO` (`ObtenerParametrosIntegraciones`): con `VALOR = "true"` se radica en solicitudes, reintegros y postulación; el flujo de crédito **no** lo consulta |
| Otros caminos al mismo sistema | Pasarela ktrionApi (E10, sólo DEBUG y `workflowattachfiles`), Service Bus (E12) y la API REST Dodo (E9) |

## 2. Reglas comunes para el mock

1. **Respuesta envuelta.** Toda respuesta tiene la forma `<{Op}Response xmlns="http://tempuri.org/"><{Op}Result>…DTO…</{Op}Result></{Op}Response>`, con los miembros del DTO en el namespace `…/WorkManager.Entities`. **El orden de los elementos es alfabético** (`DataContractSerializer` sin `Order`): `BarCode, FileCodes, Message, RecordId, Success, workflowId`. Un elemento fuera de orden se ignora silenciosamente (queda con su valor por defecto).
2. **Éxito y fallo.** GA2 decide por la propiedad `Success` del `...Result`. Un resultado nulo (p. ej. `<Form_InsertResult i:nil="true"/>`) provoca `NullReferenceException` en `WorkManagerService` → HTTP 409 (Conflict) en la API de GA2.
3. **Errores técnicos.**
   - **SOAP Fault** (`HTTP 500` + sobre `<s:Fault>`): el proxy WCF lanza `FaultException`. Salvo que se indique lo contrario en la operación, GA2 **no** la captura de forma específica y la excepción sube hasta el `ErrorValidationMiddleware` de la API, que responde al frontend **HTTP 409 (Conflict)** con un JSON de log de error.
   - **HTTP 500 sin sobre SOAP / HTML / cuerpo vacío**: `ProtocolException` o `CommunicationException`; mismo efecto.
   - **Sin respuesta**: los bindings de GA2 tienen `SendTimeout/ReceiveTimeout = TimeSpan.MaxValue` (salvo donde se indica), es decir, **la llamada no expira**. Un mock que no responde deja la petición colgada indefinidamente.
4. **Operaciones sin uso.** De las 29 operaciones del contrato, GA2 envuelve 12 (§3) y sólo ~6 tienen uso real. Las demás (§4) pueden responder con un SOAP Fault genérico.

## 3. Operaciones envueltas por GA2 (`IConsumoWorkManager`)

### 1. `Form_Insert`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (radicación) — en Release va directo por SOAP; en DEBUG la radicación de crédito va por la pasarela E10 |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_Insert"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_Insert` |
| Elemento raíz del response | `{http://tempuri.org/}Form_InsertResponse` |
| Método del proxy (.NET) | `Form_InsertAsync` |

**Propósito.** Crea un registro (radicado) en un formulario documental de WorkManager/DODO, opcionalmente con archivos y arrancando el flujo (`CheckWorkflow`).

**Quién la invoca en GA2.** `WorkManagerService.FormInsertAsyncCH` ← `GestorDocumentalBusinessLogic` (crédito), `GestorDocumentSolicitudes` (solicitudes: pago de cuota inicial `PCUOTAINI`, acreditación `ACREDV8`, reintegros `REINTG`…), `GestorDocumentalReintegrosBL`, `GestorDocumentarEmbargosBL`, `GestorDocumentalBusinessLogicFs` (postulación) y el manejador de Service Bus `WorkManagerInsertRequest` (E12).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `InsertRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_InsertResult` | `InsertResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

`Data` es una lista de pares `DictionaryDto {Field, Key, Value}`: `Field` es el código del campo del formulario (p. ej. `C0098`, `C0088`…). `FormCode` identifica el formulario (`02`, `83`, `133`, `219`, `361`, …). `Files[].Base64String` lleva el documento en Base64; `DocumentTypeCode`/`DirectoryCode` vienen de la parametrización.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_Insert xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:CheckWorkflow>true</a:CheckWorkflow>
        <a:Data>
          <a:DictionaryDto>
            <a:Field>Field</a:Field>
            <a:Key>Key</a:Key>
            <a:Value>Value</a:Value>
          </a:DictionaryDto>
        </a:Data>
        <a:Files>
          <a:FileDto>
            <a:Base64String>Base64String</a:Base64String>
            <a:Description>Description</a:Description>
            <a:DirectoryCode>DirectoryCode</a:DirectoryCode>
            <a:DocumentTypeCode>DocumentTypeCode</a:DocumentTypeCode>
            <a:Ext>Ext</a:Ext>
            <a:GUID>GUID</a:GUID>
          </a:FileDto>
        </a:Files>
        <a:FormCode>FormCode</a:FormCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:OfficeCode>OfficeCode</a:OfficeCode>
        <a:OperationUser>OperationUser</a:OperationUser>
        <a:ProcessId>1</a:ProcessId>
        <a:nextStepUser>nextStepUser</a:nextStepUser>
      </request>
    </Form_Insert>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; valores realistas):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_InsertResponse xmlns="http://tempuri.org/">
      <Form_InsertResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:BarCode>83-000123</d4p1:BarCode>
        <d4p1:FileCodes xmlns:d5p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
          <d5p1:string>FC-001</d5p1:string>
        </d4p1:FileCodes>
        <d4p1:Message>Registro creado</d4p1:Message>
        <d4p1:RecordId>4521</d4p1:RecordId>
        <d4p1:Success>true</d4p1:Success>
        <d4p1:workflowId>9981</d4p1:workflowId>
      </Form_InsertResult>
    </Form_InsertResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

- `Form_InsertResult.Success` (`bool`): si es `false`, GA2 marca la operación como **"El formulario se ha encolado para su procesamiento"** (originalmente publicaba un evento de reintento a Service Bus; hoy esa línea está comentada, así que **no hay reintento real**).
- `Form_InsertResult.BarCode` (`string`): **número de radicado**. GA2 lo guarda en la solicitud (`ActualizarRadicadoSolicitud`); si viene vacío o nulo guarda el texto `"RADICADO NO DISPONIBLE"`. Formato observado en el uso: `"<FormCode>-<consecutivo>"` (GA2 obtiene el `FormCode` con `numeroRadicado.Split("-")[0]` en `ObtenerEnlaceDodo`). **El mock debe devolver `BarCode` con ese formato**, p. ej. `"83-000123"`.
- `RecordId`, `workflowId`, `FileCodes[]`, `Message`: se devuelven al frontend y se registran, sin lógica adicional.

**Errores y casos alternos.**

Ver §2.3.

### 2. `Form_Update`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (en Release directo; en DEBUG la actualización de crédito va por la pasarela E10) |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_Update"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_Update` |
| Elemento raíz del response | `{http://tempuri.org/}Form_UpdateResponse` |
| Método del proxy (.NET) | `Form_UpdateAsync` |

**Propósito.** Actualiza campos de un registro existente, localizado mediante `FilterParameters`.

**Quién la invoca en GA2.** `WorkManagerService.FormUpdateAsyncCH` ← `GestorDocumentalBusinessLogic`, `GestorDocumentSolicitudes`, etc.; manejador Service Bus `WorkManagerUpdateRequest`.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `UpdateRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_UpdateResult` | `ResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

`FilterParameters` (`FilterParametersDto`) identifica el registro (p. ej. por el campo del radicado).

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_Update xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:Data>
          <a:DictionaryDto>
            <a:Field>Field</a:Field>
            <a:Key>Key</a:Key>
            <a:Value>Value</a:Value>
          </a:DictionaryDto>
        </a:Data>
        <a:FilterParameters>
          <a:Field>Field</a:Field>
          <a:Operator>Operator</a:Operator>
          <a:Value>Value</a:Value>
        </a:FilterParameters>
        <a:FormCode>FormCode</a:FormCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:OperationUser>OperationUser</a:OperationUser>
      </request>
    </Form_Update>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_UpdateResponse xmlns="http://tempuri.org/">
      <Form_UpdateResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Data>
          <d4p1:DictionaryDto>
            <d4p1:Field>Field</d4p1:Field>
            <d4p1:Key>Key</d4p1:Key>
            <d4p1:Value>Value</d4p1:Value>
          </d4p1:DictionaryDto>
        </d4p1:Data>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Rows>1</d4p1:Rows>
        <d4p1:Success>true</d4p1:Success>
      </Form_UpdateResult>
    </Form_UpdateResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

`Form_UpdateResult.Success` determina el mensaje ("El formulario fue actualizado" / "se ha encolado…"). `Data`/`Rows` se devuelven tal cual.

**Errores y casos alternos.**

Ver §2.3.

### 3. `Form_AttachFiles`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (la operación más invocada: ≥18 puntos) |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_AttachFiles"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_AttachFiles` |
| Elemento raíz del response | `{http://tempuri.org/}Form_AttachFilesResponse` |
| Método del proxy (.NET) | `Form_AttachFilesAsync` |

**Propósito.** Adjunta archivos (Base64) a un registro existente identificado por `BarCode` (radicado).

**Quién la invoca en GA2.** `WorkManagerService.FormAttachFilesAsyncCH` ← Crédito (≥6), Reportes (≥7: certificados y oficios generados), Solicitudes (3), Entidades; manejador Service Bus `WorkManagerAttachRequest`.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `AttachRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_AttachFilesResult` | `InsertResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

`AttachRequestDto {BarCode, CheckWorkflow, Files[], Header, OperationUser}`.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_AttachFiles xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:BarCode>BarCode</a:BarCode>
        <a:CheckWorkflow>true</a:CheckWorkflow>
        <a:Files>
          <a:FileDto>
            <a:Base64String>Base64String</a:Base64String>
            <a:Description>Description</a:Description>
            <a:DirectoryCode>DirectoryCode</a:DirectoryCode>
            <a:DocumentTypeCode>DocumentTypeCode</a:DocumentTypeCode>
            <a:Ext>Ext</a:Ext>
            <a:GUID>GUID</a:GUID>
          </a:FileDto>
        </a:Files>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:OperationUser>OperationUser</a:OperationUser>
      </request>
    </Form_AttachFiles>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_AttachFilesResponse xmlns="http://tempuri.org/">
      <Form_AttachFilesResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:BarCode>BarCode</d4p1:BarCode>
        <d4p1:FileCodes xmlns:d5p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
          <d5p1:string>FileCodes</d5p1:string>
        </d4p1:FileCodes>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:RecordId>1</d4p1:RecordId>
        <d4p1:Success>true</d4p1:Success>
        <d4p1:workflowId>1</d4p1:workflowId>
      </Form_AttachFilesResult>
    </Form_AttachFilesResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

`Form_AttachFilesResult.Success` → mensaje `"El fue adjuntado al formulario, {Message}"` o `"El archivo no fue adjuntado…"`. `FileCodes[]` se devuelve al frontend.

**Errores y casos alternos.**

Ver §2.3.

### 4. `Workflow_AttachFiles`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada** vía SOAP directo (`WorkFlowAttachFilesAsyncCH`) y por el manejador de Service Bus. En crédito (`GestorDocumentalBusinessLogic.AttachFiles`) se usa la **pasarela E10** (`api/workflowattachfiles`) |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Workflow_AttachFiles"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Workflow_AttachFiles` |
| Elemento raíz del response | `{http://tempuri.org/}Workflow_AttachFilesResponse` |
| Método del proxy (.NET) | `Workflow_AttachFilesAsync` |

**Propósito.** Adjunta archivos a una instancia de flujo (`WorkflowId`).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `WorkflowAttachFilesRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Workflow_AttachFilesResult` | `InsertResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_AttachFiles xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:Files>
          <a:FileDto>
            <a:Base64String>Base64String</a:Base64String>
            <a:Description>Description</a:Description>
            <a:DirectoryCode>DirectoryCode</a:DirectoryCode>
            <a:DocumentTypeCode>DocumentTypeCode</a:DocumentTypeCode>
            <a:Ext>Ext</a:Ext>
            <a:GUID>GUID</a:GUID>
          </a:FileDto>
        </a:Files>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:OperationUser>OperationUser</a:OperationUser>
        <a:WorkflowId>1</a:WorkflowId>
      </request>
    </Workflow_AttachFiles>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_AttachFilesResponse xmlns="http://tempuri.org/">
      <Workflow_AttachFilesResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:BarCode>BarCode</d4p1:BarCode>
        <d4p1:FileCodes xmlns:d5p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
          <d5p1:string>FileCodes</d5p1:string>
        </d4p1:FileCodes>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:RecordId>1</d4p1:RecordId>
        <d4p1:Success>true</d4p1:Success>
        <d4p1:workflowId>1</d4p1:workflowId>
      </Workflow_AttachFilesResult>
    </Workflow_AttachFilesResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

`Workflow_AttachFilesResult.Success` determina el mensaje ("Archivo adjuntado a la instancia" / "se ha encolado…").

**Errores y casos alternos.**

Ver §2.3.

### 5. `Form_GetParameters`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (`GestorDocumentSolicitudes.ObtenerEnlaceDodo`, endpoint `api/solicitud/ObtenerEnlaceDodo`) — sólo si el toggle `DODO.ESTADO = "true"` |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_GetParameters"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_GetParameters` |
| Elemento raíz del response | `{http://tempuri.org/}Form_GetParametersResponse` |
| Método del proxy (.NET) | `Form_GetParametersAsync` |

**Propósito.** Devuelve la definición del formulario (`Formulario`), usada para construir el enlace web al registro en DODO.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `FormParametersRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_GetParametersResult` | `FormParametersResponseDto` | — | `http://tempuri.org/` |

GA2 envía `FormCode` = prefijo del radicado (`numeroRadicado.Split("-")[0]`) y `Header` con `CHKeys:User/Token`. `FormId` no se envía (queda en `0`).

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetParameters xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:FormCode>FormCode</a:FormCode>
        <a:FormId>1</a:FormId>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
      </request>
    </Form_GetParameters>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetParametersResponse xmlns="http://tempuri.org/">
      <Form_GetParametersResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Form>
          <d4p1:ActivarNotificaciones>true</d4p1:ActivarNotificaciones>
          <d4p1:ActivarWorkflow>true</d4p1:ActivarWorkflow>
          <d4p1:ArchivoRequerido>true</d4p1:ArchivoRequerido>
          <d4p1:ArchivosWorkflow>1</d4p1:ArchivosWorkflow>
          <d4p1:Campos>
            <d4p1:Campo>
              <d4p1:Campo1>Campo1</d4p1:Campo1>
              <d4p1:Campo2>Campo2</d4p1:Campo2>
              <d4p1:CascadaDe>CascadaDe</d4p1:CascadaDe>
              <d4p1:Columna>Columna</d4p1:Columna>
              <d4p1:Control>1</d4p1:Control>
              <d4p1:Enlace>1</d4p1:Enlace>
              <d4p1:Estado>1</d4p1:Estado>
              <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
              <d4p1:Formulario i:nil="true" />
              <d4p1:GuardarUltimoValor>true</d4p1:GuardarUltimoValor>
              <d4p1:Host>Host</d4p1:Host>
              <d4p1:IdCampo>1</d4p1:IdCampo>
              <d4p1:IdEnlace>1</d4p1:IdEnlace>
              <d4p1:IdFormulario>1</d4p1:IdFormulario>
              <d4p1:Invisible>true</d4p1:Invisible>
              <d4p1:LongDato>1</d4p1:LongDato>
              <d4p1:LongDatoMin>1</d4p1:LongDatoMin>
              <d4p1:Mascara>Mascara</d4p1:Mascara>
              <d4p1:Mostrar>true</d4p1:Mostrar>
              <d4p1:Nombre>Nombre</d4p1:Nombre>
              <d4p1:Orden>1</d4p1:Orden>
              <d4p1:RecordarValor>true</d4p1:RecordarValor>
              <d4p1:Requerido>true</d4p1:Requerido>
              <d4p1:Sticker>true</d4p1:Sticker>
              <d4p1:TipoDato>1</d4p1:TipoDato>
              <d4p1:Unico>true</d4p1:Unico>
              <d4p1:Usuario>Usuario</d4p1:Usuario>
              <d4p1:ValoresFuturos>true</d4p1:ValoresFuturos>
              <d4p1:ValoresPositivos>true</d4p1:ValoresPositivos>
            </d4p1:Campo>
          </d4p1:Campos>
          <d4p1:Codigo>Codigo</d4p1:Codigo>
          <d4p1:ColumnaClave>ColumnaClave</d4p1:ColumnaClave>
          <d4p1:ControllerName>ControllerName</d4p1:ControllerName>
          <d4p1:Copias>
            <d4p1:Copia>
              <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
              <d4p1:Formulario i:nil="true" />
              <d4p1:Host>Host</d4p1:Host>
              <d4p1:ID>1</d4p1:ID>
              <d4p1:IdDirectorio>1</d4p1:IdDirectorio>
              <d4p1:IdFormulario>1</d4p1:IdFormulario>
              <d4p1:Usuario>Usuario</d4p1:Usuario>
            </d4p1:Copia>
          </d4p1:Copias>
          <d4p1:CreateAction>CreateAction</d4p1:CreateAction>
          <d4p1:Descripcion>Descripcion</d4p1:Descripcion>
          <d4p1:Directorios>
            <d4p1:Directorio>
              <d4p1:ArchivosFormulario>
                <d4p1:ArchivoFormulario>
                  <d4p1:Archivo i:nil="true" />
                  <d4p1:Directorios i:nil="true" />
                  <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
                  <d4p1:Host>Host</d4p1:Host>
                  <d4p1:ID>1</d4p1:ID>
                  <d4p1:IdArchivo>1</d4p1:IdArchivo>
                  <d4p1:IdDirectorio>1</d4p1:IdDirectorio>
                  <d4p1:IdFormulario>1</d4p1:IdFormulario>
                  <d4p1:IdRegistro>1</d4p1:IdRegistro>
                  <d4p1:IdWorkflow>1</d4p1:IdWorkflow>
                  <d4p1:Usuario>Usuario</d4p1:Usuario>
                </d4p1:ArchivoFormulario>
              </d4p1:ArchivosFormulario>
              <d4p1:Codigo>Codigo</d4p1:Codigo>
              <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
              <d4p1:Formulario i:nil="true" />
              <d4p1:Host>Host</d4p1:Host>
              <d4p1:IdDirectorio>1</d4p1:IdDirectorio>
              <d4p1:IdDirectorioPadre>1</d4p1:IdDirectorioPadre>
              <d4p1:IdDirectorioV10>1</d4p1:IdDirectorioV10>
              <d4p1:IdEstado>1</d4p1:IdEstado>
              <d4p1:IdFormulario>1</d4p1:IdFormulario>
              <d4p1:Nombre>Nombre</d4p1:Nombre>
              <d4p1:Tipo>1</d4p1:Tipo>
              <d4p1:Usuario>Usuario</d4p1:Usuario>
            </d4p1:Directorio>
          </d4p1:Directorios>
          <d4p1:EditAction>EditAction</d4p1:EditAction>
          <d4p1:Estado>1</d4p1:Estado>
          <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
          <d4p1:FirmarDocumentos>true</d4p1:FirmarDocumentos>
          <d4p1:Host>Host</d4p1:Host>
          <d4p1:IdFormulario>1</d4p1:IdFormulario>
          <d4p1:IdPadre>1</d4p1:IdPadre>
          <d4p1:Imagen>true</d4p1:Imagen>
          <d4p1:Indices>
            <d4p1:Indice>
              <d4p1:CamposIndice>
                <d4p1:CamposIndice>
                  <d4p1:Columna>Columna</d4p1:Columna>
                  <d4p1:Id>1</d4p1:Id>
                  <d4p1:IdIndice>1</d4p1:IdIndice>
                  <d4p1:Indices i:nil="true" />
                  <d4p1:NombreColumna>NombreColumna</d4p1:NombreColumna>
                  <d4p1:Tipo>Tipo</d4p1:Tipo>
                </d4p1:CamposIndice>
              </d4p1:CamposIndice>
              <d4p1:EsUnico>true</d4p1:EsUnico>
              <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
              <d4p1:Formularios i:nil="true" />
              <d4p1:Host>Host</d4p1:Host>
              <d4p1:IdFormulario>1</d4p1:IdFormulario>
              <d4p1:IdIndice>1</d4p1:IdIndice>
              <d4p1:Usuario>Usuario</d4p1:Usuario>
            </d4p1:Indice>
          </d4p1:Indices>
          <d4p1:LongRadicado>1</d4p1:LongRadicado>
          <d4p1:Nombre>Nombre</d4p1:Nombre>
          <d4p1:NombreCampoSticker>NombreCampoSticker</d4p1:NombreCampoSticker>
          <d4p1:NotificacionOrWorkflow>true</d4p1:NotificacionOrWorkflow>
          <d4p1:Personalizado>true</d4p1:Personalizado>
          <d4p1:RadicadoElectronico>true</d4p1:RadicadoElectronico>
          <d4p1:Tabla>Tabla</d4p1:Tabla>
          <d4p1:TipoSticker>1</d4p1:TipoSticker>
          <d4p1:Usuario>Usuario</d4p1:Usuario>
          <d4p1:WorkflowRequerido>true</d4p1:WorkflowRequerido>
          <d4p1:WorkflowUnico>true</d4p1:WorkflowUnico>
        </d4p1:Form>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Success>true</d4p1:Success>
      </Form_GetParametersResult>
    </Form_GetParametersResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

GA2 lee `Form_GetParametersResult.Form.NombreCampoSticker` y `Form_GetParametersResult.Form.IdFormulario` y arma `{SvcUtils:DodoUrlSvc}?columna={NombreCampoSticker}&valorColumna={radicado}&idFormulario={IdFormulario}`. **Si `Form` es nulo, se produce `NullReferenceException`.**

**Errores y casos alternos.**

Ver §2.3.

### 6. `Form_GetData`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Usada vía endpoint `POST api/credito/getdataforms` (`GetDataForm` → `FormGetDataAsyncCH`) |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_GetData"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_GetData` |
| Elemento raíz del response | `{http://tempuri.org/}Form_GetDataResponse` |
| Método del proxy (.NET) | `Form_GetDataAsync` |

**Propósito.** Consulta registros de un formulario aplicando `FilterParameters`.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `QueryRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_GetDataResult` | `ResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetData xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:FilterParameters>
          <a:Field>Field</a:Field>
          <a:Operator>Operator</a:Operator>
          <a:Value>Value</a:Value>
        </a:FilterParameters>
        <a:FormCode>FormCode</a:FormCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
      </request>
    </Form_GetData>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetDataResponse xmlns="http://tempuri.org/">
      <Form_GetDataResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Data>
          <d4p1:DictionaryDto>
            <d4p1:Field>Field</d4p1:Field>
            <d4p1:Key>Key</d4p1:Key>
            <d4p1:Value>Value</d4p1:Value>
          </d4p1:DictionaryDto>
        </d4p1:Data>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Rows>1</d4p1:Rows>
        <d4p1:Success>true</d4p1:Success>
      </Form_GetDataResult>
    </Form_GetDataResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

Mensaje según `Success && Rows != 0` ("El formulario fue obtenido" / "…el cliente no esta registrado!"). `Data` (pares campo-valor) se devuelve al frontend.

**Errores y casos alternos.**

Ver §2.3.

### 7. `Form_Delete`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Sólo desde el manejador de Service Bus `WorkManagerDeleteRequest` (E12) y `WorkManagerService.FormDeleteAsyncCH` (sin consumidores de negocio activos) |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_Delete"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_Delete` |
| Elemento raíz del response | `{http://tempuri.org/}Form_DeleteResponse` |
| Método del proxy (.NET) | `Form_DeleteAsync` |

**Propósito.** Elimina un registro.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `DeleteRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_DeleteResult` | `ResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_Delete xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:FilterParameters>
          <a:Field>Field</a:Field>
          <a:Operator>Operator</a:Operator>
          <a:Value>Value</a:Value>
        </a:FilterParameters>
        <a:FormCode>FormCode</a:FormCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:OperationUser>OperationUser</a:OperationUser>
      </request>
    </Form_Delete>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_DeleteResponse xmlns="http://tempuri.org/">
      <Form_DeleteResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Data>
          <d4p1:DictionaryDto>
            <d4p1:Field>Field</d4p1:Field>
            <d4p1:Key>Key</d4p1:Key>
            <d4p1:Value>Value</d4p1:Value>
          </d4p1:DictionaryDto>
        </d4p1:Data>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Rows>1</d4p1:Rows>
        <d4p1:Success>true</d4p1:Success>
      </Form_DeleteResult>
    </Form_DeleteResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

### 8. `Form_GetFileCodes`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Envuelta (`FormGetFileCodesAsyncCH`) sin consumidores de negocio activos |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_GetFileCodes"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_GetFileCodes` |
| Elemento raíz del response | `{http://tempuri.org/}Form_GetFileCodesResponse` |
| Método del proxy (.NET) | `Form_GetFileCodesAsync` |

**Propósito.** Obtiene los códigos de archivo de un registro.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `FileRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_GetFileCodesResult` | `FileResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetFileCodes xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:BarCode>BarCode</a:BarCode>
        <a:DirectoryCode>DirectoryCode</a:DirectoryCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
      </request>
    </Form_GetFileCodes>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetFileCodesResponse xmlns="http://tempuri.org/">
      <Form_GetFileCodesResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:FileProperties>
          <d4p1:FilePropertiesDto>
            <d4p1:Code>Code</d4p1:Code>
            <d4p1:Description>Description</d4p1:Description>
            <d4p1:Ext>Ext</d4p1:Ext>
            <d4p1:Size>1</d4p1:Size>
          </d4p1:FilePropertiesDto>
        </d4p1:FileProperties>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Success>true</d4p1:Success>
      </Form_GetFileCodesResult>
    </Form_GetFileCodesResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

### 9. `Form_GetBase64StringFile`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Envuelta (`FormGetBase64StringFileAsyncCH`) sin consumidores de negocio activos |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Form_GetBase64StringFile"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Form_GetBase64StringFile` |
| Elemento raíz del response | `{http://tempuri.org/}Form_GetBase64StringFileResponse` |
| Método del proxy (.NET) | `Form_GetBase64StringFileAsync` |

**Propósito.** Descarga un archivo como Base64.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `StringFileRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Form_GetBase64StringFileResult` | `StringFileResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetBase64StringFile xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:FileCode>FileCode</a:FileCode>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
      </request>
    </Form_GetBase64StringFile>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Form_GetBase64StringFileResponse xmlns="http://tempuri.org/">
      <Form_GetBase64StringFileResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:File>
          <d4p1:Code>Code</d4p1:Code>
          <d4p1:Description>Description</d4p1:Description>
          <d4p1:Ext>Ext</d4p1:Ext>
          <d4p1:Size>1</d4p1:Size>
        </d4p1:File>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Success>true</d4p1:Success>
      </Form_GetBase64StringFileResult>
    </Form_GetBase64StringFileResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

### 10. `Workflow_Start`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Envuelta (`WorkFlowStartAsyncCH`) sin consumidores de negocio activos |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Workflow_Start"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Workflow_Start` |
| Elemento raíz del response | `{http://tempuri.org/}Workflow_StartResponse` |
| Método del proxy (.NET) | `Workflow_StartAsync` |

**Propósito.** Inicia una instancia de flujo.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `StartWorkflowRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Workflow_StartResult` | `StartWorkflowResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_Start xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:BarCode>BarCode</a:BarCode>
        <a:Files>
          <a:FileDto>
            <a:Base64String>Base64String</a:Base64String>
            <a:Description>Description</a:Description>
            <a:DirectoryCode>DirectoryCode</a:DirectoryCode>
            <a:DocumentTypeCode>DocumentTypeCode</a:DocumentTypeCode>
            <a:Ext>Ext</a:Ext>
            <a:GUID>GUID</a:GUID>
          </a:FileDto>
        </a:Files>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:NextStepUser>NextStepUser</a:NextStepUser>
        <a:OperationUser>OperationUser</a:OperationUser>
        <a:ProcessId>1</a:ProcessId>
      </request>
    </Workflow_Start>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_StartResponse xmlns="http://tempuri.org/">
      <Workflow_StartResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:FileCodes xmlns:d5p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
          <d5p1:string>FileCodes</d5p1:string>
        </d4p1:FileCodes>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Success>true</d4p1:Success>
        <d4p1:WorkflowId>1</d4p1:WorkflowId>
      </Workflow_StartResult>
    </Workflow_StartResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

### 11. `Workflow_RunTask`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Envuelta (`WorkFlowRunAsyncCH`) sin consumidores de negocio activos |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/Workflow_RunTask"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}Workflow_RunTask` |
| Elemento raíz del response | `{http://tempuri.org/}Workflow_RunTaskResponse` |
| Método del proxy (.NET) | `Workflow_RunTaskAsync` |

**Propósito.** Ejecuta/cumple una tarea del flujo.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `RunTaskRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `Workflow_RunTaskResult` | `ResponseDto` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_RunTask xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:BarCode>BarCode</a:BarCode>
        <a:Comment>Comment</a:Comment>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:NextStepUser>NextStepUser</a:NextStepUser>
        <a:ProcessId>1</a:ProcessId>
        <a:Step>1</a:Step>
        <a:WorkflowId>1</a:WorkflowId>
      </request>
    </Workflow_RunTask>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <Workflow_RunTaskResponse xmlns="http://tempuri.org/">
      <Workflow_RunTaskResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Data>
          <d4p1:DictionaryDto>
            <d4p1:Field>Field</d4p1:Field>
            <d4p1:Key>Key</d4p1:Key>
            <d4p1:Value>Value</d4p1:Value>
          </d4p1:DictionaryDto>
        </d4p1:Data>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Rows>1</d4p1:Rows>
        <d4p1:Success>true</d4p1:Success>
      </Workflow_RunTaskResult>
    </Workflow_RunTaskResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

### 12. `List_GetItems`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Envuelta (`ListGetItemsAsyncCH`) sin consumidores de negocio activos |
| Transporte | `POST {SvcUtils:WorkManagerUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IWorkManagerServices/List_GetItems"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}List_GetItems` |
| Elemento raíz del response | `{http://tempuri.org/}List_GetItemsResponse` |
| Método del proxy (.NET) | `List_GetItemsAsync` |

**Propósito.** Obtiene los ítems de una lista parametrizable.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `request` | `ListRequestDto` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `List_GetItemsResult` | `ListResponseDtoOfItemListagpjPdjDr` | — | `http://tempuri.org/` |

`Header` (`HeaderDto {User, Token}`) **dentro del cuerpo**: GA2 lo llena con `CHKeys:User` y `CHKeys:Token` (credencial fija de la aplicación). No hay autenticación HTTP ni WS-Security. El mock puede aceptar cualquier valor o validar contra un par configurado.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <List_GetItems xmlns="http://tempuri.org/">
      <request xmlns:a="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:FatherItemId>1</a:FatherItemId>
        <a:Header>
          <a:Token>Token</a:Token>
          <a:User>User</a:User>
        </a:Header>
        <a:ListId>1</a:ListId>
      </request>
    </List_GetItems>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <List_GetItemsResponse xmlns="http://tempuri.org/">
      <List_GetItemsResult xmlns:d4p1="http://schemas.datacontract.org/2004/07/WorkManager.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d4p1:Data>
          <d4p1:ItemLista>
            <d4p1:Activo>true</d4p1:Activo>
            <d4p1:Codigo>Codigo</d4p1:Codigo>
            <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
            <d4p1:Host>Host</d4p1:Host>
            <d4p1:IdItem>1</d4p1:IdItem>
            <d4p1:IdItemPadre>1</d4p1:IdItemPadre>
            <d4p1:IdLista>1</d4p1:IdLista>
            <d4p1:Lista>
              <d4p1:Fecha>2026-01-15T00:00:00</d4p1:Fecha>
              <d4p1:Host>Host</d4p1:Host>
              <d4p1:IdLista>1</d4p1:IdLista>
              <d4p1:IdListaPadre>1</d4p1:IdListaPadre>
              <d4p1:IdListaV10>1</d4p1:IdListaV10>
              <d4p1:ItemsLista />
              <d4p1:Nombre>Nombre</d4p1:Nombre>
              <d4p1:Usuario>Usuario</d4p1:Usuario>
            </d4p1:Lista>
            <d4p1:Nombre>Nombre</d4p1:Nombre>
            <d4p1:Usuario>Usuario</d4p1:Usuario>
          </d4p1:ItemLista>
        </d4p1:Data>
        <d4p1:Message>Message</d4p1:Message>
        <d4p1:Success>true</d4p1:Success>
      </List_GetItemsResult>
    </List_GetItemsResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.3.

## 4. Operaciones del contrato no utilizadas por GA2

No hay código de GA2 que las invoque. El mock puede responder con SOAP Fault. Se listan por completitud (SOAPAction exacta):

| Operación | SOAPAction |
|---|---|
| `Form_GetFilesDirectories` | `"http://tempuri.org/IWorkManagerServices/Form_GetFilesDirectories"` |
| `ExternalUser_Insert` | `"http://tempuri.org/IWorkManagerServices/ExternalUser_Insert"` |
| `ExternalUser_GetByEmailID` | `"http://tempuri.org/IWorkManagerServices/ExternalUser_GetByEmailID"` |
| `Externaluser_GetByActivationCode` | `"http://tempuri.org/IWorkManagerServices/Externaluser_GetByActivationCode"` |
| `ExternalUser_VerifyAccount` | `"http://tempuri.org/IWorkManagerServices/ExternalUser_VerifyAccount"` |
| `ExternalUser_ChangePassword` | `"http://tempuri.org/IWorkManagerServices/ExternalUser_ChangePassword"` |
| `File_Delete` | `"http://tempuri.org/IWorkManagerServices/File_Delete"` |
| `Workflow_GetCases` | `"http://tempuri.org/IWorkManagerServices/Workflow_GetCases"` |
| `Workflow_GetTasks` | `"http://tempuri.org/IWorkManagerServices/Workflow_GetTasks"` |
| `Field_GetValues` | `"http://tempuri.org/IWorkManagerServices/Field_GetValues"` |
| `CC_GetService` | `"http://tempuri.org/IWorkManagerServices/CC_GetService"` |
| `CC_ProcessExist` | `"http://tempuri.org/IWorkManagerServices/CC_ProcessExist"` |
| `CC_InserProcess` | `"http://tempuri.org/IWorkManagerServices/CC_InserProcess"` |
| `CC_UpdateProcess` | `"http://tempuri.org/IWorkManagerServices/CC_UpdateProcess"` |
| `CC_ReadConfig` | `"http://tempuri.org/IWorkManagerServices/CC_ReadConfig"` |
| `CC_GetPendingProcesses` | `"http://tempuri.org/IWorkManagerServices/CC_GetPendingProcesses"` |
| `Workflow_CreateLink` | `"http://tempuri.org/IWorkManagerServices/Workflow_CreateLink"` |

## 5. Tipos de datos

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `ArchivoFormulario`

Tipo XML `ArchivoFormulario` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Archivo` | `Archivos` | opcional |  |
| 2 | `Directorios` | `Directorio` | opcional |  |
| 3 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 4 | `Host` | `xs:string` | opcional |  |
| 5 | `ID` | `xs:long` | siempre presente |  |
| 6 | `IdArchivo` | `xs:long` | siempre presente |  |
| 7 | `IdDirectorio` | `xs:long` (nillable) | opcional |  |
| 8 | `IdFormulario` | `xs:long` | siempre presente |  |
| 9 | `IdRegistro` | `xs:long` | siempre presente |  |
| 10 | `IdWorkflow` | `xs:long` (nillable) | opcional |  |
| 11 | `Usuario` | `xs:string` | opcional |  |

### `Archivos`

Tipo XML `Archivos` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Archivo` | `xs:string` | opcional |  |
| 2 | `ArchivosFormulario` | lista de `ArchivoFormulario` | opcional |  |
| 3 | `Bytes` | `xs:long` | siempre presente |  |
| 4 | `Descripcion` | `xs:string` | opcional |  |
| 5 | `Directorio` | `xs:string` | opcional |  |
| 6 | `Disco` | `xs:int` | siempre presente |  |
| 7 | `EsVersionActual` | `xs:boolean` | siempre presente |  |
| 8 | `Estado` | `xs:short` | siempre presente |  |
| 9 | `Etiquetas` | `xs:string` | opcional |  |
| 10 | `Extension` | `xs:string` | opcional |  |
| 11 | `FechaDocumento` | `xs:dateTime` | siempre presente |  |
| 12 | `FechaPublicacion` | `xs:dateTime` | siempre presente |  |
| 13 | `Host` | `xs:string` | opcional |  |
| 14 | `IdArchivo` | `xs:long` | siempre presente |  |
| 15 | `IdArchivoPrincipal` | `xs:long` | siempre presente |  |
| 16 | `IdDirectorioV10` | `xs:int` (nillable) | opcional |  |
| 17 | `IdFormularioV10` | `xs:long` (nillable) | opcional |  |
| 18 | `IdPrimeraVersion` | `xs:long` | siempre presente |  |
| 19 | `IdRegistroArchivoV10` | `xs:long` (nillable) | opcional |  |
| 20 | `IdRegistroV10` | `xs:long` (nillable) | opcional |  |
| 21 | `IdTipoDocumento` | `xs:long` | siempre presente |  |
| 22 | `Observacion` | `xs:string` | opcional |  |
| 23 | `RelacionArchivos` | lista de `RelacionArchivos` | opcional |  |
| 24 | `RutaV10` | `xs:string` | opcional |  |
| 25 | `TipoDocumento` | `TipoDocumento` | opcional |  |
| 26 | `UnidadAlmacenamiento` | `xs:string` | opcional |  |
| 27 | `UnidadConservacion` | `xs:string` | opcional |  |
| 28 | `Usuario` | `xs:string` | opcional |  |
| 29 | `Version` | `xs:string` | opcional |  |

### `AttachRequestDto`

Tipo XML `AttachRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BarCode` | `xs:string` | opcional |  |
| 2 | `CheckWorkflow` | `xs:boolean` | siempre presente |  |
| 3 | `Files` | lista de `FileDto` | opcional |  |
| 4 | `Header` | `HeaderDto` | opcional |  |
| 5 | `OperationUser` | `xs:string` | opcional |  |

### `AyudaAlmacenar`

Tipo XML `AyudaAlmacenar` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 2 | `Host` | `xs:string` | opcional |  |
| 3 | `ID` | `xs:long` | siempre presente |  |
| 4 | `IdDirectorio` | `xs:long` | siempre presente |  |
| 5 | `IdFormulario` | `xs:long` | siempre presente |  |
| 6 | `IdTipoDocumento` | `xs:long` | siempre presente |  |
| 7 | `TipoDocumento` | `TipoDocumento` | opcional |  |
| 8 | `Usuario` | `xs:string` | opcional |  |

### `Campo`

Tipo XML `Campo` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Campo1` | `xs:string` | opcional |  |
| 2 | `Campo2` | `xs:string` | opcional |  |
| 3 | `CascadaDe` | `xs:string` | opcional |  |
| 4 | `Columna` | `xs:string` | opcional |  |
| 5 | `Control` | `xs:short` | siempre presente |  |
| 6 | `Enlace` | `xs:short` (nillable) | opcional |  |
| 7 | `Estado` | `xs:short` | siempre presente |  |
| 8 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 9 | `Formulario` | `Formulario` | opcional |  |
| 10 | `GuardarUltimoValor` | `xs:boolean` (nillable) | opcional |  |
| 11 | `Host` | `xs:string` | opcional |  |
| 12 | `IdCampo` | `xs:long` | siempre presente |  |
| 13 | `IdEnlace` | `xs:long` (nillable) | opcional |  |
| 14 | `IdFormulario` | `xs:long` | siempre presente |  |
| 15 | `Invisible` | `xs:boolean` | siempre presente |  |
| 16 | `LongDato` | `xs:int` | siempre presente |  |
| 17 | `LongDatoMin` | `xs:int` | siempre presente |  |
| 18 | `Mascara` | `xs:string` | opcional |  |
| 19 | `Mostrar` | `xs:boolean` | siempre presente |  |
| 20 | `Nombre` | `xs:string` | opcional |  |
| 21 | `Orden` | `xs:int` | siempre presente |  |
| 22 | `RecordarValor` | `xs:boolean` | siempre presente |  |
| 23 | `Requerido` | `xs:boolean` | siempre presente |  |
| 24 | `Sticker` | `xs:boolean` | siempre presente |  |
| 25 | `TipoDato` | `xs:short` | siempre presente |  |
| 26 | `Unico` | `xs:boolean` | siempre presente |  |
| 27 | `Usuario` | `xs:string` | opcional |  |
| 28 | `ValoresFuturos` | `xs:boolean` | siempre presente |  |
| 29 | `ValoresPositivos` | `xs:boolean` | siempre presente |  |

### `CamposIndice`

Tipo XML `CamposIndice` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Columna` | `xs:string` | opcional |  |
| 2 | `Id` | `xs:int` | siempre presente |  |
| 3 | `IdIndice` | `xs:int` | siempre presente |  |
| 4 | `Indices` | `Indice` | opcional |  |
| 5 | `NombreColumna` | `xs:string` | opcional |  |
| 6 | `Tipo` | `xs:string` | opcional |  |

### `Copia`

Tipo XML `Copia` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 2 | `Formulario` | `Formulario` | opcional |  |
| 3 | `Host` | `xs:string` | opcional |  |
| 4 | `ID` | `xs:long` | siempre presente |  |
| 5 | `IdDirectorio` | `xs:long` | siempre presente |  |
| 6 | `IdFormulario` | `xs:long` | siempre presente |  |
| 7 | `Usuario` | `xs:string` | opcional |  |

### `DeleteRequestDto`

Tipo XML `DeleteRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FilterParameters` | `FilterParametersDto` | opcional |  |
| 2 | `FormCode` | `xs:string` | opcional |  |
| 3 | `Header` | `HeaderDto` | opcional |  |
| 4 | `OperationUser` | `xs:string` | opcional |  |

### `DictionaryDto`

Tipo XML `DictionaryDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Field` | `xs:string` | opcional |  |
| 2 | `Key` | `xs:string` | opcional |  |
| 3 | `Value` | `xs:string` | opcional |  |

### `Directorio`

Tipo XML `Directorio` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `ArchivosFormulario` | lista de `ArchivoFormulario` | opcional |  |
| 2 | `Codigo` | `xs:string` | opcional |  |
| 3 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 4 | `Formulario` | `Formulario` | opcional |  |
| 5 | `Host` | `xs:string` | opcional |  |
| 6 | `IdDirectorio` | `xs:long` | siempre presente |  |
| 7 | `IdDirectorioPadre` | `xs:long` | siempre presente |  |
| 8 | `IdDirectorioV10` | `xs:long` (nillable) | opcional |  |
| 9 | `IdEstado` | `xs:int` | siempre presente |  |
| 10 | `IdFormulario` | `xs:long` | siempre presente |  |
| 11 | `Nombre` | `xs:string` | opcional |  |
| 12 | `Tipo` | `xs:short` | siempre presente |  |
| 13 | `Usuario` | `xs:string` | opcional |  |

### `FileDto`

Tipo XML `FileDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Base64String` | `xs:string` | opcional |  |
| 2 | `Description` | `xs:string` | opcional |  |
| 3 | `DirectoryCode` | `xs:string` | opcional |  |
| 4 | `DocumentTypeCode` | `xs:string` | opcional |  |
| 5 | `Ext` | `xs:string` | opcional |  |
| 6 | `GUID` | `xs:string` | opcional |  |

### `FilePropertiesDto`

Tipo XML `FilePropertiesDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Code` | `xs:string` | opcional |  |
| 2 | `Description` | `xs:string` | opcional |  |
| 3 | `Ext` | `xs:string` | opcional |  |
| 4 | `Size` | `xs:long` | siempre presente |  |

### `FileRequestDto`

Tipo XML `FileRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BarCode` | `xs:string` | opcional |  |
| 2 | `DirectoryCode` | `xs:string` | opcional |  |
| 3 | `Header` | `HeaderDto` | opcional |  |

### `FileResponseDto`

Tipo XML `FileResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FileProperties` | lista de `FilePropertiesDto` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Success` | `xs:boolean` | siempre presente |  |

### `FilterParametersDto`

Tipo XML `FilterParametersDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Field` | `xs:string` | opcional |  |
| 2 | `Operator` | `xs:string` | opcional |  |
| 3 | `Value` | `xs:string` | opcional |  |

### `FormParametersRequestDto`

Tipo XML `FormParametersRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FormCode` | `xs:string` | opcional |  |
| 2 | `FormId` | `xs:long` | siempre presente |  |
| 3 | `Header` | `HeaderDto` | opcional |  |

### `FormParametersResponseDto`

Tipo XML `FormParametersResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Form` | `Formulario` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Success` | `xs:boolean` | siempre presente |  |

### `Formulario`

Tipo XML `Formulario` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `ActivarNotificaciones` | `xs:boolean` | siempre presente |  |
| 2 | `ActivarWorkflow` | `xs:boolean` | siempre presente |  |
| 3 | `ArchivoRequerido` | `xs:boolean` | siempre presente |  |
| 4 | `ArchivosWorkflow` | `xs:short` | siempre presente |  |
| 5 | `Campos` | lista de `Campo` | opcional |  |
| 6 | `Codigo` | `xs:string` | opcional |  |
| 7 | `ColumnaClave` | `xs:string` | opcional |  |
| 8 | `ControllerName` | `xs:string` | opcional |  |
| 9 | `Copias` | lista de `Copia` | opcional |  |
| 10 | `CreateAction` | `xs:string` | opcional |  |
| 11 | `Descripcion` | `xs:string` | opcional |  |
| 12 | `Directorios` | lista de `Directorio` | opcional |  |
| 13 | `EditAction` | `xs:string` | opcional |  |
| 14 | `Estado` | `xs:short` | siempre presente |  |
| 15 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 16 | `FirmarDocumentos` | `xs:boolean` | siempre presente |  |
| 17 | `Host` | `xs:string` | opcional |  |
| 18 | `IdFormulario` | `xs:long` | siempre presente |  |
| 19 | `IdPadre` | `xs:long` | siempre presente |  |
| 20 | `Imagen` | `xs:boolean` | siempre presente |  |
| 21 | `Indices` | lista de `Indice` | opcional |  |
| 22 | `LongRadicado` | `xs:short` | siempre presente |  |
| 23 | `Nombre` | `xs:string` | opcional |  |
| 24 | `NombreCampoSticker` | `xs:string` | opcional |  |
| 25 | `NotificacionOrWorkflow` | `xs:boolean` | siempre presente |  |
| 26 | `Personalizado` | `xs:boolean` | siempre presente |  |
| 27 | `RadicadoElectronico` | `xs:boolean` | siempre presente |  |
| 28 | `Tabla` | `xs:string` | opcional |  |
| 29 | `TipoSticker` | `xs:short` | siempre presente |  |
| 30 | `Usuario` | `xs:string` | opcional |  |
| 31 | `WorkflowRequerido` | `xs:boolean` | siempre presente |  |
| 32 | `WorkflowUnico` | `xs:boolean` | siempre presente |  |

### `HeaderDto`

Tipo XML `HeaderDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Token` | `xs:string` | opcional |  |
| 2 | `User` | `xs:string` | opcional |  |

### `Indice`

Tipo XML `Indice` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `CamposIndice` | lista de `CamposIndice` | opcional |  |
| 2 | `EsUnico` | `xs:boolean` | siempre presente |  |
| 3 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 4 | `Formularios` | `Formulario` | opcional |  |
| 5 | `Host` | `xs:string` | opcional |  |
| 6 | `IdFormulario` | `xs:long` | siempre presente |  |
| 7 | `IdIndice` | `xs:int` | siempre presente |  |
| 8 | `Usuario` | `xs:string` | opcional |  |

### `InsertRequestDto`

Tipo XML `InsertRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `CheckWorkflow` | `xs:boolean` | siempre presente |  |
| 2 | `Data` | lista de `DictionaryDto` | opcional |  |
| 3 | `Files` | lista de `FileDto` | opcional |  |
| 4 | `FormCode` | `xs:string` | opcional |  |
| 5 | `Header` | `HeaderDto` | opcional |  |
| 6 | `OfficeCode` | `xs:string` | opcional |  |
| 7 | `OperationUser` | `xs:string` | opcional |  |
| 8 | `ProcessId` | `xs:long` (nillable) | opcional |  |
| 9 | `nextStepUser` | `xs:string` | opcional |  |

### `InsertResponseDto`

Tipo XML `InsertResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BarCode` | `xs:string` | opcional |  |
| 2 | `FileCodes` | lista de `xs:string` | opcional |  |
| 3 | `Message` | `xs:string` | opcional |  |
| 4 | `RecordId` | `xs:long` | siempre presente |  |
| 5 | `Success` | `xs:boolean` | siempre presente |  |
| 6 | `workflowId` | `xs:long` | siempre presente |  |

### `ItemLista`

Tipo XML `ItemLista` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Activo` | `xs:boolean` | siempre presente |  |
| 2 | `Codigo` | `xs:string` | opcional |  |
| 3 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 4 | `Host` | `xs:string` | opcional |  |
| 5 | `IdItem` | `xs:long` | siempre presente |  |
| 6 | `IdItemPadre` | `xs:long` | siempre presente |  |
| 7 | `IdLista` | `xs:long` | siempre presente |  |
| 8 | `Lista` | `Lista` | opcional |  |
| 9 | `Nombre` | `xs:string` | opcional |  |
| 10 | `Usuario` | `xs:string` | opcional |  |

### `Lista`

Tipo XML `Lista` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 2 | `Host` | `xs:string` | opcional |  |
| 3 | `IdLista` | `xs:long` | siempre presente |  |
| 4 | `IdListaPadre` | `xs:long` | siempre presente |  |
| 5 | `IdListaV10` | `xs:long` (nillable) | opcional |  |
| 6 | `ItemsLista` | lista de `ItemLista` | opcional |  |
| 7 | `Nombre` | `xs:string` | opcional |  |
| 8 | `Usuario` | `xs:string` | opcional |  |

### `ListRequestDto`

Tipo XML `ListRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FatherItemId` | `xs:long` (nillable) | opcional |  |
| 2 | `Header` | `HeaderDto` | opcional |  |
| 3 | `ListId` | `xs:long` | siempre presente |  |

### `ListResponseDtoOfItemListagpjPdjDr`

Tipo XML `ListResponseDtoOfItemListagpjPdjDr` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Data` | lista de `ItemLista` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Success` | `xs:boolean` | siempre presente |  |

### `QueryRequestDto`

Tipo XML `QueryRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FilterParameters` | `FilterParametersDto` | opcional |  |
| 2 | `FormCode` | `xs:string` | opcional |  |
| 3 | `Header` | `HeaderDto` | opcional |  |

### `RelacionArchivos`

Tipo XML `RelacionArchivos` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Archivo` | `Archivos` | opcional |  |
| 2 | `ID` | `xs:long` | siempre presente |  |
| 3 | `IdArchivo` | `xs:long` | siempre presente |  |
| 4 | `IdRelacion` | `xs:long` | siempre presente |  |

### `ResponseDto`

Tipo XML `ResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Data` | lista de `DictionaryDto` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Rows` | `xs:int` | siempre presente |  |
| 4 | `Success` | `xs:boolean` | siempre presente |  |

### `RunTaskRequestDto`

Tipo XML `RunTaskRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BarCode` | `xs:string` | opcional |  |
| 2 | `Comment` | `xs:string` | opcional |  |
| 3 | `Header` | `HeaderDto` | opcional |  |
| 4 | `NextStepUser` | `xs:string` | opcional |  |
| 5 | `ProcessId` | `xs:int` (nillable) | opcional |  |
| 6 | `Step` | `xs:int` | siempre presente |  |
| 7 | `WorkflowId` | `xs:long` (nillable) | opcional |  |

### `StartWorkflowRequestDto`

Tipo XML `StartWorkflowRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BarCode` | `xs:string` | opcional |  |
| 2 | `Files` | lista de `FileDto` | opcional |  |
| 3 | `Header` | `HeaderDto` | opcional |  |
| 4 | `NextStepUser` | `xs:string` | opcional |  |
| 5 | `OperationUser` | `xs:string` | opcional |  |
| 6 | `ProcessId` | `xs:long` | siempre presente |  |

### `StartWorkflowResponseDto`

Tipo XML `StartWorkflowResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FileCodes` | lista de `xs:string` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Success` | `xs:boolean` | siempre presente |  |
| 4 | `WorkflowId` | `xs:long` | siempre presente |  |

### `StringFileRequestDto`

Tipo XML `StringFileRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `FileCode` | `xs:string` | opcional |  |
| 2 | `Header` | `HeaderDto` | opcional |  |

### `StringFileResponseDto`

Tipo XML `StringFileResponseDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `File` | `FilePropertiesDto` | opcional |  |
| 2 | `Message` | `xs:string` | opcional |  |
| 3 | `Success` | `xs:boolean` | siempre presente |  |

### `TipoDocumento`

Tipo XML `TipoDocumento` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Archivos` | lista de `Archivos` | opcional |  |
| 2 | `AyudasAlmacenar` | lista de `AyudaAlmacenar` | opcional |  |
| 3 | `Codigo` | `xs:string` | opcional |  |
| 4 | `ControlaVersion` | `xs:boolean` | siempre presente |  |
| 5 | `ControlaVigencia` | `xs:boolean` | siempre presente |  |
| 6 | `DiasVigencia` | `xs:int` | siempre presente |  |
| 7 | `Fecha` | `xs:dateTime` | siempre presente |  |
| 8 | `Host` | `xs:string` | opcional |  |
| 9 | `IdTipoDocumento` | `xs:long` | siempre presente |  |
| 10 | `Nombre` | `xs:string` | opcional |  |
| 11 | `TRD` | lista de `TRD` | opcional |  |
| 12 | `Usuario` | `xs:string` | opcional |  |

### `TRD`

Tipo XML `TRD` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Administrativo` | `xs:boolean` | siempre presente |  |
| 2 | `Autentico` | `xs:boolean` | siempre presente |  |
| 3 | `Cientifico` | `xs:boolean` | siempre presente |  |
| 4 | `ConservacionTotal` | `xs:boolean` | siempre presente |  |
| 5 | `Contable` | `xs:boolean` | siempre presente |  |
| 6 | `Cultural` | `xs:boolean` | siempre presente |  |
| 7 | `Desmaterializado` | `xs:boolean` | siempre presente |  |
| 8 | `Eliminacion` | `xs:boolean` | siempre presente |  |
| 9 | `EliminacionElectronica` | `xs:string` | opcional |  |
| 10 | `EstampadoCronologico` | `xs:boolean` | siempre presente |  |
| 11 | `EvidenciaNiif` | `xs:boolean` | siempre presente |  |
| 12 | `Facilitativo` | `xs:boolean` | siempre presente |  |
| 13 | `Facultativo` | `xs:boolean` | siempre presente |  |
| 14 | `FirmaBiometrica` | `xs:boolean` | siempre presente |  |
| 15 | `FirmaDigital` | `xs:boolean` | siempre presente |  |
| 16 | `Fiscal` | `xs:boolean` | siempre presente |  |
| 17 | `Fisico` | `xs:boolean` | siempre presente |  |
| 18 | `Funcional` | `xs:boolean` | siempre presente |  |
| 19 | `Historico` | `xs:boolean` | siempre presente |  |
| 20 | `ID` | `xs:long` | siempre presente |  |
| 21 | `IdFormulario` | `xs:long` | siempre presente |  |
| 22 | `IdRegistro` | `xs:long` | siempre presente |  |
| 23 | `IdTipoDocumental` | `xs:long` (nillable) | opcional |  |
| 24 | `Inmaterializado` | `xs:boolean` | siempre presente |  |
| 25 | `Integro` | `xs:boolean` | siempre presente |  |
| 26 | `Legal` | `xs:boolean` | siempre presente |  |
| 27 | `Ley1581` | `xs:boolean` | siempre presente |  |
| 28 | `Microfilmacion` | `xs:boolean` | siempre presente |  |
| 29 | `Misional` | `xs:boolean` | siempre presente |  |
| 30 | `NivelSeguridad` | `xs:string` | opcional |  |
| 31 | `Normatividad` | `xs:string` | opcional |  |
| 32 | `Observaciones` | `xs:string` | opcional |  |
| 33 | `PerteneceSGC` | `xs:boolean` | siempre presente |  |
| 34 | `PorcentajeSeleccion` | `xs:float` (nillable) | opcional |  |
| 35 | `Procedimiento` | `xs:string` | opcional |  |
| 36 | `RetencionAC` | `xs:float` (nillable) | opcional |  |
| 37 | `RetencionAG` | `xs:float` (nillable) | opcional |  |
| 38 | `RetencionElectronica` | `xs:float` (nillable) | opcional |  |
| 39 | `Seguridad` | `xs:string` | opcional |  |
| 40 | `Seleccion` | `xs:boolean` | siempre presente |  |
| 41 | `Simple` | `xs:boolean` | siempre presente |  |
| 42 | `Sustantivo` | `xs:boolean` | siempre presente |  |
| 43 | `TipoDocumento` | `TipoDocumento` | opcional |  |

### `UpdateRequestDto`

Tipo XML `UpdateRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Data` | lista de `DictionaryDto` | opcional |  |
| 2 | `FilterParameters` | `FilterParametersDto` | opcional |  |
| 3 | `FormCode` | `xs:string` | opcional |  |
| 4 | `Header` | `HeaderDto` | opcional |  |
| 5 | `OperationUser` | `xs:string` | opcional |  |

### `WorkflowAttachFilesRequestDto`

Tipo XML `WorkflowAttachFilesRequestDto` — namespace `http://schemas.datacontract.org/2004/07/WorkManager.Entities`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Files` | lista de `FileDto` | opcional |  |
| 2 | `Header` | `HeaderDto` | opcional |  |
| 3 | `OperationUser` | `xs:string` | opcional |  |
| 4 | `WorkflowId` | `xs:long` | siempre presente |  |


## 6. Escenarios mínimos recomendados

| # | Escenario | Respuesta del mock |
|---|---|---|
| 1 | Radicación exitosa | `Form_Insert` → `Success=true`, `BarCode="<FormCode>-<consecutivo>"`, `RecordId>0` |
| 2 | Radicación fallida | `Form_Insert` → `Success=false`, `Message="..."` (GA2 informa "encolado") |
| 3 | Adjuntos | `Form_AttachFiles` / `Workflow_AttachFiles` → `Success=true`, `FileCodes=["..."]` |
| 4 | Enlace DODO | `Form_GetParameters` → `Form.IdFormulario` y `Form.NombreCampoSticker` poblados |
| 5 | Falla técnica | SOAP Fault HTTP 500 |
| 6 | Resultado nulo | `Result` con `i:nil="true"` (reproduce el `NullReferenceException` de GA2) |
