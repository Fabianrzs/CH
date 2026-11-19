# Contratos de integración para servicios mock — GA2

Un documento técnico por integración, con cada operación descrita de forma que se pueda implementar un **mock compatible sin volver a analizar el código fuente**. El alcance son las integraciones E1–E12 del [inventario](../INVENTARIO_INTEGRACIONES.md); las integraciones registradas sin uso (fila L*) se mencionan dentro del documento de su sistema cuando aplica.

> **Implementación:** estos contratos están implementados y verificados en [`MockIntegrations/`](../../../MockIntegrations/README.md) (servicio .NET 10 con un mock por proveedor y Swagger).

## Índice

| # | Documento | Protocolo | Operaciones que GA2 usa | Redirigible por configuración | Toggle en BD |
|---|---|---|---|---|---|
| E1 | [Seven ERP](E01-seven-erp.md) | SOAP 1.1 (7 ASMX) | 29 (6 con lógica de negocio) | Sí (`SvcUtils:Seven*`) | No |
| E2 | [WorkManager / DODO (SOAP)](E02-workmanager-dodo-soap.md) | SOAP 1.1 WCF | 12 envueltas (~6 con uso real) | Sí (`SvcUtils:WorkManagerUrlSvc`) | `DODO` |
| E3 | [BUA SOAP](E03-bua-soap.md) | SOAP 1.1 WCF | 1 (`ObtenerBUA`, 2 proxies) | `ws.02` sí; **`ws.01` no** (URL fija) | `Bua` (un flujo) |
| E4 | [Vigía](E04-vigia.md) | SOAP 1.1 **RPC/encoded** | 1 | Sí (`SvcUtils:VigiaVerificarTerceroUrlSvc`) | `VIGIA` |
| E5 | [Fénix — biometría](E05-fenix-biometria.md) | SOAP 1.1 ASMX (DataSet) | 1 | Sí (`SvcUtils:FenixServiceSvc`) | `FENIX` |
| E6 | [Haberes — MCD](E06-haberes-mcd.md) | SOAP 1.1 ASMX (DataSet) | 3 | Sí (`SvcUtils:HaberesUrlSvc`) | No |
| E7 | [TransUnion DecisionCentre](E07-transunion-decisioncentre.md) | SOAP 1.1 + XML en string | 1 método, 7 pasos lógicos | **No** (URL fija de producción) | No |
| E8 | [TusDatos (API Gateway)](E08-tusdatos.md) | REST/JSON | 3 | Sí (`Integrationes:TusDatos`) | `TusDatos` |
| E9 | [Dodo REST](E09-dodo-rest.md) | REST/JSON | 29 (pass-through) | Sí (`Integrationes:Dodo`) | `Dodo` |
| E10 | [Pasarela ktrionApi](E10-pasarela-ktrionapi.md) | REST/JSON + Bearer | 5 (4 sólo en DEBUG) | Sí (`CHKeys:UrlCajaHonor`) | No |
| E11 | [SMTP](E11-smtp.md) | SMTP | 2 emisores activos | Emisor principal sí (`SMTP:*`); Outlook no | No |
| E12 | [Azure Service Bus](E12-azure-service-bus.md) | AMQP | 5 eventos (consumo) | Sí (`EventBusConnection`) | No |

## Cómo se obtuvo y validó el contenido

- **Sobres SOAP de request:** se compilaron los `Reference.cs` del repositorio (proxies WCF reales) en un arnés .NET 8 externo al proyecto. Cada operación se invocó con datos de relleno contra un `HttpListener` local, que capturó el XML, el `SOAPAction` y el `Content-Type` **tal como los emite GA2**.
- **Sobres SOAP de response:** se generaron con los mismos serializadores de WCF y se validaron **por ida y vuelta**: el proxy real de GA2 los deserializó sin error y con los valores esperados. Las 115 operaciones de los 19 contratos SOAP del alcance (incluidas las no usadas y los proxies duplicados) pasan esa validación.
- **Respuestas con lógica propia** (DataSets de Fénix y Haberes, `DCResponse` de TransUnion, resultados XML de Seven): además se ejecutó **la misma lógica de lectura de GA2** (`XmlSerializer(typeof(diffgram))`, `Nodes[1].FirstNode`, `Serialization.DeserializeDC/RC/Apps`, etc.).
- **JSON REST:** se generaron con los DTO reales y las mismas opciones de serialización de GA2 (`System.Text.Json` camelCase sin nulos para TusDatos/Dodo; `JsonConvert` por defecto para la pasarela).
- **Valores de relleno:** en los ejemplos genéricos, los textos llevan el nombre del campo, los números `1`/`1.5` y las fechas `2026-01-15`. Donde GA2 interpreta el contenido, los ejemplos usan **valores realistas** y se indica.
- **Ningún secreto** se reproduce: credenciales y claves aparecen como `<AppMain:Password>`, `USUARIO_TU`, etc.

## Reglas generales para mocks SOAP

1. **Transporte:** `POST` a la ruta del servicio; request con `Content-Type: text/xml; charset=utf-8` y `SOAPAction: "<acción>"` (con comillas). Lo más sencillo es un único servidor que despache por `SOAPAction`.
2. **Respuesta correcta:** HTTP 200, `Content-Type: text/xml; charset=utf-8`, sobre SOAP 1.1 **sin** header `<Action>` de WS-Addressing (si se incluye con `mustUnderstand="1"`, el cliente lo rechaza).
3. **Error:** HTTP 500 con `<s:Fault><faultcode>s:Server</faultcode><faultstring>…</faultstring></s:Fault>`.
4. **Namespaces y orden importan.** Con `DataContractSerializer` (Seven DataContract, WorkManager, BUA), un elemento con namespace incorrecto o fuera de orden **se ignora sin error** y queda con su valor por defecto. Copiar la estructura de los ejemplos.
5. **Timeouts:** casi todos los clientes SOAP de GA2 usan `TimeSpan.MaxValue`: un mock que no responde deja la petición colgada indefinidamente.
6. **HTTPS:** Vigía (en la API GA2) y TransUnion usan `https`; la pasarela ktrionApi acepta cualquier certificado; los clientes WCF validan el certificado (hace falta uno de confianza o cambiar la URL a `http`).

## Comportamiento común de GA2 ante errores

- Las excepciones no controladas llegan al `ErrorValidationMiddleware`, que responde **HTTP 409 (Conflict)** con un JSON de log de error (no 500).
- Los clientes REST nuevos (`CommonIntegracionCliente`: TusDatos, Dodo) **no lanzan** ante errores HTTP: devuelven `Response{IsSuccess=false, ReturnMessage=<cuerpo>}`. Un `404` se trata como éxito con `Data=null`. Con el toggle apagado, en cambio, **sí lanzan** (→ 409).

## Defectos encontrados durante la validación

Afectan al diseño de los mocks; conviene escalarlos:

| Integración | Defecto | Efecto |
|---|---|---|
| E12 Service Bus | `System.Text.Json` ignora el campo público `request` de los `Form_*Request` | El payload **siempre** llega nulo a WorkManager |
| E3 BUA | `ws.01` ignora la URL de configuración; `ws.01` y `ws.02` difieren en los tipos de `valor` y `tasaAporte` | Un texto en `valor` rompe `ws.01` |
| E6 Haberes | GA2 lee `Nodes[1].FirstNode` | Un DataSet indentado rompe `ReporteHaberes` (`Root element is missing`) |
| E7 TransUnion | URL de producción fija; la inyección de `xmlns` en `ConsumirTUAqm` se pierde (sobrescrita) | No se puede redirigir por configuración |
| E8/E9 | El toggle se valida fuera del `try` | Integración apagada → HTTP 409 en lugar de `IsSuccess=false` |
| E1 Seven | `InsertarFacturaP` sin `FAC_CONT`/`FAC_NUME` | `NullReferenceException` |
| E2 WorkManager | `Form_GetParameters` con `Form` nulo / `Result` nulo | `NullReferenceException` |
| E11 SMTP | `ParametrizacionBusinessLogic.SendNotification` con credenciales y destinatario fijos | Sin uso (llamada comentada), pero las credenciales siguen en el repositorio |

## Pendientes a confirmar con los dueños de cada sistema

- E1: nombre real del elemento raíz de cada `...Result` de Seven (salvo `GN_TERCE`).
- E4: dominio de `Encontrado`/`Listas`/`Metodo` y de `Pe_TipoVerificacion`.
- E7: muestras reales de `DCResponse` por paso (las tablas `RegistroLogTUSimulador` y `VLL_VALIDACION_LOG_OTP` las guardan).
- E8: valores posibles de `statusJobTusDatos` y `riskLAFT`.
- E12: productor real del topic y su formato de mensajes.
