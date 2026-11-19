using ClosedXML.Excel;
using GA2.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed.ReportePreliminarCartera
{
    public class TemplatePreliminarCartera
    {
        private IXLWorksheet worksheet;
        private List<ReportePreliminarCarteraDto> _reportePreliminarCarteraDtos;

        private readonly String[] columnas = new String[14] { "No", "Radicacion", "Identificacion", "Apellidos y Nombres", "Cliente", "Nombre Pagaduria", "Cuota", "Ultimo Pago", "Tasa Linea", "Monto ", "Saldo ", "Valor Inmueble ", "Apertura", "Terminacion" };
        public TemplatePreliminarCartera(IXLWorksheet _worksheet, List<ReportePreliminarCarteraDto> reportePreliminarCarteraDtos)
        {
            worksheet = _worksheet;
            _reportePreliminarCarteraDtos = reportePreliminarCarteraDtos;

        }

        public IXLWorksheet GetSheetReporteGeneralCartera()
        {
            int posicion = 1;
            foreach (var item in columnas)
            {
                worksheet.Cell(2, posicion)
                .Style.Font.SetFontSize(11)
                .Font.SetFontName("Calibri")
                .Font.SetBold(true)
                .Fill.SetBackgroundColor(XLColor.BlueGray);
                worksheet.Cell(2, posicion).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, posicion).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(2, posicion).Value = item;
                worksheet.Column(posicion).Width = 26.5;

                posicion++;
            }

            InsertarInformacion();
            return worksheet;
        }


        private void InsertarInformacion()
        {
            int i = 0;
            int columnaInicial = 3;
            int columnaFinal = _reportePreliminarCarteraDtos.Count;
            var ragenTotal = worksheet.Range($"A{columnaInicial}:AH{columnaFinal}");
            ragenTotal.Cells().Style.Font.SetFontSize(11)
            .Font.SetFontName("Calibri");
            worksheet.Cell(columnaInicial, i + 9).Style.NumberFormat.Format = "$ #,##0.00";
            foreach (var item in _reportePreliminarCarteraDtos)
            {
                worksheet.Cell(columnaInicial, i + 1).Value = item.Nro;
                worksheet.Cell(columnaInicial, i + 2).Value = item.NumeroRadicacion;
                worksheet.Cell(columnaInicial, i + 3).Value = item.Identificacion;
                worksheet.Cell(columnaInicial, i + 4).Value = item.Nombres;
                worksheet.Cell(columnaInicial, i + 5).Value = item.Cliente;               
                worksheet.Cell(columnaInicial, i + 6).Value = item.NombrePagaduria;
                worksheet.Cell(columnaInicial, i + 7).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 7).Value = item.Cuota;
                worksheet.Cell(columnaInicial, i + 8).Value = item.UltimoPago;
                worksheet.Cell(columnaInicial, i + 9).Style.NumberFormat.Format = "#,##0.0";
                worksheet.Cell(columnaInicial, i + 9).Value = item.TasaLinea;
                worksheet.Cell(columnaInicial, i + 10).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 10).Value = item.Monto;
                worksheet.Cell(columnaInicial, i + 11).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 11).Value = item.Saldo;
                worksheet.Cell(columnaInicial, i + 12).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 12).Value = item.ValorInmueble;
                worksheet.Cell(columnaInicial, i + 13).Value = item.Apertura;
                worksheet.Cell(columnaInicial, i + 14).Value = item.Terminacion;               
                columnaInicial += 1;
            }

            worksheet.Cell("J1").Style.Font.SetBold(true);
            worksheet.Cell("J1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("J1").FormulaA1 = $"=SUM(J3:J{columnaInicial-1})";
            worksheet.Cell("K1").Style.Font.SetBold(true);
            worksheet.Cell("K1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("K1").FormulaA1 = $"=SUM(K3:K{columnaInicial-1})";
            worksheet.Cell("L1").Style.Font.SetBold(true);
            worksheet.Cell("L1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("L1").FormulaA1 = $"=SUM(L3:L{columnaInicial-1})";
            

        }
    }
}
