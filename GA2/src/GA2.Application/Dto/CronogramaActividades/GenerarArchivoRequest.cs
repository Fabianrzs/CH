using System;

namespace GA2.Application.Dto
{
    public class GenerarArchivoRequest
    {
        public int IdUnidadEjecutora { get; set; }
        public int IdConceptoNovedad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsAlta { get; set; }
        public string Codigo { get; set; }
        public Guid IdCodigoUsuario { get; set; }
        public bool DHorizontal { get; set; }
        public bool OtroFormato { get; set; }
        public string NombreArchivo { get; set; }
    }
}
