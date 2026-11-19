using System;

namespace GA2.Domain.Entities
{
    public class ReporteOpcioCompra
    {	
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public string AFILIADO { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
    }
}
