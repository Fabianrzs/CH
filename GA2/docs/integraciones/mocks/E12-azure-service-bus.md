# E12 — Azure Service Bus (topic `chonorintegraciones`) — consumidor de eventos WorkManager

> Documento para simular el bus de eventos que consume la **API GA2**. GA2 **sólo consume**: no hay ninguna llamada activa a `IEventBus.Publish` (la publicación de reintentos en `WorkManagerService` está comentada). El productor de los mensajes está fuera del repositorio.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Tecnología | Azure Service Bus (AMQP), SDK `Azure.Messaging.ServiceBus` 7.9 |
| Conexión | `EventBusConnection` (namespace `cpvmpdsb1.servicebus.windows.net`, SAS `chonordirective`, `EntityPath=chonorintegraciones`) |
| Topic | `chonorintegraciones` (**fijo en el código**, `EventBusServiceBus.cs:21`) |
| Suscripción | `SubscriptionClientName` = `dodosubscription` |
| Procesador | `ServiceBusProcessor`, `MaxConcurrentCalls = 10`, `AutoCompleteMessages = false` |
| Registro | `Startup.ConfigureEventBus` (API GA2) se suscribe a 5 eventos |
| Autofac | Los manejadores se resuelven en un scope `AUTOFAC_SCOPE_NAME` |

## 2. Qué hace GA2 al arrancar (requisitos para el mock)

Al construir `EventBusServiceBus` (en `Startup.Configure`):

1. Crea `ServiceBusClient` y `ServiceBusAdministrationClient` con la cadena de conexión.
2. **Borra la regla `$Default`** de la suscripción (`DeleteRuleAsync`). Sólo tolera `ServiceBusException` con razón `MessagingEntityNotFound`.
3. Para cada evento, **crea una regla** `CorrelationRuleFilter { Subject = <evento> }` con el nombre del evento (tolera `ServiceBusException`, p. ej. si ya existe).
4. Arranca el procesador (`StartProcessingAsync`).

> **Sin conectividad o sin permisos de administración, la API GA2 no arranca**: la excepción del paso 2 (distinta de `MessagingEntityNotFound`) no se captura y `Configure` falla. Es el principal bloqueo para levantar GA2 de forma aislada.
>
> Opciones de mock: (a) un namespace de Service Bus de desarrollo propio; (b) el **emulador oficial de Azure Service Bus**: comprobar que soporta las operaciones de `ServiceBusAdministrationClient` (gestión de reglas), porque históricamente no las soportaba por SDK; (c) cambiar el registro de `IEventBus` por una implementación en memoria (requiere código, fuera del alcance de esta fase).

## 3. Contrato de mensajes

| Propiedad del mensaje | Valor esperado |
|---|---|
| `Subject` (Label) | Nombre del evento **sin** el sufijo `IntegrationEvent` (tabla 3.1) |
| `Body` | JSON UTF-8 |
| `MessageId` | Cualquiera |
| Deserialización | `System.Text.Json.JsonSerializer.Deserialize(body, tipoEvento)` con **opciones por defecto**: distingue mayúsculas (propiedades **PascalCase**) e **ignora campos públicos** |

### 3.1 Eventos suscritos

| `Subject` | Tipo .NET | Propiedad con el payload | Acción del manejador |
|---|---|---|---|
| `WorkManagerInsertRequest` | `WorkManagerInsertRequestIntegrationEvent` | `InsertRequest` (`Form_InsertRequest`) | `IConsumoWorkManager.Form_InsertAsync` → WorkManager (E2) |
| `WorkManagerUpdateRequest` | `WorkManagerUpdateRequestIntegrationEvent` | `UpdateRequest` (`Form_UpdateRequest`) | `Form_UpdateAsync` |
| `WorkManagerDeleteRequest` | `WorkManagerDeleteRequestIntegrationEvent` | `DeleteRequest` (`Form_DeleteRequest`) | `Form_DeleteAsync` |
| `WorkManagerWorkflowAttachFilesRequest` | `WorkManagerWorkflowAttachFilesRequestIntegrationEvent` | `WorkflowAttachFilesRequest` | `Workflow_AttachFilesAsync` |
| `WorkManagerAttachRequest` | `WorkManagerAttachRequestIntegrationEvent` | `AttachRequest` (`Form_AttachFilesRequest`) | `Form_AttachFilesAsync` |

Todos heredan de `IntegrationEvent { Id: Guid, CreatedOn: DateTime }`.

### 3.2 Defecto verificado: el payload nunca llega

Los tipos `Form_*Request` son proxies WCF que guardan el contenido en un **campo público** `request` (no una propiedad). `System.Text.Json` con opciones por defecto **ignora los campos**, así que, **sea cual sea el JSON recibido**, `InsertRequest.request` queda en `null`. Se comprobó deserializando un JSON que sí incluía `request`:

| Evento | Resultado tras deserializar |
|---|---|
| `WorkManagerInsertRequest` | <campo request = null> |
| `WorkManagerUpdateRequest` | <campo request = null> |
| `WorkManagerDeleteRequest` | <campo request = null> |
| `WorkManagerWorkflowAttachFilesRequest` | <campo request = null> |
| `WorkManagerAttachRequest` | <campo request = null> |

Consecuencias:
- El manejador llama a WorkManager con `<request i:nil="true"/>` (o vacío).
- `WorkManagerInsertRequest` además lee `result.Form_InsertResult.Success`: si el mock de WorkManager devuelve un resultado nulo, salta un `NullReferenceException`.
- **Es imposible que este canal cree o actualice registros con datos reales.**

Tampoco el `Publish` de GA2 serviría como productor de referencia, porque serializa con las mismas opciones y emite `"InsertRequest":{}`:

```json
{"InsertRequest":{},"Id":"a28b1c62-5827-4540-98aa-88cfc4ae86e2","CreatedOn":"2026-10-09T18:49:31.3170272Z"}
```

### 3.3 Mensaje de ejemplo para el mock (formato esperado por diseño)

Aunque hoy el contenido de `request` se pierda, este es el formato que corresponde a la intención del código (PascalCase en el evento; dentro de `request`, nombres de los `DataMember`):

```json
{
  "InsertRequest": {
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
  },
  "Id": "a28b1c62-5827-4540-98aa-88cfc4ae86e2",
  "CreatedOn": "2026-10-09T18:49:31.3170272Z"
}
```

Propiedades del mensaje: `Subject = "WorkManagerInsertRequest"`, `ContentType = "application/json"` (opcional; GA2 no lo lee).

## 4. Semántica de procesamiento

| Situación | Efecto |
|---|---|
| `Subject` sin suscripción registrada | El mensaje se **completa** (se descarta) |
| El manejador termina sin excepción | Se completa (`CompleteMessageAsync`) |
| El manejador lanza una excepción (p. ej. Fault de WorkManager, `NullReferenceException`) | **No se completa**: al vencer el lock se reentrega; tras `MaxDeliveryCount` (configuración del topic) va a la **dead-letter queue**. El error se registra en `ErrorHandler` |
| Concurrencia | Hasta 10 mensajes en paralelo |

## 5. Escenarios mínimos recomendados

| # | Escenario | Acción en el mock |
|---|---|---|
| 1 | Arranque | Topic y suscripción existentes; permitir borrar `$Default` y crear 5 reglas |
| 2 | Mensaje de inserción | Publicar el JSON de §3.3 con `Subject=WorkManagerInsertRequest`; verificar que GA2 llama a WorkManager (con `request` nulo, ver §3.2) |
| 3 | Error del manejador | El mock de WorkManager devuelve Fault → comprobar reintentos y dead-letter |
| 4 | Subject desconocido | Se completa sin efecto |
