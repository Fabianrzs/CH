using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante
{
    public class MovimientosContableParametersDto
    {
        public Guid IdComprobante { get; set; }
        public int? RegistrosPorPagina { get; set; }
        public int? NumeroPagina { get; set; }
    }    
    public class ObtenerComprobanteParametersDto
    {
        public Guid IdComprobante { get; set; }

    }

}

