using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Solicitud.WorkManager
{
    public class GestorDocumentSolicitudes : IGestorDocumentSolicitudes
    {
        private readonly ConfiguracionesSvc _configuration;
        private readonly IWorkManagerService _integraciones;
        private readonly ILogger<GestorDocumentSolicitudes> _logger;
        private readonly CHKeysOptions _optionsCH;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClientRequestProvider _clientRequestProvider;        
        private readonly IUsersRepository _usersRepository;
        private const string noRegistra = "NO REGISTRA";

        private readonly ICreditoRepository _creditoRepository; 
        public GestorDocumentSolicitudes(IOptions<ConfiguracionesSvc> configurationes, 
                                         IWorkManagerService integraciones,
                                         ILogger<GestorDocumentSolicitudes> logger,
                                         IOptions<CHKeysOptions> optionsCH,
                                         ICatalogosRepository catalogosRepository,
                                         IClientRequestProvider clientRequestProvider,
                                         IUsersRepository usersRepository,
                                         ICreditoRepository creditoRepository)
        {
            _configuration = configurationes.Value;
            _integraciones = integraciones ?? throw new ArgumentNullException(nameof(integraciones));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _optionsCH = optionsCH.Value ?? throw new ArgumentNullException(nameof(optionsCH));
            _catalogosRepository = catalogosRepository ?? throw new ArgumentNullException(nameof(catalogosRepository));
            _clientRequestProvider = clientRequestProvider ?? throw new ArgumentNullException(nameof(clientRequestProvider));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));

            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
        }

        public async Task<Response<Form_InsertResponse>> RadicarSolicitud(ObtenerTramiteSolicitudDto obtenerTramiteSolicitudDto)
        {

            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var  status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();  
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                InsertRequestDto insertRequest = new InsertRequestDto();

                switch (obtenerTramiteSolicitudDto.TimCodigo)
                {
                    case "PCUOTAINI":
                        insertRequest = await CreateInsertRequestForm155(obtenerTramiteSolicitudDto);
                        break;
                    case "ACREDV8":
                        insertRequest = await CreateInsertRequestAcreditacion(obtenerTramiteSolicitudDto);
                        break;
                    case "REINTG":
                        insertRequest = await CreateInsertRequestAcreditacion(obtenerTramiteSolicitudDto);
                        break;
                    case "RESAJ":
                        insertRequest = await CreateInsertRequestAcreditacion(obtenerTramiteSolicitudDto);
                        break;
                    case "NOACREDV8":
                        break;
                    default:
                        insertRequest = await CreateInsertRequest(obtenerTramiteSolicitudDto);
                        break;
                }

                var insert = await this._integraciones.FormInsertAsyncCH(insertRequest);

                try
                {
                    var data = insert.Data;
                    //var radicado = await this._creditoRepository.ActualizarRadicadoSolicitud(peticion.SOC_ID, data.BarCode);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Creacion registro workmanager fallida, {ex.Message}");
                    return null;
                }

                return insert;
            }
            else
            {
                return new Response<Form_InsertResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = null};
            }
        }

        public async Task<Response<Form_InsertResponse>> RadicarSolicitud378or113(CrearRadicadoCartaIncosistencias request )
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                InsertRequestDto insertRequest = new InsertRequestDto();

                switch (request.Codigo)
                {
                    case "378":
                        insertRequest = await CreateInsertRequest378(request);
                        break;
                    case "114":
                        insertRequest = await CreateInsertRequest114(request);
                        break;
                }
                var insert = await this._integraciones.FormInsertAsyncCH(insertRequest);

                try
                {
                    var data = insert.Data;
                    //var radicado = await this._creditoRepository.ActualizarRadicadoSolicitud(peticion.SOC_ID, data.BarCode);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Creacion registro workmanager fallida, {ex.Message}");
                    return null;
                }

                return insert;
            }else
            {
                return new Response<Form_InsertResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = null };

            }
        }
        public async Task<Response<Form_UpdateResponse>> ActualizarRadicadoSolicitud(ActualizarRadicadoSolicitudTareaDto obtenerTramiteSolicitudDto)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                UpdateRequestDto updateRequest = new UpdateRequestDto();

                updateRequest = await CreateUpdateRequest(obtenerTramiteSolicitudDto);

                var update = await this._integraciones.FormUpdateAsyncCH(updateRequest);

                try
                {
                    var data = update.Data;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Creacion registro workmanager fallida, {ex.Message}");
                    return null;
                }

                return update;
            }
            else
            {
                return new Response<Form_UpdateResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = null };
            }
        }
        public async Task<Response<Form_GetDataResponse>> ObtenerFormulariosDodo(string IdentificacionCliente, string FormCode)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                var queryRequest = CreateFormGetDataRequestForm(IdentificacionCliente, FormCode);
                var response = await this._integraciones.FormGetDataAsyncCH(queryRequest);

                return response;
            }
            else
            {
                return new Response<Form_GetDataResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = null };
            }
        }

        private async Task<InsertRequestDto> CreateInsertRequest378(CrearRadicadoCartaIncosistencias obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
          var   clienteDtoTipo = await _clientRequestProvider.ObtenerClienteInformacion(obtenerTramiteSolicitudDto.clienteId);
           var  clienteDto = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDtoTipo.Data.IdTipoIdentificacion, clienteDtoTipo.Data.Identificacion);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion);
            var direccion = string.IsNullOrEmpty(cliente.Data.Direccion) ? noRegistra : cliente.Data.Direccion;
            dictionary.Add(new() { Field = "C0001", Value = $"02-HISTORIALES DE AFILIADOS" });
            dictionary.Add(new() { Field = "C0003", Value = $"{obtenerTramiteSolicitudDto.Asunto}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{obtenerTramiteSolicitudDto.NoFolios}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{cliente.Data.NombreTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{cliente.Data.PrimerNombre} {cliente.Data.PrimerApellido}" });
            dictionary.Add(new() { Field = "C0023", Value = $"{direccion}" });
             
            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "378",
                OfficeCode = "01",
                OperationUser = $"{cliente.Data.PrimerNombre} {cliente.Data.PrimerApellido}",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
        private async Task<InsertRequestDto> CreateInsertRequest114(CrearRadicadoCartaIncosistencias obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();

            var clienteDtoTipo = await _clientRequestProvider.ObtenerClienteInformacion(obtenerTramiteSolicitudDto.clienteId);
            var clienteDto = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDtoTipo.Data.IdTipoIdentificacion, clienteDtoTipo.Data.Identificacion);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(clienteDto.Data.IdTipoIdentificacion, clienteDto.Data.Identificacion); ;
            dictionary.Add(new() { Field = "C0010", Value = $"{cliente.Data.NombreTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{obtenerTramiteSolicitudDto.Radicado}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{cliente.Data.PrimerNombre} {cliente.Data.PrimerApellido}" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "114",
                OfficeCode = "01",
                OperationUser = $"{cliente.Data.PrimerNombre} {cliente.Data.PrimerApellido}",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
        private async Task<InsertRequestDto> CreateInsertRequest(ObtenerTramiteSolicitudDto obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var user = await _usersRepository.ObtenerUsuarioPorId(obtenerTramiteSolicitudDto.CreadoPor);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(obtenerTramiteSolicitudDto.tipoIdentificacion, obtenerTramiteSolicitudDto.identificacionCliente);
            var departament = await _catalogosRepository.ObtenerDepartamentoPorId(cliente.Data.IdDepartamentoResidencia);
            var ciudad = await _catalogosRepository.ObtenerCiudadPorIdDPCyIdDPD(cliente.Data.IdCiudadResidencia, cliente.Data.IdDepartamentoResidencia);          
            var direccion = string.IsNullOrEmpty(cliente.Data.Direccion) ? noRegistra : cliente.Data.Direccion;
            var telefono = string.IsNullOrEmpty(cliente.Data.Telefono) ? noRegistra : cliente.Data.Telefono;
            var correo = string.IsNullOrEmpty(cliente.Data.Correo) ? noRegistra : cliente.Data.Correo;
            var nombre = user.USR_NOMBRE.ToLower();
            var apellido = user.USR_APELLIDO.ToLower();
            dictionary.Add(new() { Field = "C0002", Value = $"{cliente.Data.Identificacion} | {obtenerTramiteSolicitudDto.nombresCliente} {obtenerTramiteSolicitudDto.apellidosCliente}" });
            dictionary.Add(new() { Field = "C0004", Value = "14" });
            dictionary.Add(new() { Field = "C0006", Value = $"{obtenerTramiteSolicitudDto.CodProcesoGD}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{obtenerTramiteSolicitudDto.CodSubModGD}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{cliente.Data.IdTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{obtenerTramiteSolicitudDto.nombresCliente} {obtenerTramiteSolicitudDto.apellidosCliente}" });
            dictionary.Add(new() { Field = "C0013", Value = $"{departament.DPD_CODIGO_DODO}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{ciudad.DPC_CODIGO_DODO}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0018", Value = $"{telefono}" });
            dictionary.Add(new() { Field = "C0019", Value = $"{correo}" });
            dictionary.Add(new() { Field = "C0097", Value = $"50" });
            dictionary.Add(new() { Field = "C0040", Value = $"0" });
            dictionary.Add(new() { Field = "C0098", Value = $"{obtenerTramiteSolicitudDto.NumeroSolicitud}" });

            dictionary.Add(new() { Field = "C0011", Value = $"{cliente.Data.EstadoCliente}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{cliente.Data.IdFuerza}" });
            dictionary.Add(new() { Field = "C0016", Value = $"{cliente.Data.Barrio}" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "21",
                OfficeCode = "01",
                OperationUser = $"{nombre}.{apellido}",
                Header = new ()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
        private async Task<InsertRequestDto> CreateInsertRequestAcreditacion(ObtenerTramiteSolicitudDto obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var user = await _usersRepository.ObtenerUsuarioPorId(obtenerTramiteSolicitudDto.CreadoPor);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(obtenerTramiteSolicitudDto.tipoIdentificacion, obtenerTramiteSolicitudDto.identificacionCliente);
            var departament = await _catalogosRepository.ObtenerDepartamentoPorId(cliente.Data.IdDepartamentoResidencia);
            var ciudad = await _catalogosRepository.ObtenerCiudadPorIdDPCyIdDPD(cliente.Data.IdCiudadResidencia, cliente.Data.IdDepartamentoResidencia);
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.Data.IdCategoria).CTG_DESCRIPCION;
            var direccion = string.IsNullOrEmpty(cliente.Data.Direccion) ? noRegistra : cliente.Data.Direccion;
            var telefono = string.IsNullOrEmpty(cliente.Data.Telefono) ? noRegistra : cliente.Data.Telefono;
            var correo = string.IsNullOrEmpty(cliente.Data.Correo) ? noRegistra : cliente.Data.Correo;
            var nombre = user.USR_NOMBRE.ToLower();
            var apellido = user.USR_APELLIDO.ToLower();
            dictionary.Add(new() { Field = "C0002", Value = $"{cliente.Data.Identificacion} | {obtenerTramiteSolicitudDto.nombresCliente} {obtenerTramiteSolicitudDto.apellidosCliente}" });
            dictionary.Add(new() { Field = "C0006", Value = "Acreditacion" });
            dictionary.Add(new() { Field = "C0005", Value = "Acreditacion" });
            dictionary.Add(new() { Field = "C0007", Value = $"{1}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{cliente.Data.NombreTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{obtenerTramiteSolicitudDto.nombresCliente} {obtenerTramiteSolicitudDto.apellidosCliente}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{cliente.Data.Unidad}" });
            dictionary.Add(new() { Field = "C0013", Value = $"{categoria}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{departament.DPD_NOMBRE}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{ciudad.DPC_NOMBRE}" });
            dictionary.Add(new() { Field = "C0016", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0018", Value = $"{telefono}" });
            dictionary.Add(new() { Field = "C0020", Value = $"{correo}" });
            dictionary.Add(new() { Field = "C0029", Value = $"{obtenerTramiteSolicitudDto.NumeroRadicadoSG}"});
            dictionary.Add(new() { Field = "C0033", Value = $"{1}" });
            dictionary.Add(new() { Field = "C0038", Value = $"SI" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "37",
                OfficeCode = "01",
                OperationUser = $"{nombre}.{apellido}",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        private async Task<InsertRequestDto> CreateInsertRequestForm155(ObtenerTramiteSolicitudDto obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();

            //var cliente = await _clientRequestProvider.ObtenerClienteInformacion(obtenerTramiteSolicitudDto.clienteId);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(obtenerTramiteSolicitudDto.tipoIdentificacion, obtenerTramiteSolicitudDto.identificacionCliente);
            
            dictionary.Add(new() { Field = "C0001", Value = $"{cliente.Data.Identificacion}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{obtenerTramiteSolicitudDto.nombresCliente} {obtenerTramiteSolicitudDto.apellidosCliente}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{(obtenerTramiteSolicitudDto.TipoCreditoId == 2? 3: obtenerTramiteSolicitudDto.TipoCreditoId)}" });
            dictionary.Add(new() { Field = "C0003", Value = $"{cliente.Data.IdFuerza}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{obtenerTramiteSolicitudDto.valorRetirar}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{obtenerTramiteSolicitudDto.NumeroSolicitud}" });
            

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "155",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        public async Task<Response<FileResponseDto>> ObtenerDocumentosPorRadicado(string numeroRadicado)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                var request = new FileRequestDto
                {
                    BarCode = numeroRadicado,
                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    }
                };

                var response = await _integraciones.FormGetFileCodesAsyncCH(request);
                return new Response<FileResponseDto>
                {
                    Data = response.Data.Form_GetFileCodesResult
                };
            }
            else
            {
                return new Response<FileResponseDto>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = null };
            }
        }

        public async Task<Response<string>> ObtenerEnlaceDodo(string numeroRadicado)
        {
                var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
                var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                try
                {
                    var code = numeroRadicado.Split("-")[0];
                    var request = new Form_GetParametersRequest
                    {
                        request = new FormParametersRequestDto
                        {
                            FormCode = code,
                            Header = new()
                            {
                                Token = _optionsCH.Token,
                                User = _optionsCH.User
                            }
                        }

                    };

                    var response = await _integraciones.Form_GetParameters(request);
                    var form = response.Data.Form_GetParametersResult.Form;

                    //NombreCampoSticker
                    //IdFormulario
                    //http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?columna=Cod_Barras&valorColumna=361-01-2023082201700&idFormulario=20474
                    var enlaceDodo = $"{_configuration.DodoUrlSvc}?columna={form.NombreCampoSticker}&valorColumna={numeroRadicado}&idFormulario={form.IdFormulario}";

                    return new Response<string>
                    {
                        Data = enlaceDodo
                    };
                }
                catch (Exception e)
                {

                    return new Response<string>
                    {
                        Data = ""
                    };
                }
            }
            else
            {
                return new Response<string>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo",Data = "" };
            }
        }

        private async Task<UpdateRequestDto> CreateUpdateRequest(ActualizarRadicadoSolicitudTareaDto obtenerTramiteSolicitudDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var user = await _usersRepository.ObtenerUsuarioPorId(obtenerTramiteSolicitudDto.CreadoPor);
            var numeroFolio = obtenerTramiteSolicitudDto.Numero_Folio != null ? obtenerTramiteSolicitudDto.Numero_Folio : 0;
            var nombre = user.USR_NOMBRE.ToLower();
            var apellido = user.USR_APELLIDO.ToLower();

            dictionary.Add(new() { Field = "C0004", Value = $"{obtenerTramiteSolicitudDto.Sucursal}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{obtenerTramiteSolicitudDto.CodProcesoGD}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{obtenerTramiteSolicitudDto.CodSubModGD}" });
            dictionary.Add(new() { Field = "C0040", Value = $"{numeroFolio}" });
            dictionary.Add(new() { Field = "C0097", Value = $"{obtenerTramiteSolicitudDto.TipoTurno}" });
            dictionary.Add(new() { Field = "C0098", Value = $"{obtenerTramiteSolicitudDto.NumeroTurno}" });
            dictionary.Add(new() { Field = "C0110", Value = $"{obtenerTramiteSolicitudDto.TipoVivienda}" });

            return new UpdateRequestDto()
            {
                Data = dictionary.ToArray(),
                FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = obtenerTramiteSolicitudDto.Radicado },
                FormCode = "21",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                OperationUser = $"{nombre}.{apellido}"
            };
        }

        private QueryRequestDto CreateFormGetDataRequestForm(string identificacionCliente, string FormCode)
        {
            if (FormCode=="37")
            {
                return new QueryRequestDto()
                {

                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    FormCode = FormCode,
                    FilterParameters = new() { Field = "C0010", Operator = "=", Value = identificacionCliente }
                };

            }
            else if (FormCode == "102")
            {
                return new QueryRequestDto()
                {

                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    FormCode = FormCode,
                    FilterParameters = new() { Field = "C0006", Operator = "=", Value = identificacionCliente }
                };
            }else if (FormCode == "109")
            {
                return new QueryRequestDto()
                {

                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    FormCode = FormCode,
                    FilterParameters = new() { Field = "C0002", Operator = "=", Value = identificacionCliente }
                };

            }
            else
            {
                return new QueryRequestDto()
                {

                    Header = new()
                    {
                        Token = _optionsCH.Token,
                        User = _optionsCH.User
                    },
                    FormCode = FormCode,
                    FilterParameters = new() { Field = "C0005", Operator = "=", Value = identificacionCliente }
                };

            }

        }
    }
}
