# E12 · Azure Service Bus — emulador oficial + publicador del mock (AMQP, no HTTP)

**Especificación:** [docs/integraciones/mocks/E12-azure-service-bus.md](../../../docs/integraciones/mocks/E12-azure-service-bus.md).
**Código:** [`Providers/ServiceBus/ServiceBusMock.cs`](../../src/MockIntegrations.Api/Providers/ServiceBus/ServiceBusMock.cs) · emulador en [`docker-compose.yml`](../../docker-compose.yml) y [`docker/servicebus-emulator/Config.json`](../../docker/servicebus-emulator/Config.json).

GA2 **sólo consume** del topic `chonorintegraciones`, suscripción `dodosubscription`. AMQP no se puede simular con una API HTTP, así que:

1. **Broker:** el emulador oficial de Azure Service Bus (contenedor) con el topic, la suscripción y las 5 reglas de correlación (`Label` = nombre del evento) **precreadas**.
2. **Productor:** el mock publica los eventos con `POST /_mock/servicebus/publish/{subject}` (sin cuerpo publica el ejemplo de la ficha) y muestra ejemplos con `GET /_mock/servicebus/events/{subject}/sample`. Requiere `MockIntegrations:ServiceBus:ConnectionString`.

| Subject | Payload | Manejador en GA2 |
|---|---|---|
| `WorkManagerInsertRequest` | `{ InsertRequest: { request: InsertRequestDto }, Id, CreatedOn }` | `Form_InsertAsync` |
| `WorkManagerUpdateRequest` | `{ UpdateRequest: … }` | `Form_UpdateAsync` |
| `WorkManagerDeleteRequest` | `{ DeleteRequest: … }` | `Form_DeleteAsync` |
| `WorkManagerWorkflowAttachFilesRequest` | `{ WorkflowAttachFilesRequest: … }` | `Workflow_AttachFilesAsync` |
| `WorkManagerAttachRequest` | `{ AttachRequest: … }` | `Form_AttachFilesAsync` |

## Compatibilidad comprobada (emulador real, 2026-10-13)

| Cliente | Administración (borrar `$Default`, crear reglas) | Envío / recepción |
|---|---|---|
| `Azure.Messaging.ServiceBus` 7.20.1, cadena `Endpoint=sb://localhost;…;UseDevelopmentEmulator=true` | ✔ (con el puerto 5300 del emulador publicado también en el **80**) | ✔ |
| **7.9.0 (versión de GA2)** directo | ✘ (exige HTTPS 443) | ✘ (exige AMQPS 5671) |
| **7.9.0 (versión de GA2)** a través de `servicebus-tls-proxy` (perfil `ga2-sdk79`) y la CA local como confiable | ✔ | ✔ |

**GA2 arrancó** con el perfil `Mocks` contra el emulador a través del proxy TLS (suscripciones creadas, procesador iniciado). Procedimiento en la [guía de configuración §5](../guia-configuracion-ga2.md#5-azure-service-bus-emulador).

## Defectos de GA2 que impiden procesar los mensajes (verificados; no se corrigieron)

1. **Los manejadores no están registrados en DI.** `EventBusServiceBus.ProcessEvent` los resuelve con `ResolveOptional` y, como no están registrados, los salta y **completa el mensaje sin procesarlo**. Comprobado: con GA2 en ejecución, el evento publicado desaparece de la suscripción y WorkManager recibe 0 llamadas; con GA2 detenido, el mensaje permanece.
2. **El payload llega nulo:** `System.Text.Json` ignora el campo público `request` de los proxies WCF (prueba `Service_bus_sample_is_deserialized_like_EventBusServiceBus_and_shows_the_known_defect`).

En consecuencia, el canal Service Bus → WorkManager no puede verificarse funcionalmente hasta que se corrijan en GA2 (ver [pendientes](../pendientes.md)).
