using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ReporteAfiliadosRequestDto
    {
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public int IdFuerza { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public int IdCategoria { get; set; }
        public int IdGrado { get; set; }
        public int IdTipoVinculacion { get; set; }
        public int estado { get; set; }

        public DateTime? FechaInscripcionDesde { get; set; }
        public DateTime? FechaInscripcionHasta { get; set; }
        public DateTime? FechaActualizacionDesde { get; set; }
        public DateTime? FechaActualizacionHasta { get; set; }

        public int RegimenDesde { get; set; }
        public int RegimenHasta { get; set; }
    }

}
