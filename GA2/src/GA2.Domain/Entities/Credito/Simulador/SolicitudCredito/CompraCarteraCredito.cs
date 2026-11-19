using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.Simulador.SolicitudCredito
{
    public class CompraCarteraCredito
    {
        public Guid CCC_ID { get; set; }
        public decimal CCC_VALOR_COMPRA_CARTERA { get; set; }
        public decimal CCC_CUOTA { get; set; }
        public decimal CCC_PLAZO { get; set; }
        public decimal CCC_VALOR_FINAL_PRESTAMO_CARTERA { get; set; }
        public Guid SLS_ID { get; set; }
        public string CCC_NUMERO_OBLIGACION_CARTERA { get; set; }
    }
}
