using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador.DocumentosAct
{
    public class DocumentosActEstadoRequestDto
    {
        public Guid SOC_ID { get; set; }
        public string DCS_RUTA_IMAGEN { get; set; }

        public int DCS_APROBACION { get; set; }
    }
}
