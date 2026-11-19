using GA2.Application.Dto;
using GA2.Domain.Entities;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace GA2.Domain.Core
{

    /// <summary>
    /// Permite armar el MemoryStream que se usa para cada reporte
    /// </summary>
    public class TemplateReportes
    {

        /// <summary>
        /// ReporteTransferenciaCesantias
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imagenes"></param>
        /// <returns></returns>
        public MemoryStream ReporteTransferenciaCesantias(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);

            string text = "Bogotá " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            XRect rect = new XRect(250, 250, 250, 220);
            rect = new XRect(50, 150, 1000, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "AL CONTESTAR CITE " + data.Rows[0].ItemArray[2].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 175, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fecha: " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 195, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Área: GRUPO ASUNTOS JURÍDICOS DE OPERACIONES";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 215, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Doctora";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 235, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "ALEXANDRA PATRICIA OLAYA DAJER";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 255, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Jefe División Cesantías";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 275, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fondo Nacional del Ahorro";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 295, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Calle 12 No. 65 – 11 Zona Industrial de Puente Aranda ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 315, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Bogotá D.C";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 335, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Asunto: Solicitud transferencia de cesantías.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 355, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordial saludo,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 375, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "De manera atenta y de conformidad con la aceptación de afiliación a la Entidad de " +
                data.Rows[0].ItemArray[3].ToString().ToUpper() + " " + data.Rows[0].ItemArray[4].ToString().ToUpper() + " identificado(a) con " +
                data.Rows[0].ItemArray[6].ToString().ToUpper() + " No. " + data.Rows[0].ItemArray[7].ToString() + ", y en cumplimiento " +
                "del Artículo 11, numeral 4 de la Ley 973 del 21 de julio de 2005 que a su tenor literal reza: " +
                "'El ahorro por concepto de causación anual de cesantías a favor de los afiliados que la Nación apropiará y " +
                "situará anualmente para ser transferido en los términos de la presente ley, a la Caja Promotora de Vivienda Militar y de Policía' " +
                "de manera respetuosa se solicita:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 417, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Transferir a la Caja Promotora de Vivienda Militar y de Policía, el valor que por concepto de cesantías registre " +
                data.Rows[0].ItemArray[3].ToString().ToUpper() + " " + data.Rows[0].ItemArray[4].ToString().ToUpper() + ", como quiera que dichas prestaciones sociales " +
                "constituyen factor de aporte de los afiliados que en la actualidad tengan una vinculación laboral y se encuentren afiliados a la Entidad.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 497, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "En este sentido y para efectos de la consignación los valores deben ser cancelados en la cuenta " +
                "de ahorros del Banco Popular No. 22008072001-4 a nombre de la Caja Promotora de Vivienda Militar y de Policía NIT 860.021.967-7.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 557, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 607, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg. XIMENA SUÁREZ HERNÁNDEZ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 627, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Líder Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 647, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Proyectó y elaboró:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 667, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg.Leidy Contreras Amado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 677, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Profesional Universitario 02";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 687, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 697, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 707, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMiniNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;
        }

        /// <summary>
        /// ReporteTransferenciaCesantiasPONAL
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imagenes"></param>
        /// <returns></returns>
        public MemoryStream ReporteTransferenciaCesantiasPONAL(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);

            string text = "Bogotá " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            XRect rect = new XRect(250, 250, 250, 220);
            rect = new XRect(50, 150, 1000, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "AL CONTESTAR CITE " + data.Rows[0].ItemArray[2].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 175, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fecha: " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 195, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Área: GRUPO ASUNTOS JURÍDICOS DE OPERACIONES";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 215, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Mayor General,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 235, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "ÁLVARO PICO MALAVER";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 255, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Director de Talento Humano de la Policía Nacional";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 275, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Carrera 59 # 26 – 21 CAN Piso 1°";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 295, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "ditah.spqrs@policia.gov.co / ditah.oac@policia.gov.co";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 315, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Bogotá D.C";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 335, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Asunto: Solicitud transferencia de cesantías.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 355, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordial saludo,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 375, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "En atención a la solicitud radicada en la Entidad con No. 23-01-2022081900582 el 19 de agosto de 2022," +
                data.Rows[0].ItemArray[3].ToString().ToUpper() + " " + data.Rows[0].ItemArray[4].ToString().ToUpper() + " identificado(a) con " +
                data.Rows[0].ItemArray[6].ToString().ToUpper() + " No. " + data.Rows[0].ItemArray[7].ToString() + "de manera atenta se solicita que, " +
                "a partir de la fecha, se transfiera a la Caja Promotora de Vivienda Militar y de Policía, el valor que por concepto de cesantías se cause a nombre de la afiliada en mención.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 395, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Lo anterior en cumplimiento del Artículo 11, numeral 4 de la Ley 973 del año 2005 “El ahorro por concepto de causación anual de cesantías a " +
                "favor de los afiliados que la Nación apropiará y situará anualmente para ser transferido en los términos de la presente ley, a la Caja Promotora de Vivienda Militar y de Policía”.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 457, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Dichas prestaciones sociales constituyen factor de aporte de los afiliados que en la actualidad tenga una vinculación laboral.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 502, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Para efectos de la consignación de los dineros a que haya lugar, la Caja Promotora de Vivienda Militar y de Policía con " +
                "Nit.860021967-7 tiene dispuesta la CUENTA DE AHORROS No.220-080-720-014 del BANCO POPULAR, favor informar a la Entidad una vez " +
                "se realice dicho pago.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 532, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 592, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg. XIMENA SUÁREZ HERNÁNDEZ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 627, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Líder Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 652, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Proyectó y elaboró:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 667, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg.Leidy Contreras Amado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 677, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Profesional Universitario 02";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 687, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 697, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 707, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMiniNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;
        }

        /// <summary>
        /// ReporteRespuestaNoProcedente
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imagenes"></param>
        /// <returns></returns>
        public MemoryStream ReporteRespuestaNoProcedente(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();
            string text = string.Empty;

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);

            text = "Bogotá " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            XRect rect = new XRect(250, 250, 250, 220);
            rect = new XRect(50, 100, 500, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "AL CONTESTAR CITE RADICADO"; //+ data.Rows[0].ItemArray[2].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 120, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fecha: " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 135, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Área: GRUPO ASUNTOS JURÍDICOS DE OPERACIONES";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 150, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Señor(a)";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 180, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[3].ToString().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 195, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[10].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 210, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Asunto: Respuesta a radicado N° 23-01-2022081700567";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 235, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "En atención a la solicitud radicada bajo el número del asunto, la Caja Promotora de Vivienda Militar y de Policía " +
                "informa que para definir de fondo su solicitud de afiliación extemporánea, y determinar los eventuales " +
                "derechos que le puedan corresponder, es necesario que allegue la siguiente documentación:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 265, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Es de aclarar que si sus cesantías se encuentran pignoradas su solicitud de afiliación no procede, esto de conformidad con lo " +
                "establecido en el artículo 11 de la Ley 973 de 2005, por lo cual deberá esperar a que cese la pignoración de cesantías para requerir su " +
                "afiliación a esta Entidad, pues dichas prestaciones sociales constituyen factor de aporte de los afiliados que tengan una vinculación laboral, " +
                "y Caja Honor desde el momento de la afiliación es quien debe tener su administración.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 370, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "De otra parte, si transcurrido el término de 1 mes y no ha enviado la documentación requerida, favor allegar nuevamente último desprendible de pago.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 440, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Para mayor facilidad podrá remitir debidamente escaneados los documentos requeridos a nuestro correo electrónico: afiliaciones @cajahonor.gov.co";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 480, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Toda solicitud adicional, podrá realizarla a través de los canales de comunicación que se detallan a continuación: Centro de Contacto al Ciudadano CCC " +
                "en la línea de atención (601)7557070 en Bogotá o a la línea gratuita nacional 018000185570; Correo electrónico contactenos@cajahonor.gov.co; " +
                "Chat Virtual Asesor en Línea del Portal Web www.cajahonor.gov.co, personalmente en los Puntos de Atención a nivel nacional o a los " +
                "Puntos Móviles de Caja Honor; servicio en línea, Portal Transaccional. Defensoría del Consumidor Financiero, a cargo del Dr. Pablo Valencia Agudo ubicado en " +
                "la Carrera 11 A # 96 – 51 Oficina 203 Edificio Oficity de Bogotá D.C., teléfono (601) 6108161 – (601) 6108164, correo: defensoriacajahonor@legalcrc.com. " +
                "Recuerde que puede seguirnos a través de las redes sociales Facebook, Twitter, YouTube e Instagram";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 520, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 635, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg. XIMENA SUÁREZ HERNÁNDEZ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 660, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Líder Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 675, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Proyectó y elaboró:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 690, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg.Leidy Contreras Amado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 705, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Profesional Universitario 02";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 720, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 735, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 750, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMiniNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;
        }

        /// <summary>
        /// ReporteRespuestaEmbargo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imagenes"></param>
        /// <returns></returns>
        /// 
        public MemoryStream ObtenerCartaInconsistencia(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Carta de Inconsistencia";

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            XFont fontTitulo = new XFont("Arial", 12, XFontStyle.Bold, options);
            XFont fontNormal = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontPequena = new XFont("Arial", 8, XFontStyle.Regular, options);

            DataRow row = data != null && data.Rows.Count > 0 ? data.Rows[0] : null;

            #region PAGINA 1

            PdfPage page1 = document.AddPage();
            page1.Size = PdfSharp.PageSize.Letter;

            // Agregamos el 'using' para que se destruya al salir de este bloque de código
            using (XGraphics gfx = XGraphics.FromPdfPage(page1))
            {
                XTextFormatter tf = new XTextFormatter(gfx);

                double margen = 50;
                double y = 100;

                // Ciudad y fecha
                gfx.DrawString(
                    ObtenerValorOPlaceholder(row, "CiudadFecha"),
                    fontNormal,
                    XBrushes.Black,
                    new XRect(margen, y, 300, 20),
                    XStringFormats.TopLeft);

                // Caja radicado
                gfx.DrawRectangle(XPens.Black, 340, 85, 220, 60);

                tf.DrawString(
                    $"AL CONTESTAR CITE: {ObtenerValorOPlaceholder(row, "Radicado378")}\n\n" +
                    $"FECHA: {ObtenerValorOPlaceholder(row, "FechaGeneracion")}",
                    fontPequena,
                    XBrushes.Black,
                    new XRect(348, 93, 205, 45));

                y += 45;

                // Señor
                gfx.DrawString("Señor(a):", fontNormal, XBrushes.Black, margen, y);
                y += 20;

                gfx.DrawString($"Nombre: {ObtenerValorOPlaceholder(row, "NombreAfiliado")}", fontNormal, XBrushes.Black, margen, y);
                y += 18;

                gfx.DrawString($"Correo: {ObtenerValorOPlaceholder(row, "CorreoAfiliado")}", fontNormal, XBrushes.Black, margen, y);
                y += 18;

                gfx.DrawString($"Dirección: {ObtenerValorOPlaceholder(row, "DireccionAfiliado")}", fontNormal, XBrushes.Black, margen, y);
                y += 18;

                gfx.DrawString($"Ciudad: {ObtenerValorOPlaceholder(row, "CiudadAfiliado")}", fontNormal, XBrushes.Black, margen, y);
                y += 40;

                // Asunto
                gfx.DrawString("Asunto: CARTA DE INCONSISTENCIA", fontNegrita, XBrushes.Black, margen, y);
                y += 30;

                string intro = $"De manera cordial se informa que su solicitud registrada bajo el número de {ObtenerValorOPlaceholder(row, "Radicado")} con fecha de trámite {ObtenerValorOPlaceholder(row, "FechaTramite")} una vez efectuada la revisión presentó las siguientes inconsistencias:";

                tf.DrawString(intro, fontNormal, XBrushes.Black, new XRect(margen, y, 500, 50));
                y += 55;

                tf.DrawString(ObtenerValorOPlaceholder(row, "TextoInconsistencia"), fontNormal, XBrushes.Black, new XRect(margen, y, 500, 300));
                y += 310;

                string cierre = $"Teniendo en cuenta lo anterior, no es procedente su {ObtenerValorOPlaceholder(row, "NombreSolicitud")}, razón por la cual la presente solicitud se da por finalizada.";

                tf.DrawString(cierre, fontNormal, XBrushes.Black, new XRect(margen, y, 500, 50));
            } // Aquí se libera automáticamente el XGraphics de la página 1

            #endregion

            #region PAGINA 2

            PdfPage page2 = document.AddPage();
            page2.Size = PdfSharp.PageSize.Letter;

            // Agregamos el 'using' también para la página 2
            using (XGraphics gfx2 = XGraphics.FromPdfPage(page2))
            {
                XTextFormatter tf2 = new XTextFormatter(gfx2);

                double margen2 = 50;
                double y2 = 120;

                string textoContacto = @"Se le invita a seguirnos a través de las redes sociales Facebook, Twitter, YouTube e Instagram.
En caso tener inquietudes adicionales puede contactarnos a través de:

• Línea telefónica en Bogotá 7557070, a nivel nacional 018000185570 (gratuita).

• Correo electrónico contactenos@cajahonor.gov.co.

• Asesor en línea por medio Chat Bot ""Patria"" de la página web.

Adicionalmente, Caja Honor cuenta con la Defensoría del Consumidor Financiero, a cargo del Dr. Pablo Tomas Silva Mariño – Calle 13 No. 4 – 90, Oficina 309 de Bogotá D.C., teléfono 2823570, correo: ptsilvadefensor@hotmail.com.

Cordialmente,";

                tf2.DrawString(textoContacto, fontNormal, XBrushes.Black, new XRect(margen2, y2, 500, 220));
                y2 += 220;

                gfx2.DrawString(
               "MAX BELTRÁN",
               fontNegrita,
               XBrushes.Black,
               new XRect(0, y2, page2.Width, 20),
               XStringFormats.TopCenter);

                y2 += 20;
                gfx2.DrawString(
                    "LIDER DEL PUNTO DE ATENCIÓN",
                    fontNegrita,
                    XBrushes.Black,
                    new XRect(0, y2, page2.Width, 20),
                    XStringFormats.TopCenter);

                y2 += 18;

                gfx2.DrawString(
                    "Líder de grupo",
                    fontNormal,
                    XBrushes.Black,
                    new XRect(0, y2, page2.Width, 20),
                    XStringFormats.TopCenter);

                y2 += 60;

                gfx2.DrawString("Firma", fontPequena, XBrushes.Black, margen2, y2);
                y2 += 20;

                gfx2.DrawString($"Elaboró: {ObtenerValorOPlaceholder(row, "NombreAsesor")}", fontPequena, XBrushes.Black, margen2, y2);
                y2 += 20;

                gfx2.DrawString(ObtenerValorOPlaceholder(row, "CargoAsesor"), fontPequena, XBrushes.Black, margen2, y2);
                y2 += 25;

                gfx2.DrawString("Nota: Documento original firmado digitalmente", fontPequena, XBrushes.Black, margen2, y2);
            } // Aquí se libera automáticamente el XGraphics de la página 2

            #endregion

            #region PROCESAMIENTO DE ELEMENTOS COMUNES (ENCABEZADO Y PIE DE PÁGINA)

            // Ahora que las páginas están libres de contextos previos, podemos iterar de forma segura
            foreach (PdfPage page in document.Pages)
            {
                using (XGraphics currentGfx = XGraphics.FromPdfPage(page))
                {
                    // 1. Dibujar Encabezado (imagenes[0])
                    if (imagenes != null && imagenes.Length > 0 && File.Exists(imagenes[0]))
                    {
                        XImage logoHeader = XImage.FromFile(imagenes[0]);
                        currentGfx.DrawImage(logoHeader, 40, 20, 520, 50);
                    }

                    // 2. Dibujar Pie de Página (imagenes[1])
                    if (imagenes != null && imagenes.Length > 1 && File.Exists(imagenes[1]))
                    {
                        XImage logoFooter = XImage.FromFile(imagenes[1]);
                        double footerY = page.Height - 75;
                        currentGfx.DrawImage(logoFooter, 40, footerY, 520, 55);
                    }
                } // Se libera el XGraphics de la iteración actual antes de pasar a la siguiente hoja
            }

            #endregion

            document.Save(stream, false);
            stream.Position = 0;

            return stream;
        }

        public MemoryStream ObtenerRespuestaSolicitudDePago(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Formulario Único de Pago";

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            XFont fontTitulo = new XFont("Arial", 15, XFontStyle.Regular, options);
            XFont fontSeccion = new XFont("Arial", 8.5, XFontStyle.Regular, options);
            XFont fontLabel = new XFont("Arial", 5.5, XFontStyle.Italic, options);
            XFont fontValor = new XFont("Arial", 7, XFontStyle.Regular, options);
            XFont fontValorBold = new XFont("Arial", 7.5, XFontStyle.Bold, options);
            XFont fontTablaHeader = new XFont("Arial", 6.5, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6.2, XFontStyle.Regular, options);

            XColor colorSeccion = XColor.FromArgb(132, 156, 196);
            XPen pen = new XPen(XColors.Black, 0.7);
            XBrush brushHeader = new XSolidBrush(colorSeccion);

            DataRow row = data != null && data.Rows.Count > 0 ? data.Rows[0] : null;

            // =========================
            // PAGINA 1
            // =========================
            PdfPage page1 = document.AddPage();
            page1.Size = PdfSharp.PageSize.Letter;
            XGraphics gfx1 = XGraphics.FromPdfPage(page1);
            XTextFormatter tf1 = new XTextFormatter(gfx1);

            DibujarEncabezadoFormulario(
                gfx1,
                tf1,
                fontTitulo,
                fontMini,
                fontValorBold,
                imagenes,
                row,
                page1.Width
            );

            double margenX = 30;
            double anchoTotal = page1.Width - (margenX * 2); // 552
            double y = 128;
            double x1 = margenX;
            double rowH = 26;

            // Sección 1
            DibujarTituloSeccion(
                gfx1,
                tf1,
                fontSeccion,
                brushHeader,
                pen,
                new XRect(margenX, y, anchoTotal, 18),
                "Información del afiliado (carácter obligatorio)");
            y += 18;

            double[] colsAfiliado1 = { 147, 202, 203 }; // suma 552

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "Identificación", ObtenerValorOPlaceholder(row, "Identificacion"),
                "Nombres", ObtenerValorOPlaceholder(row, "Nombres"),
                "Apellidos", ObtenerValorOPlaceholder(row, "Apellidos"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "Grado", ObtenerValorOPlaceholder(row, "Grado"),
                "Fuerza", ObtenerValorOPlaceholder(row, "Fuerza"),
                "Unidad ejecutora", ObtenerValorOPlaceholder(row, "UnidadEjecutora"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "País", ObtenerValorOPlaceholder(row, "Pais"),
                "Departamento", ObtenerValorOPlaceholder(row, "Departamento"),
                "Ciudad/Municipio", ObtenerValorOPlaceholder(row, "CiudadMunicipio"));
            y += rowH;

            DibujarFila2Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 349, 203 }, rowH, // suma 552
                "Dirección", ObtenerValorOPlaceholder(row, "Direccion"),
                "Barrio", ObtenerValorOPlaceholder(row, "Barrio"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "E-mail personal", ObtenerValorOPlaceholder(row, "EmailPersonal"),
                "E-mail institucional", ObtenerValorOPlaceholder(row, "EmailInstitucional"),
                "Número de teléfono", ObtenerValorOPlaceholder(row, "Telefono"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "Número de celular", ObtenerValorOPlaceholder(row, "Celular"),
                "Estado Civil", ObtenerValorOPlaceholder(row, "EstadoCivil"),
                "Tipo Documento Cónyuge", ObtenerValorOPlaceholder(row, "TipoDocumentoConyuge"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, colsAfiliado1, rowH,
                "Número de Identificación Cónyuge", ObtenerValorOPlaceholder(row, "NumeroIdentificacionConyuge"),
                "Nombres Cónyuge", ObtenerValorOPlaceholder(row, "NombresConyuge"),
                "Apellidos Cónyuge", ObtenerValorOPlaceholder(row, "ApellidosConyuge"));
            y += rowH + 10;

            // Sección 2
            DibujarTituloSeccion(
                gfx1,
                tf1,
                fontSeccion,
                brushHeader,
                pen,
                new XRect(margenX, y, anchoTotal, 18),
                "Información de trámites, modalidad y valor a girar");
            y += 18;

            DibujarFila2Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 300, 252 }, rowH, // suma 552
                "Tipo de trámite", ObtenerValorOPlaceholder(row, "TipoTramite"),
                "Modalidad", ObtenerValorOPlaceholder(row, "Modalidad"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 135, 145, 272 }, rowH, // suma 552
                "Vlr. Solicitado / Vlr. Primer Pago", ObtenerValorOPlaceholder(row, "ValorSolicitadoPrimerPago"),
                "Identificación Estudiante", ObtenerValorOPlaceholder(row, "IdentificacionEstudiante"),
                "Nombres/Apellidos Estudiante", ObtenerValorOPlaceholder(row, "NombreApellidosEstudiante"));
            y += rowH;

            DibujarFila2Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 412, 140 }, rowH, // suma 552
                "Programa Académico", ObtenerValorOPlaceholder(row, "ProgramaAcademico"),
                "Fecha de Pago (día-mes-año)", ObtenerValorOPlaceholder(row, "FechaPago"));
            y += rowH;

            DibujarFila3Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 135, 135, 282 }, rowH, // suma 552
                "Número de Matrícula Inmobiliaria", ObtenerValorOPlaceholder(row, "NumeroMatriculaInmobiliaria"),
                "Número de Escritura Inmueble", ObtenerValorOPlaceholder(row, "NumeroEscrituraInmueble"),
                "Dirección Inmueble", ObtenerValorOPlaceholder(row, "DireccionInmueble"));
            y += rowH;

            DibujarFila4Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 135, 135, 135, 147 }, rowH, // suma 552
                "Departamento Inmueble", ObtenerValorOPlaceholder(row, "DepartamentoInmueble"),
                "Ciudad/Municipio Inmueble", ObtenerValorOPlaceholder(row, "CiudadMunicipioInmueble"),
                "Notaría", ObtenerValorOPlaceholder(row, "Notaria"),
                "Fecha Escrituración", ObtenerValorOPlaceholder(row, "FechaEscrituracion"));
            y += rowH;

            DibujarFila2Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 160, 392 }, rowH, // suma 552
                "Identificación Vendedor", ObtenerValorOPlaceholder(row, "IdentificacionVendedor"),
                "Nombres/Apellidos Vendedor", ObtenerValorOPlaceholder(row, "NombreApellidosVendedor"));
            y += rowH;

            DibujarFila2Columnas(
                gfx1, tf1, pen, fontLabel, fontValor,
                x1, y, new double[] { 160, 160 }, rowH,
                "Tipo Vivienda", ObtenerValorOPlaceholder(row, "TipoVivienda"),
                "Valor 2do Pago", ObtenerValorOPlaceholder(row, "ValorSegundoPago"));

            gfx1.DrawRectangle(pen, x1 + 320, y, 232, rowH); // completa el ancho exacto
            y += rowH + 10;

            // Sección 3
            DibujarTituloSeccion(
                gfx1,
                tf1,
                fontSeccion,
                brushHeader,
                pen,
                new XRect(margenX, y, anchoTotal, 18),
                "Información de beneficiarios");
            y += 18;

            double[] colsBenef = { 110, 110, 110, 110, 112 }; // suma 552

            DibujarHeaderTabla(
                gfx1, tf1, pen, fontTablaHeader, x1, y, colsBenef,
                new string[] { "Tipo Documento", "N° Documento", "Nombres/Apellidos", "Parentesco", "Porcentaje" });
            y += 18;

            gfx1.DrawRectangle(pen, x1, y, colsBenef.Sum(), 14);
            DibujarTextoAjustado(
                gfx1,
                tf1,
                ObtenerValorOPlaceholder(row, "Beneficiarios"),
                fontMini,
                XBrushes.Black,
                new XRect(x1 + 3, y + 2, colsBenef.Sum() - 6, 10)
            );

            DibujarPieFormulario(gfx1, tf1, fontMini, imagenes, row, page1.Height);

            // =========================
            // PAGINA 2
            // =========================
            PdfPage page2 = document.AddPage();
            page2.Size = PdfSharp.PageSize.Letter;
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);
            XTextFormatter tf2 = new XTextFormatter(gfx2);

            DibujarEncabezadoFormulario(
                gfx2,
                tf2,
                fontTitulo,
                fontMini,
                fontValorBold,
                imagenes,
                row,
                page2.Width
            );

            double y2 = 128;

            // Destinatarios
            DibujarTituloSeccion(
                gfx2,
                tf2,
                fontSeccion,
                brushHeader,
                pen,
                new XRect(margenX, y2, anchoTotal, 18),
                "Información de destinatarios de pagos");
            y2 += 18;

            double[] colsDest = { 105, 75, 80, 105, 80, 55, 52 }; // suma 552

            DibujarHeaderTabla(
                gfx2, tf2, pen, fontTablaHeader, margenX, y2, colsDest,
                new string[] { "Nombre", "Tipo Documento", "N° Documento", "Banco", "Cheque", "Tipo Cuenta", "N° Cuenta" });
            y2 += 18;

            double rowDestH = 30;
            double xDest = margenX;

            string[] valoresDest =
            {
        ObtenerValorOPlaceholder(row, "DestinatarioNombre"),
        ObtenerValorOPlaceholder(row, "DestinatarioTipoDocumento"),
        ObtenerValorOPlaceholder(row, "DestinatarioNumeroDocumento"),
        ObtenerValorOPlaceholder(row, "DestinatarioBanco"),
        ObtenerValorOPlaceholder(row, "DestinatarioCheque"),
        ObtenerValorOPlaceholder(row, "DestinatarioTipoCuenta"),
        ObtenerValorOPlaceholder(row, "DestinatarioNumeroCuenta")
    };

            for (int i = 0; i < colsDest.Length; i++)
            {
                gfx2.DrawRectangle(pen, xDest, y2, colsDest[i], rowDestH);

                DibujarTextoAjustado(
                    gfx2,
                    tf2,
                    valoresDest[i],
                    fontMini,
                    XBrushes.Black,
                    new XRect(xDest + 2, y2 + 8, colsDest[i] - 4, rowDestH - 10)
                );

                xDest += colsDest[i];
            }

            y2 += rowDestH + 20;

            // Declaraciones
            DibujarTituloSeccion(
                gfx2,
                tf2,
                fontSeccion,
                brushHeader,
                pen,
                new XRect(margenX, y2, anchoTotal, 18),
                "Declaraciones y autorizaciones");
            y2 += 18;

            DibujarFila2Columnas(
                gfx2, tf2, pen, fontLabel, fontValor,
                margenX, y2, new double[] { 276, 276 }, rowH, // suma 552
                "Notificación por email", ObtenerValorOPlaceholder(row, "NotificacionEmail"),
                "Notificación Dirección Correspondencia", ObtenerValorOPlaceholder(row, "NotificacionDireccionCorrespondencia"));
            y2 += rowH;

            double altoDeclaracion = 85;
            gfx2.DrawRectangle(pen, margenX, y2, anchoTotal, altoDeclaracion);

            string textoDeclaracion =
                "Bajo la gravedad de juramento manifiesto que todos los datos aquí consignados son ciertos y que la información que contienen los documentos que adjunto es veraz, por lo que asumo plena responsabilidad sobre lo manifestado. " +
                "Asimismo, autorizo a Caja Honor para que verifique sin limitación alguna la información suministrada y valide la correcta destinación de los recursos correspondientes a las cesantías y los aportes solicitados. " +
                "Autorizo a Caja Honor para que efectúe consultas en las diferentes centrales de riesgo, rectifique, actualice y almacene la información suministrada en las correspondientes bases de datos o archivos según lo previsto en la Ley.";

            tf2.DrawString(
                textoDeclaracion,
                fontMini,
                XBrushes.Black,
                new XRect(margenX + 4, y2 + 4, anchoTotal - 8, altoDeclaracion - 8)
            );

            y2 += altoDeclaracion;

            var observaciones = ObtenerValorOPlaceholder(row, "Observaciones");

            
            double altoObservaciones = 50;

            gfx2.DrawRectangle(pen, margenX, y2, anchoTotal, altoObservaciones);

            
            gfx2.DrawString(
                "Observaciones",
                fontLabel,
                XBrushes.Black,
                new XRect(margenX + 3, y2 + 1, 100, 8),
                XStringFormats.TopLeft);

            tf2.DrawString(
                observaciones,
                fontMini,
                XBrushes.Black,
                new XRect(margenX + 3, y2 + 10, anchoTotal - 6, altoObservaciones - 12)
            );

            y2 += altoObservaciones + 10;

            string radicado = ObtenerValorOPlaceholder(row, "Radicado");
            string fechaImpresion = ObtenerValorOPlaceholder(row, "FechaImpresion");

            gfx2.DrawString(
                $"Radicado: {radicado}    fecha de impresión: {fechaImpresion}",
                fontValor,
                XBrushes.Black,
                new XRect(margenX, y2, anchoTotal, 20),
                XStringFormats.TopLeft);

            document.Save(stream, false);
            stream.Position = 0;
            return stream;
        }
        private static void DibujarEncabezadoFormulario(
    XGraphics gfx,
    XTextFormatter tf,
    XFont fontTitulo,
    XFont fontMini,
    XFont fontValorBold,
    string[] imagenes,
    DataRow row,
    double pageWidth)
        {
            if (imagenes != null && imagenes.Length > 0 && File.Exists(imagenes[0]))
            {
                XImage image = XImage.FromFile(imagenes[0]);
                gfx.DrawImage(image, 30, 18, 540, 45);
            }

            // Se baja el título para que no invada los logos
            gfx.DrawString(
                "Formulario Único de Pago",
                fontTitulo,
                XBrushes.Black,
                new XRect(0, 52, pageWidth, 20),
                XStringFormats.TopCenter);

            gfx.DrawString(
                "Radicado",
                fontMini,
                XBrushes.Black,
                new XRect(30, 78, 80, 10),
                XStringFormats.TopLeft);

            gfx.DrawString(
                ObtenerValorOPlaceholder(row, "Radicado"),
                fontValorBold,
                XBrushes.Black,
                new XRect(30, 90, 120, 14),
                XStringFormats.TopLeft);

            gfx.DrawString(
                "Punto de Radicación",
                fontMini,
                XBrushes.Black,
                new XRect(160, 78, 100, 10),
                XStringFormats.TopLeft);

            gfx.DrawString(
                ObtenerValorOPlaceholder(row, "PuntoRadicacion"),
                fontValorBold,
                XBrushes.Black,
                new XRect(160, 90, 120, 14),
                XStringFormats.TopLeft);

            gfx.DrawString(
                "Fecha Radicación dd-mm-aaaa",
                fontMini,
                XBrushes.Black,
                new XRect(300, 78, 130, 10),
                XStringFormats.TopLeft);

            gfx.DrawString(
                ObtenerValorOPlaceholder(row, "FechaRadicacion"),
                fontValorBold,
                XBrushes.Black,
                new XRect(300, 90, 140, 14),
                XStringFormats.TopLeft);

            gfx.DrawString(
                "Folios",
                fontMini,
                XBrushes.Black,
                new XRect(470, 78, 40, 10),
                XStringFormats.TopLeft);

            gfx.DrawString(
                ObtenerValorOPlaceholder(row, "Folios"),
                fontValorBold,
                XBrushes.Black,
                new XRect(470, 90, 70, 14),
                XStringFormats.TopLeft);
        }

        private static void DibujarPieFormulario(
            XGraphics gfx,
            XTextFormatter tf,
            XFont fontMini,
            string[] imagenes,
            DataRow row,
            double pageHeight)
        {
            double yFooter = pageHeight - 65;

            if (imagenes != null && imagenes.Length > 1 && File.Exists(imagenes[1]))
            {
                XImage imageFooter = XImage.FromFile(imagenes[1]);
                gfx.DrawImage(imageFooter, 35, yFooter, 540, 45);
            }
        }

        private static void DibujarTituloSeccion(
            XGraphics gfx,
            XTextFormatter tf,
            XFont font,
            XBrush brush,
            XPen pen,
            XRect rect,
            string titulo)
        {
            gfx.DrawRectangle(brush, rect);
            gfx.DrawRectangle(pen, rect);

            tf.DrawString(
                titulo,
                font,
                XBrushes.White,
                new XRect(rect.X + 6, rect.Y + 3, rect.Width - 12, rect.Height - 6)
            );
        }

        private static void DibujarFila2Columnas(
            XGraphics gfx, XTextFormatter tf, XPen pen, XFont fontLabel, XFont fontValor,
            double x, double y, double[] widths, double h,
            string label1, string value1,
            string label2, string value2)
        {
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x, y, widths[0], h, label1, value1);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0], y, widths[1], h, label2, value2);
        }

        private static void DibujarFila3Columnas(
            XGraphics gfx, XTextFormatter tf, XPen pen, XFont fontLabel, XFont fontValor,
            double x, double y, double[] widths, double h,
            string label1, string value1,
            string label2, string value2,
            string label3, string value3)
        {
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x, y, widths[0], h, label1, value1);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0], y, widths[1], h, label2, value2);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0] + widths[1], y, widths[2], h, label3, value3);
        }

        private static void DibujarFila4Columnas(
            XGraphics gfx, XTextFormatter tf, XPen pen, XFont fontLabel, XFont fontValor,
            double x, double y, double[] widths, double h,
            string label1, string value1,
            string label2, string value2,
            string label3, string value3,
            string label4, string value4)
        {
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x, y, widths[0], h, label1, value1);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0], y, widths[1], h, label2, value2);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0] + widths[1], y, widths[2], h, label3, value3);
            DibujarCelda(gfx, tf, pen, fontLabel, fontValor, x + widths[0] + widths[1] + widths[2], y, widths[3], h, label4, value4);
        }

        private static void DibujarCelda(
            XGraphics gfx, XTextFormatter tf, XPen pen, XFont fontLabel, XFont fontValor,
            double x, double y, double w, double h,
            string label, string value)
        {
            gfx.DrawRectangle(pen, x, y, w, h);

            gfx.DrawString(
                label,
                fontLabel,
                XBrushes.Black,
                new XRect(x + 2, y + 1, w - 4, 8),
                XStringFormats.TopLeft);

            DibujarTextoAjustado(
                gfx,
                tf,
                NormalizarTextoPdf(value),
                fontValor,
                XBrushes.Black,
                new XRect(x + 2, y + 10, w - 4, h - 11)
            );
        }

        private static void DibujarHeaderTabla(
            XGraphics gfx, XTextFormatter tf, XPen pen, XFont font,
            double x, double y, double[] widths, string[] headers)
        {
            double currentX = x;

            for (int i = 0; i < widths.Length; i++)
            {
                gfx.DrawRectangle(XBrushes.WhiteSmoke, currentX, y, widths[i], 18);
                gfx.DrawRectangle(pen, currentX, y, widths[i], 18);

                DibujarTextoAjustado(
                    gfx,
                    tf,
                    headers[i],
                    font,
                    XBrushes.Black,
                    new XRect(currentX + 2, y + 3, widths[i] - 4, 12)
                );

                currentX += widths[i];
            }
        }

        private static void DibujarTextoAjustado(
            XGraphics gfx,
            XTextFormatter tf,
            string texto,
            XFont fuente,
            XBrush brush,
            XRect rect)
        {
            tf.DrawString(
                string.IsNullOrWhiteSpace(texto) ? "N/A" : texto,
                fuente,
                brush,
                rect
            );
        }

        private static string ObtenerValorOPlaceholder(DataRow row, string columnName)
        {
            if (row == null)
                return "N/A";

            if (!row.Table.Columns.Contains(columnName))
                return "N/A";

            object valor = row[columnName];

            if (valor == null || valor == DBNull.Value)
                return "N/A";

            string texto = valor.ToString()?.Trim();
            return string.IsNullOrWhiteSpace(texto) ? "N/A" : texto;
        }

        private static string NormalizarTextoPdf(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim();
        }
        public MemoryStream ReporteRespuestaEmbargo(DataTable data, string[] imagenes)
    {
            MemoryStream stream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);
            XRect rect = new XRect(0, 0, 0, 0);

            string text = "Bogotá " + DateTime.Parse(data.Rows[0].ItemArray[11].ToString()).ToString("dd/MM/yyyy");
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 100, 500, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "AL CONTESTAR CITE " + data.Rows[0].ItemArray[4].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 120, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fecha: " + DateTime.Parse(data.Rows[0].ItemArray[11].ToString()).ToString("dd/MM/yyyy");
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 140, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Área: GRUPO ASUNTOS JURÍDICOS DE OPERACIONES";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 160, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Señor(a) Juez";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 180, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[0].ToString().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 200, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[1].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 220, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Asunto: Respuesta al radicado " + data.Rows[0].ItemArray[2].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 260, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Tipo de proceso";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 280, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[3].ToString().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 280, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "No. Proceso";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 300, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[4].ToString().ToUpper(); ;
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 300, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Demandante";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 320, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[5].ToString().ToUpper().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 320, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cédula demandante";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 340, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[6].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 340, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Demandado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 360, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[7].ToString().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 360, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cédula demandado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 380, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[8].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 380, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Observaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[15].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(230, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "En atención al oficio No. " + data.Rows[0].ItemArray[9].ToString() + " emitido por su despacho el " + DateTime.Parse(data.Rows[0].ItemArray[11].ToString()).ToString("dd/MM/yyyy") + " y allegado a la Caja Promotora de Vivienda Militar y de Policía " +
                "el " + DateTime.Parse(data.Rows[0].ItemArray[10].ToString()).ToString("dd/MM/yyyy") + " mediante el radicado del asunto, a través del cual ordenó el embargo de: " + data.Rows[0].ItemArray[14].ToString() + " del(a) señor(a) " +
                data.Rows[0].ItemArray[7].ToString() + ", de manera atenta, se informa que se acató la orden y se procedió registrando la medida cautelar en la cuenta individual del afiliado " +
                "sobre: " + data.Rows[0].ItemArray[14].ToString() + " a favor del proceso No. " + data.Rows[0].ItemArray[4].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 440, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Por otra parte, es importante señalar que esta Entidad no es un ente nominador, no realiza " +
                "pagos periódicos, solamente administra las cesantías del demandado, motivo por el cual,únicamente puede realizar " +
                "depósitos judiciales o pagos tipo 1 de acuerdo con las ordenes que emita su despacho.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 520, 500, 480);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Por último, se le informa al señor Juez, que esta dependencia tiene habilitado el buzón de correo electrónico " +
                "notificacion.embargo @cajahonor.gov.co, para el recibo de comunicaciones de los despachos " +
                "judiciales dentro de los procesos en los que nuestros afiliados son parte y en los cuales se " +
                "ordenan medidas cautelares que afectan las cuentas individuales de los mismos. " +
                "Igualmente, pueden comunicarse al teléfono(601) 7468091 en la ciudad de Bogotá";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 570, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 650, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg. XIMENA SUÁREZ HERNÁNDEZ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 670, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Líder Grupo Asuntos Jurídicos de Operaciones";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 690, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Abg.Lyna García Silva";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 715, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Profesional Universitario- GAJUO";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 725, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMini, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 735, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMiniNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;

        }

        public MemoryStream ReporteRespuestaSolicitud(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);
            XRect rect = new XRect(0, 0, 0, 0);

            string text = "Bogotá " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 100, 500, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text =  data.Rows[0].ItemArray[50].ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 120, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Fecha: " + DateTime.Now.ToString();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 140, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Señor(a)";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 180, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text ="Nombre del afiliado: " + data.Rows[0].ItemArray[44].ToString().ToUpper();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 200, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text ="Correo: " + data.Rows[0].ItemArray[20].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 220, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Direccion: " + data.Rows[0].ItemArray[14].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 240, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Asunto: " + data.Rows[0].ItemArray[28].ToString().ToLower(); ;
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 260, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text =" De manera cordial se informa que su solicitud registrada bajo el número de radicado: " + data.Rows[0].ItemArray[50].ToString() +
                " y fecha de tramite " +  DateTime.Now.ToString() +  " una vez efectuada la revisión presentó las siguientes inconsistencias: ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 300, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[21].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 340, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);


            text =  data.Rows[0].ItemArray[30].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 510, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

           
            text = "Se le invita a seguirnos a través de las redes sociales Facebook, Twitter, YouTube e Instagram. " +
                "En caso tener inquietudes adicionales puede contactarnos a través de " + 
                "· Línea telefónica en Bogotá 7557070, a nivel nacional 018000185570(gratuita). " +
                "· Correo electrónico contactenos@cajahonor.gov.co. " +
                "· Asesor en línea por medio Chat Bot “Patria” de la página web.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 560, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Adicionalmente, Caja Honor cuenta con la Defensoría del Consumidor Financiero, a cargo del Dr. Pablo Tomas Silva Mariño – Calle 13 No. 4 – 90, " +
                " Oficina 309 de Bogotá D.C., teléfono 2823570, correo: ptsilvadefensor@hotmail.com.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 610, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 660, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "MAX BELTRÁN";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 670, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Líder de grupo";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 690, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Proyectó y elaboró:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 705, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = data.Rows[0].ItemArray[6].ToString().ToLower();
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 715, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Profesional Universitario - GCICE_P";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 725, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 735, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;

        }

        /// <summary>
        /// ReporteCertificadoHaberes
        /// </summary>
        /// <param name="data"></param>
        /// <param name="imagenes"></param>
        /// <returns></returns>
        public MemoryStream ReporteCertificadoHaberes(DataTable data, string[] imagenes)
        {
            MemoryStream stream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromFile(imagenes[0]);
            gfx.DrawImage(image, 50, 50, 500, 40);
            XTextFormatter tf = new XTextFormatter(gfx);

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Regular, options);
            XFont fontNegrita = new XFont("Arial", 10, XFontStyle.Bold, options);
            XFont fontMini = new XFont("Arial", 6, XFontStyle.Regular, options);
            XFont fontMiniNegrita = new XFont("Arial", 8, XFontStyle.Bold, options);
            XRect rect = new XRect(0, 0, 0, 0);

            string text = "Bogotá " + DateTime.Now.ToString("dd/MM/yyyy");
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 100, 500, 100);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "LA JEFE DEL AREA DE FINANZAS";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 140, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "CERTIFICA QUE:";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 160, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "El(la) Señor(a) " + data.Rows[0].ItemArray[5].ToString().ToUpper() + " identificado(a) con " + data.Rows[0].ItemArray[3].ToString().ToUpper()  + "No." + data.Rows[0].ItemArray[4].ToString() +
                "tiene una obligación de crédito Leasing Habitacional con la Caja Promotora de Vivienda Militar y de Policia por " +
                "concepto de cánones de arrendamiento, iniciando el pago de las cuotas de amortización a partir del 01/01/2022";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 220, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "A la fecha presenta un saldo en la obligacion de Leasing Habitacional de $ 5000000";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 280, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Se expide la presente constancia el " + DateTime.Now.ToString("dd/MM/yyyy") + " en Bogotá," +
                "a solicitud del interesado";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 340, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: El pago de dicha obligación, la puede consignar en la cuenta de ahorros No 220-080-17999-7. Del Banco Popular.";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 370, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Una vez realizada la consignación enviar por fax al número 220720/20 con los siguientes datos del deudor: nombre y apellidos, número de cedula, dirección y telefono. ";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 410, 500, 500);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Cordialmente,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 480, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "CPT ANA I. OLARTE ESTUPIÑAN";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 580, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, fontNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Jefe Área de Finanzas,";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 600, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            text = "Nota: Documento original firmado digitalmente";
            tf = new XTextFormatter(gfx);
            rect = new XRect(50, 735, 500, 0);
            gfx.DrawRectangle(XBrushes.White, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, fontMiniNegrita, XBrushes.Black, rect, XStringFormats.TopLeft);

            XImage image3 = XImage.FromFile(imagenes[1]);
            gfx.DrawImage(image3, 50, 760, 550, 65);
            document.Save(stream);

            return stream;

        }
    }
}
