using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class CausalEstadoClienteDto
    {
        public int CausalEstadoId { get; set; }
        public int? CausalId { get; set; }
        public int EstadoId { get; set; }
        public int ClienteId { get; set; }
        public string DescripcionCausal { get; set; }
        public string NombreEstado { get; set; }
        public string Razon { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaRetiro { get; set; }
        public bool Estado { get; set; }
    }
}
