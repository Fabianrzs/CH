using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ObtenerDetalleFacturaSevenDto
    {
        public Guid SOC_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string DIRECCION { get; set; }
        public string FAC_DESC { get; set; }
        public int TOP_CODI { get; set; }
        public string NUMERO_RADICADO { get; set; }
        public int Emp_codi { get; set; }
        public int MPR_CONT { get; set; }
        public int TCR_ID { get; set; }
        public string PVD_CODA { get; set; }
        public string ARB_CODA { get; set; }
        public string ARB_CODC { get; set; }
        public string ARB_CODP { get; set; }
        public string DFA_DEST { get; set; }
        public double DFA_VALO { get; set; }
        public string PRO_CODI { get; set; }
        public int AFL_ID { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public int FAC_CONT { get; set; }
        public int FAC_NUME { get; set; }
        public DateTime FECHA_CREACION_FACTURA { get; set; }
    }
}
