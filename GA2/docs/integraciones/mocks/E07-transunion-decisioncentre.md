# E7 — TransUnion DecisionCentre (IDVision, OTP, AQM/score e historial crediticio)

> Documento para implementar un **mock compatible** con `ExternalSolutionExecution.svc` de TransUnion. Es la integración más delicada: un único método SOAP (`ExecuteXMLString`) transporta un **XML de negocio dentro de un string** (`DCRequest`/`DCResponse`), y los flujos encadenan varias llamadas que comparten un `ApplicationId`.
> Los `DCRequest` de este documento se generaron **ejecutando la misma transformación de `ValidacionesTU.ConsumirTU/ConsumirTUAqm`** sobre los DTO reales, y cada `DCResponse` se validó con los deserializadores de GA2 (`Serialization.DeserializeDC`, `DeserializeRC`, `DeserializeApps`).

## 1. Resumen

| Aspecto | Valor |
|---|---|
| Protocolo | SOAP 1.1, document/literal, `XmlSerializer`; HTTPS (`BasicHttpSecurityMode.Transport`), TLS 1.2/1.3 |
| URL | **Fija en el código** (`GA2.Application/Main/TransUnion/ValidacionesTU.cs:16`): `https://www.transuniondecisioncentre.com.mx/TU.IDS.ExternalServices_latam_prod/SolutionExecution/ExternalSolutionExecution.svc` (**producción**). Las claves `SvcUtils:TransUnion`/`TrasnUnion` no se leen |
| Contrato | `IExternalSolutionExecution`, namespace `http://tempuri.org/`; **sólo se usa** `ExecuteXMLString` (`SOAPAction "http://tempuri.org/IExternalSolutionExecution/ExecuteXMLString"`) |
| Autenticación | Dentro del `DCRequest`: `<Authentication type="OnDemand"><UserId/><Password/></Authentication>`. IDVision usa `IdVision:usuarioIDVision`/`claveIDVision` (`solucionId` 142); AQM usa `Aqm:usuarioAqm`/`claveAqm` (`solucionIdAqm` 360) |
| Toggle | **No existe** (`IdVision.estado` y `Aqm.estado` no se leen) |
| Cliente .NET | `ValidacionesTU` (crea un `ExternalSolutionExecutionClient` por llamada) ← `GestorIdVisionBusinessLogic`, `GestorTransUnionBusinessLogic`, `ValidacionIdentidadBusinessLogic`, `ProspeccionBusinessLogic`, `CreditoBusinessLogic` |
| Persistencia | Cada paso se registra en BD (`CrearLogTu`, `RegistroLogTUSimulador`, `VLL_VALIDACION_LOG_OTP`). La prospección guarda el `DCResponse` en el Blob `trans-union` y **lo reutiliza si tiene menos de 30 días** (no llama a TU) |
| Redirección al mock | **No se puede por configuración**: requiere un cambio de código (hacer configurable `serviceUrl`) o resolver `www.transuniondecisioncentre.com.mx` hacia el mock con un certificado TLS de confianza |

> **Riesgo:** con la URL fija en el código, cualquier ambiente (incluido desarrollo) llama a TransUnion **producción** con datos reales.

## 2. Contrato SOAP: `ExecuteXMLString`

| Propiedad | Valor |
|---|---|
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IExternalSolutionExecution/ExecuteXMLString"` |
| Request | `<ExecuteXMLString xmlns="http://tempuri.org/"><request>` **`DCRequest` como texto escapado** `</request></ExecuteXMLString>` |
| Response | `<ExecuteXMLStringResponse xmlns="http://tempuri.org/"><ExecuteXMLStringResult>` **`DCResponse` como texto escapado** `</ExecuteXMLStringResult></ExecuteXMLStringResponse>` |

Sobre capturado del proxy real (valores de relleno):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>request</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

### 2.1 Transformación que aplica GA2 al `DCRequest` (importante para el parser del mock)

`ConsumirTU` (flujos IDVision/OTP y prospección):
1. Serializa `DCRequest` con `XmlSerializer` (raíz `<DCRequest xmlns="http://transunion.com/dc/extsvc">`, sin declaración XML).
2. Codifica el texto de `PrimaryApplicant` y luego **deshace el escape** con `Replace("&lt;","<")`/`Replace("&gt;",">")`/`Replace("&amp;","&")`.
3. Envuelve `<Applicant>…</Applicant>` y `<ApplicationData>…</ApplicationData>` en **`<![CDATA[ … ]]>`**.

`ConsumirTUAqm` (score e historial AQM): serializa sin CDATA; los XML internos quedan **escapados** (`&lt;Applicant&gt;…`). El código intenta además reemplazar `<DCRequest>` por `<DCRequest xmlns = "http://transunion.com/dc/extsvc" >`, pero **la línea siguiente sobrescribe el resultado**, así que esa sustitución no tiene efecto (defecto inocuo: el namespace ya lo pone el serializador).

> El mock debe aceptar **ambas** formas: `Field` con CDATA y `Field` con XML escapado. Con un parser XML estándar, en los dos casos el texto del `Field` es el XML interno.

### 2.2 Estructura del `DCResponse` que espera GA2

| Elemento | Tipo | Uso |
|---|---|---|
| `DCResponse/Status` | texto | `"Success"` para continuar; cualquier otro valor = error |
| `DCResponse/Authentication/Status` | texto | `"Success"` (o `"FirstTimeLogIn"`, aceptado en el inicio IDVision y en la prospección) |
| `DCResponse/Authentication/Token` | texto | No se usa |
| `DCResponse/ResponseInfo/ApplicationId` | texto | **Identificador de la transacción**; GA2 lo devuelve al frontend y lo reenvía en los pasos siguientes (`RequestInfo/ApplicationId`) |
| `DCResponse/ResponseInfo/{QueueName, ExecutionMode, SolutionSetInstanceId, CurrentQueue}` | texto | No se usan |
| `DCResponse/ContextData/Field[@key]` | texto (XML escapado) | Datos de negocio; ver claves por paso |
| `DCResponse/ErrorInfo/Message` | texto | En el envío de OTP, el texto exacto `Application does not exists in Queue PhoneSelection.` se trata como rechazo |

Los `Field` cuyo contenido es XML (`ApplicationData`, `Applicants`) llevan el **XML escapado como texto** y GA2 los deserializa con `XmlSerializer` sobre:
- `ApplicationData`: `ReturnMessage`, `RiskLevel`, `Product`, `DecisionHomologada`, `IDMOutcome`, `IDVReasonsCode/Reason`, `OTPReasons/Reason`, `VelocityCheckReasons/Reason`, `IDAReasons`, `IDMReasonsCode`.
- `Reason`: `Applicant`, `Code`, `Type`, `Description`.
- `Applicants/Applicant`: `IdType`, `IdNumber`, `LastName`, `RecentPhoneNumber`, `IdExpeditionDate`, `Occupation`, `MonthlyIncome`, `Score`, `CapacidadEndeudamiento`, `PorcEndFinal`, `DocumentState`, `DSOnlineBureauCOLData` (historial: estructura amplia en `GA2.Application/Dto/Credito/TU/Requests/DSOnlineBureauCOLData.cs`).

`CapacidadEndeudamiento` y `PorcEndFinal` se convierten con `decimal.Parse` en cultura **en-US** (la API fija `CultureInfo("en-US")`): usar **punto decimal** y sin separador de miles.

## 3. Pasos lógicos (todos usan `ExecuteXMLString`)


### 3.1 IDVision — inicio de verificación

**Quién lo invoca.** `GestorIdVisionBusinessLogic.EnvioCodigoIDVision` (paso 1) ← `ProspeccionBusinessLogic` (≈10666). `validarCliente` (misma petición) sólo tiene llamadas comentadas.

**Contenido del request.** `SolutionSetId` = `"142"` (fijo en el código), `ExecutionMode = NewWithContext`, `Field key="PrimaryApplicant"` con `Applicant {IdType, IdNumber, LastName, RecentPhoneNumber, IdExpeditionDate (dd/MM/yyyy)}`.

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><SolutionSetId>142</SolutionSetId><ExecuteLatestVersion>true</ExecuteLatestVersion><ExecutionMode>NewWithContext</ExecutionMode></RequestInfo><Fields><Field key="PrimaryApplicant"><![CDATA[<Applicant><IdType>1</IdType><IdNumber>1020304050</IdNumber><LastName>PEREZ</LastName><RecentPhoneNumber>3001234567</RecentPhoneNumber><IdExpeditionDate>15/01/2010</IdExpeditionDate></Applicant>]]></Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;SolutionSetId&gt;142&lt;/SolutionSetId&gt;&lt;ExecuteLatestVersion&gt;true&lt;/ExecuteLatestVersion&gt;&lt;ExecutionMode&gt;NewWithContext&lt;/ExecutionMode&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="PrimaryApplicant"&gt;&lt;![CDATA[&lt;Applicant&gt;&lt;IdType&gt;1&lt;/IdType&gt;&lt;IdNumber&gt;1020304050&lt;/IdNumber&gt;&lt;LastName&gt;PEREZ&lt;/LastName&gt;&lt;RecentPhoneNumber&gt;3001234567&lt;/RecentPhoneNumber&gt;&lt;IdExpeditionDate&gt;15/01/2010&lt;/IdExpeditionDate&gt;&lt;/Applicant&gt;]]&gt;&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**Respuesta OK (continúa al envío de OTP)** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>PhoneSelection</CurrentQueue></ResponseInfo><ContextData><Field key="HasError">False</Field><Field key="Applicants">&lt;Applicants&gt;&lt;Applicant&gt;&lt;IdType&gt;0&lt;/IdType&gt;&lt;IdNumber&gt;1020304050&lt;/IdNumber&gt;&lt;DocumentState&gt;VIGENTE&lt;/DocumentState&gt;&lt;/Applicant&gt;&lt;/Applicants&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;PhoneSelection&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="HasError"&gt;False&lt;/Field&gt;&lt;Field key="Applicants"&gt;&amp;lt;Applicants&amp;gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;0&amp;lt;/IdType&amp;gt;&amp;lt;IdNumber&amp;gt;1020304050&amp;lt;/IdNumber&amp;gt;&amp;lt;DocumentState&amp;gt;VIGENTE&amp;lt;/DocumentState&amp;gt;&amp;lt;/Applicant&amp;gt;&amp;lt;/Applicants&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Requiere `Status = Success` **y** `Authentication/Status = Success`.
- Si hay un `Field key="Applicants"` y `Applicant/DocumentState = "MUERTE POR TITULAR"`, rechaza con ese mensaje.
- Si no, encadena **en la misma petición HTTP de GA2** el paso 3.2 con el `ApplicationId` recibido.
- *(Variante `validarCliente`, sin uso activo: si viene `ApplicationData`, lee `IDVReasonsCode/Reason/Code` (101 = usuario bloqueado; 103/104/105 = verificación fallida) o `VelocityCheckReasons`; si no, `HasError = "False"` indica éxito.)*


### 3.2 Envío de OTP por SMS (`PhoneSelection`)

**Quién lo invoca.** Segunda llamada de `EnvioCodigoIDVision`; también `EnvioCodigoOTP` ← `ValidacionIdentidadBusinessLogic` (≈230).

**Contenido del request.** `RequestInfo {ApplicationId, QueueName="PhoneSelection", ExecutionMode="Send"}`, campos `ValidationMethod=SMS`, `PhoneType=Mobile`, `SelectedPhoneNumber`.

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><ExecutionMode>Send</ExecutionMode><ApplicationId>9876543</ApplicationId><QueueName>PhoneSelection</QueueName></RequestInfo><Fields><Field key="ValidationMethod">SMS</Field><Field key="PhoneType">Mobile</Field><Field key="SelectedPhoneNumber">3001234567</Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;ExecutionMode&gt;Send&lt;/ExecutionMode&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;QueueName&gt;PhoneSelection&lt;/QueueName&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="ValidationMethod"&gt;SMS&lt;/Field&gt;&lt;Field key="PhoneType"&gt;Mobile&lt;/Field&gt;&lt;Field key="SelectedPhoneNumber"&gt;3001234567&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**OTP enviado** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>PinVerification_OTPInput</CurrentQueue></ResponseInfo><ContextData><Field key="OTPCode">SENT</Field><Field key="Delay">60</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;PinVerification_OTPInput&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="OTPCode"&gt;SENT&lt;/Field&gt;&lt;Field key="Delay"&gt;60&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Aplicación fuera de la cola (rechazo)** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Failure</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId></ResponseInfo><ContextData /><ErrorInfo><Message>Application does not exists in Queue PhoneSelection.</Message></ErrorInfo></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Failure&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;/ResponseInfo&gt;&lt;ContextData /&gt;&lt;ErrorInfo&gt;&lt;Message&gt;Application does not exists in Queue PhoneSelection.&lt;/Message&gt;&lt;/ErrorInfo&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Con éxito lee los `Field` cuya clave **contiene** `OTPCode` y `Delay` (y `ReturnMensagge`, sin uso) y devuelve `codigoOtp`, `delayOtp` y `idAplication`.
- `EnvioCodigoOTP` hace `OTPCodes.FirstOrDefault().Text` **sin comprobar nulos**: si faltan `OTPCode`/`Delay`, salta un `NullReferenceException`, que se captura y produce `codigoRechazo = 1`.
- `ErrorInfo/Message = "Application does not exists in Queue PhoneSelection."` → `codigoRechazo = 1`.


### 3.3 Reenvío de OTP

**Quién lo invoca.** `reEnvioCodigoOTP` ← `ValidacionIdentidadBusinessLogic` (≈227).

**Contenido del request.** `QueueName="PinVerification_OTPInput"`, `ExecutionMode="Send"`, `Field Action=ResendOTP`.

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><ExecutionMode>Send</ExecutionMode><ApplicationId>9876543</ApplicationId><QueueName>PinVerification_OTPInput</QueueName></RequestInfo><Fields><Field key="Action">ResendOTP</Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;ExecutionMode&gt;Send&lt;/ExecutionMode&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;QueueName&gt;PinVerification_OTPInput&lt;/QueueName&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="Action"&gt;ResendOTP&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**Reenvío OK** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>PinVerification_OTPInput</CurrentQueue></ResponseInfo><ContextData><Field key="OTPCode">SENT</Field><Field key="Delay">60</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;PinVerification_OTPInput&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="OTPCode"&gt;SENT&lt;/Field&gt;&lt;Field key="Delay"&gt;60&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Sólo comprueba `Status = Success` y `Authentication/Status = Success` → `codigoError = 0`; en otro caso, *"Problemas en la Conexión con TransUnion.!"*.


### 3.4 Validación del PIN OTP

**Quién lo invoca.** `validacionCodigoOTPAsync` ← `ValidacionIdentidadBusinessLogic` (≈135), `ProspeccionBusinessLogic` (≈10843).

**Contenido del request.** `QueueName="PinVerification_OTPInput"`, `ExecutionMode="Send"`, campos `PinNumber=<código>`, `Action=Continue`.

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><ExecutionMode>Send</ExecutionMode><ApplicationId>9876543</ApplicationId><QueueName>PinVerification_OTPInput</QueueName></RequestInfo><Fields><Field key="PinNumber">123456</Field><Field key="Action">Continue</Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;ExecutionMode&gt;Send&lt;/ExecutionMode&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;QueueName&gt;PinVerification_OTPInput&lt;/QueueName&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="PinNumber"&gt;123456&lt;/Field&gt;&lt;Field key="Action"&gt;Continue&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**PIN correcto (código 300)** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>End</CurrentQueue></ResponseInfo><ContextData><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;OTPReasons&gt;&lt;Reason&gt;&lt;Code&gt;300&lt;/Code&gt;&lt;Type&gt;OTP&lt;/Type&gt;&lt;Description&gt;OTP valido&lt;/Description&gt;&lt;/Reason&gt;&lt;/OTPReasons&gt;&lt;/ApplicationData&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;End&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;OTPReasons&amp;gt;&amp;lt;Reason&amp;gt;&amp;lt;Code&amp;gt;300&amp;lt;/Code&amp;gt;&amp;lt;Type&amp;gt;OTP&amp;lt;/Type&amp;gt;&amp;lt;Description&amp;gt;OTP valido&amp;lt;/Description&amp;gt;&amp;lt;/Reason&amp;gt;&amp;lt;/OTPReasons&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**PIN incorrecto (código 302)** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>9876543</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>End</CurrentQueue></ResponseInfo><ContextData><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;OTPReasons&gt;&lt;Reason&gt;&lt;Code&gt;302&lt;/Code&gt;&lt;Type&gt;OTP&lt;/Type&gt;&lt;Description&gt;OTP invalido&lt;/Description&gt;&lt;/Reason&gt;&lt;/OTPReasons&gt;&lt;/ApplicationData&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;End&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;OTPReasons&amp;gt;&amp;lt;Reason&amp;gt;&amp;lt;Code&amp;gt;302&amp;lt;/Code&amp;gt;&amp;lt;Type&amp;gt;OTP&amp;lt;/Type&amp;gt;&amp;lt;Description&amp;gt;OTP invalido&amp;lt;/Description&amp;gt;&amp;lt;/Reason&amp;gt;&amp;lt;/OTPReasons&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Lee el `Field` cuya clave contiene `ApplicationData` → `ApplicationData/OTPReasons/Reason`.
- `Code = 300` → *"Validacion exitosa de código OTP."*; `Code = 302` → `codigoError = 1`, *"Validacion fallida debio a código OTP incorrecto."*.
- `Code` debe ser **numérico** (`Convert.ToInt32`).


### 3.5 AQM — score y capacidad de endeudamiento

**Quién lo invoca.** `obtenerScoreCliente` ← `ValidacionIdentidadBusinessLogic` (≈546), `CreditoBusinessLogic` (≈2976). Credenciales `Aqm:*`, `SolutionSetId = Aqm:solucionIdAqm` (360).

**Contenido del request.** `ExecutionMode="NewWithContext"`, `PrimaryApplicant` (con `Occupation` y `MonthlyIncome`; `LastName` toma la 2.ª o 3.ª palabra del apellido según cuántas tenga) y `Field key="ApplicationData"` con `<ApplicationData><Product>CONSUMO</Product></ApplicationData>`. Transformación `ConsumirTUAqm` (XML interno escapado).

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><SolutionSetId>360</SolutionSetId><ExecuteLatestVersion>true</ExecuteLatestVersion><ExecutionMode>NewWithContext</ExecutionMode></RequestInfo><Fields><Field key="PrimaryApplicant">&lt;Applicant&gt;&lt;IdType&gt;1&lt;/IdType&gt;&lt;IdNumber&gt;1020304050&lt;/IdNumber&gt;&lt;LastName&gt;PEREZ&lt;/LastName&gt;&lt;RecentPhoneNumber&gt;3001234567&lt;/RecentPhoneNumber&gt;&lt;IdExpeditionDate&gt;15/01/2010&lt;/IdExpeditionDate&gt;&lt;Occupation&gt;EMPLEADO&lt;/Occupation&gt;&lt;MonthlyIncome&gt;3500000&lt;/MonthlyIncome&gt;&lt;/Applicant&gt;</Field><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;Product&gt;CONSUMO&lt;/Product&gt;&lt;/ApplicationData&gt;</Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;SolutionSetId&gt;360&lt;/SolutionSetId&gt;&lt;ExecuteLatestVersion&gt;true&lt;/ExecuteLatestVersion&gt;&lt;ExecutionMode&gt;NewWithContext&lt;/ExecutionMode&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="PrimaryApplicant"&gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;1&amp;lt;/IdType&amp;gt;&amp;lt;IdNumber&amp;gt;1020304050&amp;lt;/IdNumber&amp;gt;&amp;lt;LastName&amp;gt;PEREZ&amp;lt;/LastName&amp;gt;&amp;lt;RecentPhoneNumber&amp;gt;3001234567&amp;lt;/RecentPhoneNumber&amp;gt;&amp;lt;IdExpeditionDate&amp;gt;15/01/2010&amp;lt;/IdExpeditionDate&amp;gt;&amp;lt;Occupation&amp;gt;EMPLEADO&amp;lt;/Occupation&amp;gt;&amp;lt;MonthlyIncome&amp;gt;3500000&amp;lt;/MonthlyIncome&amp;gt;&amp;lt;/Applicant&amp;gt;&lt;/Field&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;Product&amp;gt;CONSUMO&amp;lt;/Product&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**Cliente VIABLE** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>5550001</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>DocVerify</CurrentQueue></ResponseInfo><ContextData><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;Product&gt;CONSUMO&lt;/Product&gt;&lt;DecisionHomologada&gt;VIABLE&lt;/DecisionHomologada&gt;&lt;/ApplicationData&gt;</Field><Field key="Applicants">&lt;Applicants&gt;&lt;Applicant&gt;&lt;IdType&gt;0&lt;/IdType&gt;&lt;Score&gt;750&lt;/Score&gt;&lt;CapacidadEndeudamiento&gt;1500000.50&lt;/CapacidadEndeudamiento&gt;&lt;PorcEndFinal&gt;35.5&lt;/PorcEndFinal&gt;&lt;/Applicant&gt;&lt;/Applicants&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;5550001&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;DocVerify&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;Product&amp;gt;CONSUMO&amp;lt;/Product&amp;gt;&amp;lt;DecisionHomologada&amp;gt;VIABLE&amp;lt;/DecisionHomologada&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;Field key="Applicants"&gt;&amp;lt;Applicants&amp;gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;0&amp;lt;/IdType&amp;gt;&amp;lt;Score&amp;gt;750&amp;lt;/Score&amp;gt;&amp;lt;CapacidadEndeudamiento&amp;gt;1500000.50&amp;lt;/CapacidadEndeudamiento&amp;gt;&amp;lt;PorcEndFinal&amp;gt;35.5&amp;lt;/PorcEndFinal&amp;gt;&amp;lt;/Applicant&amp;gt;&amp;lt;/Applicants&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cliente NO VIABLE** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>5550002</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>End</CurrentQueue></ResponseInfo><ContextData><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;Product&gt;CONSUMO&lt;/Product&gt;&lt;DecisionHomologada&gt;NO VIABLE&lt;/DecisionHomologada&gt;&lt;/ApplicationData&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;5550002&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;End&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;Product&amp;gt;CONSUMO&amp;lt;/Product&amp;gt;&amp;lt;DecisionHomologada&amp;gt;NO VIABLE&amp;lt;/DecisionHomologada&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- `ApplicationData/DecisionHomologada` ∈ {`VIABLE`, `PENDIENTE`} → lee `Applicants/Applicant`: `CapacidadEndeudamiento` y `PorcEndFinal` (**decimales obligatorios**, `decimal.Parse`) y `Score`; devuelve `decision` e `idAplication`.
- Otro valor (p. ej. `NO VIABLE`) → `tipoRechazo = "Politicas"`, *"Cliente No VIABLE"*.
- Sin `ApplicationData` → `codigoRechazo = 1`, *"Error en la Conexión con Buro."*.
- Este paso **no** comprueba `Status`.


### 3.6 AQM — historial crediticio

**Quién lo invoca.** `obtenerHistorialCliente` ← `CreditoBusinessLogic` (≈2978).

**Contenido del request.** `RequestInfo {ApplicationId, QueueName="DocVerify", ExecutionMode="Send"}`, `Field GenerateReport=Y`.

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><ExecutionMode>Send</ExecutionMode><ApplicationId>9876543</ApplicationId><QueueName>DocVerify</QueueName></RequestInfo><Fields><Field key="GenerateReport">Y</Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;ExecutionMode&gt;Send&lt;/ExecutionMode&gt;&lt;ApplicationId&gt;9876543&lt;/ApplicationId&gt;&lt;QueueName&gt;DocVerify&lt;/QueueName&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="GenerateReport"&gt;Y&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**Historial** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>5550001</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>End</CurrentQueue></ResponseInfo><ContextData><Field key="Applicants">&lt;Applicants&gt;&lt;Applicant&gt;&lt;IdType&gt;0&lt;/IdType&gt;&lt;Score&gt;750&lt;/Score&gt;&lt;CapacidadEndeudamiento&gt;1500000.50&lt;/CapacidadEndeudamiento&gt;&lt;PorcEndFinal&gt;35.5&lt;/PorcEndFinal&gt;&lt;DSOnlineBureauCOLData /&gt;&lt;/Applicant&gt;&lt;/Applicants&gt;</Field><Field key="DecisionHomologada">VIABLE</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;5550001&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;End&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="Applicants"&gt;&amp;lt;Applicants&amp;gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;0&amp;lt;/IdType&amp;gt;&amp;lt;Score&amp;gt;750&amp;lt;/Score&amp;gt;&amp;lt;CapacidadEndeudamiento&amp;gt;1500000.50&amp;lt;/CapacidadEndeudamiento&amp;gt;&amp;lt;PorcEndFinal&amp;gt;35.5&amp;lt;/PorcEndFinal&amp;gt;&amp;lt;DSOnlineBureauCOLData /&amp;gt;&amp;lt;/Applicant&amp;gt;&amp;lt;/Applicants&amp;gt;&lt;/Field&gt;&lt;Field key="DecisionHomologada"&gt;VIABLE&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Lee `Field key="Applicants"` → `Applicant/DSOnlineBureauCOLData`, lo serializa a JSON y lo devuelve en `historialCredito`.


### 3.7 Prospección de crédito (motor de decisión + historial)

**Quién lo invoca.** `GestorTransUnionBusinessLogic.ProspeccionCreditoTransUnion` ← `POST api/credito/ProspeccionCredito`. Antes de llamar, si la solicitud ya tiene un XML de menos de 30 días en el Blob `trans-union`, **no llama** y reutiliza ese XML.

**Contenido del request.** Credenciales `Aqm:*`, `SolutionSetId = Aqm:solucionIdAqm`, `ExecutionMode="NewWithContext"`, `PrimaryApplicant` y `Field key="ApplicationData"` como **elemento** (`<ApplicationData><Product>CONSUMO</Product>…</ApplicationData>`, envuelto en CDATA por `ConsumirTU`).

**`DCRequest` que construye GA2** (cadena exacta que va dentro de `<request>`):

```xml
<DCRequest xmlns="http://transunion.com/dc/extsvc"><Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication><RequestInfo><SolutionSetId>360</SolutionSetId><ExecuteLatestVersion>true</ExecuteLatestVersion><ExecutionMode>NewWithContext</ExecutionMode></RequestInfo><Fields><Field key="PrimaryApplicant"><![CDATA[<Applicant><IdType>1</IdType><IdNumber>1020304050</IdNumber><LastName>PEREZ</LastName><RecentPhoneNumber>3001234567</RecentPhoneNumber><IdExpeditionDate>15/01/2010</IdExpeditionDate><Occupation>EMPLEADO</Occupation><MonthlyIncome>3500000</MonthlyIncome></Applicant>]]></Field><Field key="ApplicationData"><![CDATA[<ApplicationData><Product>CONSUMO</Product></ApplicationData>]]></Field></Fields></DCRequest>
```

**Sobre SOAP enviado** (el `DCRequest` viaja escapado como texto):

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLString xmlns="http://tempuri.org/">
      <request>&lt;DCRequest xmlns="http://transunion.com/dc/extsvc"&gt;&lt;Authentication type="OnDemand"&gt;&lt;UserId&gt;USUARIO_TU&lt;/UserId&gt;&lt;Password&gt;CLAVE_TU&lt;/Password&gt;&lt;/Authentication&gt;&lt;RequestInfo&gt;&lt;SolutionSetId&gt;360&lt;/SolutionSetId&gt;&lt;ExecuteLatestVersion&gt;true&lt;/ExecuteLatestVersion&gt;&lt;ExecutionMode&gt;NewWithContext&lt;/ExecutionMode&gt;&lt;/RequestInfo&gt;&lt;Fields&gt;&lt;Field key="PrimaryApplicant"&gt;&lt;![CDATA[&lt;Applicant&gt;&lt;IdType&gt;1&lt;/IdType&gt;&lt;IdNumber&gt;1020304050&lt;/IdNumber&gt;&lt;LastName&gt;PEREZ&lt;/LastName&gt;&lt;RecentPhoneNumber&gt;3001234567&lt;/RecentPhoneNumber&gt;&lt;IdExpeditionDate&gt;15/01/2010&lt;/IdExpeditionDate&gt;&lt;Occupation&gt;EMPLEADO&lt;/Occupation&gt;&lt;MonthlyIncome&gt;3500000&lt;/MonthlyIncome&gt;&lt;/Applicant&gt;]]&gt;&lt;/Field&gt;&lt;Field key="ApplicationData"&gt;&lt;![CDATA[&lt;ApplicationData&gt;&lt;Product&gt;CONSUMO&lt;/Product&gt;&lt;/ApplicationData&gt;]]&gt;&lt;/Field&gt;&lt;/Fields&gt;&lt;/DCRequest&gt;</request>
    </ExecuteXMLString>
  </s:Body>
</s:Envelope>
```

**Decisión del motor** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>5550001</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>DocVerify</CurrentQueue></ResponseInfo><ContextData><Field key="ApplicationData">&lt;ApplicationData&gt;&lt;Product&gt;CONSUMO&lt;/Product&gt;&lt;DecisionHomologada&gt;VIABLE&lt;/DecisionHomologada&gt;&lt;/ApplicationData&gt;</Field><Field key="Applicants">&lt;Applicants&gt;&lt;Applicant&gt;&lt;IdType&gt;0&lt;/IdType&gt;&lt;Score&gt;750&lt;/Score&gt;&lt;CapacidadEndeudamiento&gt;1500000.50&lt;/CapacidadEndeudamiento&gt;&lt;PorcEndFinal&gt;35.5&lt;/PorcEndFinal&gt;&lt;/Applicant&gt;&lt;/Applicants&gt;</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;5550001&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;DocVerify&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="ApplicationData"&gt;&amp;lt;ApplicationData&amp;gt;&amp;lt;Product&amp;gt;CONSUMO&amp;lt;/Product&amp;gt;&amp;lt;DecisionHomologada&amp;gt;VIABLE&amp;lt;/DecisionHomologada&amp;gt;&amp;lt;/ApplicationData&amp;gt;&lt;/Field&gt;&lt;Field key="Applicants"&gt;&amp;lt;Applicants&amp;gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;0&amp;lt;/IdType&amp;gt;&amp;lt;Score&amp;gt;750&amp;lt;/Score&amp;gt;&amp;lt;CapacidadEndeudamiento&amp;gt;1500000.50&amp;lt;/CapacidadEndeudamiento&amp;gt;&amp;lt;PorcEndFinal&amp;gt;35.5&amp;lt;/PorcEndFinal&amp;gt;&amp;lt;/Applicant&amp;gt;&amp;lt;/Applicants&amp;gt;&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Segunda llamada (`DocVerify` + `GenerateReport=Y`) cuando `TraerHistorialCrediticio = "Y"`** — `DCResponse` (validado con `Serialization.DeserializeDC` y los deserializadores anidados de GA2):

```xml
<DCResponse><Status>Success</Status><Authentication><Status>Success</Status><Token>token-tu</Token></Authentication><ResponseInfo><ApplicationId>5550001</ApplicationId><SolutionSetInstanceId>abc-123</SolutionSetInstanceId><CurrentQueue>End</CurrentQueue></ResponseInfo><ContextData><Field key="Applicants">&lt;Applicants&gt;&lt;Applicant&gt;&lt;IdType&gt;0&lt;/IdType&gt;&lt;Score&gt;750&lt;/Score&gt;&lt;CapacidadEndeudamiento&gt;1500000.50&lt;/CapacidadEndeudamiento&gt;&lt;PorcEndFinal&gt;35.5&lt;/PorcEndFinal&gt;&lt;DSOnlineBureauCOLData /&gt;&lt;/Applicant&gt;&lt;/Applicants&gt;</Field><Field key="DecisionHomologada">VIABLE</Field></ContextData></DCResponse>
```

Sobre SOAP que debe devolver el mock:

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ExecuteXMLStringResponse xmlns="http://tempuri.org/">
      <ExecuteXMLStringResult>&lt;DCResponse&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Authentication&gt;&lt;Status&gt;Success&lt;/Status&gt;&lt;Token&gt;token-tu&lt;/Token&gt;&lt;/Authentication&gt;&lt;ResponseInfo&gt;&lt;ApplicationId&gt;5550001&lt;/ApplicationId&gt;&lt;SolutionSetInstanceId&gt;abc-123&lt;/SolutionSetInstanceId&gt;&lt;CurrentQueue&gt;End&lt;/CurrentQueue&gt;&lt;/ResponseInfo&gt;&lt;ContextData&gt;&lt;Field key="Applicants"&gt;&amp;lt;Applicants&amp;gt;&amp;lt;Applicant&amp;gt;&amp;lt;IdType&amp;gt;0&amp;lt;/IdType&amp;gt;&amp;lt;Score&amp;gt;750&amp;lt;/Score&amp;gt;&amp;lt;CapacidadEndeudamiento&amp;gt;1500000.50&amp;lt;/CapacidadEndeudamiento&amp;gt;&amp;lt;PorcEndFinal&amp;gt;35.5&amp;lt;/PorcEndFinal&amp;gt;&amp;lt;DSOnlineBureauCOLData /&amp;gt;&amp;lt;/Applicant&amp;gt;&amp;lt;/Applicants&amp;gt;&lt;/Field&gt;&lt;Field key="DecisionHomologada"&gt;VIABLE&lt;/Field&gt;&lt;/ContextData&gt;&lt;/DCResponse&gt;</ExecuteXMLStringResult>
    </ExecuteXMLStringResponse>
  </s:Body>
</s:Envelope>
```

**Cómo interpreta GA2 la respuesta.**

- Requiere `Status = Success` y `Authentication/Status` ∈ {`Success`, `FirstTimeLogIn`}.
- Deserializa `Field key="ApplicationData"` (`DecisionHomologada`, `UltimaEtapaFlujo`, `InputValReasonCodes` en los DTO `ResponseGeneralTu`/`ResponseErrorTuDos`).
- Con historial: segunda llamada a la cola `DocVerify`; acepta `Status = Success` o un `Field key="DecisionHomologada"` = `NO VIABLE`, y lee `Applicants/Applicant` (`Score`, `PorcEndFinal`, `CapacidadEndeudamiento`, `DSOnlineBureauCOLData`).
- La respuesta cruda se guarda en el Blob `trans-union` y en las tablas de log.


## 4. Errores técnicos

- `ConsumirTU` convierte cualquier excepción en `FaultException(ex.Message)` y `ConsumirTUAqm` en `Exception`. Los gestores las capturan y devuelven *"Error en la Conexión con TransUnion. …"* (`codigoError`/`codigoRechazo = 1`) en lugar de fallar con HTTP 409.
- Un `DCResponse` mal formado (no deserializable) produce el mismo efecto.
- Timeout: **no hay**. `ValidacionesTU` fija `Open/Close/Send/ReceiveTimeout = TimeSpan.MaxValue` y `MaxReceivedMessageSize = int.MaxValue`; un mock que no responde deja la petición colgada.
- SOAP Fault: mismo tratamiento (excepción capturada por el gestor).

## 5. Diseño recomendado del mock

1. **Despachar por `RequestInfo`:** `ExecutionMode=NewWithContext` + `SolutionSetId` (142 = IDVision, 360 = AQM) inicia la transacción y genera un `ApplicationId` nuevo. `ExecutionMode=Send` + `QueueName` (`PhoneSelection`, `PinVerification_OTPInput`, `DocVerify`) continúa la transacción de ese `ApplicationId`.
2. **Guardar estado por `ApplicationId`**: OTP generado, intentos y teléfono, para que el PIN `123456` (u otro configurable) devuelva 300 y los demás 302.
3. **Escenarios por documento** (`Applicant/IdNumber`) para elegir la respuesta: VIABLE / PENDIENTE / NO VIABLE, `MUERTE POR TITULAR`, cola inexistente, autenticación fallida (`Authentication/Status != Success`).
4. Responder **siempre** el `DCResponse` escapado dentro de `ExecuteXMLStringResult` (HTTP 200), salvo cuando se quiera probar un Fault.

## 6. Otras operaciones del contrato (no usadas por GA2)

No hay código de GA2 que las invoque: `AuthenticateXMLString`, `ChangePasswordXMLString`, `DownloadDocument`, `ExecuteXMLBinary`, `GetFinalTrace`, `GetQueueAppCountXMLString`, `GetAppCountByField`, `GetQueueAppCount`, `GetSolutionQueueNames`, `LockApplicationXMLString`, `LogoutSessionXMLString`, `ReadApplicationAllFieldsById`, `ReadApplicationDataById`, `ReadApplicationDataByIdAndQueue`, `ReadApplicationFieldsById`, `ReadApplicationFieldsByIdAndQueue`, `ReassignApplicationXMLString`, `RemoveDocument`, `ResetPasswordByUserXMLString`, `RetrieveAppsByRangeXMLString`, `RetrieveDocumentMetaDataXMLString`, `RetrieveSecretQuestionXMLString`, `RetrieveSSXMLString`, `RetrieveXMLString`, `SaveXMLString`, `SearchApplication`, `SearchApplicationXMLString`, `UnlockApplicationXMLString`, `UploadDocumentXMLString`, `GetDvCaseStatusHistory`, `ViewApplicationHistoryXMLString`, `ViewApplicationXMLString`.
