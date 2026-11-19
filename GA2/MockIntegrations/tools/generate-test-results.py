"""Genera ~/Downloads/Resultados_Pruebas_MockIntegrations_GA2.xlsx a partir de resultados.json (run-test-cases.py),
el catálogo del mock y el Excel de estimación (inventario de 153 métodos)."""
import json
import pathlib
import sys

from openpyxl import Workbook, load_workbook
from openpyxl.formatting.rule import CellIsRule
from openpyxl.styles import Alignment, Border, Font, PatternFill, Side
from openpyxl.utils import get_column_letter
from openpyxl.worksheet.table import Table, TableStyleInfo

HOME = pathlib.Path.home() / "Downloads"
EV = HOME / "Evidencias_MockIntegrations_GA2"
OUT = HOME / "Resultados_Pruebas_MockIntegrations_GA2.xlsx"
ROOT = pathlib.Path(__file__).resolve().parents[2]
results = json.loads((EV / "resultados.json").read_text(encoding="utf-8"))
catalog = json.loads((ROOT / "MockIntegrations/docs/estimacion/catalogo-operaciones.json").read_text(encoding="utf-8"))
est = load_workbook(HOME / "Estimacion_Mocks_GA2_desde_cero.xlsx", data_only=True)["Detalle por método"]
excel_rows = [r for r in est.iter_rows(min_row=5, values_only=True) if r[0] and str(r[0]).startswith("M")]
DISPLAY = {"Seven": "Seven ERP", "WorkManager": "WorkManager / DODO (SOAP)", "Bua": "BUA", "Vigia": "Vigía", "Fenix": "Fénix", "Haberes": "Haberes (MCD)",
           "TransUnion": "TransUnion", "TusDatos": "TusDatos", "Dodo": "Dodo REST", "KtrionApi": "Pasarela ktrionApi", "Smtp": "SMTP / correo", "ServiceBus": "Azure Service Bus"}
cat_by = {(DISPLAY[o["provider"]], o["name"]): o for o in catalog}

F = "Arial"
HEAD = Font(name=F, bold=True, color="FFFFFF")
FILL = PatternFill("solid", fgColor="1F3864")
B = Border(*(Side(style="thin", color="BFBFBF"),) * 4)
WRAP = Alignment(wrap_text=True, vertical="top")
STATUS_COLORS = {"APROBADA": "C6EFCE", "FALLIDA": "FFC7CE", "BLOQUEADA": "FFEB9C", "NO EJECUTADA": "D9D9D9"}


def sheet(ws, title, cols, widths, rows, table_name, note=None):
    ws["A1"] = title
    ws["A1"].font = Font(name=F, size=14, bold=True, color="1F3864")
    if note:
        ws["A2"] = note
        ws["A2"].font = Font(name=F, italic=True, color="595959")
    for c, (h, w) in enumerate(zip(cols, widths), 1):
        cell = ws.cell(row=4, column=c, value=h)
        cell.font, cell.fill, cell.border, cell.alignment = HEAD, FILL, B, Alignment(wrap_text=True, vertical="center")
        ws.column_dimensions[get_column_letter(c)].width = w
    for i, row in enumerate(rows):
        for c, v in enumerate(row, 1):
            cell = ws.cell(row=5 + i, column=c, value=v)
            cell.font, cell.border, cell.alignment = Font(name=F, size=10), B, WRAP
    last = 4 + max(len(rows), 1)
    t = Table(displayName=table_name, ref=f"A4:{get_column_letter(len(cols))}{last}")
    t.tableStyleInfo = TableStyleInfo(name="TableStyleMedium2", showRowStripes=True)
    ws.add_table(t)
    ws.freeze_panes = "B5"
    return last


wb = Workbook()
ws = wb.active
ws.title = "Resultados"
cols = ["ID de prueba", "Proveedor", "Método", "Escenario", "Datos de entrada", "Resultado esperado", "Resultado real", "Estado",
        "Fecha de ejecución", "Evidencia", "Incidencia o comentario", "Categoría", "ID método (Excel)"]
rows = [[r["id"], r["provider"], r["method"], r["scenario"], r["inputs"], r["expected"], r["actual"], r["status"], r["date"].replace("T", " "),
         str(EV / r["evidence"]) if r["evidence"] else "", r.get("comment") or "", r.get("category") or "", r.get("excel_id") or ""] for r in results]
last = sheet(ws, "Resultados de las pruebas de MockIntegrations – GA2", cols, [16, 18, 30, 16, 45, 45, 70, 13, 18, 60, 55, 26, 11], rows, "Resultados",
             "Una fila por caso. Resultado real = salida de la ejecución del 2026-10-19 (run-test-cases.py). Evidencia = archivo con la petición y la respuesta reales.")
for st, color in STATUS_COLORS.items():
    ws.conditional_formatting.add(f"H5:H{last}", CellIsRule(operator="equal", formula=[f'"{st}"'], fill=PatternFill("solid", fgColor=color)))

# Inventario de los 153 métodos del Excel.
inv = wb.create_sheet("Inventario de métodos")
ICOLS = ["ID (Excel)", "Proveedor", "Método", "Protocolo", "Uso en GA2 (Excel)", "Clasificación", "Endpoint", "SOAPAction", "Escenarios implementados",
         "Casos", "Aprobados", "Fallidos", "Bloqueados", "No ejecutados", "Estado consolidado"]
irows = []
for r in excel_rows:
    op = cat_by[(r[1], r[2])]
    cls = {"Business": "1. Invocado por la lógica de negocio", "PassThrough": "2. Reenviado por GA2 (pass-through)", "NotUsed": "3. Contractual, GA2 no lo invoca"}[op["usage"]]
    extra = []
    if op["provider"] in ("ServiceBus", "Smtp") or (op["provider"] in ("TransUnion", "Bua") and op["usage"] == "Business"):
        extra.append("4. Requiere infraestructura o configuración adicional")
    if (op["provider"], op["name"]) in (("Smtp", "RecuperarContrasena"), ("ServiceBus", "WorkManagerInsertRequest")):
        extra.append("5. Bloqueado por defecto conocido / falta de datos")
    scen = ["default"] + [s["name"] for s in op["scenarios"]] + ["fault", "http-500", "slow"]
    irows.append([r[0], r[1], r[2], r[3], r[4], "; ".join([cls] + extra), f"{op['method']} {op['route']}", op.get("soapAction") or "—", ", ".join(dict.fromkeys(scen)), None, None, None, None, None, None])
ilast = sheet(inv, "Inventario de métodos (fuente: Estimacion_Mocks_GA2_desde_cero.xlsx) contrastado con el mock", ICOLS,
              [9, 18, 28, 15, 24, 40, 50, 55, 40, 7, 9, 8, 10, 11, 18], irows, "Inventario",
              "Casos, aprobados, etc. = fórmulas sobre la hoja Resultados (columna 'ID método (Excel)').")
R = f"Resultados!$M$5:$M${last}"
S = f"Resultados!$H$5:$H${last}"
for i in range(5, ilast + 1):
    inv.cell(row=i, column=10, value=f"=COUNTIF({R},A{i})")
    for c, st in zip(range(11, 15), ["APROBADA", "FALLIDA", "BLOQUEADA", "NO EJECUTADA"]):
        inv.cell(row=i, column=c, value=f'=COUNTIFS({R},A{i},{S},"{st}")')
    inv.cell(row=i, column=15, value=f'=IF(J{i}=0,"SIN CASO",IF(L{i}>0,"FALLIDA",IF(M{i}>0,IF(K{i}>0,"APROBADA con bloqueos","BLOQUEADA"),"APROBADA")))')
    for c in range(10, 16):
        inv.cell(row=i, column=c).font, inv.cell(row=i, column=c).border = Font(name=F, size=10), B
for st, color in [("APROBADA", "C6EFCE"), ("FALLIDA", "FFC7CE"), ("BLOQUEADA", "FFEB9C"), ("APROBADA con bloqueos", "FFF2CC"), ("SIN CASO", "D9D9D9")]:
    inv.conditional_formatting.add(f"O5:O{ilast}", CellIsRule(operator="equal", formula=[f'"{st}"'], fill=PatternFill("solid", fgColor=color)))

# Resumen con fórmulas.
rs = wb.create_sheet("Resumen", 0)
rs["A1"] = "Resumen de la ejecución de pruebas – MockIntegrations GA2"
rs["A1"].font = Font(name=F, size=14, bold=True, color="1F3864")
rs["A2"] = "Fecha de ejecución: 2026-10-19. Instancia de pruebas del mock: http://127.0.0.1:5090 (SMTP 2526). API GA2 (perfil Mocks): http://127.0.0.1:5102."
rs["A2"].font = Font(name=F, italic=True, color="595959")
kp = [("Métodos en el Excel de referencia", f"=COUNTA('Inventario de métodos'!A5:A{ilast})"),
      ("Métodos con al menos un caso", f"=COUNTIF('Inventario de métodos'!J5:J{ilast},\">0\")"),
      ("Casos registrados", f"=COUNTA(Resultados!A5:A{last})"),
      ("Casos ejecutados (con petición real enviada)", f'=COUNTIF(Resultados!$G$5:$G${last},"<>No ejecutado")'),
      ("APROBADA", f'=COUNTIF({S},"APROBADA")'), ("FALLIDA", f'=COUNTIF({S},"FALLIDA")'),
      ("BLOQUEADA", f'=COUNTIF({S},"BLOQUEADA")'), ("NO EJECUTADA", f'=COUNTIF({S},"NO EJECUTADA")'),
      ("Pruebas automatizadas (dotnet test)", "210 de 210 aprobadas (ver 01_tests_automatizados)")]
for i, (k, v) in enumerate(kp):
    rs.cell(row=4 + i, column=1, value=k).font = Font(name=F, bold=True)
    c = rs.cell(row=4 + i, column=2, value=v)
    c.font = Font(name=F)
    if k in STATUS_COLORS:
        c.fill = PatternFill("solid", fgColor=STATUS_COLORS[k])
rs.column_dimensions["A"].width = 62
rs.column_dimensions["B"].width = 22
r0 = 15
heads = ["Proveedor", "Casos", "APROBADA", "FALLIDA", "BLOQUEADA", "NO EJECUTADA"]
for c, h in enumerate(heads, 1):
    cell = rs.cell(row=r0, column=c, value=h)
    cell.font, cell.fill, cell.border = HEAD, FILL, B
P = f"Resultados!$B$5:$B${last}"
provs = list(dict.fromkeys(r["provider"] for r in results))
for i, pv in enumerate(provs):
    rr = r0 + 1 + i
    rs.cell(row=rr, column=1, value=pv)
    rs.cell(row=rr, column=2, value=f"=COUNTIF({P},A{rr})")
    for c, st in zip(range(3, 7), ["APROBADA", "FALLIDA", "BLOQUEADA", "NO EJECUTADA"]):
        rs.cell(row=rr, column=c, value=f'=COUNTIFS({P},A{rr},{S},"{st}")')
    for c in range(1, 7):
        rs.cell(row=rr, column=c).font, rs.cell(row=rr, column=c).border = Font(name=F), B
tr = r0 + 1 + len(provs)
rs.cell(row=tr, column=1, value="TOTAL")
for c in range(2, 7):
    col = get_column_letter(c)
    rs.cell(row=tr, column=c, value=f"=SUM({col}{r0 + 1}:{col}{tr - 1})")
for c in range(1, 7):
    rs.cell(row=tr, column=c).font, rs.cell(row=tr, column=c).fill, rs.cell(row=tr, column=c).border = Font(name=F, bold=True), PatternFill("solid", fgColor="FFE699"), B
for c in range(3, 7):
    rs.column_dimensions[get_column_letter(c)].width = 14
rs.cell(row=tr + 1, column=1, value="Control: suma por proveedor = casos registrados")
rs.cell(row=tr + 1, column=2, value=f'=IF(B{tr}=B6,"OK","REVISAR")')

# Incidencias.
inc = wb.create_sheet("Incidencias")
INC = [
    ("INC-01", "Defecto del mock (corregido)", "SMTP", "La API de inspección del buzón no decodificaba el nombre del adjunto en formato RFC 2231 (filename*=utf-8''…), que usan clientes como Python. Con System.Net.Mail (cliente de GA2) funcionaba.", "Ejecución 1: SMTP-001 FALLIDA (04_ejecucion_1_antes_de_correccion/SMTP-001.txt).", "Corregido en SmtpCaptureServer.DecodeParameter + prueba de regresión Smtp_decodes_rfc2231_attachment_names_sent_by_other_clients. dotnet test: 210/210. SMTP-001 APROBADA en la ejecución final.", "Resuelta"),
    ("INC-02", "Error de la herramienta de pruebas (corregido)", "BUA", "El validador leía el primer elemento 'identificacion' del XML, que pertenece al dependiente, no al afiliado.", "Ejecución 1: BUA-001/002/005 FALLIDA.", "Validador corregido para leer los hijos directos de ObtenerBUAResult. El mock no se modificó.", "Resuelta"),
    ("INC-03", "Observación de datos de prueba", "BUA", "En la ficha del mock, el dependiente (BeneficiarioBEL) conserva valores de relleno ('identificacion', 'nombreDependiente').", "02_casos/BUA/BUA-001.txt", "No afecta a GA2 (sólo lo guarda en el Payload). Opcional: dar valores realistas en tools/build-fixtures.py.", "Abierta (menor)"),
    ("INC-04", "Limitación del entorno (resuelta)", "Fénix vía GA2", "GA2-007: GA2 respondía HTTP 500 porque la base stub no tenía dbo.InsertarNotificacionFuncionarioExterno (GA2 notifica a ASERA_T cuando Fénix no tiene datos) ni dbo.CrearRegistroError.", "00_entorno/ga2-api.log", "Se añadió el procedimiento de notificación a la base stub (docker/ga2-smoke/stub-database.sql). GA2-007 APROBADA y notificación verificada en BD (03_journal/ga2_auditoria_bd_stub.txt).", "Resuelta"),
    ("INC-05", "Defecto de GA2 (confirmado)", "Azure Service Bus", "D1: los 5 manejadores WorkManager*IntegrationEventHandler no están registrados en DI; GA2 consume y descarta los eventos. D2: 'request' llega nulo (System.Text.Json ignora campos públicos).", "SB-CONSUMO-01; 00_entorno/verificacion_problemas_conocidos.txt", "Equipo de GA2: registrar los manejadores en Autofac y deserializar con IncludeFields (o propiedades). Repetir SB-CONSUMO-01.", "Abierta – bloquea"),
    ("INC-06", "Limitación de GA2 (confirmada)", "TransUnion / BUA ws.01", "URLs fijas en el código (ValidacionesTU.cs:16 producción; ConsumoBua.cs:38 + Reference.cs).", "NE-001, NE-002", "Hacer las URLs configurables o redirigir por DNS/hosts en red aislada (TransUnion requiere además certificado TLS de confianza).", "Abierta"),
    ("INC-07", "Limitación de GA2 (confirmada)", "SMTP Identity", "RecuperaContrasena usa smtp-mail.outlook.com:587 con STARTTLS fijo en código; el mock no implementa STARTTLS.", "SMTP-004", "Hacer configurable el host SMTP de Identity o implementar STARTTLS en el mock + DNS.", "Abierta – bloquea"),
    ("INC-08", "Falta de datos", "Varios", "No existe base de datos de pruebas de GA2: los flujos de negocio completos no se pueden ejecutar.", "NE-003", "Proveer copia anonimizada de la base de pruebas (equipo de BD de GA2).", "Abierta – bloquea"),
    ("INC-09", "Contratos no confirmados", "Seven, Vigía, TransUnion, TusDatos, Service Bus, WorkManager", "Los valores de negocio del mock (p. ej. statusJobTusDatos 'pendiente/finalizado', riskLAFT, Encontrado S/N, DCResponse) son los configurados en el mock, no muestras reales del proveedor.", "Excel de estimación, hoja Pendientes (C1–C6)", "Obtener muestras reales (tablas de auditoría, equipos de cada proveedor) y ajustar fixtures.", "Abierta"),
    ("INC-10", "Escenarios inexistentes", "TusDatos", "Los nombres 'ner' y 'malnoautorizado' no existen en el código. Los implementados son: default, pep, historial, no-encontrado, unauthorized, fault, http-500, slow.", "TUSDATOS-014, TUSDATOS-015 (HTTP 400 'el escenario no existe')", "Usar 'pep' y 'unauthorized'.", "Informativa"),
    ("INC-11", "Entorno", "Todos", "Había una instancia del mock iniciada por el usuario (dotnet run, puerto 5080/2525). No se detuvo ni se modificó: las pruebas se ejecutaron en una instancia propia (5090/2526) del mismo código.", "00_entorno/arranque_entorno.txt", "Para repetir con la instancia estándar, usar http://localhost:5080 y SMTP 2525.", "Informativa"),
    ("INC-12", "Evidencias", "—", "No se pudieron obtener capturas de Swagger (Firefox headless captura antes de que Swagger UI se dibuje).", "05_capturas/LEEME.txt", "Evidencias equivalentes: peticiones/respuestas reales en 02_casos.", "Informativa"),
]
sheet(inc, "Incidencias, limitaciones y acciones", ["ID", "Tipo", "Afecta a", "Descripción", "Evidencia", "Acción / resolución", "Estado"], [8, 26, 22, 70, 45, 60, 16], [list(x) for x in INC], "Incidencias")

for w in wb.worksheets:
    w.sheet_view.zoomScale = 90
wb.save(OUT)
print(OUT, len(rows), "casos", len(irows), "métodos")
