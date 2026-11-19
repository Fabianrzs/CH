using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class RespuestaTerceroBeneficiario
    {
        public Guid TER_ID { get; set; }
        public int TID_ID_FK { get; set; }
        public int TID_ERP { get; set; }
        public int TID_EMPRESARIAL { get; set; }
        public string TID_DESCRIPCION { get; set; }
        public string TER_NUMERO_DOCUMENTO { get; set; }
        public string TER_PRIMER_NOMBRE { get; set; }
        public string TER_SEGUNDO_NOMBRE { get; set; }
        public string TER_PRIMER_APELLIDO { get; set; }
        public string TER_SEGUNDO_APELLIDO { get; set; }
        public string TER_NOMBRE_RAZON_SOCIAL { get; set; }
        public string TER_DIRECCION { get; set; }
        public int DPC_CODIGO { get; set; }
        public string DPD_CODIGO { get; set; }
        public int DPP_CODIGO { get; set; }
        public Guid ENT_ID { get; set; }
        public String ENT_NOMBRE_RAZON_SOCIAL { get; set; }
        public int ENT_CODIGO_BANCO { get; set; }
        public int CVL_ID { get; set; }
        public string CVL_CODIGO { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public string TER_NUMERO_CUENTA { get; set; }
        public decimal? TER_VALOR_RETIRAR { get; set; }
        public int? PVD_CODD { get; set; }
    }
}
