using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GA2.Domain.Core
{
    public partial class TemplateReportesPdfAnualizadoCartera
    {
        /// <summary>
        /// Extracto Anual de Plan de Pagos (Leasing Habitacional / Crédito Hipotecario).
        ///   - "Relación Pagos Realizados": pagos REALMENTE aplicados en <paramref name="periodo"/>,
        ///     obtenidos directamente de CPG_APLICACION_PAGOS (ya sin reversados).
        ///   - "Proyección anual de pagos": cuotas pendientes / futuras del plan de pago
        ///     (modelo.PlanPago).
        /// </summary>
        /// <param name="modelo">Datos del crédito, incluida la lista modelo.PlanPago (proyección).</param>
        /// <param name="pagosRealizados">Pagos aplicados durante el año, ya filtrados y sin reversados.</param>
        /// <param name="periodo">Año del extracto, ej. 2025.</param>
        /// <param name="imagenes">imagenes[0] = encabezado izq, imagenes[1] = encabezado der, imagenes[2] = pie de página.</param>
        public MemoryStream ReporteExtractoAnualPlanPagos(
            RequestProductoClienteDto modelo,
            List<PagosAplicadosCreditoAnio> pagosRealizados,
            int periodo,
            string[] imagenes)
        {
            if (modelo is null)
            {
                throw new ArgumentNullException(nameof(modelo), "El modelo del Extracto Anual no puede ser nulo.");
            }

            pagosRealizados ??= new List<PagosAplicadosCreditoAnio>();

            var stream = new MemoryStream();
            var document = new PdfDocument();

            var esLeasing = modelo.IdTipoCredito == 1;

            document.Info.Title = esLeasing
                ? $"Extracto Anual Leasing {periodo}"
                : $"Extracto Anual Crédito Hipotecario {periodo}";

            var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            var fontTitulo = new XFont("Arial", 13, XFontStyle.Bold, options);
            var font = new XFont("Arial", 9, XFontStyle.Regular, options);
            var fontNegrita = new XFont("Arial", 9, XFontStyle.Bold, options);
            var fontSeccion = new XFont("Arial", 10, XFontStyle.Bold, options);
            var fontHeaderTabla = new XFont("Arial", 7.5, XFontStyle.Bold, options);
            var fontCelda = new XFont("Arial", 7.5, XFontStyle.Regular, options);

            var culturaCo = CultureInfo.GetCultureInfo("es-CO");

            const double margen = 40;
            const double altoZonaEncabezado = 75;
            const double altoZonaPie = 70;
            const double altoFilaTabla = 16;

            // Color de las barras de título de sección ("INFORMACIÓN DEL CLIENTE", etc.).
            // Extraído por muestreo de píxeles directo de la plantilla de referencia que compartiste
            // (azul marino, ~RGB(0,32,96) / #002060 — coincide con el azul del logo "Defensa").
            var colorBarraSeccion = XColor.FromArgb(0, 32, 96);

            // ---------- Pagos realizados: vienen ya resueltos desde CPG_APLICACION_PAGOS ----------
            var realizadas = pagosRealizados
                .OrderBy(c => c.CPG_FECHA_APLICACION)
                .ToList();

            // ---------- Proyección: cuotas pendientes del plan de pago ----------
            // NOTA / SUPUESTO A CONFIRMAR: se asume que modelo.PlanPago que retorna
            // ObtenerPlanPagosCredito ya viene filtrado a solo cuotas pendientes/futuras
            // (no repite el historial ya cubierto por "realizadas"). Si en tu caso
            // ObtenerPlanPagosCredito retorna TODO el historial desde el desembolso,
            // agrega aquí un filtro adicional, por ejemplo:
            //   .Where(c => c.Fecha > (realizadas.Any() ? realizadas.Max(r => r.CPG_FECHA_APLICACION) : DateTime.Today))
            var proyeccion = (modelo.PlanPago ?? new List<PlanPagoDto>())
                .Where(c => c.Cuota > 0) // excluye la fila 0 (desembolso inicial)
                .OrderBy(c => c.Fecha)
                .ToList();

            // ---------- Columnas de cada tabla ----------
            double[] anchosProyeccion = esLeasing
                ? new double[] { 45, 62, 70, 78, 70, 78, 62, 48, 62, 62, 62 }  // 11 columnas
                : new double[] { 82, 90, 90, 82, 82, 106 };                    // 6 columnas

            string[] encabezadosProyeccion = esLeasing
                ? new[] { "N° Canon", "Fecha Cuota", "Capital A Pagar", "Capital Remanente", "Interés A Pagar", "Interés Remanente", "Seguro Vida", "Días Mora", "Interés Mora", "Otros Seguros", "Otros Gastos" }
                : new[] { "Fecha Cuota", "Capital", "Intereses", "Seguro Vida", "Seguro Hogar", "Valor Cuota" };

            double[] anchosRealizados = esLeasing
                ? new double[] { 88, 92, 88, 100, 92, 88, 88 }  // 7 columnas
                : new double[] { 86, 90, 90, 88, 88, 90 };       // 6 columnas

            string[] encabezadosRealizados = esLeasing
                ? new[] { "Fecha Vencimiento", "Valor Pagado", "Capital", "Interés Corriente", "Interés Mora", "Seguro Vida", "Seguro Hogar" }
                : new[] { "Fecha Vencimiento", "Valor Pagado", "Capital", "Intereses", "Seguro Vida", "Seguro Hogar" };

            PdfPage page = document.AddPage();
            page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
            page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            double y = altoZonaEncabezado;

            var titulo = esLeasing
                ? $"EXTRACTO ANUAL LEASING HABITACIONAL {periodo}"
                : $"EXTRACTO ANUAL CRÉDITO HIPOTECARIO {periodo}";

            // ---------- Título ----------
            gfx.DrawString(titulo, fontTitulo, XBrushes.Black,
                new XRect(margen, y, page.Width - margen * 2, 20), XStringFormats.TopCenter);
            y += 28;

            double anchoTotal = page.Width - margen * 2;

            // ---------- Información del Cliente ----------
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, colorBarraSeccion, margen, anchoTotal, y, "INFORMACIÓN DEL CLIENTE");

            // PENDIENTE DE CONFIRMAR: de dónde salen Código / Tipo Cliente / Teléfono
            // (no están en CPG_APLICACION_PAGOS/CRE_CREDITO ni se han confirmado en
            // productoCliente). Mientras se confirma, quedan en blanco igual que antes.
            List<string[]> filasInfo;
            if (esLeasing)
            {
                filasInfo = new List<string[]>
                {
                    new[] { "Código:", "", "Tipo Cliente:", "" },
                    new[] { "Nombre:", modelo.NombreCliente, "Teléfono:", "" },
                    new[] { "Identificación:", modelo.Identificacion, "Contrato:", modelo.NumeroCredito.ToString(culturaCo) },
                    new[] { "Ciudad:", modelo.CiudadInmueble, "", "" },
                };
            }
            else
            {
                filasInfo = new List<string[]>
                {
                    new[] { "Nombre:", modelo.NombreCliente, "Identificación:", modelo.Identificacion },
                    new[] { "Contrato:", modelo.NumeroCredito.ToString(culturaCo), "Ciudad:", modelo.CiudadInmueble },
                };
            }
            y = DibujarBloqueInfoCliente(gfx, font, fontNegrita, margen, y, anchoTotal, filasInfo);

            // ---------- Tabla: Proyección anual de pagos ----------
            y += 10;
            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, colorBarraSeccion, margen, anchoTotal, y, "PROYECCIÓN ANUAL DE PAGOS");
            y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosProyeccion, encabezadosProyeccion);

            for (int i = 0; i < proyeccion.Count; i++)
            {
                var cuota = proyeccion[i];

                if (y + altoFilaTabla > page.Height - altoZonaPie)
                {
                    gfx.Dispose();
                    page = document.AddPage();
                    page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
                    page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
                    gfx = XGraphics.FromPdfPage(page);
                    y = altoZonaEncabezado;
                    y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosProyeccion, encabezadosProyeccion);
                }

                // La mora solo aplica a la próxima cuota pendiente (i == 0)
                var mostrarMora = i == 0;

                if (esLeasing)
                {
                    string[] valoresFila =
                    {
                        cuota.Cuota.ToString(culturaCo),
                        FormatearFechaPlanPagos(cuota.Fecha, culturaCo, esLeasing),
                        FormatearMonedaPlanPagos(cuota.CapitalReducido == 0 ? cuota.Capital : cuota.CapitalReducido),
                        FormatearMonedaPlanPagos(cuota.CapitalPorPagar),
                        FormatearMonedaPlanPagos(cuota.InteresReducido == 0 ? cuota.Interes : cuota.InteresReducido),
                        FormatearMonedaPlanPagos(cuota.InteresPorPagar),
                        FormatearMonedaPlanPagos(cuota.SeguroVida),
                        mostrarMora ? modelo.DiasMora.ToString(culturaCo) : "0",
                        mostrarMora ? FormatearMonedaPlanPagos(modelo.AcumuladoInteresMora) : FormatearMonedaPlanPagos(0m),
                        FormatearMonedaPlanPagos(0m), // PENDIENTE: Otros Seguros — origen a confirmar
                        FormatearMonedaPlanPagos(0m), // PENDIENTE: Otros Gastos — origen a confirmar
                    };
                    DibujarFilaTablaCuotas(gfx, fontCelda, margen, y, anchosProyeccion, valoresFila);
                }
                else
                {
                    string[] valoresFila =
                    {
                        FormatearFechaPlanPagos(cuota.Fecha, culturaCo, esLeasing),
                        FormatearMonedaPlanPagos(cuota.Capital),
                        FormatearMonedaPlanPagos(cuota.Interes),
                        FormatearMonedaPlanPagos(cuota.SeguroVida),
                        FormatearMonedaPlanPagos(cuota.SeguroHogar),
                        FormatearMonedaPlanPagos(cuota.CuotaUnidadPagadora),
                    };
                    DibujarFilaTablaCuotas(gfx, fontCelda, margen, y, anchosProyeccion, valoresFila);
                }

                y += altoFilaTabla;
            }

            // ---------- Tabla: Relación Pagos Realizados ----------
            y += 14;
            if (y + altoFilaTabla * 2 > page.Height - altoZonaPie)
            {
                gfx.Dispose();
                page = document.AddPage();
                page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
                page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
                gfx = XGraphics.FromPdfPage(page);
                y = altoZonaEncabezado;
            }

            y = DibujarTituloSeccionPlanPagos(gfx, fontSeccion, colorBarraSeccion, margen, anchoTotal, y, "RELACIÓN PAGOS REALIZADOS");
            y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosRealizados, encabezadosRealizados);

            foreach (var pago in realizadas)
            {
                if (y + altoFilaTabla > page.Height - altoZonaPie)
                {
                    gfx.Dispose();
                    page = document.AddPage();
                    page.Size = esLeasing ? PdfSharp.PageSize.Legal : PdfSharp.PageSize.Letter;
                    page.Orientation = esLeasing ? PdfSharp.PageOrientation.Landscape : PdfSharp.PageOrientation.Portrait;
                    gfx = XGraphics.FromPdfPage(page);
                    y = altoZonaEncabezado;
                    y = DibujarEncabezadoTablaCuotas(gfx, fontHeaderTabla, margen, y, anchosRealizados, encabezadosRealizados);
                }

                if (esLeasing)
                {
                    string[] valoresFila =
                    {
                        FormatearFechaPlanPagos(pago.CPG_FECHA_APLICACION, culturaCo, esLeasing),
                        FormatearMonedaPlanPagos(pago.CPG_VALOR_PAGO),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_CAPITAL),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_INTERES_CORRIENTE),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_INTERES_MORA),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_SEGURO_VIDA),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_SEGURO_HOGAR),
                    };
                    DibujarFilaTablaCuotas(gfx, fontCelda, margen, y, anchosRealizados, valoresFila);
                }
                else
                {
                    string[] valoresFila =
                    {
                        FormatearFechaPlanPagos(pago.CPG_FECHA_APLICACION, culturaCo, esLeasing),
                        FormatearMonedaPlanPagos(pago.CPG_VALOR_PAGO),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_CAPITAL),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_INTERES_CORRIENTE),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_SEGURO_VIDA),
                        FormatearMonedaPlanPagos(pago.CPG_ABONO_SEGURO_HOGAR),
                    };
                    DibujarFilaTablaCuotas(gfx, fontCelda, margen, y, anchosRealizados, valoresFila);
                }

                y += altoFilaTabla;
            }

            gfx.Dispose();

            // ---------- Encabezado / pie de página en TODAS las páginas ----------
            foreach (PdfPage pagina in document.Pages)
            {
                using var gfxPagina = XGraphics.FromPdfPage(pagina);

                if (imagenes != null && imagenes.Length > 0 && File.Exists(imagenes[0]))
                {
                    var imgIzq = XImage.FromFile(imagenes[0]);
                    double alto = 40;
                    double ancho = alto * (imgIzq.PixelWidth / (double)imgIzq.PixelHeight);
                    gfxPagina.DrawImage(imgIzq, margen, 15, ancho, alto);
                }
                if (imagenes != null && imagenes.Length > 1 && File.Exists(imagenes[1]))
                {
                    var imgDer = XImage.FromFile(imagenes[1]);
                    double alto = 40;
                    double ancho = alto * (imgDer.PixelWidth / (double)imgDer.PixelHeight);
                    double xDer = pagina.Width - margen - ancho;
                    gfxPagina.DrawImage(imgDer, xDer, 15, ancho, alto);
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

        /// <summary>
        /// Título de sección con barra de color de fondo y texto en blanco,
        /// igual al estilo de la plantilla de referencia
        /// ("INFORMACIÓN DEL CLIENTE" / "PROYECCIÓN ANUAL DE PAGOS" / "RELACIÓN PAGOS REALIZADOS").
        /// </summary>
        private static double DibujarTituloSeccionPlanPagos(
            XGraphics gfx, XFont fontSeccion, XColor colorBarra, double margen, double anchoTotal, double y, string titulo)
        {
            const double altoBarra = 18;
            var fill = new XSolidBrush(colorBarra);
            gfx.DrawRectangle(fill, margen, y, anchoTotal, altoBarra);
            gfx.DrawString(titulo, fontSeccion, XBrushes.White,
                new XRect(margen + 6, y + 3, anchoTotal - 12, 14), XStringFormats.TopLeft);
            return y + altoBarra + 4;
        }

        /// <summary>
        /// Bloque "Información del Cliente" como tabla con bordes (etiqueta/valor x 2 columnas por fila),
        /// igual al recuadro de la plantilla de referencia. Cada fila trae 4 celdas:
        /// [etiqueta1, valor1, etiqueta2, valor2] — etiqueta2/valor2 pueden venir vacíos.
        /// </summary>
        private static double DibujarBloqueInfoCliente(
            XGraphics gfx, XFont font, XFont fontNegrita, double margen, double y, double anchoTotal, List<string[]> filas)
        {
            const double altoFila = 16;
            const double anchoEtiqueta = 90;
            double anchoValor = (anchoTotal / 2) - anchoEtiqueta;
            double[] anchos = { anchoEtiqueta, anchoValor, anchoEtiqueta, anchoValor };
            var pen = new XPen(XColors.Gray, 0.6);

            foreach (var fila in filas)
            {
                double x = margen;
                for (int i = 0; i < 4; i++)
                {
                    gfx.DrawRectangle(pen, x, y, anchos[i], altoFila);
                    var texto = i < fila.Length ? fila[i] : string.Empty;
                    var esEtiqueta = i == 0 || i == 2;
                    var f = esEtiqueta ? fontNegrita : font;
                    gfx.DrawString(texto ?? string.Empty, f, XBrushes.Black,
                        new XRect(x + 4, y + 2, anchos[i] - 6, 12), XStringFormats.TopLeft);
                    x += anchos[i];
                }
                y += altoFila;
            }

            return y;
        }

        private static double DibujarEncabezadoTablaCuotas(
            XGraphics gfx, XFont fontHeaderTabla, double margen, double y, double[] anchos, string[] encabezados)
        {
            var pen = new XPen(XColors.Black, 0.75);
            var brush = new XSolidBrush(XColor.FromArgb(217, 228, 240)); // tono claro de azul, también muestreado de la plantilla de referencia
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