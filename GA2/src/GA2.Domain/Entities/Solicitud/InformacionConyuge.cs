using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class InformacionConyuge
    {

        public int IdConyuge { get; set; }
        public int IdTipoIdentificacionConyuge { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string PrimerNombreConyuge { get; set; }
        public string SegundoNombreConyuge { get; set; }
        public string PrimerApellidoConyuge { get; set; }
        public string SegundoApellidoConyuge { get; set; }
        public bool ConyugeActivo { get; set; }
        public string NombreTipoIdentificacionConyuge { get; set; }
    }
}
