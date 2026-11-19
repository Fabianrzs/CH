using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConsultarConvocatoriaRequest
    {
        public int? NoConvocatoria { get; set; } = null;    
        public int? EstadoConvocatoriaId { get; set; } = null;
        public DateTime? FechaApertura { get; set; } = null;
        public DateTime? FechaCierre { get; set; } = null;
        public DateTime? FechaPublicacion { get; set; } = null; 

    }
}
