using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{

    public class ReporteFuerzaDto
    {
        public string IdFuerza { get; set; }
        public string IdEstado { get; set; }
        public int Conteo { get; set; }
    }

    public class ReporteFuerzaResumenDto
    {
        public IEnumerable<ReporteFuerzaAgrupadoDto> Fuerzas { get; set; }
        public IEnumerable<EstadoConteoDto> ConteoPorEstado { get; set; }
        public int TotalGeneral { get; set; }
    }

    public class ReporteFuerzaAgrupadoDto
    {
        public string NombreFuerza { get; set; }
        public List<EstadoConteoDto> Estados { get; set; }
        public int TotalFuerza { get; set; }
    }

    public class EstadoConteoDto
    {
        public string Estado { get; set; }
        public int Total { get; set; }
    }
}
