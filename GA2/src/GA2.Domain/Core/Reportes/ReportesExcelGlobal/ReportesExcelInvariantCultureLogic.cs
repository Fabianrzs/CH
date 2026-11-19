using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GA2.Application.Dto.Reportes;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ClosedXML.Excel;
using System.Globalization;
using System.Linq;

namespace GA2.Domain.Core.Reportes.ReportesExcelGlobal
{
    public class ReportesExcelInvariantCultureLogic : IReportesExcelInvariantCultureLogic
    {
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ReportesExcelInvariantCultureLogic(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));

        }

        public byte[] GenerarExcelBytes(ReportRequestExcelFormatDto request)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(request.SheetName);
            var startRow = 10;

            InsertarEncabezado(worksheet);
            InsertarEncabezadoTabla(worksheet, request.ColumnsInfo, startRow);
            InsertarDatos(worksheet, request.ColumnsInfo, request.ColumnData, startRow + 1);
            InsertarPiePagina(worksheet);
            AjustarContenido(worksheet, request.ColumnsInfo.Length, startRow);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        public byte[] GenerarExcelBytes(ExcelReportDto excelReport)
        {
            var workbook = new XLWorkbook();

            foreach (var request in excelReport.Sheets)
            {
                var worksheet = workbook.Worksheets.Add(request.SheetName);
                var startRow = 10;

                InsertarEncabezado(worksheet);
                InsertarEncabezadoTabla(worksheet, request.ColumnsInfo, startRow);
                InsertarDatos(worksheet, request.ColumnsInfo, request.ColumnData, startRow + 1);
                InsertarPiePagina(worksheet);
                AjustarContenido(worksheet, request.ColumnsInfo.Length, startRow);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        public async Task<byte[]> GenerarExcelBytes(ReportRequestExcelFormatAsyncDto request)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hoja1");
            var startRow = 10;

            InsertarEncabezado(worksheet);
            InsertarEncabezadoTabla(worksheet, request.ColumnsInfo, startRow);
            await InsertarDatos(worksheet, request.ColumnsInfo, request.ColumnData, startRow + 1);
            InsertarPiePagina(worksheet);
            AjustarContenido(worksheet, request.ColumnsInfo.Length, startRow);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        public Task<FileResult> GenerarExcel(ReportRequestExcelFormatDto request)
        {
            var excelBytes = GenerarExcelBytes(request);
            var result = new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{request.Filename}.xlsx"
            };

            return Task.FromResult<FileResult>(result);
        }

        public Task<FileResult> GenerarExcel(ExcelReportDto request)
        {
            var excelBytes = GenerarExcelBytes(request);
            var result = new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{request.Filename ?? "Reporte"}.xlsx"
            };

            return Task.FromResult<FileResult>(result);
        }

        public async Task<FileResult> GenerarExcel(ReportRequestExcelFormatAsyncDto request)
        {
            var excelBytes = await GenerarExcelBytes(request);
            var result = new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{request.Filename}.xlsx"
            };

            return result;
        }

        public IXLWorksheet InsertarEncabezado(IXLWorksheet worksheet)
        {
            var header = Path.Combine(_webHostEnvironment.ContentRootPath, "Img", "REPORTE_ENCABEZADO.png");
            worksheet.AddPicture(header).MoveTo(worksheet.Cell(1, 1));
            return worksheet;
        }

        public IXLWorksheet InsertarPiePagina(IXLWorksheet worksheet)
        {
            var footer = Path.Combine(_webHostEnvironment.ContentRootPath, "Img", "REPORTE_PIE_PAGINA.png");
            var footerImageCell = worksheet.Cell(worksheet.LastRowUsed().RowNumber() + 2, 1);
            worksheet.AddPicture(footer).MoveTo(footerImageCell);
            return worksheet;
        }

        public IXLWorksheet AjustarContenido(IXLWorksheet worksheet, int lenghtColums, int startRow)
        {
            var lastRow = worksheet.LastRowUsed().RowNumber();
            worksheet.Columns(1, lenghtColums).AdjustToContents(startRow, Math.Min(lastRow, 100));
            return worksheet;
        }

        private IXLWorksheet InsertarEncabezadoTabla(IXLWorksheet worksheet, IEnumerable<ColumnHeaderInfo> headers, int row)
        {
            var headerRow = worksheet.Row(row);
            var encabezados = headers.Select(x => x.Name).ToArray();

            for (int col = 0; col < encabezados.Length; col++)
            {
                var cell = headerRow.Cell(col + 1);
                cell.Value = encabezados[col];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1c3f8b");
                cell.Style.Font.FontColor = XLColor.White;
            }

            return worksheet;
        }

        private IXLWorksheet InsertarDatos(IXLWorksheet worksheet, ColumnHeaderInfo[] headerInfo, IEnumerable<string[]> datos, int startRow)
        {
            foreach (var data in datos)
            {
                InsertarFila(worksheet, headerInfo, data, startRow);
                startRow++;
            }

            return worksheet;
        }

        private async Task<IXLWorksheet> InsertarDatos(IXLWorksheet worksheet, ColumnHeaderInfo[] headerInfo, IAsyncEnumerable<string[]> datos, int startRow)
        {
            await foreach (var data in datos)
            {
                InsertarFila(worksheet, headerInfo, data, startRow);
                startRow++;
            }

            return worksheet;
        }

        private IXLWorksheet InsertarFila(IXLWorksheet worksheet, ColumnHeaderInfo[] headerInfo, string[] data, int row)
        {
            var dataRow = worksheet.Row(row);

            for (int col = 0; col < data.Length; col++)
            {
                var cellValue = data[col];
                var cell = dataRow.Cell(col + 1);

                switch (headerInfo[col].FormatType)
                {
                    case ColumnFormatType.Numerico:

                        if (long.TryParse(cellValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out long numberValue))
                        {
                            cell.Value = numberValue;
                            cell.Style.NumberFormat.NumberFormatId = 1;
                        }
                        else
                            cell.Value = cellValue;

                        break;
                    case ColumnFormatType.Decimal:

                        if (decimal.TryParse(cellValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decimalValue))
                        {
                            cell.Value = decimalValue;
                            cell.Style.NumberFormat.NumberFormatId = 4;
                        }
                        else
                            cell.Value = cellValue;

                        break;

                    case ColumnFormatType.Fecha:
                        if (DateTime.TryParse(cellValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime dateValue))
                        {
                            cell.Value = dateValue;
                            cell.Style.NumberFormat.Format = "dd/mm/yyyy";
                        }
                        else
                            cell.Value = cellValue;

                        break;
                    case ColumnFormatType.Porcentaje:

                        if (decimal.TryParse(cellValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal porcentajeValue))
                        {
                            cell.Value = porcentajeValue / 100m;
                            cell.Style.NumberFormat.Format = "0.######%";
                        }
                        else
                            cell.Value = cellValue;

                        break;
                    default:
                        cell.Value = cellValue;
                        cell.Style.NumberFormat.NumberFormatId = 0;
                        break;
                }
            }

            return worksheet;
        }

    }
}
