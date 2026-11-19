using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Catalogs.ComprobantesCartera
{
    public class TipoBeneficiarioDto
    {
        public int TipoBeneficiarioId { get; set; }
        public string TipoBeneficiario { get; set; }
        public string TablaBeneficiario { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        public Guid CreadoPor { get; set; }
        [JsonIgnore]
        public DateTime FechaModificacion { get; set; }
        [JsonIgnore]
        public Guid ModificadoPor { get; set;}
        public bool Activo { get; set; }

    }
}
