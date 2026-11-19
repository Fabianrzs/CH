using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto.Catalogs
{
    public class DescripcionConceptoDto
    {
        public int IdConcepto { get; set; }
        public string DescripcionConcepto { get; set; }
        public int FormulaCalculoConcepto { get; set; }
        public int OrdenConcepto { get; set; }
        public char TipoConcepto { get; set; }
        public bool InteresConcepto { get; set; }
    }
}
