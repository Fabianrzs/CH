using AutoMapper;
using Azure.Core;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Main;
using GA2.Domain.Core.Credito.WorkManager;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
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

namespace GA2.Domain.Core.Embargos.GestorDocumental
{

    public class GestorDocumentarEmbargosBL: IGestorDocumentarEmbargosBL
    {
        private readonly IWorkManagerService _workmanagerService;
        private readonly ILogger<GestorDocumentarEmbargosBL> _logger;
        private readonly ICreditoRepository _creditoRepository;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly IMapper _mapper;
        private readonly CHKeysOptions _optionsCH;
        private readonly IIntegracionesGA2WorkManagerBusinessLogic _integracionWorkManager;
        private readonly IBlobStorageGenericRepository _blobStorage;
        private readonly ICuentasClientesRepository _cuentasClientesRepository;
        private readonly IEmbargosRepository _juzgadosRepository;

        public GestorDocumentarEmbargosBL(IWorkManagerService workmanagerService,
                                             ILogger<GestorDocumentarEmbargosBL> logger,
                                             ICreditoRepository creditoRepository,
                                             ICatalogosRepository catalogosRepository,
                                             IClientRequestProvider clientRequestProvider,
                                             IMapper mapper,
                                             IOptions<CHKeysOptions> optionsCH,
                                             IIntegracionesGA2WorkManagerBusinessLogic integracionWorkManager,
                                             IBlobStorageGenericRepository blobStorage,
                                             ICuentasClientesRepository cuentasClientesRepository,
                                             IEmbargosRepository JuzgadosRepository)
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
        }


        public async Task<Response<Form_InsertResponse>> RadicarGDPEmbargos(RadicadoEmbargoDto embargos)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            Response<Form_GetDataResponse> ValidaExpPadre = new Response<Form_GetDataResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();
            
            Task.Run(async () => { ValidaExpPadre = await validarExpedientePadre(embargos); }).Wait();
            if (ValidaExpPadre.Data.Form_GetDataResult.Rows == 0)
            {
                Task.Run(async () => { await RadicarExpedientePadre(embargos); }).Wait();
            }
           
           
            Task.Run(async () => { insertRequest = await GetInsertRequestEmbargos(embargos); }).Wait();

            Task.Run(async () => { insert = await _workmanagerService.FormInsertAsyncCH(insertRequest); }).Wait();

            return insert;
        }

        public async Task<Response<Form_InsertResponse>> RadicarGDPEmbargosCorrespondencia(EmbargosDto embargos)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            Task.Run(async () => { insertRequest = await GetInsertRequestEmbargosCorrespondencia(embargos); }).Wait();

            Task.Run(async () => { insert = await _workmanagerService.FormInsertAsyncCH(insertRequest); }).Wait();

            return insert;
        }

        public async Task<Response<Form_GetDataResponse>> ObtenerCorrespondenciaEmbargos(string IdentificacionCliente)
        {
            var queryRequest = CreateFormGetDataRequestForm03(IdentificacionCliente);
            var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);

            return response;
        }
        public async Task<Response<Form_InsertResponse>> RadicarExpedientePadre(RadicadoEmbargoDto embargos)
        {
            Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
            InsertRequestDto insertRequest = new InsertRequestDto();

            insertRequest = await CreateInsertRequestForm02(embargos);
            insert = await this._workmanagerService.FormInsertAsyncCH(insertRequest);

            return insert;
        }

        public async Task<Response<Form_GetDataResponse>> validarExpedientePadre(RadicadoEmbargoDto embargo)
        {

            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(embargo.clienteTipoIdentificacion, embargo.IdentificacionCliente);
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

        private async Task<InsertRequestDto> GetInsertRequestEmbargos(RadicadoEmbargoDto embargos)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(embargos.IdentificacionCliente));
            var tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(embargos.clienteTipoIdentificacion);
            var juzgado = await this._juzgadosRepository.ObtenerJuzgadoPorId(embargos.JuzgadoId);
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(embargos.clienteTipoIdentificacion, embargos.IdentificacionCliente)).Data;
            var UnidadEjecutora = "";
            if (InfoClienteFinaciera != null)
            {
                 UnidadEjecutora = _catalogosRepository.ObtenerUnidadesEjecutoras().Result.First(x => x.UEJ_ID == InfoClienteFinaciera.UEJ_Id).UEJ_NOMBRE;
            }
            UnidadEjecutora= UnidadEjecutora!="" ? UnidadEjecutora: "No encontrado";
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;
            var ciudad = (await _catalogosRepository.ObtenerCiudadPorId(cliente.IdCiudadResidencia));
            var departamento = (await _catalogosRepository.ObtenerDepartamentoPorId(ciudad.DPD_ID));
            var Embargo= embargos.TipoEmbargo != "" ? embargos.TipoEmbargo : "VALOR";
            var identificacionCliente = InfoClienteFinaciera != null ? InfoClienteFinaciera.numeroDocumento : cliente.Identificacion;
            var nombreCliente = InfoClienteFinaciera != null ? InfoClienteFinaciera.nombreCliente : cliente.PrimerNombre + " " + cliente.PrimerApellido;
            var fuerza = InfoClienteFinaciera != null ? InfoClienteFinaciera.fuerza : "No tiene";
            var telefono = InfoClienteFinaciera != null ? InfoClienteFinaciera.telefonoCelular : cliente.Telefono;
            dictionary.Add(new() { Field = "C0030", Value = $"{categoria}" });
            dictionary.Add(new() { Field = "C0031", Value = $"{cliente.Barrio}" });
            dictionary.Add(new() { Field = "C0033", Value = $"{telefono}" });
            dictionary.Add(new() { Field = "C0048", Value = $"{juzgado.CODIGO_INTERNO}" });
            dictionary.Add(new() { Field = "C0047", Value = $"{juzgado.NOMBRE}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{juzgado.ID_JUZGADO}" });
            dictionary.Add(new() { Field = "C0029", Value = $"{juzgado.CORREO_ELECTRONICO}" });
            dictionary.Add(new() { Field = "C0042", Value = $"{"03"}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{UnidadEjecutora}" });
            dictionary.Add(new() { Field = "C0050", Value = $"{identificacionCliente}" });
            dictionary.Add(new() { Field = "C0051", Value = $"{nombreCliente}" });
            dictionary.Add(new() { Field = "C0027", Value = $"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0026", Value = $"{ciudad.DPC_NOMBRE}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{fuerza}" });
            dictionary.Add(new() { Field = "C0016", Value = $"{embargos.IdentificacionCliente } | { cliente.PrimerNombre } { cliente.PrimerApellido } "});
            dictionary.Add(new() { Field = "C0019", Value = $"{"10"}" });
            dictionary.Add(new() { Field = "C0025", Value = $"{Embargo}" });
            dictionary.Add(new() { Field = "C0021", Value = $"{departamento.DPD_NOMBRE}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{cliente.Direccion}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{cliente.Correo}" });
            dictionary.Add(new() { Field = "C0032", Value = $"{cliente.Telefono}" });
            dictionary.Add(new() { Field = "C0041", Value = $"{Embargo}" });
           


            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "13",
                OfficeCode = "01",
                OperationUser = "Web",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }

        private async Task<InsertRequestDto> CreateInsertRequestForm02(RadicadoEmbargoDto embargos)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(embargos.IdentificacionCliente));
            var tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(embargos.clienteTipoIdentificacion);
            var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(embargos.clienteTipoIdentificacion, embargos.IdentificacionCliente);
            Response<InfoClienteDto> clienteDto = await _clientRequestProvider.ObtenerInfoCliente(embargos.clienteTipoIdentificacion, embargos.IdentificacionCliente);
            InfoDetalleCliente detalleCliente = await _cuentasClientesRepository.ObtenerDetalleInfoCliente(_mapper.Map<InfoCliente>(clienteDto.Data));
            var departament = await _catalogosRepository.ObtenerDepartamentoPorId(cliente.Data.IdDepartamentoResidencia);
            var ciudad = await _catalogosRepository.ObtenerCiudadPorId(cliente.Data.IdCiudadResidencia);
            var direccion = string.IsNullOrEmpty(cliente.Data.Direccion) ? "NO REGISTRA" : cliente.Data.Direccion;
            var correo = string.IsNullOrEmpty(cliente.Data.Correo) ? "NO REGISTRA" : cliente.Data.Correo;

            dictionary.Add(new()
            {
                Field = "C0001",
                Value = $"{"Expediente Padre: "} {cliente.Data.Identificacion} " +
                $"{embargos.NombreCompletoCliente}"
            });

            dictionary.Add(new() { Field = "C0011", Value = $"{(string.IsNullOrEmpty(direccion) ? "NO REGISTRA" : direccion)}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{(string.IsNullOrEmpty(cliente.Data.Barrio) ? "NO REGISTRA" : cliente.Data.Barrio)}" });
            dictionary.Add(new() { Field = "C0013", Value = $"{"Colombia"}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{(string.IsNullOrEmpty(departament.DPD_NOMBRE) ? "NO REGISTRA" : departament.DPD_NOMBRE)}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{(string.IsNullOrEmpty(ciudad.DPC_NOMBRE) ? "NO REGISTRA" : ciudad.DPC_NOMBRE)}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{(string.IsNullOrEmpty(cliente.Data.Identificacion) ? "NO REGISTRA" : cliente.Data.Identificacion)}" });
            dictionary.Add(new() { Field = "C0003", Value = $"{(string.IsNullOrEmpty(embargos.NombreCompletoCliente) ? "NO REGISTRA" : embargos.NombreCompletoCliente)}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{(string.IsNullOrEmpty($"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}") ? "NO REGISTRA" : $"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}")}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{(string.IsNullOrEmpty(direccion) ? "NO REGISTRA" : direccion)}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{(string.IsNullOrEmpty(InfoClienteFinaciera?.telefonoFijo) ? "NO REGISTRA" : InfoClienteFinaciera.telefonoFijo)}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{(string.IsNullOrEmpty(InfoClienteFinaciera?.telefonoCelular) ? "NO REGISTRA" : InfoClienteFinaciera.telefonoCelular)}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{(string.IsNullOrEmpty(cliente.Data.Correo) ? "NO REGISTRA" : cliente.Data.Correo)}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{(string.IsNullOrEmpty(detalleCliente.UNIDADE_EJECUTORA) ? "NO REGISTRA" : detalleCliente.UNIDADE_EJECUTORA)}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{(string.IsNullOrEmpty($"{detalleCliente.GRADO} - {detalleCliente.CATEGORIA}") ? "NO REGISTRA" : $"{detalleCliente.GRADO} - {detalleCliente.CATEGORIA}")}" });
            

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
       
        private QueryRequestDto CreateFormGetDataRequestForm03(string identificacionCliente)
        {
            return new QueryRequestDto()
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                FormCode = "03",
                FilterParameters = new() { Field = "C0001", Operator = "=", Value = identificacionCliente }
            };
        }

        private async Task<InsertRequestDto> GetInsertRequestEmbargosCorrespondencia(EmbargosDto embargos)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var InfoClienteFinaciera = _mapper.Map<SimulacionClienteDto>(await _creditoRepository.ConsultarSimulacionCliente(embargos.IdentificacionCliente));
            var tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(embargos.TipoIdentificacionId);
            var juzgado = await this._juzgadosRepository.ObtenerJuzgadoPorId(embargos.JuzgadoId);
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(embargos.TipoIdentificacionId, InfoClienteFinaciera.numeroDocumento)).Data;
            var UnidadEjecutora = _catalogosRepository.ObtenerUnidadesEjecutoras().Result.First(x => x.UEJ_ID == InfoClienteFinaciera.UEJ_Id).UEJ_NOMBRE;
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;
            var ciudad = (await _catalogosRepository.ObtenerCiudadPorId(cliente.IdCiudadResidencia));
            var departamento = (await _catalogosRepository.ObtenerDepartamentoPorId(ciudad.DPD_ID));
            var Embargo = embargos.TipoEmbargo != null ? embargos.TipoEmbargo : "VALOR";
            dictionary.Add(new() { Field = "C0001", Value = $"{embargos.IdentificacionCliente}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoNombre}" });       
            dictionary.Add(new() { Field = "C0003", Value = $"{tipoDocumemto.TID_CODIGO}-{tipoDocumemto.TID_DESCRIPCION}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{cliente.Correo}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{departamento.DPD_NOMBRE}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{ciudad.DPC_NOMBRE}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{cliente.Direccion}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{Embargo}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{"1"}" });
            dictionary.Add(new() { Field = "C0017", Value = $"{embargos.IdentificacionCliente} | {cliente.PrimerNombre} {cliente.PrimerApellido}" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "03",
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
