using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.AuditoriaNovedades
{
    public class RequestValidaNovedadesFondoSolidaridadDto
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DescripcionConceptoNovedad { get; set; }
        public int IdConceptoNovedad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
