# Pendientes, riesgos y contratos no confirmados

## 1. Contratos no confirmados (requieren información de los dueños de cada sistema)

| # | Integración | Qué falta | Cómo obtenerlo | Impacto en el mock |
|---|---|---|---|---|
| C1 | Seven | Nombre real del elemento raíz de cada `...Result` (salvo `GN_TERCE`) y semántica de `GetCuentaContable`, `GetCuentaContableTO`, `GetAporte` | Tablas de auditoría de `EjecutarServicio` (`InsertReportIntegraciones`) en un ambiente con Seven real | GA2 no valida la raíz: compatible. Los 3 `Get*` devuelven texto de ejemplo |
| C2 | Vigía | Dominio de `Encontrado`, `Listas`, `Metodo` y de `Pe_TipoVerificacion` | Equipo de Vigía / respuestas reales | Valores de ejemplo (`S`/`N`, "OFAC, ONU") |
| C3 | TransUnion | `DCResponse` reales por paso (IDVision, OTP, AQM, historial), `DSOnlineBureauCOLData` completo | Tablas `RegistroLogTUSimulador`, `VLL_VALIDACION_LOG_OTP`, Blob `trans-union` | Respuestas mínimas validadas con los deserializadores de GA2 |
| C4 | TusDatos | Valores de `statusJobTusDatos` y `riskLAFT`; código HTTP real con API key inválida (401 o 403) | Equipo del API Gateway | Configurables (`PendingStatus`/`FinishedStatus`); el mock usa 401 (GA2 trata igual 401 y 403) |
| C5 | Service Bus | Productor real del topic `chonorintegraciones` y formato real de los mensajes | Equipo de integración | El mock publica el formato que se deduce del código |
| C6 | WorkManager | Formato real del consecutivo del `BarCode` | Radicados reales | El mock usa `{FormCode}-NNNNNN` (GA2 sólo exige el prefijo) |
| C7 | Pasarela ktrionApi | Contrato de las rutas sin consumidores (`formdelete`, `formgetdata`, `formgetfilecodes`, `formgetbase64stringfile`, `listgetitems`, `obtenerafiliadobua`) | Si se reactivan en GA2 | Responden **501** explícito |
| C8 | Operaciones del contrato sin cliente en GA2 (17 WorkManager, 32 TransUnion, 5 Haberes, `xxValidarBiometria`) | Respuestas reales | Sólo si GA2 empieza a usarlas | Responden **SOAP Fault** explícito |

## 2. Verificaciones que no se pudieron completar (y por qué)

| # | Verificación | Impedimento | Estado |
|---|---|---|---|
| V1 | Flujos de negocio completos de GA2 (radicación, pago de crédito, prospección…) contra los mocks | El repositorio no incluye el esquema ni los datos de la base GA2; cada flujo consulta procedimientos de la BD antes de llamar a la integración | **Pendiente**: requiere una copia de la base de pruebas. El cableado se comprobó con 7 endpoints reales de GA2 sobre una base *stub* ([guía §8](guia-configuracion-ga2.md#8-iniciar-ga2-y-mockintegrations-en-conjunto-procedimiento-verificado)) |
| V2 | Redirección de BUA ws.01 y TransUnion | URL fija en el código; requiere DNS/hosts y, para TransUnion, un certificado TLS de confianza para `www.transuniondecisioncentre.com.mx` | **No ejecutada**. Los contratos sí se verificaron con los proxies/clientes reales de GA2 |
| V3 | Consumo de eventos de Service Bus por GA2 hasta WorkManager | Defectos D1 y D2 de GA2 | GA2 arranca y consume del emulador, pero descarta los mensajes |
| V4 | API BUA ejecutándose con el perfil `Mocks` | Depende de la API GA2 y de la BD real | Sólo se compiló |
| V5 | SMTP de Identity (Outlook) y `SmtpTestController` | Host fijo; Outlook exige STARTTLS | No simulado |
| V6 | Mock con HTTPS para Vigía/TransUnion por nombre de host real | No necesario con el perfil `Mocks` (Vigía usa `http`); TransUnion ver V2 | — |

## 3. Defectos de GA2 encontrados (no se corrigieron: fuera del alcance)

| # | Defecto | Evidencia | Efecto |
|---|---|---|---|
| D1 | Los 5 manejadores de eventos (`WorkManager*IntegrationEventHandler`) **no están registrados en DI**; `EventBusServiceBus.ProcessEvent` usa `ResolveOptional`, los salta y **completa el mensaje** | Ejecución contra el emulador: el mensaje desaparece y WorkManager recibe 0 llamadas | Todo evento publicado se pierde sin error |
| D2 | `System.Text.Json` ignora el campo público `request` de los proxies WCF en los eventos | Prueba `Service_bus_sample_is_deserialized_like_EventBusServiceBus_and_shows_the_known_defect` | Aunque se registren los manejadores, el payload llega nulo |
| D3 | `Azure.Messaging.ServiceBus` 7.9.0 no soporta el emulador | Sondas con 7.9.0 y 7.20.1 | Requiere el proxy TLS o actualizar el paquete (≥ 7.18) |
| D4 | URL de TransUnion **producción** fija en el código (`ValidacionesTU.cs:16`) | Código | Cualquier entorno llama a producción con datos reales |
| D5 | BUA ws.01 ignora `SvcUtils:BuaUrlSvc` (`new BUA_APIClient()` sin parámetros) | Código | No redirigible por configuración |
| D6 | `EstadoIntegracionClient` (API BUA) compara `ToUpper()` contra `"TusDatos"`/`"Dodo"`: nunca coincide y `First()` lanza | Código (inventario §2.1) | Latente (BUA hoy no usa esos clientes) |
| D7 | El interruptor de TusDatos/Dodo se valida fuera del `try`: con la integración apagada, GA2 responde HTTP 409 y no `IsSuccess=false` | Código | Comportamiento inconsistente |
| D8 | Validación TLS desactivada en la pasarela (`ProviderRequestFactory.cs:292`, `:311`) | Código | Riesgo de seguridad |
| D9 | Secretos en `appsettings*.json` del repositorio (SAS de Service Bus, API key de TusDatos, credenciales de IDVision/AQM, `AppMain:Password`, `CHKeys:Token`) y credenciales SMTP en el código | Código | Rotarlos y moverlos a un almacén de secretos. El perfil `Mocks` los reemplaza por valores ficticios |
| D10 | Endpoints sin `[Authorize]`: `DodoController`, `TusDatosController`, `SmtpTestController` | Código | Proxies anónimos hacia sistemas internos |

## 4. Riesgos del entorno de mocks

- **Timeouts infinitos en SOAP** (`TimeSpan.MaxValue`): con el escenario `slow` o un mock detenido, GA2 puede quedar esperando sin límite.
- **DEBUG vs Release:** la radicación de crédito y `GenerarReporte` de Haberes van por la pasarela (E10) en DEBUG y por SOAP directo en Release. Los dos caminos están simulados, pero cada entorno debe saber cuál compila.
- **Estado en memoria:** los consecutivos, jobs y sesiones se pierden al reiniciar el mock (deterministas tras `POST /_mock/reset`).
- **Emulador de Service Bus:** depende de su SQL Server. Si se recrea el contenedor SQL con el emulador en marcha, la API de reglas queda colgada: recrear también el emulador.
- `/tmp` en algunos equipos es tmpfs (RAM): compilar GA2 dentro de él puede agotar el espacio (IronPdf copia binarios nativos grandes en cada proyecto).

## 5. Decisiones que corresponden al equipo

1. ¿Corregir D1/D2 para que el canal Service Bus sea funcional, o retirarlo?
2. ¿Hacer configurables las URLs de TransUnion y BUA ws.01 (D4/D5)? Es un cambio de código pequeño y eliminaría el uso de DNS/certificados en pruebas.
3. ¿Actualizar `Azure.Messaging.ServiceBus` (D3) o mantener el proxy TLS en los entornos de prueba?
4. Facilitar una copia (anonimizada) de la base de pruebas para completar V1.
