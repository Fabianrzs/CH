using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConceptoNovedadDto
    {
        public int IdConceptoNovedad { get; set; }
        public int IdConcepto { get; set; }
        public string DescripcionConceptoNovedad { get; set; }
        public bool ConceptoNovedadAlta { get; set; }
    }
}
