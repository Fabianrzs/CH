using AutoMapper;
using Azure.Storage.Blobs;
using GA2.Application.Dto;
using GA2.Application.Dto.Reintegros;
using GA2.Application.Dto.Seven;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.Reintegros;
using GA2.Domain.Core.Reintegros.GestorDocumental;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Postulacion;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Extensions;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GA2.Domain.Core
{
    /// <summary>
    /// ReintegrosBusinessLogic
    /// </summary>
    public class ReintegrosBusinessLogic : IReintegrosBusinessLogic
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// IMapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// IReintegrosRepository
        /// </summary>
        private readonly IReintegrosRepository _reintegrosRepository;

        /// <summary>
        /// IMensajeRepository
        /// </summary>
        private readonly IMensajeRepository _messageRepository;

        /// <summary>
        /// IReportesRequestProvider
        /// </summary>
        private readonly IReportesRequestProvider _iReportesRequestProvider;

        /// <summary>
        /// IClientRequestProvider
        /// </summary>
        private readonly IClientRequestProvider _iClienteRequestProvider;

        private readonly IGestorDocumentalReintegrosBL _gestordocumentalReintegros;

        private readonly IConsumoCDP _wsCDP;

        private readonly ICesantiasRepository _cesantiasRepository;

        private readonly ICatalogosRepository _catalogsRepository;

        private readonly IConsumoWSProveedor _wsProveedor;

        private readonly IConsumoWSTercero _wsTercero;

        private readonly IConsumoWSFactura _wsFactura;

        private readonly IConsumoSCnMcont _wsMovContable;

        private readonly ICuentasClientesBusinessLogic _cuentasClientesBusinessLogic;
        private readonly ISolicitudRepository _solicitudRepository;

        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IUsersRepository _usersRepository;




        /// <summary>
        /// ReintegrosBusinessLogic
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="reintegrosRepository"></param>
        public ReintegrosBusinessLogic(
            IMapper mapper,
            IReintegrosRepository reintegrosRepository,
            IReportesRequestProvider iReportesRequestProvider,
            IMensajeRepository messageRepository,
            IClientRequestProvider iClienteRequestProvider,
            IGestorDocumentalReintegrosBL iGestorDocumentalReintegrosBL,
            IConsumoCDP wsCDP,
            ICesantiasRepository cesantiasRepository,
            ICatalogosRepository catalogsRepository,
            IConsumoWSTercero wsTercero,
            IConsumoWSProveedor wsProveedor,
            IConsumoWSFactura wsFactura,
            IConsumoSCnMcont wsMovContable,
            ICuentasClientesBusinessLogic cuentasClientesBusinessLogic,
            IWebHostEnvironment webHostEnvironment,
            ISolicitudRepository solicitudRepository,
            IOptions<BlobStorageOptions> options,
            IUsersRepository usersRepository
            )

        {
            _mapper = mapper;
            _reintegrosRepository = reintegrosRepository ?? throw new ArgumentNullException(nameof(reintegrosRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _iReportesRequestProvider = iReportesRequestProvider ?? throw new ArgumentNullException(nameof(iReportesRequestProvider));
            _iClienteRequestProvider = iClienteRequestProvider ?? throw new ArgumentNullException(nameof(iClienteRequestProvider)); ;
            _gestordocumentalReintegros = iGestorDocumentalReintegrosBL;
            _wsCDP = wsCDP ?? throw new ArgumentNullException(nameof(wsCDP));
            _cesantiasRepository = cesantiasRepository;
            _catalogsRepository = catalogsRepository;
            _wsTercero = wsTercero ?? throw new ArgumentNullException(nameof(wsTercero));
            _wsProveedor = wsProveedor ?? throw new ArgumentNullException(nameof(wsProveedor));
            _wsFactura = wsFactura ?? throw new ArgumentNullException(nameof(wsFactura));
            _wsMovContable = wsMovContable ?? throw new ArgumentNullException(nameof(wsMovContable));
            _cuentasClientesBusinessLogic = cuentasClientesBusinessLogic ?? throw new ArgumentNullException(nameof(cuentasClientesBusinessLogic));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _solicitudRepository = solicitudRepository;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// ObtenerEstadoReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EstadoReintegrosDto>>> ObtenerEstadoReintegros()
        {
            IEnumerable<EstadoReintegrosDto> estados = _mapper.Map<IEnumerable<EstadoReintegrosDto>>(await this._reintegrosRepository.ObtenerEstadoReintegros());
            return new Response<IEnumerable<EstadoReintegrosDto>> { Data = estados };
        }

        /// <summary>
        /// ObtenerTipoReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoReintegrosDto>>> ObtenerTipoReintegros()
        {
            IEnumerable<TipoReintegrosDto> tipo = _mapper.Map<IEnumerable<TipoReintegrosDto>>(await this._reintegrosRepository.ObtenerTipoReintegros());
            return new Response<IEnumerable<TipoReintegrosDto>> { Data = tipo };
        }

        /// <summary>
        /// ObtenerTipoReintegrosById
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        public async Task<Response<TipoReintegrosDto>> ObtenerTipoReintegroById(int idTipo)
        {
            TipoReintegrosDto tipo = _mapper.Map<TipoReintegrosDto>(await this._reintegrosRepository.ObtenerTipoReintegrosById(idTipo));
            return new Response<TipoReintegrosDto> { Data = tipo };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reintegro"></param>
        /// <returns></returns>
        public async Task<Response<ReintegrosDto>> CrearReintegro(ReintegrosDto request)
        {
            string[] identificaciones = request.DetalleReintegroDto.Identificacion.Split(",");
            List<string> listIdentificaciones = new();
            listIdentificaciones.AddRange(identificaciones);
            ReintegrosDto reintegro = new();

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(listIdentificaciones));

            if (listAfiliados.Data.Count > 0)
            {
                foreach (var item in listAfiliados.Data)
                {
                    reintegro = new ReintegrosDto()
                    {
                        CreadoPor = request.CreadoPor,
                        FechaCreacion = request.FechaCreacion,
                        IdTipoReintegro = request.IdTipoReintegro,
                        IdEstadoReintegro = request.IdEstadoReintegro,
                        FechaReintegro = request.FechaReintegro,
                        NumeroRadicado = request.NumeroRadicado,
                        ValorReintegro = request.ValorReintegro,
                        IdUnidadEjecutora = request.IdUnidadEjecutora
                    };

                    ReintegrosDto response = await InsertarReintegro(reintegro);

                    if (response.IdReintegro > 0)
                    {
                        DetalleReintegroDto detalle = new()
                        {
                            IdReintegro = response.IdReintegro,
                            CreadoPor = reintegro.CreadoPor,
                            FechaCreacion = DateTime.Now,
                            Identificacion = item.Documento
                        };

                        DetalleReintegroDto responseDet = await InsertarDetalleReintegro(detalle);

                        reintegro.DetalleReintegroDto = responseDet;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Error al crear el reintegro.");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"No existe información en la base de datos para los documentos ingresados. Por favor revise.");
            }

            return new Response<ReintegrosDto>
            {
                Data = reintegro,
                ReturnMessage = null
            };
        }

        /// <summary>
        /// CrearDetalleReintegro
        /// </summary>
        /// <param name="reintegro"></param>
        /// <returns></returns>
        public async Task<Response<DetalleReintegroDto>> CrearDetalleReintegro(DetalleReintegroDto reintegro)
        {
            var response = _mapper.Map<DetalleReintegroDto>(
                    await _reintegrosRepository.CrearDetalleReintegro(
                         _mapper.Map<DetalleReintegro>(reintegro)));

            string mensaje = "";

            if (response != null)
            {
                mensaje = ResponseMessageConstants.CodigoMensaje1;
                response.Nombres = (await _iReportesRequestProvider.ObtenerNombresCliente(response.Identificacion)).Data.NombreCompleto;
            }

            MensajeDto Mensaje = _mapper.Map<MensajeDto>(await this._messageRepository.ObtenerMensaje(mensaje));
            return new Response<DetalleReintegroDto>
            {
                Data = response,
                ReturnMessage = Mensaje.Mensaje
            };
        }

        /// <summary>
        /// ObtenerTipoReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ListaReintegrosDto>>> ObtenerListadoReintegros(ReintegrosParamsDto request)
        {
            IEnumerable<ListaReintegrosDto> tipo = _mapper.Map<IEnumerable<ListaReintegrosDto>>(
                    await _reintegrosRepository.ObtenerListadoReintegros(
                         _mapper.Map<ReintegrosParams>(request)));
            if (tipo.Any())
            {
                var agruparTipo = tipo.GroupBy(x => x.Id_Reintegro).ToList();
                var result = new List<ListaReintegrosDto>();
                foreach (var item in agruparTipo)
                {
                    if (item.Count() > 1)
                    {
                        foreach (var afiliado in item)
                        {
                            afiliado.NombreAfiliado = "...";
                            afiliado.NumeroDocumento = "...";
                            afiliado.TipoReintegro = "...";
                            afiliado.ValorSueldo = "...";
                        }
                    }
                    result.Add(item.FirstOrDefault());
                }
                return new Response<IEnumerable<ListaReintegrosDto>> { Data = result };
            }

            return new Response<IEnumerable<ListaReintegrosDto>> { Data = null, ReturnMessage = "no se encontro ninguna informacion" };
        }

        /// <summary>
        /// InsertarReintegro
        /// </summary>
        /// <returns></returns>
        public async Task<ReintegrosDto> InsertarReintegro(ReintegrosDto reintegro)
        {
            var response = _mapper.Map<ReintegrosDto>(
                        await _reintegrosRepository.CrearReintegro(
                             _mapper.Map<Entities.Reintegros>(reintegro)));

            return response;
        }

        /// <summary>
        /// InsertarDetalleReintegro
        /// </summary>
        /// <returns></returns>
        public async Task<DetalleReintegroDto> InsertarDetalleReintegro(DetalleReintegroDto detalle)
        {
            var responseDet = _mapper.Map<DetalleReintegroDto>(
                        await _reintegrosRepository.CrearDetalleReintegro(
                             _mapper.Map<DetalleReintegro>(detalle)));

            return responseDet;
        }

        /// <summary>
        /// AgregarAfiliadoReintegro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<AfiliadoReintegroDto>>> AgregarAfiliadoReintegro(AfiliadoReintegroParamDto request)
        {
            string[] identificaciones = request.Identificacion.Split(",");
            List<string> listIdentificaciones = new();
            listIdentificaciones.AddRange(identificaciones);
            List<AfiliadoReintegroDto> afiliadoReintegros = new();

            var listAfiliados = (await _iReportesRequestProvider.ObtenerNombresClientesPorDocumentos(listIdentificaciones));
            TipoReintegrosDto tipo = _mapper.Map<TipoReintegrosDto>(await this._reintegrosRepository.ObtenerTipoReintegrosById(request.IdTipoReintegro));

            foreach (var item in listAfiliados.Data)
            {
                AfiliadoReintegroDto afiliado = new()
                {
                    Identificacion = item.Documento,
                    IdTipoReintegro = tipo.IdTipoReintegro,
                    Nombres = item.NombreCompleto,
                    TipoReintegro = tipo.Descripcion
                };

                afiliadoReintegros.Add(afiliado);
            };

            return new Response<IEnumerable<AfiliadoReintegroDto>> { Data = afiliadoReintegros };
        }
        public async Task<Response<ReintefroDto>> CreateReintegro(ReintefroDto request)
        {
            var mapReintegro = _mapper.Map<Reintegro>(request);
            mapReintegro.FechaCreacion = DateTime.Now;
            mapReintegro.Estado = true;
            var afiliadosValidate = new List<AfiliadosReintegrosDto>();
            var afiliadoError = new List<AfiliadosReintegrosDto>();
            AfiliadosReintegrosDto validateAfiliados = null;
            if (!request.afiliados.Any())
            {
                return new Response<ReintefroDto> { Data = null, IsSuccess = false, ReturnMessage = "No se puedo crear el reintegro, no hay afiliados" };
            }
            foreach (var item in request.afiliados)
            {
                var validateAfiliado = await _reintegrosRepository.GetValidateAfiliado(item.NumeroDocumento);
                if (validateAfiliado.Any())
                {
                    afiliadosValidate.Add(item);
                }

                var validate = validateAfiliado.Where(x => x.Id_Tipo_Reintegro == 3).ToList();
                var validateBloqueo = validateAfiliado.Where(x => x.Id_Tipo_Reintegro == EnumsTipoReintegro.Bloqueos).ToList();
                if (validateAfiliado.Any())
                {
                    foreach (var afiliado in validateBloqueo)
                    {
                        if (afiliado.Id_Tipo_Reintegro == EnumsTipoReintegro.Bloqueos)
                        {
                            if (afiliado.EstadoAfiliao == EnumsTipoEstadoReintro.Procesada)
                            {
                                if (request.Id_Tipo_Reintegro == 3)
                                {
                                    if (afiliadosValidate.Count > 0)
                                    {
                                        validateAfiliados = afiliadosValidate.Where(x => x.NumeroDocumento == item.NumeroDocumento).FirstOrDefault();

                                    }
                                    if (validateAfiliados == null)
                                    {
                                        var validateReintegro = validate.Where(
                                            x => x.Id_Tipo_Reintegro == 3
                                            && x.NumeroDocumento == item.NumeroDocumento).FirstOrDefault();
                                        if (validateReintegro is null)
                                        {
                                            afiliadosValidate.Add(item);
                                        }
                                        else
                                        {
                                            if (validateReintegro.EstadoAfiliao == EnumsTipoEstadoReintro.Procesada)
                                            {
                                                afiliadosValidate.Add(item);
                                            }
                                            else
                                            {
                                                return new Response<ReintefroDto> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo crear el reintegro, porque no se encuentra procesada el reintegro " };
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    afiliadosValidate.Add(item);
                                }
                            }
                            else
                            {
                                return new Response<ReintefroDto> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo crear el reintegro, porque no se encuentra procesada el reintegro " };
                            }
                        }

                    }

                }
                else
                {

                    afiliadoError.Add(item);
                }
            }
            if (afiliadosValidate.Count > 0)
            {
                var consecutivo = await _reintegrosRepository.UpdateConsecutiveSolReintegro(0);
                mapReintegro.NumeroSolicitud = consecutivo;
                var insertReintegro = await _reintegrosRepository.CreateReintregro(mapReintegro);
                var lisAfiliado = new List<AfiliadosReintegrosDto>();
                var insertAfiliado = new AfiliadosReintegro();
                var requestReintegro = new ReintefroDto();
                foreach (var item in afiliadosValidate)
                {
                    var mapAfiliados = _mapper.Map<AfiliadosReintegro>(item);
                    mapAfiliados.IdReintegro = insertReintegro.Id_Reintegro;
                    mapAfiliados.ValorSueldo = 0;
                    mapAfiliados.EstadoAfiliao = $"EN {EnumEstadoAfiliado.EVALUACION}";
                    if (item.Id_Tipo_Reintegro == 4)
                    {
                        mapAfiliados.EstadoAfiliao = EnumsTipoEstadoReintro.Procesada;
                    }
                    insertAfiliado = await _reintegrosRepository.CreateAfiliadoReintregro(mapAfiliados);
                }
                if (insertAfiliado is not null)
                {
                    requestReintegro = _mapper.Map<ReintefroDto>(insertReintegro);
                    var requesrAfiliado = await _reintegrosRepository.GetDataAfiliadoById(insertReintegro.Id_Reintegro);
                    var mapAfiliado = _mapper.Map<IEnumerable<AfiliadosReintegrosDto>>(requesrAfiliado);
                    requestReintegro.afiliados = mapAfiliado;
                    if (afiliadoError.Count > 0)
                    {
                        return new Response<ReintefroDto> { Data = requestReintegro, IsSuccess = true, ReturnMessage = "Por favor verifique algunos afiliados ya se encuentra registrados" };
                    }
                    return new Response<ReintefroDto> { Data = requestReintegro, IsSuccess = true, ReturnMessage = "OK" };
                }
            }
            return new Response<ReintefroDto> { Data = null, IsSuccess = false, ReturnMessage = "No se puedo crear el reintegro, valide a sus afiliados o ingrese afiliado validos" };
        }

        public async Task<Response<ReintefroDto>> GetDataReintregoByID(Guid Id)
        {
            var response = await _reintegrosRepository.GetDataReintegrById(Id);
            if (response is not null)
            {
                var mapReintegro = _mapper.Map<ReintefroDto>(response);
                var responseAfiliado = await _reintegrosRepository.GetDataAfiliadoById(Id);
                if (responseAfiliado.Any())
                {

                    var mapAfiliados = _mapper.Map<List<AfiliadosReintegrosDto>>(responseAfiliado);
                    mapReintegro.afiliados = mapAfiliados;
                    return new Response<ReintefroDto> { Data = mapReintegro, IsSuccess = true, ReturnMessage = "OK" };
                }
                return new Response<ReintefroDto> { Data = mapReintegro, IsSuccess = true, ReturnMessage = "OK" };
            }

            return new Response<ReintefroDto> { Data = null, IsSuccess = true, ReturnMessage = "OK" };
        }

        public async Task<Response<AfiliadosReintegrosDto>> UpdateAfiliadoReintegro(AfiliadosReintegrosDto request)
        {
            var mapAfiliado = _mapper.Map<AfiliadosReintegro>(request);
            mapAfiliado.FechaFin = DateTime.Now;
            var response = await _reintegrosRepository.UpdateAfiliadoReintegro(mapAfiliado);
            if (response is not null)
            {
                if (request.EstadoAfiliao == EnumsTipoEstadoReintro.Procesada)
                {
                    await _reintegrosRepository.UpdateFechaFinReintegro(request.NumeroDocumento, request.IdReintegro);
                }
                var mapReintegro = _mapper.Map<AfiliadosReintegrosDto>(response);
                return new Response<AfiliadosReintegrosDto> { Data = mapReintegro, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<AfiliadosReintegrosDto> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la informacion" };
        }
        public async Task<Response<string>> UpdateDateEndReintegro(Guid idReintegro)
        {
            var dataListAfiliados = await _reintegrosRepository.GetDataAfiliadoById(idReintegro);
            var isAviable = dataListAfiliados.Where(x => x.EstadoAfiliao == EnumsTipoEstadoReintro.Procesada).ToList();
            var resultAfiliado = new List<AfiliadosReintegro>();
            if (isAviable.Count > 0)
            {
                foreach (var afiliado in isAviable)
                {
                    var result = await _reintegrosRepository.UpdateFechaFinReintegro(afiliado.NumeroDocumento, idReintegro);
                    if (result != null)
                    {
                        resultAfiliado.Add(result);
                    }
                }
                if (resultAfiliado.Count > 0)
                {
                    return new Response<string> { Data = "Succes!!", IsSuccess = true, ReturnMessage = "Fecha generada con exito" };
                }
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo generar la fecha fin" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo generar la fecha fin" };
        }
        public async Task<Response<string>> UpdateListAfiliado(IEnumerable<AfiliadosReintegrosDto> afiliados)
        {
            if (afiliados.Any())
            {
                var messageResult = new List<int>();
                var contador = 0;
                foreach (var afiliado in afiliados)
                {
                    if (afiliado.Id_Tipo_Reintegro > 0)
                    {
                        var mapAfiliado = _mapper.Map<AfiliadosReintegro>(afiliado);
                        var response = await _reintegrosRepository.UpdateStatusAfiliadoReintegro(
                            mapAfiliado.Id_Afiliado, mapAfiliado.IdReintegro, mapAfiliado.EstadoAfiliao);
                        contador++;
                        messageResult.Add(contador);
                    }
                }

                var idReintegro = afiliados.Select(x => x.IdReintegro).FirstOrDefault()
; var dataListAfiliados = await _reintegrosRepository.GetDataAfiliadoById(idReintegro);
                var isAviable = dataListAfiliados.Where(x => x.EstadoAfiliao == EnumsTipoEstadoReintro.Procesada).ToList();
                if (isAviable.Count > 0)
                {
                    foreach (var afiliado in isAviable)
                    {
                        var result = await _reintegrosRepository.UpdateFechaFinReintegro(afiliado.NumeroDocumento, idReintegro);
                    }
                }
                if (messageResult.Count > 0)
                {
                    return new Response<string> { Data = EnumMensajeRepuesta.Exitoso.ToString(), IsSuccess = false, ReturnMessage = EnumMensajeRepuesta.Exitoso.ToString() };
                }
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo gurdar la lista" };
        }
        public async Task<Response<string>> SaveListAfiliado(IEnumerable<AfiliadoReintegroListDto> afiliados)
        {
            if (afiliados.Any())
            {
                var messageResult = new List<int>();
                foreach (var afiliado in afiliados)
                {
                    if (afiliado.Id_Tipo_Reintegro > 0)
                    {
                        var mapAfiliado = _mapper.Map<AfiliadoReinegroList>(afiliado);
                        var response = await _reintegrosRepository.SaveListAfiliado(mapAfiliado);
                        var updateStatus = await _reintegrosRepository.UpdateStatusAfiliadoReintegro
                            (afiliado.IdAfiliadoReintegro, afiliado.IdReintegro,
                            afiliado.EstadoAfiliao);

                        if (updateStatus > 0)
                        {
                            await _reintegrosRepository.UpdateFechaFinReintegro(afiliado.NumeroDocumento, afiliado.IdReintegro);
                        }
                        messageResult.Add(response);
                    }
                }
                if (messageResult.Any())
                {
                    return new Response<string> { Data = EnumMensajeRepuesta.Exitoso.ToString(), IsSuccess = false, ReturnMessage = EnumMensajeRepuesta.Exitoso.ToString() };
                }
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo gurdar la lista" };
        }
        public async Task<Response<IEnumerable<AfiliadoReintegroListDto>>> GetSaveListAfiliado(Guid idReintegro)
        {
            var resultList = await _reintegrosRepository.GetListAfiliado(idReintegro);
            if (resultList.Any())
            {
                var resultMap = _mapper.Map<IEnumerable<AfiliadoReintegroListDto>>(resultList);
                return new Response<IEnumerable<AfiliadoReintegroListDto>> { Data = resultMap, IsSuccess = true, ReturnMessage = EnumMensajeRepuesta.Exitoso.ToString() };

            }
            return new Response<IEnumerable<AfiliadoReintegroListDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo obtener la informacion" };
        }

        public async Task<Response<IEnumerable<AfiliadosReintegrosDto>>> GetAfiliadoReintegro(Guid Id)
        {
            var response = await _reintegrosRepository.GetDataAfiliadoById(Id);
            if (response.Any())
            {
                var mapReintegro = _mapper.Map<IEnumerable<AfiliadosReintegrosDto>>(response);
                return new Response<IEnumerable<AfiliadosReintegrosDto>> { Data = mapReintegro, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<IEnumerable<AfiliadosReintegrosDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la informacion" };
        }
        public async Task<Response<int>> UpdateValuePercent(UpdateValorPercent percent)
        {
            var saldoActual = await _reintegrosRepository.GetValuePercentSolicitado(percent.IdAfiliado);
            var valorIngresar = 0m;
            if (saldoActual <= 0)
            {
                valorIngresar = percent.Valor;
            }
            if (saldoActual > 0)
            {
                valorIngresar = saldoActual + percent.Valor;
            }
            var response = await _reintegrosRepository.UpdateValuePercent(percent.IdAfiliado, valorIngresar);
            if (response >= 1)
            {
                return new Response<int> { Data = 1, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<int> { Data = 0, IsSuccess = false, ReturnMessage = "No se pudo actualizar la informacion" };
        }
        public async Task<Response<int>> UpdateValueSolicitado(UpdateValorPercent percent)
        {
            var saldoActual = await _reintegrosRepository.GetValueSolicitado(percent.IdAfiliado);
            var valorIngresar = 0m;
            if (saldoActual <= 0)
            {
                valorIngresar = percent.Valor;
            }
            if (saldoActual > 0)
            {
                valorIngresar = saldoActual + percent.Valor;
            }
            var response = await _reintegrosRepository.UpdateValueSolicitado(percent.IdAfiliado, valorIngresar);
            if (response >= 1)
            {
                return new Response<int> { Data = 1, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<int> { Data = 0, IsSuccess = false, ReturnMessage = "No se pudo actualizar la informacion" };
        }
        public async Task<Response<IEnumerable<ReintegroResponseDTO>>> GetDataReintegro(AfiliadoConsulta afiliado)
        {
            var response = await _reintegrosRepository.GetDataReintegro(afiliado.NumeroDocumento.ToString(), afiliado.TipoDocumento);
            if (response is not null)
            {
                var mapeo = _mapper.Map<IEnumerable<ReintegroResponseDTO>>(response);
                foreach (var item in mapeo)
                {
                    item.TipoSolicitud = EnumsTipoSolicitudReintegro.TipoSolicitudReintegro;
                }
                return new Response<IEnumerable<ReintegroResponseDTO>> { Data = mapeo, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<IEnumerable<ReintegroResponseDTO>> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo consultar la informacion" };
        }
        public async Task<Response<int>> CleanValueAfiliado(UpdateValorPercent percent)
        {
            await _reintegrosRepository.ClenValueAfiliado(percent.IdAfiliado, percent.Valor);

            return new Response<int> { Data = 1, IsSuccess = false, ReturnMessage = "Exitosament,actualizar la informacion" };
        }
        /// <summary>
        /// ObtenerCorrespondenciaReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<Response<CorrespondenciaReintegro>> ObtenerCorrespondenciaReintegros(string radicado)
        {

            Response<CorrespondenciaReintegro> Correspondencia = new()
            {
                Data = new CorrespondenciaReintegro()
            };
            var responseDet = await _gestordocumentalReintegros.ObtenerCorrespondenciaReintegros(radicado);
            foreach (var item in responseDet.Data.Form_GetDataResult.Data)
            {
                if (item.Key == "C0006")
                {
                    Correspondencia.Data.identificacion = item.Value;
                }
                if (item.Key == "C0003")
                {
                    Correspondencia.Data.modalidad = item.Value;
                }
                if (item.Key == "C0008")
                {
                    Correspondencia.Data.unidadEjecutora = item.Value;
                }
                if (item.Key == "C0014")
                {
                    Correspondencia.Data.radicadoRelacionadoUE = item.Value;
                }
                if (item.Key == "Cod_Barras")
                {
                    Correspondencia.Data.radicado = item.Value;
                }
                //if (item.Key == "C0010")
                //{
                //    Correspondencia.Data.observaciones = item.Value;
                //}
                //if (item.Key == "C0010")//NO EXISTE EN 446
                //{
                //    Correspondencia.Data.cargo = item.Value;
                //}
                //if (item.Key == "C0009")//NO EXISTE EN 446
                //{
                //    Correspondencia.Data.contacto = item.Value;
                //}
                //if (item.Key == "C0017")//NO EXISTE EN 446
                //{
                //    Correspondencia.Data.correoElectronico = item.Value;
                //}
                //if (item.Key == "C0012")//NO EXISTE EN 446
                //{
                //    Correspondencia.Data.ciudad = item.Value;
                //}
                //if (item.Key == "C0018")//EXISTE RADICADO RELACIONADO UNIDAD EJECUTORA 
                //                        //TAMBIEN RADICADO RELACIONADO 2
                //                        //Y RADICADO
                //{
                //    Correspondencia.Data.radicadoRelacionado = item.Value;
                //}
            }
            return Correspondencia;
        }

        /// <summary>
        /// InsertFlujo03
        /// </summary>
        /// <returns></returns>
        public async Task<Response<string>> InsertarComunicaciones(insertarComunicacionDto reintegro)
        {

            Response<string> Correspondencia = new();
            var responseDet = await _gestordocumentalReintegros.ObtenerCorrespondenciaReintegros(reintegro.Radicado);
            InsertarComunicacionDto insertarComunicacion = new();
            foreach (var item in responseDet.Data.Form_GetDataResult.Data)
            {
                switch (item.Key)
                {
                    case "C0007":
                        insertarComunicacion.identificacion = item.Value;
                        break;
                    case "C0022":
                        insertarComunicacion.observaciones = item.Value;
                        break;
                    case "C0010":
                        insertarComunicacion.cargo = item.Value;
                        break;
                    case "C0009":
                        insertarComunicacion.contacto = item.Value;
                        break;
                    case "C0017":
                        insertarComunicacion.email = item.Value;
                        break;

                    case "C0012":
                        insertarComunicacion.ciudad = item.Value;
                        break;
                    case "C0008":
                        insertarComunicacion.nombreCompleto = item.Value;
                        break;
                    case "C0006":
                        insertarComunicacion.tipoDocumento = item.Value;
                        break;
                    case "C0011":
                        insertarComunicacion.departamento = item.Value;
                        break;
                    case "C0013":
                        insertarComunicacion.direccion = item.Value;
                        break;
                    case "C0005":
                        insertarComunicacion.asunto = item.Value;
                        break;

                    default:
                        break;
                }

            }
            insertarComunicacion.radicadoRelacionado = reintegro.Radicado;
            var responseInsert = await _gestordocumentalReintegros.RadicarGDPComunicaciones(insertarComunicacion);
            if (responseInsert.ReturnMessage == "El formulario fue guardado")
            {
                var numeroRadicadoResponse = responseInsert.Data.Form_InsertResult.BarCode;
                await _reintegrosRepository.ActualizarNumeroRadicadoReintegro(reintegro.IdReintegro, numeroRadicadoResponse);
                Correspondencia.ReturnMessage = "Formulario guardado exitosamente " + numeroRadicadoResponse;
                return Correspondencia;
            }
            Correspondencia.ReturnMessage = "Error al guardar el formulario";

            return Correspondencia;
        }
        public async Task<Response<string>> ActualizarRadicadoPostulacion(ActualizarPostulacionDto actualizarPostulacionDto)
        {
            var responseDet = await _gestordocumentalReintegros.ActualizarRadicadoPostulacion(actualizarPostulacionDto);
            if (responseDet.IsSuccess == true && responseDet.ReturnMessage == "El formulario fue actualizado")
            {
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "El formulario fue actualizado" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se logro actualizar" };
        }
        /// <summary>
        /// Obtiene el número de MPR_CONT que se da al crear el CDP en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>
        private static int GetValueResponseXmlSeven(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
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

        /// <summary>
        /// Obtiene el número de FAC_CONT que se da al crear Factura  en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>CDP
        private static int GetValueResponseXmlSevenFactura(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new();
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
                default:
                    break;
            }

            return Valor;
        }
        /// <summary>
        /// integración Reintegros con Seven creación del CDP
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<CrearCDPResponse>> CrearCDPSeven(ParametroObtenerReintegroDto reintegro, CancellationToken cancellationToken)
        {
            Response<CrearCDPResponse> response = new();
            int idCliente = reintegro.IdCliente != null ? reintegro.IdCliente.Value : 0;
            var cliente = (await _iClienteRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
            AfiliadosSeven datosReintegro = (await _reintegrosRepository.GetAfiliadosReintegro((Guid)reintegro.idReintegros)).FirstOrDefault();
            if (datosReintegro == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El reintegro no fué encontrado";
                return response;
            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre[] tSPgDmpres = ObtenerDeTalleCDP(cliente, datosReintegro).Result.vDetalle;

            var request = new CrearCDPDto
            {
                Emp_codi = 294,
                Top_codi = 3015,
                Mpr_nume = 0,
                Mpr_fech = DateTime.Now,
                Mpr_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - {datosReintegro.Unidad_Ejecutora} ",
                Arb_csuc = "1001",
                Ter_coda = cliente.Identificacion,
                Mpr_tdos = "COMPROBANTE",
                Mpr_ndos = 0,
                Uni_codi = 1
            };

            var _data = new TSPgMpres
            {
                Emp_codi = request.Emp_codi,
                Top_codi = request.Top_codi,
                Mpr_nume = request.Mpr_nume,
                Mpr_fech = request.Mpr_fech,
                Mpr_desc = request.Mpr_desc,
                Arb_csuc = request.Arb_csuc,
                Ter_coda = request.Ter_coda,
                Mpr_tdos = request.Mpr_tdos,
                Mpr_ndos = request.Mpr_ndos,
                Uni_codi = request.Uni_codi,
                vDetalle = tSPgDmpres
            };

            var cdprequest = new CrearCDPRequest(_data);

            response.Data = await _wsCDP.CrearCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.CrearCDPResult : "Error inesperado creando el CDP en Seven.";

            if (response.Data.CrearCDPResult.Contains("MPR_CONT"))
            {
                int MPR_CONT = GetValueResponseXmlSeven(response.Data.CrearCDPResult, 0);
                await this._reintegrosRepository.ActualizaMPRCONTReintegro(reintegro.idReintegros, MPR_CONT);
            }

            return response;
        }
        /// <summary>
        /// Construye el detalle del CDP.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        private async Task<TSPgMpres> ObtenerDeTalleCDP(ClienteDto cliente, AfiliadosSeven reintegro)
        {
            TSPgMpres detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre>();
            double valorRetiro = (double)reintegro.ValorSueldo;
            double saldoRetirado = 0;
            double saldoPendiente = (double)reintegro.ValorSueldo;

            Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClienteRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
            int IdCuentaActiva = cuentasCliente.Data
            .Where(x => x.SaldoDisponible > 0)
            .OrderByDescending(x => x.SaldoDisponible)
            .Select(x => x.CuentaId)
            .FirstOrDefault();
            int tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
            Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClienteRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _cesantiasRepository.ObtenerOrdenDescuentoSaldos());
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
                            Arb_codc = "107706",
                            Arb_coda = "3011",
                            Arb_codp = "099",
                            Dmp_valo = saldoRetirado,
                            Dmp_desc = $"{item.preProducto}"
                        };

                        items.Add(itemDetalle);
                    }
                }
            }

            detalle.vDetalle = items.ToArray();

            return detalle;
        }
        /// <summary>
        /// Paso 1 crear tercero Seven - Integración Solicitud Reintegros.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarGnTerceResponse>> InsertTerceroSeven(ParametroObtenerReintegroDto reintegro, CancellationToken cancellationToken)
        {
            Response<InsertarGnTerceResponse> response = new();
            int idCliente = reintegro.IdCliente != null ? reintegro.IdCliente.Value : 0;
            var cliente = (await _iClienteRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

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

            return response;

        }
        /// <summary>
        /// Paso 4 creación de factura Proveedor en Seven basado en la info. de la solicitud el tercero y el cliente.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSeven(ParametroObtenerReintegroDto reintegro, CancellationToken cancellationToken)
        {
            Response<InsertarFacturaPResponse> response = new();
            int idCliente = reintegro.IdCliente != null ? reintegro.IdCliente.Value : 0;
            var cliente = (await _iClienteRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            var tercero = (await _reintegrosRepository.GetAfiliadosReintegro((Guid)reintegro.idReintegros)).FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago el reintegro.";
                return response;
            }

            var _dataFactuProv = new TSPoFactu
            {
                Emp_codi = 294,
                Top_codi = 4170,
                Fac_nume = 0,
                Fac_fech = DateTime.Now,
                Arb_cods = "1001",
                Fac_desc = $"{tercero.NumeroDocumento} - {tercero.Nombres} {tercero.Apellidos}  " +
                $" - {tercero.Numero_Radicado} - {tercero.NumerSolicitud}",
                Pvd_coda = tercero.NumeroDocumento.ToString(),
                Dep_codd = tercero.PVD_CODD != 0 ? tercero.PVD_CODD : 0,
                Fac_pref = tercero.NumerSolicitud.ToString(),
                Fac_nfap = tercero.Numero_Radicado,
                Mon_codi = 1,
                Fac_tipo = "C",
                Mpr_cont = tercero.MPR_CONT != 0 ? tercero.MPR_CONT : 0,
                Fac_feta = DateTime.Now
            };
            GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact[] detalleFactuProve = ObtenerDetalleFactuProveedor(cliente, tercero).Result.vDetalle;
            _dataFactuProv.vDetalle = detalleFactuProve;

            var body = new InsertarFacturaPRequestBody(_dataFactuProv);
            var facturarequest = new InsertarFacturaPRequest(body);

            response.Data = await _wsFactura.InsertarFacturaPAsync(facturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "Error inesperado al crear la factura en Seven.";
            if (response.Data != null)
            {
                int FAC_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 0);
                int FAC_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1);
                await _reintegrosRepository.ActualizaFACTReintegro(reintegro.idReintegros, FAC_CONT.ToString(), FAC_NUME.ToString());
            }

            return response;
        }


        private async Task<TSPoFactu> ObtenerDetalleFactuProveedor(ClienteDto cliente, AfiliadosSeven solicitud)
        {
            TSPoFactu detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact>();
            double valorRetiro = (double)solicitud.ValorSueldo;
            double saldoRetirado = 0;
            double saldoPendiente = (double)solicitud.ValorSueldo;

            Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClienteRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
            int IdCuentaActiva = cuentasCliente.Data
            .Where(x => x.SaldoDisponible > 0)
            .OrderByDescending(x => x.SaldoDisponible)
            .Select(x => x.CuentaId)
            .FirstOrDefault();
            int tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
            Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClienteRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _cesantiasRepository.ObtenerOrdenDescuentoSaldos());
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
                            Arb_codc = "107706",
                            Arb_coda = codAreanegocio.ToString(),
                            Arb_codp = "099"
                        };
                        items.Add(itemDetalle);
                    }
                }
            }

            detalle.vDetalle = items.ToArray();

            return detalle;
        }
        public async Task<Response<SyncPoPvdorResponse>> CrearProveedorSeven(ParametroObtenerReintegroDto reintegro, CancellationToken cancellationToken)
        {
            Response<SyncPoPvdorResponse> response = new();
            var tercero = (await _reintegrosRepository.GetAfiliadosReintegro((Guid)reintegro.idReintegros)).FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            var ProveeRequest = new SyncPoPvdorRequest()
            {
                emp_codi = 294,
                tip_codi = 13,
                pvd_coda = tercero.NumeroDocumento.ToString(),
                pvd_dive = 0,
                pvd_nomb = $"{tercero.Nombres}",
                pvd_apel = "",
                pvd_noco = tercero.Nombres,
                pai_codi = 169,
                dep_codi = Convert.ToInt16(11),
                mun_codi = 1,
                tpr_codi = 0,
                pvd_clad = 1 == 1 ? "J" : "N",
                arb_csuc = "1001",
                cal_codi = 0,
                coc_codi = 1,
                cim_codi = 98,
                act_codi = 0,
                mon_codi = 1,
                dep_dire = tercero.Direccion ?? "No Registra",
                cup_nume = tercero.NumeroCuenta.ToString() ?? "No Registra",
                cup_tipo = tercero.TipoCuenta == 1 ? "A" : "C",
                ban_codi = tercero?.CodigoBanco != 0 ? tercero.CodigoBanco : 1,
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

                await this._reintegrosRepository.ActualizarCodProvReintegros(reintegro.idReintegros, tercero.NumeroDocumento, codProvee);

            }

            return response;
        }

        public async Task<Response<PagosSevenDto>> PagosReintegrosSeven(ParametroObtenerReintegroDto solicitud, CancellationToken cancellationToken)
        {
            Response<PagosSevenDto> result = new() { Data = new PagosSevenDto() };

            Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroSeven(solicitud, cancellationToken);

            if (responseTerceroSeven.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTerceroSeven.ReturnMessage };
            }
            else
            {

                var jsonResponseTercero = ConvertXmlToJson(responseTerceroSeven.Data.InsertarGnTerceResult);
                result.Data.insertarGnTerceResponse = jsonResponseTercero;

            }

            Response<CrearCDPResponse> crearCDPResponse = await CrearCDPSeven(solicitud, cancellationToken);

            if (crearCDPResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = crearCDPResponse.ReturnMessage };
            }
            else
            {

                var jsonResponseCDP = ConvertXmlToJson(crearCDPResponse.Data.CrearCDPResult);
                result.Data.crearCDPResponse = jsonResponseCDP;


            }

            Response<SyncPoPvdorResponse> syncPoPvdorResponse = await CrearProveedorSeven(solicitud, cancellationToken);

            if (syncPoPvdorResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = syncPoPvdorResponse.ReturnMessage };
            }
            else
            {
                var jsonResponseProveedor = ConvertXmlToJson(syncPoPvdorResponse.Data.SyncPoPvdorResult);
                result.Data.syncPoPvdorResponse = jsonResponseProveedor;

            }

            Response<InsertarFacturaPResponse> insertarFacturaPResponse = await CrearFacturaProveSeven(solicitud, cancellationToken);

            if (insertarFacturaPResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = insertarFacturaPResponse.ReturnMessage };
            }
            else
            {

                var jsonResponseFactura = ConvertXmlToJson(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult);
                result.Data.insertarFacturaPResponse = jsonResponseFactura;
            }

            return result;


        }
        public async Task<Response<bool>> GenerarPagoData(DataGenerarPagoDto data)
        {
            var resultData = await _reintegrosRepository.UpdateDataGenerarPago(_mapper.Map<DataGenerarPago>(data));
            if (resultData > 0)
            {
                return new Response<bool> { Data = true, IsSuccess = false, ReturnMessage = "Exitosamente,actualizada la informacion" };
            }
            return new Response<bool> { Data = false, IsSuccess = true, ReturnMessage = "No se actualizo la informacion" };

        }
        private static string ConvertXmlToJson(string xmlString)
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlString);

            string jsonResul = JsonConvert.SerializeXmlNode(doc);

            return jsonResul;
        }

        public async Task<Response<InsertarMovContableResponse>> MovContableReintegroSeven(ParametrosMovContableReintegroDto reintegro, CancellationToken cancellationToken)

        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new();
            var dataMovContaCesantias = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaCesantias = new();
            var dataMovContaAportes = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaAportes = new();
            var InfoFinancieraCliente = (await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(reintegro.IdTipoIdentificacion, reintegro.Identificacion)).Data;
            AfiliadosSeven responseSolicitud = (await _reintegrosRepository.GetAfiliadosReintegro(reintegro.IdReintegro)).FirstOrDefault();
            var cabeceraCesantias = 0;
            var cabeceraAportes = 0;

            var conceptos = await _cuentasClientesBusinessLogic.ObtenerCuentasConceptosPorTipoNumeroIdentificacion(reintegro.IdTipoIdentificacion, reintegro.Identificacion);
            if (conceptos.Data.Cuentas.Any())
            {
                var cuentaConceptos = conceptos.Data.Cuentas.Where(x => (x.NombreCuenta == "CUENTA INDIVIDUAL SOLUCION VIVIENDA") || (x.NombreCuenta == "CUENTA INDIVIDUAL ADMINISTRACION CESANTIAS")).FirstOrDefault();
                foreach (var item in cuentaConceptos.Conceptos)
                {
                    if (item.ConceptoDescripcion.Contains("CESANTÍAS"))
                    {
                        var DevolucionSaldosCesantias = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(4169);
                        if (cabeceraCesantias == 0)
                        {
                            dataMovContaCesantias.Emp_codi = 294;
                            dataMovContaCesantias.Top_codi = 4169;
                            dataMovContaCesantias.Mco_nume = 0;
                            dataMovContaCesantias.Mco_fech = DateTime.Now;
                            dataMovContaCesantias.Mod_codi = 7;
                            dataMovContaCesantias.Arb_csuc = "1001";
                            dataMovContaCesantias.Mco_desc = "Mov Contable Devolucion saldos cesantías";

                        }
                        var items = ObtenerDeTalleInteresesCesantiasSeven(DevolucionSaldosCesantias, InfoFinancieraCliente, item).Result;
                        detalleMovContaCesantias.AddRange(items);
                        cabeceraCesantias++;
                    }
                    else
                    {
                        var DevolucionSaldosAportes = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(4160);

                        if (cabeceraAportes == 0)
                        {
                            dataMovContaCesantias.Emp_codi = 294;
                            dataMovContaCesantias.Top_codi = 4160;
                            dataMovContaCesantias.Mco_nume = 0;
                            dataMovContaCesantias.Mco_fech = DateTime.Now;
                            dataMovContaCesantias.Mod_codi = 7;
                            dataMovContaCesantias.Arb_csuc = "1001";
                            dataMovContaCesantias.Mco_desc = "Mov Contable Devolucion saldos aportes";

                        }
                        var items = ObtenerDeTalleInteresesCesantiasSeven(DevolucionSaldosAportes, InfoFinancieraCliente, item).Result;
                        detalleMovContaCesantias.AddRange(items);
                        cabeceraAportes++;
                    }
                }
            }

            if (detalleMovContaCesantias.Count > 0)
            {
                dataMovContaCesantias.vDetalle = detalleMovContaCesantias.ToArray();

                var bodyCesantias = new InsertarMovContableRequestBody(dataMovContaCesantias);
                movContableRequest = new InsertarMovContableRequest(bodyCesantias);
                response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                var mensaje = "\n Resultado Mov. Contable Seven intereses Cesantias: ";
                response.ReturnMessage += response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven ";

            }
            if (detalleMovContaAportes.Count > 0)
            {
                dataMovContaAportes.vDetalle = detalleMovContaAportes.ToArray();

                var bodyAportes = new InsertarMovContableRequestBody(dataMovContaAportes);
                movContableRequest = new InsertarMovContableRequest(bodyAportes);
                response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                var mensaje = "\n Resultado Mov. Contable Seven intereses Aportes: ";
                response.ReturnMessage += response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven ";


            }


            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleInteresesCesantiasSeven(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, ClienteDto InfoFinancieraCliente, ConceptoCuentaDto Conceptos)
        {
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == InfoFinancieraCliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            foreach (var item in cuentasContables)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon
                {
                    Cue_codi = item.CUENTA_CONTABLE_SEVEN,
                    Dmc_desc = $"cliente: " + InfoFinancieraCliente.Identificacion,
                    Dmc_cant = 1,
                    Dmc_acti = 0,
                    Dmc_refe = "00000000",
                    Dmc_vadb = item.TIPO_OPERACION == "D" ? (decimal)Conceptos.saldo : 0,
                    Dmc_vacr = item.TIPO_OPERACION == "C" ? (decimal)Conceptos.saldo : 0,
                    Dmc_vaba = 0,
                    Ter_coda = InfoFinancieraCliente.Identificacion,
                    Arb_codc = item.TCR_ID == 1 ? "601102" : "601103",
                    Arb_coda = codAreanegocio.ToString(),
                    Arb_codp = "099",
                    Arb_cods = "1001",
                    Ter_codm = "0"
                };

                items.Add(itemDetalle);
            }
            return items;
        }
        public async Task<byte[]> ReporteOficioReintegro(string radicado)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "Templates", "HTML", "Oficios");

            //var html = new StringBuilder(await File.ReadAllTextAsync(Path.Join(path, "main-content.html")));
            var html = await File.ReadAllTextAsync(Path.Join(path, "OficioReintegro.html"));

            var cultureEs = CultureInfo.GetCultureInfo("es-ES");
            var dataDodo = await ObtenerCorrespondenciaReintegros(radicado);
            var resultData = await _reintegrosRepository.ObtnerAfiliadoReintegro(radicado);
            if (dataDodo.Data.radicadoRelacionado == null
                && dataDodo.Data.identificacion == null
                && dataDodo.Data.ciudad == null
                && dataDodo.Data.contacto == null
                && dataDodo.Data.cargo == null
                && dataDodo.Data.correoElectronico == null)
            {
                throw new Exception("No existe datos en dodo con ese radicado");
            }
            var dataLider = await _reintegrosRepository.ObtnerUserLider();

            html = html.Replace("PARAM_Nombres_Apellidos", dataDodo.Data.contacto);
            html = html.Replace("PARAM_Cargo", dataDodo.Data.cargo);
            html = html.Replace("PARAM_Email", dataDodo.Data.correoElectronico);
            html = html.Replace("PARAM_Ciudad", dataDodo.Data.ciudad);
            html = html.Replace("Radicado UEJ", dataDodo.Data.radicadoRelacionado);
            html = html.Replace("PARAM_Fecha", resultData.Select(x => x.FechaCreacion.ToString()).First());
            html = html.Replace("[Fecha y Hora]", resultData.Select(x => x.FechaCreacion.ToString()).First());
            html = html.Replace("[Fecha de Radicación]", resultData.Select(x => x.FechaCreacion.ToString()).First());
            html = html.Replace("[Numero de Radicado de Salida]", resultData.Select(x => x.RadicadoRelacionado).FirstOrDefault());
            html = html.Replace("(Número de Radicado de salida)", resultData.Select(x => x.RadicadoRelacionado).FirstOrDefault());
            html = html.Replace("[Numero de Radicado de entrada]", radicado);
            html = html.Replace("[UEJ_SIGLA]", resultData.Select(x => x.UnidadEjecutora).FirstOrDefault());
            html = html.Replace("PARAM_Area", resultData.Select(x => x.UnidadEjecutora).FirstOrDefault());
            html = html.Replace("[Usuario]", resultData.Select(x => x.UserName).FirstOrDefault());
            html = html.Replace("[ADMI_LIDER]", dataLider.NamePerfil);

            var datos = resultData.ToList();

            // Genera las filas de datos dinámicamente
            StringBuilder filasHtml = new();
            for (int i = 0; i < datos.Count; i++)
            {
                var valorFormateado = decimal.Parse(datos[i].ValorSueldo);
                var valor = valorFormateado.ToString("C");
                string filaReemplazada = $@"
        <tr>
            <td style=""border: 1px solid #000; padding: 8px;"">{datos[i].Nombre}</td>
            <td style=""border: 1px solid #000; padding: 8px;"">{datos[i].Identificacion}</td>
            <td style=""border: 1px solid #000; padding: 8px;"">{datos[i].TipoSolicitud}</td>
            <td style=""border: 1px solid #000; padding: 8px;"">{valor}</td>
        </tr>";

                filasHtml.AppendLine(filaReemplazada);
            }

            // Reemplaza solo la fila de datos original con las filas generadas dinámicamente
            html = html.Replace(@"<tr>
                        <td style=""border: 1px solid #000; padding: 8px;"">[Nombres y apellidos del Afiliado # 1]</td>
                        <td style=""border: 1px solid #000; padding: 8px;"">[Cedula # 1]</td>
                        <td style=""border: 1px solid #000; padding: 8px;"">[Tipo de Reintegro afiliado # 1]</td>
                        <td style=""border: 1px solid #000; padding: 8px;"">[Valor reintegrado afiliado # 1]</td>
                    </tr>", filasHtml.ToString());

            var datosObservacio = datos.Where(x => x.Observacion != null && x.Observacion != "").ToList();
            if (datosObservacio.Any())
            {
                StringBuilder filasHtmlObservacion = new();
                for (int i = 0; i < datosObservacio.Count; i++)
                {
                    string filaReemplazadaObservation = $@"
            <tr>
                <td style=""border: 1px solid #000; padding: 8px;"">{datosObservacio[i].Observacion} -  {datosObservacio[i].Identificacion}</td>
            </tr>";
                    filasHtmlObservacion.AppendLine(filaReemplazadaObservation);
                }

                // Reemplaza la fila de observaciones original con las filas generadas dinámicamente
                html = html.Replace(@"<td style=""border: 1px solid #000; padding: 8px;"">
                                    [Observaciones # 1]
                                </td>", filasHtmlObservacion.ToString());
            }
            else
            {
                html = html.Replace("ObservationP", " style=\"display:none;\"");

            }

            var htmlToPdf = new HtmlToPdf();
            var doc = htmlToPdf.ConvertHtmlString(html.ToString());
            return doc.Save();
        }

        public async Task<Response<FileResult>> GenerarReporteOficio(string radicado)
        {
            byte[] reportBytes = await ReporteOficioReintegro(radicado);
            var filename = $"Reporte Oficio {radicado}.pdf";
            var file = await GenerarArchivoPDF(reportBytes, filename);

            return file.BuildSuccess();
        }

        private static async Task<FileResult> GenerarArchivoPDF(byte[] byteArray, string filename, bool requiredPass = false, string password = "")
        {
            if (requiredPass)
            {
                using var input = new MemoryStream(byteArray);
                using var output = new MemoryStream();
                var reader = new PdfReader(input);
                PdfEncryptor.Encrypt(reader, output, true, password, password, PdfWriter.ALLOW_SCREENREADERS);
                byteArray = output.ToArray();
            }

            var msPdf = new MemoryStream(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = filename
            };

            await archivoPdf.FileStream.FlushAsync();
            return archivoPdf;
        }

        public async Task<bool> GetListSolicitud(string afiliado)
        {
            var typeSolicitud = await _reintegrosRepository.ObtenerListadoSolicituByAfiliado(afiliado);
            if (typeSolicitud.Count() > 1)
            {
                var type = typeSolicitud.ToList();
                if (type[1] == 2)
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<FileResult> ReportePlanillaPagoReintegro(GenerarPanillaPagoReintegro data, string userid)
        {
            var report = new Microsoft.Reporting.NETCore.LocalReport();
            try
            {
                ObtenerSolicitud obtenerSolicitud = new();
                //var response2 = await _clientsRepository.ObtenerCuentasMovimientosAfiliado(data.identificacion);                //var cuentas = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(Convert.ToInt32(data.identificacion)));


                string reportPath = $"{_webHostEnvironment.ContentRootPath}\\Reportes\\ReportPlanillaPagosReintegros.rdlc";
                report.ReportPath = reportPath;
                var dataPlanillaReintegro = await _reintegrosRepository.ObtnerDataPlanillaPagoReintegro(data.Identificacion, data.TipoIdentificacion);
                var dataUser = await _usersRepository.ObtenerUsuarioPorId(Guid.Parse(userid));
                var clienteDetallado = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(data.TipoIdentificacion, data.Identificacion);
                report =  ParametrosReportePlanillaPagoIndiCesantias(report, dataPlanillaReintegro.FirstOrDefault(), clienteDetallado.Data, data.Cdp, dataUser.USR_NOMBRE + " " + dataUser.USR_SEGUNDONOMBRE + " " + dataUser.USR_APELLIDO + " ");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            byte[] bytes;
            try
            {
                bytes = report.Render("PDF");
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }
            return await GenerarReporte(bytes, false);
        }
        public Microsoft.Reporting.NETCore.LocalReport ParametrosReportePlanillaPagoIndiCesantias(Microsoft.Reporting.NETCore.LocalReport reportParam, DataPanillaPagoReintegro data, ClienteDto cliente, string cdp, string nameUser)
        {
            var s = cliente.Cuentas.SelectMany(x => x.Conceptos);
            var sumaDatos = s.Where(x => x.ValorDevolver > 0);
            var sumaConcepto = sumaDatos.Sum(x => x.ValorDevolver);

            var reportParameters = ParametrosStaticosPlanillaPagos(data, cliente, sumaConcepto, cdp, nameUser).Result.Select(param => new Microsoft.Reporting.NETCore.ReportParameter(param.Key, param.Value)).ToList();

            var cuentas = cliente.Cuentas.SelectMany(x => x.Conceptos).ToList();
            var conceptosData = cuentas
                .Where(x => x.ValorDevolver > 0) // Filtra los conceptos donde ValorDevolver sea mayor que 0
                .Select(x => new
                {
                    NombreConcepto = x.ConceptoDescripcion,
                    ValorConcepto = x.ValorDevolver.ToString("C")
                })
                .ToList();


            var conceptos = new Microsoft.Reporting.NETCore.ReportDataSource("Conceptos", conceptosData)
            {
                Value = conceptosData
            };
            reportParam.DataSources.Add(conceptos);
            reportParam.SetParameters(reportParameters);
            return reportParam;
        }


        public async Task<Dictionary<string, string>> ParametrosStaticosPlanillaPagos(DataPanillaPagoReintegro data, ClienteDto cliente, decimal sumaConcepto, string cdp, string nameUser)
        {
            var fuerza = await _solicitudRepository.ObtenerFuerzaPorId(cliente.IdFuerza);
            var UnidadEjecutora = await _solicitudRepository.ObtenerFuerzaPorId(cliente.IdFuerza);
            var apellidoNombre = data.ApellidoNombre.Split(' ');
            var categoria = _mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias()).Where(x => x.Id == cliente.IdCategoria).FirstOrDefault();
            return new Dictionary<string, string>
                {
                    { "FecAprobacion", DateTime.Now.ToString() },
                    { "FecPlanilla", DateTime.Now.ToString()},
                    { "NroPlanilla", data.NumeroPlanilla},
                    { "TipoSolicitud", "REINTEGROS UNIDADES EJECUTORAS"},
                    { "Radicado", data.Radicado},
                    { "PrimerNombreApellido", data.ApellidoNombre},
                    { "UnidadEjecutora", data.UnidadEjecutora},
                    { "Fuerza", fuerza.FirstOrDefault().FRZ_DESCRIPCION},
                    { "FecImpresion", DateTime.Now.ToString()},
                    { "FecElaboracion", DateTime.Now.ToString()},
                    { "Regimen",cliente.Regimen.ToString()},
                    { "VlrCesantias", sumaConcepto.ToString("C") },
                    { "NombreAfiliado",data.ApellidoNombre},
                    { "NroIdentificacion",data.Identificacion.ToString()},
                    { "Categoria",categoria.Descripcion},
                    { "Cuotas",cliente.NumeroCuotas.ToString()},
                    { "CDP",cdp},
                    { "Comprobante",data.Cdp},//cambiar por comprobante solo el nombre de la variable
                    { "Banco", data.Banco},
                    { "NombreUnidadEjecutora",data.UnidadEjecutora},
                    { "NitUej",data.NIT },
                    { "Telefono",""},
                    { "Cuenta",data.NumeroCuenta +" " + data.TipoCuenta},
                    { "OrdePago",""},
                    { "Anotacion",data.UnidadEjecutora},
                    { "Direccion",""},
                    { "Ciudad","BOGOTÁ"},
                { "Departamento","BOGOTÁ D.C"},
                { "Valor",sumaConcepto.ToString("C")},
                { "User",nameUser},
            };
        }
        private async Task<FileResult> GenerarReporte(byte[] byteArray, bool requiredPass, string identificacion = "")
        {

            // Asigno valor secreto de accesibilidad al pdf si el parametro es true
            if (requiredPass)
            {
                using MemoryStream input = new(byteArray);
                using MemoryStream output = new();
                string credential = identificacion;
                PdfReader reader = new(input);
                PdfEncryptor.Encrypt(reader, output, true, credential, credential, PdfWriter.ALLOW_SCREENREADERS);
                byteArray = output.ToArray();
            }

            // Asigno nombre aleatorio al archivo 
            string hash = Guid.NewGuid().ToString();
            string namePDF = hash + ".pdf";

            // Valido que si el container no existe se crea
            BlobContainerClient container = new(_options.Value.CadenaOne, "reportes-sfc");
            await container.CreateIfNotExistsAsync();

            // Conexion al storage
            var cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.CadenaOne);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("reportes-sfc");
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(namePDF);
            cloudBlockBlob.Properties.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;

            // Subo el archivo al container
            using (Stream stream = new MemoryStream(byteArray))
            {
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }

            var fileName = cloudBlockBlob.Uri.ToString().Split("/");

            // Se asigna el nombre con el que quedo guardado el archivo en el storage
            MemoryStream msPdf = new(byteArray);
            var archivoPdf = new FileStreamResult(msPdf, "application/pdf")
            {
                FileDownloadName = fileName[4]
            };

            await archivoPdf.FileStream.FlushAsync();

            return archivoPdf;

        }
    }
}