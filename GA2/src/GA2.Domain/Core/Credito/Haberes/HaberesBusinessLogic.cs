using Azure.Storage.Blobs;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using GA2.Application.Dto;
using GA2.Application.Dto.Haberes;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.Servicios.Haberes;
using GA2.Domain.Entities;
using GA2.Domain.Entities.ServiciosDinamicosEnt;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using GA2.Transversals.Commons;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GA2.Domain.Core
{
    public class HaberesBusinessLogic : IHaberesBusinessLogic
    {
        private readonly IHaberesService _haberes;
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IBlobStorageGenericRepository _blobStorageRepository;
        private readonly IIntegracionesGA2HaberesBusinessLogic _integracionesBusinessLogic;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HaberesBusinessLogic> _logger;
        private readonly ICuentasClientesRepository _cuentasClientesRepository;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly ICreditoRepository _creditoRepository;
        private readonly LocalBlogStorageConfig _config;
        private readonly IServiciosDinamicosRepository _serviciosDinamicosRepository; 








        public HaberesBusinessLogic(IHaberesService haberes,

                                    ICreditoBusinessLogic creditoBusinessLogic,
                                    IClientRequestProvider iClientRequestProvider,
                                    IBlobStorageGenericRepository blobStorageRepository,
                                    IIntegracionesGA2HaberesBusinessLogic integracionesBusinessLogic,
                                    ILogger<HaberesBusinessLogic> logger,
                                    IWebHostEnvironment webHostEnvironment,
                                    ICuentasClientesRepository cuentasClientesRepository,
                                    ICatalogosRepository catalogsRepository,
                                    IOptions<BlobStorageOptions> options,
                                    ICreditoRepository creditoRepository,
                                    IOptions<LocalBlogStorageConfig> config, 
                                    IServiciosDinamicosRepository serviciosDinamicosRepository)
        {
            _haberes = haberes;
            _creditoBusinessLogic = creditoBusinessLogic;
            _iClientRequestProvider = iClientRequestProvider;
            _blobStorageRepository = blobStorageRepository;
            _integracionesBusinessLogic = integracionesBusinessLogic;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webHostEnvironment = webHostEnvironment;
            _cuentasClientesRepository = cuentasClientesRepository;
            _catalogsRepository = catalogsRepository;
            _options = options;
            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
            _config = config.Value;
            _serviciosDinamicosRepository = serviciosDinamicosRepository; 
        }


        public async Task<FileResult> GenerarReporteHaberes(HaberesRequestDto haberesRequest)
        {
            var cuenta = new CuentaDto();
            dynamic resultBase64String = default;
            cuenta = (await this._creditoBusinessLogic.ObtenerCuentaById(haberesRequest.ClienteIdentificacion)).Data;

            if (cuenta != null)
            {
                haberesRequest.CuentaId = cuenta.IdCuentaIntegracion;
            }
            else
            {
                throw new Exception("La cuenta no pose cuenta integracion o no se encuentra");
            }

#if DEBUG
                _logger.LogInformation("Environment development");
                resultBase64String = await _integracionesBusinessLogic.GenerarReporteHaberes(haberesRequest);
                
#else
            _logger.LogInformation("Environment Produccion");
            resultBase64String = await _haberes.GenerarReporteHaberes(new()
            {
                accountId = cuenta.IdCuentaIntegracion,
                customerIdentification = haberesRequest.ClienteIdentificacion,
                sReporte = haberesRequest.Reporte
            });

#endif

            return await GuardarReporteBlobStorage(haberesRequest, resultBase64String.Data);
        }

        public async Task<FileResult> GenerarReporteHaberesV2(HaberesRequestDto haberesRequest)
        {
            var cuenta = new CuentaDto();
            cuenta = (await this._creditoBusinessLogic.ObtenerCuentaById(haberesRequest.ClienteIdentificacion)).Data;

            if (cuenta != null)
            {
                haberesRequest.CuentaId = cuenta.IdCuentaIntegracion;
            }
            else
            {
                throw new Exception("La cuenta no pose cuenta integracion o no se encuentra");
            }
            var resultBase64String = await _haberes.GenerarReporteHaberesV2(new()
            {
                accountId = haberesRequest.CuentaId,
                customerIdentification = haberesRequest.ClienteIdentificacion,
                sReporte = haberesRequest.Reporte,
                month = haberesRequest.Mes,
                year = haberesRequest.Anio
            });
          
            return await GuardarReporteBlobStorage(haberesRequest, resultBase64String.Data);
        }
        public async Task<FileResult> ReporteHaberes(ReportHaberesRequestDto reporteHaberes)
        {
            try
            {


                var cuenta = new CuentaDto();
                var ObtenerSolicitudCredito = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, reporteHaberes.customerIdentification);

                var clienteCuentasResponse = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, reporteHaberes.customerIdentification);
                var clienteCuentas = clienteCuentasResponse.Data;

                if (clienteCuentas != null && clienteCuentasResponse.IsSuccess)
                {
                    var conceptosCuentasHV = clienteCuentas.Cuentas;
                    var ListaCuentasFiltradas = conceptosCuentasHV.Where(x => x.EstadoCuenta == "VIGENTE").ToList();
                    var gradoDescrip = clienteCuentas.IdGrado;


                    IEnumerable<Entities.Grado> grados = await _catalogsRepository.ObtenerGrados();
                    var gradosFiltrados = grados.Where(g => g.GRD_ID == gradoDescrip).Select(x => x.GRD_DESCRIPCION);
                    var nombreGradoFiltrado = gradosFiltrados.FirstOrDefault();

                    cuenta = (await this._creditoBusinessLogic.ObtenerCuentaById(reporteHaberes.customerIdentification)).Data;
                    var convertId = int.Parse(reporteHaberes.customerIdentification);
                    var cliente = (await _iClientRequestProvider.ObtenerClienteReporte(reporteHaberes.customerIdentification)).Data;

                    if (cuenta != null)
                    {
                        reporteHaberes.accountId = cuenta.IdCuentaIntegracion;
                    }
                    else
                    {
                        throw new Exception("La cuenta no pose cuenta integracion o no se encuentra");
                    }

                    var ResponseHaberes = await _haberes.ReporteHaberes(new()
                    {
                        accountId = reporteHaberes.accountId,
                        customerIdentification = reporteHaberes.customerIdentification,
                        sUserQuery = ""
                    });

                    if (ResponseHaberes.Data.ReporteHaberesResult.Nodes.Count > 1)
                    {
                        // Accede al elemento en la posición 1
                        var elemento = ResponseHaberes.Data.ReporteHaberesResult.Nodes[1].FirstNode;

                        XDocument xmlDoc = XDocument.Parse(elemento.ToString());

                        // Ahora puedes trabajar con el xmlDoc para extraerla información que necesitas
                        var array1 = xmlDoc.Descendants("CLI_SEGUNDO_NOMBRE").Select(x => x.Value).ToArray();
                        var array2 = xmlDoc.Descendants("CLI_NOMBRE_COMPLETO").Select(x => x.Value).ToArray();
                        var nombreCompleto = xmlDoc.Descendants("CLI_NOMBRE_COMPLETO").FirstOrDefault()?.Value;
                        var cedula = xmlDoc.Descendants("CLI_IDENTIFICACION").FirstOrDefault()?.Value;
                        var fuerza = xmlDoc.Descendants("UEJ_NOMBRE").FirstOrDefault()?.Value;
                        var gradosDes = nombreGradoFiltrado;
                        var tipoDocumento = xmlDoc.Descendants("TID_DESCRIPCION").FirstOrDefault()?.Value;
                        var categoria = xmlDoc.Descendants("CTG_DESCRIPCION").FirstOrDefault()?.Value;
                        var regimen = xmlDoc.Descendants("CLI_REGIMEN").FirstOrDefault()?.Value;
                        var fechaUltimaCuota = ((DateTime?)xmlDoc.Descendants("APC_FECHA_ULTIMA_CUOTA").FirstOrDefault());

                        var datosBasicos = new ReportDataSource("DatosBasicos", new[]
                        {
                             new {
                                 NOMBRES = nombreCompleto,
                                 IDENTIFICACION = cedula,
                                 FUERZA = fuerza,
                                 GRADO = nombreGradoFiltrado,
                                 CATEGORIA = categoria,
                                 FECHA_ALTA = cliente.FechaAlta,
                                 FECHA_PRIMER_APORTE = cliente.FechaInscripcion,
                                 FECHA_ULTIMO_APORTE = fechaUltimaCuota,
                                 TOTAL_CUOTAS_APORTADAS = clienteCuentasResponse.Data.NumeroCuotas,
                                 REGIMEN = regimen,
                                 ESTADO_CLIENTE = clienteCuentasResponse.Data.EstadoCliente, 
                                 ESTADO_CUENTA = clienteCuentasResponse.Data.Cuentas[0].EstadoCuenta
                             }
                         });

                        var conceptosReporte = new string[] {
                            "AHORROS OBLIGATORIOS", "AHORROS VOLUNTARIOS", "CESANTIAS",
                            "AHORROS RETROACTIVOS", "CESANTIAS RETROACTIVA", "INTERESES APORTES",
                            "INTERESES CESANTÍAS"
                        };

                        //var conceptos = xmlDoc.Elements("REPORTE_HABERES").Elements("REPORTE_HABERES")
                        //    .Where(
                        //        x => conceptosReporte.Contains(x.Element("CNC_DESCRIPCION")?.Value)
                        //    ).Select(x => new
                        //    {
                        //        CONCEPTO = x.Element("CNC_DESCRIPCION")?.Value,
                        //        EGRESOS =   0,
                        //        INGRESOS = (decimal?)x.Element("CCT_SALDO"),
                        //    }).ToList();

                        var conceptos = ListaCuentasFiltradas.SelectMany(x => x.Conceptos)
                            .GroupBy(x => x.ConceptoId).Select(x => x.FirstOrDefault());

                        //var bloqueos = ListaCuentasFiltradas.SelectMany(x => x.ConceptosEmbargosBloqueos);

                        var bloqueos = ListaCuentasFiltradas?
                        .SelectMany(x => x.ConceptosEmbargosBloqueos ?? new List<ConceptosSaldosBloqueosDto>())
                        ?? new List<ConceptosSaldosBloqueosDto>();

                        
                        // Asegura que datosBloqueo siempre tenga una lista válida

                        var movimientos = ListaCuentasFiltradas.SelectMany(x => x.Movimientos)
                                          .GroupBy(x => x.CNC_ID)
                                          .Select(x => new
                                          {
                                              conceptoId = x.Key,
                                              egresos = x.Where(x => x.CAT_TIPO_MOVIMIENTO == "D").Sum(x => x.MVT_VALOR),
                                              ingreso = x.Where(x => x.CAT_TIPO_MOVIMIENTO == "C").Sum(x => x.MVT_VALOR),
                                          });

                        var movimientoConcepto = from c in conceptos
                                                 join m in movimientos on c.ConceptoId equals m.conceptoId
                                                 select new
                                                 {
                                                     CONCEPTO = c.ConceptoDescripcion,
                                                     EGRESOS = m.egresos,
                                                     INGRESOS = m.ingreso,

                                                 };


                        var movimientoBloqueos = bloqueos.Select(m => new
                        {
                            CONCEPTO = m.Concepto,
                            VALOR = m.ValorAcumulado,
                            A_FAVOR_DE = m.Origen,
                            ESTADO = m.EstadoBloqueo,
                            FECHA = m.FechaBloqueo,
                        });

                        var CuentaIndividual = ListaCuentasFiltradas
                            .Where(x => x.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                            .ToList();

                        IEnumerable<object> movimientoCuentaIndividual = Enumerable.Empty<object>(); // Declare it outside the if blocks

                        if (CuentaIndividual.Any())
                        {
                            var movimientosCI = CuentaIndividual.First()
                                .Movimientos?.OrderBy(m => m.MVT_FECHA_MOVIMIENTO).Take(18);

                            if (movimientosCI != null)
                            {
                                IEnumerable<DescripcionConcepto> conceptosDescripcion = await this._cuentasClientesRepository.ObtenerConceptos();

                                movimientoCuentaIndividual = from m in movimientosCI
                                                             join c in conceptosDescripcion
                                                             on m.CNC_ID equals c.CNC_ID
                                                             select new
                                                             {
                                                                 TIPO_SOLICITUD = c.CNC_DESCRIPCION,
                                                                 VALOR = m.MVT_VALOR,
                                                                 DOCUMENTO = tipoDocumento,
                                                                 NRO_DOCUMENTO = cedula,
                                                                 FECHA = m.MVT_FECHA_PROCESO
                                                             };

                            }
                            else
                            {
                                movimientoCuentaIndividual = Enumerable.Empty<object>();
                            }
                        }
                        else
                        {
                            movimientoCuentaIndividual = Enumerable.Empty<object>();
                        }

                        var datosMovimientos = new ReportDataSource("DatosMovimientos", movimientoCuentaIndividual.ToList());
                        var datosCuotas = new ReportDataSource("DatosConceptos", movimientoConcepto.ToList());
                        var datosBloqueo = new ReportDataSource("DatosBloqueo", movimientoBloqueos.Any() ? movimientoBloqueos.ToList() : new List<object>());


                        var report = new LocalReport();
                        var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "Haberes", "ReporteHaberes.rdlc");
                        report.ReportPath = path;

                        var imgFirma = xmlDoc.Descendants("FIRMA_JEFE").FirstOrDefault()?.Value;
                        var jefeArea = xmlDoc.Descendants("NOMBRE_JEFE").FirstOrDefault()?.Value;
                        var fechaReporte = DateTime.Now;
                        var cultureEs = CultureInfo.GetCultureInfo("es-ES");

                        report.SetParameters(new ReportParameter("Base64Imagen", imgFirma));
                        report.SetParameters(new ReportParameter("fechaReporte", $"{fechaReporte.Day} de {fechaReporte.ToString("MMMM", cultureEs)} de {fechaReporte.Year}"));
                        report.SetParameters(new ReportParameter("jefeArea", jefeArea));
                        report.SetParameters(new ReportParameter("revisor", ""));
                        report.SetParameters(new ReportParameter("elaborador", ""));
                        report.SetParameters(new ReportParameter("fechaSaldoA", fechaReporte.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)));
                        report.SetParameters(new ReportParameter("fechaPrimerAporte", $"{cliente.FechaInscripcion.Day} de {cliente.FechaInscripcion.ToString("MMMM", cultureEs)} de {cliente.FechaInscripcion.Year}"));

                        report.DataSources.Add(datosBasicos);
                        report.DataSources.Add(datosCuotas);
                        report.DataSources.Add(datosMovimientos);
                        report.DataSources.Add(datosBloqueo);

                        byte[] reportBytes = report.Render("PDF");

                        var filename = $"Certificacion consolidada de haberes {reporteHaberes.customerIdentification}.pdf";
                        //return await GenerarArchivoPDF(reportBytes, filename);
                        return await GuardarReporteHaberes(reportBytes, filename, (Guid)ObtenerSolicitudCredito.SOC_ID);

                    }
                    else
                    {
                        throw new Exception("No existe informacion para este numero de cuenta");
                    }
                }
                else
                {
                    throw new Exception("Error al obtener datos del cliente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message );
                // throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }


        private async Task<FileResult> GuardarReporteHaberes(byte[] byteArray, string filename, Guid soc_id)
        {

            ServiciosDinamicos blobStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blobStorage.SRD_ESTADO)
            {
                FileInfo fi = new FileInfo(filename);
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "credito");
                await container.CreateIfNotExistsAsync();
                var newName = Guid.NewGuid() + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);

                using (Stream stream = new MemoryStream(byteArray))
                {
                    var result = await blob.UploadAsync(stream);

                    //await cloudBlockBlob.UploadFromStreamAsync(stream);

                }

                var Tdc_id = 0;
                DocumentosActSolicitudDto documentosActSimulacionDto = new DocumentosActSolicitudDto()
                {
                    SOC_ID = soc_id,
                    AFL_ID = 3
                };
                var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSolicitud(documentosActSimulacionDto);
                Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Equals("Certificacion consolidada de haberes", StringComparison.OrdinalIgnoreCase)).Select(x => x.TDC_ID).FirstOrDefault();


                RequestDocumentoSolicCreditoDto requestDocumentoSolicCreditoDto = new RequestDocumentoSolicCreditoDto()
                {
                    codigoBarras = "",
                    creadoPor = new Guid(),
                    fechaCreacion = DateTime.Now,
                    idSolicitudCredito = soc_id,
                    idTipoDocumento = Tdc_id,
                    rutaImagen = newName
                };



                var Response = await this._creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSolicCreditoDto);
                var msPdf = new MemoryStream(byteArray);
                var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
                {
                    FileDownloadName = filename
                };
                await archivoPdf.FileStream.FlushAsync();

                return archivoPdf;
            }
            else
            {
                return await GuardarReporteHaberesLocal(byteArray, filename, soc_id); 
            }
            

        }


        #region Guardar en local haberes
        private async Task<FileResult> GuardarReporteHaberesLocal(byte[] byteArray, string filename, Guid soc_id)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío.", nameof(byteArray));
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("El nombre del archivo no puede estar vacío.", nameof(filename));
            if (soc_id == Guid.Empty)
                throw new ArgumentException("El identificador de la solicitud no es válido.", nameof(soc_id));

            string folderPath = Path.Combine(_config.CarpetaServidorStorage, "credito");

            Directory.CreateDirectory(folderPath);

            var newName = Guid.NewGuid() + Path.GetExtension(filename);
            var localFilePath = Path.Combine(folderPath, newName);

            try
            {
                await File.WriteAllBytesAsync(localFilePath, byteArray);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el archivo en la carpeta configurada.", ex);
            }

            int Tdc_id;
            DocumentosActSolicitudDto documentosActSimulacionDto = new DocumentosActSolicitudDto()
            {
                SOC_ID = soc_id,
                AFL_ID = 3
            };

            try
            {
                var documentosActRespuestaDto = await _creditoBusinessLogic.ObtenerDocumentosActSolicitud(documentosActSimulacionDto);
                Tdc_id = documentosActRespuestaDto.Data
                           .FirstOrDefault(x => x.TIPO_DOCUMENTO.Equals("Certificacion consolidada de haberes", StringComparison.OrdinalIgnoreCase))?.TDC_ID ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los documentos asociados a la solicitud.", ex);
            }

            if (Tdc_id == 0)
                throw new Exception("No se encontró un tipo de documento válido para 'Certificacion consolidada de haberes'.");

            RequestDocumentoSolicCreditoDto requestDocumentoSolicCreditoDto = new RequestDocumentoSolicCreditoDto()
            {
                codigoBarras = "",
                creadoPor = Guid.NewGuid(), 
                fechaCreacion = DateTime.Now,
                idSolicitudCredito = soc_id,
                idTipoDocumento = Tdc_id,
                rutaImagen = localFilePath
            };

            await _creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSolicCreditoDto);

            var msPdf = new MemoryStream(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = filename
            };

            await archivoPdf.FileStream.FlushAsync();
            return archivoPdf;
        }

        #endregion



        private async Task<FileResult> GenerarArchivoPDF(byte[] byteArray, string filename)
        {
            var msPdf = new MemoryStream(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = filename
            };
            await archivoPdf.FileStream.FlushAsync();
            return archivoPdf;
        }

        private async Task<FileResult> GuardarReporteBlobStorage(HaberesRequestDto request, string reporte)
        {
            var response = new Response<string>();
            var fileName = $"REPORTE_HABERES_{request.ClienteIdentificacion}_{request.CuentaId}_.pdf";
            try
            {
                var checkBlob = await _blobStorageRepository.DescargarArchivoStorage("haberes", fileName);
                var diferenciaDias = (checkBlob.Details.LastModified.DateTime - DateTime.Now).TotalDays;
                if (diferenciaDias >= 30)
                {
                    await this._blobStorageRepository.EliminarArchivoStorage("haberes", fileName);
                    var _request = Convert.FromBase64String(reporte);

                    Stream stream = new MemoryStream(_request);

                    var blobResponse = await this._blobStorageRepository.SubirArchivoStorage(stream, "haberes", fileName);
                    var fileDownload = new FileStreamResult(checkBlob.Content, "application/octet-stream")
                    {
                        FileDownloadName = $"{Guid.NewGuid().ToString().ToUpper()}.pdf"
                    };
                    await fileDownload.FileStream.FlushAsync();
                    return fileDownload;

                }
                else
                {
                    var file = new FileStreamResult(checkBlob.Content, "application/octet-stream")
                    {
                        FileDownloadName = $"{Guid.NewGuid().ToString().ToUpper()}.pdf"
                    };
                    await file.FileStream.FlushAsync();
                    return file;
                }
            }
            catch
            {
                var _request = Convert.FromBase64String(reporte);

                Stream stream = new MemoryStream(_request);
                var blobResponse = await this._blobStorageRepository.SubirArchivoStorage(stream, "haberes", fileName);
                response.Data = blobResponse;
                var fileDownload = await this._blobStorageRepository.DescargarArchivoStorage("haberes", fileName);
                var downloadFile = new FileStreamResult(fileDownload.Content, "application/octet-stream")
                {
                    FileDownloadName = $"{Guid.NewGuid().ToString().ToUpper()}.pdf"
                };
                await downloadFile.FileStream.FlushAsync();
                return downloadFile;
            }
        }

    }
}
