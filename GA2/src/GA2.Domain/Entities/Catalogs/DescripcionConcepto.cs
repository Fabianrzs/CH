using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class DescripcionConcepto
    {

        public int CNC_ID { get; set; }
        public string CNC_DESCRIPCION { get; set; }
        public int CNC_FORMULA_CALCULO { get; set; }
        public int CNC_ORDEN { get; set; }
        public char CAT_TIPO_CONCEPTO { get; set; }
        public bool CNC_INTERES { get; set; }

    }
}
  