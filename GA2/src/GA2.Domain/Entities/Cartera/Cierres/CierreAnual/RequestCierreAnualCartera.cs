using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    public class RequestCierreAnualCartera
    {
        public int AnioCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
    }
}
