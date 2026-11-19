using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.VerificacionDerechos
{

    public class ParametrosAbonoInteres
    {

        public int ID { get; set; }

        public string NUMERO_SOLICITUD { get; set; }
        public int ANIO { get; set; }
        public int MES { get; set; }
        public decimal TOTAL_INTERESES_ABONADOS { get; set; }
        public Guid USUARIO_ID { get; set; }
        public string? USUARIO_NOMBRE { get; set; }
        public DateTime? FECHA_APROBACION { get; set; }

        public DateTime? FECHA_RECEPCION { get; set; }

        public float VALOR_IPC { get; set; }
        public float PORCENTAJE_IPC { get; set; }

        public string COMPROBANTE_SOLUCION_VIVIENDA_APORTES { get; set; }

        public string COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS { get; set; }

        public string COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS { get; set; }

        public string COMPROBANTE_AVAC { get; set; }

        public string COMPROBANTE_AVAV { get; set; }
        public string COMPROBANTE_AVC { get; set; }   


        public int? APROBADO { get; set; }
        public string NUMERO_RADICADO { get; set; }


        public int ESTADO { get; set; }
        public string NOMBRE_ESTADO { get; set; }

        public Guid SLT_ID { get; set; }

        public Guid SLT_ID_APROBAR { get; set; }


    }
    public class DatosCuentasAdministracionCesantias
    {

        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public decimal CTA_SALDO { get; set; }
        public DateTime FECHA_ULTIMO_MOVIMIENTOS_CREDITO { get; set; }
        public string IDENTIFICACION { get; set; }
        public decimal SALDO_ANUAL_CESANTIAS { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_CESANTIAS { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public decimal INTERES_CESANTIAS { get; set; }


    }


    public class DatosCuentasAdministracionCesantiasResult
    {
        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public decimal CTA_SALDO { get; set; }
        public DateTime FECHA_ULTIMO_MOVIMIENTOS_CREDITO { get; set; }
        public string IDENTIFICACION { get; set; }

        public int PERIODO_ANIO { get; set; }
        public int PERIODO_MES { get; set; }

        public decimal SALDO_ANUAL_CESANTIAS { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_CESANTIAS { get; set; }


        public decimal INTERESES_CAUSADO_CESANTIAS { get; set; }
        public decimal INTERESES_NO_CAUSADO_CESANTIAS { get; set; }
        

        public decimal? SALDO_ANTERIOR_CESANTIAS { get; set; }
        public decimal? SALDO_FINAL_CESANTIAS { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public long? NUMERO_SOLICITUD { get; set; }
        public string? TIPO_SOLICITUD { get; set; }

        public decimal INTERES_CESANTIAS { get; set; }

        public decimal INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS { get; set; }


    }


    public class DatosCuentasAV
    {
        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public int ECT_ID { get; set; }
        public DateTime CTA_FECHA_CREACION { get; set; }
        public decimal? CTA_SALDO { get; set; }

        public DateTime? AVAC { get; set; }
        public decimal? VALOR_AVAC { get; set; }
        public decimal? SALDO_ANUAL_AVAC { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVAC { get; set; }


        public DateTime? AVAV { get; set; }
        public decimal? VALOR_AVAV { get; set; }
        public decimal? SALDO_ANUAL_AVAV { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVAV { get; set; }

        public DateTime? AVC { get; set; }
        public decimal? VALOR_AVC { get; set; }
        public decimal? SALDO_ANUAL_AVC { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVC { get; set; }

        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }



    }

    public class DatosCuentasAVResult
    {
        public int ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int PERIODO_ANIO { get; set; }
        public int PERIODO_MES { get; set; }


        public decimal? VALOR_AVAC { get; set; }
        public decimal? SALDO_ANUAL_AVAC { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVAC { get; set; }
        public decimal? INTERESES_NO_CAUSADO_AVAC { get; set; }
        public decimal? INTERESES_CAUSADO_AVAC { get; set; }
        public decimal? SALDO_ANTERIOR_AVAC { get; set; }
        public decimal? SALDO_FINAL_AVAC { get; set; }




        public decimal? VALOR_AVAV { get; set; }
        public decimal? SALDO_ANUAL_AVAV { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVAV { get; set; }

        public decimal? INTERESES_NO_CAUSADO_AVAV { get; set; }
        public decimal? INTERESES_CAUSADO_AVAV { get; set; }
        public decimal? SALDO_ANTERIOR_AVAV { get; set; }
        public decimal? SALDO_FINAL_AVAV { get; set; }



        public decimal? VALOR_AVC { get; set; }
        public decimal? SALDO_ANUAL_AVC { get; set; }
        public decimal? MOVIMIENTOS_MENSUAL_AVC { get; set; }

        public decimal? INTERESES_NO_CAUSADO_AVC { get; set; }
        public decimal? INTERESES_CAUSADO_AVC { get; set; }
        public decimal? SALDO_ANTERIOR_AVC { get; set; }
        public decimal? SALDO_FINAL_AVC { get; set; }

        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public long? NUMERO_SOLICITUD { get; set; }
        public string? TIPO_SOLICITUD { get; set; }



    }

}