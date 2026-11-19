using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class AprobacionCreditoAsesor
    {
        public Guid TRS_ID { get; set; }
        public string TAC_DESCRIPCION { get; set; } 
        public string USR_USERNAME { get; set; } 
        public string NOMBRES { get; set; }
        public int ACT_ESTADO { get; set; } 
    }
}
