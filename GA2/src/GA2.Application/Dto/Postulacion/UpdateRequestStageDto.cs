using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Postulacion
{
    public class UpdateRequestStageDto
    {
        public Guid idSolicitud { get; set; }
        public int Etapa { get; set; }
        public string? Observacion { get; set; }
    }
}
