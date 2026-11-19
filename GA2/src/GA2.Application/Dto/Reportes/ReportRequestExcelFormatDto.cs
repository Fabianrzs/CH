using System.Collections.Generic;

namespace GA2.Application.Dto.Reportes
{
    public class ReportRequestExcelFormatDto
    {
        public string SheetName { get; set; } = "Hoja1";
        public string Filename { get; set; } = "Reporte";
        public ColumnHeaderInfo[] ColumnsInfo { get; set; }
        public IEnumerable<string[]> ColumnData { get; set; }

        public ReportRequestExcelFormatDto(ColumnHeaderInfo[] columnsInfo, IEnumerable<string[]> columnData, string filename = "Reporte", string sheetName = "Hoja1")
        {
            ColumnsInfo = columnsInfo;
            ColumnData = columnData;
            Filename = filename;
            SheetName = sheetName;
        }

        public ReportRequestExcelFormatDto()
        {
            
        }

    }
}
