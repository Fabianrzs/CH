using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class PostulanteConvocatoriaDto
    {
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
        public string NoRadicado { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public string Estado { get; set; }
    }
}
