using ClosedXML.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteGeneralCartera;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteGeneralGarantias;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteMoraCartera;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReportePreliminarCartera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed
{
    public class TemplateExcel:ITemplateExcel
    {
        public TemplateExcel() { }

        public MemoryStream GetReportCorreosErrados(List<ReporteClienteCorreosErradosDto> correosErrados)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte Correos errados");
                TemplateMoraCartera tmc = new TemplateMoraCartera(worksheet,null);
                worksheet = tmc.GetSheetReporteMoraCartera();
                workbook.SaveAs(stream);
            }

            return stream;
        }

        public byte[] GetReporteCarteraTemplateExcel(List<ReporteGeneralCarteraDto> reporteGeneralCarteraDtos)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte General Cartera");
                TemplateGeneralCartera tgc=new TemplateGeneralCartera(worksheet,reporteGeneralCarteraDtos);
                worksheet = tgc.GetSheetReporteGeneralCartera();
                workbook.SaveAs(stream);
            }

            return stream.ToArray();
        }

        public MemoryStream GetReporteMoraCarteraExcel(List<ReporteGeneralMoralDto> moralDtos)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte General de mora");
                TemplateMoraCartera tmc=new TemplateMoraCartera(worksheet,moralDtos);
                worksheet = tmc.GetSheetReporteMoraCartera();
                workbook.SaveAs(stream);
            }

            return stream;
        }

        public byte[] GetReportePreliminarCarteraTemplateExcel(List<ReportePreliminarCarteraDto> reportePreliminarCarteraDtos)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte preliminar de cartera");
                TemplatePreliminarCartera tgc = new TemplatePreliminarCartera(worksheet, reportePreliminarCarteraDtos);
                worksheet = tgc.GetSheetReporteGeneralCartera();
                workbook.SaveAs(stream);
            }

            return stream.ToArray();
        }

        public MemoryStream GetReportGarantiasGeneral(List<ReporteGeneralGarantiasDto> garantias)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte preliminar de cartera");
                TemplateGeneralGarantias tgg= new TemplateGeneralGarantias(worksheet, garantias);
                worksheet = tgg.GetSheetReporteGeneralGarantias();
                workbook.SaveAs(stream);
            }

            return stream;
        }
    }
}
