using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class DatosSeguros
    {
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public int IdAseguradoraVida { get; set; }
        public string AseguradoraVida { get; set; }
    }
}
