using System;

namespace GA2.Application.Dto
{
    public class NovedadesAfiliadoDto
    {
        public int IdNovedad { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string CodigoMilitar { get; set; }
        public int IdConceptoNovedad { get; set; }
        public DateTime FechaInicioNovedad { get; set; }
        public DateTime FechaFinNovedad { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Valor { get; set; }
        public bool EsAlta { get; set; }
        public bool EsBaja { get; set; }
        public string Observaciones {get; set; }
        public string? Token { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string NombreUnidadEjecutora { get; set; }
        public string NombreConceptoNovedad { get; set; }
        public string CodigoNovedadAlta { get; set; }
        public string CodigoNovedadBaja { get; set; }

    }
}
