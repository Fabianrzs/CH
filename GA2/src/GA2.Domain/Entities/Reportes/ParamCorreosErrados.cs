using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public  class ParamCorreosErrados
    {
        public int cod_fuerza { get; set; }
        public int cod_categoria { get; set; }

        public int unidad_ejecutora { get; set; }

        public Boolean Verificacion { get; set; }
    }
}
