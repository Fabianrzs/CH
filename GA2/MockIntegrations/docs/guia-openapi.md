# Guía de Swagger / OpenAPI

- **Swagger UI:** http://localhost:5080/swagger. El selector superior permite elegir "Todos los proveedores" o un proveedor concreto.
- **Documentos** (OpenAPI 3.1, generados en tiempo de ejecución a partir de los endpoints y modelos implementados con `Microsoft.AspNetCore.OpenApi`): `/openapi/all.json` y `/openapi/{seven|workmanager|bua|vigia|fenix|haberes|transunion|tusdatos|dodo|ktrionapi|smtp|servicebus|admin}.json`.
- Cada proveedor tiene su **etiqueta** (tag) y su documento. Los `operationId` son únicos y estables (`Seven_SPGOBLIG`, `Dodo_Form_Insert`, `KtrionApi_FormInsert`…).

## Operaciones SOAP

OpenAPI no puede representar varias operaciones en la misma ruta y verbo distinguidas sólo por el header `SOAPAction`. Por eso cada servicio `.asmx`/`.svc` se documenta **una vez**, como SOAP 1.1 sobre HTTP (no como REST):

- descripción con la tabla de operaciones: SOAPAction, uso en GA2, escenarios y resumen;
- parámetro de header `SOAPAction` obligatorio, con la **lista (enum) de acciones**;
- *request body* `text/xml` con **un ejemplo por operación**: el sobre capturado del proxy WCF real de GA2;
- respuestas `200` (un ejemplo por operación, escenario `default`; los valores dinámicos se muestran con valores ilustrativos), `400` y `500` (SOAP Fault).

Para probar desde Swagger UI: *Try it out* → elegir el ejemplo de la operación → seleccionar la misma acción en `SOAPAction` → *Execute*.

El esquema XML completo de cada operación (elementos, tipos, orden, ocurrencia) está en las fichas [docs/integraciones/mocks/E0*.md](../../docs/integraciones/mocks/README.md). El mock no publica WSDL porque GA2 usa proxies precompilados.

## Operaciones REST

Los esquemas salen de los **modelos del mock, copiados de los DTO de GA2** (Dodo, TusDatos) o declarados con los nombres exactos que serializa GA2 (pasarela ktrionApi: PascalCase, `request`, `workflowId`). Cada operación documenta parámetros de ruta y query, cuerpo, respuestas (200, 401, 404, 500, 501 según el caso), uso en GA2, clave de configuración, cliente de GA2 y escenarios.

## Header de escenario

Todas las operaciones admiten el header opcional `X-Mock-Scenario`, que **GA2 nunca envía**: sirve para forzar un escenario en pruebas manuales. Las respuestas incluyen `X-Mock-Provider`, `X-Mock-Operation`, `X-Mock-Scenario-Applied` y `X-Mock-Scenario-Source` (GA2 los ignora).

## Protocolos sin representación OpenAPI

- **SMTP** (TCP 2525): el documento `smtp` sólo describe la API HTTP de **inspección** del buzón. El protocolo está en [E11](providers/E11-smtp.md).
- **Service Bus** (AMQP): el documento `servicebus` describe la herramienta HTTP de **publicación** hacia el emulador. Los mensajes y el broker están en [E12](providers/E12-servicebus.md).

## Verificación

Las pruebas `CatalogAndOpenApiTests` comprueban, para los 14 documentos, que se generan; que cada operación tiene `operationId` único, `summary`, etiqueta y respuestas; que todas las operaciones del catálogo aparecen; que cada SOAPAction está en el enum con ejemplos de request y response; y que los esquemas REST usan los nombres que serializa GA2.
