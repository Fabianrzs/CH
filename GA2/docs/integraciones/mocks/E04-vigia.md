# E4 — Vigía: verificación de terceros en listas restrictivas (SARLAFT)

> Documento para implementar un **mock compatible** con el servicio PHP `SOAP_VerificaTercero.php`. El servicio usa el estilo **RPC/encoded** (SOAP 1.1 sección 5), poco habitual: los sobres de este documento se capturaron y validaron con el proxy WCF real.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Sistema | Vigía (Caja Honor): verificación de personas en listas LA/FT |
| Protocolo | SOAP 1.1, **RPC/encoded** (`XmlSerializerFormat(Style=Rpc, Use=Encoded)`) |
| URL | `SvcUtils:VigiaVerificarTerceroUrlSvc`. En la API GA2 apunta a `https://vigia.cajahonor.gov.co:443/WebSvc/SOAP_VerificaTercero.php`; en la API BUA, al ASMX `http://cpvmp-22fw:8080/...` (ver §4) |
| Contrato | `ApplicationServicesPortType`, namespace `https://vigia.cajahonor.gov.co/WebSvc` |
| Binding cliente | `BasicHttpBinding`, `Transport` si la URL es `https` (el mock debe exponer HTTPS con un certificado de confianza, o configurar la URL `http`), timeouts `TimeSpan.MaxValue`, `MaxReceivedMessageSize` 1 MB |
| Cliente .NET | `Connected Services/ServiceVigiaVerificacionTercero/VerificacionTerceroVigiaBusinessLogic.cs` |
| Toggle en BD | `VIGIA` / `ESTADO`. Con `false` **no se llama**: se responde con lo que devuelva el SP `ConsultarResponseVigiaDesactivado` (`VIG_ENCONTRADO`, `VIG_LISTAS`, `VIG_METODO`) |
| Auditoría | No pasa por `EjecutarServicio` (sin registro en BD) |

## 2. Operación `F_VERIFICATERCERO`

| Propiedad | Valor |
|---|---|
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"https://vigia.cajahonor.gov.co/WebSvc/SOAP_VerificaTercero.php/F_VERIFICATERCERO"` |
| Estilo | RPC/encoded (`s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/"`) |

**Quién la invoca.** `VigiaBusinessLogic.ServiceVertificacionTerceroAsync` (endpoint `POST api/credito/verificacionTercero`), `ProspeccionBusinessLogic` (≈10601) y `SolicitudBusinessLogic` (≈1326).

### 2.1 Request

| Parámetro | Tipo | Valor que envía GA2 |
|---|---|---|
| `param1` | `xsd:string` | `Pe_Dato`: número de documento a verificar |
| `param2` | `xsd:string` | `Pe_TipoVerificacion`: tipo de verificación que llega desde el frontend/negocio (dominio de valores a confirmar con Vigía) |
| `param3` | `xsd:string` | `Pe_Porcentaje`: porcentaje de coincidencia; **cadena vacía** si no se indica |

Los parámetros van **sin calificar** y con `xsi:type`. El elemento de la operación está calificado con el namespace del servicio:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <q1:F_VERIFICATERCERO xmlns:q1="https://vigia.cajahonor.gov.co/WebSvc">
      <param1 xsi:type="xsd:string">param1</param1>
      <param2 xsi:type="xsd:string">param2</param2>
      <param3 xsi:type="xsd:string">param3</param3>
    </q1:F_VERIFICATERCERO>
  </s:Body>
</s:Envelope>
```

### 2.2 Response

Elemento `F_VERIFICATERCEROResponse` (namespace del servicio) con una parte `return` de tipo `Resultado` (`tns:Resultado`), con tres `xsd:string`:

| Elemento | Uso en GA2 |
|---|---|
| `Encontrado` | Se devuelve al frontend tal cual (`Resultado.Encontrado`); el frontend decide. Valores reales a confirmar (en el ejemplo `S`/`N`) |
| `Listas` | Listas en las que aparece la persona (texto libre) |
| `Metodo` | Método de coincidencia (texto libre) |

**Response de ejemplo** (validado contra el cliente WCF; los valores son ilustrativos):

```xml
<s:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <ns1:F_VERIFICATERCEROResponse s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/" xmlns:ns1="https://vigia.cajahonor.gov.co/WebSvc">
      <return xmlns:tns="https://vigia.cajahonor.gov.co/WebSvc" xsi:type="tns:Resultado">
        <Encontrado xsi:type="xsd:string">S</Encontrado>
        <Metodo xsi:type="xsd:string">DOCUMENTO</Metodo>
        <Listas xsi:type="xsd:string">OFAC, ONU</Listas>
      </return>
    </ns1:F_VERIFICATERCEROResponse>
  </s:Body>
</s:Envelope>
```

> **Requisitos del formato RPC/encoded verificados:** el elemento `F_VERIFICATERCEROResponse` debe estar en el namespace `https://vigia.cajahonor.gov.co/WebSvc`, `return` y sus hijos **sin calificar**, y `xsi:type="tns:Resultado"` con `tns` apuntando al mismo namespace. Si `return` lleva un `xsi:type` vacío o desconocido, WCF lo deserializa como `XmlNode[]` y la llamada falla (`ArgumentException` en el proxy); se comprobó durante la validación.

### 2.3 Errores

- Cualquier excepción (Fault, red, formato) la captura `VigiaBusinessLogic` y devuelve `IsSuccess=false` con *"Error en el servicio de vigía por favor intentar más tarde."*
- `ProspeccionBusinessLogic` envuelve la llamada en `try/catch`; `SolicitudBusinessLogic` (≈1326) **no**: una falla sube y termina en HTTP 409 (Conflict) vía `ErrorValidationMiddleware`.
- Sin timeout efectivo (`TimeSpan.MaxValue`).

## 3. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Persona sin coincidencias | `Encontrado=N`, `Listas` vacío |
| 2 | Persona reportada | `Encontrado=S`, `Listas="OFAC"`, `Metodo="DOCUMENTO"` |
| 3 | Falla del servicio | SOAP Fault (GA2 muestra el mensaje genérico) |
| 4 | Toggle apagado | Sin llamada: verificar que GA2 usa el SP `ConsultarResponseVigiaDesactivado` |

## 4. Contratos Vigía ASMX registrados pero **sin uso** (no requieren mock)

`VigiaBusinessLogic` inyecta estos clientes, pero sus métodos no tienen llamadas (`CargarTerceroAsync`, `CargaProductoTerceroAsync`) o lanzan `NotImplementedException` (`CargarTransanccionAsync`, `VertificacionTerceroAsync`). Se listan por si se reactivan:

| Operación | Clave de URL | Servicio | SOAPAction | Estilo |
|---|---|---|---|---|

| `F_CARGATERCERO` | `SvcUtils:VigiaCargaTerceroUrlSvc` | `SOAP_CargaTercero.asmx` | `"http://tempuri.org/F_CARGATERCERO"` | document/literal wrapped (`XmlSerializer`) |
| `F_CARGAPRODUCTOTERCERO` | `SvcUtils:VigiaCargaProductosTerceroUrlSvc` | `SOAP_CargaProductosTercero.asmx` | `"http://tempuri.org/F_CARGAPRODUCTOTERCERO"` | document/literal wrapped (`XmlSerializer`) |
| `F_CARGATRANSACCION` | `SvcUtils:VigiaCargaTransaccionUrlSvc` | `SOAP_CargaTransaccion.asmx` | `"http://tempuri.org/F_CARGATRANSACCION"` | document/literal wrapped (`XmlSerializer`) |
| `F_VERIFICATERCEROV3` | `SvcUtils:VigiaVerificarTerceroUrlSvc (misma clave)` | `SOAP_VerificaTercero.asmx` | `"http://tempuri.org/F_VERIFICATERCEROV3"` | document/literal wrapped (`XmlSerializer`) |

> Ojo: `F_VERIFICATERCEROV3` (ASMX) y `F_VERIFICATERCERO` (PHP) **comparten la clave** `VigiaVerificarTerceroUrlSvc`. En la API BUA esa clave apunta al ASMX, así que si BUA llegara a usar el cliente PHP, el contrato no coincidiría.
