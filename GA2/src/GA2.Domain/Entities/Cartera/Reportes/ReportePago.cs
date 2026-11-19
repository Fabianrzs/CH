using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReportePago
    {
        public Guid CRE_ID { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int UEJ_ID { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public string AAP_CODIGO { get; set; }
        public string AAP_NOMBRE { get; set; }
        public decimal CPG_VALOR_PAGO { get; set; }
        public DateTime CPG_FECHA_PAGO { get; set; }
        public DateTime CPG_FECHA_APLICACION { get; set; }

    }

}
