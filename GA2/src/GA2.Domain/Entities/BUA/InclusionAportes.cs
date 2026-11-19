using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class InclusionAportes
    {
        public int IPD_ID { get; set; }
        public int IAP_ID { get; set; }
        public DateTime IPD_PERIODO_DESDE { get; set; }
        public DateTime IPD_PERIODO_HASTA { get; set; }
        public int CNC_ID { get; set; }
        public decimal IPD_VALOR { get; set; }
        public int IPD_CUOTAS { get; set; }
        public int IPD_CUOTAS_ANTIGUAS { get; set; }
    }
}
