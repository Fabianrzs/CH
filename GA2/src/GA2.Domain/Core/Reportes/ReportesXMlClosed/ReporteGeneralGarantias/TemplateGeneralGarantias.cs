using ClosedXML.Excel;
using GA2.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed.ReporteGeneralGarantias
{
    public class TemplateGeneralGarantias
    {
        private IXLWorksheet worksheet;
        private List<ReporteGeneralGarantiasDto> _reporteGarantias;

        private readonly String[] columnas = new String[11] { "Radicacion","Tipo_documento","Numero_documento","Nombres","Apellidos","Ciudad_inmueble","Fecha_apertura","Fecha_terminacion","Valor_inmueble","Ajuste_garantia","Valor_ajustado"  };

        public TemplateGeneralGarantias(IXLWorksheet _worksheet, List<ReporteGeneralGarantiasDto> reporteGarantias)
        {
            worksheet = _worksheet;
            _reporteGarantias= reporteGarantias;

        }

        public IXLWorksheet GetSheetReporteGeneralGarantias()
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
            int columnaFinal = _reporteGarantias.Count;
            var ragenTotal = worksheet.Range($"A{columnaInicial}:AH{columnaFinal}");
            ragenTotal.Cells().Style.Font.SetFontSize(11)
            .Font.SetFontName("Calibri");

            foreach (var item in _reporteGarantias)
            {
                worksheet.Cell(columnaInicial, i + 1).Value = item.NumeroRadicacion;
                worksheet.Cell(columnaInicial, i + 2).Value = item.TipoIdentificacion;
                worksheet.Cell(columnaInicial, i + 3).Value = item.Identificacion;
                worksheet.Cell(columnaInicial, i + 4).Value = item.Nombres;
                worksheet.Cell(columnaInicial, i + 5).Value = item.Apellidos;
                worksheet.Cell(columnaInicial, i + 6).Value = item.CiudadInmueble;
                worksheet.Cell(columnaInicial, i + 7).Value = item.FechaApertura;
                worksheet.Cell(columnaInicial, i + 8).Value = item.FechaTerminacion;
                worksheet.Cell(columnaInicial, i + 9).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 9).Value = item.ValorInmueble;
                worksheet.Cell(columnaInicial, i + 10).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 10).Value = item.AjusteGarantia;
                worksheet.Cell(columnaInicial, i + 11).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 11).Value = item.ValorAjustado;               
                            
                columnaInicial += 1;
            }

            worksheet.Cell("I1").Style.Font.SetBold(true);
            worksheet.Cell("I1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("I1").FormulaA1 = $"=SUM(I3:I{columnaInicial - 1})";
            worksheet.Cell("J1").Style.Font.SetBold(true);
            worksheet.Cell("J1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("J1").FormulaA1 = $"=SUM(J3:J{columnaInicial - 1})";
            worksheet.Cell("K1").Style.Font.SetBold(true);
            worksheet.Cell("K1").Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell("K1").FormulaA1 = $"=SUM(K3:K{columnaInicial - 1})";
            

        }
    }
}

