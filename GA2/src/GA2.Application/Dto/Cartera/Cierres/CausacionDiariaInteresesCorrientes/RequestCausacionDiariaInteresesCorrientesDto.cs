using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes
{
    public class RequestCausacionDiariaInteresesCorrientesDto
    {
        public DateTime FechaCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
        public int? TipoCierre { get; set; }

    }
}
