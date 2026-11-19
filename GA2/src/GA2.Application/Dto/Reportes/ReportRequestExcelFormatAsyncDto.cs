using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Reportes
{
    public class ReportRequestExcelFormatAsyncDto
    {
        public string Filename { get; set; }
        public ColumnHeaderInfo[] ColumnsInfo { get; set; }
        public IAsyncEnumerable<string[]> ColumnData { get; set; }

        public ReportRequestExcelFormatAsyncDto(ColumnHeaderInfo[] columnsInfo, IAsyncEnumerable<string[]> columnData, string filename = "Reporte")
        {
            ColumnsInfo = columnsInfo;
            ColumnData = columnData;
            Filename = filename;
        }

        public ReportRequestExcelFormatAsyncDto()
        {

        }

    }
}
