using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class InformacionProyeccion
    {
        public int IdProyeccion { get; set; }
        public string Cargo { get; set; }
        public int AniosCargo { get; set; }
        public double Sueldo { get; set; }
        public decimal AhorroProgramado { get; set; }
        public decimal InteresesAhorroProgramado { get; set; }
        public decimal Cesantias { get; set; }
        public decimal InteresesCesantias { get; set; }
        public int AniosAntiguedad { get; set; }
    }
}
