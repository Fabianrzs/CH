using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class ConceptoCuentaCleanDto
    {
        public int CuentaId { get; set; }
        public int ConceptoId { get; set; }
        public int IdAfiliado { get; set; }
        public int IdReintegro { get; set; }
    }
}
