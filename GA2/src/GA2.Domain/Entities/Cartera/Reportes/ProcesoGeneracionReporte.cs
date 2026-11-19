using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ProcesoGeneracionReporte
    {
        public Guid PGR_ID { get; set; }
        public string PGR_TIPO_REPORTE { get; set; }
        public string PGR_NOMBRE_REPORTE { get; set; }
        public string PGR_ESTADO { get; set; }
        public int PGR_PROGRESO { get; set; }
        public string PGR_BLOB_CONTENEDOR { get; set; }
        public string PGR_BLOB_RUTA_ACCESO { get; set; }
        public string PGR_DETALLE_PROCESO { get; set; }
        public Guid PGR_CREADO_POR { get; set; }
        public DateTime PGR_FECHA_CREACION { get; set; }
        public DateTime PGR_FECHA_MODIFICACION { get; set; }
    }

}
