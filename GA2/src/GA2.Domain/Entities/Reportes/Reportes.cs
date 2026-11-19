using System;
using System.Collections.Generic;

namespace GA2.Domain.Entities
{
    public class ReporteSFCF416
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

    public class ReporteSFCF414
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
    }

    public class ReporteSFCF453Y454
    {
        public string consecutivo_formateado { get; set; }
        public string cuenta_formateada { get; set; }
        public string formato { get; set; }
        public string columnaTable { get; set; }
        public string columna2_formateada { get; set; }
        public string subcuenta { get; set; }
        public string valor_formateado { get; set; }
    }

    public class ReporteAseguradora
    {
        public string DOCUMENTO { get; set; }
        public string NOMBRES { get; set; }
        public string INMUEBLE { get; set; }
        public string CIUDAD { get; set; }
        public decimal VALOR_ASEGURADO { get; set; }
        public decimal PRIMA_SEGURO_POSITIVA { get; set; }
        public DateTime FECHA_INICIO_VIGENCIA { get; set; }
        public DateTime FECHA_FIN_VIGENCIA { get; set; }
    }

    public class ReporteEstudioTitulos
    {
        public string DOCUMENTO { get; set; }
        public string NOMBRES { get; set; }
        public DateTime FECHA_CONTACTO { get; set; }
        public DateTime FECHA_CREACION_CASO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string CONCEPTO { get; set; }
        public DateTime FECHA_ENTREGA_ET { get; set; }
        public DateTime FECHA_ENVIO_CONTRATO { get; set; }
        public string NOTARIA { get; set; }
        public DateTime FECHA_ENVIO_MINUTA { get; set; }
        public DateTime FECHA_FIRMA_ESCRITURA { get; set; }
        public string SALIDA_REGISTRO { get; set; }
        public decimal VALOR_COMERCIAL { get; set; }
        public string NOMBRE_VENDEDORES { get; set; }
        public string DOCUMENTO_VENDEDORES { get; set; }
        public string CIUDAD { get; set; }
        public string TELEFONO { get; set; }
    }

    public class ReporteConsultasRestrictivas
    {
        public string DOCUMENTO { get; set; }
        public string NOMBRES_AFILIADO { get; set; }
        public string NOMBRES_VENDEDOR { get; set; }
        public string DOCUMENTO_VENDEDOR { get; set; }
        public int NRO_FOLIO { get; set; }
        public string RESULTADO_CONSULTA { get; set; }
    }

    public class ReporteAvaluos
    {
        public string DOCUMENTO { get; set; }
        public string NOMBRES_AFILIADO { get; set; }
        public string PUNTO_RADICACION { get; set; }
        public string RESPONSABLE { get; set; }
        public DateTime FECHA_INICIO_PROCESO { get; set; }
        public string NRO_RADICADO { get; set; }
        public string FIRMA_AVALUO { get; set; }
        public string RESULTADO_AVALUO { get; set; }
        public DateTime FECHA_REVISION_AVALUO { get; set; }
        public string VETUSTEZ { get; set; }
        public int NRO_FOLIO { get; set; }
        public string RESULTADO_CONSULTA { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public string PARQUEADERO { get; set; }
        public string DEPOSITO { get; set; }
        public string CUIDAD { get; set; }
        public string ESTADO_INMUEBLE { get; set; }
        public string AREA_INMUEBLE { get; set; }
        public string NOMBRES_VENDEDOR { get; set; }
        public decimal VALOR_VENTA { get; set; }
        public decimal VALOR_COMERCIAL { get; set; }
        public decimal DIFERENCIA { get; set; }
    }

    public class ReporteCesiones
    {
        public string DOCUMENTO { get; set; }
        public string NOMBRES { get; set; }
        public DateTime FECHA_CONTACTO { get; set; }
        public DateTime FECHA_CREACION_CASO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string CONCEPTO { get; set; }
        public DateTime FECHA_ENTREGA_ET { get; set; }
        public DateTime FECHA_ENVIO_DOCUMENTOS { get; set; }
        public string NOTARIA { get; set; }
        public DateTime FECHA_ENVIO_MINUTA { get; set; }
        public DateTime FECHA_FIRMA_ESCRITURA { get; set; }
        public string SALIDA_REGISTRO { get; set; }
        public decimal VALOR_COMERCIAL { get; set; }
        public string NOMBRE_VENDEDORES { get; set; }
        public string DOCUMENTO_VENDEDORES { get; set; }
        public string CIUDAD { get; set; }
        public string TELEFONO { get; set; }
    }

    public class ReporteCargaAnalista
    {
        public string NUMERO_RADICADO { get; set; }
        public string PUNTO_RADICACION { get; set; }
        public string DOCUMENTO { get; set; }
        public DateTime FECHA_RADICACION { get; set; }
        public string PROCESO { get; set; }
        public DateTime FECHA_RECEPCION_ANALISTA { get; set; }
        public DateTime FECHA_ANALISIS { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public string TIPO_CREDITO { get; set; }
        public decimal VALOR_SOLICITUD { get; set; }
    }

    public class ReporteDeMora
    {
        public string NUMERO_RADICACION { get; set; }
        public string ESTADO { get; set; }
        public string CODIGO_CLIENTE { get; set; }
        public string NOMBRES { get; set; }
        public string CODIGO_EMPRESA { get; set; }
        public string EMPRESA { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public string COD_PERIODICIDAD { get; set; }
        public string PERIODICIDAD { get; set; }
        public string TIPO_RECAUDO { get; set; }
        public decimal CUOTA_VENCIDA { get; set; }
        public DateTime VENCIMIENTO_PROXIMO_PAGO { get; set; }
        public decimal ULTIMO_PAGO { get; set; }
        public DateTime VENCIMIENTO_ULTIMO_PAGO { get; set; }
        public decimal VALOR_ULTIMO_PAGO { get; set; }
    }

    public class ReporteValorVenta
    {
        public string TIPO_VIVIENDA { get; set; }
        public string VETUSTEZ { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public decimal VALOR_CREDITO { get; set; }
        public int PLAZO_MESES { get; set; }
        public int ANTIGUEDAD_AFILIADO { get; set; }
        public int EDAD { get; set; }
        public decimal VALOR_CTA_INDIVIDUAL { get; set; }
        public decimal VALOR_RECURSOS_PROPIOS { get; set; }
        public decimal VALOR_VENTA { get; set; }
        public decimal VALOR_AVALUO { get; set; }
        public decimal DIFERENCIA { get; set; }
        public decimal CALCULO { get; set; }

    }

    public class ReporteCartera
    {
        public string CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string DPC_ID_IDENTIFICACION_EXPEDIDA { get; set; }
        public DateTime CLI_FECHA_EXPEDICION { get; set; }
        public string CLI_NOMBRES { get; set; }
        public string DRC_DIRECCION { get; set; }
        public string DTL_TELEFONO { get; set; }
        public string DCE_CORREO { get; set; }
        public DateTime CLI_FECHA_NACIMIENTO { get; set; }
        public DateTime CLI_LUGAR_NACIMIENTO { get; set; }
        public int CLI_EDAD { get; set; }
        public string CAT_ESTADO_CIVIL { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public string CAT_SEXO { get; set; }
        public DateTime FECHA_SOLICITUD { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public int TCR_ID { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public decimal CRE_PLAZO_TOTAL { get; set; }
        public decimal CRE_PLAZO_ACTUAL { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public decimal CRE_TASA_SEGURO_VIDA { get; set; }
        public decimal CRE_TASA_SEGURO_HOGAR { get; set; }
        public decimal CRE_TASA_CREDITO_PER { get; set; }
        public decimal CRE_TASA_FRECH { get; set; }
        public decimal CRE_VALOR_ALIVIO_CUOTA { get; set; }
        public decimal CRE_TIPO_ABONO_EXTRA { get; set; }
        public decimal CRE_VALOR_ABONO_EXTRA { get; set; }
        public int TIV_ID { get; set; }
        public int TVV_ID { get; set; }
        public decimal CRE_VALOR_CANON_INICIAL { get; set; }
        public decimal CRE_CANON_EXTRAORDINARIO { get; set; }
        public int SOC_SCORE { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public int ESC_ID { get; set; }
        public string CLASE_INMUEBLE { get; set; }
        public string CRE_DIRECCION_INMUEBLE { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string NUMERO_MATRICULA { get; set; }
        public string ESCRITURA_INMUEBLE { get; set; }
        public DateTime FECHA_ESCRITURA { get; set; }
        public string NOTARIA { get; set; }
        public string SLI_LINDEROS { get; set; }
        public string DESCRIPCION_INMUEBLE { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_CORRIENTE { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_VIDA { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_HOGAR { get; set; }
        public decimal CRE_VALOR_DEUDA_REMANENTE { get; set; }
        public decimal CRE_PROVISION { get; set; }
        public decimal CRE_PROVISION_PORCENTAJE { get; set; }
        public string CRE_CALIFICACION { get; set; }

    }

    public class ReporteGeneralCartera
    {
        public string NUMERO_RADICACION { get; set; }
        public string PAGARE { get; set; }
        public string CLIENTE { get; set; }
        public string ESTADO { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string PAGADURIA { get; set; }
        public string NOMBRE_PAGADURIA { get; set; }
        public string APERTURA { get; set; }
        public string TERMINACION { get; set; }       
        public decimal TASA_LINEA { get; set; }
        public string DESTINACION { get; set; }
        public decimal DESEMBOLSO { get; set; }
        public decimal MONTO { get; set; }
        public decimal SALDO { get; set; }
        public decimal CUOTA { get; set; }
        public int DIAS { get; set; }
        public String ULTIMO_PAGO { get; set; }
        public String PRÓXIMO_PAGO { get; set; }
        public decimal MORA_INTERIOR { get; set; }
        public decimal INTERES { get; set; }
        public string SEGUROS { get; set; }
        public decimal CAPITAL { get; set; }
        public decimal TOTAL_PAGAR { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
        public int PLAZO_OBLIGACION { get; set; }
        public string CALIFICACION { get; set; }
        public int NUMERO_CUOTAS_FALTANTES { get; set; }
        public decimal VALOR_CUOTA_EXTRAORDINARIA { get; set; }
        public decimal VALOR_OPCION_COMPRA { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public decimal PUNTAJE_SCORING { get; set; }
    }

    public class ReportePreliminarCartera
    {
        public Int64 NRO { get; set; }
        public String NUMERO_RADICACION { get; set; }
        public String IDENTIFICACION { get; set; }
        public String NOMBRES { get; set; }
        public String CLIENTE { get; set; }
        public String NOMBRE_PAGADURIA { get; set; }
        public Double CUOTA { get; set; }
        public String ULTIMO_PAGO { get; set; }
        public Double VALOR_AJUSTADO { get; set; }
        public String APERTURA { get; set; }
        public Double MONTO { get; set; }
        public Double SALDO { get; set; }
        public Double VALOR_INMUEBLE { get; set; }
        public Double TASA_LINEA { get; set; }
        public String TERMINACION { get; set; }
    }

    public class ReporteGeneralGarantias
    {
        public string NUMERO_RADICACION { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public DateTime FECHA_APERTURA { get; set; }
        public DateTime FECHA_TERMINACION { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
        public decimal AJUSTE_GARANTIA { get; set; }
        public decimal VALOR_AJUSTADO { get; set; }
        public string DIRECCION_INMUEBLE { get; set; }
        public string NUMERO_MATRICULA { get; set; }
        public string ESCRITURA_INMUEBLE { get; set; }
    }

    public class ReporteSeguroHogar
    {
        public string CIUDAD_INMUEBLE { get; set; }
        public string COD_CLIENTE { get; set; }
        public string DIRECCION_INMUEBLE { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRE { get; set; }
        public string SEGURO_HOGAR { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
    }

    public class ReporteSeguroVida
    {
        public string CIUDAD_INMUEBLE { get; set; }
        public string COD_CLIENTE { get; set; }
        public string DIRECCION_INMUEBLE { get; set; }
        public string IDENTIFICACION { get; set; }
        public decimal MONTO_CAPITAL { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public string NOMBRE { get; set; }
        public DateTime TERMINACION_CREDITO { get; set; }
    }

    public class ReporteDescuentosNomina
    {
        public int NRO { get; set; }
        public string NUMERO_RADICACION { get; set; }
        public string CLIENTE { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string PAGADURIA { get; set; }
        public string NOMBRE_PAGADURIA { get; set; }
        public string APERTURA { get; set; }
        public string TERMINACION { get; set; }
        public decimal TASA_LINEA { get; set; }
        public decimal MONTO { get; set; }
        public decimal SALDO { get; set; }
        public decimal CUOTA { get; set; }
        public decimal ULTIMO_PAGO { get; set; }
        public decimal PROXIMO_PAGO { get; set; }
        public int PLAZO_OBLIGACION { get; set; }
    }

    public class InformacionCliente
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

    public class OficioImprocedente
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

    public class InformacionFinancieraExtractoAnual
    {
        public decimal TOTAL_CUOTA { get; set; }
        public decimal CUOTAS_PAGADAS { get; set; }
        public decimal CUOTAS_PENDIENTES { get; set; }
        public decimal CUOTAS_MORA { get; set; }
        public decimal INTERES_CORRIENTE_EA { get; set; }
        public decimal INTERES_MORA_EA { get; set; }
        public DateTime FECHA_CORTE { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
    }

    public class ProyeccionAnualExtractoAnual
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

    public class RelacionPagosExtractoAnual
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

    public class InformacionCuotasExtractoMensual
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

    public class InformacionValoresExtractoMensual
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

    public class InformacionDiscriminadaExtractoMensual
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

    public class InformacionMontosCertificadoIYR
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

    public class InformacionBasicaCertificadoHaberes
    {
        public string TITULAR { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string IDENTIFICACION { get; set; }
        public string TIPO_CREDITO { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public decimal SALDO { get; set; }
    }

    public class InformacionBasicaEstadoCuenta
    {
        public string CODIGO { get; set; }
        public string NOMBRES { get; set; }
        public string IDENTIFICACION { get; set; }
        public string DIRECCION { get; set; }
        public DateTime FECHA_ESTADO { get; set; }
        public string TIPO_CLIENTE { get; set; }
        public string TELEFONO { get; set; }
    }

    public class InformacionObligacionEstadoCuenta
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

    public class InformacionDistribucionValoresEstadoCuenta
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

    public class InformacionMovimientosCarteraDetalle
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

    public class InformacionMovimientosCartera
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
    }

    public class ObtenerRespuestaEmbargo
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
        public string EBA_OBSERVACIONES { get; set; }
    }

    public class ReportePreliminarGarantias
    {
        public string RADICACION { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public DateTime FECHA_APERTURA { get; set; }
        public DateTime FECHA_TERMINACION { get; set; }
        public decimal VALOR_INMUEBLE { get; set; }
        public decimal AJUSTE_GARANTIA { get; set; }
        public decimal VALOR_AJUSTADO { get; set; }
    }

    public class CargarListadoReportes
    {
        public int RPT_ID_REPORTE { get; set; }
        public string RPT_DESCRIPCION_REPORTE { get; set; }
        public string RPT_PARAMETRO { get; set; }
        public string RPT_TIPO_REPORTE { get; set; }
        public bool RPT_ESTADO { get; set; }
    }

    public class TransferenciaCesantias
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


    public class OficiosAfiliacion
    {
        public string IDENTIFICACION { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string CORREO { get; set; }
        public string NUMERO_RADICADO { get; set; }
        public string MES_ACTIVACION_DESCUENTO { get; set; }
        public int RATIFICACION_CUOTAS { get; set; }
        public string NOMBRE_BENEFICIARIO { get; set; }
        public int RATIFICACION_CUOTAS_CONTINUA { get; set; }
        public int CATEGORIA { get; set; }
        public int FUERZA { get; set; }
        public DateTime APC_FECHA_PRIMERA_CUOTA { get; set; }
        public DateTime PERIODO_APORTES_DESDE { get; set; }
        public DateTime PERIODO_APORTES_HASTA { get; set; }
        public int VALOR_DERECHO_LIQUIDACION_CUOTAS { get; set; }
    }

    public class ParametrosReportesAfiliaciones
    {
        public string IdAfiliacion { get; set; }
        public int OpcionReporte { get; set; }
    }


    public class ReporteCierreMensualCartera
    {
        public Int64 CRE_NRO_CREDITO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public DateTime FECHA_SOLICITUD { get; set; }
        public string TIPO_CREDITO { get; set; }
        public int TIPO_CREDITO_ID { get; set; }
        public string ESTADO_CREDITO { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public string TIV_TIPO_VIVIENDA { get; set; }
        public string TVV_TIPO_VIVIENDA { get; set; }
        public decimal CRE_VALOR_CANON_INICIAL { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_POR_CANON_INICIAL { get; set; }
        public decimal CRE_POR_OPCION_COMPRA { get; set; }
        public decimal CRE_CANON_EXTRAORDINARIO { get; set; }
        public int CRE_PLAZO_TOTAL { get; set; }
        public int CRE_PLAZO_ACTUAL { get; set; }
        public int CRE_NUMERO_CUOTAS_CANCELADAS { get; set; }
        public decimal CRE_TASA_SEGURO_VIDA { get; set; }
        public decimal CRE_TASA_SEGURO_HOGAR { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public decimal CRE_TASA_CREDITO_PER { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public decimal CRE_SALDO_CAPITAL_MORA { get; set; }
        public decimal CRE_SALDO_CAPITAL_TRADICIONAL { get; set; }
        public decimal CRE_ACUMULADO_CAPITAL { get; set; }
        public int CRE_DIAS_MORA { get; set; }
        public decimal CRE_VALOR_DEUDA_REMANENTE { get; set; }
        public DateTime CRE_FECHA_ESTADO { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_HOGAR { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_VIDA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_MORA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_MORA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_CORRIENTE { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE { get; set; }
        public string CRE_CALIFICACION { get; set; }
        public decimal CRE_PROVISION { get; set; }
        public decimal CRE_PROVISION_PORCENTAJE { get; set; }
        public DateTime CRE_FECHA_CAUSACION { get; set; }
        public decimal CRE_PROVISION_ADICIONAL { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal INTERESES_CORRIENTES { get; set; }
        public decimal CAPITAL { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public decimal VALOR_DE_LA_GARANTIA_DEL_CREDITO { get; set; }

        public Guid CRE_ID { get; set; }

    }

    public class ReporteResultadoIntegracion
    {
        public string PIN_ID { get; set; }
        public string TID_ID { get; set; }
        public string PIN_NRO_IDENTIFICACION { get; set; }
        public string PIN_PARAM_ID { get; set; }
        public DateTime PIN_FECHA_EJECUCION { get; set; }
        public string PIN_RESULTADO_INTEGRACION { get; set; }
        public string USR_ID { get; set; }
    }

    public class ReporteSFCF341
    {
        public decimal ML_OPERACIONES { get; set; }
        public decimal ME_OPERACIONES { get; set; }
        public decimal ML_CAPITAL { get; set; }
        public decimal ME_CAPITAL { get; set; }
        public decimal ML_INTERESES { get; set; }
        public decimal ME_INTERESES { get; set; }
        public decimal ML_OTROS_CONCEPTOS { get; set; }
        public decimal ME_OTROS_CONCEPTOS { get; set; }
        public decimal ML_CONTINGENCIA { get; set; }
        public decimal ME_CONTINGENCIA { get; set; }
        public decimal ML_PROVISIONES_INDIVIDUALES { get; set; }
        public decimal ME_PROVISIONES_INDIVIDUALES { get; set; }
        public decimal VR_ML_GARANTIA { get; set; }
        public decimal VR_ME_GARANTIA { get; set; }
        public string TIPO_GARANTIA { get; set; }
        public DateTime FECHAULTIMOAVALUO { get; set; }
        public decimal ML_CAPITAL_CUOTA_ESPERADA { get; set; }
        public decimal ME_CAPITAL_CUOTA_ESPERADA { get; set; }
        public decimal ML_INTERESES_CUOTA_ESPERADA { get; set; }
        public decimal ME_INTERESES_CUOTA_ESPERADA { get; set; }
        public decimal ML_CAPITAL_CUOTA_RECIBIDA { get; set; }
        public decimal ME_CAPITAL_CUOTA_RECIBIDA { get; set; }
        public decimal ML_INTERESES_CUOTA_RECIBIDA { get; set; }
        public decimal ME_INTERESES_CUOTA_RECIBIDA { get; set; }
        public decimal REDESCUENTO { get; set; }
        public DateTime FECHA_INICIAL_CREDITO { get; set; }
        public DateTime FECHA_VENCIMIENTO_CREDITO { get; set; }
        public decimal TASA_PROMEDIO_CREDITO_EA { get; set; }
        public int DIAS_MORA { get; set; }
        public decimal MODELO { get; set; }
        public decimal PROBABILIDAD_INCUMPLIMIENTO { get; set; }
        public decimal ML_COMPONENTE_INDIVIDUAL_PRO { get; set; }
        public decimal ME_COMPONENTEINDIVIDUAL_PRO { get; set; }
        public decimal ML_CAPITAL_COMPONENTE_INDIVIDUAL_CON { get; set; }
        public decimal ME_CAPITAL_COMPONENTE_INDIVIDUAL_CON { get; set; }
        public decimal ML_OPERACIONES_EXPOSICION { get; set; }
        public decimal ME_OPERACIONES_EXPOSICION { get; set; }
        public decimal ML_CAPITAL_TASA_DESCUENTO { get; set; }
        public decimal ME_CAPITAL_TASA_DESCUENTO { get; set; }
        public decimal PERDIDA_INCUMPLIMIENTO { get; set; }
    }

    public class DocumentoPorPeriodo
    {
        public string CLI_IDENTIFICACION { get; set; }
    }

    public class Datos_341
    {
        public int NRO_OPERACIONES { get; set; }
        public string TIPO_REGISTRO { get; set; }
        public string FORMATO { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal SALDO_INTERESES { get; set; }
        public decimal PROVISIONES { get; set; }
        public decimal CONCEPTO { get; set; }
        public decimal VALOR_VIVIENDA { get; set; }
        public string TIPO_GARANTIA { get; set; }
        public decimal VALOR_GARANTIA { get; set; }
        public string FECHA_DESEMBOLSO { get; set; }
        public decimal CUOTA_CAPITAL { get; set; }
        public decimal CUOTA_INTERES { get; set; }
        public decimal CAPITAL_PAGADO_PERIODO { get; set; }
        public decimal INTERES_PAGADO_PERIODO { get; set; }
        public string REDESCUENTO { get; set; }
        public string FECHA_VENCIMIENTO { get; set; }
        public decimal TASA_EA { get; set; }
        public int DIAS_MORA { get; set; }
        public string MODELO { get; set; }
        public int TIPO_VIVIENDA { get; set; }
        public int TIPO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string FEC_AVA { get; set; }
        public int ESTADO_CRE { get; set; }


    }

}
