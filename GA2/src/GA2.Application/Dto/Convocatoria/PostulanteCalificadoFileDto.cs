using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Convocatoria
{
    public class PostulanteCalificadoFileDto
    {
        public string NoRadicado { get; set; }
        public Guid SolicitudId { get; set; }
        public string TipoModalidad { get; set; }
        public string ModalidadName { get; set; }
        public string Afiliado { get; set; }
        public int TipoDocumento { get; set; }
        public long NumeroDocumento { get; set; }
        public decimal puntaje { get; set; }
        public string EtapaProceso { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Categoria { get; set; }
        public string Usuario { get; set; }
        public object File { get; set; }
    }
}
