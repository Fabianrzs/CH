using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Tabla de EstadoTramites
    /// <summary>
    /// Propiedades EstadoTramites
    /// </summary>
    public class EstadoTramites
    {
        public int ESS_ID { get; set; }
        public string ESS_DESCRIPCION { get; set; }
        public string ESS_SIGLA { get; set; }
        public int ESS_ESTADO { get; set; }
        public Guid ESS_CREADO_POR { get; set; }
        public DateTime ESS_FECHA_CREACION { get; set; }
        public Guid ESS_MODIFICADO_POR { get; set; }
        public DateTime ESS_FECHA_MODIFICACION { get; set; }
    }
}
