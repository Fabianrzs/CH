using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Postulacion
{
    public class VerificatePostulacionResponse
    {
        public int IdVerificacion { get; set; }
        public Guid IdSolicitud { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int EtapaId { get; set; }
        public int StatusId { get; set; }
        public string Estado { get; set;}
        public string NombreEtapa { get; set; }
        public string UserName { get; set; }
    }
}
