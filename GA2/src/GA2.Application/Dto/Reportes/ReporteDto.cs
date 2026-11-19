          using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GA2.Application.Dto
{

    /// <summary>
    /// DTO para Reportes F453Dto
    /// </summary>
    public class ReporteSFCF453Y454Dto
    {
        public string consecutivo_formateado { get; set; }
        public string cuenta_formateada { get; set; }
        public string formato { get; set; }
        public string Columna1 { get; set; }
        public string columna2_formateada { get; set; }
        public string subcuenta { get; set; }
        public string valor_formateado { get; set; }
    }

    /// <summary>
    /// DTO para Reportes F416
    /// </summary>
    public class ReporteSFCF416Dto
    {
        public int ID { get; set; }
        public string SUBCUENTA { get; set; }
        public int TIPO_SOLICITUD { get; set; }
        public int ESTADO_SOLICITUD { get; set; }
        public int CAUSAL_RECHAZO { get; set; }
        public int MODALIDAD_CREDITO { get; set; }
        public string CODIGO_DEPARTAMENTO { get; set; }
        public string CODIGO_CIIU { get; set; }
        public decimal MONTO_SOLICITUDES { get; set; }
        public int NUMERO_SOLICITUDES { get; set; }
        public string UNIDAD_CAPTURA { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
    }

    /// <summary>
    /// DTO para Reporte Plano F477
    /// </summary>
    public class ReporteSFCF477Dto
    {
        public int Orden { get; set; }

        public int TipoRegistro { get; set; }

        public string Registro { get; set; }
    }

    public class Resumen477Dto
    {
        public int Orden { get; set; }
        public string Columna { get; set; }
        public string UnidadCaptura { get; set; }
        public string Subcuenta { get; set; }
        public decimal Valor { get; set; }
    }
    public class Resumen536Dto
    {
        public string UnidadCaptura { get; set; }
        public int TipoDocumento { get; set; }
        public long Identificacion { get; set; }
        public long NumeroCredito { get; set; }
        public string TipoNovedad { get; set; }
        public DateTime FechaNovedad { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public decimal CapitalNovedad { get; set; }
        public decimal InteresCorriente { get; set; }
        public decimal InteresMora { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal TotalDeuda { get; set; }
        public decimal NuevaTasa { get; set; }
        public decimal SaldoCapital { get; set; }
        public int PlazoActual { get; set; }
        public decimal TasaEa { get; set; }
        public decimal Provision { get; set; }
        public string Calificacion { get; set; }
    }

    /// <summary>
    /// DTO para Reportes F414
    /// </summary>
    public class ReporteSFCF414Dto
    {
        public int ID { get; set; }
        public string SUBCUENTA { get; set; }
        public string TIPO_PERSONA { get; set; }
        public string SEXO { get; set; }
        public string TAMANO_EMPRESA { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string TIPO_GARANTIA { get; set; }
        public string PRODUCTO_CREDITO { get; set; }
        public int PLAZO_CREDITO { get; set; }
        public decimal TASA_EFECTIVA_PROMEDIO_PONDERADA { get; set; }
        public decimal MARGEN_ADICIONAL_VARIACIÓN_CREDITOS_UVR { get; set; }
        public decimal MONTOS_DESEMBOLSADOS { get; set; }
        public int NUMERO_CREDITOS_DESEMBOLSADOS { get; set; }
        public string UNIDAD_CAPTURA { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int CRE_CIUDAD { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string CIUDAD { get; set; }
        public string RMD { get; set; }
    }

    /// <summary>
    /// DTO para Reportes Aseguradora
    /// </summary>
    public class ReporteAseguradoraDto
    {
        public string Documento { get; set; }
        public string NombreAfiliado { get; set; }
        public string Inmueble { get; set; }
        public string Ciudad { get; set; }
        public decimal ValorAsegurado { get; set; }
        public decimal PrimaSeguroPositiva { get; set; }
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
    }

    /// <summary>
    /// DTO para ReporteEstudioTitulos
    /// </summary>
    public class ReporteEstudioTitulosDto
    {
        public string Documento { get; set; }
        public string NombreAfiliado { get; set; }
        public DateTime FechaContacto { get; set; }
        public DateTime FechaCreacionCaso { get; set; }
        public string Observaciones { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaEntregaET { get; set; }
        public DateTime FechaEnvioContrato { get; set; }
        public string Notaria { get; set; }
        public DateTime FechaEnvioMinuta { get; set; }
        public DateTime FechaFirmaEscritura { get; set; }
        public string SalidaRegistro { get; set; }
        public decimal ValorComercial { get; set; }
        public string NombreVendedores { get; set; }
        public string DocumentoVendedores { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
    }

    /// <summary>
    /// DTO para Reporte Cesiones
    /// </summary>
    public class ReporteCesionesDto
    {
        public string Documento { get; set; }
        public string NombreAfiliado { get; set; }
        public DateTime FechaContacto { get; set; }
        public DateTime FechaCreacionCaso { get; set; }
        public string Observaciones { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaEntregaET { get; set; }
        public DateTime FechaEnvioDocumentos { get; set; }
        public string Notaria { get; set; }
        public DateTime FechaEnvioMinuta { get; set; }
        public DateTime FechaFirmaEscritura { get; set; }
        public string SalidaRegistro { get; set; }
        public decimal ValorComercial { get; set; }
        public string NombreVendedores { get; set; }
        public string DocumentoVendedores { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
    }

    /// <summary>
    /// DTO para Reporte Consultas Restrictivas
    /// </summary>
    public class ReporteConsultasRestrictivasDto
    {
        public string Documento { get; set; }
        public string NombreAfiliado { get; set; }
        public string NombreVendedor { get; set; }
        public string DocumentoVendedor { get; set; }
        public int NroFolio { get; set; }
        public string ResultadoConsulta { get; set; }
    }

    /// <summary>
    /// DTO para ReporteAvaluos
    /// </summary>
    public class ReporteAvaluosDto
    {
        public string Documento { get; set; }
        public string NombreAfiliado { get; set; }
        public string PuntoRadicacion { get; set; }
        public string Responsable { get; set; }
        public DateTime FechaInicioProceso { get; set; }
        public string NroRadicado { get; set; }
        public string FirmaAvaluo { get; set; }
        public string ResultadoAvaluo { get; set; }
        public DateTime FechaRevisionAvaluo { get; set; }
        public string Vetustez { get; set; }
        public int Nrofolio { get; set; }
        public string ResultadoConsulta { get; set; }
        public string TipoVivienda { get; set; }
        public string Parqueadero { get; set; }
        public string Deposito { get; set; }
        public string Ciudad { get; set; }
        public string EstadoInmueble { get; set; }
        public string AreaInmueble { get; set; }
        public string NombresVendedor { get; set; }
        public decimal ValorVenta { get; set; }
        public decimal ValorComercial { get; set; }
        public decimal Diferencia { get; set; }
    }

    /// <summary>
    /// Dto para Reporte Carga Analista
    /// </summary>
    public class ReporteCargaAnalistaDto
    {
        public string NumeroRadicado { get; set; }
        public string PuntoRadicacion { get; set; }
        public string Documento { get; set; }
        public DateTime FechaRadicacion { get; set; }
        public string Proceso { get; set; }
        public DateTime FechaRecepcionAnalista { get; set; }
        public DateTime FechaAnalisis { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string TipoCredito { get; set; }
        public decimal ValorSolicitud { get; set; }
    }

    /// <summary>
    /// Dto para Reporte Valor Venta
    /// </summary>
    public class ReporteValorVentaDto
    {
        public string TipoVivienda { get; set; }
        public string Vetustez { get; set; }
        public string CiudadInmueble { get; set; }
        public decimal ValorCredito { get; set; }
        public int PlazoMeses { get; set; }
        public int Antiguedad_afiliado { get; set; }
        public int Edad { get; set; }
        public decimal ValorCtaIndividual { get; set; }
        public decimal ValorRecursosPropios { get; set; }
        public decimal ValorVenta { get; set; }
        public decimal ValorAvaluo { get; set; }
        public decimal Diferencia { get; set; }
        public decimal Calculo { get; set; }

    }

    /// <summary>
    /// DTO para Reportes de Cartera
    /// </summary>
    public class ReporteCarteraDto
    {
        public string NroCredito { get; set; }
        public string Identificacion { get; set; }
        public string IdentificacionExpedida { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime LugarNacimiento { get; set; }
        public int Edad { get; set; }
        public string EstadoCivil { get; set; }
        public string Categoria { get; set; }
        public string Grado { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaFinalizaPagos { get; set; }
        public int TCR_ID { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal PlazoTotal { get; set; }
        public decimal PlazoActual { get; set; }
        public decimal TasaCreditoEA { get; set; }
        public decimal TasaSeguroVida { get; set; }
        public decimal TasaSeguroHogar { get; set; }
        public decimal TasaCreditoPER { get; set; }
        public decimal TasaFrech { get; set; }
        public decimal ValorAlivioCuota { get; set; }
        public decimal TipoAabonoExtra { get; set; }
        public decimal ValorAbonoExtra { get; set; }
        public int TIV_ID { get; set; }
        public int TVV_ID { get; set; }
        public decimal ValorCanonInicial { get; set; }
        public decimal CanonExtraordinario { get; set; }
        public int Score { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal ValorCuota { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaProximaPago { get; set; }
        public int ESC_ID { get; set; }
        public string ClaseInmueble { get; set; }
        public string DireccionInmueble { get; set; }
        public string CiudadInmueble { get; set; }
        public string NumeroMatricula { get; set; }
        public string EscrituraInmueble { get; set; }
        public DateTime FechaEscritura { get; set; }
        public string Notaria { get; set; }
        public string Linderos { get; set; }
        public string DescripcionInmueble { get; set; }
        public decimal ValorVivienda { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public decimal AcumuladoInteresCorriente { get; set; }
        public decimal AcumuladosSeguroVida { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }
        public decimal ValorDeudaRemanente { get; set; }
        public decimal Provision { get; set; }
        public decimal ProvisionPorcentaje { get; set; }
        public string Calificacion { get; set; }
    }

    /// <summary>
    /// Dto para Reporte General Cartera
    /// </summary>
    public class ReporteGeneralCarteraDto
    {
        public string NumeroRadicacion { get; set; }
        public string Pagare { get; set; }
        public string Cliente { get; set; }
        public string Estado { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Pagaduria { get; set; }
        public string NombrePagaduria { get; set; }
        public string Apertura { get; set; }
        public string Terminacion { get; set; }
        public decimal TasaLinea { get; set; }
        public string Destinacion { get; set; }
        public decimal Desembolso { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public decimal Cuota { get; set; }
        public int Dias { get; set; }
        public String UltimoPago { get; set; }
        public String PróximoPago { get; set; }
        public decimal MoraInterior { get; set; }
        public decimal Interes { get; set; }
        public string Seguros { get; set; }
        public decimal Capital { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal ValorInmueble { get; set; }
        public int PlazoObligacion { get; set; }
        public string Calificacion { get; set; }
        public int NumeroCuotasFaltantes { get; set; }
        public decimal ValorCuotaExtraordinaria { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public string CiudadInmueble { get; set; }
        public decimal PuntajeScoring { get; set; }

        public object fileExcel { get; set; }


    }

    /// <summary>
    /// Dto para ReporteGeneralGarantias
    /// </summary>
    public class ReporteGeneralGarantiasDto
    {
        public string NumeroRadicacion { get; set; }
        public string TipoCredito { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CiudadInmueble { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaTerminacion { get; set; }
        public decimal ValorInmueble { get; set; }
        public decimal AjusteGarantia { get; set; }
        public decimal ValorAjustado { get; set; }

        public byte[] file { get; set; }
        public string DireccionInmuble { get; set; }
        public string NumeroEscrituraInmueble { get; set; }
        public string MatriculaInmueble { get; set; }

        public decimal PorcentajeGarantias { get; set; }
    }

    /// <summary>
    /// Dto para Reporte Preliminar Cartera
    /// </summary>
    public class ReportePreliminarCarteraDto
    {
        public Int64 Nro { get; set; }
        public String NumeroRadicacion { get; set; }
        public String Identificacion { get; set; }
        public String Nombres { get; set; }
        public String Cliente { get; set; }
        public String NombrePagaduria { get; set; }
        public Double Cuota { get; set; }
        public String UltimoPago { get; set; }
        public Double ValorAjustado { get; set; }
        public String Apertura { get; set; }
        public Double Monto { get; set; }
        public Double Saldo { get; set; }
        public Double ValorInmueble { get; set; }
        public Double TasaLinea { get; set; }
        public String Terminacion { get; set; }

        public object fileExcel { get; set; }
    }

    /// <summary>
    /// Dto para Reporte Seguro Hogar
    /// </summary>
    public class ReporteSeguroHogarDto
    {
        public string CiudadInmueble { get; set; }
        public string CodigoCliente { get; set; }
        public string DireccionInmueble { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string SeguroHogar { get; set; }
        public decimal ValorInmueble { get; set; }
    }

    /// <summary>
    /// Dto para Reporte Seguro Vida
    /// </summary>
    public class ReporteSeguroVidaDto
    {
        public string CiudadInmueble { get; set; }
        public string CodCliente { get; set; }
        public string DireccionInmueble { get; set; }
        public string Identificacion { get; set; }
        public decimal MontoCapital { get; set; }
        public decimal SeguroVida { get; set; }
        public string Nombre { get; set; }
        public DateTime TerminacionCredito { get; set; }

    }

    /// <summary>
    /// Dto para Reporte Descuentos Nomina
    /// </summary>
    public class ReporteDescuentosNominaDto
    {
        public int Nro { get; set; }
        public string NumeroRadicacion { get; set; }
        public string Cliente { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Pagaduria { get; set; }
        public string NombrePagaduria { get; set; }
        public string Apertura { get; set; }
        public string Terminacion { get; set; }
        public decimal TasaLinea { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public decimal Cuota { get; set; }
        public decimal UltimoPago { get; set; }
        public decimal ProximoPago { get; set; }
        public int PlazoObligacion { get; set; }
    }

    /// <summary>
    /// DTO para Reportes de Mora
    /// </summary>
    public class ReporteDeMoraDto
    {
        public string NumeroRadicacion { get; set; }
        public string Estado { get; set; }
        public string CodigoCliente { get; set; }
        public string Nombres { get; set; }
        public string CodigoEmpresa { get; set; }
        public string Empresa { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal ValorCuota { get; set; }
        public string CodPeriodicidad { get; set; }
        public string Periodicidad { get; set; }
        public string TipoRecaudo { get; set; }
        public decimal CuotaVencida { get; set; }
        public DateTime VencimientoProximoPago { get; set; }
        public decimal UltimoPago { get; set; }
        public DateTime VencimientoUltimoPago { get; set; }
        public decimal ValorUltimoPago { get; set; }
    }

    /// <summary>
    /// DTO Parametros para Reportes Superfinanciera
    /// </summary>
    public class ParametrosSuperfinancieraReportesDto
    {
        public DateTime FechaAplicacion { get; set; }
    }

    /// <summary>
    /// DTO para cargar listado de reportes
    /// </summary>
    public class CargarListadoReportesDto
    {

        public int IdReporte { get; set; }
        public string NombreReporte { get; set; }
        public string TipoReporte { get; set; }
        public string TipoParametro { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Cliente
    /// </summary>
    public class InformacionClienteDto
    {
        public int CLI_ID { get; set; }
        public string CODIGO { get; set; }
        public int TID_ID { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CLI_NOMBRES { get; set; }
        public string DRC_DIRECCION { get; set; }
        public string DTL_TELEFONO { get; set; }
        public string DCE_CORREO { get; set; }
        public string CONTRATO { get; set; }
        public int TPA_ID { get; set; }
        public string TIPO_PERSONA { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public string CIUDAD { get; set; }
        public DateTime CLI_FECHA_EXPEDICION { get; set; }
        public int CTG_ID { get; set; }
        public int UEJ_ID { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Cliente
    /// </summary>
    public class InformacionClienteExtractoAnualDto
    {
        public int ID_CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string IDENTIFICACION { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string CONTRATO { get; set; }
        public string TIPO_PERSONA { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public string CIUDAD { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Financiera Extracto Anual
    /// </summary>
    public class InformacionFinancieraExtractoAnualDto
    {
        public decimal TOTAL_CUOTA { get; set; }
        public decimal CUOTAS_PAGADAS { get; set; }
        public decimal CUOTAS_PENDIENTES { get; set; }
        public decimal CUOTAS_MORA { get; set; }
        public decimal INTERES_CORRIENTE_EA { get; set; }
        public decimal INTERES_MORA_EA { get; set; }
        public decimal FECHA_CORTE { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
    }

    /// <summary>
    /// Dto para Proyeccion Anual Extracto Anual
    /// </summary>
    public class ProyeccionAnualExtractoAnualDto
    {
        public decimal CUOTA { get; set; }
        public DateTime FECHA_CUOTA { get; set; }
        public decimal CAPITAL_APAGAR { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public decimal INTERES_APAGAR { get; set; }
        public decimal INTERES_REMANENTE { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public int DIAS_MORA { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal OTROS_SEGUROS { get; set; }
        public decimal OTROS_GASTOS { get; set; }
    }

    /// <summary>
    /// Dto para Relacion Pagos Extracto Anual
    /// </summary>
    public class RelacionPagosExtractoAnualDto
    {
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public decimal VALOR_PAGADO { get; set; }
        public decimal CAPITAL { get; set; }
        public decimal REMANENTE { get; set; }
        public decimal INTERESES_CORRIENTES { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal SEGURO_TODO_RIESGO { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SALDO_CUOTA { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Cliente Extracto Mensual
    /// </summary>
    public class InformacionClienteExtractoMensualDto
    {
        public string NOMBRE { get; set; }
        public string EMAIL { get; set; }
        public string NRO_CONTRATO { get; set; }
        public string CIUDAD { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Cuotas Extracto Mensual
    /// </summary>
    public class InformacionCuotasExtractoMensualDto
    {
        public decimal TOTAL_CUOTA { get; set; }
        public decimal CUOTAS_PAGADAS { get; set; }
        public decimal CUOTAS_PENDIENTES { get; set; }
        public decimal CUOTAS_MORA { get; set; }
        public decimal INTERES_CORRIENTE_EA { get; set; }
        public decimal INTERES_MORA_EA { get; set; }
        public string FECHA_CORTE { get; set; }
        public decimal SALDO { get; set; }
        public decimal PLAZO_TOTAL { get; set; }
    }

    /// <summary>
    /// Dto Informacion Valores Extracto Mensual
    /// </summary>
    public class InformacionValoresExtractoMensualDto
    {
        public decimal INTERES_MORA { get; set; }
        public decimal SEGUROS { get; set; }
        public decimal CAPITAL { get; set; }
        public decimal INTERES_CORRIENTES { get; set; }
        public decimal ANTICIPOS { get; set; }
        public decimal OTROS_CARGOS { get; set; }
        public decimal FRESH { get; set; }
        public decimal TOTAL_APLICADO { get; set; }

    }

    /// <summary>
    /// Dto para Informacion Discriminada Extracto Mensual
    /// </summary>
    public class InformacionDiscriminadaExtractoMensualDto
    {
        public decimal CAPITAL { get; set; }
        public decimal INTERES_CORRIENTE { get; set; }
        public decimal INTERES_MORA { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_TODO_RIESGO { get; set; }
        public decimal OTROS_GASTOS { get; set; }
        public decimal TOTAL_PAGAR { get; set; }
        public DateTime PAGUE_ANTES_DE { get; set; }
        public int NUMERO_CREDITO { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string AMORTIZACION { get; set; }
        public string PERIODO { get; set; }

    }

    /// <summary>
    /// Dto para Informacion Cliente Certificado IYR
    /// </summary>
    public class InformacionClienteCertificadoIYRDto
    {
        public string TITULAR { get; set; }
        public string IDENTIFICACION { get; set; }
        public string CONTRATO { get; set; }
        public string NRO_CONTRATO { get; set; }
    }

    /// <summary>
    /// Dto Informacion Montos Certificado IYR
    /// </summary>
    public class InformacionMontosCertificadoIYRDto
    {
        public decimal DEDUCCION_INDIVIDUAL { get; set; }
        public decimal COSTOS_FINANCIEROS { get; set; }
        public decimal CANONES_PERIODO { get; set; }
        public decimal SALDO_ACTUAL { get; set; }
        public decimal SALDO_INICIAL { get; set; }
        public decimal MONTO_INICIAL { get; set; }
        public DateTime FECHA_SALDO_INICIAL { get; set; }
        public DateTime FECHA_MONTO_INICIAL { get; set; }
        public DateTime FECHA_SALDO_ACTUAL { get; set; }
    }

    /// <summary>
    /// Dto Informacion Basica Certificado Haberes
    /// </summary>
    public class InformacionBasicaCertificadoHaberesDto
    {
        public string TITULAR { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string IDENTIFICACION { get; set; }
        public string TIPO_CREDITO { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public decimal SALDO { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Basica Estado Cuenta 
    /// </summary>
    public class InformacionBasicaEstadoCuentaDto
    {
        public string CODIGO { get; set; }
        public string NOMBRES { get; set; }
        public string IDENTIFICACION { get; set; }
        public string DIRECCION { get; set; }
        public DateTime FECHA_ESTADO { get; set; }
        public string TIPO_CLIENTE { get; set; }
        public string TELEFONO { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Obligacion Estado Cuenta 
    /// </summary>
    public class InformacionObligacionEstadoCuentaDto
    {
        public string NRO_OBLIGACION { get; set; }
        public string PRODUCTO { get; set; }
        public decimal MONTO { get; set; }
        public decimal CUOTA { get; set; }
        public decimal SALDO { get; set; }
        public string ESTADO_CREDITO { get; set; }
        public decimal SALDO_CONGELADO { get; set; }
        public decimal SALDO_CANON { get; set; }
        public DateTime FECHA_EMISION { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public string MODALIDAD { get; set; }
        public decimal TASA_EFECTIVA { get; set; }
        public decimal TASA_NOMINAL { get; set; }
        public DateTime FECHA_PPAGO { get; set; }
        public DateTime FECHA_ACUERDO { get; set; }
    }

    /// <summary>
    /// Dto para Informacion Distribucion Valores Estado Cuenta
    /// </summary>
    public class InformacionDistribucionValoresEstadoCuentaDto
    {
        public decimal CUOTA { get; set; }
        public DateTime FECHA_CUOTA { get; set; }
        public decimal CAPITAL_APAGAR { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public decimal INTERES_APAGAR { get; set; }
        public decimal INTERES_REMANENTE { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public int DIAS_MORA { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal OTROS_SEGUROS { get; set; }
        public decimal OTROS_GASTOS { get; set; }
    }

    public class InformacionMovimientosCarteraDetalleDto
    {
        public DateTime FECHA_CUOTA { get; set; }
        public int CUOTAS_CANCELADAS { get; set; }
        public string TIPO_MOVIMIENTO { get; set; }
        public decimal VALOR_CAPITAL { get; set; }
        public decimal INTERESES_CORRIENTES { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public string NUMERO_COMPROBANTE { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal OTROS_CONCEPTOS { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
    }

    public class InformacionMovimientosCarteraDto
    {
        public int PLAZO_TOTAL { get; set; }
        public decimal VALOR_VIVIENDA { get; set; }
        public decimal CUOTA { get; set; }
        public decimal SALDO { get; set; }
        public DateTime FECHA_ULTIMO_PAGO { get; set; }
        public DateTime FECHA_PROXIMO_PAGO { get; set; }
        public decimal CANON_EXTRA { get; set; }
        public decimal TASA_HISTORICA { get; set; }
        public decimal FRECH { get; set; }
        public decimal VALOR_DESEMBOLSO { get; set; }
        public decimal OPCION_COMPRA { get; set; }
        public string NRO_TITULO { get; set; }
        public string PRODUCTO { get; set; }
        public DateTime FECHA_EMISION { get; set; }
        public string PERIODO { get; set; }
        public decimal VALOR_BIEN { get; set; }
    }

    /// <summary>
    /// Dto para Obtener Respuesta Embargo 
    /// </summary>
    public class ObtenerRespuestaEmbargoDto
    {
        public string JUZGADO { get; set; }
        public string CORREO_JUZGADO { get; set; }
        public string RADICADO { get; set; }
        public string TIPO_PROCESO { get; set; }
        public string NUMERO_PROCESO { get; set; }
        public string NOMBRE_DEMANDANTE { get; set; }
        public string CEDULA_DEMANDANTE { get; set; }
        public string NOMBRE_DEMANDADO { get; set; }
        public string CEDULA_DEMANDADO { get; set; }
        public string NUMERO_OFICIO_JUZGADO { get; set; }
        public DateTime FECHA_RADICADO_JUZGADO { get; set; }
        public DateTime FECHA_RADICADO { get; set; }
        public decimal VALOR_EMBARGO { get; set; }
        public string NUMERO_EXPEDIENTE { get; set; }
        public string? DESCRIPCION { get; set; }
        public string EBA_OBSERVACIONES { get; set; }
        public List<EmbargoCuentaConceptoDto>? Conceptos { get; set; }
    }



    public class ReportePreliminarGarantiasDto
    {
        public string Radicacion { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CiudadInmueble { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaTerminacion { get; set; }
        public decimal ValorInmueble { get; set; }
        public decimal AjusteGarantia { get; set; }
        public decimal ValorAjustado { get; set; }
    }

    public class TransferenciaCesantiasDto
    {
        public DateTime FECHA_ACTUAL { get; set; }
        public DateTime AFL_FECHA_RADICADO { get; set; }
        public string AFL_NUMERO_RADICADO { get; set; }
        public string GENERO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public int TIPO_DOCUMENTO { get; set; }
        public string DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
    }

    public class OficioImprocedenteDto
    {
        public DateTime FECHA_ELABORACION { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string CIUDAD { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string FUERZA { get; set; }
        public string CIUDAD_RESIDENCIA { get; set; }
        public string DEPARTAMENTO_RESIDENCIA { get; set; }
        public string BARRIO { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string CORREO { get; set; }
    }

    public class ParametrosReportesAfiliacionesDto
    {
        public string IdAfiliacion { get; set; }
        public int OpcionReporte { get; set; }
    }

    public class OficiosAfiliacionDto
    {
        public string Identificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string NumeroRadicado { get; set; }
        public string MesActivacionDescuento { get; set; }
        public int RatificacionCuotas { get; set; }
        public string NombreBeneficiario { get; set; }
        public int RatificacionCuotasContinuas { get; set; }
        public int Categoria { get; set; }
        public int Fuerza { get; set; }
        public DateTime FechaPrimeraCuota { get; set; }
        public DateTime AportesDesde { get; set; }
        public DateTime AportesHasta { get; set; }
        public int DerechoLiquidacionCuotas { get; set; }
    }


    public class ReporteCierreMensualCarteraDto
    {
        public Int64 NumeroCredito { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Categoria { get; set; }
        public string Grado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TipoCredito { get; set; }
        public int TipoCreditoId { get; set; }
        public string EstadoCredito { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaFinalizaPagos { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal ValorVivienda { get; set; }
        public string TipoVivienda { get; set; }
        public string ClasificacionVivienda { get; set; }
        public decimal CanonInicial { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public decimal PorcentajeCanonInicial { get; set; }
        public decimal PorcentajeOpcionCompra { get; set; }
        public decimal CanonExtraordinario { get; set; }
        public int PlazoTotal { get; set; }
        public int PlazoActual { get; set; }
        public int CuotasCanceladas { get; set; }
        public decimal TasaSeguroVida { get; set; }
        public decimal TasaSeguroHogar { get; set; }
        public decimal TasaCreditoEA { get; set; }
        public decimal TasaPeriodica { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal SaldoMora { get; set; }
        public decimal SaldoCapitalTradicional { get; set; }
        public decimal AcumuladoCapital { get; set; }
        public int DiasMora { get; set; }
        public decimal ValorDeudaRemanente { get; set; }
        public DateTime FechaEstado { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaProximoPago { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }
        public DateTime FechaUltimoPagoSeguroHogar { get; set; }
        public decimal AcumuladoSeguroVida { get; set; }
        public DateTime FechaUltimoPagoSeguroVida { get; set; }
        public decimal AcumuladoInteresesMora { get; set; }
        public DateTime FechaUltimoPagoInteresesMora { get; set; }
        public decimal AcumuladoInteresesCorrientes { get; set; }
        public DateTime FechaUltimoPagoInteresesCorrientes { get; set; }
        public string Calificacion { get; set; }
        public decimal Provision { get; set; }
        public decimal ProvisionPorcentaje { get; set; }
        public decimal ProvisionAdicional { get; set; }
        public decimal SeguroVidaSeven { get; set; }
        public decimal SeguroHogarSeven { get; set; }
        public decimal InteresesCorrientesSeven { get; set; }
        public decimal CapitalSeven { get; set; }
        public decimal ValorCuotaSeven { get; set; }
        public int? UnidadEjecutoraID { get; set; }
        public decimal ValorAjusteGarantia { get; set; }
        public Guid CreditoId { get; set; }
    }

    public class ReporteResultadoIntegracionDto
    {
        public string Id { get; set; }
        public string TidiD { get; set; }
        public string Identificacion { get; set; }
        public string ParId { get; set; }
        public DateTime Fecha { get; set; }
        public string Resultado { get; set; }
        public string UsuarioEjecutor { get; set; }
        public string PNombre { get; set; }
        public string SNombre { get; set; }
        public string PApellidos { get; set; }
        public string SApellidos { get; set; }

    }

    public class RequestReporteResultadoIntegracionDto
    {
        public string? NumeroDocumento { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
    }

    public class ReporteSFCF341Dto
    {
        public decimal MLOperaciones { get; set; }
        public decimal MEOperaciones { get; set; }
        public decimal MLCapital { get; set; }
        public decimal MECapital { get; set; }
        public decimal MLIntereses { get; set; }
        public decimal MEIntereses { get; set; }
        public decimal MLOtrosConceptos { get; set; }
        public decimal MEOtrosConceptos { get; set; }
        public decimal MLContingencia { get; set; }
        public decimal MEContingencia { get; set; }
        public decimal MLProvisionesIndividuales { get; set; }
        public decimal MEProvisionesIndividuales { get; set; }
        public decimal VrMLGarantia { get; set; }
        public decimal VrMEGarantia { get; set; }
        public string TipoGarantia { get; set; }
        public DateTime FechaUltimoAvaluo { get; set; }
        public decimal MLCapitalCuotaEsperada { get; set; }
        public decimal MECapitalCuotaEsperada { get; set; }
        public decimal MLInteresesCuotaEsperada { get; set; }
        public decimal MEInteresesCuotaEsperada { get; set; }
        public decimal MLCapitalCuotaRecibida { get; set; }
        public decimal MECapitalCuotaRecibida { get; set; }
        public decimal MLInteresesCuotaRecibida { get; set; }
        public decimal MEInteresesCuotaRecibida { get; set; }
        public decimal Redescuento { get; set; }
        public DateTime FechaInicialCredito { get; set; }
        public DateTime FechaVencimientoCredito { get; set; }
        public decimal TasaPromedioCreditoEA { get; set; }
        public int DiasMora { get; set; }
        public decimal Modelo { get; set; }
        public decimal ProbabilidadIncumplimiento { get; set; }
        public decimal MLComponenteIndividualPro { get; set; }
        public decimal MEComponenteIndividualPro { get; set; }
        public decimal MLCapitalComponenteIndividualCon { get; set; }
        public decimal MECapitalComponenteIndividualCon { get; set; }
        public decimal MLOperacionesExposicion { get; set; }
        public decimal MEOperacionesExposicion { get; set; }
        public decimal MLCapitalTasaDescuento { get; set; }
        public decimal MECapitalTasaDescuento { get; set; }
        public decimal PerdidaIncumplimiento { get; set; }
    }

    public class ColumnasReporte_414Dto
    {
        public string TipoPersona { get; set; }
        public string Sexo { get; set; }
        public string TamanioEmpresa { get; set; }
        public string TipoCredito { get; set; }
        public string TipoGarantia { get; set; }
        public string ProductoCredito { get; set; }
        public int PlazoCredito { get; set; }
        public decimal TasaEfectivaPromedio { get; set; }
        public decimal Margen { get; set; }
        public decimal MontoSolicitudes { get; set; }
        public int NumeroSolicitudes { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string CodigoColumna { get; set; }
        public string Municipio { get; set; }
        public string RANGO_MONTO_DESEMBOLSADO { get; set; }
    }

    public class RegistroTipo1_414Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string FechaCorte { get; set; }
        public string TotalRegistros { get; set; }
        public string Entidad { get; set; }
        public string TipoInforme { get; set; }
        public string AreaInformacion { get; set; }
    }

    public class RegistroTipo2_414Dto
    {
        public string NumeroRegistro { get; set; }
        public int TipoRegistro { get; set; }
        public string CodigoOficina { get; set; }
        public int Constante { get; set; }
        public int TipoMoneda { get; set; }
        public int TipoInformacion { get; set; }
        public int TipoFideicomiso { get; set; }
        public string CodigoFideicomiso { get; set; }
    }

    public class RegistroTipo4_414Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public string CodigoFormato { get; set; }
        public string CodigoColumna { get; set; }
        public string CodigoUnidadCaptura { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string Signo { get; set; }
        public string Valor { get; set; }
    }

    public class RegistroTipo5_414Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
    }

    public class RegistroTipo1_416Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string FechaCorte { get; set; }
        public string TotalRegistros { get; set; }
        public string Entidad { get; set; }
        public string TipoInforme { get; set; }
        public string AreaInformacion { get; set; }
    }

    public class RegistroTipo3_416Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoEvaluacion { get; set; }
        public string TipoFideicomiso { get; set; }
        public string CodigoFideicomiso { get; set; }
        public string TipoEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string ValorFideicomiso { get; set; }
    }

    public class RegistroTipo4_416Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string CodigoCIIU { get; set; }
        public int CodigoNaturaleza { get; set; }
        public int Variable1 { get; set; }
        public int Variable2 { get; set; }
        public int Variable3 { get; set; }
        public int CodigoReportado { get; set; }
    }

    public class RegistroTipo5_416Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public string CodigoFormato { get; set; }
        public string CodigoColumna { get; set; }
        public string CodigoUnidadCaptura { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string Signo { get; set; }
        public string Valor { get; set; }
    }

    public class RegistroTipo6_416Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
    }

    public class ColumnasReporte_416Dto
    {
        public string TipoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string CausalRechazo { get; set; }
        public string ModalidadCredito { get; set; }
        public string CodigoDpto { get; set; }
        public string CodigoCIIU { get; set; }
        public decimal MontoSolicitudes { get; set; }
        public int NumeroSolicitudes { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string CodigoColumna { get; set; }
    }

    public class RegistroTipo1_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string FechaCorte { get; set; }
        public string TotalRegistros { get; set; }
        public string PalabraClave { get; set; }
        public string AreaInformacion { get; set; }
        public string TipoInforme { get; set; }
    }

    public class RegistroTipo2_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string DigitoChequeo { get; set; }
        public string Nombres { get; set; }
    }

    public class RegistroTipo3_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public string TipoEvaluacion { get; set; }
        public string TipoFideicomiso { get; set; }
        public string CodigoFideicomiso { get; set; }
        public string TipoEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string ValorFideicomiso { get; set; }
    }

    public class RegistroTipo4_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string CodigoCIIU { get; set; }
        public string Naturaleza { get; set; }
        public string CodigoReestructurado { get; set; }
        public string CodigoAcrededores { get; set; }
        public string CodigoAcuerdo { get; set; }
        public string CodigoReportado { get; set; }
    }

    public class Datos_341Dto
    {
        public int NumeroOperacion { get; set; }
        public string TipoRegistro { get; set; }
        public string Formato { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal Concepto { get; set; }
        public decimal SaldoIntereses { get; set; }
        public decimal Provisiones { get; set; }
        public decimal ValorVivienda { get; set; }
        public string TipoGarantia { get; set; }
        public decimal ValorGarantia { get; set; }
        public string FechaDesembolso { get; set; }
        public decimal CuotaCapital { get; set; }
        public decimal CuotaInteres { get; set; }
        public decimal CapitalPagadoPeriodo { get; set; }
        public decimal InteresPagadoPeriodo { get; set; }
        public string Redescuento { get; set; }
        public string FechaVencimiento { get; set; }
        public decimal TasaEA { get; set; }
        public int DiasMora { get; set; }
        public string Modelo { get; set; }
        public int TipoVivienda { get; set; }
        public int TipoCredito { get; set; }
        public string Identificacion { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string CodigoColumna { get; set; }
        public string Signo { get; set; }
        public decimal Exposicion { get; set; }
        public string FecAvaluo { get;set; }

        public string EstadoCredito { get; set; }

    }

    public class RegistroTipo5_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
        public string CodigoFormato { get; set; }
        public string CodigoColumna { get; set; }
        public string Signo { get; set; }
        public string CodigoSubCuenta { get; set; }
        public string CodigoUnidadCaptura { get; set; }
        public string Valor { get; set; }
    }

    public class RegistroTipo6_341Dto
    {
        public string NumeroSecuencia { get; set; }
        public int TipoRegistro { get; set; }
    }

    public class DocumentoPorPeriodoDto
    {
        public string Identificacion { get; set; }
    }

    public class ParametrosReportePlano341
    {
        public string Identificacion { get; set; }
        public string NumeroSecuencia { get; set; }
        public DateTime FechaAplicacion { get; set; }
    }




}
