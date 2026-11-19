using System;

namespace GA2.Application.Dto
{
    public class ErrorCargueArchivoDto
    {
        public Guid NmnId { get; set; }
        public int DctId { get; set; }
        public int NumeroLinea { get; set; }
        public int InconsistenciasNombre { get; set; }
        public int InconsistenciasEncabezado { get; set; }
        public int InconsistenciasCuerpo { get; set; }
        public int InconsistenciasFin { get; set; }
        public int CodigoIncidencia { get; set; }
        public string Descripcion { get; set; }
        public string TipoError { get; set; }
        public DateTime FechaCreacion { get; set; }      
        public int EstadoError { get; set; }

    }
}
