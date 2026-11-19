using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Cierres.CierresCausacionesRealizadas
{
    public class RequestCierresCausacionesRealizadasDto
    {
        public DateTime? FechaCierreInicial { get; set; }
        public DateTime? FechaCierreFinal { get; set; }
        public IEnumerable<int> TiposCierre { get; set; }
    }
}
