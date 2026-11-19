using ClosedXML.Excel;
using GA2.Application.Dto.Reportes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesExcelGlobal
{
    public interface IReportesExcelInvariantCultureLogic
    {
        Task<FileResult> GenerarExcel(ReportRequestExcelFormatDto request);
        Task<FileResult> GenerarExcel(ReportRequestExcelFormatAsyncDto request);
        Task<FileResult> GenerarExcel(ExcelReportDto request);
        byte[] GenerarExcelBytes(ReportRequestExcelFormatDto request);
        Task<byte[]> GenerarExcelBytes(ReportRequestExcelFormatAsyncDto request);
        byte[] GenerarExcelBytes(ExcelReportDto excelReport);
    }
}
