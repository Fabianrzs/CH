# Guía de ejecución de MockIntegrations

## Requisitos

- SDK de **.NET 10** (probado con 10.0.401, runtime 10.0.12). `MockIntegrations/global.json` fija la versión: la raíz del repositorio fija .NET 5 para GA2 y **no** aplica dentro de `MockIntegrations/`.
- Opcional: Docker (imagen del mock, emulador de Service Bus).

## Ejecutar en local

```bash
cd MockIntegrations
dotnet run --project src/MockIntegrations.Api            # http://localhost:5080 (perfil "MockIntegrations")
# o con HTTPS además (certificado de desarrollo de .NET):
dotnet run --project src/MockIntegrations.Api --launch-profile "MockIntegrations (HTTP + HTTPS)"
```

| URL | Contenido |
|---|---|
| http://localhost:5080/swagger | Swagger UI (un documento por proveedor + "Todos") |
| http://localhost:5080/openapi/{proveedor}.json | OpenAPI 3.1 (`all`, `seven`, `workmanager`, `bua`, `vigia`, `fenix`, `haberes`, `transunion`, `tusdatos`, `dodo`, `ktrionapi`, `smtp`, `servicebus`, `admin`) |
| http://localhost:5080/_mock/catalog | Inventario de operaciones simuladas |
| http://localhost:5080/_mock/scenarios | Escenarios disponibles y activos |
| http://localhost:5080/_mock/journal | Últimas llamadas recibidas |
| http://localhost:5080/Seven/WsCdp/WPgOblig.asmx (GET) | Página de ayuda de cada servicio SOAP |
| smtp://localhost:2525 | Servidor SMTP de captura |

## Con Docker Compose

```bash
cd MockIntegrations
cp .env.example .env                       # contraseña del SQL del emulador (archivo no versionado)
docker compose up -d --build               # mock + emulador de Service Bus + SQL
docker compose --profile ga2-sdk79 up -d   # + proxy TLS para el SDK 7.9 de GA2 (requiere ./docker/tls-proxy/generate-certs.sh)
docker compose logs -f mockintegrations
docker compose down
```

## Configuración del mock (`appsettings.json`, sección `MockIntegrations`)

| Clave | Por defecto | Uso |
|---|---|---|
| `Urls` | `http://0.0.0.0:5080` | Dirección HTTP |
| `FixturesOverridePath` | — | Carpeta con fixtures que reemplazan a los embebidos (`{Proveedor}/Fixtures/{Op}.{escenario}.xml`), sin recompilar |
| `SlowScenarioDelayMs` | 20000 | Retardo del escenario `slow` |
| `LatencyMs:{Proveedor}` | — | Latencia fija por proveedor |
| `Scenarios:Defaults:{Proveedor}:{Op}` | — | Escenario por defecto (`*` = todas las operaciones) |
| `Scenarios:Rules[]` | reglas por documento | Escenario según un dato del request ([escenarios](escenarios.md)) |
| `Journal:Capacity` / `MaxBodyChars` | 500 / 65536 | Registro de llamadas |
| `Smtp:Port` / `Enabled` / `RejectRecipients` | 2525 / true / `rechazado@mock.test` | SMTP |
| `ServiceBus:ConnectionString` / `Topic` | — / `chonorintegraciones` | Publicación de eventos en el emulador |
| `TusDatos:ApiKey`, `PollsUntilFinished`, `PendingStatus`, `FinishedStatus` | —, 1, `pendiente`, `finalizado` | Gateway TusDatos |
| `KtrionApi:RequireIssuedToken` | true | Exige el Bearer emitido por `api/autenticacion` |
| `TransUnion:ValidPin`, `FirstApplicationId` | `123456`, 9876543 | TransUnion |

Todas las claves admiten variables de entorno (`MockIntegrations__Smtp__Port=25`).

## Pruebas automatizadas

```bash
cd MockIntegrations
dotnet test                                  # 209 pruebas
```

Las pruebas arrancan el mock en Kestrel (puerto libre) y lo consumen con **el código real de GA2**, enlazado desde `../src` (no copiado): los proxies WCF (`Reference.cs`), los DTO REST, `Response<T>`, el cliente de TransUnion (`ValidacionesTU`), `Serialization` y los eventos de Service Bus. Si GA2 cambia un contrato, las pruebas fallan. Detalle en la [matriz de cobertura](matriz-cobertura.md).

## Herramientas de mantenimiento

| Script | Uso |
|---|---|
| `tools/build-fixtures.py` | Regenera los fixtures SOAP a partir de las fichas `docs/integraciones/mocks/E0*.md`, con la curación documentada en el propio script |
| `tools/copy-ga2-models.py` | Vuelve a copiar los DTO REST de GA2 (Dodo, TusDatos) |
| `tools/generate-coverage.py` | Regenera `docs/matriz-cobertura.md` desde `/_mock/catalog` |

## Añadir una operación o un escenario

- **Escenario SOAP nuevo:** añada `Providers/{Proveedor}/Fixtures/{Operación}.{escenario}.xml`. Se descubre solo (aparece en `/_mock/scenarios` y en OpenAPI). Marcadores disponibles: `{{req:Campo|defecto}}`, `{{seq:nombre:inicio:formato}}`, `{{now:formato}}`, `{{guid}}`.
- **Operación SOAP nueva:** añádala a la definición `SoapService` del proveedor (`SoapAction`, uso, resumen) y su fixture `.default.xml`. Para lógica propia, use `Handler` (ver `BuaMock` o `TransUnionMock`).
- **Operación REST nueva:** `app.MapMock(...)` con un `RestOperationSpec` y los modelos del proveedor.
