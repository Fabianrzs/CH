using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConvocatoriaDeclineDto
    {
        public Guid SolicitudId { get; set; }
        public string TipoModalidad { get; set; }
        public string NombreAfiliado { get; set; }
        public string Categoria { get; set; }
        public string UnidadEjecutora { get; set; }
        public string EtapaProceso { get; set; }
        public string NoRadicado { get; set; }
    }
}
