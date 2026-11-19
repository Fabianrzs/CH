using ClosedXML.Excel;
using GA2.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteGeneralCartera
{
    public class TemplateGeneralCartera
    {
        private IXLWorksheet worksheet;
        private List<ReporteGeneralCarteraDto> reporteGeneralCarteraDtos;

        private String[] columnas = new String[32] { "Radicación", "Pagaré", "Cliente", "Estado", "Identificación", "Apellidos y Nombres", "Pagaduria", "Nombre Pagaduria", "Apertura", "Terminacion", "Tasa de Linea", "Destinación", "Desembolso", "Monto", "Saldo", "Cuota", "Dias", "Ultimo Pago", "Próximo Pago", "Mora Anterior", "Interes", "Seguros", "Capital", "Total a pagar", "Valor del Inmueble", "Plazo de la Obligación", "Calificación", "Numero de cuotas faltantes", "Valor cuota Extraordinaria", " Valor Opción de compra", "Ciudad del Inmueble", "Puntaje Scoring" };
       
        public TemplateGeneralCartera(IXLWorksheet _worksheet, List<ReporteGeneralCarteraDto> _reporteGeneralCarteraDtos) 
        { 
            worksheet = _worksheet;
            reporteGeneralCarteraDtos = _reporteGeneralCarteraDtos;
           
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
             
                worksheet.Cell(2, posicion).Value= item;
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
            int columnaFinal = reporteGeneralCarteraDtos.Count;
            var ragenTotal = worksheet.Range($"A{columnaInicial}:AH{columnaFinal}");
            ragenTotal.Cells().Style.Font.SetFontSize(11)
            .Font.SetFontName("Calibri");
            
            foreach (var item in reporteGeneralCarteraDtos)
            {
                worksheet.Cell(columnaInicial, i + 1).Value = item.NumeroRadicacion;
                worksheet.Cell(columnaInicial, i + 2).Value = item.Pagare;
                worksheet.Cell(columnaInicial, i + 3).Value = item.Cliente;
                worksheet.Cell(columnaInicial, i + 4).Value = item.Estado;
                worksheet.Cell(columnaInicial, i + 5).Value = item.Identificacion;
                worksheet.Cell(columnaInicial, i + 6).Value = item.Nombres;
                worksheet.Cell(columnaInicial, i + 7).Value = item.Pagaduria;
                worksheet.Cell(columnaInicial, i + 8).Value = item.NombrePagaduria;
                worksheet.Cell(columnaInicial, i + 9).Value = item.Apertura;
                worksheet.Cell(columnaInicial, i + 10).Value = item.Terminacion;
                worksheet.Cell(columnaInicial, i + 11).Value = item.TasaLinea;
                worksheet.Cell(columnaInicial, i + 12).Value = item.Destinacion;
                worksheet.Cell(columnaInicial, i + 13).Value = item.Desembolso;
                worksheet.Cell(columnaInicial, i + 14).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 14).Value = item.Monto;
                worksheet.Cell(columnaInicial, i + 15).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 15).Value = item.Saldo;
                worksheet.Cell(columnaInicial, i + 16).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 16).Value = item.Cuota;
                worksheet.Cell(columnaInicial, i + 17).Value = item.Dias;
                worksheet.Cell(columnaInicial, i + 18).Value = item.UltimoPago;
                worksheet.Cell(columnaInicial, i + 19).Value = item.PróximoPago;
                worksheet.Cell(columnaInicial, i + 20).Value = item.MoraInterior;
                worksheet.Cell(columnaInicial, i + 21).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 21).Value = item.Interes;
                worksheet.Cell(columnaInicial, i + 22).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 22).Value = Convert.ToDouble(item.Seguros);
                worksheet.Cell(columnaInicial, i + 23).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 23).Value = item.Capital;
                worksheet.Cell(columnaInicial, i + 24).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 24).Value = item.TotalPagar;
                worksheet.Cell(columnaInicial, i + 25).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 25).Value = item.ValorInmueble;
                worksheet.Cell(columnaInicial, i + 26).Value = item.PlazoObligacion;
                worksheet.Cell(columnaInicial, i + 27).Value = item.Calificacion;
                worksheet.Cell(columnaInicial, i + 28).Value = item.NumeroCuotasFaltantes;
                worksheet.Cell(columnaInicial, i + 29).Value = item.ValorCuotaExtraordinaria;
                worksheet.Cell(columnaInicial, i + 30).Value = item.ValorOpcionCompra;
                worksheet.Cell(columnaInicial, i + 31).Value = item.CiudadInmueble;
                worksheet.Cell(columnaInicial, i + 32).Value = item.PuntajeScoring;
                columnaInicial += 1;
            }

            worksheet.Cell("N1").Style.Font.SetBold(true);
            worksheet.Cell("N1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("N1").FormulaA1 = $"=SUM(N3:N{columnaInicial-1})";
            worksheet.Cell("O1").Style.Font.SetBold(true);
            worksheet.Cell("O1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("O1").FormulaA1 = $"=SUM(O3:O{columnaInicial-1})";
            worksheet.Cell("P1").Style.Font.SetBold(true);
            worksheet.Cell("P1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("P1").FormulaA1 = $"=SUM(P3:P{columnaInicial-1})";
            worksheet.Cell("U1").Style.Font.SetBold(true);
            worksheet.Cell("U1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("U1").FormulaA1 = $"=SUM(U3:U{columnaInicial-1})";
            worksheet.Cell("V1").Style.Font.SetBold(true);
            worksheet.Cell("V1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("V1").FormulaA1 = $"=SUM(V3:V{columnaInicial})";
            worksheet.Cell("W1").Style.Font.SetBold(true);
            worksheet.Cell("W1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("W1").FormulaA1 = $"=SUM(W3:W{columnaInicial-1})";
            worksheet.Cell("X1").Style.Font.SetBold(true);
            worksheet.Cell("X1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("X1").FormulaA1 = $"=SUM(X3:X{columnaInicial-1})";
            worksheet.Cell("Y1").Style.Font.SetBold(true);
            worksheet.Cell("Y1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("Y1").FormulaA1 = $"=SUM(Y3:Y{columnaInicial-1})";

        }
    }
}
