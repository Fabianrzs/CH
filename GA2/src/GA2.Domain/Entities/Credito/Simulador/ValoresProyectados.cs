using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.Simulador
{
    public class ValoresProyectados
    {
        public Guid SVP_ID { get; set; }
        public Guid SLS_ID { get; set; }
        public decimal SVP_SUELDO { get; set; }
        public decimal SVP_AHORROS { get; set; }
        public decimal SVP_CESANTIAS { get; set; }
        public decimal SVP_SUBSIDIO { get; set; }
        public decimal SVP_EA { get; set; }
    }
}
