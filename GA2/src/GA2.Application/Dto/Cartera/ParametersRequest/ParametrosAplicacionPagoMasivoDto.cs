using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.ParametersRequest
{
    public class ParametrosAplicacionPagoMasivoDto
    {
        public int IdProceso { get; set; }
        public IEnumerable<long> IdDetalles { get; set; }
        public Guid CreadoPor { get; set; }
        public bool AplicanPagos { get; set; }
    }
}
