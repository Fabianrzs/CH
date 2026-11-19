using System;

namespace GA2.Application.Dto
{
    public class NovedadPorUnidadEjecutoraDto
    {
        public int IdUnidadEjecutora { get; set; }
        public string NombreUnidadEjecutora { get; set; }
        public int IdConceptoNovedad { get; set; }
        public string NombreConceptoNovedad { get; set; }
        public string CodigoAlta { get; set; }
        public string CodigoBaja { get; set; }
        public int NovedadesAlta { get; set; }
        public int NovedadesBaja { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
