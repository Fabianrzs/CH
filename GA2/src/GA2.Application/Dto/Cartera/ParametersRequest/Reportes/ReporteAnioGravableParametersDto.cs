using System;
using System.Text.Json.Serialization;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteAnioGravableParametersDto
    {
        public long? NumeroCredito { get; set; }
        public int? TipoCredito { get; set; }
        public int Anio { get; set; }

        [JsonIgnore]
        public Guid IdProceso { get; set; }

        [JsonIgnore]
        public Guid IdUsuario { get; set; }
    }
}