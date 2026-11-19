using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    public abstract class ProviderDocumentos
    {
        public static async Task<FileResult> CrearDocumento(string HTML,
                                                     string CSS,
                                                     TipoDocumentoArchivo tipoDocumento,
                                                     string nombreArchivo,
                                                     Dictionary<string, string> datosReemplazo = null,
                                                     string contraseña = null,
                                                     string header = null,
                                                     string footer = null,
                                                     bool margins = false,
                                                     int marginleft=10,
                                                     int marginRigth = 10
                                                     )
        {

            var documentoHtml = LlenarDocumentoParametros(HTML, datosReemplazo);
            var documento = CrearTipoDocumento(documentoHtml, tipoDocumento, CSS);
            return await ExportaArchivo(documento, tipoDocumento, nombreArchivo, contraseña, header, footer,marginleft,marginRigth, margins);
        }

        public static async Task<string> CrearDocumentoBase64(string HTML,
                                                      string CSS,
                                                      TipoDocumentoArchivo tipoDocumento,
                                                      string nombreArchivo,
                                                      Dictionary<string, string> datosReemplazo = null,
                                                      string contraseña = null,
                                                      string header = null,
                                                      string footer = null,
                                                      bool margins = false,
                                                      int marginleft = 10,
                                                      int marginRigth = 10)
        {
            // 1. Reemplaza parámetros dentro del HTML
            var documentoHtml = LlenarDocumentoParametros(HTML, datosReemplazo);

            // 2. Genera el documento (PDF, Word, Excel, etc.)
            var documento = CrearTipoDocumento(documentoHtml, tipoDocumento, CSS);

            // 3. Exporta el archivo en un stream (sin FileResult)
            var fileBytes = await ExportaArchivoBytes(documento, tipoDocumento, nombreArchivo, contraseña, header, footer, marginleft, marginRigth, margins);

            // 4. Convierte a Base64
            return Convert.ToBase64String(fileBytes);
        }

        private static async Task<byte[]> ExportaArchivoBytes(string contenido,
                                                      TipoDocumentoArchivo tipoDocumentoArchivo,
                                                      string nombreArchivo,
                                                      string passwordPdf,
                                                      string header,
                                                      string footer,
                                                      int marginLeft = 10,
                                                      int marginRight = 10,
                                                      bool margins = false)
{
    byte[] buffer = Encoding.UTF8.GetBytes(contenido);
    MemoryStream ms = new(buffer);

    // SI ES PDF
    if (tipoDocumentoArchivo == TipoDocumentoArchivo.PDF)
    {
        HtmlToPdf converter = new();
        converter.Options.PdfPageSize = PdfPageSize.Letter;
        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

        // Header
        if (!string.IsNullOrEmpty(header))
        {
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 85;
            PdfHtmlSection headerHtml = new(header, "");
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);
        }

        // Footer
        if (!string.IsNullOrEmpty(footer))
        {
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 50;

            PdfHtmlSection footerHtml = new(footer, "")
            {
                AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            };

            converter.Footer.Add(footerHtml);
        }

        // Margins
        converter.Options.MarginTop = margins ? 20 : 40;
        converter.Options.MarginLeft = marginLeft;
        converter.Options.MarginRight = marginRight;
        converter.Options.MarginBottom = 40;
        converter.Options.PdfStandard = PdfStandard.PdfA;

        // Generar PDF
        PdfDocument doc = converter.ConvertHtmlString(contenido, "");

        if (!string.IsNullOrWhiteSpace(passwordPdf))
            doc.Security.UserPassword = passwordPdf;

        // Retornar bytes del PDF
        return doc.Save();
    }

    // SI NO ES PDF → texto plano u otro tipo
    using (var msOut = new MemoryStream())
    {
        await ms.CopyToAsync(msOut);
        return msOut.ToArray();
    }
}


        /// <summary>
        /// CrearDocumentoPreaprobados
        /// </summary>
        /// <param name="HTML"></param>
        /// <param name="CSS"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="datosReemplazo"></param>
        /// <param name="credential"></param>
        /// <param name="margins"></param>
        /// <returns></returns>
        public static async Task<FileResult> CrearDocumentos(string HTML,
                                                     string CSS,
                                                     TipoDocumentoArchivo tipoDocumento,
                                                     string nombreArchivo,
                                                     string header,
                                                     string footer,
                                                     Dictionary<string, string> datosReemplazo = null,
                                                     string credential = null,
                                                     bool margins = false)
        {
            var documentoHtml = LlenarDocumentoParametros(HTML, datosReemplazo);
            var documento = CrearTipoDocumento(documentoHtml, tipoDocumento, CSS);
            return await ExportaArchivo(documento, tipoDocumento, nombreArchivo, credential,header,footer,0,30, margins);
        }

        private static string LlenarDocumentoParametros(string contenidoHtml, Dictionary<string, string> contenido)
        {
            if (contenido != null)
            {
                foreach (var i in contenido)
                {
                    contenidoHtml = contenidoHtml.Replace(i.Key, i.Value);
                }
            }

            return contenidoHtml;
        }

        /// <summary>
        /// Crear tipo de doucmentos 
        /// </summary>
        /// <param name="bodyHtml">Contendio html de cuerpo</param>
        /// <param name="tipoDocumentoArchivo">Tipo de documento a crear</param>
        /// <param name="cssHtml">Estilos css</param>
        /// <returns></returns>
        private static string CrearTipoDocumento(string bodyHtml, TipoDocumentoArchivo tipoDocumentoArchivo, string cssHtml = null, bool margins = false)
        {
            StringBuilder constructorDocumento = new();
            constructorDocumento.Append(ProviderDocumentosConstants.XML_OFFICE);
            switch (tipoDocumentoArchivo)
            {
                case TipoDocumentoArchivo.DOC:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_WORD);
                    break;
                case TipoDocumentoArchivo.DOCX:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_WORDX);
                    break;
                case TipoDocumentoArchivo.XSL:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_XLS);
                    break;
                case TipoDocumentoArchivo.XSLX:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_XLSX);
                    break;
                case TipoDocumentoArchivo.PDF:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_PDF);
                    break;
                case TipoDocumentoArchivo.XPS:
                    constructorDocumento.Append(ProviderDocumentosConstants.XML_XPS);
                    break;
                default:
                    break;
            }

            constructorDocumento.Append(ProviderDocumentosConstants.AddHtmlAndCss(bodyHtml, cssHtml, margins));
            var documento = constructorDocumento.ToString();
            return documento;
        }

        /// <summary>
        /// Exportar archivos creados
        /// </summary>
        /// <param name="contendito">Contenido html</param>
        /// <param name="tipoDocumentoArchivo">Tipo de documento creado</param>
        /// <param name="nombreArchivo">Nombre archivo</param>
        /// <returns>Archivo creado</returns>
        private static async Task<FileResult> ExportaArchivo(string contendito, TipoDocumentoArchivo tipoDocumentoArchivo, string nombreArchivo, string passwordPdf, string header, string footer, int marginLeft = 10, int marginRight = 10, bool margins = false)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(contendito);
            MemoryStream ms = new(buffer);
            var fileName = nombreArchivo ?? Guid.NewGuid().ToString();
            fileName = fileName + '.' + HelpersEnums.GetEnumDescription(tipoDocumentoArchivo);

            var archivo = new FileStreamResult(ms, "text/plain")
            {
                FileDownloadName = fileName
            };

            if (tipoDocumentoArchivo == TipoDocumentoArchivo.PDF)
            {
                HtmlToPdf converter = new();
                converter.Options.PdfPageSize = PdfPageSize.Letter;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                // Reemplaza el marcador de posición en el header con la fecha actual
                if (!string.IsNullOrEmpty(header))
                {
                    converter.Options.DisplayHeader = true;
                    converter.Header.Height = 85;
                    PdfHtmlSection headerHtml = new(header, /*basePath*/"");
                    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Header.Add(headerHtml);
                }

                if (!string.IsNullOrEmpty(footer))
                {
                    converter.Options.DisplayFooter = true;
                    converter.Footer.Height = 50;
                    PdfHtmlSection footerHtml = new(footer, /*basePath*/"")
                    {
                        AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
                    };
                    converter.Footer.Add(footerHtml);
                }

                if(margins)
                {
                    converter.Options.MarginTop = 20;
                }
                else
                {
                    converter.Options.MarginTop = 40;
                }

                converter.Options.MarginLeft = marginLeft;
                converter.Options.MarginRight = marginRight;
                converter.Options.MarginBottom = 40;
                converter.Options.PdfStandard = PdfStandard.PdfA;

                PdfDocument doc = converter.ConvertHtmlString(contendito, /*basePath*/"");
                if (!string.IsNullOrWhiteSpace(passwordPdf))
                {
                    doc.Security.UserPassword = passwordPdf;
                }

                byte[] bufferPdf = doc.Save();
                MemoryStream msPdf = new(bufferPdf);
                var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                {
                    FileDownloadName = fileName
                };
                var dd = msPdf.ToArray();
                var ss = Convert.ToBase64String(dd);
                await archivoPdf.FileStream.FlushAsync();
                return archivoPdf;
            }

            await archivo.FileStream.FlushAsync();
            return archivo;
        }


        /// <summary>
        /// ExportaArchivo
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="tipoDocumentoArchivo"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="margins"></param>
        /// <returns></returns>
        private static async Task<FileResult> ExportaArchivo(string contenido, TipoDocumentoArchivo tipoDocumentoArchivo, string nombreArchivo, string passwordPdf, bool margins = false)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(contenido);
            MemoryStream ms = new(buffer);
            var fileName = nombreArchivo ?? Guid.NewGuid().ToString();
            fileName = fileName + '.' + HelpersEnums.GetEnumDescription(tipoDocumentoArchivo);
            var archivo = new FileStreamResult(ms, "text/plain")
            {
                FileDownloadName = fileName
            };

            if (tipoDocumentoArchivo == TipoDocumentoArchivo.PDF)
            {
                HtmlToPdf converter = new();
                converter.Options.PdfPageSize = PdfPageSize.Letter;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;                
                converter.Options.PdfStandard = PdfStandard.PdfA;
                PdfDocument doc = converter.ConvertHtmlString(contenido, /*basePath*/"");
                
                if (!string.IsNullOrWhiteSpace(passwordPdf)) doc.Security.UserPassword = passwordPdf;
                byte[] bufferPdf = doc.Save();
                MemoryStream msPdf = new(bufferPdf);
                var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                {
                    FileDownloadName = fileName
                };
                await archivoPdf.FileStream.FlushAsync();
                return archivoPdf;
            }

            await archivo.FileStream.FlushAsync();
            return archivo;
        }


        
        public static FileContentResult ExportaArchivoExcel(MemoryStream stream, TipoDocumentoArchivo tipoDocumentoArchivo)
        {
            
            var fileName = Guid.NewGuid().ToString();
            fileName = fileName + '.' + HelpersEnums.GetEnumDescription(tipoDocumentoArchivo);
            var archivo=new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };

            //await archivo.FileStream.FlushAsync();
            return archivo;
        }



    }
}
