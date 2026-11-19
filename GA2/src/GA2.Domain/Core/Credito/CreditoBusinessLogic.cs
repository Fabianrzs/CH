using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto.Cartera.Cierres;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Application.Dto.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.Seven;
using GA2.Application.Dto.Credito.Simulador;
using GA2.Application.Dto.Credito.Simulador.Comite;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Application.Dto.Credito.TU;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.InfoVendedor;
using GA2.Application.Dto.Prospeccion;
using GA2.Application.Dto.Reportes;
using GA2.Application.Dto.Seven;
using GA2.Application.Main;
using GA2.Domain.Core.DocumentsCreators.Riesgo;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.Solicitud.WorkManager;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Cierres;
using GA2.Domain.Entities.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Entities.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Domain.Entities.Correo;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.Simulador;
using GA2.Domain.Entities.Credito.Simulador.DocumentosAct;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Entities.Credito.TransUnion;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.Reportes;
using GA2.Domain.Entities.ServiciosDinamicosEnt;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.EmailNotificacion;
using GA2.Infraestructure.Repositories.InfoVendedor;
using GA2.Infraestructure.Repositories.LocalBlogStorage;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using GA2.Transversals.Commons;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Root = GA2.Domain.Entities.Root;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Consecutivo Logica
    /// <author>Cristian Gonzalez </author>
    /// <date>24/04/2021</date>
    /// </summary>
    /// 
    public class CreditoBusinessLogic : ICreditoBusinessLogic
    {
        private readonly ILogger<CreditoBusinessLogic> _logger;
        private readonly IMensajeRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IOptions<CarpetaBlogStorage> _optionCarpetaStorage;
        private readonly IOptions<AqmOptions> _optionsAqm;
        private readonly IValidacionIdentidadBusinessLogic _validacionIdentidadBusinessLogic;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IReportesRequestProvider _iReportesRequestProvider;
        private readonly IGestorDocumentalBusinessLogic _gestorDocumentalBusinessLogic;
        private readonly IConsumoWSTercero _wsTercero;
        private readonly IConsumoCDP _wsCDP;
        private readonly IConsumoWSProveedor _wsProveedor;
        private readonly IConsumoWSFactura _wsFactura;
        private readonly IConsumoSCnMcont _wsMovContable;
        private readonly ICarteraRepository _carteraRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly IInformacionVendedorRepository _informacionVendedorRepository;
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly ILocalBlogStorageRepository _localBlogStorageRepository;
        private readonly IServiciosDinamicosRepository _serviciosDinamicosRepository;
        private readonly IReportesRepository _reportesRepository;
        private readonly LocalBlogStorageConfig _config;
        private readonly Lazy<IReportesBusinessLogic> _reportesBusinessLogic;


        private readonly Lazy<ICarteraBusinessLogic> _carteraBusinessLogic;
        //private readonly IdentityDbContext _context;
        private readonly IEmailNotificacionRepository _emailNotificacionRepository;
        private readonly Lazy<IGestorDocumentSolicitudes> _gestorDocumentSolicitudes; 

        public CreditoBusinessLogic(ICatalogosRepository catalogsRepository,
            ILogger<CreditoBusinessLogic> logger,
            IMensajeRepository messageRepository,
            IMapper mapper,
            ICreditoRepository creditoRepository,
            IOptions<BlobStorageOptions> options,
            IOptions<CarpetaBlogStorage> optionsCarpetaStorage,
            IOptions<AqmOptions> optionsAqm,
            IValidacionIdentidadBusinessLogic validacionIdentidadBusinessLogic,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            IClientRequestProvider iClientRequestProvider,
            IReportesRequestProvider iReportesRequestProvider,
            IGestorDocumentalBusinessLogic iGestorDocumentalBusinessLogic,
            IConsumoWSTercero wsTercero,
            IConsumoCDP wsCDP,
            IConsumoWSProveedor wsProveedor,
            IConsumoWSFactura wsFactura,
            IConsumoSCnMcont wsMovContable,
            ICarteraRepository carteraRepository,
            IClientRequestProvider clientRequestProvider,
            IInformacionVendedorRepository informacionVendedorRepository,
            ICatalogosBusinessLogic catalogosBusinessLogic,
            ILocalBlogStorageRepository localBlogStorageRepository,
            IServiciosDinamicosRepository serviciosDinamicosRepository,
            IOptions<LocalBlogStorageConfig> config,
            IReportesRepository reportesRepository,
            Lazy<ICarteraBusinessLogic> carteraBusinessLogic,
            IEmailNotificacionRepository emailNotificacionRepository,
            Lazy<IReportesBusinessLogic> reportesBusinessLogic,
             Lazy<IGestorDocumentSolicitudes> gestorDocumentSolicitudes
            )
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _creditoRepository = creditoRepository;
            _options = options;
            _optionsAqm = optionsAqm;
            _validacionIdentidadBusinessLogic = validacionIdentidadBusinessLogic;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _iClientRequestProvider = iClientRequestProvider;
            _catalogsRepository = catalogsRepository;
            _iReportesRequestProvider = iReportesRequestProvider ?? throw new ArgumentNullException(nameof(iReportesRequestProvider));
            _gestorDocumentalBusinessLogic = iGestorDocumentalBusinessLogic;
            _wsTercero = wsTercero ?? throw new ArgumentNullException(nameof(wsTercero));
            _wsCDP = wsCDP ?? throw new ArgumentNullException(nameof(wsCDP));
            _wsProveedor = wsProveedor ?? throw new ArgumentNullException(nameof(wsProveedor));
            _wsFactura = wsFactura ?? throw new ArgumentNullException(nameof(wsFactura));
            _wsMovContable = wsMovContable ?? throw new ArgumentNullException(nameof(wsMovContable));
            _carteraRepository = carteraRepository;
            _clientRequestProvider = clientRequestProvider;
            _informacionVendedorRepository = informacionVendedorRepository;
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _optionCarpetaStorage = optionsCarpetaStorage;
            _localBlogStorageRepository = localBlogStorageRepository;
            _serviciosDinamicosRepository = serviciosDinamicosRepository;
            _config = config.Value;
            _reportesRepository = reportesRepository;
            _carteraBusinessLogic = carteraBusinessLogic;
            _emailNotificacionRepository = emailNotificacionRepository;
            _reportesBusinessLogic = reportesBusinessLogic;
            _gestorDocumentSolicitudes = gestorDocumentSolicitudes; 

        }

        /// <summary>
        /// Metodo actualiza actividad de tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns>ActualizaActividadTramiteDto</returns>
        public async Task<Response<ActualizaActividadTramiteDto>> ActualizaActividadTramite(ActividadTramiteSolicitudDto actualizacion)
        {
            var peticionActualizacion = this._mapper.Map<ActividadTramiteSolicitud>(actualizacion);
            return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.ActualizaActividadTramite(peticionActualizacion)) };
        }


        /// <summary>
        /// Metodo que obtiene los documentos Act solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <returns>IEnumerable<DocumentosActRespuestaDto></returns>
        public async Task<Response<IEnumerable<DocumentosActRespuestaDto>>> ObtenerDocumentosActSolicitud(DocumentosActSolicitudDto request)
        {
            var peticionDocumento = this._mapper.Map<DocumentosActSolicitud>(request);
            return new Response<IEnumerable<DocumentosActRespuestaDto>> { Data = this._mapper.Map<IEnumerable<DocumentosActRespuestaDto>>(await this._creditoRepository.ObtenerDocumentosActSolicitud(peticionDocumento)) };
        }



        /// <summary>
        /// metodo obtiene los docuemntos de flujo
        /// </summary>
        /// <param name="solicitud"></param>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <returns>IEnumerable<RespuestaFlujoDocumentosDto></returns>
        public async Task<Response<IEnumerable<RespuestaFlujoDocumentosDto>>> ObtenerDocumentosFlujo(PeticionFlujoDocumentosDto solicitud)
        {
            var peticionFlujo = this._mapper.Map<PeticionFlujoDocumentos>(solicitud);
            return new Response<IEnumerable<RespuestaFlujoDocumentosDto>> { Data = this._mapper.Map<IEnumerable<RespuestaFlujoDocumentosDto>>(await this._creditoRepository.ObtenerDocumentosFlujo(peticionFlujo)) };
        }

        /// <summary>
        /// Obtiene solicitud Credito producto
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticionSolicitud"></param>
        /// <returns>Response<RespuestaSolicitudCreditoProductoDto></returns>
        public async Task<Response<RespuestaSolicitudCreditoProductoDto>> ObtenerSolicCreditoProducto(PeticionSolicitudCreditoProductoDto peticionSolicitud)
        {

            var informacionDesembolso = await _creditoRepository.ObtenerDesembolsosSolicitudCredito(peticionSolicitud.SOC_ID);

            var peticion = _mapper.Map<PeticionSolicitudCreditoProducto>(peticionSolicitud);

            var resultado = await _creditoRepository.ObtenerSolicCreditoProducto(peticion);
            var respuesta = _mapper.Map<RespuestaSolicitudCreditoProductoDto>(resultado);

            if (informacionDesembolso != null)
            {
                respuesta.fechaDesembolso = informacionDesembolso.SID_FECHA_DESEMBOLSO;

            }

            return new Response<RespuestaSolicitudCreditoProductoDto> { Data = respuesta };
        }


        /// <summary>
        /// Obtiene solicitud Credito Basica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticionSolicitud"></param>
        /// <returns>Response<RespuestaCreditoBasicaDto></returns>
        public async Task<Response<RespuestaCreditoBasicaDto>> ObtenerSolicCreditoBasica(PeticionCreditoBasicaDto peticionSolicitud)
        {
            var peticion = this._mapper.Map<PeticionCreditoBasica>(peticionSolicitud);
            return new Response<RespuestaCreditoBasicaDto> { Data = this._mapper.Map<RespuestaCreditoBasicaDto>(await this._creditoRepository.ObtenerSolicCreditoBasica(peticion)) };
        }

        /// <summary>
        /// Obtener Solicitud Credito financiero
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticionSolicitud"></param>
        /// <returns>Response<RespuestaCreditoFinancieroDto></returns>
        public async Task<Response<RespuestaCreditoFinancieroDto>> ObtenerSolicCreditoFinancieros(PeticionCreditoFinancieroDto peticionSolicitud)
        {

            peticionSolicitud.IND = 2;
            peticionSolicitud.SOF_ID = Guid.NewGuid();
            var peticion = this._mapper.Map<PeticionCreditoFinanciero>(peticionSolicitud);
            return new Response<RespuestaCreditoFinancieroDto> { Data = this._mapper.Map<RespuestaCreditoFinancieroDto>(await this._creditoRepository.ObtenerSolicCreditoFinancieros(peticion)) };
        }

        /// <summary>
        /// Metodo logica de negocio para obtener la solicitud del tramite para crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicitud"></param>
        /// <returns>Response<IEnumerable<RespuestaSolicitudObtenerTramiteDto>></returns>
        public async Task<Response<IEnumerable<RespuestaSolicitudObtenerTramiteDto>>> ObtenerTramiteSolicitud(PeticionSolicitudObtenerTramiteDto solicitud)
        {
            var tramiteSolicitud = this._mapper.Map<PeticionSolicitudObtenerTramite>(solicitud);
            return new Response<IEnumerable<RespuestaSolicitudObtenerTramiteDto>> { Data = this._mapper.Map<IEnumerable<RespuestaSolicitudObtenerTramiteDto>>(await this._creditoRepository.ObtenerTramiteSolicitud(tramiteSolicitud)) };
        }

        /// <summary>
        /// Metodo para obtener observacion tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns>Response<IEnumerable<RespuestaObservacionTramiteDto>></returns>
        public async Task<Response<IEnumerable<RespuestaObservacionTramiteDto>>> ObtenerObservacionTramite(PeticionObtenerObservacionTramiteDto request)
        {
            RespuestaObservacionTramite respuestaPortal = await _creditoRepository.ConsultarActividadPorTramiteID(request.TRS_ID);
            string usuarioPortal = "";

            if (respuestaPortal.USUARIO_ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ClienteDto cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(respuestaPortal.TIPO_DOCUMENTO_AFILIADO, respuestaPortal.NUMERO_DOCUMENTO_AFILIADO)).Data;
                usuarioPortal = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}";
            }
            else
            {
                User usuarioEncontrado = await _creditoRepository.ConsultarUsuarioPorId(respuestaPortal.USUARIO_ID);
                usuarioPortal = usuarioEncontrado.USR_USERNAME;
            }

            var requestObservacion = this._mapper.Map<PeticionObtenerObservacionTramite>(request);
            var observaciones = (await _creditoRepository.ObtenerObservacionTramite(requestObservacion)).ToList();
            var observacionesMapeadas = this._mapper.Map<List<RespuestaObservacionTramiteDto>>(observaciones);

            var nuevoRegistro = new RespuestaObservacionTramiteDto
            {
                ID_TRAMITE = request.TRS_ID,
                ID_ACTIVIDAD_TRAMITE = respuestaPortal.ID_ACTIVIDAD_TRAMITE,
                ID_ACTIVIDAD = respuestaPortal.ID_ACTIVIDAD,
                ACTIVIDAD = "DILIGENCIAMIENTO SOLICITUD DE CRÉDITO",
                OBSERVACION = "Solicitud de crédito diligenciada desde el portal de afiliados",
                FECHA_INGRESO = respuestaPortal.FECHA_INGRESO,
                USUARIO_INGRESO = usuarioPortal,
                DESCRIPCION = respuestaPortal.DESCRIPCION
            };

            observacionesMapeadas.Insert(0, nuevoRegistro);

            return new Response<IEnumerable<RespuestaObservacionTramiteDto>>
            {
                Data = observacionesMapeadas
            };
        }


        /// <summary>
        /// metodo envia subsanacion actividad tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns>Response<ActualizaActividadTramiteDto></returns>
        public async Task<Response<ActualizaActividadTramiteDto>> EnviaSubsancionActividadTramite(ActividadTramiteSolicitudDto actualizacion)
        {
            var peticionActualizacion = this._mapper.Map<ActividadTramiteSolicitud>(actualizacion);
            return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.EnviaSubsancionActividadTramite(peticionActualizacion)) };
        }
        /// <summary>
        /// Metodo aprueba actividad de tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns>Response<ActualizaActividadTramiteDto></returns>
        public async Task<Response<ActualizaActividadTramiteDto>> ApruebaActividadTramite(ActividadTramiteSolicitudDto actualizacion)
        {
            ResponseVerificarCierreActSeg confirmacion = new ResponseVerificarCierreActSeg();
            var peticionActualizacion = this._mapper.Map<ActividadTramiteSolicitud>(actualizacion);
            bool puedeAprobar = true;
            if (peticionActualizacion.ACTIVIDAD_PRINCIPAL?.ToUpper() == "S")
            {
                confirmacion = await this._creditoRepository.VerificarCierreActSeg(new RequestVerificarCierreActSeg { AFL_ID = peticionActualizacion.AFL_ID, TRS_ID = peticionActualizacion.TRS_ID });
                if (confirmacion.Mensaje != "OK")
                {
                    puedeAprobar = false;
                }
            }
            if (puedeAprobar)
            {


                if (peticionActualizacion.AFL_ID == 3)
                {

                    //aqui va el crear 
                    var informacionSimulacion = await _creditoRepository.ObtenerSolicitudCreditoNumeroDocumento(actualizacion.NUMERO_DOCUMENTO);

                    IdentificacionProspeccion datosReporte = new IdentificacionProspeccion
                    {
                        SLS_ID = informacionSimulacion.SLS_ID,
                        numeroDocumento = actualizacion.NUMERO_DOCUMENTO,
                        tipoDocumento = 1

                    };

                    var clase = await _reportesBusinessLogic.Value.ReporteApruebaActividadTramite(datosReporte);

                    IdentificacionProspeccion identificacionProspeccion = new IdentificacionProspeccion();


                    //var archivo = await _reportesBusineesLogic.ReporteApruebaActividadTramite(identificacionProspeccion);

                    ClienteDto cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, actualizacion.NUMERO_DOCUMENTO)).Data;

                    var email = new Email
                    {
                        Modulo = "credito",

                        RutaHtml = "TemplateAprobacionCredito.html",

                        RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                        Asunto = "Comunicado Oficial - aprobación de crédito",

                        Destinatarios = new List<string>
                    {
                        "analisis.procesos@grupokriterion.com.co",
                        "desarrollok30@grupokriterion.com.co"
                    },

                        Parametros = new Dictionary<string, string>
                    {
                        { "NombreCompleto", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" }
                    },
                        AdjuntoBase64 = clase.Base64,
                        EnvioBase64 = true,
                        NombreDocumentoAdjunto = "Comunicación oficial de avalúo con concepto favorable"
                    };

                    await _emailNotificacionRepository.EnviarAsync(email);


                    RequestDocumentoSolicCreditoDto requestDocumentoSimDto = new RequestDocumentoSolicCreditoDto()
                    {
                        idTipoDocumento = 161,
                        codigoBarras = "",
                        creadoPor = new Guid(),
                        fechaCreacion = DateTime.Now,
                        rutaImagen = clase.Archivo.FileDownloadName,
                        idSolicitudCredito = informacionSimulacion.SOC_ID,
                        Radicado = actualizacion.RADICADO
                    };




                    AttachRequestDto attachRequestDto = new AttachRequestDto();
                    attachRequestDto.UserOperation = "web";
                    attachRequestDto.Files = new List<FileDto>();

                    attachRequestDto.Radicado = actualizacion.RADICADO;

                    var infoTipoDocument = ObtenerTipoDocumentoPorId("161").Result.Data;

                    attachRequestDto.Files.Add(CrearObjetoWorkmanager(clase.Base64, infoTipoDocument, "pdf", false));

                    var documentos = await _gestorDocumentSolicitudes.Value
   .ObtenerDocumentosPorRadicado(attachRequestDto.Radicado);

                    bool documentoYaCargado = documentos?.Data?.FileProperties?
                        .Any(x => string.Equals(
                            x.Description?.Trim(),
                            infoTipoDocument?.Descripcion?.Trim(),
                            StringComparison.OrdinalIgnoreCase)) == true;

                    if (!documentoYaCargado)
                    {
                        if (attachRequestDto.Files.Count > 0)
                        {
                            var responseWorkmanager = await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                        }
                    }

                    await CrearDocumentoSolicCredito(requestDocumentoSimDto);

                }




                return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.ApruebaActividadTramite(peticionActualizacion)) };
            }
            else return new Response<ActualizaActividadTramiteDto> { ReturnMessage = confirmacion.Mensaje };
        }
        /// <summary>
        /// Metodo rechazar actividad tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns>Response<ActualizaActividadTramiteDto></returns>
        public async Task<Response<ActualizaActividadTramiteDto>> RechazarActividadTramite(ActividadTramiteSolicitudDto actualizacion)
        {
            ResponseVerificarCierreActSeg confirmacion = new ResponseVerificarCierreActSeg();
            var peticionActualizacion = this._mapper.Map<ActividadTramiteSolicitud>(actualizacion);
            bool puedeRechazar = true;
            if (peticionActualizacion.ACTIVIDAD_PRINCIPAL.ToUpper() == "S")
            {
                confirmacion = await this._creditoRepository.VerificarCierreActSeg(new RequestVerificarCierreActSeg { AFL_ID = peticionActualizacion.AFL_ID, TRS_ID = peticionActualizacion.TRS_ID });
                if (confirmacion.Mensaje != "OK")
                {
                    puedeRechazar = false;
                }
            }
            if (puedeRechazar)
            {
                ClienteDto cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, actualizacion.NUMERO_DOCUMENTO)).Data;


                var email = new Email
                {
                    Modulo = "credito",

                    RutaHtml = "TemplateRechazoCredito.html",

                    RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                    Asunto = "Comunicado Oficial - Rechazo",

                    Destinatarios = new List<string>
                {
                    "analisis.procesos@grupokriterion.com.co"
                },

                    Parametros = new Dictionary<string, string>
                {
                    { "NombreCompleto", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" }

                }

                };

                await _emailNotificacionRepository.EnviarAsync(email);

                return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.RechazarActividadTramite(peticionActualizacion)) };
            }
            else return new Response<ActualizaActividadTramiteDto> { ReturnMessage = confirmacion.Mensaje };

        }

        /// <summary>
        /// Metodo desistir actividad tramite
        /// </summary>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <param name="desistimiento"></param>
        /// <returns>Response<ActualizaActividadTramiteDto></returns>
        public async Task<Response<ActualizaActividadTramiteDto>> DesistirActividadTramite(ActividadTramiteSolicitudDto desistimiento)
        {
            ResponseVerificarCierreActSeg confirmacion = new ResponseVerificarCierreActSeg();
            var peticionDesistimiento = this._mapper.Map<ActividadTramiteSolicitud>(desistimiento);
            bool puedeRechazar = true;
            if (peticionDesistimiento.ACTIVIDAD_PRINCIPAL.ToUpper() == "S")
            {
                confirmacion = await this._creditoRepository.VerificarCierreActSeg(new RequestVerificarCierreActSeg { AFL_ID = peticionDesistimiento.AFL_ID, TRS_ID = peticionDesistimiento.TRS_ID });
                if (confirmacion.Mensaje != "OK")
                {
                    puedeRechazar = false;
                }
            }
            if (puedeRechazar)
            {
                return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.DesistirActividadTramite(peticionDesistimiento)) };
            }
            else return new Response<ActualizaActividadTramiteDto> { ReturnMessage = confirmacion.Mensaje };

        }

        /// <summary>
        /// Devolver Actividad Tramite
        /// </summary>
        /// <param name="devolucion"></param>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <returns></returns>
        public async Task<Response<ActualizaActividadTramiteDto>> DevolverActividadTramite(DevolverActividadTramiteDto devolucion)
        {
            var peticionDevolucion = this._mapper.Map<DevolverActividadTramite>(devolucion);

            return new Response<ActualizaActividadTramiteDto> { Data = this._mapper.Map<ActualizaActividadTramiteDto>(await this._creditoRepository.DevolverActividadTramite(peticionDevolucion)) };
        }

        /// <summary>
        /// Metodo crear solicitud credito informacion Tecnica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfTecnica"></param>
        /// <returns>Response<SolicCreditoInfTecnicaDto></returns>
        public async Task<Response<SolicCreditoInfTecnicaDto>> CrearSolicCreditoInfTecnica(SolicCreditoInfTecnicaDto solicCreditoInfTecnica)
        {
            var solicitudInfTecnica = this._mapper.Map<SolicCreditoInfTecnica>(solicCreditoInfTecnica);
            if (solicCreditoInfTecnica.IdSolicitudInformacionTecnica == default) solicitudInfTecnica.SIT_ID = Guid.NewGuid();
            var responseInfoTecnica = await this._creditoRepository.CrearSolicCreditoInfTecnica(solicitudInfTecnica);
            var infInmueble = new List<SolicCreditoInfTecInmDto>();
            if (solicCreditoInfTecnica.solicitudesCreditoInfTecnicaInmueble != null)
            {
                foreach (SolicCreditoInfTecInmDto request in solicCreditoInfTecnica.solicitudesCreditoInfTecnicaInmueble)
                {
                    request.idSolicitudtecnica = solicitudInfTecnica.SIT_ID;
                    if (request.idSolicitudInfoTecnicaInmueble == default) request.idSolicitudInfoTecnicaInmueble = Guid.NewGuid();
                    var solicitudInfTecInm = this._mapper.Map<SolicCreditoInfTecInm>(request);
                    var responseInfoTecInm = await this._creditoRepository.CrearSolicCreditoInfTecInm(solicitudInfTecInm);
                    infInmueble.Add(this._mapper.Map<SolicCreditoInfTecInmDto>(responseInfoTecInm));
                }
            }
            SolicCreditoInfTecnicaDto response = new SolicCreditoInfTecnicaDto
            {
                IdSolicitudInformacionTecnica = responseInfoTecnica.SIT_ID,
                IdSolicitudCredito = responseInfoTecnica.SOC_ID,
                ValorAvaluoComercial = responseInfoTecnica.SIT_VALOR_AVALUO_COMERCIAL,
                ValorVentaInmueble = responseInfoTecnica.SIT_VALOR_VENTA_INMUEBLE,
                ConsideracionesFinales = responseInfoTecnica.SIT_CONSIDERACIONES_FINALES,
                DeclaraCumplimiento = responseInfoTecnica.SIT_DECLARA_CUMPLIMIENTO,
                CondicionesSalvedades = responseInfoTecnica.SIT_CONDICIONES_SALVEADES,
                ConceptoEstTecnico = responseInfoTecnica.SIT_CONCEPTO_EST_TECNICO,
                CreadoPor = responseInfoTecnica.SIT_CREADO_POR,
                FechaCreacion = responseInfoTecnica.SIT_FECHA_CREACION,
                solicitudesCreditoInfTecnicaInmueble = infInmueble,
                Estrato = responseInfoTecnica.SIT_ESTRATO,
                NombreAliado = responseInfoTecnica.NOMBRE_ALIADO,
                ValorAvaluoCatastral = responseInfoTecnica.SIT_VALOR_AVALUO_CATASTRAL
            };


            return new Response<SolicCreditoInfTecnicaDto> { Data = response };
        }

        /// <summary>
        /// Crear solicitud Credito Informacion Tecnica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfTecInm"></param>
        /// <returns>Response<SolicCreditoInfTecInmDto></returns>
        public async Task<Response<SolicCreditoInfTecInmDto>> CrearSolicCreditoInfTecInm(SolicCreditoInfTecInmDto solicCreditoInfTecInm)
        {
            var solicitud = this._mapper.Map<SolicCreditoInfTecInm>(solicCreditoInfTecInm);
            return new Response<SolicCreditoInfTecInmDto> { Data = this._mapper.Map<SolicCreditoInfTecInmDto>(await this._creditoRepository.CrearSolicCreditoInfTecInm(solicitud)) };
        }

        /// <summary>
        /// Metodo crear Solicitud Credito Informacion Juridica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfJuridica"></param>
        /// <returns>Response<SolicCreditoInfJuridicaDto></returns>
        public async Task<Response<SolicCreditoInfJuridicaDto>> CrearSolicCreditoInfJuridica(SolicCreditoInfJuridicaDto solicCreditoInfJuridica)
        {
            var solicitudInfJuridica = this._mapper.Map<SolicCreditoInfJuridica>(solicCreditoInfJuridica);

            if (solicCreditoInfJuridica.idSolicitudInformacionJuridica == default)
                solicitudInfJuridica.SIJ_ID = Guid.NewGuid();

            var responseInfoJuridica = await this._creditoRepository.CrearSolicCreditoInfJuridica(solicitudInfJuridica);
            var infInmueble = new List<SolicCreditoInfJurInmDto>();
            if (solicCreditoInfJuridica.solicitudesCreditoInfoJurInmueble != null)
            {
                foreach (SolicCreditoInfJurInmDto request in solicCreditoInfJuridica.solicitudesCreditoInfoJurInmueble)
                {
                    request.IdSolicitudInformacionJuridica = solicitudInfJuridica.SIJ_ID;
                    if (request.IdSolicitudInfJuridicaInmueble == default) request.IdSolicitudInfJuridicaInmueble = Guid.NewGuid();
                    var solicitudInfJurInm = this._mapper.Map<SolicCreditoInfJurInm>(request);
                    var responseInfoJurInm = await this._creditoRepository.CrearSolicCreditoInfJurInm(solicitudInfJurInm);
                    infInmueble.Add(this._mapper.Map<SolicCreditoInfJurInmDto>(responseInfoJurInm));
                }
            }
            SolicCreditoInfJuridicaDto response = new SolicCreditoInfJuridicaDto
            {
                idSolicitudInformacionJuridica = responseInfoJuridica.SIJ_ID,
                idSolicitudCredito = responseInfoJuridica.SOC_ID,
                linderos = responseInfoJuridica.SIJ_LINDEROS,
                tipoParqueadero = responseInfoJuridica.SIJ_TIPO_PARQUEADERO,
                edadJuridica = responseInfoJuridica.SIJ_EDAD_JURIDICA,
                fechaPrimerActo = responseInfoJuridica.SIJ_FECHA_PRIMER_ACTO,
                tradicionInmueble = responseInfoJuridica.SIJ_TRADICION_INMUEBLE,
                analisisUlt20Años = responseInfoJuridica.SIJ_ANALISIS_ULT_20_AÑOS,
                analisisSitJuridica = responseInfoJuridica.SIJ_ANALISIS_SIT_JURIDICA,
                analisisRegPropHorizontal = responseInfoJuridica.SIJ_ANALISIS_REG_PROP_HOR,
                analisisPazySalvo = responseInfoJuridica.SIJ_ANALISIS_PAZ_Y_SALVO,
                analisisVendedor = responseInfoJuridica.SIJ_ANALISIS_VENDEDOR,
                viabilidadJurNegocio = responseInfoJuridica.SIJ_VIABILIDAD_JUR_NEGOCIO,
                recomendaciones = responseInfoJuridica.SIJ_RECOMENDACIONES,
                conceptoJuridicoFin = responseInfoJuridica.SIJ_CONCEPTO_JURIDICO_FIN,
                conceptoEstJuridicio = responseInfoJuridica.SIJ_CONCEPTO_EST_JURIDICO,
                creadoPor = responseInfoJuridica.SIJ_CREADO_POR,
                fechaCreacion = responseInfoJuridica.SIJ_FECHA_CREACION,
                direccion = responseInfoJuridica.SIJ_DIRECCION,
                codigoDepartamento = responseInfoJuridica.DPD_ID,
                codigoCiudad = responseInfoJuridica.DPC_ID,
                solicitudesCreditoInfoJurInmueble = infInmueble,
                departamentoNotaria = responseInfoJuridica.SIJ_DEPARTAMENTO_NOTARIA,
                ciudadNotaria = responseInfoJuridica.SIJ_CIUDAD_NOTARIA,
                Escritura = responseInfoJuridica.SJI_ESCRITURA,
                Notaria = responseInfoJuridica.SJI_NOTARIA,
                FechaEscritura = responseInfoJuridica.SJI_FECHA_ESCRITURA,
                DescripcionInmueble = responseInfoJuridica.SIJ_DESCRIPCION_INMUEBLE
            };


            return new Response<SolicCreditoInfJuridicaDto> { Data = response };


        }

        /// <summary>
        /// Crear Solicitud Credito Informacion Juridica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfJuridica"></param>
        /// <returns>Response<SolicCreditoInfJurInmDto></returns>
        public async Task<Response<SolicCreditoInfJurInmDto>> CrearSolicCreditoInfJurInm(SolicCreditoInfJurInmDto solicCreditoInfJurInm)
        {
            var solicitud = this._mapper.Map<SolicCreditoInfJurInm>(solicCreditoInfJurInm);
            return new Response<SolicCreditoInfJurInmDto> { Data = this._mapper.Map<SolicCreditoInfJurInmDto>(await this._creditoRepository.CrearSolicCreditoInfJurInm(solicitud)) };
        }

        /// <summary>
        /// Obtener Solicitud Credito Informacion Tecnica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfJuridica"></param>
        /// <returns>Response<SolicCreditoInfTecnicaDto></returns>
        public async Task<Response<SolicCreditoInfTecnicaDto>> ObtenerSolicCreditoInfTecnica(RequestSolicCreditoInfTecnicaDto requestSolicCreditoInfTecnica)
        {
            var solicitudInfoTec = this._mapper.Map<RequestSolicCreditoInfTecnica>(requestSolicCreditoInfTecnica);
            var responseInfoTec = await this._creditoRepository.ObtenerSolicCreditoInfTecnica(solicitudInfoTec);
            if (responseInfoTec == null)
            {
                return new Response<SolicCreditoInfTecnicaDto> { Data = null };
            }
            else
            {
                var solicitudInfoTecInm = new RequestSolicCreditoInfTecInm { SOC_ID = responseInfoTec.SOC_ID, IND = 3 };
                var responseInfoTecInm = await this._creditoRepository.ObtenerSolicCreditoInfTecInm(solicitudInfoTecInm);

                var solicCreditoInfTecnica = new SolicCreditoInfTecnicaDto
                {
                    IdSolicitudInformacionTecnica = responseInfoTec.SIT_ID,
                    IdSolicitudCredito = responseInfoTec.SOC_ID,
                    ValorAvaluoComercial = responseInfoTec.SIT_VALOR_AVALUO_COMERCIAL,
                    ValorAvaluoCatastral = responseInfoTec.SIT_VALOR_AVALUO_CATASTRAL,
                    ValorVentaInmueble = responseInfoTec.SIT_VALOR_VENTA_INMUEBLE,
                    ConsideracionesFinales = responseInfoTec.SIT_CONSIDERACIONES_FINALES,
                    DeclaraCumplimiento = responseInfoTec.SIT_DECLARA_CUMPLIMIENTO,
                    CondicionesSalvedades = responseInfoTec.SIT_CONDICIONES_SALVEADES,
                    ConceptoEstTecnico = responseInfoTec.SIT_CONCEPTO_EST_TECNICO,
                    NombreAliado = responseInfoTec.NOMBRE_ALIADO,
                    CreadoPor = responseInfoTec.SIT_CREADO_POR,
                    FechaCreacion = responseInfoTec.SIT_FECHA_CREACION,
                    Estrato = responseInfoTec.SIT_ESTRATO,
                    //ValorAvaluoCatastral = responseInfoTec.SIT_VALOR_AVALUO_CATASTRAL,
                    solicitudesCreditoInfTecnicaInmueble = this._mapper.Map<IEnumerable<SolicCreditoInfTecInmDto>>(responseInfoTecInm)
                };
                return new Response<SolicCreditoInfTecnicaDto> { Data = solicCreditoInfTecnica };
            }

        }

        /// <summary>
        /// Obtener Solicitud Credito Informacion Tecnica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfTecInm"></param>
        /// <returns>Response<IEnumerable<SolicCreditoInfTecInmDto>></returns>
        public async Task<Response<IEnumerable<SolicCreditoInfTecInmDto>>> ObtenerSolicCreditoInfTecInm(RequestSolicCreditoInfTecInmDto requestSolicCreditoInfTecInm)
        {
            var solicitud = this._mapper.Map<RequestSolicCreditoInfTecInm>(requestSolicCreditoInfTecInm);
            return new Response<IEnumerable<SolicCreditoInfTecInmDto>> { Data = this._mapper.Map<IEnumerable<SolicCreditoInfTecInmDto>>(await this._creditoRepository.ObtenerSolicCreditoInfTecInm(solicitud)) };

        }

        /// <summary>
        /// Metodo obtener Solicitud Credito Informacion Juridica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfJuridica"></param>
        /// <returns>Response<SolicCreditoInfJuridicaDto></returns>
        public async Task<Response<SolicCreditoInfJuridicaDto>> ObtenerSolicCreditoInfJuridica(RequestSolicCreditoInfJuridicaDto requestSolicCreditoInfJuridica)
        {
            var solicitudInfoJur = this._mapper.Map<RequestSolicCreditoInfJuridica>(requestSolicCreditoInfJuridica);
            var responseInfoJur = await this._creditoRepository.ObtenerSolicCreditoInfJuridica(solicitudInfoJur);
            if (responseInfoJur == null)
            {
                return new Response<SolicCreditoInfJuridicaDto> { Data = null };
            }
            else
            {
                var solicitudInfoJurInm = new RequestSolicCreditoInfJurInm { SOC_ID = responseInfoJur.SOC_ID, IND = 3 };
                var responseInfoJurInm = await this._creditoRepository.ObtenerSolicCreditoInfJurInm(solicitudInfoJurInm);

                var solicCreditoInfJuridica = new SolicCreditoInfJuridicaDto
                {
                    idSolicitudInformacionJuridica = responseInfoJur.SIJ_ID,
                    idSolicitudCredito = responseInfoJur.SOC_ID,
                    linderos = responseInfoJur.SIJ_LINDEROS,
                    tipoParqueadero = responseInfoJur.SIJ_TIPO_PARQUEADERO,
                    edadJuridica = responseInfoJur.SIJ_EDAD_JURIDICA,
                    fechaPrimerActo = responseInfoJur.SIJ_FECHA_PRIMER_ACTO,
                    tradicionInmueble = responseInfoJur.SIJ_TRADICION_INMUEBLE,
                    analisisUlt20Años = responseInfoJur.SIJ_ANALISIS_ULT_20_AÑOS,
                    analisisSitJuridica = responseInfoJur.SIJ_ANALISIS_SIT_JURIDICA,
                    analisisRegPropHorizontal = responseInfoJur.SIJ_ANALISIS_REG_PROP_HOR,
                    analisisPazySalvo = responseInfoJur.SIJ_ANALISIS_PAZ_Y_SALVO,
                    analisisVendedor = responseInfoJur.SIJ_ANALISIS_VENDEDOR,
                    viabilidadJurNegocio = responseInfoJur.SIJ_VIABILIDAD_JUR_NEGOCIO,
                    recomendaciones = responseInfoJur.SIJ_RECOMENDACIONES,
                    conceptoJuridicoFin = responseInfoJur.SIJ_CONCEPTO_JURIDICO_FIN,
                    conceptoEstJuridicio = responseInfoJur.SIJ_CONCEPTO_EST_JURIDICO,
                    codigoCiudad = responseInfoJur.DPC_ID,
                    direccion = responseInfoJur.SIJ_DIRECCION,
                    codigoDepartamento = responseInfoJur.DPD_ID,
                    departamento = responseInfoJur.DEPARTAMENTO,
                    ciudad = responseInfoJur.CIUDAD,
                    nombreAliado = responseInfoJur.NOMBRE_ALIADO,
                    creadoPor = responseInfoJur.SIJ_CREADO_POR,
                    fechaCreacion = responseInfoJur.SIJ_FECHA_CREACION,
                    Notaria = responseInfoJur.SJI_NOTARIA,
                    FechaEscritura = responseInfoJur.SJI_FECHA_ESCRITURA,
                    Escritura = responseInfoJur.SJI_ESCRITURA,
                    ciudadNotaria = responseInfoJur.SIJ_CIUDAD_NOTARIA,
                    departamentoNotaria = responseInfoJur.SIJ_DEPARTAMENTO_NOTARIA,
                    DescripcionInmueble = responseInfoJur.SIJ_DESCRIPCION_INMUEBLE,
                    FolioMatriculas = responseInfoJur.SIJ_FOLIOS_MATRICULAS,
                    OficinaInstrumentosPublicos = responseInfoJur.SIJ_OFICINA_INSTRUMENTOS_PUBLICOS,
                    solicitudesCreditoInfoJurInmueble = this._mapper.Map<IEnumerable<SolicCreditoInfJurInmDto>>(responseInfoJurInm)
                };
                return new Response<SolicCreditoInfJuridicaDto> { Data = solicCreditoInfJuridica };
            }

        }

        /// <summary>
        /// Metodo Obtener Solicitud Credito Informacion Juridica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfJurInm"></param>
        /// <returns>Response<IEnumerable<SolicCreditoInfJurInmDto>></returns>
        public async Task<Response<IEnumerable<SolicCreditoInfJurInmDto>>> ObtenerSolicCreditoInfJurInm(RequestSolicCreditoInfJurInmDto requestSolicCreditoInfJurInm)
        {
            var solicitud = this._mapper.Map<RequestSolicCreditoInfJurInm>(requestSolicCreditoInfJurInm);
            return new Response<IEnumerable<SolicCreditoInfJurInmDto>> { Data = this._mapper.Map<IEnumerable<SolicCreditoInfJurInmDto>>(await this._creditoRepository.ObtenerSolicCreditoInfJurInm(solicitud)) };

        }

        /// <summary>
        /// Metodo Crear Documento Solicitud Credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="documentoSolicCredito"></param>
        /// <returns></returns>
        public async Task<Response<ResponseDocumentoSolicCreditoDto>> CrearDocumentoSolicCredito(RequestDocumentoSolicCreditoDto requestDocumentoSolicCredito)
        {
            //174   Paz y Salvo
            if (requestDocumentoSolicCredito.numero_documento != null && requestDocumentoSolicCredito.idTipoDocumento ==174)
            {
                DocumentosAdicionalesCargados documentosAdicionales = new()
                {
                    DSA_DOCUMENTO = "Paz y Salvo",
                    DSA_RUTA = requestDocumentoSolicCredito.rutaImagen,
                    DSA_CARPETA = "reportes-sfc",
                    DSA_MODULO = "credito",
                    DSA_CARGADO_DODO = false,
                    DSA_NUMERO_DOCUMENTO = requestDocumentoSolicCredito.numero_documento
                };

                await _reportesRepository.GuardarDocumentoAdicionalCargado(documentosAdicionales);

                return new Response<ResponseDocumentoSolicCreditoDto>
                {
                    Data = null,
                    IsSuccess = true,
                    ReturnMessage = $"Archivo creado con éxito {requestDocumentoSolicCredito.rutaImagen}"
                };
            }
            else
            {
                var attachNew = new Core.ReferenciasSvc.WorkManager.AttachRequestDto();
                AttachRequestDto attachRequestDto = new AttachRequestDto();
                attachRequestDto.UserOperation = "web";
                attachRequestDto.Files = new List<FileDto>();

                string FilePath = requestDocumentoSolicCredito.rutaImagen;
                var solicitudCredito = await _creditoRepository
    .ObtenerSolicitudCredito(requestDocumentoSolicCredito.idSolicitudCredito, 1);

                attachRequestDto.Radicado = !string.IsNullOrWhiteSpace(solicitudCredito?.SOC_NUMERO_RADICADO)
                    ? solicitudCredito.SOC_NUMERO_RADICADO
                    : requestDocumentoSolicCredito.Radicado;

                var fileExtensions = new List<string> { "pdf", "tif" };
                int folios = 0;
                string archivoBase64 = string.Empty;
                byte[] conten;

                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
                if (blogStorage.SRD_ESTADO)
                {
                    var bdinfo = await _blobStorageGenericRepository.DescargarArchivoStorage("credito", $"{requestDocumentoSolicCredito.rutaImagen}");
                    var fileSResult = new FileStreamResult(bdinfo.Content, bdinfo.ContentType);

                    if (fileExtensions.Contains(FilePath.Split(".")[1]))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await bdinfo.Content.CopyToAsync(memoryStream);
                            conten = memoryStream.ToArray();
                        }
                        string contenidoTexto = Encoding.UTF8.GetString(conten);

                        folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                        archivoBase64 = Convert.ToBase64String(conten);

                        using (StreamReader stramReadre = new StreamReader(fileSResult.FileStream))
                        {
                            conten = Encoding.UTF8.GetBytes(stramReadre.ReadToEnd());
                            archivoBase64 = Convert.ToBase64String(conten);
                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(stramReadre.ReadToEnd()).Count;
                        };
                    }
                    else folios = 0;

                }
                else
                {
                    var bdinfoLocal = await _localBlogStorageRepository.DescargarArchivoLocal("credito", $"{requestDocumentoSolicCredito.rutaImagen}");

                    if (bdinfoLocal.IsSuccess && bdinfoLocal.Data != null)
                    {
                        byte[] fileBytes = bdinfoLocal.Data;
                        string extension = Path.GetExtension(requestDocumentoSolicCredito.rutaImagen).Replace(".", "").ToLower();

                        if (fileExtensions.Contains(extension))
                        {
                            // Convertimos los bytes a Base64
                            archivoBase64 = Convert.ToBase64String(fileBytes);

                            // Convertimos bytes a string para contar folios (solo PDFs)
                            string contenidoTexto = Encoding.UTF8.GetString(fileBytes);

                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;
                        }
                        else
                        {
                            folios = 0;
                        }
                    }
                    else
                    {
                        // Si no existe el archivo o falló la lectura
                        folios = 0;
                        archivoBase64 = string.Empty;
                    }
                }






                var filenamewhitoutext = blogStorage.SRD_ESTADO ? Guid.Parse(FilePath.Split(".")[0]) : Guid.Parse(Path.GetFileNameWithoutExtension(FilePath));
                var fileExtension = FilePath.Split(".")[1];
                var documento = new ResponseDocumentoSolicCredito
                {
                    DCS_ID = filenamewhitoutext,
                    SOC_ID = requestDocumentoSolicCredito.idSolicitudCredito,
                    TDC_ID = requestDocumentoSolicCredito.idTipoDocumento,
                    DCS_CODIGO_BARRAS = requestDocumentoSolicCredito.codigoBarras,
                    DCS_FECHA_DOCUMENTO = DateTime.Now,
                    DCS_NUMERO_FOLIOS = blogStorage.SRD_ESTADO ? folios : 0,
                    DCS_TAMAÑO = 0,
                    DCS_RUTA_IMAGEN = requestDocumentoSolicCredito.rutaImagen,
                    DCS_ESTADO = "1",
                    DCS_CREADO_POR = requestDocumentoSolicCredito.creadoPor,
                    DCS_FECHA_CREACION = DateTime.Now
                };



                var infoTipoDocument = ObtenerTipoDocumentoPorId(requestDocumentoSolicCredito.idTipoDocumento.ToString()).Result.Data;

                attachRequestDto.Files.Add(CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, fileExtension));

                DocumentoCargadoDodo documentoCargado =
    await _creditoRepository.ConsultaDocumentoCargadoDodo(
        requestDocumentoSolicCredito.idSolicitudCredito,
        infoTipoDocument.Descripcion);

                var documentos = await _gestorDocumentSolicitudes.Value
    .ObtenerDocumentosPorRadicado(attachRequestDto.Radicado);

                bool documentoYaCargado = documentos?.Data?.FileProperties?
                    .Any(x => string.Equals(
                        x.Description?.Trim(),
                        infoTipoDocument?.Descripcion?.Trim(),
                        StringComparison.OrdinalIgnoreCase)) == true;

                if (!documentoYaCargado)
                {
                    DocumentoCargadoDodo dod_dodo = new DocumentoCargadoDodo
                    {
                        DCD_CARGADO = true,
                        DCD_DESCRIPCION = infoTipoDocument.Descripcion,
                        RUTA = requestDocumentoSolicCredito.rutaImagen,
                        SLS_ID = requestDocumentoSolicCredito.idSolicitudCredito,
                        TDC_ID = requestDocumentoSolicCredito.idTipoDocumento
                    };

                    await _creditoRepository.InsertarDocumentoCargadoDodo(dod_dodo);

                    var responseWorkmanager =
                        await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);
                }
                return new Response<ResponseDocumentoSolicCreditoDto> { Data = this._mapper.Map<ResponseDocumentoSolicCreditoDto>(await this._creditoRepository.CrearDocumentoSolicCredito(documento)) };
            }
        }

        private FileDto CrearObjetoWorkmanager(string fileBase64, TipoDocumentoDto infoDoc, string fileExtension, bool flujo83 = false)
        {
            if (flujo83)
            {
                return new FileDto(fileBase64, infoDoc.Descripcion, "00", "00", fileExtension, "");

            }

            return new FileDto(fileBase64, infoDoc.Descripcion, infoDoc.DirectoryCode.ToString(), infoDoc.DocumentTypeCode.ToString(), fileExtension, "");
        }




        /// <summary>
        /// Consulta la simulacion dle cliente
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="documento"></param>
        /// <returns></returns>
        public async Task<Response<SimulacionClienteDto>> ConsultarSimulacionCliente(string documento)
        {
            var response = new Response<SimulacionClienteDto> { Data = this._mapper.Map<SimulacionClienteDto>(await this._creditoRepository.ConsultarSimulacionCliente(documento)) };
            if (response.Data != null)
            {
                response.Data.departamento = (await _catalogsRepository.ObtenerDepartamentoPorId(response.Data.dpd_inmueble)).DPD_NOMBRE;
                var ciudadesDepartamento = await _catalogsRepository.ObtenerCiudadesPorDepartamento(response.Data.dpd_inmueble);
                response.Data.ciudad = ciudadesDepartamento.First(c => c.DPC_CODIGO == response.Data.dpc_inmueble).DPC_NOMBRE;
                response.Data.dpc_inmueble = ciudadesDepartamento.First(c => c.DPC_CODIGO == response.Data.dpc_inmueble).DPC_ID;
            }
            return response;
        }

        /// <summary>
        /// Obtiene el detalle de la simulación de un cliente
        /// </summary>
        /// <param name="simulacionId">Guid que identifica la simulación del cliente</param>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <returns></returns>
        public async Task<Response<IEnumerable<SimulacionDto>>> ObtenerDetalleSimulacionCliente(Guid simulacionId)
        {
            List<SimulacionDto> planillaSimulacion = new();
            var response = new Response<IEnumerable<DetalleSimulacionClienteDto>> { Data = this._mapper.Map<IEnumerable<DetalleSimulacionClienteDto>>(await this._creditoRepository.ObtenerDetalleSimulacionCliente(simulacionId)) };
            foreach (DetalleSimulacionClienteDto item in response.Data)
            {
                planillaSimulacion.Add(new SimulacionDto
                {
                    IdSimulacion = item.SimulacionId,
                    IdDetalleSimulacion = item.SimulacionDetalleId,
                    IdSimulacionCliente = item.SimulacionId,
                    Cuota = item.Cuota,
                    FechaCorte = item.FechaCorte,
                    FechaPago = item.FechaPago,
                    Saldo = (double)item.Saldo,
                    Capital = (double)item.Capital,
                    Intereses = (double)item.Intereses,
                    SeguroVida = (double)item.SeguroVida,
                    SeguroHogar = (double)item.SeguroTerremoto,
                    TotalCuota = (double)item.CuotaTotal,
                    Subsidio = 0,
                    AbonoExtra = (double)item.AbonoExtra,
                    CuotaUnidadPagadora = item.CuotaUnidadPagadora,
                    InteresesReduccion = (double)item.InteresesReduccion,
                    CuotaReduccion = (double)item.CuotaReduccion,
                    CobroFonvivienda = (double)item.CobroFonvivienda,
                    SaldoLeasing = item.SaldoLeasing,
                    InteresLeasing = item.InteresLeasing,
                    CapitalLeasing = item.CapitalLeasing,
                    CuotaLeasing = item.CuotaLeasing,
                    CuotaLeasingSubsidio = item.CuotaLeasingSubsidio,
                    InteresTradicionalSubsidio = item.InteresTradicionalSubsidio,
                    InteresLeasingSubsidio = item.InteresLeasingSubsidio,
                    ValorPrestamo = 0,
                    valorPrestamoLeasing = 0,
                    SeguroVidaLeasing = item.SeguroVidaLeasing,
                    ValorTasa = 0,
                    AbonoMensual = (double)item.AbonoMensual,
                    CuotaUnidadPagadorados = item.CuotaUnidadPagadora,
                    AbonoUnico = item.AbonoUnico,
                    AfectacionCuentaIndividual = item.AfectacionIndividualLeasing,
                    CapitalReducido = item.SaldoReducidoLeasing,
                    InteresesPorPagar = item.InteresesPagadosLeasing,
                    CapitalPorPagar = item.CapitalPagadoLeasing,
                    Capitalpagar = item.CapitalAPagarLeasing,
                    CapitalSaldo = item.SaldoCapitalLeasing,
                    CapitalInteresporPagar = item.InteresAPagarLeasing,
                    CapitalSaldoTotal = item.SalDoTotalLeasing,
                    InteresPorPagar = item.InteresesPagadosLeasing,
                    ValorFinanciar = item.ValorFinanciar,
                    PlazoAnos = item.plazoAnos,
                    PlnNroCredito = item.plnNroCredito,
                    RiesgoMinimoTransUnion = item.Score,
                    FechaDesembolso = item.FechaDesembolso
                });
            }
            return new Response<IEnumerable<SimulacionDto>> { Data = planillaSimulacion };
        }

        /// <summary>
        /// Carga los documentos para la solicitud de crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<string>> CargaDocumentoSolicCredito(IFormFile request)
        {
            try
            {
                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
                if (blogStorage.SRD_ESTADO)
                {
                    var _request = request;
                    FileInfo fi = new FileInfo(_request.FileName);
                    BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "credito");
                    await container.CreateIfNotExistsAsync();
                    var newName = Guid.NewGuid() + fi.Extension;
                    BlobClient blob = container.GetBlobClient(newName);
                    var result = await blob.UploadAsync(request.OpenReadStream());
                    return new Response<string> { Data = newName };
                }
                else
                {
                    return await this._localBlogStorageRepository.CargaDocumentoSolicCredito(request);
                }

            }
            catch (Exception)
            {
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardado documento en el Blob Storage" };
            }


        }


        /// <summary>
        /// Descarga los archivos de la solicitud de crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<FileResult> DescargaDocumentoSolicCredito(string filename)
        {
            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "credito");
                BlobClient blob = container.GetBlobClient(filename);
                BlobDownloadInfo download = await blob.DownloadAsync();

                var descargado = new FileStreamResult(download.Content, download.ContentType);
                descargado.FileDownloadName = filename;
                await descargado.FileStream.FlushAsync();
                return descargado;
            }
            else
            {
                return await _localBlogStorageRepository.DescargaDocumentoSolicCreditoDesdeCarpeta(filename);
            }

        }





        /// <summary>
        /// Obtiene el documento de los datos de solicitud Comite de crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="datosSolicitudComite"></param>
        /// <returns></returns>
        public async Task<FileResult> ObtenerDatosSolicComite(Guid guid)
        {

            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            if (blogStorage.SRD_ESTADO)
            {
                var fileBase = "8d2906d0-ac60-40cf-a25b-b5bae98fe50c.html";
                var cssBase = "3cd4863b-3b85-42ec-9027-25890b4fcf52.css";
                DatosSolicitudComite respuesta = await this._creditoRepository.ObtenerDatosSolicComite(guid);
                var simulacion = await this._creditoRepository.ConsultarSimulacionCliente(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
                var historial = await this._creditoRepository.ObtenerHistorialComercialBuro(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
                var financiera = await this._creditoRepository.ObtenerDatosFinancierosPersonales(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
                var informacionAQM = await _creditoRepository.ConsultarDatosEndeudamientoProspeccion(respuesta.SOB_NUMERO_DOCUMENTO.ToString());

                PdfCreator pdf = new PdfCreator();
                if (respuesta?.SOB_NUMERO_DOCUMENTO == null)
                {
                    throw new InvalidOperationException(nameof(respuesta));
                }
                else
                {
                    BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "credito");
                    BlobClient blobBase = container.GetBlobClient(fileBase);
                    BlobClient blobCss = container.GetBlobClient(cssBase);
                    BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                    BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                    var basefile = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                    var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                    await basefile.FileStream.FlushAsync();
                    await baseCss.FileStream.FlushAsync();

                    BlobContainerClient img = new BlobContainerClient(this._options.Value.CadenaOne, "imagenes-caja");
                    BlobClient header = img.GetBlobClient("Header.html");
                    BlobClient footer = img.GetBlobClient("Footer.html");

                    BlobDownloadInfo downloadHeader = await header.DownloadAsync();
                    BlobDownloadInfo downloadFooter = await footer.DownloadAsync();

                    var baseHeaderhtml = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                    var baseFooterhtml = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                    await baseHeaderhtml.FileStream.FlushAsync();
                    await baseFooterhtml.FileStream.FlushAsync();
                    string Header = await (new StreamReader(baseHeaderhtml.FileStream)).ReadToEndAsync();
                    string Footer = await (new StreamReader(baseFooterhtml.FileStream)).ReadToEndAsync();

                    var resultado = await pdf.GenerarDatosSolicComite(respuesta, simulacion, historial, basefile, baseCss, Header, Footer, financiera, informacionAQM);
                    return resultado;
                }
            }
            else
            {
                return await GenerarDatosSolicComiteLocal(guid);
            }
        }


        public async Task<FileResult> GenerarDatosSolicComiteLocal(Guid guid)
        {
            // Rutas locales de los archivos necesarios
            var fileBasePath = Path.Combine(_config.CarpetaServidorStorage + "\\credito", "8d2906d0-ac60-40cf-a25b-b5bae98fe50c.html");
            var cssBasePath = Path.Combine(_config.CarpetaServidorStorage + "\\credito", "3cd4863b-3b85-42ec-9027-25890b4fcf52.css");
            var headerPath = Path.Combine(_config.CarpetaServidorStorage + "\\imagenes-caja", "Header.html");
            var footerPath = Path.Combine(_config.CarpetaServidorStorage + "\\imagenes-caja", "Footer.html");

            // Obtener datos
            DatosSolicitudComite respuesta = await this._creditoRepository.ObtenerDatosSolicComite(guid);
            var simulacion = await this._creditoRepository.ConsultarSimulacionCliente(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
            var historial = await this._creditoRepository.ObtenerHistorialComercialBuro(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
            var financiera = await this._creditoRepository.ObtenerDatosFinancierosPersonales(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
            var informacionAQM = await _creditoRepository.ConsultarDatosEndeudamientoProspeccion(respuesta.SOB_NUMERO_DOCUMENTO.ToString()); 

            if (respuesta?.SOB_NUMERO_DOCUMENTO == null)
            {
                throw new InvalidOperationException(nameof(respuesta));
            }

            // Leer archivos locales
            var htmlStream = new FileStream(fileBasePath, FileMode.Open, FileAccess.Read);
            var cssStream = new FileStream(cssBasePath, FileMode.Open, FileAccess.Read);
            var headerHtml = await File.ReadAllTextAsync(headerPath);
            var footerHtml = await File.ReadAllTextAsync(footerPath);

            var baseHtml = new FileStreamResult(htmlStream, "text/html");
            var baseCss = new FileStreamResult(cssStream, "text/css");

            // Crear PDF
            PdfCreator pdf = new PdfCreator();
            var resultado = await pdf.GenerarDatosSolicComite(respuesta, simulacion, historial, baseHtml, baseCss, headerHtml, footerHtml, financiera, informacionAQM);

            return resultado;
        }



        /// <summary>
        /// Genera un pdf de un preaprobado
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FileResult> GenerarPDFPreAprobado(RequestPdf request)
        {
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "credito");
            BlobClient blobBase = container.GetBlobClient("0038c8b3-52a4-4b35-bcf6-d6e3b110d368.html");
            BlobClient blobCss = container.GetBlobClient("f040de05-083b-4699-be01-b66eaff830c1.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            var solicitud = await this.ConsultarSimulacionCliente(request.Documento);
            solicitud.Data.plazo = request.PlazoMeses;
            solicitud.Data.años = (double)Math.Round((decimal)(request.PlazoMeses / 12), 1);
            PdfCreator pdf = new PdfCreator();
            var result = await pdf.GenerarConfirmacionPreAprobado(solicitud.Data, baseHtlm, baseCss);
            return result;
        }

        /// <summary>
        /// Actualiza solicitud de credito Recomendacion
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<SolicCreditoRecomendacionDto>> ActualizarSolicCreditoRecomendacion(SolicCreditoRecomendacionDto request)
        {
            var solicitud = this._mapper.Map<SolicCreditoRecomendacion>(request);
            return new Response<SolicCreditoRecomendacionDto> { Data = this._mapper.Map<SolicCreditoRecomendacionDto>(await this._creditoRepository.ActualizaSolicCreditoRecomendacion(solicitud)) };
        }

        /// <summary>
        /// Consultar Solicitud Credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ConsultaSolicitudCreditoDto>> ConsultarSolicitudCredito(ConsultaSolicitudCreditoDto request)
        {
            var consulta = this._mapper.Map<ConsultaSolicitudCredito>(request);
            return new Response<ConsultaSolicitudCreditoDto> { Data = this._mapper.Map<ConsultaSolicitudCreditoDto>(await this._creditoRepository.ConsultarSolicitudCredito(consulta)) };
        }

        /// <summary>
        /// Actualiza informacion seguro en solicitud credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ConsultaSolicitudCreditoDto>> ActualizaSolicCreditoSeguro(DatosActualizaSolicCreditoSeguroDto request)
        {
            var datos = this._mapper.Map<DatosActualizaSolicCreditoSeguro>(request);
            datos.SOC_FECHA_ACTUALIZA = DateTime.Now;

            if (datos.SOC_CONVENIO_ASEGURADORA == string.Empty || datos.SOC_CONVENIO_ASEGURADORA == null) datos.SOC_CONVENIO_ASEGURADORA = "0";
            if (datos.SOC_CONVENIO_ASEGURADORA_HOGAR == string.Empty || datos.SOC_CONVENIO_ASEGURADORA_HOGAR == null) datos.SOC_CONVENIO_ASEGURADORA = "0";

            return new Response<ConsultaSolicitudCreditoDto> { Data = this._mapper.Map<ConsultaSolicitudCreditoDto>(await this._creditoRepository.ActualizaSolicCreditoSeguro(datos)) };
        }
        /// <summary>
        /// Crea la simulacion del credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<PlanillaSimulacionCreditoDto>>> CrearSimulacionCliente(CreacionSimulacionClienteDto request)
        {
            return new Response<IEnumerable<PlanillaSimulacionCreditoDto>> { Data = this._mapper.Map<IEnumerable<PlanillaSimulacionCreditoDto>>(await this._creditoRepository.CrearSimulacionCliente(request)) };
        }



        #region Consecutivo
        /// <summary>
        /// CrearConsecutivo
        /// </summary>
        /// <param name="consecutivo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ConsecutivoDto>> CrearConsecutivo(ConsecutivoDto consecutivo)
        {
            string codigoMensaje = "";
            var responseData = this._mapper.Map<ConsecutivoDto>(
                    await this._creditoRepository.CrearConsecutivo(
                         this._mapper.Map<Consecutivo>(consecutivo)));
            if (responseData != null)
            {
                codigoMensaje = ResponseMessageConstants.CodigoMensaje1;
            }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ConsecutivoDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje

            };
        }



        /// <summary>
        /// ActualizarConsecutivo
        /// </summary>
        /// <param name="consecutivo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ConsecutivoDto>> ActualizarConsecutivo(ConsecutivoDto consecutivo)
        {
            var ResponseData = this._mapper.Map<ConsecutivoDto>(
                    await this._creditoRepository.ActualizarConsecutivo(
                         this._mapper.Map<Consecutivo>(consecutivo)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; };
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ConsecutivoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerConsecutivoPorId
        /// </summary>
        /// <param name="consecutivoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ConsecutivoDto>> ObtenerConsecutivoPorId(string consecutivoid)
        {
            var ResponseData = this._mapper.Map<ConsecutivoDto>(
                    await this._creditoRepository.ObtenerConsecutivoPorId(consecutivoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);

            return new Response<ConsecutivoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }

        /// <summary>
        /// ObtenerConsecutivo
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<ConsecutivoDto>>> ObtenerConsecutivo()
        {
            IEnumerable<ConsecutivoDto> responseData = this._mapper.Map<IEnumerable<ConsecutivoDto>>(
                await this._creditoRepository.ObtenerConsecutivo());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje1;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<ConsecutivoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        /// <summary>
        /// EliminarConsecutivoPorid
        /// </summary>
        /// <param name="consecutivoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<ConsecutivoDto>> EliminarConsecutivoPorid(string consecutivoid)
        {
            var ResponseData = this._mapper.Map<ConsecutivoDto>(
                    await this._creditoRepository.EliminarConsecutivoPorid(consecutivoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ConsecutivoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        #endregion

        #region Aseguradoras
        /// <summary>
        /// CrearAseguradoras
        /// </summary>
        /// <param name="aseguradoras"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<AseguradorasDto>> CrearAseguradoras(AseguradorasDto aseguradoras)
        {
            var ResponseData = this._mapper.Map<AseguradorasDto>(
                    await this._creditoRepository.CrearAseguradoras(
                         this._mapper.Map<Aseguradoras>(aseguradoras)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AseguradorasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarAseguradoras
        /// </summary>
        /// <param name="aseguradoras"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<AseguradorasDto>> ActualizarAseguradoras(AseguradorasDto aseguradoras)
        {
            var ResponseData = this._mapper.Map<AseguradorasDto>(
                    await this._creditoRepository.ActualizarAseguradoras(
                         this._mapper.Map<Aseguradoras>(aseguradoras)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AseguradorasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerAseguradorasPorId
        /// </summary>
        /// <param name="aseguradorasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<AseguradorasDto>> ObtenerAseguradorasPorId(string aseguradorasid)
        {
            var ResponseData = this._mapper.Map<AseguradorasDto>(
                    await this._creditoRepository.ObtenerAseguradorasPorId(aseguradorasid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AseguradorasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarAseguradorasPorid
        /// </summary>
        /// <param name="aseguradorasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<AseguradorasDto>> EliminarAseguradorasPorid(string aseguradorasid)
        {
            var ResponseData = this._mapper.Map<AseguradorasDto>(
                    await this._creditoRepository.EliminarAseguradorasPorid(aseguradorasid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AseguradorasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerAseguradoras
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<AseguradorasDto>>> ObtenerAseguradoras()
        {

            IEnumerable<AseguradorasDto> ResponseData = this._mapper.Map<IEnumerable<AseguradorasDto>>(
                await this._creditoRepository.ObtenerAseguradoras());
            string CodigoMensaje = ResponseMessageConstants.CodigoMensaje1;
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<AseguradorasDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }



        /// <summary>
        /// ActualizarDatosPersonalesSolicitud
        /// </summary>
        /// <author>DG</author>
        /// <date>05/01/2024</date>
        /// 

        public async Task<Response<SimulacionDatosPersonalesDto>> ActualizarDatosPersonalesSolicitud(SimulacionDatosPersonalesDto request)
        {
            var peticion = this._mapper.Map<SimulacionDatosPersonales>(request);

            return new Response<SimulacionDatosPersonalesDto>() { Data = _mapper.Map<SimulacionDatosPersonalesDto>(await this._creditoRepository.ActualizarDatosPersonalesSolicitud(peticion)) };
        }





        #endregion

        #region AlertaAutomaticas
        /// <summary>
        /// CrearAlertaAutomaticas
        /// </summary>
        /// <param name="alertaautomaticas"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<AlertaAutomaticasDto>> CrearAlertaAutomaticas(AlertaAutomaticasDto alertaautomaticas)
        {
            var ResponseData = this._mapper.Map<AlertaAutomaticasDto>(
                    await this._creditoRepository.CrearAlertaAutomaticas(
                         this._mapper.Map<AlertaAutomaticas>(alertaautomaticas)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AlertaAutomaticasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarAlertaAutomaticas
        /// </summary>
        /// <param name="alertaautomaticas"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<Response<AlertaAutomaticasDto>> ActualizarAlertaAutomaticas(AlertaAutomaticasDto alertaautomaticas)
        {
            var ResponseData = this._mapper.Map<AlertaAutomaticasDto>(
                    await this._creditoRepository.ActualizarAlertaAutomaticas(
                         this._mapper.Map<AlertaAutomaticas>(alertaautomaticas)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AlertaAutomaticasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje

            };
        }
        /// <summary>
        /// ObtenerAlertaAutomaticasPorId
        /// </summary>
        /// <param name="alertaautomaticasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<AlertaAutomaticasDto>> ObtenerAlertaAutomaticasPorId(string alertaautomaticasid)
        {
            var ResponseData = this._mapper.Map<AlertaAutomaticasDto>(
                    await this._creditoRepository.ObtenerAlertaAutomaticasPorId(alertaautomaticasid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AlertaAutomaticasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarAlertaAutomaticasPorid
        /// </summary>
        /// <param name="alertaautomaticasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<AlertaAutomaticasDto>> EliminarAlertaAutomaticasPorid(string alertaautomaticasid)
        {
            var ResponseData = this._mapper.Map<AlertaAutomaticasDto>(
                    await this._creditoRepository.EliminarAlertaAutomaticasPorid(alertaautomaticasid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<AlertaAutomaticasDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerAlertaAutomaticas
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<AlertaAutomaticasDto>>> ObtenerAlertaAutomaticas()
        {
            IEnumerable<AlertaAutomaticasDto> ResponseData =
                this._mapper.Map<IEnumerable<AlertaAutomaticasDto>>(await this._creditoRepository.ObtenerAlertaAutomaticas());
            string CodigoMensaje = ResponseMessageConstants.CodigoMensaje1;
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<AlertaAutomaticasDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }


        #endregion

        #region ColorGrilla
        /// <summary>
        /// CrearColorGrilla
        /// </summary>
        /// <param name="colorgrilla"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ColorGrillaDto>> CrearColorGrilla(ColorGrillaDto colorgrilla)
        {
            var ResponseData = this._mapper.Map<ColorGrillaDto>(
                    await this._creditoRepository.CrearColorGrilla(
                         this._mapper.Map<ColorGrilla>(colorgrilla)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ColorGrillaDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarColorGrilla
        /// </summary>
        /// <param name="colorGrilla"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ColorGrillaDto>> ActualizarColorGrilla(ColorGrillaDto colorgrilla)
        {
            var ResponseData = this._mapper.Map<ColorGrillaDto>(
                    await this._creditoRepository.ActualizarColorGrilla(
                         this._mapper.Map<ColorGrilla>(colorgrilla)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ColorGrillaDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerColorGrillaPorId
        /// </summary>
        /// <param name="colorGrillaId"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ColorGrillaDto>> ObtenerColorGrillaPorId(string colorgrillaId)
        {
            var ResponseData = this._mapper.Map<ColorGrillaDto>(
                    await this._creditoRepository.ObtenerColorGrillaPorId(colorgrillaId));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ColorGrillaDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarColorGrillaPorid
        /// </summary>
        /// <param name="colorgrillaId"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<ColorGrillaDto>> EliminarColorGrillaPorid(string colorgrillaId)
        {
            var ResponseData = this._mapper.Map<ColorGrillaDto>(
                    await this._creditoRepository.EliminarColorGrillaPorid(colorgrillaId));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ColorGrillaDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerColorGrilla
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<ColorGrillaDto>>> ObtenerColorGrilla()
        {
            IEnumerable<ColorGrillaDto> ResponseData =
                this._mapper.Map<IEnumerable<ColorGrillaDto>>(await this._creditoRepository.ObtenerColorGrilla());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<ColorGrillaDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }


        #endregion

        #region Estado Actividad
        /// <summary>
        /// CrearEstadoActividad
        /// </summary>
        /// <param name="estadoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoActividadDto>> CrearEstadoActividad(EstadoActividadDto estadoActividad)
        {
            var ResponseData = this._mapper.Map<EstadoActividadDto>(
                    await this._creditoRepository.CrearEstadoActividad(
                         this._mapper.Map<EstadoActividad>(estadoActividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarEstadoActividad
        /// </summary>
        /// <param name="estadoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoActividadDto>> ActualizarEstadoActividad(EstadoActividadDto estadoActividad)
        {
            var ResponseData = this._mapper.Map<EstadoActividadDto>(
                    await this._creditoRepository.ActualizarEstadoActividad(
                         this._mapper.Map<EstadoActividad>(estadoActividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerEstadoActividadPorId
        /// </summary>
        /// <param name="estadoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoActividadDto>> ObtenerEstadoActividadPorId(string estadoActividadid)
        {
            var ResponseData = this._mapper.Map<EstadoActividadDto>(
                    await this._creditoRepository.ObtenerEstadoActividadPorId(estadoActividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarEstadoActividadPorid
        /// </summary>
        /// <param name="estadoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<EstadoActividadDto>> EliminarEstadoActividadPorid(string estadoActividadid)
        {
            var ResponseData = this._mapper.Map<EstadoActividadDto>(
                    await this._creditoRepository.EliminarEstadoActividadPorid(estadoActividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerEstadoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<EstadoActividadDto>>> ObtenerEstadoActividad()
        {
            IEnumerable<EstadoActividadDto> ResponseData =
                this._mapper.Map<IEnumerable<EstadoActividadDto>>(await this._creditoRepository.ObtenerEstadoActividad());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);

            return new Response<IEnumerable<EstadoActividadDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje

            };
        }


        #endregion

        #region Estado Solicitud
        /// <summary>
        /// CrearEstadoSolicitud
        /// </summary>
        /// <param name="estadosolicitud"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoSolicitudDto>> CrearEstadoSolicitud(EstadoSolicitudDto estadosolicitud)
        {
            var ResponseData = this._mapper.Map<EstadoSolicitudDto>(
                    await this._creditoRepository.CrearEstadoSolicitud(
                         this._mapper.Map<EstadoSolicitud>(estadosolicitud)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoSolicitudDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarEstadoSolicitud
        /// </summary>
        /// <param name="estadosolicitud"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoSolicitudDto>> ActualizarEstadoSolicitud(EstadoSolicitudDto estadosolicitud)
        {
            var ResponseData = this._mapper.Map<EstadoSolicitudDto>(
                    await this._creditoRepository.ActualizarEstadoSolicitud(
                         this._mapper.Map<EstadoSolicitud>(estadosolicitud)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoSolicitudDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje

            };
        }
        /// <summary>
        /// ObtenerEstadoSolicitudPorId
        /// </summary>
        /// <param name="estadosolicitudid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<EstadoSolicitudDto>> ObtenerEstadoSolicitudPorId(string estadosolicitudid)
        {
            var ResponseData = this._mapper.Map<EstadoSolicitudDto>(
                    await this._creditoRepository.ObtenerEstadoSolicitudPorId(estadosolicitudid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoSolicitudDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarEstadoSolicitudPorid
        /// </summary>
        /// <param name="estadosolicitudid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<EstadoSolicitudDto>> EliminarEstadoSolicitudPorid(string estadosolicitudid)
        {
            var ResponseData = this._mapper.Map<EstadoSolicitudDto>(
                    await this._creditoRepository.EliminarEstadoSolicitudPorid(estadosolicitudid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<EstadoSolicitudDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerEstadoSolicitud
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<EstadoSolicitudDto>>> ObtenerEstadoSolicitud()
        {
            IEnumerable<EstadoSolicitudDto> ResponseData =
                this._mapper.Map<IEnumerable<EstadoSolicitudDto>>(await this._creditoRepository.ObtenerEstadoSolicitud());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<EstadoSolicitudDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }


        #endregion

        #region Tipo Actividad
        /// <summary>
        /// CrearTipoActividad
        /// </summary>
        /// <param name="tipoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<TipoActividadDto>> CrearTipoActividad(TipoActividadDto tipoActividad)
        {
            var ResponseData = this._mapper.Map<TipoActividadDto>(
                    await this._creditoRepository.CrearTipoActividad(
                         this._mapper.Map<TipoActividad>(tipoActividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarTipoActividad
        /// </summary>
        /// <param name="TipoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<TipoActividadDto>> ActualizarTipoActividad(TipoActividadDto tipoActividad)
        {
            var ResponseData = this._mapper.Map<TipoActividadDto>(
                    await this._creditoRepository.ActualizarTipoActividad(
                         this._mapper.Map<TipoActividad>(tipoActividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerTipoActividadPorId
        /// </summary>
        /// <param name="tipoActividadid"></param>
        /// /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoActividadDto>>> ObtenerTipoActividadPorId(string tipoActividadid)
        {
            IEnumerable<TipoActividad> ResponseData = await this._creditoRepository.ObtenerTipoActividadPorId(tipoActividadid);

            List<TipoActividadDto> listaTipoActividad = new List<TipoActividadDto>();
            foreach (var item in ResponseData)
            {
                TipoActividadDto actividad = new TipoActividadDto();

                actividad.Id = item.TAC_ID;
                actividad.Descripcion = item.TAC_DESCRIPCION;

                listaTipoActividad.Add(actividad);
            }
            string codigoMensaje = "";
            if (ResponseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<TipoActividadDto>>
            {
                Data = listaTipoActividad,
                ReturnMessage = Mensaje.Mensaje
            };

        }

        /// <summary>
        /// EliminarTipoActividadPorid
        /// </summary>
        /// <param name="tipoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// <returns></returns>
        public async Task<Response<TipoActividadDto>> EliminarTipoActividadPorid(string tipoActividadid)
        {
            var ResponseData = this._mapper.Map<TipoActividadDto>(
                    await this._creditoRepository.EliminarTipoActividadPorid(tipoActividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerTipoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<TipoActividadDto>>> ObtenerTipoActividad(int AFL_ID)
        {
            IEnumerable<TipoActividadDto> ResponseData =
                this._mapper.Map<IEnumerable<TipoActividadDto>>(await this._creditoRepository.ObtenerTipoActividad(AFL_ID));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<TipoActividadDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        public async Task<Response<TipoActividadDto>> CrearMatriculaCredito(TipoActividadDto tipoActividad)
        {
            var ResponseData = this._mapper.Map<TipoActividadDto>(
                    await this._creditoRepository.CrearTipoActividad(
                         this._mapper.Map<TipoActividad>(tipoActividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }


        #endregion

        #region Tipo Documento
        /// <summary>
        /// CrearTipoDocumento
        /// </summary>
        /// <param name="tipodocumento"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<TipoDocumentoDto>> CrearTipoDocumento(TipoDocumentoDto tipodocumento)
        {
            var ResponseData = this._mapper.Map<TipoDocumentoDto>(
                    await this._creditoRepository.CrearTipoDocumento(
                         this._mapper.Map<TipoDocumento>(tipodocumento)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoDocumentoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarTipoDocumento
        /// </summary>
        /// <param name="tipodocumento"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<TipoDocumentoDto>> ActualizarTipoDocumento(TipoDocumentoDto tipodocumento)
        {
            var ResponseData = this._mapper.Map<TipoDocumentoDto>(
                    await this._creditoRepository.ActualizarTipoDocumento(
                         this._mapper.Map<TipoDocumento>(tipodocumento)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoDocumentoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerTipoDocumentoPorId
        /// </summary>
        /// <param name="tipodocumentoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<TipoDocumentoDto>> ObtenerTipoDocumentoPorId(string tipodocumentoid)
        {
            TipoDocumentoDto ResponseData = new TipoDocumentoDto();
            try
            {
                ResponseData = this._mapper.Map<TipoDocumentoDto>(
                    await this._creditoRepository.ObtenerTipoDocumentoPorId(tipodocumentoid));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipo de documento.");
            }

            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoDocumentoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarTipoDocumentoPorid
        /// </summary>
        /// <param name="tipodocumentoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<TipoDocumentoDto>> EliminarTipoDocumentoPorid(string tipodocumentoid)
        {
            var ResponseData = this._mapper.Map<TipoDocumentoDto>(
                    await this._creditoRepository.EliminarTipoDocumentoPorid(tipodocumentoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<TipoDocumentoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerTipoDocumento
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<IEnumerable<TipoDocumentoDto>>> ObtenerTipoDocumento()
        {
            IEnumerable<TipoDocumentoDto> ResponseData =
                this._mapper.Map<IEnumerable<TipoDocumentoDto>>(await this._creditoRepository.ObtenerTipoDocumento());
            string codigoMensaje = "";
            if (ResponseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<TipoDocumentoDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        public async Task<Response<IEnumerable<TiposArchivosDto>>> ObtenerTiposArchivos()
        {
            IEnumerable<TiposArchivosDto> ResponseData =
                this._mapper.Map<IEnumerable<TiposArchivosDto>>(await this._creditoRepository.ObtenerTiposArchivos());
            string codigoMensaje = "";
            if (ResponseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<TiposArchivosDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        #endregion

        #region Flujo

        /// <summary>
        /// CrearFlujo
        /// </summary>
        /// <param name="flujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoDto>> CrearFlujo(FlujoDto flujo)
        {
            var ResponseData = this._mapper.Map<FlujoDto>(
                    await this._creditoRepository.CrearFlujo(
                         this._mapper.Map<Flujo>(flujo)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);

            return new Response<FlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarFlujo
        /// </summary>
        /// <param name="flujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoDto>> ActualizarFlujo(FlujoDto flujo)
        {
            var ResponseData = this._mapper.Map<FlujoDto>(
                    await this._creditoRepository.ActualizarFlujo(
                         this._mapper.Map<Flujo>(flujo)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerFlujoPorId
        /// </summary>
        /// <param name="flujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoDto>> ObtenerFlujoPorId(string flujoid)
        {
            var ResponseData = this._mapper.Map<FlujoDto>(
                    await this._creditoRepository.ObtenerFlujoPorId(flujoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarFlujoPorid
        /// </summary>
        /// <param name="flujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<FlujoDto>> EliminarFlujoPorid(string flujoid)
        {
            var ResponseData = this._mapper.Map<FlujoDto>(
                    await this._creditoRepository.EliminarFlujoPorid(flujoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerFlujo
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<IEnumerable<FlujoDto>>> ObtenerFlujo()
        {
            IEnumerable<FlujoDto> Responsedata =
                this._mapper.Map<IEnumerable<FlujoDto>>(await this._creditoRepository.ObtenerFlujo());
            string CodigoMensaje = "";
            if (Responsedata != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<FlujoDto>>
            {
                Data = Responsedata,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        #endregion

        #region Flujo Tipo Credito
        /// <summary>
        /// CrearFlujoTipoCredito
        /// </summary>
        /// <param name="flujotipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoTipoCreditoDto>> CrearFlujoTipoCredito(FlujoTipoCreditoDto flujotipoCredito)
        {
            var ResponseData = this._mapper.Map<FlujoTipoCreditoDto>(
                    await this._creditoRepository.CrearFlujoTipoCredito(
                         this._mapper.Map<FlujoTipoCredito>(flujotipoCredito)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoTipoCreditoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ActualizarFlujoTipoCredito
        /// </summary>
        /// <param name="flujotipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoTipoCreditoDto>> ActualizarFlujoTipoCredito(FlujoTipoCreditoDto flujotipoCredito)
        {
            var ResponseData = this._mapper.Map<FlujoTipoCreditoDto>(
                    await this._creditoRepository.ActualizarFlujoTipoCredito(
                         this._mapper.Map<FlujoTipoCredito>(flujotipoCredito)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoTipoCreditoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerFlujoTipoCreditoPorId
        /// </summary>
        /// <param name="flujotipoCreditoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<FlujoTipoCreditoDto>> ObtenerFlujoTipoCreditoPorId(string flujotipoCreditoid)
        {
            var ResponseData = this._mapper.Map<FlujoTipoCreditoDto>(
                    await this._creditoRepository.ObtenerFlujoTipoCreditoPorId(flujotipoCreditoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoTipoCreditoDto>
            {
                Data = this._mapper.Map<FlujoTipoCreditoDto>(
                    await this._creditoRepository.ObtenerFlujoTipoCreditoPorId(flujotipoCreditoid))
            };

        }
        /// <summary>
        /// EliminarFlujoTipoCreditoPorid
        /// </summary>
        /// <param name="flujotipoCreditoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<FlujoTipoCreditoDto>> EliminarFlujoTipoCreditoPorid(string flujotipoCreditoid)
        {
            var ResponseDate = this._mapper.Map<FlujoTipoCreditoDto>(
                    await this._creditoRepository.EliminarFlujoTipoCreditoPorid(flujotipoCreditoid));
            string CodigoMensaje = "";
            if (ResponseDate != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<FlujoTipoCreditoDto>
            {
                Data = ResponseDate,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerFlujoTipoCredito
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<IEnumerable<FlujoTipoCreditoDto>>> ObtenerFlujoTipoCredito()
        {
            IEnumerable<FlujoTipoCreditoDto> ResponseData =
                this._mapper.Map<IEnumerable<FlujoTipoCreditoDto>>(await this._creditoRepository.ObtenerFlujoTipoCredito());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<FlujoTipoCreditoDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        #endregion

        #region Actividad Flujo
        /// <summary>
        /// CrearActividadFlujo
        /// </summary>
        /// <param name="actividadflujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<ActividadFlujoDto>> CrearActividadFlujo(ActividadFlujoDto actividadflujo)
        {
            var ResponseData = this._mapper.Map<ActividadFlujoDto>(
                    await this._creditoRepository.CrearActividadFlujo(
                         this._mapper.Map<ActividadFlujo>(actividadflujo)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ActividadFlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerActividadFlujoPorId
        /// </summary>
        /// <param name="actividadflujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<ActividadFlujoDto>> ObtenerActividadFlujoPorId(string actividadflujoid)
        {
            var ResponseData = this._mapper.Map<ActividadFlujoDto>(
                    await this._creditoRepository.ObtenerActividadFlujoPorId(actividadflujoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ActividadFlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarActividadFlujoPorid
        /// </summary>
        /// <param name="actividadflujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<ActividadFlujoDto>> EliminarActividadFlujoPorid(string actividadflujoid)
        {
            var ResponseData = this._mapper.Map<ActividadFlujoDto>(
                    await this._creditoRepository.EliminarActividadFlujoPorid(actividadflujoid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<ActividadFlujoDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerActividadFlujo
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 


        public async Task<Response<IEnumerable<ActividadFlujoDto>>> ObtenerActividadFlujo()
        {
            IEnumerable<ActividadFlujoDto> ResponseData =
                this._mapper.Map<IEnumerable<ActividadFlujoDto>>(await this._creditoRepository.ObtenerActividadFlujo());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<ActividadFlujoDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        #endregion

        #region Usuario Actividad
        /// <summary>
        /// CrearUsuarioActividad
        /// </summary>
        /// <param name="usuarioactividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<UsuarioActividadDto>> CrearUsuarioActividad(UsuarioActividadDto usuarioactividad)
        {
            var ResponseData = this._mapper.Map<UsuarioActividadDto>(
                    await this._creditoRepository.CrearUsuarioActividad(
                         this._mapper.Map<UsuarioActividad>(usuarioactividad)));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<UsuarioActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerUsuarioActividadPorId
        /// </summary>
        /// <param name="usuarioactividadId"></param>
        /// 
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Response<UsuarioActividadDto>> ObtenerUsuarioActividadPorId(string usuarioactividadid)
        {
            var ResponseData = this._mapper.Map<UsuarioActividadDto>(
                    await this._creditoRepository.ObtenerUsuarioActividadPorId(usuarioactividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<UsuarioActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarUsuarioActividadPorid
        /// </summary>
        /// <param name="usuarioactividadId"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<UsuarioActividadDto>> EliminarUsuarioActividadPorid(string usuarioactividadid)
        {
            var ResponseData = this._mapper.Map<UsuarioActividadDto>(
                    await this._creditoRepository.EliminarUsuarioActividadPorid(usuarioactividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<UsuarioActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerUsuarioActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<IEnumerable<UsuarioActividadDto>>> ObtenerUsuarioActividad()
        {
            IEnumerable<UsuarioActividadDto> ResponseData =
                this._mapper.Map<IEnumerable<UsuarioActividadDto>>(await this._creditoRepository.ObtenerUsuarioActividad());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<UsuarioActividadDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje

            };
        }

        #endregion

        #region Documento Actividad
        /// <summary>
        /// CrearDocumentoActividad
        /// </summary>
        /// <param name="documentoactividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<DocumentoActividadDto>> CrearDocumentoActividad(DocumentoActividadDto documentoactividad)
        {
            var ResponseData = this._mapper.Map<DocumentoActividadDto>(
                    await this._creditoRepository.CrearDocumentoActividad(
                         this._mapper.Map<DocumentoActividad>(documentoactividad)));
            string CodigoMenaje = "";
            if (ResponseData != null) { CodigoMenaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMenaje);
            return new Response<DocumentoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }
        /// <summary>
        /// ObtenerDocumentoActividadPorId
        /// </summary>
        /// <param name="documentoactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<DocumentoActividadDto>> ObtenerDocumentoActividadPorId(string documentoactividadid)
        {

            var ResponseData = this._mapper.Map<DocumentoActividadDto>(
                    await this._creditoRepository.ObtenerDocumentoActividadPorId(documentoactividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<DocumentoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };

        }
        /// <summary>
        /// EliminarDocumentoActividadPorid
        /// </summary>
        /// <param name="documentoactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<DocumentoActividadDto>> EliminarDocumentoActividadPorid(string documentoactividadid)
        {
            var ResponseData = this._mapper.Map<DocumentoActividadDto>(
                    await this._creditoRepository.EliminarDocumentoActividadPorid(documentoactividadid));
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<DocumentoActividadDto>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerDocumentoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<IEnumerable<DocumentoActividadDto>>> ObtenerDocumentoActividad()
        {

            IEnumerable<DocumentoActividadDto> ResponseData =
                this._mapper.Map<IEnumerable<DocumentoActividadDto>>
                (await this._creditoRepository.ObtenerDocumentoActividad());
            string CodigoMensaje = "";
            if (ResponseData != null) { CodigoMensaje = ResponseMessageConstants.CodigoMensaje1; }
            MensajeDto Mensaje = await ObtenerMensaje(CodigoMensaje);
            return new Response<IEnumerable<DocumentoActividadDto>>
            {
                Data = ResponseData,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        #endregion

        /// <summary>
        /// Obtiene los datos del comite de crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<Response<List<DatosComiteCreditoDto>>> ObtenerComiteCredito(DatosComiteCreditoDto datosComiteCredito)
        {
            var datos = this._mapper.Map<DatosComiteCredito>(datosComiteCredito);
            return new Response<List<DatosComiteCreditoDto>> { Data = this._mapper.Map<List<DatosComiteCreditoDto>>(await this._creditoRepository.ObtenerComiteCredito(datos)) };
        }

        /// <summary>
        /// Aprueba comite de credito
        /// </summary>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<Response<DatosComiteCreditoDto>> AprobarComiteCredito(DatosComiteCreditoDto datosComiteCredito)
        {
            var datos = this._mapper.Map<DatosComiteCredito>(datosComiteCredito);
            return new Response<DatosComiteCreditoDto> { Data = this._mapper.Map<DatosComiteCreditoDto>(await this._creditoRepository.AprobarComiteCredito(datos)) };
        }

        /// <summary>
        /// Obtiene la lista de integrantes del comité de credito
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<IntegranteComiteDto>>> ObtenerIntegranteComite()
        {
            return new Response<IEnumerable<IntegranteComiteDto>> { Data = this._mapper.Map<IEnumerable<IntegranteComiteDto>>(await this._creditoRepository.ObtenerIntegranteComite()) };
        }

        /// <summary>
        /// Obtiene la lista de temas del comité de credito
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TemaComiteDto>>> ObtenerTemasComite()
        {
            return new Response<IEnumerable<TemaComiteDto>> { Data = this._mapper.Map<IEnumerable<TemaComiteDto>>(await this._creditoRepository.ObtenerTemasComite()) };
        }

        /// <summary>
        /// Obtiene los integrantes de un comite según el comité
        /// </summary>
        /// <param name="integranteComite"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<IntegrantePorComiteDto>>> ObtenerComiteCreditoIntegrante(IntegrantePorComiteDto integrantePorComite)
        {
            var datos = this._mapper.Map<IntegrantePorComite>(integrantePorComite);
            return new Response<IEnumerable<IntegrantePorComiteDto>> { Data = this._mapper.Map<IEnumerable<IntegrantePorComiteDto>>(await this._creditoRepository.ObtenerComiteCreditoIntegrante(datos)) };
        }

        /// <summary>
        /// Crea comité de crédito
        /// </summary>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<Response<DatosComiteCreditoDto>> CrearComiteCredito(DatosComiteCreditoDto datosComiteCredito)
        {
            if (datosComiteCredito.ComiteIntegrantesDto != null && datosComiteCredito.TemasComiteCreditoDto != null)
            {
                var Datos = this._mapper.Map<DatosComiteCredito>(datosComiteCredito);
                var DatosComite = this._mapper.Map<DatosComiteCreditoDto>(await this._creditoRepository.CrearComiteCredito(Datos));

                List<ComiteIntegranteDto> ListaIntegrantesComite = new List<ComiteIntegranteDto>();
                foreach (var Integrante in datosComiteCredito.ComiteIntegrantesDto)
                {
                    Integrante.idComiteCredito = DatosComite.CodigoComite;
                    Integrante.creadoPor = datosComiteCredito.CreadoPor;

                    var DatosIntegrante = this._mapper.Map<ComiteIntegrante>(Integrante);
                    var IntegranteCreado = this._mapper.Map<ComiteIntegranteDto>(await this._creditoRepository.CrearComiteCreditoIntegrante(DatosIntegrante));

                    ListaIntegrantesComite.Add(IntegranteCreado);
                }

                List<TemaComiteCreditoDto> ListaTemasComite = new List<TemaComiteCreditoDto>();
                var contadorTemas = 1;
                foreach (var Tema in datosComiteCredito.TemasComiteCreditoDto)
                {
                    var DatosTema = this._mapper.Map<TemaComiteCredito>(Tema);
                    DatosTema.COT_CREADO_POR = datosComiteCredito.CreadoPor;
                    DatosTema.COT_FECHA_CREACION = datosComiteCredito.FechaCreacion;
                    DatosTema.COC_ID = DatosComite.CodigoComite;
                    DatosTema.TCO_ID = contadorTemas;
                    ListaTemasComite.Add(this._mapper.Map<TemaComiteCreditoDto>(await this._creditoRepository.CrearComiteCreditoTemas(DatosTema)));
                    contadorTemas++;
                }

                DatosComite.ComiteIntegrantesDto = ListaIntegrantesComite;
                DatosComite.TemasComiteCreditoDto = ListaTemasComite;
                return new Response<DatosComiteCreditoDto> { Data = DatosComite };
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Crea integrante para comité de credito
        /// </summary>
        /// <param name="integrante"></param>
        /// <returns></returns>
        public async Task<Response<ComiteIntegranteDto>> CrearComiteCreditoIntegrante(ComiteIntegranteDto integrante)
        {
            var datos = this._mapper.Map<ComiteIntegrante>(integrante);
            return new Response<ComiteIntegranteDto> { Data = this._mapper.Map<ComiteIntegranteDto>(await this._creditoRepository.CrearComiteCreditoIntegrante(datos)) };
        }

        /// <summary>
        /// Crea tema para comite de credito
        /// </summary>
        /// <param name="temaComiteCredito"></param>
        /// <returns></returns>
        public async Task<Response<TemaComiteCreditoDto>> CrearComiteCreditoTemas(TemaComiteCreditoDto temaComiteCredito)
        {
            var datos = this._mapper.Map<TemaComiteCredito>(temaComiteCredito);
            return new Response<TemaComiteCreditoDto> { Data = this._mapper.Map<TemaComiteCreditoDto>(await this._creditoRepository.CrearComiteCreditoTemas(datos)) };
        }

        /// <summary>
        /// Obtiene Solicitud Comite Credito
        /// </summary>
        /// <param name="solicitudComite"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<SolicitudComiteDto>>> ObtenerComiteCreditoSolicitud(SolicitudComiteDto solicitudComite)
        {
            var datos = this._mapper.Map<SolicitudComite>(solicitudComite);
            return new Response<IEnumerable<SolicitudComiteDto>> { Data = this._mapper.Map<IEnumerable<SolicitudComiteDto>>(await this._creditoRepository.ObtenerComiteCreditoSolicitud(datos)) };
        }

        /// <summary>
        /// Crear solicitud comite credito
        /// </summary>
        /// <param name="solicitudComite"></param>
        /// <returns></returns>
        public async Task<Response<SolicitudComiteCreacionDto>> CrearComiteCreditoSolicitud(SolicitudComiteCreacionDto solicitudComite)
        {
            if (solicitudComite.aprobada.ToUpper() != solicitudComite.rechazada.ToUpper())
            {
                var datos = this._mapper.Map<SolicitudComiteCreacion>(solicitudComite);
                return new Response<SolicitudComiteCreacionDto> { Data = this._mapper.Map<SolicitudComiteCreacionDto>(await this._creditoRepository.CrearComiteCreditoSolicitud(datos)) };

            }
            else return new Response<SolicitudComiteCreacionDto> { ReturnMessage = "Aprobado y rechazado no pueden ser iguales" };
        }

        /// <summary>
        /// Obtener Historial Comercial Buro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FileResult> ObtenerHistorialComercialBuro(RequestTuDto request)
        {
            request.IdValidation = Guid.NewGuid().ToString();
            var optionsAqm = _optionsAqm.Value;
            double diferenciaDias = 0;
            HistorialCredito consulta = new HistorialCredito();


            try
            {
                consulta = await this._creditoRepository.ObtenerHistorialComercialBuro(request.IdNumber);
                diferenciaDias = (DateTime.Now - consulta.CBC_FECHA_CONSULTA).TotalDays;
            }
            catch (Exception)
            {
                diferenciaDias = 0;
            }
            if (optionsAqm.estado)
            {
                if (diferenciaDias > 90 || diferenciaDias == 0)
                {
                    GestorIdVisionBusinessLogic ConsultasTransUnion = new GestorIdVisionBusinessLogic();

                    request.usrId = optionsAqm.usuarioAqm;
                    request.pasword = optionsAqm.claveAqm;
                    request.solucionId = optionsAqm.solucionIdAqm;
                    var datosPersonales = await this._creditoRepository.ObtenerSimulacionPersonales(request.IdNumber);
                    var fianancieros = await this.ObtenerDatosFinancierosPersonales(request.IdNumber);
                    request.LastName = datosPersonales.SDP_NOMBRES_APELLIDOS;
                    request.RecentPhoneNumber = datosPersonales.SDP_TELEFONO_CELULAR;
                    request.sueldo = fianancieros.Data.ValorSalario.ToString();
                    var ScoreCliente = await ConsultasTransUnion.obtenerScoreCliente(request);
                    request.idAplication = ScoreCliente.Data.idAplication;
                    var HistorialCliente = await ConsultasTransUnion.obtenerHistorialCliente(request);
                    consulta = await this._creditoRepository.ObtenerHistorialComercialBuro(request.IdNumber);
                }
            }

            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "credito");
            BlobClient blobBase = container.GetBlobClient(@"f424b2cf-2b73-444a-b07e-af13264050d3.html");
            BlobClient blobCss = container.GetBlobClient(@"9ed25983-6561-44f4-b8dd-deecda8e0050.css");
            BlobClient BaseCuentasVigentes = container.GetBlobClient(@"5066ce7d-b246-4c67-920c-14fdad44ac38.html");
            BlobClient BaseObligacionesExtinguidas = container.GetBlobClient(@"3bc60303-81ab-4483-9e9e-2af89e632a40.html");
            BlobClient BaseObligacionesVigentes = container.GetBlobClient(@"fb124596-cc2f-4a5a-9054-d991baff9adb.html");
            BlobClient BaseInformacionConsolidadaTrimestreI = container.GetBlobClient(@"eb06817f-1c04-4c1a-80a6-d6ccc1fbd4c6.html");
            BlobClient BaseInformacionDetalladaTrimestreI = container.GetBlobClient(@"dc15ab68-593d-437c-b4ba-cea6c71086ac.html");
            BlobClient BaseInformacionConsolidadaTrimestreII = container.GetBlobClient(@"eb06817f-1c04-4c1a-80a6-d6ccc1fbd4c6.html");
            BlobClient BaseInformacionDetalladaTrimestreII = container.GetBlobClient(@"dc15ab68-593d-437c-b4ba-cea6c71086ac.html");
            BlobClient BaseInformacionConsolidadaTrimestreIII = container.GetBlobClient(@"eb06817f-1c04-4c1a-80a6-d6ccc1fbd4c6.html");
            BlobClient BaseInformacionDetalladaTrimestreIII = container.GetBlobClient(@"dc15ab68-593d-437c-b4ba-cea6c71086ac.html");
            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();
            BlobDownloadInfo downloadBaseCuentasVigentes = await BaseCuentasVigentes.DownloadAsync();
            BlobDownloadInfo downloadBaseObligacionesExtinguidas = await BaseObligacionesExtinguidas.DownloadAsync();
            BlobDownloadInfo downloadBaseObligacionesVigentes = await BaseObligacionesVigentes.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionConsolidadaTrimestreI = await BaseInformacionConsolidadaTrimestreI.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionDetalladaTrimestreI = await BaseInformacionDetalladaTrimestreI.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionConsolidadaTrimestreII = await BaseInformacionConsolidadaTrimestreII.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionDetalladaTrimestreII = await BaseInformacionDetalladaTrimestreII.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionConsolidadaTrimestreIII = await BaseInformacionConsolidadaTrimestreIII.DownloadAsync();
            BlobDownloadInfo downloadBaseInformacionDetalladaTrimestreIII = await BaseInformacionDetalladaTrimestreIII.DownloadAsync();

            var basefile = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);
            var baseCuentasVigentes = new FileStreamResult(downloadBaseCuentasVigentes.Content, downloadBaseCuentasVigentes.ContentType);
            var baseObligacionesExtinguidas = new FileStreamResult(downloadBaseObligacionesExtinguidas.Content, downloadBaseObligacionesExtinguidas.ContentType);
            var baseObligacionesVigentes = new FileStreamResult(downloadBaseObligacionesVigentes.Content, downloadBaseObligacionesVigentes.ContentType);
            var baseInformacionConsolidadaTrimestreI = new FileStreamResult(downloadBaseInformacionConsolidadaTrimestreI.Content, downloadBaseInformacionConsolidadaTrimestreI.ContentType);
            var baseInformacionDetalladaTrimestreI = new FileStreamResult(downloadBaseInformacionDetalladaTrimestreI.Content, downloadBaseInformacionDetalladaTrimestreI.ContentType);
            var baseInformacionConsolidadaTrimestreII = new FileStreamResult(downloadBaseInformacionConsolidadaTrimestreII.Content, downloadBaseInformacionConsolidadaTrimestreII.ContentType);
            var baseInformacionDetalladaTrimestreII = new FileStreamResult(downloadBaseInformacionDetalladaTrimestreII.Content, downloadBaseInformacionDetalladaTrimestreII.ContentType);
            var baseInformacionConsolidadaTrimestreIII = new FileStreamResult(downloadBaseInformacionConsolidadaTrimestreIII.Content, downloadBaseInformacionConsolidadaTrimestreIII.ContentType);
            var baseInformacionDetalladaTrimestreIII = new FileStreamResult(downloadBaseInformacionDetalladaTrimestreIII.Content, downloadBaseInformacionDetalladaTrimestreIII.ContentType);

            await basefile.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();
            await baseCuentasVigentes.FileStream.FlushAsync();
            await baseObligacionesExtinguidas.FileStream.FlushAsync();
            await baseObligacionesVigentes.FileStream.FlushAsync();
            await baseInformacionConsolidadaTrimestreI.FileStream.FlushAsync();
            await baseInformacionDetalladaTrimestreI.FileStream.FlushAsync();
            await baseInformacionConsolidadaTrimestreII.FileStream.FlushAsync();
            await baseInformacionDetalladaTrimestreII.FileStream.FlushAsync();
            await baseInformacionConsolidadaTrimestreIII.FileStream.FlushAsync();
            await baseInformacionDetalladaTrimestreIII.FileStream.FlushAsync();



            var historialCial = JsonConvert.DeserializeObject<Root>(consulta.CBC_HISTORIAL_CREDITO);
            PdfCreator archivoHistorial = new PdfCreator();
            var documento = await archivoHistorial.GenerarHistorialComercial(historialCial, basefile, baseCss, baseCuentasVigentes, baseObligacionesExtinguidas, baseObligacionesVigentes, baseInformacionConsolidadaTrimestreI, baseInformacionDetalladaTrimestreI, baseInformacionConsolidadaTrimestreII, baseInformacionDetalladaTrimestreII, baseInformacionConsolidadaTrimestreIII, baseInformacionDetalladaTrimestreIII);

            return documento;
        }

        /// <summary>
        /// Obtener Comite Credito Temas
        /// </summary>
        /// <param name="requestTemaComite"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TemaComiteCreditoDto>>> ObtenerComiteCreditoTemas(RequestTemaComiteCreditoDto requestTemaComite)
        {
            var Solicitud = this._mapper.Map<RequestTemaComiteCredito>(requestTemaComite);
            return new Response<IEnumerable<TemaComiteCreditoDto>> { Data = this._mapper.Map<IEnumerable<TemaComiteCreditoDto>>(await this._creditoRepository.ObtenerComiteCreditoTemas(Solicitud)) };
        }

        /// <summary>
        /// Actualiza Solic Credito Tasas
        /// </summary>
        /// <param name="tasaSolicitudCredito"></param>
        /// <returns></returns>
        public async Task<Response<TasaSolicitudCreditoDto>> ActualizaSolicCreditoTasas(TasaSolicitudCreditoDto tasaSolicitudCredito)
        {
            var tasa = this._mapper.Map<TasaSolicitudCredito>(tasaSolicitudCredito);
            return new Response<TasaSolicitudCreditoDto> { Data = this._mapper.Map<TasaSolicitudCreditoDto>(await this._creditoRepository.ActualizaSolicCreditoTasas(tasa)) };
        }

        /// <summary>
        /// Obtener Fuentes Recursos
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<FuenteRecursosDto>>> ObtenerFuentesRecursos()
        {
            return new Response<IEnumerable<FuenteRecursosDto>> { Data = this._mapper.Map<IEnumerable<FuenteRecursosDto>>(await this._creditoRepository.ObtenerFuentesRecursos()) };
        }

        /// <summary>
        /// Mensaje
        /// </summary>
        /// <param name="codigoMensaje"></param>
        ///<author>Cristian Gonzalez</author>
        /// <date>10/06/2021</date>
        private async Task<MensajeDto> ObtenerMensaje(string codigoMensaje)
        {
            return this._mapper.Map<MensajeDto>(await this._messageRepository.ObtenerMensaje(codigoMensaje));
        }
        /// <summary>
        /// Generar Documento Cesion Leasing
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FileResult> GenerarCesionLeasing(Guid request)
        {
            var respuesta = await this._creditoRepository.ObtenerDatosSolicComite(request);
            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "legalizacion-cartera");
            BlobClient blobBase = container.GetBlobClient("fcc24e2d-c829-4f2e-8957-2029b7f9d053.html");
            BlobClient blobCss = container.GetBlobClient(@"d813b70b-cdce-4144-9544-f6a37c26df94.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();
            PdfCreator pdf = new PdfCreator();
            var Resultado = await pdf.GenerarCesionLeasig(respuesta, baseHtlm, baseCss);
            return Resultado;
        }

        /// <summary>
        /// Obtener Desembolsos Solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<SolicitudDesembolsoDto>>> ObtenerDesembolsosSolicitud(SolicitudDesembolsoDto request)
        {
            var solicitud = this._mapper.Map<SolicitudDesembolso>(request);
            return new Response<IEnumerable<SolicitudDesembolsoDto>> { Data = this._mapper.Map<IEnumerable<SolicitudDesembolsoDto>>(await this._creditoRepository.ObtenerDesembolsosSolicitud(solicitud)) };
        }

        /// <summary>
        /// Obtiene valor del desembolso del credito
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public async Task<Response<ValorDesembolsoCreditoDto>> ObtenerValorDesembolsoCredito(string numeroDocumento)
        {
            return new Response<ValorDesembolsoCreditoDto> { Data = this._mapper.Map<ValorDesembolsoCreditoDto>(await this._creditoRepository.ObtenerValorDesembolsoCredito(numeroDocumento)) };
        }
        public async Task<Response<SocSlsSimulacionCreditoDto>> ObtenerSocSlsSimulacionCredito(Guid idSolicitud)
        {
            return new Response<SocSlsSimulacionCreditoDto> { Data = this._mapper.Map<SocSlsSimulacionCreditoDto>(await this._creditoRepository.ObtenerSlsSocSolicitudCredito(idSolicitud)) };

        }
        /// <summary>
        /// Crear Solic Credito Desembolsos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<SolicitudCreditosDesembolsosDto>>> CrearSolicCreditoDesembolsos(IEnumerable<SolicitudCreditosDesembolsosDto> request)
        {
            var lista = new List<SolicitudCreditosDesembolsosDto>();
            foreach (var request_ in request)
            {
                request_.idSolicitudDesembolso = Guid.NewGuid();
                var solicitud = this._mapper.Map<SolicitudCreditosDesembolsos>(request_);
                lista.Add(this._mapper.Map<SolicitudCreditosDesembolsosDto>(await this._creditoRepository.CrearSolicCreditoDesembolsos(solicitud)));
            }

            return new Response<IEnumerable<SolicitudCreditosDesembolsosDto>> { Data = lista };
        }
        /// <summary>
        /// AplicarDesembolsoSolicitud
        /// </summary>
        /// <param name="desembolso"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<DesembolsoCreditoSolicitudDto>> AplicarDesembolsoSolicitud(DesembolsoCreditoSolicitudDto desembolso)
        {
            return new Response<DesembolsoCreditoSolicitudDto>
            {
                Data = this._mapper.Map<DesembolsoCreditoSolicitudDto>(
                    await this._creditoRepository.AplicarDesembolsoSolicitud(
                         this._mapper.Map<DesembolsoCreditoSolicitud>(desembolso)))

            };
        }

        /// <summary>
        /// AplicarDesembolsoSolicitud
        /// </summary>
        /// <param name="desembolso"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Response<IEnumerable<ObtenerCausalDesistimientoDto>>> ObtenerCausalDesistimiento()
        {

            return new Response<IEnumerable<ObtenerCausalDesistimientoDto>>
            {
                Data = this._mapper.Map<IEnumerable<ObtenerCausalDesistimientoDto>>(
                    await this._creditoRepository.ObtenerCausalDesistimiento())

            };
        }

        /// <summary>
        /// Carga documentos de credito al blobstorage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>27/06/2021</date>
        public async Task<Response<string>> CargarDocumentos(IFormFile request)
        {
            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            if (blogStorage.SRD_ESTADO)
            {
                var _request = request;
                FileInfo fi = new FileInfo(_request.FileName);
                BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "credito");
                await container.CreateIfNotExistsAsync();
                var newName = Guid.NewGuid() + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);
                var result = await blob.UploadAsync(request.OpenReadStream());
                return new Response<string> { Data = newName };
            }
            else
            {
                return await _localBlogStorageRepository.CargarDocumentosLocalSolicitudCredito(request);
            }
        }

        /// <summary>
        /// Descarga documentos de credito al blobstorage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>27/06/2021</date>
        public async Task<FileResult> DescargarDocumentos(string request)
        {
            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "credito");
                BlobClient blob = container.GetBlobClient(request);
                BlobDownloadInfo download = await blob.DownloadAsync();

                var descargado = new FileStreamResult(download.Content, download.ContentType);
                descargado.FileDownloadName = request;
                await descargado.FileStream.FlushAsync();
                return descargado;
            }
            else
            {
                return await _localBlogStorageRepository.DescargarDocumentosLocalSolicitudCredito(request);
            }
        }

        /// <summary>
        /// Crear Solic Credito Financieros
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>27/06/2021</date>
        public async Task<Response<CreditoFinancieroDto>> CrearSolicCreditoFinancieros(CreditoFinancieroDto request)
        {
            var solicitud = this._mapper.Map<CreditoFinanciero>(request);
            return new Response<CreditoFinancieroDto> { Data = this._mapper.Map<CreditoFinancieroDto>(await this._creditoRepository.CrearSolicCreditoFinancieros(solicitud)) };
        }

        /// <summary>
        /// ObtenerDatosReparto
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>27/06/2021</date>
        public async Task<Response<DatosRepartoDto>> ObtenerDatosReparto(Guid request)
        {
            var response = new DatosRepartoDto();
            response = this._mapper.Map<DatosRepartoDto>(await this._creditoRepository.ObtenerDatosReparto(request));
            if (response != null) response.IdSolicitud = request;
            return new Response<DatosRepartoDto> { Data = response };
        }

        /// <summary>
        /// DesistirDesembolsoSolicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>27/06/2021</date>
        public async Task<Response<DesistimientoDesembolsoDto>> DesistirDesembolsoSolicitud(DesistimientoDesembolsoDto request)
        {
            var solicitud = this._mapper.Map<DesistimientoDesembolso>(request);
            return new Response<DesistimientoDesembolsoDto> { Data = this._mapper.Map<DesistimientoDesembolsoDto>(await this._creditoRepository.DesistirDesembolsoSolicitud(solicitud)) };
        }

        /// <summary>
        /// CargarDatosFinancieros
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>19/07/2021</date>
        public async Task<Response<DatosFinancierosDto>> CargarDatosFinancieros(DatosFinancierosDto request)
        {
            var SolicCreditoFinancieros = new CreditoFinancieroDto
            {
                IdSolicitudF = request.IdSof,
                IdSolicitudCredito = request.IdSolicitudCredito,
                ValorConceptoSalario = request.ValorSalario,
                ConceptoSalario = request.DescripcionSalario,
                Concepto1 = request.DescripcionOtroIng1,
                ValorConcepto1 = request.ValorOtrosIngresos1,
                Concepto2 = request.DescripcionOtroIng2,
                ValorConcepto2 = request.ValorOtrosIngresos2,
                Concepto3 = request.DescripcionOtroIng3,
                ValorConcepto3 = request.ValorOtrosIngresos3,
                Concepto4 = request.DescripcionOtroIng4,
                ValorConcepto4 = request.ValorOtrosIngresos4,
                Concepto5 = request.DescripcionOtroIng5,
                ValorConcepto5 = request.ValorOtrosIngresos5,
                ValorTotalIngresos = request.ValorTotalIngresos,
                ValorTotalEgresos = request.ValorTotalEgresos,
                DireccionVivienda = request.DireccionVivienda,
                MatriculaVivienda = request.MatriculaVivienda,
                ValorComercialVivienda = request.ValorComercialVivienda,
                MarcaVehiculo = request.MarcaVehiculo,
                PlacaVehiculo = request.PlacaVehiculo,
                ValorComercialVehiculo = request.ValorComercialVehiculo,
                CreadoPor = request.CreadoPor,
                FechaCreacion = request.FechaCreacion,
                ActualizadoPor = request.UsuarioActualiza,
                FechaActualiza = request.FechaActualiza
            };

            var SimulacionDatosFinancieros = new SimulacionDatosFinancierosDto
            {
                IdSimulacion = request.IdSimulacion,
                DescripcionSalario = request.DescripcionSalario,
                ValorSalario = request.ValorSalario,
                DescripcionOtroIng1 = request.DescripcionOtroIng1,
                ValorOtrosIngresos1 = request.ValorOtrosIngresos1,
                DescripcionOtroIng2 = request.DescripcionOtroIng2,
                ValorOtrosIngresos2 = request.ValorOtrosIngresos2,
                DescripcionOtroIng3 = request.DescripcionOtroIng3,
                ValorOtrosIngresos3 = request.ValorOtrosIngresos3,
                DescripcionOtroIng4 = request.DescripcionOtroIng4,
                ValorOtrosIngresos4 = request.ValorOtrosIngresos4,
                DescripcionOtroIng5 = request.DescripcionOtroIng5,
                ValorOtrosIngresos5 = request.ValorOtrosIngresos5,
                DescripcionOtroIngresos = request.DescripcionOtroIngresos,
                ValorOtrosIngresos = request.ValorOtrosIngresos,
                ValorTotalIngresos = request.ValorTotalIngresos,
                ValorTotalEgresos = request.ValorTotalEgresos,
                UsuarioActualiza = request.UsuarioActualiza,
                FechaActualiza = request.FechaActualiza
            };

            try
            {
                await this._validacionIdentidadBusinessLogic.CrearSimulacionDatosFinancieros(SimulacionDatosFinancieros);
                await this.CrearSolicCreditoFinancieros(SolicCreditoFinancieros);
                return new Response<DatosFinancierosDto> { Data = request };

            }
            catch (Exception ex)
            {
                return new Response<DatosFinancierosDto> { ReturnMessage = ex.ToString() };
            }
        }

        /// <summary>
        /// CargarDatosSolicCreditoTasasDesembolso
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>19/07/2021</date>
        public async Task<Response<DatosSolicCreditoDto>> CargarDatosSolicCreditoTasasDesembolso(DatosSolicCreditoDto request)
        {
            var response = new DatosSolicCreditoDto();
            response.SolicitudCreditosDesembolsos = (await this.CrearSolicCreditoDesembolsos(request.SolicitudCreditosDesembolsos)).Data;
            response.TasaSolicitudCreditoDto = (await this.ActualizaSolicCreditoTasas(request.TasaSolicitudCreditoDto)).Data;

            return new Response<DatosSolicCreditoDto> { Data = response };


        }

        /// <summary>
        /// ObtenerFuentesRecursosSolicitud
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        public async Task<ActionResult<Response<IEnumerable<SolicitudCreditosDesembolsosDto>>>> ObtenerFuentesRecursosSolicitud(Guid request)
        {
            return new Response<IEnumerable<SolicitudCreditosDesembolsosDto>> { Data = this._mapper.Map<IEnumerable<SolicitudCreditosDesembolsosDto>>(await this._creditoRepository.ObtenerFuentesRecursosSolicitud(request)) };
        }

        /// <summary>
        /// GenerarFormatoReparto
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>19/07/2021</date>
        public async Task<FileResult> GenerarFormatoReparto(Guid request)
        {
            var datos = await ObtenerDatosReparto(request);

            BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "legalizacion-cartera");
            BlobClient blobBase = container.GetBlobClient("29b93d1e-0191-4253-b9f7-2c698a21e055.html");
            BlobClient blobCss = container.GetBlobClient(@"40a472dc-fc82-4db9-ad68-befaabeac7c6.css");

            BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
            BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

            var baseHtlm = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
            var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

            await baseHtlm.FileStream.FlushAsync();
            await baseCss.FileStream.FlushAsync();

            var pdf = new PdfCreator();
            var resultado = await pdf.GenerarReparto(datos.Data, baseHtlm, baseCss);
            return resultado;

        }

        /// <summary>
        /// Validar Documentos flujo
        /// </summary>
        /// <param name="validacionDocumentos"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ValidacionDocumentosDto>>> ValidarDocumentosFlujo(ValidacionDocumentosDto validacionDocumentos)
        {
            var solicitud = this._mapper.Map<ValidacionDocumentos>(validacionDocumentos);
            return new Response<IEnumerable<ValidacionDocumentosDto>> { Data = this._mapper.Map<IEnumerable<ValidacionDocumentosDto>>(await this._creditoRepository.ValidarDocumentosFlujo(solicitud)) };

        }

        public async Task<FileResult> GenerarActaComite(int id)
        {
            var LstsolicitudComiteInfFinancieraDto = new List<SolicitudComiteInfFinancieraDto>();
            var LstsolicitudComiteInfInmuebleDto = new List<SolicitudComiteInfInmuebleDto>();
            var datosSolicitudes = new List<DatosSolicitudComiteDto>();
            var requestComite = new DatosComiteCreditoDto { CodigoComite = id };
            var comiteCredito = this.ObtenerComiteCredito(requestComite).Result.Data;

            var requestIntegrantes = new IntegrantePorComiteDto { idComite = id };
            var integrantes = this.ObtenerComiteCreditoIntegrante(requestIntegrantes).Result.Data;

            var requestTemas = new RequestTemaComiteCreditoDto { idComite = id };
            var temas = this.ObtenerComiteCreditoTemas(requestTemas).Result.Data;

            var requestSolicitudes = new SolicitudComiteDto { idComiteCredito = id };
            var solicitudes = this.ObtenerComiteCreditoSolicitud(requestSolicitudes).Result.Data;

            var Departamentos = await _catalogsRepository.ObtenerDepartamentos();
            var Ciudades = await _catalogsRepository.ObtenerCiudades();
            var TipoVivienda = await _catalogsRepository.ObtenerTiposDeVivienda();
            var TipoMatricula = await _catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(90);


            foreach (SolicitudComiteDto solicitud in solicitudes)
            {
                var datos = this._mapper.Map<DatosSolicitudComiteDto>(this._creditoRepository.ObtenerDatosSolicComite((Guid)solicitud.idSolicitud).Result);
                datosSolicitudes.Add(datos);
                var requestInmTecnica = new RequestSolicCreditoInfTecnicaDto { Indicador = 2, SolicitudCredito = solicitud.idSolicitud.ToString() };
                var InmTecnica = this.ObtenerSolicCreditoInfTecnica(requestInmTecnica).Result.Data;
                var recursos = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.Tipo_Identificacion, solicitud.identificacion)).Data;
                var Financiara = this.ObtenerDatosFinancierosPersonales(solicitud.identificacion).Result.Data;
                var Personales = this.ObtenerSimulacionPersonales(solicitud.identificacion).Result.Data;

                var RequestCreditoProducto = new PeticionSolicitudCreditoProductoDto { IND = 2, SOC_ID = (Guid)solicitud.idSolicitud };
                var CreditoProducto = this.ObtenerSolicCreditoProducto(RequestCreditoProducto).Result.Data;

                var RequestCreditoInfJurInm = new RequestSolicCreditoInfJurInmDto { indicador = 3, idSolicitudCredito = (Guid)solicitud.idSolicitud, identificacion = solicitud.identificacion };
                var CreditoInfJurInm = this.ObtenerSolicCreditoInfJurInm(RequestCreditoInfJurInm).Result.Data;

                var RequestCreditoInfTecInm = new RequestSolicCreditoInfTecInmDto { indicador = 3, idSolicitudCredito = (Guid)solicitud.idSolicitud };
                var CreditoInfTecInm = this.ObtenerSolicCreditoInfTecInm(RequestCreditoInfTecInm).Result.Data;

                var RequestInfTec = new RequestSolicCreditoInfTecnicaDto { SolicitudCredito = solicitud.idSolicitud.ToString(), Indicador = 2 };
                var InfTec = this.ObtenerSolicCreditoInfTecnica(RequestInfTec).Result.Data;

                var MatriculasCreditoSolicitud = this.ObtenerMatriculasCreditoporSolicitud((Guid)solicitud.idSolicitud).Result.Data;

                var Edad = DateTime.Now.Year - recursos.FechaNacimiento.Year;

                #region Info_Financiera
                InformacionAfiliadoComiteDto informacionAfiliadoComiteDto = new()
                {
                    CuotasAportadas = recursos.NumeroCuotas,
                    SalarioBasico = (double)Financiara.ValorSalario,
                    SalarioTotal = (double)Financiara.TotalIngresos,
                    EstadoCivil = recursos.EstadoCivil,
                    Edad = recursos.FechaNacimiento > DateTime.Now.AddYears(-Edad) ? Edad-- : Edad,
                    Score = solicitud.Score
                };

                InformacionFinancieraComiteDto informacionFinancieraComiteDto = new()
                {
                    ValorCredito = solicitud.valorCredito,
                    TasaEA = Math.Round(solicitud.teaCredito, 2, MidpointRounding.AwayFromZero) * 100,
                    Plazo = solicitud.plazoCredito,
                    PlazoAnio = (int)Math.Round((decimal)solicitud.plazoCredito / 12, 0, MidpointRounding.AwayFromZero),
                    CuotaMensual = solicitud.valorCuota,
                    EndeudamientoCCH = solicitud.EndeudamientoCCH,
                    EndeudamientoSCH = solicitud.EndeudamientoSCH,
                    CuentaIndividual = (double)recursos.SaldoDisponible,
                    Subsidio = (double)solicitud.Subsidio,
                    RecursosPropios = (double)(InmTecnica.ValorVentaInmueble - solicitud.valorCredito - recursos.SaldoDisponible - solicitud.Subsidio)
                };

                InformacionInmuebleComiteDto informacionInmuebleComiteDto = new()
                {
                    AvaluoCatastral = InmTecnica.ValorAvaluoCatastral,
                    AvaluoComercial = InmTecnica.ValorAvaluoComercial,
                    ValorVenta = InmTecnica.ValorVentaInmueble,
                    PorcFinanciacion = ((decimal)solicitud.valorCredito * 100.0M) / (decimal)InmTecnica.ValorVentaInmueble,
                    Ubicación = Departamentos.Where(x => x.DPD_ID == Personales.DpdIdInmueble).Select(x => x.DPD_NOMBRE).FirstOrDefault(),
                    EstadoVivienda = TipoVivienda.Where(x => x.TVV_ID == Personales.TvvId).Select(x => x.TVV_DESCRIPCION).FirstOrDefault()
                };

                SolicitudComiteInfFinancieraDto solicitudComiteInfFinanciera = new()
                {
                    InformacionAfilidado = informacionAfiliadoComiteDto,
                    InformacionFinanciera = informacionFinancieraComiteDto,
                    InformacionInmueble = informacionInmuebleComiteDto,
                    Sol_id = (Guid)solicitud.idSolicitud
                };
                LstsolicitudComiteInfFinancieraDto.Add(solicitudComiteInfFinanciera);
                #endregion
                #region Info_TecnicaInmueble
                SolicitudComiteInfInmuebleAvaComercialDto solicitudComiteInfInmuebleAvaComercialDto = new()
                {
                    AreaPrivada = ((from InfJuridica in CreditoInfJurInm
                                    join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                    join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                    where tipoM.CVL_DESCRIPCION == "APARTAMENTO" || tipoM.CVL_DESCRIPCION == "CASA"
                                    select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()),
                    ValorM2Privada = ((from InfJuridica in CreditoInfJurInm
                                       join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                       join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                       where tipoM.CVL_DESCRIPCION == "APARTAMENTO" || tipoM.CVL_DESCRIPCION == "CASA"
                                       select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()) / InmTecnica.ValorAvaluoComercial,
                    AreaGaraje = ((from InfJuridica in CreditoInfJurInm
                                   join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                   join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                   where tipoM.CVL_DESCRIPCION == "GARAJE"
                                   select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()),
                    ValorM2Garaje = ((from InfJuridica in CreditoInfJurInm
                                      join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                      join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                      where tipoM.CVL_DESCRIPCION == "GARAJE"
                                      select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()) / InmTecnica.ValorAvaluoComercial,
                    AreaDeposito = ((from InfJuridica in CreditoInfJurInm
                                     join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                     join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                     where tipoM.CVL_DESCRIPCION == "DEPOSITO"
                                     select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()),
                    ValorM2Deposito = ((from InfJuridica in CreditoInfJurInm
                                        join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                        join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                        where tipoM.CVL_DESCRIPCION == "DEPOSITO"
                                        select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()) / InmTecnica.ValorAvaluoComercial,
                    ValorM2Sector = ((from InfJuridica in CreditoInfJurInm
                                      join MatriculasSolicitud in MatriculasCreditoSolicitud on InfJuridica.NumeroMatricula equals MatriculasSolicitud.NumeroMatricula
                                      join tipoM in TipoMatricula on MatriculasSolicitud.Tipo_Matricula equals tipoM.CVL_ID
                                      where tipoM.CVL_DESCRIPCION == "APARTAMENTO" || tipoM.CVL_DESCRIPCION == "CASA"
                                      select (MatriculasSolicitud.AreaPrivadaConstruida)).FirstOrDefault()) / InmTecnica.ValorAvaluoCatastral
                };

                SolicitudComiteInfInmuebleUbicacionDto solicitudComiteInfInmuebleUbicacionDto = new()
                {
                    Ciudad = Ciudades.Where(x => x.DPD_ID == CreditoProducto.DPD_ID && x.DPC_ID == CreditoProducto.DPC_ID).Select(x => x.DPC_NOMBRE).FirstOrDefault(),
                    Direccion = CreditoProducto.SOP_DIRECCION_INMUEBLE,
                    Municipio = Departamentos.Where(x => x.DPD_ID == CreditoProducto.DPD_ID).Select(x => x.DPD_NOMBRE).FirstOrDefault(),
                    NombrePropiedad = CreditoProducto.SOP_PROPIEDAD_HORIZONTAL
                };

                SolicitudComiteInfInmueblePolGarantiasDto solicitudComiteInfInmueblePolGarantiasDto = new()
                {
                    EstadoVivienda = TipoVivienda.Where(x => x.TVV_ID == Personales.TvvId).Select(x => x.TVV_DESCRIPCION).FirstOrDefault(),
                    Estrato = InmTecnica.Estrato,
                    AñosConstruccion = CreditoProducto.SOP_AÑOS_CONSTRUCCION,
                    EdadPrimeraTradicion = CreditoProducto.SOP_AÑOS_CONSTRUCCION,
                    TipoParquadero = CreditoProducto.SOP_TIPO_GARAGE,
                    Avaluo = InfTec.ConceptoEstTecnico,
                    EstudioTitulos = InfTec.ConceptoEstTecnico
                };

                SolicitudComiteInfInmuebleDto solicitudComiteInfInmuebleDto = new()
                {
                    AvaluoComercial = solicitudComiteInfInmuebleAvaComercialDto,
                    PoliticasGarantias = solicitudComiteInfInmueblePolGarantiasDto,
                    Ubicacion = solicitudComiteInfInmuebleUbicacionDto,
                    Solicitud_id = (Guid)solicitud.idSolicitud
                };

                LstsolicitudComiteInfInmuebleDto.Add(solicitudComiteInfInmuebleDto);

                #endregion
            }
            var UsuarioModifica = new UsuarioDto();
            foreach (var item in comiteCredito)
            {
                var requestUser = new User { USR_ID = item.CreadoPor };
                UsuarioModifica = this.ConsultaUsuarioActaComite(requestUser).Result.Data;
            }

            var modelValid = UsuarioModifica == null || temas.Count() == 0 || integrantes.Count() == 0 || comiteCredito.Count() == 0 ? true : false;
            if (modelValid)
            {
                throw new InvalidOperationException($"No existe información suficiente en la base de datos para generar el acta de comite");
            }

            var requestDocumento = new SolicitudActaComiteDto { DatosComite = comiteCredito, IntegrantesComite = integrantes, TemasComite = temas, Solicitudes = solicitudes, DatosSolicitud = datosSolicitudes, Usuario = UsuarioModifica, DatosFinanciera = LstsolicitudComiteInfFinancieraDto, DatosInmueble = LstsolicitudComiteInfInmuebleDto };

            ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            // Declarar las variables fuera del if-else para que sean accesibles después
            FileStreamResult baseHtml;
            FileStreamResult baseDetalleActa;
            FileStreamResult baseCss;
            FileStreamResult baseHeaderhtml;
            FileStreamResult baseFooterhtml;

            if (blogStorage.SRD_ESTADO)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "legalizacion-cartera");
                BlobClient blobBase = container.GetBlobClient("a34c0f5f-4e98-4946-8734-b8c2d82ff353.html");
                BlobClient BlobDetalleActa = container.GetBlobClient("DetalleActaComite.html");
                BlobClient blobCss = container.GetBlobClient("8589af31-f2ed-4cea-8210-cdc3154d6fbf.css");

                BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                BlobDownloadInfo downloadDetalleActa = await BlobDetalleActa.DownloadAsync();
                BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();

                baseHtml = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                baseDetalleActa = new FileStreamResult(downloadDetalleActa.Content, downloadDetalleActa.ContentType);
                baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);

                await baseHtml.FileStream.FlushAsync();
                await baseDetalleActa.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();

                BlobContainerClient img = new BlobContainerClient(this._options.Value.CadenaOne, "imagenes-caja");
                BlobClient header = img.GetBlobClient("Header.html");
                BlobClient footer = img.GetBlobClient("Footer.html");

                BlobDownloadInfo downloadHeader = await header.DownloadAsync();
                BlobDownloadInfo downloadFooter = await footer.DownloadAsync();

                baseHeaderhtml = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                baseFooterhtml = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                await baseHeaderhtml.FileStream.FlushAsync();
                await baseFooterhtml.FileStream.FlushAsync();
            }
            else
            {

                string localPath = _config.CarpetaServidorStorage;

                string baseHtmlPath = Path.Combine(localPath + "\\legalizacion-cartera", "a34c0f5f-4e98-4946-8734-b8c2d82ff353.html");
                string detalleActaPath = Path.Combine(localPath + "\\legalizacion-cartera", "DetalleActaComite.html");
                string cssPath = Path.Combine(localPath + "\\legalizacion-cartera", "8589af31-f2ed-4cea-8210-cdc3154d6fbf.css");

                baseHtml = new FileStreamResult(new FileStream(baseHtmlPath, FileMode.Open, FileAccess.Read), "text/html");
                baseDetalleActa = new FileStreamResult(new FileStream(detalleActaPath, FileMode.Open, FileAccess.Read), "text/html");
                baseCss = new FileStreamResult(new FileStream(cssPath, FileMode.Open, FileAccess.Read), "text/css");

                await baseHtml.FileStream.FlushAsync();
                await baseDetalleActa.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();

                string headerPath = Path.Combine(localPath + "\\imagenes-caja", "Header.html");
                string footerPath = Path.Combine(localPath + "\\imagenes-caja", "Footer.html");

                baseHeaderhtml = new FileStreamResult(new FileStream(headerPath, FileMode.Open, FileAccess.Read), "text/html");
                baseFooterhtml = new FileStreamResult(new FileStream(footerPath, FileMode.Open, FileAccess.Read), "text/html");

                await baseHeaderhtml.FileStream.FlushAsync();
                await baseFooterhtml.FileStream.FlushAsync();
            }

            PdfCreator pdf = new PdfCreator();
            var generado = await pdf.GenerarFichaComite(requestDocumento, baseHtml, baseCss, baseHeaderhtml, baseFooterhtml, baseDetalleActa);


            return generado;
        }

        public async Task<Response<ActualizacionSolicitudCreditoDesemFirmaDto>> ActualizaSolicCreditoDesembolso(ActualizacionSolicitudCreditoDesemFirmaDto request)
        {
            var solicitud = this._mapper.Map<ActualizacionSolicitudCreditoDesemFirma>(request);
            return new Response<ActualizacionSolicitudCreditoDesemFirmaDto> { Data = this._mapper.Map<ActualizacionSolicitudCreditoDesemFirmaDto>(await this._creditoRepository.ActualizaSolicCreditoDesembolso(solicitud)) };
        }

        public async Task<Response<ActualizacionSolicitudCreditoDesemFirmaDto>> ActualizaSolicCreditoFirmas(ActualizacionSolicitudCreditoDesemFirmaDto request)
        {
            var solicitud = this._mapper.Map<ActualizacionSolicitudCreditoDesemFirma>(request);
            return new Response<ActualizacionSolicitudCreditoDesemFirmaDto> { Data = this._mapper.Map<ActualizacionSolicitudCreditoDesemFirmaDto>(await this._creditoRepository.ActualizaSolicCreditoFirmas(solicitud)) };
        }

        public async Task<FileResult> ObtenerFichaRiesgo(RequestFichaRiesgo request)
        {
            var respuesta = await this._creditoRepository.ObtenerDatosSolicComite(request.IdSolicitud);
            var datosPersonales = await this._creditoRepository.ObtenerSimulacionPersonales(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
            var simulacion = await this._creditoRepository.ConsultarSimulacionCliente(respuesta.SOB_NUMERO_DOCUMENTO.ToString());
            var solicitud = await this._creditoRepository.ConsultarSolicitudCredito(new ConsultaSolicitudCredito { SOC_ID = respuesta.SOC_ID, CLI_IDENTIFICACION = respuesta.SOB_NUMERO_DOCUMENTO.ToString(), SOC_ESTADO = "0" });
            RiesgoPdfCreator pdf = new RiesgoPdfCreator();
            if (respuesta != null)
            {
                try
                {
                    var blobCheck = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", $"{request.IdSolicitud.ToString().ToUpper()}.pdf");
                    var blobMod = (int)(DateTime.Now - blobCheck.Details.LastModified).TotalDays;

                    if (blobMod >= 30)
                    {
                        await _blobStorageGenericRepository.EliminarArchivoStorage("riesgo", $"{request.IdSolicitud.ToString().ToUpper()}.pdf");
                        throw new Exception();
                    }
                    var file = new FileStreamResult(blobCheck.Content, "application/octet-stream")
                    {
                        FileDownloadName = $"{request.IdSolicitud.ToString().ToUpper()}.pdf"
                    };
                    await file.FileStream.FlushAsync();
                    return file;
                }
                catch
                {
                    #region Prepara template
                    //Descarga archivo base d eblob storage
                    var tempStream = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", "552ddaf2-9cd8-4c34-b70b-f833a09a28c6.html");
                    var sr = new StreamReader(tempStream.Content);
                    var fileBase = await sr.ReadToEndAsync();

                    //descarga header de blob storage
                    tempStream = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", "9dd8a639-dfca-4837-b040-699d186ba0b9.html");
                    sr = new StreamReader(tempStream.Content);
                    var header = await sr.ReadToEndAsync();

                    //descarga footer de blob storage
                    tempStream = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", "82401d26-f129-4406-aa32-5c55aac7dade.html");
                    sr = new StreamReader(tempStream.Content);
                    var footer = await sr.ReadToEndAsync();

                    //descarga css de blob storage
                    tempStream = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", "ccb522a9-2cb0-4c89-8363-b8b3959b0b5b.css");
                    sr = new StreamReader(tempStream.Content);
                    var css = await sr.ReadToEndAsync();
                    #endregion


                    var resultado = await pdf.GenerarFichaRiesgo(respuesta, simulacion, datosPersonales, solicitud, fileBase, header, footer, css);
                    var fileStreamResult = (FileStreamResult)resultado;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                    await _blobStorageGenericRepository.SubirArchivoStorage("riesgo", $"{request.IdSolicitud.ToString().ToUpper()}.pdf", fileStreamResult);
                    var blobCheck = await _blobStorageGenericRepository.DescargarArchivoStorage("riesgo", $"{request.IdSolicitud.ToString().ToUpper()}.pdf");
                    var returnFile = new FileStreamResult(blobCheck.Content, "application/octet-stream")
                    {
                        FileDownloadName = $"{request.IdSolicitud.ToString().ToUpper()}.pdf"
                    };
                    await returnFile.FileStream.FlushAsync();
                    return returnFile;
                }



            }
            else throw new ArgumentNullException(nameof(respuesta));
        }

        /// <summary>
        /// Obtiene los datos personale spara la simulacion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<SimulacionDatosPersonalesDto>> ObtenerSimulacionPersonales(string request)
        {

            try
            {
                var verificacion = int.Parse(request);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cedula Invalida", ex);
            }
            var result = new SimulacionDatosPersonales();
            try
            {
                result = await this._creditoRepository.ObtenerSimulacionPersonales(request);
                if (result == null)
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                result = _mapper.Map<SimulacionDatosPersonales>((await this._validacionIdentidadBusinessLogic.CrearSimulacionDatosPersonales(new SimulacionDatosPersonalesDto { NumeroDocumento = request })).Data);

                if (result == null)
                {
                    return new Response<SimulacionDatosPersonalesDto> { ReturnMessage = "Datos insuficientes del usuario, por favor actualizar" };

                }
            }
            try
            {
                if (result != null && result.DPC_ID_INMUEBLE > 0 && result.DPD_ID_INMUEBLE > 0)
                {
                    var ciudadesInmueble = await this._catalogsRepository.ObtenerCiudadesPorDepartamento(result.DPD_ID_INMUEBLE);
                    result.CIUDAD_INMUEBLE = ciudadesInmueble.FirstOrDefault(c => c.DPC_CODIGO == result.DPC_ID_INMUEBLE).DPC_NOMBRE;
                }
                if (result != null && result.DPC_ID > 0 && result.DPD_ID > 0)
                {
                    var ciudades = await this._catalogsRepository.ObtenerCiudadesPorDepartamento(result.DPD_ID);
                    result.CIUDAD_RESIDENCIA = ciudades.FirstOrDefault(c => c.DPC_CODIGO == result.DPC_ID).DPC_NOMBRE;
                }
                if (result != null && result.TVV_ID != 0)
                {
                    var tvvs = await this._catalogsRepository.ObtenerTiposDeVivienda();
                    result.TIPO_VIVIENDA = tvvs.FirstOrDefault(tv => tv.TVV_ID == result.TVV_ID).TVV_DESCRIPCION;
                }
            }
            catch (Exception)
            {
                return new Response<SimulacionDatosPersonalesDto> { Data = this._mapper.Map<SimulacionDatosPersonalesDto>(result) };
            }
            return new Response<SimulacionDatosPersonalesDto> { Data = this._mapper.Map<SimulacionDatosPersonalesDto>(result) };
        }
        /// <summary>
        /// Obtiene la cuenta segun la cedula
        /// </summary>
        /// <param name="clienteIdentificacion"></param>
        /// <returns></returns>
        public async Task<Response<CuentaDto>> ObtenerCuentaById(string clienteIdentificacion)
        {
            var cuenta = await _iClientRequestProvider.ObtenerCuentaById(clienteIdentificacion);


            return cuenta;
        }

        /// <summary>
        /// Obtiene los datos personales financieros d eun usuario
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<DatosFinancierosPersonalesDto>> ObtenerDatosFinancierosPersonales(string request)
        {
            return new Response<DatosFinancierosPersonalesDto> { Data = this._mapper.Map<DatosFinancierosPersonalesDto>(await this._creditoRepository.ObtenerDatosFinancierosPersonales(request)) };
        }

        /// <summary>
        /// Carga documentos en base 64 al blob storage
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<string>> CargaDocumentoSolicCreditoBase64(Base64FileDto request)
        {
            var _request = Convert.FromBase64String(request.File);

            Stream stream = new MemoryStream(_request);

            //FileInfo fi = new FileInfo(fs.Name);
            BlobContainerClient container = new BlobContainerClient(_options.Value.CadenaOne, "credito");
            await container.CreateIfNotExistsAsync();
            var newName = Guid.NewGuid() + ".pdf";
            BlobClient blob = container.GetBlobClient(newName);
            stream.Position = 0;
            var result = await blob.UploadAsync(stream);
            return new Response<string> { Data = newName };
        }

        /// <summary>
        /// Actualiza info personal vivienda
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<SimulacionDatosPersonalesDto>> ActualizarPersonalesVivienda(SimulacionDatosPersonalesDto request)
        {
            var peticion = this._mapper.Map<SimulacionDatosPersonales>(request);

            return new Response<SimulacionDatosPersonalesDto>() { Data = _mapper.Map<SimulacionDatosPersonalesDto>(await this._creditoRepository.ActualizarPersonalesVivienda(peticion)) };
        }

        /// <summary>
        /// Obtiene la dolicitud de credito
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ActualizacionSolicitudCreditoDesemFirmaDto>> ObtenerSolicitudCredito(RequestSolicitudCreditoDto requestSolicitudCredito)
        {
            var request = requestSolicitudCredito.Indicador == 1 ? requestSolicitudCredito.IdSolicitud : requestSolicitudCredito.Documento;
            var indicador = requestSolicitudCredito.Indicador;
            Guid.TryParse(request, out Guid result);
            return new Response<ActualizacionSolicitudCreditoDesemFirmaDto>() { Data = _mapper.Map<ActualizacionSolicitudCreditoDesemFirmaDto>(await this._creditoRepository.ObtenerSolicitudCredito(result, indicador)) };
        }

        /// <summary>
        /// Obtiene la carta de instrucciones y el pagare
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FileResult> ObtenerPagareCartaInstrucciones(Guid request)
        {
            var pagareBase = "810bc370-d71d-4a14-834d-927abca1b4b8.html";
            var cssBase = "f4c60f3d-834c-471f-872d-4547406f633b.css";
            var header = "44290a0c-6b18-49b4-a250-e9209d8fc6f0.html";
            var footer = "b0f2bdfb-cc86-4cd2-83fd-48e96e58d28f.html";

            PeticionCreditoBasicaDto peticion = new() { IND = 2, SOB_ID = null, SOC_ID = request };
            PeticionSolicitudCreditoProductoDto peticionCredito = new() { IND = 2, SOC_ID = request };
            RespuestaCreditoBasicaDto datosBasicos = (await this.ObtenerSolicCreditoBasica(peticion)).Data;
            RespuestaSolicitudCreditoProductoDto datosProducto = (await this.ObtenerSolicCreditoProducto(peticionCredito)).Data;
            var listCreditos = (await ObtenerTramiteSolicitud(new PeticionSolicitudObtenerTramiteDto() { Usuario = Guid.Parse("3e3ab8c4-2c45-417d-86f6-fed3d8ccc05f") })).Data;
            var producto = listCreditos.FirstOrDefault(x => x.ID_SOLICITUD == request);
            if (producto != default(RespuestaSolicitudObtenerTramiteDto) && producto.ID_TIPO_CREDITO == 1)
            {
                BlobContainerClient container = new BlobContainerClient(this._options.Value.CadenaOne, "legalizacion-cartera");
                BlobClient blobBase = container.GetBlobClient(pagareBase);
                BlobClient blobCss = container.GetBlobClient(cssBase);
                BlobClient blobHeader = container.GetBlobClient(header);
                BlobClient blobFooter = container.GetBlobClient(footer);
                BlobDownloadInfo downloadBase = await blobBase.DownloadAsync();
                BlobDownloadInfo downloadCss = await blobCss.DownloadAsync();
                BlobDownloadInfo downloadHeader = await blobHeader.DownloadAsync();
                BlobDownloadInfo downloadFooter = await blobFooter.DownloadAsync();

                var basePagare = new FileStreamResult(downloadBase.Content, downloadBase.ContentType);
                var baseCss = new FileStreamResult(downloadCss.Content, downloadCss.ContentType);
                var baseHeader = new FileStreamResult(downloadHeader.Content, downloadHeader.ContentType);
                var baseFooter = new FileStreamResult(downloadFooter.Content, downloadFooter.ContentType);

                await basePagare.FileStream.FlushAsync();
                await baseCss.FileStream.FlushAsync();
                await baseHeader.FileStream.FlushAsync();
                await baseFooter.FileStream.FlushAsync();
                PdfCreator pdf = new PdfCreator();



                return await pdf.GenerarPagareCartaInstrucciones(datosBasicos, datosProducto, basePagare, baseCss, baseHeader, baseFooter);
            }
            else { return null; }

        }

        public async Task<Response<IEnumerable<SolicitudCreditosDesembolsosDto>>> EliminarFuentesRecursosSolicitud(SolicitudCreditosDesembolsosDto request)
        {
            var solicitud = _mapper.Map<SolicitudCreditosDesembolsos>(request);
            return new Response<IEnumerable<SolicitudCreditosDesembolsosDto>>() { Data = _mapper.Map<IEnumerable<SolicitudCreditosDesembolsosDto>>(await this._creditoRepository.EliminarFuentesRecursosSolicitud(solicitud)) };
        }

        public async Task<Response<IEnumerable<EqivalenciaSimuladorDto>>> ObtenerEquivalenciasSimuladorCredito()
        {
            return new Response<IEnumerable<EqivalenciaSimuladorDto>>() { Data = _mapper.Map<IEnumerable<EqivalenciaSimuladorDto>>(await this._creditoRepository.ObtenerEquivalenciasSimuladorCredito()) };
        }

        public async Task<Response<SolicitudSimulacionDto>> CrearSolicitudSimulacionCredito(SolicitudSimulacionDto solicitudSimulacionDto)
        {
            return new Response<SolicitudSimulacionDto>() { Data = _mapper.Map<SolicitudSimulacionDto>(await this._creditoRepository.CrearSolicitudSimulacionCredito(_mapper.Map<SolicitudSimulacion>(solicitudSimulacionDto))) };
        }

        public async Task<Response<IEnumerable<SimulacionDetalle>>> CrearDetalleSimulacion(IEnumerable<SimulacionDto> detallesSimulacion, string documento)
        {
            List<Thread> inserciones = new List<Thread>();
            List<SimulacionDetalle> detalles = new List<SimulacionDetalle>();
            foreach (var simulacionDetalle in detallesSimulacion)
            {

                /*var detalle=*/
                detalles.Add(new SimulacionDetalle()
                {
                    SMD_ID = simulacionDetalle.IdDetalleSimulacion,
                    SMC_ID = simulacionDetalle.IdSimulacionCliente,
                    SMD_ABONO_EXTRA = simulacionDetalle.AbonoExtra,
                    SMD_ABONO_MENSUAL = simulacionDetalle.AbonoMensual,
                    SMD_CAPITAL = simulacionDetalle.Capital,
                    SMD_CAPITAL_LEASING = simulacionDetalle.CapitalLeasing,
                    SMD_COBRO_FONVIVIENDA = simulacionDetalle.CobroFonvivienda,
                    SMD_CUOTA = simulacionDetalle.Cuota,
                    SMD_CUOTA_LEASING = simulacionDetalle.CuotaLeasing,
                    SMD_CUOTA_LEASING_SUBSIDIO = simulacionDetalle.CuotaLeasingSubsidio,
                    SMD_CUOTA_REDUCCION = simulacionDetalle.CuotaReduccion,
                    SMD_CUOTA_TOTAL = simulacionDetalle.TotalCuota,
                    SMD_CUOTA_UNIDAD_PAGADORA = simulacionDetalle.CuotaUnidadPagadora,
                    SMD_FECHA_CORTE = simulacionDetalle.FechaCorte,
                    SMD_FECHA_PAGO = simulacionDetalle.FechaPago,
                    SMD_INTERESES = simulacionDetalle.Intereses,
                    SMD_INTERESES_REDUCCION = simulacionDetalle.InteresesReduccion,
                    SMD_INTERES_LEASING = simulacionDetalle.InteresLeasing,
                    SMD_INTERES_LEASING_SUBSIDIO = simulacionDetalle.Subsidio,
                    SMD_INTERES_TRADICIONAL_SUBSIDIO = simulacionDetalle.InteresTradicionalSubsidio,
                    SMD_SALDO = simulacionDetalle.Saldo,
                    SMD_SALDO_LEASING = simulacionDetalle.SaldoLeasing,
                    SMD_SEGURO_TERREMOTO = simulacionDetalle.SeguroHogar,
                    SMD_SEGURO_VIDA = simulacionDetalle.SeguroVida,
                    SMD_SEGURO_VIDA_LEASING = simulacionDetalle.SeguroVidaLeasing,
                    SMD_SALDO_REDUCIDO_LEASING = simulacionDetalle.SaldoCapitalReducido,
                    SMD_INTERESES_PAGADOS_LEASING = simulacionDetalle.InteresesPorPagar,
                    SMD_CAPITAL_PAGADO_LEASING = simulacionDetalle.CapitalPorPagar,
                    SMD_CAPITAL_A_PAGAR_LEASING = simulacionDetalle.Capitalpagar,
                    SMD_SALDO_CAPITAL_LEASING = simulacionDetalle.CapitalSaldo,
                    SMD_INTERES_A_PAGAR_LEASING = simulacionDetalle.CapitalInteresporPagar,
                    SMD_SALDO_TOTAL_LEASING = simulacionDetalle.CapitalSaldoTotal,
                    SMD_AFECTACION_INDIVIDUAL_LEASING = simulacionDetalle.AfectacionCuentaIndividual,
                    SMD_ABONO_UNICO = (double)simulacionDetalle.AbonoUnico,
                    AFL_ID = simulacionDetalle.Afl_id,
                    NUMERO_DOCUMENTO = documento,
                    SMD_VALOR_FINANCIAR = simulacionDetalle.ValorFinanciar,
                    SMD_SEGUROS_TOTAL = simulacionDetalle.SegurosTotal,
                    SMD_SCORE = simulacionDetalle.RiesgoMinimoTransUnion
                });

                //inserciones.Add(new Thread(async () =>
                //{
                //    detalles.Add(await this._creditoRepository.CrearDetalleSimulacion(detalle));
                //}));
            };

            //foreach (var insercion in inserciones)
            //    insercion.Start();
            var simulacion = new SimulacionClienteDto();
            //while (simulacion.valorCuota != 0)
            //{
            //    await LimpiarSimulacionCliente(detalles[0].SMC_ID);
            foreach (var detalle in detalles)
            {
                await this._creditoRepository.CrearDetalleSimulacion(detalle);
            }
            simulacion = (await this.ConsultarSimulacionCliente(documento)).Data;
            //}

            return new Response<IEnumerable<SimulacionDetalle>>() { Data = detalles };
        }

        public async Task<Response<SMCSimulacionClienteDto>> CrearSimulacionClienteSMC(SMCSimulacionClienteDto sMCSimulacionClienteDto)
        {
            var request = _mapper.Map<SMCSimulacionCliente>(sMCSimulacionClienteDto);
            return new Response<SMCSimulacionClienteDto>() { Data = _mapper.Map<SMCSimulacionClienteDto>(await this._creditoRepository.CrearSimulacionClienteSMC(request)) };

        }

        public async Task<Response<SolicitudSimulacionDto>> CrearPreAprobado(SolicitudSimulacionDto request)
        {
            try
            {
                var entity = _mapper.Map<SolicitudSimulacion>(request);

                var crearPreAprobado = await _creditoRepository.CrearPreAprobado(entity);

                var result = new Response<SolicitudSimulacionDto>()
                {
                    Data = _mapper.Map<SolicitudSimulacionDto>(crearPreAprobado),
                    IsSuccess = true
                };

                if (result.Data == null)
                {
                    return new Response<SolicitudSimulacionDto>()
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "No fue posible crear el preaprobado"
                    };
                }

                SolicitudSimulacion informacionSimulacion =
                    await _creditoRepository.ObtenerSolicitudSimulacionSLS(request.Id);

                string radicadoActual =
                    informacionSimulacion?.SLS_NUMERO_RADICADO ?? string.Empty;

                DateTime fechaVigencia =
                    informacionSimulacion?.SLS_FECHA_VIGENCIA_361 ?? DateTime.Now;

                int diasDiferencia = (DateTime.Now - fechaVigencia).Days;

                bool generarNuevoRadicado =
                    string.IsNullOrWhiteSpace(radicadoActual) ||
                    diasDiferencia > 30;

                if (generarNuevoRadicado)
                {
                    if (result.Data.NumeroPreAprobado != 0)
                    {
                        request.NumeroPreAprobado = result.Data.NumeroPreAprobado;

                        entity.SLS_NUMERO_PREAPROBADO =
                            result.Data.NumeroPreAprobado;

                        var radicado =
                            await _gestorDocumentalBusinessLogic
                                .RadicarGDPreApronado(request);

                        if (radicado != null &&
                            radicado.IsSuccess &&
                            radicado.ReturnMessage == "El formulario fue guardado")
                        {
                            entity.SLS_NUMERO_RADICADO =
                                radicado.Data?.Form_InsertResult?.BarCode
                                ?? "RADICADO NO DISPONIBLE";

                            entity.SLS_RECORD_ID =
                                radicado.Data?.Form_InsertResult?.RecordId.ToString()
                                ?? string.Empty;

                            await _creditoRepository
                                .ActualizarRadicadoPreAprobado(entity);

                            result.Data.NumRadicadoGD =
                                entity.SLS_NUMERO_RADICADO;

                            result.Data.RecordIdGD =
                                entity.SLS_RECORD_ID;
                        }
                        else
                        {
                            result.Data.NumRadicadoGD =
                                "RADICADO NO DISPONIBLE";
                        }
                    }

                    return result;
                }

                if (diasDiferencia < 30 &&
                    !string.IsNullOrWhiteSpace(radicadoActual))
                {
                    result.Data.NumRadicadoGD = radicadoActual;

                    return result;
                }

                return new Response<SolicitudSimulacionDto>()
                {
                    Data = result.Data,
                    IsSuccess = true,
                    ReturnMessage = "Proceso completado"
                };
            }
            catch (Exception ex)
            {
                return new Response<SolicitudSimulacionDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error en CrearPreAprobado: {ex.Message}"
                };
            }
        }

        public async Task<Response<ResponseFlujo83>> CrearRadicadoPreAprobadoFlujo83(DatosIdentificacionDto request)
        {
            try
            {
                ResponseFlujo83 response = new ResponseFlujo83();

                var cliente = await _clientRequestProvider
                    .ObtenerClientePorTipoIdentificationYNumero(
                        request.TipoIdentificacion,
                        request.Identificacion);

                if (cliente == null || cliente.Data == null)
                {
                    return new Response<ResponseFlujo83>()
                    {
                        IsSuccess = false,
                        ReturnMessage = "No fue posible obtener la información del cliente"
                    };
                }

                SolicitudSimulacion informacionSimulacion =
                    await _creditoRepository.ObtenerSolicitudSimulacionSLS(request.SLS_ID);

                string radicadoActual = informacionSimulacion?.SLS_NUMERO_RADICADO_83 ?? string.Empty;

                DateTime fechaVigencia = informacionSimulacion?.SLS_FECHA_VIGENCIA_83 ?? DateTime.Now;

                int diasDiferencia = (DateTime.Now - fechaVigencia).Days;

                bool generarNuevoRadicado =
                    string.IsNullOrWhiteSpace(radicadoActual) ||
                    diasDiferencia >
                    30;

                if (generarNuevoRadicado)
                {
                    var radicado = await _gestorDocumentalBusinessLogic
                        .RadicadoFlujo83(request);

                    if (radicado != null &&
                        radicado.IsSuccess &&
                        radicado.ReturnMessage == "El formulario fue guardado")
                    {
                        response.Radicado =
                            radicado.Data?.Form_InsertResult?.BarCode
                            ?? "RADICADO NO DISPONIBLE";

                        await _creditoRepository
                            .ActualizarRadicadoPreAprobadoFlujo83(
                                request.SLS_ID,
                                response.Radicado);

                        return new Response<ResponseFlujo83>()
                        {
                            Data = response,
                            IsSuccess = true,
                            ReturnMessage = "Flujo 83 - Radicado generado con éxito"
                        };
                    }

                    response.Radicado = "RADICADO NO DISPONIBLE";

                    return new Response<ResponseFlujo83>()
                    {
                        Data = response,
                        IsSuccess = false,
                        ReturnMessage = "No fue posible generar el radicado"
                    };
                }

                response.Radicado = radicadoActual;

                return new Response<ResponseFlujo83>()
                {
                    Data = response,
                    IsSuccess = true,
                    ReturnMessage = "Flujo 83 - Radicado vigente"
                };
            }
            catch (Exception ex)
            {
                return new Response<ResponseFlujo83>()
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error generando radicado flujo 83: {ex.Message}"
                };
            }
        }





        public async Task<Response<SolicitudCreditoDto>> CrearSolicitudCredito(SolicitudCreditoDto solicitudCredito)
        {
            var _request = _mapper.Map<SolicitudCredito>(solicitudCredito);
            var numeroRadicadoGDDodo = (await _creditoRepository.ConsultarPreAprobado(solicitudCredito.CLI_IDENTIFICACION)).SLS_NUMERO_RADICADO;

            _request.DPC_CIUDAD_NOTARIA = solicitudCredito.DPC_ID_INMUEBLE_NOTARIA;
            _request.DPD_DEPARTAMENTO_NOTARIA = solicitudCredito.DPD_ID_INMUEBLE_NOTARIA;

            var result = new Response<SolicitudCreditoDto>()
            {
                Data = _mapper.Map<SolicitudCreditoDto>(await _creditoRepository.CrearSolicitudCredito(_mapper.Map<SolicitudCredito>(_request)))
            };

            if (result.Data != null)
            {
                //var radicado = await _gestorDocumentalBusinessLogic.RadicarGDSolicitudCredito(result.Data);
                if (!string.IsNullOrEmpty(numeroRadicadoGDDodo))
                {
                    var numradicado = (await _creditoRepository.ActualizarRadicadoSolicitud(result.Data.SOC_ID, numeroRadicadoGDDodo)).SOC_NUMERO_RADICADO;
                    result.Data.SOC_NUMERO_RADICADO = numradicado;
                }
                else
                    result.Data.SOC_NUMERO_RADICADO = "RADICADO NO DISPONIBLE";
            }

            return result;
        }

        public async Task<Response<FormularioCreditoBasicaDto>> FormularioCreditoBasica(Guid idSolicitudCredito)
        {

            IEnumerable<TipoIdentificacionDto> tiposIdentificacion = this._mapper.Map<IEnumerable<TipoIdentificacionDto>>(await this._catalogsRepository.ObtenerTiposIdentificacion());
            IEnumerable<DepartamentoDto> departamentos = this._mapper.Map<IEnumerable<DepartamentoDto>>(await this._catalogsRepository.ObtenerDepartamentos());
            IEnumerable<FuerzasDto> fuerzas = this._mapper.Map<IEnumerable<FuerzasDto>>(await this._catalogsRepository.ObtenerFuerzas());
            IEnumerable<CategoriaDto> categorias = this._mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias());
            IEnumerable<SexoDto> sexoTypes = this._mapper.Map<IEnumerable<SexoDto>>(await this._catalogsRepository.ObtenerTipoDeSexo());
            IEnumerable<EstadoCivilDto> estadoCivilTypes = this._mapper.Map<IEnumerable<EstadoCivilDto>>(await this._catalogsRepository.ObtenerEstadosCiviles());
            IEnumerable<NivelAcademicoCatalogoDto> nivelesAcademicos = this._mapper.Map<IEnumerable<NivelAcademicoCatalogoDto>>(await this._catalogsRepository.ObtenerNivelEducativoCatalogoValor());
            IEnumerable<ProfesionDto> profesiones = this._mapper.Map<IEnumerable<ProfesionDto>>(await this._catalogsRepository.ObtenerProfesiones());
            PeticionCreditoBasica pet = new PeticionCreditoBasica();
            pet.SOC_ID = idSolicitudCredito;
            pet.IND = 2;
            RespuestaCreditoBasicaDto RespuestaCreditoBasica = this._mapper.Map<RespuestaCreditoBasicaDto>(await this._creditoRepository.ObtenerSolicCreditoBasica(pet));
            FormularioCreditoBasicaDto credito = new FormularioCreditoBasicaDto();
            credito.TiposIdentificacion = (List<TipoIdentificacionDto>)tiposIdentificacion;
            credito.Departamentos = (List<DepartamentoDto>)departamentos;
            credito.Fuerzas = (List<FuerzasDto>)fuerzas;
            credito.Categorias = (List<CategoriaDto>)categorias;
            credito.Sexos = (List<SexoDto>)sexoTypes;
            credito.EstadosCiviles = (List<EstadoCivilDto>)estadoCivilTypes;
            credito.NivelesAcademicos = (List<NivelAcademicoCatalogoDto>)nivelesAcademicos;
            credito.Profeciones = (List<ProfesionDto>)profesiones;
            credito.CreditoBasica = RespuestaCreditoBasica;

            return new Response<FormularioCreditoBasicaDto>() { Data = credito };

        }

        public async Task<Response<SocSolicitudInfobasicaDto>> CrearSolicCreditoBasicos(SocSolicitudInfobasicaDto infoBasica)
        {
            SocSolicitudInfobasica request = _mapper.Map<SocSolicitudInfobasica>(infoBasica);
            SocSolicitudInfobasicaDto Respuesta = this._mapper.Map<SocSolicitudInfobasicaDto>(await this._creditoRepository.CrearSolicCreditoBasicos(request));
            return new Response<SocSolicitudInfobasicaDto>() { Data = Respuesta };

        }

        public async Task<Response<IEnumerable<TasaHogarCiudadDto>>> ObtenerTasasHogarCiudad()
        {
            var Respuesta = this._mapper.Map<IEnumerable<TasaHogarCiudadDto>>(await this._creditoRepository.ObtenerTasasHogarCiudad());
            return new Response<IEnumerable<TasaHogarCiudadDto>>() { Data = Respuesta };
        }

        public async Task<Response<IEnumerable<TasaHogarCiudadDto>>> ObtenerTasasHogarUbicacion(int departamentoId)
        {
            var Respuesta = this._mapper.Map<IEnumerable<TasaHogarCiudadDto>>(await this._creditoRepository.ObtenerTasasHogarUbicacion(departamentoId));
            return new Response<IEnumerable<TasaHogarCiudadDto>>() { Data = Respuesta };
        }

        public async Task<Response<FormularioCreditoConyugueDto>> FormularioCreditoConyugue(Guid idSolicitudCredito)
        {
            FormularioCreditoConyugueDto form = new FormularioCreditoConyugueDto();

            IEnumerable<TipoIdentificacionDto> tiposIdentificacion = this._mapper.Map<IEnumerable<TipoIdentificacionDto>>(await this._catalogsRepository.ObtenerTiposIdentificacion());
            IEnumerable<DepartamentoDto> departamentos = this._mapper.Map<IEnumerable<DepartamentoDto>>(await this._catalogsRepository.ObtenerDepartamentos());

            PeticionCreditoBasica pet = new PeticionCreditoBasica();
            pet.SOY_ID = Guid.NewGuid();
            pet.SOC_ID = idSolicitudCredito;
            pet.IND = 2;

            InformacionConyugueDto SolicitudInfoConyugue = this._mapper.Map<InformacionConyugueDto>(await this._creditoRepository.ObtenerSolicCreditoConyugue(pet));

            form.Departamentos = (List<DepartamentoDto>)departamentos;
            form.TiposIdentificacion = (List<TipoIdentificacionDto>)tiposIdentificacion;
            form.InformacionConyugue = SolicitudInfoConyugue;

            return new Response<FormularioCreditoConyugueDto>() { Data = form };


        }

        public async Task<Response<InformacionConyugueDto>> CrearSolicCreditoConyugue(InformacionConyugueDto infoConyugue)
        {
            if (infoConyugue.TipoIdentificacionId == 0 || string.IsNullOrEmpty(infoConyugue.NumeroDocumento))
            {
                InformacionConyugueDto SolicitudInfoConyugueEliminada = this._mapper.Map<InformacionConyugueDto>(await this._creditoRepository.EliminarInformacionSolicConyuge(infoConyugue.SolicitudId));
                return new Response<InformacionConyugueDto>() { Data = SolicitudInfoConyugueEliminada };
            }
            InformacionConyugue InfoConyugue = this._mapper.Map<InformacionConyugue>(infoConyugue);
            InformacionConyugueDto SolicitudInfoConyugue = this._mapper.Map<InformacionConyugueDto>(await this._creditoRepository.CrearSolicCreditoConyugue(InfoConyugue));
            return new Response<InformacionConyugueDto>() { Data = SolicitudInfoConyugue };


        }
        // ============================================================================
        // CierreDiarioCartera - VERSIÓN CORREGIDA
        // ============================================================================
        // Reglas de negocio implementadas:
        //   1. La causación de mora siempre avanza según los días transcurridos
        //      (se suma el interés de HOY sobre lo ya acumulado).
        //   2. "IntMoraAnterior" es únicamente informativo (mora del mes pasado,
        //      congelada el día 1). No participa en el cálculo diario, salvo
        //      en el TotalMora del día 1 (regla 3).
        //   3. TotalMora:
        //        - Día 1 del mes: CausacionInteresesMora + IntMoraAnterior
        //        - Día 2+: solo CausacionInteresesMora (ya lo incluye, no se
        //          vuelve a sumar IntMoraAnterior o se duplicaría).
        //   4. Si un pago parcial cancela POR COMPLETO el interés de mora
        //      acumulado (queda solo saldo de capital pendiente), los días
        //      de mora reinician a 1 y la causación arranca de cero.
        //
        // PENDIENTE DE TU LADO (no incluido aquí porque no tengo el código):
        //   - Agregar CRE_INT_MORA_MES_ANTERIOR al SELECT/mapeo de
        //     ObtenerCreditoEnMoraCarteraPorIdentificacion (repositorio).
        //   - Agregar el parámetro nuevo a la firma C# de
        //     ActualizarCausacionInteresMora (repositorio) — ver
        //     ActualizarCausacionInteresMora.sql para el SP ya corregido.
        //   - Confirmar que CierreDiarioCarteraDto tiene la propiedad
        //     IntMoraAnterior (ya se usaba en el código original, así que
        //     debería existir).
        // ============================================================================

        public async Task<Response<IEnumerable<CierreDiarioCarteraDto>>> CierreDiarioCartera(RequestCierreDiarioCarteraDto request)
        {
            if (request.Actualiza)
            {
                var fechaDiaAnterior = request.FechaCierre.AddDays(-1);
                var requestValidacion = new RequestCierresCausacionesRealizadasDto
                {
                    TiposCierre = new[] { 1 },
                    FechaCierreInicial = fechaDiaAnterior,
                    FechaCierreFinal = fechaDiaAnterior
                };
                var cierresAnterior = await ObtenerCierresCausacionesRealizadas(requestValidacion);
                if (cierresAnterior == null || !cierresAnterior.Data.Any())
                {
                    return new Response<IEnumerable<CierreDiarioCarteraDto>>
                    {
                        IsSuccess = false,
                        ReturnMessage = $"No se puede ejecutar el cierre del {request.FechaCierre:dd/MM/yyyy}. " +
                                        $"Primero debe ejecutar el cierre del día anterior ({fechaDiaAnterior:dd/MM/yyyy}).",
                        Data = null
                    };
                }
            }

            var requestCierreDiario = this._mapper.Map<RequestCierreDiarioCartera>(request);
            var response = this._mapper.Map<IEnumerable<CierreDiarioCarteraDto>>(
                await this._creditoRepository.CierreDiarioCartera(requestCierreDiario));
            var parametrizationCredito = await _carteraRepository.ObtenerParamGeneralesCreditoYCartera();

            bool esDia1 = request.FechaCierre.Day == 1;

            foreach (var item in response)
            {
                var requestPlanpago = new ObtenerProductoCartera()
                {
                    Tcr_Id = item.TipoCredito == "LEASING HABITACIONAL" ? 1 : 2,
                    Nro_Credito = item.NumeroCredito
                };
                var dataPlanPago = await _carteraBusinessLogic.Value.ObtenerPlanPagosCredito(requestPlanpago);
                var cuotaPendiente = dataPlanPago.Data.PlanPago
                    .Where(x => x.Fecha < request.FechaCierre)
                    .OrderByDescending(x => x.Fecha)
                    .FirstOrDefault();
                var dataCredito = await _carteraRepository.ObtenerCreditoEnMoraCarteraPorIdentificacion(item.Identificacion);

                // ═══════════════════════════════════════════════════════
                // REGLA 2: "Interés de mora anterior" — SOLO informativo.
                // Día 1: se congela AHORA, tomando lo que trae AcumuladoInteresesMora
                //        (todavía es el cierre íntegro del mes pasado, nadie lo tocó).
                // Día 2+: se lee el valor YA congelado por la ejecución del día 1.
                // No participa en el cálculo diario salvo en el TotalMora del día 1.
                // ═══════════════════════════════════════════════════════
                item.IntMoraAnterior = esDia1
                    ? item.AcumuladoInteresesMora
                    : dataCredito.CRE_INT_MORA_MES_ANTERIOR;

                // ═══════════════════════════════════════════════════════
                // PASO 3: DÍAS DE MORA
                // ═══════════════════════════════════════════════════════
                var moraReiniciadaPorPago = false;    // controla si la CAUSACIÓN arranca de cero (PASO 5)
                var estaDentroDeParcial = false;        // controla si se respeta el DiasMora calculado (no pisarlo con día del mes)
                var diasMoraAcumuladoOriginal = item.DiasMora;

                if (cuotaPendiente != null && cuotaPendiente.EstadoCuota == "PAGADA")
                {
                    item.FechaUltimoPago = cuotaPendiente.Fecha;
                    item.SaldoCapital = requestPlanpago.Tcr_Id == 1
                        ? cuotaPendiente.CapitalReducido
                        : cuotaPendiente.Capital;
                    var obtenerAplicacionPago = await _carteraRepository.ObtenerAplicacionPago(dataCredito.CRE_ID);
                    var lastAplicationPayment = obtenerAplicacionPago
                        .OrderByDescending(x => x.CPG_FECHA_PAGO)
                        .FirstOrDefault();
                    if (lastAplicationPayment != null && lastAplicationPayment.CPG_FECHA_PAGO > item.FechaProximoPago)
                    {
                        var diasDesdePago = (request.FechaCierre - lastAplicationPayment.CPG_FECHA_PAGO).Days;
                        item.DiasMora = diasDesdePago == 0
                            ? (lastAplicationPayment.CPG_FECHA_PAGO - item.FechaProximoPago).Days
                            : 0;
                    }
                    else
                    {
                        item.DiasMora = 0;
                    }
                }
                else
                {
                    if (cuotaPendiente != null && cuotaPendiente.EstadoCuota == "PAGADA PARCIALMENTE")
                    {
                        var cuotaPagadaParcialmente = dataPlanPago.Data.PlanPago
                            .Where(x => x.EstadoCuota == "PAGADA PARCIALMENTE" && x.Fecha <= request.FechaCierre)
                            .OrderByDescending(x => x.Fecha)
                            .FirstOrDefault();
                        if (cuotaPagadaParcialmente != null)
                        {
                            var movimientosPago = await _carteraRepository.ObtenerAplicacionPago(dataCredito.CRE_ID);
                            var ultimoPago = movimientosPago
                                .Where(x => x.CPG_VALOR_PAGO > 0 && (x.CC_ESTADO_ID == 1 || x.CC_ESTADO_ID == 2))
                                .OrderByDescending(x => x.CPG_FECHA_CREACION)
                                .FirstOrDefault();

                            if (ultimoPago != null)
                            {
                                estaDentroDeParcial = true; // ✅ siempre true dentro de esta rama

                                bool moraIntCanceladaCompleta =
                                    ultimoPago.CPG_ABONO_INTERES_MORA >= dataCredito.CRE_ACUMULADO_INTERES_MORA;
                                bool esElDiaDelPago = ultimoPago.CPG_FECHA_PAGO.Date == request.FechaCierre.Date;

                                if (moraIntCanceladaCompleta && esElDiaDelPago)
                                {
                                    // Día exacto del reinicio: causación arranca de cero.
                                    item.DiasMora = 1;
                                    diasMoraAcumuladoOriginal = 1;
                                    moraReiniciadaPorPago = true;
                                }
                                else if (moraIntCanceladaCompleta)
                                {
                                    // Días después del reinicio: sigue contando desde la fecha
                                    // del pago (+1 porque ese mismo día ya contó como día 1),
                                    // pero la causación CONTINÚA (no vuelve a arrancar de cero).
                                    var diasDesdeVencimiento = (request.FechaCierre - ultimoPago.CPG_FECHA_PAGO).Days + 1;
                                    item.DiasMora = diasDesdeVencimiento > 0 ? diasDesdeVencimiento : 1;
                                    diasMoraAcumuladoOriginal = Math.Max((request.FechaCierre - cuotaPendiente.Fecha).Days, 1);
                                    // moraReiniciadaPorPago se queda en false a propósito
                                }
                                else
                                {
                                    var diasDesdeVencimiento = (request.FechaCierre - ultimoPago.CPG_FECHA_PAGO).Days;
                                    item.DiasMora = diasDesdeVencimiento > 0 ? diasDesdeVencimiento : 1;
                                    diasMoraAcumuladoOriginal = Math.Max((request.FechaCierre - cuotaPendiente.Fecha).Days, 1);
                                    moraReiniciadaPorPago = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (item.DiasMora == 0)
                        {
                            var diasDesdeVencimiento = (request.FechaCierre - item.FechaProximoPago).Days;
                            item.DiasMora = diasDesdeVencimiento > 0 ? diasDesdeVencimiento : 0;
                        }
                        cuotaPendiente = dataPlanPago.Data.PlanPago
                            .Where(x => x.Fecha == item.FechaProximoPago)
                            .FirstOrDefault();
                    }
                }

                var diasMoraParaBD = moraReiniciadaPorPago ? diasMoraAcumuladoOriginal : item.DiasMora;

                //  FIX: ahora se respeta item.DiasMora ya calculado en cualquier caso
                // dentro de "PAGADA PARCIALMENTE" (reinicio o continuación), no solo
                // cuando hay reinicio el mismo día. El "día del mes" (FechaCierre.Day)
                // solo aplica al caso normal de mora sin pago parcial de por medio.
                item.DiasMora = item.DiasMora > 0
                    ? ((moraReiniciadaPorPago || estaDentroDeParcial) ? item.DiasMora : request.FechaCierre.Day)
                    : 0;
                item.DiasAcumuladosMora = diasMoraParaBD;
                // ═══════════════════════════════════════════════════════
                // PASO 3B: CONSOLIDACIÓN DE SALDO EN MORA AL INICIO DE MES
                // ═══════════════════════════════════════════════════════
                if (esDia1 && diasMoraParaBD > 0 && dataCredito.CRE_SALDO_CAPITAL_MORA > 0 && cuotaPendiente != null)
                {
                    var capitalPlanPago = requestPlanpago.Tcr_Id == 1
                        ? cuotaPendiente.CapitalReducido
                        : cuotaPendiente.Capital;
                    item.SaldoCapital = capitalPlanPago + dataCredito.CRE_SALDO_CAPITAL_MORA;
                }

                // ═══════════════════════════════════════════════════════
                // PASO 4: SALDO DE MORA
                // ═══════════════════════════════════════════════════════
                if (dataCredito.CRE_SALDO_CAPITAL_MORA > 0)
                {
                    item.SaldoMora = dataCredito.CRE_SALDO_CAPITAL_MORA;
                }
                else
                {
                    var cuotasVencidas = dataPlanPago.Data.PlanPago
                        .Where(x => x.Fecha.Date <= request.FechaCierre.Date
                                 && x.EstadoCuota != "PAGADA"
                                 && x.EstadoCuota != "PAGADA PARCIALMENTE")
                        .ToList();
                    if (cuotasVencidas.Any())
                    {
                        item.SaldoMora = cuotasVencidas.Sum(x => requestPlanpago.Tcr_Id == 1
                            ? x.CapitalReducido
                            : x.Capital);
                    }
                    else if (cuotaPendiente != null)
                    {
                        item.SaldoMora = requestPlanpago.Tcr_Id == 1
                            ? cuotaPendiente.CapitalReducido
                            : cuotaPendiente.Capital;
                    }
                }

                // ═══════════════════════════════════════════════════════
                // PASO 5: CAUSACIÓN DE MORA
                // REGLA 1: siempre avanza según los días transcurridos.
                // REGLA 3: día 1 arranca solo con el interés de hoy (lo de
                //          antes queda en IntMoraAnterior); el resto de los
                //          días se suma al acumulado persistido de ayer.
                // ═══════════════════════════════════════════════════════
                item.TasaCreditoEA = Math.Round(item.TasaCreditoEA, 6);
                decimal interesDiario = 0;
                if (item.DiasMora > 0)
                {
                    var tasaEAOrdnaria = item.TasaCreditoEA / 100;
                    var tasaDiariaOrdnaria = (decimal)Math.Pow(1 + (double)tasaEAOrdnaria, 1.0 / 365) - 1;
                    var tasaEAMora = parametrizationCredito.PRM_TASA_MORA * item.TasaCreditoEA;
                    var tasaEAMoraDecimal = tasaEAMora / 100;
                    var tasaDiariaM = (decimal)Math.Pow(1 + (double)tasaEAMoraDecimal, 1.0 / 365) - 1;
                    interesDiario = item.SaldoMora * tasaDiariaM;
                    item.CausacionDeHoy = interesDiario;
                    if (moraReiniciadaPorPago)
                    {
                        // Se pagó y reinició: la causación arranca de cero, solo hoy.
                        item.CausacionInteresesMora = interesDiario;
                    }
                    else if (esDia1)
                    {
                        // Arranque del mes: solo el interés de hoy (lo de antes va en IntMoraAnterior).
                        item.CausacionInteresesMora = interesDiario;
                    }
                    else
                    {
                        // Continuación: lo persistido hasta ayer + hoy.
                        item.CausacionInteresesMora = item.AcumuladoInteresesMora + interesDiario;
                    }

                    item.TasaCreditoEAMora = Math.Round(tasaEAMoraDecimal, 6);
                    item.TasaPeriodicaMora = Math.Round(tasaDiariaM * 100, 6);
                }
                else
                {
                    item.CausacionInteresesMora = 0;
                    item.TasaCreditoEAMora = 0;
                    item.TasaPeriodicaMora = 0;
                }

                // ═══════════════════════════════════════════════════════
                // PASO 5B: TOTAL DE MORA
                // REGLA 3: día 1 suma IntMoraAnterior explícitamente; el resto
                // de los días NO se vuelve a sumar (CausacionInteresesMora ya
                // lo incluye desde el día 1).
                // ═══════════════════════════════════════════════════════
                item.TotalMora = esDia1
                    ? item.CausacionInteresesMora + item.IntMoraAnterior
                    : item.CausacionInteresesMora;

                item.AcumuladoInteresesMora = item.TotalMora;
                item.TasaCreditoEAMora = Math.Round(item.TasaCreditoEAMora, 4);

                // ═══════════════════════════════════════════════════════
                // PASO 6: GUARDAR EN BD
                // ═══════════════════════════════════════════════════════
                if (request.Actualiza)
                {
                    var intMoraMesAnteriorAPersistir = esDia1
                        ? item.IntMoraAnterior
                        : dataCredito.CRE_INT_MORA_MES_ANTERIOR;

                    await _carteraRepository.ActualizarCausacionInteresMora(
                        dataCredito.CRE_ID,
                        item.TotalMora,
                        diasMoraParaBD,
                        request.FechaCierre,
                        item.SaldoMora,
                        intMoraMesAnteriorAPersistir); // <-- nuevo parámetro, agregar en el repositorio
                }
            }

            List<CierreDiarioCarteraDto> list = new();
            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(
                response.Select(x => x.Identificacion).Distinct().ToList())).Data;
            list = (from r in response
                    join la in listAfiliados on r.Identificacion equals la.Documento
                    select new CierreDiarioCarteraDto
                    {
                        NumeroCredito = r.NumeroCredito,
                        TipoIdentificacion = r.TipoIdentificacion,
                        Identificacion = r.Identificacion,
                        Nombres = la.NombreCompleto,
                        UnidadEjecutora = r.UnidadEjecutora,
                        Categoria = r.Categoria,
                        Grado = r.Grado,
                        FechaSolicitud = r.FechaSolicitud,
                        TipoCredito = r.TipoCredito,
                        EstadoCredito = r.EstadoCredito,
                        FechaDesembolso = r.FechaDesembolso,
                        FechaFinalizaPagos = r.FechaFinalizaPagos,
                        ValorDesembolso = r.ValorDesembolso,
                        ValorVivienda = r.ValorVivienda,
                        TipoVivienda = r.TipoVivienda,
                        ClasificacionVivienda = r.ClasificacionVivienda,
                        CanonInicial = r.CanonInicial,
                        ValorOpcionCompra = r.ValorOpcionCompra,
                        PorcentajeCanonInicial = r.PorcentajeCanonInicial,
                        PorcentajeOpcionCompra = r.PorcentajeOpcionCompra,
                        CanonExtraordinario = r.CanonExtraordinario,
                        PlazoTotal = r.PlazoTotal,
                        PlazoActual = r.PlazoActual,
                        CuotasCanceladas = r.CuotasCanceladas,
                        TasaSeguroVida = r.TasaSeguroVida,
                        TasaSeguroHogar = r.TasaSeguroHogar,
                        TasaCreditoEA = r.TasaCreditoEA,
                        TasaPeriodica = Math.Round(r.TasaPeriodica * 100, 6),
                        ValorCuota = r.ValorCuota,
                        SaldoCapital = r.SaldoCapital,
                        SaldoMora = r.SaldoMora,
                        DiasMora = r.DiasMora,
                        DiasAcumuladosMora = r.DiasAcumuladosMora,
                        ValorDeudaRemanente = r.ValorDeudaRemanente,
                        FechaEstado = r.FechaEstado,
                        FechaUltimoPago = r.FechaUltimoPago,
                        FechaProximoPago = r.FechaProximoPago,
                        AcumuladoSeguroHogar = r.AcumuladoSeguroHogar,
                        FechaUltimoPagoSeguroHogar = r.FechaUltimoPagoSeguroHogar,
                        AcumuladoSeguroVida = r.AcumuladoSeguroVida,
                        FechaUltimoPagoSeguroVida = r.FechaUltimoPagoSeguroVida,
                        AcumuladoInteresesMora = r.AcumuladoInteresesMora,
                        FechaUltimoPagoInteresesMora = r.FechaUltimoPagoInteresesMora,
                        AcumuladoInteresesCorrientes = r.AcumuladoInteresesCorrientes,
                        FechaUltimoPagoInteresesCorrientes = r.FechaUltimoPagoInteresesCorrientes,
                        Calificacion = r.Calificacion,
                        Provision = r.Provision,
                        ProvisionAdicional = r.ProvisionAdicional,
                        ProvisionPorcentaje = r.ProvisionPorcentaje * 100,
                        CausacionInteresesMora = r.CausacionInteresesMora,
                        TasaCreditoEAMora = r.TasaCreditoEAMora * 100,
                        TasaPeriodicaMora = r.TasaPeriodicaMora,
                        IntMoraAnterior = r.IntMoraAnterior,
                        TotalMora = r.TotalMora,
                        AreaNegocio = r.AreaNegocio,
                        CausacionDeHoy = r.CausacionDeHoy
                    }).ToList();
            return new Response<IEnumerable<CierreDiarioCarteraDto>> { Data = list };
        }

        public async Task<Response<InsertarMovContableResponse>> CierreDiarioCausacionDiariaInteresesMoratoriosSeven(RequestCausacionDiariaInteresesCorrientesDto request, List<CausacionDiariaInteresesCorrientesDto> causacionDiariaInteresesCorrientesDtos = null)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";

            if (request.Actualiza)
            {
                request.Actualiza = false;
                //TODO: validar por que el Sp no trae datos de una fecha especifica de causacion trae o filas 
                var responseCierre = new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = causacionDiariaInteresesCorrientesDtos };
                //await CausacionDiariaInteresesCorrientes(request);
                IEnumerable<TipoCreditoDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCreditoDto>>(await this._catalogsRepository.ObtenerTipoCredito());
                var descripcionLeasing = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
                var descripcionHipotecario = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

                var creditosLeasing = responseCierre.Data.Where(x => x.TipoCredito == descripcionLeasing);
                var creditosHipotecarios = responseCierre.Data.Where(x => x.TipoCredito == descripcionHipotecario);


                var dataMovContaLeasing = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var dataMovContaHipotecario = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsHipotecarios = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

                if (creditosLeasing.Count() > 0)
                {
                    var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);
                    dataMovContaLeasing.Emp_codi = 294;
                    dataMovContaLeasing.Top_codi = 9065;
                    dataMovContaLeasing.Mco_nume = 0;
                    dataMovContaLeasing.Mco_fech = request.FechaCierre;
                    dataMovContaLeasing.Mod_codi = 7;
                    dataMovContaLeasing.Arb_csuc = "1001";
                    dataMovContaLeasing.Mco_desc = "Cierre diario de interes moratorio leasing " + request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss"); ;
                    foreach (var item in creditosLeasing)
                    {
                        //itemsLeasing = await ObtenerDeTalleCausacionDiariaInteresesCorrientesSeven(equivalenciaCuentasLeasing, item, request);
                        itemsLeasing = ObtenerDeTalleCuotaDiariaInteresMoratoriosSeven(equivalenciaCuentasLeasing, item).Result;

                        detalleMovContaLeasing.AddRange(itemsLeasing);
                    }
                    /*Envio consumo contable Seven*/
                    //var respuestaCausacionTotal = await CausacionDiariaTotal(1, creditosLeasing, equivalenciaCuentasLeasing, request.FechaCierre, cancellationToken);
                    //response.ReturnMessage += "Causacion total diario: " + respuestaCausacionTotal.ReturnMessage;

                    if (detalleMovContaLeasing.Count > 0)
                    {
                        dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    }
                    else
                        response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";


                }
                if (creditosHipotecarios.Count() > 0)
                {
                    var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);
                    dataMovContaHipotecario.Emp_codi = 294;
                    dataMovContaHipotecario.Top_codi = 9002;
                    dataMovContaHipotecario.Mco_nume = 0;
                    dataMovContaHipotecario.Mco_fech = request.FechaCierre;
                    dataMovContaHipotecario.Mod_codi = 7;
                    dataMovContaHipotecario.Arb_csuc = "1001";
                    dataMovContaHipotecario.Mco_desc = "Cierre diario de interes moratorio Hipotecario " + request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");
                    foreach (var item in creditosHipotecarios)
                    {
                        //itemsHipotecarios = await ObtenerDeTalleCausacionDiariaInteresesCorrientesSeven(equivalenciaCuentasHipotecario, item, request);
                        itemsHipotecarios = ObtenerDeTalleCuotaDiariaInteresMoratoriosSeven(equivalenciaCuentasHipotecario, item).Result;
                        detalleMovContaHipotecario.AddRange(itemsHipotecarios);
                    }
                    /*Envio consumo contable Seven*/
                    //var respuestaCausacionTotal = await CausacionDiariaTotal(2, creditosHipotecarios, equivalenciaCuentasHipotecario, request.FechaCierre, cancellationToken);
                    //response.ReturnMessage += "Causacion total diario: " + respuestaCausacionTotal.ReturnMessage;

                    if (detalleMovContaHipotecario.Count > 0)
                    {
                        dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();
                    }

                }
                else
                    response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven en el rango de fechas seleccionado";

                if (dataMovContaLeasing.vDetalle?.Count() > 0 || dataMovContaHipotecario.vDetalle?.Count() > 0)
                {
                    if (dataMovContaLeasing.vDetalle.Count() > 0)
                    {

                        decimal totalDmcVacr = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                        decimal totalDmcCadb = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);

                        var jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataMovContaLeasing.vDetalle);

                        //var listasSegementadas = SegmentarListas(dataMovContaLeasing.vDetalle.ToList(),200);
                        RequestDetallePago requestDetallePagoLeasing = new RequestDetallePago();
                        requestDetallePagoLeasing.Emp_codi = dataMovContaLeasing.Emp_codi;
                        requestDetallePagoLeasing.Top_codi = dataMovContaLeasing.Top_codi;
                        requestDetallePagoLeasing.Mco_fech = dataMovContaLeasing.Mco_fech;
                        requestDetallePagoLeasing.Mod_codi = dataMovContaLeasing.Mod_codi;
                        requestDetallePagoLeasing.Mco_desc = dataMovContaLeasing.Mco_desc;
                        requestDetallePagoLeasing.Respuesta_Seven = response.ReturnMessage;
                        requestDetallePagoLeasing.Fecha_Ejecucion = DateTime.Now;

                        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoLeasing, "CIM", totalDmcVacr, totalDmcCadb);
                        requestDetallePagoLeasing.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                        //var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        requestDetallePagoLeasing.Cpg_id = new Guid();
                        requestDetallePagoLeasing.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoLeasing);
                        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoLeasing);

                        //foreach (var listaSegmentada in listasSegementadas)
                        //{
                        //    var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        //    requestDetallePagoLeasing.Cpg_id = new Guid();
                        //    requestDetallePagoLeasing.Mov_Contable_Seven = listaSerializada;
                        //    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoLeasing);
                        //    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoLeasing);
                        //}


                        response.ReturnMessage = requestDetallePagoLeasing.Cc_Id is not null ? "Resultado Comprobante Contable Causacion diario leasing: " + requestDetallePagoLeasing.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                    }
                    if (dataMovContaHipotecario.vDetalle.Count() > 0)
                    {


                        decimal totalDmcVacr = dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr);
                        decimal totalDmcCadb = dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb);

                        string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataMovContaHipotecario.vDetalle);
                        // var listasSegementadas = SegmentarListas(dataMovContaLeasing.vDetalle.ToList(), 200);

                        RequestDetallePago requestDetallePagoHipotecario = new RequestDetallePago();

                        requestDetallePagoHipotecario.Emp_codi = dataMovContaHipotecario.Emp_codi;
                        requestDetallePagoHipotecario.Top_codi = dataMovContaHipotecario.Top_codi;
                        requestDetallePagoHipotecario.Mco_fech = dataMovContaHipotecario.Mco_fech;
                        requestDetallePagoHipotecario.Mod_codi = dataMovContaHipotecario.Mod_codi;
                        requestDetallePagoHipotecario.Mco_desc = dataMovContaHipotecario.Mco_desc;
                        requestDetallePagoHipotecario.Respuesta_Seven = response.ReturnMessage;
                        requestDetallePagoHipotecario.Fecha_Ejecucion = DateTime.Now;
                        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoHipotecario, "CCIC", totalDmcVacr, totalDmcCadb);
                        requestDetallePagoHipotecario.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                        requestDetallePagoHipotecario.Cpg_id = new Guid();
                        requestDetallePagoHipotecario.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);
                        //foreach (var listaSegmentada in listasSegementadas)
                        //{
                        //    var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        //    requestDetallePagoHipotecario.Cpg_id = new Guid();
                        //    requestDetallePagoHipotecario.Mov_Contable_Seven = listaSerializada;
                        //    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                        //    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);
                        //}


                        response.ReturnMessage += requestDetallePagoHipotecario.Cc_Id is not null ? "Resultado Comprobante Contable Causacion diario  hipotecario: " + requestDetallePagoHipotecario.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";

                    }



                }
                return response;
            }

            return response;

        }


        public async Task<Response<InsertarMovContableResponse>> CierreDiarioCarteraInteresMoraSeven(
            RequestCierreDiarioCarteraDto request,
            CancellationToken cancellationToken, List<CierreDiarioCarteraDto> cierreDiarioCarteraDtos)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var respuestaSevenLeasing = "";
            var respuestaSevenHipotecario = "";

            if (request.Actualiza)
            {
                request.Actualiza = false;
                var responseCierre = await CierreDiarioCartera(request);
                if (!responseCierre.IsSuccess && responseCierre.Data == null)
                {
                    return new Response<InsertarMovContableResponse>
                    {
                        IsSuccess = false,
                        ReturnMessage = responseCierre.ReturnMessage,
                        Data = null
                    };
                }
                IEnumerable<TipoCreditoDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCreditoDto>>(
                    await this._catalogsRepository.ObtenerTipoCredito());

                var descripcionCreditosLeasing = catalogoValores
                    .FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
                var descripcionCreditosHipotecarios = catalogoValores
                    .FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

                var creditosLeasing = cierreDiarioCarteraDtos.Where(x => x.TipoCredito == descripcionCreditosLeasing);
                var creditosHipotecarios = cierreDiarioCarteraDtos.Where(x => x.TipoCredito == descripcionCreditosHipotecarios);

                var dataMovContaLeasing = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing =
                    new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var dataMovContaHipotecario = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario =
                    new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsHipotecarios = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

                // ═════════════════════════════════════════════════════════════════════════════
                // PROCESAMIENTO LEASING
                // ═════════════════════════════════════════════════════════════════════════════
                if (creditosLeasing.Count() > 0)
                {
                    var equivalenciaCuentasLeasing = await this._carteraRepository
                        .ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9064);

                    dataMovContaLeasing.Emp_codi = 294;
                    dataMovContaLeasing.Top_codi = 9064;
                    dataMovContaLeasing.Mco_nume = 0;
                    dataMovContaLeasing.Mco_fech = request.FechaCierre;
                    dataMovContaLeasing.Mod_codi = 7;
                    dataMovContaLeasing.Arb_csuc = "1001";
                    dataMovContaLeasing.Mco_desc = "Cierre diario de interes moratorio leasing " +
                        request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                    foreach (var item in creditosLeasing)
                    {
                        itemsLeasing = ObtenerDeTalleCuotaDiariaInteresMoraSeven(equivalenciaCuentasLeasing, item, request.FechaCierre);
                        detalleMovContaLeasing.AddRange(itemsLeasing);
                    }

                    if (detalleMovContaLeasing.Count > 0)
                    {
                        dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();
                    }
                    else
                        response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven.";
                }

                // ═════════════════════════════════════════════════════════════════════════════
                // PROCESAMIENTO HIPOTECARIO
                // ═════════════════════════════════════════════════════════════════════════════
                if (creditosHipotecarios.Count() > 0)
                {
                    var equivalenciaCuentasHipotecario = await this._carteraRepository
                        .ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9002);

                    dataMovContaHipotecario.Emp_codi = 294;
                    dataMovContaHipotecario.Top_codi = 9002;
                    dataMovContaHipotecario.Mco_nume = 0;
                    dataMovContaHipotecario.Mco_fech = request.FechaCierre;
                    dataMovContaHipotecario.Mod_codi = 7;
                    dataMovContaHipotecario.Arb_csuc = "1001";
                    dataMovContaHipotecario.Mco_desc = "Cierre diario de interes moratorio Hipotecario " +
                        request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                    foreach (var item in creditosHipotecarios)
                    {
                        itemsHipotecarios = ObtenerDeTalleCuotaDiariaInteresMoraSeven(equivalenciaCuentasHipotecario, item, request.FechaCierre);
                        detalleMovContaHipotecario.AddRange(itemsHipotecarios);
                    }

                    if (detalleMovContaHipotecario.Count > 0)
                    {
                        dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();
                    }
                    else
                        response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven.";
                }

                // ═════════════════════════════════════════════════════════════════════════════
                // GENERACIÓN DE COMPROBANTES LEASING
                // ═════════════════════════════════════════════════════════════════════════════
                if (dataMovContaLeasing.vDetalle?.Count() > 0)
                {
                    decimal totalDmcVacr = dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr);
                    decimal totalDmcCadb = dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb);

                    RequestDetallePago requestDetallePago = new RequestDetallePago();
                    string jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataMovContaLeasing.vDetalle);

                    requestDetallePago.Cpg_id = new Guid();
                    requestDetallePago.Emp_codi = dataMovContaLeasing.Emp_codi;
                    requestDetallePago.Top_codi = dataMovContaLeasing.Top_codi;
                    requestDetallePago.Mco_fech = dataMovContaLeasing.Mco_fech;
                    requestDetallePago.Mod_codi = dataMovContaLeasing.Mod_codi;
                    requestDetallePago.Mco_desc = dataMovContaLeasing.Mco_desc;
                    requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                    requestDetallePago.Respuesta_Seven = respuestaSevenLeasing;
                    requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                    var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CIM", totalDmcVacr, totalDmcCadb);
                    requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);

                    response.ReturnMessage = requestDetallePago.Cc_Id is not null
                        ? "Resultado Comprobante Contable interno Créditos leasing: " + requestDetallePago.Cc_Id +
                          "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR></CN_MCONT>"
                        : "Error inesperado al crear el comprobante Interno Contable Créditos leasing, " +
                          "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";

                    responseCierre.Data.ToList().ForEach(x => x.ComprobanteInternoLeasing = (int)requestDetallePago.Cc_Id);
                }
                // ═════════════════════════════════════════════════════════════════════════════
                // GENERACIÓN DE COMPROBANTES HIPOTECARIO
                // ═════════════════════════════════════════════════════════════════════════════
                if (dataMovContaHipotecario.vDetalle?.Count() > 0)
                {
                    decimal totalDmcVacr = dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr);
                    decimal totalDmcCadb = dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb);

                    RequestDetallePago requestDetallePago = new RequestDetallePago();
                    string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataMovContaHipotecario.vDetalle);

                    requestDetallePago.Cpg_id = new Guid();
                    requestDetallePago.Emp_codi = dataMovContaHipotecario.Emp_codi;
                    requestDetallePago.Top_codi = dataMovContaHipotecario.Top_codi;
                    requestDetallePago.Mco_fech = dataMovContaHipotecario.Mco_fech;
                    requestDetallePago.Mod_codi = dataMovContaHipotecario.Mod_codi;
                    requestDetallePago.Mco_desc = dataMovContaHipotecario.Mco_desc;
                    requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                    requestDetallePago.Respuesta_Seven = respuestaSevenHipotecario;
                    requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                    var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePago, "CIM", totalDmcVacr, totalDmcCadb);
                    requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);

                    response.ReturnMessage += requestDetallePago.Cc_Id is not null
                        ? "Resultado Comprobante Contable interno Créditos hipotecarios: " + requestDetallePago.Cc_Id +
                          "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR>"
                        : "Error inesperado al crear el comprobante Interno Contable Créditos hipotecarios, " +
                          "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                    responseCierre.Data.ToList().ForEach(x => x.ComprobanteInternoHipotecario = (int)requestDetallePago.Cc_Id);
                }

                return response;
            }

            return response;


            /*vlaidaciones de negocio consumos*/
            void enviarConsumoContableSeven(TSCnMcont dataMovContaLeasing)
            {
                var bodyLeasing = new InsertarMovContableRequestBody(dataMovContaLeasing);
                movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                response.Data = _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken).Result;
                respuestaSevenLeasing = response.Data.Body.InsertarMovContableResult;
                response.ReturnMessage = response.Data is not null ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                    "para créditos de Leasing";
            }
        }

        private List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> ObtenerDeTalleCuotaDiariaInteresMoraSeven(
            IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
            CierreDiarioCarteraDto pago, DateTime fechaCierre)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

            // Obtener 1 débito y 1 crédito de intereses
            var cuentasDebito = cuentasContables
                .Where(x => x.TIPO_OPERACION == "D" &&
                            !x.REF_COLUMN_APLICA_PAGO.ToUpper().Contains("SEGURO"))
                .FirstOrDefault();

            var cuentasCredito = cuentasContables
                .Where(x => x.TIPO_OPERACION == "C" &&
                            (x.REF_COLUMN_APLICA_PAGO.ToUpper().Contains("INTERES") ||
                             x.REF_COLUMN_APLICA_PAGO.ToUpper().Contains("RENDIMIENTO")))
                .FirstOrDefault();
            bool esDia1 = fechaCierre.Day == 1;

            decimal causacionDeHoy = esDia1
                ? pago.CausacionInteresesMora  // día 1: toda la causación es "de hoy" (lo de antes ya se contabilizó en meses previos, vía IntMoraAnterior)
                : pago.CausacionInteresesMora; //

            // MOVIMIENTO 1: DÉBITO por crédito
            if (cuentasDebito != null && pago.AcumuladoInteresesMora > 0)
            {
                var itemDebito = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();
                itemDebito.Cue_codi = cuentasDebito.CUENTA_CONTABLE_SEVEN;
                itemDebito.Dmc_desc = $"Cliente: {pago.Nombres}";
                itemDebito.Dmc_cant = 1;
                itemDebito.Dmc_acti = 0;
                itemDebito.Dmc_refe = pago.NumeroCredito.ToString();
                itemDebito.Dmc_vadb = pago.CausacionDeHoy;  // DÉBITO
                itemDebito.Dmc_vacr = 0;                             // NO CRÉDITO
                itemDebito.Dmc_vaba = 0;
                itemDebito.Ter_coda = pago.Identificacion;
                itemDebito.Arb_codc = cuentasDebito.TCR_ID == 1 ? "601102" : "601103";
                itemDebito.Arb_coda = pago.AreaNegocio.ToString();
                itemDebito.Arb_codp = "099";
                itemDebito.Arb_cods = "1001";
                itemDebito.Ter_codm = "0";
                items.Add(itemDebito);
            }

            // MOVIMIENTO 2: CRÉDITO por crédito
            if (cuentasCredito != null && pago.AcumuladoInteresesMora > 0)
            {
                var itemCredito = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();
                itemCredito.Cue_codi = cuentasCredito.CUENTA_CONTABLE_SEVEN;
                itemCredito.Dmc_desc = $"Cliente: {pago.Nombres}";
                itemCredito.Dmc_cant = 1;
                itemCredito.Dmc_acti = 0;
                itemCredito.Dmc_refe = pago.NumeroCredito.ToString();
                itemCredito.Dmc_vadb = 0;                             // NO DÉBITO
                itemCredito.Dmc_vacr = pago.CausacionDeHoy;  // CRÉDITO
                itemCredito.Dmc_vaba = 0;
                itemCredito.Ter_coda = pago.Identificacion;
                itemCredito.Arb_codc = cuentasCredito.TCR_ID == 1 ? "601102" : "601103";
                itemCredito.Arb_coda = pago.AreaNegocio.ToString();
                itemCredito.Arb_codp = "099";
                itemCredito.Arb_cods = "1001";
                itemCredito.Ter_codm = "0";
                items.Add(itemCredito);
            }
            return items;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCuotaDiariaInteresMoratoriosSeven(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, CausacionDiariaInteresesCorrientesDto pago)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));

            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = $"cliente: " + pago.Identificacion;
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = pago.NumeroCredito.ToString();
                itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? pago.AcumuladoInteresesMora : 0;
                itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? pago.AcumuladoInteresesMora : 0;
                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = pago.Identificacion;
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = "3011";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }

        public async Task<Response<InsertarMovContableResponse>> CierreDiarioCausacionDiariaInteresesCorrientesSeven(RequestCausacionDiariaInteresesCorrientesDto request, List<CausacionDiariaInteresesCorrientesDto> causacionDiariaInteresesCorrientesDtos = null)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

            if (request.Actualiza)
            {
                request.Actualiza = false;
                //TODO: validar por que el Sp no trae datos de una fecha especifica de causacion trae o filas 
                var responseCierre = new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = causacionDiariaInteresesCorrientesDtos };//await CausacionDiariaInteresesCorrientes(request);
                IEnumerable<TipoCreditoDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCreditoDto>>(await this._catalogsRepository.ObtenerTipoCredito());
                var descripcionLeasing = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "LEASING_HABITACIONAL").TCR_DESCRIPCION;
                var descripcionHipotecario = catalogoValores.FirstOrDefault(x => x.TCR_CODIGO == "HIPOTECARIO").TCR_DESCRIPCION;

                var creditosLeasing = responseCierre.Data.Where(x => x.TipoCredito == descripcionLeasing);
                var creditosHipotecarios = responseCierre.Data.Where(x => x.TipoCredito == descripcionHipotecario);


                var dataMovContaLeasing = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var dataMovContaHipotecario = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var itemsHipotecarios = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                if (creditosLeasing.Count() > 0)
                {
                    var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9064);
                    var cuentasAfectar = equivalenciaCuentasLeasing.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Interes Leasing"));
                    dataMovContaLeasing.Emp_codi = 294;
                    dataMovContaLeasing.Top_codi = 9064;
                    dataMovContaLeasing.Mco_nume = 0;
                    dataMovContaLeasing.Mco_fech = request.FechaCierre;
                    dataMovContaLeasing.Mod_codi = 7;
                    dataMovContaLeasing.Arb_csuc = "1001";
                    dataMovContaLeasing.Mco_desc = "Causación diaria  leasing " + request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");
                    foreach (var item in creditosLeasing)
                    {
                        itemsLeasing = await ObtenerDeTalleCausacionDiariaInteresesCorrientesSeven(cuentasAfectar, item, request);
                        detalleMovContaLeasing.AddRange(itemsLeasing);
                    }
                    /*Envio consumo contable Seven*/
                    //var respuestaCausacionTotal = await CausacionDiariaTotal(1, creditosLeasing, equivalenciaCuentasLeasing, request.FechaCierre, cancellationToken);
                    //response.ReturnMessage += "Causacion total diario: " + respuestaCausacionTotal.ReturnMessage;

                    if (detalleMovContaLeasing.Count > 0)
                    {
                        dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    }

                }

                if (creditosHipotecarios.Count() > 0)
                {
                    var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9002);
                    dataMovContaHipotecario.Emp_codi = 294;
                    dataMovContaHipotecario.Top_codi = 9002;
                    dataMovContaHipotecario.Mco_nume = 0;
                    dataMovContaHipotecario.Mco_fech = request.FechaCierre;
                    dataMovContaHipotecario.Mod_codi = 7;
                    dataMovContaHipotecario.Arb_csuc = "1001";
                    dataMovContaHipotecario.Mco_desc = "Causación diaria Hipotecarios " + request.FechaCierre.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");
                    foreach (var item in creditosHipotecarios)
                    {
                        itemsHipotecarios = await ObtenerDeTalleCausacionDiariaInteresesCorrientesSeven(equivalenciaCuentasHipotecario, item, request);
                        detalleMovContaHipotecario.AddRange(itemsHipotecarios);
                    }
                    /*Envio consumo contable Seven*/
                    //var respuestaCausacionTotal = await CausacionDiariaTotal(2, creditosHipotecarios, equivalenciaCuentasHipotecario, request.FechaCierre, cancellationToken);
                    //response.ReturnMessage += "Causacion total diario: " + respuestaCausacionTotal.ReturnMessage;

                    if (detalleMovContaHipotecario.Count > 0)
                    {
                        dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();
                    }

                }
                if (dataMovContaLeasing.vDetalle?.Count() > 0 || dataMovContaHipotecario.vDetalle?.Count() > 0)
                {
                    if (dataMovContaLeasing.vDetalle.Count() > 0)
                    {

                        decimal totalDmcVacr = dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vacr);
                        decimal totalDmcCadb = dataMovContaLeasing.vDetalle.Sum(x => x.Dmc_vadb);

                        var jsonMovimientosContablesLeasing = System.Text.Json.JsonSerializer.Serialize(dataMovContaLeasing.vDetalle);

                        //var listasSegementadas = SegmentarListas(dataMovContaLeasing.vDetalle.ToList(),200);
                        RequestDetallePago requestDetallePagoLeasing = new RequestDetallePago();
                        requestDetallePagoLeasing.Emp_codi = dataMovContaLeasing.Emp_codi;
                        requestDetallePagoLeasing.Top_codi = dataMovContaLeasing.Top_codi;
                        requestDetallePagoLeasing.Mco_fech = dataMovContaLeasing.Mco_fech;
                        requestDetallePagoLeasing.Mod_codi = dataMovContaLeasing.Mod_codi;
                        requestDetallePagoLeasing.Mco_desc = dataMovContaLeasing.Mco_desc;
                        requestDetallePagoLeasing.Respuesta_Seven = response.ReturnMessage;
                        requestDetallePagoLeasing.Fecha_Ejecucion = DateTime.Now;

                        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoLeasing, "CCIC", totalDmcVacr, totalDmcCadb);
                        requestDetallePagoLeasing.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                        //var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        requestDetallePagoLeasing.Cpg_id = new Guid();
                        requestDetallePagoLeasing.Mov_Contable_Seven = jsonMovimientosContablesLeasing;
                        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoLeasing);
                        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoLeasing);

                        //foreach (var listaSegmentada in listasSegementadas)
                        //{
                        //    var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        //    requestDetallePagoLeasing.Cpg_id = new Guid();
                        //    requestDetallePagoLeasing.Mov_Contable_Seven = listaSerializada;
                        //    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoLeasing);
                        //    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoLeasing);
                        //}


                        response.ReturnMessage = requestDetallePagoLeasing.Cc_Id is not null ? "Resultado Comprobante Contable Causacion diario leasing: " + requestDetallePagoLeasing.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";
                    }
                    if (dataMovContaHipotecario.vDetalle.Count() > 0)
                    {


                        decimal totalDmcVacr = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vacr), 2);
                        decimal totalDmcCadb = Math.Round(dataMovContaHipotecario.vDetalle.Sum(x => x.Dmc_vadb), 2);


                        string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataMovContaHipotecario.vDetalle);
                        // var listasSegementadas = SegmentarListas(dataMovContaLeasing.vDetalle.ToList(), 200);

                        RequestDetallePago requestDetallePagoHipotecario = new RequestDetallePago();

                        requestDetallePagoHipotecario.Emp_codi = dataMovContaHipotecario.Emp_codi;
                        requestDetallePagoHipotecario.Top_codi = dataMovContaHipotecario.Top_codi;
                        requestDetallePagoHipotecario.Mco_fech = dataMovContaHipotecario.Mco_fech;
                        requestDetallePagoHipotecario.Mod_codi = dataMovContaHipotecario.Mod_codi;
                        requestDetallePagoHipotecario.Mco_desc = dataMovContaHipotecario.Mco_desc;
                        requestDetallePagoHipotecario.Respuesta_Seven = response.ReturnMessage;
                        requestDetallePagoHipotecario.Fecha_Ejecucion = DateTime.Now;
                        var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoHipotecario, "CCIC", totalDmcVacr, totalDmcCadb);
                        requestDetallePagoHipotecario.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;

                        requestDetallePagoHipotecario.Cpg_id = new Guid();
                        requestDetallePagoHipotecario.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                        await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                        await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);
                        //foreach (var listaSegmentada in listasSegementadas)
                        //{
                        //    var listaSerializada = System.Text.Json.JsonSerializer.Serialize(listaSegmentada);
                        //    requestDetallePagoHipotecario.Cpg_id = new Guid();
                        //    requestDetallePagoHipotecario.Mov_Contable_Seven = listaSerializada;
                        //    await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoHipotecario);
                        //    await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoHipotecario);
                        //}


                        response.ReturnMessage += requestDetallePagoHipotecario.Cc_Id is not null ? "Resultado Comprobante Contable Causacion diario  hipotecario: " + requestDetallePagoHipotecario.Cc_Id + "<CN_MCONT><RETORNO>0</RETORNO> <TXTERROR></TXTERROR> </CN_MCONT>" : "Error inesperado al crear el comprabante Interno Contable Créditos leasing, " + "<CN_MCONT><RETORNO></RETORNO> <TXTERROR></TXTERROR></CN_MCONT>";

                    }



                }
                return response;
            }

            return response;

        }

        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCausacionDiariaInteresesCorrientesSeven(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, CausacionDiariaInteresesCorrientesDto pago, RequestCausacionDiariaInteresesCorrientesDto request)
        {

            int ultimoDia = DateTime.DaysInMonth(request.FechaCierre.Year, request.FechaCierre.Month);
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;
            decimal seguroHogarDiario = 0; decimal seguroVidaDiario = 0; decimal interesCorrienteDiario = 0; decimal interesRemanenteDiario = 0;
            interesCorrienteDiario = Math.Round(pago.CausacionInteresesCorriente, 5);
            seguroHogarDiario = Math.Round(pago.CausacionSeguroHogar, 5);
            decimal intereses = 0;
            if (pago.CausacionInteresesRemanente > 0)
            {

                interesRemanenteDiario = Math.Round(pago.CausacionInteresesRemanente, 5);

            }
            if (pago.CausacionSeguroVida > 0)
            {
                seguroVidaDiario = Math.Round(pago.CausacionSeguroVida, 5);
            }
            if (seguroVidaDiario == 0 && interesRemanenteDiario == 0)
            {
                cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes"
                || x.REF_COLUMN_APLICA_PAGO == "Seguro vida" || x.REF_COLUMN_APLICA_PAGO == "Seguro vida deudor"
                || x.REF_COLUMN_APLICA_PAGO == "Hipotecario Seguro Vida" || x.REF_COLUMN_APLICA_PAGO == "Cobrar Seguro Vida")).ToList();

            }
            else if (seguroVidaDiario == 0)
            {
                cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Seguro vida" || x.REF_COLUMN_APLICA_PAGO == "Seguro vida deudor"
                || x.REF_COLUMN_APLICA_PAGO == "Hipotecario Seguro Vida" || x.REF_COLUMN_APLICA_PAGO == "Cobrar Seguro Vida")).ToList();
            }
            else if (interesRemanenteDiario == 0)
            {
                cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "Categoria A remanentes"));
            }
            intereses = Math.Round(interesCorrienteDiario, 5) + Math.Round(interesRemanenteDiario, 5);
            var intereses2 = interesCorrienteDiario + interesRemanenteDiario;
            if (intereses2 != intereses)
            {

                Console.WriteLine("diferencia encontrada");
            }

            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                    {
                        { "Categoria A reducidos", interesCorrienteDiario },
                        { "Categoria A remanentes", interesRemanenteDiario },
                        { "Seguro vida", seguroVidaDiario },
                        { "Seguro todo riesgo", seguroHogarDiario },
                        { "Interes Corriente Leasing", interesCorrienteDiario },
                        { "Interes Remanente Leasing", interesRemanenteDiario },
                        { "Seguro vida deudor", seguroVidaDiario },
                        { "Seguro todo hogar", seguroHogarDiario }
                    };
            Dictionary<string, decimal> mapeoPagosHipotecario = new Dictionary<string, decimal>
                    {
                        { "Categoria A Riesgo Normal", interesCorrienteDiario },
                        { "Hipotecario Seguro Vida", seguroVidaDiario },
                        { "Hipotecario Seguro Hogar", seguroHogarDiario },
                        { "Intereses y rendimientos", interesCorrienteDiario },
                        { "Cobrar Seguro Vida", seguroVidaDiario },
                        { "Cobrar Seguro Hogar", seguroHogarDiario }
                    };
            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = $"cliente: " + pago.Nombres;
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = pago.NumeroCredito.ToString();
                if (item.TCR_ID == 1)
                {
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                }
                else
                {
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                }

                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = pago.Identificacion + ";" + pago.AreaNegocio;
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = pago.AreaNegocio ?? "3011";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }
        private int GetValueResponseXmlSevenCartera(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("MCO_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("MCO_NUME");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                case 2:
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }
        public async Task<Response<InsertarMovContableResponse>> CausacionDiariaTotal(int tipoCausacion, IEnumerable<CausacionDiariaInteresesCorrientesDto> request, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, DateTime fechaCierreDiarioSeven, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataMovContaLeasing = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var dataMovContaHipotecario = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

            if (tipoCausacion == 1)
            {
                dataMovContaLeasing.Emp_codi = 294;
                dataMovContaLeasing.Top_codi = 9064;
                dataMovContaLeasing.Mco_nume = 0;
                dataMovContaLeasing.Mco_fech = fechaCierreDiarioSeven;
                dataMovContaLeasing.Mod_codi = 7;
                dataMovContaLeasing.Arb_csuc = "1001";
                dataMovContaLeasing.Mco_desc = "Causación diaria  leasing " + fechaCierreDiarioSeven.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                var itemsLeasing = ObtenerDeTalleCausacionDiariaTotales(request, cuentasContables, fechaCierreDiarioSeven).Result;
                detalleMovContaLeasing.AddRange(itemsLeasing);

                if (detalleMovContaLeasing.Count > 0)
                {
                    dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    var bodyLeasing = new InsertarMovContableRequestBody(dataMovContaLeasing);
                    movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);

                    response.ReturnMessage = response.Data is not null ? "Resultado causacion total leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos de Leasing";
                }
                else
                    response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";

            }

            if (tipoCausacion == 2)
            {
                dataMovContaHipotecario.Emp_codi = 294;
                dataMovContaHipotecario.Top_codi = 9002;
                dataMovContaHipotecario.Mco_nume = 0;
                dataMovContaHipotecario.Mco_fech = fechaCierreDiarioSeven;
                dataMovContaHipotecario.Mod_codi = 7;
                dataMovContaHipotecario.Arb_csuc = "1001";
                dataMovContaHipotecario.Mco_desc = "Causación diaria Hipotecarios " + fechaCierreDiarioSeven.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss");

                var itemHipotecario = ObtenerDeTalleCausacionDiariaTotales(request, cuentasContables, fechaCierreDiarioSeven).Result;
                detalleMovContaHipotecario.AddRange(itemHipotecario);

                if (detalleMovContaHipotecario.Count > 0)
                {
                    dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataMovContaHipotecario);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    response.ReturnMessage += response.Data is not null ? " Resultado causacion total Hipotecario: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos Hipotecarios";
                }
                else
                    response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven en el rango de fechas seleccionado";

            }
            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCausacionDiariaTotales(IEnumerable<CausacionDiariaInteresesCorrientesDto> request, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, DateTime fechaCierreDiarioSeven)
        {

            int ultimoDia = DateTime.DaysInMonth(fechaCierreDiarioSeven.Year, fechaCierreDiarioSeven.Month);
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;
            decimal seguroHogarDiario = 0; decimal seguroVidaDiario = 0; decimal interesCorrienteDiario = 0; decimal interesRemanenteDiario = 0;
            decimal intereses = 0;
            foreach (var pago in request)
            {
                interesCorrienteDiario += Math.Round(pago.CausacionInteresesCorriente, 2);
                seguroHogarDiario += Math.Round(pago.CausacionSeguroHogar, 2);
                seguroVidaDiario += Math.Round(pago.CausacionSeguroVida, 2);
                interesRemanenteDiario += Math.Round(pago.CausacionInteresesRemanente, 2);

            }
            intereses = interesRemanenteDiario + interesCorrienteDiario;

            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                    {
                        { "Categoria A reducidos", interesCorrienteDiario },
                        { "Categoria A remanentes", interesRemanenteDiario },
                        { "Seguro vida", seguroVidaDiario },
                        { "Seguro todo riesgo", seguroHogarDiario },
                        { "Interes Leasing", intereses },
                        { "Seguro vida deudor", seguroVidaDiario },
                        { "Seguro todo hogar", seguroHogarDiario }
                    };
            Dictionary<string, decimal> mapeoPagosHipotecario = new Dictionary<string, decimal>
                    {
                        { "Categoria A Riesgo Normal", interesCorrienteDiario },
                        { "Hipotecario Seguro Vida", seguroVidaDiario },
                        { "Hipotecario Seguro Hogar", seguroHogarDiario },
                        { "Intereses y rendimientos", intereses },
                        { "Cobrar Seguro Vida", seguroVidaDiario },
                        { "Cobrar Seguro Hogar", seguroHogarDiario }
                    };
            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = item.TCR_ID == 1 ? "000201507" : "000201508";
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = item.TCR_ID == 1 ? "000201507" : "000201508";
                if (item.TCR_ID == 1)
                {
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                }
                else
                {
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagosHipotecario[item.REF_COLUMN_APLICA_PAGO] : 0;
                }

                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = item.TCR_ID == 1 ? "000201507" : "000201508";
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = "3011";
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }
        public async Task<Response<InsertarMovContableResponse>> CierreDiarioCarteraSeven(CierreDiarioDto request, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

            var aplicacionPagos = await this._carteraRepository.ObtenerAplicacionPagoByFechaPago(request);
            var creditosLeasing = aplicacionPagos.Where(x => x.TCR_ID == 1);
            var creditosHipotecarios = aplicacionPagos.Where(x => x.TCR_ID == 2);

            var dataMovContaLeasing = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var dataMovContaHipotecario = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

            var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9065);
            var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9003);

            if (creditosLeasing.Count() > 0)
            {
                dataMovContaLeasing.Emp_codi = 294;
                dataMovContaLeasing.Top_codi = 9065;//leasing cod. tipo operación 9064
                dataMovContaLeasing.Mco_nume = 0;
                dataMovContaLeasing.Mco_fech = DateTime.Now;
                dataMovContaLeasing.Mod_codi = 7;
                dataMovContaLeasing.Arb_csuc = "1001";
                dataMovContaLeasing.Mco_desc = "Aplicación Pagos Cierre Diario Créditos Leasing.";
                foreach (var item in creditosLeasing)
                {
                    var items = ObtenerDeTalleCuotaDiariaSeven(equivalenciaCuentasLeasing, item).Result;
                    detalleMovContaLeasing.AddRange(items);
                }

                if (detalleMovContaLeasing.Count > 0)
                {
                    dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    var bodyLeasing = new InsertarMovContableRequestBody(dataMovContaLeasing);
                    movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);

                    response.ReturnMessage = response.Data is not null ? "Resultado Mov. Contable Seven Créditos leasing: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos de Leasing";
                }
                else
                    response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";


            }


            if (creditosHipotecarios.Count() > 0)
            {
                dataMovContaHipotecario.Emp_codi = 294;
                dataMovContaHipotecario.Top_codi = 9003;
                dataMovContaHipotecario.Mco_nume = 0;
                dataMovContaHipotecario.Mco_fech = DateTime.Now;
                dataMovContaHipotecario.Mod_codi = 7;
                dataMovContaHipotecario.Arb_csuc = "1001";
                dataMovContaHipotecario.Mco_desc = "Aplicación Pagos Cierre Diario Créditos Hipotecarios";
                foreach (var item in creditosHipotecarios)
                {
                    var items = ObtenerDeTalleCuotaDiariaSeven(equivalenciaCuentasLeasing, item).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }

                if (detalleMovContaHipotecario.Count > 0)
                {
                    dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataMovContaHipotecario);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    response.ReturnMessage += response.Data is not null ? " Resultado Mov.Contable Seven Créditos Hipotecario: " + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos Hipotecarios";
                }
                else
                    response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven en el rango de fechas seleccionado";

            }

            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleCuotaDiariaSeven(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, AplicacionPago pago)
        {

            var sumaIntereses = pago.CPG_ABONO_INTERES_CORRIENTE + pago.CPG_ABONO_INTERES_MORA + pago.CPG_ABONO_INTERES_REMANENTE;
            var sumaCapital = pago.CPG_ABONO_CAPITAL + pago.CPG_ABONO_CAPITAL_MORA + pago.CPG_ABONO_CAPITAL_REMANENTE;
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var sumaTotal = pago.CPG_ABONO_INTERES_MORA + pago.CPG_ABONO_INTERES_CORRIENTE + pago.CPG_ABONO_INTERES_REMANENTE + pago.CPG_ABONO_CAPITAL_REMANENTE
                + pago.CPG_ABONO_CAPITAL_MORA + pago.CPG_ABONO_CAPITAL + pago.CPG_ABONO_SEGURO_HOGAR + pago.CPG_ABONO_SEGURO_VIDA;
            if (pago.CPG_VALOR_PAGO > 0 && sumaTotal == pago.CPG_VALOR_PAGO)
            {

                //consulta generica--debo verificar primero
                var cuentasAfectar = cuentasContables;

                if (sumaIntereses > 0 && sumaCapital > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE" || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital > 0 && sumaIntereses == 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_CAPITAL" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital == 0 && sumaIntereses > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital > 0 && sumaIntereses == 0 && pago.CPG_ABONO_SEGURO_VIDA > 0 && pago.CPG_ABONO_SEGURO_HOGAR > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_INTERES_CORRIENTE"));
                }
                if (sumaCapital == 0 && sumaIntereses == 0 && pago.CPG_ABONO_SEGURO_VIDA > 0 && pago.CPG_ABONO_SEGURO_HOGAR > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA" || x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_HOGAR" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital == 0 && sumaIntereses == 0 && pago.CPG_ABONO_SEGURO_VIDA == 0 && pago.CPG_ABONO_SEGURO_HOGAR > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_HOGAR" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital == 0 && sumaIntereses == 0 && pago.CPG_ABONO_SEGURO_VIDA > 0 && pago.CPG_ABONO_SEGURO_HOGAR == 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA" || x.REF_COLUMN_APLICA_PAGO == "CPG_VALOR_PAGO"));
                }
                if (sumaCapital > 0 && sumaIntereses > 0 && pago.CPG_ABONO_SEGURO_VIDA > 0 && pago.CPG_ABONO_SEGURO_HOGAR > 0)
                {
                    cuentasAfectar = cuentasContables;
                }
                if (sumaCapital > 0 && sumaIntereses > 0 && pago.CPG_ABONO_SEGURO_VIDA > 0 && pago.CPG_ABONO_SEGURO_HOGAR == 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_HOGAR"));
                }
                if (sumaCapital > 0 && sumaIntereses > 0 && pago.CPG_ABONO_SEGURO_VIDA == 0 && pago.CPG_ABONO_SEGURO_HOGAR > 0)
                {
                    cuentasAfectar = cuentasContables.Where(x => !(x.REF_COLUMN_APLICA_PAGO == "CPG_ABONO_SEGURO_VIDA"));
                }
                Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
                {
                    { "CPG_ABONO_CAPITAL", sumaCapital },//credito
                    { "CPG_ABONO_INTERES_CORRIENTE", sumaIntereses },
                    { "CPG_ABONO_SEGURO_HOGAR", pago.CPG_ABONO_SEGURO_HOGAR },
                    { "CPG_ABONO_SEGURO_VIDA", pago.CPG_ABONO_SEGURO_VIDA },
                    { "CPG_VALOR_PAGO", pago.CPG_VALOR_PAGO } //DEBITO
                };

                foreach (var item in cuentasAfectar)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                    itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                    itemDetalle.Dmc_desc = $"cliente: " + pago.CLI_IDENTIFICACION;
                    itemDetalle.Dmc_cant = 1;
                    itemDetalle.Dmc_acti = 0;
                    itemDetalle.Dmc_refe = pago.CRE_NRO_CREDITO.ToString();
                    itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0;
                    itemDetalle.Dmc_vaba = 0;
                    itemDetalle.Ter_coda = pago.CLI_IDENTIFICACION;
                    itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                    itemDetalle.Arb_coda = "3011";
                    itemDetalle.Arb_codp = "099";
                    itemDetalle.Arb_cods = "1001";
                    itemDetalle.Ter_codm = "0";

                    if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                    {
                        items.Add(itemDetalle);
                    }
                }
            }
            return items;
        }

        public async Task<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>> CausacionDiariaInteresesCorrientes(
          RequestCausacionDiariaInteresesCorrientesDto request)
        {
            var response = new List<CausacionDiariaInteresesCorrientesDto>();
            var fechaCierre = request.FechaCierre;
            DateTime horaInicio = DateTime.Now;

            try
            {
                if (request.Actualiza)
                {
                    var fechaDiaAnterior = request.FechaCierre.AddDays(-1);
                    var requestValidacion = new RequestCierresCausacionesRealizadasDto
                    {
                        TiposCierre = new[] { 3 },
                        FechaCierreInicial = fechaDiaAnterior,
                        FechaCierreFinal = fechaDiaAnterior
                    };

                    var cierresAnterior = await ObtenerCierresCausacionesRealizadas(requestValidacion);

                    if (cierresAnterior == null || !cierresAnterior.Data.Any())
                    {
                        return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>
                        {
                            IsSuccess = false,
                            ReturnMessage = $"No se puede ejecutar el cierre del {request.FechaCierre:dd/MM/yyyy}. " +
                                                 $"Primero debe ejecutar el cierre del día anterior ({fechaDiaAnterior:dd/MM/yyyy}).",
                            Data = null
                        };
                    }
                }

                // MEJORA: CausacionDiariaInteresCorriente ya procesa todos sin discriminar
                // Aquí solo llamamos UNA VEZ, no por tipo de crédito

                var causacionesResult = await EjecutarCausacionDiariaInteresesCorrientes(
                    request);

                // ===============================================
                // PASO 4: PROCESAR RESULTADOS POR TIPO (BATCH)
                // ===============================================
                // MEJORA: Agrupar por tipo y guardar en batch (4 queries en lugar de 3142)
                // Esto reduce el tiempo de 16-20s a 3-6s para 1571 créditos

                if (causacionesResult != null
                    && causacionesResult.Count > 0)
                {
                    decimal totalCausacionDiaria = 0m;
                    int totalCreditos = causacionesResult.Count;
                    if (request.Actualiza)
                    {
                        var fechaPlanPago = new DateTime(
                            request.FechaCierre.Year,
                            request.FechaCierre.Month, 1)
                            .AddDays(-1);
                        // Corregir SaldoCapital Leasing desde plan de pagos
                        var leasingCreditos = causacionesResult
                            .Where(c => c.IdTipoCredito == 1)
                            .Select(c => (c.NumeroCredito, c.IdTipoCredito))
                            .Distinct()
                            .ToList();

                        if (leasingCreditos.Any())
                        {
                            var planesPago = await _carteraBusinessLogic.Value.ObtenerPlanPagoEnBatch(
                                leasingCreditos, fechaPlanPago);

                            foreach (var causacion in causacionesResult
                                .Where(c => c.IdTipoCredito == 1))
                            {
                                if (planesPago.TryGetValue(
                                    (causacion.NumeroCredito, causacion.IdTipoCredito),
                                    out var plan) && plan != null)
                                {
                                    causacion.SaldoCapital = plan.SaldoCapital; // ← confirmar campo
                                }
                            }
                        }
                        var leasings = causacionesResult.Where(c => c.IdTipoCredito == 1).ToList();
                        var hipotecarios = causacionesResult
                            .Where(c => c.IdTipoCredito == 2)
                            .ToList();
                        if (leasings.Count > 0)
                        {
                            // 1 query para insertar todos los históricos
                            await _creditoRepository.InsertarCausacionDiariaLeasingHistoricoEnBatchTVP(leasings, request.FechaCierre);

                            // 1 query para actualizar todos los saldos actuales
                            await _creditoRepository.ActualizarCausacionDiariaLeasingSaldoActualEnBatchTVP(leasings, request.FechaCierre);
                        }

                        if (hipotecarios.Count > 0)
                        {
                            await _creditoRepository.InsertarCausacionDiariaHipotecarioHistoricoEnBatchTVP(
                                hipotecarios, request.FechaCierre);
                            await _creditoRepository.ActualizarCausacionDiariaHipotecarioSaldoActualEnBatchTVP(
                                hipotecarios, request.FechaCierre);
                        }
                        var tareasMovimientos = GenerarMovimientosContablesSeven(request, causacionesResult);

                        await Task.WhenAll(tareasMovimientos);
                    }
                    response = causacionesResult;
                }
            }
            catch (InvalidOperationException ex)
            {
                return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = null, IsSuccess = false };
            }
            catch (Exception ex)
            {

                return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = null, IsSuccess = false };
            }

            return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = response, IsSuccess = true };
        }

        // ===============================================
        // MÉTODO AUXILIAR: Ejecutar causación
        // ===============================================
        private async Task<List<CausacionDiariaInteresesCorrientesDto>> EjecutarCausacionDiariaInteresesCorrientes(
            RequestCausacionDiariaInteresesCorrientesDto request)
        {

            try
            {
                var requestCausacionDiaria = _mapper.Map<RequestCausacionDiariaInteresesCorrientes>(request);
                var datosRaw = await this._creditoRepository.CausacionDiariaInteresCorriente(requestCausacionDiaria);
                var response = _mapper.Map<List<CausacionDiariaInteresesCorrientesDto>>(datosRaw);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // ===============================================
        // MÉTODO AUXILIAR: Generar movimientos Seven
        // ===============================================
        private async Task GenerarMovimientosContablesSeven(RequestCausacionDiariaInteresesCorrientesDto requets,
            List<CausacionDiariaInteresesCorrientesDto> causacion)
        {
            await this.CierreDiarioCausacionDiariaInteresesCorrientesSeven(requets, causacion);

            await Task.CompletedTask;
        }



        //public async Task<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>> CausacionDiariaInteresesCorrientes(RequestCausacionDiariaInteresesCorrientesDto request)
        //{
        //    var requestCausacionDiaria = this._mapper.Map<RequestCausacionDiariaInteresesCorrientes>(request);
        //    var response = this._mapper.Map<IEnumerable<CausacionDiariaInteresesCorrientesDto>>(await this._creditoRepository.
        //            CausacionDiariaInteresCorriente(requestCausacionDiaria)).ToList();

        //    var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
        //            Select(x => x.Identificacion).Distinct().ToList())).Data;

        //    var list = response.GroupJoin(listAfiliados, r => r.Identificacion, la => la.Documento, (causac, doc) =>
        //    {
        //        causac.Nombres = doc.FirstOrDefault()?.NombreCompleto ?? "";
        //        return causac;
        //    }).ToList();

        //    //TODO: validar por que el sp despues que hace la causacion y se consulta despues con actualiza en false no trae datos, por eso se envia desde aca
        //    await this.CierreDiarioCausacionDiariaInteresesCorrientesSeven(request, list);

        //    return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = list };
        //}
        public async Task<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>> RecuperacionCausacionDiariaInteresesCorrientes(RequestCausacionDiariaInteresesCorrientesDto request)
        {
            var requestCausacionDiaria = this._mapper.Map<RequestCausacionDiariaInteresesCorrientes>(request);
            var response = this._mapper.Map<IEnumerable<CausacionDiariaInteresesCorrientesDto>>(await this._creditoRepository.
                    RecuperacionCausacionDiariaInteresCorriente(requestCausacionDiaria)).ToList();

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
                    Select(x => x.Identificacion).Distinct().ToList())).Data;

            var list = response.GroupJoin(listAfiliados, r => r.Identificacion, la => la.Documento, (causac, doc) =>
            {
                causac.Nombres = doc.FirstOrDefault()?.NombreCompleto ?? "";
                return causac;
            }).ToList();

            //TODO: validar por que el sp despues que hace la causacion y se consulta despues con actualiza en false no trae datos, por eso se envia desde aca
            await this.CierreDiarioCausacionDiariaInteresesCorrientesSeven(request, list);

            return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = list };
        }

        public async Task<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>> RecuperacionCausacionDiariaInteresesMoratorios(RequestCausacionDiariaInteresesCorrientesDto request)
        {
            var requestCausacionDiaria = this._mapper.Map<RequestCausacionDiariaInteresesCorrientes>(request);
            var response = this._mapper.Map<IEnumerable<CausacionDiariaInteresesCorrientesDto>>(await this._creditoRepository.
                    RecuperacionCausacionDiariaInteresesMoratorio(requestCausacionDiaria)).ToList();

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(response.
                    Select(x => x.Identificacion).Distinct().ToList())).Data;

            var list = response.GroupJoin(listAfiliados, r => r.Identificacion, la => la.Documento, (causac, doc) =>
            {
                causac.Nombres = doc.FirstOrDefault()?.NombreCompleto ?? "";
                return causac;
            }).ToList();

            //TODO: validar por que el sp despues que hace la causacion y se consulta despues con actualiza en false no trae datos, por eso se envia desde aca
            //await this.CierreDiarioCarteraInteresMoraSeven(request, list);
            await this.CierreDiarioCausacionDiariaInteresesMoratoriosSeven(request, list);


            return new Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>> { Data = list };
        }

        public async Task<Response<IEnumerable<TipoCierreCarteraDto>>> ObtenerTiposDeCierreCartera()
        {
            var response = this._mapper.Map<IEnumerable<TipoCierreCarteraDto>>(
                await this._creditoRepository.ObtenerTiposDeCierreCartera()
            ).ToList();

            return new Response<IEnumerable<TipoCierreCarteraDto>> { Data = response };
        }

        public async Task<Response<IEnumerable<CierresCausacionesRealizadasDto>>> ObtenerCierresCausacionesRealizadas(RequestCierresCausacionesRealizadasDto request)
        {
            var requestCierres = this._mapper.Map<RequestCierresCausacionesRealizadas>(request);
            var response = this._mapper.Map<IEnumerable<CierresCausacionesRealizadasDto>>(
                await this._creditoRepository.ObtenerCierresCausacionesRealizadas(requestCierres)
            ).ToList();

            return new Response<IEnumerable<CierresCausacionesRealizadasDto>> { Data = response };
        }

        public async Task<Response<SolicitudCreditoDto>> ObtenerSolicitudCreditoTipoNumeroIdentificacion(int tipoDocumento, string numeroDocumento)
        {
            return new Response<SolicitudCreditoDto>() { Data = _mapper.Map<SolicitudCreditoDto>(await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(tipoDocumento, numeroDocumento)) };

        }

        public async Task<Response<SolicitudCreditoDto>> ObtenerInformacionSolicitudCreditoCartera(int tipoDocumento, string numeroDocumento)
        {
            return new Response<SolicitudCreditoDto>() { Data = _mapper.Map<SolicitudCreditoDto>(await this._creditoRepository.ObtenerInformacionSolicitudCreditoCartera(tipoDocumento, numeroDocumento)) };

        }

        public async Task<Response<SocSolicitudInfobasicaDto>> ActualizaSolicCreditoBasica(SocSolicitudInfobasicaDto infoBasica)
        {

            SocSolicitudInfobasica request = _mapper.Map<SocSolicitudInfobasica>(infoBasica);
            SocSolicitudInfobasicaDto Respuesta = this._mapper.Map<SocSolicitudInfobasicaDto>(await this._creditoRepository.ActualizaSolicCreditoBasica(request));
            return new Response<SocSolicitudInfobasicaDto>() { Data = Respuesta };

        }

        public async Task<Response<SolicitudCreditoReferernciasDto>> CrearSolicCreditoRefererncias(SolicitudCreditoReferernciasDto data)
        {

            SolicCreditoFinancieros financiero = new SolicCreditoFinancieros();
            financiero.SOF_ID = data.activos.Id;
            financiero.SOC_ID = data.activos.SolicitudId;
            financiero.SOF_DIRECCION_VIVIENDA = data.activos.DireccionVivienda;
            financiero.SOF_MATRICULA_VIVIENDA = data.activos.MatriculaVivienda;
            financiero.SOF_VALOR_COMERCIAL_VIVIENDA = data.activos.ValorComercialVivienda;
            financiero.SOF_MARCA_VEHICULO = data.activos.MarcaVehiculo;
            financiero.SOF_PLACA_VEHICULO = data.activos.PlacaVehiculo;
            financiero.SOF_VALOR_COMERCIAL_VEHICULO = data.activos.ValorComercial;

            SolicCreditoFinancierosDto Respuesta = this._mapper.Map<SolicCreditoFinancierosDto>(await this._creditoRepository.ActualizaSolicCreditoFinancieros(financiero));
            SolicitudCreditoReferernciasDto creref = new SolicitudCreditoReferernciasDto();
            creref.activos = Respuesta;
            List<SolicCreditoReferernciasDto> listaref = new List<SolicCreditoReferernciasDto>();

            foreach (SolicCreditoReferernciasDto referencias in data.referencias)
            {
                SolicCreditoRefererncias refere = new SolicCreditoRefererncias();
                refere.SOR_ID = referencias.Id;
                refere.SOC_ID = referencias.SolicitudId;
                refere.SOR_TIPO_REFERENCIA = referencias.TipoReferencia;
                refere.SOR_PRIMER_APELLIDO = referencias.PrimerApellido;
                refere.SOR_SEGUNDO_APELLIDO = referencias.SegundoApellido;
                refere.SOR_NOMBRES = referencias.Nombres;
                refere.SOR_PARENTESCO = referencias.Parentesco;
                refere.SOR_RELACION = referencias.Relacion;
                refere.SOR_DIRECCION = referencias.Direccion;
                refere.DPD_ID = referencias.DepartamentoId;
                refere.DPC_ID = referencias.Ciudadid;
                refere.SOR_TELEFONO = referencias.Telefono;
                refere.SOR_CELULAR = referencias.Celular;
                refere.SOR_REFERENCIA = referencias.referencia;
                SolicCreditoReferernciasDto datos = this._mapper.Map<SolicCreditoReferernciasDto>(await this._creditoRepository.CrearSolicCreditoRefererncias(refere));
                listaref.Add(datos);
            }

            creref.referencias = listaref;
            return new Response<SolicitudCreditoReferernciasDto> { Data = creref };

        }

        public async Task<Response<IEnumerable<SolicCreditoReferernciasDto>>> ObtenerSolicCreditoReferencias(Guid ID_SOC)
        {

            IEnumerable<SolicCreditoRefererncias> optener = await this._creditoRepository.ObtenerSolicCreditoReferencias(ID_SOC);
            return new Response<IEnumerable<SolicCreditoReferernciasDto>>() { Data = _mapper.Map<IEnumerable<SolicCreditoReferernciasDto>>(optener) };

        }

        public async Task<Response<SolicitudCreditoDto>> ActualizaSolicCreditoAutorizacionDatos(SolicitudCreditoDto solic)
        {
            SolicitudCredito request = _mapper.Map<SolicitudCredito>(solic);
            SolicitudCreditoDto Respuesta = this._mapper.Map<SolicitudCreditoDto>(await this._creditoRepository.ActualizaSolicCreditoAutorizacionDatos(request));
            return new Response<SolicitudCreditoDto>() { Data = Respuesta };
        }

        public async Task<Response<EstadoSolicCreditoDto>> ObtenerEstadoSolicCredito(string documento)
        {

            EstadoSolicCreditoDto Respuesta = this._mapper.Map<EstadoSolicCreditoDto>(await this._creditoRepository.ObtenerEstadoSolicCredito(documento));
            return new Response<EstadoSolicCreditoDto>() { Data = Respuesta };
        }

        public async Task LimpiarSimulacionCliente(Guid simulacionId)
        {
            await this._creditoRepository.LimpiarSimulacionCliente(simulacionId);
        }

        public async Task<Response<ConsultaSolicitudCreditoDto>> CrearNoSolicitudCredito(ConsultaSolicitudCreditoDto solic)
        {
            ConsultaSolicitudCredito request = _mapper.Map<ConsultaSolicitudCredito>(solic);
            ConsultaSolicitudCreditoDto Respuesta = this._mapper.Map<ConsultaSolicitudCreditoDto>(await this._creditoRepository.CrearNoSolicitudCredito(request));
            return new Response<ConsultaSolicitudCreditoDto>() { Data = Respuesta };

        }

        public async Task<Response<RespuestaSolicitudObtenerTramiteDto>> ObtenerTramiteSolicitudCli(string identificacion)
        {
            return new Response<RespuestaSolicitudObtenerTramiteDto> { Data = this._mapper.Map<RespuestaSolicitudObtenerTramiteDto>(await this._creditoRepository.ObtenerTramiteSolicitudCli(identificacion)) };
        }

        public async Task<Response<IEnumerable<UsuarioDto>>> ObtenerPeritos()
        {
            return new Response<IEnumerable<UsuarioDto>> { Data = this._mapper.Map<IEnumerable<UsuarioDto>>(await this._creditoRepository.ObtenerPeritos()) };
        }

        public async Task<Response<TramiteSolicitudDto>> ActualizaTramitePerito(RequestTramiteSolicitudDto request)
        {
            request.TRS_MODIFICADO_POR = Guid.NewGuid();
            request.TRS_FECHA_MODIFICACION = DateTime.Now;
            RequestTramiteSolicitud requestdata = _mapper.Map<RequestTramiteSolicitud>(request);
            TramiteSolicitudDto Respuesta = this._mapper.Map<TramiteSolicitudDto>(await this._creditoRepository.ActualizaTramitePerito(requestdata));
            return new Response<TramiteSolicitudDto>() { Data = Respuesta };

        }

        public async Task<Response<ResponseActividadTramiteDto>> SubsanarActividadTramite(RequestActividadTramiteDto request)
        {
            RequestActividadTramite requestdata = _mapper.Map<RequestActividadTramite>(request);
            ResponseActividadTramiteDto Respuesta = this._mapper.Map<ResponseActividadTramiteDto>(await this._creditoRepository.SubsanarActividadTramite(requestdata));
            return new Response<ResponseActividadTramiteDto>() { Data = Respuesta };
        }

        public async Task<Response<IEnumerable<PreaprobacionesDto>>> ObtenerPreaprobaciones(RequestPreaprobacionesDto request)
        {

            IEnumerable<PreaprobacionesDto> preaprobaciones = _mapper.Map<IEnumerable<PreaprobacionesDto>>(await _creditoRepository.ObtenerPreaprobaciones(request.NumeroDocumento, request.Estado, request.FechaSimulacion, request.FechaSimulacionFin));

            List<PreaprobacionesDto> ListPreaprobacionesDto = new List<PreaprobacionesDto>();

            foreach (PreaprobacionesDto preapro in preaprobaciones)
            {
                Response<ClienteCedulaDto> clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(preapro.NumeroDcocumento);

                if (clienteDto.Data != null)
                {
                    preapro.PrimerApellido = clienteDto.Data.PrimerNombre;
                    preapro.SegundoApellido = clienteDto.Data.SegundoApellido;
                    preapro.PrimerNombre = clienteDto.Data.PrimerNombre;
                    preapro.SegundoNombre = clienteDto.Data.SegundoNombre;

                    ListPreaprobacionesDto.Add(preapro);
                }

            }

            return new Response<IEnumerable<PreaprobacionesDto>> { Data = ListPreaprobacionesDto };
        }

        public async Task<Response<ResponseDatosSimulacionParaEstudioDto>> ObtenerDatosSimulacionParaEstudio(string numeroDocumento)
        {

            ResponseDatosSimulacionParaEstudioDto Respuesta = this._mapper.Map<ResponseDatosSimulacionParaEstudioDto>(await this._creditoRepository.ObtenerDatosSimulacionParaEstudio(numeroDocumento));
            Response<ResponseDatosSimulacionParaEstudioDto> respuesta = new Response<ResponseDatosSimulacionParaEstudioDto>();

            if (Respuesta == null)
            {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existe simulación de crédito con el número de documento";
                return respuesta;
            }

            Response<ResponseDatosSimulacionParaEstudioDto> responseBUA = await this._iClientRequestProvider.ObtenerCuotasNumeroIdentificacion(numeroDocumento);


            if (responseBUA.IsSuccess & responseBUA.Data != null)
            {

                Respuesta.Cuotas = responseBUA.Data.Cuotas;
                Respuesta.TipoActorId = responseBUA.Data.TipoActorId;
                return new Response<ResponseDatosSimulacionParaEstudioDto> { Data = Respuesta };

            }
            else
            {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existe cliente con el número de documento";
                return respuesta;

            }
        }

        async Task<Response<IEnumerable<ParametrosSeguroUbicacionDto>>> ICreditoBusinessLogic.ObtenerParametrosSeguroUbicacion()
        {
            return new Response<IEnumerable<ParametrosSeguroUbicacionDto>> { Data = (IEnumerable<ParametrosSeguroUbicacionDto>)_mapper.Map<IEnumerable<ParametrosSeguroUbicacionDto>>(await this._creditoRepository.ObtenerParametrosSeguroUbicacion()) };
        }

        async Task<Response<IEnumerable<ParametroSeguroRegionDto>>> ICreditoBusinessLogic.ObtenerParametrosporSeguroRegion()
        {
            return new Response<IEnumerable<ParametroSeguroRegionDto>> { Data = (IEnumerable<ParametroSeguroRegionDto>)_mapper.Map<IEnumerable<ParametroSeguroRegionDto>>(await this._creditoRepository.ObtenerParametrosporSeguroRegion()) };
        }

        public async Task<Response<IEnumerable<ParametroSeguroRegionGuiDto>>> ActualizarSeguroPorRegion(ParametroSeguroRegionGuiDto request)
        {

            IEnumerable<ParametroSeguroRegionGuiDto> valor = _mapper.Map<IEnumerable<ParametroSeguroRegionGuiDto>>(await _creditoRepository.ActualizarSeguroPorRegion(request.id, (double)request.seguro));

            return new Response<IEnumerable<ParametroSeguroRegionGuiDto>> { Data = valor };
        }

        public async Task<Response<ResultadoIntegracionDto>> InsertarResultadoIntegracion(ResultadoIntegracionDto request)
        {
            return new Response<ResultadoIntegracionDto> { Data = this._mapper.Map<ResultadoIntegracionDto>(await this._creditoRepository.InsertarResultadoIntegracion(request)) };

        }

        async Task<Response<ProyeccionClienteDto>> ICreditoBusinessLogic.ObtenerProyeccionCliente(string request)
        {
            var current = DateTime.Now.Year;
            var data = await this._creditoRepository.ObtenerProyeccionCliente(request, current);
            data ??= await this._creditoRepository.ObtenerProyeccionCliente(request, current - 1);
            return new Response<ProyeccionClienteDto> { Data = _mapper.Map<ProyeccionClienteDto>(data) };
        }

        public async Task<Response<SmlvAnualDto>> ObtenerSalarioMinimoAnual(ObtenerSmlvAnualDto anio)
        {
            var data = await this._creditoRepository.ObtenerSalarioMinimoAnual(anio);
            return new Response<SmlvAnualDto> { Data = data };
        }

        async Task<Response<IEnumerable<ProyeccionClienteDto>>> ICreditoBusinessLogic.ObtenerSimCargo(string request)
        {
            return new Response<IEnumerable<ProyeccionClienteDto>> { Data = (IEnumerable<ProyeccionClienteDto>)_mapper.Map<IEnumerable<ProyeccionClienteDto>>(await this._creditoRepository.ObtenerSimCargo(request)) };
        }

        async Task<Response<MatriculaCreditoDto>> ICreditoBusinessLogic.CrearMatriculaCredito(MatriculaCreditoDto request)
        {
            return new Response<MatriculaCreditoDto> { Data = this._mapper.Map<MatriculaCreditoDto>(await this._creditoRepository.CrearMatriculaCredito(request)) };
        }

        async Task<Response<IEnumerable<MatriculaCreditoDto>>> ICreditoBusinessLogic.ObtenerMatriculasCredito()
        {
            return new Response<IEnumerable<MatriculaCreditoDto>> { Data = (IEnumerable<MatriculaCreditoDto>)_mapper.Map<IEnumerable<MatriculaCreditoDto>>(await this._creditoRepository.ObtenerMatriculasCredito()) };
        }

        public async Task<Response<MatriculaCreditoDto>> EliminarMatriculasCreditoSolicitud(Guid? solicitudId)
        {
            return new Response<MatriculaCreditoDto>() { Data = (await this._creditoRepository.EliminarMatriculasCreditoSolicitud(solicitudId)) };
        }

        async Task<Response<IEnumerable<CausalesRechazoDto>>> ICreditoBusinessLogic.ObtenerCausalesRechazo()
        {
            IEnumerable<CausalesRechazoDto> respuesta = _mapper.Map<IEnumerable<CausalesRechazoDto>>(await _creditoRepository.ObtenerCausalesRechazo());
            return new Response<IEnumerable<CausalesRechazoDto>> { Data = respuesta };
        }

        async Task<Response<IEnumerable<ActividadTramiteSolicitudDto>>> ICreditoBusinessLogic.ObtenerActividadTramite()
        {
            IEnumerable<ActividadTramiteSolicitudDto> respuesta = _mapper.Map<IEnumerable<ActividadTramiteSolicitudDto>>(await _creditoRepository.ObtenerActividadTramite());
            return new Response<IEnumerable<ActividadTramiteSolicitudDto>> { Data = respuesta };
        }


        async Task<Response<IEnumerable<EstadoTramitesDto>>> ICreditoBusinessLogic.ObtenerEstadosTramite()
        {
            IEnumerable<EstadoTramitesDto> respuesta = _mapper.Map<IEnumerable<EstadoTramitesDto>>(await _creditoRepository.ObtenerEstadosTramite());
            return new Response<IEnumerable<EstadoTramitesDto>> { Data = respuesta };
        }

        async Task<Response<IEnumerable<TramitesPendientesDto>>> ICreditoBusinessLogic.ObtenerTramitesPendientes()
        {
            IEnumerable<TramitesPendientesDto> respuesta = _mapper.Map<IEnumerable<TramitesPendientesDto>>(await _creditoRepository.ObtenerTramitesPendientes());

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(respuesta.
                Select(x => x.Identificacion).Distinct().ToList())).Data;

            List<TramitesPendientesDto> list = new();

            list = (from r in respuesta
                    join la in listAfiliados on r.Identificacion equals la.Documento
                    select new TramitesPendientesDto
                    {
                        DescripcionCredito = r.DescripcionCredito,
                        Identificacion = r.Identificacion,
                        Nombres = la.NombreCompleto,
                        NumeroSolicitud = r.NumeroSolicitud,
                        Plazo = r.Plazo,
                        TipoCredito = r.TipoCredito,
                        ValorCredito = r.ValorCredito,
                        SolicitudId = r.SolicitudId
                    }).ToList();

            return new Response<IEnumerable<TramitesPendientesDto>> { Data = list };
        }

        async Task<Response<IEnumerable<MatriculaCreditoDto>>> ICreditoBusinessLogic.ActualizarMatriculasCredito(MatriculaCreditoDto resultado)
        {
            return new Response<IEnumerable<MatriculaCreditoDto>> { Data = this._mapper.Map<IEnumerable<MatriculaCreditoDto>>(await this._creditoRepository.ActualizarMatriculasCredito(resultado)) };
        }

        public async Task<Response<MatriculaCreditoDto>> ObtenerMatriculasCreditoporid(Guid? solicitudId)
        {
            return new Response<MatriculaCreditoDto>() { Data = (await this._creditoRepository.EliminarMatriculasCreditoSolicitud(solicitudId)) };
        }

        public async Task<Response<IEnumerable<MatriculaCreditoDto>>> ObtenerMatriculasCreditoporSolicitud(Guid? solicitudId)
        {
            return new Response<IEnumerable<MatriculaCreditoDto>>() { Data = (await this._creditoRepository.ObtenerMatriculasCreditoporSolicitud(solicitudId)) };
        }
        /// <summary>
        /// Paso 1 crear tercero Seven - Integración Solicitud Crédito.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarGnTerceResponse>> InsertTerceroSeven(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<InsertarGnTerceResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            var terceroRequest = new InsertarGnTerceRequest
            {
                emp_codi = 294,
                tip_codi = cliente.IdTipoIdentificacion,
                ter_coda = cliente.Identificacion,
                ter_dive = 0,
                ter_nomb = $"{cliente.PrimerNombre} {cliente.SegundoNombre}",
                ter_apel = $"{cliente.PrimerApellido} {cliente.SegundoApellido}",
                ter_nocm = string.Empty,
                mod_codi = 1,
                pai_codi = cliente.IdPaisResidencia,
                dep_codi = cliente.IdDepartamentoResidencia,
                mun_codi = cliente.IdCiudadResidencia,
                ter_dire = cliente.Direccion,
                ter_ntel = cliente.Telefono,
                ter_mail = cliente.Correo,
                ter_nfax = string.Empty,
                arb_clte = "0",
                ter_audp = "N"
            };

            response.Data = await _wsTercero.InsertarGnTerceAsync(terceroRequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.InsertarGnTerceResult : "Error inesperado creado el tercero en Seven.";

            if (response.Data is not null)
            {
                FacturaSevenCredito facturaSeven = new FacturaSevenCredito();
                facturaSeven.FAC_TIPO_DOCUMENTO = solicitud.tipoIdentificacion;
                facturaSeven.FAC_NUMERO_DOCUMENTO = solicitud.NumeroDocumentoCliente;
                facturaSeven.FAC_NOMBRE_AFILIADO = cliente.PrimerNombre;
                facturaSeven.FAC_SEGUNDO_NOMBRE_AFILIADO = cliente.SegundoNombre;
                facturaSeven.FAC_APELLIDO_AFILIADO = cliente.PrimerApellido;
                facturaSeven.FAC_SEGUNDO_APELLIDO_AFILIADO = cliente.SegundoApellido;
                facturaSeven.FAC_FECHA_REGISTRO_TERCERO = DateTime.Now;
                facturaSeven.FLUJO = 1;
                facturaSeven.SOC_ID = solicitud.idSolicitudCredito;
                await _creditoRepository.InsertarDatosFacturaSevenCredito(facturaSeven);
            }

            return response;

        }
        /// <summary>
        /// Crea CDP en seven primer pago ("Cuentas Individuales")
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<CrearCDPResponse>> CrearCDPSevenPrimerPago(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<CrearCDPResponse> response = new();

            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud de crédito no fué encontrada";
                return response;
            }
            string tipoOperacion = "";
            var primerPagoCuentaIndividual = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(solicitud.idSolicitudCredito)).Where(x => x.FRC_DESCRIPCION == "Cuenta Individual").Sum(x => x.SID_VALOR_DESEMBOLSO);
            if (primerPagoCuentaIndividual > 0)
            {
                tipoOperacion = "Cuenta Individual";
                IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _creditoRepository.ObtenerOrdenDescuentoSaldosCredito());
                Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
                int IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
                Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
                double saldoCuentas = 0;

                decimal saldoDesembolsado = (datosSolicitud.SOC_DESEMBOLSO_APLICADO + primerPagoCuentaIndividual);
                foreach (var item in orden)
                {
                    var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                    if (cuenta != null)
                    {
                        saldoCuentas += cuenta.ConceptoSaldoCuenta;

                    }
                }

                if ((double)primerPagoCuentaIndividual > saldoCuentas)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El monto supera el saldo total disponible.";
                    return response;
                }
                if ((double)saldoDesembolsado > saldoCuentas)
                {
                    var valorDiferencia = ((double)saldoDesembolsado - saldoCuentas);
                    response.IsSuccess = false;
                    response.ReturnMessage = "El monto supera el saldo total disponible. Diferencia: " + valorDiferencia;
                    return response;
                }


            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre[] tSPgDmpres = ObtenerDeTalleCDPPrimerPago(cliente, datosSolicitud, solicitud.idSolicitudCredito).Result.vDetalle;

            if (tSPgDmpres == null)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.ReturnMessage = "El cliente no cuenta con los recursos para efectuar el primer pago";
                return response;
            }

            var request = new CrearCDPDto
            {
                Emp_codi = 294,
                Top_codi = 3015,
                Mpr_nume = 0,
                Mpr_fech = DateTime.Now,
                Mpr_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                $"{datosSolicitud.SOC_NUMERO_RADICADO} - {datosSolicitud.SOC_NUMERO_SOLICITUD}",
                Arb_csuc = "1001",
                Ter_coda = cliente.Identificacion,
                Mpr_tdos = "COMPROBANTE",
                Mpr_ndos = 0,
                Uni_codi = 1
            };

            var _data = new TSPgMpres();
            _data.Emp_codi = request.Emp_codi;
            _data.Top_codi = request.Top_codi;
            _data.Mpr_nume = request.Mpr_nume;
            _data.Mpr_fech = request.Mpr_fech;
            _data.Mpr_desc = request.Mpr_desc;
            _data.Arb_csuc = request.Arb_csuc;
            _data.Ter_coda = request.Ter_coda;
            _data.Mpr_tdos = request.Mpr_tdos;
            _data.Mpr_ndos = request.Mpr_ndos;
            _data.Uni_codi = request.Uni_codi;
            _data.vDetalle = tSPgDmpres;

            var cdprequest = new CrearCDPRequest(_data);

            response.Data = await _wsCDP.CrearCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.CrearCDPResult : "Error inesperado creando el CDP en Seven.";
            if (response.Data.CrearCDPResult.Contains("MPR_CONT"))
            {
                string responseTextMensaje;
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(response.Data.CrearCDPResult, out responseTextMensaje);
                if (valorRetorno == 0)
                {
                    int MPR_CONT = GetValueResponseXmlSeven(response.Data.CrearCDPResult, 0);
                    InsertarValoresCDPSeven valoresCDPSeven = new InsertarValoresCDPSeven();
                    valoresCDPSeven.SOC_ID = solicitud.idSolicitudCredito;
                    valoresCDPSeven.NUMERO_DOC = cliente.Identificacion;
                    valoresCDPSeven.NOMBRE_AFILIADO = cliente.PrimerNombre + " " + cliente.SegundoNombre + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                    valoresCDPSeven.SOC_DESEMBOLSO_APLICADO = datosSolicitud.SOC_DESEMBOLSO_APLICADO;
                    valoresCDPSeven.MPR_CONT = MPR_CONT;
                    valoresCDPSeven.VALOR_PRESTAMO = datosSolicitud.SMC_VALOR_PRESTAMO;
                    valoresCDPSeven.Arb_csuc = _data.Arb_csuc;
                    valoresCDPSeven.TCR_ID = datosSolicitud.TCR_ID;
                    valoresCDPSeven.Emp_codi = _data.Emp_codi;
                    valoresCDPSeven.Top_codi = _data.Top_codi;
                    valoresCDPSeven.DPD_ID = datosSolicitud.DPD_ID;
                    valoresCDPSeven.DPC_ID = datosSolicitud.DPC_ID;
                    valoresCDPSeven.DMP_VALO = _data.vDetalle[0].Dmp_valo;
                    valoresCDPSeven.ARB_CODC = _data.vDetalle[0].Arb_codc;
                    valoresCDPSeven.PRO_CODI = _data.vDetalle[0].Pro_codi;
                    valoresCDPSeven.FECHA_CREACION_CDP = DateTime.Now;
                    await this._creditoRepository.GuardarValorCDP(valoresCDPSeven);
                    await this._creditoRepository.ActualizaMPRCONTSolicitudCredito(solicitud.idSolicitudCredito, MPR_CONT, 1);
                    if (tipoOperacion == "Cuenta Individual")
                    {
                        decimal saldoDesembolsado = (datosSolicitud.SOC_DESEMBOLSO_APLICADO + primerPagoCuentaIndividual);
                        await this._creditoRepository.ActualizarEstadoDesembolso(solicitud.idSolicitudCredito, saldoDesembolsado);

                    }
                }
            }

            return response;

        }
        private async Task<TSPgMpres> ObtenerDeTalleCDPPrimerPago(ClienteDto cliente, ActualizacionSolicitudCreditoDesemFirma solicitud, Guid idSolicitud)
        {
            TSPgMpres detalle = new();
            string tipoOperacion = "";
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre>();
            var fuenteDeRecursos = await this._creditoRepository.ObtenerFuentesRecursosSolicitud(idSolicitud);
            var primerPagoCuentaIndividual = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(idSolicitud)).Where(x => x.FRC_DESCRIPCION == "Cuenta Individual").Sum(x => x.SID_VALOR_DESEMBOLSO);
            var primerPagoRecursosPropios = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(idSolicitud)).Where(x => x.FRC_DESCRIPCION == "Recursos Propios").Sum(x => x.SID_VALOR_DESEMBOLSO);
            decimal valorPrimerPago = 0;
            if (primerPagoCuentaIndividual > 0)
            {
                valorPrimerPago = primerPagoCuentaIndividual;
                tipoOperacion = "Cuenta Individual";
            }
            else
            {
                valorPrimerPago = primerPagoRecursosPropios;
                tipoOperacion = "Recursos Propios";
            }
            double valorRetiro = (double)valorPrimerPago;
            double saldoRetirado = 0;
            double saldoPendiente = (double)valorPrimerPago;
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _creditoRepository.ObtenerOrdenDescuentoSaldosCredito());
            if (tipoOperacion == "Cuenta Individual")
            {
                Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
                int IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
                int tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
                Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
                IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
                int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

                foreach (var item in orden)
                {
                    var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                    if (cuenta != null)
                    {
                        if (saldoPendiente > 0)
                        {
                            if (saldoPendiente <= cuenta.ConceptoSaldoCuenta)
                            {
                                saldoRetirado = saldoPendiente;
                                saldoPendiente = 0;
                            }
                            else
                            {
                                saldoRetirado = cuenta.ConceptoSaldoCuenta;
                                saldoPendiente -= saldoRetirado;
                            }

                            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre
                            {
                                Rub_codi = "0",
                                Pro_codi = item.concepto,
                                Arb_codc = solicitud.TCR_ID == 1 ? "601102" : "601103",
                                Arb_coda = "3011",
                                Arb_codp = "099",
                                Dmp_valo = saldoRetirado,
                                Dmp_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                                $"{solicitud.SOC_NUMERO_RADICADO} - {solicitud.SOC_NUMERO_SOLICITUD}"
                            };

                            items.Add(itemDetalle);
                        }
                    }
                }
            }
            else
            {
                foreach (var item in orden)
                {
                    if (saldoPendiente > 0)
                    {
                        saldoPendiente = 0;
                        var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre
                        {
                            Rub_codi = "0",
                            Pro_codi = item.concepto,
                            Arb_codc = solicitud.TCR_ID == 1 ? "601102" : "601103",
                            Arb_coda = "3011",
                            Arb_codp = "099",
                            Dmp_valo = (double)valorPrimerPago,
                            Dmp_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                                        $"{solicitud.SOC_NUMERO_RADICADO} - {solicitud.SOC_NUMERO_SOLICITUD}"
                        };
                        items.Add(itemDetalle);
                    }
                }

            }

            if (saldoPendiente == 0)
                detalle.vDetalle = items.ToArray();
            else
                detalle.vDetalle = null;

            return detalle;
        }

        /// <summary>
        /// creación de CDP Seven Segundo Pago ("Recursos Caja").
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<CrearCDPResponse>> CrearCDPSevenSegundoPago(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<CrearCDPResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud de crédito no fué encontrada";
                return response;
            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre[] tSPgDmpres = ObtenerDeTalleCDPSegundoPago(cliente, datosSolicitud).Result.vDetalle;

            var request = new CrearCDPDto
            {
                Emp_codi = 294,
                Top_codi = 3015,
                Mpr_nume = 0,
                Mpr_fech = DateTime.Now,
                Mpr_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                $"{datosSolicitud.SOC_NUMERO_RADICADO} - {datosSolicitud.SOC_NUMERO_SOLICITUD}",
                Arb_csuc = "1001",
                Ter_coda = cliente.Identificacion,
                Mpr_tdos = "COMPROBANTE",
                Mpr_ndos = 0,
                Uni_codi = 1
            };

            var _data = new TSPgMpres();
            _data.Emp_codi = request.Emp_codi;
            _data.Top_codi = request.Top_codi;
            _data.Mpr_nume = request.Mpr_nume;
            _data.Mpr_fech = request.Mpr_fech;
            _data.Mpr_desc = request.Mpr_desc;
            _data.Arb_csuc = request.Arb_csuc;
            _data.Ter_coda = request.Ter_coda;
            _data.Mpr_tdos = request.Mpr_tdos;
            _data.Mpr_ndos = request.Mpr_ndos;
            _data.Uni_codi = request.Uni_codi;
            _data.vDetalle = tSPgDmpres;

            var cdprequest = new CrearCDPRequest(_data);

            response.Data = await _wsCDP.CrearCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.CrearCDPResult : "Error inesperado creando el CDP en Seven.";

            if (response.Data.CrearCDPResult.Contains("MPR_CONT"))
            {
                int MPR_CONT = GetValueResponseXmlSeven(response.Data.CrearCDPResult, 0);
                await this._creditoRepository.ActualizaMPRCONTSolicitudCredito(solicitud.idSolicitudCredito, MPR_CONT, 2);
            }

            if (response.Data is not null)
            {
                FacturaSevenCredito facturaSeven = new FacturaSevenCredito();
                facturaSeven.FAC_CDP = GetValueResponseXmlSeven(response.Data.CrearCDPResult, 0);
                facturaSeven.FAC_TIPO_CREDITO = datosSolicitud.TCR_ID;
                facturaSeven.FAC_VALOR_DESEMBOLSO = (decimal)_data.vDetalle.Select(x => x.Dmp_valo).FirstOrDefault();
                facturaSeven.FAC_RADICADO = datosSolicitud.SOC_NUMERO_RADICADO;
                facturaSeven.FAC_AREA_NEGOCIO = _data.vDetalle.Select(x => x.Arb_coda).FirstOrDefault();
                facturaSeven.FLUJO = 3;
                facturaSeven.SOC_ID = datosSolicitud.SOC_ID;
                await _creditoRepository.InsertarDatosFacturaSevenCredito(facturaSeven);
            }

            return response;
        }
        private async Task<TSPgMpres> ObtenerDeTalleCDPSegundoPago(ClienteDto cliente, ActualizacionSolicitudCreditoDesemFirma solicitud)
        {
            TSPgMpres detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre>();
            var valorDesembolso = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(solicitud.SOC_ID)).Where(x => x.FRC_DESCRIPCION == "Recursos Caja");
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre
            {

                Pro_codi = solicitud.TCR_ID == 2 ? "40401" : "40301", //estan invertidos
                Arb_codc = solicitud.TCR_ID == 2 ? "601102" : "601103",
                Arb_coda = "3011",//codAreanegocio.ToString(),
                Arb_codp = "099",
                Dmp_valo = (double)valorDesembolso.Select(x => x.SID_VALOR_DESEMBOLSO).FirstOrDefault(),
                Dmp_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                $"{solicitud.SOC_NUMERO_RADICADO} - {solicitud.SOC_NUMERO_SOLICITUD}"
            };

            items.Add(itemDetalle);
            detalle.vDetalle = items.ToArray();

            return detalle;
        }

        /// <summary>
        /// Obtiene el número de MPR_CONT que se da al crear el CDP en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>
        private Int32 GetValueResponseXmlSeven(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            Int32 Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("MPR_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("PVD_CODD");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }
        public async Task<Response<SyncPoPvdorResponse>> CrearProveedorSeven(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<SyncPoPvdorResponse> response = new();
            var tercero = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente);
            consultarVendedorDto request = new consultarVendedorDto();
            //request.Sls_Id = tercero.SLS_ID;
            request.Soc_Id = tercero.SOC_ID;
            var Vendedor = await this._informacionVendedorRepository.ObtenerInformacionVendedor(request);
            var primerVendedor = Vendedor.FirstOrDefault();


            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un Tercero de pago a la solicitud.";
                return response;
            }

            if (Vendedor.Count() == 0)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            var banco = (await this._catalogsRepository.ObtenerEntidadBancaria()).Where(x => x.ENT_ID == primerVendedor.IVEN_BANCO).FirstOrDefault();
            var tipoDocumento = await this._creditoRepository.ObtenerTipoIdentificacionPorId(primerVendedor.IVEN_TIPO_DOC);
            var ProveeRequest = new SyncPoPvdorRequest()
            {
                emp_codi = 294,
                tip_codi = tipoDocumento.TID_ERP,
                pvd_coda = primerVendedor.IVEN_NRO_DOC,
                pvd_dive = 0,
                pvd_nomb = $"{primerVendedor.IVEN_NOMBRE}",
                pvd_apel = "",
                pvd_noco = primerVendedor.IVEN_NOMBRE,
                pai_codi = tercero.DPP_CODIGO,
                dep_codi = Convert.ToInt16(tercero.DPD_CODIGO),
                mun_codi = tercero.DPC_CODIGO,
                tpr_codi = 0,
                pvd_clad = tercero.TID_EMPRESARIAL == 1 ? "J" : "N",
                arb_csuc = "1001",
                cal_codi = 0,
                coc_codi = 1,
                cim_codi = 98,
                act_codi = 0,
                mon_codi = 1,
                dep_dire = tercero.SOP_DIRECCION_VENDEDOR,
                cup_nume = primerVendedor.IVEN_NRO_CUENTA.ToString(),
                cup_tipo = primerVendedor.IVEN_TIPO_CUENTA == 1 ? "A" : "C",
                ban_codi = banco.ENT_CODIGO_BANCO,
                sub_codi = 1,
                cup_idba = "0",
                cup_esta = "A",
                pvd_audp = "N",
                cup_clas = "S",
                pvr_riva = "C",
                pvr_auto = "N"
            };

            response.Data = await _wsProveedor.SyncPoPvdorAsync(ProveeRequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncPoPvdorResult : "Error inesperado creando el proveedor en Seven";

            if (response.Data.SyncPoPvdorResult.Contains("PVD_CODD"))
            {
                int codProvee = GetValueResponseXmlSeven(response.Data.SyncPoPvdorResult, 1);
                await this._creditoRepository.ActualizarCodProveedorSevenSolicitudCredito(solicitud.idSolicitudCredito, codProvee, 1);
            }

            if (response.Data is not null)
            {
                FacturaSevenCredito facturaSeven = new FacturaSevenCredito();
                facturaSeven.FAC_TIPO_DOCUMENTO_VENDEDOR = primerVendedor.IVEN_TIPO_DOC;
                facturaSeven.FAC_NUMERO_DOCUMENTO_VENDEDOR = primerVendedor.IVEN_NRO_DOC;
                facturaSeven.FAC_NOMBRE_COMPLETO_VENDEDOR = primerVendedor.IVEN_NOMBRE;
                facturaSeven.FAC_TIPO_CUENTA_VENDEDOR = Convert.ToInt32(primerVendedor.IVEN_TIPO_CUENTA);
                facturaSeven.FAC_NUMERO_CUENTA_VENDEDOR = primerVendedor.IVEN_NRO_CUENTA.ToString();
                facturaSeven.FAC_BANCO_VENDEDOR = Guid.Parse(primerVendedor.IVEN_BANCO.ToString());
                facturaSeven.FAC_FECHA_REGISTRO_VENDEDOR = DateTime.Now;
                facturaSeven.FLUJO = 2;
                facturaSeven.SOC_ID = solicitud.idSolicitudCredito;
                await _creditoRepository.InsertarDatosFacturaSevenCredito(facturaSeven);
            }

            return response;
        }
        /// <summary>
        /// Crea Cuenta por pagar en seven para primer pago ("Cuentas Individuales").
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSevenPrimerPago(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<InsertarFacturaPResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
            var tercero = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente);
            consultarVendedorDto request2 = new consultarVendedorDto();
            //request2.Sls_Id = tercero.SLS_ID;
            request2.Soc_Id = tercero.SOC_ID;
            var Vendedor = await this._informacionVendedorRepository.ObtenerInformacionVendedor(request2);
            var primerVendedor = Vendedor.FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un tercero de pago a la solicitud.";
                return response;
            }
            if (Vendedor.Count() == 0)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud de crédito no fué encontrada";
                return response;
            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact[] detalleFactuProve = ObtenerDeTalleFacturaPrimerPago(cliente, datosSolicitud, solicitud.idSolicitudCredito).Result.vDetalle;

            if (detalleFactuProve == null)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.ReturnMessage = "El cliente no cuenta con los recursos para efectuar el primer pago";
                return response;
            }

            var _dataFactuProv = new TSPoFactu();
            var numeroRadicado = tercero.SOC_NUMERO_RADICADO != "RADICADO NO DISPONIBLE" ? tercero.SOC_NUMERO_RADICADO : tercero.SLS_NUMERO_RADICADO;
            _dataFactuProv.Emp_codi = 294;
            _dataFactuProv.Top_codi = 4133;
            _dataFactuProv.Fac_nume = 0;
            _dataFactuProv.Fac_fech = DateTime.Now;
            _dataFactuProv.Arb_cods = "1001";
            _dataFactuProv.Fac_desc = $"{tercero.SOP_NUMERO_DOCUMENTO_VENDEDOR} - " +
                $"{tercero.SOP_NOMBRE_VENDEDOR} - {numeroRadicado} - {tercero.SOC_NUMERO_SOLICITUD}";
            _dataFactuProv.Pvd_coda = tercero.SOP_NUMERO_DOCUMENTO_VENDEDOR;
            _dataFactuProv.Dep_codd = primerVendedor.PVD_CODD ?? 1;
            _dataFactuProv.Fac_pref = tercero.SOC_NUMERO_SOLICITUD.ToString();
            _dataFactuProv.Fac_nfap = tercero.SOC_NUMERO_RADICADO != "RADICADO NO DISPONIBLE" ? tercero.SOC_NUMERO_RADICADO : tercero.SLS_NUMERO_RADICADO;
            _dataFactuProv.Mon_codi = 1;
            _dataFactuProv.Mpr_cont = tercero.MPR_CONT ?? 0;
            _dataFactuProv.vDetalle = detalleFactuProve;

            var body = new InsertarFacturaPRequestBody(_dataFactuProv);
            var facturarequest = new InsertarFacturaPRequest(body);

            response.Data = await _wsFactura.InsertarFacturaPAsync(facturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "Error inesperado al crear la factura en Seven.";
            InsertarValoresFacturaSeven valoresFacturaSeven = new InsertarValoresFacturaSeven();
            valoresFacturaSeven.SOC_ID = datosSolicitud.SOC_ID;
            valoresFacturaSeven.IDENTIFICACION = cliente.Identificacion;
            valoresFacturaSeven.PRIMER_NOMBRE = cliente.PrimerNombre;
            valoresFacturaSeven.SEGUNDO_NOMBRE = cliente.SegundoNombre;
            valoresFacturaSeven.PRIMER_APELLIDO = cliente.PrimerApellido;
            valoresFacturaSeven.SEGUNDO_APELLIDO = cliente.SegundoApellido;
            valoresFacturaSeven.TOP_CODI = _dataFactuProv.Top_codi;
            valoresFacturaSeven.Emp_codi = _dataFactuProv.Emp_codi;
            valoresFacturaSeven.MPR_CONT = _dataFactuProv.Mpr_cont;
            valoresFacturaSeven.TCR_ID = tercero.TCR_ID;
            valoresFacturaSeven.PVD_CODA = _dataFactuProv.Pvd_coda;
            valoresFacturaSeven.ARB_CODA = facturarequest.Body.pFactura.vDetalle[0].Arb_coda;
            valoresFacturaSeven.ARB_CODC = facturarequest.Body.pFactura.vDetalle[0].Arb_codc;
            valoresFacturaSeven.DFA_VALO = facturarequest.Body.pFactura.vDetalle[0].Dfa_valo;
            valoresFacturaSeven.PRO_CODI = facturarequest.Body.pFactura.vDetalle[0].Pro_codi;
            valoresFacturaSeven.DPD_ID = datosSolicitud.DPD_ID;
            valoresFacturaSeven.DPC_ID = datosSolicitud.DPC_ID;
            valoresFacturaSeven.FECHA_CREACION_FACTURA = _dataFactuProv.Fac_fech;
            if (response.Data != null)
            {
                int FAC_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 0);
                int FAC_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1);
                valoresFacturaSeven.FAC_CONT = FAC_CONT;
                valoresFacturaSeven.FAC_NUME = FAC_NUME;
                if (FAC_CONT > 0)
                {
                    await this._creditoRepository.GuardarValorFactura(valoresFacturaSeven);
                }
                RequestAcualizarFacContDto request = new RequestAcualizarFacContDto();
                request.idSolicitud = solicitud.idSolicitudCredito;
                request.fac_cont = FAC_CONT.ToString();
                request.fac_nume = FAC_NUME.ToString();
                request.tipo_pago = 1;
                await this._creditoRepository.ActualizaFACCONTSolicitudCredito(request);
            }
            return response;
        }
        private async Task<TSPoFactu> ObtenerDeTalleFacturaPrimerPago(ClienteDto cliente, ActualizacionSolicitudCreditoDesemFirma solicitud, Guid idSolicitudCredito)
        {
            TSPoFactu detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact>();
            string tipoOperacion = "";
            var primerPagoCuentaIndividual = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(idSolicitudCredito)).Where(x => x.FRC_DESCRIPCION == "Cuenta Individual").Sum(x => x.SID_VALOR_DESEMBOLSO);
            var primerPagoRecursosPropios = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(idSolicitudCredito)).Where(x => x.FRC_DESCRIPCION == "Recursos Propios").Sum(x => x.SID_VALOR_DESEMBOLSO);
            decimal valorPrimerPago = 0;
            if (primerPagoCuentaIndividual > 0)
            {
                valorPrimerPago = primerPagoCuentaIndividual;
                tipoOperacion = "Cuenta Individual";
            }
            else
            {
                valorPrimerPago = primerPagoRecursosPropios;
                tipoOperacion = "Recursos Propios";
            }

            double valorRetiro = (double)valorPrimerPago;
            double saldoRetirado = 0;
            double saldoPendiente = (double)valorPrimerPago;
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _creditoRepository.ObtenerOrdenDescuentoSaldosCredito());
            if (tipoOperacion == "Cuenta Individual")
            {
                Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
                int IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
                int tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
                Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
                IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
                int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

                foreach (var item in orden)
                {
                    var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                    if (cuenta != null)
                    {
                        if (saldoPendiente > 0)
                        {
                            if (saldoPendiente <= cuenta.ConceptoSaldoCuenta)
                            {
                                saldoRetirado = saldoPendiente;
                                saldoPendiente = 0;
                            }
                            else
                            {
                                saldoRetirado = cuenta.ConceptoSaldoCuenta;
                                saldoPendiente -= saldoRetirado;
                            }

                            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact
                            {
                                Pro_codi = item.concepto,
                                Dfa_dest = "100",
                                Dfa_valo = saldoRetirado,
                                Arb_codc = solicitud.TCR_ID == 1 ? "601102" : "601103",
                                Arb_coda = codAreanegocio.ToString(),
                                Arb_codp = "099"
                            };

                            items.Add(itemDetalle);
                        }
                    }
                }
            }
            else
            {
                foreach (var item in orden)
                {
                    if (saldoPendiente > 0)
                    {
                        saldoPendiente = 0;
                        var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact
                        {
                            Pro_codi = item.concepto,
                            Dfa_dest = "100",
                            Dfa_valo = (double)valorPrimerPago,
                            Arb_codc = solicitud.TCR_ID == 1 ? "601102" : "601103",
                            Arb_coda = "3011",
                            Arb_codp = "099"
                        };

                        items.Add(itemDetalle);
                    }
                }

            }

            if (saldoPendiente == 0)
                detalle.vDetalle = items.ToArray();
            else
                detalle.vDetalle = null;

            return detalle;
        }
        /// <summary>
        /// Obtiene el número de FAC_CONT que se da al crear Factura  en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>CDP
        private int GetValueResponseXmlSevenFactura(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("FAC_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("FAC_NUME");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                case 2:
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }
        /// <summary>
        /// Obtiene el número de FAC_CONT que se da al crear Factura  en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>CDP
        private int GetValueResponseXmlSevenAprobacion(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("MCO_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("MCO_NUME");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                case 2:
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }

        /// <summary>
        /// Crea cuenta por pagar en seven para segundo pago ("Recursos Caja")
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSevenSegundoPago(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<InsertarFacturaPResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.tipoIdentificacion, solicitud.NumeroDocumentoCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
            var tercero = await this._creditoRepository.ObtenerSolicitudCreditoTipoNumeroIdentificacion(1, solicitud.NumeroDocumentoCliente);
            consultarVendedorDto request2 = new consultarVendedorDto();
            //request2.Sls_Id = tercero.SLS_ID;
            request2.Soc_Id = tercero.SOC_ID;
            var beneficiario = await this._informacionVendedorRepository.ObtenerInformacionVendedor(request2);
            var primerBeneficiario = beneficiario.FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un tercero de pago a la solicitud.";
                return response;
            }
            if (beneficiario == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud de crédito no fué encontrada";
                return response;
            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact[] detalleFactuProve = ObtenerDeTalleFacturaSegundoPago(cliente, datosSolicitud).Result.vDetalle;

            if (detalleFactuProve == null)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.ReturnMessage = "El cliente no cuenta con los recursos para efectuar el primer pago";
                return response;
            }

            var _dataFactuProv = new TSPoFactu();

            _dataFactuProv.Emp_codi = 294;
            _dataFactuProv.Top_codi = datosSolicitud.TCR_ID == 2 ? 4137 : 4133;
            _dataFactuProv.Fac_nume = tercero.FAC_NUME == null ? 0 : int.Parse(tercero.FAC_NUME);//0;numero de documento que devuelve la factura del primer pago
            _dataFactuProv.Fac_fech = DateTime.Now;
            _dataFactuProv.Arb_cods = "1001";
            _dataFactuProv.Fac_desc = $"{tercero.SOP_NUMERO_DOCUMENTO_VENDEDOR} - " +
                $"{tercero.SOP_NOMBRE_VENDEDOR} - {tercero.SOC_NUMERO_RADICADO} - {tercero.SOC_NUMERO_SOLICITUD}";
            _dataFactuProv.Pvd_coda = tercero.SOP_NUMERO_DOCUMENTO_VENDEDOR;
            _dataFactuProv.Dep_codd = primerBeneficiario.PVD_CODD ?? 0;
            _dataFactuProv.Fac_pref = tercero.SOC_NUMERO_SOLICITUD.ToString();
            _dataFactuProv.Fac_nfap = tercero.FAC_CONT == null ? "0" : tercero.FAC_CONT;//tercero.SOC_NUMERO_RADICADO;//numero de factura q devuelve la factura del primer pago
            _dataFactuProv.Mon_codi = 1;
            _dataFactuProv.Fac_nfap = tercero.SOC_NUMERO_RADICADO;
            _dataFactuProv.Mpr_cont = tercero.MPR_CONT_SP ?? 0;
            _dataFactuProv.vDetalle = detalleFactuProve;

            var body = new InsertarFacturaPRequestBody(_dataFactuProv);
            var facturarequest = new InsertarFacturaPRequest(body);

            response.Data = await _wsFactura.InsertarFacturaPAsync(facturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "Error inesperado al crear la factura en Seven.";
            if (response.Data != null)
            {
                int FAC_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 0);
                int FAC_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1);

                RequestAcualizarFacContDto request = new RequestAcualizarFacContDto();
                request.idSolicitud = solicitud.idSolicitudCredito;
                request.fac_cont = FAC_CONT.ToString();
                request.fac_nume = FAC_NUME.ToString();
                request.tipo_pago = 2;
                await this._creditoRepository.ActualizaFACCONTSolicitudCredito(request);
            }

            if (response.Data is not null)
            {
                FacturaSevenCredito facturaSeven = new FacturaSevenCredito();
                facturaSeven.FAC_NUMERO_FACTURA = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1).ToString();
                facturaSeven.FLUJO = 4;
                facturaSeven.SOC_ID = datosSolicitud.SOC_ID;
                await _creditoRepository.InsertarDatosFacturaSevenCredito(facturaSeven);
            }

            return response;
        }
        private async Task<TSPoFactu> ObtenerDeTalleFacturaSegundoPago(ClienteDto cliente, ActualizacionSolicitudCreditoDesemFirma solicitud)
        {
            TSPoFactu detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact>();
            var valorDesembolso = (await this._creditoRepository.ObtenerFuentesRecursosSolicitud(solicitud.SOC_ID)).Where(x => x.FRC_DESCRIPCION == "Recursos Caja").Sum(x => x.SID_VALOR_DESEMBOLSO);
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact
            {
                Pro_codi = solicitud.TCR_ID == 2 ? "40401" : "40301",
                Dfa_dest = "200",
                Dfa_valo = (double)valorDesembolso,
                Arb_codc = solicitud.TCR_ID == 2 ? "601102" : "601103",
                Arb_coda = "3011",
                Arb_codp = "099"
            };

            items.Add(itemDetalle);
            detalle.vDetalle = items.ToArray();

            return detalle;
        }
        public async Task<Response<InsertarMovContableResponse>> CrearMovContableCierreDiarioSeven(CierreDiarioDto request, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            string mensaje = string.Empty;
            var aplicacionPagos = await this._carteraRepository.ObtenerAplicacionPagoByFechaPago(request);
            bool generoTableMovContable = false;

            foreach (var item in aplicacionPagos)
            {
                var equivalenciaCuentas = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByTipoCredito(item.TCR_ID);
                generoTableMovContable = await crearRegistroMovContable(item, equivalenciaCuentas);
            }

            if (generoTableMovContable)
            {
                var creditosLeasing = aplicacionPagos.Where(x => x.TCR_ID == 1);
                var creditosHipotecarios = aplicacionPagos.Where(x => x.TCR_ID == 2);
                var dataMovContaLeasing = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                var dataMovContaHipotecario = new TSCnMcont();
                List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
                int cabecera = 0;

                foreach (var item in creditosLeasing)
                {
                    if (cabecera == 0)
                    {
                        dataMovContaLeasing.Emp_codi = 294;
                        dataMovContaLeasing.Top_codi = 9065;//leasing cod. tipo operación 9065
                        dataMovContaLeasing.Mco_nume = 0;
                        dataMovContaLeasing.Mco_fech = DateTime.Now;
                        dataMovContaLeasing.Mod_codi = 7;
                        dataMovContaLeasing.Arb_csuc = "1001";
                        dataMovContaLeasing.Mco_desc = "Aplicación Pagos Cierre Diario Créditos Leasing.";
                    }
                    var movimientoContable = await this._carteraRepository.ObtenerMovContableCierreDiarioByNumCredito(((int)item.CRE_NRO_CREDITO));
                    var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION, item.CLI_IDENTIFICACION)).Data;
                    var items = ObtenerDetalleMovContableCliereDiario(movimientoContable, cliente, item.TCR_ID).Result;
                    detalleMovContaLeasing.AddRange(items);

                    cabecera += 1;
                }

                if (detalleMovContaLeasing.Count > 0)
                {
                    dataMovContaLeasing.vDetalle = detalleMovContaLeasing.ToArray();

                    var bodyLeasing = new InsertarMovContableRequestBody(dataMovContaLeasing);
                    movContableRequest = new InsertarMovContableRequest(bodyLeasing);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    mensaje = "Resultado Mov. Contable Seven Créditos leasing: ";
                    response.ReturnMessage = response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos de Leasing";
                }
                else
                    response.ReturnMessage = "No hay créditos leasing para crear un mov. contable en seven en el rango de fechas seleccionado.";

                cabecera = 0;

                foreach (var item in creditosHipotecarios)
                {
                    if (cabecera == 0)
                    {
                        dataMovContaHipotecario.Emp_codi = 294;
                        dataMovContaHipotecario.Top_codi = 9003;//hipotecario cod. tipo operación 9003
                        dataMovContaHipotecario.Mco_nume = 0;
                        dataMovContaHipotecario.Mco_fech = DateTime.Now;
                        dataMovContaHipotecario.Mod_codi = 7;
                        dataMovContaHipotecario.Arb_csuc = "1001";
                        dataMovContaHipotecario.Mco_desc = "Aplicación Pagos Cierre Diario Créditos Hipotecarios";
                    }
                    var movimientoContable = await this._carteraRepository.ObtenerMovContableCierreDiarioByNumCredito(((int)item.CRE_NRO_CREDITO));
                    var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION, item.CLI_IDENTIFICACION)).Data;
                    var items = ObtenerDetalleMovContableCliereDiario(movimientoContable, cliente, item.TCR_ID).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }

                if (detalleMovContaHipotecario.Count > 0)
                {
                    dataMovContaHipotecario.vDetalle = detalleMovContaHipotecario.ToArray();

                    var bodyHipotecario = new InsertarMovContableRequestBody(dataMovContaHipotecario);
                    movContableRequest = new InsertarMovContableRequest(bodyHipotecario);

                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    mensaje = "\n Resultado Mov. Contable Seven Créditos Hipotecario: ";
                    response.ReturnMessage += response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven, " +
                        "para créditos Hipotecarios";
                }
                else
                    response.ReturnMessage = "No hay créditos Hípotecarios para crear un mov. contable en seven en el rango de fechas seleccionado";


            }

            return response;
        }
        private async Task<bool> crearRegistroMovContable(AplicacionPago infoPagoAplica, IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables)
        {
            bool result = false;

            foreach (var item in cuentasContables)
            {
                MovimientoContable movContable = new MovimientoContable();
                movContable.CRE_MOV_ID = Guid.NewGuid();
                movContable.FECHA_MOVIMIENTO = infoPagoAplica.CPG_FECHA_PAGO;
                movContable.CUENTA_CONTABLE_SEVEN = item.CUENTA_CONTABLE_SEVEN;
                movContable.NUM_CREDITO = ((int)infoPagoAplica.CRE_NRO_CREDITO);
                movContable.TIPO_IDENTIFICACION = infoPagoAplica.TIPO_IDENTIFICACION;
                movContable.CLI_IDENTIFICACION = infoPagoAplica.CLI_IDENTIFICACION;
                movContable.CONCEPTO_APLICADO = item.REF_COLUMN_APLICA_PAGO;
                movContable.TIPO_MOVIMIENTO = item.TIPO_OPERACION;

                switch (item.REF_COLUMN_APLICA_PAGO)
                {
                    case "CPG_ABONO_CAPITAL":
                        movContable.VALOR_MOVIMIENTO = infoPagoAplica.CPG_ABONO_CAPITAL;
                        break;
                    case "CPG_ABONO_INTERES_CORRIENTE":
                        movContable.VALOR_MOVIMIENTO = infoPagoAplica.CPG_ABONO_INTERES_CORRIENTE;
                        break;
                    case "CPG_ABONO_SEGURO_HOGAR":
                        movContable.VALOR_MOVIMIENTO = infoPagoAplica.CPG_ABONO_SEGURO_HOGAR;
                        break;
                    case "CPG_ABONO_SEGURO_VIDA":
                        movContable.VALOR_MOVIMIENTO = infoPagoAplica.CPG_ABONO_SEGURO_VIDA;
                        break;
                    case "CPG_VALOR_PAGO":
                        movContable.VALOR_MOVIMIENTO = infoPagoAplica.CPG_VALOR_PAGO;
                        break;
                    default:
                        break;
                }

                await this._carteraRepository.CrearMovContableCierreDiario(movContable);
                result = true;

            }

            return result;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleMovContableCliereDiario(IEnumerable<MovimientoContableCierreDiario> movContable, ClienteDto cliente, int tipoCredito)
        {
            //TSCnMcont detalle = new();
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            foreach (var item in movContable)
            {

                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"Aplicación cuota cliente: {item.CLI_IDENTIFICACION}",
                    Dmc_cant = 0,
                    Dmc_acti = 0,
                    Dmc_refe = item.CONCEPTO_APLICADO,
                    Dmc_vadb = item.TIPO_MOVIMIENTO == "D" ? item.VALOR_MOVIMIENTO : 0,
                    Dmc_vacr = item.TIPO_MOVIMIENTO == "C" ? item.VALOR_MOVIMIENTO : 0,
                    Dmc_vaba = 0,
                    Ter_coda = item.CLI_IDENTIFICACION,
                    Arb_codc = tipoCredito == 1 ? "601102" : "601103",
                    Arb_coda = codAreanegocio.ToString(),
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };
                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }

            return items;
        }
        /// <summary>
        /// Obtiene el número de RETORNO y TXTERROR que nos devuelve Seven en cada proceso
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>
        private int GetValueResponseXmlSevenValidarTipoRetorno(string xmlResponse, out string mensaje)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            string ValorTexto = "";

            XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
            Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);

            XmlNodeList elemlistText = xmlString.GetElementsByTagName("TXTERROR");
            ValorTexto = Convert.ToString(elemlistText[0].InnerXml);

            mensaje = ValorTexto;
            return Valor;
        }



        public async Task<Response<PagosSevenDto>> DesEmbolsoCreditolSevenMomento1(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);
            Response<PagosSevenDto> result = new Response<PagosSevenDto> { Data = new PagosSevenDto() };
            var responseTextMensaje = "";

            Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroSeven(solicitud, cancellationToken);

            if (responseTerceroSeven.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = responseTerceroSeven.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var jsonResponseTercero = ConvertXmlToJson(responseTerceroSeven.Data.InsertarGnTerceResult);
                result.Data.insertarGnTerceResponse = jsonResponseTercero;
            }

            Response<CrearCDPResponse> crearCDPResponse = await CrearCDPSevenPrimerPago(solicitud, cancellationToken);

            if (crearCDPResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = crearCDPResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(crearCDPResponse.Data.CrearCDPResult, out responseTextMensaje);
                var jsonResponseCDP = ConvertXmlToJson(crearCDPResponse.Data.CrearCDPResult);
                result.Data.crearCDPResponse = jsonResponseCDP;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }

            }
            Response<SyncPoPvdorResponse> syncPoPvdorResponse = await CrearProveedorSeven(solicitud, cancellationToken);

            if (syncPoPvdorResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = syncPoPvdorResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(syncPoPvdorResponse.Data.SyncPoPvdorResult, out responseTextMensaje);
                var jsonResponseProveedor = ConvertXmlToJson(syncPoPvdorResponse.Data.SyncPoPvdorResult);
                result.Data.syncPoPvdorResponse = jsonResponseProveedor;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            Response<InsertarFacturaPResponse> insertarFacturaPResponse = await CrearFacturaProveSevenPrimerPago(solicitud, cancellationToken);

            if (insertarFacturaPResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = insertarFacturaPResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult, out responseTextMensaje);
                var jsonResponseFactura = ConvertXmlToJson(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult);
                result.Data.insertarFacturaPResponse = jsonResponseFactura;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            return result;

        }
        public async Task<Response<PagosSevenDto>> DesEmbolsoCreditolSevenMomento2(RequestCreditoSeven solicitud, CancellationToken cancellationToken)
        {
            Response<PagosSevenDto> result = new Response<PagosSevenDto> { Data = new PagosSevenDto() };
            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(solicitud.idSolicitudCredito, 1);
            var responseTextMensaje = "";

            Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroSeven(solicitud, cancellationToken);

            if (responseTerceroSeven.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = responseTerceroSeven.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var jsonResponseTercero = ConvertXmlToJson(responseTerceroSeven.Data.InsertarGnTerceResult);
                result.Data.insertarGnTerceResponse = jsonResponseTercero;
            }

            Response<CrearCDPResponse> crearCDPResponse = await CrearCDPSevenSegundoPago(solicitud, cancellationToken);

            if (crearCDPResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = crearCDPResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(crearCDPResponse.Data.CrearCDPResult, out responseTextMensaje);
                var jsonResponseCDP = ConvertXmlToJson(crearCDPResponse.Data.CrearCDPResult);
                result.Data.crearCDPResponse = jsonResponseCDP;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }
            Response<SyncPoPvdorResponse> syncPoPvdorResponse = await CrearProveedorSeven(solicitud, cancellationToken);

            if (syncPoPvdorResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = syncPoPvdorResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(syncPoPvdorResponse.Data.SyncPoPvdorResult, out responseTextMensaje);
                var jsonResponseProveedor = ConvertXmlToJson(syncPoPvdorResponse.Data.SyncPoPvdorResult);
                result.Data.syncPoPvdorResponse = jsonResponseProveedor;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            Response<InsertarFacturaPResponse> insertarFacturaPResponse = await CrearFacturaProveSevenSegundoPago(solicitud, cancellationToken);

            if (insertarFacturaPResponse.Data == null)
            {
                result.IsSuccess = false;
                result.ReturnMessage = insertarFacturaPResponse.ReturnMessage;
                result.Data = null;
                return result;
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult, out responseTextMensaje);
                var jsonResponseFactura = ConvertXmlToJson(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult);
                result.Data.insertarFacturaPResponse = jsonResponseFactura;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            return result;

        }

        public async Task<Response<InsertarMovContableResponse>> CreaMovContableAprobacionCredito(RequestDesembolsoSeven desembolso, CancellationToken cancellationToken)
        {
            SolicitudComiteDto solicitudCredito = new SolicitudComiteDto();
            ComprobanteCarteraUpdate comprobanteCarteraUpdate = new();
            solicitudCredito.idComiteCredito = desembolso.idComiteCredito;
            var solicitudes = await this.ObtenerComiteCreditoSolicitud(solicitudCredito);
            var requestList = solicitudes.Data.Select(solicitud => new PeticionSolicitudCreditoProductoDto { SOC_ID = (Guid)solicitud.idSolicitud }).ToList();

            Guid solicitudId = desembolso.idSolicitudCredito != Guid.Parse("00000000-0000-0000-0000-000000000000") ? desembolso.idSolicitudCredito : requestList[0].SOC_ID;

            Guid soc_id = desembolso.tipoOperacion == 2 ? requestList[0].SOC_ID : solicitudId;
            var datosSolicitud = await _creditoRepository.ObtenerSolicitudCredito(soc_id, 1);

            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

            var dataCabeceraMovConta = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            string etapaProceso = string.Empty;
            var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9077);
            var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9006);

            if (desembolso.tipoOperacion == 1)
            {
                RequestCreditoSeven reqestTer = new RequestCreditoSeven();
                reqestTer.idSolicitudCredito = desembolso.tipoOperacion == 2 ? requestList[0].SOC_ID : desembolso.idSolicitudCredito;
                reqestTer.NumeroDocumentoCliente = datosSolicitud.CLI_IDENTIFICACION;
                reqestTer.tipoIdentificacion = datosSolicitud.TID_ID;

                Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroSeven(reqestTer, cancellationToken);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseTerceroSeven.ReturnMessage);

                XmlNode errorNode = xmlDoc.SelectSingleNode("//GN_TERCE/TXTERROR");
                XmlNode retornoNode = xmlDoc.SelectSingleNode("//GN_TERCE/RETORNO");

                if (retornoNode.InnerText == "1")
                {
                    if (!errorNode.InnerText.Contains("ya se encuentra registrado"))
                    {
                        var insertarMovContableResponse = new Response<InsertarMovContableResponse>
                        {

                            IsSuccess = responseTerceroSeven.IsSuccess,
                            ReturnMessage = responseTerceroSeven.ReturnMessage

                        };
                        return insertarMovContableResponse;
                    }
                }

            }
            var listAprobacionLeasing = solicitudes.Data.Where(x => x.resultado == "APROBADA" && x.idTipoCredito == 1).ToList();
            var listAprobacionHipotecario = solicitudes.Data.Where(x => x.resultado == "APROBADA" && x.idTipoCredito == 2).ToList();
            if (listAprobacionLeasing.Count > 0)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9077;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = DateTime.Now;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = desembolso.tipoOperacion.Equals(1) ? $"1. Aprobación leasing {DateTime.Now.ToString("yyyy-MM-dd")}" : $"2. Desembolso leasing {DateTime.Now.ToString("yyyy-MM-dd")}";
                if (desembolso.tipoOperacion.Equals(1))
                {
                    var cuentasAprobacionLeasing = equivalenciaCuentasLeasing.Where(x => x.REF_COLUMN_APLICA_PAGO == "1. Aprobación leasing");
                    var items = ObtenerDetalleMovContableAprobacionCredito(cuentasAprobacionLeasing, datosSolicitud, listAprobacionLeasing).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                else
                {
                    var cuentasAprobacionLeasing = equivalenciaCuentasLeasing.Where(x => x.REF_COLUMN_APLICA_PAGO == "2. Desembolso leasing");
                    var items = ObtenerDetalleMovContableAprobacionCredito(cuentasAprobacionLeasing, datosSolicitud, listAprobacionLeasing).Result;
                    detalleMovContaLeasing.AddRange(items);
                }
                dataCabeceraMovConta.vDetalle = detalleMovContaLeasing.ToArray();
                var bodyLeasing = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                var cuentasLeasing = desembolso.tipoOperacion.Equals(1)
                    ? equivalenciaCuentasLeasing.Where(x => x.REF_COLUMN_APLICA_PAGO == "1. Aprobación leasing")
                    : equivalenciaCuentasLeasing.Where(x => x.REF_COLUMN_APLICA_PAGO == "2. Desembolso leasing");

                movContableRequest = new InsertarMovContableRequest(bodyLeasing);

                #region integración comprobantes cartera 
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovConta.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovConta.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovConta.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovConta.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovConta.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovConta.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = string.Empty;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                string proceso = desembolso.tipoOperacion == 1 ? "APN" : "CDE";


                decimal totalDmcVacr = Math.Round(dataCabeceraMovConta.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovConta.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteCreditoSeven(requestDetallePago, proceso, totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);
                #endregion

                foreach (var item in bodyLeasing.pContable.vDetalle)
                {
                    item.Cue_codi = cuentasLeasing.Where(x => x.CUENTA_CONTABLE_SEVEN == item.Cue_codi).FirstOrDefault().CUENTAS_NIF;
                }
                response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";
                int MCO_CONT = GetValueResponseXmlSevenAprobacion(response.Data.Body.InsertarMovContableResult, 0);
                int MCO_NUME = GetValueResponseXmlSevenAprobacion(response.Data.Body.InsertarMovContableResult, 1);

                comprobanteCarteraUpdate.RESPUESTA_SEVEN = response.Data?.Body?.InsertarMovContableResult;
                comprobanteCarteraUpdate.ES_EXITOSO = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 2) == 0;
                comprobanteCarteraUpdate.CC_ID = numeroComprobante.Data.Comprobante_Id;
                comprobanteCarteraUpdate.TIPO_OPERACION = dataCabeceraMovConta.Top_codi;
                comprobanteCarteraUpdate.CC_APROBADO_POR = desembolso.User;
                comprobanteCarteraUpdate.CC_COMPROBANTE_SEVEN_ID = GetValueResponseXmlSevenAprobacion(response.Data?.Body?.InsertarMovContableResult, 1);
                comprobanteCarteraUpdate.ESTADO = (comprobanteCarteraUpdate.ES_EXITOSO ?? false) ? "Aprobado" : "Rechazado";

                await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);


                InsertarAprobadosDesembolso aprobadosDesembolso = new InsertarAprobadosDesembolso();

                if (response.Data != null)
                {
                    if (MCO_CONT > 0 && MCO_NUME > 0)
                    {
                        foreach (var item in detalleMovContaLeasing)
                        {
                            aprobadosDesembolso.Soc_id = datosSolicitud.SOC_ID;
                            aprobadosDesembolso.Identificacion = datosSolicitud.CLI_IDENTIFICACION;
                            aprobadosDesembolso.Emp_codi = dataCabeceraMovConta.Emp_codi;
                            aprobadosDesembolso.Top_codi = dataCabeceraMovConta.Top_codi;
                            aprobadosDesembolso.Mco_nume = MCO_NUME;
                            aprobadosDesembolso.Mco_cont = MCO_CONT;
                            aprobadosDesembolso.Mco_fech = dataCabeceraMovConta.Mco_fech;
                            aprobadosDesembolso.Mod_codi = dataCabeceraMovConta.Mod_codi;
                            aprobadosDesembolso.Arb_csuc = dataCabeceraMovConta.Arb_csuc;
                            aprobadosDesembolso.Mco_desc = dataCabeceraMovConta.Mco_desc;
                            aprobadosDesembolso.Cue_codi = item.Cue_codi;
                            aprobadosDesembolso.Dmc_desc = item.Dmc_desc;
                            aprobadosDesembolso.Dmc_cant = item.Dmc_cant;
                            aprobadosDesembolso.Dmc_acti = item.Dmc_acti;
                            aprobadosDesembolso.Dmc_refe = item.Dmc_refe;
                            aprobadosDesembolso.Dmc_vadb = item.Dmc_vadb;
                            aprobadosDesembolso.Dmc_vacr = item.Dmc_vacr;
                            aprobadosDesembolso.Dmc_vaba = item.Dmc_vaba;
                            aprobadosDesembolso.Ter_coda = item.Ter_coda;
                            aprobadosDesembolso.Arb_codc = item.Arb_codc;
                            aprobadosDesembolso.Arb_coda = item.Arb_coda;
                            aprobadosDesembolso.Arb_codp = item.Arb_codp;
                            aprobadosDesembolso.Arb_cods = item.Arb_cods;
                            aprobadosDesembolso.Ter_codm = item.Ter_codm;
                            aprobadosDesembolso.Fecha_creacion = DateTime.Now;
                            await this._creditoRepository.GuardarAprobacionDesembolso(aprobadosDesembolso);

                        }
                    }
                }
            }
            if (listAprobacionHipotecario.Count > 0)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9006;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = DateTime.Now;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = desembolso.tipoOperacion.Equals(1) ? $"1. Aprobación crédito hipotecario {DateTime.Now.ToString("yyyy-MM-dd")}" : $"2. Desembolso crédito hipotecario {DateTime.Now.ToString("yyyy-MM-dd")}";

                if (desembolso.tipoOperacion.Equals(1))
                {
                    var cuentasAprobacionHipotecario = equivalenciaCuentasHipotecario.Where(x => x.REF_COLUMN_APLICA_PAGO == "1. Aprobación crédito hipotecario");
                    var items = ObtenerDetalleMovContableAprobacionCredito(cuentasAprobacionHipotecario, datosSolicitud, listAprobacionHipotecario).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }
                else
                {
                    var cuentasAprobacionHipotecario = equivalenciaCuentasHipotecario.Where(x => x.REF_COLUMN_APLICA_PAGO == "2. Desembolso crédito hipotecario");
                    var items = ObtenerDetalleMovContableAprobacionCredito(cuentasAprobacionHipotecario, datosSolicitud, listAprobacionHipotecario).Result;
                    detalleMovContaHipotecario.AddRange(items);
                }
                dataCabeceraMovConta.vDetalle = detalleMovContaHipotecario.ToArray();
                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                var cuentasHipotecario = desembolso.tipoOperacion.Equals(1)
                    ? equivalenciaCuentasHipotecario.Where(x => x.REF_COLUMN_APLICA_PAGO == "1. Aprobación crédito hipotecario")
                    : equivalenciaCuentasHipotecario.Where(x => x.REF_COLUMN_APLICA_PAGO == "2. Desembolso crédito hipotecario");

                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);


                #region integración comprobantes cartera 
                RequestDetallePago requestDetallePago = new RequestDetallePago();
                string jsonMovimientosContablesHipotecarios = System.Text.Json.JsonSerializer.Serialize(dataCabeceraMovConta.vDetalle);
                requestDetallePago.Cpg_id = new Guid();
                requestDetallePago.Emp_codi = dataCabeceraMovConta.Emp_codi;
                requestDetallePago.Top_codi = dataCabeceraMovConta.Top_codi;
                requestDetallePago.Mco_fech = dataCabeceraMovConta.Mco_fech;
                requestDetallePago.Mod_codi = dataCabeceraMovConta.Mod_codi;
                requestDetallePago.Mco_desc = dataCabeceraMovConta.Mco_desc;
                requestDetallePago.Mov_Contable_Seven = jsonMovimientosContablesHipotecarios;
                requestDetallePago.Respuesta_Seven = string.Empty;
                requestDetallePago.Fecha_Ejecucion = DateTime.Now;

                string proceso = desembolso.tipoOperacion == 1 ? "APN" : "CDE";


                decimal totalDmcVacr = Math.Round(dataCabeceraMovConta.vDetalle.Sum(x => x.Dmc_vacr), 2);
                decimal totalDmcCadb = Math.Round(dataCabeceraMovConta.vDetalle.Sum(x => x.Dmc_vadb), 2);
                var numeroComprobante = await GenerarComprobanteCreditoSeven(requestDetallePago, proceso, totalDmcVacr, totalDmcCadb);
                requestDetallePago.Cc_Id = numeroComprobante.IsSuccess ? numeroComprobante.Data.Comprobante_Id : null;
                await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePago);
                await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePago);


                #endregion
                foreach (var item in bodyHipotecario.pContable.vDetalle)
                {
                    item.Cue_codi = cuentasHipotecario.Where(x => x.CUENTA_CONTABLE_SEVEN == item.Cue_codi).FirstOrDefault().CUENTAS_NIF;
                }
                response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "Error inesperado creando el SCnMcont en Seven.";
                int MCO_CONT = GetValueResponseXmlSevenAprobacion(response.Data.Body.InsertarMovContableResult, 0);
                int MCO_NUME = GetValueResponseXmlSevenAprobacion(response.Data.Body.InsertarMovContableResult, 1);




                comprobanteCarteraUpdate.RESPUESTA_SEVEN = response.Data?.Body?.InsertarMovContableResult;
                comprobanteCarteraUpdate.ES_EXITOSO = GetValueResponseXmlSevenFactura(response.Data?.Body?.InsertarMovContableResult, 2) == 0;
                comprobanteCarteraUpdate.CC_ID = numeroComprobante.Data.Comprobante_Id;
                comprobanteCarteraUpdate.TIPO_OPERACION = dataCabeceraMovConta.Top_codi;
                comprobanteCarteraUpdate.CC_APROBADO_POR = desembolso.User;
                comprobanteCarteraUpdate.CC_COMPROBANTE_SEVEN_ID = GetValueResponseXmlSevenAprobacion(response.Data?.Body?.InsertarMovContableResult, 1);
                comprobanteCarteraUpdate.ESTADO = (comprobanteCarteraUpdate.ES_EXITOSO ?? false) ? "Aprobado" : "Rechazado";

                await _carteraRepository.UpdateComprobanteContableCartera(comprobanteCarteraUpdate);

                InsertarAprobadosDesembolso aprobadosDesembolso = new InsertarAprobadosDesembolso();

                if (response.Data != null)
                {
                    if (MCO_CONT > 0 && MCO_NUME > 0)
                    {
                        foreach (var item in detalleMovContaHipotecario)
                        {
                            aprobadosDesembolso.Soc_id = datosSolicitud.SOC_ID;
                            aprobadosDesembolso.Identificacion = datosSolicitud.CLI_IDENTIFICACION;
                            aprobadosDesembolso.Emp_codi = dataCabeceraMovConta.Emp_codi;
                            aprobadosDesembolso.Top_codi = dataCabeceraMovConta.Top_codi;
                            aprobadosDesembolso.Mco_nume = MCO_NUME;
                            aprobadosDesembolso.Mco_cont = MCO_CONT;
                            aprobadosDesembolso.Mco_fech = dataCabeceraMovConta.Mco_fech;
                            aprobadosDesembolso.Mod_codi = dataCabeceraMovConta.Mod_codi;
                            aprobadosDesembolso.Arb_csuc = dataCabeceraMovConta.Arb_csuc;
                            aprobadosDesembolso.Mco_desc = dataCabeceraMovConta.Mco_desc;
                            aprobadosDesembolso.Cue_codi = item.Cue_codi;
                            aprobadosDesembolso.Dmc_desc = item.Dmc_desc;
                            aprobadosDesembolso.Dmc_cant = item.Dmc_cant;
                            aprobadosDesembolso.Dmc_acti = item.Dmc_acti;
                            aprobadosDesembolso.Dmc_refe = item.Dmc_refe;
                            aprobadosDesembolso.Dmc_vadb = item.Dmc_vadb;
                            aprobadosDesembolso.Dmc_vacr = item.Dmc_vacr;
                            aprobadosDesembolso.Dmc_vaba = item.Dmc_vaba;
                            aprobadosDesembolso.Ter_coda = item.Ter_coda;
                            aprobadosDesembolso.Arb_codc = item.Arb_codc;
                            aprobadosDesembolso.Arb_coda = item.Arb_coda;
                            aprobadosDesembolso.Arb_codp = item.Arb_codp;
                            aprobadosDesembolso.Arb_cods = item.Arb_cods;
                            aprobadosDesembolso.Ter_codm = item.Ter_codm;
                            aprobadosDesembolso.Fecha_creacion = DateTime.Now;
                            await this._creditoRepository.GuardarAprobacionDesembolso(aprobadosDesembolso);

                        }
                    }
                }
            }
            return response;
        }

        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleMovContableAprobacionCredito(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
            ActualizacionSolicitudCreditoDesemFirma desembolso, List<SolicitudComiteDto> listComite)
        {
            var fechaActual = DateTime.Now;
            var dmcRefe = $"{fechaActual:yyyyMMdd}{desembolso.SOC_NUMERO_SOLICITUD}";
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            foreach (var afiliado in listComite)
            {
                var client = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(afiliado.Tipo_Identificacion, afiliado.identificacion);
                var area = await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras();
                var areaCodigo = area.Data.Where(x => x.Id == client.Data.IdUnidadEjecutora).FirstOrDefault().AreaNegocio;
                foreach (var item in cuentasContables)
                {
                    var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                    {
                        Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                        Dmc_desc = $"cliente: {client.Data.PrimerNombre} {client.Data.SegundoNombre} {client.Data.PrimerApellido} {client.Data.SegundoApellido}",
                        Dmc_cant = 0,
                        Dmc_acti = 0,
                        Dmc_refe = dmcRefe,
                        Dmc_vadb = item.TIPO_OPERACION == "D" ? afiliado.valorCredito : 0,
                        Dmc_vacr = item.TIPO_OPERACION == "C" ? afiliado.valorCredito : 0,
                        Dmc_vaba = 0,
                        Ter_coda = afiliado.identificacion,
                        Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                        Arb_coda = areaCodigo.ToString(),
                        Arb_codp = "099",
                        Arb_cods = "1001",
                        Ter_codm = "0"
                    };
                    items.Add(itemDetalle);
                }
            }

            return items;
        }
        public async Task<Response<InsertarMovContableResponse>> CrearMovContableGarantiasCredito(RequestGarantiasSeven garantia, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

            var dataCabeceraMovConta = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            string etapaProceso = string.Empty;
            var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9099);
            var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9005);
            decimal totalDmcVacr = 0;
            decimal totalDmcCadb = 0;

            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovConta = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            //List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (garantia.tipoCredito == 1)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9099;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = garantia.fechaGarantia;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = "Garantia Crédito Leasing.";

                var items = ObtenerDetalleMovContableGarantiasCredito(equivalenciaCuentasLeasing, garantia).Result;

                detalleMovConta.AddRange(items);
                dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();
                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);


            }
            else if (garantia.tipoCredito == 2)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9005;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = garantia.fechaGarantia;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = "Garantia Crédito Hipotecario.";

                var items = ObtenerDetalleMovContableGarantiasCredito(equivalenciaCuentasHipotecario, garantia).Result;

                detalleMovConta.AddRange(items);
                dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();
                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);
            }

            RequestDetallePago requestDetallePagoComprobante = new RequestDetallePago();
            //requestDetallePagoComprobante.Cpg_id = request.cpgId;
            requestDetallePagoComprobante.Emp_codi = dataCabeceraMovConta.Emp_codi;
            requestDetallePagoComprobante.Top_codi = dataCabeceraMovConta.Top_codi;
            requestDetallePagoComprobante.Mco_fech = dataCabeceraMovConta.Mco_fech;
            requestDetallePagoComprobante.Mod_codi = dataCabeceraMovConta.Mod_codi;
            requestDetallePagoComprobante.Mco_desc = dataCabeceraMovConta.Mco_desc;
            requestDetallePagoComprobante.Fecha_Ejecucion = DateTime.Now;

            if (detalleMovConta.Count > 0)
            {
                string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovConta);
                requestDetallePagoComprobante.Mov_Contable_Seven = jsonMovimientosContables;
                requestDetallePagoComprobante.Respuesta_Seven = response.ReturnMessage;
            }


            var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoComprobante, "CGI", totalDmcVacr, totalDmcCadb);

            requestDetallePagoComprobante.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;
            await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoComprobante);
            await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoComprobante);

            //UpdatePlanPago updatePlanPago = new UpdatePlanPago();
            //int MCO_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarMovContableResult, 0);
            //int MCO_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarMovContableResult, 1);
            //updatePlanPago.numeroCredito = request.numeroCredito;
            //updatePlanPago.cpgId = request.cpgId;
            //updatePlanPago.mcoCont = 0;
            //updatePlanPago.mcoNume = 0;
            //var respuesta = await _carteraRepository.UpdatePlanPago(updatePlanPago);
            if (numeroComprobante.IsSuccess)
            {
                response.ReturnMessage += "Comprobante garantias Inmueble " + numeroComprobante.Data + " creado correctamente ";
                response.IsSuccess = true;
                response.Data = null;
            }

            return response;
        }
        public async Task<Response<int>> CrearMovContableProvisionIndividualCredito(RequestProvisionIndSeven provision, CancellationToken cancellationToken)
        {
            Response<int> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();

            var dataCabeceraMovConta = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaLeasing = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaHipotecario = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            string etapaProceso = string.Empty;
            var equivalenciaCuentasLeasing = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9072);
            var equivalenciaCuentasHipotecario = await this._carteraRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(9004);
            decimal totalDmcVacr = 0;
            decimal totalDmcCadb = 0;

            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovConta = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            //List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            if (provision.tipoCredito == 1)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9072;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = provision.fechaProvision;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = "Provisión Individual Crédito Leasing.";

                var items = ObtenerDetalleMovContableProvisionIndCredito(equivalenciaCuentasLeasing, provision).Result;

                detalleMovConta.AddRange(items);
                dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();
                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);


            }
            else if (provision.tipoCredito == 2)
            {
                dataCabeceraMovConta.Emp_codi = 294;
                dataCabeceraMovConta.Top_codi = 9004;
                dataCabeceraMovConta.Mco_nume = 0;
                dataCabeceraMovConta.Mco_fech = provision.fechaProvision;
                dataCabeceraMovConta.Mod_codi = 7;
                dataCabeceraMovConta.Arb_csuc = "1001";
                dataCabeceraMovConta.Mco_desc = "Provisión Individual Crédito Hipotecario.";

                var items = ObtenerDetalleMovContableProvisionIndCredito(equivalenciaCuentasHipotecario, provision).Result;

                detalleMovConta.AddRange(items);
                dataCabeceraMovConta.vDetalle = detalleMovConta.ToArray();
                var bodyHipotecario = new InsertarMovContableRequestBody(dataCabeceraMovConta);
                movContableRequest = new InsertarMovContableRequest(bodyHipotecario);
                totalDmcVacr = Math.Round(detalleMovConta.Sum(x => x.Dmc_vacr), 2);
                totalDmcCadb = Math.Round(detalleMovConta.Sum(x => x.Dmc_vadb), 2);
            }

            RequestDetallePago requestDetallePagoComprobante = new RequestDetallePago();
            //requestDetallePagoComprobante.Cpg_id = request.cpgId;
            requestDetallePagoComprobante.Emp_codi = dataCabeceraMovConta.Emp_codi;
            requestDetallePagoComprobante.Top_codi = dataCabeceraMovConta.Top_codi;
            requestDetallePagoComprobante.Mco_fech = dataCabeceraMovConta.Mco_fech;
            requestDetallePagoComprobante.Mod_codi = dataCabeceraMovConta.Mod_codi;
            requestDetallePagoComprobante.Mco_desc = dataCabeceraMovConta.Mco_desc;
            requestDetallePagoComprobante.Fecha_Ejecucion = DateTime.Now;

            if (detalleMovConta.Count > 0)
            {
                string jsonMovimientosContables = System.Text.Json.JsonSerializer.Serialize(detalleMovConta);
                requestDetallePagoComprobante.Mov_Contable_Seven = jsonMovimientosContables;
                requestDetallePagoComprobante.Respuesta_Seven = response.ReturnMessage;
            }


            var numeroComprobante = await GenerarComprobanteContabilidad(requestDetallePagoComprobante, "CPC", totalDmcVacr, totalDmcCadb);

            requestDetallePagoComprobante.Cc_Id = numeroComprobante.IsSuccess ? (int)numeroComprobante.Data : null;
            await _carteraRepository.InsertarCpgDetallesPagosSeven(requestDetallePagoComprobante);
            await _carteraRepository.InsertarComprobantePagosSeven(requestDetallePagoComprobante);

            if (numeroComprobante.IsSuccess)
            {
                response.ReturnMessage += "Comprobante garantias Inmueble " + numeroComprobante.Data + " creado correctamente ";
                response.IsSuccess = true;
                response.Data = (int)numeroComprobante.Data;   //  retornar el ID

            }

            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleMovContableGarantiasCredito(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
            RequestGarantiasSeven garantia)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();

            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
            {
                { "desembolso hipotecario", garantia.valorGarantia },
                { "Inmuebles Hipotecario", garantia.valorGarantiaAct }
            };

            // var NombreTercero = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(new List<string> { request.NumeroDocumentoCliente }));  //.Distinct().ToList())).Data;


            foreach (var item in cuentasContables)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"cliente: {garantia.numeroDocumentoCliente}",
                    Dmc_cant = 0,
                    Dmc_acti = 0,
                    Dmc_refe = garantia.numeroCredito,
                    Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0,
                    Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] : 0,
                    Dmc_vaba = 0,
                    Ter_coda = garantia.numeroDocumentoCliente,
                    Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                    Arb_coda = "3011",
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };
                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }
            return items;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDetalleMovContableProvisionIndCredito(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables,
             RequestProvisionIndSeven provision)
        {
            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var cuentasAfectar = cuentasContables;



            if (provision.tipoCredito == 1)
            {
                if (provision.codigo == "OPCION_COMPRA")
                {
                    cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Cierres" && x.CUENTA_CONTABLE_SEVEN != "41601102");
                }
                else if (provision.codigo == "LIQUIDACION_LEASING")
                {
                    cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "CierresIR");
                }

                else if (provision.codigo == "CANCELACION_TOTAL" || provision.codigo == "RESTITUCION_INMUEBLE" || provision.codigo == "ACELERACION")
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" && x.CUENTA_CONTABLE_SEVEN != "41601102") ||
                    x.REF_COLUMN_APLICA_PAGO == "CierresIR");
                }
                else if (provision.codigo == "REESTRUCTURACION")
                {
                    cuentasAfectar = cuentasContables.Where(x => (x.REF_COLUMN_APLICA_PAGO == "Cierres" && x.CUENTA_CONTABLE_SEVEN != "41601102") ||
                    x.REF_COLUMN_APLICA_PAGO == "CierresIR" || x.REF_COLUMN_APLICA_PAGO == "Categoría A intereses reducidos");
                }

            }
            else
            {
                cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual");

                //if (provision.valorPagoInteres == 0)
                //{
                //    cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual");

                //}
                //else
                //{
                //cuentasAfectar = cuentasContables.Where(x => x.REF_COLUMN_APLICA_PAGO == "Provision Individual"
                //|| x.REF_COLUMN_APLICA_PAGO == "Provision cuentas por cobrar");
                //}
            }
            Dictionary<string, decimal> mapeoPagos = new Dictionary<string, decimal>
            {
                { "Provision Individual", provision.valorPagoCapital },
                { "Provision cuentas por cobrar", provision.valorPagoInteres },
                { "Cierres", provision.valorPagoCapital },
                { "CierresIR", provision.valorPagoInteres }
            };
            if (provision.codigo == "REESTRUCTURACION")
            {
                var requestObtenerProductClient = new SolicitudProductoPorClienteDto
                {
                    Identificacion = provision.numeroDocumentoCliente,
                    TipoIdentificacion = 1
                };
                var dataProduct = await _carteraBusinessLogic.Value.ObtenerProductoCliente(requestObtenerProductClient);
                mapeoPagos.Add("Categoría A intereses reducidos", dataProduct.Data.Select(x => x.InteresesRemanenteFinal).FirstOrDefault());

            }


            // var NombreTercero = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(new List<string> { request.NumeroDocumentoCliente }));  //.Distinct().ToList())).Data;
            // DEBO CAMBIAR LA PROVISION Y EL Q NO MEAPARESCA EN GARANTIAS SI NO EN PRIVISIION

            foreach (var item in cuentasAfectar)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"cliente: {provision.numeroDocumentoCliente}",
                    Dmc_cant = 0,
                    Dmc_acti = 0,
                    Dmc_refe = provision.numeroCredito,
                    Dmc_vadb = item.TIPO_OPERACION == "D" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] * 0.01m : 0,
                    Dmc_vacr = item.TIPO_OPERACION == "C" ? mapeoPagos[item.REF_COLUMN_APLICA_PAGO] * 0.01m : 0,
                    Dmc_vaba = 0,
                    Ter_coda = provision.numeroDocumentoCliente,
                    Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                    Arb_coda = "3011",
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };
                if (itemDetalle.Dmc_vadb != 0 || itemDetalle.Dmc_vacr != 0)
                {
                    items.Add(itemDetalle);
                }
            }
            return items;
        }
        private string ConvertXmlToJson(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            string jsonResul = JsonConvert.SerializeXmlNode(doc);

            return jsonResul;
        }

        private static List<List<T>> SegmentarListas<T>(List<T> lista, int tamañoSegmento)
        {
            List<List<T>> listasSegmentadas = new List<List<T>>();
            for (int i = 0; i < lista.Count; i += tamañoSegmento)
            {
                List<T> subLista = lista.GetRange(i, Math.Min(tamañoSegmento, lista.Count - i));
                listasSegmentadas.Add(subLista);
            }
            return listasSegmentadas;

        }

        public async Task<Response<UsuarioDto>> ConsultaUsuarioActaComite(User usuario)
        {
            return new Response<UsuarioDto> { Data = this._mapper.Map<UsuarioDto>(await this._creditoRepository.ConsultaUsuarioActaComite(usuario)) };
        }

        public async Task<Response<IntegranteComiteCreditoDto>> CrearIntegranteComiteCredito(CrearIntegranteComiteCreditoDto Integrante)
        {
            IntegranteComite integranteComite = this._mapper.Map<IntegranteComite>(Integrante);
            return new Response<IntegranteComiteCreditoDto> { Data = this._mapper.Map<IntegranteComiteCreditoDto>(await this._creditoRepository.CrearIntegranteComiteCredito(integranteComite)) };
        }
        public async Task<Response<IntegranteComiteCreditoDto>> ActualizarIntegranteComiteCredito(CrearIntegranteComiteCreditoDto Integrante)
        {
            IntegranteComite integranteComite = this._mapper.Map<IntegranteComite>(Integrante);
            return new Response<IntegranteComiteCreditoDto> { Data = this._mapper.Map<IntegranteComiteCreditoDto>(await this._creditoRepository.ActualizarIntegranteComiteCredito(integranteComite)) };
        }

        public async Task<Response<bool>> EliminarIntegranteComiteCredito(long Id_Integrante)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = await _creditoRepository.EliminarIntegranteComiteCredito(Id_Integrante);
            respuesta.IsSuccess = true;
            return respuesta;
        }

        public async Task<Response<IEnumerable<DocumentosActRespuestaDto>>> ObtenerDocumentosActSimulacion(DocumentosActSimulacionDto request)
        {
            try
            {
                var peticionDocumento = this._mapper.Map<DocumentosActSimulacion>(request);
                return new Response<IEnumerable<DocumentosActRespuestaDto>> { Data = this._mapper.Map<IEnumerable<DocumentosActRespuestaDto>>(await this._creditoRepository.ObtenerDocumentosActSimulacion(peticionDocumento)) };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<DocumentosActRespuestaDto>> { Data = null, IsSuccess = false, ReturnMessage = "Error Obteniendo documentos de PreAprobado, error: " + ex.Message };
            }

        }

        public async Task<Response<ResponseDocumentoSimDto>> CrearDocumentoSimulacion(RequestDocumentoSimDto requestDocumentoSolicCredito)
        {

            var attachNew = new Core.ReferenciasSvc.WorkManager.AttachRequestDto();
            AttachRequestDto attachRequestDto = new AttachRequestDto();
            attachRequestDto.UserOperation = "web";
            string extension = "";
            attachRequestDto.Files = new List<FileDto>();
            SolicitudSimulacion informacionSimulacion = await _creditoRepository.ObtenerSolicitudSimulacionSLS(requestDocumentoSolicCredito.idSimulacion);
            try
            {
                string FilePath = requestDocumentoSolicCredito.rutaImagen;
                var fileExtensions = new List<string> { "pdf", "tif" };
                int folios = 0;

                string archivoBase64 = string.Empty;
                byte[] conten;

                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
                if (blogStorage.SRD_ESTADO)
                {
                    var bdinfo = await _blobStorageGenericRepository.DescargarArchivoStorage("credito", $"{requestDocumentoSolicCredito.rutaImagen}");
                    var fileSResult = new FileStreamResult(bdinfo.Content, bdinfo.ContentType);
                    extension = (FilePath.Split(".")[1]);
                    if (fileExtensions.Contains(FilePath.Split(".")[1]))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await bdinfo.Content.CopyToAsync(memoryStream);
                            conten = memoryStream.ToArray();
                        }
                        string contenidoTexto = Encoding.UTF8.GetString(conten);

                        folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                        archivoBase64 = Convert.ToBase64String(conten);

                        using (StreamReader stramReadre = new StreamReader(fileSResult.FileStream))
                        {
                            conten = Encoding.UTF8.GetBytes(stramReadre.ReadToEnd());
                            archivoBase64 = Convert.ToBase64String(conten);
                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(stramReadre.ReadToEnd()).Count;
                        };
                    }
                    else folios = 0;

                }
                else
                {
                    string carpeta = ""; 
                    if(requestDocumentoSolicCredito.idTipoDocumento == 161)
                    {
                        carpeta = "reportes-sfc"; 
                    }
                    else
                    {
                        carpeta = "credito"; 
                    }
                    var bdinfoLocal = await _localBlogStorageRepository.DescargarArchivoLocal(carpeta, $"{requestDocumentoSolicCredito.rutaImagen}");
                    if (bdinfoLocal.IsSuccess && bdinfoLocal.Data != null)
                    {
                        byte[] fileBytes = bdinfoLocal.Data;
                        extension = Path.GetExtension(requestDocumentoSolicCredito.rutaImagen).Replace(".", "").ToLower();

                        if (fileExtensions.Contains(extension))
                        {
                            // Convertimos los bytes a Base64
                            archivoBase64 = Convert.ToBase64String(fileBytes);

                            // Convertimos bytes a string para contar folios (solo PDFs)
                            string contenidoTexto = Encoding.UTF8.GetString(fileBytes);

                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;
                        }
                        else
                        {
                            folios = 0;
                        }
                    }
                    else
                    {
                        // Si no existe el archivo o falló la lectura
                        folios = 0;
                        archivoBase64 = string.Empty;
                    }
                }

                var filenamewhitoutext = Guid.Parse(FilePath.Split(".")[0]);
                var documento = new ResponseDocumentoSimulacion
                {
                    DSC_ID = filenamewhitoutext,
                    SLS_ID = requestDocumentoSolicCredito.idSimulacion,
                    TDC_ID = requestDocumentoSolicCredito.idTipoDocumento,
                    DSC_CODIGO_BARRAS = requestDocumentoSolicCredito.codigoBarras,
                    DSC_FECHA_DOCUMENTO = DateTime.Now,
                    DSC_NUMERO_FOLIOS = blogStorage.SRD_ESTADO ? folios : 0,
                    DSC_TAMAÑO = 0,
                    DSC_RUTA_IMAGEN = requestDocumentoSolicCredito.rutaImagen,
                    DSC_ESTADO = "1",
                    DSC_CREADO_POR = requestDocumentoSolicCredito.creadoPor,
                    DSC_FECHA_CREACION = DateTime.Now,
                };

                var infoTipoDocument = ObtenerTipoDocumentoPorId(requestDocumentoSolicCredito.idTipoDocumento.ToString()).Result.Data;

                var documentos = await _gestorDocumentSolicitudes.Value
    .ObtenerDocumentosPorRadicado(informacionSimulacion.SLS_NUMERO_RADICADO);

                bool documentoYaCargado = documentos?.Data?.FileProperties?
                    .Any(x => string.Equals(
                        x.Description?.Trim(),
                        infoTipoDocument?.Descripcion?.Trim(),
                        StringComparison.OrdinalIgnoreCase)) == true;


                if (informacionSimulacion != null)
                    {
                    attachRequestDto.Radicado = informacionSimulacion.SLS_NUMERO_RADICADO;

                    if (!documentoYaCargado)
                    {
                        attachRequestDto.Files.Add(CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, extension));

                        if (attachRequestDto.Files.Count > 0)
                        {
                            var responseWorkmanager = await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                        }
                    }
                }


                return new Response<ResponseDocumentoSimDto> { Data = this._mapper.Map<ResponseDocumentoSimDto>(await this._creditoRepository.CrearDocumentoSimulacion(documento)) };
            }
            catch (Exception ex)
            {
                return new Response<ResponseDocumentoSimDto> { Data = null, IsSuccess = false, ReturnMessage = "Error creando documento en la BD, error: " + ex.Message };
            }

        }

        public async Task<Response<ResponseDocumentoSimDto>> CargarDocumentosProspeccion(RequestDocumentoSimDto requestDocumentoSolicCredito)
        {
            var attachRequestDto = new AttachRequestDto
            {
                UserOperation = "web",
                Files = new List<FileDto>()
            };

            string extension = string.Empty;

            // Obtener información de simulación
            SolicitudSimulacion informacionSimulacion =
                await _creditoRepository.ObtenerSolicitudSimulacionSLS(requestDocumentoSolicCredito.idSimulacion);

            // Obtener documentos generados
            var documentosActSimulacionDto = new DocumentosActSimulacionDto
            {
                SLS_ID = requestDocumentoSolicCredito.idSimulacion,
                AFL_ID = 1
            };
            var documentosGenerado = await ObtenerDocumentosActSimulacion(documentosActSimulacionDto);

            // Documentos a cargar
            List<string> ListaDocumentosCargar = new()
            {
                "Registro consulta centrales de riesgos",
                "Reporte centrales de riesgo"
            };


            SolicitudSimulacion datosPreaprobadoSimulacion = new SolicitudSimulacion();
            datosPreaprobadoSimulacion.SLS_ID = requestDocumentoSolicCredito.idSimulacion;
            datosPreaprobadoSimulacion.SLS_NUMERO_PREAPROBADO = informacionSimulacion.SLS_NUMERO_PREAPROBADO;

            var solicitudPreA = (await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(datosPreaprobadoSimulacion));

            IEnumerable<DocumentosAdicionalesCargados> documentosAdicionales = await _reportesRepository.ConsultarDocumentosAdicionalesCargadosReclasificacion(solicitudPreA.SMC_NUMERO_DOCUMENTO);



            DocumentosActRespuestaDto respuestaDto = new DocumentosActRespuestaDto();

            foreach (var item in documentosAdicionales)
            {
                ListaDocumentosCargar.Add(item.DSA_DOCUMENTO);
            }


            var documentosFiltrados = new List<DocumentosActRespuestaDto>();

            foreach (var item in ListaDocumentosCargar)
            {
                foreach (var listaDeDocumentoParam in documentosGenerado.Data)
                {
                    if (item.Contains(listaDeDocumentoParam.TIPO_DOCUMENTO))
                    {
                        foreach (var itemDocumentoAdicional in documentosAdicionales)
                        {
                            if (itemDocumentoAdicional.DSA_DOCUMENTO.Contains(listaDeDocumentoParam.TIPO_DOCUMENTO))
                            {
                                listaDeDocumentoParam.RUTA_IMAGEN = itemDocumentoAdicional.DSA_RUTA;

                            }
                        }
                        documentosFiltrados.Add(listaDeDocumentoParam);
                    }

                }
            }


            foreach (var item in documentosFiltrados)
            {
                attachRequestDto.Files = new List<FileDto>();

                try
                {
                    string filePath = item.RUTA_IMAGEN;
                    var fileExtensions = new List<string> { "pdf", "tif" };
                    int folios = 0;
                    string archivoBase64 = string.Empty;
                    byte[] contenidoBytes;

                    ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

                    // -------------------------------
                    // Obtención archivo desde Storage
                    // -------------------------------
                    if (blogStorage.SRD_ESTADO)
                    {
                        var bdinfo = await _blobStorageGenericRepository
                                        .DescargarArchivoStorage("credito", item.RUTA_IMAGEN);

                        extension = filePath.Split(".")[1];

                        if (fileExtensions.Contains(extension))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                await bdinfo.Content.CopyToAsync(memoryStream);
                                contenidoBytes = memoryStream.ToArray();
                            }

                            string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                            archivoBase64 = Convert.ToBase64String(contenidoBytes);
                        }
                        else
                        {
                            folios = 0;
                        }
                    }
                    else
                    {
                        var bdinfoLocal = await _localBlogStorageRepository
                                           .DescargarArchivoLocal("credito", item.RUTA_IMAGEN);

                        if (bdinfoLocal.IsSuccess && bdinfoLocal.Data != null)
                        {
                            contenidoBytes = bdinfoLocal.Data;
                            extension = Path.GetExtension(item.RUTA_IMAGEN)
                                            .Replace(".", "")
                                            .ToLower();

                            if (fileExtensions.Contains(extension))
                            {
                                archivoBase64 = Convert.ToBase64String(contenidoBytes);

                                string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                                folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;
                            }
                        }
                        else
                        {
                            folios = 0;
                            archivoBase64 = string.Empty;
                        }
                    }

                    // -------------------------------
                    // Crear documento
                    // -------------------------------

                    var documento = new ResponseDocumentoSimulacion
                    {
                        DSC_ID = Guid.NewGuid(),
                        SLS_ID = requestDocumentoSolicCredito.idSimulacion,
                        TDC_ID = requestDocumentoSolicCredito.idTipoDocumento,
                        DSC_CODIGO_BARRAS = requestDocumentoSolicCredito.codigoBarras,
                        DSC_FECHA_DOCUMENTO = DateTime.Now,
                        DSC_NUMERO_FOLIOS = blogStorage.SRD_ESTADO ? folios : 0,
                        DSC_TAMAÑO = 0,
                        DSC_RUTA_IMAGEN = requestDocumentoSolicCredito.rutaImagen,
                        DSC_ESTADO = "1",
                        DSC_CREADO_POR = requestDocumentoSolicCredito.creadoPor,
                        DSC_FECHA_CREACION = DateTime.Now
                    };

                    DocumentoCargadoDodo dod_dodo = new DocumentoCargadoDodo();

                    dod_dodo.DCD_CARGADO = true;
                    dod_dodo.DCD_DESCRIPCION = item.TIPO_DOCUMENTO;
                    dod_dodo.RUTA = item.RUTA_IMAGEN;
                    dod_dodo.SLS_ID = requestDocumentoSolicCredito.idSimulacion;
                    dod_dodo.TDC_ID = item.TDC_ID;

                    // -------------------------------
                    // Adjuntar al gestor documental
                    // -------------------------------
                    if (informacionSimulacion != null)
                    {
                        attachRequestDto.Radicado = informacionSimulacion.SLS_NUMERO_RADICADO;

                        var infoTipoDocument =
                            ObtenerTipoDocumentoPorId(item.TDC_ID.ToString()).Result.Data;

                        attachRequestDto.Files.Add(
                            CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, extension)
                        );

                        if (attachRequestDto.Files.Count > 0)
                        {
                          
                            var documentos = await _gestorDocumentSolicitudes.Value
   .ObtenerDocumentosPorRadicado(attachRequestDto.Radicado);

                            bool documentoYaCargado = documentos?.Data?.FileProperties?
                                .Any(x => string.Equals(
                                    x.Description?.Trim(),
                                    infoTipoDocument?.Descripcion?.Trim(),
                                    StringComparison.OrdinalIgnoreCase)) == true;

                            if (!documentoYaCargado)
                            {

                                await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new Response<ResponseDocumentoSimDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "Error creando documento en la BD, error: " + ex.Message
                    };
                }
            }

            return new Response<ResponseDocumentoSimDto>
            {
                Data = null,
                IsSuccess = false,
                ReturnMessage = "Todo cargado en DODO"
            };
        }

        public async Task<Response<RequestUpdateSlsTuDto>> UpdateSlsReporteTU(RequestUpdateSlsTuDto requestUpdateSlsTuDto)
        {
            UpdateSlsTu Request = this._mapper.Map<UpdateSlsTu>(requestUpdateSlsTuDto);
            return new Response<RequestUpdateSlsTuDto> { Data = this._mapper.Map<RequestUpdateSlsTuDto>(await this._creditoRepository.ActualizarSlsReporteTU(Request)) };
        }

        public async Task<Response<ValoresProyectadosDto>> GuardarValoresProyectados(ValoresProyectadosDto valoresProyectadosDto)
        {
            ValoresProyectados Request = this._mapper.Map<ValoresProyectados>(valoresProyectadosDto);
            return new Response<ValoresProyectadosDto> { Data = this._mapper.Map<ValoresProyectadosDto>(await this._creditoRepository.GuardarValoresProyectados(Request)) };
        }
        public async Task<Response<ValoresProyectadosDto>> ObtenerValoresProyectados(Guid RequestSLS)
        {
            return new Response<ValoresProyectadosDto> { Data = this._mapper.Map<ValoresProyectadosDto>(await this._creditoRepository.ObtenerValoresProyectados(RequestSLS)) };
        }

        public async Task<Response<bool>> CheckDocumentosCredito(CheckDocumentosCreditoDto checkDocumentos)
        {
            await _creditoRepository.EliminarCheckArchivosCredito(checkDocumentos.SolicitudCreditoId);

            foreach (var item in checkDocumentos.DocumentosVerificados)
            {
                await _creditoRepository.CheckArchivosCredito(checkDocumentos.SolicitudCreditoId, item);
            }

            return new Response<bool>
            {
                Data = true,
            }; ;
        }

        public async Task<Response<IEnumerable<CheckArchivo>>> ObtenerArchivosChecksCredito(Guid IdSolicitud)
        {
            var archivosChecks = await _creditoRepository.ObtenerArchivosCheckCredito(IdSolicitud);

            return new Response<IEnumerable<CheckArchivo>>
            {
                Data = archivosChecks
            };
        }


        public async Task<Response<IEnumerable<ArchivosCredito>>> ConsultarArchivosPorTipoCredito()
        {
            var archivos = await _creditoRepository.ConsultarArchivosPorTipoCredito();
            return new Response<IEnumerable<ArchivosCredito>>
            {
                Data = archivos
            };

        }

        public async Task<Response<IEnumerable<MediosCreditoVivienda>>> ConsultarMediosCreditosVivienda()
        {
            var archivos = await _creditoRepository.ConsultarMediosCreditosVivienda();
            return new Response<IEnumerable<MediosCreditoVivienda>>
            {
                Data = archivos
            };
        }


        public async Task<Response<DatosAdicionalesSimulacion>> ActualizarDatosAdicionalesSimulacion(DatosAdicionalesSimulacionDto simulacionDto)
        {

            DatosAdicionalesSimulacion datosAdicionalesSimulacion = this._mapper.Map<DatosAdicionalesSimulacion>(simulacionDto);

            var datosSimulacion = await _creditoRepository.ActualizarDatosAdicionalesSimulacion(datosAdicionalesSimulacion);
            return new Response<DatosAdicionalesSimulacion>
            {
                Data = datosSimulacion
            };

        }

        public async Task<Response<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>>> ObtenerDetallePagoSeven(RequestDetalleSeven solicitud)
        {
            var obtenerDetalle = this._mapper.Map<RequestDetalleSeven>(solicitud);
            var response = this._mapper.Map<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>>(
                            await this._creditoRepository.ObtenerDetallePagoSeven(obtenerDetalle)
                        ).ToList();


            return new Response<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>> { Data = response };
        }
        public async Task<Response<IEnumerable<ObtenerValoresCDPSevenDto>>> ObtenerValorCDP(RequestValoresCDPSeven request)
        {
            var obtenerDetalle = this._mapper.Map<RequestValoresCDPSeven>(request);
            var response = this._mapper.Map<IEnumerable<ObtenerValoresCDPSevenDto>>(
                            await this._creditoRepository.ObtenerValorCDP(obtenerDetalle)
                        ).ToList();
            return new Response<IEnumerable<ObtenerValoresCDPSevenDto>> { Data = response };

        }
        public async Task<Response<DocumentosActRespuestaDto>> ActualizarDocLegalizacion(DocumentosActEstadoRequestDto request)
        {
            var actualizarEstadoDoc = await _creditoRepository.ActualizarDocLegalizacion(request);

            return new Response<DocumentosActRespuestaDto>
            {
                Data = actualizarEstadoDoc
            };
        }
        public async Task<Response<object>> GenerarComprobanteContabilidad(RequestDetallePago detallepago, string proceso, decimal total_Credito, decimal total_Debito)
        {
            try
            {
                var tipoComprobanteId = (await _catalogosBusinessLogic.ObtenerTipoComprobanteCartera()).Data.FirstOrDefault(x => x.Codigo.Equals(proceso));

                if (tipoComprobanteId is null)
                    return new() { IsSuccess = false, ReturnMessage = "Error No se encontro el tipo de comprobante." };

                ComprobanteCarteraDto ReqComprobante = new()
                {
                    Fecha_Comprobante = DateTime.Now,
                    Tipo_Comprobante = tipoComprobanteId.TipoComprobanteId,
                    Ciudad = 1,
                    Departamento = 5,
                    Concepto = 1,
                    Beneficiario = 1,
                    LstMovimientoComprobante = null,
                    Total_Comprobante_Credito = total_Credito,
                    Total_Comprobante_Debito = total_Debito
                };


                var crearComprobanteCartera = this._mapper.Map<ComprobanteCartera>(ReqComprobante);

                var response = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.CrearComprobanteCartera(crearComprobanteCartera));

                //return new Response<ComprobanteCarteraDto> { Data = response };

                //var Comprobante = await CrearComprobanteCartera(ReqComprobante, cancellationToken);
                return new Response<object>
                {
                    IsSuccess = true,
                    ReturnMessage = "Comprobante Contabilidad Interno Creado: " + +response.Comprobante_Id,
                    Data = response.Comprobante_Id
                };

            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al generar comprobante." };
            }
        }
        public async Task<Response<IEnumerable<ObtenerDetalleFacturaSevenDto>>> ObtenerValorFactura(RequestValoresFactura request)
        {
            var obtenerDetalle = this._mapper.Map<RequestValoresFactura>(request);
            var response = this._mapper.Map<IEnumerable<ObtenerDetalleFacturaSevenDto>>(
                            await this._creditoRepository.ObtenerValorFactura(obtenerDetalle)
                        ).ToList();
            return new Response<IEnumerable<ObtenerDetalleFacturaSevenDto>> { Data = response };

        }
        public async Task<Response<IEnumerable<ObtenerComprobantesDto>>> ObtenerComprobantesCredito(RequestObtenerComprobantesCredito request)
        {
            var obtenerDetalle = this._mapper.Map<RequestObtenerComprobantesCredito>(request);
            var response = this._mapper.Map<IEnumerable<ObtenerComprobantesDto>>(
                            await this._creditoRepository.ObtenerComprobantesCredito(obtenerDetalle)
                        ).ToList();
            return new Response<IEnumerable<ObtenerComprobantesDto>> { Data = response };

        }



        public async Task<bool> InsertarNotificacionesSolicitudes(NotificacionesSolicitudesDto notificacionesSolicitudesDto)
        {
            NotificacionesSolicitudes notificacionesSolicitudes = new NotificacionesSolicitudes();
            var datosNotificacion = await _creditoRepository.ConsultarNotificacionesSolicitudes(notificacionesSolicitudesDto.simulationID);

            string consecutivoCDP = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString("D2") + notificacionesSolicitudesDto.NumeroSolicitud.ToString();

            notificacionesSolicitudes.NOT_CDP = consecutivoCDP;
            notificacionesSolicitudes.SLS_ID = notificacionesSolicitudesDto.simulationID;

            return await _creditoRepository.InsertarNotificacionesSolicitudes(notificacionesSolicitudes);

        }

        public async Task<Response<NotificacionesSolicitudes>> ConsultarNotificacionesSolicitudes(Guid simulacionID)
        {

            var datosNotificacion = await _creditoRepository.ConsultarNotificacionesSolicitudes(simulacionID);

            if (datosNotificacion != null)
            {
                return new Response<NotificacionesSolicitudes>
                {
                    Data = datosNotificacion
                };
            }
            else
            {
                return new Response<NotificacionesSolicitudes>
                {
                    Data = null
                };
            }

        }

        public async Task<Response<IEnumerable<AprobacionCreditoAsesor>>> ConsultarAprobacionesCreditoAsesor(Guid trsID)
        {
            var archivos = await _creditoRepository.ConsultarAprobacionAsesor(trsID);

            if (archivos != null)
            {
                return new Response<IEnumerable<AprobacionCreditoAsesor>>
                {
                    Data = archivos
                };
            }
            else
            {
                return new Response<IEnumerable<AprobacionCreditoAsesor>>
                {
                    Data = null
                };
            }
        }


        public async Task<Response<DatosAsegurabilidadSolicitudCredito>> ConsultarAsegurabilidadCredito(Guid solicitudCreditoID)
        {

            var datosCredito = await _creditoRepository.ConsultarDatosAsegurabilidadCredito(solicitudCreditoID);

            if (datosCredito != null)
            {
                return new Response<DatosAsegurabilidadSolicitudCredito>
                {
                    Data = datosCredito
                };
            }
            else
            {
                return new Response<DatosAsegurabilidadSolicitudCredito>
                {
                    Data = null
                };
            }

        }


        public async Task<bool> InsertarPrimerPagoCDP(InsertarDatosCDPPrimerPago notificacionesSolicitudesDto)
        {
            InsertarDatosCDPPrimerPago notificacionesSolicitudes = new InsertarDatosCDPPrimerPago();

            notificacionesSolicitudes.PPC_CDP = notificacionesSolicitudesDto.PPC_CDP;
            notificacionesSolicitudes.SLS_ID = notificacionesSolicitudesDto.SLS_ID;

            return await _creditoRepository.InsertarPrimerPagoCDP(notificacionesSolicitudes);

        }


        public async Task<Response<PrimerPagoCDPCredito>> ConsultarPrimerPagoCDP(Guid simulacionID)
        {

            var datosPrimerPagoCDP = await _creditoRepository.ConsultarPrimerPagoCDP(simulacionID);

            if (datosPrimerPagoCDP != null)
            {
                return new Response<PrimerPagoCDPCredito>
                {
                    Data = datosPrimerPagoCDP
                };
            }
            else
            {
                return new Response<PrimerPagoCDPCredito>
                {
                    Data = null
                };
            }

        }

        public async Task<Response<DatosPersonalesCreditoEducativo>> ActualizarDatosPersonalesEducativo(DatosPersonalesCreditoEducativoDto datosDto)
        {

            var response = new Response<DatosPersonalesCreditoEducativo>();

            if (datosDto == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Los datos personales no pueden ser nulos";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.NumeroDocumento))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El número de documento es obligatorio";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.PrimerNombreEstudiante))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El primer nombre del estudiante es obligatorio";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.PrimerApellidoEstudiante))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El primer apellido del estudiante es obligatorio";
                return response;
            }

            if (datosDto.ValorDeCredito <= 0)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El valor del crédito debe ser mayor a 0";
                return response;
            }





            DatosPersonalesCreditoEducativo datosEntity = new DatosPersonalesCreditoEducativo
            {
                DPE_ID = Guid.NewGuid(),
                DPE_TIPO_DOCUMENTO = datosDto.TipoDocumento,
                DPE_NUMERO_DOCUMENTO = datosDto.NumeroDocumento,
                DPE_PRIMER_NOMBRE_ESTUDIANTE = datosDto.PrimerNombreEstudiante,
                DPE_SEGUNDO_NOMBRE_ESTUDIANTE = datosDto.SegundoNombreEstudiante,
                DPE_PRIMER_APELLIDO_ESTUDIANTE = datosDto.PrimerApellidoEstudiante,
                DPE_SEGUNDO_APELLIDO_ESTUDIANTE = datosDto.SegundoApellidoEstudiante,
                DPE_DIRECCION_ESTUDIANTE = datosDto.DireccionEstudiante,
                DPE_VALOR_CREDITO = datosDto.ValorDeCredito,
                DPE_PERIODO_LECTIVO = datosDto.PeriodoLectivo,
                FK_ENE_ID = datosDto.EntidadEducativaID,
                ENE_DPD_ID = datosDto.DepartamentoID,
                ENE_DPC_ID = datosDto.CiudadID,
                DPE_NUMERO_IDENTIFICACION_TITULAR = datosDto.NumeroIdentificacionTitular
            };


            var beneficiarioConsulta = await _creditoRepository.ConsultarDatosPersonalesEducativo(datosEntity);


            if (beneficiarioConsulta != null)
            {
                if (beneficiarioConsulta.DPE_NUMERO_IDENTIFICACION_TITULAR == datosEntity.DPE_NUMERO_IDENTIFICACION_TITULAR
               && beneficiarioConsulta.DPE_NUMERO_DOCUMENTO != datosEntity.DPE_NUMERO_DOCUMENTO)
                {
                    response.IsSuccess = true;
                    response.ReturnMessage = "El afiliado cuenta con un credito educativo en tramite para este beneficiario";
                    response.Data = datosEntity;
                }


                if (beneficiarioConsulta.DPE_NUMERO_IDENTIFICACION_TITULAR != datosEntity.DPE_NUMERO_IDENTIFICACION_TITULAR
                    && beneficiarioConsulta.DPE_NUMERO_DOCUMENTO == datosEntity.DPE_NUMERO_DOCUMENTO)
                {
                    response.IsSuccess = true;
                    response.ReturnMessage = "El beneficiario cuenta con un proceso de credito educativo en tramite";
                    response.Data = datosEntity;
                }
            }


            var actualizado = await _creditoRepository.ActualizarDatosPersonalesEducativo(datosEntity);

            if (actualizado.DPE_NUMERO_DOCUMENTO != null)
            {
                response.IsSuccess = true;
                response.ReturnMessage = "Datos personales actualizados correctamente";
                response.Data = datosEntity;
            }
            else
            {
                response.IsSuccess = false;
                response.ReturnMessage = "No se pudo actualizar los datos personales";
            }

            return response;
        }

        public async Task<Response<DatosPersonalesCreditoEducativo>> InsertarDatosPersonalesEducativo(DatosPersonalesCreditoEducativoDto datosDto)
        {
            var response = new Response<DatosPersonalesCreditoEducativo>();

            if (datosDto == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Los datos personales no pueden ser nulos";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.NumeroDocumento))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El número de documento es obligatorio";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.PrimerNombreEstudiante))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El primer nombre del estudiante es obligatorio";
                return response;
            }

            if (string.IsNullOrWhiteSpace(datosDto.PrimerApellidoEstudiante))
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El primer apellido del estudiante es obligatorio";
                return response;
            }

            if (datosDto.ValorDeCredito <= 0)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El valor del crédito debe ser mayor a 0";
                return response;
            }

            DatosPersonalesCreditoEducativo datosEntity = new DatosPersonalesCreditoEducativo
            {
                DPE_ID = Guid.NewGuid(),
                DPE_TIPO_DOCUMENTO = datosDto.TipoDocumento,
                DPE_NUMERO_DOCUMENTO = datosDto.NumeroDocumento,
                DPE_PRIMER_NOMBRE_ESTUDIANTE = datosDto.PrimerNombreEstudiante,
                DPE_SEGUNDO_NOMBRE_ESTUDIANTE = datosDto.SegundoNombreEstudiante,
                DPE_PRIMER_APELLIDO_ESTUDIANTE = datosDto.PrimerApellidoEstudiante,
                DPE_SEGUNDO_APELLIDO_ESTUDIANTE = datosDto.SegundoApellidoEstudiante,
                DPE_DIRECCION_ESTUDIANTE = datosDto.DireccionEstudiante,
                DPE_VALOR_CREDITO = datosDto.ValorDeCredito,
                DPE_PERIODO_LECTIVO = datosDto.PeriodoLectivo,
                FK_ENE_ID = datosDto.EntidadEducativaID,
                ENE_DPD_ID = datosDto.DepartamentoID,
                ENE_DPC_ID = datosDto.CiudadID
            };

            var actualizado = await _creditoRepository.InsertarDatosPersonalesEducativo(datosEntity);

            if (actualizado.DPE_NUMERO_DOCUMENTO != null)
            {
                response.IsSuccess = true;
                response.ReturnMessage = "Datos personales actualizados correctamente";
                response.Data = datosEntity;
            }
            else
            {
                response.IsSuccess = false;
                response.ReturnMessage = "No se pudo actualizar los datos personales";
            }

            return response;
        }

        public async Task<Response<DatosPersonalesCreditoEducativo>> ConsultarDatosPersonalesEducativo(DatosPersonalesCreditoEducativoDto datosPersonales)
        {
            var response = new Response<DatosPersonalesCreditoEducativo>();

            try
            {
                DatosPersonalesCreditoEducativo datosEntity = new DatosPersonalesCreditoEducativo
                {
                    DPE_NUMERO_IDENTIFICACION_TITULAR = datosPersonales.NumeroIdentificacionTitular
                };

                var resultado = await _creditoRepository.ConsultarDatosPersonalesEducativo(datosEntity);

                if (resultado == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No se encontraron datos personales para el número de documento proporcionado.";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = resultado;
                response.ReturnMessage = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = $"Error inesperado: {ex.Message}";
            }

            return response;
        }

        public async Task<Response<BeneficiarioEntidadEducativa>> InsertarBeneficiarioEntidadEducativa(BeneficiarioEntidadEducativaDto datosPersonalesDto)
        {
            if (datosPersonalesDto == null)
            {
                return new Response<BeneficiarioEntidadEducativa>
                {
                    IsSuccess = false,
                    ReturnMessage = "Los datos del beneficiario no pueden ser nulos.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(datosPersonalesDto.numeroDocumentoInstituto) || string.IsNullOrWhiteSpace(datosPersonalesDto.nombreInstituto))
            {
                return new Response<BeneficiarioEntidadEducativa>
                {
                    IsSuccess = false,
                    ReturnMessage = "El NIT y la razón social son obligatorios.",
                    Data = null
                };
            }

            // Mapeo de DTO a Entidad
            var beneficiarioEntidadEducativa = new BeneficiarioEntidadEducativa
            {
                BEN_NIT = datosPersonalesDto.numeroDocumentoInstituto,
                BEN_TIPO_DOCUENTO = datosPersonalesDto.tipoDucumentoEntidad,
                BEN_RAZON_SOCIAL_ENTIDAD = datosPersonalesDto.nombreInstituto,
                BEN_DIRECCION = datosPersonalesDto.direccionEntidad,
                BEN_DEPARTAMENTO = datosPersonalesDto.departamentoIdEntidad,
                BEN_CIUDAD = datosPersonalesDto.ciudadIdEntidad,
                BEN_CORREO = datosPersonalesDto.correoEntidad,
                BEN_TELEFONO = datosPersonalesDto.telefonoEntidad,
                BEN_TIPO_CUENTA = datosPersonalesDto.tipoCuentaEntidad,
                BEN_NUMERO_CUENTA = datosPersonalesDto.numeroCuentaEntidad,
                BEN_BANCO = datosPersonalesDto.bancoEntidad,
                BEN_FECHA_LIMITE = datosPersonalesDto.fechaLimite,
                SLS_ID = datosPersonalesDto.SLS_ID,
                DPE_NUMERO_TITULAR = datosPersonalesDto.NumeroTitularAfiliado
            };

            var resultado = await _creditoRepository.InsertarBeneficiarioEntidadEducativa(beneficiarioEntidadEducativa);

            return new Response<BeneficiarioEntidadEducativa>
            {
                IsSuccess = true,
                ReturnMessage = "Beneficiario insertado correctamente.",
                Data = resultado
            };
        }


        public async Task<Response<InformacionGeneralCreditoEducativo>> ConsultarInformacionCreditoEducativo(string NumeroDocumento)
        {
            var response = new Response<InformacionGeneralCreditoEducativo>();

            try
            {
                DatosPersonalesCreditoEducativo datosEntity = new DatosPersonalesCreditoEducativo
                {
                    DPE_NUMERO_IDENTIFICACION_TITULAR = NumeroDocumento
                };

                var informacionSolicitudCredito = await _creditoRepository.ConsultarDatosPersonalesEducativo(datosEntity);

                if (informacionSolicitudCredito == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No se encontraron datos personales para el número de documento proporcionado.";
                    return response;
                }



                var informacionGeneral = new InformacionGeneralCreditoEducativo
                {
                    InformacionSolicitudCredito = new InformacionSolicitudCreditoEducativo
                    {
                        TCR_ID = 3,
                        NOMBRE_ESTUDIANTE = informacionSolicitudCredito.DPE_PRIMER_NOMBRE_ESTUDIANTE,
                        NUMERO_DOCUMENTO = informacionSolicitudCredito.DPE_NUMERO_DOCUMENTO,
                        DIRECCION = informacionSolicitudCredito.DPE_DIRECCION_ESTUDIANTE
                    },
                    DetalleProducto = new DetalleProductoCreditoEducativo
                    {
                        PORCENTAJE_FINACIACION = 100,
                        PLAZO = 6,
                        VALOR_CREDITO = informacionSolicitudCredito.DPE_VALOR_CREDITO
                    },
                    EntidadEducativa = new BeneficiarioEntidadEducativa
                    {
                        BEN_TIPO_DOCUENTO = informacionSolicitudCredito.TIPO_DOCUMENTO_ENTIDAD_EDUCATIVA,
                        BEN_CIUDAD = informacionSolicitudCredito.ENE_DPC_ID,
                        BEN_DEPARTAMENTO = informacionSolicitudCredito.ENE_DPD_ID,
                        BEN_RAZON_SOCIAL_ENTIDAD = informacionSolicitudCredito.NOMBRE_ENTIDAD_EDUCATIVA
                    }

                };

                response.IsSuccess = true;
                response.ReturnMessage = "Información consultada exitosamente.";
                response.Data = informacionGeneral;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = $"Error inesperado: {ex.Message}";
            }

            return response;
        }

        public async Task<Response<IEnumerable<ReportesSolicitudCredito>>> ObtenerReportesSolicitudCreditoGeneral(ReportesSolicitudCreditoDto datos)
        {

            var resultado = await _creditoRepository.ReportesSolicitudCreditoGeneral(datos);

            return new Response<IEnumerable<ReportesSolicitudCredito>>
            {
                Data = resultado,
                IsSuccess = true,
                ReturnMessage = "Lista de creditos"

            };
        }

        public async Task<Response<IEnumerable<ReportesSolicitudCredito>>> ReportesSolicitudCreditoGeneralUsuuarioExterno(ReportesSolicitudCreditoDto datos)
        {
            var resultado = await _creditoRepository.ReportesSolicitudCreditoGeneralUsuuarioExterno(datos);

            return new Response<IEnumerable<ReportesSolicitudCredito>>
            {
                Data = resultado,
                IsSuccess = true,
                ReturnMessage = "Lista de créditos"
            };
        }


        public async Task<Response<FacturaSevenCredito>> ConsultarDatosFactura(Guid SOC_ID)
        {

            try
            {
                FacturaSevenCredito datosFactura = await _creditoRepository.ConsultarDatosFactura(SOC_ID);

                return new Response<FacturaSevenCredito>
                {
                    IsSuccess = true,
                    Data = datosFactura,
                    ReturnMessage = "Consulta exitosa."
                };

            }
            catch (Exception ex)
            {
                return new Response<FacturaSevenCredito>
                {
                    IsSuccess = true,
                    Data = null,
                    ReturnMessage = "Error en la consulta"
                };
            }
        }

        public async Task<Response<ComprobanteCarteraDto>> GenerarComprobanteCreditoSeven(RequestDetallePago detallepago, string proceso, decimal total_Debito, decimal total_Credito)
        {
            try
            {
                var tipoComprobanteId = (await _catalogosBusinessLogic.ObtenerTipoComprobanteCartera()).Data.FirstOrDefault(x => x.Codigo.Equals(proceso));
                ComprobanteCarteraDto ReqComprobante = new()
                {
                    Fecha_Comprobante = DateTime.Now,
                    Tipo_Comprobante = tipoComprobanteId.TipoComprobanteId,
                    Ciudad = 1,
                    Departamento = 5,
                    Concepto = 1,
                    Beneficiario = 1,
                    LstMovimientoComprobante = null,
                    Total_Comprobante_Credito = total_Credito,
                    Total_Comprobante_Debito = total_Debito,
                    estado = false
                };

                var crearComprobanteCartera = this._mapper.Map<ComprobanteCartera>(ReqComprobante);
                var response = this._mapper.Map<ComprobanteCarteraDto>(await _carteraRepository.CrearComprobanteCartera(crearComprobanteCartera));

                //return new Response<ComprobanteCarteraDto> { Data = response };

                //var Comprobante = await CrearComprobanteCartera(ReqComprobante, cancellationToken);
                return new Response<ComprobanteCarteraDto>
                {
                    IsSuccess = true,
                    ReturnMessage = "Comprobante Creado desde crédito: " + +response.Comprobante_Id,
                    Data = response
                };

            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al generar comprobante desde crédito." };
            }
        }



        public async Task<Response<bool>> RegistrarBloqueoCuentaCredito(BloqueoRecursosCreditoDto datos)
        {
            try
            {
                var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(datos.TipoIdentificacion, datos.NumeroIdentificacion)).Data;


                foreach (var item in cliente.Cuentas)
                {
                    BloqueoRecursosCredito datosBloqueo = new BloqueoRecursosCredito();
                    datosBloqueo.BRC_TIPO_IDENTIFICACION = datos.TipoIdentificacion;
                    datosBloqueo.BRC_NUMERO_IDENTIFICACION = datos.NumeroIdentificacion;
                    datosBloqueo.BRC_NUMERO_CUENTA = item.NumeroCuenta;
                    datosBloqueo.BRC_ESTADO = item.IdEct;
                    await _creditoRepository.RegistrarBloqueoCuentaCredito(datosBloqueo);
                }

                return new Response<bool>
                {
                    IsSuccess = true,
                    ReturnMessage = "Datos de afiliado con cuenta bloqueada"
                };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, ReturnMessage = "Error al guardar datos de cuenta bloqueada." };
            }

        }


        public async Task<Response<List<BloqueoRecursosCreditoRespuesta>>> ConsultarBloqueoCuentaCredito(BloqueoRecursosCreditoDto bloqueoRecursos)
        {

            var resultado = await _creditoRepository.ConsultarBloqueoCuentaCredito(bloqueoRecursos);
            List<BloqueoRecursosCreditoRespuesta> listaRecursosBloqueado = new List<BloqueoRecursosCreditoRespuesta>();

            foreach (var item in resultado)
            {
                BloqueoRecursosCreditoRespuesta bloqueoRecursosCredito = new BloqueoRecursosCreditoRespuesta();
                bloqueoRecursosCredito.NumeroCuenta = item.BRC_NUMERO_CUENTA;
                bloqueoRecursosCredito.EstadoCuenta = item.BRC_ESTADO;
                listaRecursosBloqueado.Add(bloqueoRecursosCredito);
            }

            return new Response<List<BloqueoRecursosCreditoRespuesta>>
            {
                Data = listaRecursosBloqueado,
                IsSuccess = true,
                ReturnMessage = "Lista de creditos"
            };
        }

        public async Task<Response<ValidacionOTPTemporal>> ValidacionOTPTemporalTU(ValidacionOTPTemporalDto datos)
        {
            try
            {
                var validacionOTPTemporal = await _creditoRepository.ConsultarValidacionOTPTemporal(datos);

                // Verifica si han pasado más de 24 horas
                if (validacionOTPTemporal != null)
                {
                    TimeSpan diferencia = DateTime.Now - validacionOTPTemporal.VOT_FECHA;

                    if (diferencia.TotalHours >= 24)
                    {
                        await _creditoRepository.EliminarValidacionOTPTemporal(datos);
                        validacionOTPTemporal = null;
                    }
                }

                // Inserta si no existe registro
                if (validacionOTPTemporal == null)
                {
                    await _creditoRepository.InsertarValidacionOTPTemporal(datos);
                    validacionOTPTemporal = await _creditoRepository.ConsultarValidacionOTPTemporal(datos);
                }

                // Si está activo, incrementa intento
                if (validacionOTPTemporal.VOT_ESTADO)
                {
                    await _creditoRepository.InsertarValidacionOTPTemporal(datos);
                }

                // Si está bloqueado, marcar BloqueoDia = true
                if (!validacionOTPTemporal.VOT_ESTADO)
                {
                    validacionOTPTemporal.BloqueoDia = true;
                }

                return new Response<ValidacionOTPTemporal>
                {
                    IsSuccess = true,
                    Data = validacionOTPTemporal,
                    ReturnMessage = validacionOTPTemporal.VOT_ESTADO
                        ? "Validación activa"
                        : "Cuenta bloqueada por exceso de intentos"
                };
            }
            catch (Exception ex)
            {
                return new Response<ValidacionOTPTemporal>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en validación de OTP: {ex.Message}"
                };
            }
        }


        public async Task<Response<ValidacionOTPTemporal>> ConsultarValidacionOTPTemporalTU(ValidacionOTPTemporalDto datos)
        {
            try
            {
                var validacionOTPTemporal = await _creditoRepository.ConsultarValidacionOTPTemporal(datos);

                if (validacionOTPTemporal == null)
                {
                    return new Response<ValidacionOTPTemporal>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró información para la validación del OTP.",
                        Data = null
                    };
                }

                if (!validacionOTPTemporal.VOT_ESTADO)
                {
                    validacionOTPTemporal.BloqueoDia = true;
                }

                // Si todo está bien, retornamos la data
                return new Response<ValidacionOTPTemporal>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta validación OTP exitosa.",
                    Data = validacionOTPTemporal
                };
            }
            catch (Exception ex)
            {
                return new Response<ValidacionOTPTemporal>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en validación de OTP: {ex.Message}"
                };
            }
        }

        public async Task<Response<ValidacionIdentidadAfiliado>> ValidacionAfiliadoIdentidad(DatosIdentificacionDto datos)
        {
            try
            {
                ValidacionIdentidadAfiliado validacionIdentidad = new ValidacionIdentidadAfiliado();
                var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(datos.TipoIdentificacion, datos.Identificacion)).Data;

                double totalEmbargosConceptos = 0;
                decimal totalBloqueosConceptos = 0;
                string estadoCuenta = "";
                bool validacionSegundoPago = false;

                if (cliente == null)
                {
                    validacionIdentidad.Mensaje = "Aún no cuenta con una afiliación activa. Por favor, actualiza tus datos o ponte en contacto con nuestro equipo de soporte";
                    validacionIdentidad.Acceso = false;
                    validacionIdentidad.Caso = 6;
                    return new Response<ValidacionIdentidadAfiliado>
                    {
                        IsSuccess = true,
                        ReturnMessage = "Afiliado sin cuenta regitrada",
                        Data = validacionIdentidad
                    };
                }
                int vinculacion = cliente.IdTipoActor;

                bool pasoAValidacionAfilicionVeterano = false;

                foreach (var solicitudes in cliente.Solicitudes)
                {

                    if (((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("AFILIADO VETERANO")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        || ((solicitudes.TipoSolicitud.Contains("AFILIACION") && solicitudes.Modalidad.Contains("SOLICITUD DE CRÉDITO AVAV")) && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                    {
                        pasoAValidacionAfilicionVeterano = true;

                    }

                }


                if (vinculacion != 14)
                {
                    //no
                    //if (cliente.EstadoCliente != "VIGENTE" && !pasoAValidacionAfilicionVeterano && cliente.EstadoCliente != "BLOQUEADO")
                    //{
                    //    validacionIdentidad.Mensaje = "No es viable, afiliado presenta estado " + cliente.EstadoCliente;
                    //    validacionIdentidad.Acceso = false;
                    //    validacionIdentidad.Caso = 1;
                    //    return new Response<ValidacionIdentidadAfiliado>
                    //    {
                    //        IsSuccess = true,
                    //        ReturnMessage = "Bloqueo por estado del afiliado",
                    //        Data = validacionIdentidad
                    //    };
                    //}

                    //vinculacion 16 y 2 pertecenten a FUNCIONARIO AFILIADO ADMINISTRACION CESANTÍAS y AFILIADO ADMINISTRACION CESANTIAS respectivamente
                    foreach (var cuentas in cliente.Cuentas)
                    {
                        if (vinculacion == 16 || vinculacion == 2)
                        {
                            if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS")
                            {
                                estadoCuenta = cuentas.EstadoCuenta;
                                foreach (var conceptoEmbargado in cuentas.Embargos)
                                {
                                    totalEmbargosConceptos += conceptoEmbargado.Valor;
                                }
                                foreach (var conceptoBloqueoado in cuentas.ConceptosSaldosBloqueos)
                                {
                                    totalBloqueosConceptos += conceptoBloqueoado.ValorBloqueo;
                                }
                            }
                        }
                        else
                        {
                            if (cuentas.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA")
                            {
                                estadoCuenta = cuentas.EstadoCuenta;
                                foreach (var conceptoEmbargado in cuentas.Embargos)
                                {
                                    totalEmbargosConceptos += conceptoEmbargado.Valor;
                                }
                                foreach (var conceptoBloqueoado in cuentas.ConceptosSaldosBloqueos)
                                {
                                    totalBloqueosConceptos += conceptoBloqueoado.ValorBloqueo;
                                }
                            }
                        }
                    }

                    foreach (var solicitudes in cliente.Solicitudes)
                    {

                        if ((solicitudes.TipoSolicitud.Contains("DESAFILIACION") && solicitudes.Modalidad.Contains("MUERTE DEL AFILIADO")) || (solicitudes.TipoSolicitud.Contains("PAGOS EXTRAORDINARIOS") && solicitudes.Modalidad.Equals("MUERTE DEL AFILIADO")))
                        {
                            validacionIdentidad.Mensaje = "Afiliado fallecido por favor acercarse a un punto de atención";
                            validacionIdentidad.Acceso = false;
                            validacionIdentidad.Caso = 10;
                            return new Response<ValidacionIdentidadAfiliado>
                            {
                                IsSuccess = true,
                                ReturnMessage = "Afiliado fallecido",
                                Data = validacionIdentidad
                            };
                        }

                        //if ((solicitudes.TipoSolicitud.Contains("Embargo") || solicitudes.TipoSolicitud.Contains("EMBARGO")) || solicitudes.TipoSolicitud.Contains("EMBARGOS") && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        //{
                        //    validacionIdentidad.Mensaje = "No es viable, afiliado presenta embargos en solicitudes";
                        //    validacionIdentidad.Acceso = false;
                        //    validacionIdentidad.Caso = 2;
                        //    return new Response<ValidacionIdentidadAfiliado>
                        //    {
                        //        IsSuccess = true,
                        //        ReturnMessage = "Bloqueo por embargos",
                        //        Data = validacionIdentidad
                        //    };
                        //}



                        if (vinculacion == 2 || vinculacion == 16)
                        {
                            if ((solicitudes.TipoSolicitud.Contains("SUBSIDIO (SEGUNDO PAGO)") && solicitudes.EstadoSolicitud.Equals("PROCESADA")))
                            {
                                validacionSegundoPago = true;
                            }
                        }


                        //if(vinculacion == 2 || vinculacion == 16)
                        //{
                        //    if ((solicitudes.TipoSolicitud.Contains("RETIRO PARCIAL DE CESANTIAS") || solicitudes.TipoSolicitud.Contains("DEVOLUCION PARCIAL CESANTIAS")) && solicitudes.EstadoSolicitud.Equals("PROCESADA"))
                        //    {
                        //        validacionIdentidad.Mensaje = "Afiliado realizó retiro parcial de censantias sin completar los 12 aportes";
                        //        validacionIdentidad.Acceso = false;
                        //        validacionIdentidad.Caso = 8;
                        //        return new Response<ValidacionIdentidadAfiliado>
                        //        {
                        //            IsSuccess = true,
                        //            ReturnMessage = "Solicitudes en estados procesada de retiro o devolución parcial de cesantías ",
                        //            Data = validacionIdentidad
                        //        };
                        //    }
                        //}

                    }

                    foreach (var embargos in cliente.EmbargosAfiliado)
                    {
                        if (embargos.ESTADO_BLOQUEO == "VIGENTE")
                        {
                            validacionIdentidad.Mensaje = "No es viable, afiliado presenta embargos";
                            validacionIdentidad.Acceso = false;
                            validacionIdentidad.Caso = 13;
                            return new Response<ValidacionIdentidadAfiliado>
                            {
                                IsSuccess = true,
                                ReturnMessage = "Bloqueo por conceptos",
                                Data = validacionIdentidad
                            };
                        }
                    }



                    if (totalEmbargosConceptos > 0)
                    {
                        validacionIdentidad.Mensaje = "No es viable, afiliado presenta embargos";
                        validacionIdentidad.Acceso = false;
                        validacionIdentidad.Caso = 2;
                        return new Response<ValidacionIdentidadAfiliado>
                        {
                            IsSuccess = true,
                            ReturnMessage = "Bloqueo por embargos",
                            Data = validacionIdentidad
                        };
                    }

                    if (totalBloqueosConceptos > 0)
                    {
                        validacionIdentidad.Mensaje = "No es viable, afiliado presenta bloqueo";
                        validacionIdentidad.Acceso = false;
                        validacionIdentidad.Caso = 12;
                        return new Response<ValidacionIdentidadAfiliado>
                        {
                            IsSuccess = true,
                            ReturnMessage = "Bloqueo por conceptos",
                            Data = validacionIdentidad
                        };
                    }


                    //no
                    if ((estadoCuenta == "CANCELADA" || estadoCuenta == "BLOQUEADA") && !pasoAValidacionAfilicionVeterano)
                    {
                        validacionIdentidad.Mensaje = "No es viable, afiliado presenta cuenta " + estadoCuenta;
                        validacionIdentidad.Acceso = false;
                        validacionIdentidad.Caso = 3;
                        return new Response<ValidacionIdentidadAfiliado>
                        {
                            IsSuccess = true,
                            ReturnMessage = "Bloqueo por estado de cuenta no vigente",
                            Data = validacionIdentidad
                        };
                    }

                    //no
                    if (cliente.NumeroCuotas <= 11 && vinculacion != 14 && vinculacion != 16 && vinculacion != 2 && !pasoAValidacionAfilicionVeterano && !validacionSegundoPago)
                    {
                        validacionIdentidad.Mensaje = "NO CUMPLE CON LAS CUOTAS APORTADAS\r\n\r\nSe requieren mínimo 12 cuotas aportadas";
                        validacionIdentidad.Acceso = false;
                        validacionIdentidad.Caso = 4;
                        return new Response<ValidacionIdentidadAfiliado>
                        {
                            IsSuccess = true,
                            ReturnMessage = "Bloqueo por cantidad de cuotas aportadas",
                            Data = validacionIdentidad
                        };
                    }
                }

                ValidacionOTPTemporalDto validacionOTP = new ValidacionOTPTemporalDto();
                validacionOTP.Celular = cliente.Telefono;
                validacionOTP.Identificacion = datos.Identificacion;


                var validacionOTPTemporal = await _creditoRepository
                    .ConsultarValidacionOTPTemporal(validacionOTP);

                if (validacionOTPTemporal != null)
                {
                    // Validar si ya pasaron 24 horas desde la fecha de bloqueo
                    if (validacionOTPTemporal.VOT_FECHA.AddHours(24) <= DateTime.Now)
                    {
                        await _creditoRepository.EliminarValidacionOTPTemporal(validacionOTP);
                    }
                    else if (!validacionOTPTemporal.VOT_ESTADO)
                    {
                        validacionOTPTemporal.BloqueoDia = true;
                        validacionIdentidad.Mensaje =
                            "Bloqueo Intentos Fallido - Has excedido el número de reintentos Código OTP. Afiliado bloqueado por 24 horas";
                        validacionIdentidad.Acceso = false;
                        validacionIdentidad.Caso = 5;

                        return new Response<ValidacionIdentidadAfiliado>
                        {
                            IsSuccess = true,
                            ReturnMessage = "Bloqueo de OTP por 24 horas",
                            Data = validacionIdentidad
                        };
                    }
                }



                validacionIdentidad.Mensaje = "Afiliado supera todos los filtros";
                validacionIdentidad.Acceso = true;
                validacionIdentidad.Caso = 0;
                // Si todo está bien, retornamos la data
                return new Response<ValidacionIdentidadAfiliado>
                {
                    IsSuccess = true,
                    ReturnMessage = "Sin excepciones",
                    Data = validacionIdentidad
                };
            }
            catch (Exception ex)
            {
                return new Response<ValidacionIdentidadAfiliado>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error, al validar el servicio que trae la información del afiliado: {ex.Message}"
                };
            }
        }


        public async Task<Response<CompraCarteraCredito>> InsertarValoresCompraCarteraCredito(CompraCarteraCreditoDto datos)
        {
            try
            {

                CompraCarteraCredito compraCartera = new CompraCarteraCredito();

                compraCartera.SLS_ID = datos.SLS_ID;
                compraCartera.CCC_CUOTA = datos.Cuota;
                compraCartera.CCC_VALOR_COMPRA_CARTERA = datos.ValorCompraCartera;
                compraCartera.CCC_PLAZO = datos.Plazo;
                compraCartera.CCC_NUMERO_OBLIGACION_CARTERA = datos.NumeroObligacionCompraCartera;
                compraCartera.CCC_VALOR_FINAL_PRESTAMO_CARTERA = datos.ValorCompraCartera + (datos.Cuota * 2);

                CompraCarteraCredito valoresCompraCartera = await _creditoRepository.InsertarValoresCompraCartera(compraCartera);

                return new Response<CompraCarteraCredito>
                {
                    IsSuccess = true,
                    ReturnMessage = "Información compra cartera insertado con exito!!",
                    Data = valoresCompraCartera
                };
            }
            catch (Exception ex)
            {
                return new Response<CompraCarteraCredito>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en validación de OTP: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Obtiene solo el saldo actual (sin histórico) - más rápido
        /// </summary>
        public async Task<Response<List<CausacionSaldoActualDto>>> ObtenerSaldoActualAsync(
            long numeroCredito,
            int idTipoCredito)
        {
            try
            {
                if (numeroCredito <= 0)
                    throw new ArgumentException("Número de crédito debe ser positivo", nameof(numeroCredito));

                var saldoActual = await _creditoRepository.ObtenerSaldoActualCreditoAsync(
                    numeroCredito, idTipoCredito);

                if (saldoActual == null)
                {
                    throw new KeyNotFoundException($"Crédito {numeroCredito} no tiene saldo actual registrado");
                }
                var saldoActualDto = await MapearSaldoActualDto(saldoActual, numeroCredito, idTipoCredito);

                return new Response<List<CausacionSaldoActualDto>> { Data = saldoActualDto };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response<List<CausacionHistoricoDto>>> ObtenerHistorialCausacionInteresCorrienteAsync(
            long numeroCredito, int idTipoCredito, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                var result = await _creditoRepository.ObtenerHistorialCausacionInteresCorrienteAsync(
                numeroCredito, idTipoCredito, fechaInicial, fechaFinal);
                if (result.Count == 0)
                {
                    return new Response<List<CausacionHistoricoDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se encontro informacion" };
                }
                var responseHitorico = MapearHistoricoDto(result, numeroCredito, idTipoCredito);

                return new Response<List<CausacionHistoricoDto>> { Data = responseHitorico, IsSuccess = true, ReturnMessage = "Process completed" };
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private async Task<List<CausacionSaldoActualDto>> MapearSaldoActualDto(
    List<CausacionSaldoActual> saldoActual,
    long numeroCredito,
    int idTipoCredito)
        {
            var interesRemamente = 0m;
            ProductoClienteDto Datacredito = null;
            var request = new ObtenerProductoCartera
            {
                Nro_Credito = numeroCredito,
                Tcr_Id = idTipoCredito
            };
            var obtenerCredito = await _carteraBusinessLogic.Value.ObtenerCreditoCartera(request);
            var requetsProduct = new SolicitudProductoPorClienteDto
            {
                Identificacion = obtenerCredito.Data.NumeroIdentificacion,
                TipoIdentificacion = 1
            };
            var credito = await _carteraBusinessLogic.Value.ObtenerProductoCliente(requetsProduct);
            var requetPlangoPago = new ObtenerProductoCartera
            {
                Nro_Credito = numeroCredito,
                Tcr_Id = idTipoCredito
            };
            Datacredito = credito.Data.Where(x => x.NumeroCredito == numeroCredito).FirstOrDefault();

            var planPago = await _carteraBusinessLogic.Value.ObtenerPlanPagosCredito(requetPlangoPago);

            var novedad = await _carteraRepository.ObtenerNovedadesCartera(Datacredito.IdCredito);
            var esReestructurado = novedad != null && novedad.Any(n => n.TNV_ID == 3 && n.ENC_ID == 5);

            // Mes/año de causación que se está procesando (ej. diciembre 2025).
            var fechaCausacion = saldoActual.FirstOrDefault()?.FechaCausacion ?? DateTime.Now;
            var primerDiaMesCausacion = new DateTime(fechaCausacion.Year, fechaCausacion.Month, 1);
            var ultimoDiaMesCausacion = primerDiaMesCausacion.AddMonths(1).AddDays(-1);

            if (idTipoCredito == 1 && !esReestructurado)
            {
                var cuotasHastaCorte = planPago.Data.PlanPago
                    .Where(x => x.Fecha < primerDiaMesCausacion)
                    .ToList();
                interesRemamente = cuotasHastaCorte.Sum(x => x.InteresPorPagar);
            }

            // Metodo el que obtiene el historial de pago (TODO el histórico del crédito)
            var listPayment = await _carteraRepository.ObtenerAplicacionPago(Datacredito.IdCredito);

            // Pagos válidos (no reversados) y SOLO del mes de causación que se está
            // procesando -- ObtenerAplicacionPago trae todo el historial, hay que
            // filtrar para no arrastrar pagos de otros meses.
            var pagosDelMes = listPayment
                .Where(p => (p.CC_ESTADO_ID == 1 || p.CC_ESTADO_ID == 2))
                .Where(p => p.CPG_FECHA_PAGO.Date >= primerDiaMesCausacion
                         && p.CPG_FECHA_PAGO.Date <= ultimoDiaMesCausacion)
                .ToList();

            var pagosPorFecha = pagosDelMes
                .GroupBy(p => p.CPG_FECHA_PAGO.Date)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.CPG_VALOR_PAGO - p.CPG_ABONO_INTERES_MORA));

            var resultado = new List<CausacionSaldoActualDto>();
            decimal? saldoAcumulado = null;
            bool pagoAplicado = false;
            // >>> FILA DE MORA: solo si el crédito está en mora (ESC_ID == 2). Es una
            // fila 100% informativa que se inserta PRIMERO en el resultado, armada
            // enteramente con datos del plan de pago (no participa en la cascada ni
            // en el cálculo normal de los días de saldoActual).
            // - SaldoInicial = SaldoCapital de la cuota ANTERIOR a la impaga (ej. oct) + interés acumulado hasta esa fecha.
            // - Interés/seguros/SaldoFinal = los de la cuota IMPAGA (ej. noviembre).
            if (Datacredito.IdEstadoCredito == 2)
            {
                var mesCuotaImpaga = primerDiaMesCausacion.AddMonths(-1);   // ej. noviembre
                var mesCuotaAnterior = primerDiaMesCausacion.AddMonths(-2); // ej. octubre

                var cuotaImpaga = planPago.Data.PlanPago
                    .FirstOrDefault(c => c.Fecha.Year == mesCuotaImpaga.Year && c.Fecha.Month == mesCuotaImpaga.Month);

                var cuotaAnterior = planPago.Data.PlanPago
                    .FirstOrDefault(c => c.Fecha.Year == mesCuotaAnterior.Year && c.Fecha.Month == mesCuotaAnterior.Month);

                if (cuotaImpaga != null && cuotaAnterior != null)
                {
                    var interesHastaCuotaAnterior = planPago.Data.PlanPago
                        .Where(c => c.Fecha < mesCuotaImpaga)
                        .Sum(c => c.InteresPorPagar);

                    var interes = idTipoCredito == 1
                        ? Math.Round(cuotaImpaga.InteresReducido, 0)
                        : Math.Round(cuotaImpaga.Interes, 0);
                    var saldoInicialMora = Math.Round(cuotaAnterior.SaldoCapital + interesHastaCuotaAnterior, 0);
                    var totalCausacionMora = Math.Round(cuotaImpaga.InteresPorPagar + interes + cuotaImpaga.SeguroVida + cuotaImpaga.SeguroHogar, 2);
                    var saldoFinalMora = idTipoCredito == 1
                        ? Math.Round(saldoInicialMora + totalCausacionMora, 0)
                        : Math.Round(saldoInicialMora + totalCausacionMora, 0);

                    resultado.Add(new CausacionSaldoActualDto
                    {
                        TipoDato = "SALDO EN MORA",
                        NumeroCredito = numeroCredito,
                        FechaCausacion = cuotaImpaga.Fecha,
                        SaldoInicial = saldoInicialMora,
                        CausacionInteresCorriente = Math.Round(interes, 2),
                        CausacionInteresRemanente = Math.Round(cuotaImpaga.InteresPorPagar, 2),
                        CausacionSeguroVida = Math.Round(cuotaImpaga.SeguroVida, 2),
                        CausacionSeguroHogar = Math.Round(cuotaImpaga.SeguroHogar, 2),
                        TotalCausacionDiaria = totalCausacionMora,
                        AbonosPagosRealizados = 0,
                        SaldoFinal = saldoFinalMora
                    });
                    // : el saldoFinal de esta fila arranca la cascada.
                    saldoAcumulado = saldoFinalMora;
                    pagoAplicado = true;
                }
            }
            else
            {
                var mes = fechaCausacion.Month -1;
                var anio = fechaCausacion.Year;
                var creditoEnMora = await _carteraRepository.ObtenerCreditoEnMoraPeriodo(numeroCredito,mes,anio);
                if (creditoEnMora > 0)
                {
                    var mesCuotaImpaga = primerDiaMesCausacion.AddMonths(-1);   // ej. noviembre
                    var mesCuotaAnterior = primerDiaMesCausacion.AddMonths(-2); // ej. octubre

                    var cuotaImpaga = planPago.Data.PlanPago
                    .FirstOrDefault(c => c.Fecha.Year == mesCuotaImpaga.Year && c.Fecha.Month == mesCuotaImpaga.Month);

                    var cuotaAnterior = planPago.Data.PlanPago
                        .FirstOrDefault(c => c.Fecha.Year == mesCuotaAnterior.Year && c.Fecha.Month == mesCuotaAnterior.Month);

                    if (cuotaImpaga != null && cuotaAnterior != null)
                    {
                        var interesHastaCuotaAnterior = planPago.Data.PlanPago
                            .Where(c => c.Fecha < mesCuotaImpaga)
                            .Sum(c => c.InteresPorPagar);

                        var interes = idTipoCredito == 1
                            ? Math.Round(cuotaImpaga.InteresReducido, 0)
                            : Math.Round(cuotaImpaga.Interes, 0);
                        var saldoInicialMora = Math.Round(cuotaAnterior.SaldoCapital + interesHastaCuotaAnterior, 0);
                        var totalCausacionMora = Math.Round(cuotaImpaga.InteresPorPagar + interes + cuotaImpaga.SeguroVida + cuotaImpaga.SeguroHogar, 2);
                        var saldoFinalMora = idTipoCredito == 1
                            ? Math.Round(saldoInicialMora + totalCausacionMora, 0)
                            : Math.Round(saldoInicialMora + totalCausacionMora, 0);

                        resultado.Add(new CausacionSaldoActualDto
                        {
                            TipoDato = "SALDO EN MORA",
                            NumeroCredito = numeroCredito,
                            FechaCausacion = cuotaImpaga.Fecha,
                            SaldoInicial = saldoInicialMora,
                            CausacionInteresCorriente = Math.Round(interes, 2),
                            CausacionInteresRemanente = Math.Round(cuotaImpaga.InteresPorPagar, 2),
                            CausacionSeguroVida = Math.Round(cuotaImpaga.SeguroVida, 2),
                            CausacionSeguroHogar = Math.Round(cuotaImpaga.SeguroHogar, 2),
                            TotalCausacionDiaria = totalCausacionMora,
                            AbonosPagosRealizados = 0,
                            SaldoFinal = saldoFinalMora
                        });
                        // : el saldoFinal de esta fila arranca la cascada.
                        saldoAcumulado = saldoFinalMora;
                        pagoAplicado = true;
                    }
                }
            }
            
            // >>> Días normales de saldoActual (ej. 1 al 31 de diciembre). Esta parte
            // NO se ve afectada por la fila de mora de arriba -- arranca su propio

            foreach (var x in saldoActual.OrderBy(r => r.FechaCausacion))
            {
                decimal saldoInicial;

                if (pagoAplicado)
                {
                    // Modo cascada: ya hubo un pago en un día anterior de este mes.
                    saldoInicial = saldoAcumulado.Value;
                }
                else if (idTipoCredito == 1 && esReestructurado)
                {
                    var cuotaAnteriorCausacion = planPago.Data.PlanPago
                        .Where(c => c.Fecha < x.FechaCausacion)
                        .OrderByDescending(c => c.Fecha)
                        .FirstOrDefault();

                    var saldoCapitalBase = cuotaAnteriorCausacion != null
                        ? cuotaAnteriorCausacion.SaldoCapital
                        : x.SaldoCapital;

                    saldoInicial = Math.Round(saldoCapitalBase + interesRemamente, 2);
                }
                else if (idTipoCredito == 1)
                {
                    saldoInicial = Math.Round(x.SaldoCapital + interesRemamente, 2);
                }
                else
                {
                    saldoInicial = Math.Round(x.SaldoCapital, 2);
                }

                decimal saldoFinal;
                if (idTipoCredito == 1)
                {
                    if (pagoAplicado || esReestructurado)
                    {
                        saldoFinal = Math.Round(saldoInicial + x.TotalCausacionDiaria, 2);
                    }
                    else
                    {
                        saldoFinal = Math.Round(x.SaldoCapital + interesRemamente + x.TotalCausacionDiaria, 2);
                    }
                }
                else if (idTipoCredito == 2 && pagoAplicado)
                {

                    saldoFinal = Math.Round(saldoInicial + x.TotalCausacionDiaria, 2);
                }
                else
                {
                    saldoFinal = Math.Round(x.SaldoSinPagoCuota, 2);
                }

                pagosPorFecha.TryGetValue(x.FechaCausacion.Date, out var afectacionPago);
                var afectacionRedondeada = Math.Round(afectacionPago, 0);

                if (afectacionRedondeada != 0)
                {
                    saldoFinal -= afectacionRedondeada;
                    pagoAplicado = true;
                }

                saldoAcumulado = saldoFinal;

                resultado.Add(new CausacionSaldoActualDto
                {
                    TipoDato = x.TipoDato ?? "SALDO ACTUAL",
                    NumeroCredito = numeroCredito,
                    FechaCausacion = x.FechaCausacion,
                    SaldoInicial = saldoInicial,
                    CausacionInteresCorriente = Math.Round(x.CausacionInteresCorriente, 2),
                    CausacionInteresRemanente = Math.Round(x.CausacionInteresRemanente ?? 0, 2),
                    CausacionSeguroVida = Math.Round(x.CausacionSeguroVida, 2),
                    CausacionSeguroHogar = Math.Round(x.CausacionSeguroHogar, 2),
                    TotalCausacionDiaria = Math.Round(x.TotalCausacionDiaria, 2),
                    AbonosPagosRealizados = afectacionRedondeada,
                    SaldoFinal = saldoFinal
                });
            }

            return resultado;
        }
        /// <summary>
        /// Mapea resultado dinámico a CausacionHistoricoDto
        /// </summary>
        private List<CausacionHistoricoDto> MapearHistoricoDto(List<CausacionHistorico> historicoDinamico, long numeroCredito, int idTipoCredito)
        {
            try
            {
                return historicoDinamico.Select(x => new CausacionHistoricoDto
                {
                    TipoDato = x.TipoDato ?? "HISTORICO",
                    NumeroCredito = numeroCredito,
                    IdTipoCredito = idTipoCredito,
                    FechaCausacion = x.FechaCausacion,
                    CausacionInteresesCorrientes = x.CausacionInteresesCorrientes,
                    CausacionInteresesRemanente = x.CausacionInteresesRemanente,
                    CausacionSeguroVida = x.CausacionSeguroVida,
                    CausacionSeguroHogar = x.CausacionSeguroHogar,
                    AcumuladoInteresesCorrientes = x.AcumuladoInteresesCorrientes,
                    AcumuladoInteresesRemanente = x.AcumuladoInteresesRemanente,
                    AcumuladoSeguroVida = x.AcumuladoSeguroVida,
                    AcumuladoSeguroHogar = x.AcumuladoSeguroHogar,
                    ValorDeudaRemanente = x.ValorDeudaRemanente,
                    SaldoCapital = x.SaldoCapital,
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// ═════════════════════════════════════════════════════════════════════════════
        /// FUNCIONALIDAD 1: Eliminar Afiliado del Comprobante
        /// ═════════════════════════════════════════════════════════════════════════════
        public async Task<Response<ComprobanteCarteraOperacionDto>> EliminarAfiliadoDelComprobante(
            EliminarAfiliadoComprobanteDto request)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────
                // PASO 1: Validar parámetros
                // ─────────────────────────────────────────────────────────────────────
                if (request.CC_ID <= 0)
                {
                    return new Response<ComprobanteCarteraOperacionDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "CC_ID inválido"
                    };
                }

                if (request.Dmc_refe == null || request.Dmc_refe.Count == 0)
                {
                    return new Response<ComprobanteCarteraOperacionDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "Referencia de afiliado requerida"
                    };
                }


                //  Remover duplicados de la lista
                var dmcRefesUnicos = request.Dmc_refe.Distinct().ToList();
                // ─────────────────────────────────────────────────────────────────────
                // P Obtener comprobante con detalles
                // ─────────────────────────────────────────────────────────────────────
                var requestObtenerComprobante = new ObtenerComprobantePorProcesoDto { ComprobanteId = request.CC_ID.ToString() };
                var comprobante = await _carteraRepository.ObtenerComprobanteCartera(requestObtenerComprobante, 1);
                if (!comprobante.Any())
                {
                    return new Response<ComprobanteCarteraOperacionDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "El comprobante no tiene movimientos"
                    };
                }
                MovimientosContableParametersDto requiest = new MovimientosContableParametersDto
                {
                    IdComprobante = (Guid)comprobante.FirstOrDefault().Detalle_Movimiento_Id,
                    NumeroPagina = 500,
                    RegistrosPorPagina = 100
                };
                var detalletComprobante = this._mapper.Map<List<MovimientoComprobantePorProcesoDto>>(await _carteraRepository.ObtenerMovimientosContableCartera(requiest));
                // ─────────────────────────────────────────────────────────────────────
                // PASO 5: Validar que el afiliado exista en el comprobante
                // ─────────────────────────────────────────────────────────────────────
                var movimientosAfiliado = detalletComprobante;

                if (movimientosAfiliado.Count == 0)
                {
                    return new Response<ComprobanteCarteraOperacionDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = $"Afiliado {request.Dmc_refe} no existe en este comprobante"
                    };
                }

                // ─────────────────────────────────────────────────────────────────────
                // PASO 6: Eliminar movimientos del afiliado
                // ─────────────────────────────────────────────────────────────────────
                var afiliadosNoEncontrados = dmcRefesUnicos
                    .Where(ref_ => !movimientosAfiliado.Any(x => x.Numero_Identificacion == ref_))
                    .ToList();
                if (afiliadosNoEncontrados.Any())
                {
                    var afiliadosStr = string.Join(", ", afiliadosNoEncontrados);
                    return new Response<ComprobanteCarteraOperacionDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = $"Los siguientes afiliados no existen en este comprobante: {afiliadosStr}"
                    };
                }
                int movimientosAntes = movimientosAfiliado.Count;

                var movimientosRestantes = movimientosAfiliado
                    .Where(x => !dmcRefesUnicos.Contains(x.Numero_Identificacion))
                    .ToList();

                int movimientosEliminados = movimientosAntes - movimientosRestantes.Count;

                // ─────────────────────────────────────────────────────────────────────
                // PASO 7: Recalcular totales
                // ─────────────────────────────────────────────────────────────────────
                var nuevoTotalCredito = movimientosRestantes.Sum(x => x.Debito);
                var nuevoTotalDebito = movimientosRestantes.Sum(x => x.Credito);

                if (comprobante.FirstOrDefault().Tipo_Comprobante == "Cierre Diario Interes Moratorio")
                {
                    //  Agrupar movimientos eliminados por NÚMERO DE CRÉDITO
                    var movimientosEliminadosPorCredito = movimientosAfiliado
                        .Where(x => dmcRefesUnicos.Contains(x.Numero_Identificacion))
                        .GroupBy(x => x.Numero_Credito)
                        .ToList();

                    //  Para cada crédito, restar su causación y 1 día
                    foreach (var grupoCredito in movimientosEliminadosPorCredito)
                    {
                        int tipoCredito = movimientosAfiliado.FirstOrDefault().Detalle?.Contains("leasing") == true ? 1 :
                                           movimientosAfiliado.FirstOrDefault().Detalle?.Contains("Hipotecario") == true ? 2 : 0;

                        string numeroCredito = grupoCredito.Key;
                        decimal valorCausacionDelCredito = grupoCredito.Sum(x => x.Debito);
                        var requestConultaCartera = new ObtenerProductoCartera
                        {
                            Nro_Credito = long.Parse(numeroCredito),
                            Tcr_Id = tipoCredito
                        };
                        var creditoActual = await _carteraRepository.ObtenerCreditoCartera(requestConultaCartera);

                        decimal nuevoAcumuladoMora = creditoActual.CRE_ACUMULADO_INTERES_MORA - valorCausacionDelCredito;
                        int nuevosDiasMora = creditoActual.CRE_DIAS_MORA - 1;

                        await _carteraRepository.ActualizarMoraDelCredito(
                            creditoActual.CRE_ID,
                            nuevoAcumuladoMora,
                            nuevosDiasMora);
                    }
                }
                // ─────────────────────────────────────────────────────────────────────
                // PASO 8: Serializar nuevo JSON
                // ─────────────────────────────────────────────────────────────────────
                var movimientosFormatoOriginal = movimientosRestantes.Select(m => new
                {
                    Cue_codi = m.Cuenta_Contable,
                    Dmc_desc = m.Nombre_Tercero,
                    Dmc_cant = 1,
                    Dmc_acti = 0,
                    Dmc_refe = m.Numero_Credito,
                    Dmc_vadb = m.Debito,                   // Valor débito
                    Dmc_vacr = m.Credito,
                    Dmc_vaba = 0,
                    Ter_coda = m.Numero_Identificacion,
                    Arb_codc = m.Centro_Costo_Credito ?? "601102",     // Del DTO o valor por defecto
                    Arb_coda = m.Centro_Costo,             // Centro costo
                    Arb_codp = "099",        // Presupuestal (del DTO o fijo)
                    Arb_cods = "1001",       // Sucursal (del DTO o fijo)
                    Ter_codm = "0"           // Tercero módulo (del DTO o fijo)
                }).ToList();

                var nuevoJson = System.Text.Json.JsonSerializer.Serialize(movimientosFormatoOriginal);

                // ─────────────────────────────────────────────────────────────────────
                // PASO 9: Actualizar en BD
                // ─────────────────────────────────────────────────────────────────────
                bool actualizado = await _carteraRepository.ActualizarComprobanteCarteraEliminarAfiliado(
                    request.CC_ID,
                    nuevoTotalCredito,
                    nuevoTotalDebito,
                    nuevoJson);


                int cantidadMovimientosReales = movimientosEliminados;
                int cantidadAfiliadosRestantes = movimientosRestantes
                    .Select(x => x.Numero_Identificacion)
                    .Distinct()
                    .Count();
                // ─────────────────────────────────────────────────────────────────────
                // PASO 10: Preparar respuesta
                // ─────────────────────────────────────────────────────────────────────
                var respuesta = new ComprobanteCarteraOperacionDto
                {
                    CC_ID = request.CC_ID,
                    TipoOperacion = "ELIMINAR_AFILIADO",
                    TOTAL_COMPROMBANTE_CREDITO = nuevoTotalCredito,
                    TOTAL_COMPROMBANTE_DEBITO = nuevoTotalDebito,
                    CantidadMovimientos = cantidadMovimientosReales,
                    CantidadAfiliados = cantidadAfiliadosRestantes,
                    Estado = "COMPLETADO",
                    FechaOperacion = DateTime.Now,
                    MovimientosResultado = nuevoJson
                };


                return new Response<ComprobanteCarteraOperacionDto>
                {
                    Data = respuesta,
                    IsSuccess = true,
                    ReturnMessage = $" Afiliado eliminado exitosamente. " +
                                   $"Movimientos removidos: {movimientosAfiliado.Count}"
                };
            }
            catch (Exception ex)
            {
                return new Response<ComprobanteCarteraOperacionDto>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error: {ex.Message}"
                };
            }
        }
        public async Task<Response<CompraCarteraCredito>> ConsultarValoresCompraCarteraCredito(CompraCarteraCreditoDto datos)
        {
            try
            {

                CompraCarteraCredito compraCartera = new CompraCarteraCredito();

                compraCartera.SLS_ID = datos.SLS_ID;

                CompraCarteraCredito valoresCompraCartera = await _creditoRepository.ConsultarValoresCompraCartera(compraCartera);

                if (valoresCompraCartera == null)
                {
                    return new Response<CompraCarteraCredito>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontró información de compra de cartera",
                        Data = null
                    };
                }

                return new Response<CompraCarteraCredito>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta exitosa",
                    Data = valoresCompraCartera
                };
            }
            catch (Exception ex)
            {
                return new Response<CompraCarteraCredito>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en validación de OTP: {ex.Message}"
                };
            }
        }
        #region

        /// <summary>
        /// Obtiene el valor de una propiedad de un objeto dinámico o fuertemente tipado
        /// Usa reflexión para soportar ambos casos
        /// </summary>
        private object GetPropertyValue(dynamic obj, string propertyName)
        {
            try
            {
                if (obj == null) return null;

                var objType = ((object)obj).GetType();

                // Intentar búsqueda insensible a mayúsculas
                var property = objType.GetProperty(propertyName,
                    System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (property != null && property.CanRead)
                {
                    return property.GetValue(obj);
                }

                // Si es un diccionario (IDictionary), intentar acceder como clave
                if (obj is System.Collections.IDictionary dict)
                {
                    if (dict.Contains(propertyName))
                        return dict[propertyName];

                    var lowerKey = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                    if (dict.Contains(lowerKey))
                        return dict[lowerKey];
                }

                // Intentar acceso dinámico como último recurso
                try
                {
                    return ((dynamic)obj).propertyName;
                }
                catch
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo propiedad {propertyName}: {ex.Message}");
                return null;
            }
        }

        private decimal ConvertToDecimal(dynamic value)
        {
            if (value == null) return 0m;
            if (value is decimal d) return d;
            if (value is int i) return (decimal)i;
            if (value is long l) return (decimal)l;
            if (value is double dbl) return (decimal)dbl;
            if (value is float f) return (decimal)f;
            if (decimal.TryParse(value.ToString(), out decimal result)) return result;
            return 0m;
        }

        private decimal? ConvertToDecimalNullable(dynamic value)
        {
            if (value == null) return null;
            if (value is decimal d) return d;
            if (value is int i) return (decimal)i;
            if (value is long l) return (decimal)l;
            if (value is double dbl) return (decimal)dbl;
            if (value is float f) return (decimal)f;
            if (decimal.TryParse(value.ToString(), out decimal result)) return result;
            return null;
        }

        private int ConvertToInt(dynamic value)
        {
            if (value == null) return 0;
            if (value is int i) return i;
            if (value is long l) return (int)l;
            if (int.TryParse(value.ToString(), out int result)) return result;
            return 0;
        }

        private DateTime ConvertToDateTime(dynamic value)
        {
            if (value == null) return DateTime.Now;
            if (value is DateTime dt) return dt;
            if (DateTime.TryParse(value.ToString(), out DateTime result)) return result;
            return DateTime.Now;
        }

        private DateTime? ConvertToDateTimeNullable(dynamic value)
        {
            if (value == null) return null;
            if (value is DateTime dt) return dt;
            if (DateTime.TryParse(value.ToString(), out DateTime result)) return result;
            return null;
        }
        #endregion

        private async Task<bool> EnviarCorreoAsesor(int AFL_ID)
        {

            //ANÁLISIS TÉCNICO CH
            if (AFL_ID == 3)
            {
                var email = new Email
                {
                    Modulo = "credito",

                    RutaHtml = "TemplateRechazoCredito.html",

                    RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                    Asunto = "Comunicado Oficial - Rechazo",

                    Destinatarios = new List<string>
                {
                    "analisis.procesos@grupokriterion.com.co"
                },

                    Parametros = new Dictionary<string, string>
                {
                    { "NombreCompleto", "" }

                }
                };

                await _emailNotificacionRepository.EnviarAsync(email);
            }

            return true;
        }


        public async Task<Response<ResponseDocumentoSimDto>> CargarCartaRechazoProspeccionDODO(RequestDocumentoSimDto requestDocumentoSolicCredito)
        {
            var attachRequestDto = new AttachRequestDto
            {
                UserOperation = "web",
                Files = new List<FileDto>()
            };

            string extension = string.Empty;

            // Obtener información de simulación
            SolicitudSimulacion informacionSimulacion =
                await _creditoRepository.ObtenerSolicitudSimulacionSLS(requestDocumentoSolicCredito.idSimulacion);


            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(1, requestDocumentoSolicCredito.NumeroDocumento)).Data;


            DocumentosAdicionalesCargados documentosAdicionales = await _reportesRepository.ConsultarDocumentosAdicionalesCargados(requestDocumentoSolicCredito.NumeroDocumento);

            DocumentosActRespuestaDto respuestaDto = new DocumentosActRespuestaDto();

            if (documentosAdicionales != null)
            {

                respuestaDto.RUTA_IMAGEN = documentosAdicionales.DSA_RUTA;
                respuestaDto.TDC_ID = 173;
                respuestaDto.TIPO_DOCUMENTO = "Comunicado Oficial - Rechazo";

            }



            attachRequestDto.Files = new List<FileDto>();

            try
            {
                string filePath = respuestaDto.RUTA_IMAGEN;
                var fileExtensions = new List<string> { "pdf", "tif" };
                int folios = 0;
                string archivoBase64 = string.Empty;
                byte[] contenidoBytes;

                ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

                // -------------------------------
                // Obtención archivo desde Storage
                // -------------------------------
                if (blogStorage.SRD_ESTADO)
                {
                    var bdinfo = await _blobStorageGenericRepository
                                    .DescargarArchivoStorage("reportes-sfc", respuestaDto.RUTA_IMAGEN);

                    extension = filePath.Split(".")[1];

                    if (fileExtensions.Contains(extension))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await bdinfo.Content.CopyToAsync(memoryStream);
                            contenidoBytes = memoryStream.ToArray();
                        }

                        string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                        folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                        archivoBase64 = Convert.ToBase64String(contenidoBytes);
                    }
                    else
                    {
                        folios = 0;
                    }
                }
                else
                {
                    var bdinfoLocal = await _localBlogStorageRepository
                                       .DescargarArchivoLocalRechazo("reportes-sfc", respuestaDto.RUTA_IMAGEN);

                    if (bdinfoLocal.IsSuccess && bdinfoLocal.Data != null)
                    {
                        contenidoBytes = bdinfoLocal.Data;
                        extension = Path.GetExtension(respuestaDto.RUTA_IMAGEN)
                                        .Replace(".", "")
                                        .ToLower();

                        if (fileExtensions.Contains(extension))
                        {
                            archivoBase64 = Convert.ToBase64String(contenidoBytes);

                            string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;
                        }
                    }
                    else
                    {
                        folios = 0;
                        archivoBase64 = string.Empty;
                    }
                }

                // -------------------------------
                // Crear documento
                // -------------------------------

                var documento = new ResponseDocumentoSimulacion
                {
                    DSC_ID = Guid.NewGuid(),
                    SLS_ID = requestDocumentoSolicCredito.idSimulacion,
                    TDC_ID = requestDocumentoSolicCredito.idTipoDocumento,
                    DSC_CODIGO_BARRAS = requestDocumentoSolicCredito.codigoBarras,
                    DSC_FECHA_DOCUMENTO = DateTime.Now,
                    DSC_NUMERO_FOLIOS = blogStorage.SRD_ESTADO ? folios : 0,
                    DSC_TAMAÑO = 0,
                    DSC_RUTA_IMAGEN = requestDocumentoSolicCredito.rutaImagen,
                    DSC_ESTADO = "1",
                    DSC_CREADO_POR = requestDocumentoSolicCredito.creadoPor,
                    DSC_FECHA_CREACION = DateTime.Now
                };

                var email = new Email
                {
                    Modulo = "credito",

                    RutaHtml = "TemplateRechazoCredito.html",

                    RutaImagenEncabezado = "encabezadoRechazoCredito.png",

                    Asunto = "Comunicado Oficial - Rechazo",

                    Destinatarios = new List<string>
                {
                    "analisis.procesos@grupokriterion.com.co"
                },

                    Parametros = new Dictionary<string, string>
                {
                    { "NombreCompleto", $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" }

                },
                    Adjuntos = new List<string>
                    {
                        $"C:\\inetpub\\Deployed\\LocalBlobStorage\\reportes-sfc\\{respuestaDto.RUTA_IMAGEN}",
                    },
                };

                await _emailNotificacionRepository.EnviarAsync(email);


                DocumentoCargadoDodo dod_dodo = new DocumentoCargadoDodo();

                dod_dodo.DCD_CARGADO = true;
                dod_dodo.DCD_DESCRIPCION = "Comunicado Oficial - Rechazo";
                dod_dodo.RUTA = documentosAdicionales.DSA_RUTA;
                dod_dodo.SLS_ID = requestDocumentoSolicCredito.idSimulacion;
                dod_dodo.TDC_ID = 173;

                // -------------------------------
                // Adjuntar al gestor documental
                // -------------------------------
                if (informacionSimulacion != null)
                {
                    attachRequestDto.Radicado = informacionSimulacion.SLS_NUMERO_RADICADO_83;

                    var infoTipoDocument =
                        ObtenerTipoDocumentoPorId(respuestaDto.TDC_ID.ToString()).Result.Data;


                    attachRequestDto.Files.Add(
                        CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, extension, true)
                    );

                    DocumentoCargadoDodo documentoCargado = await _creditoRepository.ConsultaDocumentoCargadoDodo(requestDocumentoSolicCredito.idSimulacion, "Comunicado Oficial - Rechazo");

                    if (documentoCargado == null || !documentoCargado.DCD_CARGUE_83)
                    {
                        dod_dodo.DCD_CARGUE_83 = true;

                        await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                        await _creditoRepository.InsertarDocumentoCargadoDodo(dod_dodo);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response<ResponseDocumentoSimDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Error creando documento en la BD, error: " + ex.Message
                };
            }

            return new Response<ResponseDocumentoSimDto>
            {
                Data = null,
                IsSuccess = false,
                ReturnMessage = "Todo cargado en DODO"
            };
        }


        public async Task<Response<IEnumerable<InformacionPerito>>> ConsultarSolicitudesTramitePerito(string usuarioPerito)
        {
            var methodName = nameof(ConsultarSolicitudesTramitePerito);

            try
            {
                #region VALIDACIONES

                if (string.IsNullOrWhiteSpace(usuarioPerito))
                {
                    return new Response<IEnumerable<InformacionPerito>>
                    {
                        IsSuccess = false,
                        ReturnMessage = "El usuario del perito es obligatorio."
                    };
                }

                #endregion

                #region REPOSITORY

                var informacion = await _creditoRepository.ConsultarSolicitudesTramitePerito(usuarioPerito);

                #endregion

                #region VALIDACION_RESPUESTA

                if (informacion == null || !informacion.Any())
                {
                    return new Response<IEnumerable<InformacionPerito>>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontraron solicitudes para el usuario indicado."
                    };
                }

                #endregion

                #region RESPUESTA

                return new Response<IEnumerable<InformacionPerito>>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta realizada correctamente.",
                    Data = informacion
                };

                #endregion
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<InformacionPerito>>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en {methodName}: {ex.Message}"
                };
            }
        }

        public async Task<Response<IEnumerable<AliadosEstrategicos>>> ConsultarAliadosEstrategicos()
        {
            var methodName = nameof(ConsultarAliadosEstrategicos);

            try
            {
                #region REPOSITORY

                var informacion = await _creditoRepository.ConsultarAliadosEstrategicos();

                #endregion

                #region VALIDACION_RESPUESTA

                if (informacion == null || !informacion.Any())
                {
                    return new Response<IEnumerable<AliadosEstrategicos>>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No se encontraron aliados estratégicos."
                    };
                }

                #endregion

                #region RESPUESTA

                return new Response<IEnumerable<AliadosEstrategicos>>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta realizada correctamente.",
                    Data = informacion
                };

                #endregion
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<AliadosEstrategicos>>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en {methodName}: {ex.Message}"
                };
            }
        }

        public async Task<Response<AvaluoComercialCreditoDto>> InsertarActualizarAvaluoComercialCredito(AvaluoComercialCreditoDto request)
        {
            Response<AvaluoComercialCreditoDto> response = new();

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La información del avalúo es obligatoria.";
                    return response;
                }

                if (request.SocId == Guid.Empty)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La solicitud de crédito es obligatoria.";
                    return response;
                }

                var entity = new AvaluoComercialCredito
                {
                    SOC_ID = request.SocId,

                    AVC_CLIENTE = request.ResumenEjecutivo.Cliente,
                    AVC_NUMERO_DOCUMENTO = request.ResumenEjecutivo.NumeroDocumento,
                    AVC_DIRECCION = request.ResumenEjecutivo.Direccion,
                    AVC_SECTOR = request.ResumenEjecutivo.Sector,
                    AVC_BARRIO = request.ResumenEjecutivo.Barrio,
                    AVC_ID_DPD = request.ResumenEjecutivo.DepartamentoId,
                    AVC_ID_DPC = request.ResumenEjecutivo.CiudadId,

                    AVC_FECHA_INFORME = request.Antecedentes.FechaInforme,
                    AVC_VETUSTEZ = request.Antecedentes.Vetustez,
                    AVC_FECHA_PAGO_AVALUO = request.Antecedentes.FechaPagoAvaluo,
                    AVC_FECHA_VISITA = request.Antecedentes.FechaVisita,

                    AVC_PROPIETARIO = request.AspectosJuridicos.Propietario,
                    AVC_ESCRITURA = request.AspectosJuridicos.Escritura,
                    AVC_NOTARIA = request.AspectosJuridicos.Notaria,
                    AVC_FECHA_ESCRITURA = request.AspectosJuridicos.FechaEscritura,
                    AVC_DPD_ID_ESCRITURA = request.AspectosJuridicos.DepartamentoEscrituraId,
                    AVC_DPC_ID_ESCRITURA = request.AspectosJuridicos.CiudadEscrituraId,
                    AVC_CEDULA_CATASTRAL = request.AspectosJuridicos.CedulaCatastral,
                    AVC_LICENCIA_CONSTRUCCION = request.AspectosJuridicos.LicenciaConstruccion,
                    AVC_NOMBRE_CONJUNTO = request.AspectosJuridicos.NombreConjunto,
                    AVC_COEFICIENTE_COPROPIEDAD = request.AspectosJuridicos.CoeficienteCopropiedad,

                    AVC_DISTRIBUCION_INMUEBLE = request.ObservacionesGenerales.DistribucionInmueble,
                    AVC_CARACTERISTICAS_ADICIONALES = request.ObservacionesGenerales.CaracteristicasAdicionales,

                    AVC_DESCRIPCION_DEPENDENCIA = request.Dependencias.Descripcion,
                    AVC_GARAJE = request.Dependencias.Garaje,
                    AVC_DESCRIPCION_GARAJE = request.Dependencias.DescripcionGaraje,

                    AVC_SALA_AREA_CONSTRUIDA = request.Construccion.SalaAreaConstruida,
                    AVC_AREA_PRIVADA = request.Construccion.AreaPrivada,

                    AVC_CUADRO_VALORACION_INMUEBLE = request.CuadroValoracion?.Inmueble
                };

                var resultado = await _creditoRepository.InsertarActualizarAvaluoComercialCredito(entity);

                if (resultado == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No fue posible almacenar la información.";
                    return response;
                }

                response.Data = new AvaluoComercialCreditoDto
                {
                    SocId = resultado.SOC_ID,

                    ResumenEjecutivo = new ResumenEjecutivoDto
                    {
                        Cliente = resultado.AVC_CLIENTE,
                        NumeroDocumento = resultado.AVC_NUMERO_DOCUMENTO,
                        Direccion = resultado.AVC_DIRECCION,
                        Sector = resultado.AVC_SECTOR,
                        Barrio = resultado.AVC_BARRIO,
                        DepartamentoId = resultado.AVC_ID_DPD,
                        CiudadId = resultado.AVC_ID_DPC
                    },

                    Antecedentes = new AntecedentesDto
                    {
                        FechaInforme = resultado.AVC_FECHA_INFORME,
                        Vetustez = resultado.AVC_VETUSTEZ,
                        FechaPagoAvaluo = resultado.AVC_FECHA_PAGO_AVALUO,
                        FechaVisita = resultado.AVC_FECHA_VISITA
                    },

                    AspectosJuridicos = new AspectosJuridicosDto
                    {
                        Propietario = resultado.AVC_PROPIETARIO,
                        Escritura = resultado.AVC_ESCRITURA,
                        Notaria = resultado.AVC_NOTARIA,
                        FechaEscritura = resultado.AVC_FECHA_ESCRITURA,
                        DepartamentoEscrituraId = resultado.AVC_DPD_ID_ESCRITURA,
                        CiudadEscrituraId = resultado.AVC_DPC_ID_ESCRITURA,
                        CedulaCatastral = resultado.AVC_CEDULA_CATASTRAL,
                        LicenciaConstruccion = resultado.AVC_LICENCIA_CONSTRUCCION,
                        NombreConjunto = resultado.AVC_NOMBRE_CONJUNTO,
                        CoeficienteCopropiedad = resultado.AVC_COEFICIENTE_COPROPIEDAD
                    },

                    ObservacionesGenerales = new ObservacionesGeneralesDto
                    {
                        DistribucionInmueble = resultado.AVC_DISTRIBUCION_INMUEBLE,
                        CaracteristicasAdicionales = resultado.AVC_CARACTERISTICAS_ADICIONALES
                    },

                    Dependencias = new DependenciasDto
                    {
                        Descripcion = resultado.AVC_DESCRIPCION_DEPENDENCIA,
                        Garaje = resultado.AVC_GARAJE,
                        DescripcionGaraje = resultado.AVC_DESCRIPCION_GARAJE
                    },

                    Construccion = new ConstruccionDto
                    {
                        SalaAreaConstruida = resultado.AVC_SALA_AREA_CONSTRUIDA,
                        AreaPrivada = resultado.AVC_AREA_PRIVADA
                    },

                    CuadroValoracion = new CuadroValoracionDto
                    {
                        Inmueble = resultado.AVC_CUADRO_VALORACION_INMUEBLE
                    }
                };

                response.IsSuccess = true;
                response.ReturnMessage = "La información fue almacenada correctamente.";

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
                return response;
            }
        }

        public async Task<Response<AvaluoComercialCreditoDto>> ConsultarAvaluoComercialCredito(Guid socId)
        {
            Response<AvaluoComercialCreditoDto> response = new();

            try
            {
                var entity = await _creditoRepository.ConsultarAvaluoComercialCredito(socId);

                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No se encontró información del avalúo comercial.";
                    return response;
                }

                response.Data = new AvaluoComercialCreditoDto
                {
                    SocId = entity.SOC_ID,

                    ResumenEjecutivo = new ResumenEjecutivoDto
                    {
                        Cliente = entity.AVC_CLIENTE,
                        NumeroDocumento = entity.AVC_NUMERO_DOCUMENTO,
                        Direccion = entity.AVC_DIRECCION,
                        Sector = entity.AVC_SECTOR,
                        Barrio = entity.AVC_BARRIO,
                        DepartamentoId = entity.AVC_ID_DPD,
                        CiudadId = entity.AVC_ID_DPC
                    },

                    Antecedentes = new AntecedentesDto
                    {
                        FechaInforme = entity.AVC_FECHA_INFORME,
                        Vetustez = entity.AVC_VETUSTEZ,
                        FechaPagoAvaluo = entity.AVC_FECHA_PAGO_AVALUO,
                        FechaVisita = entity.AVC_FECHA_VISITA
                    },

                    AspectosJuridicos = new AspectosJuridicosDto
                    {
                        Propietario = entity.AVC_PROPIETARIO,
                        Escritura = entity.AVC_ESCRITURA,
                        Notaria = entity.AVC_NOTARIA,
                        FechaEscritura = entity.AVC_FECHA_ESCRITURA,
                        DepartamentoEscrituraId = entity.AVC_DPD_ID_ESCRITURA,
                        CiudadEscrituraId = entity.AVC_DPC_ID_ESCRITURA,
                        CedulaCatastral = entity.AVC_CEDULA_CATASTRAL,
                        LicenciaConstruccion = entity.AVC_LICENCIA_CONSTRUCCION,
                        NombreConjunto = entity.AVC_NOMBRE_CONJUNTO,
                        CoeficienteCopropiedad = entity.AVC_COEFICIENTE_COPROPIEDAD
                    },

                    ObservacionesGenerales = new ObservacionesGeneralesDto
                    {
                        DistribucionInmueble = entity.AVC_DISTRIBUCION_INMUEBLE,
                        CaracteristicasAdicionales = entity.AVC_CARACTERISTICAS_ADICIONALES
                    },

                    Dependencias = new DependenciasDto
                    {
                        Descripcion = entity.AVC_DESCRIPCION_DEPENDENCIA,
                        Garaje = entity.AVC_GARAJE,
                        DescripcionGaraje = entity.AVC_DESCRIPCION_GARAJE
                    },

                    Construccion = new ConstruccionDto
                    {
                        SalaAreaConstruida = entity.AVC_SALA_AREA_CONSTRUIDA,
                        AreaPrivada = entity.AVC_AREA_PRIVADA
                    },

                    CuadroValoracion = new CuadroValoracionDto
                    {
                        Inmueble = entity.AVC_CUADRO_VALORACION_INMUEBLE
                    }
                };
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }


        public async Task<Response<ValoresSimulacionCreditoReclasificacion>> ConsultaValoresSimulacionCredito(string numeroDocumento)
        {
            Response<ValoresSimulacionCreditoReclasificacion> response = new();

            try
            {
                if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El número de documento es obligatorio.";
                    return response;
                }

                var resultado = await _creditoRepository.ConsultaValoresSimulacionCredito(numeroDocumento);

                if (resultado == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No se encontró información para el número de documento ingresado.";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = resultado;
                response.ReturnMessage = "Consulta realizada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<ValidacionCreditoProspeccionDto>> ConsultarValidacionCreditoProspeccion(string numeroDocumento)
        {
            Response<ValidacionCreditoProspeccionDto> response = new();

            try
            {
                if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El número de documento es obligatorio.";
                    return response;
                }

                var entity = await _creditoRepository.ConsultarValidacionCreditoProspeccion(numeroDocumento);

                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No se encontró información para el número de documento.";
                    return response;
                }

                response.Data = new ValidacionCreditoProspeccionDto
                {
                    VcpId = entity.VCP_ID,
                    NumeroDocumento = entity.VCP_NUMERO_DOCUMENTO,
                    CargarDocumento = entity.VCP_CARGAR_DOCUMENTO,
                    Estado = entity.VCP_ESTADO,
                    Fecha = entity.VCP_FECHA
                };

                response.IsSuccess = true;
                response.ReturnMessage = "Consulta realizada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<ValidacionCreditoProspeccionDto>> InsertarActualizarValidacionCreditoProspeccion(ValidacionCreditoProspeccionDto request)
        {
            Response<ValidacionCreditoProspeccionDto> response = new();

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La información es obligatoria.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.NumeroDocumento))
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El número de documento es obligatorio.";
                    return response;
                }

                var entity = new ValidacionCreditoProspeccion
                {
                    VCP_NUMERO_DOCUMENTO = request.NumeroDocumento,
                    VCP_ESTADO = request.Estado
                };

                var resultado = await _creditoRepository.InsertarActualizarValidacionCreditoProspeccion(entity);

                if (resultado == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "No fue posible almacenar la información.";
                    return response;
                }

                response.Data = new ValidacionCreditoProspeccionDto
                {
                    VcpId = resultado.VCP_ID,
                    NumeroDocumento = resultado.VCP_NUMERO_DOCUMENTO,
                    CargarDocumento = resultado.VCP_CARGAR_DOCUMENTO,
                    Estado = resultado.VCP_ESTADO,
                    Fecha = resultado.VCP_FECHA
                };

                response.IsSuccess = true;
                response.ReturnMessage = "La información fue almacenada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<ResponseDocumentoSimDto>> CargarDocumentosActividadCredito(CargarDocumentosSolicitudCreditoDto documentos)
        {
            var attachRequestDto = new AttachRequestDto
            {
                UserOperation = "web",
                Files = new List<FileDto>()
            };

            string extension = string.Empty;
            documentos.Documentos = new List<RequestDocumentoSolicCreditoDto>();
            foreach (var item in documentos.Documentos)
            {
                attachRequestDto.Files = new List<FileDto>();

                try
                {
                    string filePath = item.rutaImagen;
                    var fileExtensions = new List<string> { "pdf", "tif" };
                    int folios = 0;
                    string archivoBase64 = string.Empty;
                    byte[] contenidoBytes;

                    ServiciosDinamicos blogStorage = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

                    // -------------------------------
                    // Obtención archivo desde Storage
                    // -------------------------------
                    if (blogStorage.SRD_ESTADO)
                    {
                        var bdinfo = await _blobStorageGenericRepository
                            .DescargarArchivoStorage("credito", item.rutaImagen);

                        extension = filePath.Split(".")[1];

                        if (fileExtensions.Contains(extension))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                await bdinfo.Content.CopyToAsync(memoryStream);
                                contenidoBytes = memoryStream.ToArray();
                            }

                            string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                            folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                            archivoBase64 = Convert.ToBase64String(contenidoBytes);
                        }
                        else
                        {
                            folios = 0;
                        }
                    }
                    else
                    {
                        var bdinfoLocal = await _localBlogStorageRepository
                            .DescargarArchivoLocal("credito", item.rutaImagen);

                        if (bdinfoLocal.IsSuccess && bdinfoLocal.Data != null)
                        {
                            contenidoBytes = bdinfoLocal.Data;
                            extension = Path.GetExtension(item.rutaImagen)
                                .Replace(".", "")
                                .ToLower();

                            if (fileExtensions.Contains(extension))
                            {
                                archivoBase64 = Convert.ToBase64String(contenidoBytes);

                                string contenidoTexto = Encoding.UTF8.GetString(contenidoBytes);
                                folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;
                            }
                        }
                        else
                        {
                            folios = 0;
                            archivoBase64 = string.Empty;
                        }
                    }

                    DocumentosCargadoDodo dod_dodo = new DocumentosCargadoDodo();

                    var infoTipoDocument = (await ObtenerTipoDocumentoPorId(item.idTipoDocumento.ToString())).Data;

                    dod_dodo.DCD_CARGADO = true;
                    dod_dodo.DCD_DESCRIPCION = infoTipoDocument.Descripcion;
                    dod_dodo.RUTA = item.rutaImagen;
                    dod_dodo.SLS_ID = item.idSolicitudCredito;
                    dod_dodo.TDC_ID = item.idTipoDocumento;



                    // -------------------------------
                    // Validar si ya existe el documento
                    // -------------------------------

                    var documentoExistente = await _creditoRepository.ObtenerDocumentoCargado(
                        new DocumentosCargadoDodo
                        {
                            SLS_ID = item.idSolicitudCredito,
                            TDC_ID = item.idTipoDocumento
                        });

                    if (documentoExistente == null)
                    {
                        // -------------------------------
                        // Adjuntar al gestor documental
                        // -------------------------------

                        attachRequestDto.Radicado = documentos.Radicado;

                        attachRequestDto.Files.Add(
                            CrearObjetoWorkmanager(
                                archivoBase64,
                                infoTipoDocument,
                                extension));

                        dod_dodo.DCD_FECHA_CARGUE = DateTime.Now;
                        await _creditoRepository.InsertarDocumentoCargado(dod_dodo);

                        await _gestorDocumentalBusinessLogic.Form_AttachFilesAsync(attachRequestDto);


                       
                    }
                }
                catch (Exception ex)
                {
                    return new Response<ResponseDocumentoSimDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "Error creando documento en la BD, error: " + ex.Message
                    };
                }
            }

            return new Response<ResponseDocumentoSimDto>
            {
                Data = null,
                IsSuccess = true,
                ReturnMessage = "Todo cargado en DODO"
            };
        }
        public async Task<Response<IEnumerable<UsuarioCreditoTramite>>> ConsultarUsuarioCredito(int indicador)
        {
            Response<IEnumerable<UsuarioCreditoTramite>> response = new Response<IEnumerable<UsuarioCreditoTramite>>();

            if (indicador != 1 && indicador != 2)
            {
                response.ReturnMessage = "El indicador no es válido.";
                return response;
            }

            response.Data = await _creditoRepository.ConsultarUsuarioCredito(indicador);
            response.ReturnMessage = "Consulta realizada correctamente.";

            return response;
        }

        public async Task<Response<IEnumerable<DocumentosActRespuesta>>> ObtenerDocumentosPorTAC(DocumentosActSolicitud request)
        {
            var documentos = await _creditoRepository.ObtenerDocumentosPorTAC(request);

            return new Response<IEnumerable<DocumentosActRespuesta>>
            {
                Data = documentos
            };
        }

        public async Task<Response<ValidacionCreditoProspeccion>> InsertarDocumentoActividadCredito(DocumentosCredito request)
        {
            Response<ValidacionCreditoProspeccion> response = new();

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La solicitud no puede ser nula.";
                    return response;
                }

                if (request.TAC_ID <= 0)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El TAC_ID es obligatorio.";
                    return response;
                }

                if (request.TDC_ID <= 0)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El TDC_ID es obligatorio.";
                    return response;
                }

                response.Data = await _creditoRepository.InsertarDocumentoActividadCredito(request);
                response.IsSuccess = true;
                response.ReturnMessage = "Documento registrado correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<IEnumerable<TipoDocumento>>> ObtenerTiposDocumento()
        {
            Response<IEnumerable<TipoDocumento>> response = new();

            try
            {
                response.Data = await _creditoRepository.ObtenerTiposDocumento();
                response.IsSuccess = true;
                response.ReturnMessage = "Tipos de documento obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<bool>> EliminarDocumentoActividadCredito(DocumentosCredito request)
        {
            Response<bool> response = new();

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "La solicitud no puede ser nula.";
                    return response;
                }

                if (request.TAC_ID <= 0)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El TAC_ID es obligatorio.";
                    return response;
                }

                if (request.TDC_ID <= 0)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = "El TDC_ID es obligatorio.";
                    return response;
                }

                response.Data = await _creditoRepository.EliminarDocumentoActividadCredito(request);
                response.IsSuccess = true;
                response.ReturnMessage = "Documento eliminado correctamente.";

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = ex.Message;
            }

            return response;
        }


        public async Task<Response<IEnumerable<ReporteProspeccionPreaprobado>>> ObtenerReporteTransUnionProspeccion(string numeroIdentificacion,DateTime? fechaDesde,DateTime? fechaHasta,int? estado)
        {
            try
            {
                

                // Validación: rango de fechas
                if (fechaDesde.HasValue && fechaHasta.HasValue &&
                    fechaDesde.Value.Date > fechaHasta.Value.Date)
                {
                    return new Response<IEnumerable<ReporteProspeccionPreaprobado>>
                    {
                        IsSuccess = false,
                        ReturnMessage = "La fecha inicial no puede ser mayor que la fecha final.",
                        Data = Enumerable.Empty<ReporteProspeccionPreaprobado>()
                    };
                }

                // Validación: estado permitido
                if (estado.HasValue && !new[] { 1, 2, 3 }.Contains(estado.Value))
                {
                    return new Response<IEnumerable<ReporteProspeccionPreaprobado>>
                    {
                        IsSuccess = false,
                        ReturnMessage = "El estado enviado no es válido.",
                        Data = Enumerable.Empty<ReporteProspeccionPreaprobado>()
                    };
                }

                var resultado = await _creditoRepository.ObtenerReporteTransUnion(
                    numeroIdentificacion,
                    fechaDesde,
                    fechaHasta,
                    estado);

                if (resultado == null || !resultado.Any())
                {
                    return new Response<IEnumerable<ReporteProspeccionPreaprobado>>
                    {
                        IsSuccess = true,
                        ReturnMessage = "No se encontraron registros.",
                        Data = Enumerable.Empty<ReporteProspeccionPreaprobado>()
                    };
                }

                return new Response<IEnumerable<ReporteProspeccionPreaprobado>>
                {
                    IsSuccess = true,
                    ReturnMessage = "Consulta realizada correctamente.",
                    Data = resultado
                };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ReporteProspeccionPreaprobado>>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error consultando el reporte TransUnion: {ex.Message}",
                    Data = Enumerable.Empty<ReporteProspeccionPreaprobado>()
                };
            }
        }
    }
}

