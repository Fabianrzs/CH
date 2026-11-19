using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ReportePlanillaPagosIndivCesantias
    {
        public Guid tipoSubModalidadId { get; set; }
        public string identificacion { get; set; }
        public int tipoIdentificacion { get; set; }
        public Guid solicitudTareaId { get; set; }
        public string tipoCuenta { get; set; }

    }
}
