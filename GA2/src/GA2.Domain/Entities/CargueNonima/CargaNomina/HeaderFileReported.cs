using System;

namespace GA2.Domain.Entities
{
    public class HeaderFileReported
    {
        public int CONSECUTIVE { get; set; }
        public int FILE_TIPO { get; set; }
        public string NOMBRE_NOMINA_UNIDAD_EJECUTORA { get; set; }
        public int ID_UNIDAD_EJECUTORA { get; set; }
        public DateTime FECHA_APORTES { get; set; }
    }
}
