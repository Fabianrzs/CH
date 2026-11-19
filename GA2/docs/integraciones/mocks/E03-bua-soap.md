# E3 — BUA SOAP (Base Única de Afiliados de Caja Honor) — `IBUA_API.ObtenerBUA`

> Documento para implementar un **mock compatible** con `BUA_API.svc`. GA2 tiene **dos proxies** del mismo servicio con diferencias de tipos; aquí se documenta un único mock que sirve a ambos (**verificado** sirviendo la misma respuesta a los dos clientes WCF).

## 1. Resumen

| Aspecto | `ws.01` (`ConsumoBua`) | `ws.02` (`BuaService2BusinessLogic`) |
|---|---|---|
| URL efectiva | **Fija en código**: `http://cpvmp-09x/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc` (`ConsumoBua.cs:38` usa `new BUA_APIClient()` y **ignora** `SvcUtils:BuaUrlSvc`) | `SvcUtils:BuaUrlServiceApi` = `http://cpvmp-09x/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc` |
| Proxy | `ReferenciasSvc/Bua/Reference.cs` (namespace .NET `GA2.Domain.Core.BuaServiceSvc`) | `Connected Services/ServiceReference1/Reference.cs` |
| Timeouts / tamaño | Binding por defecto del `Reference.cs`: `SendTimeout` 1 min, `MaxReceivedMessageSize` = `int.MaxValue` | 5 min (open/send/receive), `MaxReceivedMessageSize` 10 MB |
| Manejo de errores | Ninguno (la excepción sube) | Captura `FaultException`, `EndpointNotFoundException`, `CommunicationException`, `TimeoutException` → `Response{IsSuccess=false}` |
| Consumidores | `CarteraBusinessLogic` (≈4537), `VerificacionDerechosBusinessLogic` (≈5345), `CuentasConceptosBusinessLogic` (≈542), `ClientsBusinessLogic.ObtenerClienteIntegracionGA2` (restaurar integración) | `ClientsBusinessLogic.ObtenerClienteIntegracionGA2New` (consulta de cliente por tipo y número), sólo si el toggle `Bua` ≠ `"false"` |
| Host | APIs BUA y GA2 | API BUA |

| Aspecto común | Valor |
|---|---|
| Protocolo | SOAP 1.1, WCF `basicHttpBinding`, `DataContractSerializer` |
| Contrato | `IBUA_API`, namespace `http://tempuri.org/`, `SOAPAction = "http://tempuri.org/IBUA_API/ObtenerBUA"` |
| Namespace de los DTO | `http://schemas.datacontract.org/2004/07/Kriterion.BEL` |
| Autenticación | Ninguna |
| Redirección al mock | `ws.02`: cambiar `SvcUtils:BuaUrlServiceApi`. `ws.01`: **no se puede por configuración**; hay que resolver `cpvmp-09x` hacia el mock (DNS/hosts) y servir la ruta `/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc` |

## 2. Operación `ObtenerBUA`

| Propiedad | Valor |
|---|---|
| Transporte | `POST` — SOAP 1.1 |
| Header `Content-Type` | `text/xml; charset=utf-8` |
| Header `SOAPAction` | `"http://tempuri.org/IBUA_API/ObtenerBUA"` |
| Elemento raíz request | `{http://tempuri.org/}ObtenerBUA` → `<afiliado>` (`BUAConsultaBE`) |
| Elemento raíz response | `{http://tempuri.org/}ObtenerBUAResponse` → `<ObtenerBUAResult>` (`BUA`) |

**Propósito.** Devuelve la ficha completa del afiliado: datos personales, residencia, cónyuge, grado, cuentas con conceptos, saldos, bloqueos y movimientos, aportes por categoría, dependientes, solicitudes, embargos y causales de bloqueo.

### 2.1 Request

| Elemento | Tipo | Descripción |
|---|---|---|
| `afiliado/identificacion` | `xs:string` | Número de documento |
| `afiliado/tipo_Id` | `xs:int` | Tipo de documento. GA2 reintenta en el orden **1 → 4 → 5** cuando el anterior no encuentra al afiliado (`ClientsBusinessLogic`, cuando el tipo recibido es `0`) |

```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <ObtenerBUA xmlns="http://tempuri.org/">
      <afiliado xmlns:a="http://schemas.datacontract.org/2004/07/Kriterion.BEL" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:identificacion>identificacion</a:identificacion>
        <a:tipo_Id>1</a:tipo_Id>
      </afiliado>
    </ObtenerBUA>
  </s:Body>
</s:Envelope>
```

### 2.2 Response

- **Afiliado encontrado:** `ObtenerBUAResult` con el objeto `BUA` (estructura completa en §3 y ejemplo en §2.4).
- **Afiliado no encontrado:** `ObtenerBUAResult` **nulo** (`<ObtenerBUAResult i:nil="true"/>`) o vacío. GA2 trata `ObtenerBUAResult == null` como "no existe" y prueba el siguiente `tipo_Id`.

**Cómo usa GA2 la respuesta.** El objeto se serializa a JSON (Newtonsoft) y se proyecta, **sin distinguir mayúsculas**, sobre `ClienteIntegracionGA2DTO` (`GA2.Application/Dto/BUA/ClienteIntegracionGA2DTO.cs`). Los campos que llegan al negocio son los que coinciden por nombre: `primerNombre`, `segundoNombre`, `primerApellido`, `segundoApellido`, `identificacion`, `fechaNacimiento`, `fechaExpedicionId`, `lugarExpedicion`, `ciudadNacimiento`, `sexo`, `estadoCivil`, `profesion`, `actividad`, `direccion` (`Celular`, `Telefono`, `CorreoPersonal`, `CorreoInstitucional`, `DireccionResidencia`, `CiudadResidencia`…), `conyuge`, `grado` (`IdGrado`, `Categoria`, `Fuerza`), `estadoActualCli`, `informacionFinanciera`, `fechaAlta`, `fechaInscripcion`, `derechoAdquirido`, `regimen`, `tasaAporte`, `participacion`, `modeloSubsidio`, `Cuentas[]` (con `ConceptosSaldosBloqueos[]` y `Movimientos[]`), `AportesCategoria[]`, `Dependiente[]`, `Solicitudes[]`, `CausalesBloqueoAfiliado[]`, `EmbargosAfiliado[]`. GA2 también guarda el JSON completo en `Payload`.

### 2.3 Diferencias de tipos entre `ws.01` y `ws.02` (reglas para un mock único)

| Elemento | `ws.01` | `ws.02` | Regla para el mock |
|---|---|---|---|
| `Cuentas/…/Movimientos/MovimientoCuentaBEL/valor` | `int?` | `string` | Enviar **un entero** (p. ej. `150000`). Un texto no numérico rompe `ws.01` (verificado: `SerializationException`) |
| `tasaAporte`, `tasaAporteVoluntario` | `decimal` | `int` | Enviar **enteros** |
| `CausalesBloqueoAfiliado`, `EmbargosAfiliado` | no existen | `CausalesBloqueoAfiliadoBEL[]`, `EmbargosAfiliadoBEL[]` | Se pueden incluir: `ws.01` los ignora |
| `ConceptoSaldoBloqueoBEL/{demandante, nombre_demandante, razon, solicitud, tipo_cuenta}` | no existen | `string` | Se pueden incluir: `ws.01` los ignora |

> **Orden:** `DataContractSerializer` exige el orden alfabético ordinal de los elementos (mayúsculas antes que minúsculas: `Anotaciones, AportesCategoria, CausalesBloqueoAfiliado, Cuentas, Dependiente, EmbargosAfiliado, Solicitudes, actividad, ciudadNacimiento, …`). Un elemento fuera de orden **se ignora sin error** y queda con su valor por defecto. Copiar el orden del ejemplo.

### 2.4 Response de ejemplo (estructura `ws.02`, validada contra **ambos** clientes)

Valores de relleno: textos = nombre del campo, números = `1`, fechas = `2026-01-15`, `valor = 150000`.

```xml
<s:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
  <s:Body>
    <ObtenerBUAResponse xmlns="http://tempuri.org/">
      <ObtenerBUAResult xmlns:d1p1="http://schemas.datacontract.org/2004/07/Kriterion.BEL" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <d1p1:Anotaciones>
          <d1p1:AnotacionesBEL>
            <d1p1:AFILIADO>AFILIADO</d1p1:AFILIADO>
            <d1p1:ANOTACION>ANOTACION</d1p1:ANOTACION>
            <d1p1:CLI_ID>1</d1p1:CLI_ID>
            <d1p1:COD_USUARIO>1</d1p1:COD_USUARIO>
            <d1p1:FECHA_ANOTACION>2026-01-15T00:00:00</d1p1:FECHA_ANOTACION>
            <d1p1:IDENTIFICACION>IDENTIFICACION</d1p1:IDENTIFICACION>
            <d1p1:USUARIO>USUARIO</d1p1:USUARIO>
          </d1p1:AnotacionesBEL>
        </d1p1:Anotaciones>
        <d1p1:AportesCategoria>
          <d1p1:AporteCategoriaBEL>
            <d1p1:cuotasAcumuladas>1</d1p1:cuotasAcumuladas>
            <d1p1:fechaPrimeraCuota>2026-01-15T00:00:00</d1p1:fechaPrimeraCuota>
            <d1p1:fechaUltimaCuota>2026-01-15T00:00:00</d1p1:fechaUltimaCuota>
            <d1p1:idCategoria>1</d1p1:idCategoria>
          </d1p1:AporteCategoriaBEL>
        </d1p1:AportesCategoria>
        <d1p1:CausalesBloqueoAfiliado>
          <d1p1:CausalesBloqueoAfiliadoBEL>
            <d1p1:CAUSAL>CAUSAL</d1p1:CAUSAL>
            <d1p1:CEC_ID>CEC_ID</d1p1:CEC_ID>
            <d1p1:CEL_ID>CEL_ID</d1p1:CEL_ID>
            <d1p1:COLUMNA_SELECCION>COLUMNA_SELECCION</d1p1:COLUMNA_SELECCION>
            <d1p1:ECL_ID>ECL_ID</d1p1:ECL_ID>
            <d1p1:ESTADO>ESTADO</d1p1:ESTADO>
            <d1p1:ESTADO_BLOQUEO>ESTADO_BLOQUEO</d1p1:ESTADO_BLOQUEO>
            <d1p1:FECHA_REGISTRO>2026-01-15T00:00:00</d1p1:FECHA_REGISTRO>
            <d1p1:FECHA_RETIRO>2026-01-15T00:00:00</d1p1:FECHA_RETIRO>
            <d1p1:OBSERVACIONES>OBSERVACIONES</d1p1:OBSERVACIONES>
          </d1p1:CausalesBloqueoAfiliadoBEL>
        </d1p1:CausalesBloqueoAfiliado>
        <d1p1:Cuentas>
          <d1p1:CuentaBEL>
            <d1p1:causalEstadoCuenta>1</d1p1:causalEstadoCuenta>
            <d1p1:conceptosSaldosBloqueos>
              <d1p1:ConceptoSaldoBloqueoBEL>
                <d1p1:cantidadIngresos>1</d1p1:cantidadIngresos>
                <d1p1:concepto>concepto</d1p1:concepto>
                <d1p1:demandante>demandante</d1p1:demandante>
                <d1p1:documento>1</d1p1:documento>
                <d1p1:estadoBloqueo>estadoBloqueo</d1p1:estadoBloqueo>
                <d1p1:fechaBloqueo>2026-01-15T00:00:00</d1p1:fechaBloqueo>
                <d1p1:fechaUltimoAporteConcepto>2026-01-15T00:00:00</d1p1:fechaUltimoAporteConcepto>
                <d1p1:idConcepto>1</d1p1:idConcepto>
                <d1p1:idExpd>1</d1p1:idExpd>
                <d1p1:nombre_demandante>nombre_demandante</d1p1:nombre_demandante>
                <d1p1:nroExpd>nroExpd</d1p1:nroExpd>
                <d1p1:origen>origen</d1p1:origen>
                <d1p1:razon>razon</d1p1:razon>
                <d1p1:saldoConceptoCuenta>1.5</d1p1:saldoConceptoCuenta>
                <d1p1:saldoInicialAno>1.5</d1p1:saldoInicialAno>
                <d1p1:solicitud>solicitud</d1p1:solicitud>
                <d1p1:tipo_cuenta>tipo_cuenta</d1p1:tipo_cuenta>
                <d1p1:valorAcumulado>1.5</d1p1:valorAcumulado>
                <d1p1:valorBloqueo>1.5</d1p1:valorBloqueo>
              </d1p1:ConceptoSaldoBloqueoBEL>
            </d1p1:conceptosSaldosBloqueos>
            <d1p1:cuotas>1</d1p1:cuotas>
            <d1p1:documento>
              <d1p1:alerta>alerta</d1p1:alerta>
              <d1p1:anulacion>1</d1p1:anulacion>
              <d1p1:fechaDecision>2026-01-15T00:00:00</d1p1:fechaDecision>
              <d1p1:fechaInicial>2026-01-15T00:00:00</d1p1:fechaInicial>
              <d1p1:idCausalEstado>1</d1p1:idCausalEstado>
              <d1p1:idDocumento>1</d1p1:idDocumento>
              <d1p1:idEstadoDocumento>1</d1p1:idEstadoDocumento>
              <d1p1:idTipoDocumento>1</d1p1:idTipoDocumento>
            </d1p1:documento>
            <d1p1:estadoCuenta>1</d1p1:estadoCuenta>
            <d1p1:fechaCierre>2026-01-15T00:00:00</d1p1:fechaCierre>
            <d1p1:fechaCreacion>2026-01-15T00:00:00</d1p1:fechaCreacion>
            <d1p1:fechaPrimerAporte>2026-01-15T00:00:00</d1p1:fechaPrimerAporte>
            <d1p1:idCuenta>1</d1p1:idCuenta>
            <d1p1:idTipoProducto>1</d1p1:idTipoProducto>
            <d1p1:movimientos>
              <d1p1:MovimientoCuentaBEL>
                <d1p1:anoPeriodo>anoPeriodo</d1p1:anoPeriodo>
                <d1p1:documento>
                  <d1p1:alerta>alerta</d1p1:alerta>
                  <d1p1:anulacion>1</d1p1:anulacion>
                  <d1p1:fechaDecision>2026-01-15T00:00:00</d1p1:fechaDecision>
                  <d1p1:fechaInicial>2026-01-15T00:00:00</d1p1:fechaInicial>
                  <d1p1:idCausalEstado>1</d1p1:idCausalEstado>
                  <d1p1:idDocumento>1</d1p1:idDocumento>
                  <d1p1:idEstadoDocumento>1</d1p1:idEstadoDocumento>
                  <d1p1:idTipoDocumento>1</d1p1:idTipoDocumento>
                </d1p1:documento>
                <d1p1:fechaProceso>2026-01-15T00:00:00</d1p1:fechaProceso>
                <d1p1:idConcepto>1</d1p1:idConcepto>
                <d1p1:idMovimiento>1</d1p1:idMovimiento>
                <d1p1:tipoMovimiento>tipoMovimiento</d1p1:tipoMovimiento>
                <d1p1:valor>150000</d1p1:valor>
              </d1p1:MovimientoCuentaBEL>
            </d1p1:movimientos>
            <d1p1:numeroCuenta>numeroCuenta</d1p1:numeroCuenta>
            <d1p1:saldo>1.5</d1p1:saldo>
            <d1p1:saldoDisponible>1.5</d1p1:saldoDisponible>
            <d1p1:saldoEnCanje>1.5</d1p1:saldoEnCanje>
          </d1p1:CuentaBEL>
        </d1p1:Cuentas>
        <d1p1:Dependiente>
          <d1p1:BeneficiarioBEL>
            <d1p1:identificacion>identificacion</d1p1:identificacion>
            <d1p1:nombreDependiente>nombreDependiente</d1p1:nombreDependiente>
            <d1p1:particDependiente>1.5</d1p1:particDependiente>
          </d1p1:BeneficiarioBEL>
        </d1p1:Dependiente>
        <d1p1:EmbargosAfiliado>
          <d1p1:EmbargosAfiliadoBEL>
            <d1p1:CODIGO_JUZGADO>CODIGO_JUZGADO</d1p1:CODIGO_JUZGADO>
            <d1p1:CTA_ID>CTA_ID</d1p1:CTA_ID>
            <d1p1:CUENTA>CUENTA</d1p1:CUENTA>
            <d1p1:ESTADO_BLOQUEO>ESTADO_BLOQUEO</d1p1:ESTADO_BLOQUEO>
            <d1p1:ESTADO_SOLICITUD>ESTADO_SOLICITUD</d1p1:ESTADO_SOLICITUD>
            <d1p1:MONTOPORCENTAJE>MONTOPORCENTAJE</d1p1:MONTOPORCENTAJE>
            <d1p1:NOMBRE_DEMANDANTE>NOMBRE_DEMANDANTE</d1p1:NOMBRE_DEMANDANTE>
            <d1p1:RADICADO>RADICADO</d1p1:RADICADO>
            <d1p1:RAZON>RAZON</d1p1:RAZON>
            <d1p1:SLC_ID>SLC_ID</d1p1:SLC_ID>
            <d1p1:SOLICITUD>SOLICITUD</d1p1:SOLICITUD>
          </d1p1:EmbargosAfiliadoBEL>
        </d1p1:EmbargosAfiliado>
        <d1p1:Solicitudes>
          <d1p1:SolicitudAfiliadoBEL>
            <d1p1:BENEFICIARIO>BENEFICIARIO</d1p1:BENEFICIARIO>
            <d1p1:CDP>CDP</d1p1:CDP>
            <d1p1:CLI_ID>1</d1p1:CLI_ID>
            <d1p1:COD_CONCEPTO_APLICA>1</d1p1:COD_CONCEPTO_APLICA>
            <d1p1:COD_ESTADO_DOCUMENTO>1</d1p1:COD_ESTADO_DOCUMENTO>
            <d1p1:COD_ESTADO_SOLICITUD>1</d1p1:COD_ESTADO_SOLICITUD>
            <d1p1:COD_MODALIDAD_SOLICITUD>1</d1p1:COD_MODALIDAD_SOLICITUD>
            <d1p1:COD_TIPO_DOCUMENTO>1</d1p1:COD_TIPO_DOCUMENTO>
            <d1p1:COD_TIPO_SOLICITUD>1</d1p1:COD_TIPO_SOLICITUD>
            <d1p1:COD_USUARIO_APROBADOR>1</d1p1:COD_USUARIO_APROBADOR>
            <d1p1:COD_USUARIO_CREA>1</d1p1:COD_USUARIO_CREA>
            <d1p1:CONCEPTO_APLICA>CONCEPTO_APLICA</d1p1:CONCEPTO_APLICA>
            <d1p1:DOCUMENTO_CAUSANTE>DOCUMENTO_CAUSANTE</d1p1:DOCUMENTO_CAUSANTE>
            <d1p1:DOCUMENTO_ID>1</d1p1:DOCUMENTO_ID>
            <d1p1:ESTADO_DOCUMENTO>ESTADO_DOCUMENTO</d1p1:ESTADO_DOCUMENTO>
            <d1p1:ESTADO_SOLICITUD>ESTADO_SOLICITUD</d1p1:ESTADO_SOLICITUD>
            <d1p1:FACTURA_PAGO>FACTURA_PAGO</d1p1:FACTURA_PAGO>
            <d1p1:FECHA_FINAL_SOLICITUD>2026-01-15T00:00:00</d1p1:FECHA_FINAL_SOLICITUD>
            <d1p1:FECHA_INICIAL_DOCUMENTO>2026-01-15T00:00:00</d1p1:FECHA_INICIAL_DOCUMENTO>
            <d1p1:FECHA_INICIO_SOLICITUD>2026-01-15T00:00:00</d1p1:FECHA_INICIO_SOLICITUD>
            <d1p1:IDENTIFICACION_BENEFICIARIO>IDENTIFICACION_BENEFICIARIO</d1p1:IDENTIFICACION_BENEFICIARIO>
            <d1p1:INTERES>1.5</d1p1:INTERES>
            <d1p1:MODALIDAD_SOLICITUD>MODALIDAD_SOLICITUD</d1p1:MODALIDAD_SOLICITUD>
            <d1p1:MONTO>1.5</d1p1:MONTO>
            <d1p1:MONTO_TOTAL_PAGO>1.5</d1p1:MONTO_TOTAL_PAGO>
            <d1p1:NRO_CUENTA>1</d1p1:NRO_CUENTA>
            <d1p1:NRO_SOLICITUD>1</d1p1:NRO_SOLICITUD>
            <d1p1:OBSERVACION_SOLICITUD>OBSERVACION_SOLICITUD</d1p1:OBSERVACION_SOLICITUD>
            <d1p1:RADICADO>RADICADO</d1p1:RADICADO>
            <d1p1:SOLICITANTE>SOLICITANTE</d1p1:SOLICITANTE>
            <d1p1:TIPO_DOCUMENTO>TIPO_DOCUMENTO</d1p1:TIPO_DOCUMENTO>
            <d1p1:TIPO_SOLICITUD>TIPO_SOLICITUD</d1p1:TIPO_SOLICITUD>
            <d1p1:USUARIO_APROBADOR>USUARIO_APROBADOR</d1p1:USUARIO_APROBADOR>
            <d1p1:USUARIO_CREA>USUARIO_CREA</d1p1:USUARIO_CREA>
            <d1p1:VALOR_DISPONIBLE>1.5</d1p1:VALOR_DISPONIBLE>
            <d1p1:VALOR_TOTAL_PAGADO_BENEFICIARIO>1</d1p1:VALOR_TOTAL_PAGADO_BENEFICIARIO>
          </d1p1:SolicitudAfiliadoBEL>
        </d1p1:Solicitudes>
        <d1p1:actividad>actividad</d1p1:actividad>
        <d1p1:ciudadNacimiento>
          <d1p1:departamento>
            <d1p1:idDepartamento>idDepartamento</d1p1:idDepartamento>
            <d1p1:pais>
              <d1p1:idPais>idPais</d1p1:idPais>
            </d1p1:pais>
          </d1p1:departamento>
          <d1p1:idCiudad>idCiudad</d1p1:idCiudad>
        </d1p1:ciudadNacimiento>
        <d1p1:conyuge>
          <d1p1:celularConyuge>celularConyuge</d1p1:celularConyuge>
          <d1p1:identificacionConyuge>identificacionConyuge</d1p1:identificacionConyuge>
          <d1p1:primerApellidoConyuge>primerApellidoConyuge</d1p1:primerApellidoConyuge>
          <d1p1:primerNombreConyuge>primerNombreConyuge</d1p1:primerNombreConyuge>
          <d1p1:segundoApellidoConyuge>segundoApellidoConyuge</d1p1:segundoApellidoConyuge>
          <d1p1:segundoNombreConyuge>segundoNombreConyuge</d1p1:segundoNombreConyuge>
          <d1p1:telefonoFijoConyuge>telefonoFijoConyuge</d1p1:telefonoFijoConyuge>
          <d1p1:tipoIdConyuge>
            <d1p1:idTipoId>1</d1p1:idTipoId>
          </d1p1:tipoIdConyuge>
        </d1p1:conyuge>
        <d1p1:derechoAdquirido>2026-01-15T00:00:00</d1p1:derechoAdquirido>
        <d1p1:direccion>
          <d1p1:barrioResidencia>barrioResidencia</d1p1:barrioResidencia>
          <d1p1:celular>celular</d1p1:celular>
          <d1p1:ciudadResidencia>
            <d1p1:departamento>
              <d1p1:idDepartamento>idDepartamento</d1p1:idDepartamento>
              <d1p1:pais>
                <d1p1:idPais>idPais</d1p1:idPais>
              </d1p1:pais>
            </d1p1:departamento>
            <d1p1:idCiudad>idCiudad</d1p1:idCiudad>
          </d1p1:ciudadResidencia>
          <d1p1:correoInstitucional>correoInstitucional</d1p1:correoInstitucional>
          <d1p1:correoPersonal>correoPersonal</d1p1:correoPersonal>
          <d1p1:direccionResidencia>direccionResidencia</d1p1:direccionResidencia>
          <d1p1:fechaActualizacion>2026-01-15T00:00:00</d1p1:fechaActualizacion>
          <d1p1:telefono>telefono</d1p1:telefono>
          <d1p1:tipoDirCo>1</d1p1:tipoDirCo>
          <d1p1:tipoDirRes>1</d1p1:tipoDirRes>
        </d1p1:direccion>
        <d1p1:envioInformacion>true</d1p1:envioInformacion>
        <d1p1:estadoActualCli>
          <d1p1:causalBloqueo>
            <d1p1:CausalBloqueoBEL>
              <d1p1:idBloqueo>1</d1p1:idBloqueo>
            </d1p1:CausalBloqueoBEL>
          </d1p1:causalBloqueo>
          <d1p1:idEstado>1</d1p1:idEstado>
        </d1p1:estadoActualCli>
        <d1p1:estadoCivil>estadoCivil</d1p1:estadoCivil>
        <d1p1:fechaActualizaDatos>2026-01-15T00:00:00</d1p1:fechaActualizaDatos>
        <d1p1:fechaAdquisicionVivienda>2026-01-15T00:00:00</d1p1:fechaAdquisicionVivienda>
        <d1p1:fechaAlta>2026-01-15T00:00:00</d1p1:fechaAlta>
        <d1p1:fechaAltaCalculada>2026-01-15T00:00:00</d1p1:fechaAltaCalculada>
        <d1p1:fechaExpedicionId>2026-01-15T00:00:00</d1p1:fechaExpedicionId>
        <d1p1:fechaInscripcion>2026-01-15T00:00:00</d1p1:fechaInscripcion>
        <d1p1:fechaNacimiento>2026-01-15T00:00:00</d1p1:fechaNacimiento>
        <d1p1:grado>
          <d1p1:IdGrado>1</d1p1:IdGrado>
          <d1p1:categoria>
            <d1p1:idCategoria>1</d1p1:idCategoria>
          </d1p1:categoria>
          <d1p1:fuerza>
            <d1p1:IdFuerza>1</d1p1:IdFuerza>
          </d1p1:fuerza>
        </d1p1:grado>
        <d1p1:idPatrocinador>1</d1p1:idPatrocinador>
        <d1p1:idRegistro>1</d1p1:idRegistro>
        <d1p1:identificacion>identificacion</d1p1:identificacion>
        <d1p1:informacionFinanciera>
          <d1p1:conceptoIngresosAdicionales>conceptoIngresosAdicionales</d1p1:conceptoIngresosAdicionales>
          <d1p1:egresoMensual>1</d1p1:egresoMensual>
          <d1p1:ingresoMensual>1</d1p1:ingresoMensual>
          <d1p1:ingresosAdicionales>1</d1p1:ingresosAdicionales>
          <d1p1:totalActivos>1</d1p1:totalActivos>
          <d1p1:totalPasivos>1</d1p1:totalPasivos>
          <d1p1:totalPatrimonio>1</d1p1:totalPatrimonio>
        </d1p1:informacionFinanciera>
        <d1p1:lugarExpedicion>
          <d1p1:departamento>
            <d1p1:idDepartamento>idDepartamento</d1p1:idDepartamento>
            <d1p1:pais>
              <d1p1:idPais>idPais</d1p1:idPais>
            </d1p1:pais>
          </d1p1:departamento>
          <d1p1:idCiudad>idCiudad</d1p1:idCiudad>
        </d1p1:lugarExpedicion>
        <d1p1:modeloSubsidio>1</d1p1:modeloSubsidio>
        <d1p1:participacion>1</d1p1:participacion>
        <d1p1:participacionSubsidio>1</d1p1:participacionSubsidio>
        <d1p1:primerApellido>primerApellido</d1p1:primerApellido>
        <d1p1:primerNombre>primerNombre</d1p1:primerNombre>
        <d1p1:profesion>profesion</d1p1:profesion>
        <d1p1:regimen>1</d1p1:regimen>
        <d1p1:resolucion>resolucion</d1p1:resolucion>
        <d1p1:segundoApellido>segundoApellido</d1p1:segundoApellido>
        <d1p1:segundoNombre>segundoNombre</d1p1:segundoNombre>
        <d1p1:sexo>sexo</d1p1:sexo>
        <d1p1:tasaAporte>1</d1p1:tasaAporte>
        <d1p1:tasaAporteVoluntario>1</d1p1:tasaAporteVoluntario>
        <d1p1:tasaSalarioBase>1</d1p1:tasaSalarioBase>
        <d1p1:tipoActor>
          <d1p1:idTipoActor>1</d1p1:idTipoActor>
        </d1p1:tipoActor>
        <d1p1:tipoId>
          <d1p1:idTipoId>1</d1p1:idTipoId>
        </d1p1:tipoId>
        <d1p1:tipoViviendaPosee>
          <d1p1:tipoViviendaId>1</d1p1:tipoViviendaId>
        </d1p1:tipoViviendaPosee>
        <d1p1:unidad>unidad</d1p1:unidad>
        <d1p1:unidadEjecutora>1</d1p1:unidadEjecutora>
      </ObtenerBUAResult>
    </ObtenerBUAResponse>
  </s:Body>
</s:Envelope>
```

### 2.5 Errores

- **SOAP Fault / caída:** en `ws.02` GA2 lo captura y devuelve `IsSuccess=false` (el flujo usa sólo la BD local). En `ws.01` la excepción **sube** (HTTP 409 (Conflict) en la API que la invocó, vía `ErrorValidationMiddleware`).
- **Timeout:** `ws.02` falla a los 5 minutos; `ws.01` al minuto (`SendTimeout` por defecto).
- **Tamaño:** `ws.02` rechaza respuestas de más de 10 MB (`QuotaExceededException`).

## 3. Tipos de datos (`ws.02`, superconjunto de `ws.01`)

Tablas generadas por reflexión sobre los proxies compilados. El orden de las filas es el orden en que el elemento aparece en el XML (cuando el serializador es `DataContractSerializer`, el orden es: primero los miembros sin `Order` en orden alfabético ordinal y luego por `Order`). Ante cualquier duda, manda el ejemplo de sobre SOAP de cada operación.

### `ActorBEL`

Tipo XML `ActorBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idTipoActor` | `xs:int` (nillable) | opcional |  |

### `AnotacionesBEL`

Tipo XML `AnotacionesBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `AFILIADO` | `xs:string` | opcional |  |
| 2 | `ANOTACION` | `xs:string` | opcional |  |
| 3 | `CLI_ID` | `xs:int` (nillable) | opcional |  |
| 4 | `COD_USUARIO` | `xs:int` (nillable) | opcional |  |
| 5 | `FECHA_ANOTACION` | `xs:dateTime` (nillable) | opcional |  |
| 6 | `IDENTIFICACION` | `xs:string` | opcional |  |
| 7 | `USUARIO` | `xs:string` | opcional |  |

### `AporteCategoriaBEL`

Tipo XML `AporteCategoriaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `cuotasAcumuladas` | `xs:int` (nillable) | opcional |  |
| 2 | `fechaPrimeraCuota` | `xs:dateTime` (nillable) | opcional |  |
| 3 | `fechaUltimaCuota` | `xs:dateTime` (nillable) | opcional |  |
| 4 | `idCategoria` | `xs:int` (nillable) | opcional |  |

### `BeneficiarioBEL`

Tipo XML `BeneficiarioBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `identificacion` | `xs:string` | opcional |  |
| 2 | `nombreDependiente` | `xs:string` | opcional |  |
| 3 | `particDependiente` | `xs:decimal` | siempre presente |  |

### `BUA`

Tipo XML `BUA` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `Anotaciones` | lista de `AnotacionesBEL` | opcional |  |
| 2 | `AportesCategoria` | lista de `AporteCategoriaBEL` | opcional |  |
| 3 | `CausalesBloqueoAfiliado` | lista de `CausalesBloqueoAfiliadoBEL` | opcional |  |
| 4 | `Cuentas` | lista de `CuentaBEL` | opcional |  |
| 5 | `Dependiente` | lista de `BeneficiarioBEL` | opcional |  |
| 6 | `EmbargosAfiliado` | lista de `EmbargosAfiliadoBEL` | opcional |  |
| 7 | `Solicitudes` | lista de `SolicitudAfiliadoBEL` | opcional |  |
| 8 | `actividad` | `xs:string` | opcional |  |
| 9 | `ciudadNacimiento` | `CiudadBEL` | opcional |  |
| 10 | `conyuge` | `ConyugeBEL` | opcional |  |
| 11 | `derechoAdquirido` | `xs:dateTime` | siempre presente |  |
| 12 | `direccion` | `ResidenciaBEL` | opcional |  |
| 13 | `envioInformacion` | `xs:boolean` | siempre presente |  |
| 14 | `estadoActualCli` | `EstadoClienteBEL` | opcional |  |
| 15 | `estadoCivil` | `xs:string` | opcional |  |
| 16 | `fechaActualizaDatos` | `xs:dateTime` | siempre presente |  |
| 17 | `fechaAdquisicionVivienda` | `xs:dateTime` | siempre presente |  |
| 18 | `fechaAlta` | `xs:dateTime` | siempre presente |  |
| 19 | `fechaAltaCalculada` | `xs:dateTime` | siempre presente |  |
| 20 | `fechaExpedicionId` | `xs:dateTime` | siempre presente |  |
| 21 | `fechaInscripcion` | `xs:dateTime` | siempre presente |  |
| 22 | `fechaNacimiento` | `xs:dateTime` | siempre presente |  |
| 23 | `grado` | `GradoBEL` | opcional |  |
| 24 | `idPatrocinador` | `xs:int` | siempre presente |  |
| 25 | `idRegistro` | `xs:int` | siempre presente |  |
| 26 | `identificacion` | `xs:string` | opcional |  |
| 27 | `informacionFinanciera` | `InformacionFinancieraBEL` | opcional |  |
| 28 | `lugarExpedicion` | `CiudadBEL` | opcional |  |
| 29 | `modeloSubsidio` | `xs:int` | siempre presente |  |
| 30 | `participacion` | `xs:int` | siempre presente |  |
| 31 | `participacionSubsidio` | `xs:int` | siempre presente |  |
| 32 | `primerApellido` | `xs:string` | opcional |  |
| 33 | `primerNombre` | `xs:string` | opcional |  |
| 34 | `profesion` | `xs:string` | opcional |  |
| 35 | `regimen` | `xs:int` | siempre presente |  |
| 36 | `resolucion` | `xs:string` | opcional |  |
| 37 | `segundoApellido` | `xs:string` | opcional |  |
| 38 | `segundoNombre` | `xs:string` | opcional |  |
| 39 | `sexo` | `xs:string` | opcional |  |
| 40 | `tasaAporte` | `xs:int` | siempre presente |  |
| 41 | `tasaAporteVoluntario` | `xs:int` | siempre presente |  |
| 42 | `tasaSalarioBase` | `xs:int` | siempre presente |  |
| 43 | `tipoActor` | `ActorBEL` | opcional |  |
| 44 | `tipoId` | `TipoIdentificacionBEL` | opcional |  |
| 45 | `tipoViviendaPosee` | `TipoViviendaBEL` | opcional |  |
| 46 | `unidad` | `xs:string` | opcional |  |
| 47 | `unidadEjecutora` | `xs:int` | siempre presente |  |

### `BUAConsultaBE`

Tipo XML `BUAConsultaBE` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `identificacion` | `xs:string` | opcional |  |
| 2 | `tipo_Id` | `xs:int` | siempre presente |  |

### `CategoriaBEL`

Tipo XML `CategoriaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idCategoria` | `xs:int` (nillable) | opcional |  |

### `CausalBloqueoBEL`

Tipo XML `CausalBloqueoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idBloqueo` | `xs:int` (nillable) | opcional |  |

### `CausalesBloqueoAfiliadoBEL`

Tipo XML `CausalesBloqueoAfiliadoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `CAUSAL` | `xs:string` | opcional |  |
| 2 | `CEC_ID` | `xs:string` | opcional |  |
| 3 | `CEL_ID` | `xs:string` | opcional |  |
| 4 | `COLUMNA_SELECCION` | `xs:string` | opcional |  |
| 5 | `ECL_ID` | `xs:string` | opcional |  |
| 6 | `ESTADO` | `xs:string` | opcional |  |
| 7 | `ESTADO_BLOQUEO` | `xs:string` | opcional |  |
| 8 | `FECHA_REGISTRO` | `xs:dateTime` (nillable) | opcional |  |
| 9 | `FECHA_RETIRO` | `xs:dateTime` (nillable) | opcional |  |
| 10 | `OBSERVACIONES` | `xs:string` | opcional |  |

### `CiudadBEL`

Tipo XML `CiudadBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `departamento` | `DepartamentoBEL` | opcional |  |
| 2 | `idCiudad` | `xs:string` | opcional |  |

### `ConceptoSaldoBloqueoBEL`

Tipo XML `ConceptoSaldoBloqueoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `cantidadIngresos` | `xs:int` (nillable) | opcional |  |
| 2 | `concepto` | `xs:string` | opcional |  |
| 3 | `demandante` | `xs:string` | opcional |  |
| 4 | `documento` | `xs:int` (nillable) | opcional |  |
| 5 | `estadoBloqueo` | `xs:string` | opcional |  |
| 6 | `fechaBloqueo` | `xs:dateTime` (nillable) | opcional |  |
| 7 | `fechaUltimoAporteConcepto` | `xs:dateTime` (nillable) | opcional |  |
| 8 | `idConcepto` | `xs:int` (nillable) | opcional |  |
| 9 | `idExpd` | `xs:int` (nillable) | opcional |  |
| 10 | `nombre_demandante` | `xs:string` | opcional |  |
| 11 | `nroExpd` | `xs:string` | opcional |  |
| 12 | `origen` | `xs:string` | opcional |  |
| 13 | `razon` | `xs:string` | opcional |  |
| 14 | `saldoConceptoCuenta` | `xs:decimal` (nillable) | opcional |  |
| 15 | `saldoInicialAno` | `xs:decimal` (nillable) | opcional |  |
| 16 | `solicitud` | `xs:string` | opcional |  |
| 17 | `tipo_cuenta` | `xs:string` | opcional |  |
| 18 | `valorAcumulado` | `xs:decimal` (nillable) | opcional |  |
| 19 | `valorBloqueo` | `xs:decimal` (nillable) | opcional |  |

### `ConyugeBEL`

Tipo XML `ConyugeBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `celularConyuge` | `xs:string` | opcional |  |
| 2 | `identificacionConyuge` | `xs:string` | opcional |  |
| 3 | `primerApellidoConyuge` | `xs:string` | opcional |  |
| 4 | `primerNombreConyuge` | `xs:string` | opcional |  |
| 5 | `segundoApellidoConyuge` | `xs:string` | opcional |  |
| 6 | `segundoNombreConyuge` | `xs:string` | opcional |  |
| 7 | `telefonoFijoConyuge` | `xs:string` | opcional |  |
| 8 | `tipoIdConyuge` | `TipoIdentificacionBEL` | opcional |  |

### `CuentaBEL`

Tipo XML `CuentaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `causalEstadoCuenta` | `xs:int` (nillable) | opcional |  |
| 2 | `conceptosSaldosBloqueos` | lista de `ConceptoSaldoBloqueoBEL` | opcional |  |
| 3 | `cuotas` | `xs:int` (nillable) | opcional |  |
| 4 | `documento` | `DocumentoBEL` | opcional |  |
| 5 | `estadoCuenta` | `xs:int` (nillable) | opcional |  |
| 6 | `fechaCierre` | `xs:dateTime` (nillable) | opcional |  |
| 7 | `fechaCreacion` | `xs:dateTime` (nillable) | opcional |  |
| 8 | `fechaPrimerAporte` | `xs:dateTime` (nillable) | opcional |  |
| 9 | `idCuenta` | `xs:int` | siempre presente |  |
| 10 | `idTipoProducto` | `xs:int` (nillable) | opcional |  |
| 11 | `movimientos` | lista de `MovimientoCuentaBEL` | opcional |  |
| 12 | `numeroCuenta` | `xs:string` | opcional |  |
| 13 | `saldo` | `xs:decimal` (nillable) | opcional |  |
| 14 | `saldoDisponible` | `xs:decimal` (nillable) | opcional |  |
| 15 | `saldoEnCanje` | `xs:decimal` (nillable) | opcional |  |

### `DepartamentoBEL`

Tipo XML `DepartamentoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idDepartamento` | `xs:string` | opcional |  |
| 2 | `pais` | `PaisBEL` | opcional |  |

### `DocumentoBEL`

Tipo XML `DocumentoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `alerta` | `xs:string` | opcional |  |
| 2 | `anulacion` | `xs:int` (nillable) | opcional |  |
| 3 | `fechaDecision` | `xs:dateTime` (nillable) | opcional |  |
| 4 | `fechaInicial` | `xs:dateTime` (nillable) | opcional |  |
| 5 | `idCausalEstado` | `xs:int` (nillable) | opcional |  |
| 6 | `idDocumento` | `xs:int` (nillable) | opcional |  |
| 7 | `idEstadoDocumento` | `xs:int` (nillable) | opcional |  |
| 8 | `idTipoDocumento` | `xs:int` (nillable) | opcional |  |

### `EmbargosAfiliadoBEL`

Tipo XML `EmbargosAfiliadoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `CODIGO_JUZGADO` | `xs:string` | opcional |  |
| 2 | `CTA_ID` | `xs:string` | opcional |  |
| 3 | `CUENTA` | `xs:string` | opcional |  |
| 4 | `ESTADO_BLOQUEO` | `xs:string` | opcional |  |
| 5 | `ESTADO_SOLICITUD` | `xs:string` | opcional |  |
| 6 | `MONTOPORCENTAJE` | `xs:string` | opcional |  |
| 7 | `NOMBRE_DEMANDANTE` | `xs:string` | opcional |  |
| 8 | `RADICADO` | `xs:string` | opcional |  |
| 9 | `RAZON` | `xs:string` | opcional |  |
| 10 | `SLC_ID` | `xs:string` | opcional |  |
| 11 | `SOLICITUD` | `xs:string` | opcional |  |

### `EstadoClienteBEL`

Tipo XML `EstadoClienteBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `causalBloqueo` | lista de `CausalBloqueoBEL` | opcional |  |
| 2 | `idEstado` | `xs:int` (nillable) | opcional |  |

### `FuerzaBEL`

Tipo XML `FuerzaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `IdFuerza` | `xs:int` (nillable) | opcional |  |

### `GradoBEL`

Tipo XML `GradoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `IdGrado` | `xs:int` (nillable) | opcional |  |
| 2 | `categoria` | `CategoriaBEL` | opcional |  |
| 3 | `fuerza` | `FuerzaBEL` | opcional |  |

### `InformacionFinancieraBEL`

Tipo XML `InformacionFinancieraBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `conceptoIngresosAdicionales` | `xs:string` | opcional |  |
| 2 | `egresoMensual` | `xs:int` (nillable) | opcional |  |
| 3 | `ingresoMensual` | `xs:int` (nillable) | opcional |  |
| 4 | `ingresosAdicionales` | `xs:int` (nillable) | opcional |  |
| 5 | `totalActivos` | `xs:int` (nillable) | opcional |  |
| 6 | `totalPasivos` | `xs:int` (nillable) | opcional |  |
| 7 | `totalPatrimonio` | `xs:int` (nillable) | opcional |  |

### `MovimientoCuentaBEL`

Tipo XML `MovimientoCuentaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `anoPeriodo` | `xs:string` | opcional |  |
| 2 | `documento` | `DocumentoBEL` | opcional |  |
| 3 | `fechaProceso` | `xs:dateTime` (nillable) | opcional |  |
| 4 | `idConcepto` | `xs:int` (nillable) | opcional |  |
| 5 | `idMovimiento` | `xs:int` (nillable) | opcional |  |
| 6 | `tipoMovimiento` | `xs:string` | opcional |  |
| 7 | `valor` | `xs:string` | opcional |  |

### `PaisBEL`

Tipo XML `PaisBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idPais` | `xs:string` | opcional |  |

### `ResidenciaBEL`

Tipo XML `ResidenciaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `barrioResidencia` | `xs:string` | opcional |  |
| 2 | `celular` | `xs:string` | opcional |  |
| 3 | `ciudadResidencia` | `CiudadBEL` | opcional |  |
| 4 | `correoInstitucional` | `xs:string` | opcional |  |
| 5 | `correoPersonal` | `xs:string` | opcional |  |
| 6 | `direccionResidencia` | `xs:string` | opcional |  |
| 7 | `fechaActualizacion` | `xs:dateTime` | siempre presente |  |
| 8 | `telefono` | `xs:string` | opcional |  |
| 9 | `tipoDirCo` | `xs:int` (nillable) | opcional |  |
| 10 | `tipoDirRes` | `xs:int` (nillable) | opcional |  |

### `SolicitudAfiliadoBEL`

Tipo XML `SolicitudAfiliadoBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `BENEFICIARIO` | `xs:string` | opcional |  |
| 2 | `CDP` | `xs:string` | opcional |  |
| 3 | `CLI_ID` | `xs:int` (nillable) | opcional |  |
| 4 | `COD_CONCEPTO_APLICA` | `xs:int` (nillable) | opcional |  |
| 5 | `COD_ESTADO_DOCUMENTO` | `xs:int` (nillable) | opcional |  |
| 6 | `COD_ESTADO_SOLICITUD` | `xs:int` (nillable) | opcional |  |
| 7 | `COD_MODALIDAD_SOLICITUD` | `xs:int` (nillable) | opcional |  |
| 8 | `COD_TIPO_DOCUMENTO` | `xs:int` (nillable) | opcional |  |
| 9 | `COD_TIPO_SOLICITUD` | `xs:int` (nillable) | opcional |  |
| 10 | `COD_USUARIO_APROBADOR` | `xs:int` (nillable) | opcional |  |
| 11 | `COD_USUARIO_CREA` | `xs:int` (nillable) | opcional |  |
| 12 | `CONCEPTO_APLICA` | `xs:string` | opcional |  |
| 13 | `DOCUMENTO_CAUSANTE` | `xs:string` | opcional |  |
| 14 | `DOCUMENTO_ID` | `xs:int` (nillable) | opcional |  |
| 15 | `ESTADO_DOCUMENTO` | `xs:string` | opcional |  |
| 16 | `ESTADO_SOLICITUD` | `xs:string` | opcional |  |
| 17 | `FACTURA_PAGO` | `xs:string` | opcional |  |
| 18 | `FECHA_FINAL_SOLICITUD` | `xs:dateTime` (nillable) | opcional |  |
| 19 | `FECHA_INICIAL_DOCUMENTO` | `xs:dateTime` (nillable) | opcional |  |
| 20 | `FECHA_INICIO_SOLICITUD` | `xs:dateTime` (nillable) | opcional |  |
| 21 | `IDENTIFICACION_BENEFICIARIO` | `xs:string` | opcional |  |
| 22 | `INTERES` | `xs:decimal` (nillable) | opcional |  |
| 23 | `MODALIDAD_SOLICITUD` | `xs:string` | opcional |  |
| 24 | `MONTO` | `xs:decimal` (nillable) | opcional |  |
| 25 | `MONTO_TOTAL_PAGO` | `xs:decimal` (nillable) | opcional |  |
| 26 | `NRO_CUENTA` | `xs:int` (nillable) | opcional |  |
| 27 | `NRO_SOLICITUD` | `xs:int` (nillable) | opcional |  |
| 28 | `OBSERVACION_SOLICITUD` | `xs:string` | opcional |  |
| 29 | `RADICADO` | `xs:string` | opcional |  |
| 30 | `SOLICITANTE` | `xs:string` | opcional |  |
| 31 | `TIPO_DOCUMENTO` | `xs:string` | opcional |  |
| 32 | `TIPO_SOLICITUD` | `xs:string` | opcional |  |
| 33 | `USUARIO_APROBADOR` | `xs:string` | opcional |  |
| 34 | `USUARIO_CREA` | `xs:string` | opcional |  |
| 35 | `VALOR_DISPONIBLE` | `xs:decimal` (nillable) | opcional |  |
| 36 | `VALOR_TOTAL_PAGADO_BENEFICIARIO` | `xs:int` (nillable) | opcional |  |

### `TipoIdentificacionBEL`

Tipo XML `TipoIdentificacionBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `idTipoId` | `xs:int` (nillable) | opcional |  |

### `TipoViviendaBEL`

Tipo XML `TipoViviendaBEL` — namespace `http://schemas.datacontract.org/2004/07/Kriterion.BEL`

| # | Elemento XML | Tipo | Ocurrencia | Notas |
|---|---|---|---|---|
| 1 | `tipoViviendaId` | `xs:int` (nillable) | opcional |  |


## 4. Escenarios mínimos recomendados

| # | Escenario | Respuesta |
|---|---|---|
| 1 | Afiliado con cuentas, saldos, embargos y dependientes | `BUA` completo |
| 2 | No encontrado con `tipo_Id=1`, encontrado con `tipo_Id=4` | `nil` para 1; `BUA` para 4 (prueba del reintento 1→4→5) |
| 3 | No encontrado | `nil` en los tres tipos |
| 4 | Fault / timeout | Para comparar el comportamiento de `ws.01` y `ws.02` |
