using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class DatosProductoCarteraProducto
    {
        public string NumeroCredito { get; set; }
        public int TipoCredito { get; set; }
        public int? LineaCredito { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal CanonInial { get; set; }
        public decimal OpcionDeCompra { get; set; }
        public decimal TipoAbonoExtra { get; set; }
        public decimal ValorAbonoExtra { get; set; }
        public string TasaCreditoEA { get; set; }
        public string Score { get; set; }
        public string Calificacion { get; set; }
        public int PlazoTotal { get; set; }
        public string NumeroRadicado { get; set; }
        public List<InformacionDesembolsoCarteraProducto> Desembolsos { get; set; }

    }
}
