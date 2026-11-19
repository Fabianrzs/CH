using System;
using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs.Models;
using GA2.Domain.Entities.Bpm;
using GA2.Domain.Core.Solicitud.WorkManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GA2.Domain.Entities;
using System.Security.Policy;
using iTextSharp.text.pdf.codec.wmf;
using Newtonsoft.Json;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities.Solicitud;
using DocumentFormat.OpenXml.InkML;
using GA2.Application.Main.ProviderRequest;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.IdentityModel.Tokens;
using SharpCompress;
using GA2.Domain.Core.Credito.WorkManager;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using GA2.Application.Dto.BUA;
using System.Diagnostics;
using Google.Protobuf;
using Irony.Parsing;
using GA2.Domain.Entities.Catalogs.ComprobantesCartera;
using GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero;
using Dapper;
using GA2.Infraestructure.Data;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Logica
    /// <author>Erwin Pantoja España</author>
    /// <date>20/10/2021</date>
    /// </summary>
    ///
    public class SolicitudBusinessLogic : ISolicitudBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IGestorDocumentSolicitudes _gestordocumentalSolicitudes;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ITareaRepository _itareaRepository;
        private readonly BlobStorageOptions _options;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IGestorDocumentalBusinessLogic _WMBusinessLogic;
        private readonly ICreditoRepository _creditoRepository;
        private readonly ILogger<SolicitudBusinessLogic> _logger;
        private readonly IVigiaBusinessLogic _vigiaBusinessLogic;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClienteRepository _clientsRepository;
        private readonly IEmbargosProvider _iEmbargosRequestProvider;
        private readonly IMensajeRepository _messageRepository;
        private readonly IOptions<BlobStorageOptions> _optionsDocs;
        private readonly IVerificacionDerechosRepository _verificacionDerechosRepository;
        private readonly IEmbargosRepository _embargosRepository;
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IVerificacionTerceroVigia _verificarTerceroVigia;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="solicitudRepository"></param>
        public SolicitudBusinessLogic(
            IOptions<ConfiguracionesSvc> configurationes,
            IMapper mapper,
            ISolicitudRepository solicitudRepository,
            IClientRequestProvider iClientRequestProvider,
            ITareaRepository itareaRepository,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            IOptions<BlobStorageOptions> options,
            IGestorDocumentSolicitudes gestorDocumentSolicitudes,
            IGestorDocumentalBusinessLogic WMBusinessLogic,
            ICreditoRepository creditoRepository,
            ILogger<SolicitudBusinessLogic> logger,
            IVigiaBusinessLogic vigiaBusinessLogic,
            ICatalogosRepository catalogosRepository,
            IClienteRepository clientsRepository,
            IEmbargosProvider embargosProvider,
            IMensajeRepository messageRepository,
            IOptions<BlobStorageOptions> optionsDocs,
            IVerificacionDerechosRepository verificacionDerechosRepository,
            IEmbargosRepository embargosRepository,
            IPostulacionRepository postulacionRepository,
            IUsersRepository usersRepository,
             IVerificacionTerceroVigia verificarTerceroVigia
            )
        {

            _mapper = mapper;
            _solicitudRepository = solicitudRepository;
            _iClientRequestProvider = iClientRequestProvider;
            _itareaRepository = itareaRepository;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _options = options.Value;
            _gestordocumentalSolicitudes = gestorDocumentSolicitudes;
            _WMBusinessLogic = WMBusinessLogic;
            _creditoRepository = creditoRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vigiaBusinessLogic = vigiaBusinessLogic;
            _catalogosRepository = catalogosRepository;
            _clientsRepository = clientsRepository;
            _iEmbargosRequestProvider = embargosProvider;
            _messageRepository = messageRepository;
            _optionsDocs = optionsDocs;
            _verificacionDerechosRepository = verificacionDerechosRepository;
            _embargosRepository = embargosRepository;
            _postulacionRepository = postulacionRepository;
            _usersRepository = usersRepository;
            _verificarTerceroVigia = verificarTerceroVigia;

        }


        /// <summary>
        /// Metodo logica de negocio para insertar una solicitud
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <param name="crearSolicitudDto"></param>
        /// <returns>Response<ObtenerTramiteSolicitudDto></returns>
        public async Task<Response<ObtenerTramiteSolicitudDto>> CrearSolicitud(CrearSolicitudDto crearSolicitudDto)
        {
            CrearSolicitud parametrosSolicitud = this._mapper.Map<CrearSolicitud>(crearSolicitudDto);
            IEnumerable<Tarea> tareas = await _itareaRepository.ObtenerTareasPorProcesoId(crearSolicitudDto.procesoId);
            parametrosSolicitud.TRA_ID = tareas.Where(x => x.TRA_ORDEN == 1).FirstOrDefault().TRA_ID;

            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();
            IEnumerable<Entities.Bpm.EstadoSolicitud> listEstadoSol = await _itareaRepository.ObtenerEstadoSolicitud();
            var tareaDescription = tareas.Where(x => x.TRA_NOMBRE == "Verificación en tiempo").FirstOrDefault();
            if (tareaDescription is not null)
            {
                parametrosSolicitud.SOL_ESTADO_SOLICITUD = listEstadoSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;  //SolicitudConstants.EstadoInicialSolicitud;
                parametrosSolicitud.STL_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;  //SolicitudConstants.EstadoInicialTareaSolicitud;
            }
            else
            {
                parametrosSolicitud.SOL_ESTADO_SOLICITUD = listEstadoSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;  //SolicitudConstants.EstadoInicialSolicitud;
                parametrosSolicitud.STL_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;  //SolicitudConstants.EstadoInicialTareaSolicitud;
            }


            Response<ObtenerTramiteSolicitudDto> respuesta = new Response<ObtenerTramiteSolicitudDto>();

            Response<ClienteDto> clienteDto = new Response<ClienteDto>();
            Response<ClienteDto> clienteDtoTipo = new Response<ClienteDto>();
            if (crearSolicitudDto.clienteId == 0)
            {
                return new Response<ObtenerTramiteSolicitudDto> { Data = null, IsSuccess = false, ReturnMessage = "El id cliente no puede ser igual 0" };
            }
            clienteDtoTipo = await _iClientRequestProvider.ObtenerClienteInformacion(crearSolicitudDto.clienteId);
            clienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDtoTipo.Data.IdTipoIdentificacion, clienteDtoTipo.Data.Identificacion);
            var find = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);

            if (clienteDto.Data.EstadoCliente == "DESVINCULADO")
            {
                return new Response<ObtenerTramiteSolicitudDto> { Data = null, IsSuccess = false, ReturnMessage = "El afiliado se encuentra Retirado!" };
            }

            respuesta.Data = new ObtenerTramiteSolicitudDto();
            HomKriterionGestorDocumental homologacion = null;
            string codProcesoGD = "";
            string codSubModGD = "";
            decimal valorRetirar = 0;
            Guid solicitudCreditoId = Guid.Empty;

            IEnumerable<TipoSolicitud> listTipoSolicitud = await _catalogosRepository.ObtenerTipoSolicitud(2);
            string _PCS_NOMBRE = listTipoSolicitud.Where(x => x.PCS_ID == crearSolicitudDto.procesoId).FirstOrDefault().PCS_NOMBRE.ToUpper();

            //IEnumerable<TipoModalidad> listTipoModalidad = await _catalogosRepository.ObtenerTipoModalidad(crearSolicitudDto.procesoId.ToString());
            respuesta.Data.TimCodigo = crearSolicitudDto.timCodigo;
            respuesta.Data.NumeroSolicitud = 1321;

            RespuestaEstadoTramiteSolicitudDesembolso estadoTramiteSolicitudDes = null;
            RespuestaValorDesembolsoCredito valorDesembolsoCredito = null;
            if (_PCS_NOMBRE == "CRÉDITO HIPOTECARIO" && crearSolicitudDto.timCodigo == "PCUOTAINI")
            {
                estadoTramiteSolicitudDes = await this._creditoRepository.ObtenerEstadoTramiteSolicitudDesembolso(clienteDto.Data.Identificacion);

                if (estadoTramiteSolicitudDes == null || estadoTramiteSolicitudDes.ESTADO_ACTIVIDAD != "FINALIZADA")
                {
                    return new Response<ObtenerTramiteSolicitudDto> { Data = null, IsSuccess = false, ReturnMessage = "El cliente no tiene un trámite de crédito hipotecario con actividad desembolso finalizada!" };
                }

                valorDesembolsoCredito = await this._creditoRepository.ObtenerValorDesembolsoCredito(clienteDto.Data.Identificacion);
                if (valorDesembolsoCredito == null || valorDesembolsoCredito.ValorDesembolso == 0)
                {
                    return new Response<ObtenerTramiteSolicitudDto> { Data = null, IsSuccess = false, ReturnMessage = "El velor de la cuota inicial debe ser mayor que cero!" };
                }

                respuesta.Data.NumeroSolicitud = estadoTramiteSolicitudDes.NUMERO_SOLICITUD;
                respuesta.Data.TipoCreditoId = estadoTramiteSolicitudDes.ID_TIPO_CREDITO;
                valorRetirar = valorDesembolsoCredito == null ? 15000000 : valorDesembolsoCredito.ValorDesembolso;
                solicitudCreditoId = estadoTramiteSolicitudDes.ID_SOLICITUD;
            }
            else
            {
                Guid parametroIdTipoSolicitud = _itareaRepository.ObtenerParametrosTipoSolicitud("CESANTIAS").Result.PRM_ID;

                homologacion = await _itareaRepository.ObtenerHomGestorDocumental(crearSolicitudDto.procesoId,
                    crearSolicitudDto.tipoSubModalidad, parametroIdTipoSolicitud);

                if (homologacion == null)
                    return new Response<ObtenerTramiteSolicitudDto> { Data = null, IsSuccess = false, ReturnMessage = "Debe configurar la homologación con el Gestor Documental!" };

                codProcesoGD = homologacion.PCS_NOMBRE_GESTOR_DOC;
                codSubModGD = homologacion.TPS_SUB_NOMBRE_GESTOR_DOC;
            }


            respuesta.Data.CodProcesoGD = codProcesoGD;
            respuesta.Data.CodSubModGD = codSubModGD;
            respuesta.Data.clienteId = crearSolicitudDto.clienteId;
            respuesta.Data.nombresCliente = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre}";
            respuesta.Data.apellidosCliente = $"{clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
            respuesta.Data.identificacionCliente = $"{clienteDto.Data.Identificacion}";
            respuesta.Data.tipoIdentificacion = clienteDto.Data.IdTipoIdentificacion;
            respuesta.Data.CreadoPor = crearSolicitudDto.creadoPor;
            respuesta.Data.fechaSolicitud = DateTime.Now;
            parametrosSolicitud.SOL_FECHACREACION = DateTime.Now;
            parametrosSolicitud.SOL_CEDULA = clienteDtoTipo.Data.Identificacion;
            parametrosSolicitud.SOL_NOMBRE = $"{clienteDtoTipo.Data.PrimerNombre} {clienteDtoTipo.Data.SegundoNombre} {clienteDtoTipo.Data.PrimerApellido} {clienteDtoTipo.Data.SegundoApellido}";

            var dataExisteSolicitud = new ObtenerTramiteSolicitud();
            var dataSolicitudMatricula = new RespuestaMatricula();

            if (respuesta.Data.TimCodigo == "ACREDV8" || respuesta.Data.TimCodigo == "REINTG" || respuesta.Data.TimCodigo == "RESAJ")
            {
                var objectExist = new ConsultarSolicitud()
                {
                    CLI_ID = respuesta.Data.clienteId,
                    CLI_IDENTIFICACION = respuesta.Data.identificacionCliente,
                    TPS_SUB_ID = Guid.NewGuid(),
                    SOL_ESTADO_ANULADO = 0
                };
                dataExisteSolicitud = await _solicitudRepository.ConsultarExisteSolicitud(objectExist);
                if (dataExisteSolicitud.SOL_RADICADO_GD is not null)
                {
                    respuesta.Data.NumeroRadicadoSG = dataExisteSolicitud.SOL_RADICADO_GD;
                }
            }
            var radicado = await _gestordocumentalSolicitudes.RadicarSolicitud(respuesta.Data);


            var radicadoresult = radicado?.Data?.Form_InsertResult.BarCode?.ToString() ?? "0";
            parametrosSolicitud.SOL_RADICADO = radicadoresult;
            if (radicado.IsSuccess && radicadoresult != "0")
            {
                if (respuesta.Data.TimCodigo == "ACREDV8" || respuesta.Data.TimCodigo == "REINTG" || respuesta.Data.TimCodigo == "RESAJ")
                {
                    var inforMatricula = await _solicitudRepository.ConsultarSolicitudMatricula(dataExisteSolicitud.SOL_ID);
                    if (inforMatricula.Count() > 0)
                    {
                        dataSolicitudMatricula = inforMatricula.FirstOrDefault();
                    }
                }


                respuesta = new()
                {
                    Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                    this._solicitudRepository.CrearSolicitud(parametrosSolicitud))
                };
                if (respuesta.Data.TimCodigo == "ACREDV8" || respuesta.Data.TimCodigo == "REINTG" || respuesta.Data.TimCodigo == "RESAJ")
                {

                    await ActualizarInformatioSolicitud(dataExisteSolicitud, respuesta);
                    if (dataSolicitudMatricula is not null)
                    {
                        await _solicitudRepository.InsertarMatriculaSolicitud(respuesta.Data.solicitudId, dataSolicitudMatricula.MAI_ID);
                    }
                }
                decimal sumatoriaTotal, sumatoriaAhorro, sumatoriaCesantia;
                sumatoriaTotal = sumatoriaAhorro = sumatoriaCesantia = 0;
                string concepto = "";

                foreach (var item in clienteDto.Data.Saldos)
                {
                    concepto = item.Concepto;
                    if (concepto.Contains("Ahorro"))
                    {
                        sumatoriaAhorro += item.Valor;
                    }
                    if (concepto.Contains("Cesantias"))
                    {
                        sumatoriaCesantia += item.Valor;
                    }
                    sumatoriaTotal += item.Valor;
                }

                respuesta.Data.CodProcesoGD = codProcesoGD;
                respuesta.Data.CodSubModGD = codSubModGD;
                respuesta.Data.clienteId = crearSolicitudDto.clienteId;
                respuesta.Data.nombresCliente = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre}";
                respuesta.Data.apellidosCliente = $"{clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
                respuesta.Data.identificacionCliente = $"{clienteDto.Data.Identificacion}";
                respuesta.Data.tipoIdentificacion = clienteDto.Data.IdTipoIdentificacion;

                respuesta.Data.valorCesantia = sumatoriaCesantia;
                respuesta.Data.valorAhorro = sumatoriaAhorro;
                respuesta.Data.valorTotal = sumatoriaTotal;
                respuesta.Data.SaldosE = clienteDto.Data.Saldos;
                respuesta.Data.valorRetirar = valorRetirar;


                Response<ClienteDto> clienteSaldostDto = new Response<ClienteDto>();
                clienteSaldostDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);


                #region Datos Conyuge
                if (clienteSaldostDto.Data != null)
                {
                    respuesta.Data.IdConyuge = clienteSaldostDto.Data.IdConyuge;
                    respuesta.Data.IdTipoIdentificacionConyuge = clienteSaldostDto.Data.IdTipoIdentificacionConyuge;
                    respuesta.Data.IdentificacionConyuge = clienteSaldostDto.Data.IdentificacionConyuge;
                    respuesta.Data.PrimerNombreConyuge = clienteSaldostDto.Data.PrimerNombreConyuge;
                    respuesta.Data.SegundoNombreConyuge = clienteSaldostDto.Data.SegundoNombreConyuge;
                    respuesta.Data.PrimerApellidoConyuge = clienteSaldostDto.Data.PrimerApellidoConyuge;
                    respuesta.Data.SegundoApellidoConyuge = clienteSaldostDto.Data.SegundoApellidoConyuge;
                    respuesta.Data.ConyugeActivo = clienteSaldostDto.Data.ConyugeActivo;
                }
                #endregion

                respuesta.Data.solicitudCreditoId = solicitudCreditoId;
                respuesta.Data.NumeroRadicadoSG = radicadoresult;

                await this._solicitudRepository.ActualizaNumeroRadicadoSolicitud(respuesta.Data);
            }
            else
            {
                try
                {
                    if (respuesta.Data.TimCodigo == "ACREDV8" || respuesta.Data.TimCodigo == "REINTG" || respuesta.Data.TimCodigo == "RESAJ" || respuesta.Data.TimCodigo == "NOACREDV8")
                    {
                        var objectExist = new ConsultarSolicitud()
                        {
                            CLI_ID = respuesta.Data.clienteId,
                            CLI_IDENTIFICACION = respuesta.Data.identificacionCliente,
                            TPS_SUB_ID = Guid.NewGuid(),
                            SOL_ESTADO_ANULADO = 0
                        };
                        dataExisteSolicitud = await _solicitudRepository.ConsultarExisteSolicitud(objectExist);
                        var inforMatricula = await _solicitudRepository.ConsultarSolicitudMatricula(dataExisteSolicitud.SOL_ID);
                        if (inforMatricula.Count() > 0)
                        {
                            dataSolicitudMatricula = inforMatricula.FirstOrDefault();
                        }

                        respuesta = new()
                        {
                            Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                        this._solicitudRepository.CrearSolicitud(parametrosSolicitud))
                        };

                        await ActualizarInformatioSolicitud(dataExisteSolicitud, respuesta);
                        if (dataSolicitudMatricula is not null)
                        {
                            await _solicitudRepository.InsertarMatriculaSolicitud(respuesta.Data.solicitudId, dataSolicitudMatricula.MAI_ID);
                        }
                    }

                }
                catch (Exception)
                {

                    return new Response<ObtenerTramiteSolicitudDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "Error al crear la solicitud con WorkManager" +
                        radicado.Data.Form_InsertResult.Message ?? radicado.ReturnMessage
                    };
                }

                return new Response<ObtenerTramiteSolicitudDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = "Error al crear la solicitud con WorkManager" +
                    radicado.Data.Form_InsertResult.Message ?? radicado.ReturnMessage
                };
            }

            return respuesta;
        }
        private async Task ActualizarInformatioSolicitud(ObtenerTramiteSolicitud request, Response<ObtenerTramiteSolicitudDto> response)
        {
            var solicitud = new ActualizarSolicitud
            {
                SOL_ID = response.Data.solicitudId,
                NUMERO_FOLIO = request.NUMERO_FOLIO,
                SOL_VALOR_A_RETIRAR = request.SOL_VALOR_A_RETIRAR

            };
            var dataUpdate = await _solicitudRepository.ActualizarSolicitud(solicitud);
        }
        public async Task<Response<ObtenerFlujosDodo>> ObtenerFlujosDodo(ObtenerFlujosDodoDto obtenerFlujosDodo)
        {
            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(obtenerFlujosDodo.tipoDocumentoId, obtenerFlujosDodo.numeroDocumento, 0);
            Response<ObtenerFlujosDodo> respuesta = new Response<ObtenerFlujosDodo>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El afiliado no existe en BUA";
                return respuesta;
            }
            var formularioFlujo37 = await _gestordocumentalSolicitudes.ObtenerFormulariosDodo(clienteDto.Data.Identificacion, "37");
            var formularioFlujo102 = await _gestordocumentalSolicitudes.ObtenerFormulariosDodo(clienteDto.Data.Identificacion, "102");
            var formularioFlujo109 = await _gestordocumentalSolicitudes.ObtenerFormulariosDodo(clienteDto.Data.Identificacion, "109");
            var formularioFlujo194 = await _gestordocumentalSolicitudes.ObtenerFormulariosDodo(clienteDto.Data.Identificacion, "194");

            respuesta.Data = new ObtenerFlujosDodo();
            respuesta.Data.tipoDocumentoId = clienteDto.Data.IdTipoIdentificacion;
            respuesta.Data.numeroDocumento = clienteDto.Data.Identificacion;

            if (formularioFlujo102?.Data?.Form_GetDataResult.Rows > 0)
            {
                respuesta.Data.tieneFlujo102 = true;
            }
            if (formularioFlujo37?.Data?.Form_GetDataResult.Rows > 0)
            {
                respuesta.Data.tieneFlujo37 = true;
            }
            if (formularioFlujo109?.Data?.Form_GetDataResult.Rows > 0)
            {
                respuesta.Data.tieneFlujo109 = true;
            }
            if (formularioFlujo194?.Data?.Form_GetDataResult.Rows > 0)
            {
                respuesta.Data.tieneFlujo194 = true;
            }

            return respuesta;

        }

        public async Task<Response<ObtenerTramiteSolicitudDto>> ConsultarExisteSolicitud(ConsultarSolicitudDto consultarSolicitudDto)
        {
            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(consultarSolicitudDto.tipoIdentificacion, consultarSolicitudDto.identificacion, 0);
            var estados = await _solicitudRepository.ObtenerEstadosCliente();
            Response<ObtenerTramiteSolicitudDto> respuesta = new Response<ObtenerTramiteSolicitudDto>();
         


            
            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El afiliado no existe en BUA";
                return respuesta;
            }

            IEnumerable<Entities.Bpm.EstadoSolicitud> listEstadoSol = await _itareaRepository.ObtenerEstadoSolicitud();
            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();

            if (consultarSolicitudDto.solicitudTareaId.ToString() != "00000000-0000-0000-0000-000000000000") // its default value for Guid
            {
                ConsultarSolicitudPorActividad consultarSolicitud = new ConsultarSolicitudPorActividad();
                consultarSolicitud.SLT_ID = consultarSolicitudDto.solicitudTareaId;

                respuesta =
                    new Response<ObtenerTramiteSolicitudDto>
                    {
                        Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                        this._solicitudRepository.ConsultarSolicitudPorActividad(consultarSolicitud))
                    };

                if (respuesta?.Data != null)
                {
                    var pantalla = respuesta.Data.volverPantalla;

                    if (pantalla == "ACREDVB" || pantalla == "RESAJ" || pantalla == "REINTG")
                    {
                        respuesta.Data.pestanas = "[\"Cargue de documentos Acreditación\"]";
                    }
                }
            }
            else
            {

                ConsultarSolicitud consultarSolicitud = new ConsultarSolicitud();
                consultarSolicitud.CLI_ID = clienteDto.Data.IdCliente;
                consultarSolicitud.CLI_IDENTIFICACION = clienteDto.Data.Identificacion;
                consultarSolicitud.TPS_SUB_ID = consultarSolicitudDto.tipoSubModalidadId;
                consultarSolicitud.SOL_ESTADO_ANULADO = listEstadoSol.Where(x => x.CVL_DESCRIPCION == "ANULADA").FirstOrDefault().CVL_ID;  //SolicitudConstants.EstadoAnuladaSolicitud;
                Procesos proceso = await _solicitudRepository.ObtenerProcesoPorSubmodalidad(consultarSolicitudDto.tipoSubModalidadId);



                respuesta = new Response<ObtenerTramiteSolicitudDto>
                {
                    Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                        this._solicitudRepository.ConsultarExisteSolicitud(consultarSolicitud))
                };

                respuesta.Data.solicitudTerceroBeneficiarioEstudioDto = (from x in await _solicitudRepository.ConsultarTerceroBeneficiarioEstudio(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroBeneficiarioEstudioId)
                                                                         select new SolicitudTerceroBeneficiarioEstudioDto
                                                                         {
                                                                             tipoDocEstudioObj = new LlaveValorStringDto
                                                                             {
                                                                                 id = x.TID_ID_FK,
                                                                                 descripcion = x.TID_DESCRIPCION
                                                                             },
                                                                             numDocEstudio = x.TER_NUMERO_DOCUMENTO,
                                                                             parentescoEstudio = x.TER_PARENTESCO,
                                                                             razonsocialEstudio = x.TER_NOMBRE_RAZON_SOCIAL,
                                                                             tipoDocEstudio = x.TID_ID_FK,
                                                                             parentescoEstudioObj = new LlaveValorStringDto
                                                                             {
                                                                                 id = x.TER_PARENTESCO,
                                                                                 descripcion = x.CVL_DESCRIPCION
                                                                             },
                                                                             tipoBeneficiario = x.TER_TIPO_TERCERO_FK,
                                                                             tipoDeBeneficiario = x.tipoDeBeneficiario

                                                                         }).ToList();


                if (respuesta.Data.EstadoCredito == "PoseeCredito")
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = "El afiliado posee un credito de leasing habitacional activo";
                    return respuesta;
                }
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(respuesta);
                    string estadocredit = "\"EstadoCredito\":\"NoPoseeCredito\",";
                    respuestaJson = respuestaJson.Replace(estadocredit, "");

                    Guid vacio = Guid.Empty;

                    if (respuesta.Data.solicitudId == vacio)
                    {
                        respuesta.Data = null;
                    }
                }

                var permitirSegundoPago = false;
                IEnumerable<ObtenerInformacionSolicitudCliente> solicitudes = await _solicitudRepository.ObtenerInformacionSolicitudCliente(consultarSolicitudDto.identificacion);
                var submodalidades = await _solicitudRepository.ObtenerSubModalidadPrimerPago(consultarSolicitudDto.tipoSubModalidadId);


                Procesos procesoActual = new Procesos();

                if (consultarSolicitudDto.tipoSubModalidadId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    procesoActual = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(consultarSolicitudDto.solicitudTareaId);
                }
                else
                {
                    procesoActual = await _solicitudRepository.ObtenerProcesoPorSubmodalidad(consultarSolicitudDto.tipoSubModalidadId);
                }

                var tipoVinculacion = consultarSolicitudDto.tipoVinculacion;

                if (procesoActual.PCS_NOMBRE == "Vivienda 14" && consultarSolicitudDto.solicitudTareaId.ToString() == "00000000-0000-0000-0000-000000000000"
                    && tipoVinculacion == "AFILIADO ADMINISTRACION CESANTIAS")
                {
                    foreach (var solicitud in solicitudes)
                    {
                        if (solicitud.TPS_SUB_ID == submodalidades.TPS_PRIMER_PAGO)
                        {
                            permitirSegundoPago = true;
                        }
                    }

                    if (submodalidades.TPS_PRIMER_PAGO.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        if (respuesta.IsSuccess && permitirSegundoPago)
                        {
                            respuesta.IsSuccess = false;
                            respuesta.ReturnMessage = "";
                            return respuesta;
                        }
                        //else
                        //{
                        //    respuesta.IsSuccess = false;
                        //    respuesta.ReturnMessage = "Afiliado no cuenta con una solicitud de primer pago procesado";
                        //    return respuesta;
                        //}
                    }
                }

            }


            Response<ClienteDto> clienteSaldostDto = new Response<ClienteDto>();
            clienteSaldostDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(consultarSolicitudDto.tipoIdentificacion, consultarSolicitudDto.identificacion);

            //inicio revisión de embargos 



            foreach (var cuentas in clienteSaldostDto.Data.Cuentas)
            {
                var SaldoDisponible = cuentas.SaldoDisponible;

                foreach (var Embargos in cuentas.Embargos)
                {
                    var MontoEmbargo = (decimal)Embargos.Valor;
                    var MontoEmbargoFormateado = MontoEmbargo.ToString("N");
                    var TipoEmbargo = Embargos.TipoEmbargo;
                    var Indicador = "";
                    var Mensaje = "";

                    if (MontoEmbargo == SaldoDisponible)
                    {
                        if (TipoEmbargo == "VALOR")
                        {
                            TipoEmbargo = "MONTO";
                            Indicador = "$";
                            respuesta.IsSuccess = false;
                            respuesta.ReturnMessage = "El afiliado tiene un embargo por una cantidad de " + Indicador + " " + MontoEmbargoFormateado + ", por concepto de " + Embargos.TipoProceso + "";
                            return respuesta;
                        }
                    }
                    else if (TipoEmbargo == "PORCENTAJE" && MontoEmbargo == 100)
                    {
                        Indicador = "%";
                        respuesta.IsSuccess = false;
                        respuesta.ReturnMessage = "El afiliado tiene un embargo del " + MontoEmbargoFormateado + " " + Indicador + "  , por concepto de " + Embargos.TipoProceso + "";
                        return respuesta;
                    }
                }
            }







            decimal sumatoriaTotal, sumatoriaAhorro, sumatoriaCesantia;
            sumatoriaTotal = sumatoriaAhorro = sumatoriaCesantia = 0;
            string concepto = "";

            foreach (var item in clienteSaldostDto.Data.Saldos)
            {
                concepto = item.Concepto;
                if (concepto.Contains("Ahorro"))
                {
                    sumatoriaAhorro += item.Valor;
                }
                if (concepto.Contains("Cesantias"))
                {
                    sumatoriaCesantia += item.Valor;
                }
                sumatoriaTotal += item.Valor;
            }

            clienteSaldostDto.Data.Cuentas.ForEach(x => x.Embargos.ForEach(y => sumatoriaTotal -= (decimal)y.Valor));







            if (respuesta.Data != null)
            {



                var datos = await _solicitudRepository.ConsultarInformacionSolicitud(respuesta.Data.NumeroSolicitud);
                var solTarea = await _solicitudRepository.ConsultarSolicitudTareaPorId(respuesta.Data.solicitudTareaId);


                Procesos procesoActual = new Procesos();

                procesoActual = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(respuesta.Data.solicitudTareaId);





                respuesta.Data.nombresCliente = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre}";
                respuesta.Data.apellidosCliente = $"{clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
                respuesta.Data.identificacionCliente = $"{clienteDto.Data.Identificacion}";
                respuesta.Data.clienteId = clienteDto.Data.IdCliente;
                respuesta.Data.tipoIdentificacion = clienteDto.Data.IdTipoIdentificacion;
                respuesta.Data.telefonoCliente = clienteDto.Data.TelefonoResidencia;
                respuesta.Data.correoCliente = clienteDto.Data.CorreoPersonal;
                respuesta.Data.celularCliente = clienteDto.Data.NumeroCelular;
                respuesta.Data.direccionCliente = clienteDto.Data.Direccion;
                respuesta.IsSuccess = false;
                respuesta.Data.valorCesantia = sumatoriaCesantia;
                respuesta.Data.valorAhorro = sumatoriaAhorro;
                respuesta.Data.valorTotal = sumatoriaTotal;
                respuesta.Data.SaldosE = clienteSaldostDto.Data.Saldos;
                respuesta.Data.Numero_Folio = datos.NUMERO_FOLIO;
                respuesta.Data.SOL_RECHAZADA = solTarea.SOL_RECHAZADA;
                respuesta.Data.FechaFirmaEscritura = datos.FECHA_FIRMA_ESCRITURA != null ? datos.FECHA_FIRMA_ESCRITURA : DateTime.Now;

                SolicitudCuenta misCuentas = await _solicitudRepository.ConsultarMisCuentas(respuesta.Data.solicitudId);
                if (misCuentas != null)
                {
                    respuesta.Data.SolicitudMiscuentasDto = new MisCuentasDto
                    {
                        SaldoCuenta = misCuentas.SALDO_CUENTA,
                        Cuenta = misCuentas.CUENTA,
                        TrasladoAutorizado = misCuentas.TRASLADO_AUTORIZADO
                    };
                }

                #region Datos Conyuge
                if (clienteSaldostDto.Data != null)
                {
                    respuesta.Data.IdConyuge = clienteSaldostDto.Data.IdConyuge;
                    respuesta.Data.IdTipoIdentificacionConyuge = clienteSaldostDto.Data.IdTipoIdentificacionConyuge;
                    respuesta.Data.IdentificacionConyuge = clienteSaldostDto.Data.IdentificacionConyuge;
                    respuesta.Data.PrimerNombreConyuge = clienteSaldostDto.Data.PrimerNombreConyuge;
                    respuesta.Data.SegundoNombreConyuge = clienteSaldostDto.Data.SegundoNombreConyuge;
                    respuesta.Data.PrimerApellidoConyuge = clienteSaldostDto.Data.PrimerApellidoConyuge;
                    respuesta.Data.SegundoApellidoConyuge = clienteSaldostDto.Data.SegundoApellidoConyuge;
                    respuesta.Data.ConyugeActivo = clienteSaldostDto.Data.ConyugeActivo;
                }
                #endregion





                respuesta.ReturnMessage = $"Ya existe una solicitud de {respuesta.Data.descripcionModalidad} en pendiente para " +
                $"{clienteDto.Data.PrimerNombre.ToUpper()} {(clienteDto.Data.SegundoNombre != null ? clienteDto.Data.SegundoNombre.ToUpper() : "")} " +
                $"{clienteDto.Data.PrimerApellido.ToUpper()} {(clienteDto.Data.SegundoApellido != null ? clienteDto.Data.SegundoApellido.ToUpper() : "")}";



                int estadoInicial = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;
                //int estadoTramite = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "EN TRÁMITE").FirstOrDefault().CVL_ID;
                int estadoAprobada = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "APROBADA").FirstOrDefault().CVL_ID;
                int estadoAnulada = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "ANULADA").FirstOrDefault().CVL_ID;
                int estadoBLOQUEADO = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "BLOQUEADO").FirstOrDefault().CVL_ID;
                int estadoINCONSISTENTE = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "INCONSISTENTE").FirstOrDefault().CVL_ID;
                // AQUI PENDIENTE PRUEBA


                if (respuesta.Data.solicitudTareaEstadoId == estadoInicial ||
                   respuesta.Data.solicitudTareaEstadoId == estadoAprobada ||
                   respuesta.Data.solicitudTareaEstadoId == estadoINCONSISTENTE ||
                   respuesta.Data.solicitudTareaEstadoId == estadoAnulada
                  || respuesta.Data.solicitudTareaEstadoId == estadoBLOQUEADO
                  )
                {
                    InformacionConyugeDto informacionConyuge = new InformacionConyugeDto();
                    informacionConyuge.TipoIdentificacion = clienteSaldostDto.Data.IdTipoIdentificacion;
                    informacionConyuge.Identificacion = clienteSaldostDto.Data.Identificacion;

                    Response<InformacionConyuge> infoConyuge = await InformacionConyuge(informacionConyuge);

                    if (infoConyuge != null)
                    {
                        respuesta.Data.InformacionConyugeDto = new InformacionConyugeRespuesta
                        {
                            IdConyuge = infoConyuge.Data.IdConyuge,
                            IdTipoIdentificacionConyuge = infoConyuge.Data.IdTipoIdentificacionConyuge,
                            IdentificacionConyuge = infoConyuge.Data.IdentificacionConyuge,
                            PrimerNombreConyuge = infoConyuge.Data.PrimerNombreConyuge,
                            SegundoNombreConyuge = infoConyuge.Data.SegundoNombreConyuge,
                            PrimerApellidoConyuge = infoConyuge.Data.PrimerApellidoConyuge,
                            SegundoApellidoConyuge = infoConyuge.Data.SegundoApellidoConyuge,
                            ConyugeActivo = infoConyuge.Data.ConyugeActivo,
                            NombreTipoIdentificacionConyuge = infoConyuge.Data.NombreTipoIdentificacionConyuge
                        };
                    }




                    respuesta.Data.InformacionTurnoDto = this._mapper.Map<InformacionTurnoDto>(
                    await _solicitudRepository.ObterInformacionTurnoSolicitud(respuesta.Data.solicitudId));

                    respuesta.Data.solicitudMatriculaDto = (from s in await _solicitudRepository.ConsultarSolicitudMatricula(respuesta.Data.solicitudId)
                                                            select new MatriculaRespuestaDto
                                                            {
                                                                causal = s.CAUSAL,
                                                                departamentoId = s.DPD_ID_FK,
                                                                ciudadId = s.DPC_ID_FK,
                                                                direccion = s.MAI_DIRECCION,
                                                                esPrincipal = s.MAI_PRINCIPAL,
                                                                numeroMatricula = s.MAI_NUMERO_MATRICULA,
                                                                id = s.MAI_ID,
                                                                ciudad = new LlaveValorDto
                                                                {
                                                                    id = s.DPC_ID_FK,
                                                                    descripcion = s.DPC_NOMBRE
                                                                },
                                                                departamento = new LlaveValorDto
                                                                {
                                                                    id = (int)s.DPD_ID,
                                                                    descripcion = s.DPD_NOMBRE
                                                                },
                                                                bloqueo = s.MAI_BLOQUEO,
                                                                registros = s.MAI_REGISTRO,
                                                                tipo_bloqueo = s.TIPO_BLOQUEO,
                                                                conyugeActivo = s.MAI_CONYUGE_ACTIVO
                                                            }).ToList();


                    List<RespuestaPropietario> respuestaPropietario = (from s in await _solicitudRepository.ConsultarSolicitudPropietario(respuesta.Data.solicitudId) select s).ToList();

                    respuesta.Data.solicitudPropietarioDto = (from s in respuestaPropietario
                                                              group s by new { s.PRP_ID, s.PRP_NUMERO_IDENTIFICACION, s.PRP_NOMBRE_RAZON_SOCIAL, s.TID_ID_FK, s.TID_DESCRIPCION, s.PRP_PRIMER_NOMBRE, s.PRP_SEGUNDO_NOMBRE, s.PRP_PRIMER_APELLIDO, s.PRP_SEGUNDO_APELLIDO } into g
                                                              select new PropietarioRespuestaDto
                                                              {
                                                                  numeroIdentificacion = g.Key.PRP_NUMERO_IDENTIFICACION,
                                                                  razonSocial = g.Key.PRP_NOMBRE_RAZON_SOCIAL,
                                                                  tipoIdentificacion = new LlaveValorDto
                                                                  {
                                                                      id = g.Key.TID_ID_FK,
                                                                      descripcion = g.Key.TID_DESCRIPCION
                                                                  },
                                                                  PrimerNombre = g.Key.PRP_PRIMER_NOMBRE,
                                                                  SegundoNombre = g.Key.PRP_SEGUNDO_NOMBRE,
                                                                  PrimerApellido = g.Key.PRP_PRIMER_APELLIDO,
                                                                  SegundoApellido = g.Key.PRP_SEGUNDO_APELLIDO,
                                                                  matricula = (from mp in respuestaPropietario
                                                                               where mp.PRP_ID == g.Key.PRP_ID
                                                                               select new MatriculaPropietarioDto
                                                                               {
                                                                                   id = mp.MAI_ID,
                                                                                   numeroMatricula = mp.MAI_NUMERO_MATRICULA,
                                                                                   estado = mp.MAI_PRINCIPAL ? "Principal" : "Relacionado"
                                                                               }).ToList()
                                                              }).Distinct().ToList();


                    respuesta.Data.solicitudTerceroVendedorDto = (from s in await _solicitudRepository.ConsultarTerceroVendedor(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroVendedorId)
                                                                  select new TerceroVendedorRespuestaDto
                                                                  {
                                                                      ciudad = s.DPC_ID,
                                                                      ciudadObj = new LlaveValorDto
                                                                      {
                                                                          id = s.DPC_ID,
                                                                          descripcion = s.DPC_NOMBRE
                                                                      },
                                                                      id = s.TER_ID,
                                                                      correo = s.TER_CORREO_ELECTRONICO,
                                                                      departamentoVendedor = new LlaveValorDto
                                                                      {
                                                                          id = s.DPD_ID,
                                                                          descripcion = s.DPD_NOMBRE
                                                                      },
                                                                      direccion = s.TER_DIRECCION,
                                                                      numDoc = s.TER_NUMERO_DOCUMENTO,
                                                                      razonsocial = s.TER_NOMBRE_RAZON_SOCIAL,
                                                                      telefono = s.TER_TELEFONO,
                                                                      tipoDoc = s.TID_ID_FK,
                                                                      tipoDocObj = new LlaveValorDto
                                                                      {
                                                                          id = s.TID_ID_FK,
                                                                          descripcion = s.TID_DESCRIPCION
                                                                      },
                                                                      PrimerNombre = s.TER_PRIMER_NOMBRE,
                                                                      SegundoNombre = s.TER_SEGUNDO_NOMBRE,
                                                                      PrimerApellido = s.TER_PRIMER_APELLIDO,
                                                                      SegundoApellido = s.TER_SEGUNDO_APELLIDO,
                                                                  }).ToList();


                    respuesta.Data.solicitudTerceroBeneficiarioDto = (from s in await _solicitudRepository.ConsultarTerceroBeneficiario(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroBeneficiarioId)
                                                                      select new TerceroBeneficiarioRespuestaDto
                                                                      {
                                                                          id = s.TER_ID,
                                                                          numDoc = s.TER_NUMERO_DOCUMENTO,
                                                                          razonsocial = s.TER_NOMBRE_RAZON_SOCIAL,
                                                                          tipoDoc = s.TID_ID_FK,
                                                                          tipoDocObj = new LlaveValorDto
                                                                          {
                                                                              id = s.TID_ID_FK,
                                                                              descripcion = s.TID_DESCRIPCION
                                                                          },
                                                                          entidad = s.ENT_ID,
                                                                          entidadObj = new LlaveValortextoDto
                                                                          {
                                                                              id = s.ENT_ID,
                                                                              descripcion = s.ENT_NOMBRE_RAZON_SOCIAL
                                                                          },
                                                                          numeroCuenta = s.TER_NUMERO_CUENTA,
                                                                          tipoCuenta = s.CVL_ID,
                                                                          tipoCuentaObj = new LlaveValorDto
                                                                          {
                                                                              id = s.CVL_ID,
                                                                              descripcion = s.CVL_DESCRIPCION
                                                                          },
                                                                          valorRetirarDos = s.TER_VALOR_RETIRAR
                                                                      }).ToList();

                    respuesta.Data.solicitudTerceroApoderadoDto = (from s in await _solicitudRepository.ConsultarTerceroApoderado(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroApoderadoId)
                                                                   select new TerceroApoderadoRespuestaDto
                                                                   {
                                                                       id = s.TER_ID,
                                                                       tipoDoc = s.TID_ID_FK,
                                                                       tipoDocObj = new LlaveValorDto
                                                                       {
                                                                           id = s.TID_ID_FK,
                                                                           descripcion = s.TID_DESCRIPCION
                                                                       },
                                                                       numDoc = s.TER_NUMERO_DOCUMENTO,
                                                                       razonsocial = s.TER_NOMBRE_RAZON_SOCIAL,
                                                                       parentesco = s.CVL_ID,
                                                                       parentescoObj = new LlaveValorDto
                                                                       {
                                                                           id = s.CVL_ID,
                                                                           descripcion = s.CVL_DESCRIPCION
                                                                       },
                                                                       esAutorizado = s.TER_ESAUTORIZADO,
                                                                       PrimerNombre = s.TER_PRIMER_NOMBRE,
                                                                       SegundoNombre = s.TER_SEGUNDO_NOMBRE,
                                                                       PrimerApellido = s.TER_PRIMER_APELLIDO,
                                                                       SegundoApellido = s.TER_SEGUNDO_APELLIDO,
                                                                   }).ToList();


                    respuesta.Data.SolicitudBeneficiarioDto = (from s in await _solicitudRepository.ConsultarSolicitudBeneficiario(respuesta.Data.solicitudId, SolicitudConstants.TipoBeneficiario)
                                                                   select new SolicitudBeneficiarioDto
                                                                   {
                                                                       id = s.TER_ID,
                                                                       TipoDoc = s.TID_ID_FK,
                                                                       tipoDocObj = new LlaveValorDto
                                                                       {
                                                                           id = s.TID_ID_FK,
                                                                           descripcion = s.TID_DESCRIPCION
                                                                       },
                                                                       NumDoc = s.TER_NUMERO_DOCUMENTO,
                                                                       Parentesco = s.CVL_ID,
                                                                       parentescoObj = new LlaveValorDto
                                                                       {
                                                                           id = s.CVL_ID,
                                                                           descripcion = s.CVL_DESCRIPCION
                                                                       },
                                                                       PrimerNombre = s.TER_PRIMER_NOMBRE,
                                                                       SegundoNombre = s.TER_SEGUNDO_NOMBRE,
                                                                       PrimerApellido = s.TER_PRIMER_APELLIDO,
                                                                       SegundoApellido = s.TER_SEGUNDO_APELLIDO,
                                                                       ValorRetirarDos = s.TER_VALOR_RETIRAR
                                                                   }).ToList();




                    IEnumerable<RespuestaTerceroEntidadEducativa> entidadEducativa = await _solicitudRepository.ConsultarTerceroEntidadEducativa(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroEntidadEducativa);

                    if (entidadEducativa != null)
                    {
                        respuesta.Data.solicitudTerceroEntidadEducativaDto = entidadEducativa
                            .Select(x => new TerceroEntidadEducativaRespuestaDto
                            {
                                fechalimite = x.TER_FECHA_LIMITE_PAGO,
                                ID = x.TER_ID,
                                idNivel = x.NVE_ID,
                                reciboPago = x.TER_NUM_RECIBO_PAGO,
                                idEntidad = x.PGN_ID,
                                idProgramaEntidad = x.PGN_ID,
                                razonSocial = $"{x.ENE_ID_FK}_{x.TER_NUMERO_DOCUMENTO}_{x.TER_NOMBRE_RAZON_SOCIAL}",
                                Departamento = x.TER_ENT_DEPARTAMENTO,
                                Ciudad = x.TER_ENT_CIUDAD,
                                Modalidad_estudio = x.TER_ENT_MODALIDAD_ESTUDIO,
                                periodoAcademico = x.TER_ENT_PERIODO_ACADEMICO,
                                Horas = x.TER_ENT_HORAS,
                                Fecha_de_inicio = x.TER_ENT_FECHA_INICIO,
                                Fecha_fin = x.TER_ENT_FECHA_FIN,
                                Acreditada = x.TER_ENT_ACREDITADA,
                                ProgramaEducativo = x.ProgramaEducativo,
                                NivelEducativo = x.NivelEducativo,
                                valorRecibo = x.TER_ENT_VALOR_RECIBO
                            }).ToList();
                    }


                    respuesta.Data.solicitudTerceroBeneficiarioEstudioDto = (from x in await _solicitudRepository.ConsultarTerceroBeneficiarioEstudio(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroBeneficiarioEstudioId)
                                                                             select new SolicitudTerceroBeneficiarioEstudioDto
                                                                             {
                                                                                 tipoDocEstudioObj = new LlaveValorStringDto
                                                                                 {
                                                                                     id = x.TID_ID_FK,
                                                                                     descripcion = x.TID_DESCRIPCION
                                                                                 },
                                                                                 numDocEstudio = x.TER_NUMERO_DOCUMENTO,
                                                                                 parentescoEstudio = x.TER_PARENTESCO,
                                                                                 razonsocialEstudio = x.TER_NOMBRE_RAZON_SOCIAL,
                                                                                 tipoDocEstudio = x.TID_ID_FK,
                                                                                 parentescoEstudioObj = new LlaveValorStringDto
                                                                                 {
                                                                                     id = x.TER_PARENTESCO,
                                                                                     descripcion = x.CVL_DESCRIPCION
                                                                                 },
                                                                                 tipoBeneficiario = x.TER_TIPO_TERCERO_FK,
                                                                                 tipoDeBeneficiario = x.tipoDeBeneficiario
                                                                             }).ToList();

                    RespuestaTerceroEntidadSeguroEducativo entidadSeguroEducativo = await _solicitudRepository.ConsultarTerceroEntidadSeguroEducativo(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroEntidadSeguroEducativoId);

                    if (entidadSeguroEducativo != null)
                    {
                        respuesta.Data.solicitudTerceroEntidadSeguroEducativoDto = new SolicitudTerceroEntidadSeguroEducativoDto
                        {
                            numDoc = entidadSeguroEducativo.ESE_NUMERO_IDENTIFICACION,
                            razonSocialAseguradora = entidadSeguroEducativo.ESE_NOMBRE_RAZON_SOCIAL,
                            tipoDocId = entidadSeguroEducativo.TID_ID_FK,
                            entidadSeguroEducativoId = entidadSeguroEducativo.ESE_ID,
                            numeroPoliza = entidadSeguroEducativo.TER_NUM_POLIZA
                        };
                    }

                    SolicitudAdjudicacion SolicitudAdjudicacion = await _solicitudRepository.ConsultarSolicitudAdjudicacion(respuesta.Data.solicitudId);

                    if (SolicitudAdjudicacion != null)
                    {
                        respuesta.Data.SolicitudAdjudicacion = new SolicitudAdjudicacionDto
                        {
                            SIA_ID = SolicitudAdjudicacion.SIA_ID,
                            SOL_ID_FK = SolicitudAdjudicacion.SOL_ID_FK,
                            SIA_TIPO_VIVIENDA = SolicitudAdjudicacion.SIA_TIPO_VIVIENDA,
                            SIA_DIRECCION = SolicitudAdjudicacion.SIA_DIRECCION,
                            SIA_MATRICULA = SolicitudAdjudicacion.SIA_MATRICULA,
                            SIA_VALOR_SUBSIDIADO = SolicitudAdjudicacion.SIA_VALOR_SUBSIDIADO,
                            SIA_CIUDAD = SolicitudAdjudicacion.SIA_CIUDAD
                        };
                    }

                    SolicitudEscrituracion SolicitudEscrituracion = await _solicitudRepository.ConsultarSolicitudEscrituracion(respuesta.Data.solicitudId);

                    if (SolicitudEscrituracion != null)
                    {
                        respuesta.Data.SolicitudEscrituracion = new SolicitudEscrituracionDto
                        {
                            SIE_ID = SolicitudEscrituracion.SIE_ID,
                            SOL_ID_FK = SolicitudEscrituracion.SOL_ID_FK,
                            SIE_NOTARIA = SolicitudEscrituracion.SIE_NOTARIA,
                            SIE_NUMERO_ESCRITURACION = SolicitudEscrituracion.SIE_NUMERO_ESCRITURACION,
                            SIE_FECHA_ESCRITURACION = SolicitudEscrituracion.SIE_FECHA_ESCRITURACION
                        };
                    }

                    RespuestaTerceroTenedorAcciones tenedorAcciones = await _solicitudRepository.ConsultarTerceroTenedorAcciones(respuesta.Data.solicitudId, SolicitudConstants.TipoTerceroTenedorAccionesId);

                    if (tenedorAcciones != null)
                    {
                        respuesta.Data.solicitudTerceroTenedorAccionesDto = new SolicitudTerceroTenedorAccionesDto
                        {
                            TipoDoc = tenedorAcciones.TID_ID_FK,
                            NumDoc = tenedorAcciones.TER_NUMERO_DOCUMENTO,
                            Razonsocial = tenedorAcciones.TER_NOMBRE_RAZON_SOCIAL,
                            Direccion = tenedorAcciones.TER_DIRECCION,
                            Ciudad = tenedorAcciones.DPC_ID,
                            Correo = tenedorAcciones.TER_CORREO_ELECTRONICO,
                            Telefono = tenedorAcciones.TER_TELEFONO,
                            Emisor = tenedorAcciones.TER_EMISOR_NOMBRE_RAZON_SOCIAL,
                            Departamento = tenedorAcciones.DPD_ID
                        };
                    }

                }
            }
            else
            {
                respuesta.Data = new ObtenerTramiteSolicitudDto
                {
                    clienteId = clienteDto.Data.IdCliente,
                    tipoIdentificacion = clienteDto.Data.IdTipoIdentificacion,

                    valorCesantia = sumatoriaCesantia,
                    valorAhorro = sumatoriaAhorro,
                    valorTotal = sumatoriaTotal,
                    SaldosE = clienteSaldostDto.Data.Saldos
                };



            }

            int idEstado = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID;

            if (respuesta.Data.solicitudTareaEstadoId == idEstado) //SolicitudConstants.EstadoTramiteTareaSolicitud
            {
                respuesta.Data.solicitudEncontrada = true;
            }

            if (consultarSolicitudDto.solicitudTareaId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var procesos = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(consultarSolicitudDto.solicitudTareaId);

                if (procesos?.PCS_NOMBRE == "Vivienda 14")
                {
                    IEnumerable<ObtenerInformacionSolicitudCliente> solicitudes = await _solicitudRepository.ObtenerInformacionSolicitudCliente(clienteSaldostDto.Data.Identificacion);
                    var obtenerCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteSaldostDto.Data.IdTipoIdentificacion, clienteSaldostDto.Data.Identificacion);
                    var submodalidadesPrimerPago = await _solicitudRepository.ConsultarSubmodalidadPrimerPago();

                    foreach (var informacionSolicitudes in solicitudes)
                    {
                        foreach (var submodalidadPrimerPago in submodalidadesPrimerPago)
                        {
                            if (informacionSolicitudes.TIM_DESCRIPCION == submodalidadPrimerPago.SPP_DESCRIPCION)
                            {
                                respuesta.IsSuccess = false;
                                respuesta.ReturnMessage = "";
                                return respuesta;
                            }
                        }
                    }
                }
            }






            return respuesta;
        }

        public async Task<Response<TerceroApoderadoRespuestaDto>> ConsultarExistePersona(string numeroDocumento)
        {
            Response<TerceroApoderadoRespuestaDto> respuesta = new Response<TerceroApoderadoRespuestaDto>();
            RespuestaTerceroApoderado respuestaTerceroApoderado = new RespuestaTerceroApoderado();

            respuestaTerceroApoderado = await _solicitudRepository.
                ConsultarTerceroRazonSocial(numeroDocumento);

            if (respuestaTerceroApoderado == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El tercero no existe";
                respuesta.Data = null;

                return respuesta;
            }

            respuesta.IsSuccess = true;
            respuesta.Data = new TerceroApoderadoRespuestaDto()
            {
                numDoc = respuestaTerceroApoderado.TER_NUMERO_DOCUMENTO,
                razonsocial = respuestaTerceroApoderado.TER_NOMBRE_RAZON_SOCIAL,
                PrimerNombre = respuestaTerceroApoderado.TER_PRIMER_NOMBRE,
                SegundoNombre = respuestaTerceroApoderado.TER_SEGUNDO_NOMBRE,
                PrimerApellido = respuestaTerceroApoderado.TER_PRIMER_APELLIDO,
                SegundoApellido = respuestaTerceroApoderado.TER_SEGUNDO_APELLIDO,
                esAutorizado = respuestaTerceroApoderado.TER_ESAUTORIZADO,
                parentesco = respuestaTerceroApoderado.TER_PARENTESCO,
                tipoDocObj = new LlaveValorDto
                {
                    id = respuestaTerceroApoderado.TID_ID_FK,
                    descripcion = respuestaTerceroApoderado.TID_DESCRIPCION
                }
            };

            return respuesta;
        }

        // Consultar propietarios entre solicitudes para integridad de datos, provisional mientras se fusiona tabla props con terceros.
        public async Task<Response<TerceroApoderadoRespuestaDto>> ConsultarExistePropProv(string numeroIdentificacion)
        {
            Response<TerceroApoderadoRespuestaDto> respuesta = new Response<TerceroApoderadoRespuestaDto>();
            SolicitudPropietario solicitudPropietario = new SolicitudPropietario();

            solicitudPropietario = await _solicitudRepository.
               ConsultarPropietarioIntegridad(numeroIdentificacion);

            if (solicitudPropietario == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El propietario no existe";
                respuesta.Data = null;

                return respuesta;
            }

            respuesta.IsSuccess = true;
            respuesta.Data = new TerceroApoderadoRespuestaDto()
            {
                numDoc = solicitudPropietario.PRP_NUMERO_IDENTIFICACION,
                razonsocial = solicitudPropietario.PRP_NOMBRE_RAZON_SOCIAL,
                tipoDocObj = new LlaveValorDto
                {
                    id = solicitudPropietario.TID_ID_FK,
                    descripcion = solicitudPropietario.TID_DESCRIPCION
                },
                PrimerNombre = solicitudPropietario.PrimerNombre,
                SegundoNombre = solicitudPropietario.SegundoNombre,
                PrimerApellido = solicitudPropietario.PrimerApellido,
                SegundoApellido = solicitudPropietario.SegundoApellido
            };

            return respuesta;
        }
        public async Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarSolicitud(FiltroSolicitudes filtro)
        {
            return null;
        }
        public async Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarSolicitudAtendidas(FiltroSolicitudes filtro)
        {
            return null;
        }
        public async Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarTramiteSolicitud(FiltroSolicitudes filtro)
        {
            return null;
        }
        public async Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarTReporteDiarioSolicitudes(FiltroSolicitudes filtro)
        {
            return null;
        }
        public async Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarExisteMatricula(string numeroMatricula)
        {
            Response<MatriculaInmobiliariaRespuestaDto> respuesta = new Response<MatriculaInmobiliariaRespuestaDto>();
            SolicitudMatriculaInmobiliaria matriculaInmobiliaria = new SolicitudMatriculaInmobiliaria();



            matriculaInmobiliaria = await _solicitudRepository.
               ConsultarMatriculaInmobiliaria(numeroMatricula);

            if (matriculaInmobiliaria == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "La matrícula no existe";
                respuesta.Data = null;

                return respuesta;
            }

            respuesta.IsSuccess = true;
            respuesta.Data = new MatriculaInmobiliariaRespuestaDto()
            {
                MaiNumeroMatricula = matriculaInmobiliaria.MAI_NUMERO_MATRICULA,
                MaiDireccion = matriculaInmobiliaria.MAI_DIRECCION,
                DpdId = matriculaInmobiliaria.DPD_ID,
                DpcId = matriculaInmobiliaria.DPC_ID,
                MaiPrincipal = matriculaInmobiliaria.MAI_PRINCIPAL,
                MaiEstado = matriculaInmobiliaria.MAI_ESTADO,
                MaiBloqueo = matriculaInmobiliaria.MAI_BLOQUEO,
                DpdOficinaRegistro = matriculaInmobiliaria.DPD_OFICINA_REGISTRO,
                OfcId = matriculaInmobiliaria.OFC_ID,
                OfcDescripcion = matriculaInmobiliaria.OFC_DESCRIPCION,
                DpdNombre = matriculaInmobiliaria.DPD_NOMBRE,
                DpcNombre = matriculaInmobiliaria.DPC_NOMBRE,
                DpdOfcId = matriculaInmobiliaria.DPD_OFC_ID,
                OfcCodigo = matriculaInmobiliaria.OFC_CODIGO,
                Tramitada = matriculaInmobiliaria.MAI_TRAMITADA
            };

            return respuesta;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="crearSolicitudCompraViviendaDto"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CrearSolicitudTarea(CrearSolicitudTareaDto crearSolicitudTareaDto)
        {
            Response<bool> respuesta = new Response<bool>();

            respuesta.Data = false;
            respuesta.IsSuccess = false;
            if (crearSolicitudTareaDto.SolicitudMatriculaDto.Count != 0)
            {
                crearSolicitudTareaDto.SolicitudMatriculaDto[0].Bandera = 0;
            }
            string mensajeValidacionMatricula = ValidarMatriculas(crearSolicitudTareaDto.SolicitudMatriculaDto);

            if (mensajeValidacionMatricula != "")
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = mensajeValidacionMatricula;
                return respuesta;
            }

            string mensajeValidacionPropietario = ValidarPropietario(crearSolicitudTareaDto.SolicitudPropietarioDto, crearSolicitudTareaDto.SolicitudMatriculaDto);

            if (mensajeValidacionPropietario != "")
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = mensajeValidacionPropietario;
                return respuesta;
            }



            string identificaciones = "";
            identificaciones = String.Join(",", crearSolicitudTareaDto.SolicitudPropietarioDto.Select(x => x.NumeroIdentificacion));
            identificaciones += "," + String.Join(",", crearSolicitudTareaDto.SolicitudTerceroBeneficiarioDto.Select(x => x.NumDoc));

            string validacionVigia = "";

            Response<IEnumerable<ClienteIdentificacionDto>> clienteDto = new Response<IEnumerable<ClienteIdentificacionDto>>();
            clienteDto = await _iClientRequestProvider.ConsultarClientePorIdentificacion(identificaciones);

            foreach (var cliente in clienteDto.Data)
            {
                 
                //Application.Dto.VerificacionTerceroRequest clienteVigia = new Application.Dto.VerificacionTerceroRequest()
                //{
                //    Pe_Dato = cliente.Identificacion,
                //    Pe_Origen = "CORE GA2",
                //    Pe_TipoVerificacion = "1",
                //    Pe_Porcentaje = ""
                //};


                var resultado = await _verificarTerceroVigia.VertificacionTerceroSolicitud(cliente.Identificacion, "1", "100");

                //Response<ResultadoVerificacionTercero> respuestaValidacion = await _vigiaBusinessLogic.VertificacionTerceroAsync(clienteVigia);

                if (!resultado.IsSuccess)
                {

                    respuesta.Data = false;
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = resultado.ReturnMessage;
                    return respuesta;
                }
                
            }

            if (validacionVigia != "")
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = validacionVigia;
                return respuesta;
            }



            ObtenerSolicitud obtenerSolicitud = await _solicitudRepository.ConsultaSolicitud(crearSolicitudTareaDto.SolicitudId);
            Response<ClienteDto> clientesDto = new Response<ClienteDto>();
            clientesDto = await _iClientRequestProvider.ObtenerClienteInformacion(obtenerSolicitud.CLI_ID);
            var obtenerInfoCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clientesDto.Data.IdTipoIdentificacion, clientesDto.Data.Identificacion);

            crearSolicitudTareaDto.Identificacion = obtenerInfoCliente.Data.Identificacion;
            crearSolicitudTareaDto.TipoIdentificacion = obtenerInfoCliente.Data.IdTipoIdentificacion;

            crearSolicitudTareaDto.Opcion = true;


            respuesta = await InsertarSolicitudTarea(crearSolicitudTareaDto);

            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();

            if (respuesta.Data)
            {
                ActualizarSolicitudTarea actualizarSolicitudTarea = new ActualizarSolicitudTarea()
                {
                    SLT_ID = crearSolicitudTareaDto.solicitudTareaId,
                    SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID, //SolicitudConstants.EstadoTramiteTareaSolicitud;
                    SLT_OBSERVACION = crearSolicitudTareaDto.observacion
                };

                await _solicitudRepository.ActualizarSolicitudTarea(actualizarSolicitudTarea);

                ActualizarSolicitud actualizarSolicitud = new ActualizarSolicitud()
                {
                    SOL_ID = crearSolicitudTareaDto.SolicitudId,
                    SOL_VALOR_A_RETIRAR = crearSolicitudTareaDto.valorRetirar,
                    Posee_Avac = crearSolicitudTareaDto.Posee_Avac,
                    NUMERO_FOLIO = crearSolicitudTareaDto.Numero_Folio,
                    FECHA_FIRMA_ESCRITURA = crearSolicitudTareaDto.Fecha_Firma_Escritura != null ? crearSolicitudTareaDto.Fecha_Firma_Escritura : DateTime.Now
                };

                ObtenerSolicitud datosSolicitud = await _solicitudRepository.ConsultaSolicitud(crearSolicitudTareaDto.SolicitudId);
                SolicitudInfo datosSolicit = await _solicitudRepository.ConsultarModadidaYSubmodalidadXSolicitud((Guid)crearSolicitudTareaDto.SolicitudId);
                Response<ClienteDto> clientes = new Response<ClienteDto>();
                clientes = await _iClientRequestProvider.ObtenerClienteInformacion(datosSolicitud.CLI_ID);
                ActualizarRadicadoSolicitudTareaDto actualizarRadicadoSolicitudTareaDto = new ActualizarRadicadoSolicitudTareaDto();

                Response<ClienteCesantiasDto> clienteDto2 = new Response<ClienteCesantiasDto>();

                Response<InfoClienteDto> Infocliente = await _iClientRequestProvider.ObtenerInfoCliente(clientes.Data.IdTipoIdentificacion, clientes.Data.Identificacion);

                var find = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clientes.Data.IdTipoIdentificacion, clientes.Data.Identificacion);
                var cuentas = find.Data.Cuentas;
                var proceso = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(crearSolicitudTareaDto.solicitudTareaId);

                CuentaDto cuentaSeleccionada = new CuentaDto();
                decimal valorRetirar = 0;
                decimal valorRetirarSolicitud = crearSolicitudTareaDto.valorRetirar;
                decimal totalDescuento = 0;
                if (proceso.PCS_NOMBRE == "Vivienda 14" || proceso.PCS_NOMBRE == "Vivienda 8")
                {
                    cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA");
                    IEnumerable<ConceptosAfectadosSolicitud> conceptos = await _solicitudRepository.ConsultarTipoConcepto(cuentaSeleccionada.TipoCuenta);

                    foreach (var conceptosAfectados in cuentaSeleccionada.Conceptos)
                    {
                        var descripcionConcepto = conceptosAfectados.ConceptoDescripcion;

                        decimal saldoConcepto = (decimal)conceptosAfectados.saldo;

                        decimal valorRetirarConcepto = 0;

                        foreach (var tipoConcepto in conceptos)
                        {
                            if (descripcionConcepto == tipoConcepto.CTC_DESCRIPCION)
                            {
                                valorRetirarConcepto = Math.Min(valorRetirarSolicitud - valorRetirar, saldoConcepto);
                                valorRetirar += valorRetirarConcepto;
                            }
                        }


                        CuentasAfectadasSolicitud cuentasAfectadasSolicitud = new CuentasAfectadasSolicitud()
                        {
                            CTC_ID = conceptosAfectados.Id,
                            CTC_DESCRIPCION = descripcionConcepto,
                            SOL_ID_FK = datosSolicitud.SOL_ID,
                            CLI_ID = Infocliente.Data.ClienteId,
                            CLI_IDENTIFICACION = Infocliente.Data.Identificacion,
                            CAC_SALDO = saldoConcepto,
                            CAC_VALOR_A_RETIRAR = valorRetirarConcepto,
                            CAC_VALOR_DESCONTADO = valorRetirarConcepto,
                            NUMERO_CUENTA = cuentaSeleccionada.NumeroCuenta,
                            DESCRIPCION_CUENTA = cuentaSeleccionada.TipoCuenta
                        };
                        totalDescuento += valorRetirarConcepto;

                        if (totalDescuento <= valorRetirarSolicitud && valorRetirarConcepto > 0)
                        {
                            await _solicitudRepository.InsertarConceptoAfectadoCliente(cuentasAfectadasSolicitud);
                        }
                    }
                }
                else
                {

                    IEnumerable<TiposDeCuentaSolicitud> tiposCuentas = await _solicitudRepository.ConsultarTipoCuentas();

                    foreach (var tipoCuenta in tiposCuentas)

                    {
                        IEnumerable<ConceptosAfectadosSolicitud> conceptos = Enumerable.Empty<ConceptosAfectadosSolicitud>();
                        cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == tipoCuenta.CSA_DESCRIPCION);


                        if (cuentaSeleccionada != null)
                        {
                            conceptos = await _solicitudRepository.ConsultarTipoConcepto(cuentaSeleccionada.TipoCuenta);

                            foreach (var conceptosAfectados in cuentaSeleccionada.Conceptos)
                            {
                                var descripcionConcepto = conceptosAfectados.ConceptoDescripcion;
                                decimal saldoConcepto = (decimal)conceptosAfectados.saldo;
                                decimal valorRetirarConcepto = 0;


                                foreach (var tipoConcepto in conceptos)
                                {
                                    if (tipoConcepto.CTC_DESCRIPCION == descripcionConcepto)
                                    {
                                        valorRetirarConcepto = Math.Min(valorRetirarSolicitud - valorRetirar, saldoConcepto);
                                        valorRetirar += valorRetirarConcepto;
                                    }
                                }



                                if (valorRetirarConcepto > 0)
                                {
                                    CuentasAfectadasSolicitud cuentasAfectadasSolicitud = new CuentasAfectadasSolicitud()
                                    {
                                        CTC_ID = conceptosAfectados.Id,
                                        CTC_DESCRIPCION = descripcionConcepto,
                                        SOL_ID_FK = datosSolicitud.SOL_ID,
                                        CLI_ID = Infocliente.Data.ClienteId,
                                        CLI_IDENTIFICACION = Infocliente.Data.Identificacion,
                                        CAC_SALDO = saldoConcepto,
                                        CAC_VALOR_A_RETIRAR = valorRetirarConcepto,
                                        CAC_VALOR_DESCONTADO = valorRetirarConcepto,
                                        NUMERO_CUENTA = cuentaSeleccionada.NumeroCuenta,
                                        DESCRIPCION_CUENTA = cuentaSeleccionada.TipoCuenta
                                    };
                                    totalDescuento += valorRetirarConcepto;

                                    if (totalDescuento <= valorRetirarSolicitud && valorRetirarConcepto > 0)
                                    {
                                        await _solicitudRepository.InsertarConceptoAfectadoCliente(cuentasAfectadasSolicitud);
                                    }
                                }
                            }
                        }
                    }
                }


                PeticionConsultarSolicitudCesantiasDto consultaSolicitud = new PeticionConsultarSolicitudCesantiasDto();
                consultaSolicitud.Identificacion = clientes.Data.Identificacion;

                if (proceso.PCS_DESCRIPCION.Equals("ACREDITACION V8"))
                {
                    var actualizarSolicitudAcreditacion = new ActualizacionSolicitudAcreditacion()
                    {
                        CumpleRequisito = crearSolicitudTareaDto.CumpleRequisito,
                        Causal = crearSolicitudTareaDto.Causal,
                        AccionRealizar = crearSolicitudTareaDto.AccionRealizar,
                        Acuerdo = crearSolicitudTareaDto.Acuerdo,
                        SolicitudId = crearSolicitudTareaDto.SolicitudId
                    };
                    // var radicado = await _gestordocumentalSolicitudes.ActualizarRadicadoSolicitud(actualizarRadicadoSolicitudTareaDto);
                    await _solicitudRepository.ActualizarSolicitud(actualizarSolicitud);
                    await _solicitudRepository.ActualizarObservacionSolicitud(crearSolicitudTareaDto);
                    await _solicitudRepository.ActualizarSolicitudAcreditacion(actualizarSolicitudAcreditacion);
                }
                else
                {

                    var sucursales = await _solicitudRepository.ObtenerSucursales(crearSolicitudTareaDto.InformacionTurnoDto.TipoSucursal.ToString());
                    var recepciones = await _solicitudRepository.ObtenerRecepcion(crearSolicitudTareaDto.InformacionTurnoDto.TipoRecepcion.ToString());
                    var turnos = await _solicitudRepository.ObtenerTurnos(crearSolicitudTareaDto.InformacionTurnoDto.TipoTurno.ToString());

                    Sucursal sucursal = null;
                    Turno turno = null;
                    Recepcion recepcion = null;

                    if (sucursales.ToList().Count > 0) sucursal = sucursales.First();
                    if (turnos.ToList().Count > 0) turno = turnos.First();
                    if (recepciones.ToList().Count > 0) recepcion = recepciones.First();

                    actualizarRadicadoSolicitudTareaDto.tipoIdentificacion = clientes.Data.IdTipoIdentificacion;
                    actualizarRadicadoSolicitudTareaDto.identificacionCliente = clientes.Data.Identificacion;
                    actualizarRadicadoSolicitudTareaDto.CodProcesoGD = datosSolicit.TRAMITE_PAGO;
                    actualizarRadicadoSolicitudTareaDto.CodSubModGD = datosSolicit.MODALIDAD_PAGO;
                    actualizarRadicadoSolicitudTareaDto.NumeroSolicitud = datosSolicitud.SOL_NUMERO_SOLICITUD;
                    actualizarRadicadoSolicitudTareaDto.Radicado = datosSolicit.SOL_RADICADO_GD;
                    actualizarRadicadoSolicitudTareaDto.TipoSolicitud = datosSolicit.PCS_NOMBRE;
                    actualizarRadicadoSolicitudTareaDto.TipoTurno = turno != null ? turno.TIT_NOMBRE : "NO REGISTRA";
                    actualizarRadicadoSolicitudTareaDto.NumeroTurno = crearSolicitudTareaDto.InformacionTurnoDto.Turno;
                    actualizarRadicadoSolicitudTareaDto.CanalRecepcion = recepcion != null ? recepcion.RIT_DESCRIPCION : "Web";
                    actualizarRadicadoSolicitudTareaDto.Sucursal = sucursal != null ? sucursal.SIT_DESCRIPCION_DODO : "";
                    actualizarRadicadoSolicitudTareaDto.CreadoPor = crearSolicitudTareaDto.CreadoPor;
                    actualizarRadicadoSolicitudTareaDto.Numero_Folio = crearSolicitudTareaDto.Numero_Folio;
                    actualizarRadicadoSolicitudTareaDto.TipoVivienda = datosSolicit.TIPO_VIVIENDA;
                    actualizarRadicadoSolicitudTareaDto.Fecha_Firma_Escritura = crearSolicitudTareaDto.Fecha_Firma_Escritura;


                    var radicado = await _gestordocumentalSolicitudes.ActualizarRadicadoSolicitud(actualizarRadicadoSolicitudTareaDto);
                    await _solicitudRepository.ActualizarSolicitud(actualizarSolicitud);
                    await GuardarMisCuentas(datosSolicitud.SOL_ID, crearSolicitudTareaDto.SolicitudMiscuentasDto);
                }
            }
            else
            {
                await EliminarTareasSolicitud(crearSolicitudTareaDto);
            }

            return respuesta;
        }

        public async Task<Response<ObtenerTramiteSolicitudDto>> AprobarSolicitudTarea(AprobarSolicitudTareaDto aprobarSolicitudTareaDto)
        {
            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", aprobarSolicitudTareaDto.clienteId);

            Response<InfoClienteDto> clienteAportesDto = await _iClientRequestProvider.ObtenerInfoCliente(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);

            bool existeProrrateo = clienteAportesDto.Data.Aportes.Any(aportes => aportes.CTG_ID != clienteAportesDto.Data.CategoriaId);
            int prorrateo_v14 = 0;
            if (existeProrrateo)
            {
                prorrateo_v14 = 1;
            }
            else
            {
                prorrateo_v14 = 0;
            }

            var find = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);
            var cuentas = find.Data.Cuentas;
            var proceso = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(aprobarSolicitudTareaDto.solicitudTareaId);

            CuentaDto cuentaSeleccionada = new CuentaDto();

            if (proceso.PCS_NOMBRE == "Vivienda 14" || proceso.PCS_NOMBRE == "Vivienda 8")
            {
                cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA");
                var conceptos = await _solicitudRepository.ConsultarTipoConcepto(cuentaSeleccionada.TipoCuenta);

            }
            else if (proceso.PCS_NOMBRE == "Retiro parcial de cesantías")
            {
                cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS");
            }
            else
            {
                cuentaSeleccionada = cuentas.OrderByDescending(c => c.Saldo).FirstOrDefault();
            }
            
            Response<ObtenerTramiteSolicitudDto> respuesta = new Response<ObtenerTramiteSolicitudDto>();
            respuesta.Data = null;
            respuesta.IsSuccess = false;


            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();
            IEnumerable<Entities.Bpm.EstadoSolicitud> listEstadoSol = await _itareaRepository.ObtenerEstadoSolicitud();

            var EmbargoYBloqueo =
            (find.Data.EmbargosAfiliado != null &&
             find.Data.EmbargosAfiliado.Any(e => e.ESTADO_BLOQUEO == "VIGENTE"))
            ? 3
            : 0;
            AprobarSolicitudTarea actualizarSolicitudTarea = new AprobarSolicitudTarea()
            {
                SLT_ID = aprobarSolicitudTareaDto.solicitudTareaId,
                SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "APROBADA").FirstOrDefault().CVL_ID, // SolicitudConstants.EstadoAprobadaTareaSolicitud,
                SLT_ESTADO_SOLICITUD_NUEVO = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID, // SolicitudConstants.EstadoInicialTareaSolicitud
                SLT_ESTADO_CLIENTE = clienteDto.Data.EstadoCliente,
                SLT_ESTADO_CUENTA = cuentaSeleccionada.IdEct,
                FRZ_CODIGO = clienteDto.Data.FRZ_ID,
                NOMBRE_COMPLETO = clienteDto.Data.PrimerNombre + ' ' + clienteDto.Data.SegundoNombre + ' ' + clienteDto.Data.PrimerApellido + ' ' + clienteDto.Data.SegundoApellido,
                NUM_CEDULA = clienteDto.Data.Identificacion,
                IdGrado = find.Data.IdGrado,
                Prorrateo = prorrateo_v14,
                SOL_RECHAZADA = aprobarSolicitudTareaDto.sOL_RECHAZADA ?? "2",
                NivelRiesgo = aprobarSolicitudTareaDto.NivelRiesgo,
                EmbargoBloqueo = EmbargoYBloqueo
            };

            if (proceso.PCS_NOMBRE == "Acreditación V8")
            {
                respuesta.Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
        this._solicitudRepository.AprobarSolicitudTareaAcreditacion(actualizarSolicitudTarea));
            }
            else
            {
                respuesta.Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
        this._solicitudRepository.AprobarSolicitudTarea(actualizarSolicitudTarea));
            }

            respuesta.IsSuccess = true;
            respuesta.Data.nombresCliente = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre}";
            respuesta.Data.apellidosCliente = $"{clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
            respuesta.Data.identificacionCliente = $"{clienteDto.Data.Identificacion}";
            respuesta.Data.clienteId = clienteDto.Data.IdCliente;
            respuesta.Data.tipoIdentificacion = clienteDto.Data.IdTipoIdentificacion;
            respuesta.Data.telefonoCliente = clienteDto.Data.TelefonoResidencia;
            respuesta.Data.correoCliente = clienteDto.Data.CorreoPersonal;
            respuesta.Data.celularCliente = clienteDto.Data.NumeroCelular;
            respuesta.Data.direccionCliente = clienteDto.Data.Direccion;
            respuesta.Data.estadoCuenta = actualizarSolicitudTarea.SLT_ESTADO_CUENTA;

            return respuesta;
        }



        public async Task<Response<bool>> RechazarSolicitudTarea(RechazarSolicitudTareaDto rechazarSolicitudTareaDto)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = false;
            respuesta.IsSuccess = false;


            ConsultarSolicitudPorActividad consultarSolicitud = new ConsultarSolicitudPorActividad();
            consultarSolicitud.SLT_ID = rechazarSolicitudTareaDto.solicitudTareaId;

            Response<ObtenerTramiteSolicitudDto> DataSolicitud = new Response<ObtenerTramiteSolicitudDto>();
            DataSolicitud = new Response<ObtenerTramiteSolicitudDto>
            {
                Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                    this._solicitudRepository.ConsultarSolicitudPorActividad(consultarSolicitud))
            };


            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();
            IEnumerable<Entities.Bpm.EstadoSolicitud> listEstadoSol = await _itareaRepository.ObtenerEstadoSolicitud();

            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", DataSolicitud.Data.clienteId);



            RechazarSolicitudTarea actualizarSolicitudTarea = new RechazarSolicitudTarea()
            {
                SLT_ID = rechazarSolicitudTareaDto.solicitudTareaId,
                SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "RECHAZADA").FirstOrDefault().CVL_ID,   //SolicitudConstants.EstadoRechazadaTareaSolicitud,
                SLT_ESTADO_SOLICITUD_NUEVO = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID,//SolicitudConstants.EstadoInicialTareaSolicitud,
                SOL_ESTADO_SOLICITUD = listEstadoSol.Where(x => x.CVL_DESCRIPCION == "ANULADA").FirstOrDefault().CVL_ID,  //SolicitudConstants.EstadoAnuladaSolicitud
                NOMBRE_COMPLETO = clienteDto.Data.PrimerNombre + ' ' + clienteDto.Data.SegundoNombre + ' ' + clienteDto.Data.PrimerApellido + ' ' + clienteDto.Data.SegundoApellido,
                NUM_CEDULA = clienteDto.Data.Identificacion,
                SOL_SOLICITUD = DataSolicitud.Data.solicitudId,
                SOL_FECHA_REGISTRO = DateTime.Now
            };

            respuesta.Data = await _solicitudRepository.RechazarSolicitudTarea(actualizarSolicitudTarea);
            respuesta.IsSuccess = true;

            return respuesta;
        }

        public async Task<Response<bool>> AnularSolicitudTarea(RechazarSolicitudTareaDto rechazarSolicitudTareaDto)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = false;
            respuesta.IsSuccess = false;

            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();
            IEnumerable<Entities.Bpm.EstadoSolicitud> listEstadoSol = await _itareaRepository.ObtenerEstadoSolicitud();

            ConsultarSolicitudPorActividad consultarSolicitud = new ConsultarSolicitudPorActividad();
            consultarSolicitud.SLT_ID = rechazarSolicitudTareaDto.solicitudTareaId;

            Response<ObtenerTramiteSolicitudDto> DataSolicitud = new Response<ObtenerTramiteSolicitudDto>();
            DataSolicitud = new Response<ObtenerTramiteSolicitudDto>
            {
                Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                    this._solicitudRepository.ConsultarSolicitudPorActividad(consultarSolicitud))
            };

            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", DataSolicitud.Data.clienteId);


            AnularSolicitudTarea actualizarSolicitudTarea = new AnularSolicitudTarea()
            {
                SLT_ID = rechazarSolicitudTareaDto.solicitudTareaId,
                SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "ANULADA").FirstOrDefault().CVL_ID,   //SolicitudConstants.EstadoRechazadaTareaSolicitud,
                SOL_ESTADO_SOLICITUD = listEstadoSol.Where(x => x.CVL_DESCRIPCION == "ANULADA").FirstOrDefault().CVL_ID,  //SolicitudConstants.EstadoAnuladaSolicitud
                NOMBRE_COMPLETO = clienteDto.Data.PrimerNombre + ' ' + clienteDto.Data.SegundoNombre + ' ' + clienteDto.Data.PrimerApellido + ' ' + clienteDto.Data.SegundoApellido,
                NUM_CEDULA = clienteDto.Data.Identificacion
            };

            respuesta.Data = await _solicitudRepository.AnularSolicitudTarea(actualizarSolicitudTarea);
            respuesta.IsSuccess = true;

            return respuesta;
        }

        public async Task<Response<bool>> SubsanarSolicitudTarea(ActualizarSolicitudTareaDto actualizarSolicitudTarea)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = false;
            respuesta.IsSuccess = false;

            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();

            ConsultarSolicitudPorActividad consultarSolicitud = new ConsultarSolicitudPorActividad();
            consultarSolicitud.SLT_ID = actualizarSolicitudTarea.solicitudTareaId;

            Response<ObtenerTramiteSolicitudDto> DataSolicitud = new Response<ObtenerTramiteSolicitudDto>();
            DataSolicitud = new Response<ObtenerTramiteSolicitudDto>
            {
                Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                    this._solicitudRepository.ConsultarSolicitudPorActividad(consultarSolicitud))
            };

            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", DataSolicitud.Data.clienteId);



            ActualizarSolicitudTarea actualizarSolicitud = new ActualizarSolicitudTarea()
            {
                SLT_ID = actualizarSolicitudTarea.solicitudTareaId,
                SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "EN SUBSANACIÓN").FirstOrDefault().CVL_ID,
                NOMBRE_COMPLETO = clienteDto.Data.PrimerNombre + ' ' + clienteDto.Data.SegundoNombre + ' ' + clienteDto.Data.PrimerApellido + ' ' + clienteDto.Data.SegundoApellido,
                NUM_CEDULA = clienteDto.Data.Identificacion
            };

            respuesta.Data = await _solicitudRepository.ActualizarSolicitudTarea(actualizarSolicitud);
            respuesta.IsSuccess = true;

            return respuesta;
        }


        public async Task<Response<bool>> ActualizarSolicitudTarea(CrearSolicitudTareaDto actualizarSolicitudTareaDto)
        {
            Response<bool> respuesta = new Response<bool>();

            respuesta.Data = false;
            respuesta.IsSuccess = false;
            string mensajeValidacionMatricula = ValidarMatriculas(actualizarSolicitudTareaDto.SolicitudMatriculaDto);

            if (mensajeValidacionMatricula != "")
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = mensajeValidacionMatricula;
                return respuesta;
            }

            string mensajeValidacionPropietario = ValidarPropietario(actualizarSolicitudTareaDto.SolicitudPropietarioDto, actualizarSolicitudTareaDto.SolicitudMatriculaDto);

            if (mensajeValidacionPropietario != "")
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = mensajeValidacionPropietario;
                return respuesta;
            }


            ObtenerSolicitud obtenerSolicitud = await _solicitudRepository.ConsultaSolicitud(actualizarSolicitudTareaDto.SolicitudId);
            Response<ClienteDto> clientesDto = new Response<ClienteDto>();
            clientesDto = await _iClientRequestProvider.ObtenerClienteInformacion(obtenerSolicitud.CLI_ID);
            var obtenerInfoCliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clientesDto.Data.IdTipoIdentificacion, clientesDto.Data.Identificacion);

            actualizarSolicitudTareaDto.Identificacion = obtenerInfoCliente.Data.Identificacion;
            actualizarSolicitudTareaDto.TipoIdentificacion = obtenerInfoCliente.Data.IdTipoIdentificacion;
            actualizarSolicitudTareaDto.Opcion = true;
            if (actualizarSolicitudTareaDto.SolicitudMatriculaDto.Count != 0)
            {
                actualizarSolicitudTareaDto.SolicitudMatriculaDto[0].Bandera = 1;
            }

            if (!obtenerSolicitud.SOL_RADICADO_GD.StartsWith("37"))
            {
                respuesta = await InsertarSolicitudTarea(actualizarSolicitudTareaDto);
            }

            if (respuesta.Data)
            {
                ActualizarSolicitud actualizarSolicitud = new ActualizarSolicitud()
                {
                    SOL_ID = actualizarSolicitudTareaDto.SolicitudId,
                    SOL_VALOR_A_RETIRAR = actualizarSolicitudTareaDto.valorRetirar,
                    NUMERO_FOLIO = actualizarSolicitudTareaDto.Numero_Folio,
                    FECHA_FIRMA_ESCRITURA = actualizarSolicitudTareaDto.Fecha_Firma_Escritura != null ? actualizarSolicitudTareaDto.Fecha_Firma_Escritura : DateTime.Now
                };

                ObtenerSolicitud datosSolicitud = await _solicitudRepository.ConsultaSolicitud(actualizarSolicitudTareaDto.SolicitudId);
                SolicitudInfo datosSolicit = await _solicitudRepository.ConsultarModadidaYSubmodalidadXSolicitud((Guid)actualizarSolicitudTareaDto.SolicitudId);
                Response<ClienteDto> clientes = new Response<ClienteDto>();
                clientes = await _iClientRequestProvider.ObtenerClienteInformacion(datosSolicitud.CLI_ID);
                ActualizarRadicadoSolicitudTareaDto actualizarRadicadoSolicitudTareaDto = new ActualizarRadicadoSolicitudTareaDto();


                //mapear los datos necesarios
                var sucursales = await _solicitudRepository.ObtenerSucursales(actualizarSolicitudTareaDto.InformacionTurnoDto.TipoSucursal.ToString());
                var recepciones = await _solicitudRepository.ObtenerRecepcion(actualizarSolicitudTareaDto.InformacionTurnoDto.TipoRecepcion.ToString());
                var turnos = await _solicitudRepository.ObtenerTurnos(actualizarSolicitudTareaDto.InformacionTurnoDto.TipoTurno.ToString());

                Sucursal sucursal = null;
                Turno turno = null;
                Recepcion recepcion = null;


                if (sucursales.ToList().Count > 0) sucursal = sucursales.First();
                if (turnos.ToList().Count > 0) turno = turnos.First();
                if (recepciones.ToList().Count > 0) recepcion = recepciones.First();



                if (!datosSolicit.PCS_NOMBRE.Equals("ACREDITACIÓN V8"))
                {
                    actualizarRadicadoSolicitudTareaDto.tipoIdentificacion = clientes.Data.IdTipoIdentificacion;
                    actualizarRadicadoSolicitudTareaDto.identificacionCliente = clientes.Data.Identificacion;
                    actualizarRadicadoSolicitudTareaDto.CodProcesoGD = datosSolicit.TRAMITE_PAGO;
                    actualizarRadicadoSolicitudTareaDto.CodSubModGD = datosSolicit.MODALIDAD_PAGO;
                    actualizarRadicadoSolicitudTareaDto.NumeroSolicitud = datosSolicitud.SOL_NUMERO_SOLICITUD;
                    actualizarRadicadoSolicitudTareaDto.Radicado = datosSolicit.SOL_RADICADO_GD;
                    actualizarRadicadoSolicitudTareaDto.TipoSolicitud = datosSolicit.PCS_NOMBRE;
                    actualizarRadicadoSolicitudTareaDto.TipoTurno = turno != null ? turno.TIT_NOMBRE : "NO REGISTRA";
                    actualizarRadicadoSolicitudTareaDto.NumeroTurno = actualizarSolicitudTareaDto.InformacionTurnoDto.Turno;
                    actualizarRadicadoSolicitudTareaDto.CanalRecepcion = recepcion != null ? recepcion.RIT_DESCRIPCION : "Web";
                    actualizarRadicadoSolicitudTareaDto.Sucursal = sucursal != null ? sucursal.SIT_DESCRIPCION_DODO : "";
                    actualizarRadicadoSolicitudTareaDto.CreadoPor = actualizarSolicitudTareaDto.CreadoPor;
                    actualizarRadicadoSolicitudTareaDto.Numero_Folio = actualizarSolicitudTareaDto.Numero_Folio;
                    actualizarRadicadoSolicitudTareaDto.TipoVivienda = datosSolicit.TIPO_VIVIENDA;
                    actualizarRadicadoSolicitudTareaDto.Fecha_Firma_Escritura = actualizarSolicitudTareaDto.Fecha_Firma_Escritura;
                    var radicado = await _gestordocumentalSolicitudes.ActualizarRadicadoSolicitud(actualizarRadicadoSolicitudTareaDto);
                }

                await _solicitudRepository.ActualizarSolicitud(actualizarSolicitud);
                IEnumerable<SolicitudTerceroApoderado> terceros = await _solicitudRepository.ConsultarTercerosPorSolicitud(actualizarSolicitudTareaDto.solicitudTareaId);
               
                foreach (SolicitudTerceroBeneficiarioDto item in actualizarSolicitudTareaDto.SolicitudTerceroBeneficiarioDto.Distinct())
                {
                    var tercero = terceros.FirstOrDefault(t =>
                        t.TER_NUMERO_DOCUMENTO == item.NumDoc);

                    if (tercero != null)
                    {
                        await _solicitudRepository.ActualizarTerceroBeneficiario(
                            actualizarSolicitudTareaDto.SolicitudId,
                            tercero.TER_ID,
                            item.TipoDoc,
                            item.NumDoc,
                            item.Razonsocial,
                            item.TipoDoc,
                            item.Entidad ?? Guid.Empty,
                            item.NumeroCuenta,
                            item.ValorRetirarDos ?? default(int)
                        );
                    }
                }
                await GuardarMisCuentas(datosSolicitud.SOL_ID, actualizarSolicitudTareaDto.SolicitudMiscuentasDto);
            }

            if (obtenerSolicitud.SOL_RADICADO_GD.StartsWith("37"))
            {
                ActualizarSolicitud actualizarSolicitud = new ActualizarSolicitud()
                {
                    SOL_ID = actualizarSolicitudTareaDto.SolicitudId,
                    SOL_VALOR_A_RETIRAR = actualizarSolicitudTareaDto.valorRetirar,
                    NUMERO_FOLIO = actualizarSolicitudTareaDto.Numero_Folio,
                    FECHA_FIRMA_ESCRITURA = actualizarSolicitudTareaDto.Fecha_Firma_Escritura != null ? actualizarSolicitudTareaDto.Fecha_Firma_Escritura : DateTime.Now
                };
                var actualizarSolicitudAcreditacion = new ActualizacionSolicitudAcreditacion()
                {
                    CumpleRequisito = actualizarSolicitudTareaDto.CumpleRequisito,
                    Causal = actualizarSolicitudTareaDto.Causal,
                    AccionRealizar = actualizarSolicitudTareaDto.AccionRealizar,
                    Acuerdo = actualizarSolicitudTareaDto.Acuerdo,
                    SolicitudId = actualizarSolicitudTareaDto.SolicitudId
                };
                await _solicitudRepository.ActualizarSolicitud(actualizarSolicitud);
                await GuardarMisCuentas(actualizarSolicitudTareaDto.SolicitudId, actualizarSolicitudTareaDto.SolicitudMiscuentasDto);
                await _solicitudRepository.ActualizarSolicitudAcreditacion(actualizarSolicitudAcreditacion);
                respuesta.IsSuccess = true;
            }
            return respuesta;
        }

        private async Task GuardarMisCuentas(
            Guid solicitudId,
            MisCuentasDto misCuentas)
        {
            if (misCuentas == null)
                return;

            SolicitudCuenta solicitudCuenta = new()
            {
                SOL_ID = solicitudId,
                SALDO_CUENTA = misCuentas.SaldoCuenta,
                CUENTA = misCuentas.Cuenta,
                TRASLADO_AUTORIZADO = misCuentas.TrasladoAutorizado
            };

            await _solicitudRepository.GuardarMisCuentas(solicitudCuenta);
        }

        public async Task<Response<IEnumerable<DocumentosPorSubModalidadDto>>> ConsultarDocumentosPorSubModalidad(ConsultaDocumentoSubModalidadTarea consultaDocumentoSubModalidadTarea)
        {
            var documentos = this._mapper.Map<IEnumerable<DocumentosPorSubModalidadDto>>(
                await this._solicitudRepository.ConsultarDocumentosPorSubModalidad(
                    consultaDocumentoSubModalidadTarea.idSubModalidad,
                    consultaDocumentoSubModalidadTarea.idSolicitudtarea
                )
            );

            foreach (var doc in documentos)
            {
                if (!string.IsNullOrWhiteSpace(doc.nombreArchivo))
                {
                    doc.nombreArchivo = doc.nombreArchivo.Replace('\u00A0', ' ').Trim();
                }


            }

            return new Response<IEnumerable<DocumentosPorSubModalidadDto>>
            {
                Data = documentos,
                IsSuccess = true,
                ReturnMessage = ""
            };
        }


        /// <summary>
        /// Carga los documentos para la solicitud de cesantías
        /// </summary>
        /// <author>Hanson Restrepo</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CargarDocumentosSolicitud(List<CargarDocumentosSolicitudDto> cargarDocumentosSolicitudDto, Guid IdSolicitud)
        {
            var attachNew = new Core.ReferenciasSvc.WorkManager.AttachRequestDto();
            AttachRequestDto attachRequestDto = new AttachRequestDto();
            attachRequestDto.UserOperation = "web";
            attachRequestDto.Files = new List<FileDto>();
            string nombreArchivo = "";

            try
            {
                ObtenerSolicitud solicitud = await _solicitudRepository.ConsultaSolicitud(IdSolicitud);
                attachRequestDto.Radicado = solicitud.SOL_RADICADO_GD;

                foreach (var x in cargarDocumentosSolicitudDto)
                {
                    FileStreamResult fileStreamResult = new FileStreamResult(x.archivosCargados.OpenReadStream(), x.archivosCargados.ContentType);

                    FileInfo fi = new FileInfo(x.archivosCargados.FileName.Split("--")[1]);
                    nombreArchivo = fi.Name;
                    if (fi.Extension == ".pdf" || fi.Extension == ".PDF")
                    {
                        var newName = Path.GetRandomFileName() + fi.Extension.ToLower();
                        var result = await _blobStorageGenericRepository.SubirArchivoBlobStorage("cesantias", newName.ToString(), fileStreamResult);

                        InsertarArchivo insertarArchivo = new InsertarArchivo();
                        insertarArchivo.AST_NOMBRE_ARCHIVO = fi.Name;
                        insertarArchivo.AST_RUTA_STORAGE = newName;
                        insertarArchivo.AST_EXTENSION = fi.Extension;
                        insertarArchivo.SOL_ID_FK = IdSolicitud;
                        insertarArchivo.PAM_ID_FK = x.archivoParametrizadoId;
                        insertarArchivo.AST_CREATEDOPOR = new Guid();
                        insertarArchivo.AST_NUMERO_FOLIO = x.numeroFolio;

                        attachRequestDto.Files.Add(CrearObjetoWorkmanager(fi, x.archivosCargados));

                        await _solicitudRepository.InsertarArchivoPorSolicitudTarea(insertarArchivo);
                    }
                    else { return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Algunos documentos no fueron almacenados. Solo puede cargar documentos PDF" }; }

                }

                if (attachRequestDto.Files.Count > 0)
                {
                    var responseWorkmanager = await _WMBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                }

            }
            catch (Exception e)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = $"Error al cargar los archivos, {nombreArchivo}" };
            }
            return new Response<bool> { Data = true, IsSuccess = true, ReturnMessage = "Documentos almacenados correctamente" };
        }


        public async Task<Response<IEnumerable<ConsultarArchivoPorSolicitudDto>>> ConsultarArchivoPorSolicitud(Guid IdSolicitud)
        {
            IEnumerable<ConsultarArchivoPorSolicitudDto> archivo = this._mapper.Map<IEnumerable<ConsultarArchivoPorSolicitudDto>>(await this._solicitudRepository.ConsultarArchivoPorSolicitud(IdSolicitud));
            return new Response<IEnumerable<ConsultarArchivoPorSolicitudDto>> { Data = archivo, IsSuccess = true, ReturnMessage = "" };
        }

        public async Task<Response<bool>> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitudDto eliminarArchivoPorSolicitudDto)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = false;
            respuesta.IsSuccess = false;

            //elimina del blobstorage
            var result = await _blobStorageGenericRepository.EliminarArchivoStorage("cesantias", eliminarArchivoPorSolicitudDto.rutaStorage);

            //elimina de la bd
            EliminarArchivoPorSolicitud eliminarArchivo = new EliminarArchivoPorSolicitud()
            {
                AST_ID = eliminarArchivoPorSolicitudDto.idArchivo,
                AST_RUTA_STORAGE = eliminarArchivoPorSolicitudDto.rutaStorage
            };

            respuesta.Data = await _solicitudRepository.EliminarArchivoPorSolicitud(eliminarArchivo);
            respuesta.IsSuccess = true;

            return respuesta;
        }

        public async Task<FileResult> DescargarArchivoPorSolicitud(string rutaStorage)
        {

            var blobCheck = await _blobStorageGenericRepository.DescargarArchivoStorage("cesantias", rutaStorage);
            var descargado = new FileStreamResult(blobCheck.Content, blobCheck.ContentType);
            //descargado.FileDownloadName = rutaStorage;
            //await descargado.FileStream.FlushAsync();
            //respuesta.Data = descargado;
            //BlobContainerClient container = new BlobContainerClient(this._options.CadenaOne, "cesantias");
            //BlobClient blob = container.GetBlobClient(rutaStorage);
            //BlobDownloadInfo download = await blob.DownloadAsync();

            //var descargado = new FileStreamResult(download.Content, download.ContentType);
            descargado.FileDownloadName = rutaStorage;
            await descargado.FileStream.FlushAsync();

            return descargado;
        }

        /// <summary>
        /// Insertar una inconsistencia
        /// </summary>
        /// <param name="insertarInconsistenciaSolicitud"></param>
        /// <returns></returns>
        public async Task<Response<InconsistenciaSolicitudDto>> InsertarInconsistenciaSolicitud(InconsistenciaSolicitudDto insertarInconsistenciaSolicitud)
        {
            var resultadoSolicitud = await this._solicitudRepository.InsertarInconsistenciaSolicitud(
             this._mapper.Map<InconsistenciaSolicitud>(insertarInconsistenciaSolicitud));

            var response = new Response<InconsistenciaSolicitudDto>
            {
                Data = this._mapper.Map<InconsistenciaSolicitudDto>(resultadoSolicitud)
            };



            return response;



        }





        /// <summary>
        /// Insertar una inconsistencia
        /// </summary>
        /// <param name="actualizarInconsistenciaSolicitud"></param>
        /// <returns></returns>
        public async Task<Response<InconsistenciaSolicitudDto>> ActualizarInconsistenciaSolicitud(InconsistenciaSolicitudDto actualizarInconsistenciaSolicitud)
        {
            return new Response<InconsistenciaSolicitudDto>
            {
                Data = this._mapper.Map<InconsistenciaSolicitudDto>(
                    await this._solicitudRepository.ActualizarInconsistenciaSolicitud(
                         this._mapper.Map<InconsistenciaSolicitud>(actualizarInconsistenciaSolicitud)))
            };
        }

        public async Task<Response<IEnumerable<InconsistenciaSolicitudDto>>> ConsultarInconsistenciaSolicitud(Guid IdSolicitud)
        {
            IEnumerable<InconsistenciaSolicitudDto> archivo = this._mapper.Map<IEnumerable<InconsistenciaSolicitudDto>>(await this._solicitudRepository.ConsultarInconsistenciaSolicitud(IdSolicitud));
            return new Response<IEnumerable<InconsistenciaSolicitudDto>> { Data = archivo, IsSuccess = true, ReturnMessage = "" };
        }


        /// <summary>
        /// Metodo logica de negocio para obtener las solicitudes de cesantías
        /// </summary>
        /// <author>Hanson Restrepo</author>
        /// <param name = "solicitud" ></ param >ConsultarSolicitudCesantias
        /// < returns >Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>></ returns >
        public async Task<Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>> ConsultarSolicitudCesantias(PeticionConsultarSolicitudCesantiasDto solicitud)
        {
            IEnumerable<RespuestaConsultarSolicitudCesantiasDto> dataResult;


            var consultaSolicitud = this._mapper.Map<PeticionConsultarSolicitudCesantias>(solicitud);
            var Identificacion = "";
            if (consultaSolicitud.Identificacion != "" && consultaSolicitud.Identificacion != null)
            {
                Identificacion = consultaSolicitud.Identificacion;
                consultaSolicitud.Identificacion = await ObtenerIdCliente(consultaSolicitud.Identificacion);
            }
            IEnumerable<RespuestaConsultarSolicitudCesantiasDto> result = this._mapper.Map<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>(await this._solicitudRepository.ConsultarSolicitudCesantias(consultaSolicitud, (int)10000, (int)solicitud.PageNumber));
            if (Identificacion != "")
            {
                consultaSolicitud.Identificacion = Identificacion;
            }
            var listIdCliente = "";

            // SI HAY IDENTIFICACIONES
            if (solicitud.Identificacion != "" && solicitud.Identificacion != null)
            {
                dataResult = await AsignarNombreEIdentificacion(solicitud.Identificacion, result);
                if (dataResult != null)
                {
                    dataResult = ValidarTareaActualSolicitud(dataResult);
                }
            }
            // SI TRAE TODOS LOS PROCESOS
            else
            {
                if (result.Count() == 0)
                {
                    if (consultaSolicitud.Identificacion == null || consultaSolicitud.Identificacion == "")
                    {
                        return new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> { Data = null, IsSuccess = true, ReturnMessage = "Informacion filtrada no existe" };


                    }
                    return new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> { Data = null, IsSuccess = true, ReturnMessage = "Afiliado no existe" };

                }

                // Crear una lista auxiliar para almacenar IDs únicos
                List<int> idClientesUnicos = new List<int>();

                result.ToList().ForEach(x =>
                {
                    if (!idClientesUnicos.Contains(x.IdCliente))
                    {
                        // Agregar el ID a la cadena y a la lista auxiliar si es único
                        listIdCliente += x.IdCliente + ",";
                        idClientesUnicos.Add(x.IdCliente);
                    }
                });

                // Eliminar la última coma de la cadena si hay elementos
                if (!string.IsNullOrEmpty(listIdCliente))
                {
                    listIdCliente = listIdCliente.Remove(listIdCliente.Length - 1);
                }


                dataResult = ValidarTareaActualSolicitud(await AsignarNombreEIdentificacionProcesos(listIdCliente, result));
            }
            if (dataResult?.Count() > 0)
            {

                return new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> { Data = dataResult, IsSuccess = true, ReturnMessage = "" };
            }
            if (consultaSolicitud.Identificacion == null || consultaSolicitud.Identificacion == "")
            {
                return new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> { Data = null, IsSuccess = true, ReturnMessage = "Informacion filtrada no existe" };

            }
            return new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> { Data = null, IsSuccess = true, ReturnMessage = "Afiliado no existe" };
        }

        private async Task<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> AsignarNombreEIdentificacion(string identificacion, IEnumerable<RespuestaConsultarSolicitudCesantiasDto> result)
        {
            Response<ClienteCedulaDto> clienteDto = new Response<ClienteCedulaDto>();
            clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(identificacion);

            if (clienteDto.Data != null)
            {
                result = result.Where(x => x.IdCliente == clienteDto.Data.IdCliente);

                foreach (var item in result)
                {
                    item.IdTipoIdentificacion = clienteDto.Data.IdTipoIdentificacion;
                    item.CedulaCliente = clienteDto.Data.Identificacion;
                    item.IdCliente = clienteDto.Data.IdCliente;
                    item.NombreCliente = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre} {clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
                }
            }
            else
            {
                result = (IEnumerable<RespuestaConsultarSolicitudCesantiasDto>)clienteDto.Data;
            }

            return result;
        }

        private async Task<string> ObtenerIdCliente(string identificacion)
        {
            Response<ClienteCedulaDto> clienteDto = new Response<ClienteCedulaDto>();
            clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(identificacion);

            return clienteDto.Data != null ? clienteDto.Data.IdCliente.ToString() : "";
        }

        private async Task<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> AsignarNombreEIdentificacionProcesos(string cadenaIds, IEnumerable<RespuestaConsultarSolicitudCesantiasDto> result)
        {
            Response<IEnumerable<ClienteDto>> clienteDto = new Response<IEnumerable<ClienteDto>>();
            int maximoId = 250;
            List<int> ids = cadenaIds.Split(',')
                .Select(int.Parse)
                .ToList();

            List<List<int>> grupos = ids.Select((id, index) => new { id, index })
                .GroupBy(x => x.index / maximoId)
                .Select(g => g.Select(x => x.id).ToList())
                .ToList();
            List<string> idsConver = grupos.Select(x => string.Join(",", x)).ToList();

            var data = new List<RespuestaConsultarSolicitudCesantiasDto>();
            foreach (var item in idsConver)
            {
                clienteDto = await _iClientRequestProvider.ObtenerClientesById(item);

                var joined = result.Join(clienteDto.Data,
                resultKey => resultKey.IdCliente,
                clienteKey => clienteKey.IdCliente,
                (resultKey, clienteKey) => new RespuestaConsultarSolicitudCesantiasDto
                {
                    NumeroSolicitud = resultKey.NumeroSolicitud,
                    ProcesoDescripcion = resultKey.ProcesoDescripcion,
                    IdTipoIdentificacion = clienteKey.IdTipoIdentificacion,
                    CedulaCliente = clienteKey.Identificacion,
                    NombreCliente = clienteKey.PrimerNombre + " " + clienteKey.SegundoNombre + " " + clienteKey.PrimerApellido + " " + clienteKey.SegundoApellido,
                    FechaSolicitud = resultKey.FechaSolicitud,
                    ValorRetirar = resultKey.ValorRetirar,
                    TareaNombre = resultKey.TareaNombre,
                    EstadoSolicitud = resultKey.EstadoSolicitud,
                    IdCliente = resultKey.IdCliente,
                    TimCodigo = resultKey.TimCodigo,
                    TpsSubId = resultKey.TpsSubId,
                    TpsSubDescripcion = resultKey.TpsSubDescripcion,
                    RadicadoGestionDocumental = resultKey.RadicadoGestionDocumental,
                    EstadoSolicitudId = resultKey.EstadoSolicitudId,
                    IdUsuarioAsignado = resultKey.IdUsuarioAsignado,
                    UsuarioAsignado = resultKey.UsuarioAsignado,
                    SolicitudTareaId = resultKey.SolicitudTareaId,
                    SOL_RECHAZADA = resultKey.SOL_RECHAZADA, 
                    ModalidadDescripcion = resultKey.ModalidadDescripcion
                });
                data.AddRange(joined);
            }
            return data;
        }

        public async Task<Response<bool>> CargarDocumentosSolicitudCredito(List<CargarDocumentosSolicitudDto> cargarDocumentosSolicitudDto, Guid IdSolicitud)
        {
            var attachNew = new Core.ReferenciasSvc.WorkManager.AttachRequestDto();
            AttachRequestDto attachRequestDto = new AttachRequestDto();
            attachRequestDto.UserOperation = "web";
            attachRequestDto.Files = new List<FileDto>();

            try
            {
                ObtenerSolicitud solicitud = await _solicitudRepository.ConsultaSolicitud(IdSolicitud);// un nuevo sp para la tabla de credito radicado del GD.
                attachRequestDto.Radicado = solicitud.SOL_RADICADO_GD;

                foreach (var x in cargarDocumentosSolicitudDto)
                {
                    FileStreamResult fileStreamResult = new FileStreamResult(x.archivosCargados.OpenReadStream(), x.archivosCargados.ContentType);

                    FileInfo fi = new FileInfo(x.archivosCargados.FileName.Split("--")[1]);
                    if (fi.Extension == ".pdf" || fi.Extension == ".PDF")
                    {
                        var newName = Path.GetRandomFileName() + fi.Extension.ToLower();
                        var result = await _blobStorageGenericRepository.SubirArchivoBlobStorage("credito", newName.ToString(), fileStreamResult);

                        InsertarArchivo insertarArchivo = new InsertarArchivo();
                        insertarArchivo.AST_NOMBRE_ARCHIVO = fi.Name;
                        insertarArchivo.AST_RUTA_STORAGE = newName;
                        insertarArchivo.AST_EXTENSION = fi.Extension;
                        insertarArchivo.SOL_ID_FK = IdSolicitud;
                        insertarArchivo.PAM_ID_FK = x.archivoParametrizadoId;
                        insertarArchivo.AST_CREATEDOPOR = new Guid();
                        insertarArchivo.AST_NUMERO_FOLIO = x.numeroFolio;

                        attachRequestDto.Files.Add(CrearObjetoWorkmanager(fi, x.archivosCargados));

                        await _solicitudRepository.InsertarArchivoPorSolicitudTarea(insertarArchivo);//VALIDAR DONDE SE GUARDAN LOS DE CREDITOS.
                    }
                    else { return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Algunos documentos no fueron almacenados. Solo puede cargar documentos PDF" }; }

                }

                if (attachRequestDto.Files.Count > 0)
                {
                    var responseWorkmanager = await _WMBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

                }

            }
            catch (Exception e)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Error al cargar los archivos." };
            }
            return new Response<bool> { Data = true, IsSuccess = true, ReturnMessage = "Documentos almacenados correctamente" };
        }

        public async Task<Response<ObservacionSolicitudDto>> ActualizarObservacionesSolicitud(ObservacionSolicitudDto observaciones)
        {
            return new Response<ObservacionSolicitudDto>
            {
                Data = this._mapper.Map<ObservacionSolicitudDto>(
                   await this._solicitudRepository.ActualizarObservacionesSolicitud(this._mapper.Map<ObservacionSolicitud>(observaciones)))
            };
        }

        public async Task<Response<IEnumerable<TrazabilidadSolicitudDto>>> ObtenerTrazabilidadSolicitud(Guid IdSolicitud)
        {
            return new Response<IEnumerable<TrazabilidadSolicitudDto>>
            {
                Data = this._mapper.Map<IEnumerable<TrazabilidadSolicitudDto>>(await this._solicitudRepository.ObtenerTrazabilidadSolicitud(IdSolicitud)).Select(x => { x.FechaCreacion.AddHours(-5); return x; }).OrderByDescending(x => x.FechaCreacion)
            };
        }

        #region Metodos de negocio

        private async Task<Response<bool>> EliminarTareasSolicitud(CrearSolicitudTareaDto eliminarSolicitudTareaDto)
        {

            Response<bool> respuesta = new Response<bool>();

            respuesta.Data = false;
            respuesta.IsSuccess = false;

            await _solicitudRepository.EliminarTerceroSolicitud(eliminarSolicitudTareaDto.SolicitudId);
            await _solicitudRepository.EliminarMatriculaPropietarioSolicitud(eliminarSolicitudTareaDto.SolicitudId);
            await _solicitudRepository.EliminarPropietarioSolicitud(eliminarSolicitudTareaDto.SolicitudId);

            foreach (var objectMatricula in eliminarSolicitudTareaDto.SolicitudMatriculaDto)
            {
                await _solicitudRepository.EliminarMatriculaInmobiliaria(objectMatricula.id);
            }



            respuesta.Data = true;
            respuesta.IsSuccess = true;

            return respuesta;
        }

        private async Task<Response<bool>> DeleteDataSolicitud(CrearSolicitudTareaDto solicitud)
        {
            Response<bool> respuesta = new Response<bool>();
            await this._solicitudRepository.EliminarActualizarDatoSolicitud(solicitud);
            return respuesta;
        }
        /// <summary>
        /// Metodo Crear Documento Solicitud
        /// </summary>
        /// <author>Camilo Brito</author>
        /// <param name="documentoSolicitud"></param>
        /// <returns></returns>
        public async Task<Response<ResponseDocumentoSolicitudes>> CrearDocumentoSolicitud(RequestDocumentoSolicitudDto requestDocumentoSolicitud)
        {
            var attachNew = new Core.ReferenciasSvc.WorkManager.AttachRequestDto();
            AttachRequestDto attachRequestDto = new AttachRequestDto();
            attachRequestDto.UserOperation = "web";
            attachRequestDto.Files = new List<FileDto>();
            string FilePath = requestDocumentoSolicitud.rutaImagen;

            ObtenerSolicitud solicitud = await _solicitudRepository.ConsultaSolicitud(requestDocumentoSolicitud.idSolicitud);
            attachRequestDto.Radicado = solicitud.SOL_RADICADO_GD;


            var fileExtensions = new List<string> { "pdf", "tif" };
            int folios;
            var bdinfo = await _blobStorageGenericRepository.DescargarArchivoStorage("solicitud", $"{requestDocumentoSolicitud.rutaImagen}");
            var fileSResult = new FileStreamResult(bdinfo.Content, bdinfo.ContentType);
            string archivoBase64 = string.Empty;
            byte[] conten;


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

            }
            else folios = 0;

            var filenamewhitoutext = Guid.Parse(FilePath.Split(".")[0]);
            var fileExtension = FilePath.Split(".")[1];
            var documento = new ResponseDocumentoSolicitudes
            {
                DCS_ID = filenamewhitoutext,
                SOC_ID = requestDocumentoSolicitud.idSolicitud,
                DCS_NUMERO_RADICADO = solicitud.SOL_RADICADO_GD,
                DCS_FECHA_DOCUMENTO = DateTime.Now,
                DCS_NUMERO_FOLIOS = folios,
                DCS_TAMAÑO = 0,
                DCS_RUTA_IMAGEN = requestDocumentoSolicitud.rutaImagen,
                DCS_ESTADO = "1"
            };
            TipoDocumentoSolicitudesDto infoTipoDocument = new TipoDocumentoSolicitudesDto();

            infoTipoDocument.Descripcion = requestDocumentoSolicitud.DescripcionDocumento;
            infoTipoDocument.DocumentName = requestDocumentoSolicitud.nombreDocumento;
            infoTipoDocument.DocumentTypeCode = 1;
            infoTipoDocument.DirectoryCode = "00";

            attachRequestDto.Files.Add(CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, fileExtension));

            if (attachRequestDto.Files.Count > 0)
            {
                var responseWorkmanager = await _WMBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

            }
            return new Response<ResponseDocumentoSolicitudes> { Data = this._mapper.Map<ResponseDocumentoSolicitudes>(documento) };

        }

        private FileDto CrearObjetoWorkmanager(string fileBase64, TipoDocumentoSolicitudesDto infoDoc, string fileExtension)
        {
            return new FileDto(fileBase64, infoDoc.DocumentName, infoDoc.DirectoryCode.ToString(), infoDoc.DocumentTypeCode.ToString(), fileExtension, "");
        }

        /// <summary>
        /// Carga los documentos para la solicitud
        /// </summary>
        /// <author>Camilo Brito</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<string>> CargaDocumentoSolicitud(IFormFile request)
        {
            try
            {
                var _request = request;
                FileInfo fi = new FileInfo(_request.FileName);
                BlobContainerClient container = new BlobContainerClient(_optionsDocs.Value.CadenaOne, "solicitud");
                await container.CreateIfNotExistsAsync();
                var newName = Guid.NewGuid() + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);
                var result = await blob.UploadAsync(request.OpenReadStream());
                return new Response<string> { Data = newName };
            }
            catch (Exception)
            {
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardado documento en el Blob Storage" };
            }


        }

        private string ObtenerNumeroMatricula(string input)
        {
            int indiceGuion = input.IndexOf('-');

            if (indiceGuion != -1 && indiceGuion < input.Length - 1)
            {
                return new string(input.Skip(indiceGuion + 1).Where(char.IsDigit).ToArray());
            }

            return string.Empty;
        }

        private string ObtenerNumerosDespuesDelGuion(string input)
        {
            int indiceGuion = input.IndexOf('-');

            if (indiceGuion != -1 && indiceGuion < input.Length - 1)
            {

                string numerosDespuesDelGuion = new string(input
                    .Skip(indiceGuion + 1)
                    .Where(char.IsDigit)
                    .Take(4)
                    .ToArray());

                return numerosDespuesDelGuion;
            }

            return string.Empty;
        }


        private async Task<Response<bool>> InsertarSolicitudTarea(CrearSolicitudTareaDto crearSolicitudCompraViviendaDto)
        {
            Response<bool> respuesta = new Response<bool>();

            SolicitudMatricula matriculaInsertada = new SolicitudMatricula();
            SolicitudInfo datosSolicitud = new SolicitudInfo();
            SubmodalidadSolicitud submodalidades = new SubmodalidadSolicitud();

            if (crearSolicitudCompraViviendaDto.SolicitudMatriculaDto.Count != 0)
            {
                if (crearSolicitudCompraViviendaDto.SolicitudMatriculaDto[0].Bandera == 0)
                {
                    crearSolicitudCompraViviendaDto.SolicitudMatriculaDto = null;

                }
            }

            List<SolicitudMatricula> solicitudMatricula = this._mapper.Map<List<SolicitudMatricula>>(crearSolicitudCompraViviendaDto.SolicitudMatriculaDto);

            string numero_matricula = "";
            int id_ofc = 0;
            foreach (SolicitudMatricula item in solicitudMatricula)
            {
                var resultMatriculas = await _solicitudRepository.ConsultarMatriculaInmobiliaria(item.MAI_NUMERO_MATRICULA);
                item.MAI_FECHA_REGISTRO = DateTime.Now;

                item.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                item.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                item.MAI_NUM_MATRICULA = ObtenerNumeroMatricula(item.MAI_NUMERO_MATRICULA);
                numero_matricula = ObtenerNumerosDespuesDelGuion(item.MAI_NUMERO_MATRICULA);



                id_ofc = item.OFC_ID;

                if (resultMatriculas == null)
                {

                    var matricula = await this._solicitudRepository.ConsultarMatriculaInmobiliariaPorNumeroMatricula(numero_matricula, id_ofc);

                    matriculaInsertada = await this._solicitudRepository.InsertarSolicitudMatricula(item);

                    if (matricula != null)
                    {
                        await this._solicitudRepository.ParamBloqueoMatricula(matriculaInsertada.MAI_ID);
                    }
                    item.MAI_ID = matriculaInsertada.MAI_ID;

                    if (item.MAI_CONYUGE_ACTIVO)
                    {
                        SolicitudPropietario propietarioInsertadoConyuge = new SolicitudPropietario();

                        SolicitudPropietario datoInsertar = new SolicitudPropietario
                        {
                            SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId,
                            CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor,
                            PRP_NOMBRE_RAZON_SOCIAL = "",
                            PRP_NUMERO_IDENTIFICACION = item.MAI_CEDULA_CONYUGE,
                            TID_ID_FK = 1,
                            PrimerNombre = item.MAI_PRIMER_NOMBRE_CONYUGE,
                            SegundoNombre = item.MAI_SEGUNDO_NOMBRE_CONYUGE,
                            PrimerApellido = item.MAI_PRIMER_APELLIDO_CONYUGE,
                            SegundoApellido = item.MAI_SEGUNDO_APELLIDO_CONYUGE,
                            PRP_FECHACREACION = DateTime.Now
                        };


                        SolicitudPropietario propietariosSolicitud = await _solicitudRepository.ConsultarPropietariosPorSolicitud(crearSolicitudCompraViviendaDto.solicitudTareaId, datoInsertar.PRP_NUMERO_IDENTIFICACION);

                        if (propietariosSolicitud.PRP_NUMERO_IDENTIFICACION != datoInsertar.PRP_NUMERO_IDENTIFICACION)
                        {
                            propietarioInsertadoConyuge = await _solicitudRepository.InsertarSolicitudPropietario(datoInsertar);
                            SolicitudPropietarioMatricula insert = new SolicitudPropietarioMatricula();
                            insert.PMA_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                            insert.PRP_ID = propietarioInsertadoConyuge.PRP_ID;
                            insert.MAI_ID = matriculaInsertada.MAI_ID;
                            await _solicitudRepository.InsertarSolicitudPropietarioMatricula(insert);
                        }
                        else
                        {
                            await _solicitudRepository.ActualizarPropietarioMatricula(datoInsertar);
                        }


                    }
                    else
                    {
                        SolicitudPropietario propietarioInsertado = new SolicitudPropietario();

                        foreach (SolicitudPropietarioDto propietaro in crearSolicitudCompraViviendaDto.SolicitudPropietarioDto.Distinct())
                        {
                            if (propietaro.TipoIdentificacion == null)
                            {
                                propietaro.TipoIdentificacion = new TipoIdentificacionMatricula();
                            }

                            SolicitudPropietario datoInsertar = new SolicitudPropietario
                            {
                                SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId,
                                CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor,
                                PRP_NOMBRE_RAZON_SOCIAL = propietaro.RazonSocial,
                                PRP_NUMERO_IDENTIFICACION = propietaro.NumeroIdentificacion,
                                TID_ID_FK = propietaro.TipoIdentificacion.Id,
                                PrimerNombre = propietaro.PrimerNombre,
                                SegundoNombre = propietaro.SegundoNombre,
                                PrimerApellido = propietaro.PrimerApellido,
                                SegundoApellido = propietaro.SegundoApellido,
                                PRP_FECHACREACION = DateTime.Now
                            };

                            SolicitudPropietario propietariosSolicitud = await _solicitudRepository.ConsultarPropietariosPorSolicitud(crearSolicitudCompraViviendaDto.solicitudTareaId, datoInsertar.PRP_NUMERO_IDENTIFICACION);

                            if (propietariosSolicitud == null)
                            {
                                propietarioInsertado = await _solicitudRepository.InsertarSolicitudPropietario(datoInsertar);
                                SolicitudPropietarioMatricula insert = new SolicitudPropietarioMatricula();
                                insert.PMA_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                                insert.PRP_ID = propietarioInsertado.PRP_ID;
                                insert.MAI_ID = matriculaInsertada.MAI_ID;
                                await _solicitudRepository.InsertarSolicitudPropietarioMatricula(insert);
                            }
                            else
                            {
                                await _solicitudRepository.ActualizarPropietarioMatricula(datoInsertar);
                            }


                        }
                    }


                }
                else
                {

                    if (item.MAI_CONYUGE_ACTIVO)
                    {
                        SolicitudPropietario propietarioInsertadoConyuge = new SolicitudPropietario();

                        SolicitudPropietario datoInsertar = new SolicitudPropietario
                        {
                            SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId,
                            CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor,
                            PRP_NOMBRE_RAZON_SOCIAL = "",
                            PRP_NUMERO_IDENTIFICACION = item.MAI_CEDULA_CONYUGE,
                            TID_ID_FK = 1,
                            PrimerNombre = item.MAI_PRIMER_NOMBRE_CONYUGE,
                            SegundoNombre = item.MAI_SEGUNDO_NOMBRE_CONYUGE,
                            PrimerApellido = item.MAI_PRIMER_APELLIDO_CONYUGE,
                            SegundoApellido = item.MAI_SEGUNDO_APELLIDO_CONYUGE,
                            PRP_FECHACREACION = DateTime.Now
                        };


                        SolicitudPropietario propietariosSolicitud = await _solicitudRepository.ConsultarPropietariosPorSolicitud(crearSolicitudCompraViviendaDto.solicitudTareaId, datoInsertar.PRP_NUMERO_IDENTIFICACION);

                        if (propietariosSolicitud == null)
                        {
                            propietarioInsertadoConyuge = await _solicitudRepository.InsertarSolicitudPropietario(datoInsertar);
                            SolicitudPropietarioMatricula insert = new SolicitudPropietarioMatricula();
                            insert.PMA_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                            insert.PRP_ID = propietarioInsertadoConyuge.PRP_ID;
                            insert.MAI_ID = resultMatriculas.MAI_ID;
                            await _solicitudRepository.InsertarSolicitudPropietarioMatricula(insert);
                        }
                        else
                        {
                            await _solicitudRepository.ActualizarPropietarioMatricula(datoInsertar);
                        }

                    }
                    else
                    {
                        SolicitudPropietario propietarioInsertado = new SolicitudPropietario();

                        foreach (SolicitudPropietarioDto propietaro in crearSolicitudCompraViviendaDto.SolicitudPropietarioDto.Distinct())
                        {
                            if (propietaro.TipoIdentificacion == null)
                            {
                                propietaro.TipoIdentificacion = new TipoIdentificacionMatricula();
                            }

                            SolicitudPropietario datoInsertar = new SolicitudPropietario
                            {
                                SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId,
                                CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor,
                                PRP_NOMBRE_RAZON_SOCIAL = propietaro.RazonSocial,
                                PRP_NUMERO_IDENTIFICACION = propietaro.NumeroIdentificacion,
                                TID_ID_FK = propietaro.TipoIdentificacion.Id,
                                PrimerNombre = propietaro.PrimerNombre,
                                SegundoNombre = propietaro.SegundoNombre,
                                PrimerApellido = propietaro.PrimerApellido,
                                SegundoApellido = propietaro.SegundoApellido,
                                PRP_FECHACREACION = DateTime.Now
                            };

                            SolicitudPropietario propietariosSolicitud = await _solicitudRepository.ConsultarPropietariosPorSolicitud(crearSolicitudCompraViviendaDto.solicitudTareaId, datoInsertar.PRP_NUMERO_IDENTIFICACION);

                            if (propietariosSolicitud == null)
                            {
                                propietarioInsertado = await _solicitudRepository.InsertarSolicitudPropietario(datoInsertar);

                                SolicitudPropietarioMatricula insert = new SolicitudPropietarioMatricula();
                                insert.PMA_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                                insert.PRP_ID = propietarioInsertado.PRP_ID;
                                insert.MAI_ID = resultMatriculas.MAI_ID;
                                await _solicitudRepository.InsertarSolicitudPropietarioMatricula(insert);
                            }
                            else
                            {
                                await _solicitudRepository.ActualizarPropietarioMatricula(datoInsertar);
                            }



                        }
                    }

                    item.MAI_ID = resultMatriculas.MAI_ID;

                    DateTime fechaBloqueo = resultMatriculas.MAI_FECHA_REGISTRO;
                    TimeSpan diferencia = DateTime.Now - fechaBloqueo;

                    if (diferencia.TotalMinutes >= (15))
                    {
                        await _solicitudRepository.DesbloqueoMatriculaSolicitud(item.MAI_ID);
                    }
                    item.MAI_FECHA_BLOQUEO = DateTime.Now;


                    await this._solicitudRepository.ActualizarSolicitudMatricula(item);
                }

                if (crearSolicitudCompraViviendaDto.Opcion)
                {
                    await _solicitudRepository.InsertarMatriculaSolicitud(item.SOL_ID_FK, item.MAI_ID);
                }


            }

            IEnumerable<SolicitudTerceroApoderado> tercero = await _solicitudRepository.ConsultarTercerosPorSolicitud(crearSolicitudCompraViviendaDto.solicitudTareaId);


            // 1. LÓGICA PARA APODERADO
            EstadosCatalogosValorSolicitudes APODERADO = await this._solicitudRepository.ConsultarEstadosPorDescripcion("APODERADO");

            foreach (var ter in tercero)
            {
                if (ter.TER_TIPO_TERCERO_FK == APODERADO.CVL_ID)
                {
                    await _solicitudRepository.EliminarTerceroSolicitud(ter.TER_ID);
                }
            }

            foreach (SolicitudTerceroApoderadoDto item in crearSolicitudCompraViviendaDto.SolicitudTerceroApoderadoDto.Distinct())
            {
                SolicitudTerceroApoderado solicitudTerceroApoderado = this._mapper.Map<SolicitudTerceroApoderado>(item);

                solicitudTerceroApoderado.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroApoderado.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudTerceroApoderado.TER_TIPO_TERCERO_FK = APODERADO.CVL_ID; 
                solicitudTerceroApoderado.TER_FECHA_REGISTRO = DateTime.Now;

                await _solicitudRepository.InsertarSolicitudTerceroApoderado(solicitudTerceroApoderado);
            }


           
            EstadosCatalogosValorSolicitudes BENEFICIARIOTERCERO = await this._solicitudRepository.ConsultarEstadosPorDescripcion("BENEFICIARIO");

            
            foreach (var ter in tercero)
            {
                if (ter.TER_TIPO_TERCERO_FK == BENEFICIARIOTERCERO.CVL_ID)
                {
                    await _solicitudRepository.EliminarTerceroSolicitud(ter.TER_ID);
                }
            }

         
            foreach (SolicitudBeneficiarioDto item in crearSolicitudCompraViviendaDto.SolicitudBeneficiarioDto.Distinct())
            {
                SolicitudBeneficiario solicitudBeneficiario = this._mapper.Map<SolicitudBeneficiario>(item);

                solicitudBeneficiario.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudBeneficiario.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudBeneficiario.TER_TIPO_TERCERO_FK = BENEFICIARIOTERCERO.CVL_ID; // Corregido: Usamos el ID de BENEFICIARIO
                solicitudBeneficiario.TER_FECHA_REGISTRO = DateTime.Now;

                await _solicitudRepository.SolicitudBeneficiario(solicitudBeneficiario);
            }


            // Aqui guardas tercero beneficiario
            EstadosCatalogosValorSolicitudes BENEFICIARIO =
                await this._solicitudRepository.ConsultarEstadosPorDescripcion("BENEFICIARIO DE PAGO");
     
            foreach (var ter in tercero)
            {
                if (ter.TER_TIPO_TERCERO_FK == BENEFICIARIO.CVL_ID)
                {
                    await _solicitudRepository
                        .EliminarTerceroSolicitud(ter.TER_ID);
                }
            }


            foreach (SolicitudTerceroBeneficiarioDto item in crearSolicitudCompraViviendaDto
                         .SolicitudTerceroBeneficiarioDto.Distinct())
            {
               
                    SolicitudTerceroBeneficiario solicitudTerceroBeneficiario =
                        this._mapper.Map<SolicitudTerceroBeneficiario>(item);

                    solicitudTerceroBeneficiario.TER_TIPO_TERCERO_FK =
                        SolicitudConstants.TipoTerceroBeneficiarioId;

                    solicitudTerceroBeneficiario.SOL_ID_FK =
                        crearSolicitudCompraViviendaDto.SolicitudId;

                    solicitudTerceroBeneficiario.CREATEDO_POR =
                        crearSolicitudCompraViviendaDto.CreadoPor;

                    solicitudTerceroBeneficiario.TER_FECHA_CREACION =
                        DateTime.Now;

                    await _solicitudRepository
                        .InsertarSolicitudTerceroBeneficiario(solicitudTerceroBeneficiario);
                
            }



            //aqui guarda informacion de adjudicacion
            if (crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto != null)
            {
                InformacionAdjudicacion InformacionAdjudicacion = new InformacionAdjudicacion();
                InformacionAdjudicacion.TipoVivienda = crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto.TipoVivienda;
                InformacionAdjudicacion.Direccion = crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto.Direccion;
                InformacionAdjudicacion.MatriculaInmobiliaria = crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto.MatriculaInmobiliaria;
                InformacionAdjudicacion.ValorSubsidio = crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto.ValorSubsidio;
                InformacionAdjudicacion.SIA_CIUDAD = crearSolicitudCompraViviendaDto.InformacionAdjudicacionDto.SIA_CIUDAD;
                await _solicitudRepository.InsertarSolicitudInformacionAdjudicacion(InformacionAdjudicacion, crearSolicitudCompraViviendaDto.SolicitudId, crearSolicitudCompraViviendaDto.CreadoPor);
            }


            //aqui guarda informacion de Escrituracion
            if (crearSolicitudCompraViviendaDto.InformacionEscrituracionDto != null)
            {
                InformacionEscrituracion InformacionEscrituracion = new InformacionEscrituracion();
                InformacionEscrituracion.NumeroEscritura = crearSolicitudCompraViviendaDto.InformacionEscrituracionDto.NumeroEscritura;
                InformacionEscrituracion.FechaEscrituracion = crearSolicitudCompraViviendaDto.InformacionEscrituracionDto.FechaEscrituracion;
                InformacionEscrituracion.Notaria = crearSolicitudCompraViviendaDto.InformacionEscrituracionDto.Notaria;
                InformacionEscrituracion.FechaEscrituracionRegistro = DateTime.Now;
                await _solicitudRepository.InsertarSolicitudInformacionEscrituracion(InformacionEscrituracion, crearSolicitudCompraViviendaDto.SolicitudId, crearSolicitudCompraViviendaDto.CreadoPor);
            }



            //aqui guardas tercero vendedor
            foreach (SolicitudTerceroVendedorDto item in crearSolicitudCompraViviendaDto.SolicitudTerceroVendedorDto)
            {
                SolicitudTerceroVendedor solicitudTerceroVendedor = this._mapper.Map<SolicitudTerceroVendedor>(item);
                solicitudTerceroVendedor.TER_TIPO_TERCERO_FK = SolicitudConstants.TipoTerceroVendedorId;
                solicitudTerceroVendedor.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroVendedor.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudTerceroVendedor.TER_FECHA_CREACION = DateTime.Now;
                await _solicitudRepository.InsertarSolicitudTerceroVendedor(solicitudTerceroVendedor);
            }

            //aqui guardas tercero constructor
            if (crearSolicitudCompraViviendaDto.SolicitudTerceroConstructorDto != null)
            {
                SolicitudTerceroConstructor solicitudTerceroConstructor = this._mapper.Map<SolicitudTerceroConstructor>(crearSolicitudCompraViviendaDto.SolicitudTerceroConstructorDto);
                solicitudTerceroConstructor.TER_TIPO_TERCERO_FK = SolicitudConstants.TipoTerceroConstructorId;
                solicitudTerceroConstructor.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroConstructor.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                await _solicitudRepository.InsertarSolicitudTerceroConstructor(solicitudTerceroConstructor);
            }

            //aqui guardas tercero autorizado
            if (crearSolicitudCompraViviendaDto.SolicitudTerceroAutorizadoDto != null)
            {
                SolicitudTerceroAutorizado solicitudTerceroAutorizado = this._mapper.Map<SolicitudTerceroAutorizado>(crearSolicitudCompraViviendaDto.SolicitudTerceroAutorizadoDto);
                solicitudTerceroAutorizado.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroAutorizado.CREATEDO_POR = crearSolicitudCompraViviendaDto.CreadoPor;
                await _solicitudRepository.InsertarSolicitudTerceroAutorizado(solicitudTerceroAutorizado);
            }

            //aqui guardas tercero entidad educativa
            if (crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadEducativaDto != null && crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadEducativaDto.Any())
            {
                foreach (var terceroEntidadDto in crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadEducativaDto)
                {
                    SolicitudTerceroEntidadEducativa solicitudTerceroEntidadEducativa = new SolicitudTerceroEntidadEducativa();
                    solicitudTerceroEntidadEducativa.ENE_ID = terceroEntidadDto.IdEntidad;
                    solicitudTerceroEntidadEducativa.ENE_NIT = terceroEntidadDto.Nit;
                    solicitudTerceroEntidadEducativa.ENE_NOMBRE_RAZON_SOCIAL = terceroEntidadDto.RazonSocial;
                    // solicitudTerceroEntidadEducativa.PRN_ID_FK = terceroEntidadDto.IdNivel;
                    solicitudTerceroEntidadEducativa.PRN_ID_FK = null;
                    solicitudTerceroEntidadEducativa.TER_NUM_RECIBO_PAGO = terceroEntidadDto.ReciboPago;
                    solicitudTerceroEntidadEducativa.TER_FECHA_LIMITE_PAGO = terceroEntidadDto.Fechalimite;
                    solicitudTerceroEntidadEducativa.ENE_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                    solicitudTerceroEntidadEducativa.TER_TIPO_TERCERO_FK = SolicitudConstants.TipoTerceroEntidadEducativa;
                    solicitudTerceroEntidadEducativa.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                    solicitudTerceroEntidadEducativa.TID_ID_FK = SolicitudConstants.TipoIdentificacionNit;

                    // Nuevos campos
                    solicitudTerceroEntidadEducativa.TER_ENT_DEPARTAMENTO = terceroEntidadDto.Departamento;
                    solicitudTerceroEntidadEducativa.TER_ENT_CIUDAD = terceroEntidadDto.Ciudad;
                    solicitudTerceroEntidadEducativa.TER_ENT_MODALIDAD_ESTUDIO = terceroEntidadDto.Modalidad_estudio;
                    solicitudTerceroEntidadEducativa.TER_ENT_PERIODO_ACADEMICO = terceroEntidadDto.Periodo_académico;
                    solicitudTerceroEntidadEducativa.TER_ENT_HORAS = terceroEntidadDto.Horas;
                    solicitudTerceroEntidadEducativa.TER_ENT_FECHA_INICIO = terceroEntidadDto.Fecha_de_inicio;
                    solicitudTerceroEntidadEducativa.TER_ENT_FECHA_FIN = terceroEntidadDto.Fecha_fin;
                    solicitudTerceroEntidadEducativa.TER_ENT_ACREDITADA = terceroEntidadDto.Acreditada;
                    solicitudTerceroEntidadEducativa.TER_ENT_VALOR_RECIBO = terceroEntidadDto.valorRecibo;

                    await _solicitudRepository.InsertarSolicitudTerceroEntidadEducativa(solicitudTerceroEntidadEducativa);
                }
            }


            //aqui guardas tercero beneficiario de estudio
            foreach (SolicitudTerceroBeneficiarioEstudioDto item in crearSolicitudCompraViviendaDto.SolicitudTerceroBeneficiarioEstudioDto)
            {
                SolicitudTerceroBeneficiarioEstudio solicitudTerceroBeneficiarioEstudio = new SolicitudTerceroBeneficiarioEstudio();
                solicitudTerceroBeneficiarioEstudio.TER_NUMERO_DOCUMENTO = item.numDocEstudio;
                solicitudTerceroBeneficiarioEstudio.TER_NOMBRE_RAZON_SOCIAL = item.razonsocialEstudio;
                solicitudTerceroBeneficiarioEstudio.TER_PARENTESCO = item.parentescoEstudio;
                solicitudTerceroBeneficiarioEstudio.CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudTerceroBeneficiarioEstudio.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroBeneficiarioEstudio.TID_ID_FK = item.tipoDocEstudio ?? item.tipoDocEstudioObj.id;
                solicitudTerceroBeneficiarioEstudio.TER_TIPO_TERCERO_FK = item.tipoBeneficiario ?? SolicitudConstants.TipoTerceroBeneficiarioEstudioId;

                await _solicitudRepository.InsertarSolicitudTerceroBeneficiarioEstudio(solicitudTerceroBeneficiarioEstudio);
            }

            //aqui guardas tercero entidad seguro educativo
            if (crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto != null)
            {
                SolicitudTerceroEntidadSeguroEducativo solicitudTerceroEntidadSeguroEducativo = new SolicitudTerceroEntidadSeguroEducativo();
                solicitudTerceroEntidadSeguroEducativo.TER_NUMERO_DOCUMENTO = crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto.numDoc;
                solicitudTerceroEntidadSeguroEducativo.TER_NOMBRE_RAZON_SOCIAL = crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto.razonSocialAseguradora;
                solicitudTerceroEntidadSeguroEducativo.TER_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudTerceroEntidadSeguroEducativo.TER_TIPO_TERCERO_FK = SolicitudConstants.TipoTerceroEntidadSeguroEducativoId;
                solicitudTerceroEntidadSeguroEducativo.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroEntidadSeguroEducativo.TID_ID_FK = crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto.tipoDocId;
                solicitudTerceroEntidadSeguroEducativo.ESE_ID_FK = crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto.entidadSeguroEducativoId;
                solicitudTerceroEntidadSeguroEducativo.TER_NUM_POLIZA = crearSolicitudCompraViviendaDto.SolicitudTerceroEntidadSeguroEducativoDto.numeroPoliza;

                await _solicitudRepository.InsertarSolicitudTerceroEntidadSeguroEducativo(solicitudTerceroEntidadSeguroEducativo);
            }

            //aqui guardas tercero tenedor de acciones
            if (crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto != null)
            {
                SolicitudTerceroTenedorAcciones solicitudTerceroTenedorAcciones = new SolicitudTerceroTenedorAcciones();
                solicitudTerceroTenedorAcciones.TER_TIPO_TERCERO_FK = SolicitudConstants.TipoTerceroTenedorAccionesId;
                solicitudTerceroTenedorAcciones.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                solicitudTerceroTenedorAcciones.TER_CREATEDOPOR = crearSolicitudCompraViviendaDto.CreadoPor;
                solicitudTerceroTenedorAcciones.TID_ID_FK = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.TipoDoc;
                solicitudTerceroTenedorAcciones.TER_NUMERO_DOCUMENTO = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.NumDoc;
                solicitudTerceroTenedorAcciones.TER_NOMBRE_RAZON_SOCIAL = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Razonsocial;
                solicitudTerceroTenedorAcciones.TER_DIRECCION = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Direccion;
                solicitudTerceroTenedorAcciones.TER_CORREO_ELECTRONICO = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Correo;
                solicitudTerceroTenedorAcciones.TER_TELEFONO = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Telefono;
                solicitudTerceroTenedorAcciones.TER_EMISOR_NOMBRE_RAZON_SOCIAL = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Emisor;
                solicitudTerceroTenedorAcciones.DPC_ID_FK = crearSolicitudCompraViviendaDto.SolicitudTerceroTenedorAccionesDto.Ciudad;

                await _solicitudRepository.InsertarSolicitudTerceroTenedorAcciones(solicitudTerceroTenedorAcciones);
            }

            //aqui guardas las observaciones
            if (crearSolicitudCompraViviendaDto.observacion != null)
            {
                IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();

                ActualizarSolicitudTarea actualizarSolicitudTarea = new ActualizarSolicitudTarea()
                {
                    SLT_ID = crearSolicitudCompraViviendaDto.solicitudTareaId,
                    SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID, //SolicitudConstants.EstadoTramiteTareaSolicitud;
                    SLT_OBSERVACION = crearSolicitudCompraViviendaDto.observacion
                };

                await this._solicitudRepository.ActualizarSolicitudTarea(actualizarSolicitudTarea);
            }

            if (crearSolicitudCompraViviendaDto.InformacionTurnoDto != null)
            { //Guardar Informacion del turno 
                var informacionTurno = _mapper.Map<InformacionTurno>(crearSolicitudCompraViviendaDto.InformacionTurnoDto);
                informacionTurno.SIT_FECHA_COMISION = (DateTime)crearSolicitudCompraViviendaDto.InformacionTurnoDto.FechaComision;
                informacionTurno.SOL_ID_FK = crearSolicitudCompraViviendaDto.SolicitudId;
                var response = await _solicitudRepository.CrearInformacionTurnoSolicitud(informacionTurno);
            }
            respuesta.Data = true;
            respuesta.IsSuccess = true;

            return respuesta;

        }

        #endregion

        #region Metodos privados
        private string ValidarMatriculas(List<SolicitudMatriculaDto> listaMatricula)
        {
            var resultadoGroupBy = from l in listaMatricula
                                   group l.NumeroMatricula by l.NumeroMatricula into g
                                   select new { NumeroMatricula = g.Key, total = g.Count() };

            var encontroDuplicados = resultadoGroupBy.Where(x => x.total > 1).FirstOrDefault();

            if (encontroDuplicados != null)
            {
                return "Existen matrículas duplicadas";
            }

            var resultadoGroupByEsPrincipal = from l in listaMatricula
                                              group l.EsPrincipal by l.EsPrincipal into g
                                              select new { EsPrincipal = g.Key, total = g.Count() };

            var encontroDuplicadosEsPrincipal = resultadoGroupByEsPrincipal.Where(x => x.total > 1 && x.EsPrincipal == true).FirstOrDefault();

            // validar que solo exista una matricula principal
            if (encontroDuplicadosEsPrincipal != null)
            {
                return "Existe mas de una matrícula principal";
            }

            var resultadoGroupByUnaPrincipal = from l in listaMatricula
                                               group l.EsPrincipal by l.EsPrincipal into g
                                               select new { EsPrincipal = g.Key, total = g.Count() };

            var encontroDuplicadosUnaPrincipal = resultadoGroupByUnaPrincipal.Where(x => x.EsPrincipal == true).FirstOrDefault();

            // validar que solo exista una matricula principal
            if (encontroDuplicadosUnaPrincipal == null && listaMatricula.Count > 0)
            {
                return "Al menos una matrícula debe de ser principal";
            }


            return "";
        }

        /// <summary>
        /// filtra el ultimo proceso que fue gestionado de uan solicitud
        /// </summary>
        /// <param name="listaSolicitudes"></param>
        /// <returns>FileDto</returns>
        private IEnumerable<RespuestaConsultarSolicitudCesantiasDto> ValidarTareaActualSolicitud(IEnumerable<RespuestaConsultarSolicitudCesantiasDto> listaSolicitudes)
        {
            // se aplican las diferentes validadciones para devolver el registro actual
            List<RespuestaConsultarSolicitudCesantiasDto> respuestaConsultarSolicitudCesantiasDtos = new List<RespuestaConsultarSolicitudCesantiasDto>();
            var sublistaAgrupada = listaSolicitudes.GroupBy(x => x.NumeroSolicitud).ToList();
            listaSolicitudes.GroupBy(x => x.NumeroSolicitud).ToList().ForEach(x =>
            {
                var item = x.OrderByDescending(o => o.FechaSolicitud).FirstOrDefault();
                if (item != null)
                {
                    respuestaConsultarSolicitudCesantiasDtos.Add(item);
                }
            });

            return respuestaConsultarSolicitudCesantiasDtos;


        }


        private string ValidarPropietario(List<SolicitudPropietarioDto> listaPropietario, List<SolicitudMatriculaDto> listaMatricula)
        {
            string mensaje = "";
            listaPropietario.ForEach(x =>
            {
                var resultadoGroupBy = from l in x.Matricula
                                       group l by new { l.NumeroMatricula } into g
                                       select new { NumeroIdentificacion = g.Key, total = g.Count() };

                var encontroDuplicados = resultadoGroupBy.Where(x => x.total > 1).FirstOrDefault();

                if (encontroDuplicados != null)
                {
                    mensaje = "Existe un propietario asociado mas de una vez a una misma matrícula";
                    return;
                }
            });

            if (mensaje != "")
            {
                return mensaje;
            }

            if (listaPropietario.Count > 0)
            {
                listaMatricula.ForEach(ma =>
                {
                    var total = listaPropietario.SelectMany(x => x.Matricula).Where(x => x.NumeroMatricula == ma.NumeroMatricula).Count();
                    if (total == 0)
                    {
                        mensaje = "Existen matrículas sin propietario asignado";
                        return;
                    }
                });
            }

            if (mensaje != "")
            {
                return mensaje;
            }


            return "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aprobarSolicitudTareaDto"></param>
        /// <returns></returns>
        public async Task<Response<ObtenerTramiteSolicitudDto>> AprobarSolicitudTareaDocumento(AprobarSolicitudTareaDto aprobarSolicitudTareaDto)
        {

            Response<ClienteDto> clientes = new Response<ClienteDto>();

            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", aprobarSolicitudTareaDto.clienteId);

            clientes = await _iClientRequestProvider.ObtenerClienteInformacion(clienteDto.Data.IdCliente);

            int añoUltimaCuota = clientes.Data.FechaUltimaCuota.Year;
            int reglaRegimenCuotas = 0;
            if (clientes.Data.NumeroCuotas == 168 && (añoUltimaCuota > clientes.Data.Regimen))
            {
                reglaRegimenCuotas = 1;
            }

            var find = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);
            var cuentas = find.Data.Cuentas;
            var proceso = await _solicitudRepository.ObtenerProcesoPorSolicitudTarea(aprobarSolicitudTareaDto.solicitudTareaId);

            Response<InfoClienteDto> clienteAportesDto = await _iClientRequestProvider.ObtenerInfoCliente(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);

            bool existeProrrateo = clienteAportesDto.Data.Aportes.Any(aportes => aportes.CTG_ID != clienteAportesDto.Data.CategoriaId);
            int prorrateo_v14 = 0;
            if (existeProrrateo)
            {
                prorrateo_v14 = 1;
            }
            else
            {
                prorrateo_v14 = 0;
            }

            // Aplicar las condiciones para seleccionar la cuenta adecuada
            CuentaDto cuentaSeleccionada = new CuentaDto();

            if (proceso.PCS_NOMBRE == "Vivienda 14" || proceso.PCS_NOMBRE == "Vivienda 8")
            {
                cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA");
            }
            else if (proceso.PCS_NOMBRE == "Retiro parcial de cesantías")
            {
                cuentaSeleccionada = cuentas.FirstOrDefault(c => c.TipoCuenta == "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS");
            }
            else
            {
                cuentaSeleccionada = cuentas.OrderByDescending(c => c.Saldo).FirstOrDefault();
            }

            Response<ObtenerTramiteSolicitudDto> respuesta = new Response<ObtenerTramiteSolicitudDto>();
            respuesta.Data = null;
            respuesta.IsSuccess = false;


            IEnumerable<Entities.Bpm.EstadoTareaSolicitud> listEstadoTareaSol = await _itareaRepository.ObtenerEstadoSolicitudTarea();
            var EmbargoYBloqueo =
             (
                 // NO existen embargos vigentes
                 !(find.Data.EmbargosAfiliado != null &&
                   find.Data.EmbargosAfiliado.Any(e =>
                       e.ESTADO_BLOQUEO == "VIGENTE"))

                 &&

                 // NO existen valores bloqueados
                 !(find.Data.Cuentas != null &&
                   find.Data.Cuentas.Any(c =>
                       c.ConceptosSaldosBloqueos != null &&
                       c.ConceptosSaldosBloqueos.Any(cs => cs.ValorAcumulado > 0)))
             )
             ? 1 : 0;
            AprobarSolicitudTarea actualizarSolicitudTarea = new AprobarSolicitudTarea()
            {
                SLT_ID = aprobarSolicitudTareaDto.solicitudTareaId,
                SLT_ESTADO_SOLICITUD = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "APROBADA").FirstOrDefault().CVL_ID, // SolicitudConstants.EstadoAprobadaTareaSolicitud,
                SLT_ESTADO_SOLICITUD_NUEVO = listEstadoTareaSol.Where(x => x.CVL_DESCRIPCION == "PENDIENTE").FirstOrDefault().CVL_ID, // SolicitudConstants.EstadoInicialTareaSolicitud
                SLT_ESTADO_CLIENTE = clienteDto.Data.EstadoCliente,
                SLT_ESTADO_CUENTA = cuentaSeleccionada.IdEct,
                FRZ_CODIGO = clienteDto.Data.FRZ_ID,
                NOMBRE_COMPLETO = clienteDto.Data.PrimerNombre + ' ' + clienteDto.Data.SegundoNombre + ' ' + clienteDto.Data.PrimerApellido + ' ' + clienteDto.Data.SegundoApellido,
                NUM_CEDULA = clienteDto.Data.Identificacion,
                IdGrado = find.Data.IdGrado,
                Prorrateo = prorrateo_v14,
                ReglaRegimenCuota = reglaRegimenCuotas,
                SOL_RECHAZADA = null,
                EmbargoBloqueo = EmbargoYBloqueo
            };

            respuesta.Data = this._mapper.Map<ObtenerTramiteSolicitudDto>(await
                    this._solicitudRepository.AprobarSolicitudTarea(actualizarSolicitudTarea));

            Guid idSiguienteTarea = await this._solicitudRepository.ConsultarSiguienteTareaSolicitud(actualizarSolicitudTarea);
            actualizarSolicitudTarea.SLT_ID = idSiguienteTarea;

            respuesta.IsSuccess = true;
            return respuesta;
        }

        /// <summary>
        /// Crea el objeto por cada unos de los archivos a adjuntar
        /// </summary>
        /// <param name="fi"></param>
        /// <returns>FileDto</returns>
        private FileDto CrearObjetoWorkmanager(FileInfo fi, IFormFile file)
        {
            return new FileDto(convertBase64Pdf(file), fi.FullName, "00", "00", ".pdf", "");
        }

        /// <summary>
        /// Convierte el Objeto File en una base 64.
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns>string</returns>
        private string convertBase64Pdf(IFormFile file)
        {

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return Convert.ToBase64String(fileBytes);
            }

        }

        public async Task<Response<InconsistenciaSolicitudDto>> EliminarInconsistenciaSolicitud(Guid inconsistenciaId)
        {
            return new Response<InconsistenciaSolicitudDto>
            {
                Data = this._mapper.Map<InconsistenciaSolicitudDto>(
                    await this._solicitudRepository.EliminarInconsistenciaSolicitud(inconsistenciaId))
            };
        }

        public async Task<Response<FormulariosSolicitudDto>> ObtenerFomulariosSolicitud(string timCodigo)
        {
            IEnumerable<ObtenerFormularioSolicitudDto> data = this._mapper.Map<IEnumerable<ObtenerFormularioSolicitudDto>>(await this._solicitudRepository.ObtenerFomulariosSolicitud(timCodigo));
            FormulariosSolicitudDto formData = new FormulariosSolicitudDto();

            if (data.Count() > 0)
            {
                formData.id = data.First().id;
                formData.titulo = data.First().titulo;
                formData.subtitulo = data.First().subtitulo;
                formData.icono = data.First().icono;
                formData.botonera = data.First().botonera;
            }

            foreach (var dataItem in data)
            {
                formData.formularios.Add(dataItem.formulario);
            }

            return new Response<FormulariosSolicitudDto>
            {
                Data = formData
            };
        }

        public async Task<Response<IEnumerable<ComunicacionSolicitudDto>>> ObtenerComunicacionesSolicitud(Guid IdSolicitud)
        {
            return new Response<IEnumerable<ComunicacionSolicitudDto>>
            {
                Data = this._mapper.Map<IEnumerable<ComunicacionSolicitudDto>>(
                    await this._solicitudRepository.ObtenerComunicacionesSolicitud(IdSolicitud))
            };
        }

        public async Task<Response<ComunicacionSolicitudDto>> InsertarComunicacionSolicitud(ComunicacionSolicitudDto request)
        {
            return new Response<ComunicacionSolicitudDto>
            {
                Data = this._mapper.Map<ComunicacionSolicitudDto>(
                    await this._solicitudRepository.InsertarComunicacionSolicitud(this._mapper.Map<ComunicacionSolicitud>(request)))
            };
        }

        public async Task<Response<IEnumerable<ComunicacionSolicitudDto>>> ActualizarComunicacionesSolicitud(IEnumerable<ComunicacionSolicitudDto> request)
        {
            var data = new List<ComunicacionSolicitudDto>();
            foreach (var item in request)
            {
                var response = this._mapper.Map<ComunicacionSolicitudDto>(
                    await this._solicitudRepository.ActualizarComunicacionesSolicitud(this._mapper.Map<ComunicacionSolicitud>(item))
                    );
                data.Add(response);
            }
            return new Response<IEnumerable<ComunicacionSolicitudDto>>
            {
                Data = data
            };
        }

        public async Task<Response<IEnumerable<TareasSolicitudDto>>> ConsultarAsignacionTareas(Guid IdSolicitud)
        {
            var result = this._mapper.Map<IEnumerable<TareasSolicitudDto>>(await this._solicitudRepository.ConsultarAsignacionTareas(IdSolicitud));
            ///MOSTRAR LAS TAREA DE FONDO DE SOLIDARIDAD
            if (result.Where(x => x.Modulo == "Fondo De Solidaridad").Count() > 0)
            {
                var resultFondo = await this._solicitudRepository.ConsultarAsignacionFondo(IdSolicitud);
                var resultFondoCargueDocument = await this._solicitudRepository.ConsultarAsignacionFondoArchivo(IdSolicitud);
                var resultFondoEntrevista = await this._solicitudRepository.ConsultarAsignacionFondoEntrevista(IdSolicitud);
                var resultFondoVerificate = await this._solicitudRepository.ConsultarAsignacionVerificationFondo(IdSolicitud);
                var resultFondoCalificacion = await this._solicitudRepository.ConsultarAsignacionFondoCalificacion(IdSolicitud);
                foreach (var item in resultFondo)
                {
                    result.Where(x => x.Modulo == "Fondo De Solidaridad" && x.IdFlujo == item.FLJ_ID).ToList()
                        .ForEach(x =>
                        {
                            x.Radicado = item.Radicado;
                            x.TipoModalidad = item.TipoModalidad;
                            x.IdentificacionAfiliado = item.IdentificacionAfiliado;
                            x.TipoSubModalidad = item.TipoSubModalidad;
                            x.TipoSolicitud = item.TipoSolicitud;
                            x.Estado_Tarea = item.Estado_Tarea;
                            x.USR_ENVIA = item.USR_ENVIA;
                            x.Modulo = item.Modulo;
                            x.Descripcion = item.AST_MENSAJE;
                            x.IdFlujo = item.FLJ_ID;
                            x.TRA_NOMBRE = item.NOMBRE_TAREA;
                        }
                    );
                }
                if (resultFondoCargueDocument.Count() > 0)
                {
                    foreach (var item in resultFondoCargueDocument)
                    {
                        result.Where(x => x.Modulo == "Fondo De Solidaridad" && x.IdFlujo == item.FLJ_ID).ToList()
                            .ForEach(x =>
                            {
                                x.Radicado = item.Radicado;
                                x.TipoModalidad = item.TipoModalidad;
                                x.TipoSubModalidad = item.TipoSubModalidad;
                                x.IdentificacionAfiliado = item.IdentificacionAfiliado;
                                x.TipoSolicitud = item.TipoSolicitud;
                                x.Estado_Tarea = item.Estado_Tarea;
                                x.USR_ENVIA = item.USR_ENVIA;
                                x.Modulo = item.Modulo;
                                x.Descripcion = item.AST_MENSAJE;
                                x.IdFlujo = item.FLJ_ID;
                                x.TRA_NOMBRE = item.NOMBRE_TAREA;
                            }
                        );
                    }
                }
                if (resultFondoVerificate.Count() > 0)
                {
                    foreach (var item in resultFondoVerificate)
                    {
                        result.Where(x => x.Modulo == "Fondo De Solidaridad" && x.IdFlujo == item.FLJ_ID).ToList()
                            .ForEach(x =>
                            {
                                x.Radicado = item.Radicado;
                                x.TipoModalidad = item.TipoModalidad;
                                x.TipoSubModalidad = item.TipoSubModalidad;
                                x.IdentificacionAfiliado = item.IdentificacionAfiliado;
                                x.TipoSolicitud = item.TipoSolicitud;
                                x.Estado_Tarea = item.Estado_Tarea;
                                x.USR_ENVIA = item.USR_ENVIA;
                                x.Modulo = item.Modulo;
                                x.Descripcion = item.AST_MENSAJE;
                                x.IdFlujo = item.FLJ_ID;
                                x.TRA_NOMBRE = item.NOMBRE_TAREA;
                            }
                        );
                    }
                }
                if (resultFondoEntrevista.Count() > 0)
                {
                    foreach (var item in resultFondoEntrevista)
                    {
                        result.Where(x => x.Modulo == "Fondo De Solidaridad" && x.IdFlujo == item.FLJ_ID).ToList()
                            .ForEach(x =>
                            {
                                x.Radicado = item.Radicado;
                                x.TipoModalidad = item.TipoModalidad;
                                x.TipoSubModalidad = item.TipoSubModalidad;

                                x.IdentificacionAfiliado = item.IdentificacionAfiliado;
                                x.TipoSolicitud = item.TipoSolicitud;
                                x.Estado_Tarea = item.Estado_Tarea;
                                x.USR_ENVIA = item.USR_ENVIA;
                                x.Modulo = item.Modulo;
                                x.Descripcion = item.AST_MENSAJE;
                                x.IdFlujo = item.FLJ_ID;
                                x.TRA_NOMBRE = item.NOMBRE_TAREA;
                            }
                        );
                    }
                }
                if (resultFondoCalificacion.Count() > 0)
                {
                    foreach (var item in resultFondoCalificacion)
                    {
                        result.Where(x => x.Modulo == "Fondo De Solidaridad" && x.IdFlujo == item.FLJ_ID).ToList()
                            .ForEach(x =>
                            {
                                x.Radicado = item.Radicado;
                                x.TipoModalidad = item.TipoModalidad;
                                x.TipoSubModalidad = item.TipoSubModalidad;
                                x.TipoSolicitud = item.TipoSolicitud;
                                x.IdentificacionAfiliado = item.IdentificacionAfiliado;
                                x.Estado_Tarea = item.Estado_Tarea;
                                x.USR_ENVIA = item.USR_ENVIA;
                                x.Modulo = item.Modulo;
                                x.Descripcion = item.AST_MENSAJE;
                                x.IdFlujo = item.FLJ_ID;
                                x.TRA_NOMBRE = item.NOMBRE_TAREA;
                            }
                        );
                    }
                }
            }

            if (result.Where(x => x.Modulo == "Verificación de Derechos").Count() > 0)
            {
                foreach (var item in result.Where(x => x.Modulo == "Verificación de Derechos"))
                {
                    var dataVerificacion = await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechoBySolId(item.IdFlujo);
                    if (dataVerificacion == null)
                    {
                        item.Radicado = string.Empty;
                        item.TipoModalidad = item.Modulo;
                        item.TipoSubModalidad = item.Modulo;
                        item.TipoSolicitud = item.Modulo;
                        item.IdentificacionAfiliado = string.Empty;
                        item.Estado_Tarea = "Pendiente por procesar";
                        item.USR_ENVIA = item.USR_ENVIA;
                        item.Modulo = item.Modulo;
                        item.Descripcion = item.Descripcion;
                        item.IdFlujo = item.IdFlujo;
                        item.TRA_NOMBRE = "Procesar verificación de derechos";
                    }
                    else
                    {
                        item.Radicado = dataVerificacion?.NumeroRadicado == null ? string.Empty : dataVerificacion?.NumeroRadicado;
                        item.TipoModalidad = item.Modulo;
                        item.TipoSubModalidad = item.Modulo;
                        item.TipoSolicitud = item.Modulo;
                        item.IdentificacionAfiliado = string.Empty;
                        item.Estado_Tarea = dataVerificacion?.NombreEstado == null ? string.Empty : dataVerificacion?.NombreEstado == "Aprobado" ? "APROBADO" : dataVerificacion?.NombreEstado;
                        item.USR_ENVIA = item.USR_ENVIA;
                        item.Modulo = item.Modulo;
                        var cadenDescripcion = item.Descripcion;
                        string[] cadena = cadenDescripcion.Split(" ");
                        var nombreCompleto = cadena[1] + " " + cadena[2] + " " + cadena[3] + "  " + cadena[4];
                        item.Descripcion = $"Sr.(a) {nombreCompleto} se le ha asignado la tarea de solicitud Verificación de Derechos en la etapa de {item.Estado_Tarea} correspondiente al periodo {dataVerificacion.Anio}/{dataVerificacion.Mes}";
                        item.IdFlujo = item.IdFlujo;
                        item.TRA_NOMBRE = "Procesar verificación de derechos";
                        item.AST_FECHA_REGISTRO = item.AST_FECHA_REGISTRO;
                    }
                }
                if (result.Where(x => x.Modulo.ToUpper() == "ABONO DE INTERESES").Count() > 0)
                {
                    foreach (var item in result.Where(x => x.Modulo.ToUpper() == "ABONO DE INTERESES"))
                    {
                        var dataAbono = await _verificacionDerechosRepository.ObtenerAbonoInteresBySolId(item.IdFlujo);
                        if (dataAbono == null)
                        {
                            var cadenDescripcion = item.Descripcion;
                            string[] cadena = cadenDescripcion.Split(" ");
                            var nombreCompleto = cadena[1] + " " + cadena[2] + " " + cadena[3] + "  " + cadena[4];

                            item.Estado_Tarea = "Pendiente por procesar";
                            item.Radicado = item.Radicado;
                            item.TipoModalidad = item.Modulo;
                            item.TipoSubModalidad = item.Modulo;
                            item.TipoSolicitud = item.Modulo;
                            item.USR_ENVIA = item.USR_ENVIA;
                            item.Descripcion = $"Sr.(a) {nombreCompleto} se le ha asignado la tarea de solicitud  Abono de Intereses en la etapa de {item.Estado_Tarea}";
                            item.IdFlujo = item.IdFlujo;
                            item.TRA_NOMBRE = "Procesar Abono de Intereses";
                            item.IdentificacionAfiliado = string.Empty;
                            item.AST_FECHA_REGISTRO = item.AST_FECHA_REGISTRO;
                        }
                        else
                        {

                            var cadenDescripcion = item.Descripcion;
                            string[] cadena = cadenDescripcion.Split(" ");
                            var nombreCompleto = cadena[1] + " " + cadena[2] + " " + cadena[3] + "  " + cadena[4];

                            item.Estado_Tarea = item.Estado_Tarea == null ? string.Empty : item?.Estado_Tarea == "Aprobado" ? "APROBADO" : item?.Estado_Tarea;
                            item.Radicado = item.Radicado;
                            item.TipoModalidad = item.Modulo;
                            item.TipoSubModalidad = item.Modulo;
                            item.TipoSolicitud = item.Modulo;
                            item.USR_ENVIA = item.USR_ENVIA;
                            item.Descripcion = $"Sr.(a) {nombreCompleto} se le ha asignado la tarea de solicitud  Abono de Intereses en la etapa de {item.Estado_Tarea}";
                            item.IdFlujo = item.IdFlujo;
                            item.TRA_NOMBRE = "Procesar Abono de Intereses";
                            item.IdentificacionAfiliado = string.Empty;
                            item.AST_FECHA_REGISTRO = item.AST_FECHA_REGISTRO;

                        }

                    }


                }
            }

            //reglas de negocio
            return new Response<IEnumerable<TareasSolicitudDto>>
            {
                Data = result
            };
        }

        public Task<Response<bool>> ReglasDeNegocioVivienda8(Guid IdSolicitud)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<TareasSolicitudDto>> AsignacionDeTareas(TareasSolicitudDto IdSolicitud)
        {

            return new Response<TareasSolicitudDto>
            {
                Data = this._mapper.Map<TareasSolicitudDto>(
                    await this._solicitudRepository.AsignacionDeTareas(_mapper.Map<TareaSolicitud>(IdSolicitud)))
            };
        }

        public async Task<Response<List<EstadosClienteDto>>> ObtenerEstadosCliente()
        {
            var consulta = await this._solicitudRepository.ObtenerEstadosCliente();
            var respnse = this._mapper.Map<List<EstadosClienteDto>>(consulta);
            return new Response<List<EstadosClienteDto>>
            {
                Data = respnse
            };
        }

        public async Task<Response<List<EstadoCausalClienteDto>>> ObtenerCausalEstadoCliente()
        {
            var consulta = await this._solicitudRepository.ObtenerCausalEstadoCliente();
            var response = this._mapper.Map<List<EstadoCausalClienteDto>>(consulta);
            return new Response<List<EstadoCausalClienteDto>>
            {
                Data = response
            };
        }

        public async Task<Response<InformacionTurnoResponse>> ObtenerInformacionTurno()
        {
            var recepciones = await _solicitudRepository.ObtenerRecepcion();
            var turnos = await _solicitudRepository.ObtenerTurnos();
            var sucursales = await _solicitudRepository.ObtenerSucursales();
            var turnoResponse = new InformacionTurnoResponse()
            {
                Recepciones = recepciones.ToList(),
                Sucursales = sucursales.ToList(),
                Turnos = turnos.ToList()
            };

            return new Response<InformacionTurnoResponse>
            {
                Data = turnoResponse
            };
        }
        public async Task<Response<ReferenciasSvc.WorkManager.FileResponseDto>> ObtenerDocumentosPorRadicado(string numeroRadicado)
        {
            return await _gestordocumentalSolicitudes.ObtenerDocumentosPorRadicado(numeroRadicado);
        }

        public async Task<Response<string>> ObtenerEnlaceDodo(string numeroRadicado)
        {
            return await _gestordocumentalSolicitudes.ObtenerEnlaceDodo(numeroRadicado);
        }

        public async Task<Response<IEnumerable<InconsistenciasDto>>> SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD(Guid IdSolicitud)
        {

            var listadoInconsistencia = await _solicitudRepository.SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD(IdSolicitud);
            return new Response<IEnumerable<InconsistenciasDto>>
            {
                Data = listadoInconsistencia
            };
        }

        public async Task<Response<bool>> CheckDocumentosSolicitud(CheckDocumentosDto checkDocumentos)
        {
            await _solicitudRepository.EliminarCheckArchivos(checkDocumentos.IdSolicitud);

            foreach (var item in checkDocumentos.DocumentosVerificados)
            {
                await _solicitudRepository.CheckArchivosSolicitud(checkDocumentos.IdSolicitud, item);
            }

            return new Response<bool>
            {
                Data = true,
            }; ;
        }

        public async Task<Response<IEnumerable<CheckArchivo>>> ObtenerArchivosCheck(Guid IdSolicitud)
        {
            var archivosChecks = await _solicitudRepository.ObtenerArchivosCheck(IdSolicitud);
            return new Response<IEnumerable<CheckArchivo>>
            {
                Data = archivosChecks
            };
        }



        #endregion

        public async Task<Response<List<ConsultarNovedad>>> ConsultarNovedadSolicitud()
        {
            var novedades = await this._solicitudRepository.ConsultarNovedadSolicitud();
            var response = this._mapper.Map<List<ConsultarNovedad>>(novedades);
            return new Response<List<ConsultarNovedad>>
            {
                Data = response
            };
        }

        public async Task<Response<IEnumerable<ConsultarTipoNovedad>>> ConsultarTipoNovedadSolicitud(string nov_descripcion)
        {
            var tipoNovedades = await _solicitudRepository.ConsultarTipoNovedadSolicitud(nov_descripcion);
            return new Response<IEnumerable<ConsultarTipoNovedad>>
            {
                Data = tipoNovedades
            };
        }

        public async Task<Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>> ConsultarCausalSolicitud(string nov_descripcion)
        {
            var tipoNovedades = await _solicitudRepository.ConsultarCausalInterrupcionSolicitud(nov_descripcion);
            return new Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>
            {
                Data = tipoNovedades
            };
        }

        public async Task<Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>> ConsultarPagoSucesivoSolicitud(string nov_descripcion)
        {
            var tipoNovedades = await _solicitudRepository.ConsultarPagoSucesivoSolicitud(nov_descripcion);
            return new Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>
            {
                Data = tipoNovedades
            };
        }

        public async Task<Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>> ConsultarInformeInconsistenciaSolicitud(string nov_descripcion)
        {
            var tipoNovedades = await _solicitudRepository.ConsultarInformeInconsistenciaSolicitud(nov_descripcion);
            return new Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>
            {
                Data = tipoNovedades
            };
        }

        public async Task<Response<InconsistenciaSolicitudDto>> InsertarInconsistenciaSeguridadSolicitud(InconsistenciaSolicitudDto insertarInconsistenciaSolicitud)
        {
            var resultadoSolicitud = await this._solicitudRepository.InsertarInconsistenciaSeguridadSolicitud(
             this._mapper.Map<InconsistenciaSeguridadSolicitud>(insertarInconsistenciaSolicitud));

            var response = new Response<InconsistenciaSolicitudDto>
            {
                Data = this._mapper.Map<InconsistenciaSolicitudDto>(resultadoSolicitud)
            };

            return response;

        }

        public async Task<Response<IEnumerable<InconsistenciaSolicitudDto>>> ConsultarInconsistenciaSeguridadSolicitud(Guid IdSolicitud)
        {
            IEnumerable<InconsistenciaSolicitudDto> archivo = this._mapper.Map<IEnumerable<InconsistenciaSolicitudDto>>(await this._solicitudRepository.ConsultarInconsistenciasSeguridadSolicitud(IdSolicitud));
            return new Response<IEnumerable<InconsistenciaSolicitudDto>> { Data = archivo, IsSuccess = true, ReturnMessage = "" };
        }

        public async Task<Response<InconsistenciaSeguridadDto>> ActualizarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadDto actualizarInconsistenciaSolicitud)
        {
            return new Response<InconsistenciaSeguridadDto>
            {
                Data = this._mapper.Map<InconsistenciaSeguridadDto>(
                    await this._solicitudRepository.ActualizarInconsistenciaSeguridadSolicitud(
                         this._mapper.Map<InconsistenciaSeguridadSolicitud>(actualizarInconsistenciaSolicitud)))
            };
        }



        public async Task<Response<InconsistenciaSeguridadDto>> EliminarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud inconsistencia)
        {
            return new Response<InconsistenciaSeguridadDto>
            {
                Data = this._mapper.Map<InconsistenciaSeguridadDto>(
                    await this._solicitudRepository.EliminarInconsistenciaSeguridadSolicitud(inconsistencia))
            };
        }

        public async Task<Response<IEnumerable<CuentasAfectadasSolicitud>>> ObtenerConceptosAfectado(ObtenerConceptosAfectadosClientes cuentasAfectadas)
        {
            var conceptosAfectados = await _solicitudRepository.ObtenerConceptosAfectados(cuentasAfectadas);
            return new Response<IEnumerable<CuentasAfectadasSolicitud>>
            {
                Data = conceptosAfectados
            };
        }

        public async Task<Response<List<CuentasAfectadasSolicitud>>> ObtenerConceptosAfectadosSolicitud(string cuentasAfectadas)
        {
            var conceptosAfectados = await _solicitudRepository.ObtenerConceptosAfectadosSolicitud(cuentasAfectadas);
            return new Response<List<CuentasAfectadasSolicitud>>
            {
                Data = conceptosAfectados
            };
        }

        public async Task<Response<ReporteAfiliadosCesantiasResponse>> ReporteDeCesantias(ReporteAfiliadosCesantiasRequestDto request)
        {
            var unidades = await _catalogosRepository.ObtenerUnidadesEjecutoras();
            var grados = await _catalogosRepository.ObtenerGrados();
            var categorias = await _catalogosRepository.ObtenerCategorias();
            var fuerzas = await _catalogosRepository.ObtenerFuerzas();
            var listAfiliaciones = await _catalogosRepository.obtenerTipoActor();

            var resp = await _solicitudRepository.GenerarReportePagoDeCesantias(request);


            foreach (var x in resp)
            {
                Response<ClienteCedulaDto> cliente;
                cliente = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(x.CLI_IDENTIFICACION);

                if (cliente == null) continue;

                 x.IdFuerza = fuerzas.FirstOrDefault(f => f.FRZ_ID == cliente.Data.IdFuerza)?.FRZ_DESCRIPCION; 
                 x.IdUnidadEjecutora = unidades.FirstOrDefault(u => u.UEJ_ID == cliente.Data.IdUnidadEjecutora)?.UEJ_NOMBRE;
                 x.IdGrado = grados.FirstOrDefault(g => g.GRD_ID == cliente.Data.IdGrado)?.GRD_DESCRIPCION;
                 x.IdCategoria = categorias.FirstOrDefault(c => c.CTG_ID == cliente.Data.IdCategoria)?.CTG_DESCRIPCION;
                 x.TipoActor = listAfiliaciones.FirstOrDefault(a => a.TPA_ID == cliente.Data.IdTipoActor)?.TPA_DESCRIPCION;


            }

            return new Response<ReporteAfiliadosCesantiasResponse>
            {
                Data = new ReporteAfiliadosCesantiasResponse { Afiliados = resp },
                IsSuccess = true,
                ReturnMessage = "Reporte generado correctamente"
            };
        }

        public async Task<Response<IEnumerable<CuentasAfectadasSolicitud>>> ObtenerCuentasAfectadas(ObtenerConceptosAfectadosClientes cuentasAfectadas)
        {
            var cuentasAfectadasCliente = await _solicitudRepository.ObtenerCuentasAfectadas(cuentasAfectadas);
            return new Response<IEnumerable<CuentasAfectadasSolicitud>>
            {
                Data = cuentasAfectadasCliente
            };
        }

        public async Task<Response<InformacionConyuge>> InformacionConyuge(InformacionConyugeDto informacionConyuge)
        {
            Response<ClienteDto> clienteDto = new Response<ClienteDto>();
            clienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(informacionConyuge.TipoIdentificacion, informacionConyuge.Identificacion);
            IEnumerable<TipoIdentificacion> tiposIdentificacion = await _catalogosRepository.ObtenerTiposIdentificacion();
            Response<InformacionConyuge> respuesta = new Response<InformacionConyuge>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El afiliado no existe en BUA";
                return respuesta;
            }

            respuesta.Data = new InformacionConyuge();

            foreach (var item in tiposIdentificacion)
            {
                if (item.TID_ID == informacionConyuge.TipoIdentificacion)
                {
                    respuesta.Data.NombreTipoIdentificacionConyuge = item.TID_DESCRIPCION.ToString();
                }
            }

            if (clienteDto.Data != null)
            {
                respuesta.Data.IdConyuge = clienteDto.Data.IdConyuge;
                respuesta.Data.IdTipoIdentificacionConyuge = clienteDto.Data.IdTipoIdentificacionConyuge;
                respuesta.Data.IdentificacionConyuge = clienteDto.Data.IdentificacionConyuge;
                respuesta.Data.PrimerNombreConyuge = clienteDto.Data.PrimerNombreConyuge;
                respuesta.Data.SegundoNombreConyuge = clienteDto.Data.SegundoNombreConyuge;
                respuesta.Data.PrimerApellidoConyuge = clienteDto.Data.PrimerApellidoConyuge;
                respuesta.Data.SegundoApellidoConyuge = clienteDto.Data.SegundoApellidoConyuge;
                respuesta.Data.ConyugeActivo = clienteDto.Data.ConyugeActivo;
            }

            if (respuesta.Data.IdentificacionConyuge == "")
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El afiliado no tiene conyuge registrado";
                return respuesta;
            }

            if (respuesta.Data != null)
            {
                respuesta.IsSuccess = true;
                respuesta.ReturnMessage = "Informacion del conyuge del afiliado";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>> ConsultarSubmodalidadesDePrimerPago()
        {
            var submodadalidadesPrimerPago = await _solicitudRepository.ConsultarSubmodalidadPrimerPago();
            return new Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>
            {
                Data = submodadalidadesPrimerPago
            };
        }

        public async Task<Response<RespuestaConsultarRadicadoSolicitud>> ConsultarRadicadoSolicitud(ConsultarRadicadoSolicitudDto solicitudDto)
        {
            RespuestaConsultarRadicadoSolicitud radicadoSolicitud = await _solicitudRepository.ConsultarRadicadoSolicitud(solicitudDto.SolicitudId);

            return new Response<RespuestaConsultarRadicadoSolicitud>
            {
                IsSuccess = true,
                Data = radicadoSolicitud
            };

        }

        public async Task<Response<ConsultarRadicadoSolicitudDto>> InsertarDescripcionInconsistenciaSolicitud(ConsultarRadicadoSolicitudDto insertarInconsistenciaSolicitud)
        {
            var insertarDescripcion = await this._solicitudRepository.InsertarDescripcionInconsistenciaSolicitud(
             _mapper.Map<RespuestaConsultarRadicadoSolicitud>(insertarInconsistenciaSolicitud));

            var response = new Response<ConsultarRadicadoSolicitudDto>
            {
                Data = this._mapper.Map<ConsultarRadicadoSolicitudDto>(insertarDescripcion)
            };

            return response;

        }

        public async Task<Response<IEnumerable<UsuarioIngresoSolicitud>>> ConsultarUsuarioSolicitud(RespuestaConsultarUsuarioIngresoSolicitud usuarioIngreso)
        {

            RespuestaConsultarUsuarioIngresoSolicitud respuestaUsuarioIngreso = new RespuestaConsultarUsuarioIngresoSolicitud();
            respuestaUsuarioIngreso.Nombres_usuario = usuarioIngreso.Nombres_usuario;
            respuestaUsuarioIngreso.Apellidos_usuario = usuarioIngreso.Apellidos_usuario;

            IEnumerable<UsuarioIngresoSolicitud> usuarios = await _solicitudRepository.ConsultarUsuarioIngresoSolicitud(respuestaUsuarioIngreso);

            var response = new Response<IEnumerable<UsuarioIngresoSolicitud>>
            {
                IsSuccess = true,
                Data = usuarios,
                ReturnMessage = $"Lista de asesores que coinciden con el nombre {usuarioIngreso.Nombres_usuario} {usuarioIngreso.Apellidos_usuario}"
            };


            return response;
        }

        public async Task<Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>> ConsultarSolicitanteBeneficiarioTerceroSolicitud(SolicitanteBeneficiarioTerceroSolicitudDto beneficiante)
        {

            SolicitanteBeneficiarioTerceroSolicitudDto solicitanteBeneficiarioTercero = new SolicitanteBeneficiarioTerceroSolicitudDto();
            solicitanteBeneficiarioTercero.Nombres_solicitante = beneficiante.Nombres_solicitante;
            solicitanteBeneficiarioTercero.Apellidos_solicitante = beneficiante.Apellidos_solicitante;

            IEnumerable<SolicitanteBeneficiarioTerceroSolicitud> solicitantes = await _solicitudRepository.ConsultarSolicitanteApoderadoSolicitud(solicitanteBeneficiarioTercero);

            var response = new Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>
            {
                IsSuccess = true,
                Data = solicitantes,
                ReturnMessage = $"Lista de beneficiante que coinciden con el nombre {solicitanteBeneficiarioTercero.Nombres_solicitante} {solicitanteBeneficiarioTercero.Apellidos_solicitante}"
            };


            return response;
        }

        public async Task<Response<IEnumerable<IntervinientesActor>>> IntervinientesActor()
        {

            IEnumerable<IntervinientesActor> actores = await _solicitudRepository.ObtenerIntervinienteActor();

            var response = new Response<IEnumerable<IntervinientesActor>>
            {
                IsSuccess = true,
                Data = actores,
                ReturnMessage = $"Lista de  actores"
            };


            return response;
        }

        public async Task<Response<IntervinientesActorSolicitud>> RegistrarIntervinientes(IntervinientesActorSolicitudDto intervinientesActor)
        {
            var resultado = await this._solicitudRepository.RegistrarIntervinientesActor(intervinientesActor);
            var response = new Response<IntervinientesActorSolicitud>();
            {
                response.Data = resultado;
                response.IsSuccess = true;
                response.ReturnMessage = "Interviniente registrado exitosamente.";
            }
            return response;

        }



        public async Task<Response<IntervinientesActorSolicitud>> ConsultarIntervinientesPorId(Guid idIntervinientes)
        {
            IntervinientesActorSolicitud interviniente = await _solicitudRepository.ObtenerIntervinientePorId(idIntervinientes);

            var response = new Response<IntervinientesActorSolicitud>
            {
                IsSuccess = true,
                Data = this._mapper.Map<IntervinientesActorSolicitud>(interviniente)
            };

            return response;

        }
        public async Task<Response<List<Tramite>>> TramiteIntervimiente(ConsultaTramitesIntervinientesActorSolicitud interviniente)
        {
            List<Tramite> tramites = await _solicitudRepository.TramitesInterviniente(interviniente);
            Response<ClienteCedulaDto> clienteResponse;

            foreach (var item in tramites)
            {
                clienteResponse = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(item.SOL_CEDULA);

                if (clienteResponse?.Data != null)
                {
                    var cliente = clienteResponse.Data;
                    item.PRP_NOMBRE_RAZON_SOCIAL = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}";
                }
            }

            var response = new Response<List<Tramite>>
            {
                IsSuccess = true,
                Data = this._mapper.Map<List<Tramite>>(tramites)
            };

            return response;
        }


        public async Task<Response<IEnumerable<IntervinientesActorSolicitud>>> FiltrarIntervinientes(BuscarIntervinientesDto intervinientes)
        {

            IEnumerable<IntervinientesActorSolicitud> filtroIntervinientes = await this._solicitudRepository.BuscarIntervinientesActores(
             this._mapper.Map<BuscarIntervinientesDto>(intervinientes));

            var response = new Response<IEnumerable<IntervinientesActorSolicitud>>
            {
                IsSuccess = true,
                Data = filtroIntervinientes,
                ReturnMessage = $"Filtro de intervinientes"
            };


            return response;
        }

        public async Task<Response<List<IntervinientesActorSolicitud>>> ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(int tipoDocumento, string numeroDocumento)
        {
            // Validaciones de negocio
            if (tipoDocumento <= 0)
            {
                return new Response<List<IntervinientesActorSolicitud>>
                {
                    IsSuccess = false,
                    ReturnMessage = "El tipo de documento es inválido",
                    Data = new List<IntervinientesActorSolicitud>()
                };
            }

            if (string.IsNullOrWhiteSpace(numeroDocumento))
            {
                return new Response<List<IntervinientesActorSolicitud>>
                {
                    IsSuccess = false,
                    ReturnMessage = "El número de documento es obligatorio",
                    Data = new List<IntervinientesActorSolicitud>()
                };
            }

            // Normalización
            numeroDocumento = numeroDocumento.Trim();

            // Llamado a repositorio (debe devolver lista)
            var intervinientes = await _solicitudRepository
                .ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(tipoDocumento, numeroDocumento)
                ?? new List<IntervinientesActorSolicitud>();

            // Regla de negocio post-consulta
            if (intervinientes.Count == 0)
            {
                return new Response<List<IntervinientesActorSolicitud>>
                {
                    IsSuccess = false,
                    ReturnMessage = "No se encontraron intervinientes con los datos proporcionados",
                    Data = new List<IntervinientesActorSolicitud>()
                };
            }

            // Respuesta exitosa
            return new Response<List<IntervinientesActorSolicitud>>
            {
                IsSuccess = true,
                Data = intervinientes,
                ReturnMessage = "Intervinientes consultados correctamente"
            };
        }


        public async Task<Response<IntervinientesActorSolicitud>> ActualizarIntervinientes(ActualizarIntervinientesActorSolicitudDto intervinientesActor)
        {
            try
            {
                if (intervinientesActor == null)
                {
                    return new Response<IntervinientesActorSolicitud>
                    {
                        IsSuccess = false,
                        ReturnMessage = "Los datos de intervinientes son obligatorios"
                    };
                }

                var resultadoSolicitud = await _solicitudRepository.ActualizarIntervinienteActor(intervinientesActor);

                if (resultadoSolicitud == null)
                {
                    return new Response<IntervinientesActorSolicitud>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No fue posible actualizar los intervinientes"
                    };
                }

                var data = _mapper.Map<IntervinientesActorSolicitud>(resultadoSolicitud);

                return new Response<IntervinientesActorSolicitud>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (AutoMapperMappingException ex)
            {
                return new Response<IntervinientesActorSolicitud>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error de mapeo: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new Response<IntervinientesActorSolicitud>
                {
                    IsSuccess = false,
                    ReturnMessage = "Ocurrió un error inesperado al actualizar los intervinientes"
                    // 🔒 No expongas ex.Message en producción
                };
            }
        }


        public async Task<Response<IntervinientesActorSolicitud>> EliminarIntervinienteActor(EliminarIntervinientesActorSolicitudDto intervinientesActor)
        {
            var resultadoSolicitud = await _solicitudRepository.EliminarIntervinienteActor(intervinientesActor);
            var response = new Response<IntervinientesActorSolicitud>
            {
                IsSuccess = true,
                ReturnMessage = "Registro eliminado con éxito"
            };

            return response;

        }

        public async Task<Response<string>> CrearRadicadoCartaIncosistencia(CrearRadicadoCartaIncosistencias request)
        {

            var radicado = await _gestordocumentalSolicitudes.RadicarSolicitud378or113(request);

            var radicadoresult = radicado?.Data?.Form_InsertResult.BarCode?.ToString() ?? "0";


            if (radicado.IsSuccess && radicadoresult != "0")
            {
                var data = new ComunicacionSolicitud
                {
                    CMN_ID = Guid.NewGuid(),
                    SOL_ID = request.SolicitudId,
                    CMN_TEXTO_INCLUIR = request.Asunto,
                    CMN_RADICADO = request.Radicado,
                    CMN_RADICADO_RELACIONADO = radicadoresult,
                    CMN_FIRMA_DIGITAL = true,
                    CSC_ID = request.IdComunicacion
                };
                await this._solicitudRepository.InsertarComunicacionSolicitud(data);
                var response = new Response<string>
                {
                    IsSuccess = true,
                    Data = radicadoresult
                };
                return response;
            }
            return new Response<string>
            {
                IsSuccess = false,
                Data = "No se creo el radicacion exitosamente."
            };
        }

        private async Task<DataReportSolicitud> GetDataReport(RequestReporteSolicitud request)
        {
            var responseData = new DataReportSolicitud();
            var solicitudes = new List<RespuestaConsultarSolicitudCesantias>();
            var embargos = new List<Embargo>();
            var estadosSolictud = await _catalogosRepository.ObtenerEstadoTareaSolicitud();
            var estadosEmbargos = await _catalogosRepository.ObtenerValoresCatalogosPorIdCatalogo(72);
            var estadosFs = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();

          
            if (request?.TipoSolicitudes == null)
                {
                    var peticion = new PeticionConsultarSolicitudCesantias
                    {
                        NumRadicado = request?.Radicado,
                        TipoSolicitud = request?.IdTipoSolicitudes,
                        FechaFin = request?.FechaFin,
                        FechaIni = request?.FechaInicio,
                        UsuarioIngreso = request.usuario
                    };
                    var response = await _solicitudRepository.ConsultarSolicitudCesantias(peticion, 100, 10);
                    solicitudes.AddRange(response);
                    responseData.DataSolicitudes = solicitudes;
                }
                if (request?.IdTipoSolicitudes == null && (request?.TipoSolicitudes == 598 || request?.TipoSolicitudes == null))

                {

                    var peticionEmbargo = new ConsultaEmbargosfiltro()
                    {
                        FECHA_FINAL = request?.FechaFin,
                        FECHA_INICIAL = request?.FechaInicio,
                        EBA_RADICADO_WORK_MANAGER = request?.Radicado,
                        Estado = 1,
                        UsrId = request.UserId
                    };
                    var respons = await _embargosRepository.ObtenerEmbargosRangoFiltros(peticionEmbargo);
                    embargos.AddRange(respons);

                    responseData.DataEmbargo = embargos;
                }
                if (request?.IdTipoSolicitudes == null &&
                   (request?.TipoSolicitudes == null ||
                    request.TipoSolicitudes == 1 ||
                    request.TipoSolicitudes == 5 ||
                    request.TipoSolicitudes == 6))
                {
                    var solicitudesFondo = await _postulacionRepository.Consultar_Tipo_Solicitud_Postulacion();
                    var typeSolicitudFs = solicitudesFondo.Where(x => x.TSP_ID.Equals(request.TipoSolicitudes)).FirstOrDefault();
                    var peticionFondoSolidaridad = new SolicitudPostulacionRequest
                    {
                        NoRadicado = request?.Radicado,
                        TipoSolicitudId = request.TipoSolicitudes,
                        UsrId = request.UserId
                    };
                    var fondoSolidaridad = await _postulacionRepository.ConsultarSolicitudPostulacion(peticionFondoSolidaridad);

                    responseData.DataFondoSolidaridad = fondoSolidaridad.ToList();
                }

               
            

           

            var reporteUnificado = new List<ReporteUnificado>();

            // Solicitudes  
            if (responseData.DataSolicitudes != null)
            {
                reporteUnificado.AddRange(responseData.DataSolicitudes
                    .Where(s => !string.IsNullOrEmpty(s.SOL_CEDULA))
                    .Select(s => new ReporteUnificado
                    {
                        Radicado = s.SOL_RADICADO_GD?.ToString(),
                        TipoSolicitud = s.PCS_DESCRIPCION,
                        FechaInicial = s.SOL_FECHA_SOLICITUD,
                        IdentificacionAfiliado = s.SOL_CEDULA,
                        Afiliado = s.CLI_PRIMER_NOMBRE,
                        Estado = s.CVL_DESCRIPCION,
                        Usuario = s.UsuarioAsignado,
                        Fuente = "Solicitud",
                        FechaFinal = DateTime.Now
                    }));
            }

            // Embargos
            if (responseData.DataEmbargo != null)
            {
                reporteUnificado.AddRange(responseData.DataEmbargo
                    .Where(e => !string.IsNullOrEmpty(e.CLI_IDENTIFICACION))
                    .Select(e => new ReporteUnificado
                    {
                        Radicado = e.EBA_RADICADO_WORK_MANAGER,
                        FechaInicial = e.EBA_FECHA_REGISTRO,
                        IdentificacionAfiliado = e.CLI_IDENTIFICACION,
                        Afiliado = e.EBA_NOMBRE_DEMANDADO,
                        Estado = e.EBA_ESTADO_EMBARGO,
                        Usuario = e.EBA_CREADOPOR_NOMBRE + " " + e.EBA_APELLIDO_CREADOPOR,
                        Fuente = "Embargo",
                        FechaFinal = e.EBA_FECHA_ACTUALIZACION
                    }));
            }

            // Fondo Solidaridad
            if (responseData.DataFondoSolidaridad != null)
            {
                reporteUnificado.AddRange(responseData.DataFondoSolidaridad
                    .Where(f => f.NumeroDocumento.ToString() != "")
                    .Select(f => new ReporteUnificado
                    {
                        Radicado = f.NoRadicado,
                        FechaInicial = f.FechaCreacion,
                        IdentificacionAfiliado = f.NumeroDocumento.ToString(),
                        Afiliado = f.NombreCompletoAfiliado,
                        Estado = f.EtapaProceso,
                        Usuario = f.UserName,
                        Fuente = "Fondo Solidaridad",
                        FechaFinal = DateTime.Now
                    }));
            }


            if (request.Estados != null && request.Estados.Any())
            {
                responseData.DataReporteUnificado = reporteUnificado
                    .Where(r => request.Estados.Any(e =>
                        string.Equals(e, r.Estado, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
            else
            {
                
                responseData.DataReporteUnificado = reporteUnificado;
            }

            Response<ClienteCedulaDto> clienteResponse;

            IEnumerable<Fuerzas> fuerzas = await _catalogosRepository.ObtenerFuerzas();
            IEnumerable<UnidadEjecutora> unidades = await _catalogosRepository.ObtenerUnidadesEjecutoras();
            var grados = await _catalogosRepository.ObtenerGrados();
            var categorias = await _catalogosRepository.ObtenerCategorias();

            foreach (var item in responseData.DataReporteUnificado)
            {
                if (item.IdentificacionAfiliado != null)
                {
                    clienteResponse = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(item.IdentificacionAfiliado);


                    if (clienteResponse?.Data != null)
                    {
                        var cliente = clienteResponse.Data;

                        // Nombre completo
                        item.Afiliado = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim();

                        // Fuerza
                        var fuerza = fuerzas.FirstOrDefault(s => s.FRZ_ID == cliente.IdFuerza);
                        if (fuerza != null)
                            item.Fuerza = fuerza.FRZ_DESCRIPCION;

                        // Unidad ejecutora
                        var unidad = unidades.FirstOrDefault(u => u.UEJ_ID == cliente.IdUnidadEjecutora);
                        if (unidad != null)
                            item.UnidadEjecutora = unidad.UEJ_NOMBRE;

                        // Grado
                        var grado = grados.FirstOrDefault(g => g.GRD_ID == cliente.IdGrado);
                        if (grado != null)
                            item.Grado = grado.GRD_DESCRIPCION;

                        // Categoría
                        var categoria = categorias.FirstOrDefault(c => c.CTG_ID == cliente.IdCategoria);
                        if (categoria != null)
                            item.Categoria = categoria.CTG_DESCRIPCION;
                    }
                }

            }

            return responseData;
        }

        public async Task<Response<DataReportSolicitud>> DataReporteSolictud(RequestReporteSolicitud request)
        {

            var data = await GetDataReport(request);
            if (data is not null)
            {
                if (request.Radicado != null)
                {
                    if (data.DataSolicitudes.Count > 0)
                    {
                        data.DataSolicitudes = data.DataSolicitudes.Where(s => s.SOL_RADICADO_GD == request.Radicado).ToList();
                    }
                    if (data.DataFondoSolidaridad.Count > 0)
                    {
                        data.DataFondoSolidaridad = data.DataFondoSolidaridad.Where(s => s.NoRadicado == request.Radicado).ToList();
                    }
                    if (data.DataEmbargo.Count > 0)
                    {
                        data.DataEmbargo = data.DataEmbargo.Where(s => s.EBA_RADICADO_WORK_MANAGER == request.Radicado).ToList();
                    }
                }

                if (request.FechaInicio != null && request.FechaFin != null)
                {
                    if (data.DataSolicitudes.Count > 0)
                    {
                        data.DataSolicitudes = data.DataSolicitudes.Where(s => s.SOL_FECHA_SOLICITUD >= request.FechaInicio && s.SOL_FECHA_SOLICITUD <= request.FechaFin).ToList();

                    }
                    if (data.DataFondoSolidaridad.Count > 0)
                    {
                        data.DataFondoSolidaridad = data.DataFondoSolidaridad.Where(s => s.FechaCreacion >= request.FechaInicio && s.FechaCreacion <= request.FechaFin).ToList();

                    }
                    if (data.DataEmbargo.Count > 0)
                    {
                        data.DataEmbargo = data.DataEmbargo.Where(s => s.EBA_FECHA_REGISTRO >= request.FechaInicio && s.EBA_FECHA_REGISTRO <= request.FechaFin).ToList();

                    }
                }
                else if (request.FechaInicio != null)
                {
                    if (data.DataSolicitudes.Count > 0)
                    {
                        data.DataSolicitudes = data.DataSolicitudes.Where(s => s.SOL_FECHA_SOLICITUD >= request.FechaInicio).ToList();

                    }
                    if (data.DataFondoSolidaridad.Count > 0)
                    {
                        data.DataFondoSolidaridad = data.DataFondoSolidaridad.Where(s => s.FechaCreacion >= request.FechaInicio).ToList();

                    }
                    if (data.DataEmbargo.Count > 0)
                    {
                        data.DataEmbargo = data.DataEmbargo.Where(s => s.EBA_FECHA_REGISTRO >= request.FechaInicio).ToList();

                    }
                }
                else if (request.FechaFin != null)
                {
                    if (data.DataSolicitudes.Count > 0)
                    {
                        data.DataSolicitudes = data.DataSolicitudes.Where(s => s.SOL_FECHA_SOLICITUD <= request.FechaFin).ToList();
                    }
                    if (data.DataFondoSolidaridad.Count > 0)
                    {
                        data.DataFondoSolidaridad = data.DataFondoSolidaridad.Where(s => s.FechaCreacion <= request.FechaFin).ToList();
                    }
                    if (data.DataEmbargo.Count > 0)
                    {
                        data.DataEmbargo = data.DataEmbargo.Where(s => s.EBA_FECHA_REGISTRO <= request.FechaFin).ToList();

                    }
                }

                //if (request.UserId != null)
                //{
                //    if (data.DataSolicitudes.Count > 0)
                //    {

                //    }
                //    if (data.DataFondoSolidaridad.Count > 0)
                //    {

                //    }
                //    if (data.DataEmbargo.Count > 0)
                //    {

                //    }
                //}
            }





            return new Response<DataReportSolicitud> { Data = data };
        }
    }
}
