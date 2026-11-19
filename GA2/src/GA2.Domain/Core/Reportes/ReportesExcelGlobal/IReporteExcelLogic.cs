using ClosedXML.Excel;
using GA2.Application.Dto.Reportes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesExcelGlobal
{
    public interface IReportesExcelLogic
    {
        Task<FileResult> GenerarExcel(ReportRequestExcelDto request);

        XLWorkbook EncabezadosExcel(XLWorkbook workbook, string[] encabezados);
        XLWorkbook DatosExcel(XLWorkbook workbook, List<ColumnData> datos);
        Task<FileResult> GenerarExcelRanking(ReportRequestExcelDto request);
        Task<FileResult> GenerarExcelSolicitudNomina(RequestReporteSolicitudNomina request); 
    }
}
