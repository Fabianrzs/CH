using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public interface IColumnData
    {
        List<string> Data { get; set; }
    }

    public class ColumnDataEncabezado : IColumnData
    {
        public List<string> Data { get; set; }
    }

    public class ColumnDataCuerpo : IColumnData
    {
        public List<string> Data { get; set; }
    }

    public class ColumnDataFin : IColumnData
    {
        public List<string> Data { get; set; }
    }

    public class RequestReporteSolicitudNomina
    {
        public string[] ColumnNameEncabezado { get; set; }
        public List<ColumnDataEncabezado> ColumnDataEncabezado { get; set; }

        public string[] ColumnNameCuerpo { get; set; }
        public List<ColumnDataCuerpo> ColumnDataCuerpo { get; set; }

        public string[] ColumnNameFin { get; set; }
        public List<ColumnDataFin> ColumnDataFin { get; set; }
    }

}
