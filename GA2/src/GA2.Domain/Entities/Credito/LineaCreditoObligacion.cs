using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class LineaCreditoObligacion
    {

		public Guid OLN_ID { get; set; }
        public string OLN_LINEA_CREDITO { get; set; }
		public decimal OLN_SALDO_OBLIGACION { get; set; }
		public decimal OLN_VALOR_CUOTA { get; set; }
		public string OLN_ESTADO { get; set; }
		public int OLN_CUOTAS_PACTADAS { get; set; }
		public int OLN_CUOTAS_CANCELADAS { get; set;  }
		public int OLN_CUOTAS_RESTANTES { get; set; }
		public string OLN_NUMERO_DOCUMENTO { get; set; }
		public bool OLN_ESTADO_RECALCULAR { get; set; }
		public string OLN_CALIDAD { get; set; }
		public string OLN_NUMERO_OBLIGACION { get; set; }
		public string OLN_TIPO_OBLIGACION { get; set; }
		public DateTime OLN_FECHA_LINEA { get; set; }
    }
}
