# MockIntegrations — mocks de las integraciones de GA2

Servicio .NET 10 (ASP.NET Core) que simula a los proveedores externos de GA2, para poder ejecutar GA2 en pruebas sin depender de ellos. Reproduce el contrato que **GA2 consume** (rutas originales, SOAPAction, estructuras, códigos y errores), agrupado por proveedor y documentado en Swagger/OpenAPI.

| | |
|---|---|
| Proveedores | Seven ERP · WorkManager/DODO · BUA · Vigía · Fénix · Haberes · TransUnion · TusDatos · Dodo REST · Pasarela ktrionApi · SMTP · Azure Service Bus |
| Operaciones | 153 (34 de negocio, 53 pass-through y 66 del contrato sin uso en GA2, que responden de forma explícita) |
| Protocolos | SOAP 1.1 (document/literal, RPC/encoded, DataSet .NET, XML en texto), REST/JSON, SMTP, AMQP (emulador oficial) |
| Verificación | 209 pruebas con los proxies, DTO y clientes **reales** de GA2 · GA2 (.NET 5) arrancado con el perfil `Mocks`: 7/7 llamadas reales correctas |

## Inicio rápido

```bash
cd MockIntegrations
dotnet run --project src/MockIntegrations.Api     # http://localhost:5080/swagger · SMTP en :2525
dotnet test                                       # pruebas de compatibilidad
```

GA2 contra los mocks: `ASPNETCORE_ENVIRONMENT=Mocks` (perfil `GA2 (Mocks)`). Ver la [guía de configuración de GA2](docs/guia-configuracion-ga2.md); incluye Service Bus, las dependencias de arranque y las integraciones con URL fija.

Entorno completo con Docker (mock + emulador de Service Bus):

```bash
cp .env.example .env && docker compose up -d --build
```

## Estructura

```
MockIntegrations.sln          global.json (.NET 10; la raíz del repo fija .NET 5 para GA2)
src/MockIntegrations.Api/
  Program.cs                  composición: proveedores, OpenAPI, Swagger UI, registro de llamadas
  Configuration/              MockOptions (sección "MockIntegrations")
  Infrastructure/
    Soap/                     despachador SOAP por SOAPAction, faults, página de ayuda
    Rest/                     registro de endpoints REST simulados
    Scenarios/                selección determinista de escenarios, overrides, latencia
    Fixtures/                 fixtures embebidos + marcadores {{req:…}} {{seq:…}}
    Journal/                  registro de llamadas (/_mock/journal)
    OpenApi/                  un documento por proveedor; documentación de endpoints SOAP
    Catalog/ Admin/           catálogo de operaciones y API /_mock
  Providers/{Proveedor}/      definición de cada mock, modelos y Fixtures/
tests/MockIntegrations.Tests/ pruebas de contrato y compatibilidad con código de GA2 enlazado
docker/                       emulador de Service Bus, proxy TLS (SDK 7.9 de GA2), base stub y prueba de humo
ga2-config/                   variables de entorno equivalentes al perfil Mocks (bash / PowerShell)
tools/                        generación de fixtures, copia de DTO de GA2, matriz de cobertura
docs/                         documentación (índice en docs/README.md)
```

## Documentación

[Índice](docs/README.md) · [Inventario](docs/inventario.md) · [Proveedores](docs/providers/) · [Escenarios](docs/escenarios.md) · [Ejecución](docs/guia-ejecucion.md) · [Configurar GA2](docs/guia-configuracion-ga2.md) · [OpenAPI](docs/guia-openapi.md) · [Cobertura](docs/matriz-cobertura.md) · [Pendientes y riesgos](docs/pendientes.md)

Los contratos detallados por operación (request/response) están en [`../docs/integraciones/mocks/`](../docs/integraciones/mocks/README.md).
