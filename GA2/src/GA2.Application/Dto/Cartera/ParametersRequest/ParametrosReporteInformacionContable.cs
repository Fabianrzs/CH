using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest
{
    public class ParametrosReporteInformacionContable
    {
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string CuentaContable { get; set; }
    }
}
