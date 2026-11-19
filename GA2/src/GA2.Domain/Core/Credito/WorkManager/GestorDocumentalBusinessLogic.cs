using AutoMapper;
using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Credito;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AttachRequestDto = GA2.Domain.Core.ReferenciasSvc.WorkManager.AttachRequestDto;

namespace GA2.Domain.Core.Credito.WorkManager
{
    /// <summary>
    /// Work Manager Core Negocio
    /// <author>Nicolas FLorez Sarrazola</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class GestorDocumentalBusinessLogic : IGestorDocumentalBusinessLogic
    {

        private readonly IWorkManagerService _workmanagerService;
        private readonly ILogger<GestorDocumentalBusinessLogic> _logger;
        private readonly ICreditoRepository _creditoRepository;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly IMapper _mapper;
        private readonly CHKeysOptions _optionsCH;
        private readonly IIntegracionesGA2WorkManagerBusinessLogic _integracionWorkManager;
        private readonly IBlobStorageGenericRepository _blobStorage;
        private readonly ICuentasClientesRepository _cuentasClientesRepository;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly IPuntoAtencionRepository _puntoAtencionRepository;

        /// <summary>
        /// Constructor GestorDocumental
        /// </summary>
        /// <param name="integraciones">Logica integraciones externas</param>
        /// <param name="logger">Logger aplicacion</param>
        /// <param name="creditoRepository">Repositorio credito ga2</param>
        /// <param name="catalogosRepository">Repositorio catalogo ga2</param>
        /// <param name="mapper">Mapper dependencia</param>
        /// <param name="optionsCH">Opciones de configuration api</param>
        /// <exception cref="ArgumentNullException">Excepcion tipo null object</exception>
        public GestorDocumentalBusinessLogic(IWorkManagerService workmanagerService,
                                             ILogger<GestorDocumentalBusinessLogic> logger,
                                             ICreditoRepository creditoRepository,
                                             ICatalogosRepository catalogosRepository,
                                             IClientRequestProvider clientRequestProvider,
                                             IMapper mapper,
                                             IOptions<CHKeysOptions> optionsCH,
                                             IIntegracionesGA2WorkManagerBusinessLogic integracionWorkManager,
                                             IBlobStorageGenericRepository blobStorage,
                                             ICuentasClientesRepository cuentasClientesRepository,
                                             IEmbargosRepository JuzgadosRepository,
                                             IPuntoAtencionRepository PuntosAtencionRepository)
        {
            _workmanagerService = workmanagerService ?? throw new ArgumentNullException(nameof(workmanagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
            _catalogosRepository = catalogosRepository ?? throw new ArgumentNullException(nameof(catalogosRepository));
            _clientRequestProvider = clientRequestProvider ?? throw new ArgumentNullException(nameof(clientRequestProvider));            
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _optionsCH = optionsCH.Value ?? throw new ArgumentNullException(nameof(optionsCH));
            _integracionWorkManager = integracionWorkManager ?? throw new ArgumentNullException(nameof(integracionWorkManager));
            _blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
            _cuentasClientesRepository = cuentasClientesRepository;
            _juzgadosRepository = JuzgadosRepository;
            _puntoAtencionRepository = PuntosAtencionRepository;
        }


        #region Credito

        #region Metodos Publicos Credito

        public async Task<object> ActualizarRegistro(RequestUpdateDto request)
        {
            var updateRequest = GenUpdateRequest(request);
            dynamic update = default;
#if DEBUG
            _logger.LogInformation("Enviroment development");

            update = await this._integracionWorkManager.FormUpdateWorkManager(new Form_UpdateRequest
            {
                request= updateRequest
            });
#else
            _logger.LogInformation("Enviroment produccion");
            update = await this._workmanagerService.FormUpdateAsyncCH(updateRequest);
#endif
            var data = update.Data;
            return update;
        }

        public async Task<Response<Form_InsertResponse>> RadicarGDPreApronado(SolicitudSimulacionDto preAprobado)
        {           
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            Response<Form_GetDataResponse> ValidaExpPadre = new Response<Form_GetDataResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            Task.Run(async () => { ValidaExpPadre = await validarExpedientePadre(preAprobado); }).Wait();
            if (ValidaExpPadre.Data.Form_GetDataResult.Rows == 0)
            {
                Task.Run(async () => { await RadicarExpedientePadre(preAprobado); }).Wait(); 
            }

            Task.Run(async () => { insertRequest = await GetInsertRequestPreAprobado(preAprobado); }).Wait();

            Task.Run(async () => { insert = await _workmanagerService.FormInsertAsyncCH(insertRequest); }).Wait();

            if (insert.IsSuccess && insert.ReturnMessage == "El formulario fue guardado")
            {
                var update = new UpdateRequestDto
                {
                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    OperationUser = "Web",
                    FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = insert.Data.Form_InsertResult.BarCode },
                    FormCode = "361"                    
                };

                var queryRequest = CreateFormGetDataRequest(insertRequest.Data.First(x => x.Field == "C0089").Value);
                var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);
                foreach (var item in response.Data.Form_GetDataResult.Data)
                {
                    if (item.Key == "ID")
                    {
                        update.Data = new ReferenciasSvc.WorkManager.DictionaryDto[] { new() { Field = "ID_EXT", Value = item.Value } };
                    }
                }

                Task.Run(async () => { var updateRes = await _workmanagerService.FormUpdateAsyncCH(update); }).Wait();
            }

            

            return insert;
        }

        public async Task<Response<Form_InsertResponse>> RadicarExpedientePadre(SolicitudSimulacionDto preAprobado)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            insertRequest = await CreateInsertRequestForm02(preAprobado);
            insert = await this._workmanagerService.FormInsertAsyncCH(insertRequest);

            return insert;
        }
        public async Task<Response<Form_GetDataResponse>> validarExpedientePadre(SolicitudSimulacionDto preAprobado)
        {
            var solicitudSimulacion = _mapper.Map<SolicitudSimulacion>(preAprobado);
            var numeroDocumento = (await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(solicitudSimulacion)).SMC_NUMERO_DOCUMENTO;
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(numeroDocumento));
            var IdTipoDocIdentidad = (await _clientRequestProvider.ObtenerClientesByNumeroIdentificacion(InfoClienteFinaciera.numeroDocumento)).Data.IdTipoIdentificacion;
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(IdTipoDocIdentidad, InfoClienteFinaciera.numeroDocumento);
            var queryRequest = CreateFormGetDataRequest(cliente.Data.Identificacion);
            var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);

            return response;
        }
        private QueryRequestDto CreateFormGetDataRequest(string identificacionCliente)
        {
            return new QueryRequestDto()
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                FormCode = "02",
                FilterParameters = new() { Field = "C0002", Operator = "=", Value = identificacionCliente }
            };
        }



        public async Task<Response<Form_InsertResponse>> RadicadoFlujo83(DatosIdentificacionDto datosIdentificacion)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            Response<Form_GetDataResponse> ValidaExpPadre = new Response<Form_GetDataResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            Task.Run(async () => { ValidaExpPadre = await validarExpedientePadreFlujo83(datosIdentificacion); }).Wait();
            if (ValidaExpPadre.Data.Form_GetDataResult.Rows == 0)
            {
                Task.Run(async () => { await RadicarExpedientePadreFlujo83(datosIdentificacion); }).Wait();
            }

            Task.Run(async () => { insertRequest = await GetInsertRequestPreAprobadoFlujo83(datosIdentificacion); }).Wait();

            Task.Run(async () => { insert = await _workmanagerService.FormInsertAsyncCH(insertRequest); }).Wait();

            if (insert.IsSuccess && insert.ReturnMessage == "El formulario fue guardado")
            {
                var update = new UpdateRequestDto
                {
                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    OperationUser = "Web",
                    FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = insert.Data.Form_InsertResult.BarCode },
                    FormCode = "83"
                };

                var queryRequest = CreateFormGetDataRequestFlujo83(insertRequest.Data.First(x => x.Field == "C0005").Value);
                var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);
                foreach (var item in response.Data.Form_GetDataResult.Data)
                {
                    if (item.Key == "ID")
                    {
                        update.Data = new ReferenciasSvc.WorkManager.DictionaryDto[] { new() { Field = "ID_EXT", Value = item.Value } };
                    }
                }

                Task.Run(async () => { var updateRes = await _workmanagerService.FormUpdateAsyncCH(update); }).Wait();
            }



            return insert;
        }

        public async Task<Response<Form_InsertResponse>> RadicarExpedientePadreFlujo83(DatosIdentificacionDto datosIdentificacion)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            insertRequest = await GetInsertRequestPreAprobadoFlujo83(datosIdentificacion);
            insert = await this._workmanagerService.FormInsertAsyncCH(insertRequest);

            return insert;
        }
        public async Task<Response<Form_GetDataResponse>> validarExpedientePadreFlujo83(DatosIdentificacionDto datosIdentificacion)
        {
            //var solicitudSimulacion = _mapper.Map<SolicitudSimulacion>(preAprobado);
            //var numeroDocumento = (await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(solicitudSimulacion)).SMC_NUMERO_DOCUMENTO;
            //var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(numeroDocumento));
            //var IdTipoDocIdentidad = (await _clientRequestProvider.ObtenerClientesByNumeroIdentificacion(InfoClienteFinaciera.numeroDocumento)).Data.IdTipoIdentificacion;
            //var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(IdTipoDocIdentidad, InfoClienteFinaciera.numeroDocumento);
            var queryRequest = CreateFormGetDataRequestFlujo83(datosIdentificacion.Identificacion);
            var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);

            return response;
        }



        private QueryRequestDto CreateFormGetDataRequestFlujo83(string identificacionCliente)
        {
            return new QueryRequestDto()
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                FormCode = "83",
                FilterParameters = new() { Field = "C0005", Operator = "=", Value = identificacionCliente }
            };
        }

        //public async Task<Response<Form_InsertResponse>> RadicarGDSolicitudCredito(SolicitudCreditoDto solicitud)
        //{
        //    var insertRequest = await GetInsertRequestSolcitudCredito(solicitud);
        //    var insert = await _workmanagerService.FormInsertAsyncCH(insertRequest);

        //    return insert;
        //}

        public async Task<Response<Form_InsertResponse>> CrearRegistro(PeticionCreditoBasicaDto peticionCredito)
        {
            var peticion = this._mapper.Map<PeticionCreditoBasica>(peticionCredito);
            var credito = await this._creditoRepository.ObtenerSolicCreditoBasica(peticion);
            var insRequest = await GenInserRequest(credito);
            var insert = new Response<Form_InsertResponse>();
            if (string.IsNullOrEmpty(credito.NUMERO_RADICADO))
            {

#if DEBUG
                insert = await _integracionWorkManager.FormInsertWorkManager(new Form_InsertRequest {  request = insRequest });
#else
            insert = await this._workmanagerService.FormInsertAsyncCH(insRequest);
#endif
            }
            try
            {
                string radicado = string.Empty;
                if (string.IsNullOrEmpty(credito.NUMERO_RADICADO))
                {
                    radicado = (await this._creditoRepository.ActualizarRadicadoSolicitud(peticion.SOC_ID, string.IsNullOrEmpty(insert.Data.Form_InsertResult.BarCode)? "RADICADO NO DISPONIBLE":insert.Data.Form_InsertResult.BarCode)).SOC_NUMERO_RADICADO;

                }
                credito = await this._creditoRepository.ObtenerSolicCreditoBasica(peticion);
                var documentosSubidos = await this._creditoRepository.ObtenerDocumentosSolicCredito(peticion.SOC_ID);
                var documentosBase = new List<Application.Dto.FileDto>();
                foreach (var documento in documentosSubidos)
                {
                    if (!documento.DCS_RUTA_IMAGEN.Contains(@"\\"))
                    {
                        var blob = (await _blobStorage.DescargarArchivoStorage("credito", documento.DCS_RUTA_IMAGEN)).Content;
                        byte[] bytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            blob.CopyTo(memoryStream);
                            bytes = memoryStream.ToArray();
                        }

                        string base64 = Convert.ToBase64String(bytes);

                        documentosBase.Add(new Application.Dto.FileDto
                        {
                            Base64String = base64,
                            Description = documento.TDC_DESCRIPCION,
                            Ext = ".pdf",
                            DirectoryCode = "00",
                            DocumentTypeCode = "00",
                            GUID = documento.DCS_ID.ToString()
                        });
                    }
                }
                if (documentosBase.Count() > 0)
                {
                    await this.AttachFiles(new Application.Dto.AttachRequestDto { Files = documentosBase, Radicado = credito.NUMERO_RADICADO, UserOperation = "web" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Creacion registro workmanager fallida, {ex.Message}");
                return insert;
            }
            if (!string.IsNullOrEmpty(insert.Data.Form_InsertResult.BarCode))
            {
                insert.ReturnMessage = insert.Data.Form_InsertResult.BarCode;
            }
            else
            {
                insert.ReturnMessage = null;
            }
            return insert;
        }
        // Request AttachFiles
        public async Task<object> AttachFiles(Application.Dto.AttachRequestDto request)
        {

            var attachRequest = GenAttachRequest(request);

            var attach = await _integracionWorkManager.WorkflowAttachFilesWorkManager(new Workflow_AttachFilesRequest { request = attachRequest });

            var data = attach.Data;
            return attach;
        }

        public async Task<object> Form_AttachFilesAsync(Application.Dto.AttachRequestDto request)
        {

            var attachRequest = GenAttachRequestFiles(request);
            var attach  = await this._workmanagerService.FormAttachFilesAsyncCH(attachRequest);
            var data = attach.Data;
            return attach;
        }

        //** Crear dos metods del formInsert y WorkFlowAttachFiles

        public async Task<object> CrearFlujoFirmasComite(RequestFlujoFirmasDto request)
        {
            var datosComite = (await _creditoRepository.ObtenerComiteCredito(new DatosComiteCredito()
            {
                COC_ID = request.IdComite,
                COC_FECHA = request.FechaComite,
                ESTADO = request.EstadoComite,
                COC_NUMERO_COMITE = request.ComiteNumero
            })).ToArray();
            var datoComite = datosComite.FirstOrDefault();
            var temasComite = await _creditoRepository.ObtenerComiteCreditoTemas(new RequestTemaComiteCredito() { COC_ID = datoComite.COC_ID, IND = 0 });

            var insRequest = GenInserRequestFlujoFirmas(datoComite, temasComite, request.RequiereFirmaGerencia, request.AttachedFile);
            var insert = await this._workmanagerService.FormInsertAsyncCH(insRequest);
            try
            {
                var data = insert.Data;
                var radicado = await this._creditoRepository.ActualizarRadicadoComiteCredito(request.ComiteNumero, data.Form_InsertResult.BarCode);
            }
            catch (Exception ex)
            {
                insert.ReturnMessage = ex.Message;
                return insert;
            }

            return insert;
        }

        #endregion Metodos Publicos Credito

        #region Metodos Privados Credito

        private UpdateRequestDto GenUpdateRequest(RequestUpdateDto request)
        {
            return new UpdateRequestDto
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                OperationUser = request.OperationUser != string.Empty && request.OperationUser != null ? request.OperationUser : "web",
                FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = request.Radicado },
                FormCode = request.FormCode != string.Empty && request.FormCode != null ? request.FormCode : "219",
                Data = new ReferenciasSvc.WorkManager.DictionaryDto[] { new() { Field = "C0021", Key = "C0021", Value = request.Actividad } }
            };
        }

        private UpdateRequestDto GenUpdateRequestPreAprobado(RequestUpdateDto request)
        {
            return new UpdateRequestDto
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                OperationUser = request.OperationUser != string.Empty && request.OperationUser != null ? request.OperationUser : "Web",
                FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = request.Radicado },
                FormCode = request.FormCode != string.Empty && request.FormCode != null ? request.FormCode : "361",
                Data = new ReferenciasSvc.WorkManager.DictionaryDto[] { new() { Field = "ID_EXT", Value = request.Actividad } }
            };
        }

        private async Task<InsertRequestDto> GetInsertRequestPreAprobado(SolicitudSimulacionDto preAprobado)
        {
            var tipoCreditoDinamico = preAprobado.TcrId == 1 ? 2 : 1; 
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var solicitudSimulacion = _mapper.Map<SolicitudSimulacion>(preAprobado);
            var solicitudPreA = (await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(solicitudSimulacion));
            var numeroDocumento = solicitudPreA.SMC_NUMERO_DOCUMENTO;
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(numeroDocumento));
            var IdTipoDocIdentidad = (await _clientRequestProvider.ObtenerClientesByNumeroIdentificacion(InfoClienteFinaciera.numeroDocumento)).Data.IdTipoIdentificacion;
            var tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(IdTipoDocIdentidad);
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(IdTipoDocIdentidad, InfoClienteFinaciera.numeroDocumento)).Data;
            var UnidadEjecutora = _catalogosRepository.ObtenerUnidadesEjecutoras().Result.First(x => x.UEJ_ID == InfoClienteFinaciera.UEJ_Id).UEJ_NOMBRE;
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;
            var codigoCiudad = cliente.IdCiudadResidencia == 0 ? cliente.IdCiudadNacimiento : cliente.IdCiudadResidencia;
            var ciudad = (await _catalogosRepository.ObtenerCiudadPorId(codigoCiudad));
            var departamento = (await _catalogosRepository.ObtenerDepartamentoPorId(ciudad.DPD_ID));
            string tipoCredito = (await _catalogosRepository.ObtenerTipoCredito()).Where(tc => tc.TCR_ID == tipoCreditoDinamico).Select(tp => tp.TCR_DESCRIPCION).First();
            var direccion = cliente.Direccion != "" ? cliente.Direccion : "No Registra";
            var puntoAtencion = (await this._puntoAtencionRepository.ObtenerPuntosAtencionPorId(solicitudSimulacion.PA_ID));
            var medioRecepcion = (await this._creditoRepository.ObtenerMediosRecepcionById(solicitudPreA.MEDIO_RECEPCION));
            var medioCreditoVivienda = (await this._creditoRepository.ObtenerMediosCreditosViviendaById(solicitudPreA.MEDIOS_CREDITO));

            //var queryRequest = CreateFormGetDataRequest(numeroDocumento);
            //var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);
            //foreach (var item in response.Data.Form_GetDataResult.Data)
            //{
            //    if (item.Key == "ID")
            //    {
            //        dictionary.Add(new() { Field = "ID_EXT", Value = $"{item.Value}" });
            //    }
            //}
            
            dictionary.Add(new() { Field = "C0088", Value = $"{puntoAtencion.PA_PUNTO}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{medioRecepcion.MR_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0008", Value = $"SOLICITUD {tipoCredito}" });
            dictionary.Add(new() { Field = "C0009", Value = $"PROSPECCIÓN {tipoCredito}" });

            dictionary.Add(new() { Field = "C0014", Value = $"{medioCreditoVivienda.MCV_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{solicitudPreA.NUM_FOLIOS}" });
            dictionary.Add(new() { Field = "C0013", Value = $"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0089", Value = $"{InfoClienteFinaciera.numeroDocumento}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{InfoClienteFinaciera.nombreCliente}" });
            dictionary.Add(new() { Field = "C0003", Value = $"{UnidadEjecutora}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{categoria}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{InfoClienteFinaciera.numeroDocumento} | {InfoClienteFinaciera.nombreCliente}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{departamento.DPD_NOMBRE}" });
            dictionary.Add(new() { Field = "C0016", Value = $"{ciudad.DPC_NOMBRE}" });
            dictionary.Add(new() { Field = "C0017", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0018", Value = $"{cliente.Telefono}" });
            dictionary.Add(new() { Field = "C0019", Value = $"{cliente.Correo2}" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "361",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        private async Task<InsertRequestDto> CreateInsertRequestForm02(SolicitudSimulacionDto preAprobado)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var solicitudSimulacion = _mapper.Map<SolicitudSimulacion>(preAprobado);
            var numeroDocumento = (await _creditoRepository.ObtenerDatosSolicitudClienteByPreAprobado(solicitudSimulacion)).SMC_NUMERO_DOCUMENTO;
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(numeroDocumento));
            var IdTipoDocIdentidad = (await _clientRequestProvider.ObtenerClientesByNumeroIdentificacion(InfoClienteFinaciera.numeroDocumento)).Data.IdTipoIdentificacion;
            var tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(IdTipoDocIdentidad);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(IdTipoDocIdentidad, InfoClienteFinaciera.numeroDocumento);
            Response<InfoClienteDto> clienteDto = await _clientRequestProvider.ObtenerInfoCliente(IdTipoDocIdentidad, InfoClienteFinaciera.numeroDocumento);
            InfoDetalleCliente detalleCliente = await _cuentasClientesRepository.ObtenerDetalleInfoCliente(_mapper.Map<InfoCliente>(clienteDto.Data));
            var departament = await _catalogosRepository.ObtenerDepartamentoPorId(cliente.Data.IdDepartamentoResidencia);
            var ciudad = await _catalogosRepository.ObtenerCiudadPorId(cliente.Data.IdCiudadResidencia);
            var direccion = string.IsNullOrEmpty(cliente.Data.Direccion) ? "NO REGISTRA" : cliente.Data.Direccion;
            var correo = string.IsNullOrEmpty(cliente.Data.Correo) ? "NO REGISTRA" : cliente.Data.Correo;

            dictionary.Add(new()
            {
                Field = "C0001",
                Value = $"{"Expediente Padre: "} {cliente.Data.Identificacion} " +
                $"{InfoClienteFinaciera.nombreCliente}"
            });
            dictionary.Add(new() { Field = "C0002", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0003", Value = $"{InfoClienteFinaciera.nombreCliente}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{InfoClienteFinaciera.telefonoFijo}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{InfoClienteFinaciera.telefonoCelular}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{correo}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{detalleCliente.UNIDADE_EJECUTORA}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{detalleCliente.GRADO} - {detalleCliente.CATEGORIA}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{cliente.Data.Barrio}" });
            dictionary.Add(new() { Field = "C0013", Value = $"{"Colombia"}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{departament.DPD_NOMBRE}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{ciudad.DPC_NOMBRE}" });
            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "02",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        //private async Task<InsertRequestDto> GetInsertRequestSolcitudCredito(SolicitudCreditoDto solicitudCredito)
        //{
        //    List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
        //    var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitudCredito.TID_ID, solicitudCredito.CLI_IDENTIFICACION)).Data;
        //    var UnidadEjecutora = _catalogosRepository.ObtenerUnidadesEjecutoras().Result.First(x => x.UEJ_ID == cliente.IdUnidadEjecutora).UEJ_NOMBRE;
        //    var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;
        //    var ciudad = (await _catalogosRepository.ObtenerCiudadPorId(cliente.IdCiudadResidencia)).DPC_NOMBRE;

        //    dictionary.Add(new() { Field = "C0001", Value = $"{ solicitudCredito.CLI_IDENTIFICACION }" });
        //    dictionary.Add(new() { Field = "C0002", Value = $"{ string.Concat( cliente.PrimerNombre, cliente.SegundoNombre, cliente.PrimerApellido, cliente.SegundoApellido) }" });
        //    dictionary.Add(new() { Field = "C0003", Value = $"{ UnidadEjecutora }" });
        //    dictionary.Add(new() { Field = "C0004", Value = $"{ cliente.FechaInscripcion.ToString("yyyy-MM-dd")}" });
        //    dictionary.Add(new() { Field = "C0005", Value = $"{ categoria }" });
        //    dictionary.Add(new() { Field = "C0007", Value = $"{ ciudad }" });
        //    dictionary.Add(new() { Field = "C0008", Value = $"{ cliente.Direccion }" });
        //    dictionary.Add(new() { Field = "C0009", Value = $"{ cliente.Correo }" });
        //    dictionary.Add(new() { Field = "C0010", Value = $"{ cliente.Telefono }" });

        //    return new InsertRequestDto()
        //    {
        //        CheckWorkflow = true,
        //        Data = dictionary.ToArray(),
        //        FormCode = "219",
        //        OfficeCode = "01",
        //        OperationUser = "Web",
        //        Header = new()
        //        {
        //            Token = _optionsCH.Token,
        //            User = _optionsCH.User
        //        }
        //    };
        //}

        private async Task<InsertRequestDto> GenInserRequest(RespuestaCreditoBasica credito)
        {
            var listaCodigos = new List<string>
            {
                "C0001",
                "C0002",
                "C0003",
                "C0004",
                "C0005",
                "C0006",
                "C0007",
                "C0008",
                "C0009",
                "C0010",
                "C0011",
                "C0012",
                "C0013",
                "C0014"
            };
            var diccionarios = new List<ReferenciasSvc.WorkManager.DictionaryDto>();
            var departamento = await this._catalogosRepository.ObtenerDepartamentoPorId(credito.DPD_ID_NACIMIENTO);
            //var ciudad = await this._catalogosRepository.ObtenerCiudadPorId(credito.DPC_ID_RESIDENCIA);
            var ciudad = new Ciudad { DPC_NOMBRE = "BOGOTA" };

            foreach (var codigo in listaCodigos)
            {
                var dictionary = new ReferenciasSvc.WorkManager.DictionaryDto();
                dictionary.Field = codigo;
                switch (codigo)
                {
                    case "C0001":
                        dictionary.Value = credito.SOB_NUMERO_DOCUMENTO;
                        break;
                    case "C0002":
                        dictionary.Value = $"{credito.SOB_PRIMER_NOMBRE} {credito.SOB_SEGUNDO_NOMBRE} {credito.SOB_PRIMER_APELLIDO} {credito.SOB_SEGUNDO_APELLIDO}";
                        break;
                    case "C0003":
                        dictionary.Value = credito.FUERZA;
                        break;
                    case "C0004":
                        dictionary.Value = credito.FECHA_AFILIACION.ToString("yyyy-MM-dd");
                        break;
                    case "C0005":
                        dictionary.Value = credito.CATEGORIA;
                        break;
                    case "C0006":
                        dictionary.Value = departamento.DPD_NOMBRE;
                        break;
                    case "C0007":
                        dictionary.Value = ciudad.ToString();
                        break;
                    case "C0008":
                        dictionary.Value = credito.SOB_DIRECCION_RESIDENCIA;
                        break;
                    case "C0009":
                        dictionary.Value = !string.IsNullOrEmpty(credito.SOB_CORREO_INSTITUCIONAL) ? credito.SOB_CORREO_INSTITUCIONAL != null ? credito.SOB_CORREO_INSTITUCIONAL : "Email": credito.SOB_CORREO_PERSONAL;
                        break;
                    case "C0010":
                        dictionary.Value = credito.SOB_TELEFONO_RESIDENCIA;
                        break;
                    case "C0011":
                        dictionary.Value = credito.TIPO_DOCUMENTO_CLIENTE;
                        break;
                    case "C0012":
                        dictionary.Value = credito.SOB_FECHA_EXPEDICION.ToString("yyyy-MM-dd");
                        break;
                    case "C0013":
                        dictionary.Value = credito.CIUDAD_EXPEDICION;
                        break;
                    case "C0014":
                        dictionary.Value = credito.SOB_FECHA_NACIMIENTO.ToString("yyyy-MM-dd");
                        break;
                    default:
                        break;
                }

                diccionarios.Add(dictionary);
            }

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = diccionarios.ToArray(),
                FormCode = "219",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
        private WorkflowAttachFilesRequestDto GenAttachRequest(Application.Dto.AttachRequestDto request)
        {
            var lista = new List<ReferenciasSvc.WorkManager.FileDto>();

            request.Files.ForEach(x => lista.Add(new ReferenciasSvc.WorkManager.FileDto
            {
                Base64String = x.Base64String,
                Description = x.Description,
                DirectoryCode = x.DirectoryCode,
                DocumentTypeCode = x.DocumentTypeCode,
                Ext = x.Ext,
                GUID = x.GUID,
            }));

            return new WorkflowAttachFilesRequestDto
            {
                WorkflowId = 0,
                Files = lista.ToArray(),
                OperationUser = request.UserOperation,
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
        //AttachRequestDto
        private AttachRequestDto GenAttachRequestFiles(Application.Dto.AttachRequestDto request)
        {
            var lista = new List<ReferenciasSvc.WorkManager.FileDto>();

            request.Files.ForEach(x => lista.Add(new ReferenciasSvc.WorkManager.FileDto
            {
                Base64String = x.Base64String,
                Description = x.Description,
                DirectoryCode = x.DirectoryCode,
                DocumentTypeCode = x.DocumentTypeCode,
                Ext = x.Ext,
                GUID = x.GUID,
            }));

            return new AttachRequestDto
            {
                Files = lista.ToArray(),
                OperationUser = request.UserOperation,
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                BarCode= request.Radicado
            };
        }
        private InsertRequestDto GenInserRequestFlujoFirmas(DatosComiteCredito datosComite, IEnumerable<TemaComiteCredito> temasComite, bool requiereFirmaGerencia, string attachedFile)
        {
            var listaCodigos = new List<string>
            {
                "C0001",
                "C0002",
                "C0003",
                "C0004",
                "C0005",
                "C0006",
                "C0007",
                "C0008",
                "C0009",
                "C0010",
                "C0011"
            };

            var diccionarios = new List<ReferenciasSvc.WorkManager.DictionaryDto>();

            foreach (var codigo in listaCodigos)
            {
                var dictionary = new ReferenciasSvc.WorkManager.DictionaryDto();
                dictionary.Field = codigo;
                switch (codigo)
                {
                    case "C0001":
                        dictionary.Value = $"{datosComite.COC_NUMERO_COMITE}";
                        break;
                    case "C0002":
                        dictionary.Value = $"{datosComite.COC_FECHA}";
                        break;
                    case "C0003":
                        dictionary.Value = "TEMAS";
                        break;
                    case "C0004":
                        dictionary.Value = $"{temasComite.ToArray()[0].DESCRIPCION_TEMA}";
                        break;
                    case "C0005":
                        dictionary.Value = $"{temasComite.ToArray()[0].COT_OBSERVACION}";
                        break;
                    case "C0006":
                        dictionary.Value = $"{temasComite.ToArray()[1].DESCRIPCION_TEMA}";
                        break;
                    case "C0007":
                        dictionary.Value = $"{temasComite.ToArray()[1].COT_OBSERVACION}";
                        break;
                    case "C0008":
                        dictionary.Value = $"{temasComite.ToArray()[2].DESCRIPCION_TEMA}";
                        break;
                    case "C0009":
                        dictionary.Value = $"{temasComite.ToArray()[2].COT_OBSERVACION}";
                        break;
                    case "C0010":
                        dictionary.Value = $"{requiereFirmaGerencia}";
                        break;
                    case "C0011":
                        dictionary.Value = attachedFile;
                        break;
                    default:
                        break;
                }
                diccionarios.Add(dictionary);
            }

            var lista = new List<ReferenciasSvc.WorkManager.FileDto>() { new() { Base64String = attachedFile, Description = "133-FIRMA ACTAS COMITÉ DE CRÉDITO LEASING" } };
            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = diccionarios.ToArray(),
                FormCode = "133",
                OfficeCode = "01",
                OperationUser = "Web",
                Files = lista.ToArray(),
                ProcessId = 289,
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        #endregion Metodos Privados Credito

        #endregion Credito


        private async Task<InsertRequestDto> GetInsertRequestPreAprobadoFlujo83(DatosIdentificacionDto identificacionDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();

            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(identificacionDto.TipoIdentificacion, identificacionDto.Identificacion)).Data;
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(identificacionDto.Identificacion));


            string prestamo = "0";
            string plazo = "0"; 
            string valorCuota = "0";
            string tipoCredito = "Crédito Hipotecario";

            if (InfoClienteFinaciera != null)
            {
                prestamo= InfoClienteFinaciera.valorPrestamo.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
                plazo = InfoClienteFinaciera.plazo.ToString();
                valorCuota= InfoClienteFinaciera.valorCuota.ToString("C0", new System.Globalization.CultureInfo("es-CO"));
                tipoCredito = InfoClienteFinaciera.tipoCredito == 1 ? "Crédito Hipotecario" : "Leasing Habitacional"; 
            }

            dictionary.Add(new() { Field = "C0001", Value = $"{identificacionDto.Identificacion} | {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" });
            dictionary.Add(new() { Field = "C0002", Value = "Grupo Originación de Crédito" });
            dictionary.Add(new() { Field = "C0003", Value = "Comunicación oficial" });
            dictionary.Add(new() { Field = "C0004", Value = $"{identificacionDto.TipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{identificacionDto.Identificacion}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{cliente.Correo2}" });
            dictionary.Add(new() { Field = "C0008", Value = $"AFILIADO" });
            dictionary.Add(new() { Field = "C0009", Value = $"Analista" });
            dictionary.Add(new() { Field = "C0010", Value = prestamo });
            dictionary.Add(new() { Field = "C0011", Value = plazo});
            dictionary.Add(new() { Field = "C0012", Value = valorCuota});
            dictionary.Add(new() { Field = "C0013", Value = "Grupo Originación de Crédito" });
            dictionary.Add(new() { Field = "C0014", Value =  tipoCredito});
            dictionary.Add(new() { Field = "C0015", Value = "NO" });
            dictionary.Add(new() { Field = "C0016", Value = 0.ToString() });


            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "83",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
    }
}
