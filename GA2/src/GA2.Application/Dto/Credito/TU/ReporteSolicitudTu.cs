using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.TU
{
    public class ReporteSolicitudTu
    {
        public string nroDocumento { get; set; }
        public int tipoIdentificacion { get; set; }

        public DateTime fecExpeDocumento { get; set; }
        public string nroCelular { get; set; }
    }
}
