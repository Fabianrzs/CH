using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class SimulacionPlanCliente
    {
        public Guid SPC_ID { get; set; }
        public Guid SLS_ID { get; set; }
        public Guid SDP_ID { get; set; }
        public string SPC_TIPO_VIVIENDA { get; set; }
        public double? SPC_VALOR_VIVIENDA { get; set; }
        public string SPC_TIPO_ALIVIO { get; set; }
        public char SPC_TIPO_ABONO { get; set; }
        public string SPC_NUMERO_DOCUMENTO { get; set; }
        public decimal SPC_VALOR_PRESTAMO { get; set; }
        public decimal SPC_VALOR_TASA_EA { get; set; }
        public double SPC_VALOR_TASA_MV { get; set; }
        public decimal SPC_VALOR_TASA_MV_FRESH { get; set; }
        public string SPC_VIVIENDA_VIS { get; set; }
        public DateTime SPC_FECHA_SIMULACION { get; set; }
        public char SPC_USA_REC_ACUMULADO { get; set; }
        public char SPC_TOMA_SEGURO { get; set; }
        public decimal SPC_VALOR_PRESTAMO_LSG { get; set; }
        public long? SPC_NUMERO_PREAPROBADO { get; set; }
        public int? SPC_TOTAL_CUOTAS { get; set; }
        public DateTime? SPC_FECHA_APROBACION { get; set; }
        public decimal SPC_OPCION_COMPRA { get; set; }
        public decimal? SPC_PORCE_TASAFRES { get; set; }
        public decimal? SPC_TASA_SEGURO_VIDA { get; set; }
        public decimal? SPC_TASA_SEGURO_HOGAR { get; set; }
        public decimal? SPC_VALOR_PRESTAMO_SOLICITADO { get; set; }
        public decimal? SPC_PORCEN_CANON_INICIAL { get; set; }
        public decimal? SPC_PORCEN_OPCION_DE_COMPRA { get; set; }
        public int APORTE_VOLUNTARIO { get; set; }
        public DateTime? SPC_FECHA_VIGENCIA { get; set; }
        public double? SPC_VALOR_VIVIENDA_SEGURO { get; set; }
        public bool? SPC_TASA_VOLUNTARIA { get; set; }
        public int? NUM_FOLIOS { get; set; }
        public Guid? MEDIOS_CREDITO { get; set; }
        public decimal SPC_FACTOR { get; set; }
        public int AFL_ID { get; set; }
        public Guid? USR_ID { get; set; }
        public int TCR_ID { get; set; }
    }
}
