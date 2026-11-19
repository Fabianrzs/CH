using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class EstadoCausalClienteDto
    {
        public int EstadoCausalClienteId { get; set; }
        public string Causal { get; set; }
        public int EstadoId { get; set; }
        public bool Activo { get; set; }
    }
}
