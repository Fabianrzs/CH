// Genera ~/Downloads/Guia_Pruebas_MockIntegrations_GA2.docx a partir de los resultados reales
// (Evidencias_MockIntegrations_GA2/resultados.json + archivos de evidencia) y del catálogo del mock.
// Uso: NODE_PATH=<carpeta con node_modules/docx> node MockIntegrations/tools/generate-test-guide.js
const fs = require("fs");
const path = require("path");
const os = require("os");
const {
  Document, Packer, Paragraph, TextRun, HeadingLevel, Table, TableRow, TableCell, WidthType, ShadingType,
  AlignmentType, LevelFormat, PageOrientation, TableOfContents, Footer, PageNumber, BorderStyle, PageBreak,
} = require("docx");

const HOME = path.join(os.homedir(), "Downloads");
const EV = path.join(HOME, "Evidencias_MockIntegrations_GA2");
const OUT = path.join(HOME, "Guia_Pruebas_MockIntegrations_GA2.docx");
const ROOT = path.resolve(__dirname, "../..");
const results = JSON.parse(fs.readFileSync(path.join(EV, "resultados.json"), "utf8"));
const catalog = JSON.parse(fs.readFileSync(path.join(ROOT, "MockIntegrations/docs/estimacion/catalogo-operaciones.json"), "utf8"));
const byId = Object.fromEntries(results.map((r) => [r.id, r]));
const count = (st) => results.filter((r) => r.status === st).length;
const STD = "http://localhost:5080";
const RUN = "http://127.0.0.1:5090";

// ─── estilos y utilidades ──────────────────────────────────────────────────
const FONT = "Arial";
const PORTRAIT_W = 9638;   // A4 vertical, márgenes 2 cm
const LAND_W = 14570;      // A4 apaisado
const COLORS = { APROBADA: "C6EFCE", FALLIDA: "FFC7CE", BLOQUEADA: "FFEB9C", "NO EJECUTADA": "D9D9D9" };
const border = { style: BorderStyle.SINGLE, size: 4, color: "BFBFBF" };
const borders = { top: border, bottom: border, left: border, right: border };

const P = (text, opts = {}) => new Paragraph({ spacing: { after: 120 }, ...opts.para, children: runs(text, opts) });
function runs(text, opts = {}) {
  // **negrita** y `código` dentro del texto.
  const out = [];
  String(text).split(/(\*\*[^*]+\*\*|`[^`]+`)/).filter(Boolean).forEach((part) => {
    if (part.startsWith("**")) out.push(new TextRun({ text: part.slice(2, -2), bold: true, font: FONT, size: opts.size || 21 }));
    else if (part.startsWith("`")) out.push(new TextRun({ text: part.slice(1, -1), font: "Consolas", size: (opts.size || 21) - 2, color: "1F3864" }));
    else out.push(new TextRun({ text: part, font: FONT, size: opts.size || 21, bold: opts.bold, italics: opts.italics, color: opts.color }));
  });
  return out;
}
const H = (text, level) => new Paragraph({ heading: level, spacing: { before: 240, after: 120 }, children: [new TextRun({ text, font: FONT })] });
const bullet = (text, level = 0) => new Paragraph({ numbering: { reference: "bullets", level }, spacing: { after: 60 }, children: runs(text) });
const numbered = (text, ref = "steps") => new Paragraph({ numbering: { reference: ref, level: 0 }, spacing: { after: 60 }, children: runs(text) });
let stepRefs = 0;
const newSteps = () => `steps${++stepRefs}`;

function code(text, maxLines = 40) {
  let lines = String(text).replace(/\r/g, "").split("\n");
  if (lines.length > maxLines) lines = [...lines.slice(0, maxLines), `… (${lines.length - maxLines} líneas más en el archivo de evidencia)`];
  return lines.map((l, i) => new Paragraph({
    shading: { type: ShadingType.CLEAR, fill: "F2F2F2", color: "auto" },
    spacing: { after: 0, before: i === 0 ? 60 : 0 },
    children: [new TextRun({ text: l.length > 150 ? l.slice(0, 150) + " …" : (l || " "), font: "Consolas", size: 15 })],
  })).concat([new Paragraph({ spacing: { after: 100 }, children: [] })]);
}

function table(headers, rows, widths, opts = {}) {
  const total = widths.reduce((a, b) => a + b, 0);
  const size = opts.size || 16;
  const cell = (text, w, isHead, fill) => new TableCell({
    borders, width: { size: w, type: WidthType.DXA },
    shading: isHead ? { type: ShadingType.CLEAR, fill: "1F3864", color: "auto" } : (fill ? { type: ShadingType.CLEAR, fill, color: "auto" } : undefined),
    margins: { top: 40, bottom: 40, left: 70, right: 70 },
    children: String(text ?? "").split("\n").map((t) => new Paragraph({ children: isHead ? [new TextRun({ text: t, bold: true, color: "FFFFFF", font: FONT, size })] : runs(t, { size }) })),
  });
  return new Table({
    width: { size: total, type: WidthType.DXA }, columnWidths: widths,
    rows: [new TableRow({ tableHeader: true, children: headers.map((h, i) => cell(h, widths[i], true)) }),
      ...rows.map((r) => new TableRow({ children: r.map((v, i) => cell(v, widths[i], false, opts.fill ? opts.fill(r, i) : undefined)) }))],
  });
}
const statusFill = (col) => (r, i) => (i === col ? COLORS[r[col]] : undefined);

function evidence(rel) {
  try { return fs.readFileSync(path.join(EV, rel), "utf8"); } catch { return ""; }
}
function splitEvidence(rel) {
  const t = evidence(rel);
  const [req, resp] = t.split("=== RESPUESTA ===");
  return { req: (req || "").replace("=== PETICIÓN ===\n", "").trim(), resp: (resp || "").trim() };
}
function parseRequest(req) {
  const lines = req.split("\n");
  const [method, url] = lines[0].split(" ");
  const headers = [];
  let i = 1;
  for (; i < lines.length && lines[i].trim() !== ""; i++) { const m = lines[i].match(/^([^:]+):\s*(.*)$/); if (m) headers.push([m[1], m[2]]); }
  const body = lines.slice(i + 1).join("\n").split("\n\n---")[0].split("\n(")[0].trim();
  return { method, url: (url || "").replace(RUN, STD), headers, body };
}
function responseBody(resp) {
  const parts = resp.split("\n\n");
  return { status: parts[0].split("\n")[0], body: parts.slice(1).join("\n\n").trim() };
}
function pretty(body) {
  const b = body.trim();
  if (b.startsWith("{") || b.startsWith("[")) { try { return JSON.stringify(JSON.parse(b), null, 2); } catch { return b; } }
  if (b.startsWith("<") && !b.includes("\n") && b.length > 200) return b.replace(/></g, ">\n<");
  return b;
}
function checksOf(r) {
  // "HTTP 200; desc: detalle; desc: detalle" → [[desc, detalle]]
  return String(r.actual).split("; ").slice(1).map((c) => { const k = c.indexOf(": "); return k > 0 ? [c.slice(0, k), c.slice(k + 2)] : [c, ""]; });
}

// ─── contenido ─────────────────────────────────────────────────────────────
const provSummary = {};
results.forEach((r) => { (provSummary[r.provider] ||= { APROBADA: 0, FALLIDA: 0, BLOQUEADA: 0, "NO EJECUTADA": 0, total: 0 }); provSummary[r.provider][r.status]++; provSummary[r.provider].total++; });
const DISPLAY = { Seven: "Seven ERP", WorkManager: "WorkManager / DODO (SOAP)", Bua: "BUA", Vigia: "Vigía", Fenix: "Fénix", Haberes: "Haberes (MCD)", TransUnion: "TransUnion", TusDatos: "TusDatos", Dodo: "Dodo REST", KtrionApi: "Pasarela ktrionApi", Smtp: "SMTP / correo", ServiceBus: "Azure Service Bus" };
const SWAGGER_DOC = { Seven: "E1 · Seven ERP", WorkManager: "E2 · WorkManager / DODO (SOAP)", Bua: "E3 · BUA SOAP", Vigia: "E4 · Vigía", Fenix: "E5 · Fénix (biometría)", Haberes: "E6 · Haberes (MCD)", TransUnion: "E7 · TransUnion DecisionCentre", TusDatos: "E8 · TusDatos (API Gateway)", Dodo: "E9 · Dodo REST", KtrionApi: "E10 · Pasarela ktrionApi", Smtp: "E11 · SMTP", ServiceBus: "E12 · Azure Service Bus" };
const USO = { Business: "Lógica de negocio", PassThrough: "Reenvío (pass-through)", NotUsed: "Contractual, no invocado" };
function methodStatus(op) {
  const rs = results.filter((r) => r.excel_id && r.provider === DISPLAY[op.provider] && (r.method === op.name || r.method.startsWith(op.name + " ") || r.method.startsWith(op.name + " (")));
  if ((op.provider === "Smtp" && op.name === "RecuperarContrasena")) return "Bloqueado (host fijo)";
  if (op.provider === "ServiceBus" && op.name === "WorkManagerInsertRequest") return "Publicación lista; consumo bloqueado (defecto GA2)";
  if (op.provider === "ServiceBus") return "Publicación lista; consumo bloqueado (defecto GA2)";
  if (op.provider === "TransUnion" && op.name === "ExecuteXMLString") return "Listo (sólo directo al mock; URL fija en GA2)";
  if (op.provider === "Bua") return "Listo (ws.01 sólo directo al mock)";
  if (op.usage === "NotUsed" && op.certainty === "NotImplemented") return "Listo: responde error explícito";
  return rs.every((r) => r.status === "APROBADA") ? "Listo" : "Revisar";
}
const priority = (op) => (op.usage === "Business" ? "Alta" : op.usage === "PassThrough" ? "Media" : "Baja");

const DETAILED = {
  "TUSDATOS-001": "Comprobar que una consulta de cliente inicia un trabajo asíncrono (job) y devuelve su identificador.",
  "TUSDATOS-002": "Comprobar que el primer sondeo del job devuelve el estado pendiente (respuesta posterior a la inicial).",
  "TUSDATOS-003": "Comprobar que el segundo sondeo del mismo job devuelve el resultado final.",
  "TUSDATOS-004": "Comprobar el resultado tomado del histórico (sin job) usando un documento de prueba con regla.",
  "TUSDATOS-005": "Comprobar el mismo escenario 'historial' forzándolo con el header X-Mock-Scenario.",
  "TUSDATOS-006": "Comprobar el escenario de persona expuesta políticamente (PEP): inicia un job.",
  "TUSDATOS-007": "Comprobar que el job PEP termina con isPEPS=true y riesgo ALTO.",
  "TUSDATOS-008": "Comprobar el caso 'documento no encontrado' (HTTP 404).",
  "TUSDATOS-009": "Comprobar que sin el header obligatorio x-api-key el gateway responde 401.",
  "TUSDATOS-011": "Comprobar un error técnico del proveedor (HTTP 500).",
  "TUSDATOS-013": "Comprobar una respuesta lenta (timeout de GA2: 15 s).",
  "TUSDATOS-014": "Confirmar que el nombre de escenario 'ner' no existe (el correcto es 'pep').",
  "TUSDATOS-019": "Comprobar la consulta de riesgo LA/FT (Vigía a través del gateway).",
  "SEVEN-CrearCDP-01": "Comprobar la creación de un CDP en Seven (respuesta de éxito con consecutivo MPR_CONT).",
  "SEVEN-CrearCDP-02": "Comprobar el error de negocio de Seven (RETORNO=1 con TXTERROR).",
  "SEVEN-CrearCDP-04": "Comprobar un error técnico SOAP (SOAP Fault, HTTP 500).",
  "SEVEN-InsertarGnTerce-03": "Comprobar el caso 'tercero ya registrado' (GA2 continúa el pago).",
  "WM-Form_Insert-01": "Comprobar la radicación en WorkManager: número de radicado con el prefijo del formulario.",
  "WM-Form_Insert-04": "Comprobar una radicación fallida (Success=false) usando el formulario de prueba 999.",
  "WM-Form_GetParameters-01": "Comprobar los datos que GA2 usa para construir el enlace al formulario DODO.",
  "BUA-001": "Comprobar la consulta del afiliado en la BUA (servicio ws.02).",
  "BUA-003": "Comprobar el caso de afiliado no encontrado (resultado nulo).",
  "BUA-005": "Comprobar el reintento por tipo de documento (sólo se encuentra con tipo_Id = 4).",
  "VIGIA-001": "Comprobar la verificación de una persona sin coincidencias en listas.",
  "VIGIA-002": "Comprobar la verificación de una persona reportada en listas.",
  "FENIX-001": "Comprobar la validación biométrica completa.",
  "FENIX-003": "Comprobar el caso de afiliado sin datos biométricos.",
  "HABERES-GenerarReporte-01": "Comprobar que el certificado de haberes llega como un PDF válido.",
  "HABERES-ReporteHaberes-01": "Comprobar el DataSet del reporte de haberes (debe llegar sin espacios).",
  "TU-001": "Paso 1 de TransUnion: iniciar la verificación de identidad (IDVision).",
  "TU-002": "Paso 2: enviar el código OTP por SMS usando el ApplicationId del paso 1.",
  "TU-005": "Paso 4: validar el PIN correcto (código 300).",
  "TU-006": "Paso 5: obtener score y capacidad de endeudamiento (AQM).",
  "DODO-Form_Insert-01": "Comprobar la radicación por la API REST de Dodo.",
  "DODO-Version-01": "Comprobar que la versión llega como texto JSON.",
  "KTRION-Autenticacion-01": "Comprobar que la pasarela entrega un token.",
  "KTRION-FormInsert-01": "Comprobar la radicación por la pasarela con el token obtenido.",
  "KTRION-FormInsert-02": "Comprobar que sin token la pasarela responde 401.",
  "SMTP-001": "Comprobar que un correo con PDF adjunto queda capturado en el buzón del mock.",
  "SMTP-002": "Comprobar el rechazo de un destinatario (código 550).",
  "SB-WorkManagerInsertRequest-01": "Comprobar la publicación de un evento en el topic que consume GA2.",
  "SB-CONSUMO-01": "Comprobar que GA2 procesa el evento y llama a WorkManager.",
  "GA2-001": "Comprobar de extremo a extremo que GA2 llama al mock de TusDatos e interpreta la respuesta.",
  "GA2-005": "Comprobar de extremo a extremo la verificación de Vigía a través de GA2.",
  "GA2-007": "Comprobar de extremo a extremo el caso Fénix sin datos y su efecto en GA2 (notificación).",
  "ADMIN-001": "Comprobar la API de escenarios: listar, fijar, prioridad del header y quitar.",
};

function detailedCase(id) {
  const r = byId[id];
  if (!r) return [];
  const out = [H(`${r.id} · ${r.provider} · ${r.method}`, HeadingLevel.HEADING_3)];
  const { req, resp } = r.evidence ? splitEvidence(r.evidence) : { req: "", resp: "" };
  const isGa2 = id.startsWith("GA2-");
  const isHttp = req.match(/^(GET|POST|PUT|DELETE) /);
  const pr = isHttp ? parseRequest(req) : null;
  const rb = resp ? responseBody(resp) : null;
  const isSoap = pr && pr.headers.some(([k]) => k === "SOAPAction");
  const swagger = Object.keys(DISPLAY).find((k) => DISPLAY[k] === r.provider);
  const rows = [
    ["Objetivo", DETAILED[id]],
    ["Escenario del mock", r.scenario],
    ["Precondiciones", isGa2 ? "MockIntegrations y la API GA2 (perfil Mocks, base stub, emulador de Service Bus) en ejecución (§2.4)." : (id.startsWith("SB-") ? "MockIntegrations con MockIntegrations:ServiceBus:ConnectionString y el emulador de Service Bus en ejecución (§2.4)." : "MockIntegrations en ejecución (§2.2). Recomendado: POST /_mock/reset antes de la prueba.")],
    ["Herramienta", isGa2 ? "curl o Postman contra la API GA2" : id.startsWith("SMTP") ? "Cliente SMTP (script) + API de inspección /smtp/messages" : isSoap ? `Swagger (documento "${SWAGGER_DOC[swagger]}") o curl / SoapUI` : `Swagger (documento "${SWAGGER_DOC[swagger] || "Administración del mock"}") o curl / Postman`],
  ];
  if (pr) {
    rows.push(["URL", pr.url.replace("http://127.0.0.1:5102", "http://localhost:5102")]);
    rows.push(["Método HTTP", pr.method]);
    rows.push(["Headers", pr.headers.filter(([k]) => !["Accept"].includes(k)).map(([k, v]) => `${k}: ${v}`).join("\n") || "—"]);
  }
  rows.push(["Datos de entrada", r.inputs]);
  rows.push(["Respuesta HTTP esperada / resultado esperado", r.expected]);
  out.push(table(["Campo", "Valor"], rows, [2600, PORTRAIT_W - 2600], { size: 17 }));
  out.push(P("**Pasos**", { para: { spacing: { before: 120, after: 60 } } }));
  const ref = newSteps();
  if (isGa2) {
    ["Vacíe el registro del mock: `DELETE http://localhost:5080/_mock/journal`.", `Envíe la petición a GA2 indicada arriba (con el token JWT de prueba si el endpoint lo exige; ver §2.4).`, "Revise la respuesta de GA2 (campos `isSuccess`, `returnMessage`, `data`).", `Abra \`http://localhost:5080/_mock/journal?provider=${swagger}\` y confirme que el mock recibió la llamada con el escenario esperado.`].forEach((s) => out.push(numbered(s, ref)));
  } else if (id.startsWith("SMTP")) {
    ["Envíe el correo al servidor SMTP del mock (puerto 2525; en la ejecución registrada, 2526) sin TLS. Por ejemplo con el script de pruebas o cualquier cliente SMTP.", "Abra `http://localhost:5080/smtp/messages` (o el documento \"E11 · SMTP\" en Swagger).", "Compruebe asunto, remitente, destinatarios y adjuntos del correo capturado."].forEach((s) => out.push(numbered(s, ref)));
  } else if (isSoap) {
    const action = pr.headers.find(([k]) => k === "SOAPAction")[1];
    [`Abra Swagger (\`http://localhost:5080/swagger\`) y seleccione arriba el documento "${SWAGGER_DOC[swagger]}".`, `Expanda la operación \`POST ${new URL(pr.url).pathname}\` y pulse **Try it out**.`, `En el campo del header **SOAPAction** elija \`${action.replace(/"/g, "")}\`.`, ...(pr.headers.find(([k]) => k === "X-Mock-Scenario") ? [`En el header **X-Mock-Scenario** escriba \`${pr.headers.find(([k]) => k === "X-Mock-Scenario")[1]}\`.`] : []), "En **Request body** pegue el XML de la sección «Cuerpo» (o elija el ejemplo de la operación y cambie los valores indicados en «Datos de entrada»).", "Pulse **Execute** y revise el código de respuesta y el XML devuelto según las validaciones."].forEach((s) => out.push(numbered(s, ref)));
  } else if (pr) {
    const u = new URL(pr.url);
    [`Abra Swagger (\`http://localhost:5080/swagger\`) y seleccione el documento "${SWAGGER_DOC[swagger] || "Administración del mock"}".`, `Expanda la operación \`${pr.method} ${decodeURIComponent(u.pathname)}\` y pulse **Try it out**.`, "Rellene los parámetros de ruta y los headers con los valores de «Datos de entrada» (los headers que no aparecen en Swagger, como x-api-key o Authorization, se envían con curl o Postman).", ...(pr.body ? ["Pegue el cuerpo JSON de la sección «Cuerpo»."] : []), "Pulse **Execute** y compare la respuesta con las validaciones."].forEach((s) => out.push(numbered(s, ref)));
  } else {
    out.push(numbered("Siga el procedimiento descrito en «Datos de entrada».", ref));
  }
  if (pr && pr.body) {
    out.push(P("**Cuerpo (body) enviado**", { para: { spacing: { before: 120, after: 40 } } }));
    out.push(...code(pretty(pr.body), 45));
  } else if (!pr && req) {
    out.push(P("**Entrada enviada**", { para: { spacing: { before: 120, after: 40 } } }));
    out.push(...code(req, 30));
  }
  if (pr) {
    const h = pr.headers.filter(([k]) => !["Accept"].includes(k)).map(([k, v]) => ` -H '${k}: ${v.replace(/'/g, "")}'`).join("");
    const curl = pr.body ? (isSoap ? `# Guarde el XML del cuerpo en peticion.xml\ncurl -i -X ${pr.method} '${pr.url}'${h} --data-binary @peticion.xml` : `curl -i -X ${pr.method} '${pr.url}'${h} -d '${pr.body.replace(/\n\s*/g, "")}'`) : `curl -i -X ${pr.method} '${pr.url}'${h}`;
    out.push(P("**Alternativa con curl**", { para: { spacing: { before: 120, after: 40 } } }));
    out.push(...code(curl, 10));
  }
  if (rb) {
    out.push(P(`**Respuesta real obtenida** (${rb.status})`, { para: { spacing: { before: 120, after: 40 } } }));
    out.push(...code(pretty(rb.body) || "(sin cuerpo)", 30));
  }
  out.push(P("**Validaciones y resultado real**", { para: { spacing: { before: 120, after: 60 } } }));
  out.push(table(["Validación (criterio de aprobación)", "Valor obtenido"], checksOf(r).map(([a, b]) => [a, b || "✓"]), [5200, PORTRAIT_W - 5200], { size: 16 }));
  out.push(table(["Veredicto", "Evidencia", "Comentario"], [[r.status, r.evidence ? `Evidencias_MockIntegrations_GA2/${r.evidence}` : "—", r.comment || "—"]], [1700, 4400, PORTRAIT_W - 6100], { size: 16, fill: statusFill(0) }));
  return out;
}

// ─── secciones ─────────────────────────────────────────────────────────────
const s1 = [
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 1800, after: 200 }, children: [new TextRun({ text: "Guía de ejecución y validación de pruebas de MockIntegrations – GA2", bold: true, size: 44, font: FONT, color: "1F3864" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { after: 120 }, children: [new TextRun({ text: "Datos de prueba, procedimientos paso a paso y resultados reales de ejecución", size: 26, font: FONT, color: "595959" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { after: 600 }, children: [new TextRun({ text: "Ejecución: 19 de octubre de 2026", size: 22, font: FONT })] }),
  table(["Resultado de la ejecución", "Cantidad"], [
    ["Métodos del Excel de referencia (todos con al menos un caso)", "153"],
    ["Casos de prueba registrados", String(results.length)],
    ["Casos ejecutados (petición real enviada)", String(results.filter((r) => r.actual !== "No ejecutado").length)],
    ["APROBADA", String(count("APROBADA"))], ["FALLIDA", String(count("FALLIDA"))], ["BLOQUEADA", String(count("BLOQUEADA"))], ["NO EJECUTADA", String(count("NO EJECUTADA"))],
    ["Pruebas automatizadas (dotnet test)", "210 de 210 aprobadas"],
  ], [6500, 3138], { size: 20, fill: (r, i) => (i === 1 ? COLORS[r[0]] : undefined) }),
  new Paragraph({ children: [new PageBreak()] }),
  H("Contenido", HeadingLevel.HEADING_1),
  P("Si el índice aparece vacío, en Word pulse con el botón derecho sobre él y elija «Actualizar campos».", { italics: true, size: 18 }),
  new TableOfContents("Contenido", { hyperlink: true, headingStyleRange: "1-2" }),
  new Paragraph({ children: [new PageBreak()] }),

  H("1. Introducción", HeadingLevel.HEADING_1),
  P("GA2 se comunica con 12 sistemas externos (Seven, WorkManager, BUA, Vigía, Fénix, Haberes, TransUnion, TusDatos, Dodo, la pasarela ktrionApi, el correo SMTP y Azure Service Bus). **MockIntegrations** es un programa que **imita** a esos sistemas: responde como ellos, pero sin conectarse a ningún servicio real. Así GA2 se puede probar sin tocar producción."),
  P("Esta guía explica, para una persona que no conoce el código, **cómo probar cada método** del mock: qué datos escribir, dónde escribirlos y cómo saber si la prueba pasó. También presenta los **resultados reales** de la ejecución del 19 de octubre de 2026."),
  H("1.1 Alcance", HeadingLevel.HEADING_2),
  bullet("**Fuente del alcance:** el Excel `Estimacion_Mocks_GA2_desde_cero.xlsx` (hoja «Detalle por método»): 153 métodos de 12 proveedores. Se comprobó que coinciden exactamente con los 153 métodos que publica el mock (caso ENV-002)."),
  bullet("**Clasificación:** 34 métodos los usa la lógica de negocio de GA2; 53 GA2 los reenvía tal cual (pass-through); 66 existen en los contratos pero GA2 no los invoca (el mock responde un error explícito o una respuesta validada)."),
  bullet("**Qué se probó:** cada método directamente contra el mock (respuestas, escenarios de error, formatos) y, para 5 integraciones, el recorrido completo **GA2 → mock** con la API GA2 real."),
  bullet("**Qué no se probó y por qué:** se explica en §7 y §8 (URLs fijas a producción, defectos de GA2 en Service Bus, falta de base de datos de pruebas)."),
  H("1.2 Conceptos básicos", HeadingLevel.HEADING_2),
  table(["Término", "Qué significa"], [
    ["Mock", "Imitación de un sistema externo. Devuelve respuestas preparadas que respetan el contrato que GA2 espera. No es el sistema real."],
    ["Escenario", "La variante de respuesta del mock: `default` (éxito), `no-encontrado`, `fault` (error técnico), etc. Cada método tiene los suyos."],
    ["Header X-Mock-Scenario", "Cabecera opcional que fuerza un escenario en una prueba manual. GA2 nunca la envía."],
    ["Documento de prueba", "Números de identificación ficticios que activan un escenario automáticamente (p. ej. `1000000404` = no encontrado). No son cédulas reales."],
    ["SOAP / SOAPAction", "Formato XML que usan los servicios antiguos. El header SOAPAction indica qué operación se llama dentro del mismo endpoint."],
    ["REST / JSON", "Formato moderno: URL + datos JSON."],
    ["Journal (registro de llamadas)", "Lista de las llamadas que recibió el mock: `http://localhost:5080/_mock/journal`."],
    ["Respuesta configurada vs. real", "Lo que devuelve el mock es la respuesta **configurada** a partir del contrato. Algunos valores de negocio no están confirmados con el proveedor real (se indica en cada caso y en §8)."],
  ], [2600, PORTRAIT_W - 2600], { size: 18 }),

  H("2. Preparación del entorno", HeadingLevel.HEADING_1),
  H("2.1 Requisitos", HeadingLevel.HEADING_2),
  bullet("SDK de .NET 10 (en este equipo: `~/.dotnet/dotnet`, versión 10.0.401)."),
  bullet("Un navegador para Swagger. Opcional: Postman, SoapUI o `curl`."),
  bullet("Sólo para las pruebas de Service Bus y GA2 extremo a extremo: Docker y la API GA2 compilada (.NET 5)."),
  H("2.2 Arrancar MockIntegrations (obligatorio)", HeadingLevel.HEADING_2),
  P("Abra una terminal y ejecute:"),
  ...code("cd ~/Downloads/GA2/MockIntegrations\nexport PATH=$HOME/.dotnet:$PATH\ndotnet run --project src/MockIntegrations.Api"),
  P("El mock queda escuchando en **http://localhost:5080** (HTTP) y en el puerto **2525** (SMTP). Deje la terminal abierta."),
  P("**Nota sobre la ejecución registrada:** el 19 de octubre ya había una instancia del mock iniciada por el usuario en 5080/2525. No se detuvo; las pruebas se ejecutaron en una segunda instancia del mismo código en **http://127.0.0.1:5090** (SMTP 2526), con el retardo del escenario `slow` reducido a 3 s. Por eso las evidencias muestran el puerto 5090. En su equipo use 5080.", { italics: true }),
  H("2.3 Comprobar que el entorno está listo", HeadingLevel.HEADING_2),
  table(["Comprobación", "Cómo", "Resultado esperado"], [
    ["El mock responde", "Abrir `http://localhost:5080/health`", "`{\"status\":\"ok\"}`"],
    ["Swagger abre", "Abrir `http://localhost:5080/swagger`", "Página «MockIntegrations · GA2» con selector de documentos"],
    ["Están las 153 operaciones", "Abrir `http://localhost:5080/_mock/catalog`", "Lista JSON con 153 elementos"],
    ["Estado limpio", "`curl -X POST http://localhost:5080/_mock/reset`", "HTTP 204 (reinicia contadores, escenarios fijados y registro)"],
  ], [2600, 3800, PORTRAIT_W - 6400], { size: 17 }),
  H("2.4 Entorno completo (Service Bus y GA2 extremo a extremo)", HeadingLevel.HEADING_2),
  P("Sólo necesario para los casos SB-* y GA2-*. Orden de arranque:"),
  ...code(`cd ~/Downloads/GA2/MockIntegrations
cp .env.example .env                        # contraseña local del SQL del emulador
./docker/tls-proxy/generate-certs.sh        # certificados de prueba (proxy TLS)
docker compose up -d servicebus-sql         # 1. SQL del emulador (esperar ~15 s)
docker compose --profile ga2-sdk79 up -d    # 2. emulador Service Bus + mock + proxy TLS
# 3. base de datos stub para GA2 (sólo arranque, interruptores, auditoría y notificaciones)
PW=$(grep MSSQL_SA_PASSWORD .env | cut -d= -f2)
docker compose exec -T servicebus-sql /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$PW" \\
  -i /dev/stdin < docker/ga2-smoke/stub-database.sql
# 4. API GA2 con el perfil Mocks (ver docs/guia-configuracion-ga2.md §8)
export ASPNETCORE_ENVIRONMENT=Mocks SSL_CERT_FILE=$PWD/docker/tls-proxy/certs/ga2-mocks-ca.crt
export ConnectionStrings__connectionName="Server=127.0.0.1,14333;Database=GA2_MOCKS_SMOKE;User Id=sa;Password=$PW"
export JwtOptions__Key=$(openssl rand -hex 32)   # clave de PRUEBA para firmar el token
dotnet run --project ../src/GA2.Apis/GA2 --urls http://localhost:5102`),
  P("Para que GA2 apunte a una instancia del mock en otra dirección o puerto, cargue antes las variables con `MOCK_BASE_URL=http://host:puerto MOCK_SMTP_PORT=… source ga2-config/ga2-mocks.env.sh`. Así se hizo en la ejecución registrada (5090/2526); la configuración efectiva está en `00_entorno/ga2_variables_entorno_mocks.env`."),
  P("**Seguridad:** el perfil Mocks sustituye las credenciales de integración por valores ficticios. TransUnion y BUA ws.01 tienen la URL fija en el código de GA2: **no** ejecute desde GA2 flujos que las usen, porque saldrían a los servidores reales (ver §8)."),
  H("2.5 Repetir todas las pruebas automáticamente", HeadingLevel.HEADING_2),
  ...code(`cd ~/Downloads/GA2/MockIntegrations && dotnet test        # 210 pruebas de contrato
cd ~/Downloads/GA2
python3 MockIntegrations/tools/run-test-cases.py --mock http://localhost:5080 --smtp-port 2525 \\
  --evidence ~/Downloads/Evidencias_MockIntegrations_GA2 [--ga2 http://localhost:5102 --jwt-key <clave>]`),

  H("3. Cómo usar Swagger", HeadingLevel.HEADING_1),
  P("Swagger es una página web que lista las operaciones del mock y permite probarlas desde el navegador."),
  H("3.1 Abrir y localizar un proveedor", HeadingLevel.HEADING_2),
  ...[`Abra **http://localhost:5080/swagger**.`, `Arriba a la derecha, en **Select a definition**, elija el proveedor, por ejemplo «E8 · TusDatos (API Gateway)». «Todos los proveedores» muestra todo junto.`, `Cada operación aparece como una barra con el verbo (GET, POST) y la ruta. Pulse sobre ella para **expandirla**: verá la descripción, los escenarios disponibles, los parámetros y los ejemplos.`].map((s, _, a) => numbered(s, "swagger1")),
  H("3.2 Rellenar campos, headers y ejecutar", HeadingLevel.HEADING_2),
  ...[`Pulse **Try it out** (en este mock ya viene activado).`, `Escriba los **parámetros de ruta** (por ejemplo, identification, documentType, checkHistory).`, `Para forzar un escenario escriba su nombre en el header **X-Mock-Scenario** (por ejemplo, \`no-encontrado\`). Si lo deja vacío se aplica el escenario por defecto o la regla del documento de prueba.`, `Pulse **Execute**.`, `Más abajo, en **Responses**, revise: **Code** (código HTTP), **Response body** (contenido) y **Response headers**. El mock añade \`x-mock-scenario-applied\` (escenario aplicado) y \`x-mock-scenario-source\` (por qué se aplicó: Header, Rule, RuntimeOverride, Default).`].map((s) => numbered(s, "swagger2")),
  P("**Limitación:** Swagger no permite añadir headers que la operación no declara, como `x-api-key` en TusDatos o `Authorization` en la pasarela. Para esas pruebas use `curl` o Postman (los comandos están en cada caso de §6)."),
  H("3.3 Ejemplo REST real: TusDatos – consulta de cliente", HeadingLevel.HEADING_2),
  P("Plantilla de la operación: `GET /kiosko-dev/api/v1/tusdatos/customer/{identification}/document-type/{documentType}/check-history/{checkHistory}`."),
  table(["Parámetro", "Dónde", "Formato según el código", "Valor de ejemplo"], [
    ["identification", "Ruta", "Texto (número de documento). Los documentos de prueba activan escenarios: 1000000100 historial, 1000000666 PEP, 1000000404 no encontrado.", "1020304050"],
    ["documentType", "Ruta", "Texto, p. ej. CC (GA2 lo reenvía tal cual).", "CC"],
    ["checkHistory", "Ruta", "`true` o `false` en minúsculas (GA2 hace checkHistory.ToString().ToLower()). El mock no valida el valor (caso TUSDATOS-016).", "true"],
    ["x-api-key", "Header (obligatorio)", "Cualquier valor no vacío (el mock no tiene clave configurada). Sin él: 401.", "clave-de-prueba"],
    ["X-Mock-Scenario", "Header (opcional)", "default, pep, historial, no-encontrado, unauthorized, fault, http-500, slow. Los nombres «ner» y «malnoautorizado» NO existen (casos TUSDATOS-014/015).", "(vacío)"],
  ], [1700, 1400, 4400, PORTRAIT_W - 7500], { size: 16 }),
  P("Respuesta real del caso TUSDATOS-001 (HTTP 200). Es una consulta **asíncrona**: la primera respuesta sólo devuelve el `jobId`; el resultado se obtiene después con «ConsultarJob» (`GET …/tusdatos/jobid/{jobId}/customer/{identification}`), que la primera vez devuelve `pendiente` y la segunda `finalizado` (casos TUSDATOS-002 y 003)."),
  ...code(pretty(responseBody(splitEvidence(byId["TUSDATOS-001"].evidence).resp).body)),
  table(["Escenario TusDatos", "Cómo activarlo en Swagger", "HTTP esperado", "Campos a revisar", "Qué hace GA2 después"], [
    ["default", "identification=1020304050, sin X-Mock-Scenario", "200", "jobId=mock-job-…, isFromHistory=false", "Devuelve el jobId; el frontend sondea el job"],
    ["historial", "identification=1000000100 o X-Mock-Scenario=historial", "200", "isFromHistory=true, jobId=null, historyDataCustomer", "Muestra el resultado del histórico"],
    ["pep", "identification=1000000666 o X-Mock-Scenario=pep", "200", "jobId; al sondear 2 veces: isPEPS=true, detectionRisk=ALTO", "Muestra persona PEP"],
    ["no-encontrado", "identification=1000000404 o X-Mock-Scenario=no-encontrado", "404", "Sin cuerpo", "Lo trata como éxito con data=null (verificado en GA2-002)"],
    ["unauthorized", "X-Mock-Scenario=unauthorized (o quitar x-api-key)", "401", "Texto del error", "IsSuccess=false con ese texto"],
    ["fault / http-500", "X-Mock-Scenario=fault / http-500", "500", "Texto / HTML", "IsSuccess=false"],
    ["slow", "X-Mock-Scenario=slow", "200 con retardo", "Tiempo de respuesta", "Si supera 15 s, GA2 corta (IsSuccess=false)"],
  ], [1500, 2900, 1100, 2300, PORTRAIT_W - 7800], { size: 15 }),
  H("3.4 Ejemplo SOAP real: Fénix – ValidarBiometria", HeadingLevel.HEADING_2),
  ...[`Elija el documento «E5 · Fénix (biometría)» y expanda \`POST /WsFenixBiometria/WebServicesBiometrias.asmx\`.`, `En **SOAPAction** elija \`http://tempuri.org/ValidarBiometria\`.`, `En **Request body**, en el desplegable **Examples**, elija «ValidarBiometria» y cambie el valor de \`<identificacion>\` por \`1020304050\`.`, `Pulse **Execute**. Respuesta esperada: HTTP 200 con un DataSet cuyo \`A001NUM_IDENTIFICACION\` es 1020304050 y \`A008HUELLA\` = 1.`].map((s) => numbered(s, "swagger3")),
  ...code(splitEvidence(byId["FENIX-001"].evidence).req.split("\n\n").slice(1).join("\n\n")),
];

// Sección 4 y 5 (apaisadas)
const methodRows = catalog.slice().sort((a, b) => (a.provider + a.name).localeCompare(b.provider + b.name)).map((op) => [
  DISPLAY[op.provider], op.name, op.soapAction ? `POST ${op.route}\nSOAPAction: ${op.soapAction.replace(/^https?:\/\/[^/]+\/(.*\/)?/, "…/")}` : `${op.method} ${op.route}`,
  { Soap11: "SOAP", Rest: "REST", Smtp: "SMTP", ServiceBus: "Service Bus" }[op.protocol] + " · " + USO[op.usage], methodStatus(op), priority(op),
]);
const provRows = Object.keys(DISPLAY).map((k) => {
  const ops = catalog.filter((o) => o.provider === k);
  return [DISPLAY[k], String(ops.length), String(ops.filter((o) => o.usage === "Business").length), String(ops.filter((o) => o.usage === "PassThrough").length), String(ops.filter((o) => o.usage === "NotUsed").length), { Soap11: "SOAP", Rest: "REST", Smtp: "SMTP", ServiceBus: "Service Bus" }[ops[0].protocol]];
});
const s2 = [
  H("4. Resumen de métodos", HeadingLevel.HEADING_1),
  P("Clasificación: **1** invocado por la lógica de negocio de GA2 (prioridad Alta) · **2** reenviado por GA2 (Media) · **3** contractual, GA2 no lo invoca (Baja) · **4** requiere infraestructura adicional (Service Bus, SMTP, URLs fijas de TransUnion y BUA ws.01) · **5** bloqueado por defecto conocido (SMTP de Identity, consumo de Service Bus). El inventario completo con sus fórmulas está en la hoja «Inventario de métodos» del Excel de resultados."),
  table(["Proveedor", "Métodos", "Negocio", "Reenvío", "No invocados", "Protocolo"], provRows, [3400, 1500, 1500, 1500, 1700, LAND_W - 9600], { size: 17 }),
  new Paragraph({ spacing: { after: 160 }, children: [] }),
  table(["Proveedor", "Método", "Endpoint", "Tipo de integración", "Estado de preparación", "Prioridad"], methodRows, [1900, 2700, 4600, 2300, 2170, 900], { size: 14 }),

  H("5. Datos de prueba", HeadingLevel.HEADING_1),
  H("5.1 Documentos de prueba (sintéticos)", HeadingLevel.HEADING_2),
  P("Ningún dato corresponde a personas reales. Estos números activan escenarios mediante las reglas de `appsettings.json` del mock:"),
  table(["Valor", "Dónde se introduce", "Proveedor", "Escenario que provoca"], [
    ["1020304050", "identificación / identification / param1 / IdNumber / customerIdentification", "Todos", "default (éxito)"],
    ["1000000404", "identificacion (BUA, Fénix), identification (TusDatos), customerIdentification (Haberes)", "BUA, Fénix, TusDatos, Haberes", "no-encontrado / sin-datos / sin-reporte / vacio"],
    ["1000000004", "identificacion + tipo_Id", "BUA", "encontrado-tipo-4 (nulo con tipo 1, ficha con tipo 4)"],
    ["1000000002", "identificacion", "Fénix", "parcial (A008HUELLA=0)"],
    ["1000000666", "param1 (Vigía), identification (TusDatos), IdNumber (TransUnion)", "Vigía, TusDatos, TransUnion", "reportado / pep / riesgo / no-viable"],
    ["1000000777", "IdNumber", "TransUnion", "pendiente"],
    ["1000000999", "IdNumber", "TransUnion", "muerte-titular"],
    ["1000000100", "identification", "TusDatos", "historial"],
    ["1000000500", "identificación del proveedor", "BUA, Vigía, Fénix, TransUnion", "fault (error técnico)"],
    ["FormCode = 999", "FormCode de Form_Insert", "WorkManager", "fallido (Success=false)"],
    ["PIN 123456", "PinNumber", "TransUnion", "OTP válido (código 300); otro PIN: 302"],
    ["rechazado@mock.test", "Destinatario del correo", "SMTP", "550 en RCPT TO"],
  ], [2300, 5400, 3000, LAND_W - 10700], { size: 16 }),
  H("5.2 Casos y valores por método", HeadingLevel.HEADING_2),
  P("Una fila por caso ejecutado o registrado. «Datos de entrada» indica los valores y dónde introducirlos. El detalle completo de cada petición está en la evidencia indicada en el Excel de resultados."),
  table(["ID", "Proveedor", "Método", "Escenario", "Datos de entrada (qué y dónde)", "Resultado esperado", "Estado"],
    results.map((r) => [r.id, r.provider, r.method, r.scenario, r.inputs, r.expected, r.status]), [1900, 1500, 2300, 1300, 3800, 2770, 1000], { size: 13, fill: statusFill(6) }),
];

const s3 = [
  H("6. Pruebas paso a paso", HeadingLevel.HEADING_1),
  P("Se detallan los casos representativos de cada proveedor y todos los escenarios de TusDatos. Para los demás casos el procedimiento es el mismo según el tipo (§6.1) con los valores de §5.2."),
  H("6.1 Procedimientos comunes", HeadingLevel.HEADING_2),
  table(["Tipo de prueba", "Procedimiento"], [
    ["Operación SOAP (Seven, WorkManager, BUA, Vigía, Fénix, Haberes, TransUnion)", "Swagger → documento del proveedor → POST del servicio → Try it out → elegir SOAPAction → elegir el ejemplo de la operación en Request body → ajustar valores → Execute. Éxito: HTTP 200 con el elemento {Operación}Result. Error técnico: HTTP 500 con s:Fault."],
    ["Escenario de error funcional (error-negocio, fallido, resultado-nulo…)", "Igual que la anterior, escribiendo el escenario en el header X-Mock-Scenario. Comprobar el header de respuesta x-mock-scenario-applied."],
    ["Operación contractual no invocada por GA2", "Igual que SOAP con cuerpo vacío <Operación xmlns=\"…\"/>. Esperado: HTTP 500 con faultstring que contiene «GA2 no la invoca» (pasarela: HTTP 501)."],
    ["Operación REST de Dodo (POST JSON)", "Swagger → «E9 · Dodo REST» → operación → cuerpo JSON en camelCase (ver evidencia del caso) → Execute. Éxito: 200 con success=true. Escenarios: fallido (200 success=false), no-encontrado (404)."],
    ["Pasarela ktrionApi", "Primero POST api/autenticacion para obtener Data.Token; luego la operación con el header Authorization: Bearer <token> (curl o Postman)."],
  ], [3400, PORTRAIT_W - 3400], { size: 17 }),
];
const GROUPS = [
  ["6.2 TusDatos (REST, asíncrono)", ["TUSDATOS-001", "TUSDATOS-002", "TUSDATOS-003", "TUSDATOS-004", "TUSDATOS-005", "TUSDATOS-006", "TUSDATOS-007", "TUSDATOS-008", "TUSDATOS-009", "TUSDATOS-011", "TUSDATOS-013", "TUSDATOS-014", "TUSDATOS-019"]],
  ["6.3 Seven ERP (SOAP)", ["SEVEN-CrearCDP-01", "SEVEN-CrearCDP-02", "SEVEN-CrearCDP-04", "SEVEN-InsertarGnTerce-03"]],
  ["6.4 WorkManager / DODO (SOAP)", ["WM-Form_Insert-01", "WM-Form_Insert-04", "WM-Form_GetParameters-01"]],
  ["6.5 BUA (SOAP)", ["BUA-001", "BUA-003", "BUA-005"]],
  ["6.6 Vigía (SOAP RPC/encoded)", ["VIGIA-001", "VIGIA-002"]],
  ["6.7 Fénix (SOAP, DataSet)", ["FENIX-001", "FENIX-003"]],
  ["6.8 Haberes (SOAP)", ["HABERES-GenerarReporte-01", "HABERES-ReporteHaberes-01"]],
  ["6.9 TransUnion (SOAP con estado)", ["TU-001", "TU-002", "TU-005", "TU-006"]],
  ["6.10 Dodo REST", ["DODO-Form_Insert-01", "DODO-Version-01"]],
  ["6.11 Pasarela ktrionApi", ["KTRION-Autenticacion-01", "KTRION-FormInsert-01", "KTRION-FormInsert-02"]],
  ["6.12 SMTP", ["SMTP-001", "SMTP-002"]],
  ["6.13 Azure Service Bus", ["SB-WorkManagerInsertRequest-01", "SB-CONSUMO-01"]],
  ["6.14 GA2 extremo a extremo", ["GA2-001", "GA2-005", "GA2-007"]],
  ["6.15 API de administración del mock", ["ADMIN-001"]],
];
GROUPS.forEach(([title, ids]) => { s3.push(H(title, HeadingLevel.HEADING_2)); ids.forEach((id) => s3.push(...detailedCase(id))); });

const blockedRows = results.filter((r) => r.status !== "APROBADA").map((r) => [r.id, r.provider, r.method, r.status, r.comment]);
s3.push(
  H("7. Resultados obtenidos", HeadingLevel.HEADING_1),
  P(`Ejecución del 19 de octubre de 2026 con \`run-test-cases.py\` (resultados en el Excel \`Resultados_Pruebas_MockIntegrations_GA2.xlsx\`). Se registraron **${results.length} casos**: **${count("APROBADA")} aprobados**, **${count("FALLIDA")} fallidos**, **${count("BLOQUEADA")} bloqueados** y **${count("NO EJECUTADA")} no ejecutados**. Los 153 métodos del Excel tienen al menos un caso. Las 210 pruebas automatizadas (\`dotnet test\`) pasaron.`),
  table(["Proveedor", "Casos", "APROBADA", "FALLIDA", "BLOQUEADA", "NO EJECUTADA"], Object.entries(provSummary).map(([k, v]) => [k, String(v.total), String(v.APROBADA), String(v.FALLIDA), String(v.BLOQUEADA), String(v["NO EJECUTADA"])]), [3100, 1100, 1350, 1300, 1350, PORTRAIT_W - 8200], { size: 17 }),
  H("7.1 Casos bloqueados y no ejecutados", HeadingLevel.HEADING_2),
  table(["ID", "Proveedor", "Método", "Estado", "Causa y desbloqueo"], blockedRows, [1500, 1500, 1900, 1300, PORTRAIT_W - 6200], { size: 15, fill: statusFill(3) }),
  H("7.2 Primera ejecución y correcciones", HeadingLevel.HEADING_2),
  P("La primera ejecución (guardada en `04_ejecucion_1_antes_de_correccion`) tuvo **4 casos fallidos**, que no se ocultaron:"),
  bullet("**SMTP-001 – defecto real del mock:** la API de inspección no decodificaba el nombre del adjunto en formato RFC 2231 (`filename*=utf-8''…`). Se corrigió `SmtpCaptureServer` y se añadió la prueba de regresión `Smtp_decodes_rfc2231_attachment_names_sent_by_other_clients` (210/210 pruebas). Con el cliente de GA2 (System.Net.Mail) ya funcionaba."),
  bullet("**BUA-001, BUA-002, BUA-005 – error de la herramienta de pruebas:** el validador leía la identificación del dependiente en lugar de la del afiliado. Se corrigió el validador; el mock no se modificó."),
  bullet("**GA2-007 (antes de la ejecución final) – limitación del entorno:** la base stub no tenía el procedimiento con el que GA2 registra la notificación de Fénix. Se añadió a la base stub y GA2-007 pasó, con la notificación verificada en la base de datos."),
  P("Ninguna corrección cambió el comportamiento de un mock para que una prueba pasara."),

  H("8. Incidencias", HeadingLevel.HEADING_1),
  table(["Incidencia", "Causa", "Acción necesaria", "Quién / qué recurso"], [
    ["Service Bus: GA2 descarta los eventos (D1) y el contenido «request» llega nulo (D2)", "Los 5 manejadores WorkManager*IntegrationEventHandler no están registrados en la inyección de dependencias; System.Text.Json ignora el campo público «request». Verificado en el código (Startup.cs:173-177, EventBusServiceBus.cs:186-189) y en SB-CONSUMO-01: el evento se consume y WorkManager recibe 0 llamadas.", "Registrar los manejadores en Autofac y deserializar incluyendo campos. Repetir SB-CONSUMO-01.", "Equipo de desarrollo de GA2"],
    ["TransUnion y BUA ws.01 con URL fija", "ValidacionesTU.cs:16 (producción) y ConsumoBua.cs:38 + Reference.cs (cpvmp-09x).", "Hacer las URLs configurables, o redirigir por DNS/hosts en red aislada (TransUnion además con certificado TLS de confianza).", "Equipo de GA2 / infraestructura"],
    ["SMTP de Identity (Outlook) no simulado", "LoginBusinessLogic.cs:355-356: smtp-mail.outlook.com:587 con STARTTLS fijo.", "Hacer configurable el host o implementar STARTTLS en el mock.", "Equipo de GA2"],
    ["Sin base de datos de pruebas de GA2", "El repositorio no incluye esquema ni datos; los flujos de negocio consultan procedimientos almacenados antes de llamar a la integración.", "Copia anonimizada de la base de pruebas.", "Equipo de base de datos de GA2"],
    ["Valores de negocio no confirmados", "statusJobTusDatos, riskLAFT (TusDatos), Encontrado/Listas (Vigía), DCResponse (TransUnion), raíz de los Result (Seven), formato del BarCode (WorkManager): son los configurados en el mock.", "Obtener muestras reales (tablas de auditoría, equipos de cada proveedor) y ajustar los fixtures.", "Dueños de cada sistema"],
    ["Escenarios «ner» y «malnoautorizado»", "No existen en el código; los correctos son «pep» y «unauthorized».", "Usar los nombres implementados.", "—"],
    ["Ficha BUA: dependiente con valores de relleno", "El fixture conserva textos como «identificacion» en BeneficiarioBEL.", "Opcional: valores realistas en tools/build-fixtures.py.", "Mantenedor del mock"],
    ["Sin capturas de Swagger", "Firefox en modo sin interfaz captura antes de que Swagger se dibuje.", "Las evidencias son las peticiones/respuestas reales.", "—"],
  ], [2400, 3200, 2400, PORTRAIT_W - 8000], { size: 15 }),

  H("9. Cómo comprobar una prueba manualmente (lista de verificación)", HeadingLevel.HEADING_1),
  ...[
    "El mock está arrancado: `http://localhost:5080/health` devuelve `{\"status\":\"ok\"}`.",
    "Estado limpio: ejecuté `POST http://localhost:5080/_mock/reset` (sobre todo antes de TusDatos, TransUnion y contadores de Seven/WorkManager).",
    "Elegí el documento del proveedor correcto en Swagger y la operación indicada en el caso.",
    "Para SOAP, el header SOAPAction coincide con la operación del cuerpo XML.",
    "Introduje exactamente los valores de «Datos de entrada» (documento de prueba, escenario, headers).",
    "El **código HTTP** coincide con el esperado (200, 404, 401, 500…).",
    "El header de respuesta `x-mock-scenario-applied` es el escenario esperado.",
    "Los **campos y valores** de la respuesta coinciden con la tabla de validaciones del caso (no basta con que responda 200).",
    "En operaciones asíncronas (TusDatos), consulté el job después de la respuesta inicial.",
    "Revisé `http://localhost:5080/_mock/journal` para confirmar que la llamada quedó registrada con el escenario y el código.",
    "Anoté el resultado: APROBADA si todas las validaciones se cumplen; FALLIDA si alguna no; BLOQUEADA si no se pudo ejecutar por una causa externa; NO EJECUTADA si se decidió no ejecutarla (indicar motivo).",
  ].map((s) => numbered(s, "checklist")),

  H("10. Evidencias y anexos", HeadingLevel.HEADING_1),
  P("Todas las evidencias corresponden a ejecuciones reales del 19 de octubre de 2026 y están en `~/Downloads/Evidencias_MockIntegrations_GA2/`:"),
  table(["Ubicación", "Contenido"], [
    ["00_entorno/", "Entorno (.NET, commit), arranque y comprobaciones, log del mock de pruebas, log de la API GA2, variables de entorno usadas por GA2, verificación en el código de los problemas conocidos, scripts usados (run-test-cases.py, stub-database.sql)"],
    ["01_tests_automatizados/", "build.log (0 advertencias, 0 errores), dotnet-test.log y resultados.trx (209/209 antes de la corrección), dotnet-test_tras_correccion.log y .trx (210/210)"],
    ["02_casos/<Proveedor>/<ID>.txt", "Petición completa (método, URL, headers, cuerpo) y respuesta completa (código, headers, cuerpo) de cada caso ejecutado (272 archivos)"],
    ["03_journal/", "journal_completo.json (282 llamadas registradas por el mock), resumen_journal.txt, ga2_auditoria_bd_stub.txt (auditoría y notificación escritas por GA2)"],
    ["04_ejecucion_1_antes_de_correccion/", "Resultados y consola de la primera ejecución con los 4 fallos, y sus evidencias"],
    ["05_capturas/LEEME.txt", "Explicación de por qué no hay capturas de Swagger"],
    ["resultados.json · consola_ejecucion_final.log", "Resultados estructurados y salida de la ejecución final"],
    ["~/Downloads/Resultados_Pruebas_MockIntegrations_GA2.xlsx", "Resultados (una fila por caso), resumen con fórmulas, inventario de 153 métodos con estado consolidado e incidencias"],
  ], [3600, PORTRAIT_W - 3600], { size: 16 }),
  P("Documentación técnica complementaria del repositorio: `MockIntegrations/docs/` (guía de configuración de GA2, escenarios, pendientes, contratos por proveedor) y `docs/integraciones/mocks/` (contratos de request/response)."),

  H("11. Conclusión", HeadingLevel.HEADING_1),
  bullet(`**Validado directamente contra el mock:** los 153 métodos del Excel. Todos los métodos de negocio y de reenvío responden según el contrato que GA2 espera, con sus escenarios de éxito, error funcional, no encontrado y error técnico (${count("APROBADA")} casos aprobados). Las 66 operaciones que GA2 no invoca responden un error explícito, salvo 5 de WorkManager con respuesta validada.`),
  bullet("**Validado de extremo a extremo con GA2 real:** TusDatos (consulta, 404 → data=null, Vigía), Dodo, Vigía SOAP, Fénix (con biometría y sin datos, incluida la notificación en base de datos) y Seven, a través de 8 endpoints de GA2."),
  bullet("**Validado con el código real de GA2** en las pruebas automatizadas: proxies WCF, DTO, el cliente de TransUnion y la lectura de DataSets (210 pruebas)."),
  bullet("**Pendiente:** el consumo de eventos de Service Bus (defecto de GA2), el SMTP de Identity, los flujos de GA2 que usan TransUnion o BUA ws.01 (URL fija a sistemas reales), los flujos completos que necesitan la base de datos de pruebas y la confirmación de los valores de negocio con muestras reales de los proveedores."),
);

const doc = new Document({
  creator: "MockIntegrations", title: "Guía de ejecución y validación de pruebas de MockIntegrations – GA2",
  styles: {
    default: { document: { run: { font: FONT, size: 21 } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true, run: { size: 32, bold: true, font: FONT, color: "1F3864" }, paragraph: { spacing: { before: 360, after: 160 }, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true, run: { size: 26, bold: true, font: FONT, color: "2F5496" }, paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 1 } },
      { id: "Heading3", name: "Heading 3", basedOn: "Normal", next: "Normal", quickFormat: true, run: { size: 22, bold: true, font: FONT, color: "1F3864" }, paragraph: { spacing: { before: 240, after: 100 }, outlineLevel: 2 } },
    ],
  },
  numbering: {
    config: [
      { reference: "bullets", levels: [{ level: 0, format: LevelFormat.BULLET, text: "•", alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 360 } } } }, { level: 1, format: LevelFormat.BULLET, text: "–", alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 1080, hanging: 360 } } } }] },
      ...["swagger1", "swagger2", "swagger3", "checklist", ...Array.from({ length: 80 }, (_, i) => `steps${i + 1}`)].map((reference) => ({ reference, levels: [{ level: 0, format: LevelFormat.DECIMAL, text: "%1.", alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 360 } } } }] })),
    ],
  },
  sections: [
    { properties: { page: { size: { width: 11906, height: 16838 }, margin: { top: 1134, bottom: 1134, left: 1134, right: 1134 } } }, footers: { default: footer() }, children: s1 },
    { properties: { page: { size: { width: 11906, height: 16838, orientation: PageOrientation.LANDSCAPE }, margin: { top: 1134, bottom: 1134, left: 1134, right: 1134 } } }, footers: { default: footer() }, children: s2 },
    { properties: { page: { size: { width: 11906, height: 16838 }, margin: { top: 1134, bottom: 1134, left: 1134, right: 1134 } } }, footers: { default: footer() }, children: s3 },
  ],
});
function footer() {
  return new Footer({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Guía de pruebas MockIntegrations – GA2 · Página ", font: FONT, size: 16, color: "7F7F7F" }), new TextRun({ children: [PageNumber.CURRENT], font: FONT, size: 16, color: "7F7F7F" })] })] });
}
if (stepRefs > 80) throw new Error("Aumente el número de listas numeradas: " + stepRefs);
Packer.toBuffer(doc).then((buf) => { fs.writeFileSync(OUT, buf); console.log(OUT, buf.length, "bytes; casos detallados:", stepRefs); });
