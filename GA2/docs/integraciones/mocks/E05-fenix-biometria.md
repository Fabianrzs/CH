# E5 — Fénix: validación biométrica (ASMX que devuelve un DataSet .NET)

> Documento para implementar un **mock compatible** con `WebServicesBiometrias.asmx`. La respuesta es un **DataSet .NET serializado** (esquema XSD + `diffgram`). Los ejemplos de este documento se validaron con el proxy WCF real **y** con la misma lógica de deserialización que usa GA2.

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | SOAP 1.1, document/literal, `XmlSerializer` |
| URL | `SvcUtils:FenixServiceSvc` = `http://cpvmp-09xc/WsFenixBiometria/WebServicesBiometrias.asmx?WSDL` (la URL incluye `?WSDL` y se usa como dirección del endpoint: el mock debe aceptar el `POST` con esa query string) |
| Contrato | `WebServicesBiometriasSoap`, namespace `http://tempuri.org/` |
| Binding | `BasicHttpBinding` sin seguridad, timeouts `TimeSpan.MaxValue`, `MaxReceivedMessageSize` 1 000 000 bytes |
| Cliente .NET | `ReferenciasSvc/Fenix/ConsumoFenix.cs` → `ServiciosSOA/Fenix/FenixService` → `Credito/Fenix/FenixBusinessLogic` |
| Toggle en BD | `FENIX` / `ESTADO` (si es `false`, GA2 responde *"No se pudo realizar la validacion de biometria para el documento"* sin llamar) |
| Auditoría | `EjecutarServicio(..., ReferenciaSvc.Fenix)` |
| Entradas en GA2 | `GET api/credito/validarBiometria?request=<identificación>`; prospección de crédito (`ProspeccionBusinessLogic` ≈10547) |

## 2. Operación `ValidarBiometria`

| Propiedad | Valor |
|---|---|
| Header `SOAPAction` | `"http://tempuri.org/ValidarBiometria"` |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Request | `<ValidarBiometria xmlns="http://tempuri.org/"><identificacion>xs:int</identificacion></ValidarBiometria>` |
| Response | `<ValidarBiometriaResponse><ValidarBiometriaResult>` = DataSet (`xs:schema` + `diffgr:diffgram`) |

**Request.** `identificacion` es `xs:int`: GA2 hace `int.Parse(request)`, así que documentos con más de 10 dígitos o con letras fallan **antes** de llamar.

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ValidarBiometria xmlns="http://tempuri.org/">
      <identificacion>1</identificacion>
    </ValidarBiometria>
  </s:Body>
</s:Envelope>
```

### 2.1 Cómo lee GA2 la respuesta (requisitos estrictos)

1. Toma `ValidarBiometriaResult.Nodes[1]`, el **segundo** nodo hijo del resultado. Por eso el **primer** hijo debe ser el `xs:schema` y el **segundo** el `diffgr:diffgram`.
2. Deserializa ese nodo con `XmlSerializer(typeof(diffgram))` (`GA2.Application/Dto/Fenix/Diffgram.cs`):
   - raíz `diffgram` en el namespace `urn:schemas-microsoft-com:xml-diffgram-v1`;
   - hijo `NewDataSet` **sin namespace**, con un único `Table1`;
   - campos de `Table1`: `A001NUM_IDENTIFICACION` (**`xs:unsignedInt`**), `A001PRIMER_APELLIDO`, `A001SEGUNDO_APELLIDO`, `A001PRIMER_NOMBRE`, `A001SEGUNDO_NOMBRE` (`xs:string`), `A008CEDULA`, `A008ROSTRO`, `A008HUELLA`, `A008FIRMA` (**`xs:unsignedByte`**, 0–255; el `1` indica que la evidencia biométrica existe).
3. Si `NewDataSet` no viene (diffgram vacío): `IsSuccess=false`, *"No hay información en Fénix para ese afiliado"*, y GA2 **inserta una notificación** al perfil `ASERA_T` en BD.
4. Si la deserialización falla (p. ej. un `A008*` no numérico): `IsSuccess=false`, *"Usuario sin datos en Fenix"*, más la misma notificación.
5. Con datos: `IsSuccess=true`, *"Biometria verificada"*, y `Data` (el `diffgram`) se devuelve al frontend, que interpreta los `A008*`.

### 2.2 Response de ejemplo — afiliado con biometría (validado)

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <ValidarBiometriaResponse xmlns="http://tempuri.org/">
      <ValidarBiometriaResult>
        <xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
          <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
            <xs:complexType>
              <xs:choice minOccurs="0" maxOccurs="unbounded">
                <xs:element name="Table1">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="A001NUM_IDENTIFICACION" type="xs:unsignedInt" minOccurs="0" />
                      <xs:element name="A001PRIMER_APELLIDO" type="xs:string" minOccurs="0" />
                      <xs:element name="A001SEGUNDO_APELLIDO" type="xs:string" minOccurs="0" />
                      <xs:element name="A001PRIMER_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="A001SEGUNDO_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="A008CEDULA" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008ROSTRO" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008HUELLA" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008FIRMA" type="xs:unsignedByte" minOccurs="0" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        <diffgr:diffgram xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1">
          <NewDataSet xmlns="">
            <Table1 diffgr:id="Table11" msdata:rowOrder="0">
              <A001NUM_IDENTIFICACION>1020304050</A001NUM_IDENTIFICACION>
              <A001PRIMER_APELLIDO>PEREZ</A001PRIMER_APELLIDO>
              <A001SEGUNDO_APELLIDO>GOMEZ</A001SEGUNDO_APELLIDO>
              <A001PRIMER_NOMBRE>JUAN</A001PRIMER_NOMBRE>
              <A001SEGUNDO_NOMBRE>CARLOS</A001SEGUNDO_NOMBRE>
              <A008CEDULA>1</A008CEDULA>
              <A008ROSTRO>1</A008ROSTRO>
              <A008HUELLA>1</A008HUELLA>
              <A008FIRMA>1</A008FIRMA>
            </Table1>
          </NewDataSet>
        </diffgr:diffgram>
      </ValidarBiometriaResult>
    </ValidarBiometriaResponse>
  </soap:Body>
</soap:Envelope>

```

### 2.3 Response de ejemplo — afiliado sin datos (validado: GA2 responde "No hay información en Fénix")

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <ValidarBiometriaResponse xmlns="http://tempuri.org/">
      <ValidarBiometriaResult>
        <xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
          <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
            <xs:complexType>
              <xs:choice minOccurs="0" maxOccurs="unbounded">
                <xs:element name="Table1">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="A001NUM_IDENTIFICACION" type="xs:unsignedInt" minOccurs="0" />
                      <xs:element name="A001PRIMER_APELLIDO" type="xs:string" minOccurs="0" />
                      <xs:element name="A001SEGUNDO_APELLIDO" type="xs:string" minOccurs="0" />
                      <xs:element name="A001PRIMER_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="A001SEGUNDO_NOMBRE" type="xs:string" minOccurs="0" />
                      <xs:element name="A008CEDULA" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008ROSTRO" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008HUELLA" type="xs:unsignedByte" minOccurs="0" />
                      <xs:element name="A008FIRMA" type="xs:unsignedByte" minOccurs="0" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        <diffgr:diffgram xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1">
          
        </diffgr:diffgram>
      </ValidarBiometriaResult>
    </ValidarBiometriaResponse>
  </soap:Body>
</soap:Envelope>

```

> Se validó que ambas variantes (indentada y minificada) funcionan para Fénix.

### 2.4 Errores

- **SOAP Fault / caída:** `ConsumoFenix` no captura nada; la excepción sube a `FenixBusinessLogic`, que tampoco la captura alrededor de la llamada → HTTP 409 (Conflict) en la API.
- `MaxReceivedMessageSize` = 1 000 000 bytes: una respuesta más grande falla con `QuotaExceededException`.

## 3. Operación `xxValidarBiometria`

Está en el contrato (`SOAPAction "http://tempuri.org/xxValidarBiometria"`) pero **no la invoca nadie**. No requiere mock.

## 4. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Biometría completa | `Table1` con `A008*` = 1 |
| 2 | Biometría parcial | Algún `A008*` = 0 |
| 3 | Sin datos | diffgram sin `NewDataSet` (GA2 crea la notificación `ASERA_T`) |
| 4 | Dato inválido | `A008HUELLA` = `X` (GA2 responde "Usuario sin datos en Fenix") |
| 5 | Falla técnica | SOAP Fault |
