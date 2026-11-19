using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ParamGeneralesCreditoYCartera
    {
        public int PRM_SMLV { get; set; }
        public decimal PRM_PORC_INFLACION { get; set; }
        public decimal PRM_PORC_FINANCIA_VIS { get; set; }
        public decimal PRM_PORC_FINANCIA_NO_VIS { get; set; }
        public decimal PRM_NO_SALARIO_MIN_VIS { get; set; }
        public int PRM_DIAS_VALIDO_PREAPROB { get; set; }
        public int PRM_DIAS_VALIDO_APROB { get; set; }
        public int PRM_SCORE_MINIMO { get; set; }
        public decimal PRM_PORC_CAPACIDAD_ENDEUDA { get; set; }
        public int PRM_VIGENCIA_CONSULTA_BURO { get; set; }
        public int PRM_PORC_CANON_INI_LSG { get; set; }
        public int PRM_PORC_CANON_FIN_LSG { get; set; }
        public int PRM_PORC_OPC_COMPRA_LSG { get; set; }
        public int PRM_MESES_LEY_FRECH { get; set; }
        public int PRM_MESES_SUBSIDIO_CUOTA { get; set; }
        public decimal PRM_FACTOR_AHORRO { get; set; }
        public decimal PRM_FACTOR_CESANTIAS { get; set; }
        public decimal PRM_PORC_INCREMENTO_SUBSIDIO { get; set; }
        public decimal PRM_PORC_INCREMENTO_ANUAL { get; set; }
        public int PRM_TARIFA_UNICA_AVALUOS { get; set; }
        public int PRM_TARIFA_ESTUDIO_TITULOS { get; set; }
        public int PRM_VALOR_MINIMO_CUOTA { get; set; }
        public decimal PRM_TASA_USURA { get; set; }
        public DateTime PRM_FECHA_TASA_USURA { get; set; }
        public decimal PRM_TASA_MORA { get; set; }
        public DateTime PRM_FECHA_TASA_MORA { get; set; }
        public bool PRM_TRANSUNION { get; set; }
        public int PRM_SCORE_TU_MIN { get; set; }
        public int PRM_SMMLV { get; set; }
        public int PRM_MESES_DIFERIR { get; set; }
        public bool PRM_BLOBSTORAGE { get; set; }
        public decimal PRM_TASA_USURA_PERIODICA { get; set; }
        public decimal PRM_VALOR_IPV_ANUAL { get; set; }

        public decimal PRM_VALOR_IPV_ANUAL_OTRAS_CIUDADES { get; set; }
        public decimal PRM_PORCENTAJE_ENDUDAMIENTO { get; set; }
        public bool PRM_AHORRO_INTERESES { get; set; }
        public bool PRM_RANGO_ASCENSO { get; set; }
        public bool PRM_CESANTIAS_INTERESES { get; set; }
        public bool PRM_OBLIGACIONES_EXTERNAS_CAPACIDAD_PAGO { get; set; }
        public bool PRM_ASIGNACION_SALARIO_CARGO { get; set; }

    }
}
