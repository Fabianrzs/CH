using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ConceptosSaldosBloqueos
    {
        public int CCT_ID { get; set; }
        public int CTA_ID { get; set; }
        public int CNC_ID { get; set; }
        public decimal CCT_SALDO { get; set; }
        public DateTime CCT_FECHA_ULTIMO_APORTE { get; set; }
        public int CCT_INGRESOS { get; set; }
        public decimal CCT_SVA { get; set; }
        public int BCC_ID { get; set; }
        public bool CCT_BLOQUEO { get; set; }
    }
}
