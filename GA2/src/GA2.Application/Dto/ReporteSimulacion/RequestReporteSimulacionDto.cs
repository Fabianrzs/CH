using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.ReporteSimulacion
{
    public class RequestReporteSimulacionDto
    {
        public Guid SimulacionId { get; set; }//sls_id || soc_id
        public Guid Smc_id { get; set; }
        public int Afl_id { get; set; }
    }
}
