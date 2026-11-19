using System.Collections.Generic;

namespace GA2.Application.Dto.Reportes
{
    public class ExcelSheetDto
    {
        public string SheetName { get; set; } = "Hoja1";
        public ColumnHeaderInfo[] ColumnsInfo { get; set; }
        public IEnumerable<string[]> ColumnData { get; set; }

        public ExcelSheetDto(ColumnHeaderInfo[] columnsInfo, IEnumerable<string[]> columnData, string sheetName = "Hoja1")
        {
            ColumnsInfo = columnsInfo;
            ColumnData = columnData;
            SheetName = sheetName;
        }

        public ExcelSheetDto()
        {

        }

    }
}
