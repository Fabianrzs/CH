using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Convocatoria
{
    public class UpdateFechaCierreDto
    {
        public DateTime FechaPublicacionDiario { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public int IdConvocatoria { get; set; }

    }
}
