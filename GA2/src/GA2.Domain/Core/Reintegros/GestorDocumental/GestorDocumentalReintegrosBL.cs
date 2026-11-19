using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Domain.Entities.Postulacion;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Reintegros.GestorDocumental
{
    public class GestorDocumentalReintegrosBL: IGestorDocumentalReintegrosBL
    {
        private readonly IWorkManagerService _workmanagerService;
        private readonly ILogger<GestorDocumentalReintegrosBL> _logger;
        private readonly IMapper _mapper;
        private readonly CHKeysOptions _optionsCH;
        private readonly IIntegracionesGA2WorkManagerBusinessLogic _integracionWorkManager;
        private readonly IBlobStorageGenericRepository _blobStorage;
        private readonly IUsersRepository _usersRepository; 
        private readonly ICreditoRepository _creditoRepository;
        public GestorDocumentalReintegrosBL(IWorkManagerService workmanagerService,
                                             ILogger<GestorDocumentalReintegrosBL> logger,
                                             IMapper mapper,
                                             IOptions<CHKeysOptions> optionsCH,
                                             IIntegracionesGA2WorkManagerBusinessLogic integracionWorkManager,
                                             IBlobStorageGenericRepository blobStorage,
                                             IUsersRepository usersRepository,
                                         ICreditoRepository creditoRepository)
        {
            _workmanagerService = workmanagerService ?? throw new ArgumentNullException(nameof(workmanagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _optionsCH = optionsCH.Value ?? throw new ArgumentNullException(nameof(optionsCH));
            _integracionWorkManager = integracionWorkManager ?? throw new ArgumentNullException(nameof(integracionWorkManager));
            _blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
            _usersRepository = usersRepository;

            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
        }

        public async Task<Response<Form_GetDataResponse>> ObtenerCorrespondenciaReintegros(string radicado)
        {

            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                var queryRequest = CreateFormGetDataRequestForm446(radicado);
                var response = await this._workmanagerService.FormGetDataAsyncCH(queryRequest);

                return response;
            }
            else
            {
                return new Response<Form_GetDataResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo", Data = null };
            }
        }
        public async Task<Response<Form_InsertResponse>> RadicarGDPComunicaciones(InsertarComunicacionDto insertarComunicacion)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();
                Response<Form_GetDataResponse> ValidaExpPadre = new Response<Form_GetDataResponse>();
                InsertRequestDto insertRequest = new InsertRequestDto();

                Task.Run(async () => { insertRequest = await GetInsertRequestEmbargosCorrespondencia(insertarComunicacion); }).Wait();

                Task.Run(async () => { insert = await _workmanagerService.FormInsertAsyncCH(insertRequest); }).Wait();

                return insert;
            }
            else
            {
                return new Response<Form_InsertResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo", Data = null };
            }
        }

        public async Task<Response<Form_UpdateResponse>> ActualizarRadicadoPostulacion(ActualizarPostulacionDto actualizarPostulacionDto)
        {
            UpdateRequestDto updateRequest = new UpdateRequestDto();

            updateRequest = await CreateUpdateRequest(actualizarPostulacionDto);

            var update = await this._workmanagerService.FormUpdateAsyncCH(updateRequest);

            return update;
        }
        private QueryRequestDto CreateFormGetDataRequestForm446(string radicado)
        {
            return new QueryRequestDto()
            {
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                FormCode = "446",
                FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = radicado }
            };
        }

        private async Task<UpdateRequestDto> CreateUpdateRequest(ActualizarPostulacionDto actualizarPostulacionDto)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var user = await _usersRepository.ObtenerUsuarioPorId(actualizarPostulacionDto.CreadoPor);
            var numeroFolio = actualizarPostulacionDto.Numero_Folio != null ? actualizarPostulacionDto.Numero_Folio : 0;
            var nombre = user.USR_NOMBRE.ToLower();
            var apellido = user.USR_APELLIDO.ToLower();


            dictionary.Add(new() { Field = "C0034", Value = $"{numeroFolio}" });

            return new UpdateRequestDto()
            {
                Data = dictionary.ToArray(),
                FilterParameters = new() { Field = "Cod_Barras", Operator = "=", Value = actualizarPostulacionDto.Radicado },
                FormCode = "39",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                },
                OperationUser = $"{nombre}.{apellido}"
            };
        }

        private async Task<InsertRequestDto> GetInsertRequestEmbargosCorrespondencia(InsertarComunicacionDto insertarComunicacion)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            dictionary.Add(new() { Field = "C0001", Value = $"{insertarComunicacion.identificacion}" });
            dictionary.Add(new() { Field = "C0002", Value = $"{insertarComunicacion.nombreCompleto}" });
            dictionary.Add(new() { Field = "C0003", Value = $"{insertarComunicacion.tipoDocumento}" });
            dictionary.Add(new() { Field = "C0004", Value = $"{insertarComunicacion.contacto}" });
            dictionary.Add(new() { Field = "C0005", Value = $"{insertarComunicacion.cargo}" });
            dictionary.Add(new() { Field = "C0006", Value = $"{insertarComunicacion.email}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{insertarComunicacion.departamento}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{insertarComunicacion.ciudad}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{insertarComunicacion.direccion}" });
            dictionary.Add(new() { Field = "C0010", Value = $"{insertarComunicacion.asunto}" });
            dictionary.Add(new() { Field = "C0012", Value = $"{"1"}" });
            dictionary.Add(new() { Field = "C0011", Value = $"{insertarComunicacion.anexo}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{insertarComunicacion.barrio}" });
            dictionary.Add(new() { Field = "C0029", Value = $"{insertarComunicacion.radicadoRelacionado}" });

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
