"""Genera docs/estimacion/Estimacion_Mocks_GA2_desde_cero.xlsx.

Estimación de esfuerzo para construir DESDE CERO los mocks de cada método de integración de GA2.
Entrada: catálogo de operaciones del mock (docs/estimacion/catalogo-operaciones.json, exportado de
GET /_mock/catalog), que sirve sólo como inventario de contratos: no se descuenta nada por estar ya hecho.

Uso: PYTHONPATH=<openpyxl> python3 MockIntegrations/tools/generate-estimation.py
Las horas por actividad se derivan de la tabla de reglas RULES (visible en la hoja "Supuestos y criterios")
y de los ajustes por método documentados en la columna "Supuestos y riesgos". Totales, días y resúmenes
son fórmulas de Excel.
"""
import json
import pathlib

from openpyxl import Workbook
from openpyxl.chart import BarChart, Reference
from openpyxl.comments import Comment
from openpyxl.formatting.rule import CellIsRule, FormulaRule
from openpyxl.styles import Alignment, Border, Font, PatternFill, Side
from openpyxl.utils import get_column_letter
from openpyxl.worksheet.table import Table, TableStyleInfo

ROOT = pathlib.Path(__file__).resolve().parents[2]
OUT = ROOT / "MockIntegrations/docs/estimacion/Estimacion_Mocks_GA2_desde_cero.xlsx"
catalog = json.loads((ROOT / "MockIntegrations/docs/estimacion/catalogo-operaciones.json").read_text(encoding="utf-8"))

S_SUP = "Supuestos y criterios"
S_DET = "Detalle por método"
S_INT = "Resumen por integración"
S_EXE = "Resumen ejecutivo"
S_RISK = "Pendientes y riesgos"
JORNADA = f"'{S_SUP}'!$C$5"
PERSONAS = f"'{S_SUP}'!$C$6"
EFICIENCIA = f"'{S_SUP}'!$C$7"

ACTIVITIES = ["Análisis", "Diseño", "Implementación", "Pruebas automatizadas", "Validación con GA2", "Documentación", "Ajustes e integración", "Configuración del entorno"]
# Horas base por actividad para 3 mocks (cada mock adicional: +1,0 h implementación y +0,75 h pruebas).
RULES = {
    "Adicional": [0.5, 0.25, 1.0, 0.75, 0.25, 0.25, 0.25, 0.0],
    "Baja": [1.0, 0.5, 2.0, 1.5, 0.5, 0.5, 0.5, 0.0],
    "Media": [2.0, 1.0, 4.0, 3.0, 1.5, 1.0, 1.0, 0.0],
    "Alta": [3.0, 2.0, 8.0, 5.0, 3.0, 1.5, 2.0, 0.0],
    "Muy alta": [6.0, 3.0, 16.0, 8.0, 5.0, 2.0, 4.0, 0.0],
}
EXTRA_PER_MOCK = {2: 1.0, 3: 0.75}

PROTOCOL = {"Soap11": "SOAP 1.1 / HTTP", "Rest": "REST / JSON", "Smtp": "SMTP (TCP)", "ServiceBus": "AMQP (Azure Service Bus)"}
USAGE = {"Business": "Sí – lógica de negocio", "PassThrough": "Sí – pass-through (reenvío)", "NotUsed": "No – operación adicional del contrato"}
CERTAINTY = {"Validated": "Validado con el código de GA2", "StructureOnly": "Estructura validada; contenido por confirmar", "NotImplemented": "Sin contrato de respuesta validado"}
SPEC = {
    "Seven": "docs/integraciones/mocks/E01-seven-erp.md", "WorkManager": "docs/integraciones/mocks/E02-workmanager-dodo-soap.md",
    "Bua": "docs/integraciones/mocks/E03-bua-soap.md", "Vigia": "docs/integraciones/mocks/E04-vigia.md",
    "Fenix": "docs/integraciones/mocks/E05-fenix-biometria.md", "Haberes": "docs/integraciones/mocks/E06-haberes-mcd.md",
    "TransUnion": "docs/integraciones/mocks/E07-transunion-decisioncentre.md", "TusDatos": "docs/integraciones/mocks/E08-tusdatos.md",
    "Dodo": "docs/integraciones/mocks/E09-dodo-rest.md", "KtrionApi": "docs/integraciones/mocks/E10-pasarela-ktrionapi.md",
    "Smtp": "docs/integraciones/mocks/E11-smtp.md", "ServiceBus": "docs/integraciones/mocks/E12-azure-service-bus.md",
}
SEVEN_PROXY = {
    "scnmcont": "SCnMcont", "WPgOblig": "WSCDP", "spofactu": "WSFactura", "SIeWssec": "WSProveedor",
    "wgnterce": "WSTercero", "WInTipro": "WSWintipro", "ValidarAportes": "WSAportes",
}
PROXY = {
    "WorkManager": "src/GA2.Domain/Core/ReferenciasSvc/WorkManager/Reference.cs",
    "Bua": "src/GA2.Domain/Core/ReferenciasSvc/Bua/Reference.cs; src/GA2.Domain/Core/Connected Services/ServiceReference1/Reference.cs",
    "Vigia": "src/GA2.Domain/Core/Connected Services/ServiceVigiaVerificacionTercero/Reference.cs",
    "Fenix": "src/GA2.Domain/Core/ReferenciasSvc/Fenix/Reference.cs",
    "Haberes": "src/GA2.Domain/Core/ReferenciasSvc/Haberes/Reference.cs",
    "TransUnion": "src/GA2.Application/Main/TransUnionService/Reference.cs; src/GA2.Application/Main/TransUnion/ValidacionesTU.cs",
    "TusDatos": "src/GA2.Domain/Core/ReferenciasSvc/TusDatos/TusDatosClient.cs",
    "KtrionApi": "src/GA2.Application/Main/ProviderRequest/ProviderRequestFactory.cs; src/GA2.Application/Main/ProviderRequest/IntegracionesGA2/",
    "ServiceBus": "src/GA2.BuildingBlocks/EventBusServiceBus/EventBusServiceBus.cs; src/GA2.Domain/Core/IntegrationEvents/Events/WorkManager/WorkManagerIntegrationEvent.cs",
}
SMTP_REF = {
    "EnviarNotificacion": "src/GA2.Infraestructure/Repositories/EmailNotificacion/EmailNotificacionRepository.cs",
    "RecuperarContrasena": "src/GA2.Domain/Core/Login/LoginBusinessLogic.cs; src/GA2.Apis/GA2/Endpoints/Integraciones/SmtpTestController.cs",
}
DISPLAY = {"Seven": "Seven ERP", "WorkManager": "WorkManager / DODO (SOAP)", "Bua": "BUA", "Vigia": "Vigía", "Fenix": "Fénix",
           "Haberes": "Haberes (MCD)", "TransUnion": "TransUnion", "TusDatos": "TusDatos", "Dodo": "Dodo REST",
           "KtrionApi": "Pasarela ktrionApi", "Smtp": "SMTP / correo", "ServiceBus": "Azure Service Bus"}
ORDER = ["Bua", "Fenix", "Seven", "Vigia", "TransUnion", "Haberes", "WorkManager", "TusDatos", "Dodo", "KtrionApi", "Smtp", "ServiceBus"]

# ─── Escenarios (3–5 mocks) ─────────────────────────────────────────────────
SC = {
    "soap_retorno4": "1) Éxito: RETORNO=0 con consecutivos. 2) Error funcional: RETORNO=1 con TXTERROR. 3) Entrada inválida: SOAP Fault s:Client. 4) Error técnico: SOAP Fault s:Server / HTTP 500 HTML.",
    "soap_retorno5": "1) Éxito: RETORNO=0 con consecutivos. 2) Error funcional: RETORNO=1 con TXTERROR. 3) Entrada inválida: SOAP Fault s:Client. 4) Caso límite propio del método (ver supuestos). 5) Error técnico: Fault / HTTP 500 / timeout.",
    "soap_typed3": "1) Éxito con el resultado tipado. 2) Error funcional (Retorno/TxtError). 3) Entrada inválida: SOAP Fault s:Client.",
    "soap_text3": "1) Éxito (texto del ERP; contenido de ejemplo). 2) Error funcional (formato a confirmar). 3) Entrada inválida: SOAP Fault s:Client.",
    "wm5": "1) Éxito Success=true. 2) Error funcional Success=false + Message. 3) Entrada inválida: SOAP Fault s:Client. 4) Caso límite: Result nulo (i:nil). 5) Error técnico: Fault / HTTP 500 / timeout.",
    "wm4": "1) Éxito Success=true. 2) Error funcional Success=false. 3) Entrada inválida: SOAP Fault s:Client. 4) Caso límite: Result nulo (i:nil).",
    "wm_wrapped3": "1) Éxito con la estructura validada. 2) Error funcional Success=false. 3) Entrada inválida: SOAP Fault s:Client.",
    "unused3": "1) Éxito con la estructura del proxy (contrato de respuesta a confirmar). 2) Error funcional / SOAP Fault explícito. 3) Entrada inválida.",
    "unused_rest3": "1) Éxito con el patrón Response<T> de la pasarela (contrato a confirmar). 2) Error funcional IsSuccess=false. 3) Entrada inválida / 401.",
    "dodo4": "1) Éxito success=true con datos. 2) Error funcional: HTTP 200 success=false. 3) Entrada inválida: HTTP 400. 4) Recurso no encontrado: HTTP 404.",
    "dodo_version3": "1) Éxito: cadena JSON con la versión. 2) No encontrado: HTTP 404 (GA2: Data=null). 3) Error técnico: HTTP 500.",
}
SPECIAL = {
    ("Seven", "InsertarGnTerce"): ("Media", 5, "1) Éxito GN_TERCE/RETORNO=0. 2) Tercero ya existente (RETORNO=1, GA2 continúa). 3) Error funcional RETORNO=1. 4) Entrada inválida: Fault s:Client. 5) Error técnico: Fault / HTTP 500.", "Raíz GN_TERCE obligatoria (GA2 usa //GN_TERCE/RETORNO). Primer paso del flujo de pago."),
    ("Seven", "SyncPoPvdor"): ("Media", 4, SC["soap_retorno4"], "GA2 lee PVD_CODD (entero)."),
    ("Seven", "CrearCDP"): ("Media", 5, SC["soap_retorno5"].replace("Caso límite propio del método (ver supuestos)", "Rubro sin saldo (detiene el pago)"), "GA2 guarda MPR_CONT; consecutivo dinámico."),
    ("Seven", "InsertarFacturaP"): ("Media", 5, SC["soap_retorno5"].replace("Caso límite propio del método (ver supuestos)", "RETORNO=0 sin FAC_CONT/FAC_NUME (NullReferenceException en GA2)"), "GA2 lee FAC_CONT y FAC_NUME."),
    ("Seven", "InsertarMovContable"): ("Alta", 5, SC["soap_retorno5"].replace("Caso límite propio del método (ver supuestos)", "Comprobante con vDetalle de varias líneas / segundo proxy XmlSerializer"), "DTO TSCnMcont con detalle; dos proxies de GA2 (DataContract y XmlSerializer) deben aceptar el mismo XML."),
    ("Seven", "AnularCnMcont"): ("Media", 4, SC["soap_retorno4"], "GA2 (Cartera) lee RETORNO con expresión regular."),
    ("Seven", "InsertarPoFactu"): ("Media", 4, SC["soap_retorno4"], "Request extenso (TSPoFactu1, ~90 elementos)."),
    ("Seven", "InsertarPoFactuP"): ("Media", 4, SC["soap_retorno4"], "Request extenso (TSPoFactu2, ~110 elementos)."),
    ("Seven", "SetContabToper"): ("Baja", 3, SC["soap_typed3"], "Resultado tipado TRetToper[]."),
    ("Seven", "SyncFaClienComplex"): ("Baja", 3, SC["soap_typed3"], "Resultado tipado TSalida."),
    ("Seven", "GetCuentaContable"): ("Baja", 3, SC["soap_text3"], "Semántica del texto devuelto no confirmada con Seven."),
    ("Seven", "GetCuentaContableTO"): ("Baja", 3, SC["soap_text3"], "Semántica del texto devuelto no confirmada con Seven."),
    ("Seven", "GetAporte"): ("Baja", 3, SC["soap_text3"], "Semántica del texto devuelto no confirmada con Seven."),
    ("WorkManager", "Form_Insert"): ("Alta", 5, "1) Éxito: radicado BarCode {FormCode}-{n}, RecordId, workflowId. 2) Error funcional Success=false ('encolado'). 3) Entrada inválida: Fault s:Client. 4) Resultado nulo (NullReferenceException). 5) Error técnico: Fault / timeout.", "Consecutivo dinámico; orden alfabético estricto del DataContractSerializer. En DEBUG la radicación de crédito va por la pasarela."),
    ("WorkManager", "Form_Update"): ("Media", 5, SC["wm5"], "Usada en Release; en DEBUG va por la pasarela."),
    ("WorkManager", "Form_AttachFiles"): ("Media", 5, SC["wm5"], "Operación más invocada (≥18 puntos); FileCodes por archivo."),
    ("WorkManager", "Workflow_AttachFiles"): ("Media", 5, SC["wm5"], "SOAP directo y manejador de Service Bus."),
    ("WorkManager", "Form_GetParameters"): ("Alta", 5, "1) Éxito: Form.IdFormulario y NombreCampoSticker. 2) Error funcional Success=false. 3) Entrada inválida: Fault s:Client. 4) Form nulo (NullReferenceException en ObtenerEnlaceDodo). 5) Error técnico: Fault / timeout.", "DTO Formulario extenso (~130 elementos anidados)."),
    ("WorkManager", "Form_Delete"): ("Media", 4, SC["wm4"], "Sólo desde el manejador de Service Bus."),
    ("WorkManager", "Form_GetData"): ("Media", 4, SC["wm4"], "Pass-through api/credito/getdataforms."),
    ("Bua", "ObtenerBUA"): ("Muy alta", 5, "1) Éxito: ficha completa (47 campos, cuentas, aportes, embargos). 2) No encontrado: Result nulo. 3) Encontrado sólo con otro tipo_Id (reintento 1→4→5). 4) Entrada inválida: Fault s:Client. 5) Error técnico: Fault / timeout.", "Un único XML debe servir a dos proxies con tipos distintos (ws.01/ws.02); orden alfabético estricto. ws.01 con URL fija (hosts/DNS)."),
    ("Vigia", "F_VERIFICATERCERO"): ("Alta", 5, "1) Sin coincidencias (Encontrado=N). 2) Persona reportada (Encontrado=S, Listas). 3) Entrada inválida: Fault s:Client. 4) Error técnico: SOAP Fault. 5) Infraestructura: HTTP 500 HTML / timeout.", "Estilo RPC/encoded (xsi:type obligatorio). Dominio de Encontrado/Listas/Metodo no confirmado."),
    ("Fenix", "ValidarBiometria"): ("Alta", 5, "1) Biometría completa (A008*=1). 2) Biometría parcial. 3) Sin datos: diffgram sin NewDataSet. 4) Dato inválido: falla la deserialización. 5) Error técnico: SOAP Fault.", "DataSet .NET (xs:schema + diffgram); GA2 lee Nodes[1]."),
    ("Haberes", "GenerarReporte"): ("Media", 4, "1) Éxito: PDF válido en Report. 2) Sin reporte: Status=false. 3) Entrada inválida: Fault s:Client. 4) Error técnico: SOAP Fault.", "Sólo en Release; en DEBUG va por la pasarela. Requiere un PDF válido."),
    ("Haberes", "GenerarReporteV2"): ("Media", 4, "1) Éxito: PDF válido en Report. 2) Sin reporte: Status=false. 3) Entrada inválida: Fault s:Client. 4) Error técnico: SOAP Fault.", "Igual que GenerarReporte con mes y año."),
    ("Haberes", "ReporteHaberes"): ("Alta", 5, "1) Éxito: DataSet REPORTE_HABERES minificado. 2) DataSet vacío (Nodes.Count=1). 3) Entrada inválida: Fault s:Client. 4) Caso límite: DataSet indentado rompe a GA2 (validar minificado). 5) Error técnico: SOAP Fault.", "GA2 lee Nodes[1].FirstNode: la respuesta debe ir sin espacios."),
    ("TransUnion", "ExecuteXMLString"): ("Muy alta", 5, "1) Flujo exitoso por paso (inicio IDVision, OTP, reenvío, PIN 300, AQM VIABLE, historial). 2) Error funcional: PIN 302 / NO VIABLE / PENDIENTE. 3) Entrada inválida: DCRequest mal formado / autenticación fallida. 4) Caso límite: aplicación fuera de cola, MUERTE POR TITULAR, sin ApplicationData. 5) Error técnico: Fault / timeout.", "EXCEPCIÓN DOCUMENTADA: un solo método con 6 pasos lógicos y estado por ApplicationId; los 5 mocks agrupan ≈9 variantes. Horas base de 'Muy alta' duplicadas en análisis, diseño, implementación, pruebas, validación y ajustes. URL de producción fija en código (DNS + TLS). Sin muestras reales de DCResponse."),
    ("TusDatos", "ConsultarCliente"): ("Alta", 5, "1) Éxito: inicia job (jobId). 2) Resultado del histórico (isFromHistory). 3) Persona PEP. 4) No encontrado: HTTP 404 (GA2: Data=null). 5) API key ausente/inválida: HTTP 401.", "Header x-api-key; inicia el estado asíncrono del job."),
    ("TusDatos", "ConsultarJob"): ("Alta", 5, "1) Job pendiente. 2) Job finalizado con resultado. 3) Job desconocido: HTTP 404. 4) API key inválida: HTTP 401. 5) Error técnico / timeout (>15 s).", "Estado entre llamadas (N sondeos). Dominio de statusJobTusDatos no confirmado."),
    ("TusDatos", "ConsultarVigia"): ("Media", 5, "1) Sin riesgo. 2) Riesgo LA/FT alto. 3) No encontrado: HTTP 404. 4) API key inválida: HTTP 401. 5) Error técnico: HTTP 500 / timeout.", "Ruta sin el segmento 'tusdatos'. Dominio de riskLAFT no confirmado."),
    ("Dodo", "Version"): ("Baja", 3, SC["dodo_version3"], "GA2 exige una cadena JSON entre comillas."),
    ("Dodo", "Form_Insert"): ("Media", 4, SC["dodo4"], "barCode {formCode}-{n}, fileCodes por archivo, workflowId si checkWorkflow."),
    ("Dodo", "Form_AttachFiles"): ("Media", 4, SC["dodo4"], "fileCodes por archivo."),
    ("Dodo", "Workflow_AttachFiles"): ("Media", 4, SC["dodo4"], "fileCodes por archivo."),
    ("Dodo", "Workflow_Start"): ("Media", 4, SC["dodo4"], "Genera workflowId."),
    ("KtrionApi", "Autenticacion"): ("Alta", 5, "1) Éxito: emite token. 2) Credenciales rechazadas: HTTP 401 (GA2 → 409). 3) Data nulo (NullReferenceException en GA2). 4) Entrada inválida: HTTP 400. 5) Error técnico: HTTP 500.", "GA2 pide un token antes de cada llamada; el mock debe registrar los tokens emitidos."),
    ("KtrionApi", "FormInsert"): ("Alta", 5, "1) Éxito: Form_InsertResult con BarCode. 2) Error funcional Success=false. 3) Token ausente/no emitido: HTTP 401. 4) Entrada inválida: HTTP 400. 5) Error técnico: HTTP 500 / timeout (100 s).", "Cuerpo = proxy WCF serializado con Newtonsoft (PascalCase). Sólo en DEBUG."),
    ("KtrionApi", "FormUpdate"): ("Media", 4, "1) Éxito: Form_UpdateResult. 2) Error funcional Success=false. 3) Token inválido: HTTP 401. 4) Error técnico: HTTP 500.", "Sólo en DEBUG; ruta con mayúscula (api/formUpdate)."),
    ("KtrionApi", "WorkflowAttachFiles"): ("Media", 5, "1) Éxito: Workflow_AttachFilesResult con FileCodes. 2) Error funcional Success=false. 3) Token inválido: HTTP 401. 4) Entrada inválida: HTTP 400. 5) Error técnico: HTTP 500.", "Se usa en Debug y en Release."),
    ("KtrionApi", "GenerarReporteHaberes"): ("Media", 4, "1) Éxito: PDF Base64 en Data. 2) Sin reporte: IsSuccess=false. 3) Token inválido: HTTP 401. 4) Error técnico: HTTP 500.", "Sólo en DEBUG."),
    ("Smtp", "EnviarNotificacion"): ("Muy alta", 5, "1) Envío exitoso con HTML y PDF adjunto. 2) Destinatario rechazado (550). 3) Autenticación opcional (AUTH LOGIN/PLAIN). 4) Servidor caído (puerto cerrado). 5) Lentitud > 30 s (timeout).", "Implica construir un servidor SMTP de captura y su API de inspección. GA2 lee plantillas de disco antes de enviar."),
    ("Smtp", "RecuperarContrasena"): ("Alta", 4, "1) Envío exitoso por STARTTLS. 2) Destinatario rechazado. 3) Credenciales rechazadas. 4) Servidor no disponible.", "Host smtp-mail.outlook.com:587 fijo en código: requiere STARTTLS con certificado de confianza y redirección DNS. Hoy no simulado."),
    ("ServiceBus", "WorkManagerInsertRequest"): ("Alta", 5, "1) Mensaje válido con Subject correcto. 2) Payload con request nulo (defecto de GA2). 3) Subject desconocido (se completa sin efecto). 4) Manejador falla → reentrega / dead-letter. 5) Arranque: reglas existentes y $Default ausente.", "Incluye la preparación del emulador oficial + SQL Server + proxy TLS (SDK 7.9 de GA2). Validación bloqueada por los defectos D1/D2."),
}
ENV = {  # Horas de configuración del entorno por proveedor, asignadas a su método principal.
    ("Seven", "InsertarMovContable"): (2.0, "7 claves SvcUtils:Seven* en el perfil de pruebas"),
    ("WorkManager", "Form_Insert"): (1.0, "SvcUtils:WorkManagerUrlSvc"),
    ("Bua", "ObtenerBUA"): (4.0, "ws.02 por configuración; ws.01 por hosts/DNS hacia el puerto 80"),
    ("Vigia", "F_VERIFICATERCERO"): (1.0, "URL http en lugar de https"),
    ("Fenix", "ValidarBiometria"): (1.0, "URL con ?WSDL"),
    ("Haberes", "GenerarReporte"): (1.0, "SvcUtils:HaberesUrlSvc"),
    ("TransUnion", "ExecuteXMLString"): (6.0, "URL fija: DNS + certificado TLS de confianza para el host de producción"),
    ("TusDatos", "ConsultarCliente"): (1.0, "BaseUrl y x-api-key ficticia"),
    ("Dodo", "Form_Insert"): (1.0, "BaseUrl"),
    ("KtrionApi", "Autenticacion"): (2.0, "CHKeys:UrlCajaHonor y credenciales ficticias de AppMain"),
    ("Smtp", "EnviarNotificacion"): (2.0, "SMTP:Host/Puerto y plantillas de correo"),
    ("Smtp", "RecuperarContrasena"): (6.0, "DNS de smtp-mail.outlook.com + STARTTLS con certificado"),
    ("ServiceBus", "WorkManagerInsertRequest"): (16.0, "Emulador oficial + SQL Server + proxy TLS + CA local"),
}


def classify(op):
    key = (op["provider"], op["name"])
    if key in SPECIAL:
        return SPECIAL[key]
    if op["provider"] == "Seven":
        return ("Baja", 4, SC["soap_retorno4"], "Pass-through: GA2 reenvía la respuesta. Raíz del XML de ...Result sin confirmar.")
    if op["provider"] == "Dodo":
        return ("Baja", 4, SC["dodo4"], "Pass-through vía DodoController.")
    if op["provider"] == "ServiceBus":
        return ("Media", 4, "1) Mensaje válido con Subject correcto. 2) Payload con request nulo (defecto de GA2). 3) Subject desconocido. 4) Manejador falla → reentrega / dead-letter.", "Validación bloqueada por los defectos D1/D2 de GA2.")
    if op["usage"] == "NotUsed":
        if op["certainty"] == "Validated":
            return ("Baja", 3, SC["wm_wrapped3"], "Envuelta por GA2 sin consumidores: alcance adicional.")
        if op["provider"] == "KtrionApi":
            return ("Adicional", 3, SC["unused_rest3"], "Ruta definida en GA2 sin consumidores: contrato no validado. Alcance adicional.")
        return ("Adicional", 3, SC["unused3"], "En el contrato pero sin llamadas en GA2: contrato de respuesta no validado. Alcance adicional.")
    raise SystemExit(f"Sin regla para {key}")


def hours(op, complexity, mocks):
    h = list(RULES[complexity])
    for idx, extra in EXTRA_PER_MOCK.items():
        h[idx] += extra * (mocks - 3)
    if (op["provider"], op["name"]) == ("TransUnion", "ExecuteXMLString"):
        for i in (0, 1, 2, 3, 4, 6):
            h[i] *= 2
    env = ENV.get((op["provider"], op["name"]))
    if env:
        h[7] += env[0]
    return h


def reference(op):
    p = op["provider"]
    if p == "Seven":
        svc = op["route"].rsplit("/", 1)[-1].split(".")[0]
        code = f"src/GA2.Domain/Core/ReferenciasSvc/Seven/{SEVEN_PROXY[svc]}/Reference.cs"
        if op["name"] in ("InsertarMovContable", "AnularCnMcont"):
            code += "; src/GA2.Domain/Core/Connected Services/ServiceSeven/Reference.cs"
    elif p == "Dodo":
        code = "src/GA2.Domain/Core/" + op["ga2Source"]
    elif p == "Smtp":
        code = SMTP_REF[op["name"]]
    else:
        code = PROXY[p]
    target = f"SOAPAction {op['soapAction']}" if op.get("soapAction") else f"{op['method']} {op['route']}"
    return code, f"{code} | {target} | Contrato: {SPEC[p]}"


# ─── Estilos ────────────────────────────────────────────────────────────────
FONT = "Arial"
F_BASE = Font(name=FONT, size=10)
F_HEAD = Font(name=FONT, size=10, bold=True, color="FFFFFF")
F_TITLE = Font(name=FONT, size=14, bold=True, color="1F3864")
F_SUB = Font(name=FONT, size=10, italic=True, color="595959")
F_BOLD = Font(name=FONT, size=10, bold=True)
F_INPUT = Font(name=FONT, size=10, color="0000FF")
F_LINK = Font(name=FONT, size=10, color="008000")
FILL_HEAD = PatternFill("solid", fgColor="1F3864")
FILL_TOTAL = PatternFill("solid", fgColor="D9E1F2")
FILL_GRAND = PatternFill("solid", fgColor="FFE699")
FILL_INPUT = PatternFill("solid", fgColor="FFF2CC")
THIN = Side(style="thin", color="BFBFBF")
BORDER = Border(left=THIN, right=THIN, top=THIN, bottom=THIN)
WRAP = Alignment(wrap_text=True, vertical="top")
H_FMT = "#,##0.0"
D_FMT = "#,##0.00"

wb = Workbook()
ws_exe = wb.active
ws_exe.title = S_EXE
ws_det = wb.create_sheet(S_DET)
ws_int = wb.create_sheet(S_INT)
ws_sup = wb.create_sheet(S_SUP)
ws_risk = wb.create_sheet(S_RISK)


def header(ws, row, values, widths=None):
    for c, v in enumerate(values, 1):
        cell = ws.cell(row=row, column=c, value=v)
        cell.font, cell.fill, cell.alignment, cell.border = F_HEAD, FILL_HEAD, Alignment(wrap_text=True, vertical="center"), BORDER
    if widths:
        for c, w in enumerate(widths, 1):
            ws.column_dimensions[get_column_letter(c)].width = w


def style_body(ws, r1, r2, c1, c2):
    for r in range(r1, r2 + 1):
        for c in range(c1, c2 + 1):
            cell = ws.cell(row=r, column=c)
            if cell.font == Font() or cell.font.name != FONT:
                cell.font = F_BASE
            cell.alignment, cell.border = WRAP, BORDER


def table(ws, name, ref, style="TableStyleMedium2"):
    t = Table(displayName=name, ref=ref)
    t.tableStyleInfo = TableStyleInfo(name=style, showRowStripes=True)
    ws.add_table(t)


# ─── Hoja 4: Supuestos y criterios ──────────────────────────────────────────
ws = ws_sup
ws["A1"] = "Supuestos y criterios de la estimación desde cero"
ws["A1"].font = F_TITLE
ws["A2"] = "Celdas en azul con fondo amarillo = parámetros editables. Todos los totales del libro se recalculan al cambiarlos."
ws["A2"].font = F_SUB
header(ws, 4, ["Parámetro", "", "Valor", "Unidad", "Uso"], [46, 4, 14, 18, 90])
params = [
    ("Horas por jornada laboral", 8, "horas/día", "Días de esfuerzo = horas totales / horas por jornada (supuesto inicial indicado por el solicitante)."),
    ("Personas asignadas (sólo duración calendario indicativa)", 2, "personas", "No afecta al esfuerzo. Supuesto para la duración calendario del Resumen ejecutivo."),
    ("Dedicación efectiva (sólo duración calendario indicativa)", 0.8, "fracción", "Proporción de la jornada dedicada a este trabajo (reuniones, soporte y bloqueos fuera). No afecta al esfuerzo."),
]
for i, (name, val, unit, use) in enumerate(params):
    r = 5 + i
    ws.cell(row=r, column=1, value=name)
    c = ws.cell(row=r, column=3, value=val)
    c.font, c.fill = F_INPUT, FILL_INPUT
    if unit == "fracción":
        c.number_format = "0%"
    ws.cell(row=r, column=4, value=unit)
    ws.cell(row=r, column=5, value=use)
style_body(ws, 5, 7, 1, 5)
for r in range(5, 8):
    ws.cell(row=r, column=3).font, ws.cell(row=r, column=3).fill = F_INPUT, FILL_INPUT

r = 10
ws.cell(row=r, column=1, value="Regla de horas por actividad (base para 3 mocks)").font = F_BOLD
header(ws, r + 1, ["Complejidad", "", *ACTIVITIES, "Total base (3 mocks)"])
for i, (cx, vals) in enumerate(RULES.items()):
    rr = r + 2 + i
    ws.cell(row=rr, column=1, value=cx)
    for j, v in enumerate(vals):
        cell = ws.cell(row=rr, column=3 + j, value=v)
        cell.number_format = H_FMT
    ws.cell(row=rr, column=11, value=f"=SUM(C{rr}:J{rr})").number_format = H_FMT
style_body(ws, r + 2, r + 1 + len(RULES), 1, 11)
for col in "FGHIJK":
    ws.column_dimensions[col].width = 14
rr = r + 2 + len(RULES)
ws.cell(row=rr, column=1, value="Cada mock adicional sobre 3: +1,0 h de implementación y +0,75 h de pruebas. La configuración del entorno se suma al método principal de cada proveedor (detalle en la columna 'Supuestos y riesgos'). TransUnion ExecuteXMLString duplica la base de 'Muy alta' (excepción documentada).").alignment = WRAP
ws.merge_cells(start_row=rr, start_column=1, end_row=rr, end_column=11)
ws.row_dimensions[rr].height = 42
ws.cell(row=rr + 1, column=1, value="Las horas por actividad de la hoja 'Detalle por método' son valores de entrada calculados con esta regla al generar el libro. Si se cambian aquí, no se propagan solas: edite directamente las horas del método (los totales sí se recalculan).").alignment = WRAP
ws.merge_cells(start_row=rr + 1, start_column=1, end_row=rr + 1, end_column=11)
ws.row_dimensions[rr + 1].height = 30

r = rr + 3
ws.cell(row=r, column=1, value="Criterios").font = F_BOLD
header(ws, r + 1, ["Tema", "", "Criterio"])
ws.merge_cells(start_row=r + 1, start_column=3, end_row=r + 1, end_column=11)
criteria = [
    ("Qué es un mock", "Un comportamiento simulado del proveedor para un método: la respuesta (o el error) que el sistema real daría ante un tipo de entrada. NO es una prueba automatizada: las pruebas validan esos comportamientos y se estiman aparte, en 'Pruebas automatizadas'."),
    ("Selección de 3, 4 o 5 mocks", "3: éxito, error funcional y entrada inválida. 4: además un caso límite, respuesta vacía o recurso no encontrado. 5: además un error técnico, timeout, indisponibilidad o escenario alternativo relevante. Se adaptan al comportamiento real (autenticación, pasos, estados, OTP, procesos asíncronos)."),
    ("Excepción al rango", "TransUnion ExecuteXMLString es un único método con 6 pasos lógicos y ≈9 variantes: se mantiene en 5 mocks agrupando variantes por categoría y se compensa duplicando las horas base."),
    ("Complejidad: Adicional", "Operación del contrato que GA2 no invoca y sin contrato de respuesta validado. Se estima para que el equipo vea el esfuerzo adicional; no es parte del alcance principal."),
    ("Complejidad: Baja", "Respuesta simple o pass-through, un DTO pequeño, sin estado ni lógica condicional."),
    ("Complejidad: Media", "DTO medianos o grandes, códigos de negocio (RETORNO, Success), consecutivos dinámicos o varias ramas de error."),
    ("Complejidad: Alta", "Formatos delicados (RPC/encoded, DataSet .NET, XML minificado), autenticación con token, estado asíncrono, o lógica que GA2 interpreta con fragilidad."),
    ("Complejidad: Muy alta", "Varios clientes con tipos distintos, protocolos no HTTP (servidor SMTP) o flujos con estado de varios pasos."),
    ("Estimación por actividad", "Análisis del contrato y del protocolo; diseño de escenarios; implementación de respuestas, estados, autenticación y comportamiento dinámico; pruebas automatizadas; validación con los clientes reales de GA2; documentación Swagger/OpenAPI y ejemplos; depuración, ajustes e integración; configuración del entorno."),
    ("Alcance incluido", "Las 153 operaciones del catálogo: 34 usadas por la lógica de negocio de GA2, 53 expuestas como pass-through y 66 adicionales de los contratos (sin uso en GA2), separadas en los resúmenes. Además, los componentes transversales necesarios para entregar la solución (hoja 'Resumen por integración')."),
    ("Alcance excluido", "Integraciones registradas sin ningún uso ni mock (OTP WsUtilidades, Vigía ASMX de carga y verificación v3, pasarela para BUA/Vigía/Fénix, ProviderRequestG2, EmailRepository). Corrección de defectos de GA2. Base de datos de pruebas de GA2. Gestión, reuniones, revisiones de código de terceros y contingencia."),
    ("Base de la estimación", "Desde cero: no se descuenta el código, las pruebas ni la documentación existentes en MockIntegrations. El inventario existente sólo se usa para conocer los contratos y la complejidad."),
    ("Supuestos técnicos", "Un desarrollador con experiencia en .NET, WCF/SOAP y ASP.NET Core; acceso al código de GA2 para compilar sus proxies y DTO; las fichas de contrato de docs/integraciones/mocks como insumo; Docker disponible para el emulador de Service Bus."),
    ("Días-persona", "La suma de días es esfuerzo (días-persona), no duración calendario. La duración calendario del Resumen ejecutivo es indicativa y depende de personas, dedicación, paralelismo y bloqueos."),
    ("Factores que pueden modificar los tiempos", "Confirmación de contratos (Seven, Vigía, TransUnion, TusDatos, Service Bus, WorkManager); disponibilidad de una base de pruebas de GA2; corrección de los defectos de Service Bus; URLs fijas en el código (TransUnion, BUA ws.01, SMTP Outlook); diferencias DEBUG/Release; experiencia del equipo con WCF y SOAP RPC/encoded."),
]
for i, (k, v) in enumerate(criteria):
    rr = r + 2 + i
    ws.cell(row=rr, column=1, value=k).font = F_BOLD
    ws.cell(row=rr, column=3, value=v)
    ws.merge_cells(start_row=rr, start_column=3, end_row=rr, end_column=11)
    ws.row_dimensions[rr].height = 44
    for c in range(1, 12):
        ws.cell(row=rr, column=c).alignment, ws.cell(row=rr, column=c).border = WRAP, BORDER
ws.freeze_panes = "A5"

# ─── Hoja 2: Detalle por método ─────────────────────────────────────────────
ws = ws_det
ws["A1"] = "Detalle por método — esfuerzo desde cero"
ws["A1"].font = F_TITLE
ws["A2"] = "Una fila por método (153). Horas por actividad = valores de entrada (azul); Total y Días = fórmulas. Días = Total / horas por jornada (hoja Supuestos)."
ws["A2"].font = F_SUB
cols = ["ID", "Proveedor", "Método", "Protocolo", "Uso actual en GA2", "Complejidad", "Cantidad de mocks", "Descripción de los escenarios",
        "Horas de análisis", "Horas de diseño", "Horas de implementación", "Horas de pruebas automatizadas", "Horas de validación con GA2",
        "Horas de documentación", "Horas de ajustes e integración", "Horas de configuración del entorno", "Total de horas desde cero",
        "Días de esfuerzo desde cero", "Referencia al código o contrato", "Supuestos y riesgos", "Estado del contrato"]
widths = [9, 20, 30, 16, 24, 12, 10, 60, 10, 10, 12, 12, 12, 12, 12, 12, 12, 12, 60, 55, 26]
HR = 4
header(ws, HR, cols, widths)
ws.row_dimensions[HR].height = 45

ops = sorted(catalog, key=lambda o: (ORDER.index(o["provider"]), {"Business": 0, "PassThrough": 1, "NotUsed": 2}[o["usage"]], o["name"]))
rows, refs = [], []
for n, op in enumerate(ops, 1):
    cx, mocks, scen, note = classify(op)
    h = hours(op, cx, mocks)
    code, ref = reference(op)
    refs.append(code)
    risk = note
    env = ENV.get((op["provider"], op["name"]))
    if env:
        risk += f" Entorno: {env[1]} ({env[0]:g} h)."
    if op["certainty"] == "StructureOnly" and "confirm" not in risk:
        risk += " Contenido de negocio por confirmar con el proveedor."
    rows.append((f"M{n:03d}", DISPLAY[op["provider"]], op["name"], PROTOCOL[op["protocol"]], USAGE[op["usage"]], cx, mocks, scen, *h, ref, risk, CERTAINTY[op["certainty"]]))

first = HR + 1
for i, row in enumerate(rows):
    r = first + i
    for c, v in enumerate(row[:16], 1):
        ws.cell(row=r, column=c, value=v)
    for c in range(9, 17):
        ws.cell(row=r, column=c).number_format = H_FMT
        ws.cell(row=r, column=c).font = F_INPUT
    ws.cell(row=r, column=7).font = F_INPUT
    ws.cell(row=r, column=17, value=f"=SUM(I{r}:P{r})").number_format = H_FMT
    ws.cell(row=r, column=18, value=f"=Q{r}/{JORNADA}").number_format = D_FMT
    ws.cell(row=r, column=19, value=row[16])
    ws.cell(row=r, column=20, value=row[17])
    ws.cell(row=r, column=21, value=row[18])
last = first + len(rows) - 1
style_body(ws, first, last, 1, 21)
for r in range(first, last + 1):
    for c in list(range(9, 17)) + [7]:
        ws.cell(row=r, column=c).font = F_INPUT
    ws.cell(row=r, column=17).font = F_BOLD
table(ws, "DetalleMetodos", f"A{HR}:U{last}")
tot = last + 2
ws.cell(row=tot, column=2, value="TOTAL GENERAL (métodos)").font = F_BOLD
ws.cell(row=tot, column=3, value=f"=COUNTA(C{first}:C{last})")
ws.cell(row=tot, column=7, value=f"=SUM(G{first}:G{last})")
for c in range(9, 19):
    col = get_column_letter(c)
    ws.cell(row=tot, column=c, value=f"=SUM({col}{first}:{col}{last})").number_format = D_FMT if c == 18 else H_FMT
for c in range(1, 22):
    ws.cell(row=tot, column=c).fill, ws.cell(row=tot, column=c).font, ws.cell(row=tot, column=c).border = FILL_GRAND, F_BOLD, BORDER
ws.freeze_panes = ws.cell(row=HR + 1, column=4)
ws.auto_filter.ref = None

CX_COLORS = {"Adicional": "E7E6E6", "Baja": "E2EFDA", "Media": "FFF2CC", "Alta": "FCE4D6", "Muy alta": "F8CBAD"}
for cx, color in CX_COLORS.items():
    ws.conditional_formatting.add(f"F{first}:F{last}", CellIsRule(operator="equal", formula=[f'"{cx}"'], fill=PatternFill("solid", fgColor=color)))
for text, color in [("Validado con el código de GA2", "E2EFDA"), ("Estructura validada; contenido por confirmar", "FFF2CC"), ("Sin contrato de respuesta validado", "F8CBAD")]:
    ws.conditional_formatting.add(f"U{first}:U{last}", CellIsRule(operator="equal", formula=[f'"{text}"'], fill=PatternFill("solid", fgColor=color)))
ws.conditional_formatting.add(f"E{first}:E{last}", FormulaRule(formula=[f'LEFT($E{first},2)="No"'], font=Font(name=FONT, color="7F7F7F", italic=True)))

DET = f"'{S_DET}'"
RNG = lambda col: f"{DET}!${col}${first}:${col}${last}"

# ─── Hoja 3: Resumen por integración ────────────────────────────────────────
ws = ws_int
ws["A1"] = "Resumen por integración — esfuerzo desde cero"
ws["A1"].font = F_TITLE
ws["A2"] = "Todas las cifras son fórmulas sobre la hoja 'Detalle por método'. Mín./máx. de mocks = métodos × 3 / × 5; 'Mocks estimados' = suma de la cantidad por método."
ws["A2"].font = F_SUB
icols = ["Proveedor", "Protocolo", "Métodos", "Métodos usados por GA2", "Métodos adicionales (sin uso)", "Mocks mínimos (×3)", "Mocks máximos (×5)",
         "Mocks estimados", "Horas totales desde cero", "Horas – alcance principal", "Horas – alcance adicional", "Días de esfuerzo desde cero",
         "Complejidad predominante", "Métodos más complejos", "Dependencias y riesgos", "Conclusión"]
header(ws, HR, icols, [24, 22, 9, 11, 12, 10, 10, 10, 12, 12, 12, 12, 14, 40, 60, 75])
ws.row_dimensions[HR].height = 45
RISKS = {
    "Bua": "ws.01 con URL fija (hosts/DNS). Un XML para dos proxies con tipos distintos. Flujo ws.02 depende del interruptor 'Bua' vía API GA2.",
    "Fenix": "DataSet .NET: GA2 lee Nodes[1]. Interruptor FENIX en BD.",
    "Seven": "Raíz del XML de ...Result y semántica de GetCuentaContable/GetAporte sin confirmar. Flujo de pago completo requiere BD de pruebas.",
    "Vigia": "RPC/encoded. Dominio de Encontrado/Listas/Metodo sin confirmar. Interruptor VIGIA en BD.",
    "TransUnion": "URL de producción fija en código (DNS + TLS). Sin muestras reales de DCResponse. 6 pasos con estado.",
    "Haberes": "DataSet minificado. En DEBUG GenerarReporte va por la pasarela. PDF y Blob en GA2.",
    "WorkManager": "Formato real del BarCode sin confirmar. 22 operaciones sin uso en GA2. DEBUG vs Release.",
    "TusDatos": "Dominios de statusJobTusDatos y riskLAFT sin confirmar. Estado asíncrono en memoria.",
    "Dodo": "Sólo pass-through en GA2 (DodoController); migración en curso desde WorkManager SOAP.",
    "KtrionApi": "Rutas sólo en DEBUG; 6 rutas sin consumidores con contrato no validado. GA2 acepta cualquier certificado.",
    "Smtp": "SMTP de Outlook fijo en código con STARTTLS. Plantillas de correo en disco.",
    "ServiceBus": "SDK 7.9 sin soporte del emulador (proxy TLS). Manejadores sin registrar en DI y payload nulo (D1/D2) bloquean la validación funcional. Productor real desconocido.",
}
CX_RANK = {"Adicional": 0, "Baja": 1, "Media": 2, "Alta": 3, "Muy alta": 4}
providers = [p for p in ORDER]
for i, p in enumerate(providers):
    r = first + i
    name = DISPLAY[p]
    prows = [row for row in rows if row[1] == name]
    counts = {}
    for row in prows:
        counts[row[5]] = counts.get(row[5], 0) + 1
    predominant = max(counts.items(), key=lambda kv: (kv[1], CX_RANK[kv[0]]))[0]
    top = sorted(prows, key=lambda row: (-CX_RANK[row[5]], -sum(row[8:16])))[:3]
    ws.cell(row=r, column=1, value=name)
    ws.cell(row=r, column=2, value=", ".join(sorted({row[3] for row in prows})))
    ws.cell(row=r, column=3, value=f'=COUNTIF({RNG("B")},$A{r})')
    ws.cell(row=r, column=4, value=f'=COUNTIFS({RNG("B")},$A{r},{RNG("E")},"Sí*")')
    ws.cell(row=r, column=5, value=f'=COUNTIFS({RNG("B")},$A{r},{RNG("E")},"No*")')
    ws.cell(row=r, column=6, value=f"=C{r}*3")
    ws.cell(row=r, column=7, value=f"=C{r}*5")
    ws.cell(row=r, column=8, value=f'=SUMIFS({RNG("G")},{RNG("B")},$A{r})')
    ws.cell(row=r, column=9, value=f'=SUMIFS({RNG("Q")},{RNG("B")},$A{r})').number_format = H_FMT
    ws.cell(row=r, column=10, value=f'=SUMIFS({RNG("Q")},{RNG("B")},$A{r},{RNG("E")},"Sí*")').number_format = H_FMT
    ws.cell(row=r, column=11, value=f'=SUMIFS({RNG("Q")},{RNG("B")},$A{r},{RNG("E")},"No*")').number_format = H_FMT
    ws.cell(row=r, column=12, value=f"=I{r}/{JORNADA}").number_format = D_FMT
    ws.cell(row=r, column=13, value=predominant)
    ws.cell(row=r, column=14, value="; ".join(f"{row[2]} ({row[5]})" for row in top))
    ws.cell(row=r, column=15, value=RISKS[p])
    ws.cell(row=r, column=16, value=f'=A{r}&" tiene "&C{r}&" métodos identificados ("&D{r}&" usados por GA2). Se estima construir entre "&F{r}&" y "&G{r}&" mocks ("&H{r}&" según el detalle), con un esfuerzo de "&TEXT(I{r},"0.0")&" horas, equivalente a "&TEXT(L{r},"0.0")&" días laborables desde cero."')
plast = first + len(providers) - 1
style_body(ws, first, plast, 1, 16)
table(ws, "ResumenIntegracion", f"A{HR}:P{plast}", "TableStyleMedium9")
pt = plast + 1
ws.cell(row=pt, column=1, value="Total métodos (suma de proveedores)")
for c in range(3, 13):
    col = get_column_letter(c)
    ws.cell(row=pt, column=c, value=f"=SUM({col}{first}:{col}{plast})").number_format = D_FMT if c == 12 else (H_FMT if c >= 9 else "0")
for c in range(1, 17):
    ws.cell(row=pt, column=c).fill, ws.cell(row=pt, column=c).font, ws.cell(row=pt, column=c).border = FILL_TOTAL, F_BOLD, BORDER

# Componentes transversales (no son métodos: se suman aparte al total general).
tr0 = pt + 3
ws.cell(row=tr0 - 1, column=1, value="Componentes transversales necesarios para entregar la solución desde cero (no son métodos; no cuentan mocks)").font = F_BOLD
header_row = tr0
for c, v in enumerate(["Componente", "Descripción", "Horas"], 1):
    cell = ws.cell(row=header_row, column=c, value=v)
    cell.font, cell.fill, cell.border = F_HEAD, FILL_HEAD, BORDER
ws.merge_cells(start_row=header_row, start_column=2, end_row=header_row, end_column=8)
TRANSVERSAL = [
    ("Estructura de la solución y host", "Solución .NET, proyecto ASP.NET Core, configuración por appsettings/variables, Kestrel, health.", 8),
    ("Despachador SOAP común", "Enrutado por SOAPAction, parseo del sobre, SOAP Fault, HTTP 500 HTML, página de ayuda por servicio.", 16),
    ("Fixtures y respuestas dinámicas", "Almacén de fixtures embebidos/override, motor de marcadores (valores del request, consecutivos, fechas), herramienta de generación desde las fichas.", 12),
    ("Motor de escenarios y administración", "Selección determinista (header, override, reglas por dato, defaults), latencia, API /_mock, registro de llamadas, reinicio de estado.", 16),
    ("OpenAPI / Swagger UI", "Un documento por proveedor, documentación de endpoints SOAP con ejemplos por operación, Swagger UI.", 16),
    ("Arnés de pruebas de compatibilidad", "Servidor de pruebas en Kestrel; enlace de proxies WCF, DTO y clientes reales de GA2; utilidades de invocación.", 16),
    ("Contenedores", "Dockerfile, docker-compose (mock, emulador, SQL), variables de entorno.", 8),
    ("Perfil de configuración de GA2", "appsettings.Mocks.json (GA2, BUA), perfiles de arranque, scripts de variables de entorno.", 8),
    ("Prueba de humo GA2 → mocks", "Compilación de GA2 (.NET 5), base stub, script de humo y verificación de extremo a extremo.", 12),
    ("Documentación general", "Guías de ejecución, configuración de GA2, OpenAPI, escenarios, inventario, matriz y pendientes.", 16),
]
for i, (k, d, h) in enumerate(TRANSVERSAL):
    rr = tr0 + 1 + i
    ws.cell(row=rr, column=1, value=k)
    ws.cell(row=rr, column=2, value=d)
    ws.merge_cells(start_row=rr, start_column=2, end_row=rr, end_column=8)
    c = ws.cell(row=rr, column=9, value=h)
    c.font, c.number_format = F_INPUT, H_FMT
    ws.cell(row=rr, column=12, value=f"=I{rr}/{JORNADA}").number_format = D_FMT
trl = tr0 + len(TRANSVERSAL)
style_body(ws, tr0 + 1, trl, 1, 12)
for rr in range(tr0 + 1, trl + 1):
    ws.cell(row=rr, column=9).font = F_INPUT
tt = trl + 1
ws.cell(row=tt, column=1, value="Total transversal")
ws.cell(row=tt, column=9, value=f"=SUM(I{tr0 + 1}:I{trl})").number_format = H_FMT
ws.cell(row=tt, column=12, value=f"=I{tt}/{JORNADA}").number_format = D_FMT
for c in range(1, 17):
    ws.cell(row=tt, column=c).fill, ws.cell(row=tt, column=c).font, ws.cell(row=tt, column=c).border = FILL_TOTAL, F_BOLD, BORDER
gt = tt + 2
ws.cell(row=gt, column=1, value="TOTAL GENERAL (proveedores + transversal)")
ws.cell(row=gt, column=3, value=f"=C{pt}")
ws.cell(row=gt, column=6, value=f"=F{pt}")
ws.cell(row=gt, column=7, value=f"=G{pt}")
ws.cell(row=gt, column=8, value=f"=H{pt}")
ws.cell(row=gt, column=9, value=f"=I{pt}+I{tt}").number_format = H_FMT
ws.cell(row=gt, column=12, value=f"=I{gt}/{JORNADA}").number_format = D_FMT
for c in range(1, 17):
    ws.cell(row=gt, column=c).fill, ws.cell(row=gt, column=c).font, ws.cell(row=gt, column=c).border = FILL_GRAND, F_BOLD, BORDER
ck = gt + 2
ws.cell(row=ck, column=1, value="Controles de coherencia").font = F_BOLD
checks = [
    ("Métodos: suma por proveedor = filas del detalle", f'=IF(C{pt}=COUNTA({RNG("C")}),"OK","REVISAR")'),
    ("Horas: suma por proveedor = suma del detalle", f'=IF(ABS(I{pt}-SUM({RNG("Q")}))<0.001,"OK","REVISAR")'),
    ("Horas: principal + adicional = total", f'=IF(ABS(J{pt}+K{pt}-I{pt})<0.001,"OK","REVISAR")'),
    ("Métodos: usados + adicionales = total", f'=IF(D{pt}+E{pt}=C{pt},"OK","REVISAR")'),
    ("Sin métodos duplicados (proveedor + método)", f'=IF(SUMPRODUCT(({RNG("B")}&"|"&{RNG("C")}<>"")/COUNTIFS({RNG("B")},{RNG("B")},{RNG("C")},{RNG("C")}))=COUNTA({RNG("C")}),"OK","REVISAR")'),
    ("Mocks por método dentro de 3–5", f'=IF(AND(MIN({RNG("G")})>=3,MAX({RNG("G")})<=5),"OK","REVISAR")'),
]
for i, (k, f) in enumerate(checks):
    rr = ck + 1 + i
    ws.cell(row=rr, column=1, value=k)
    ws.merge_cells(start_row=rr, start_column=1, end_row=rr, end_column=8)
    ws.cell(row=rr, column=9, value=f).font = F_BOLD
style_body(ws, ck + 1, ck + len(checks), 1, 9)
ws.conditional_formatting.add(f"I{ck + 1}:I{ck + len(checks)}", CellIsRule(operator="equal", formula=['"OK"'], fill=PatternFill("solid", fgColor="C6EFCE")))
ws.conditional_formatting.add(f"I{ck + 1}:I{ck + len(checks)}", CellIsRule(operator="equal", formula=['"REVISAR"'], fill=PatternFill("solid", fgColor="FFC7CE")))
for cx, color in CX_COLORS.items():
    ws.conditional_formatting.add(f"M{first}:M{plast}", CellIsRule(operator="equal", formula=[f'"{cx}"'], fill=PatternFill("solid", fgColor=color)))
ws.freeze_panes = ws.cell(row=HR + 1, column=2)
INT = f"'{S_INT}'"

# ─── Hoja 1: Resumen ejecutivo ──────────────────────────────────────────────
ws = ws_exe
ws["A1"] = "Estimación desde cero de los mocks de integraciones de GA2"
ws["A1"].font = F_TITLE
ws["A2"] = "Esfuerzo para construir los mocks como si no existiera implementación previa. Fuente del inventario: código de GA2, fichas docs/integraciones/mocks y catálogo de MockIntegrations (153 operaciones)."
ws["A2"].font = F_SUB
ws.merge_cells("A2:H2")
ws.column_dimensions["A"].width = 52
for col, w in zip("BCDEFGH", [16, 14, 14, 14, 14, 14, 60]):
    ws.column_dimensions[col].width = w
header(ws, 4, ["Indicador", "Valor", "Nota"])
ws.merge_cells("C4:H4")
kpis = [
    ("Número total de proveedores", f"=COUNTA({INT}!A{first}:A{plast})", "0", "Proveedores con al menos un método."),
    ("Número total de métodos identificados", f"={INT}!C{pt}", "0", "Una fila por método en 'Detalle por método'."),
    ("   · usados por GA2 (negocio + pass-through)", f"={INT}!D{pt}", "0", "Alcance principal."),
    ("   · adicionales de los contratos (sin uso en GA2)", f"={INT}!E{pt}", "0", "Alcance adicional, separado para decidir si se incluye."),
    ("Total mínimo de mocks (métodos × 3)", f"={INT}!F{pt}", "#,##0", ""),
    ("Total máximo de mocks (métodos × 5)", f"={INT}!G{pt}", "#,##0", ""),
    ("Mocks estimados según el detalle (3–5 por método)", f"={INT}!H{pt}", "#,##0", "Suma de la cantidad asignada a cada método."),
    ("Horas de los métodos – alcance principal", f"={INT}!J{pt}", H_FMT, ""),
    ("Horas de los métodos – alcance adicional", f"={INT}!K{pt}", H_FMT, ""),
    ("Horas de componentes transversales", f"={INT}!I{tt}", H_FMT, "Infraestructura común: despachador SOAP, escenarios, OpenAPI, arnés de pruebas, contenedores…"),
    ("TOTAL DE HORAS ESTIMADAS DESDE CERO", f"={INT}!I{gt}", H_FMT, "Métodos (principal + adicional) + transversal."),
    ("TOTAL DE DÍAS LABORABLES DE ESFUERZO (días-persona)", f"={INT}!L{gt}", D_FMT, "Horas totales / horas por jornada (Supuestos). Esfuerzo, no duración calendario."),
    ("Días-persona sólo del alcance principal + transversal", f"=({INT}!J{pt}+{INT}!I{tt})/{JORNADA}", D_FMT, "Si se excluyen las operaciones sin uso en GA2."),
    ("Duración calendario indicativa (días hábiles)", f"=B16/({PERSONAS}*{EFICIENCIA})", D_FMT, "Supuesto: personas y dedicación de la hoja Supuestos, trabajo paralelizable por proveedor y sin bloqueos externos. No es un compromiso de fecha."),
]
for i, (k, f, fmt, note) in enumerate(kpis):
    r = 5 + i
    ws.cell(row=r, column=1, value=k)
    c = ws.cell(row=r, column=2, value=f)
    c.number_format, c.font = fmt, F_LINK
    ws.cell(row=r, column=3, value=note)
    ws.merge_cells(start_row=r, start_column=3, end_row=r, end_column=8)
klast = 4 + len(kpis)
style_body(ws, 5, klast, 1, 8)
for r in range(5, klast + 1):
    ws.cell(row=r, column=2).font = F_LINK
for r in (15, 16):
    for c in range(1, 9):
        ws.cell(row=r, column=c).fill = FILL_GRAND
        ws.cell(row=r, column=c).font = Font(name=FONT, size=10, bold=True, color="008000" if c == 2 else "000000")
assert ws.cell(row=16, column=1).value.startswith("TOTAL DE DÍAS")

t0 = klast + 3
ws.cell(row=t0 - 1, column=1, value="Resumen por proveedor").font = F_BOLD
header(ws, t0, ["Proveedor", "Métodos", "Mocks mín.", "Mocks máx.", "Horas", "Días-persona", "% del total", "Principales riesgos"])
for i in range(len(providers)):
    r = t0 + 1 + i
    src = first + i
    for c, col in zip(range(1, 7), ["A", "C", "F", "G", "I", "L"]):
        cell = ws.cell(row=r, column=c, value=f"={INT}!{col}{src}")
        cell.font = F_LINK
    ws.cell(row=r, column=5).number_format = H_FMT
    ws.cell(row=r, column=6).number_format = D_FMT
    ws.cell(row=r, column=7, value=f"=E{r}/{INT}!$I${gt}").number_format = "0.0%"
    ws.cell(row=r, column=8, value=f"={INT}!O{src}").font = F_LINK
tl = t0 + len(providers)
style_body(ws, t0 + 1, tl, 1, 8)
for r in range(t0 + 1, tl + 1):
    for c in (1, 2, 3, 4, 5, 6, 8):
        ws.cell(row=r, column=c).font = F_LINK
table(ws, "ResumenProveedores", f"A{t0}:H{tl}", "TableStyleMedium2")
ttr = tl + 1
ws.cell(row=ttr, column=1, value="Componentes transversales")
ws.cell(row=ttr, column=5, value=f"={INT}!I{tt}").number_format = H_FMT
ws.cell(row=ttr, column=6, value=f"={INT}!L{tt}").number_format = D_FMT
ws.cell(row=ttr, column=7, value=f"=E{ttr}/{INT}!$I${gt}").number_format = "0.0%"
gtr = ttr + 1
ws.cell(row=gtr, column=1, value="TOTAL GENERAL")
for c, f in [(2, f"=SUM(B{t0 + 1}:B{tl})"), (3, f"=SUM(C{t0 + 1}:C{tl})"), (4, f"=SUM(D{t0 + 1}:D{tl})"), (5, f"=SUM(E{t0 + 1}:E{ttr})"), (6, f"=SUM(F{t0 + 1}:F{ttr})"), (7, f"=SUM(G{t0 + 1}:G{ttr})")]:
    ws.cell(row=gtr, column=c, value=f)
ws.cell(row=gtr, column=5).number_format = H_FMT
ws.cell(row=gtr, column=6).number_format = D_FMT
ws.cell(row=gtr, column=7).number_format = "0.0%"
for r, fill in ((ttr, FILL_TOTAL), (gtr, FILL_GRAND)):
    for c in range(1, 9):
        ws.cell(row=r, column=c).fill, ws.cell(row=r, column=c).font, ws.cell(row=r, column=c).border = fill, F_BOLD, BORDER

m0 = gtr + 3
ws.cell(row=m0 - 1, column=1, value="Proveedores con mayor esfuerzo").font = F_BOLD
header(ws, m0, ["Posición", "Proveedor", "Horas", "Días-persona"])
HRS = f"$E${t0 + 1}:$E${tl}"
for k in range(1, 4):
    r = m0 + k
    ws.cell(row=r, column=1, value=k)
    ws.cell(row=r, column=2, value=f"=INDEX($A${t0 + 1}:$A${tl},MATCH(LARGE({HRS},A{r}),{HRS},0))")
    ws.cell(row=r, column=3, value=f"=LARGE({HRS},A{r})").number_format = H_FMT
    ws.cell(row=r, column=4, value=f"=C{r}/{JORNADA}").number_format = D_FMT
style_body(ws, m0 + 1, m0 + 3, 1, 4)

chart = BarChart()
chart.type = "bar"
chart.style = 10
chart.title = "Días-persona estimados por proveedor (desde cero)"
chart.y_axis.title = "Días-persona"
chart.x_axis.title = None
chart.add_data(Reference(ws, min_col=6, min_row=t0, max_row=tl), titles_from_data=True)
chart.set_categories(Reference(ws, min_col=1, min_row=t0 + 1, max_row=tl))
chart.legend = None
chart.height, chart.width = 11, 22
from openpyxl.chart.label import DataLabelList
chart.dataLabels = DataLabelList()
chart.dataLabels.showVal = True
chart.dataLabels.showSerName = False
chart.dataLabels.showCatName = False
chart.dataLabels.showLegendKey = False
chart.dataLabels.numFmt = "0.0"
ws.add_chart(chart, f"A{m0 + 6}")
ws.freeze_panes = "A5"

# ─── Hoja 5: Pendientes y riesgos ───────────────────────────────────────────
ws = ws_risk
ws["A1"] = "Pendientes y riesgos que afectan la construcción desde cero"
ws["A1"].font = F_TITLE
ws["A2"] = "Fuente: MockIntegrations/docs/pendientes.md y verificaciones ejecutadas. No se asignan responsables ni plazos de terceros."
ws["A2"].font = F_SUB
rcols = ["ID", "Tipo", "Descripción", "Proveedor afectado", "Impacto en el esfuerzo", "Bloquea", "Acción requerida", "Dependencias conocidas"]
header(ws, HR, rcols, [7, 18, 60, 20, 45, 14, 50, 45])
RISKS_ROWS = [
    ("C1", "Contrato no confirmado", "Nombre real de la raíz de cada ...Result y semántica de GetCuentaContable, GetCuentaContableTO y GetAporte.", "Seven ERP", "Bajo: GA2 no valida la raíz; ajustar 3 mocks si el texto real difiere (+2–4 h).", "Validación", "Obtener muestras reales de la auditoría de EjecutarServicio.", "Ambiente con Seven real y tabla de auditoría."),
    ("C2", "Contrato no confirmado", "Dominio de Encontrado, Listas, Metodo y Pe_TipoVerificacion.", "Vigía", "Bajo: cambiar valores de los mocks (+1–2 h).", "Validación", "Confirmar valores con el equipo de Vigía.", "Equipo de Vigía."),
    ("C3", "Contrato no confirmado", "DCResponse reales por paso (IDVision, OTP, AQM, historial) y DSOnlineBureauCOLData completo.", "TransUnion", "Alto: rediseño parcial de respuestas y pruebas (+8–16 h).", "Validación", "Extraer muestras de RegistroLogTUSimulador, VLL_VALIDACION_LOG_OTP o del Blob trans-union.", "Acceso a la BD y al Blob de un ambiente con TransUnion."),
    ("C4", "Contrato no confirmado", "Valores de statusJobTusDatos y riskLAFT; código real ante API key inválida (401/403).", "TusDatos", "Bajo (+1–2 h).", "Validación", "Confirmar con el equipo del API Gateway.", "Equipo del API Gateway."),
    ("C5", "Contrato no confirmado", "Productor real del topic chonorintegraciones y formato real de los mensajes.", "Azure Service Bus", "Medio: el formato se deduce del código (+4–8 h si difiere).", "Validación", "Identificar el productor y obtener mensajes reales.", "Equipo de integración."),
    ("C6", "Contrato no confirmado", "Formato real del consecutivo de BarCode.", "WorkManager / DODO (SOAP)", "Bajo (+1 h).", "Ninguno", "Revisar radicados reales.", "Datos de un ambiente real."),
    ("C7", "Operaciones sin consumidor", "Rutas de la pasarela sin uso (formdelete, formgetdata, formgetfilecodes, formgetbase64stringfile, listgetitems, obtenerafiliadobua).", "Pasarela ktrionApi", "Incluidas como alcance adicional; el contrato de respuesta no está validado.", "Ninguno", "Decidir si se incluyen en el alcance.", "—"),
    ("C8", "Operaciones sin consumidor", "66 operaciones de los contratos sin uso en GA2 (17 WorkManager, 32 TransUnion, 5 Haberes, 6 pasarela, xxValidarBiometria…).", "Varios", "Alcance adicional estimado por separado (ver Resumen ejecutivo).", "Ninguno", "Decidir si entran en el alcance; sin ellas el total baja al alcance principal.", "—"),
    ("V1", "Verificación incompleta", "Flujos de negocio completos de GA2 (radicación, pago de crédito, prospección): cada uno consulta la BD antes de llamar a la integración.", "Varios", "La validación con GA2 queda limitada a endpoints puntuales sobre una base stub.", "Validación", "Proveer una copia (anonimizada) de la base de pruebas de GA2.", "Equipo de base de datos de GA2."),
    ("V2", "URL fija en código", "BUA ws.01 (http://cpvmp-09x/...) y TransUnion (producción) no se pueden redirigir por configuración.", "BUA; TransUnion", "Incluido en 'Configuración del entorno' (4 h BUA, 6 h TransUnion). Riesgo de llamar a TransUnion producción.", "Validación", "Hosts/DNS + certificado TLS, o hacer configurables las URLs (cambio de código).", "Decisión del equipo de GA2."),
    ("V3", "Defecto de GA2", "Los 5 manejadores de eventos no están registrados en DI: GA2 completa y descarta cada mensaje.", "Azure Service Bus", "La validación funcional del canal no es posible; el esfuerzo de construcción no cambia.", "Validación", "Corregir el registro en DI de GA2 (fuera del alcance de los mocks).", "Equipo de GA2."),
    ("V4", "Defecto de GA2", "System.Text.Json ignora el campo público 'request' de los proxies WCF: el payload llega nulo.", "Azure Service Bus", "Igual que V3.", "Validación", "Corregir la serialización en GA2.", "Equipo de GA2."),
    ("V5", "Dependencia técnica", "Azure.Messaging.ServiceBus 7.9.0 no soporta el emulador: requiere proxy TLS y CA local.", "Azure Service Bus", "Incluido: 16 h de entorno en WorkManagerInsertRequest.", "Desarrollo", "Mantener el proxy TLS o actualizar el paquete a ≥ 7.18.", "Docker; decisión del equipo de GA2."),
    ("V6", "SMTP", "SMTP de Identity (Outlook) y SmtpTestController con host fijo; Outlook exige STARTTLS.", "SMTP / correo", "RecuperarContrasena estimada como Alta con 6 h de entorno; hoy no simulada.", "Validación", "Redirección DNS + STARTTLS con certificado, o hacer configurable el host.", "Decisión del equipo de GA2."),
    ("R1", "Riesgo de entorno", "Timeouts infinitos (TimeSpan.MaxValue) en los clientes SOAP: un mock lento o detenido cuelga las peticiones de GA2.", "Varios (SOAP)", "Pruebas con timeout explícito; depuración más lenta (+0,5–1 h por proveedor SOAP).", "Ninguno", "Usar timeouts acotados en las pruebas.", "—"),
    ("R2", "Riesgo de entorno", "Diferencias DEBUG/Release: radicación de crédito y GenerarReporte van por la pasarela en DEBUG y por SOAP en Release.", "WorkManager; Haberes; Pasarela ktrionApi", "Obliga a simular y validar ambos caminos (ya incluido en el inventario).", "Ninguno", "Definir qué configuración de compilación usa cada entorno.", "Equipo de GA2."),
    ("R3", "Riesgo de entorno", "Estado en memoria (consecutivos, jobs, sesiones TransUnion, tokens): se pierde al reiniciar el mock.", "Varios", "Bajo: reinicio determinista; persistencia no incluida.", "Ninguno", "Reiniciar el estado antes de cada suite.", "—"),
    ("R4", "Riesgo de entorno", "Emulador de Service Bus dependiente de su SQL Server; recrear el SQL con el emulador en marcha lo deja colgado.", "Azure Service Bus", "+1–2 h de depuración.", "Ninguno", "Orden de arranque documentado.", "Docker."),
    ("R5", "Riesgo de entorno", "Compilar GA2 (.NET 5, IronPdf) requiere ≈8,5 GB y el SDK 5.0 en contenedor.", "Varios", "+2–4 h de preparación si el equipo no tiene el entorno.", "Validación", "Preparar una máquina o contenedor de compilación de GA2.", "Docker o SDK .NET 5."),
    ("R6", "Riesgo de seguridad", "Secretos en appsettings del repositorio y validación TLS desactivada en la pasarela.", "Varios", "Sin impacto en horas; el perfil de pruebas usa credenciales ficticias.", "Ninguno", "Rotar secretos y moverlos a un almacén seguro.", "Equipo de GA2 / seguridad."),
]
for i, row in enumerate(RISKS_ROWS):
    for c, v in enumerate(row, 1):
        ws.cell(row=first + i, column=c, value=v)
rl = first + len(RISKS_ROWS) - 1
style_body(ws, first, rl, 1, 8)
table(ws, "PendientesRiesgos", f"A{HR}:H{rl}", "TableStyleMedium3")
for text, color in [("Desarrollo", "F8CBAD"), ("Validación", "FFF2CC"), ("Ninguno", "E2EFDA")]:
    ws.conditional_formatting.add(f"F{first}:F{rl}", CellIsRule(operator="equal", formula=[f'"{text}"'], fill=PatternFill("solid", fgColor=color)))
ws.freeze_panes = ws.cell(row=HR + 1, column=3)

# Evidencias (referencias verificables): todas deben existir.
missing = set()
for code in refs:
    for part in code.split(";"):
        path = part.strip()
        if path and not (ROOT / path).exists():
            missing.add(path)
for spec in SPEC.values():
    if not (ROOT / spec).exists():
        missing.add(spec)
if missing:
    raise SystemExit(f"Referencias inexistentes: {sorted(missing)}")

for sheet in wb.worksheets:
    sheet.sheet_view.zoomScale = 90
OUT.parent.mkdir(parents=True, exist_ok=True)
wb.save(OUT)
print(f"{OUT.relative_to(ROOT)}: {len(rows)} métodos, {len(providers)} proveedores")
