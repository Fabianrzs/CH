using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class SocSlsSimulacionCreditoDto
    {
        public Guid? socId { get; set; }
        public Guid? slsId { get; set; }
        public decimal slsSecFinan{ get; set; }
        public decimal slsSecReal { get; set;}
        public DateTime slsFecha { get; set;}
        public decimal socSecFinan { get; set;}
        public decimal socSecReal { get; set;}
        public DateTime socSecFecha { get; set;}
        public string socXml { get; set; }
        public string slsXml { get; set; }
        public bool slsValidacionIdentidad { get; set; }
    }
}
