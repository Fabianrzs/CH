using System;

namespace GA2.Domain.Entities
{
    public class ColumnasArchivoCargueMasivo
    {
        public int CACM_ID { get; set; }
        public string CACM_ID_COLUMNA { get; set; }
        public string CACM_ENCABEZADO { get; set; }
        public DateTime? CACM_FECHA_CREACION { get; set; }
        public DateTime? CACM_FECHA_MODIFICACION { get; set; }
        public int UEJ_ID { get; set; }
    }
}
