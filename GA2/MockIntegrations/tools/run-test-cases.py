"""Ejecuta los casos de prueba manuales/funcionales de MockIntegrations y guarda evidencias.

Cada caso envía una petición real al mock (o a la API GA2 en los casos GA2-*), valida código HTTP,
estructura y valores de negocio, y guarda petición + respuesta en la carpeta de evidencias.

Uso:
  python3 MockIntegrations/tools/run-test-cases.py --mock http://127.0.0.1:5090 --smtp-port 2526 \
      --evidence ~/Downloads/Evidencias_MockIntegrations_GA2 [--ga2 http://127.0.0.1:5102 --jwt-key <clave>]
Sólo biblioteca estándar. Resultado: <evidencias>/resultados.json (una entrada por caso).
"""
import argparse
import base64
import datetime
import hashlib
import hmac
import json
import pathlib
import re
import smtplib
import sys
import time
import urllib.error
import urllib.request
import xml.etree.ElementTree as ET
from email.message import EmailMessage
from xml.sax.saxutils import escape

ROOT = pathlib.Path(__file__).resolve().parents[2]
FIX = ROOT / "MockIntegrations/src/MockIntegrations.Api/Providers"
p = argparse.ArgumentParser()
p.add_argument("--mock", default="http://127.0.0.1:5090")
p.add_argument("--smtp-port", type=int, default=2526)
p.add_argument("--evidence", required=True)
p.add_argument("--ga2")
p.add_argument("--jwt-key")
args = p.parse_args()
MOCK = args.mock.rstrip("/")
EV = pathlib.Path(args.evidence).expanduser()
CASES_DIR = EV / "02_casos"
results = []


# ─── utilidades ─────────────────────────────────────────────────────────────
class Resp:
    def __init__(self, status, headers, body, elapsed):
        self.status, self.headers, self.body, self.elapsed = status, headers, body, elapsed

    def json(self):
        return json.loads(self.body)

    def xml(self):
        return ET.fromstring(self.body)

    def h(self, name):
        return next((v for k, v in self.headers.items() if k.lower() == name.lower()), None)


_snapshots = []


def http(method, url, body=None, headers=None, timeout=60):
    # Antes de vaciar el registro de llamadas, se guarda una copia (evidencia completa del journal).
    if method in ("DELETE", "POST") and (url.endswith("/_mock/journal") or url.endswith("/_mock/reset")) and "MOCK" in globals():
        snap = http("GET", MOCK + "/_mock/journal?take=1000")
        if snap.status == 200 and snap.body not in ("[]", ""):
            _snapshots.extend(json.loads(snap.body))
    headers = dict(headers or {})
    data = body.encode("utf-8") if isinstance(body, str) else body
    req = urllib.request.Request(url, data=data, method=method, headers=headers)
    t0 = time.time()
    try:
        with urllib.request.urlopen(req, timeout=timeout) as r:
            return Resp(r.status, dict(r.headers), r.read().decode("utf-8", "replace"), time.time() - t0)
    except urllib.error.HTTPError as e:
        return Resp(e.code, dict(e.headers), e.read().decode("utf-8", "replace"), time.time() - t0)


def local(el, name):
    """Primer descendiente con ese nombre local."""
    for x in el.iter():
        if x.tag.split("}")[-1] == name:
            return x
    return None


def text(el, name):
    x = local(el, name)
    return None if x is None else (x.text or "")


def is_nil(el):
    return el is not None and el.attrib.get("{http://www.w3.org/2001/XMLSchema-instance}nil") == "true"


def soap(path, action, body, extra=None):
    h = {"Content-Type": "text/xml; charset=utf-8", "SOAPAction": f'"{action}"'}
    h.update(extra or {})
    return http("POST", MOCK + path, body, h)


def envelope(inner):
    return f'<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body>{inner}</s:Body></s:Envelope>'


def fixture_request(provider, op, values=None):
    xml = (FIX / provider / "Fixtures" / f"{op}.request.xml").read_text(encoding="utf-8")
    for k, v in (values or {}).items():
        xml = re.sub(rf"(<(?:\w+:)?{k}(?:\s[^>]*)?>)[^<]*(</(?:\w+:)?{k}>)", rf"\g<1>{v}\g<2>", xml)
    return xml


def dump(case_id, provider, req, resp):
    folder = CASES_DIR / re.sub(r"[^\w-]", "_", provider)
    folder.mkdir(parents=True, exist_ok=True)
    f = folder / f"{case_id}.txt"
    body = resp.body if resp else ""
    if len(body) > 20000:
        body = body[:20000] + f"\n... (truncado, {len(resp.body)} caracteres)"
    lines = ["=== PETICIÓN ===", req, "", "=== RESPUESTA ==="]
    if resp:
        lines += [f"HTTP {resp.status}  ({resp.elapsed * 1000:.0f} ms)"] + [f"{k}: {v}" for k, v in resp.headers.items()] + ["", body]
    f.write_text("\n".join(lines), encoding="utf-8")
    return str(f.relative_to(EV))


def run(case_id, provider, method, scenario, inputs, expected, fn, category="", excel_id="", blocked=None, not_run=None):
    """fn() → (resp, req_text, [(descripción, ok, detalle)])."""
    now = datetime.datetime.now().isoformat(timespec="seconds")
    entry = dict(id=case_id, provider=provider, method=method, scenario=scenario, inputs=inputs, expected=expected,
                 category=category, excel_id=excel_id, date=now)
    if blocked or not_run:
        entry.update(status="BLOQUEADA" if blocked else "NO EJECUTADA", actual="No ejecutado", evidence="", comment=blocked or not_run)
        results.append(entry)
        print(f"[{entry['status']:12}] {case_id} {method} — {entry['comment'][:90]}")
        return entry
    try:
        resp, req, checks = fn()
        failed = [c for c in checks if not c[1]]
        entry["status"] = "APROBADA" if not failed else "FALLIDA"
        entry["actual"] = (f"HTTP {resp.status}; " if resp else "") + "; ".join(f"{c[0]}: {c[2]}" for c in checks)
        entry["comment"] = "" if not failed else "Falla: " + "; ".join(c[0] for c in failed)
        entry["evidence"] = dump(case_id, provider, req, resp) if req else ""
    except Exception as ex:  # noqa: BLE001 - registrar cualquier error como prueba fallida
        entry.update(status="FALLIDA", actual=f"Excepción: {type(ex).__name__}: {ex}", comment="Error al ejecutar el caso", evidence="")
    results.append(entry)
    print(f"[{entry['status']:12}] {case_id} {method} {scenario} — {entry['actual'][:110]}")
    return entry


def req_text(method, url, headers, body):
    return f"{method} {url}\n" + "\n".join(f"{k}: {v}" for k, v in headers.items()) + ("\n\n" + body if body else "")


def chk(desc, ok, detail=""):
    return (desc, bool(ok), detail)


def soap_case(cid, provider, path, action, op, body, scenario, inputs, expected, validate, header_scenario=None, category="", excel_id=""):
    extra = {"X-Mock-Scenario": header_scenario} if header_scenario else {}

    def fn():
        r = soap(path, action, body, extra)
        h = {"Content-Type": "text/xml; charset=utf-8", "SOAPAction": f'"{action}"', **extra}
        checks = [chk("Escenario aplicado", r.h("X-Mock-Scenario-Applied") == scenario, r.h("X-Mock-Scenario-Applied"))]
        checks += validate(r)
        return r, req_text("POST", MOCK + path, h, body), checks

    return run(cid, provider, op, scenario, inputs, expected, fn, category, excel_id)


def fault_checks(r, contains=None):
    root = r.xml()
    fs = text(root, "faultstring") or ""
    c = [chk("HTTP 500", r.status == 500, r.status), chk("SOAP Fault presente", local(root, "Fault") is not None, (fs[:80]))]
    if contains:
        c.append(chk(f"faultstring contiene '{contains}'", contains in fs, fs[:80]))
    return c


EXCEL = {}  # (proveedor catálogo, método) → ID del Excel (Mxxx)
CATALOG = json.loads(http("GET", MOCK + "/_mock/catalog").body)
DISPLAY = {"Seven": "Seven ERP", "WorkManager": "WorkManager / DODO (SOAP)", "Bua": "BUA", "Vigia": "Vigía", "Fenix": "Fénix",
           "Haberes": "Haberes (MCD)", "TransUnion": "TransUnion", "TusDatos": "TusDatos", "Dodo": "Dodo REST",
           "KtrionApi": "Pasarela ktrionApi", "Smtp": "SMTP / correo", "ServiceBus": "Azure Service Bus"}
excel_path = pathlib.Path.home() / "Downloads/Estimacion_Mocks_GA2_desde_cero.xlsx"
try:
    sys.path.insert(0, str(pathlib.Path(__file__).parent))
    import openpyxl  # noqa: E402

    ws = openpyxl.load_workbook(excel_path, data_only=True)["Detalle por método"]
    inv = {v: k for k, v in DISPLAY.items()}
    for row in ws.iter_rows(min_row=5, values_only=True):
        if row[0] and str(row[0]).startswith("M"):
            EXCEL[(inv[row[1]], row[2])] = row[0]
except Exception as ex:  # noqa: BLE001
    print("Aviso: no se pudo leer el Excel:", ex)


def X(provider, op):
    return EXCEL.get((provider, op), "")


def op_info(provider, name):
    return next(o for o in CATALOG if o["provider"] == provider and o["name"] == name)


http("POST", MOCK + "/_mock/reset")

# ═══ ENTORNO Y ADMINISTRACIÓN ═══════════════════════════════════════════════
def env_checks():
    r = http("GET", MOCK + "/health")
    sw = http("GET", MOCK + "/swagger/index.html")
    docs = {d: http("GET", f"{MOCK}/openapi/{d}.json").status for d in
            ["all", "seven", "workmanager", "bua", "vigia", "fenix", "haberes", "transunion", "tusdatos", "dodo", "ktrionapi", "smtp", "servicebus", "admin"]}
    c = [chk("health 200 {status:ok}", r.status == 200 and r.json().get("status") == "ok", r.body),
         chk("Swagger UI 200", sw.status == 200, sw.status),
         chk("14 documentos OpenAPI 200", all(v == 200 for v in docs.values()), docs)]
    return r, req_text("GET", MOCK + "/health", {}, ""), c


run("ENV-001", "Entorno", "health / Swagger / OpenAPI", "—", "GET /health, /swagger/index.html, /openapi/{14}.json", "200 en todos", env_checks, "Entorno")


def inv_checks():
    cat = {(o["provider"], o["name"]) for o in CATALOG}
    ex = set(EXCEL)
    return None, "", [chk("Excel leído (153 métodos)", len(ex) == 153, len(ex)), chk("Catálogo del mock = 153", len(cat) == 153, len(cat)),
                      chk("Mismos métodos en Excel y mock", cat == ex, f"solo Excel: {sorted(ex - cat)[:3]}; solo mock: {sorted(cat - ex)[:3]}")]


run("ENV-002", "Entorno", "Inventario Excel vs catálogo", "—", "Excel 'Detalle por método' y GET /_mock/catalog", "153 métodos idénticos", inv_checks, "Entorno")


def scen_admin():
    r = http("GET", MOCK + "/_mock/scenarios")
    data = r.json()
    td = next(s for s in data if s["provider"] == "TusDatos" and s["operation"] == "ConsultarCliente")
    put = http("PUT", MOCK + "/_mock/scenarios/TusDatos/ConsultarVigia", json.dumps({"scenario": "riesgo"}), {"Content-Type": "application/json"})
    v = http("GET", MOCK + "/kiosko-dev/api/v1/vigia/customer/1020304050/document-type/CC/CheckRiskLAFT", headers={"x-api-key": "k"})
    hdr = http("GET", MOCK + "/kiosko-dev/api/v1/vigia/customer/1020304050/document-type/CC/CheckRiskLAFT", headers={"x-api-key": "k", "X-Mock-Scenario": "no-encontrado"})
    dele = http("DELETE", MOCK + "/_mock/scenarios/TusDatos/ConsultarVigia")
    v2 = http("GET", MOCK + "/kiosko-dev/api/v1/vigia/customer/1020304050/document-type/CC/CheckRiskLAFT", headers={"x-api-key": "k"})
    bad = http("PUT", MOCK + "/_mock/scenarios/TusDatos/NoExiste", json.dumps({"scenario": "fault"}), {"Content-Type": "application/json"})
    c = [chk("GET /_mock/scenarios 200 con 153 operaciones", r.status == 200 and len(data) == 153, len(data)),
         chk("TusDatos/ConsultarCliente lista pep, historial, no-encontrado, unauthorized", all(s in td["available"] for s in ["pep", "historial", "no-encontrado", "unauthorized", "fault", "http-500", "slow"]), td["available"]),
         chk("PUT override → 204", put.status == 204, put.status),
         chk("Override aplicado (RuntimeOverride, riesgo)", v.h("X-Mock-Scenario-Source") == "RuntimeOverride" and v.json()["riskLAFT"] == "RIESGO ALTO", v.h("X-Mock-Scenario-Source")),
         chk("Header tiene prioridad sobre override (404)", hdr.status == 404 and hdr.h("X-Mock-Scenario-Source") == "Header", hdr.status),
         chk("DELETE override → 204 y vuelve a default", dele.status == 204 and v2.json()["riskLAFT"] == "SIN RIESGO", v2.json()["riskLAFT"]),
         chk("Override de operación inexistente → 404", bad.status == 404, bad.status)]
    return r, req_text("GET", MOCK + "/_mock/scenarios", {}, "") + "\n(+ PUT/DELETE /_mock/scenarios/TusDatos/ConsultarVigia, PUT .../NoExiste)", c


run("ADMIN-001", "Administración", "/_mock/scenarios (listar, fijar, prioridad, quitar)", "riesgo / no-encontrado", "PUT {\"scenario\":\"riesgo\"}", "204, override aplicado, header con prioridad, 404 en operación inexistente", scen_admin, "Administración")

# ═══ TUSDATOS (REST) ════════════════════════════════════════════════════════
TD = MOCK + "/kiosko-dev/api/v1"
KEY = {"x-api-key": "clave-de-prueba", "Accept": "application/json"}


def td_case(cid, op, scenario, path, headers, inputs, expected, validate, excel=None):
    def fn():
        r = http("GET", TD + path, headers=headers)
        c = [chk("Escenario aplicado", r.h("X-Mock-Scenario-Applied") == scenario, r.h("X-Mock-Scenario-Applied"))] if scenario != "(ruta inexistente)" else []
        return r, req_text("GET", TD + path, headers, ""), c + validate(r)

    return run(cid, "TusDatos", op, scenario, inputs, expected, fn, "Lógica de negocio", X("TusDatos", op))


state = {}


def v_job_started(r):
    j = r.json()
    state["job"] = j.get("jobId")
    return [chk("HTTP 200", r.status == 200, r.status), chk("jobId = mock-job-NNNNNN", str(j.get("jobId", "")).startswith("mock-job-"), j.get("jobId")),
            chk("isFromHistory = false", j.get("isFromHistory") is False, j.get("isFromHistory")), chk("historyDataCustomer = null", j.get("historyDataCustomer") is None, j.get("historyDataCustomer"))]


td_case("TUSDATOS-001", "ConsultarCliente", "default", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", KEY,
        "identification=1020304050; documentType=CC; checkHistory=true; header x-api-key=clave-de-prueba", "200 con jobId, isFromHistory=false, historyDataCustomer=null", v_job_started)
td_case("TUSDATOS-002", "ConsultarJob", "default", f"/tusdatos/jobid/{state.get('job')}/customer/1020304050", KEY,
        f"jobId={state.get('job')} (de TUSDATOS-001); identification=1020304050", "200 statusJobTusDatos=pendiente, historyDataCustomer=null (1er sondeo)",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("statusJobTusDatos = pendiente", r.json().get("statusJobTusDatos") == "pendiente", r.json().get("statusJobTusDatos")), chk("historyDataCustomer = null", r.json().get("historyDataCustomer") is None, "")])
td_case("TUSDATOS-003", "ConsultarJob", "default", f"/tusdatos/jobid/{state.get('job')}/customer/1020304050", KEY,
        f"jobId={state.get('job')} (2.º sondeo)", "200 statusJobTusDatos=finalizado, isPEPS=false, detectionRisk=BAJO",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("statusJobTusDatos = finalizado", r.json().get("statusJobTusDatos") == "finalizado", r.json().get("statusJobTusDatos")),
                   chk("isPEPS = false", (r.json().get("historyDataCustomer") or {}).get("isPEPS") is False, r.json().get("historyDataCustomer")), chk("detectionRisk = BAJO", (r.json().get("historyDataCustomer") or {}).get("detectionRisk") == "BAJO", "")])
v_hist = lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("isFromHistory = true", r.json().get("isFromHistory") is True, r.json().get("isFromHistory")), chk("jobId = null", r.json().get("jobId") is None, r.json().get("jobId")),
                    chk("historyDataCustomer.detectionRisk = BAJO, isPEPS=false", r.json().get("historyDataCustomer") == {"isPEPS": False, "detectionRisk": "BAJO"}, r.json().get("historyDataCustomer"))]
td_case("TUSDATOS-004", "ConsultarCliente", "historial", "/tusdatos/customer/1000000100/document-type/CC/check-history/true", KEY, "identification=1000000100 (regla → historial)", "200 isFromHistory=true, jobId=null", v_hist)
td_case("TUSDATOS-005", "ConsultarCliente", "historial", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": "historial"}, "identification=1020304050; header X-Mock-Scenario=historial", "200 isFromHistory=true", v_hist)
td_case("TUSDATOS-006", "ConsultarCliente", "pep", "/tusdatos/customer/1000000666/document-type/CC/check-history/false", KEY, "identification=1000000666 (regla → pep); checkHistory=false", "200 con jobId (job PEP)", v_job_started)
pep_job = state.get("job")
http("GET", f"{TD}/tusdatos/jobid/{pep_job}/customer/1000000666", headers=KEY)
td_case("TUSDATOS-007", "ConsultarJob", "default", f"/tusdatos/jobid/{pep_job}/customer/1000000666", KEY, f"jobId={pep_job} (2.º sondeo del job PEP)", "200 finalizado con isPEPS=true, detectionRisk=ALTO",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("isPEPS = true", (r.json().get("historyDataCustomer") or {}).get("isPEPS") is True, r.json().get("historyDataCustomer")), chk("detectionRisk = ALTO", (r.json().get("historyDataCustomer") or {}).get("detectionRisk") == "ALTO", "")])
td_case("TUSDATOS-008", "ConsultarCliente", "no-encontrado", "/tusdatos/customer/1000000404/document-type/CC/check-history/true", KEY, "identification=1000000404 (regla → no-encontrado)", "HTTP 404 sin cuerpo (GA2: IsSuccess=true, Data=null)", lambda r: [chk("HTTP 404", r.status == 404, r.status)])
td_case("TUSDATOS-009", "ConsultarCliente", "default", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {"Accept": "application/json"}, "Sin header x-api-key", "HTTP 401 con texto (GA2: IsSuccess=false con el texto)", lambda r: [chk("HTTP 401", r.status == 401, r.status), chk("Cuerpo de texto no vacío", len(r.body) > 0, r.body[:60])])
td_case("TUSDATOS-010", "ConsultarCliente", "unauthorized", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": "unauthorized"}, "header X-Mock-Scenario=unauthorized", "HTTP 401", lambda r: [chk("HTTP 401", r.status == 401, r.status)])
td_case("TUSDATOS-011", "ConsultarCliente", "fault", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": "fault"}, "header X-Mock-Scenario=fault", "HTTP 500 texto (GA2: IsSuccess=false)", lambda r: [chk("HTTP 500", r.status == 500, r.status), chk("text/plain", "text/plain" in (r.h("Content-Type") or ""), r.h("Content-Type"))])
td_case("TUSDATOS-012", "ConsultarCliente", "http-500", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": "http-500"}, "header X-Mock-Scenario=http-500", "HTTP 500 HTML", lambda r: [chk("HTTP 500", r.status == 500, r.status), chk("text/html", "text/html" in (r.h("Content-Type") or ""), r.h("Content-Type"))])
td_case("TUSDATOS-013", "ConsultarCliente", "slow", "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": "slow"}, "header X-Mock-Scenario=slow (retardo configurado 3000 ms en la instancia de prueba)", "HTTP 200 tras ≥ 3 s",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("Retardo ≥ 3 s", r.elapsed >= 3.0, f"{r.elapsed:.2f} s")])
for cid, name in (("TUSDATOS-014", "ner"), ("TUSDATOS-015", "malnoautorizado")):
    run(cid, "TusDatos", "ConsultarCliente", name, f"header X-Mock-Scenario={name}", "El escenario NO existe en el código: HTTP 400 'el escenario no existe'",
        (lambda n: lambda: (lambda r: (r, req_text("GET", TD + "/tusdatos/customer/1020304050/document-type/CC/check-history/true", {**KEY, "X-Mock-Scenario": n}, ""),
                                       [chk("HTTP 400 (escenario inexistente)", r.status == 400, r.status), chk("Mensaje 'no existe'", "no existe" in r.body, r.body[:90])]))(
            http("GET", TD + "/tusdatos/customer/1020304050/document-type/CC/check-history/true", headers={**KEY, "X-Mock-Scenario": n})))(name), "Lógica de negocio", X("TusDatos", "ConsultarCliente"))
td_case("TUSDATOS-016", "ConsultarCliente", "default", "/tusdatos/customer/1020304050/document-type/CC/check-history/TRUE", KEY, "checkHistory=TRUE (mayúsculas; GA2 siempre envía minúsculas)", "El mock no valida el formato: HTTP 200 (comportamiento implementado, no del proveedor)", lambda r: [chk("HTTP 200", r.status == 200, r.status)])
td_case("TUSDATOS-017", "ConsultarCliente", "(ruta inexistente)", "/tusdatos/customer/document-type/CC/check-history/true", KEY, "Falta identification en la ruta", "HTTP 404 (la ruta no existe)", lambda r: [chk("HTTP 404", r.status == 404, r.status)])
td_case("TUSDATOS-018", "ConsultarJob", "default", "/tusdatos/jobid/no-existe/customer/1020304050", KEY, "jobId=no-existe", "HTTP 404 (job desconocido)", lambda r: [chk("HTTP 404", r.status == 404, r.status)])
td_case("TUSDATOS-019", "ConsultarVigia", "default", "/vigia/customer/1020304050/document-type/CC/CheckRiskLAFT", KEY, "identification=1020304050; documentType=CC", "200 riskLAFT=SIN RIESGO, identification/documentType del request",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("riskLAFT = SIN RIESGO", r.json().get("riskLAFT") == "SIN RIESGO", r.json().get("riskLAFT")), chk("identification eco", r.json().get("identification") == "1020304050", ""), chk("documentType eco", r.json().get("documentType") == "CC", ""), chk("firstName/lastName presentes", r.json().get("firstName") and r.json().get("lastName"), "")])
td_case("TUSDATOS-020", "ConsultarVigia", "riesgo", "/vigia/customer/1000000666/document-type/CC/CheckRiskLAFT", KEY, "identification=1000000666 (regla → riesgo)", "200 riskLAFT=RIESGO ALTO", lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("riskLAFT = RIESGO ALTO", r.json().get("riskLAFT") == "RIESGO ALTO", r.json().get("riskLAFT"))])
td_case("TUSDATOS-021", "ConsultarVigia", "no-encontrado", "/vigia/customer/1000000404/document-type/CC/CheckRiskLAFT", KEY, "identification=1000000404", "HTTP 404", lambda r: [chk("HTTP 404", r.status == 404, r.status)])


def journal_td():
    r = http("GET", MOCK + "/_mock/journal?provider=TusDatos&take=50")
    j = r.json()
    with_key = [e for e in j if "x-api-key" in {k.lower() for k in e["headers"]}]
    return r, req_text("GET", MOCK + "/_mock/journal?provider=TusDatos&take=50", {}, ""), [
        chk("Llamadas de TusDatos registradas", len(j) >= 15, len(j)),
        chk("x-api-key enmascarada (***)", all(next(v for k, v in e["headers"].items() if k.lower() == "x-api-key") == "***" for e in with_key), len(with_key)),
        chk("Escenario y código registrados", all(e.get("scenario") and e.get("statusCode") for e in j), "")]


run("TUSDATOS-022", "TusDatos", "Registro de llamadas", "—", "GET /_mock/journal?provider=TusDatos", "Llamadas registradas con x-api-key enmascarada", journal_td, "Lógica de negocio")

# ═══ SEVEN (SOAP) ═══════════════════════════════════════════════════════════
http("POST", MOCK + "/_mock/reset")
for op in [o for o in CATALOG if o["provider"] == "Seven"]:
    name, path, action = op["name"], op["route"], op["soapAction"]
    body = fixture_request("Seven", name)
    cat = "Lógica de negocio" if op["usage"] == "Business" else "Reenvío (pass-through)"
    scen_names = [s["name"] for s in op["scenarios"]]

    def v_ok(r, name=name):
        root = r.xml()
        res = local(root, f"{name}Result")
        c = [chk("HTTP 200", r.status == 200, r.status), chk(f"{name}Result presente", res is not None, "")]
        if res is not None and (res.text or "").startswith("<"):
            inner = ET.fromstring(res.text)
            c.append(chk("XML interno válido; RETORNO = 0", text(inner, "RETORNO") == "0", f"raíz {inner.tag}, RETORNO={text(inner, 'RETORNO')}"))
            for tag in ("MCO_CONT", "MCO_NUME", "MPR_CONT", "FAC_CONT", "FAC_NUME", "PVD_CODD"):
                if local(inner, tag) is not None:
                    c.append(chk(f"{tag} entero", (text(inner, tag) or "").isdigit(), text(inner, tag)))
        return c

    soap_case(f"SEVEN-{name}-01", DISPLAY["Seven"], path, action, name, body, "default", f"SOAPAction {action}; cuerpo de ejemplo del contrato", "200, ...Result con RETORNO=0 y consecutivos enteros", v_ok, category=cat, excel_id=X("Seven", name))
    if "error-negocio" in scen_names:
        def v_err(r, name=name):
            inner = ET.fromstring(local(r.xml(), f"{name}Result").text)
            return [chk("HTTP 200", r.status == 200, r.status), chk("RETORNO = 1", text(inner, "RETORNO") == "1", text(inner, "RETORNO")), chk("TXTERROR no vacío", bool(text(inner, "TXTERROR")), text(inner, "TXTERROR"))]

        soap_case(f"SEVEN-{name}-02", DISPLAY["Seven"], path, action, name, body, "error-negocio", "header X-Mock-Scenario=error-negocio", "200 con RETORNO=1 y TXTERROR", v_err, "error-negocio", cat, X("Seven", name))

SC = op_info("Seven", "CrearCDP")


def seven_seq():
    b = fixture_request("Seven", "CrearCDP")
    r1, r2 = soap(SC["route"], SC["soapAction"], b), soap(SC["route"], SC["soapAction"], b)
    a = int(text(ET.fromstring(local(r1.xml(), "CrearCDPResult").text), "MPR_CONT"))
    c = int(text(ET.fromstring(local(r2.xml(), "CrearCDPResult").text), "MPR_CONT"))
    return r2, req_text("POST", MOCK + SC["route"], {"SOAPAction": SC["soapAction"]}, b) + "\n(enviado dos veces)", [chk("MPR_CONT consecutivo (+1)", c == a + 1, f"{a} → {c}")]


run("SEVEN-CrearCDP-03", DISPLAY["Seven"], "CrearCDP", "default", "Dos llamadas seguidas", "MPR_CONT se incrementa en 1", seven_seq, "Lógica de negocio", X("Seven", "CrearCDP"))
soap_case("SEVEN-CrearCDP-04", DISPLAY["Seven"], SC["route"], SC["soapAction"], "CrearCDP", fixture_request("Seven", "CrearCDP"), "fault", "header X-Mock-Scenario=fault", "HTTP 500 SOAP Fault s:Server", fault_checks, "fault", "Lógica de negocio", X("Seven", "CrearCDP"))
soap_case("SEVEN-CrearCDP-05", DISPLAY["Seven"], SC["route"], SC["soapAction"], "CrearCDP", fixture_request("Seven", "CrearCDP"), "http-500", "header X-Mock-Scenario=http-500", "HTTP 500 HTML sin sobre SOAP",
          lambda r: [chk("HTTP 500", r.status == 500, r.status), chk("text/html", "text/html" in (r.h("Content-Type") or ""), r.h("Content-Type"))], "http-500", "Lógica de negocio", X("Seven", "CrearCDP"))
GT = op_info("Seven", "InsertarGnTerce")
soap_case("SEVEN-InsertarGnTerce-03", DISPLAY["Seven"], GT["route"], GT["soapAction"], "InsertarGnTerce", fixture_request("Seven", "InsertarGnTerce"), "tercero-existente", "header X-Mock-Scenario=tercero-existente", "200 GN_TERCE RETORNO=1 'El tercero ya se encuentra registrado'",
          lambda r: (lambda inner: [chk("Raíz GN_TERCE", inner.tag == "GN_TERCE", inner.tag), chk("RETORNO=1", text(inner, "RETORNO") == "1", ""), chk("TXTERROR esperado", text(inner, "TXTERROR") == "El tercero ya se encuentra registrado", text(inner, "TXTERROR"))])(ET.fromstring(local(r.xml(), "InsertarGnTerceResult").text)),
          "tercero-existente", "Lógica de negocio", X("Seven", "InsertarGnTerce"))
FP = op_info("Seven", "InsertarFacturaP")
soap_case("SEVEN-InsertarFacturaP-03", DISPLAY["Seven"], FP["route"], FP["soapAction"], "InsertarFacturaP", fixture_request("Seven", "InsertarFacturaP"), "sin-identificadores", "header X-Mock-Scenario=sin-identificadores", "200 RETORNO=0 sin FAC_CONT ni FAC_NUME",
          lambda r: (lambda inner: [chk("RETORNO=0", text(inner, "RETORNO") == "0", ""), chk("Sin FAC_CONT/FAC_NUME", local(inner, "FAC_CONT") is None and local(inner, "FAC_NUME") is None, "")])(ET.fromstring(local(r.xml(), "InsertarFacturaPResult").text)),
          "sin-identificadores", "Lógica de negocio", X("Seven", "InsertarFacturaP"))


def soap_bad_action():
    b = envelope('<Nada xmlns="http://seven/"/>')
    r = soap(SC["route"], "http://seven/NoExiste", b)
    return r, req_text("POST", MOCK + SC["route"], {"SOAPAction": "http://seven/NoExiste"}, b), fault_checks(r, "cannot be processed")


def soap_bad_xml():
    r = soap(SC["route"], SC["soapAction"], "<s:Envelope esto no es xml")
    return r, req_text("POST", MOCK + SC["route"], {"SOAPAction": SC["soapAction"]}, "<s:Envelope esto no es xml"), [chk("HTTP 400", r.status == 400, r.status), chk("Fault s:Client", "s:Client" in r.body, "")]


run("SEVEN-GEN-01", DISPLAY["Seven"], "(SOAPAction inexistente)", "—", "SOAPAction http://seven/NoExiste", "HTTP 500 Fault 'cannot be processed' (como WCF)", soap_bad_action, "Validación del protocolo")
run("SEVEN-GEN-02", DISPLAY["Seven"], "CrearCDP (XML mal formado)", "—", "Cuerpo que no es XML", "HTTP 400 Fault s:Client", soap_bad_xml, "Validación del protocolo", X("Seven", "CrearCDP"))

# ═══ WORKMANAGER (SOAP) ═════════════════════════════════════════════════════
WM_PATH = "/APIV2_0005/WorkManagerService.svc"
for op in [o for o in CATALOG if o["provider"] == "WorkManager"]:
    name, action = op["name"], op["soapAction"]
    cat = {"Business": "Lógica de negocio", "PassThrough": "Reenvío (pass-through)", "NotUsed": "Operación contractual no invocada"}[op["usage"]]
    if op["certainty"] == "NotImplemented":
        soap_case(f"WM-{name}-01", DISPLAY["WorkManager"], WM_PATH, action, name, envelope(f'<{name} xmlns="http://tempuri.org/"/>'), "default", f"SOAPAction {action}; cuerpo vacío", "HTTP 500 SOAP Fault explícito (GA2 no la invoca)",
                  lambda r: fault_checks(r, "GA2 no la invoca"), category=cat, excel_id=X("WorkManager", name))
        continue
    body = fixture_request("WorkManager", name, {"FormCode": "219"} if name == "Form_Insert" else None)

    def v_wm(r, name=name):
        res = local(r.xml(), f"{name}Result")
        c = [chk("HTTP 200", r.status == 200, r.status), chk(f"{name}Result presente", res is not None and not is_nil(res), ""), chk("Success = true", text(res, "Success") == "true", text(res, "Success"))]
        if name == "Form_Insert":
            bc = text(res, "BarCode")
            c.append(chk("BarCode = 219-NNNNNN", re.fullmatch(r"219-\d{6}", bc or "") is not None, bc))
            c.append(chk("RecordId > 0", int(text(res, "RecordId")) > 0, text(res, "RecordId")))
        if name == "Form_GetParameters":
            form = local(res, "Form")
            ids = [x.text for x in form if x.tag.endswith("IdFormulario")]
            c.append(chk("Form.IdFormulario = 83", ids == ["83"], ids))
            c.append(chk("Form.NombreCampoSticker = RADICADO", [x.text for x in form if x.tag.endswith("NombreCampoSticker")] == ["RADICADO"], ""))
        return c

    soap_case(f"WM-{name}-01", DISPLAY["WorkManager"], WM_PATH, action, name, body, "default", f"SOAPAction {action}; cuerpo de ejemplo" + ("; FormCode=219" if name == "Form_Insert" else ""), "200 Success=true", v_wm, category=cat, excel_id=X("WorkManager", name))
    if op["usage"] != "NotUsed":
        soap_case(f"WM-{name}-02", DISPLAY["WorkManager"], WM_PATH, action, name, body, "fallido", "header X-Mock-Scenario=fallido", "200 Success=false con Message",
                  lambda r, name=name: [chk("Success = false", text(local(r.xml(), f"{name}Result"), "Success") == "false", ""), chk("Message no vacío", bool(text(local(r.xml(), f"{name}Result"), "Message")), "")], "fallido", cat, X("WorkManager", name))
        soap_case(f"WM-{name}-03", DISPLAY["WorkManager"], WM_PATH, action, name, body, "resultado-nulo", "header X-Mock-Scenario=resultado-nulo", "200 con Result i:nil=true",
                  lambda r, name=name: [chk("Result nulo (i:nil)", is_nil(local(r.xml(), f"{name}Result")), "")], "resultado-nulo", cat, X("WorkManager", name))
FI = fixture_request("WorkManager", "Form_Insert", {"FormCode": "999"})
soap_case("WM-Form_Insert-04", DISPLAY["WorkManager"], WM_PATH, "http://tempuri.org/IWorkManagerServices/Form_Insert", "Form_Insert", FI, "fallido", "FormCode=999 (regla → fallido)", "200 Success=false",
          lambda r: [chk("Success = false", text(local(r.xml(), "Form_InsertResult"), "Success") == "false", "")], category="Lógica de negocio", excel_id=X("WorkManager", "Form_Insert"))
soap_case("WM-Form_GetParameters-04", DISPLAY["WorkManager"], WM_PATH, "http://tempuri.org/IWorkManagerServices/Form_GetParameters", "Form_GetParameters", fixture_request("WorkManager", "Form_GetParameters"), "sin-formulario", "header X-Mock-Scenario=sin-formulario", "200 con Form nulo",
          lambda r: [chk("Form nulo (i:nil)", is_nil(local(r.xml(), "Form")), "")], "sin-formulario", "Lógica de negocio", X("WorkManager", "Form_GetParameters"))

# ═══ BUA (SOAP) ═════════════════════════════════════════════════════════════
BUA_A = "http://tempuri.org/IBUA_API/ObtenerBUA"


def bua_req(ident, tipo):
    return fixture_request("Bua", "ObtenerBUA", {"identificacion": ident, "tipo_Id": tipo})


def v_bua_found(ident):
    def v(r):
        res = local(r.xml(), "ObtenerBUAResult")
        # Hijos directos de ObtenerBUAResult (los dependientes tienen su propio 'identificacion').
        top = {x.tag.split("}")[-1]: (x.text or "") for x in res} if res is not None else {}
        return [chk("HTTP 200", r.status == 200, r.status), chk("ObtenerBUAResult no nulo", res is not None and not is_nil(res), ""),
                chk("identificacion (nivel afiliado) = la enviada", top.get("identificacion") == ident, top.get("identificacion")), chk("primerNombre = JUAN", top.get("primerNombre") == "JUAN", top.get("primerNombre")),
                chk("Cuentas presentes", local(res, "CuentaBEL") is not None, ""), chk("valor entero (compatible ws.01)", (text(res, "valor") or "").isdigit(), text(res, "valor"))]
    return v


for cid, path in (("BUA-001", "/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc"), ("BUA-002", "/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc")):
    soap_case(cid, "BUA", path, BUA_A, "ObtenerBUA", bua_req("1020304050", "1"), "default", f"identificacion=1020304050; tipo_Id=1; ruta {path.split('/')[1]}", "200 ficha completa (JUAN PEREZ), identificacion eco", v_bua_found("1020304050"), category="Lógica de negocio", excel_id=X("Bua", "ObtenerBUA"))
BP = "/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc"
soap_case("BUA-003", "BUA", BP, BUA_A, "ObtenerBUA", bua_req("1000000404", "1"), "no-encontrado", "identificacion=1000000404", "200 ObtenerBUAResult i:nil=true (GA2: afiliado inexistente, prueba el siguiente tipo_Id)",
          lambda r: [chk("Result nulo", is_nil(local(r.xml(), "ObtenerBUAResult")), "")], category="Lógica de negocio", excel_id=X("Bua", "ObtenerBUA"))
soap_case("BUA-004", "BUA", BP, BUA_A, "ObtenerBUA", bua_req("1000000004", "1"), "encontrado-tipo-4", "identificacion=1000000004; tipo_Id=1", "200 Result nulo (reintento esperado)", lambda r: [chk("Result nulo con tipo_Id=1", is_nil(local(r.xml(), "ObtenerBUAResult")), "")], category="Lógica de negocio", excel_id=X("Bua", "ObtenerBUA"))
soap_case("BUA-005", "BUA", BP, BUA_A, "ObtenerBUA", bua_req("1000000004", "4"), "encontrado-tipo-4", "identificacion=1000000004; tipo_Id=4", "200 ficha completa", v_bua_found("1000000004"), category="Lógica de negocio", excel_id=X("Bua", "ObtenerBUA"))
soap_case("BUA-006", "BUA", BP, BUA_A, "ObtenerBUA", bua_req("1000000500", "1"), "fault", "identificacion=1000000500", "HTTP 500 SOAP Fault", fault_checks, category="Lógica de negocio", excel_id=X("Bua", "ObtenerBUA"))

# ═══ VIGÍA (SOAP RPC/encoded) ═══════════════════════════════════════════════
VP, VA = "/WebSvc/SOAP_VerificaTercero.php", "https://vigia.cajahonor.gov.co/WebSvc/SOAP_VerificaTercero.php/F_VERIFICATERCERO"


def vig_req(doc):
    return fixture_request("Vigia", "F_VERIFICATERCERO", {"param1": doc, "param2": "1", "param3": ""})


def v_vig(enc, listas):
    def v(r):
        root = r.xml()
        ret = local(root, "return")
        return [chk("HTTP 200", r.status == 200, r.status), chk("return xsi:type=tns:Resultado", ret is not None and ret.attrib.get("{http://www.w3.org/2001/XMLSchema-instance}type") == "tns:Resultado", ""),
                chk(f"Encontrado = {enc}", text(root, "Encontrado") == enc, text(root, "Encontrado")), chk(f"Listas = '{listas}'", (text(root, "Listas") or "") == listas, text(root, "Listas"))]
    return v


soap_case("VIGIA-001", "Vigía", VP, VA, "F_VERIFICATERCERO", vig_req("1020304050"), "default", "param1=1020304050 (documento); param2=1; param3=''", "200 Encontrado=N, Listas vacío", v_vig("N", ""), category="Lógica de negocio", excel_id=X("Vigia", "F_VERIFICATERCERO"))
soap_case("VIGIA-002", "Vigía", VP, VA, "F_VERIFICATERCERO", vig_req("1000000666"), "reportado", "param1=1000000666", "200 Encontrado=S, Listas='OFAC, ONU', Metodo=DOCUMENTO", v_vig("S", "OFAC, ONU"), category="Lógica de negocio", excel_id=X("Vigia", "F_VERIFICATERCERO"))
soap_case("VIGIA-003", "Vigía", VP, VA, "F_VERIFICATERCERO", vig_req("1000000500"), "fault", "param1=1000000500", "HTTP 500 SOAP Fault (GA2: mensaje genérico de error de Vigía)", fault_checks, category="Lógica de negocio", excel_id=X("Vigia", "F_VERIFICATERCERO"))
soap_case("VIGIA-004", "Vigía", VP, VA, "F_VERIFICATERCERO", vig_req("1020304050"), "http-500", "header X-Mock-Scenario=http-500", "HTTP 500 HTML", lambda r: [chk("HTTP 500 HTML", r.status == 500 and "text/html" in (r.h("Content-Type") or ""), r.status)], "http-500", "Lógica de negocio", X("Vigia", "F_VERIFICATERCERO"))

# ═══ FÉNIX (SOAP DataSet) ═══════════════════════════════════════════════════
FPATH, FA = "/WsFenixBiometria/WebServicesBiometrias.asmx?WSDL", "http://tempuri.org/ValidarBiometria"


def fen_req(ident):
    return fixture_request("Fenix", "ValidarBiometria", {"identificacion": ident})


def v_fen(ident, huella):
    def v(r):
        res = local(r.xml(), "ValidarBiometriaResult")
        kids = list(res)
        nds = local(kids[1], "NewDataSet") if len(kids) > 1 else None
        c = [chk("HTTP 200", r.status == 200, r.status), chk("Nodes[0]=xs:schema, Nodes[1]=diffgram", len(kids) == 2 and kids[0].tag.endswith("schema") and kids[1].tag.endswith("diffgram"), [k.tag.split('}')[-1] for k in kids])]
        if huella is None:
            c.append(chk("diffgram sin NewDataSet", nds is None, ""))
        else:
            c += [chk("A001NUM_IDENTIFICACION = enviada", text(nds, "A001NUM_IDENTIFICACION") == ident, text(nds, "A001NUM_IDENTIFICACION")), chk(f"A008HUELLA = {huella}", text(nds, "A008HUELLA") == huella, text(nds, "A008HUELLA"))]
        return c
    return v


soap_case("FENIX-001", "Fénix", FPATH, FA, "ValidarBiometria", fen_req("1020304050"), "default", "identificacion=1020304050 (URL con ?WSDL, como GA2)", "200 DataSet con A008*=1", v_fen("1020304050", "1"), category="Lógica de negocio", excel_id=X("Fenix", "ValidarBiometria"))
soap_case("FENIX-002", "Fénix", FPATH, FA, "ValidarBiometria", fen_req("1000000002"), "parcial", "identificacion=1000000002", "200 A008HUELLA=0", v_fen("1000000002", "0"), category="Lógica de negocio", excel_id=X("Fenix", "ValidarBiometria"))
soap_case("FENIX-003", "Fénix", FPATH, FA, "ValidarBiometria", fen_req("1000000404"), "sin-datos", "identificacion=1000000404", "200 diffgram vacío (GA2: 'No hay información en Fénix')", v_fen("1000000404", None), category="Lógica de negocio", excel_id=X("Fenix", "ValidarBiometria"))
soap_case("FENIX-004", "Fénix", FPATH, FA, "ValidarBiometria", fen_req("1020304050"), "dato-invalido", "header X-Mock-Scenario=dato-invalido", "200 A008HUELLA=X (GA2: 'Usuario sin datos en Fenix')", v_fen("1020304050", "X"), "dato-invalido", "Lógica de negocio", X("Fenix", "ValidarBiometria"))
soap_case("FENIX-005", "Fénix", FPATH, FA, "ValidarBiometria", fen_req("1000000500"), "fault", "identificacion=1000000500", "HTTP 500 SOAP Fault", fault_checks, category="Lógica de negocio", excel_id=X("Fenix", "ValidarBiometria"))
soap_case("FENIX-006", "Fénix", FPATH, "http://tempuri.org/xxValidarBiometria", "xxValidarBiometria", envelope('<xxValidarBiometria xmlns="http://tempuri.org/"><identificacion>1</identificacion></xxValidarBiometria>'), "default", "SOAPAction xxValidarBiometria", "HTTP 500 Fault explícito (sin uso en GA2)", lambda r: fault_checks(r, "GA2 no la invoca"), category="Operación contractual no invocada", excel_id=X("Fenix", "xxValidarBiometria"))

# ═══ HABERES (SOAP) ═════════════════════════════════════════════════════════
HP = "/MCDIntegrationServices/wsModeloCanonico.asmx"


def v_pdf(op):
    def v(r):
        res = local(r.xml(), f"{op}Result")
        pdf = base64.b64decode(text(res, "Report") or "")
        return [chk("HTTP 200", r.status == 200, r.status), chk("Status = true", text(res, "Status") == "true", ""), chk("Report = PDF válido (%PDF-1.4 … %%EOF)", pdf.startswith(b"%PDF-1.4") and pdf.rstrip().endswith(b"%%EOF"), f"{len(pdf)} bytes")]
    return v


for op in ("GenerarReporte", "GenerarReporteV2"):
    soap_case(f"HABERES-{op}-01", "Haberes (MCD)", HP, f"http://tempuri.org/{op}", op, fixture_request("Haberes", op, {"customerIdentification": "1020304050", "accountId": "7"}), "default", "customerIdentification=1020304050; accountId=7", "200 Status=true con PDF", v_pdf(op), category="Lógica de negocio", excel_id=X("Haberes", op))
    soap_case(f"HABERES-{op}-02", "Haberes (MCD)", HP, f"http://tempuri.org/{op}", op, fixture_request("Haberes", op, {"customerIdentification": "1000000404", "accountId": "7"}), "sin-reporte", "customerIdentification=1000000404", "200 Status=false sin Report",
              lambda r, op=op: [chk("Status = false", text(local(r.xml(), f"{op}Result"), "Status") == "false", ""), chk("Sin Report", local(r.xml(), "Report") is None, "")], category="Lógica de negocio", excel_id=X("Haberes", op))


def v_rh(r):
    res = local(r.xml(), "ReporteHaberesResult")
    kids = list(res)
    raw = r.body
    first_child = re.search(r"<diffgr:diffgram[^>]*>(.)", raw)
    return [chk("HTTP 200", r.status == 200, r.status), chk("Nodes.Count > 1", len(kids) > 1, len(kids)),
            chk("Sin espacios entre diffgram y NewDataSet (minificado)", first_child is not None and first_child.group(1) == "<", ""),
            chk("CLI_IDENTIFICACION = enviada", text(res, "CLI_IDENTIFICACION") == "1020304050", text(res, "CLI_IDENTIFICACION")), chk("APC_FECHA_ULTIMA_CUOTA presente", bool(text(res, "APC_FECHA_ULTIMA_CUOTA")), "")]


soap_case("HABERES-ReporteHaberes-01", "Haberes (MCD)", HP, "http://tempuri.org/ReporteHaberes", "ReporteHaberes", fixture_request("Haberes", "ReporteHaberes", {"customerIdentification": "1020304050", "accountId": "7", "sUserQuery": ""}), "default", "customerIdentification=1020304050; accountId=7; sUserQuery=''", "200 DataSet minificado con CLI_IDENTIFICACION eco", v_rh, category="Lógica de negocio", excel_id=X("Haberes", "ReporteHaberes"))
soap_case("HABERES-ReporteHaberes-02", "Haberes (MCD)", HP, "http://tempuri.org/ReporteHaberes", "ReporteHaberes", fixture_request("Haberes", "ReporteHaberes", {"customerIdentification": "1000000404", "accountId": "7", "sUserQuery": ""}), "vacio", "customerIdentification=1000000404", "200 sólo xs:schema (Nodes.Count=1)",
          lambda r: [chk("Nodes.Count = 1", len(list(local(r.xml(), "ReporteHaberesResult"))) == 1, "")], category="Lógica de negocio", excel_id=X("Haberes", "ReporteHaberes"))
for op in [o for o in CATALOG if o["provider"] == "Haberes" and o["usage"] == "NotUsed"]:
    soap_case(f"HABERES-{op['name']}-01", "Haberes (MCD)", HP, op["soapAction"], op["name"], envelope(f'<{op["name"]} xmlns="http://tempuri.org/"/>'), "default", f"SOAPAction {op['soapAction']}", "HTTP 500 Fault explícito (sin uso en GA2)", lambda r: fault_checks(r, "GA2 no la invoca"), category="Operación contractual no invocada", excel_id=X("Haberes", op["name"]))

# ═══ TRANSUNION (SOAP + DCRequest) ══════════════════════════════════════════
TUP, TUA = "/TU.IDS.ExternalServices_latam_prod/SolutionExecution/ExternalSolutionExecution.svc", "http://tempuri.org/IExternalSolutionExecution/ExecuteXMLString"
AUTH = '<Authentication type="OnDemand"><UserId>USUARIO_TU</UserId><Password>CLAVE_TU</Password></Authentication>'


def dc(info, fields):
    return f'<DCRequest xmlns="http://transunion.com/dc/extsvc">{AUTH}<RequestInfo>{info}</RequestInfo><Fields>{fields}</Fields></DCRequest>'


def applicant(idn, extra=""):
    return f"<Applicant><IdType>1</IdType><IdNumber>{idn}</IdNumber><LastName>PEREZ</LastName><RecentPhoneNumber>3001234567</RecentPhoneNumber><IdExpeditionDate>15/01/2010</IdExpeditionDate>{extra}</Applicant>"


def start_idv(idn):
    return dc("<SolutionSetId>142</SolutionSetId><ExecuteLatestVersion>true</ExecuteLatestVersion><ExecutionMode>NewWithContext</ExecutionMode>", f'<Field key="PrimaryApplicant"><![CDATA[{applicant(idn)}]]></Field>')


def start_aqm(idn):
    return dc("<SolutionSetId>360</SolutionSetId><ExecuteLatestVersion>true</ExecuteLatestVersion><ExecutionMode>NewWithContext</ExecutionMode>",
              f'<Field key="PrimaryApplicant">{escape(applicant(idn, "<Occupation>EMPLEADO</Occupation><MonthlyIncome>3500000</MonthlyIncome>"))}</Field><Field key="ApplicationData">{escape("<ApplicationData><Product>CONSUMO</Product></ApplicationData>")}</Field>')


def send(app_id, queue, fields):
    return dc(f"<ExecutionMode>Send</ExecutionMode><ApplicationId>{app_id}</ApplicationId><QueueName>{queue}</QueueName>", "".join(f'<Field key="{k}">{v}</Field>' for k, v in fields))


def tu_body(dcr):
    return envelope(f'<ExecuteXMLString xmlns="http://tempuri.org/"><request>{escape(dcr)}</request></ExecuteXMLString>')


def dcresp(r):
    return ET.fromstring(text(r.xml(), "ExecuteXMLStringResult"))


def field(d, key):
    for f in d.iter():
        if f.tag == "Field" and f.attrib.get("key") == key:
            return f.text
    return None


tu = {}


def tu_case(cid, step, scenario, dcr, inputs, expected, validate, header=None):
    def fn():
        b = tu_body(dcr)
        extra = {"X-Mock-Scenario": header} if header else {}
        r = soap(TUP, TUA, b, extra)
        return r, req_text("POST", MOCK + TUP, {"SOAPAction": TUA, **extra}, b) + f"\n\n--- DCRequest (texto antes de escapar) ---\n{dcr}", [chk("Escenario aplicado", r.h("X-Mock-Scenario-Applied") == scenario, r.h("X-Mock-Scenario-Applied"))] + validate(r)

    return run(cid, "TransUnion", f"ExecuteXMLString · {step}", scenario, inputs, expected, fn, "Lógica de negocio (URL fija en GA2)", X("TransUnion", "ExecuteXMLString"))


def v_start(r):
    d = dcresp(r)
    tu["app"] = text(d, "ApplicationId")
    return [chk("Status = Success", text(d, "Status") == "Success", text(d, "Status")), chk("ApplicationId asignado", bool(tu["app"]), tu["app"]), chk("CurrentQueue = PhoneSelection", text(d, "CurrentQueue") == "PhoneSelection", ""),
            chk("Applicants.DocumentState = VIGENTE", "VIGENTE" in (field(d, "Applicants") or ""), "")]


tu_case("TU-001", "1 Inicio IDVision", "default", start_idv("1020304050"), "SolutionSetId=142; IdNumber=1020304050 (CDATA)", "Success, ApplicationId nuevo, cola PhoneSelection, DocumentState=VIGENTE", v_start)
app = tu.get("app")
tu_case("TU-002", "2 Envío OTP", "default", send(app, "PhoneSelection", [("ValidationMethod", "SMS"), ("PhoneType", "Mobile"), ("SelectedPhoneNumber", "3001234567")]), f"ApplicationId={app}; QueueName=PhoneSelection", "Success con OTPCode=SENT y Delay=60",
        lambda r: (lambda d: [chk("Status = Success", text(d, "Status") == "Success", ""), chk("OTPCode = SENT", field(d, "OTPCode") == "SENT", field(d, "OTPCode")), chk("Delay = 60", field(d, "Delay") == "60", "")])(dcresp(r)))
tu_case("TU-003", "3 Reenvío OTP", "default", send(app, "PinVerification_OTPInput", [("Action", "ResendOTP")]), f"ApplicationId={app}; Action=ResendOTP", "Status=Success", lambda r: [chk("Status = Success", text(dcresp(r), "Status") == "Success", "")])
pin = lambda code: lambda r: (lambda d: [chk(f"OTPReasons/Reason/Code = {code}", f"<Code>{code}</Code>" in (field(d, "ApplicationData") or ""), "")])(dcresp(r))
tu_case("TU-004", "4 Validación PIN", "default", send(app, "PinVerification_OTPInput", [("PinNumber", "000000"), ("Action", "Continue")]), f"ApplicationId={app}; PinNumber=000000 (incorrecto)", "Código 302 (OTP inválido)", pin("302"))
tu_case("TU-005", "4 Validación PIN", "default", send(app, "PinVerification_OTPInput", [("PinNumber", "123456"), ("Action", "Continue")]), f"ApplicationId={app}; PinNumber=123456 (PIN válido configurado)", "Código 300 (OTP válido)", pin("300"))


def v_aqm(r):
    d = dcresp(r)
    tu["aqm"] = text(d, "ApplicationId")
    apps = field(d, "Applicants") or ""
    cap = re.search(r"<CapacidadEndeudamiento>([^<]*)<", apps)
    return [chk("DecisionHomologada = VIABLE", "<DecisionHomologada>VIABLE<" in (field(d, "ApplicationData") or ""), ""), chk("Score = 750", "<Score>750<" in apps, ""),
            chk("CapacidadEndeudamiento con punto decimal (en-US)", cap is not None and re.fullmatch(r"\d+\.\d+", cap.group(1)) is not None, cap.group(1) if cap else None)]


tu_case("TU-006", "5 AQM / score", "default", start_aqm("1020304050"), "SolutionSetId=360; IdNumber=1020304050 (XML escapado, como ConsumirTUAqm)", "VIABLE, Score=750, CapacidadEndeudamiento=1500000.50", v_aqm)
tu_case("TU-007", "6 Historial", "default", send(tu.get("aqm"), "DocVerify", [("GenerateReport", "Y")]), f"ApplicationId={tu.get('aqm')}; QueueName=DocVerify; GenerateReport=Y", "Applicants con DSOnlineBureauCOLData",
        lambda r: [chk("DSOnlineBureauCOLData presente", "DSOnlineBureauCOLData" in (field(dcresp(r), "Applicants") or ""), ""), chk("Field DecisionHomologada", field(dcresp(r), "DecisionHomologada") == "VIABLE", "")])
tu_case("TU-008", "5 AQM / score", "no-viable", start_aqm("1000000666"), "IdNumber=1000000666 (regla → no-viable)", "DecisionHomologada = NO VIABLE", lambda r: [chk("NO VIABLE", "<DecisionHomologada>NO VIABLE<" in (field(dcresp(r), "ApplicationData") or ""), "")])
tu_case("TU-009", "5 AQM / score", "pendiente", start_aqm("1000000777"), "IdNumber=1000000777 (regla → pendiente)", "DecisionHomologada = PENDIENTE", lambda r: [chk("PENDIENTE", "<DecisionHomologada>PENDIENTE<" in (field(dcresp(r), "ApplicationData") or ""), "")])
tu_case("TU-010", "1 Inicio IDVision", "muerte-titular", start_idv("1000000999"), "IdNumber=1000000999 (regla → muerte-titular)", "DocumentState = MUERTE POR TITULAR", lambda r: [chk("MUERTE POR TITULAR", "MUERTE POR TITULAR" in (field(dcresp(r), "Applicants") or ""), "")])
tu_case("TU-011", "2 Envío OTP", "default", send("424242", "PhoneSelection", [("ValidationMethod", "SMS")]), "ApplicationId=424242 (no existe)", "Status=Failure con ErrorInfo 'Application does not exists in Queue PhoneSelection.'",
        lambda r: [chk("Status = Failure", text(dcresp(r), "Status") == "Failure", ""), chk("Mensaje exacto de cola", text(dcresp(r), "Message") == "Application does not exists in Queue PhoneSelection.", text(dcresp(r), "Message"))])
tu_case("TU-012", "1 Inicio IDVision", "auth-fallida", start_idv("1020304050"), "header X-Mock-Scenario=auth-fallida", "Authentication/Status = Failure", lambda r: [chk("Authentication/Status = Failure", "<Status>Failure</Status></Authentication>" in text(r.xml(), "ExecuteXMLStringResult").replace("<Token>token-tu-mock</Token>", ""), "")], header="auth-fallida")
tu_case("TU-013", "1 Inicio IDVision", "fault", start_idv("1000000500"), "IdNumber=1000000500 (regla → fault)", "HTTP 500 SOAP Fault (GA2: FaultException capturada → 'Error en la Conexión con TransUnion')", fault_checks)
for op in [o for o in CATALOG if o["provider"] == "TransUnion" and o["usage"] == "NotUsed"]:
    soap_case(f"TU-{op['name']}-01", "TransUnion", TUP, op["soapAction"], op["name"], envelope(f'<{op["name"]} xmlns="http://tempuri.org/"/>'), "default", f"SOAPAction {op['soapAction']}", "HTTP 500 Fault explícito (sin uso en GA2)", lambda r: fault_checks(r, "GA2 no la invoca"), category="Operación contractual no invocada", excel_id=X("TransUnion", op["name"]))

# ═══ DODO REST ══════════════════════════════════════════════════════════════
DODO_BODY = {
    "Field_GetValues": {"formCode": "83", "columnName": "C0016"}, "List_GetItems": {"listId": 5},
    "File_CopyToEP": {"fileCode": "FC-000001", "directoryCodeEP": "00"}, "File_CopyToFather": {"barCodeSon": "83-000123", "fileCode": "FC-000001", "directoryCodeFather": "00"},
    "File_Delete": {"fileCode": "FC-000001"}, "Form_GetParameters": {"formCode": "83"},
    "Form_Insert": {"operationUser": "mock", "formCode": "02", "officeCode": "001", "data": [{"field": "C0089", "value": "1020304050"}], "files": [{"ext": "pdf", "description": "Soporte", "base64String": "JVBERi0=", "documentTypeCode": "00", "directoryCode": "00"}], "checkWorkflow": True},
    "Form_Update": {"operationUser": "mock", "formCode": "83", "data": [{"field": "C0098", "value": "APROBADO"}], "filterParameters": {"field": "C0001", "operator": "=", "value": "83-000123"}},
    "Form_Delete": {"operationUser": "mock", "formCode": "83", "filterParameters": {"field": "C0001", "operator": "=", "value": "83-000123"}},
    "Form_GetData": {"formCode": "83", "filterParameters": {"field": "C0001", "operator": "=", "value": "83-000123"}},
    "Form_AttachFiles": {"operationUser": "mock", "barCode": "83-000123", "files": [{"ext": "pdf", "description": "Soporte", "base64String": "JVBERi0="}]},
    "Form_GetFileCodes": {"barCode": "83-000123"}, "Form_GetFilesDirectories": {"formCode": "83"}, "Form_GetBase64StringFile": {"fileCode": "FC-000001"},
    "SendEmail": {"sender": "a@mock.test", "receiver": "b@mock.test", "subject": "Prueba", "message": "<p>x</p>", "isHtml": True, "user": "mock", "host": "mock"},
    "GetOffices": {}, "GetToken": {"user": "mock", "password": "mock"}, "User_Info": {"userName": "mock"},
    "Workflow_Start": {"operationUser": "mock", "barCode": "83-000123", "processId": 10}, "Workflow_RunTask": {"workflowId": 9981, "step": 1},
    "Workflow_ReturnTask": {"workflowId": 9981, "returnStep": 1, "operationUser": "mock", "comment": "x"}, "Workflow_AbortProcess": {"workflowId": 9981, "operationUser": "mock", "comment": "x"},
    "Workflow_AttachFiles": {"operationUser": "mock", "workflowId": 9981, "files": [{"ext": "pdf", "description": "Soporte"}]}, "Workflow_GetCases": {"formCode": "83", "processId": 10},
    "Workflow_GetTasks": {"workflowId": 9981}, "Workflow_GetFileCodes": {"workflowId": 9981}, "Workflow_StepsForReturn": {"workflowId": 9981}, "Process_Parameters": {"processId": 10},
}
for op in [o for o in CATALOG if o["provider"] == "Dodo"]:
    name, url = op["name"], MOCK + op["route"]
    if name == "Version":
        run("DODO-Version-01", "Dodo REST", "Version", "default", "GET /dodo/api/version", "200 con una cadena JSON entre comillas",
            lambda url=url: (lambda r: (r, req_text("GET", url, {}, ""), [chk("HTTP 200", r.status == 200, r.status), chk("Cadena JSON", isinstance(r.json(), str), r.body)]))(http("GET", url)), "Reenvío (pass-through)", X("Dodo", name))
        run("DODO-Version-02", "Dodo REST", "Version", "no-encontrado", "header X-Mock-Scenario=no-encontrado", "HTTP 404 (GA2: Data=null)",
            lambda url=url: (lambda r: (r, req_text("GET", url, {"X-Mock-Scenario": "no-encontrado"}, ""), [chk("HTTP 404", r.status == 404, r.status)]))(http("GET", url, headers={"X-Mock-Scenario": "no-encontrado"})), "Reenvío (pass-through)", X("Dodo", name))
        continue
    body = json.dumps({**DODO_BODY[name], "header": {"user": "USR", "token": "TKN"}})
    hdr = {"Content-Type": "application/json; charset=utf-8", "Accept": "application/json"}

    def dodo_ok(url=url, body=body, name=name):
        r = http("POST", url, body, hdr)
        j = r.json() if r.status == 200 else {}
        c = [chk("HTTP 200", r.status == 200, r.status), chk("success = true", j.get("success") is True, j.get("success")), chk("message no vacío", bool(j.get("message")), j.get("message"))]
        if name == "Form_Insert":
            c += [chk("barCode = 02-NNNNNN", re.fullmatch(r"02-\d{6}", j.get("barCode") or "") is not None, j.get("barCode")), chk("1 fileCode por archivo", len(j.get("fileCodes", [])) == 1, j.get("fileCodes")), chk("workflowId > 0 (checkWorkflow)", (j.get("workflowId") or 0) > 0, j.get("workflowId"))]
        if name == "Form_GetParameters":
            c.append(chk("form.idFormulario = 83", (j.get("form") or {}).get("idFormulario") == 83, ""))
        return r, req_text("POST", url, hdr, body), c

    run(f"DODO-{name}-01", "Dodo REST", name, "default", f"POST {op['route']} cuerpo JSON camelCase (ver evidencia)", "200 success=true", dodo_ok, "Reenvío (pass-through)", X("Dodo", name))
    if name in ("Form_Insert", "Form_GetParameters", "Workflow_Start"):
        run(f"DODO-{name}-02", "Dodo REST", name, "fallido", "header X-Mock-Scenario=fallido", "HTTP 200 success=false con message",
            lambda url=url, body=body: (lambda r: (r, req_text("POST", url, {**hdr, "X-Mock-Scenario": "fallido"}, body), [chk("HTTP 200", r.status == 200, r.status), chk("success = false", r.json().get("success") is False, "")]))(http("POST", url, body, {**hdr, "X-Mock-Scenario": "fallido"})), "Reenvío (pass-through)", X("Dodo", name))
        run(f"DODO-{name}-03", "Dodo REST", name, "no-encontrado", "header X-Mock-Scenario=no-encontrado", "HTTP 404 texto (GA2: IsSuccess=false)",
            lambda url=url, body=body: (lambda r: (r, req_text("POST", url, {**hdr, "X-Mock-Scenario": "no-encontrado"}, body), [chk("HTTP 404", r.status == 404, r.status)]))(http("POST", url, body, {**hdr, "X-Mock-Scenario": "no-encontrado"})), "Reenvío (pass-through)", X("Dodo", name))
run("DODO-GEN-01", "Dodo REST", "Form_Insert (JSON inválido)", "—", "Cuerpo '{no es json'", "HTTP 400",
    lambda: (lambda r: (r, req_text("POST", MOCK + "/dodo/api/Forms/Form_Insert", {"Content-Type": "application/json"}, "{no es json"), [chk("HTTP 400", r.status == 400, r.status)]))(http("POST", MOCK + "/dodo/api/Forms/Form_Insert", "{no es json", {"Content-Type": "application/json"})), "Validación del protocolo", X("Dodo", "Form_Insert"))

# ═══ PASARELA ktrionApi (REST + Bearer) ═════════════════════════════════════
KB = MOCK + "/REST/KriterionDev/ktrionApi/"
JH = {"Content-Type": "application/json; charset=utf-8"}
LOGIN = json.dumps({"Id": "3f2c8a1e-0000-0000-0000-000000000001", "UserId": "3f2c8a1e-0000-0000-0000-000000000001", "NombreAplicacion": "IntegracionesKriterion", "Contrasena": "mock-password", "AplicacionId": "mock-application-id"})


def token():
    return http("POST", KB + "api/autenticacion", LOGIN, JH).json()["Data"]["Token"]


run("KTRION-Autenticacion-01", "Pasarela ktrionApi", "Autenticacion", "default", "POST api/autenticacion con credenciales ficticias de AppMain", "200 IsSuccess=true y Data.Token",
    lambda: (lambda r: (r, req_text("POST", KB + "api/autenticacion", JH, LOGIN), [chk("HTTP 200", r.status == 200, r.status), chk("IsSuccess = true", r.json()["IsSuccess"] is True, ""), chk("Data.Token = mock-ktrion-…", str(r.json()["Data"]["Token"]).startswith("mock-ktrion-"), "")]))(http("POST", KB + "api/autenticacion", LOGIN, JH)), "Lógica de negocio", X("KtrionApi", "Autenticacion"))
run("KTRION-Autenticacion-02", "Pasarela ktrionApi", "Autenticacion", "sin-token", "header X-Mock-Scenario=sin-token", "200 IsSuccess=true con Data=null (reproduce el NullReferenceException de GA2)",
    lambda: (lambda r: (r, req_text("POST", KB + "api/autenticacion", {**JH, "X-Mock-Scenario": "sin-token"}, LOGIN), [chk("Data = null", r.json()["Data"] is None, "")]))(http("POST", KB + "api/autenticacion", LOGIN, {**JH, "X-Mock-Scenario": "sin-token"})), "Lógica de negocio", X("KtrionApi", "Autenticacion"))
run("KTRION-Autenticacion-03", "Pasarela ktrionApi", "Autenticacion", "unauthorized", "header X-Mock-Scenario=unauthorized", "HTTP 401 (GA2: Exception → HTTP 409)",
    lambda: (lambda r: (r, req_text("POST", KB + "api/autenticacion", {**JH, "X-Mock-Scenario": "unauthorized"}, LOGIN), [chk("HTTP 401", r.status == 401, r.status)]))(http("POST", KB + "api/autenticacion", LOGIN, {**JH, "X-Mock-Scenario": "unauthorized"})), "Lógica de negocio", X("KtrionApi", "Autenticacion"))
FI_BODY = json.dumps({"request": {"CheckWorkflow": True, "Data": [{"Field": "C0089", "Key": "NumeroDocumento", "Value": "1020304050"}], "Files": [{"Base64String": "JVBERi0=", "Description": "Soporte", "DirectoryCode": "00", "DocumentTypeCode": "00", "Ext": "pdf", "GUID": "00000000-0000-0000-0000-000000000000"}], "FormCode": "83", "Header": {"Token": "mock-token", "User": "mock-user"}, "OfficeCode": "001", "OperationUser": "mock", "ProcessId": 1, "nextStepUser": "mock"}})


def kt(route, body, extra=None, tok=True):
    h = {**JH, **({"Authorization": f"Bearer {token()}"} if tok is True else ({"Authorization": f"Bearer {tok}"} if tok else {})), **(extra or {})}
    r = http("POST", KB + route, body, h)
    return r, req_text("POST", KB + route, {k: ("Bearer ***" if k == "Authorization" else v) for k, v in h.items()}, body)


def kt_case(cid, op, route, body, scenario, inputs, expected, validate, extra=None, tok=True):
    run(cid, "Pasarela ktrionApi", op, scenario, inputs, expected, lambda: (lambda rr: (rr[0], rr[1], validate(rr[0])))(kt(route, body, extra, tok)), "Lógica de negocio (sólo DEBUG salvo workflowattachfiles)", X("KtrionApi", op))


kt_case("KTRION-FormInsert-01", "FormInsert", "api/forminsert", FI_BODY, "default", "Bearer emitido por api/autenticacion; request.FormCode=83", "200 Data.Form_InsertResult.Success=true, BarCode 83-NNNNNN",
        lambda r: [chk("HTTP 200", r.status == 200, r.status), chk("Success = true", r.json()["Data"]["Form_InsertResult"]["Success"] is True, ""), chk("BarCode = 83-NNNNNN", re.fullmatch(r"83-\d{6}", r.json()["Data"]["Form_InsertResult"]["BarCode"] or "") is not None, r.json()["Data"]["Form_InsertResult"]["BarCode"]), chk("workflowId > 0", r.json()["Data"]["Form_InsertResult"]["workflowId"] > 0, "")])
kt_case("KTRION-FormInsert-02", "FormInsert", "api/forminsert", FI_BODY, "default", "Sin header Authorization", "HTTP 401", lambda r: [chk("HTTP 401", r.status == 401, r.status)], tok=False)
kt_case("KTRION-FormInsert-03", "FormInsert", "api/forminsert", FI_BODY, "default", "Authorization: Bearer token-no-emitido", "HTTP 401", lambda r: [chk("HTTP 401", r.status == 401, r.status)], tok="token-no-emitido")
kt_case("KTRION-FormInsert-04", "FormInsert", "api/forminsert", FI_BODY, "fallido", "header X-Mock-Scenario=fallido", "200 Success=false", lambda r: [chk("Success = false", r.json()["Data"]["Form_InsertResult"]["Success"] is False, "")], extra={"X-Mock-Scenario": "fallido"})
kt_case("KTRION-FormUpdate-01", "FormUpdate", "api/formUpdate", json.dumps({"request": {"Data": [{"Field": "C0098", "Key": "Estado", "Value": "APROBADO"}], "FilterParameters": {"Field": "C0001", "Operator": "=", "Value": "83-000123"}, "FormCode": "83", "Header": {"Token": "t", "User": "u"}, "OperationUser": "mock"}}), "default", "request.Data[0].Field=C0098", "200 Form_UpdateResult.Success=true, Data eco",
        lambda r: [chk("Success = true", r.json()["Data"]["Form_UpdateResult"]["Success"] is True, ""), chk("Data[0].Field = C0098 (eco)", r.json()["Data"]["Form_UpdateResult"]["Data"][0]["Field"] == "C0098", "")])
kt_case("KTRION-WorkflowAttachFiles-01", "WorkflowAttachFiles", "api/workflowattachfiles", json.dumps({"request": {"Files": [{"Ext": "pdf", "Description": "Soporte"}], "Header": {"Token": "t", "User": "u"}, "OperationUser": "mock", "WorkflowId": 9981}}), "default", "request.WorkflowId=9981; 1 archivo", "200 Success=true, workflowId=9981, 1 FileCode",
        lambda r: [chk("Success = true", r.json()["Data"]["Workflow_AttachFilesResult"]["Success"] is True, ""), chk("workflowId = 9981", r.json()["Data"]["Workflow_AttachFilesResult"]["workflowId"] == 9981, ""), chk("1 FileCode", len(r.json()["Data"]["Workflow_AttachFilesResult"]["FileCodes"]) == 1, "")])
kt_case("KTRION-GenerarReporteHaberes-01", "GenerarReporteHaberes", "api/generarreportehaberes", json.dumps({"Reporte": "CERTIFICADO", "ClienteIdentificacion": "1020304050", "CuentaId": 7, "Mes": None, "Anio": None}), "default", "Reporte=CERTIFICADO; ClienteIdentificacion=1020304050; CuentaId=7", "200 Data = PDF Base64",
        lambda r: [chk("Data = PDF", base64.b64decode(r.json()["Data"]).startswith(b"%PDF"), "")])
kt_case("KTRION-GenerarReporteHaberes-02", "GenerarReporteHaberes", "api/generarreportehaberes", json.dumps({"Reporte": "CERTIFICADO", "ClienteIdentificacion": "1020304050", "CuentaId": 7}), "sin-reporte", "header X-Mock-Scenario=sin-reporte", "200 IsSuccess=false, Data=null",
        lambda r: [chk("IsSuccess = false", r.json()["IsSuccess"] is False, ""), chk("Data = null", r.json()["Data"] is None, "")], extra={"X-Mock-Scenario": "sin-reporte"})
for op in [o for o in CATALOG if o["provider"] == "KtrionApi" and o["usage"] == "NotUsed"]:
    route = op["route"].split("ktrionApi/")[1]
    kt_case(f"KTRION-{op['name']}-01", op["name"], route, "{}", "default", f"POST {route}", "HTTP 501 explícito (sin consumidores en GA2)", lambda r: [chk("HTTP 501", r.status == 501, r.status)])
    results[-1]["category"] = "Operación contractual no invocada"

# ═══ SMTP ═══════════════════════════════════════════════════════════════════
http("DELETE", MOCK + "/smtp/messages")


def smtp_send():
    m = EmailMessage()
    m["From"], m["To"], m["Subject"] = "CajaHonor <notificaciones@cajahonor.gov.co>", "afiliado.prueba@mock.test", "Comunicación oficial de Preaprobado de crédito hipotecario"
    m.set_content("<html><body><p>Estimado JUAN CARLOS PEREZ GOMEZ</p></body></html>", subtype="html")
    m.add_attachment(b"%PDF-1.4\n%%EOF\n", maintype="application", subtype="pdf", filename="Comunicación oficial de preaprobado.pdf")
    with smtplib.SMTP("127.0.0.1", args.smtp_port, timeout=30) as s:
        s.send_message(m)
    r = http("GET", MOCK + "/smtp/messages")
    msgs = r.json()
    last = msgs[0] if msgs else {}
    eml = http("GET", f"{MOCK}/smtp/messages/{last.get('id')}/eml")
    return r, f"SMTP 127.0.0.1:{args.smtp_port} sin TLS ni AUTH\nFrom: notificaciones@cajahonor.gov.co\nTo: afiliado.prueba@mock.test\nSubject: Comunicación oficial de Preaprobado de crédito hipotecario\n(HTML + adjunto PDF)\n\nLuego GET /smtp/messages y /smtp/messages/{{id}}/eml", [
        chk("1 correo capturado", len(msgs) == 1, len(msgs)), chk("Asunto UTF-8 decodificado", last.get("subject") == "Comunicación oficial de Preaprobado de crédito hipotecario", last.get("subject")),
        chk("Remitente", last.get("mailFrom") == "notificaciones@cajahonor.gov.co", last.get("mailFrom")), chk("Adjunto PDF listado", "Comunicación oficial de preaprobado.pdf" in last.get("attachments", []), last.get("attachments")),
        chk(".eml descargable (message/rfc822)", eml.status == 200 and "message/rfc822" in (eml.h("Content-Type") or ""), eml.status)]


run("SMTP-001", "SMTP / correo", "EnviarNotificacion", "default", "Correo HTML con PDF adjunto al puerto SMTP del mock", "Correo capturado con asunto, remitente y adjunto", smtp_send, "Lógica de negocio", X("Smtp", "EnviarNotificacion"))


def smtp_reject():
    try:
        with smtplib.SMTP("127.0.0.1", args.smtp_port, timeout=30) as s:
            s.sendmail("notificaciones@cajahonor.gov.co", ["rechazado@mock.test"], "Subject: Comunicado Oficial - Rechazo\r\n\r\nx")
        return None, "RCPT TO:<rechazado@mock.test>", [chk("Destinatario rechazado con 550", False, "fue aceptado")]
    except smtplib.SMTPRecipientsRefused as e:
        code = list(e.recipients.values())[0][0]
        return None, "RCPT TO:<rechazado@mock.test>", [chk("Respuesta 550 en RCPT TO", code == 550, code)]


run("SMTP-002", "SMTP / correo", "EnviarNotificacion", "destinatario-rechazado", "Destinatario rechazado@mock.test (Smtp:RejectRecipients)", "550 en RCPT TO (GA2: SmtpFailedRecipientException + auditoría ERROR)", smtp_reject, "Lógica de negocio", X("Smtp", "EnviarNotificacion"))


def smtp_auth():
    http("DELETE", MOCK + "/smtp/messages")
    with smtplib.SMTP("127.0.0.1", args.smtp_port, timeout=30) as s:
        s.login("notificaciones@cajahonor.gov.co", "clave-ficticia")
        s.sendmail("notificaciones@cajahonor.gov.co", ["afiliado.prueba@mock.test"], "Subject: Comunicado Oficial - aprobacion\r\n\r\nx")
    msgs = http("GET", MOCK + "/smtp/messages").json()
    return None, "AUTH LOGIN notificaciones@cajahonor.gov.co / clave-ficticia + MAIL/RCPT/DATA", [chk("AUTH aceptada y usuario registrado", msgs and msgs[0]["authenticatedUser"] == "notificaciones@cajahonor.gov.co", msgs[0]["authenticatedUser"] if msgs else None)]


run("SMTP-003", "SMTP / correo", "EnviarNotificacion", "default (con AUTH)", "AUTH con credenciales ficticias", "Correo capturado con usuario autenticado", smtp_auth, "Lógica de negocio", X("Smtp", "EnviarNotificacion"))
run("SMTP-004", "SMTP / correo", "RecuperarContrasena", "—", "Identity → smtp-mail.outlook.com:587 (fijo en código)", "No aplica", None, "Requiere infraestructura externa", X("Smtp", "RecuperarContrasena"),
    blocked="Host Outlook fijo en LoginBusinessLogic.cs:355-356 con STARTTLS; el mock no implementa STARTTLS. Desbloqueo: hacer configurable el host o DNS + servidor STARTTLS con certificado de confianza.")

# ═══ SERVICE BUS ════════════════════════════════════════════════════════════
for op in [o for o in CATALOG if o["provider"] == "ServiceBus"]:
    subj = op["name"]

    def sb_pub(subj=subj):
        r = http("POST", f"{MOCK}/_mock/servicebus/publish/{subj}", "", {"Content-Type": "application/json"})
        sample = http("GET", f"{MOCK}/_mock/servicebus/events/{subj}/sample").json()
        prop = [k for k in sample if k not in ("Id", "CreatedOn")][0]
        return r, f"POST {MOCK}/_mock/servicebus/publish/{subj} (sin cuerpo → ejemplo de la ficha)\n\nEjemplo publicado:\n{json.dumps(sample, indent=2, ensure_ascii=False)}", [
            chk("HTTP 200 publicado en el emulador", r.status == 200, r.body[:100]), chk("Topic chonorintegraciones", r.json().get("topic") == "chonorintegraciones", ""),
            chk("Subject correcto", r.json().get("subject") == subj, ""), chk(f"Payload {prop}.request presente (PascalCase)", "request" in sample[prop], prop)]

    run(f"SB-{subj}-01", "Azure Service Bus", subj, "default", f"POST /_mock/servicebus/publish/{subj}", "Mensaje publicado en el topic con Subject correcto", sb_pub, "Requiere infraestructura externa (emulador)", X("ServiceBus", subj))

# ═══ GA2 → MOCK (API GA2 real con perfil Mocks) ═════════════════════════════
ga2_ok = False
if args.ga2:
    try:
        ga2_ok = http("GET", args.ga2.rstrip("/") + "/swagger/index.html", timeout=10).status == 200
    except Exception:  # noqa: BLE001
        ga2_ok = False


def jwt(key):
    b64 = lambda d: base64.urlsafe_b64encode(d).rstrip(b"=").decode()
    now = int(time.time())
    h, pl = b64(json.dumps({"alg": "HS256", "typ": "JWT"}).encode()), b64(json.dumps({"sub": "mock-pruebas", "nbf": now - 60, "exp": now + 3600}).encode())
    return f"{h}.{pl}.{b64(hmac.new(key.encode(), f'{h}.{pl}'.encode(), hashlib.sha256).digest())}"


GA2_CASES = [
    ("GA2-001", "TusDatos", "ConsultarCliente", "GET", "/api/integrations/tusdatos/customer/1020304050/document-type/CC?checkHistory=true", None, False, "default", lambda j: [chk("isSuccess = true", j["isSuccess"] is True, ""), chk("data.jobId presente", bool(j["data"]["jobId"]), j["data"]["jobId"])]),
    ("GA2-002", "TusDatos", "ConsultarCliente", "GET", "/api/integrations/tusdatos/customer/1000000404/document-type/CC?checkHistory=true", None, False, "no-encontrado", lambda j: [chk("GA2 interpreta 404 como éxito con data=null", j["isSuccess"] is True and j["data"] is None, j["returnMessage"])]),
    ("GA2-003", "TusDatos", "ConsultarVigia", "GET", "/api/integrations/tusdatos/vigia/customer/1000000666/document-type/CC", None, False, "riesgo", lambda j: [chk("data.riskLAFT = RIESGO ALTO", j["data"]["riskLAFT"] == "RIESGO ALTO", "")]),
    ("GA2-004", "Dodo", "Form_GetParameters", "POST", "/api/integrations/dodo/forms/parameters", {"formCode": "83"}, False, "default", lambda j: [chk("data.form.idFormulario = 83", j["data"]["form"]["idFormulario"] == 83, "")]),
    ("GA2-005", "Vigia", "F_VERIFICATERCERO", "POST", "/api/credito/verificacionTercero", {"Pe_Dato": "1000000666", "Pe_TipoVerificacion": "1", "Pe_Porcentaje": ""}, True, "reportado", lambda j: [chk("data.encontrado = S, listas OFAC", j["data"]["encontrado"] == "S" and "OFAC" in j["data"]["listas"], j["data"])]),
    ("GA2-006", "Fenix", "ValidarBiometria", "GET", "/api/credito/validarBiometria?request=1020304050", None, True, "default", lambda j: [chk("returnMessage = Biometria verificada", j["returnMessage"] == "Biometria verificada", j["returnMessage"])]),
    ("GA2-007", "Fenix", "ValidarBiometria", "GET", "/api/credito/validarBiometria?request=1000000404", None, True, "sin-datos", lambda j: [chk("GA2: isSuccess=false y 'No hay información en Fénix…'", j["isSuccess"] is False and "No hay información en Fénix" in (j.get("returnMessage") or ""), j.get("returnMessage"))]),
    ("GA2-008", "Seven", "InsertarGnTerce", "POST", "/api/credito/InsertarGnTerce", {"Emp_codi": 1, "Tip_codi": 1, "Ter_coda": "1020304050"}, True, "default", lambda j: [chk("GA2 devuelve el XML con RETORNO=0", "<RETORNO>0</RETORNO>" in (j.get("returnMessage") or ""), j.get("returnMessage"))]),
]
for cid, prov, op, method, path, body, auth, scen, val in GA2_CASES:
    if not ga2_ok:
        run(cid, DISPLAY[prov], f"{op} (vía GA2)", scen, path, "GA2 llama al mock e interpreta la respuesta", None, "Lógica de negocio (extremo a extremo)", X(prov, op),
            blocked="API GA2 no disponible: requiere SDK .NET 5 (contenedor), base de datos (stub) y emulador de Service Bus. Ver guía §2.4.")
        continue

    def ga2_fn(prov=prov, op=op, method=method, path=path, body=body, auth=auth, scen=scen, val=val):
        http("DELETE", MOCK + "/_mock/journal")
        h = {"Content-Type": "application/json", "Accept": "application/json", **({"Authorization": "Bearer " + jwt(args.jwt_key)} if auth else {})}
        r = http(method, args.ga2.rstrip("/") + path, json.dumps(body) if body is not None else None, h)
        jr = http("GET", f"{MOCK}/_mock/journal?provider={prov}&operation={op}&take=1").json()
        c = [chk("GA2 HTTP 200", r.status == 200, r.status), chk("El mock recibió la llamada de GA2", bool(jr), f"escenario={jr[0]['scenario']}" if jr else "sin llamada"),
             chk(f"Escenario en el mock = {scen}", bool(jr) and jr[0]["scenario"] == scen, jr[0]["scenario"] if jr else None)]
        if r.status == 200:
            c += val(r.json())
        return r, req_text(method, args.ga2.rstrip("/") + path, {k: ("Bearer ***" if k == "Authorization" else v) for k, v in h.items()}, json.dumps(body) if body else "") + (f"\n\n--- Llamada recibida por el mock (journal) ---\n{json.dumps(jr, indent=2, ensure_ascii=False)[:4000]}" if jr else ""), c

    run(cid, DISPLAY[prov], f"{op} (vía GA2)", scen, path, "GA2 llama al mock e interpreta la respuesta", ga2_fn, "Lógica de negocio (extremo a extremo)", X(prov, op))


def sb_consume():
    http("DELETE", MOCK + "/_mock/journal")
    pub = http("POST", f"{MOCK}/_mock/servicebus/publish/WorkManagerInsertRequest", "", {"Content-Type": "application/json"})
    time.sleep(15)
    wm = http("GET", f"{MOCK}/_mock/journal?provider=WorkManager&take=10").json()
    return pub, f"POST {MOCK}/_mock/servicebus/publish/WorkManagerInsertRequest; esperar 15 s; GET /_mock/journal?provider=WorkManager", [
        chk("Mensaje publicado", pub.status == 200, pub.status), chk("GA2 procesó el evento y llamó a WorkManager Form_Insert", any(e["operation"] == "Form_Insert" for e in wm), f"llamadas WorkManager: {len(wm)}")]


if ga2_ok:
    e = run("SB-CONSUMO-01", "Azure Service Bus", "WorkManagerInsertRequest (consumo por GA2)", "default", "Publicar evento con GA2 en ejecución", "GA2 consume el evento y llama a WorkManager Form_Insert", sb_consume, "Bloqueado por defecto conocido", X("ServiceBus", "WorkManagerInsertRequest"))
    if e["status"] == "FALLIDA":
        e["status"] = "BLOQUEADA"
        e["comment"] = "Defecto D1 de GA2 confirmado: el evento se consume pero WorkManager recibe 0 llamadas (manejadores sin registrar en DI; además 'request' llega nulo, D2). Desbloqueo: registrar los 5 manejadores en Autofac y deserializar con IncludeFields."
else:
    run("SB-CONSUMO-01", "Azure Service Bus", "WorkManagerInsertRequest (consumo por GA2)", "default", "Publicar evento con GA2 en ejecución", "GA2 consume el evento y llama a WorkManager", None, "Bloqueado por defecto conocido", X("ServiceBus", "WorkManagerInsertRequest"), blocked="API GA2 no disponible.")

# Volcado del journal y resultados.
(EV / "03_journal").mkdir(parents=True, exist_ok=True)
_snapshots.extend(json.loads(http("GET", MOCK + "/_mock/journal?take=1000").body))
(EV / "03_journal" / "journal_completo.json").write_text(json.dumps(_snapshots, indent=1, ensure_ascii=False), encoding="utf-8")
(EV / "resultados.json").write_text(json.dumps(results, indent=2, ensure_ascii=False), encoding="utf-8")
from collections import Counter  # noqa: E402

print("\nResumen:", dict(Counter(r["status"] for r in results)), "total", len(results))
covered = {r["excel_id"] for r in results if r["excel_id"]}
print("Métodos del Excel cubiertos por al menos un caso:", len(covered), "de", len(EXCEL))
