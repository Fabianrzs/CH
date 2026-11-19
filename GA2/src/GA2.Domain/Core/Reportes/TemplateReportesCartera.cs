using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GA2.Domain.Core
{
    public class TemplateReportesCartera
    {
        /// <summary>
        /// Plan de Pagos Crédito Hipotecario / Leasing.
        /// </summary>
        /// <param name="modelo">Datos del crédito, incluida la lista modelo.PlanPago con la amortización.</param>
        /// <param name="imagenes">imagenes[0] = encabezado (logos), imagenes[1] = pie de página.</param>
        /// <returns>MemoryStream con el PDF (igual que los demás métodos de esta clase).</returns>
        public MemoryStream ReportePlanPagosHipotecario(RequestProductoClienteDto modelo, string[] imagenes)
        {
            var stream = new MemoryStream();
            var document = new PdfDocument();

            var esLeasing = modelo.IdTipoCredito == 1;

            document.Info.Title = esLeasing ? "Plan de Pagos Crédito Leasing" : "Plan de Pagos Crédito Hipotecario";

            var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            var fontTitulo = new XFont("Arial", 13, XFontStyle.Bold, options);
            var font = new XFont("Arial", 9, XFontStyle.Regular, options);
            var fontNegrita = new XFont("Arial", 9, XFontStyle.Bold, options);
            var fontSeccion = new XFont("Arial", 9, XFontStyle.Bold, options);
            var fontHeaderTabla = new XFont("Arial", 8, XFontStyle.Bold, options);
            var fontCelda = new XFont("Arial", 8, XFontStyle.Regular, options);

            var culturaCo = CultureInfo.GetCultureInfo("es-CO");
            var ahora = DateTime.Now;

            const double margen = 40;
            const double altoZonaEncabezado = 75;   // espacio reservado arriba para la imagen de encabezado
            const double altoZonaPie = 70;           // espacio reservado abajo para la imagen de pie de página
            const double altoFilaTabla = 16;

            // Leasing tiene 11 columnas: en vertical (532pt útiles) no caben legibles -> se usa horizontal (712pt útiles).
            // Los anchos de cada set suman exactamente el ancho útil de su propia orientación.
            double[] anchosColumnas = esLeasing
                   ? new double[] { 65, 75, 78, 78, 100, 78, 100, 72, 78, 84, 100 } // suma 928 (Legal horizontal - 40*2)
                : new double[] { 55, 65, 68, 68, 62, 62, 68, 84 };            // suma 532 (Letter vertical - 40*2)

            string[] encabezadosColumnas = esLeasing
                ? new[] { "N° Canon", "Fecha", "Saldo Inicial", "Capital", "Capital Por Pagar", "Intereses", "Interes Por Pagar", "Seguro Vida", "Seguro Hogar", "A pagar" ,"Deuda Remanente" }
                : new[] { "N° Cuota", "Fecha", "Capital", "Intereses", "Seguro Vida", "Seguro Hogar", "Cuota", "Saldo" };

            PdfPage page = document.AddPage();
            page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
            page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            double y = altoZonaEncabezado;

            var titulo = esLeasing ? "PLAN DE PAGOS CRÉDITO LEASING" : "PLAN DE PAGOS CRÉDITO HIPOTECARIO";

            // ---------- Título ----------
            gfx.DrawString(titulo, fontTitulo, XBrushes.Black,
                new XRect(margen, y, page.Width - margen * 2, 20), XStringFormats.TopCenter);
            y += 28;

            // ---------- Información del Cliente ----------
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, margen, y, "Información del Cliente");
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Identificación Cliente:", $"{modelo.Identificacion}",
                "Fecha:", ahora.ToString("MM/dd/yyyy", culturaCo));
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Nombre Cliente:", modelo.NombreCliente,
                "Hora:", ahora.ToString("hh:mm:ss tt", culturaCo));

            // ---------- Información del Crédito ----------
            y += 6;
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, margen, y, "Información del Crédito");
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Número de Contrato:", modelo.NumeroCredito.ToString(culturaCo),
                "Fecha solicitud:", FormatearFechaPlanPagos(modelo.FechaSolicitud, culturaCo, esLeasing));
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Línea de Crédito:", modelo.LineaCredito,
                "Fecha Final:", FormatearFechaPlanPagos(modelo.FechaFinPagos, culturaCo, esLeasing));
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Plazo:", $"{modelo.PlazoTotal} MESES",
                "Periodicidad:", "Mensual");
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Valor Vivienda:", "$ " + FormatearMonedaPlanPagos(modelo.ValorVivienda),
                "Cálculo de tasa:", "Fijo");
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Cuota inicial:", "$ " + FormatearMonedaPlanPagos(modelo.ValorVivienda - modelo.ValorDesembolso),
                "Modalidad de cuota:", "Mes Vencido");
            y = DibujarParDatos(gfx, font, fontNegrita, margen, y,
                "Valor Préstamo:", "$ " + FormatearMonedaPlanPagos(modelo.ValorDesembolso),
                string.Empty, string.Empty);

            // ---------- Información Liquidación ----------
            y += 6;
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, margen, y, "Información Liquidación");
            var numeroCuotas = modelo.PlanPago?.Count > 0 ? modelo.PlanPago.Count : modelo.PlazoTotal;
            y = DibujarUnDato(gfx, font, fontNegrita, margen, y, esLeasing == false ? "Número de Cuota" : "Número de Canon:", modelo.PlazoTotal.ToString(culturaCo));
            y = DibujarUnDato(gfx, font, fontNegrita, margen, y, esLeasing == false ? "Valor Cuota:" : "Valor Canon:", "$ " + FormatearMonedaPlanPagos(modelo.ValorCuota));

            // ---------- Descripción Total del Bien ----------
            y += 6;
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, margen, y, "Descripción Total del Bien");
            y = DibujarUnDato(gfx, font, fontNegrita, margen, y, "Ciudad del Inmueble:", modelo.CiudadInmueble);

            // ---------- Tabla de cuotas (con salto de página automático) ----------
            y += 10;
            y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosColumnas, encabezadosColumnas);

            var cuotas = (modelo.PlanPago ?? new System.Collections.Generic.List<PlanPagoDto>())
                .OrderBy(c => c.Cuota)
                .ToList();

            foreach (var cuota in cuotas)
            {
                // Si la fila no cabe antes de la zona de pie de página, se crea una página nueva
                if (y + altoFilaTabla > page.Height - altoZonaPie)
                {
                    gfx.Dispose(); // OBLIGATORIO: PdfSharp no deja crear otro XGraphics para la misma página si este no se libera antes

                    page = document.AddPage();
                    page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
                    page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
                    gfx = XGraphics.FromPdfPage(page);
                    y = altoZonaEncabezado;
                    y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosColumnas, encabezadosColumnas);
                }

                var esFilaInicial = cuota.Cuota == 0;
                var fuenteFila = esFilaInicial ? fontHeaderTabla : fontCelda;

                if (esLeasing)
                {
                    string[] valoresFila =
                    {
                        cuota.Cuota.ToString(culturaCo),
                        FormatearFechaPlanPagos(cuota.Fecha, culturaCo,esLeasing),
                        "$ " + FormatearMonedaPlanPagos(cuota.SaldoInicial),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.CapitalReducido == 0 ? cuota.Capital :cuota.CapitalReducido ),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.CapitalPorPagar),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.InteresReducido == 0 ? cuota.Interes : cuota.InteresReducido),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.InteresPorPagar),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.SeguroVida),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.SeguroHogar),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.CuotaUnidadPagadora),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.DeudaRemanente),
                    };

                    DibujarFilaTablaCuotas(gfx, fuenteFila, margen, y, anchosColumnas, valoresFila);
                }
                else
                {
                    string[] valoresFila =
                    {
                        cuota.Cuota.ToString(culturaCo),
                        FormatearFechaPlanPagos(cuota.Fecha, culturaCo,esLeasing),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.Capital),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.Interes),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.SeguroVida),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.SeguroHogar),
                        esFilaInicial ? string.Empty : FormatearMonedaPlanPagos(cuota.CuotaUnidadPagadora),
                        "$ " + FormatearMonedaPlanPagos(cuota.Saldo)
                    };

                    DibujarFilaTablaCuotas(gfx, fuenteFila, margen, y, anchosColumnas, valoresFila);
                }

                y += altoFilaTabla;
            }

            gfx.Dispose();
            // libera el XGraphics de la última página antes del loop de encabezado/pie

            // ---------- Encabezado / pie de página en TODAS las páginas ----------
            foreach (PdfPage pagina in document.Pages)
            {
                using var gfxPagina = XGraphics.FromPdfPage(pagina);

                if (imagenes != null && imagenes.Length > 0 && File.Exists(imagenes[0]))
                {
                    // Defensa: izquierda
                    var imgDefensa = XImage.FromFile(imagenes[0]);
                    double altoDefensa = 40;
                    double anchoDefensa = altoDefensa * (imgDefensa.PixelWidth / (double)imgDefensa.PixelHeight);
                    gfxPagina.DrawImage(imgDefensa, margen, 15, anchoDefensa, altoDefensa);
                }
                if (imagenes != null && imagenes.Length > 1 && File.Exists(imagenes[1]))
                {
                    // CajaHonor: derecha, misma fila que Defensa
                    var imgCajaHonor = XImage.FromFile(imagenes[1]);
                    double altoCaja = 40;
                    double anchoCaja = altoCaja * (imgCajaHonor.PixelWidth / (double)imgCajaHonor.PixelHeight);
                    double xCaja = pagina.Width - margen - anchoCaja;
                    gfxPagina.DrawImage(imgCajaHonor, xCaja, 15, anchoCaja, altoCaja);
                }
                if (imagenes != null && imagenes.Length > 2 && File.Exists(imagenes[2]))
                {
                    double yPie = pagina.Height - altoZonaPie + 10;
                    gfxPagina.DrawImage(XImage.FromFile(imagenes[2]), margen, yPie, pagina.Width - margen * 2, 45);
                }
            }

            document.Save(stream, false);
            stream.Position = 0;
            return stream;
        }

        // ===================== Helpers privados (mismo estilo que el resto de la clase) =====================

        private static double DibujarTituloSeccionPlanPagos(XGraphics gfx, XFont fontSeccion, double margen, double y, string titulo)
        {
            gfx.DrawString(titulo, fontSeccion, XBrushes.Black, new XRect(margen, y, 400, 14), XStringFormats.TopLeft);
            return y + 16;
        }

        private static double DibujarParDatos(
            XGraphics gfx, XFont font, XFont fontNegrita, double margen, double y,
            string etiquetaIzq, string valorIzq, string etiquetaDer, string valorDer)
        {
            gfx.DrawString(etiquetaIzq, font, XBrushes.Black, new XRect(margen, y, 120, 12), XStringFormats.TopLeft);
            gfx.DrawString(valorIzq ?? string.Empty, fontNegrita, XBrushes.Black, new XRect(margen + 122, y, 220, 12), XStringFormats.TopLeft);

            if (!string.IsNullOrEmpty(etiquetaDer))
            {
                gfx.DrawString(etiquetaDer, font, XBrushes.Black, new XRect(margen + 350, y, 100, 12), XStringFormats.TopLeft);
                gfx.DrawString(valorDer ?? string.Empty, fontNegrita, XBrushes.Black, new XRect(margen + 452, y, 120, 12), XStringFormats.TopLeft);
            }

            return y + 14;
        }

        private static double DibujarUnDato(XGraphics gfx, XFont font, XFont fontNegrita, double margen, double y, string etiqueta, string valor)
        {
            gfx.DrawString(etiqueta, font, XBrushes.Black, new XRect(margen, y, 120, 12), XStringFormats.TopLeft);
            gfx.DrawString(valor ?? string.Empty, fontNegrita, XBrushes.Black, new XRect(margen + 122, y, 300, 12), XStringFormats.TopLeft);
            return y + 14;
        }

        private static double DibujarEncabezadoTablaCuotas(
            XGraphics gfx, XFont fontHeaderTabla, double margen, double y, double[] anchos, string[] encabezados)
        {
            var pen = new XPen(XColors.Black, 0.75);
            var brush = new XSolidBrush(XColor.FromArgb(238, 242, 248));
            double x = margen;

            for (int i = 0; i < anchos.Length; i++)
            {
                gfx.DrawRectangle(brush, x, y, anchos[i], 16);
                gfx.DrawRectangle(pen, x, y, anchos[i], 16);
                gfx.DrawString(encabezados[i], fontHeaderTabla, XBrushes.Black,
                    new XRect(x, y + 2, anchos[i], 12), XStringFormats.TopCenter);
                x += anchos[i];
            }

            return y + 16;
        }

        private static void DibujarFilaTablaCuotas(XGraphics gfx, XFont font, double margen, double y, double[] anchos, string[] valores)
        {
            var pen = new XPen(XColors.Black, 0.5);
            double x = margen;

            for (int i = 0; i < anchos.Length; i++)
            {
                gfx.DrawRectangle(pen, x, y, anchos[i], 16);
                var formato = i <= 1 ? XStringFormats.TopCenter : XStringFormats.TopRight;
                var anchoTexto = i <= 1 ? anchos[i] : anchos[i] - 4;
                var xTexto = x;
                gfx.DrawString(valores[i], font, XBrushes.Black, new XRect(xTexto, y + 3, anchoTexto, 12), formato);
                x += anchos[i];
            }
        }

        private static string FormatearMonedaPlanPagos(decimal valor) =>
            valor.ToString("#,##0.00", CultureInfo.GetCultureInfo("es-CO"));

        private static string FormatearFechaPlanPagos(DateTime fecha, CultureInfo cultura, bool esLeasing) =>
       fecha == default
           ? string.Empty
           : fecha.ToString(esLeasing ? "dd/MM/yyyy" : "MM/dd/yyyy", cultura);
    }
}
