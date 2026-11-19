# Registro de avance

| Fecha | Fase | Resultado |
|---|---|---|
| 2026-10-09 | 1–2 · Análisis e inventario | Inventario técnico y 12 fichas de contrato validadas con los proxies de GA2 (`docs/integraciones/`) |
| 2026-10-13 | 1 · Reverificación | Puntos de riesgo contrastados de nuevo con el código (TusDatos, Fénix/Haberes, pasarela, TransUnion, Dodo, Service Bus) |
| 2026-10-13 | 3 · Diseño | Solución `MockIntegrations` (.NET 10, ASP.NET Core): un proceso con un módulo por proveedor, despachador SOAP por `SOAPAction`, fixtures embebidos y marcadores dinámicos, modelos REST copiados de GA2, escenarios deterministas, registro de llamadas, SMTP propio |
| 2026-10-13 | 4–5 · Mocks | 12 proveedores, 153 operaciones (34 de negocio, 53 pass-through, 66 sin uso con respuesta explícita); 155 fixtures generados con `tools/build-fixtures.py` |
| 2026-10-13 | 7 · OpenAPI | 14 documentos OpenAPI 3.1 (uno por proveedor, administración y "todos") y Swagger UI |
| 2026-10-13 | 8 · Pruebas | 209 pruebas automatizadas (todas pasan) con los proxies, DTO y clientes reales de GA2 enlazados desde `src/` |
| 2026-10-13 | 6 · Configuración GA2 | `appsettings.Mocks.json` (GA2, BUA), perfiles de arranque, copia a `bin` en los `.csproj`, scripts de variables de entorno |
| 2026-10-13 | 8 · Integración con GA2 | GA2 y BUA compilan con .NET 5.0.408. GA2 arranca con el perfil `Mocks` contra el emulador de Service Bus (vía proxy TLS) y una base stub; **7/7 llamadas reales GA2 → mock correctas**. Hallados los defectos D1–D3 del canal Service Bus |
| 2026-10-13 | 9–10 · Documentación y validación | Guías, documentos por proveedor, matriz de cobertura y lista de pendientes |
