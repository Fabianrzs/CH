using AutoMapper;
using GA2.Application.Main;
using GA2.Domain.Core.Reportes.ReportesXMlClosed;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Application.Dto.Reportes;
using Azure.Core;
using Org.BouncyCastle.Asn1.Ocsp;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;
using SelectPdf;
using System.Drawing.Printing;
using System.IO;
using ClosedXML.Excel;
using CsvHelper;
using System.Drawing;
using System.Globalization;

namespace GA2.Domain.Core.Reportes.ReportesExcelGlobal
{
    public class ReportesExcelLogic : IReportesExcelLogic
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IMapper _mapper;
        private readonly IReportesRepository _reportesRepository;


        public ReportesExcelLogic(IWebHostEnvironment webHostEnvironment,
                                     IOptions<BlobStorageOptions> options,
                                     IMapper mapper,
                                     IReportesRepository reportesRepository
                                     )
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportesRepository = reportesRepository ?? throw new ArgumentNullException(nameof(reportesRepository));;

        }
        public async Task<FileResult> GenerarExcel(ReportRequestExcelDto request)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hoja1");
            //var worksheet = workbook.Worksheet("Hoja1");

            // Agregar encabezado con imagen en la parte superior
            var header = $"{_webHostEnvironment.ContentRootPath}\\Img\\Header.png";
            var footer = $"{_webHostEnvironment.ContentRootPath}\\Img\\FOOTER.png";

            var headerPicture = worksheet.AddPicture(header); // Cambia a la ruta de tu imagen
            var headerImageCell = worksheet.Cell(1, 1); // Celda donde deseas que aparezca la imagen
            headerPicture.MoveTo(headerImageCell);
            workbook = EncabezadosExcel(workbook, request.ColumnName);
            workbook = DatosExcel(workbook, request.ColumnDataList);
            // Agregar pie de página con imagen
            var footerPicture = worksheet.AddPicture(footer); // Cambia a la ruta de tu imagen
            var footerImageCell = worksheet.Cell(worksheet.LastRowUsed().RowNumber() + 1, 1); // Celda después de la última fila usada
            footerPicture.MoveTo(footerImageCell);

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var excelBytes = stream.ToArray();
                var result = new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "archivo.xlsx"
                };

                return result;
            }
        }
        public async Task<FileResult> GenerarExcelRanking(ReportRequestExcelDto request)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hoja1");

            //Agregar encabezado con imagen en la parte superior
            var header = $"{_webHostEnvironment.ContentRootPath}\\Img\\Header.png";
            var footer = $"{_webHostEnvironment.ContentRootPath}\\Img\\FOOTER.png";
            var headerPicture = worksheet.AddPicture(header); // Cambia a la ruta de tu imagen
            var headerImageCell = worksheet.Cell(1, 1); // Celda donde deseas que aparezca la imagen
            headerPicture.MoveTo(headerImageCell);

            workbook = EncabezadosExcel(workbook, request.ColumnName);
            workbook = DatosExcel(workbook, request.ColumnDataList);
 

            // Agregar pie de página con imagen
            var footerPicture = worksheet.AddPicture(footer); // Cambia a la ruta de tu imagen
            var footerImageCell = worksheet.Cell(worksheet.LastRowUsed().RowNumber() + 1, 1); // Celda después de la última fila usada
            footerPicture.MoveTo(footerImageCell);

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var excelBytes = stream.ToArray();
                var result = new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "archivo.xlsx"
                };

                return result;
            }
        }


        public XLWorkbook EncabezadosExcel(XLWorkbook workbook, string[] encabezados)
        {
            var worksheet = workbook.Worksheet("Hoja1");
            var headerRow = worksheet.Row(12);

            for (int col = 0; col < encabezados.Length; col++)
            {
                var cell = headerRow.Cell(col + 1);
                cell.Value = encabezados[col];

                // Aplicar estilo al encabezado
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1c3f8b");
                cell.Style.Font.FontColor = XLColor.White;
            }

            return workbook;
        }
        public XLWorkbook DatosExcel(XLWorkbook workbook, List<ColumnData> datos)
        {
            var worksheet = workbook.Worksheet("Hoja1");
            for (int col = 0; col < datos.Count; col++)
            {
                var data = datos[col].Data;
                var dataRow = worksheet.Row(13 + col);
                for (int row = 0; row < data.Count; row++)
                {
                    var cell = dataRow.Cell(row + 1); // +1 para contar la columna de encabezados
                    var cellValue = data[row];

                    var valorLimpio = cellValue?.Replace("$", "").Replace(",", "").Trim();

                    if (!string.IsNullOrEmpty(valorLimpio) &&
                        decimal.TryParse(valorLimpio, NumberStyles.Number, CultureInfo.InvariantCulture, out var numericValue))
                    {
                        cell.Value = numericValue;
                    }
                    else
                    {
                        cell.Value = cellValue;
                    }
                }
            }
            return workbook;
        }


        public XLWorkbook EncabezadosExcelNomina(XLWorkbook workbook, string[] encabezados, int startRow)
        {
            var worksheet = workbook.Worksheet("Hoja1");
            var headerRow = worksheet.Row(startRow);

            for (int col = 0; col < encabezados.Length; col++)
            {
                var cell = headerRow.Cell(col + 1);
                cell.Value = encabezados[col];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1c3f8b");
                cell.Style.Font.FontColor = XLColor.White;
            }

            return workbook;
        }

        public XLWorkbook DatosExcelGenerico<T>(XLWorkbook workbook, List<T> datos, int startRow) where T : IColumnData
        {
            var worksheet = workbook.Worksheet("Hoja1");

            for (int i = 0; i < datos.Count; i++)
            {
                var data = datos[i].Data;
                var dataRow = worksheet.Row(startRow + i);

                for (int j = 0; j < data.Count; j++)
                {
                    dataRow.Cell(j + 1).Value = data[j];
                }
            }

            return workbook;
        }

        public async Task<FileResult> GenerarExcelSolicitudNomina(RequestReporteSolicitudNomina request)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hoja1");

            var headerImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Img", "Header.png");
            var footerImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Img", "FOOTER.png");

            // Imagen encabezado
            worksheet.AddPicture(headerImagePath).MoveTo(worksheet.Cell(1, 1));

            int currentRow = 14; // Empieza luego de la imagen de encabezado

            // Sección: Encabezado
            workbook = EncabezadosExcelNomina(workbook, request.ColumnNameEncabezado, currentRow);
            currentRow++;
            workbook = DatosExcelGenerico(workbook, request.ColumnDataEncabezado, currentRow);
            currentRow += request.ColumnDataEncabezado.Count + 2;

            // Sección: Cuerpo
            workbook = EncabezadosExcelNomina(workbook, request.ColumnNameCuerpo, currentRow);
            currentRow++;
            workbook = DatosExcelGenerico(workbook, request.ColumnDataCuerpo, currentRow);
            currentRow += request.ColumnDataCuerpo.Count + 2;

            // Sección: Fin
            workbook = EncabezadosExcelNomina(workbook, request.ColumnNameFin, currentRow);
            currentRow++;
            workbook = DatosExcelGenerico(workbook, request.ColumnDataFin, currentRow);
            currentRow += request.ColumnDataFin.Count + 1;

            // Imagen pie de página
            worksheet.AddPicture(footerImagePath).MoveTo(worksheet.Cell(currentRow, 1));

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "archivo.xlsx"
            };
        }

    }
}
