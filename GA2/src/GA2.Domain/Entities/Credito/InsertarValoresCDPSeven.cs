using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class InsertarValoresCDPSeven
    {
        public Guid SOC_ID { get; set; }
        public string NUMERO_DOC { get; set; }
        public string NOMBRE_AFILIADO { get; set; }
        public decimal VALOR_PRESTAMO { get; set; }
        public decimal SOC_DESEMBOLSO_APLICADO { get; set; }
        public int MPR_CONT { get; set; }
        public int TCR_ID { get; set; }
        public int TID_ID { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public string Arb_csuc { get; set; }
        public double DMP_VALO { get; set; }
        public string ARB_CODC { get; set; }
        public string PRO_CODI { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public DateTime FECHA_CREACION_CDP { get; set; }
    }
}
