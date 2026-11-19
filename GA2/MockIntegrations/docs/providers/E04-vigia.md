# E4 · Vigía — mock SOAP RPC/encoded

**Especificación:** [docs/integraciones/mocks/E04-vigia.md](../../../docs/integraciones/mocks/E04-vigia.md).
**Código:** [`Providers/Vigia/VigiaMock.cs`](../../src/MockIntegrations.Api/Providers/Vigia/VigiaMock.cs).

| Clave | Ruta en el mock | Operación |
|---|---|---|
| `SvcUtils:VigiaVerificarTerceroUrlSvc` (API GA2) | `POST /WebSvc/SOAP_VerificaTercero.php` | `F_VERIFICATERCERO(param1=documento, param2=tipo de verificación, param3=porcentaje)` → `Resultado {Encontrado, Listas, Metodo}` |

- **Estilo RPC/encoded** (SOAP 1.1 sección 5): `F_VERIFICATERCEROResponse` en el namespace `https://vigia.cajahonor.gov.co/WebSvc`, `return` con `xsi:type="tns:Resultado"`. Validado con el proxy WCF de GA2.
- En GA2 la URL original es `https`; con el perfil `Mocks` se usa `http://…` (el binding de GA2 elige la seguridad según el esquema).

| Escenario | Respuesta | Regla |
|---|---|---|
| `default` | `Encontrado=N`, `Listas` y `Metodo` vacíos | — |
| `reportado` | `Encontrado=S`, `Metodo=DOCUMENTO`, `Listas="OFAC, ONU"` | `param1 = 1000000666` |
| `fault` | SOAP Fault (GA2: "Error en el servicio de vigía…") | `param1 = 1000000500` |

**No confirmado con Vigía:** el dominio real de `Encontrado`, `Listas`, `Metodo` y `param2`. GA2 los devuelve al frontend sin interpretarlos.
**No simulados (sin uso en GA2):** los contratos ASMX `F_CARGATERCERO`, `F_CARGAPRODUCTOTERCERO`, `F_CARGATRANSACCION`, `F_VERIFICATERCEROV3` (`http://cpvmp-22fw:8080/WebSvc_asmx/*`).
Probado de extremo a extremo a través de GA2: `POST api/credito/verificacionTercero`.
