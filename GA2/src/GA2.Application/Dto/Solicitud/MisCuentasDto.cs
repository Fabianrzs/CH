using GA2.Domain.Entities.Solicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class MisCuentasDto
    {
        public decimal? SaldoCuenta { get; set; }
        public string? Cuenta { get; set; }
        public bool? TrasladoAutorizado { get; set; }
    }
}
