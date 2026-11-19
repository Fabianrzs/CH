"""Genera docs/matriz-cobertura.md a partir del catálogo del mock (GET /_mock/catalog).

Uso (con el mock en ejecución):
  python3 MockIntegrations/tools/generate-coverage.py http://localhost:5080
La relación operación → pruebas se declara abajo (TESTS / SMOKE) y debe mantenerse junto con las pruebas.
"""
import json
import pathlib
import sys
import urllib.request

ROOT = pathlib.Path(__file__).resolve().parents[1]
base = sys.argv[1] if len(sys.argv) > 1 else "http://localhost:5080"
catalog = json.load(urllib.request.urlopen(f"{base.rstrip('/')}/_mock/catalog"))

COMMON = ["CatalogAndOpenApiTests (registro, SOAPAction/ruta, OpenAPI)"]
TESTS = {
    "Seven": lambda op: ["SevenCompatibilityTests.Each_operation_is_answered_and_deserialized_by_the_GA2_proxy"]
    + (["SevenCompatibilityTests.Business_error_scenario_returns_RETORNO_1_with_TXTERROR"] if any(s["name"] == "error-negocio" for s in op["scenarios"]) else [])
    + {
        "InsertarGnTerce": ["Payment_flow…", "Existing_third_party_keeps_the_GN_TERCE_root"],
        "SyncPoPvdor": ["Payment_flow…"],
        "CrearCDP": ["Payment_flow…", "Technical_errors_surface_as_WCF_exceptions"],
        "InsertarFacturaP": ["Payment_flow…", "Invoice_without_identifiers…"],
        "InsertarMovContable": ["Payment_flow…", "The_second_GA2_proxy_of_scnmcont…"],
        "AnularCnMcont": ["The_second_GA2_proxy_of_scnmcont…"],
    }.get(op["name"], []),
    "WorkManager": lambda op: (["SoapProvidersCompatibilityTests.WorkManager_operations_unused_by_GA2_answer_an_explicit_fault"]
                               if op["certainty"] == "NotImplemented" else ["SoapProvidersCompatibilityTests.WorkManager_wrapped_operations_return_a_successful_result"])
    + {"Form_Insert": ["Form_Insert_returns_a_radicado…", "Form_Insert_failure_and_null_result_scenarios"],
       "Form_GetParameters": ["Form_GetParameters_gives_GA2_what_it_needs…"]}.get(op["name"], []),
    "Bua": lambda op: ["SoapProvidersCompatibilityTests.ObtenerBUA_is_read_by_both_GA2_proxies (ws.01 y ws.02)", "ObtenerBUA_not_found_and_retry_by_document_type"],
    "Vigia": lambda op: ["SoapProvidersCompatibilityTests.F_VERIFICATERCERO_rpc_encoded_response_is_read_by_the_GA2_proxy"],
    "Fenix": lambda op: ["SoapProvidersCompatibilityTests.ValidarBiometria_dataset_is_deserialized_like_FenixBusinessLogic", "ValidarBiometria_invalid_value…"]
    if op["name"] == "ValidarBiometria" else ["CatalogAndOpenApiTests (SOAPAction registrada)"],
    "Haberes": lambda op: {"GenerarReporte": ["GenerarReporte_and_V2_return_a_real_pdf"], "GenerarReporteV2": ["GenerarReporte_and_V2_return_a_real_pdf"],
                           "ReporteHaberes": ["ReporteHaberes_dataset_is_parsed_like_HaberesBusinessLogic"]}.get(op["name"], ["Unused_Haberes_operations_answer_an_explicit_fault (CustomerAccount)"]),
    "TransUnion": lambda op: ["TransUnionCompatibilityTests (ValidacionesTU real: IDVision/OTP, AQM, historial, prospección, escenarios, fault)"]
    if op["name"] == "ExecuteXMLString" else ["TransUnionCompatibilityTests.Unused_operations_answer_an_explicit_fault (AuthenticateXMLString)"],
    "TusDatos": lambda op: ["RestCompatibilityTests.TusDatos_* (DTO y opciones JSON de GA2; 200/404/401)", "ScenarioSelectionTests"],
    "Dodo": lambda op: ["RestCompatibilityTests.Dodo_operation_accepts_the_GA2_request_and_returns_the_GA2_response"]
    + (["Dodo_form_insert_returns_a_barcode…"] if op["name"] == "Form_Insert" else [])
    + (["Dodo_version_is_a_json_string"] if op["name"] == "Version" else []),
    "KtrionApi": lambda op: ["RestCompatibilityTests.Gateway_unused_routes_answer_501"] if op["certainty"] == "NotImplemented"
    else ["RestCompatibilityTests.Gateway_* (Newtonsoft + Response<T> + proxies WCF de GA2)"],
    "Smtp": lambda op: ["MessagingCompatibilityTests.Smtp_* (System.Net.Mail.SmtpClient)"] if op["name"] == "EnviarNotificacion" else ["— (host fijo en código: ver pendientes)"],
    "ServiceBus": lambda op: ["MessagingCompatibilityTests.Service_bus_* (eventos de GA2)", "Verificación manual con emulador (guía de configuración §7)"],
}
SMOKE = {("TusDatos", "ConsultarCliente"), ("TusDatos", "ConsultarVigia"), ("Dodo", "Form_GetParameters"),
         ("Vigia", "F_VERIFICATERCERO"), ("Fenix", "ValidarBiometria"), ("Seven", "InsertarGnTerce")}

usage_es = {"Business": "Negocio", "PassThrough": "Pass-through", "NotUsed": "Sin uso"}
certainty_es = {"Validated": "Validado", "StructureOnly": "Estructura validada; contenido a confirmar", "NotImplemented": "Fault/501 explícito"}

lines = [
    "# Matriz de cobertura de operaciones y pruebas",
    "",
    f"> Generada con `tools/generate-coverage.py` a partir de `GET /_mock/catalog` ({len(catalog)} operaciones). No editar a mano.",
    "> **Pruebas automatizadas:** `tests/MockIntegrations.Tests` (proxies WCF, DTO y deserializadores reales de GA2 enlazados desde `src/`).",
    "> **Humo GA2:** la operación se ejecutó a través de la API GA2 real (perfil `Mocks`) con `docker/ga2-smoke/smoke.py`.",
    "",
    "| Proveedor | Operación | Protocolo | Ruta / SOAPAction | Uso en GA2 | Contrato | Escenarios de negocio | Pruebas automatizadas | Humo GA2 |",
    "|---|---|---|---|---|---|---|---|---|",
]
summary = {}
for op in sorted(catalog, key=lambda o: (o["provider"], o["usage"] != "Business", o["name"])):
    tests = TESTS[op["provider"]](op)
    target = f"`{op['soapAction']}`" if op.get("soapAction") else f"`{op['method']} {op['route']}`"
    scenarios = " ".join(f"`{s['name']}`" for s in op["scenarios"]) or "—"
    smoke = "✔" if (op["provider"], op["name"]) in SMOKE else ""
    lines.append(f"| {op['provider']} | `{op['name']}` | {op['protocol']} | {target} | {usage_es[op['usage']]} | {certainty_es[op['certainty']]} | {scenarios} | {'<br>'.join(tests)} | {smoke} |")
    s = summary.setdefault(op["provider"], {"total": 0, "Business": 0, "PassThrough": 0, "NotUsed": 0, "smoke": 0})
    s["total"] += 1
    s[op["usage"]] += 1
    s["smoke"] += 1 if smoke else 0

lines[6:6] = [
    "## Resumen por proveedor",
    "",
    "| Proveedor | Operaciones | Negocio | Pass-through | Sin uso en GA2 | Ejecutadas vía GA2 |",
    "|---|---|---|---|---|---|",
    *[f"| {p} | {s['total']} | {s['Business']} | {s['PassThrough']} | {s['NotUsed']} | {s['smoke']} |" for p, s in sorted(summary.items())],
    f"| **Total** | **{sum(s['total'] for s in summary.values())}** | {sum(s['Business'] for s in summary.values())} | {sum(s['PassThrough'] for s in summary.values())} | {sum(s['NotUsed'] for s in summary.values())} | {sum(s['smoke'] for s in summary.values())} |",
    "",
    "Todas las operaciones están además cubiertas por: " + ", ".join(COMMON) + ".",
    "",
    "## Detalle por operación",
    "",
]
(ROOT / "docs" / "matriz-cobertura.md").write_text("\n".join(lines) + "\n", encoding="utf-8")
print(f"docs/matriz-cobertura.md: {len(catalog)} operaciones")
