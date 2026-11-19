using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed
{
    public interface ITemplateExcelReporteRCredito
    {
        byte[] GenerarReporteRCredito(DatosReporteRCredito parametersDto);
    }
}
