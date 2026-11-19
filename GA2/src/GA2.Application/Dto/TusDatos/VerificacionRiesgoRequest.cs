using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.TusDatos
{
    public class VerificacionActualizacionRequest
    {
        public string Identificacion { get; set; }
        public string DocumentType { get; set; }
    }
    public class VerificacionRiesgoRequest
    {
        public string Identificacion { get; set; }
        public string DocumentType { get; set; }
        public bool CheckHistory { get; set; }=true;
    }

    public class PollingVerificacionRiesgoRequest
    {
        public string Identificacion { get; set; }
        public string JobId { get; set; }
    }

}
