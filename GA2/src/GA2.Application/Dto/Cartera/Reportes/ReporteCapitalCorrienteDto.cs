using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteCapitalCorrienteDto
    {
        public long NumeroCredito { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int IdTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public IEnumerable<CapitalCorrienteDto> Periodos { get; set; }
        public decimal TotalCapital { get; set; }

    }
}
