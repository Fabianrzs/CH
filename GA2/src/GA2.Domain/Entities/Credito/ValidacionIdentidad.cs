using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ValidacionIdentidadAfiliado
    {
        public int Caso { get; set; }
        public string Mensaje { get; set; }
        public bool Acceso { get; set; }
    }
}
