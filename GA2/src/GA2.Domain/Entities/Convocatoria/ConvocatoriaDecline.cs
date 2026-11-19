using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ConvocatoriaDecline
    {
        public Guid SolicitudId { get; set; }
        public string TipoModalidad { get; set; }
        public string NombreAfiliado { get; set; }
        public string Categoria { get; set; }
        public string UnidadEjecutora { get; set; }
        public int EtapaProceso { get; set; }
        public long NumeroDcoumento { get; set; }
        public int NoConvocatoria { get; set; }
    }
}
