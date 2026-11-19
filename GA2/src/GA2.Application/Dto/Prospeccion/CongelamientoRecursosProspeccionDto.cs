using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class CongelamientoRecursosProspeccionDto
    {
        public Guid SimulacionId { get; set; }
        public string DescripcionRecurso { get; set; }
        public decimal ValorRecurso { get; set; }
        public int OrdenRecurso { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
