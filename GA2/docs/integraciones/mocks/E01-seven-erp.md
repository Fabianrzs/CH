# E1 — Seven ERP (Digital Ware): servicios ASMX de contabilidad, presupuesto, facturación y terceros

> Documento para implementar un **mock compatible** con los 7 servicios SOAP de Seven que consume GA2. Todo lo que sigue se obtuvo del código (`develop`, 2026-10-09). Los sobres de **request** se capturaron ejecutando los proxies WCF reales del repositorio contra un servidor local, y los de **response** se validaron haciendo que esos mismos proxies los deserializaran (ida y vuelta).

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Sistema | ERP Seven (contabilidad, presupuesto/CDP, cuentas por pagar, terceros) |
| Protocolo | SOAP 1.1 sobre HTTP (servicios ASMX), sin WS-Security ni autenticación HTTP |
| Namespace de los contratos | `http://seven/` (`SOAPAction = "http://seven/<Operación>"`) |
| Host configurado | `http://cpvmp-306x/Seven/...` (`SvcUtils:*`); los `Reference.cs` traen por defecto `cpvmp-34xp` |
| Binding cliente | `BasicHttpBinding` — `Security=None` si la URL es `http`, `Transport` si es `https`; timeouts `TimeSpan.MaxValue` |
| Clientes .NET | `src/GA2.Domain/Core/ReferenciasSvc/Seven/*/ConsumoXxx.cs` (`IConsumo*`) y `Connected Services/ServiceSeven/MovimientoContableService.cs` |
| Toggle en BD | No existe: si Seven no responde, el flujo de negocio falla |
| Auditoría | Las llamadas pasan por `IAuditoriaServicesBusinessLogic.EjecutarServicio(..., ReferenciaSvc.Seven)` (registro en BD) |
| Consumidor host | API GA2 |

### Servicios y URL configurables

| Clave `SvcUtils` | Ruta | Contrato | Operaciones |
|---|---|---|---|
| `SevenWSMovContable` | `/Seven/WsMovContable/scnmcont.asmx` | `SCNMCONTSoap` | InsertarCnMcont, InsertarCnDmcon, AplicarCnMcont, RevertirCnMcont, AnularCnMcont, InsertarMovContable, SetContabToper |
| `SevenWSCdp` | `/Seven/WsCdp/WPgOblig.asmx` | `SPGOBLIGSoap` | InsertarCDP, InsertarDetCDP, AplicarCDP, AnularCDP, CrearCDP |
| `SevenWSFactura` | `/Seven/Wsfactura/spofactu.asmx` | `SPOFACTUSoap` | InsertarFacturaP, AnularFactura, InsertarPoFactu, AnularPoFactu, InsertarPoFactuP |
| `SevenWSProveedor` | `/Seven/Wsproveedor/SIeWssec.asmx` | `SIEWSSECSoap` | SyncGnTerce, SyncFaClien, SyncFaClienComplex, SyncPoPvdor, SyncFaConta, SyncEmpleadoPvdor, ChangeNit, ActModalidadCupo |
| `SevenWSTercero` | `/Seven/Wstercero/wgnterce.asmx` | `SGNTERCESoap` | InsertarGnTerce |
| `SevenWintipro` | `/Seven/Wintipro/WInTipro.asmx` | `SINTIPROSoap` | GetCuentaContable, GetCuentaContableTO |
| `SevenWSAportes` | `/Seven/WSaportes/ValidarAportes.asmx` | `ValidarAportesSoap` | GetAporte |

> **Redirigir al mock:** basta cambiar cada clave `SvcUtils:Seven*` en `appsettings` por la URL del mock (p. ej. `http://localhost:9001/Seven/WsMovContable/scnmcont.asmx`). El mock puede exponer un único servicio HTTP que despache por el header `SOAPAction`.

## 2. Reglas comunes para el mock

1. **Formato de respuesta.** Todas las operaciones devuelven un único elemento `<{Operación}Result>`. En las de negocio es un `string` con **XML escapado**. Patrón:

```xml
<CrearCDPResult>&lt;PG_MPRES&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;MPR_CONT&gt;45871&lt;/MPR_CONT&gt;&lt;/PG_MPRES&gt;</CrearCDPResult>
```

2. **Códigos de negocio.** `RETORNO = 0` es éxito y `RETORNO = 1` es error (mensaje en `TXTERROR`). En la tabla, "Raíz propuesta" es el nombre de elemento sugerido para el mock; GA2 no lo valida (salvo `GN_TERCE`):

| Operación | Etiquetas que GA2 lee | Raíz propuesta |
|---|---|---|
| `InsertarGnTerce` | `RETORNO`, `TXTERROR` (ruta `//GN_TERCE/...`) | `GN_TERCE` (**obligatoria**) |
| `SyncPoPvdor` | `PVD_CODD` (int), `RETORNO`, `TXTERROR` | `PO_PVDOR` |
| `CrearCDP` | `MPR_CONT` (int), `RETORNO`, `TXTERROR` | `PG_MPRES` |
| `InsertarFacturaP` | `FAC_CONT` (int), `FAC_NUME` (int), `RETORNO`, `TXTERROR` | `PO_FACTU` |
| `InsertarMovContable` | `MCO_CONT` (int), `MCO_NUME` (int), `RETORNO` | `CN_MCONT` |
| `AnularCnMcont` | `RETORNO` (regex `<RETORNO>(.*?)</RETORNO>`) | `CN_MCONT` |

   Los nombres de raíz salen del uso de las tablas Seven en el código y **no** se pudieron confirmar con el servicio real: verificar con una muestra de producción (tablas de auditoría de `EjecutarServicio`).
3. **Orden de un pago completo** (Crédito, Cesantías, Reintegros, Embargos): `InsertarGnTerce` → `SyncPoPvdor` → `CrearCDP` → `InsertarFacturaP` → `InsertarMovContable`. Si en un paso `RETORNO = 1`, GA2 corta el flujo y devuelve `TXTERROR`.
4. **Errores técnicos.**
   - **SOAP Fault** (`HTTP 500` + sobre `<s:Fault>`): el proxy WCF lanza `FaultException`. Salvo que se indique lo contrario en la operación, GA2 **no** la captura de forma específica y la excepción sube hasta el `ErrorValidationMiddleware` de la API, que responde al frontend **HTTP 409 (Conflict)** con un JSON de log de error.
   - **HTTP 500 sin sobre SOAP / HTML / cuerpo vacío**: `ProtocolException` o `CommunicationException`; mismo efecto.
   - **Sin respuesta**: los bindings de GA2 tienen `SendTimeout/ReceiveTimeout = TimeSpan.MaxValue` (salvo donde se indica), es decir, **la llamada no expira**. Un mock que no responde deja la petición colgada indefinidamente.
   Formato de SOAP Fault que debe devolver el mock (HTTP 500, `Content-Type: text/xml; charset=utf-8`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <s:Fault>
      <faultcode>s:Server</faultcode>
      <faultstring xml:lang="es-CO">Mensaje de error del servicio</faultstring>
    </s:Fault>
  </s:Body>
</s:Envelope>
```

5. **Validación del request.** El mock **no necesita validar** el contenido. Si se quiere, los tipos de la §4 dan nombres, tipos y orden exactos. Para pruebas de integración conviene que el mock guarde el XML recibido, para hacer aserciones sobre los asientos contables.
6. **Segundo proxy de `scnmcont.asmx`.** `Connected Services/ServiceSeven/Reference.cs` (usado por `MovimientoContableService`) es otro proxy del mismo WSDL con `XmlSerializer` y parámetros planos. Se verificó que produce **el mismo XML** (sólo cambian declaraciones de namespace), así que un único mock sirve a ambos.


## 3. Operaciones


## 3.1 Movimientos contables — `SCNMCONTSoap` (scnmcont.asmx)

URL: `SvcUtils:SevenWSMovContable` → `http://cpvmp-306x/Seven/WsMovContable/scnmcont.asmx`

### 1. `InsertarCnMcont`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Pass-through (también lo invoca el endpoint `ChangeNit` por error de mapeo, ver nota) |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarCnMcont"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarCnMcontAsync` |

**Propósito.** Inserta la cabecera de un comprobante contable.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarCnMcont/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnMcont/top_codi` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnMcont/mco_nume` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnMcont/mco_fech` | `xs:dateTime` | siempre presente | `http://seven/` |
| `InsertarCnMcont/mod_codi` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnMcont/arb_cods` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnMcont/mco_desc` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarCnMcontResponse/InsertarCnMcontResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarCnMcont xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <top_codi>1</top_codi>
      <mco_nume>1</mco_nume>
      <mco_fech>2026-01-15T00:00:00</mco_fech>
      <mod_codi>1</mod_codi>
      <arb_cods>arb_cods</arb_cods>
      <mco_desc>mco_desc</mco_desc>
    </InsertarCnMcont>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarCnMcontResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarCnMcontResult>&lt;CN_MCONT&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;MCO_CONT&gt;551200&lt;/MCO_CONT&gt;&lt;MCO_NUME&gt;3301&lt;/MCO_NUME&gt;&lt;/CN_MCONT&gt;</InsertarCnMcontResult>
    </InsertarCnMcontResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 2. `InsertarCnDmcon`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/InsertarCnDmcon` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarCnDmcon"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarCnDmconAsync` |

**Propósito.** Inserta una línea de detalle de un comprobante.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarCnDmcon/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/mco_cont` | `xs:int` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/cue_codi` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/dmc_desc` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/dmc_cant` | `xs:double` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/dmc_acti` | `xs:double` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/dmc_refe` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/dmc_vadb` | `xs:decimal` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/dmc_vacr` | `xs:decimal` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/dmc_vaba` | `xs:decimal` | siempre presente | `http://seven/` |
| `InsertarCnDmcon/ter_coda` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/arb_codc` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/arb_codp` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/arb_coda` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `InsertarCnDmcon/arb_cods` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarCnDmconResponse/InsertarCnDmconResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarCnDmcon xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <mco_cont>1</mco_cont>
      <cue_codi>cue_codi</cue_codi>
      <dmc_desc>dmc_desc</dmc_desc>
      <dmc_cant>1.5</dmc_cant>
      <dmc_acti>1.5</dmc_acti>
      <dmc_refe>dmc_refe</dmc_refe>
      <dmc_vadb>1.5</dmc_vadb>
      <dmc_vacr>1.5</dmc_vacr>
      <dmc_vaba>1.5</dmc_vaba>
      <ter_coda>ter_coda</ter_coda>
      <arb_codc>arb_codc</arb_codc>
      <arb_codp>arb_codp</arb_codp>
      <arb_coda>arb_coda</arb_coda>
      <arb_cods>arb_cods</arb_cods>
    </InsertarCnDmcon>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarCnDmconResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarCnDmconResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</InsertarCnDmconResult>
    </InsertarCnDmconResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 3. `AplicarCnMcont`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Pass-through + `MovimientoContableService` (sin llamadas de negocio activas) |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AplicarCnMcont"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `AplicarCnMcontAsync` |

**Propósito.** Aplica (contabiliza) un comprobante previamente insertado.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AplicarCnMcont/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `AplicarCnMcont/mco_cont` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AplicarCnMcontResponse/AplicarCnMcontResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AplicarCnMcont xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <mco_cont>1</mco_cont>
    </AplicarCnMcont>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AplicarCnMcontResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <AplicarCnMcontResult>&lt;CN_MCONT&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/CN_MCONT&gt;</AplicarCnMcontResult>
    </AplicarCnMcontResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 4. `RevertirCnMcont`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Pass-through + `MovimientoContableService` (sin llamadas de negocio activas) |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/RevertirCnMcont"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `RevertirCnMcontAsync` |

**Propósito.** Revierte un comprobante contable.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `RevertirCnMcont/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `RevertirCnMcont/mco_cont` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `RevertirCnMcontResponse/RevertirCnMcontResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <RevertirCnMcont xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <mco_cont>1</mco_cont>
    </RevertirCnMcont>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <RevertirCnMcontResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <RevertirCnMcontResult>&lt;CN_MCONT&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/CN_MCONT&gt;</RevertirCnMcontResult>
    </RevertirCnMcontResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 5. `AnularCnMcont`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (Cartera: anulación de comprobantes de pagos masivos) + pass-through |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AnularCnMcont"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `AnularCnMcontAsync` |

**Propósito.** Anula un comprobante contable por empresa (`emp_codi`) y consecutivo (`mco_cont`).

**Quién la invoca en GA2.** `CarteraBusinessLogic` (≈ líneas 2529 y 2586) y `MovimientoContableService.AnularCnMcontAsync`.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularCnMcont/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `AnularCnMcont/mco_cont` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularCnMcontResponse/AnularCnMcontResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularCnMcont xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <mco_cont>1</mco_cont>
    </AnularCnMcont>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularCnMcontResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <AnularCnMcontResult>&lt;CN_MCONT&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/CN_MCONT&gt;</AnularCnMcontResult>
    </AnularCnMcontResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

GA2 aplica `Regex <RETORNO>(.*?)</RETORNO>` y considera éxito sólo si el valor es `0`.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 6. `InsertarMovContable`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** (núcleo contable) + pass-through |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarMovContable"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarMovContableAsync` |

**Propósito.** Registra un comprobante contable (cabecera `TSCnMcont` + líneas `TSCnDmcon` en `vDetalle`) en Seven.

**Quién la invoca en GA2.** Cartera (cierres diarios, mensuales y anuales, aplicación de pagos; `CarteraBusinessLogic`, también vía `Connected Services/ServiceSeven/MovimientoContableService`), Crédito (desembolsos y aprobación, ≈8 puntos), Cesantías (intereses), Reintegros.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarMovContable/pContable` | `TSCnMcont` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarMovContableResponse/InsertarMovContableResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarMovContable xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <pContable>
        <vDetalle>
          <TSCnDmcon>
            <Cue_codi>Cue_codi</Cue_codi>
            <Dmc_desc>Dmc_desc</Dmc_desc>
            <Dmc_cant>1.5</Dmc_cant>
            <Dmc_acti>1.5</Dmc_acti>
            <Dmc_refe>Dmc_refe</Dmc_refe>
            <Dmc_vadb>1.5</Dmc_vadb>
            <Dmc_vacr>1.5</Dmc_vacr>
            <Dmc_vaba>1.5</Dmc_vaba>
            <Ter_coda>Ter_coda</Ter_coda>
            <Arb_codc>Arb_codc</Arb_codc>
            <Arb_coda>Arb_coda</Arb_coda>
            <Arb_codp>Arb_codp</Arb_codp>
            <Arb_cods>Arb_cods</Arb_cods>
            <Ter_codm>Ter_codm</Ter_codm>
          </TSCnDmcon>
        </vDetalle>
        <Emp_codi>1</Emp_codi>
        <Top_codi>1</Top_codi>
        <Mco_nume>1</Mco_nume>
        <Mco_fech>2026-01-15T00:00:00</Mco_fech>
        <Mod_codi>1</Mod_codi>
        <Arb_csuc>Arb_csuc</Arb_csuc>
        <Mco_desc>Mco_desc</Mco_desc>
      </pContable>
    </InsertarMovContable>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarMovContableResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarMovContableResult>&lt;CN_MCONT&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;MCO_CONT&gt;551200&lt;/MCO_CONT&gt;&lt;MCO_NUME&gt;3301&lt;/MCO_NUME&gt;&lt;/CN_MCONT&gt;</InsertarMovContableResult>
    </InsertarMovContableResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

Etiquetas que lee GA2: `MCO_CONT` y `MCO_NUME` (enteros: consecutivo y número del comprobante, que se guardan en BD, p. ej. `CC_COMPROBANTE_SEVEN_ID`) y `RETORNO`.

Además, `MovimientoContableService` **valida el request antes de llamar** (`MovimientoContableValidator`): empresa, tipo de operación, fecha y descripción obligatorios; cada línea con `Cue_codi` numérico, débito o crédito (no ambos, no negativos) y `Arb_codc` ≤ 20 caracteres. Si la validación falla, no hay llamada.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 7. `SetContabToper`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Pass-through + `MovimientoContableService.SetContabToperAsync` |
| Transporte | `POST {SvcUtils:SevenWSMovContable}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SetContabToper"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `SetContabToperAsync` |

**Propósito.** Marca tipos de operación como contabilizados; devuelve un arreglo de resultados por tipo de operación.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SetContabToper/pInput` | lista de `TToper` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SetContabToperResponse/SetContabToperResult` | lista de `TRetToper` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <SetContabToper xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <pInput>
        <TToper>
          <Mco_secu>1</Mco_secu>
          <Emp_codi>1</Emp_codi>
          <Top_codi>1</Top_codi>
          <Mco_nume>1</Mco_nume>
          <Mco_nech>1.5</Mco_nech>
          <Arb_cods>Arb_cods</Arb_cods>
          <Ter_coda>Ter_coda</Ter_coda>
          <Mco_desc>Mco_desc</Mco_desc>
          <Mco_valo>1.5</Mco_valo>
          <Arb_codc>Arb_codc</Arb_codc>
          <Arb_codp>Arb_codp</Arb_codp>
          <Arb_coda>Arb_coda</Arb_coda>
        </TToper>
      </pInput>
    </SetContabToper>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <SetContabToperResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <SetContabToperResult>
        <TRetToper>
          <Mco_secu>1</Mco_secu>
          <Retorno>Retorno</Retorno>
          <TxtError>TxtError</TxtError>
          <Mco_cont>1</Mco_cont>
          <Mco_nume>1</Mco_nume>
        </TRetToper>
      </SetContabToperResult>
    </SetContabToperResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.2 Presupuesto / CDP — `SPGOBLIGSoap` (WPgOblig.asmx)

URL: `SvcUtils:SevenWSCdp` → `http://cpvmp-306x/Seven/WsCdp/WPgOblig.asmx`

### 8. `InsertarCDP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/InsertarCDP` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSCdp}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarCDP"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}InsertarCDP` |
| Elemento raíz del response | `{http://seven/}InsertarCDPResponse` |
| Método del proxy (.NET) | `InsertarCDPAsync` |

**Propósito.** Inserta la cabecera de un CDP.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `top_codi` | `xs:int` | — | `http://seven/` |
| `mpr_nume` | `xs:int` | — | `http://seven/` |
| `mpr_fech` | `xs:dateTime` | — | `http://seven/` |
| `mpr_desc` | `xs:string` | — | `http://seven/` |
| `arb_cods` | `xs:string` | — | `http://seven/` |
| `ter_coda` | `xs:string` | — | `http://seven/` |
| `mpr_tdos` | `xs:string` | — | `http://seven/` |
| `mpr_ndos` | `xs:int` | — | `http://seven/` |
| `uni_codi` | `xs:int` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarCDPResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarCDP xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <top_codi>1</top_codi>
      <mpr_nume>1</mpr_nume>
      <mpr_fech>2026-01-15T00:00:00</mpr_fech>
      <mpr_desc>mpr_desc</mpr_desc>
      <arb_cods>arb_cods</arb_cods>
      <ter_coda>ter_coda</ter_coda>
      <mpr_tdos>mpr_tdos</mpr_tdos>
      <mpr_ndos>1</mpr_ndos>
      <uni_codi>1</uni_codi>
    </InsertarCDP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarCDPResponse xmlns="http://seven/">
      <InsertarCDPResult>&lt;PG_MPRES&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;MPR_CONT&gt;45871&lt;/MPR_CONT&gt;&lt;/PG_MPRES&gt;</InsertarCDPResult>
    </InsertarCDPResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 9. `InsertarDetCDP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/InsertarDetCDP` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSCdp}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarDetCDP"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}InsertarDetCDP` |
| Elemento raíz del response | `{http://seven/}InsertarDetCDPResponse` |
| Método del proxy (.NET) | `InsertarDetCDPAsync` |

**Propósito.** Inserta el detalle de un CDP.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `mpr_cont` | `xs:int` | — | `http://seven/` |
| `rub_codi` | `xs:string` | — | `http://seven/` |
| `pro_codi` | `xs:string` | — | `http://seven/` |
| `arb_codc` | `xs:string` | — | `http://seven/` |
| `arb_codp` | `xs:string` | — | `http://seven/` |
| `arb_coda` | `xs:string` | — | `http://seven/` |
| `dmp_valo` | `xs:double` | — | `http://seven/` |
| `dmp_desc` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarDetCDPResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarDetCDP xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <mpr_cont>1</mpr_cont>
      <rub_codi>rub_codi</rub_codi>
      <pro_codi>pro_codi</pro_codi>
      <arb_codc>arb_codc</arb_codc>
      <arb_codp>arb_codp</arb_codp>
      <arb_coda>arb_coda</arb_coda>
      <dmp_valo>1.5</dmp_valo>
      <dmp_desc>dmp_desc</dmp_desc>
    </InsertarDetCDP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarDetCDPResponse xmlns="http://seven/">
      <InsertarDetCDPResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</InsertarDetCDPResult>
    </InsertarDetCDPResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 10. `AplicarCDP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/AplicarCDP` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSCdp}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AplicarCDP"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}AplicarCDP` |
| Elemento raíz del response | `{http://seven/}AplicarCDPResponse` |
| Método del proxy (.NET) | `AplicarCDPAsync` |

**Propósito.** Aplica un CDP.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `mpr_cont` | `xs:int` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AplicarCDPResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <AplicarCDP xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <mpr_cont>1</mpr_cont>
    </AplicarCDP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <AplicarCDPResponse xmlns="http://seven/">
      <AplicarCDPResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</AplicarCDPResult>
    </AplicarCDPResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 11. `AnularCDP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/AnularCDP` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSCdp}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AnularCDP"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}AnularCDP` |
| Elemento raíz del response | `{http://seven/}AnularCDPResponse` |
| Método del proxy (.NET) | `AnularCDPAsync` |

**Propósito.** Anula un CDP.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `mpr_cont` | `xs:int` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularCDPResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <AnularCDP xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <mpr_cont>1</mpr_cont>
    </AnularCDP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <AnularCDPResponse xmlns="http://seven/">
      <AnularCDPResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</AnularCDPResult>
    </AnularCDPResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 12. `CrearCDP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** + pass-through (`POST api/credito/crearCDP`) |
| Transporte | `POST {SvcUtils:SevenWSCdp}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/CrearCDP"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}CrearCDP` |
| Elemento raíz del response | `{http://seven/}CrearCDPResponse` |
| Método del proxy (.NET) | `CrearCDPAsync` |

**Propósito.** Crea un Certificado de Disponibilidad Presupuestal (CDP) con cabecera `TSPgMpres` y detalle `TSPgDmpre`.

**Quién la invoca en GA2.** Crédito (`CrearCDPSevenCreditoPrimerPago`/`SegundoPago`), Cesantías, Hipoteca, Pagos de embargos, Reintegros.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `pCDP` | `TSPgMpres` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `CrearCDPResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <CrearCDP xmlns="http://seven/">
      <pCDP>
        <vDetalle>
          <TSPgDmpre>
            <Rub_codi>Rub_codi</Rub_codi>
            <Pro_codi>Pro_codi</Pro_codi>
            <Arb_codc>Arb_codc</Arb_codc>
            <Arb_coda>Arb_coda</Arb_coda>
            <Arb_codp>Arb_codp</Arb_codp>
            <Dmp_valo>1.5</Dmp_valo>
            <Dmp_desc>Dmp_desc</Dmp_desc>
          </TSPgDmpre>
        </vDetalle>
        <Emp_codi>1</Emp_codi>
        <Top_codi>1</Top_codi>
        <Mpr_nume>1</Mpr_nume>
        <Mpr_fech>2026-01-15T00:00:00</Mpr_fech>
        <Mpr_desc>Mpr_desc</Mpr_desc>
        <Arb_csuc>Arb_csuc</Arb_csuc>
        <Ter_coda>Ter_coda</Ter_coda>
        <Mpr_tdos>Mpr_tdos</Mpr_tdos>
        <Mpr_ndos>1</Mpr_ndos>
        <Uni_codi>1</Uni_codi>
      </pCDP>
    </CrearCDP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <CrearCDPResponse xmlns="http://seven/">
      <CrearCDPResult>&lt;PG_MPRES&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;MPR_CONT&gt;45871&lt;/MPR_CONT&gt;&lt;/PG_MPRES&gt;</CrearCDPResult>
    </CrearCDPResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

Etiquetas que lee GA2: `MPR_CONT` (entero, consecutivo del CDP; si el texto contiene `MPR_CONT` se guarda en la solicitud), `RETORNO` y `TXTERROR` (si `RETORNO = 1` el flujo de pago se detiene y devuelve `TXTERROR` al usuario). El texto completo también se convierte a JSON (`JsonConvert.SerializeXmlNode`) y se devuelve al frontend.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.3 Facturas / cuentas por pagar — `SPOFACTUSoap` (spofactu.asmx)

URL: `SvcUtils:SevenWSFactura` → `http://cpvmp-306x/Seven/Wsfactura/spofactu.asmx`

### 13. `InsertarFacturaP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** + pass-through |
| Transporte | `POST {SvcUtils:SevenWSFactura}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarFacturaP"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarFacturaPAsync` |

**Propósito.** Crea una factura/cuenta por pagar a proveedor (`TSPoFactu` con detalle `TSPoDfact`).

**Quién la invoca en GA2.** Crédito (`CrearFacturaSevenPrimerPago`/`SegundoPago`), Cesantías, Hipoteca, Pagos de embargos, Reintegros.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarFacturaP/pFactura` | `TSPoFactu` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarFacturaPResponse/InsertarFacturaPResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarFacturaP xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <pFactura>
        <vDetalle>
          <TSPoDfact>
            <Pro_codi>Pro_codi</Pro_codi>
            <Dfa_dest>Dfa_dest</Dfa_dest>
            <Dfa_valo>1.5</Dfa_valo>
            <Arb_codc>Arb_codc</Arb_codc>
            <Arb_coda>Arb_coda</Arb_coda>
            <Arb_codp>Arb_codp</Arb_codp>
          </TSPoDfact>
        </vDetalle>
        <Emp_codi>1</Emp_codi>
        <Top_codi>1</Top_codi>
        <Fac_nume>1</Fac_nume>
        <Fac_fech>2026-01-15T00:00:00</Fac_fech>
        <Arb_cods>Arb_cods</Arb_cods>
        <Fac_desc>Fac_desc</Fac_desc>
        <Pvd_coda>Pvd_coda</Pvd_coda>
        <Dep_codd>1</Dep_codd>
        <Fac_pref>Fac_pref</Fac_pref>
        <Fac_nfap>Fac_nfap</Fac_nfap>
        <Mon_codi>1</Mon_codi>
        <Fac_tipo>Fac_tipo</Fac_tipo>
        <Mpr_cont>1</Mpr_cont>
        <Fac_feta>2026-01-15T00:00:00</Fac_feta>
      </pFactura>
    </InsertarFacturaP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarFacturaPResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarFacturaPResult>&lt;PO_FACTU&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;FAC_CONT&gt;77812&lt;/FAC_CONT&gt;&lt;FAC_NUME&gt;1203&lt;/FAC_NUME&gt;&lt;/PO_FACTU&gt;</InsertarFacturaPResult>
    </InsertarFacturaPResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

Etiquetas que lee GA2: `FAC_CONT` y `FAC_NUME` (enteros, se guardan en la solicitud; **si faltan, `Convert.ToInt32` sobre un nodo inexistente lanza `NullReferenceException`**), `RETORNO` y `TXTERROR`.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 14. `AnularFactura`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/AnularFactura` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSFactura}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AnularFactura"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `AnularFacturaAsync` |

**Propósito.** Anula una factura.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularFactura/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `AnularFactura/fac_cont` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularFacturaResponse/AnularFacturaResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularFactura xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <fac_cont>1</fac_cont>
    </AnularFactura>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularFacturaResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <AnularFacturaResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</AnularFacturaResult>
    </AnularFacturaResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 15. `InsertarPoFactu`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/InsertarPoFactu` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSFactura}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarPoFactu"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarPoFactuAsync` |

**Propósito.** Inserta una factura (variante sin pago).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarPoFactu/pFactura` | `TSPoFactu1` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarPoFactuResponse/InsertarPoFactuResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarPoFactu xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <pFactura>
        <vDetalle>
          <TSPoDfact1>
            <Bod_codi>1</Bod_codi>
            <Pro_codi>Pro_codi</Pro_codi>
            <Dfa_dest>Dfa_dest</Dfa_dest>
            <Dfa_cant>1.5</Dfa_cant>
            <Dfa_valo>1.5</Dfa_valo>
            <Dfa_desc>Dfa_desc</Dfa_desc>
            <Ter_coda>Ter_coda</Ter_coda>
            <Dfa_refe>Dfa_refe</Dfa_refe>
            <Pvd_coda>Pvd_coda</Pvd_coda>
            <Dep_codd>1</Dep_codd>
            <vDistrib>
              <TSPoDdisp1>
                <Arb_codi>Arb_codi</Arb_codi>
                <Tar_codi>1</Tar_codi>
                <Ddi_porc>1.5</Ddi_porc>
              </TSPoDdisp1>
            </vDistrib>
            <VDistribM>
              <TOPoDmfac>
                <arb_codc>arb_codc</arb_codc>
                <arb_coda>arb_coda</arb_coda>
                <arb_codp>arb_codp</arb_codp>
                <arb_cods>arb_cods</arb_cods>
                <dfa_dest>dfa_dest</dfa_dest>
                <dmf_tipo>dmf_tipo</dmf_tipo>
                <dmf_valo>1.5</dmf_valo>
                <dmf_porc>1.5</dmf_porc>
              </TOPoDmfac>
            </VDistribM>
          </TSPoDfact1>
        </vDetalle>
        <vCAdic>
          <TSPoDcoad1>
            <Coa_codi>Coa_codi</Coa_codi>
            <Coa_valo>1.5</Coa_valo>
          </TSPoDcoad1>
        </vCAdic>
        <vAnticipo>
          <TSPoDanti1>
            <vDAntic>
              <TSPoDlant1>
                <Dvt_codi>Dvt_codi</Dvt_codi>
                <Dla_lega>1.5</Dla_lega>
              </TSPoDlant1>
            </vDAntic>
            <Top_cant>1</Top_cant>
            <Fac_nant>1</Fac_nant>
            <Fac_fant>2026-01-15T00:00:00</Fac_fant>
            <Suc_cant>Suc_cant</Suc_cant>
            <Tip_lega>Tip_lega</Tip_lega>
          </TSPoDanti1>
        </vAnticipo>
        <vCruceAut>
          <TSPoCruat1>
            <Top_ccru>1</Top_ccru>
            <Fac_ncru>1</Fac_ncru>
            <Fac_fcru>2026-01-15T00:00:00</Fac_fcru>
            <Suc_ccru>Suc_ccru</Suc_ccru>
            <Cru_valo>1.5</Cru_valo>
            <Cxp_cont>1</Cxp_cont>
          </TSPoCruat1>
        </vCruceAut>
        <Emp_codi>1</Emp_codi>
        <Top_codi>1</Top_codi>
        <Fac_nume>1</Fac_nume>
        <Fac_fech>2026-01-15T00:00:00</Fac_fech>
        <Arb_cods>Arb_cods</Arb_cods>
        <Fac_desc>Fac_desc</Fac_desc>
        <Pvd_coda>Pvd_coda</Pvd_coda>
        <Dep_codd>1</Dep_codd>
        <Fac_tipo>Fac_tipo</Fac_tipo>
        <Fac_pref>Fac_pref</Fac_pref>
        <Fac_nfap>Fac_nfap</Fac_nfap>
        <Fac_esta>Fac_esta</Fac_esta>
        <Mon_codi>1</Mon_codi>
        <Fac_base>1</Fac_base>
        <Bir_cont>1.5</Bir_cont>
        <Fac_auto>Fac_auto</Fac_auto>
        <Fac_mulc>Fac_mulc</Fac_mulc>
        <Cal_impu>1</Cal_impu>
        <Fac_mfev>Fac_mfev</Fac_mfev>
        <Fac_feve>2026-01-15T00:00:00</Fac_feve>
        <Fac_tdis>Fac_tdis</Fac_tdis>
      </pFactura>
    </InsertarPoFactu>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarPoFactuResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarPoFactuResult>&lt;PO_FACTU&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;FAC_CONT&gt;77812&lt;/FAC_CONT&gt;&lt;FAC_NUME&gt;1203&lt;/FAC_NUME&gt;&lt;/PO_FACTU&gt;</InsertarPoFactuResult>
    </InsertarPoFactuResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 16. `AnularPoFactu`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/AnularPoFactu` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSFactura}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/AnularPoFactu"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `AnularPoFactuAsync` |

**Propósito.** Anula una factura de proveedor.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularPoFactu/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `AnularPoFactu/fac_cont` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `AnularPoFactuResponse/AnularPoFactuResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularPoFactu xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <fac_cont>1</fac_cont>
    </AnularPoFactu>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <AnularPoFactuResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <AnularPoFactuResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</AnularPoFactuResult>
    </AnularPoFactuResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 17. `InsertarPoFactuP`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/InsertarPoFactuP` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSFactura}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarPoFactuP"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `InsertarPoFactuPAsync` |

**Propósito.** Inserta una factura con pago.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarPoFactuP/pFactura` | `TSPoFactu2` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarPoFactuPResponse/InsertarPoFactuPResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarPoFactuP xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <pFactura>
        <vDetalle>
          <TSPoDfact2>
            <vDistrib>
              <TSPoDdisp2>
                <Arb_codi>Arb_codi</Arb_codi>
                <Tar_codi>1</Tar_codi>
                <Ddi_tipo>Ddi_tipo</Ddi_tipo>
                <Ddi_valo>1.5</Ddi_valo>
                <Ddi_porc>1.5</Ddi_porc>
              </TSPoDdisp2>
            </vDistrib>
            <vDistribm>
              <TOPoDmfac>
                <arb_codc>arb_codc</arb_codc>
                <arb_coda>arb_coda</arb_coda>
                <arb_codp>arb_codp</arb_codp>
                <arb_cods>arb_cods</arb_cods>
                <dfa_dest>dfa_dest</dfa_dest>
                <dmf_tipo>dmf_tipo</dmf_tipo>
                <dmf_valo>1.5</dmf_valo>
                <dmf_porc>1.5</dmf_porc>
              </TOPoDmfac>
            </vDistribm>
            <Bod_codi>1</Bod_codi>
            <Pro_codi>Pro_codi</Pro_codi>
            <Dfa_dest>Dfa_dest</Dfa_dest>
            <Dfa_cant>1.5</Dfa_cant>
            <Dfa_valo>1.5</Dfa_valo>
            <Dfa_desc>Dfa_desc</Dfa_desc>
            <Ter_coda>Ter_coda</Ter_coda>
            <Dfa_refe>Dfa_refe</Dfa_refe>
            <Ctr_cont>1</Ctr_cont>
            <Pvd_coda>Pvd_coda</Pvd_coda>
            <Dep_codd>1</Dep_codd>
          </TSPoDfact2>
        </vDetalle>
        <vCAdic>
          <TSPoDcoad2>
            <Coa_codi>Coa_codi</Coa_codi>
            <Coa_valo>1.5</Coa_valo>
          </TSPoDcoad2>
        </vCAdic>
        <vAnticipo>
          <TSPoDanti2>
            <vDAntic>
              <TSPoDlant2>
                <Dvt_codi>Dvt_codi</Dvt_codi>
                <Dla_lega>1.5</Dla_lega>
              </TSPoDlant2>
            </vDAntic>
            <Top_cant>1</Top_cant>
            <Fac_nant>1</Fac_nant>
            <Fac_fant>Fac_fant</Fac_fant>
            <Suc_cant>Suc_cant</Suc_cant>
            <Tip_lega>Tip_lega</Tip_lega>
          </TSPoDanti2>
        </vAnticipo>
        <vCruceAut>
          <TSPoCruat2>
            <Top_ccru>1</Top_ccru>
            <Fac_ncru>1</Fac_ncru>
            <Fac_fcru>Fac_fcru</Fac_fcru>
            <Suc_ccru>Suc_ccru</Suc_ccru>
            <Cru_valo>1.5</Cru_valo>
            <Cxp_cont>1</Cxp_cont>
          </TSPoCruat2>
        </vCruceAut>
        <vDocBase>
          <TSPoCtrlo2>
            <Top_base>1</Top_base>
            <Bas_nume>1</Bas_nume>
            <Bas_fech>Bas_fech</Bas_fech>
            <Bas_csuc>Bas_csuc</Bas_csuc>
            <Bas_cont>1</Bas_cont>
          </TSPoCtrlo2>
        </vDocBase>
        <vDocPres>
          <TSCmRdoba2>
            <Mpr_cont>1</Mpr_cont>
            <Dmp_cont>1</Dmp_cont>
            <Dmp_valo>1.5</Dmp_valo>
          </TSCmRdoba2>
        </vDocPres>
        <Emp_codi>1</Emp_codi>
        <Top_codi>1</Top_codi>
        <Fac_nume>1</Fac_nume>
        <Fac_fech>Fac_fech</Fac_fech>
        <Arb_cods>Arb_cods</Arb_cods>
        <Fac_desc>Fac_desc</Fac_desc>
        <Pvd_coda>Pvd_coda</Pvd_coda>
        <Dep_codd>1</Dep_codd>
        <Fac_tipo>Fac_tipo</Fac_tipo>
        <Fac_orig>Fac_orig</Fac_orig>
        <Fac_pref>Fac_pref</Fac_pref>
        <Fac_nfap>Fac_nfap</Fac_nfap>
        <Fac_esta>Fac_esta</Fac_esta>
        <Mon_codi>1</Mon_codi>
        <Bir_cont>1.5</Bir_cont>
        <Fac_auto>Fac_auto</Fac_auto>
        <Fac_refe>Fac_refe</Fac_refe>
        <Fac_mulc>Fac_mulc</Fac_mulc>
        <Cal_impu>1</Cal_impu>
        <Fac_mfev>Fac_mfev</Fac_mfev>
        <Fac_feve>2026-01-15T00:00:00</Fac_feve>
        <Fac_fpse>2026-01-15T00:00:00</Fac_fpse>
      </pFactura>
    </InsertarPoFactuP>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <InsertarPoFactuPResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <InsertarPoFactuPResult>&lt;PO_FACTU&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;FAC_CONT&gt;77812&lt;/FAC_CONT&gt;&lt;FAC_NUME&gt;1203&lt;/FAC_NUME&gt;&lt;/PO_FACTU&gt;</InsertarPoFactuPResult>
    </InsertarPoFactuPResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.4 Proveedores / clientes / terceros — `SIEWSSECSoap` (SIeWssec.asmx)

URL: `SvcUtils:SevenWSProveedor` → `http://cpvmp-306x/Seven/Wsproveedor/SIeWssec.asmx`

### 18. `SyncGnTerce`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Pass-through (`SevenBusinessLogic.SyncGnTerceAsync`), sin endpoint dedicado visible |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncGnTerce"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncGnTerce` |
| Elemento raíz del response | `{http://seven/}SyncGnTerceResponse` |
| Método del proxy (.NET) | `SyncGnTerceAsync` |

**Propósito.** Sincroniza un tercero general.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `tip_codi` | `xs:int` | — | `http://seven/` |
| `ter_coda` | `xs:string` | — | `http://seven/` |
| `ter_dive` | `xs:int` | — | `http://seven/` |
| `ter_nomb` | `xs:string` | — | `http://seven/` |
| `ter_apel` | `xs:string` | — | `http://seven/` |
| `ter_noco` | `xs:string` | — | `http://seven/` |
| `mod_codi` | `xs:int` | — | `http://seven/` |
| `pai_codi` | `xs:int` | — | `http://seven/` |
| `dep_codi` | `xs:int` | — | `http://seven/` |
| `mun_codi` | `xs:int` | — | `http://seven/` |
| `ter_dire` | `xs:string` | — | `http://seven/` |
| `ter_ntel` | `xs:string` | — | `http://seven/` |
| `ter_mail` | `xs:string` | — | `http://seven/` |
| `ter_nfax` | `xs:string` | — | `http://seven/` |
| `ter_audp` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncGnTerceResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncGnTerce xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <tip_codi>1</tip_codi>
      <ter_coda>ter_coda</ter_coda>
      <ter_dive>1</ter_dive>
      <ter_nomb>ter_nomb</ter_nomb>
      <ter_apel>ter_apel</ter_apel>
      <ter_noco>ter_noco</ter_noco>
      <mod_codi>1</mod_codi>
      <pai_codi>1</pai_codi>
      <dep_codi>1</dep_codi>
      <mun_codi>1</mun_codi>
      <ter_dire>ter_dire</ter_dire>
      <ter_ntel>ter_ntel</ter_ntel>
      <ter_mail>ter_mail</ter_mail>
      <ter_nfax>ter_nfax</ter_nfax>
      <ter_audp>ter_audp</ter_audp>
    </SyncGnTerce>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncGnTerceResponse xmlns="http://seven/">
      <SyncGnTerceResult>&lt;GN_TERCE&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/GN_TERCE&gt;</SyncGnTerceResult>
    </SyncGnTerceResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 19. `SyncFaClien`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/SyncFaClien` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncFaClien"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncFaClien` |
| Elemento raíz del response | `{http://seven/}SyncFaClienResponse` |
| Método del proxy (.NET) | `SyncFaClienAsync` |

**Propósito.** Sincroniza un cliente de facturación.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `tip_codi` | `xs:int` | — | `http://seven/` |
| `ter_coda` | `xs:string` | — | `http://seven/` |
| `cli_dive` | `xs:int` | — | `http://seven/` |
| `cli_nomb` | `xs:string` | — | `http://seven/` |
| `cli_apel` | `xs:string` | — | `http://seven/` |
| `cli_noco` | `xs:string` | — | `http://seven/` |
| `mod_codi` | `xs:int` | — | `http://seven/` |
| `pai_codi` | `xs:int` | — | `http://seven/` |
| `dep_codi` | `xs:int` | — | `http://seven/` |
| `mun_codi` | `xs:int` | — | `http://seven/` |
| `cli_coda` | `xs:string` | — | `http://seven/` |
| `tcl_codi` | `xs:int` | — | `http://seven/` |
| `cal_codi` | `xs:int` | — | `http://seven/` |
| `coc_codi` | `xs:int` | — | `http://seven/` |
| `cim_codi` | `xs:int` | — | `http://seven/` |
| `arb_csuc` | `xs:string` | — | `http://seven/` |
| `dcl_dire` | `xs:string` | — | `http://seven/` |
| `dcl_ntel` | `xs:string` | — | `http://seven/` |
| `dcl_mail` | `xs:string` | — | `http://seven/` |
| `dcl_nfax` | `xs:string` | — | `http://seven/` |
| `arb_clte` | `xs:string` | — | `http://seven/` |
| `cli_inna` | `xs:string` | — | `http://seven/` |
| `ven_codi` | `xs:double` | — | `http://seven/` |
| `arb_ccec` | `xs:string` | — | `http://seven/` |
| `lis_codi` | `xs:int` | — | `http://seven/` |
| `dcl_obse` | `xs:string` | — | `http://seven/` |
| `dcl_apar` | `xs:string` | — | `http://seven/` |
| `dcl_cloc` | `xs:string` | — | `http://seven/` |
| `cli_anex` | `xs:string` | — | `http://seven/` |
| `cli_hpra` | `xs:string` | — | `http://seven/` |
| `cli_fecm` | `xs:int` | — | `http://seven/` |
| `cli_feca` | `xs:int` | — | `http://seven/` |
| `cli_audp` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncFaClienResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaClien xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <tip_codi>1</tip_codi>
      <ter_coda>ter_coda</ter_coda>
      <cli_dive>1</cli_dive>
      <cli_nomb>cli_nomb</cli_nomb>
      <cli_apel>cli_apel</cli_apel>
      <cli_noco>cli_noco</cli_noco>
      <mod_codi>1</mod_codi>
      <pai_codi>1</pai_codi>
      <dep_codi>1</dep_codi>
      <mun_codi>1</mun_codi>
      <cli_coda>cli_coda</cli_coda>
      <tcl_codi>1</tcl_codi>
      <cal_codi>1</cal_codi>
      <coc_codi>1</coc_codi>
      <cim_codi>1</cim_codi>
      <arb_csuc>arb_csuc</arb_csuc>
      <dcl_dire>dcl_dire</dcl_dire>
      <dcl_ntel>dcl_ntel</dcl_ntel>
      <dcl_mail>dcl_mail</dcl_mail>
      <dcl_nfax>dcl_nfax</dcl_nfax>
      <arb_clte>arb_clte</arb_clte>
      <cli_inna>cli_inna</cli_inna>
      <ven_codi>1.5</ven_codi>
      <arb_ccec>arb_ccec</arb_ccec>
      <lis_codi>1</lis_codi>
      <dcl_obse>dcl_obse</dcl_obse>
      <dcl_apar>dcl_apar</dcl_apar>
      <dcl_cloc>dcl_cloc</dcl_cloc>
      <cli_anex>cli_anex</cli_anex>
      <cli_hpra>cli_hpra</cli_hpra>
      <cli_fecm>1</cli_fecm>
      <cli_feca>1</cli_feca>
      <cli_audp>cli_audp</cli_audp>
    </SyncFaClien>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaClienResponse xmlns="http://seven/">
      <SyncFaClienResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</SyncFaClienResult>
    </SyncFaClienResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 20. `SyncFaClienComplex`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/SyncFaClienComplex` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncFaClienComplex"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncFaClienComplex` |
| Elemento raíz del response | `{http://seven/}SyncFaClienComplexResponse` |
| Método del proxy (.NET) | `SyncFaClienComplexAsync` |

**Propósito.** Sincroniza un cliente de facturación (variante con objeto complejo).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `toFaClient` | `TOFaClien` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncFaClienComplexResult` | `TSalida` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaClienComplex xmlns="http://seven/">
      <toFaClient>
        <emp_codi>1</emp_codi>
        <tip_codi>1</tip_codi>
        <ter_coda>ter_coda</ter_coda>
        <cli_dive>1</cli_dive>
        <cli_nomb>cli_nomb</cli_nomb>
        <cli_apel>cli_apel</cli_apel>
        <cli_noco>cli_noco</cli_noco>
        <mod_codi>1</mod_codi>
        <pai_codi>1</pai_codi>
        <dep_codi>1</dep_codi>
        <mun_codi>1</mun_codi>
        <cli_coda>cli_coda</cli_coda>
        <tcl_codi>1</tcl_codi>
        <cal_codi>1</cal_codi>
        <coc_codi>1</coc_codi>
        <cim_codi>1</cim_codi>
        <arb_csuc>arb_csuc</arb_csuc>
        <dcl_dire>dcl_dire</dcl_dire>
        <dcl_ntel>dcl_ntel</dcl_ntel>
        <dcl_mail>dcl_mail</dcl_mail>
        <dcl_nfax>dcl_nfax</dcl_nfax>
        <arb_clte>arb_clte</arb_clte>
        <cli_inna>cli_inna</cli_inna>
        <ven_codi>1.5</ven_codi>
        <arb_ccec>arb_ccec</arb_ccec>
        <lis_codi>1</lis_codi>
        <dcl_obse>dcl_obse</dcl_obse>
        <dcl_apar>dcl_apar</dcl_apar>
        <dcl_cloc>dcl_cloc</dcl_cloc>
        <cli_anex>cli_anex</cli_anex>
        <cli_hpra>cli_hpra</cli_hpra>
        <cli_fecm>1</cli_fecm>
        <cli_feca>1</cli_feca>
        <cli_audp>cli_audp</cli_audp>
        <arb_cpro>arb_cpro</arb_cpro>
        <arb_care>arb_care</arb_care>
        <act_codi>act_codi</act_codi>
      </toFaClient>
    </SyncFaClienComplex>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaClienComplexResponse xmlns="http://seven/">
      <SyncFaClienComplexResult>
        <Retorno>1</Retorno>
        <TxtError>TxtError</TxtError>
        <CliCodi>1</CliCodi>
        <DclCodd>1</DclCodd>
      </SyncFaClienComplexResult>
    </SyncFaClienComplexResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 21. `SyncPoPvdor`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** + pass-through |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncPoPvdor"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncPoPvdor` |
| Elemento raíz del response | `{http://seven/}SyncPoPvdorResponse` |
| Método del proxy (.NET) | `SyncPoPvdorAsync` |

**Propósito.** Crea/sincroniza el proveedor (beneficiario del pago) en Seven.

**Quién la invoca en GA2.** Crédito (`CrearProveedorSevenCredito`), Cesantías, Hipoteca, Pagos de embargos, Reintegros.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `tip_codi` | `xs:int` | — | `http://seven/` |
| `pvd_coda` | `xs:string` | — | `http://seven/` |
| `pvd_dive` | `xs:int` | — | `http://seven/` |
| `pvd_nomb` | `xs:string` | — | `http://seven/` |
| `pvd_apel` | `xs:string` | — | `http://seven/` |
| `pvd_noco` | `xs:string` | — | `http://seven/` |
| `pai_codi` | `xs:int` | — | `http://seven/` |
| `dep_codi` | `xs:int` | — | `http://seven/` |
| `mun_codi` | `xs:int` | — | `http://seven/` |
| `tpr_codi` | `xs:int` | — | `http://seven/` |
| `pvd_clad` | `xs:string` | — | `http://seven/` |
| `arb_csuc` | `xs:string` | — | `http://seven/` |
| `cal_codi` | `xs:int` | — | `http://seven/` |
| `coc_codi` | `xs:int` | — | `http://seven/` |
| `cim_codi` | `xs:int` | — | `http://seven/` |
| `act_codi` | `xs:int` | — | `http://seven/` |
| `mon_codi` | `xs:int` | — | `http://seven/` |
| `cup_nume` | `xs:string` | — | `http://seven/` |
| `cup_idba` | `xs:string` | — | `http://seven/` |
| `ban_codi` | `xs:int` | — | `http://seven/` |
| `sub_codi` | `xs:int` | — | `http://seven/` |
| `cup_tipo` | `xs:string` | — | `http://seven/` |
| `pvr_riva` | `xs:string` | — | `http://seven/` |
| `pvr_auto` | `xs:string` | — | `http://seven/` |
| `pvr_reau` | `xs:string` | — | `http://seven/` |
| `cup_tdba` | `xs:string` | — | `http://seven/` |
| `cup_tcta` | `xs:string` | — | `http://seven/` |
| `cup_ib04` | `xs:string` | — | `http://seven/` |
| `cup_ib05` | `xs:string` | — | `http://seven/` |
| `cup_ib06` | `xs:string` | — | `http://seven/` |
| `cup_ib07` | `xs:string` | — | `http://seven/` |
| `cup_ib08` | `xs:string` | — | `http://seven/` |
| `cup_ib09` | `xs:string` | — | `http://seven/` |
| `cup_ib10` | `xs:string` | — | `http://seven/` |
| `dep_dire` | `xs:string` | — | `http://seven/` |
| `dep_ntel` | `xs:string` | — | `http://seven/` |
| `dep_mail` | `xs:string` | — | `http://seven/` |
| `dep_codd` | `xs:int` | — | `http://seven/` |
| `cup_numi` | `xs:string` | — | `http://seven/` |
| `cup_nomi` | `xs:string` | — | `http://seven/` |
| `ban_coin` | `xs:int` | — | `http://seven/` |
| `ban_dire` | `xs:string` | — | `http://seven/` |
| `cup_coab` | `xs:string` | — | `http://seven/` |
| `cup_coib` | `xs:string` | — | `http://seven/` |
| `cup_coas` | `xs:string` | — | `http://seven/` |
| `cup_cobi` | `xs:string` | — | `http://seven/` |
| `cup_clas` | `xs:string` | — | `http://seven/` |
| `dep_apar` | `xs:string` | — | `http://seven/` |
| `dep_nfax` | `xs:string` | — | `http://seven/` |
| `dep_cloc` | `xs:string` | — | `http://seven/` |
| `dep_nomb` | `xs:string` | — | `http://seven/` |
| `cup_esta` | `xs:string` | — | `http://seven/` |
| `pvd_audp` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncPoPvdorResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncPoPvdor xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <tip_codi>1</tip_codi>
      <pvd_coda>pvd_coda</pvd_coda>
      <pvd_dive>1</pvd_dive>
      <pvd_nomb>pvd_nomb</pvd_nomb>
      <pvd_apel>pvd_apel</pvd_apel>
      <pvd_noco>pvd_noco</pvd_noco>
      <pai_codi>1</pai_codi>
      <dep_codi>1</dep_codi>
      <mun_codi>1</mun_codi>
      <tpr_codi>1</tpr_codi>
      <pvd_clad>pvd_clad</pvd_clad>
      <arb_csuc>arb_csuc</arb_csuc>
      <cal_codi>1</cal_codi>
      <coc_codi>1</coc_codi>
      <cim_codi>1</cim_codi>
      <act_codi>1</act_codi>
      <mon_codi>1</mon_codi>
      <cup_nume>cup_nume</cup_nume>
      <cup_idba>cup_idba</cup_idba>
      <ban_codi>1</ban_codi>
      <sub_codi>1</sub_codi>
      <cup_tipo>cup_tipo</cup_tipo>
      <pvr_riva>pvr_riva</pvr_riva>
      <pvr_auto>pvr_auto</pvr_auto>
      <pvr_reau>pvr_reau</pvr_reau>
      <cup_tdba>cup_tdba</cup_tdba>
      <cup_tcta>cup_tcta</cup_tcta>
      <cup_ib04>cup_ib04</cup_ib04>
      <cup_ib05>cup_ib05</cup_ib05>
      <cup_ib06>cup_ib06</cup_ib06>
      <cup_ib07>cup_ib07</cup_ib07>
      <cup_ib08>cup_ib08</cup_ib08>
      <cup_ib09>cup_ib09</cup_ib09>
      <cup_ib10>cup_ib10</cup_ib10>
      <dep_dire>dep_dire</dep_dire>
      <dep_ntel>dep_ntel</dep_ntel>
      <dep_mail>dep_mail</dep_mail>
      <dep_codd>1</dep_codd>
      <cup_numi>cup_numi</cup_numi>
      <cup_nomi>cup_nomi</cup_nomi>
      <ban_coin>1</ban_coin>
      <ban_dire>ban_dire</ban_dire>
      <cup_coab>cup_coab</cup_coab>
      <cup_coib>cup_coib</cup_coib>
      <cup_coas>cup_coas</cup_coas>
      <cup_cobi>cup_cobi</cup_cobi>
      <cup_clas>cup_clas</cup_clas>
      <dep_apar>dep_apar</dep_apar>
      <dep_nfax>dep_nfax</dep_nfax>
      <dep_cloc>dep_cloc</dep_cloc>
      <dep_nomb>dep_nomb</dep_nomb>
      <cup_esta>cup_esta</cup_esta>
      <pvd_audp>pvd_audp</pvd_audp>
    </SyncPoPvdor>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncPoPvdorResponse xmlns="http://seven/">
      <SyncPoPvdorResult>&lt;PO_PVDOR&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;PVD_CODD&gt;900123&lt;/PVD_CODD&gt;&lt;/PO_PVDOR&gt;</SyncPoPvdorResult>
    </SyncPoPvdorResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

Etiquetas que lee GA2: `PVD_CODD` (entero, código del proveedor; si el texto contiene `PVD_CODD` se actualiza el tercero beneficiario), `RETORNO` y `TXTERROR`.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 22. `SyncFaConta`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/SyncFaConta` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncFaConta"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncFaConta` |
| Elemento raíz del response | `{http://seven/}SyncFaContaResponse` |
| Método del proxy (.NET) | `SyncFaContaAsync` |

**Propósito.** Sincroniza un contacto de cliente.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `cli_coda` | `xs:string` | — | `http://seven/` |
| `dcl_codd` | `xs:int` | — | `http://seven/` |
| `con_codi` | `xs:int` | — | `http://seven/` |
| `con_nomb` | `xs:string` | — | `http://seven/` |
| `con_carg` | `xs:string` | — | `http://seven/` |
| `con_dire` | `xs:string` | — | `http://seven/` |
| `con_dibo` | `xs:string` | — | `http://seven/` |
| `con_tele` | `xs:string` | — | `http://seven/` |
| `con_nufa` | `xs:string` | — | `http://seven/` |
| `con_mail` | `xs:string` | — | `http://seven/` |
| `con_fcum` | `xs:dateTime` | — | `http://seven/` |
| `con_afic` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncFaContaResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaConta xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <cli_coda>cli_coda</cli_coda>
      <dcl_codd>1</dcl_codd>
      <con_codi>1</con_codi>
      <con_nomb>con_nomb</con_nomb>
      <con_carg>con_carg</con_carg>
      <con_dire>con_dire</con_dire>
      <con_dibo>con_dibo</con_dibo>
      <con_tele>con_tele</con_tele>
      <con_nufa>con_nufa</con_nufa>
      <con_mail>con_mail</con_mail>
      <con_fcum>2026-01-15T00:00:00</con_fcum>
      <con_afic>con_afic</con_afic>
    </SyncFaConta>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncFaContaResponse xmlns="http://seven/">
      <SyncFaContaResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</SyncFaContaResult>
    </SyncFaContaResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 23. `SyncEmpleadoPvdor`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/SyncEmpleadoPvdor` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/SyncEmpleadoPvdor"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}SyncEmpleadoPvdor` |
| Elemento raíz del response | `{http://seven/}SyncEmpleadoPvdorResponse` |
| Método del proxy (.NET) | `SyncEmpleadoPvdorAsync` |

**Propósito.** Sincroniza un empleado como proveedor.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `tip_codi` | `xs:int` | — | `http://seven/` |
| `pvd_coda` | `xs:string` | — | `http://seven/` |
| `pvd_dive` | `xs:int` | — | `http://seven/` |
| `pvd_nomb` | `xs:string` | — | `http://seven/` |
| `pvd_apel` | `xs:string` | — | `http://seven/` |
| `pvd_noco` | `xs:string` | — | `http://seven/` |
| `pai_codi` | `xs:int` | — | `http://seven/` |
| `dep_codi` | `xs:int` | — | `http://seven/` |
| `mun_codi` | `xs:int` | — | `http://seven/` |
| `arb_csuc` | `xs:string` | — | `http://seven/` |
| `cup_nume` | `xs:string` | — | `http://seven/` |
| `ban_codi` | `xs:int` | — | `http://seven/` |
| `sub_codi` | `xs:int` | — | `http://seven/` |
| `cup_tipo` | `xs:string` | — | `http://seven/` |
| `dep_dire` | `xs:string` | — | `http://seven/` |
| `dep_ntel` | `xs:string` | — | `http://seven/` |
| `dep_mail` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `SyncEmpleadoPvdorResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncEmpleadoPvdor xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <tip_codi>1</tip_codi>
      <pvd_coda>pvd_coda</pvd_coda>
      <pvd_dive>1</pvd_dive>
      <pvd_nomb>pvd_nomb</pvd_nomb>
      <pvd_apel>pvd_apel</pvd_apel>
      <pvd_noco>pvd_noco</pvd_noco>
      <pai_codi>1</pai_codi>
      <dep_codi>1</dep_codi>
      <mun_codi>1</mun_codi>
      <arb_csuc>arb_csuc</arb_csuc>
      <cup_nume>cup_nume</cup_nume>
      <ban_codi>1</ban_codi>
      <sub_codi>1</sub_codi>
      <cup_tipo>cup_tipo</cup_tipo>
      <dep_dire>dep_dire</dep_dire>
      <dep_ntel>dep_ntel</dep_ntel>
      <dep_mail>dep_mail</dep_mail>
    </SyncEmpleadoPvdor>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <SyncEmpleadoPvdorResponse xmlns="http://seven/">
      <SyncEmpleadoPvdorResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</SyncEmpleadoPvdorResult>
    </SyncEmpleadoPvdorResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 24. `ChangeNit`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/ChangeNit` (pass-through, `SevenEndpoints`); sin otros consumidores. Ojo: en `IConsumoWSProveedor` el método se llama `InsertarCnMcontAsync` pero invoca `ChangeNit` |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/ChangeNit"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}ChangeNit` |
| Elemento raíz del response | `{http://seven/}ChangeNitResponse` |
| Método del proxy (.NET) | `ChangeNitAsync` |

**Propósito.** Cambia el NIT/identificación de un tercero.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `ter_coda` | `xs:string` | — | `http://seven/` |
| `ter_codn` | `xs:string` | — | `http://seven/` |
| `tip_codn` | `xs:int` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `ChangeNitResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ChangeNit xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <ter_coda>ter_coda</ter_coda>
      <ter_codn>ter_codn</ter_codn>
      <tip_codn>1</tip_codn>
    </ChangeNit>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ChangeNitResponse xmlns="http://seven/">
      <ChangeNitResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</ChangeNitResult>
    </ChangeNitResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 25. `ActModalidadCupo`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/ActModalidadCupo` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSProveedor}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/ActModalidadCupo"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}ActModalidadCupo` |
| Elemento raíz del response | `{http://seven/}ActModalidadCupoResponse` |
| Método del proxy (.NET) | `ActModalidadCupoAsync` |

**Propósito.** Actualiza la modalidad de cupo de un proveedor.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `cli_coda` | `xs:string` | — | `http://seven/` |
| `arb_codc` | `xs:string` | — | `http://seven/` |
| `ite_moda` | `xs:string` | — | `http://seven/` |
| `dcl_cupo` | `xs:double` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `ActModalidadCupoResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ActModalidadCupo xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <cli_coda>cli_coda</cli_coda>
      <arb_codc>arb_codc</arb_codc>
      <ite_moda>ite_moda</ite_moda>
      <dcl_cupo>1.5</dcl_cupo>
    </ActModalidadCupo>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ActModalidadCupoResponse xmlns="http://seven/">
      <ActModalidadCupoResult>&lt;RESULTADO&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/RESULTADO&gt;</ActModalidadCupoResult>
    </ActModalidadCupoResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.5 Terceros — `SGNTERCESoap` (wgnterce.asmx)

URL: `SvcUtils:SevenWSTercero` → `http://cpvmp-306x/Seven/Wstercero/wgnterce.asmx`

### 26. `InsertarGnTerce`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada por negocio** + pass-through |
| Transporte | `POST {SvcUtils:SevenWSTercero}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/InsertarGnTerce"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://seven/}InsertarGnTerce` |
| Elemento raíz del response | `{http://seven/}InsertarGnTerceResponse` |
| Método del proxy (.NET) | `InsertarGnTerceAsync` |

**Propósito.** Crea el tercero (persona/afiliado) en Seven antes de crear proveedor, CDP y factura.

**Quién la invoca en GA2.** Crédito (`CrearTerceroSevenCredito`, desembolsos), Cesantías, Hipoteca, Pagos de embargos, Reintegros.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `emp_codi` | `xs:int` | — | `http://seven/` |
| `tip_codi` | `xs:int` | — | `http://seven/` |
| `ter_coda` | `xs:string` | — | `http://seven/` |
| `ter_dive` | `xs:int` | — | `http://seven/` |
| `ter_nomb` | `xs:string` | — | `http://seven/` |
| `ter_apel` | `xs:string` | — | `http://seven/` |
| `ter_nocm` | `xs:string` | — | `http://seven/` |
| `mod_codi` | `xs:int` | — | `http://seven/` |
| `pai_codi` | `xs:int` | — | `http://seven/` |
| `dep_codi` | `xs:int` | — | `http://seven/` |
| `mun_codi` | `xs:int` | — | `http://seven/` |
| `ter_dire` | `xs:string` | — | `http://seven/` |
| `ter_ntel` | `xs:string` | — | `http://seven/` |
| `ter_mail` | `xs:string` | — | `http://seven/` |
| `ter_nfax` | `xs:string` | — | `http://seven/` |
| `arb_clte` | `xs:string` | — | `http://seven/` |
| `ter_audp` | `xs:string` | — | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `InsertarGnTerceResult` | `xs:string` | — | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarGnTerce xmlns="http://seven/">
      <emp_codi>1</emp_codi>
      <tip_codi>1</tip_codi>
      <ter_coda>ter_coda</ter_coda>
      <ter_dive>1</ter_dive>
      <ter_nomb>ter_nomb</ter_nomb>
      <ter_apel>ter_apel</ter_apel>
      <ter_nocm>ter_nocm</ter_nocm>
      <mod_codi>1</mod_codi>
      <pai_codi>1</pai_codi>
      <dep_codi>1</dep_codi>
      <mun_codi>1</mun_codi>
      <ter_dire>ter_dire</ter_dire>
      <ter_ntel>ter_ntel</ter_ntel>
      <ter_mail>ter_mail</ter_mail>
      <ter_nfax>ter_nfax</ter_nfax>
      <arb_clte>arb_clte</arb_clte>
      <ter_audp>ter_audp</ter_audp>
    </InsertarGnTerce>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (estructura validada contra el cliente; contenido de `...Result` realista según las etiquetas que lee GA2):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <InsertarGnTerceResponse xmlns="http://seven/">
      <InsertarGnTerceResult>&lt;GN_TERCE&gt;&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;&lt;/GN_TERCE&gt;</InsertarGnTerceResult>
    </InsertarGnTerceResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

El `...Result` es un **`xs:string` que contiene un documento XML escapado** (en el sobre aparece como `&lt;...&gt;`). GA2 lo carga con `XmlDocument.LoadXml` y lee etiquetas con `GetElementsByTagName` (o por regex), por lo que:

- debe ser **XML bien formado** (si no, `XmlException` → HTTP 409 en la API de GA2);
- el nombre del elemento raíz **no** se valida (se proponen nombres de tabla Seven, a confirmar con el servicio real), salvo en `InsertarGnTerce`, donde el flujo de desembolso usa la ruta `//GN_TERCE/RETORNO`;
- `RETORNO` = `0` → éxito; `RETORNO` = `1` → error de negocio, con el texto en `TXTERROR`.

En el **desembolso de crédito** (`CreditoBusinessLogic`, ≈ línea 7685) se usa `SelectSingleNode("//GN_TERCE/RETORNO")` y `//GN_TERCE/TXTERROR`, así que aquí **la raíz debe ser `GN_TERCE`**. Si `RETORNO = 1` y `TXTERROR` contiene *"ya se encuentra registrado"*, el error se tolera (el tercero ya existe) y el flujo continúa; con cualquier otro texto el desembolso se detiene. En el resto de flujos el texto sólo se muestra como mensaje o se convierte a JSON.

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.6 Cuentas contables por tipo de operación — `SINTIPROSoap` (WInTipro.asmx)

URL: `SvcUtils:SevenWintipro` → `http://cpvmp-306x/Seven/Wintipro/WInTipro.asmx`

### 27. `GetCuentaContable`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/GetCuentaContable` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWintipro}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/GetCuentaContable"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `GetCuentaContableAsync` |

**Propósito.** Consulta la cuenta contable de un tipo de operación.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetCuentaContable/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `GetCuentaContable/pro_codi` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `GetCuentaContable/dsp_codi` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetCuentaContableResponse/GetCuentaContableResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetCuentaContable xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <pro_codi>pro_codi</pro_codi>
      <dsp_codi>dsp_codi</dsp_codi>
    </GetCuentaContable>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetCuentaContableResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <GetCuentaContableResult>GetCuentaContableResult</GetCuentaContableResult>
    </GetCuentaContableResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).

### 28. `GetCuentaContableTO`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/GetCuentaContableTO` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWintipro}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/GetCuentaContableTO"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `GetCuentaContableTOAsync` |

**Propósito.** Consulta cuentas contables por tipo de operación (variante TO).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetCuentaContableTO/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `GetCuentaContableTO/top_codi` | `xs:int` | siempre presente | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetCuentaContableTOResponse/GetCuentaContableTOResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetCuentaContableTO xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <top_codi>1</top_codi>
    </GetCuentaContableTO>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetCuentaContableTOResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <GetCuentaContableTOResult>GetCuentaContableTOResult</GetCuentaContableTOResult>
    </GetCuentaContableTOResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


## 3.7 Aportes — `ValidarAportesSoap` (ValidarAportes.asmx)

URL: `SvcUtils:SevenWSAportes` → `http://cpvmp-306x/Seven/WSaportes/ValidarAportes.asmx`

### 29. `GetAporte`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | Expuesta tal cual en `POST api/credito/GetAporte` (pass-through, `SevenEndpoints`); sin otros consumidores |
| Transporte | `POST {SvcUtils:SevenWSAportes}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://seven/GetAporte"` |
| Estilo | document/literal wrapped (`DataContractSerializer`) |
| Método del proxy (.NET) | `GetAporteAsync` |

**Propósito.** Consulta/valida aportes de un afiliado.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetAporte/emp_codi` | `xs:int` | siempre presente | `http://seven/` |
| `GetAporte/top_codi` | `xs:int` | siempre presente | `http://seven/` |
| `GetAporte/mte_nume` | `xs:int` | siempre presente | `http://seven/` |
| `GetAporte/mte_anop` | `xs:int` | siempre presente | `http://seven/` |
| `GetAporte/ter_coda` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `GetAporte/arb_coda` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `GetAporte/cie_codi` | `xs:int` | siempre presente | `http://seven/` |
| `GetAporte/ind_clas` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |
| `GetAporte/ind_tipo` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GetAporteResponse/GetAporteResult` | `xs:string` | opcional (se omite si es nulo/0) | `http://seven/` |

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetAporte xmlns="http://seven/" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <emp_codi>1</emp_codi>
      <top_codi>1</top_codi>
      <mte_nume>1</mte_nume>
      <mte_anop>1</mte_anop>
      <ter_coda>ter_coda</ter_coda>
      <arb_coda>arb_coda</arb_coda>
      <cie_codi>1</cie_codi>
      <ind_clas>ind_clas</ind_clas>
      <ind_tipo>ind_tipo</ind_tipo>
    </GetAporte>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado: el cliente WCF de GA2 lo deserializa sin error):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <GetAporteResponse xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://seven/">
      <GetAporteResult>GetAporteResult</GetAporteResult>
    </GetAporteResponse>
  </s:Body>
</s:Envelope>
```

**Errores y casos alternos.**

Ver §2.4 (errores técnicos comunes).


---

# 4. Tipos de datos por servicio

## 4.1 Movimientos contables — `SCNMCONTSoap` (scnmcont.asmx)

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `TRetToper`

Tipo XML `TRetToper` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Mco_secu` | `xs:int` | obligatorio |  |
| 2 | `Retorno` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 3 | `TxtError` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `Mco_cont` | `xs:int` | obligatorio |  |
| 5 | `Mco_nume` | `xs:int` | obligatorio |  |

### `TSCnDmcon`

Tipo XML `TSCnDmcon` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Cue_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Dmc_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 3 | `Dmc_cant` | `xs:double` | obligatorio |  |
| 4 | `Dmc_acti` | `xs:double` | obligatorio |  |
| 5 | `Dmc_refe` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `Dmc_vadb` | `xs:decimal` | obligatorio |  |
| 7 | `Dmc_vacr` | `xs:decimal` | obligatorio |  |
| 8 | `Dmc_vaba` | `xs:decimal` | obligatorio |  |
| 9 | `Ter_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 10 | `Arb_codc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Arb_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 12 | `Arb_codp` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 13 | `Arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 14 | `Ter_codm` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TSCnMcont`

Tipo XML `TSCnMcont` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDetalle` | lista de `TSCnDmcon` | opcional (se omite si es nulo/0) |  |
| 2 | `Emp_codi` | `xs:int` | obligatorio |  |
| 3 | `Top_codi` | `xs:int` | obligatorio |  |
| 4 | `Mco_nume` | `xs:int` | obligatorio |  |
| 5 | `Mco_fech` | `xs:dateTime` | obligatorio |  |
| 6 | `Mod_codi` | `xs:int` | obligatorio |  |
| 7 | `Arb_csuc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 8 | `Mco_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TToper`

Tipo XML `TToper` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Mco_secu` | `xs:int` | obligatorio |  |
| 2 | `Emp_codi` | `xs:int` | obligatorio |  |
| 3 | `Top_codi` | `xs:int` | obligatorio |  |
| 4 | `Mco_nume` | `xs:int` | obligatorio |  |
| 5 | `Mco_nech` | `xs:double` | obligatorio |  |
| 6 | `Arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 7 | `Ter_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 8 | `Mco_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 9 | `Mco_valo` | `xs:decimal` | obligatorio |  |
| 10 | `Arb_codc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Arb_codp` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 12 | `Arb_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |


## 4.2 Presupuesto / CDP — `SPGOBLIGSoap` (WPgOblig.asmx)

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `TSPgDmpre`

Tipo XML `TSPgDmpre` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Rub_codi` | `xs:string` | opcional (minOccurs=0) |  |
| 2 | `Pro_codi` | `xs:string` | opcional (minOccurs=0) |  |
| 3 | `Arb_codc` | `xs:string` | opcional (minOccurs=0) |  |
| 4 | `Arb_coda` | `xs:string` | opcional (minOccurs=0) |  |
| 5 | `Arb_codp` | `xs:string` | opcional (minOccurs=0) |  |
| 6 | `Dmp_valo` | `xs:double` | siempre presente |  |
| 7 | `Dmp_desc` | `xs:string` | opcional (minOccurs=0) |  |

### `TSPgMpres`

Tipo XML `TSPgMpres` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Emp_codi` | `xs:int` | siempre presente |  |
| 2 | `Top_codi` | `xs:int` | siempre presente |  |
| 3 | `Mpr_nume` | `xs:int` | siempre presente |  |
| 4 | `Mpr_fech` | `xs:dateTime` | siempre presente |  |
| 5 | `Mpr_desc` | `xs:string` | opcional (minOccurs=0) |  |
| 6 | `Arb_csuc` | `xs:string` | opcional (minOccurs=0) |  |
| 7 | `Ter_coda` | `xs:string` | opcional (minOccurs=0) |  |
| 8 | `Mpr_tdos` | `xs:string` | opcional (minOccurs=0) |  |
| 9 | `Mpr_ndos` | `xs:int` | siempre presente |  |
| 10 | `Uni_codi` | `xs:int` | siempre presente |  |
| 11 | `vDetalle` | lista de `TSPgDmpre` | opcional (minOccurs=0) |  |


## 4.3 Facturas / cuentas por pagar — `SPOFACTUSoap` (spofactu.asmx)

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `TOPoDmfac`

Tipo XML `TOPoDmfac` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `arb_codc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `arb_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 3 | `arb_codp` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `dfa_dest` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `dmf_tipo` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 7 | `dmf_valo` | `xs:double` | obligatorio |  |
| 8 | `dmf_porc` | `xs:double` | obligatorio |  |

### `TSCmRdoba2`

Tipo XML `TSCmRdoba2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Mpr_cont` | `xs:int` | obligatorio |  |
| 2 | `Dmp_cont` | `xs:int` | obligatorio |  |
| 3 | `Dmp_valo` | `xs:double` | obligatorio |  |

### `TSPoCruat1`

Tipo XML `TSPoCruat1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Top_ccru` | `xs:int` | obligatorio |  |
| 2 | `Fac_ncru` | `xs:int` | obligatorio |  |
| 3 | `Fac_fcru` | `xs:dateTime` | obligatorio |  |
| 4 | `Suc_ccru` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Cru_valo` | `xs:double` | obligatorio |  |
| 6 | `Cxp_cont` | `xs:int` | obligatorio |  |

### `TSPoCruat2`

Tipo XML `TSPoCruat2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Top_ccru` | `xs:int` | obligatorio |  |
| 2 | `Fac_ncru` | `xs:int` | obligatorio |  |
| 3 | `Fac_fcru` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `Suc_ccru` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Cru_valo` | `xs:double` | obligatorio |  |
| 6 | `Cxp_cont` | `xs:int` | obligatorio |  |

### `TSPoCtrlo2`

Tipo XML `TSPoCtrlo2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Top_base` | `xs:int` | obligatorio |  |
| 2 | `Bas_nume` | `xs:int` | obligatorio |  |
| 3 | `Bas_fech` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `Bas_csuc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Bas_cont` | `xs:int` | obligatorio |  |

### `TSPoDanti1`

Tipo XML `TSPoDanti1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDAntic` | lista de `TSPoDlant1` | opcional (se omite si es nulo/0) |  |
| 2 | `Top_cant` | `xs:int` | obligatorio |  |
| 3 | `Fac_nant` | `xs:int` | obligatorio |  |
| 4 | `Fac_fant` | `xs:dateTime` | obligatorio |  |
| 5 | `Suc_cant` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `Tip_lega` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TSPoDanti2`

Tipo XML `TSPoDanti2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDAntic` | lista de `TSPoDlant2` | opcional (se omite si es nulo/0) |  |
| 2 | `Top_cant` | `xs:int` | obligatorio |  |
| 3 | `Fac_nant` | `xs:int` | obligatorio |  |
| 4 | `Fac_fant` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Suc_cant` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `Tip_lega` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TSPoDcoad1`

Tipo XML `TSPoDcoad1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Coa_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Coa_valo` | `xs:double` | obligatorio |  |

### `TSPoDcoad2`

Tipo XML `TSPoDcoad2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Coa_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Coa_valo` | `xs:double` | obligatorio |  |

### `TSPoDdisp1`

Tipo XML `TSPoDdisp1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Arb_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Tar_codi` | `xs:int` | obligatorio |  |
| 3 | `Ddi_porc` | `xs:double` | obligatorio |  |

### `TSPoDdisp2`

Tipo XML `TSPoDdisp2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Arb_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Tar_codi` | `xs:int` | obligatorio |  |
| 3 | `Ddi_tipo` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `Ddi_valo` | `xs:double` | obligatorio |  |
| 5 | `Ddi_porc` | `xs:double` | obligatorio |  |

### `TSPoDfact`

Tipo XML `TSPoDfact` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Pro_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Dfa_dest` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 3 | `Dfa_valo` | `xs:double` | obligatorio |  |
| 4 | `Arb_codc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Arb_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `Arb_codp` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TSPoDfact1`

Tipo XML `TSPoDfact1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Bod_codi` | `xs:int` | obligatorio |  |
| 2 | `Pro_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 3 | `Dfa_dest` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 4 | `Dfa_cant` | `xs:double` | obligatorio |  |
| 5 | `Dfa_valo` | `xs:double` | obligatorio |  |
| 6 | `Dfa_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 7 | `Ter_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 8 | `Dfa_refe` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 9 | `Pvd_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 10 | `Dep_codd` | `xs:int` | obligatorio |  |
| 11 | `vDistrib` | lista de `TSPoDdisp1` | opcional (se omite si es nulo/0) |  |
| 12 | `VDistribM` | lista de `TOPoDmfac` | opcional (se omite si es nulo/0) |  |

### `TSPoDfact2`

Tipo XML `TSPoDfact2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDistrib` | lista de `TSPoDdisp2` | opcional (se omite si es nulo/0) |  |
| 2 | `vDistribm` | lista de `TOPoDmfac` | opcional (se omite si es nulo/0) |  |
| 3 | `Bod_codi` | `xs:int` | obligatorio |  |
| 4 | `Pro_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 5 | `Dfa_dest` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 6 | `Dfa_cant` | `xs:double` | obligatorio |  |
| 7 | `Dfa_valo` | `xs:double` | obligatorio |  |
| 8 | `Dfa_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 9 | `Ter_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 10 | `Dfa_refe` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Ctr_cont` | `xs:int` | obligatorio |  |
| 12 | `Pvd_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 13 | `Dep_codd` | `xs:int` | obligatorio |  |

### `TSPoDlant1`

Tipo XML `TSPoDlant1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Dvt_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Dla_lega` | `xs:double` | obligatorio |  |

### `TSPoDlant2`

Tipo XML `TSPoDlant2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Dvt_codi` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 2 | `Dla_lega` | `xs:double` | obligatorio |  |

### `TSPoFactu`

Tipo XML `TSPoFactu` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDetalle` | lista de `TSPoDfact` | opcional (se omite si es nulo/0) |  |
| 2 | `Emp_codi` | `xs:int` | obligatorio |  |
| 3 | `Top_codi` | `xs:int` | obligatorio |  |
| 4 | `Fac_nume` | `xs:int` | obligatorio |  |
| 5 | `Fac_fech` | `xs:dateTime` | obligatorio |  |
| 6 | `Arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 7 | `Fac_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 8 | `Pvd_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 9 | `Dep_codd` | `xs:int` | obligatorio |  |
| 10 | `Fac_pref` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Fac_nfap` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 12 | `Mon_codi` | `xs:int` | obligatorio |  |
| 13 | `Fac_tipo` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 14 | `Mpr_cont` | `xs:int` | obligatorio |  |
| 15 | `Fac_feta` | `xs:dateTime` | opcional (se omite si es nulo/0) |  |

### `TSPoFactu1`

Tipo XML `TSPoFactu1` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDetalle` | lista de `TSPoDfact1` | opcional (se omite si es nulo/0) |  |
| 2 | `vCAdic` | lista de `TSPoDcoad1` | opcional (se omite si es nulo/0) |  |
| 3 | `vAnticipo` | lista de `TSPoDanti1` | opcional (se omite si es nulo/0) |  |
| 4 | `vCruceAut` | lista de `TSPoCruat1` | opcional (se omite si es nulo/0) |  |
| 5 | `Emp_codi` | `xs:int` | obligatorio |  |
| 6 | `Top_codi` | `xs:int` | obligatorio |  |
| 7 | `Fac_nume` | `xs:int` | obligatorio |  |
| 8 | `Fac_fech` | `xs:dateTime` | obligatorio |  |
| 9 | `Arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 10 | `Fac_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Pvd_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 12 | `Dep_codd` | `xs:int` | obligatorio |  |
| 13 | `Fac_tipo` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 14 | `Fac_pref` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 15 | `Fac_nfap` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 16 | `Fac_esta` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 17 | `Mon_codi` | `xs:int` | obligatorio |  |
| 18 | `Fac_base` | `xs:int` | obligatorio |  |
| 19 | `Bir_cont` | `xs:double` | obligatorio |  |
| 20 | `Fac_auto` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 21 | `Fac_mulc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 22 | `Cal_impu` | `xs:int` | obligatorio |  |
| 23 | `Fac_mfev` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 24 | `Fac_feve` | `xs:dateTime` | obligatorio |  |
| 25 | `Fac_tdis` | `xs:string` | opcional (se omite si es nulo/0) |  |

### `TSPoFactu2`

Tipo XML `TSPoFactu2` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `vDetalle` | lista de `TSPoDfact2` | opcional (se omite si es nulo/0) |  |
| 2 | `vCAdic` | lista de `TSPoDcoad2` | opcional (se omite si es nulo/0) |  |
| 3 | `vAnticipo` | lista de `TSPoDanti2` | opcional (se omite si es nulo/0) |  |
| 4 | `vCruceAut` | lista de `TSPoCruat2` | opcional (se omite si es nulo/0) |  |
| 5 | `vDocBase` | lista de `TSPoCtrlo2` | opcional (se omite si es nulo/0) |  |
| 6 | `vDocPres` | lista de `TSCmRdoba2` | opcional (se omite si es nulo/0) |  |
| 7 | `Emp_codi` | `xs:int` | obligatorio |  |
| 8 | `Top_codi` | `xs:int` | obligatorio |  |
| 9 | `Fac_nume` | `xs:int` | obligatorio |  |
| 10 | `Fac_fech` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 11 | `Arb_cods` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 12 | `Fac_desc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 13 | `Pvd_coda` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 14 | `Dep_codd` | `xs:int` | obligatorio |  |
| 15 | `Fac_tipo` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 16 | `Fac_orig` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 17 | `Fac_pref` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 18 | `Fac_nfap` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 19 | `Fac_esta` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 20 | `Mon_codi` | `xs:int` | obligatorio |  |
| 21 | `Bir_cont` | `xs:double` | obligatorio |  |
| 22 | `Fac_auto` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 23 | `Fac_refe` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 24 | `Fac_mulc` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 25 | `Cal_impu` | `xs:int` | obligatorio |  |
| 26 | `Fac_mfev` | `xs:string` | opcional (se omite si es nulo/0) |  |
| 27 | `Fac_feve` | `xs:dateTime` | obligatorio |  |
| 28 | `Fac_fpse` | `xs:dateTime` | obligatorio |  |


## 4.4 Proveedores / clientes / terceros — `SIEWSSECSoap` (SIeWssec.asmx)

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `TOFaClien`

Tipo XML `TOFaClien` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `emp_codi` | `xs:int` | siempre presente |  |
| 2 | `tip_codi` | `xs:int` | siempre presente |  |
| 3 | `ter_coda` | `xs:string` | opcional (minOccurs=0) |  |
| 4 | `cli_dive` | `xs:int` | siempre presente |  |
| 5 | `cli_nomb` | `xs:string` | opcional (minOccurs=0) |  |
| 6 | `cli_apel` | `xs:string` | opcional (minOccurs=0) |  |
| 7 | `cli_noco` | `xs:string` | opcional (minOccurs=0) |  |
| 8 | `mod_codi` | `xs:int` | siempre presente |  |
| 9 | `pai_codi` | `xs:int` | siempre presente |  |
| 10 | `dep_codi` | `xs:int` | siempre presente |  |
| 11 | `mun_codi` | `xs:int` | siempre presente |  |
| 12 | `cli_coda` | `xs:string` | opcional (minOccurs=0) |  |
| 13 | `tcl_codi` | `xs:int` | siempre presente |  |
| 14 | `cal_codi` | `xs:int` | siempre presente |  |
| 15 | `coc_codi` | `xs:int` | siempre presente |  |
| 16 | `cim_codi` | `xs:int` | siempre presente |  |
| 17 | `arb_csuc` | `xs:string` | opcional (minOccurs=0) |  |
| 18 | `dcl_dire` | `xs:string` | opcional (minOccurs=0) |  |
| 19 | `dcl_ntel` | `xs:string` | opcional (minOccurs=0) |  |
| 20 | `dcl_mail` | `xs:string` | opcional (minOccurs=0) |  |
| 21 | `dcl_nfax` | `xs:string` | opcional (minOccurs=0) |  |
| 22 | `arb_clte` | `xs:string` | opcional (minOccurs=0) |  |
| 23 | `cli_inna` | `xs:string` | opcional (minOccurs=0) |  |
| 24 | `ven_codi` | `xs:double` | siempre presente |  |
| 25 | `arb_ccec` | `xs:string` | opcional (minOccurs=0) |  |
| 26 | `lis_codi` | `xs:int` | siempre presente |  |
| 27 | `dcl_obse` | `xs:string` | opcional (minOccurs=0) |  |
| 28 | `dcl_apar` | `xs:string` | opcional (minOccurs=0) |  |
| 29 | `dcl_cloc` | `xs:string` | opcional (minOccurs=0) |  |
| 30 | `cli_anex` | `xs:string` | opcional (minOccurs=0) |  |
| 31 | `cli_hpra` | `xs:string` | opcional (minOccurs=0) |  |
| 32 | `cli_fecm` | `xs:int` | siempre presente |  |
| 33 | `cli_feca` | `xs:int` | siempre presente |  |
| 34 | `cli_audp` | `xs:string` | opcional (minOccurs=0) |  |
| 35 | `arb_cpro` | `xs:string` | opcional (minOccurs=0) |  |
| 36 | `arb_care` | `xs:string` | opcional (minOccurs=0) |  |
| 37 | `act_codi` | `xs:string` | opcional (minOccurs=0) |  |

### `TSalida`

Tipo XML `TSalida` — namespace `http://seven/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Retorno` | `xs:short` | siempre presente |  |
| 2 | `TxtError` | `xs:string` | opcional (minOccurs=0) |  |
| 3 | `CliCodi` | `xs:int` | siempre presente |  |
| 4 | `DclCodd` | `xs:short` | siempre presente |  |



---

## 5. Escenarios mínimos recomendados para el mock

| # | Escenario | Operación(es) | Respuesta del mock |
|---|---|---|---|
| 1 | Pago completo exitoso | `InsertarGnTerce`, `SyncPoPvdor`, `CrearCDP`, `InsertarFacturaP`, `InsertarMovContable` | `RETORNO=0` con los identificadores de la §2.2 |
| 2 | Tercero ya existente | `InsertarGnTerce` | `<GN_TERCE><RETORNO>1</RETORNO><TXTERROR>El tercero ya se encuentra registrado</TXTERROR></GN_TERCE>` (GA2 continúa) |
| 3 | Error de negocio en CDP | `CrearCDP` | `RETORNO=1`, `TXTERROR="Rubro sin saldo"` (GA2 detiene el pago y muestra el texto) |
| 4 | Factura sin identificadores | `InsertarFacturaP` | `RETORNO=0` sin `FAC_CONT` → reproduce el `NullReferenceException` de GA2 (prueba de robustez) |
| 5 | Falla técnica | cualquiera | SOAP Fault HTTP 500 |
| 6 | Lentitud | cualquiera | respuesta con retardo (GA2 no tiene timeout: medir el impacto) |
| 7 | Anulación | `AnularCnMcont` | `<CN_MCONT><RETORNO>0</RETORNO></CN_MCONT>` y variante `RETORNO=1` |
