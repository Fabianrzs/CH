using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Catalogs.ComprobantesCartera
{
    public class TipoComprobanteDto
    {
        public int TipoComprobanteId { get; set; }
        public string TipoComprobante { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        public Guid CreadoPor { get; set; }
        [JsonIgnore]
        public DateTime FechaModificacion { get; set; }
        [JsonIgnore]
        public Guid ModificadoPor { get; set; }
        public bool Activo { get; set; }
        public string Codigo { get; set; }
        public int TipoContabilidad { get; set; }
    }
}
