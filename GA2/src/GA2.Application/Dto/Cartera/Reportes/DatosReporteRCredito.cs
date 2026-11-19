using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class DatosReporteRCredito
    {
        public ReporteRCreditoParametersDto Parametros { get; set; }
        public IEnumerable<ReporteRCreditoDatosGeneralesDto> DatosGenerales { get; set; }
        public IEnumerable<ReporteRCreditoInformacionLibranzaDto> InformacionLibranza { get; set; }
    }
}