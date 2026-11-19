# E6 — Haberes: Modelo Canónico de Datos (MCD) — `wsModeloCanonico.asmx`

> Documento para implementar un **mock compatible** con el servicio de reportes de haberes. Los sobres se capturaron y validaron con el proxy WCF real. El DataSet de `ReporteHaberes` se validó además con la misma lógica de lectura que usa GA2.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | SOAP 1.1, document/literal, `XmlSerializer` |
| URL | `SvcUtils:HaberesUrlSvc` = `http://cpvmp-92x/MCDIntegrationServices/wsModeloCanonico.asmx` |
| Contrato | `wsModeloCanonicoDatosSoap`, namespace `http://tempuri.org/` (`SOAPAction = "http://tempuri.org/<Op>"`) |
| Binding | `BasicHttpBinding` sin seguridad, timeouts `TimeSpan.MaxValue` |
| Cliente .NET | `ReferenciasSvc/Haberes/ConsumoHaberes.cs` → `ServiciosSOA/Haberes/HaberesService` → `Credito/Haberes/HaberesBusinessLogic` |
| Toggle | No hay. **Importante:** `GenerarReporte` sólo va por este SOAP en compilación **Release**; en **DEBUG** va por la pasarela ktrionApi (E10, `api/generarreportehaberes`) |
| Auditoría | `EjecutarServicio(..., ReferenciaSvc.Haberes)` |
| Entradas en GA2 | `POST api/credito/generarReporteHaberes`, `api/credito/generarReporteHaberesv2`, `api/credito/ReporteHaberes`, `api/Reportes/ReporteCertificadoHaberes` |

Antes de llamar, GA2 busca la cuenta del afiliado (`ObtenerCuentaById`) y usa su `IdCuentaIntegracion` como `accountId`. Si no la encuentra, lanza *"La cuenta no pose cuenta integracion o no se encuentra"* **sin llamar**.

## 2. Operaciones usadas

### 1. `GenerarReporte`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada** (Release). En DEBUG se sustituye por la pasarela E10 |
| Transporte | `POST {SvcUtils:HaberesUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/GenerarReporte"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}GenerarReporte` |
| Elemento raíz del response | `{http://tempuri.org/}GenerarReporteResponse` |
| Método del proxy (.NET) | `GenerarReporteAsync` |

**Propósito.** Genera un reporte/certificado de haberes en PDF.

**Quién la invoca en GA2.** `HaberesBusinessLogic.GenerarReporteHaberes`.

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `sReporte` | `xs:string` | — | `http://tempuri.org/` |
| `customerIdentification` | `xs:string` | — | `http://tempuri.org/` |
| `accountId` | `xs:int` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GenerarReporteResult` | `ServiceResponse` | — | `http://tempuri.org/` |

`sReporte` = tipo de reporte que pide el frontend (`HaberesRequestDto.Reporte`), `customerIdentification` = documento y `accountId` = `IdCuentaIntegracion`.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <GenerarReporte xmlns="http://tempuri.org/">
      <sReporte>sReporte</sReporte>
      <customerIdentification>customerIdentification</customerIdentification>
      <accountId>1</accountId>
    </GenerarReporte>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado; `Report` = un PDF mínimo en Base64):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <GenerarReporteResponse xmlns="http://tempuri.org/">
      <GenerarReporteResult>
        <Message>OK</Message>
        <Report>JVBERi0xLjQKJcOkw7zDtsOfCjEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAxIDAgUj4+CiUlRU9G</Report>
        <Status>true</Status>
      </GenerarReporteResult>
    </GenerarReporteResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

- Si `GenerarReporteResult.Status = true`, GA2 hace `Convert.ToBase64String(Report)`; si es `false`, el resultado es `null`.
- Luego `GuardarReporteBlobStorage` guarda el PDF en el contenedor Blob `credito` y lo devuelve como archivo. **`Report` debe contener un PDF válido** (bytes, viaja como `xs:base64Binary`).
- `Message` no se usa.

**Errores y casos alternos.**

SOAP Fault: no hay captura → HTTP 409 (Conflict) en la API de GA2. Con `Status=false` el Base64 queda nulo y el guardado en Blob falla aguas abajo.

### 2. `GenerarReporteV2`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada** |
| Transporte | `POST {SvcUtils:HaberesUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/GenerarReporteV2"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}GenerarReporteV2` |
| Elemento raíz del response | `{http://tempuri.org/}GenerarReporteV2Response` |
| Método del proxy (.NET) | `GenerarReporteV2Async` |

**Propósito.** Igual que `GenerarReporte`, filtrando por mes y año.

**Quién la invoca en GA2.** `HaberesBusinessLogic.GenerarReporteHaberesV2` (`api/credito/generarReporteHaberesv2`).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `sReporte` | `xs:string` | — | `http://tempuri.org/` |
| `customerIdentification` | `xs:string` | — | `http://tempuri.org/` |
| `accountId` | `xs:int` | — | `http://tempuri.org/` |
| `month` | `xs:string` | — | `http://tempuri.org/` |
| `year` | `xs:string` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `GenerarReporteV2Result` | `ServiceResponse` | — | `http://tempuri.org/` |

`month` y `year` llegan como texto desde `HaberesRequestDto.Mes`/`Anio`.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <GenerarReporteV2 xmlns="http://tempuri.org/">
      <sReporte>sReporte</sReporte>
      <customerIdentification>customerIdentification</customerIdentification>
      <accountId>1</accountId>
      <month>month</month>
      <year>year</year>
    </GenerarReporteV2>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (validado):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <GenerarReporteV2Response xmlns="http://tempuri.org/">
      <GenerarReporteV2Result>
        <Message>OK</Message>
        <Report>JVBERi0xLjQKJcOkw7zDtsOfCjEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAxIDAgUj4+CiUlRU9G</Report>
        <Status>true</Status>
      </GenerarReporteV2Result>
    </GenerarReporteV2Response>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

Igual que `GenerarReporte`. Nota: `HaberesService` invierte el mensaje (con `Data` vacío dice "petición exitosa"), sin efecto funcional.

### 3. `ReporteHaberes`

| Propiedad | Valor |
|---|---|
| Estado en GA2 | **Usada** |
| Transporte | `POST {SvcUtils:HaberesUrlSvc}` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/ReporteHaberes"` |
| Estilo | document/literal wrapped (`XmlSerializer`) |
| Elemento raíz del request | `{http://tempuri.org/}ReporteHaberes` |
| Elemento raíz del response | `{http://tempuri.org/}ReporteHaberesResponse` |
| Método del proxy (.NET) | `ReporteHaberesAsync` |

**Propósito.** Devuelve un **DataSet** con datos básicos del afiliado para armar el certificado de haberes (RDLC) en GA2.

**Quién la invoca en GA2.** `HaberesBusinessLogic.ReporteHaberes` (`api/credito/ReporteHaberes`, `api/Reportes/ReporteCertificadoHaberes`).

**Request — elementos del cuerpo** (dentro del elemento raíz):

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `customerIdentification` | `xs:string` | — | `http://tempuri.org/` |
| `accountId` | `xs:int` | — | `http://tempuri.org/` |
| `sUserQuery` | `xs:string` | — | `http://tempuri.org/` |

**Response — elementos del cuerpo**:

| Elemento | Tipo | Ocurrencia | Namespace |
|---|---|---|---|
| `ReporteHaberesResult` | `ArrayOfXElement` | — | `http://tempuri.org/` |

`sUserQuery` se envía siempre como cadena vacía.

**Request de ejemplo** (capturado del cliente WCF real con valores de relleno: textos = nombre del campo, números = `1`/`1.5`, fechas = `2026-01-15`):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ReporteHaberes xmlns="http://tempuri.org/">
      <customerIdentification>customerIdentification</customerIdentification>
      <accountId>1</accountId>
      <sUserQuery>sUserQuery</sUserQuery>
    </ReporteHaberes>
  </s:Body>
</s:Envelope>
```

**Response de ejemplo** (**minificado, como debe enviarlo el mock**; validado con el proxy y con la lógica de GA2). Versión indentada sólo para lectura en §5:

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><soap:Body><ReporteHaberesResponse xmlns="http://tempuri.org/"><ReporteHaberesResult><xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"><xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true"><xs:complexType><xs:choice minOccurs="0" maxOccurs="unbounded"><xs:element name="REPORTE_HABERES"><xs:complexType><xs:sequence><xs:element name="CLI_IDENTIFICACION" type="xs:string" minOccurs="0" /><xs:element name="CLI_NOMBRE_COMPLETO" type="xs:string" minOccurs="0" /><xs:element name="CLI_SEGUNDO_NOMBRE" type="xs:string" minOccurs="0" /><xs:element name="TID_DESCRIPCION" type="xs:string" minOccurs="0" /><xs:element name="UEJ_NOMBRE" type="xs:string" minOccurs="0" /><xs:element name="CTG_DESCRIPCION" type="xs:string" minOccurs="0" /><xs:element name="CLI_REGIMEN" type="xs:string" minOccurs="0" /><xs:element name="APC_FECHA_ULTIMA_CUOTA" type="xs:dateTime" minOccurs="0" /><xs:element name="CNC_DESCRIPCION" type="xs:string" minOccurs="0" /></xs:sequence></xs:complexType></xs:element></xs:choice></xs:complexType></xs:element></xs:schema><diffgr:diffgram xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1"><NewDataSet xmlns=""><REPORTE_HABERES diffgr:id="REPORTE_HABERES1" msdata:rowOrder="0"><CLI_IDENTIFICACION>1020304050</CLI_IDENTIFICACION><CLI_NOMBRE_COMPLETO>JUAN CARLOS PEREZ GOMEZ</CLI_NOMBRE_COMPLETO><CLI_SEGUNDO_NOMBRE>CARLOS</CLI_SEGUNDO_NOMBRE><TID_DESCRIPCION>CEDULA DE CIUDADANIA</TID_DESCRIPCION><UEJ_NOMBRE>EJERCITO NACIONAL</UEJ_NOMBRE><CTG_DESCRIPCION>OFICIALES</CTG_DESCRIPCION><CLI_REGIMEN>1</CLI_REGIMEN><APC_FECHA_ULTIMA_CUOTA>2026-08-31T00:00:00-05:00</APC_FECHA_ULTIMA_CUOTA><CNC_DESCRIPCION>AHORROS OBLIGATORIOS</CNC_DESCRIPCION></REPORTE_HABERES></NewDataSet></diffgr:diffgram></ReporteHaberesResult></ReporteHaberesResponse></soap:Body></soap:Envelope>
```

**Cómo interpreta GA2 la respuesta (lo que el mock debe respetar).**

Requisitos estrictos (validados):

1. GA2 exige `ReporteHaberesResult.Nodes.Count > 1`: primer nodo `xs:schema`, segundo `diffgr:diffgram`. Con menos nodos no arma el reporte.
2. Toma `Nodes[1].FirstNode` (el primer hijo del `diffgram`) y lo parsea como documento. **No puede haber espacios en blanco ni saltos de línea entre `<diffgr:diffgram>` y `<NewDataSet>`**: si los hay, el primer nodo es texto y GA2 falla con `XmlException: Root element is missing` (verificado). Un ASMX real responde sin indentar; **el mock debe responder minificado**.
3. Lee por nombre de elemento (en cualquier profundidad): `CLI_NOMBRE_COMPLETO`, `CLI_IDENTIFICACION`, `UEJ_NOMBRE` (fuerza), `TID_DESCRIPCION`, `CTG_DESCRIPCION`, `CLI_REGIMEN`, `CLI_SEGUNDO_NOMBRE` y `APC_FECHA_ULTIMA_CUOTA` (`xs:dateTime`, se convierte a `DateTime?`). El resto del reporte sale de la BUA API y de la BD de GA2.
4. El nombre de la tabla del DataSet no se valida. Se propone `REPORTE_HABERES`, que es el nombre que aparece en código comentado.

## 3. Operaciones del contrato no utilizadas por GA2

| Operación | SOAPAction |
|---|---|
| `CustomerAccount` | `"http://tempuri.org/CustomerAccount"` |
| `DetalleMovimiento` | `"http://tempuri.org/DetalleMovimiento"` |
| `DetalleBloqueos` | `"http://tempuri.org/DetalleBloqueos"` |
| `ExtractoGeneral` | `"http://tempuri.org/ExtractoGeneral"` |
| `DeclaracionRenta` | `"http://tempuri.org/DeclaracionRenta"` |

No hay código de GA2 que las invoque. El mock puede responder con SOAP Fault.

## 4. Tipos de datos

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `ArrayOfXElement`

Tipo XML `ArrayOfXElement`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Nodes` | `List<XElement>` | opcional (minOccurs=0) |  |

### `ServiceResponse`

Tipo XML `ServiceResponse` — namespace `http://tempuri.org/`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Message` | `xs:string` | opcional (minOccurs=0) |  |
| 2 | `Report` | `xs:base64Binary` | opcional (minOccurs=0) |  |
| 3 | `Status` | `xs:boolean` | siempre presente |  |


## 5. DataSet de `ReporteHaberes` indentado (sólo lectura; **no** enviarlo así)

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <ReporteHaberesResponse xmlns="http://tempuri.org/">
      <ReporteHaberesResult>
        <xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
          <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
            <xs:complexType>
              <xs:choice minOccurs="0" maxOccurs="unbounded">
                <xs:element name="REPORTE_HABERES">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="CLI_IDENTIFICACION" type="xs:string" minOccurs="0" />
                      <xs:element name="CLI_NOMBRE_COMPLETO" type="xs:string" minOccurs="0" />
                      <xs:element name="CLI_SEGUNDO_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="TID_DESCRIPCION" type="xs:string" minOccurs="0" />
                      <xs:element name="UEJ_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="CTG_DESCRIPCION" type="xs:string" minOccurs="0" />
                      <xs:element name="CLI_REGIMEN" type="xs:string" minOccurs="0" />
                      <xs:element name="APC_FECHA_ULTIMA_CUOTA" type="xs:dateTime" minOccurs="0" />
                      <xs:element name="CNC_DESCRIPCION" type="xs:string" minOccurs="0" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        <diffgr:diffgram xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1">
          <NewDataSet xmlns="">
            <REPORTE_HABERES diffgr:id="REPORTE_HABERES1" msdata:rowOrder="0">
              <CLI_IDENTIFICACION>1020304050</CLI_IDENTIFICACION>
              <CLI_NOMBRE_COMPLETO>JUAN CARLOS PEREZ GOMEZ</CLI_NOMBRE_COMPLETO>
              <CLI_SEGUNDO_NOMBRE>CARLOS</CLI_SEGUNDO_NOMBRE>
              <TID_DESCRIPCION>CEDULA DE CIUDADANIA</TID_DESCRIPCION>
              <UEJ_NOMBRE>EJERCITO NACIONAL</UEJ_NOMBRE>
              <CTG_DESCRIPCION>OFICIALES</CTG_DESCRIPCION>
              <CLI_REGIMEN>1</CLI_REGIMEN>
              <APC_FECHA_ULTIMA_CUOTA>2026-08-31T00:00:00-05:00</APC_FECHA_ULTIMA_CUOTA>
              <CNC_DESCRIPCION>AHORROS OBLIGATORIOS</CNC_DESCRIPCION>
            </REPORTE_HABERES>
          </NewDataSet>
        </diffgr:diffgram>
      </ReporteHaberesResult>
    </ReporteHaberesResponse>
  </soap:Body>
</soap:Envelope>

```


## 6. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Certificado generado | `Status=true`, `Report` = PDF |
| 2 | Sin reporte | `Status=false` |
| 3 | Reporte de haberes | DataSet minificado con una fila `REPORTE_HABERES` |
| 4 | DataSet vacío | Sólo `xs:schema` (Nodes.Count = 1 → GA2 no arma el reporte) |
| 5 | Falla técnica | SOAP Fault |
