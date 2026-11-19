using ClosedXML.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteMoraCartera
{
    public class TemplateMoraCartera
    {
        private IXLWorksheet worksheet;
        private List<ReporteGeneralMoralDto> reporteGeneralMoraDtos;

        private String[] columnas = new String[16] { "Número de  Radicación", "Estado", "Código del  cliente", "Nombres y Apellidos", "Codigo  Empresa", "Empresa", "Saldo a capital", "Valor  Cuota", "Cód. Periodicidad", "Periodicidad", "Tipo Recaudo", "Cuota   Vencida", "Vencimiento  Proximo Pago", "Ultimo  Pago", "Vencimiento  Ultimo Pago", "Valor último  Pago" };

        public TemplateMoraCartera(IXLWorksheet _worksheet, List<ReporteGeneralMoralDto> _reporteGeneralMoraDtos)
        {
            worksheet = _worksheet;
            reporteGeneralMoraDtos = _reporteGeneralMoraDtos;

        }

        public IXLWorksheet GetSheetReporteMoraCartera()
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
            int columnaFinal = reporteGeneralMoraDtos.Count;
            var ragenTotal = worksheet.Range($"A{columnaInicial}:P{columnaFinal}");
            ragenTotal.Cells().Style.Font.SetFontSize(11)
            .Font.SetFontName("Calibri");

            foreach (var item in reporteGeneralMoraDtos)
            {
                worksheet.Cell(columnaInicial, i + 1).Value = item.NumeroRadicacion;
                worksheet.Cell(columnaInicial, i + 2).Value = item.Estado;
                worksheet.Cell(columnaInicial, i + 3).Value = item.CodigoCliente;
                worksheet.Cell(columnaInicial, i + 4).Value = item.NombreCliente;
                worksheet.Cell(columnaInicial, i + 5).Value = item.CodigoEmpresa;
                worksheet.Cell(columnaInicial, i + 6).Value = item.Empresa;
                worksheet.Cell(columnaInicial, i + 7).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 7).Value = item.SaldoCapital;
                worksheet.Cell(columnaInicial, i + 8).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 8).Value = item.ValorCuota;
                worksheet.Cell(columnaInicial, i + 9).Value = item.CodigoPeriocidad;
                worksheet.Cell(columnaInicial, i + 10).Value = item.Periocidad;
                worksheet.Cell(columnaInicial, i + 11).Value = item.TipoRecaudo;
                worksheet.Cell(columnaInicial, i + 12).Value = item.CuotaVencida;
                worksheet.Cell(columnaInicial, i + 13).Value = item.VencimientoProximoPago;               
                worksheet.Cell(columnaInicial, i + 14).Value = item.UltimoPago;
                worksheet.Cell(columnaInicial, i + 15).Value = item.VencimientoProximoPago;
                worksheet.Cell(columnaInicial, i + 16).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 16).Value = item.ValorUltimoPago;
                
                columnaInicial += 1;
            }


            worksheet.Cell("G1").Style.Font.SetBold(true);
            worksheet.Cell("G1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("G1").FormulaA1 = $"=SUM(G3:G{columnaInicial - 1})";
            worksheet.Cell("H1").Style.Font.SetBold(true);
            worksheet.Cell("H1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("H1").FormulaA1 = $"SUM(H3:H{columnaInicial-1})";
            worksheet.Cell("P1").Style.Font.SetBold(true);
            worksheet.Cell("P1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("P1").FormulaA1 = $"SUM(P3:P{columnaInicial-1})";
            

        }
    }
}
