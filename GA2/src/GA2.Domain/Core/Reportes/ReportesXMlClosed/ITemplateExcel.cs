using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed
{
    public interface ITemplateExcel
    {
        byte[] GetReporteCarteraTemplateExcel(List<ReporteGeneralCarteraDto> reporteGeneralCarteraDtos);

        byte[] GetReportePreliminarCarteraTemplateExcel(List<ReportePreliminarCarteraDto> reportePreliminarCarteraDtos);

        MemoryStream GetReporteMoraCarteraExcel(List<ReporteGeneralMoralDto> moralDtos);

        MemoryStream GetReportGarantiasGeneral(List<ReporteGeneralGarantiasDto> garantias);

        MemoryStream GetReportCorreosErrados(List<ReporteClienteCorreosErradosDto> correosErrados);
    }
}
