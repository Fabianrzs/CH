using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.Seven;

/// <summary>
/// E1 — Seven ERP (Digital Ware): 7 servicios ASMX, namespace <c>http://seven/</c>.
/// Contrato: docs/providers/E01-seven.md. Los <c>...Result</c> de negocio son un XML escapado con
/// <c>RETORNO</c> (0 = éxito, 1 = error) y <c>TXTERROR</c>; GA2 lee los consecutivos que devuelve.
/// </summary>
public static class SevenMock
{
    public const string Provider = "Seven";
    private const string Ns = "http://seven/";

    private static readonly ScenarioInfo ErrorNegocio = new("error-negocio", "RETORNO=1 con TXTERROR; GA2 detiene el flujo y muestra el texto.");

    public static void Map(WebApplication app)
    {
        foreach (var service in Services)
        {
            app.MapSoapService(service);
        }
    }

    public static IReadOnlyList<SoapService> Services { get; } =
    [
        Service("SCNMCONT", "SCNMCONTSoap", "/Seven/WsMovContable/scnmcont.asmx", "SvcUtils:SevenWSMovContable",
            "Movimientos contables (comprobantes). Lo usan Cartera, Crédito, Cesantías y Reintegros, además del proxy alterno `Connected Services/ServiceSeven` (mismo XML).",
            "ReferenciasSvc/Seven/SCnMcont/ConsumoSCnMcont.cs",
            Op("InsertarCnMcont", Ga2Usage.PassThrough, "Inserta la cabecera de un comprobante contable.", ErrorNegocio),
            Op("InsertarCnDmcon", Ga2Usage.PassThrough, "Inserta una línea de detalle de un comprobante.", ErrorNegocio),
            Op("AplicarCnMcont", Ga2Usage.PassThrough, "Aplica (contabiliza) un comprobante.", ErrorNegocio),
            Op("RevertirCnMcont", Ga2Usage.PassThrough, "Revierte un comprobante aplicado.", ErrorNegocio),
            Op("AnularCnMcont", Ga2Usage.Business, "Anula un comprobante. GA2 (Cartera) lee RETORNO con una expresión regular.", ErrorNegocio),
            Op("InsertarMovContable", Ga2Usage.Business, "Registra un comprobante completo (cabecera + vDetalle). GA2 guarda MCO_CONT y MCO_NUME.", ErrorNegocio),
            Op("SetContabToper", Ga2Usage.PassThrough, "Contabiliza por tipo de operación; devuelve TRetToper.")),

        Service("SPGOBLIG", "SPGOBLIGSoap", "/Seven/WsCdp/WPgOblig.asmx", "SvcUtils:SevenWSCdp",
            "Presupuesto: certificados de disponibilidad presupuestal (CDP).",
            "ReferenciasSvc/Seven/WSCDP/ConsumoCDP.cs",
            Op("InsertarCDP", Ga2Usage.PassThrough, "Inserta la cabecera de un CDP.", ErrorNegocio),
            Op("InsertarDetCDP", Ga2Usage.PassThrough, "Inserta el detalle de un CDP.", ErrorNegocio),
            Op("AplicarCDP", Ga2Usage.PassThrough, "Aplica un CDP.", ErrorNegocio),
            Op("AnularCDP", Ga2Usage.PassThrough, "Anula un CDP.", ErrorNegocio),
            Op("CrearCDP", Ga2Usage.Business, "Crea un CDP completo (pagos de crédito, cesantías, hipoteca, embargos, reintegros). GA2 guarda MPR_CONT.", ErrorNegocio)),

        Service("SPOFACTU", "SPOFACTUSoap", "/Seven/Wsfactura/spofactu.asmx", "SvcUtils:SevenWSFactura",
            "Facturas y cuentas por pagar.",
            "ReferenciasSvc/Seven/WSFactura/ConsumoWSFactura.cs",
            Op("InsertarFacturaP", Ga2Usage.Business, "Crea la cuenta por pagar del pago. GA2 lee FAC_CONT y FAC_NUME (si faltan: NullReferenceException).",
                ErrorNegocio, new ScenarioInfo("sin-identificadores", "RETORNO=0 sin FAC_CONT/FAC_NUME: reproduce el NullReferenceException de GA2.")),
            Op("AnularFactura", Ga2Usage.PassThrough, "Anula una factura.", ErrorNegocio),
            Op("InsertarPoFactu", Ga2Usage.PassThrough, "Inserta una factura de proveedor (estructura TSPoFactu1).", ErrorNegocio),
            Op("AnularPoFactu", Ga2Usage.PassThrough, "Anula una factura de proveedor.", ErrorNegocio),
            Op("InsertarPoFactuP", Ga2Usage.PassThrough, "Inserta una factura de proveedor (estructura TSPoFactu2).", ErrorNegocio)),

        Service("SIEWSSEC", "SIEWSSECSoap", "/Seven/Wsproveedor/SIeWssec.asmx", "SvcUtils:SevenWSProveedor",
            "Sincronización de terceros, clientes y proveedores.",
            "ReferenciasSvc/Seven/WSProveedor/ConsumoWSProveedor.cs",
            Op("SyncGnTerce", Ga2Usage.PassThrough, "Sincroniza un tercero.", ErrorNegocio),
            Op("SyncFaClien", Ga2Usage.PassThrough, "Sincroniza un cliente.", ErrorNegocio),
            Op("SyncFaClienComplex", Ga2Usage.PassThrough, "Sincroniza un cliente; devuelve TSalida (Retorno, TxtError, CliCodi, DclCodd)."),
            Op("SyncPoPvdor", Ga2Usage.Business, "Crea/actualiza el proveedor del pago. GA2 lee PVD_CODD.", ErrorNegocio),
            Op("SyncFaConta", Ga2Usage.PassThrough, "Sincroniza contactos del cliente.", ErrorNegocio),
            Op("SyncEmpleadoPvdor", Ga2Usage.PassThrough, "Sincroniza un empleado como proveedor.", ErrorNegocio),
            Op("ChangeNit", Ga2Usage.PassThrough, "Cambia el NIT de un tercero.", ErrorNegocio),
            Op("ActModalidadCupo", Ga2Usage.PassThrough, "Actualiza la modalidad de cupo.", ErrorNegocio)),

        Service("SGNTERCE", "SGNTERCESoap", "/Seven/Wstercero/wgnterce.asmx", "SvcUtils:SevenWSTercero",
            "Registro de terceros. Primer paso del flujo de pago: InsertarGnTerce → SyncPoPvdor → CrearCDP → InsertarFacturaP → InsertarMovContable.",
            "ReferenciasSvc/Seven/WSTercero/ConsumoWSTercero.cs",
            Op("InsertarGnTerce", Ga2Usage.Business, "Registra el tercero. GA2 lee //GN_TERCE/RETORNO (la raíz GN_TERCE es obligatoria).",
                ErrorNegocio, new ScenarioInfo("tercero-existente", "RETORNO=1 'El tercero ya se encuentra registrado' (GA2 continúa el flujo)."))),

        Service("SINTIPRO", "SINTIPROSoap", "/Seven/Wintipro/WInTipro.asmx", "SvcUtils:SevenWintipro",
            "Cuentas contables por tipo de operación.",
            "ReferenciasSvc/Seven/WSWintipro/ConsumoWSWintipro.cs",
            Op("GetCuentaContable", Ga2Usage.PassThrough, "Devuelve la cuenta contable (string). Contenido de ejemplo: semántica no confirmada.", certainty: ContractCertainty.StructureOnly),
            Op("GetCuentaContableTO", Ga2Usage.PassThrough, "Devuelve la cuenta contable por tipo de operación (string). Contenido de ejemplo: semántica no confirmada.", certainty: ContractCertainty.StructureOnly)),

        Service("ValidarAportes", "ValidarAportesSoap", "/Seven/WSaportes/ValidarAportes.asmx", "SvcUtils:SevenWSAportes",
            "Validación de aportes.",
            "ReferenciasSvc/Seven/WSAportes/ConsumoWSAportes.cs",
            Op("GetAporte", Ga2Usage.PassThrough, "Devuelve el aporte (string). Contenido de ejemplo: semántica no confirmada.", certainty: ContractCertainty.StructureOnly)),
    ];

    private static SoapService Service(string id, string contract, string path, string configKey, string description, string source, params SoapOperation[] ops) =>
        new()
        {
            Provider = Provider,
            Id = $"Seven_{id}",
            Contract = contract,
            Paths = [path],
            Description = description,
            Ga2ConfigKey = configKey,
            Operations = ops.Select(o => new SoapOperation
            {
                Name = o.Name,
                SoapAction = o.SoapAction,
                Usage = o.Usage,
                Summary = o.Summary,
                Scenarios = o.Scenarios,
                Certainty = o.Certainty,
                Ga2Source = source,
            }).ToList(),
        };

    private static SoapOperation Op(string name, Ga2Usage usage, string summary, params ScenarioInfo[] scenarios) =>
        Op(name, usage, summary, ContractCertainty.Validated, scenarios);

    private static SoapOperation Op(string name, Ga2Usage usage, string summary, ContractCertainty certainty, params ScenarioInfo[] scenarios) =>
        new()
        {
            Name = name,
            SoapAction = Ns + name,
            Usage = usage,
            Summary = summary,
            Scenarios = scenarios,
            Certainty = ReturnsRetornoXml(name, scenarios) ? ContractCertainty.StructureOnly : certainty,
        };

    // RETORNO/TXTERROR y los consecutivos se validaron contra la lógica de GA2, pero el nombre del
    // elemento raíz del XML escapado (salvo GN_TERCE, que GA2 exige) no está confirmado con Seven.
    private static bool ReturnsRetornoXml(string name, ScenarioInfo[] scenarios) =>
        scenarios.Contains(ErrorNegocio) && name != "InsertarGnTerce";
}
