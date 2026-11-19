# Guía de configuración de GA2 para usar MockIntegrations

Cómo ejecutar GA2 en modo de pruebas contra los mocks **sin modificar el código fuente**: sólo con configuración. Todo lo indicado en §1–§5 se comprobó el 2026-10-13 ejecutando la API GA2 (.NET 5) contra el mock y el emulador (ver §8).

## 1. Cómo obtiene GA2 sus URLs

| Mecanismo de GA2 | Integraciones | Dónde |
|---|---|---|
| `SvcUtils:*` → `ConfiguracionesSvc` (IOptions) | Seven, WorkManager, BUA ws.02, Vigía, Fénix, Haberes | `appsettings*.json` de las API GA2 y BUA |
| `Integrationes:{TusDatos,Dodo}:BaseUrl` (`HttpClient` tipado) | TusDatos, Dodo REST | API GA2 |
| `CHKeys:UrlCajaHonor` | Pasarela ktrionApi | API GA2 y BUA |
| `SMTP:*` | Correo de notificaciones | API GA2 |
| `EventBusConnection`, `SubscriptionClientName` | Azure Service Bus | API GA2 |
| **Fijo en el código** | BUA ws.01, TransUnion, SMTP de Outlook | Ver §6 |

GA2 usa `Host.CreateDefaultBuilder`, que carga `appsettings.json`, después `appsettings.{ASPNETCORE_ENVIRONMENT}.json` y por último las **variables de entorno** (`Seccion__Clave`). Con eso basta para redirigir las integraciones.

## 2. Archivos agregados a GA2 (sólo configuración)

| Archivo | Contenido |
|---|---|
| [`src/GA2.Apis/GA2/appsettings.Mocks.json`](../../src/GA2.Apis/GA2/appsettings.Mocks.json) | URLs de los mocks (`http://localhost:5080`, SMTP `localhost:2525`), cadena del emulador de Service Bus y **credenciales de integración ficticias** (`AppMain`, `CHKeys:User/Token`, `IdVision`, `Aqm`, `TusDatos:ApiKey`), para que las reales nunca viajen al mock |
| [`src/GA2.Apis/BUA/appsettings.Mocks.json`](../../src/GA2.Apis/BUA/appsettings.Mocks.json) | Lo mismo para la API BUA |
| `GA2.csproj` y `BUA.csproj` | Un `<None Include="appsettings.Mocks.json" CopyToOutputDirectory="PreserveNewest" CopyToPublishDirectory="Never" />`. Los proyectos excluyen `appsettings.*.json`; sin esta línea el archivo no llega a `bin/`. **No se publica** |
| `Properties/launchSettings.json` (GA2, BUA) | Perfiles `GA2 (Mocks)` y `BUA (Mocks)` con `ASPNETCORE_ENVIRONMENT=Mocks` |

La API **Identity** no necesita cambios: no tiene integraciones configurables (su SMTP de Outlook está fijo en el código y `ProviderRequestG2` no se usa).

`appsettings.Mocks.json` **no cambia** la cadena de conexión a la base de datos ni `JwtOptions`: son propios de cada entorno de pruebas (ver §4).

## 3. Variables de entorno y puertos

| Variable / clave | Valor con el mock local |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Mocks` |
| `SvcUtils__SevenWSMovContable` … `SvcUtils__SevenWSAportes` | `http://localhost:5080/Seven/...` (ruta original del servicio) |
| `SvcUtils__WorkManagerUrlSvc` | `http://localhost:5080/APIV2_0005/WorkManagerService.svc` |
| `SvcUtils__BuaUrlServiceApi` | `http://localhost:5080/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc` |
| `SvcUtils__VigiaVerificarTerceroUrlSvc` | `http://localhost:5080/WebSvc/SOAP_VerificaTercero.php` |
| `SvcUtils__FenixServiceSvc` | `http://localhost:5080/WsFenixBiometria/WebServicesBiometrias.asmx?WSDL` |
| `SvcUtils__HaberesUrlSvc` | `http://localhost:5080/MCDIntegrationServices/wsModeloCanonico.asmx` |
| `Integrationes__TusDatos__BaseUrl` / `__ApiKey` | `http://localhost:5080/kiosko-dev/` / `mock-api-key` |
| `Integrationes__Dodo__BaseUrl` | `http://localhost:5080/dodo/` |
| `CHKeys__UrlCajaHonor` | `http://localhost:5080/REST/KriterionDev/ktrionApi/` |
| `SMTP__Host` / `SMTP__Puerto` / `SMTP__HabilitarSsl` | `localhost` / `2525` / `false` |
| `EventBusConnection` | `Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;EntityPath=chonorintegraciones` (clave pública y documentada del emulador, no es un secreto) |

Si el mock corre en otro equipo, use los scripts que generan **las mismas claves como variables de entorno** a partir de `MOCK_BASE_URL`:

```bash
MOCK_BASE_URL=http://10.0.0.5:5080 MOCK_SMTP_HOST=10.0.0.5 source MockIntegrations/ga2-config/ga2-mocks.env.sh
dotnet run --project src/GA2.Apis/GA2
```
```powershell
$env:MOCK_BASE_URL = "http://10.0.0.5:5080"; . .\MockIntegrations\ga2-config\ga2-mocks.env.ps1
dotnet run --project src\GA2.Apis\GA2
```

| Servicio | Puerto |
|---|---|
| MockIntegrations (SOAP, REST, Swagger) | 5080 |
| SMTP de captura | 2525 |
| Emulador Service Bus: AMQP / administración | 5672 / 5300 (+ 80 → 5300) |
| Proxy TLS para el SDK 7.9 de GA2 (perfil `ga2-sdk79`) | 443, 5671 |
| SQL Server del emulador (y base stub de la prueba de humo) | 127.0.0.1:14333 |

## 4. Dependencias de arranque de GA2

En el orden en que GA2 las usa al arrancar (`Startup.Configure`):

1. **Azure Service Bus.** GA2 borra la regla `$Default` y crea 5 reglas. Sin broker, **la API no arranca** (`DependencyResolutionException` al activar `IEventBus`). Ver §5.
2. **Base de datos.** `HangfireJobsInitializer` ejecuta `dbo.ObtenerParametrizacionHangFire`. Sin SQL Server, **la API no arranca**. Use una copia de la base de pruebas (`ConnectionStrings__connectionName`). El repositorio no incluye el esquema de GA2.
3. **Interruptores de integración en BD** (`dbo.ObtenerParametrosIntegraciones`): `VIGIA`, `FENIX`, `DODO`, `TusDatos`, `Bua` deben estar en `true` para que GA2 llame a los mocks; con `false` GA2 no llama (usa su respuesta alternativa).
4. **Plantillas de correo** en `CarpetaBlogStorage:CarpetaServidorStorage\template-email` y almacenamiento Blob (`ProviderStorage`) para los flujos que guardan PDF.

## 5. Azure Service Bus (emulador)

GA2 usa `Azure.Messaging.ServiceBus` **7.9.0**, anterior al soporte del emulador (`UseDevelopmentEmulator`, desde 7.18): exige **HTTPS en 443** para administrar reglas y **AMQPS en 5671**. El emulador sólo ofrece HTTP (5300) y AMQP sin TLS (5672). Hay dos formas de conectarlo, **ambas comprobadas**:

**A. Sin tocar GA2 (verificado arrancando GA2):** proxy TLS local con una CA de pruebas.

```bash
cd MockIntegrations
cp .env.example .env                       # contraseña del SQL del emulador
./docker/tls-proxy/generate-certs.sh       # CA local + certificado para localhost
docker compose --profile ga2-sdk79 up -d   # mock + emulador + SQL + proxy TLS
# GA2 debe confiar en la CA (Linux/contenedor .NET):
export SSL_CERT_FILE=$PWD/docker/tls-proxy/certs/ga2-mocks-ca.crt
# Windows: importar certs/ga2-mocks-ca.crt en "Entidades de certificación raíz de confianza" del equipo de pruebas.
```

**B. Actualizando el paquete** `Azure.Messaging.ServiceBus` de `GA2.BuildingBlocks/EventBusServiceBus` a ≥ 7.18 (cambio de dependencia, **no aplicado**): la cadena del emulador funciona directamente (se comprobó con 7.20.1 la misma secuencia de arranque de GA2: `DeleteRule` → `MessagingEntityNotFound` y `CreateRule` → `MessagingEntityAlreadyExists`, ambos tolerados por GA2), siempre que el puerto 5300 del emulador esté publicado también en el 80, como en el `docker-compose.yml`.

Publicar un evento para GA2: `POST http://localhost:5080/_mock/servicebus/publish/WorkManagerInsertRequest`. **Atención:** GA2 hoy descarta los mensajes porque sus manejadores no están registrados en DI ([E12](providers/E12-servicebus.md)).

> El emulador depende de su SQL Server. Si se recrea el contenedor SQL con el emulador en marcha, la API de reglas deja de responder: recree también el emulador (`docker compose up -d --force-recreate servicebus-emulator`).

## 6. Integraciones que no se pueden redirigir por configuración

| Integración | Destino fijo en el código | Opción sin cambiar código | Estado |
|---|---|---|---|
| BUA ws.01 (`ConsumoBua`: Cartera, Verificación de derechos, Cuentas) | `http://cpvmp-09x/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc` | Resolver `cpvmp-09x` hacia el equipo del mock (hosts/DNS) y exponer el mock en el puerto 80 (p. ej. `Urls=http://0.0.0.0:80`) | Contrato verificado con el proxy ws.01; redirección **no ejecutada** |
| TransUnion (`ValidacionesTU`) | `https://www.transuniondecisioncentre.com.mx/...` (**producción**) | Resolver el host hacia el mock y servir HTTPS con un certificado para ese nombre emitido por una CA de pruebas confiable | Contrato verificado con el cliente real de GA2 (redirigido por reflexión en las pruebas); redirección **no ejecutada** |
| SMTP de Identity (`RecuperaContrasena`) y `SmtpTestController` | `smtp-mail.outlook.com:587` (STARTTLS) / `cpvmp-01v5:25` | DNS hacia un SMTP con STARTTLS / hacia el mock en el puerto 25 | **No simulado** (el mock no implementa STARTTLS) |

> **Riesgo:** mientras la URL de TransUnion siga fija, cualquier ejecución de GA2 sin esa redirección llama a **TransUnion producción** con datos reales. Se recomienda hacerla configurable (cambio de código fuera del alcance).

## 7. Alternar entre proveedores reales y mocks

| Modo | Cómo |
|---|---|
| Mocks | `ASPNETCORE_ENVIRONMENT=Mocks` (perfil `GA2 (Mocks)` de Visual Studio / `dotnet run --launch-profile "GA2 (Mocks)"`), o las variables de §3 |
| Real | `ASPNETCORE_ENVIRONMENT=Development` / `Desarrollo` (sin cambios respecto a hoy) |
| Mixto | Perfil `Mocks` + variables de entorno con la URL real de las integraciones que se quieran usar de verdad (las variables tienen prioridad) |

Para apagar una integración concreta sin cambiar de modo siguen existiendo los interruptores de BD de GA2 (§4.3).

## 8. Iniciar GA2 y MockIntegrations en conjunto (procedimiento verificado)

```bash
cd MockIntegrations
cp .env.example .env && ./docker/tls-proxy/generate-certs.sh
docker compose --profile ga2-sdk79 up -d --build          # 1. mocks + Service Bus
curl http://localhost:5080/health                          #    {"status":"ok"}

cd ..                                                       # 2. GA2 (necesita su base de pruebas)
export ASPNETCORE_ENVIRONMENT=Mocks
export SSL_CERT_FILE=$PWD/MockIntegrations/docker/tls-proxy/certs/ga2-mocks-ca.crt
export ConnectionStrings__connectionName="Server=<sql de pruebas>;Database=<copia GA2>;User Id=...;Password=..."
dotnet run --project src/GA2.Apis/GA2 --urls http://localhost:5102

curl http://localhost:5080/_mock/journal                   # 3. qué llamó GA2 a cada mock
```

**Sin base de pruebas de GA2** puede comprobarse el cableado con la base *stub* de [`docker/ga2-smoke`](../docker/ga2-smoke/stub-database.sql). Sólo contiene los procedimientos de arranque, los interruptores y la auditoría; no es la base de GA2:

```bash
PW=$(grep MSSQL_SA_PASSWORD MockIntegrations/.env | cut -d= -f2)
docker compose -f MockIntegrations/docker-compose.yml exec -T servicebus-sql \
  /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$PW" -i /dev/stdin < MockIntegrations/docker/ga2-smoke/stub-database.sql
export ConnectionStrings__connectionName="Server=127.0.0.1,14333;Database=GA2_MOCKS_SMOKE;User Id=sa;Password=$PW"
export JwtOptions__Key=$(openssl rand -hex 32)              # clave de PRUEBA para firmar el token del humo
dotnet run --project src/GA2.Apis/GA2 --urls http://127.0.0.1:5102 &
python3 MockIntegrations/docker/ga2-smoke/smoke.py --jwt-key "$JwtOptions__Key"
```

Resultado obtenido el 2026-10-13 (GA2 compilado con el SDK .NET 5.0.408):

```
[OK ] TusDatos ConsultarCliente: GA2 HTTP 200 · mock 200 escenario=default
[OK ] TusDatos ConsultarVigia: GA2 HTTP 200 · mock 200 escenario=riesgo
[OK ] TusDatos 404 → Data=null: GA2 HTTP 200 · mock 404 escenario=no-encontrado
[OK ] Dodo Form_GetParameters: GA2 HTTP 200 · mock 200 escenario=default
[OK ] Vigía F_VERIFICATERCERO: GA2 HTTP 200 · mock 200 escenario=reportado
[OK ] Fénix ValidarBiometria: GA2 HTTP 200 · mock 200 escenario=default
[OK ] Seven InsertarGnTerce: GA2 HTTP 200 · mock 200 escenario=default
7/7 llamadas GA2 → mock correctas
```

La API BUA se compiló con el perfil `Mocks`, pero **no se ejecutó**: su flujo con BUA ws.02 depende de la API GA2 y de procedimientos de la base real.
