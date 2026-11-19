using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public  class SocSlsSimulacionCredito
    {
        public Guid? soc_id { get; set; }
        public Guid? sls_id { get; set; }
        public decimal sls_end_sec_finan { get; set; }
        public decimal sls_end_sec_real { get; set; }
        public DateTime sls_fecha_con_tu { get; set; }
        public decimal soc_end_sec_finan { get; set; }
        public decimal soc_end_sec_real { get; set; }
        public DateTime soc_fecha_con_tu { get; set; }
        public string soc_xml { get; set; }
        public string sls_xml { get; set; }
        public bool SLS_VALIDACION_IDENTIDAD { get; set; }
    }
}
