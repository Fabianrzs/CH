using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed
{
    public class TemplateExcelReporteRCredito : ITemplateExcelReporteRCredito
    {
        public byte[] GenerarReporteRCredito(DatosReporteRCredito datosReporte)
        {
            var workbook = new XLWorkbook();
            workbook.Style.Font.FontName = "Arial Narrow";
            workbook.Style.Font.FontSize = 10;

            var celdaSaldoMesActual = InsertarHojaDatosGenerales(workbook, datosReporte);
            InsertarHojaProvisiones(workbook, datosReporte, celdaSaldoMesActual);
            
            if(datosReporte.InformacionLibranza != null)
            {
                InsertarHojaLibranza(workbook, datosReporte);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        IXLCell InsertarHojaDatosGenerales(IXLWorkbook workbook, DatosReporteRCredito datosReporte)
        {
            var parametros = datosReporte.Parametros;
            var datosGenerales = datosReporte.DatosGenerales;
            var modalidadesPrincipales = datosGenerales.Where(x => x.IdModalidadPrincipal == null);

            var worksheet = workbook.Worksheets.Add("Datos Generales");
            worksheet.ShowGridLines = false;

            var periodo = new DateTime(parametros.Anio, parametros.Mes, DateTime.DaysInMonth(parametros.Anio, parametros.Mes));

            // A
            worksheet.Column(1).Width = 2.5;
            worksheet.Column(2).Width = 42;

            // B
            var cell = worksheet.Cell("B1");
            cell.Value = "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICIA";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B2");
            cell.Value = "860021967-7";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B3");
            cell.Value = "DATOS GENERALES CARTERA DE CRÉDITOS";
            cell.Style.Font.FontSize = 12;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B4");
            cell.Value = periodo;
            cell.Style.DateFormat.Format = "dd/MM/yyyy";
            cell.Style.Font.FontSize = 12;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var comment = cell.CreateComment();
            comment.AddText("Fecha en formato DD/MM/YYYY");
            comment.Bold = true;
            comment.Visible = true;
            comment.Style.Size.Height = 25;


            var cabeceras = new CabeceraReporte[]
            {
                "Modalidad",
                new ("Saldo a Capital mes anterior", cellWidth: 12),
                new ("Desembolso", cellWidth: 12, "Saldo", "No. operaciones"),
                new ("Recaudos", cellWidth: 12,"Saldo", "No. operaciones"),
                new ("Cancelaciones", cellWidth: 12,"Saldo", "No. operaciones"),
                new ("Castigos", cellWidth : 12, "Saldo", "No. operaciones"),
                new ("Variación por procesos de Reestructuraciones o Modificaciones", cellWidth: 16, "Saldo (+/-)", "No. operaciones"),
                new ("Saldo a Capital mes Actual", cellWidth: 12),
            };

            AgregarCabecera(worksheet, cabeceras, 7, 2);

            var modalidades = AgruparModalidades(datosGenerales);

            var (row, rowRange) = InsertarModalidades(worksheet, modalidades, 9, 2);
            return InsertarDatosModalidadTotal(worksheet, "Total Cartera", rowRange, row, 2, 14);
        }

        IEnumerable<ReporteRCreditoDatosGeneralesDto> AgruparModalidades(IEnumerable<ReporteRCreditoDatosGeneralesDto> data)
        {
            var modalidades = data.Where(x => x.IdModalidadPrincipal == null).ToList();

            foreach (var item in modalidades)
            {
                item.SubModalidades = ObtenerSubmodalidades(data, item);
                yield return item;
            }
        }

        IEnumerable<ReporteRCreditoDatosGeneralesDto> ObtenerSubmodalidades(IEnumerable<ReporteRCreditoDatosGeneralesDto> data, ReporteRCreditoDatosGeneralesDto modalidad)
        {
            var subModalidades = data.Where(x => x.IdModalidadPrincipal == modalidad.IdModadlidad).ToList();

            foreach (var item in subModalidades)
            {
                item.SubModalidades = ObtenerSubmodalidades(data, item);
                yield return item;
            }
        }


        (int, IEnumerable<int>) InsertarModalidades(IXLWorksheet sheet, IEnumerable<ReporteRCreditoDatosGeneralesDto> modalidades, int startRow, int startColumn, int nivel = 0)
        {
            var rowSubs = new List<int>();

            foreach (var modalidad in modalidades)
            {
                rowSubs.Add(startRow);

                if (modalidad.SubModalidades.Any())
                {
                    IEnumerable<int> rangeRows;
                    (startRow, rangeRows) = InsertarModalidades(sheet, modalidad.SubModalidades, startRow + 1, startColumn, nivel + 1);
                    InsertarDatosModalidadTotal(sheet, modalidad.Modalidad, rangeRows, rowSubs.Last(), startColumn, startColumn + 12, nivel);
                }
                else
                {
                    InsertarDatosModalidad(sheet, modalidad, startRow, startColumn, nivel);
                    startRow++;
                }
            }

            return (startRow, rowSubs);
        }

        IXLCell InsertarDatosModalidadTotal(IXLWorksheet sheet, string modalidad, IEnumerable<int> sumRows, int row, int startColumn, int endColumn, int nivel = 0)
        {
            var colsSaldo = new (int column, char operation)[] {
                (startColumn + 1, '+'),
                (startColumn + 2, '+'),
                (startColumn + 4, '-'),
                (startColumn + 6, '-'),
                (startColumn + 8, '-'),
                (startColumn + 10, '+')
            };

            for (int i = startColumn; i <= endColumn; i++)
            {
                var cell = sheet.Cell(row, i);

                if (i == startColumn)
                {
                    cell.Value = modalidad;
                }
                else if (i == endColumn)
                {
                    var formula = string.Join("",
                        colsSaldo.Select(x => $"{x.operation}{sheet.Cell(row, x.column).Address.ToString(XLReferenceStyle.A1)}")
                    );
                    cell.FormulaA1 = formula;
                    cell.Style.NumberFormat.NumberFormatId = 2;
                }
                else
                {
                    if (sumRows.Count() > 1 && sumRows.Zip(sumRows.Skip(1), (a, b) => b == a + 1).All(x => x))
                    {
                        var firstCell = sheet.Cell(sumRows.First(), i).Address.ToString(XLReferenceStyle.A1);
                        var lastCell = sheet.Cell(sumRows.Last(), i).Address.ToString(XLReferenceStyle.A1);
                        cell.FormulaA1 = $"=SUM({firstCell}:{lastCell})";
                    }
                    else
                    {
                        var rangeCells = string.Join('+', sumRows.Select(r => sheet.Cell(r, i).Address.ToString(XLReferenceStyle.A1)));
                        cell.FormulaA1 = $"={rangeCells}";
                    }

                    cell.Style.NumberFormat.NumberFormatId = sheet.Cell(sumRows.First(), i).Style.NumberFormat.NumberFormatId;
                }

                if (nivel == 0)
                {
                    cell.Style.Fill.BackgroundColor = modalidad.Contains("Total")
                        ? XLColor.FromArgb(217, 217, 217)
                        : XLColor.FromArgb(242, 242, 242);
                    cell.Style.Font.Bold = true;
                }

                if (i == 0 && nivel > 1)
                {
                    cell.Style.Alignment.Indent = nivel - 1;
                }
                EstablecerBorde(cell);
            }

            return sheet.Cell(row, endColumn);
        }

        int InsertarDatosModalidad(IXLWorksheet sheet, ReporteRCreditoDatosGeneralesDto modalidad, int startRow, int startColumn, int nivel = 0)
        {
            var rangeCells = new List<(char operation, IXLCell cell)>();

            var datos = new ValoresCelda[] {
                new(modalidad.Modalidad),
                new(modalidad.SaldoCapitalMesAnterior, 2, '+'),
                new(modalidad.SaldoDesembolsos, 2, '+'),
                new(modalidad.CantidadDesembolsos),
                new(modalidad.SaldoRecaudos, 2, '-'),
                new(modalidad.CantidadRecaudos),
                new(modalidad.SaldoCancelaciones, 2, '-'),
                new(modalidad.CantidadCancelaciones),
                new(modalidad.SaldoCastigos, 2, '-'),
                new(modalidad.CantidadCastigos, 0),
                new(modalidad.SaldoRestructuraciones, 2, '+'),
                new(modalidad.CantidadRestructuraciones),
                new(null, 2),
            };

            for (int i = 0; i < datos.Length; i++)
            {
                var (value, formatId, operation) = datos[i];
                var cell = sheet.Cell(startRow, startColumn + i);
                cell.Style.NumberFormat.NumberFormatId = formatId;

                if (nivel == 0)
                {
                    cell.Style.Fill.BackgroundColor = modalidad.Modalidad.Contains("Total")
                        ? XLColor.FromArgb(217, 217, 217)
                        : XLColor.FromArgb(242, 242, 242);
                    cell.Style.Font.Bold = true;
                }

                if (i == 0 && nivel > 1)
                {
                    cell.Style.Alignment.Indent = nivel - 1;
                }
                EstablecerBorde(cell);

                if (i == datos.Length - 1)
                {
                    var formula = string.Join("",
                        rangeCells.Select(x => $"{x.operation}{x.cell.Address.ToString(XLReferenceStyle.A1)}")
                    );
                    cell.FormulaA1 = formula;
                }
                else
                {
                    cell.Value = value is string or null ? value?.ToString() ?? "" : Convert.ToDecimal(value);

                    if (operation != null)
                    {
                        rangeCells.Add((operation.Value, cell));
                    }
                }
            }

            return startRow;
        }

        void InsertarHojaProvisiones(IXLWorkbook workbook, DatosReporteRCredito datosReporte, IXLCell celdaSaldoMesActual)
        {
            var parametros = datosReporte.Parametros;
            var datosGenerales = datosReporte.DatosGenerales;

            var worksheet = workbook.Worksheets.Add("Provisiones");
            worksheet.ShowGridLines = false;

            var periodo = new DateTime(parametros.Anio, parametros.Mes, DateTime.DaysInMonth(parametros.Anio, parametros.Mes));

            // A
            worksheet.Column(1).Width = 2.5;
            worksheet.Column(2).Width = 42;

            // B
            var cell = worksheet.Cell("B1");
            cell.Value = "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICIA";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B2");
            cell.Value = "860021967-7";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B3");
            cell.Value = "DATOS GENERALES CARTERA DE CRÉDITOS";
            cell.Style.Font.FontSize = 12;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B4");
            cell.Value = periodo;
            cell.Style.DateFormat.Format = "dd/MM/yyyy";
            cell.Style.Font.FontSize = 12;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var cabeceras = new CabeceraReporte[]
            {
                "Modalidad",
                new ("Provision", cellWidth: 12, "Normativa", "Adicional"),
            };

            AgregarCabecera(worksheet, cabeceras, 7, 2, 18, 18);

            var modalidades = new ReporteRCreditoDatosProvision[]
            {
                "Comercial",
                "Consumo",
                new("Vivienda") { Normativa = $"={celdaSaldoMesActual.Address.ToString(XLReferenceStyle.A1, true)} * 1%", TipoValor = TipoValorCelda.FormulaA1 },
                "Microcrédito",
                new("Total Cartera") { Normativa = $"=SUM(R[-4]C:R[-1]C)", Adicional = $"=SUM(R[-4]C:R[-1]C)", TipoValor = TipoValorCelda.FormulaRC },
                "CE 026 de 2012 (consumo)",
                "CE Estimación Perdida Esperada MRCO (c.e.47/2016)"
            };

            InsertarDatosProvision(worksheet, modalidades, 9, 2);
        }

        int InsertarDatosProvision(IXLWorksheet sheet, IEnumerable<ReporteRCreditoDatosProvision> datos, int startRow, int startColumn)
        {
            var row = startRow;

            foreach (var item in datos)
            {
                var bold = item.Nombre.Contains("Total");

                var cell = sheet.Cell(row, startColumn);
                EstablecerValorCelda(cell, item.Nombre, TipoValorCelda.Literal);
                EstablecerEstiloCelda(cell, bold, 0);
                EstablecerBorde(cell);

                cell = sheet.Cell(row, startColumn + 1);
                EstablecerValorCelda(cell, item.Normativa, item.TipoValor);
                EstablecerEstiloCelda(cell, bold);
                EstablecerBorde(cell);

                cell = sheet.Cell(row, startColumn + 2);
                EstablecerValorCelda(cell, item.Adicional, item.TipoValor);
                EstablecerEstiloCelda(cell, bold);
                EstablecerBorde(cell);

                row++;
            }

            return row;
        }

        void EstablecerEstiloCelda(IXLCell cell, bool bold = true, int formatId = 2)
        {
            cell.Style.NumberFormat.NumberFormatId = formatId;
            cell.Style.Font.Bold = bold;
        }

        void EstablecerValorCelda(IXLCell cell, object value, TipoValorCelda tipoFormula)
        {
            if (value is decimal)
            {
                cell.Value = Convert.ToDecimal(value);
            }
            else if (value is string)
            {
                switch (tipoFormula)
                {
                    case TipoValorCelda.Literal:
                        cell.Value = value.ToString();
                        break;
                    case TipoValorCelda.FormulaA1:
                        cell.FormulaA1 = value.ToString();
                        break;
                    case TipoValorCelda.FormulaRC:
                        cell.FormulaR1C1 = value.ToString();
                        break;
                }
            }
        }

        void EstablecerValor(IXLCell cell, object value, TipoValorCelda tipoValor)
        {
            if (value is decimal)
            {
                cell.Value = Convert.ToDecimal(value);
            }
            else if (value is string)
            {
                switch (tipoValor)
                {
                    case TipoValorCelda.Literal:
                        cell.Value = value.ToString();
                        break;
                    case TipoValorCelda.FormulaA1:
                        cell.FormulaA1 = value.ToString();
                        break;
                    case TipoValorCelda.FormulaRC:
                        cell.FormulaR1C1 = value.ToString();
                        break;
                }
            }
        }

        void InsertarHojaLibranza(IXLWorkbook workbook, DatosReporteRCredito datosReporte)
        {
            var parametros = datosReporte.Parametros;
            var informacionLibranza = datosReporte.InformacionLibranza;

            var worksheet = workbook.Worksheets.Add("Libranza por empleador Vivienda");
            worksheet.ShowGridLines = false;

            var periodo = new DateTime(parametros.Anio, parametros.Mes, DateTime.DaysInMonth(parametros.Anio, parametros.Mes));

            // A
            worksheet.Column(1).Width = 2.5;

            // B
            var cell = worksheet.Cell("B1");
            cell.Value = "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICIA";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B2");
            cell.Value = "860021967-7";
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Red;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B3");
            cell.Value = "INFORMACIÓN CARTERA DE LIBRANZA POR EMPLEADOR";
            cell.Style.Font.FontSize = 12;
            cell.Style.Font.Bold = true;

            cell = worksheet.Cell("B4");
            cell.Value = periodo;
            cell.Style.DateFormat.Format = "dd/MM/yyyy";
            cell.Style.Font.FontSize = 12;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Font.Bold = true;

            var cabeceras = new CabeceraReporte[]
            {
                new("Fecha de la última evaluación", cellWidth: 22),
                new("Nit de la empresa", cellWidth: 20),
                new("Nombre de la empresa", cellWidth: 52),
                new ("Tipo de empresa", cellWidth: 60, new("Carácter*", cellWidth: 20), new("Sector*", cellWidth: 42)),
                new("Estado de la pagaduría*", cellWidth: 30),
                new ("Originación directa", cellWidth: 26,
                    "Plazo máximo", "Número de obligaciones", "Número de deudores", "Saldo de capital",
                    "Número de obligaciones en mora",
                    "Número de deudores en mora",
                    "Saldo de Capital en mora mayor a 30 días"
                ),
                new("Compra de cartera", cellWidth: 20,
                    "Plazo máximo",
                    "Número de obligaciones",
                    "Número de deudores",
                    "Saldo de capital",
                    "Número de obligaciones en mora",
                    "Número de deudores en mora",
                    "Saldo de Capital en mora mayor a 30 días"
                ),
                new("Exlibranzas", cellWidth: 20,
                    "Número de obligaciones",
                    "Número de deudores",
                    "Saldo de capital ",
                    "Número de obligaciones en mora",
                    "Número de deudores en mora",
                    "Saldo de Capital en mora mayor a 30 días"
                ),
                new("Castigos*", cellWidth: 20,
                    "Número de las obligaciones castigadas",
                    "Número de los deudores castigados",
                    "Saldo de capital "
                ),
            };

            int totalColumnas = cabeceras.Sum(x => Math.Max(x.Subtitulos.Count(), 1));

            AgregarCabecera(worksheet, cabeceras, 7, 2, 20, 40);
            InsertarDatosLibranza(worksheet, informacionLibranza, 9, 2, totalColumnas);
        }

        int InsertarDatosLibranza(IXLWorksheet sheet, IEnumerable<ReporteRCreditoInformacionLibranzaDto> datos, int startRow, int startColumn, int lengthColumns)
        {
            int row = startRow;

            foreach (var item in datos)
            {
                int column = startColumn;
                var cell = sheet.Cell(row, startColumn);

                cell.Value = item.FechaEvaluacion;
                cell.Style.DateFormat.NumberFormatId = 14;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                cell = sheet.Cell(row, ++column);
                cell.Value = long.TryParse(item.NitEmpresa, out var nitEmpresa) ? nitEmpresa : item.NitEmpresa;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.NombreEmpresa;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.CaracterEmpresa;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.SectorEmpresa;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.EstadoPagaduria;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.PlazoMaximo;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.NumeroObligaciones;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.NumeroDeudores;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                cell = sheet.Cell(row, ++column);
                cell.Value = item.SaldoCapital;
                cell.Style.NumberFormat.Format = "_-* #,##0.000000_-;-* #,##0.000000_-;_-* \"-\"_-;_-@_-";
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                column += 3;

                cell = sheet.Cell(row, column);
                cell.Value = item.SaldoCapitalMora;
                cell.Style.NumberFormat.Format = "_-* #,##0.000000_-;-* #,##0.000000_-;_-* \"-\"_-;_-@_-";
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                row++;
            }

            var range = sheet.Range(startRow, startColumn, row - 1, startColumn + lengthColumns - 1);
            SetStyleBorder(range);


            return row;
        }



        void AgregarCabecera(IXLWorksheet sheet, IEnumerable<CabeceraReporte> headers, int startRow, int startColumn, int heightRow = 30, int heightSubRow = 30)
        {
            var backgroundColorCell = XLColor.FromArgb(48, 84, 150);
            var foregorundColorCell = XLColor.White;
            var anySubtitle = headers.Any(x => x.Subtitulos.Any());

            sheet.Row(startRow).Height = heightRow;

            if (anySubtitle)
            {
                sheet.Row(startRow + 1).Height = heightSubRow;
            }

            foreach (var header in headers)
            {
                var cell = sheet.Cell(startRow, startColumn);
                SetPropetiesCell(cell, header.Nombre);

                if (header.Subtitulos.Any())
                {
                    var range = sheet.Range(sheet.Cell(startRow, startColumn), sheet.Cell(startRow, startColumn + header.Subtitulos.Count() - 1));
                    range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.RightBorderColor = XLColor.White;
                    range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.BottomBorderColor = XLColor.White;
                    range.Merge();

                    var incrementColumn = startColumn;
                    foreach (var subtitle in header.Subtitulos)
                    {
                        var subCell = sheet.Cell(startRow + 1, incrementColumn);
                        SetPropetiesCell(subCell, subtitle.Nombre);
                        var column = subCell.WorksheetColumn();
                        column.Width = subtitle.AnchoCelda ?? header.AnchoCelda ?? column.Width;
                        incrementColumn++;
                    }

                    startColumn = incrementColumn;
                }
                else
                {
                    var range = sheet.Range(sheet.Cell(startRow, startColumn), sheet.Cell(startRow + 1, startColumn));
                    range.Merge();
                    range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.RightBorderColor = XLColor.White;
                    var column = cell.WorksheetColumn();
                    column.Width = header.AnchoCelda ?? column.Width;
                    cell.Style.Alignment.WrapText = true;
                    startColumn++;
                }
            }

            void SetPropetiesCell(IXLCell cell, string value)
            {
                cell.Value = value;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = backgroundColorCell;
                cell.Style.Font.FontColor = foregorundColorCell;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.RightBorderColor = XLColor.White;
                cell.Style.Alignment.WrapText = true;
            }
        }

        void EstablecerBorde(IXLCell cell)
        {
            var borderColor = XLColor.FromArgb(217, 217, 217);
            var border = cell.Style.Border;
            border.TopBorder = XLBorderStyleValues.Thin;
            border.RightBorder = XLBorderStyleValues.Thin;
            border.BottomBorder = XLBorderStyleValues.Thin;
            border.LeftBorder = XLBorderStyleValues.Thin;
            border.TopBorderColor = borderColor;
            border.RightBorderColor = borderColor;
            border.BottomBorderColor = borderColor;
            border.LeftBorderColor = borderColor;
        }

        void SetStyleBorder(IXLRange cell)
        {
            var borderColor = XLColor.FromArgb(217, 217, 217);
            var border = cell.Style.Border;
            border.TopBorder = XLBorderStyleValues.Thin;
            border.RightBorder = XLBorderStyleValues.Thin;
            border.BottomBorder = XLBorderStyleValues.Thin;
            border.LeftBorder = XLBorderStyleValues.Thin;
            border.TopBorderColor = borderColor;
            border.RightBorderColor = borderColor;
            border.BottomBorderColor = borderColor;
            border.LeftBorderColor = borderColor;
        }
    }
}
