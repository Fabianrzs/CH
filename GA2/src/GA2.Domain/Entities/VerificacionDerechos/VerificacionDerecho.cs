using System;

namespace GA2.Domain.Entities
{
    public class ParametrosVerificacionDerecho
    {

        public int ID { get; set; }
        public string NUMERO_SOLICITUD { get; set; }
        public int ANIO { get; set; }
        public int MES { get; set; }
        public DateTime FECHA_OPERACION { get; set; }
        public DateTime? FECHA_APROBACION { get; set; }
        public int TOTAL_CUENTAS_VERIFICADAS { get; set; }
        public int TOTAL_CAMBIOS_ESTADO { get; set; }
        public Guid USUARIO_ID { get; set; }
        public string? USUARIO_NOMBRE { get; set; }
        public int? APROBADO { get; set; }
        public string NUMERO_RADICADO { get; set; }

        public int ESTADO { get; set; }
        public string NOMBRE_ESTADO { get; set; }

        public Guid SLT_ID { get; set; }
        public Guid SLT_ID_APROBAR { get; set; }


    }

    public class ResultVerificacionDerecho
    {
        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public DateTime? FECHA_ULTIMO_MOVIMIENTOS_CREDITO { get; set; }
        public DateTime CTA_FECHA_PRIMER_APORTE { get; set; }
        public int? CTA_CUOTAS { get; set; }
        public int? CUOTAS_PENDIENTES { get; set; }
        public int ECT_INICIAL { get; set; }
        public int? ECT_FINAL { get; set; }
        public string CAMBIO_ESTADO { get; set; }


        public int PERIODO_ANIO { get; set; }
        public int PERIODO_MES { get; set; }
        public string? FLAG { get; set; }
        public string? IDENTIFICACION { get; set; }
        

        public decimal? INTERESES_NO_CAUSADO_APORTES { get; set; }
        public decimal? INTERESES_NO_CAUSADO_CESANTIAS { get; set; }
        public decimal? INTERESES_CAUSADO_APORTES { get; set; }
        public decimal? INTERESES_CAUSADO_CESANTIAS { get; set; }
        public decimal? CTA_SALDO { get; set; }

        public decimal? SALDO_ANUAL_APORTES { get; set; }
        public decimal? SALDO_ANUAL_CESANTIAS { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_APORTES { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_CESANTIAS { get; set; }


        public decimal? SALDO_ANTERIOR_APORTES { get; set; }
        public decimal? SALDO_ANTERIOR_CESANTIAS { get; set; }
        public decimal? SALDO_FINAL_APORTES { get; set; }
        public decimal? SALDO_FINAL_CESANTIAS { get; set; }

        public int UNIDAD_EJECUTORA { get; set; }

        public long? NUMERO_SOLICITUD { get; set; }
        public string? TIPO_SOLICITUD { get; set; }

        public decimal INTERES_APORTES { get; set; }
        public decimal INTERES_CESANTIAS { get; set; }


        public decimal INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES { get; set; }
        public decimal INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS { get; set; }


    }

    public class InfoVerificacionDerecho
    {
        public int VDD_ID { get; set; }
        public int ESTADO_INICIAL { get; set; }
        public int ESTADO_FINAL { get; set; }
        public string? MOTIVO_CAMBIO { get; set; }
        public int? CUENTAS_AFECTADAS { get; set; }


    }

    public class DatosVerificacionDerechoCuenta
    {

        public int CTA_ID { get; set; }
        public int CTA_ID_INTEGRACION { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public int ECT_ID { get; set; }
        public DateTime CTA_FECHA_CREACION { get; set; }
        public DateTime CTA_FECHA_PRIMER_APORTE { get; set; }
        public int? CTA_CUOTAS { get; set; }
        public decimal? CTA_SALDO { get; set; }
        public int? CUOTAS_ESPERADAS { get; set; }
        public int? CANTIDAD_MOVIMIENTOS_CREDITO { get; set; }
        public DateTime? FECHA_ULTIMO_MOVIMIENTOS_CREDITO { get; set; }
        public int? ULTIMO_ANIO_PERIODO { get; set; }
        public int? ULTIMO_MES_PERIODO { get; set; }
        public int? AHORROS_OBLIGATORIOS { get; set; }
        public int? IDENTIFICACION { get; set; }
        public string? FLAG { get; set; }

        public decimal? SALDO_ANUAL_APORTES { get; set; }
        public decimal? SALDO_ANUAL_CESANTIAS { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_APORTES { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_CESANTIAS { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public decimal INTERES_APORTES { get; set; }
        public decimal INTERES_CESANTIAS { get; set; }


    } 


}
