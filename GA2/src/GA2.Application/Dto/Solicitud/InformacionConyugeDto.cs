using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class InformacionConyugeDto
    {
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }

    public class InformacionConyugeRespuesta
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
