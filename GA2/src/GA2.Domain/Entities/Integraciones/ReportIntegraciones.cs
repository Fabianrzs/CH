using System;

namespace GA2.Domain.Entities.Integraciones
{
    public class ReportIntegracion
    {
        public Guid ID_INTEGRACIONES { get; set; }

        public string TIPOSERVICIO { get; set; } = string.Empty;

        public string METODOHTTP { get; set; } = string.Empty;

        public string ENDPOINT { get; set; } = string.Empty;

        public string REQUEST { get; set; } = string.Empty;

        public string? RESPONSE { get; set; }

        public int? STATUSCODE { get; set; }

        public bool SUCCESS { get; set; }

        public string? ERROR { get; set; }

        public DateTime FECHAINICIO { get; set; }

        public DateTime FECHAFIN { get; set; }

        public long DURACIONMS { get; set; }
    }

}
