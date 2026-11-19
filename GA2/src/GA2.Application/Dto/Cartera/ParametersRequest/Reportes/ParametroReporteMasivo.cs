using System;
using System.Collections.Generic;
using System.Linq;
using GA2.Application.Dto.Cartera.Reportes;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ParametroReporteMasivo
    {
        public Guid IdProceso { get; set; }

        public IAsyncEnumerable<ArchivoReporte> DatosReportes { get; set; }

        public int Total { get; set; }
    }
}