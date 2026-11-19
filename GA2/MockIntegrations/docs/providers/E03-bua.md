# E3 · BUA (Base Única de Afiliados) — mock SOAP (WCF)

**Especificación:** [docs/integraciones/mocks/E03-bua-soap.md](../../../docs/integraciones/mocks/E03-bua-soap.md).
**Código:** [`Providers/Bua/BuaMock.cs`](../../src/MockIntegrations.Api/Providers/Bua/BuaMock.cs).

## Endpoints

| Cliente GA2 | Ruta en el mock | ¿Redirigible por configuración? |
|---|---|---|
| ws.02 — `Connected Services/ServiceReference1` (API BUA, consulta de cliente) | `POST /Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc` | **Sí**: `SvcUtils:BuaUrlServiceApi` |
| ws.01 — `ReferenciasSvc/Bua/ConsumoBua.cs` (Cartera, Verificación de derechos, Cuentas, restaurar cliente) | `POST /Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc` | **No**: URL fija `http://cpvmp-09x/Kriterion.ws.01/...` en `Reference.cs` (ver [guía de configuración §6](../guia-configuracion-ga2.md#6-integraciones-que-no-se-pueden-redirigir-por-configuración)) |

Operación única `ObtenerBUA` (`SOAPAction: "http://tempuri.org/IBUA_API/ObtenerBUA"`), request `BUAConsultaBE {identificacion, tipo_Id}`.

## Comportamiento

- Ficha completa (47 campos, cuentas con conceptos/saldos/movimientos, aportes, dependientes, solicitudes, embargos, bloqueos) con datos realistas; `identificacion` = la recibida.
- **Un único fixture sirve a ambos proxies**: `valor` y `tasaAporte` van como enteros (ws.01 los tipa `int?`/`decimal`, ws.02 `string`/`int`) y los campos exclusivos de ws.02 los ignora ws.01. Verificado con los dos proxies de GA2.

## Escenarios y reglas por documento

| Escenario | Respuesta | Regla de configuración |
|---|---|---|
| `default` | Afiliado JUAN CARLOS PEREZ GOMEZ | — |
| `no-encontrado` | `ObtenerBUAResult` nulo (GA2 prueba el siguiente `tipo_Id`) | `identificacion = 1000000404` |
| `encontrado-tipo-4` | Nulo salvo con `tipo_Id = 4`: ejercita el reintento 1 → 4 de GA2 | `identificacion = 1000000004` |
| `fault` | SOAP Fault | `identificacion = 1000000500` |

## Limitaciones

- ws.01 sólo se alcanza resolviendo `cpvmp-09x` hacia el mock (DNS/hosts) en el puerto 80.
- El flujo ws.02 de la API BUA depende del interruptor `Bua`, que la API BUA consulta por HTTP a la API GA2 (`EstadoIntegracionClient`, con un defecto de comparación documentado en [pendientes](../pendientes.md)).
