using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class InformacionDesembolsoCarteraProducto
    {
        public Guid solicitudCreditoId { get; set; }
        public decimal ValorDesembolso { get; set; }
        public DateTime FechaDesembolso { get; set; }
    }
}
