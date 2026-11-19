using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TramiteDesafiliacionDto
    {
        public int IdTramiteDesafiliacion { get; set; }
        public int IdAfiliacion { get; set; }
        public Guid IdSolicitud { get; set; }
        public Guid IdModalidad { get; set; }
        public decimal Valor { get; set; }
    }
}
