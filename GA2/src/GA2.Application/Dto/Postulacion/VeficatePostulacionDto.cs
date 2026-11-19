using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Postulacion
{
    public class VeficatePostulacionDto
    {
        public Guid IdSolicitud { get; set; }    
        public string Observacion { get; set; }
        public int IdEtapa { get; set; }
    }
}
