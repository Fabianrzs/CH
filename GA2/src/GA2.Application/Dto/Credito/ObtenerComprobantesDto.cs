using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ObtenerComprobantesDto
    {
        public Guid SOC_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRE_AFILIADO { get; set; }
        public decimal SOC_DESEMBOLSO_APLICADO { get; set; }
        public int TOP_CODI { get; set; }
        public int EMP_CODI { get; set; }
        public int MPR_CONT { get; set; }
        public int TCR_ID { get; set; }
        public string PVD_CODA { get; set; }
        public string ARB_CODA { get; set; }
        public string ARB_CODC { get; set; }
        public double DFA_VALO { get; set; }
        public string PRO_CODI { get; set; }
        public double DMP_VALO { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public int FAC_CONT { get; set; }
        public int FAC_NUME { get; set; }


        public int CAD_MCO_NUME { get; set; }
        public DateTime CAD_MCO_FECH { get; set; }
        public int CAD_MOD_CODI { get; set; }
        public string Arb_csuc { get; set; }
        public string CAD_MCO_DESC { get; set; }
        public string CUENTA_CONTABLE { get; set; }
        public string CAD_DMC_DESC { get; set; }
        public string CAD_DMC_REFE { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public decimal CAD_DMC_VABA { get; set; }
        public string CAD_TER_CODA { get; set; }
        public string CAD_ARB_CODP { get; set; }
        public string CAD_ARB_CODS { get; set; }
        public DateTime FECHA_CREACION { get; set; }
    }
}

