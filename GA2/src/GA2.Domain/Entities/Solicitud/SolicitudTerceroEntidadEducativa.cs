using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudTerceroEntidadEducativa
    {
        public Guid ENE_ID { get; set; }
        public string ENE_NIT { get; set; }
        public string ENE_NOMBRE_RAZON_SOCIAL { get; set; }
        public Guid? PRN_ID_FK { get; set; }
        public string TER_NUM_RECIBO_PAGO { get; set; }
        public DateTime TER_FECHA_LIMITE_PAGO { get; set; }
        public Guid ENE_CREATEDOPOR { get; set; }
        public int? TER_TIPO_TERCERO_FK { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public Guid TER_ID { get; set; }
        public int TID_ID_FK { get; set; }

        public int TER_ENT_DEPARTAMENTO { get; set; }
        public int TER_ENT_CIUDAD { get; set; }
        public int TER_ENT_MODALIDAD_ESTUDIO { get; set; }
        public int TER_ENT_PERIODO_ACADEMICO { get; set; }
        public string TER_ENT_HORAS { get; set; }
        public int TER_ENT_ACREDITADA { get; set; }
        public decimal TER_ENT_VALOR_RECIBO { get; set; }
        public DateTime TER_ENT_FECHA_INICIO { get; set; }
        public DateTime TER_ENT_FECHA_FIN { get; set; }
    }
}