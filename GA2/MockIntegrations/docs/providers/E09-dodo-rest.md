# E9 · Dodo REST — mock REST (29 operaciones)

**Especificación:** [docs/integraciones/mocks/E09-dodo-rest.md](../../../docs/integraciones/mocks/E09-dodo-rest.md).
**Código:** [`Providers/Dodo/DodoMock.cs`](../../src/MockIntegrations.Api/Providers/Dodo/DodoMock.cs) · modelos **copiados de GA2** con [`tools/copy-ga2-models.py`](../../tools/copy-ga2-models.py) en [`Providers/Dodo/Models/`](../../src/MockIntegrations.Api/Providers/Dodo/Models/).

- **Base:** `Integrationes:Dodo:BaseUrl = http://<mock>:5080/dodo/`. Todas las rutas de GA2 cuelgan de ella: `POST /dodo/api/Forms/Form_Insert`, `GET /dodo/api/version`, etc. (lista completa en Swagger, documento `dodo`, y en la [matriz](../matriz-cobertura.md)).
- **Áreas:** Field, Lists, File, Forms (9), Notification, Offices, Token, User, Workflow (10) y `api/version`.
- **Request:** JSON camelCase sin nulos (como lo serializa `CommonIntegracionCliente`). En **Forms**, GA2 inyecta `header {user, token}`; el mock no lo valida.
- **Response:** los DTO de GA2. `Form_Insert` devuelve `barCode = "{formCode}-{consecutivo}"`, `recordId`, `fileCodes` por archivo y `workflowId` si `checkWorkflow=true`. `GET api/version` devuelve una **cadena JSON** (`"1.4.2-mock"`), como exige GA2.

| Escenario | Respuesta | Efecto en GA2 |
|---|---|---|
| `default` | `success=true` con datos de ejemplo realistas | `IsSuccess=true` |
| `fallido` | HTTP 200 con `success=false` y `message` | `IsSuccess=true`, datos con `success=false` |
| `no-encontrado` | HTTP 404 con texto | `IsSuccess=false` con el cuerpo (en `api/version`: éxito con `Data=null`) |
| `fault` / `http-500` / `slow` | HTTP 500 / retardo | `IsSuccess=false` / timeout de 15 s |

En GA2 sólo la usa `DodoController` (pass-through `api/integrations/dodo/*`). Probado de extremo a extremo: `POST api/integrations/dodo/forms/parameters`.
