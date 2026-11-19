using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera
{
    public class requestGarantiasDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int? TipoCredito { get; set; }
        public string? Identificacion { get; set; }
    }
}
