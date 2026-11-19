using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class TasaSimulador
    {
        public string SIM_ID { get; set; }
        public string SIM_DESCRIPCION { get; set; }
        public decimal SIM_TASA_EA { get; set; }
        public int SIM_MINIMO_MESES_PLAZO { get; set; }
        public int SIM_MAXIMO_MESES_PLAZO { get; set; }
        public int SIM_ESTADO { get; set; }
        public Guid SIM_CREADO_POR { get; set; }
        public DateTime SIM_FECHA_CREACION { get; set; }
        public Guid SIM_MODIFICADO_POR { get; set; }
        public DateTime SIM_FECHA_MODIFICACION { get; set; }
        public string TPA_ID { get; set; }
        public string TPA_DESCRIPCION { get; set; }
        public string PAR_ESTADO { get; set; }
        public string PAR_ID { get; set; }
        public string PAR_TASA_EA { get; set; }
        public decimal SIM_MIN_CANON_INICIAL { get; set; }
        public decimal SIM_MAX_CANON_INICIAL { get; set; }
        public decimal SIM_MAX_OPC_COMPRA { get; set; }

        public decimal SIM_TASA_NOMINAL_MENSUAL { get; set; }
        public string SIM_SEGURO_HOGAR { get; set; }
    }
}











