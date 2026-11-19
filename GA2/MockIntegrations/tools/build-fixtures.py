"""Genera los fixtures SOAP del mock a partir de las fichas de contrato validadas.

Fuente: docs/integraciones/mocks/E0*.md (sobres capturados con los proxies WCF reales de GA2 y
respuestas validadas por ida y vuelta). Este script copia esos sobres sin tocar su estructura y
aplica una curación explícita y reproducible:

* marcadores dinámicos ({{req:...}}, {{seq:...}}) en los campos que GA2 guarda o reenvía;
* valores realistas en los campos que GA2 interpreta;
* variantes de error (error de negocio, Success=false, resultado nulo...) derivadas del default.

Uso (desde la raíz del repositorio GA2): python3 MockIntegrations/tools/build-fixtures.py
Reemplaza el contenido de MockIntegrations/src/MockIntegrations.Api/Providers/*/Fixtures/*.xml
(salvo los fixtures que el script no genera, p. ej. los de TransUnion, que se construyen en código).
"""
import base64
import pathlib
import re

ROOT = pathlib.Path(__file__).resolve().parents[2]
DOCS = ROOT / "docs/integraciones/mocks"
OUT = ROOT / "MockIntegrations/src/MockIntegrations.Api/Providers"


def blocks(doc):
    """Devuelve [(operación, etiqueta, cuerpo)] de los bloques ```xml de una ficha."""
    lines = (DOCS / doc).read_text(encoding="utf-8").split("\n")
    result, current, label, i = [], None, "", 0
    while i < len(lines):
        line = lines[i]
        m = re.match(r"^###\s+(?:\d+\.\s+)?`([^`]+)`", line) or re.match(r"^## \d+\. Operación `([^`]+)`", line)
        if m:
            current = m.group(1)
        if line.startswith("**") or line.startswith("### "):
            label = line
        if line.startswith("```"):
            j = i + 1
            while not lines[j].startswith("```"):
                j += 1
            result.append((current, label, "\n".join(lines[i + 1:j]).rstrip() + "\n"))
            i = j
        i += 1
    return result


def envelopes(doc):
    """Request y response de ejemplo por operación (primer bloque de cada tipo)."""
    found = {}
    for op, label, body in blocks(doc):
        if not op or ("<s:Envelope" not in body and "<soap:Envelope" not in body):
            continue
        kind = "request" if "Request de ejemplo" in label or label.startswith("### 2.1 Request") else (
            "default" if "Response de ejemplo" in label else None)
        if kind and (op, kind) not in found:
            found[(op, kind)] = body
    return found


written = []


def write(provider, name, content):
    path = OUT / provider / "Fixtures" / name
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8")
    written.append(path)


def sub(pattern, repl, text, count=0, required=True):
    new, n = re.subn(pattern, repl, text, count=count, flags=re.M)
    if required and n == 0:
        raise SystemExit(f"Curación: el patrón no coincide: {pattern}")
    return new


def minimal_pdf(text):
    """PDF 1.4 válido de una página (con tabla xref correcta)."""
    stream = f"BT /F1 14 Tf 72 760 Td ({text}) Tj ET".encode("latin-1")
    objs = [
        b"<< /Type /Catalog /Pages 2 0 R >>",
        b"<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
        b"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 5 0 R >> >> /Contents 4 0 R >>",
        b"<< /Length " + str(len(stream)).encode() + b" >>\nstream\n" + stream + b"\nendstream",
        b"<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
    ]
    out = bytearray(b"%PDF-1.4\n")
    offsets = []
    for n, body in enumerate(objs, 1):
        offsets.append(len(out))
        out += f"{n} 0 obj\n".encode() + body + b"\nendobj\n"
    xref = len(out)
    out += f"xref\n0 {len(objs) + 1}\n0000000000 65535 f \n".encode()
    for off in offsets:
        out += f"{off:010d} 00000 n \n".encode()
    out += f"trailer\n<< /Size {len(objs) + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF\n".encode()
    return base64.b64encode(bytes(out)).decode()


# ─── E1 Seven ────────────────────────────────────────────────────────────────
seven = envelopes("E01-seven-erp.md")
seven_ids = {
    "MCO_CONT": "{{seq:seven.mco_cont:551200}}", "MCO_NUME": "{{seq:seven.mco_nume:3301}}",
    "MPR_CONT": "{{seq:seven.mpr_cont:45871}}", "FAC_CONT": "{{seq:seven.fac_cont:77812}}",
    "FAC_NUME": "{{seq:seven.fac_nume:1203}}", "PVD_CODD": "{{seq:seven.pvd_codd:900123}}",
}
for (op, kind), body in sorted(seven.items()):
    if kind == "request":
        write("Seven", f"{op}.request.xml", body)
        continue
    default = body
    for tag, token in seven_ids.items():
        default = re.sub(rf"&lt;{tag}&gt;\d+&lt;/{tag}&gt;", f"&lt;{tag}&gt;{token}&lt;/{tag}&gt;", default)
    write("Seven", f"{op}.default.xml", default)
    if "&lt;RETORNO&gt;0&lt;/RETORNO&gt;" in body:
        # RETORNO=1 → error de negocio; GA2 corta el flujo y muestra TXTERROR.
        message = {
            "CrearCDP": "Rubro sin saldo disponible (mock)",
            "InsertarGnTerce": "Error al registrar el tercero (mock)",
        }.get(op, f"Error de negocio simulado por MockIntegrations ({op})")
        error = body.replace("&lt;RETORNO&gt;0&lt;/RETORNO&gt;", "&lt;RETORNO&gt;1&lt;/RETORNO&gt;")
        error = error.replace("&lt;TXTERROR&gt;&lt;/TXTERROR&gt;", f"&lt;TXTERROR&gt;{message}&lt;/TXTERROR&gt;")
        for tag in seven_ids:
            error = re.sub(rf"&lt;{tag}&gt;\d+&lt;/{tag}&gt;", "", error)
        write("Seven", f"{op}.error-negocio.xml", error)

# El flujo de pago tolera un tercero ya existente (E01 §5, escenario 2).
write("Seven", "InsertarGnTerce.tercero-existente.xml", seven[("InsertarGnTerce", "default")].replace(
    "&lt;RETORNO&gt;0&lt;/RETORNO&gt;&lt;TXTERROR&gt;&lt;/TXTERROR&gt;",
    "&lt;RETORNO&gt;1&lt;/RETORNO&gt;&lt;TXTERROR&gt;El tercero ya se encuentra registrado&lt;/TXTERROR&gt;"))
# RETORNO=0 sin FAC_CONT/FAC_NUME: reproduce el NullReferenceException de GA2 (prueba de robustez).
write("Seven", "InsertarFacturaP.sin-identificadores.xml", re.sub(
    r"&lt;FAC_(CONT|NUME)&gt;\d+&lt;/FAC_\1&gt;", "", seven[("InsertarFacturaP", "default")]))

# ─── E2 WorkManager (SOAP) ───────────────────────────────────────────────────
wm = envelopes("E02-workmanager-dodo-soap.md")
for (op, kind), body in sorted(wm.items()):
    if kind == "request":
        write("WorkManager", f"{op}.request.xml", body)
        continue
    default = body.replace("<d4p1:Message>Message</d4p1:Message>", "<d4p1:Message>Operación exitosa (MockIntegrations)</d4p1:Message>")
    if op == "Form_Insert":
        # BarCode = número de radicado "<FormCode>-<consecutivo>" (GA2 hace Split("-")[0]).
        default = sub(r"<d4p1:BarCode>[^<]*</d4p1:BarCode>", "<d4p1:BarCode>{{req:FormCode|83}}-{{seq:wm.barcode:123:D6}}</d4p1:BarCode>", default)
        default = sub(r"<d4p1:RecordId>\d+</d4p1:RecordId>", "<d4p1:RecordId>{{seq:wm.recordid:4521}}</d4p1:RecordId>", default)
        default = sub(r"<d4p1:workflowId>\d+</d4p1:workflowId>", "<d4p1:workflowId>{{seq:wm.workflowid:9981}}</d4p1:workflowId>", default)
        default = sub(r"<d5p1:string>[^<]*</d5p1:string>", "<d5p1:string>FC-{{seq:wm.filecode:1:D6}}</d5p1:string>", default)
    if op in ("Form_AttachFiles", "Workflow_AttachFiles"):
        default = sub(r"<d4p1:BarCode>BarCode</d4p1:BarCode>", "<d4p1:BarCode>{{req:FormCode|83}}-{{seq:wm.barcode:123:D6}}</d4p1:BarCode>", default)
        default = sub(r"<d5p1:string>FileCodes</d5p1:string>", "<d5p1:string>FC-{{seq:wm.filecode:1:D6}}</d5p1:string>", default)
        default = sub(r"<d4p1:workflowId>1</d4p1:workflowId>", "<d4p1:workflowId>{{req:WorkflowId|9981}}</d4p1:workflowId>", default)
    if op == "Workflow_Start":
        default = sub(r"<d4p1:WorkflowId>1</d4p1:WorkflowId>", "<d4p1:WorkflowId>{{seq:wm.workflowid:9981}}</d4p1:WorkflowId>", default)
    if op == "Form_GetParameters":
        # GA2 sólo lee Form.IdFormulario y Form.NombreCampoSticker (hijos directos de Form, 10 espacios).
        default = sub(r"^( {10})<d4p1:IdFormulario>1</d4p1:IdFormulario>", r"\1<d4p1:IdFormulario>83</d4p1:IdFormulario>", default)
        default = sub(r"^( {10})<d4p1:NombreCampoSticker>[^<]*</d4p1:NombreCampoSticker>", r"\1<d4p1:NombreCampoSticker>RADICADO</d4p1:NombreCampoSticker>", default)
        default = sub(r"^( {10})<d4p1:Codigo>[^<]*</d4p1:Codigo>", r"\1<d4p1:Codigo>{{req:FormCode|83}}</d4p1:Codigo>", default)
    write("WorkManager", f"{op}.default.xml", default)
    if "<d4p1:Success>true</d4p1:Success>" in default:
        failed = default.replace("<d4p1:Success>true</d4p1:Success>", "<d4p1:Success>false</d4p1:Success>")
        failed = failed.replace("Operación exitosa (MockIntegrations)", f"Error funcional simulado por MockIntegrations ({op})")
        write("WorkManager", f"{op}.fallido.xml", failed)
    # Resultado nulo: reproduce el NullReferenceException de WorkManagerService (E02 §2.2).
    nil = re.sub(rf"<{op}Result [^>]*>.*</{op}Result>",
                 f'<{op}Result i:nil="true" xmlns:i="http://www.w3.org/2001/XMLSchema-instance" />', default, flags=re.S)
    write("WorkManager", f"{op}.resultado-nulo.xml", nil)
# Enlace DODO sin Form (NullReferenceException en ObtenerEnlaceDodo).
write("WorkManager", "Form_GetParameters.sin-formulario.xml",
      re.sub(r"<d4p1:Form>.*?\n {8}</d4p1:Form>", '<d4p1:Form i:nil="true" />', (OUT / "WorkManager/Fixtures/Form_GetParameters.default.xml").read_text(encoding="utf-8"), flags=re.S))

# ─── E3 BUA ──────────────────────────────────────────────────────────────────
bua_blocks = [b for op, label, b in blocks("E03-bua-soap.md") if "<s:Envelope" in b]
write("Bua", "ObtenerBUA.request.xml", bua_blocks[0])
bua = bua_blocks[1]
top = {  # hijos directos de ObtenerBUAResult (8 espacios). Valores realistas, mismos tipos.
    "actividad": "MILITAR ACTIVO", "estadoCivil": "CASADO", "identificacion": "{{req:identificacion}}",
    "primerApellido": "PEREZ", "primerNombre": "JUAN", "profesion": "ADMINISTRADOR", "resolucion": "RES-2010-0457",
    "segundoApellido": "GOMEZ", "segundoNombre": "CARLOS", "sexo": "M", "unidad": "EJERCITO NACIONAL",
    "fechaNacimiento": "1985-03-12T00:00:00", "fechaExpedicionId": "2003-04-02T00:00:00",
    "fechaAlta": "2008-02-01T00:00:00", "fechaInscripcion": "2008-02-01T00:00:00",
}
for tag, value in top.items():
    bua = sub(rf"^( {{8}})<d1p1:{tag}>[^<]*</d1p1:{tag}>", rf"\g<1><d1p1:{tag}>{value}</d1p1:{tag}>", bua)
nested = {  # residencia y cónyuge (10 espacios)
    "celular": "3001234567", "telefono": "6017654321", "correoPersonal": "juan.perez@example.com",
    "correoInstitucional": "juan.perez@example.mil.co", "direccionResidencia": "CL 100 # 15-20",
    "barrioResidencia": "CHICO", "primerNombreConyuge": "MARIA", "primerApellidoConyuge": "LOPEZ",
    "identificacionConyuge": "52123456", "celularConyuge": "3109876543",
}
for tag, value in nested.items():
    bua = sub(rf"^( {{10}})<d1p1:{tag}>[^<]*</d1p1:{tag}>", rf"\g<1><d1p1:{tag}>{value}</d1p1:{tag}>", bua)
write("Bua", "ObtenerBUA.default.xml", bua)
# No encontrado: ObtenerBUAResult nulo; GA2 reintenta con tipo_Id 1 → 4 → 5.
write("Bua", "ObtenerBUA.no-encontrado.xml",
      re.sub(r"<ObtenerBUAResult [^>]*>.*</ObtenerBUAResult>",
             '<ObtenerBUAResult i:nil="true" xmlns:i="http://www.w3.org/2001/XMLSchema-instance" />', bua, flags=re.S))

# ─── E4 Vigía ────────────────────────────────────────────────────────────────
vigia = envelopes("E04-vigia.md")
write("Vigia", "F_VERIFICATERCERO.request.xml", vigia[("F_VERIFICATERCERO", "request")])
reported = vigia[("F_VERIFICATERCERO", "default")]
clean = reported.replace('<Encontrado xsi:type="xsd:string">S</Encontrado>', '<Encontrado xsi:type="xsd:string">N</Encontrado>')
clean = clean.replace('<Metodo xsi:type="xsd:string">DOCUMENTO</Metodo>', '<Metodo xsi:type="xsd:string"></Metodo>')
clean = clean.replace('<Listas xsi:type="xsd:string">OFAC, ONU</Listas>', '<Listas xsi:type="xsd:string"></Listas>')
if clean == reported:
    raise SystemExit("Curación Vigía: no se encontró el ejemplo esperado")
write("Vigia", "F_VERIFICATERCERO.default.xml", clean)
write("Vigia", "F_VERIFICATERCERO.reportado.xml", reported)

# ─── E5 Fénix ────────────────────────────────────────────────────────────────
fenix = [b for op, label, b in blocks("E05-fenix-biometria.md") if "Envelope" in b]
write("Fenix", "ValidarBiometria.request.xml", fenix[0])
full = sub(r"<A001NUM_IDENTIFICACION>\d+</A001NUM_IDENTIFICACION>",
           "<A001NUM_IDENTIFICACION>{{req:identificacion|1020304050}}</A001NUM_IDENTIFICACION>", fenix[1])
write("Fenix", "ValidarBiometria.default.xml", full)
write("Fenix", "ValidarBiometria.parcial.xml", sub(r"<A008HUELLA>1</A008HUELLA>", "<A008HUELLA>0</A008HUELLA>", full))
write("Fenix", "ValidarBiometria.sin-datos.xml", fenix[2])
write("Fenix", "ValidarBiometria.dato-invalido.xml", sub(r"<A008HUELLA>1</A008HUELLA>", "<A008HUELLA>X</A008HUELLA>", full))

# ─── E6 Haberes ──────────────────────────────────────────────────────────────
haberes = envelopes("E06-haberes-mcd.md")
pdf = minimal_pdf("MockIntegrations - Certificado de haberes (documento de prueba)")
for (op, kind), body in sorted(haberes.items()):
    if kind == "request":
        write("Haberes", f"{op}.request.xml", body)
        continue
    if op in ("GenerarReporte", "GenerarReporteV2"):
        default = sub(r"<Report>[^<]*</Report>", f"<Report>{pdf}</Report>", body)
        write("Haberes", f"{op}.default.xml", default)
        no_report = sub(r"\s*<Report>[^<]*</Report>", "", default).replace("<Status>true</Status>", "<Status>false</Status>")
        write("Haberes", f"{op}.sin-reporte.xml", no_report.replace("<Message>OK</Message>", "<Message>No se encontró información para el reporte</Message>"))
    if op == "ReporteHaberes":
        # Minificado: GA2 lee Nodes[1].FirstNode y no tolera espacios entre diffgram y NewDataSet.
        if "\n" in body.strip():
            raise SystemExit("ReporteHaberes debe estar minificado")
        default = sub(r"<CLI_IDENTIFICACION>\d+</CLI_IDENTIFICACION>",
                      "<CLI_IDENTIFICACION>{{req:customerIdentification|1020304050}}</CLI_IDENTIFICACION>", body)
        write("Haberes", f"{op}.default.xml", default)
        write("Haberes", f"{op}.vacio.xml", re.sub(r"<diffgr:diffgram .*?</diffgr:diffgram>", "", default))

# ─── E7 TransUnion ───────────────────────────────────────────────────────────
# Las respuestas se construyen en código (estado por ApplicationId). Estos dos archivos sólo alimentan
# los ejemplos de OpenAPI: inicio IDVision (request) y su respuesta OK.
tu = [(label, b) for op, label, b in blocks("E07-transunion-decisioncentre.md") if "<s:Envelope" in b]
write("TransUnion", "ExecuteXMLString.request.xml", next(b for label, b in tu if "ExecuteXMLString xmlns" in b and "&lt;DCRequest" in b))
write("TransUnion", "ExecuteXMLString.response-example.xml", next(b for label, b in tu if "ExecuteXMLStringResult>&lt;DCResponse" in b))

print(f"{len(written)} fixtures escritos en {OUT.relative_to(ROOT)}")
