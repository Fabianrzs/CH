using System;
using System.Text.Json.Serialization;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Extractos
{
    public class ReporteExtractoMasivoParametersDto
    {
        public long? TipoCredito { get; set; }
        public int? UnidadEjecutora { get; set; }
        public long? NumeroCredito { get; set; }
        public int Anio { get; set; }
        public int? Mes { get; set; }

        [JsonIgnore]
        public Guid IdProceso { get; set; }

        [JsonIgnore]
        public Guid IdUsuario { get; set; }

        public TipoExtracto TipoExtracto => Mes is null ? TipoExtracto.Anual : TipoExtracto.Mensual;
    }
}
