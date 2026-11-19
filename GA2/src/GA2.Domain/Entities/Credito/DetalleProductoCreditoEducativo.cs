using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DetalleProductoCreditoEducativo
    {
        public decimal VALOR_CREDITO { get; set; }
        public decimal PORCENTAJE_FINACIACION { get; set; }
        public int PLAZO { get; set; }
    }
}
