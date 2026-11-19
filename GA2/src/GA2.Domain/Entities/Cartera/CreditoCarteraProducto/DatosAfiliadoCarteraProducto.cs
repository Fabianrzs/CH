using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class DatosAfiliadoCarteraProducto
    {
        public string Identificacion { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public int Edad { get; set; }
        public int Fuerza { get; set; }
        public decimal RecursosFuturos { get; set; }
    }
}
