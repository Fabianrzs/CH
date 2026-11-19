using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    public class ObtenerComprobantePorProcesoDto
    {
        public string? CodigoProceso { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFinal { get; set; }
        public string? ComprobanteId { get; set; }
        public string? TipoOperacion { get; set; }
        public int? Estado { get; set; }
        public Guid? DetalleMovimientoId { get; set; }
        public int? ComprobanteSevenId { get; set; }


    }
}
