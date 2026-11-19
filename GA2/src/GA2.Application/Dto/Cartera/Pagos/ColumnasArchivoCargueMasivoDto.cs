using System;

namespace GA2.Application.Dto
{
    public class ColumnasArchivoCargueMasivoDto
    {
        public int Id { get; set; }
        public string IdColumna { get; set; }
        public string Encabezado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int IdUnidadEjecutora { get; set; }
    }
}
