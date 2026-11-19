using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class RespuestaTerceroEntidadEducativa
    {
        public Guid TER_ID { get; set; }
        public Guid ENE_ID_FK { get; set; }
        public String TER_NUMERO_DOCUMENTO { get; set; }
        public String TER_NOMBRE_RAZON_SOCIAL { get; set; }
        public Guid PGN_ID { get; set; }
        public Guid? PRN_ID { get; set; }
        public string TER_NUM_RECIBO_PAGO { get; set; }
        public string TER_FECHA_LIMITE_PAGO { get; set; }
        public Guid? NVE_ID { get; set; }
        public int TER_ENT_DEPARTAMENTO { get; set; }
        public int TER_ENT_CIUDAD { get; set; }
        public int TER_ENT_MODALIDAD_ESTUDIO { get; set; }
        public int TER_ENT_PERIODO_ACADEMICO { get; set; }
        public string TER_ENT_HORAS { get; set; }
        public string? NivelEducativo { get; set; }
        public string? ProgramaEducativo { get; set; }
        public int TER_ENT_ACREDITADA { get; set; }
        public decimal TER_ENT_VALOR_RECIBO { get; set; }
        public DateTime TER_ENT_FECHA_INICIO { get; set; }
        public DateTime TER_ENT_FECHA_FIN { get; set; }

    }
}
