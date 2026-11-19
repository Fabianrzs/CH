using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class NovedadesCronogramaDto
    {
        public int IdNovedad { get; set; }
        public int IdConceptoNovedad { get; set; }
        public string CodigoNovedadAlta { get; set; }
        public string CodigoNovedadBaja { get; set; }
        public int IdUnidadEjecutora { get; set; }
    }
}
