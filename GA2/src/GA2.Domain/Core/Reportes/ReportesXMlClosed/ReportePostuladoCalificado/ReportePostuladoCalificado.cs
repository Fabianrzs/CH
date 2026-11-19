using ClosedXML.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.Convocatoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reportes.ReportesXMlClosed.ReportePostuladoCalificado
{
    public class ReportePostuladoCalificado
    {
        private IXLWorksheet worksheet;
        private List<PostulanteCalificadoFileDto> _postulantes;

        private readonly String[] columnas = new String[8] { "Radicado", "Tipo Modalidad", "Afiliado", "Unidad Ejecutora", "Categoria", "Puntaje", "Usuario", "Etapa Proceso"};

        public ReportePostuladoCalificado(IXLWorksheet _worksheet, List<PostulanteCalificadoFileDto> postulantes)
        {
            worksheet = _worksheet;
            _postulantes = postulantes;
        }
        public IXLWorksheet GetReportePostuladoCalificado()
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
            int columnaFinal = _postulantes.Count;
            var ragenTotal = worksheet.Range($"A{columnaInicial}:AH{columnaFinal}");
            ragenTotal.Cells().Style.Font.SetFontSize(11)
            .Font.SetFontName("Calibri");
            worksheet.Cell(columnaInicial, i + 9).Style.NumberFormat.Format = "$ #,##0.00";
            foreach (var item in _postulantes)
            {
                worksheet.Cell(columnaInicial, i + 1).Value = item.NoRadicado;
                worksheet.Cell(columnaInicial, i + 2).Value = item.ModalidadName;
                worksheet.Cell(columnaInicial, i + 3).Value = item.Afiliado;
                worksheet.Cell(columnaInicial, i + 4).Value = item.UnidadEjecutora;
                worksheet.Cell(columnaInicial, i + 5).Value = item.Categoria;
                worksheet.Cell(columnaInicial, i + 6).Value = item.puntaje;
                worksheet.Cell(columnaInicial, i + 7).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Cell(columnaInicial, i + 7).Value = item.Usuario;
                worksheet.Cell(columnaInicial, i + 8).Value = item.EtapaProceso;
                worksheet.Cell(columnaInicial, i + 8).Style.NumberFormat.Format = "#,##0.0";
                columnaInicial += 1;
            }
        }
    }

}
