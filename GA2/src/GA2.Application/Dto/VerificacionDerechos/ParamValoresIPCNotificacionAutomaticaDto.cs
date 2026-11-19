using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.VerificacionDerechos
{
    public class ParamValoresIPCNotificacionAutomaticaDto
    {

        public int Id { get; set; }

        public int Anio { get; set; }

        public int Mes { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public Guid SltId { get; set; }

    }
}
