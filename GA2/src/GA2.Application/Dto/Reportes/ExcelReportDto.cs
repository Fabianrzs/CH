using System.Collections.Generic;

namespace GA2.Application.Dto.Reportes
{
    public class ExcelReportDto
    {
        public string Filename { get; set; }
        public IEnumerable<ExcelSheetDto> Sheets { get; set; }

        public ExcelReportDto(IEnumerable<ExcelSheetDto> sheets, string filename = "Reporte")
        {
            Sheets = sheets;
            Filename = filename;
        }

        public ExcelReportDto()
        {

        }

    }
}
