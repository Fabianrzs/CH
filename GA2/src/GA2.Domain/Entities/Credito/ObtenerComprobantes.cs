using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ObtenerComprobantes
    {
        public Guid SOC_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRE_AFILIADO { get; set; }
        public int TOP_CODI { get; set; }
        public int EMP_CODI { get; set; }
        public int MPR_CONT { get; set; }
        public int TCR_ID { get; set; }
        public string PVD_CODA { get; set; }
        public string ARB_CODA { get; set; }
        public string ARB_CODC { get; set; }
        public double DFA_VALO { get; set; }
        public string PRO_CODI { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public int FAC_CONT { get; set; }
        public int FAC_NUME { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}
