using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class AfiliadoReinegroList
    {
        public int IdAfiliadoReintegro { get; set; }
        public long NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public int TipoDocumentoId { get; set; }
        public string Apellidos { get; set; }
        public Guid IdReintegro { get; set; }
        public int Id_Tipo_Reintegro { get; set; }
        public decimal ValorSueldo { get; set; }
        public string EstadoAfiliao { get; set; }
    }
}
