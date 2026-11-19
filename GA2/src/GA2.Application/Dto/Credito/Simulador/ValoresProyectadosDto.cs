using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador
{
    public class ValoresProyectadosDto
    {
        public Guid Svp_Id { get; set; }
        public Guid Sls_Id { get; set; }
        public decimal Svp_Sueldo { get; set; }
        public decimal Svp_Ahorros { get; set; }
        public decimal Svp_Cesantias { get; set; }
        public decimal Svp_Subsidio { get; set; }
        public decimal Svp_EA { get; set; }
    }
}
