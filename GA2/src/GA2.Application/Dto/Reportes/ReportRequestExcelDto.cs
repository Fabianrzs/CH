using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public class ReportRequestExcelDto
    {
        public List<ColumnData> ColumnDataList { get; set; }
        public string[] ColumnName { get; set; }

    }
    public class ColumnData
    {
        public List<string> Data { get; set; }
    }
}
