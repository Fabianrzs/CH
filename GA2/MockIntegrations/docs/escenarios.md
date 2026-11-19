# Escenarios de éxito y error

## Cómo se elige el escenario (determinista)

Para cada llamada, el mock decide el escenario con esta prioridad. Lo devuelve en `X-Mock-Scenario-Applied` y `X-Mock-Scenario-Source` y lo guarda en `/_mock/journal`.

1. Header **`X-Mock-Scenario`** (sólo pruebas manuales: GA2 no lo envía).
2. **Override en caliente:** `PUT /_mock/scenarios/{proveedor}/{operación}` con `{"scenario":"..."}` (`*` = todo el proveedor). Se quita con `DELETE` (o `POST /_mock/reset`).
3. **Reglas por dato de entrada** (`MockIntegrations:Scenarios:Rules`): campo del request (nombre local XML, propiedad JSON, ruta o query) igual a un valor o con un prefijo.
4. **Escenario por defecto configurado** (`MockIntegrations:Scenarios:Defaults:{Proveedor}:{Op}`).
5. `default`.

Un escenario que no existe para la operación **no** devuelve una respuesta aproximada: responde SOAP Fault (`s:Client`) o HTTP 400, con el motivo.

```bash
curl -X PUT localhost:5080/_mock/scenarios/Seven/CrearCDP -H 'Content-Type: application/json' -d '{"scenario":"error-negocio"}'
curl localhost:5080/_mock/scenarios | jq '.[] | select(.provider=="Seven" and .operation=="CrearCDP")'
curl -X POST localhost:5080/_mock/reset
```

## Escenarios integrados (todas las operaciones)

| Escenario | SOAP | REST | Efecto típico en GA2 |
|---|---|---|---|
| `default` | Respuesta de éxito | Respuesta de éxito | Flujo normal |
| `fault` | HTTP 500 + `s:Fault` | HTTP 500 texto | `FaultException` → HTTP 409 (o mensaje controlado si GA2 captura) / `IsSuccess=false` |
| `http-500` | HTTP 500 HTML sin sobre | HTTP 500 HTML | `CommunicationException` / `IsSuccess=false` |
| `slow` | Éxito tras `SlowScenarioDelayMs` | Igual | Timeouts de GA2: SOAP sin límite (`TimeSpan.MaxValue`), TusDatos/Dodo 15 s, pasarela 100 s |

## Escenarios de negocio por proveedor

| Proveedor | Escenarios | Detalle |
|---|---|---|
| Seven | `error-negocio`, `tercero-existente`, `sin-identificadores` | [E01](providers/E01-seven.md) |
| WorkManager | `fallido`, `resultado-nulo`, `sin-formulario` | [E02](providers/E02-workmanager.md) |
| BUA | `no-encontrado`, `encontrado-tipo-4` | [E03](providers/E03-bua.md) |
| Vigía | `reportado` | [E04](providers/E04-vigia.md) |
| Fénix | `parcial`, `sin-datos`, `dato-invalido` | [E05](providers/E05-fenix.md) |
| Haberes | `sin-reporte`, `vacio` | [E06](providers/E06-haberes.md) |
| TransUnion | `auth-fallida`, `muerte-titular`, `fuera-de-cola`, `sin-otp`, `pin-incorrecto`, `pendiente`, `no-viable`, `sin-applicationdata` | [E07](providers/E07-transunion.md) |
| TusDatos | `pep`, `historial`, `riesgo`, `no-encontrado`, `unauthorized` | [E08](providers/E08-tusdatos.md) |
| Dodo | `fallido`, `no-encontrado` | [E09](providers/E09-dodo-rest.md) |
| ktrionApi | `unauthorized`, `sin-token`, `fallido`, `sin-reporte` | [E10](providers/E10-ktrionapi.md) |
| SMTP | destinatarios rechazados (`Smtp:RejectRecipients`), servidor apagado (`Smtp:Enabled=false`) | [E11](providers/E11-smtp.md) |

## Datos de prueba incluidos (reglas de `appsettings.json`)

| Documento / dato | Proveedor | Escenario |
|---|---|---|
| `1000000404` | BUA, Fénix, Haberes (`customerIdentification`), TusDatos | No encontrado / sin datos / sin reporte / DataSet vacío |
| `1000000004` | BUA | Sólo se encuentra con `tipo_Id = 4` |
| `1000000002` | Fénix | Biometría parcial |
| `1000000666` | Vigía (`param1`), TusDatos, TransUnion (`IdNumber`) | Reportado / PEP / riesgo alto / NO VIABLE |
| `1000000777` | TransUnion | PENDIENTE |
| `1000000999` | TransUnion | MUERTE POR TITULAR |
| `1000000100` | TusDatos | Resultado del histórico |
| `1000000500` | BUA, Vigía, Fénix, TransUnion | Falla técnica (`fault`) |
| `FormCode = 999` | WorkManager `Form_Insert` | `Success=false` |
| PIN `123456` | TransUnion | OTP válido (código 300); cualquier otro: 302 |
| `rechazado@mock.test` | SMTP | 550 en `RCPT TO` |

Cualquier otro documento recibe el escenario `default`.

## Estado entre llamadas

Se mantiene sólo donde el flujo de GA2 lo requiere, en memoria (se reinicia con `POST /_mock/reset` o al reiniciar el proceso):
- consecutivos de Seven y WorkManager/Dodo/pasarela (radicados, comprobantes, CDP, facturas);
- jobs de TusDatos (pendiente → finalizado);
- transacciones de TransUnion por `ApplicationId` (escenario heredado del inicio);
- tokens emitidos por la pasarela ktrionApi;
- buzón SMTP y registro de llamadas.
