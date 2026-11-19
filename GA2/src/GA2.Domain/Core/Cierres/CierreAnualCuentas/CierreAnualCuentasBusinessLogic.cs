using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class CierreAnualCuentasBusinessLogic : ICierreAnualCuentasBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ICierreAnualCuentasRepository _cierreAnualRepository;

        public CierreAnualCuentasBusinessLogic(IMapper mapper, ICierreAnualCuentasRepository cierreAnualRepository)
        {
            _mapper = mapper;
            _cierreAnualRepository = cierreAnualRepository;
        }

        private readonly ICierreAnualCuentasRequestProvider _iCierreAnualRequestProvider;

        public CierreAnualCuentasBusinessLogic(
             IMapper mapper,
             ICierreAnualCuentasRequestProvider iCierreAnualRequestProvider,
             ICierreAnualCuentasRepository iCierreAnualRepository
             )
        {
            _mapper = mapper;
            _iCierreAnualRequestProvider = iCierreAnualRequestProvider;
            _cierreAnualRepository = iCierreAnualRepository;
        }

        /// <summary>
        /// ValidarExisteCierre
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<bool>> ValidarExisteCierre(CierreAnualDto request)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = false;
            respuesta.IsSuccess = false;
            Task<bool> existeCierre = ValidarExisteCierreAsync(request.Anio, request.Mes);

            if (existeCierre.Result == true)
            {
                respuesta.Data = false;
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Ya existe un cierre programado para el periodo " + request.Anio + "-" + request.Mes;
                return respuesta;
            }

            return respuesta;
        }




        /// <summary>
        /// ProgramarCierreAnual
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ProgramacionCierreAnualCuentasDto>> ProgramarCierreAnualCuentas(ParametrosCierreAnualDto request)
        {

            CierreAnualDto cierre = new CierreAnualDto();

            Response<IPCDto> response = new Response<IPCDto>
            {
                Data = _mapper.Map<IPCDto>(await _cierreAnualRepository.ObtenerIPCMes())
            };

            cierre.IPC = response.Data.ValorIPC;
            cierre.Anio = response.Data.Anio;
            cierre.Mes = response.Data.Mes;
            cierre.GeneraActualizacion = request.GeneraActualizacion;
            cierre.TipoProceso = request.TipoProceso;

            // 1. AHORROS
            if (request.TipoProceso == 1)
            {
                List<int> listConceptosAhorro = await _cierreAnualRepository.ObtenerCategoriaConceptos("A");
                cierre.ListConceptosAhorros = listConceptosAhorro;

                int cuentaAbonoAhorro = await _cierreAnualRepository.ObtenerCuentaAbono("A");
                cierre.CuentaAbonoAhorros = cuentaAbonoAhorro;
            }

            // 2. CESANTIAS
            if (request.TipoProceso == 2)
            {
                List<int> listConceptosCesantias = await _cierreAnualRepository.ObtenerCategoriaConceptos("C");
                cierre.ListConceptosCesantias = listConceptosCesantias;

                int cuentaAbonoCesantias = await _cierreAnualRepository.ObtenerCuentaAbono("C");
                cierre.CuentaAbonoCesantias = cuentaAbonoCesantias;
            }

            // 3. AMBOS
            if (request.TipoProceso == 3)
            {
                List<int> listConceptosAhorro = await _cierreAnualRepository.ObtenerCategoriaConceptos("A");
                cierre.ListConceptosAhorros = listConceptosAhorro;

                int cuentaAbonoAhorro = await _cierreAnualRepository.ObtenerCuentaAbono("A");
                cierre.CuentaAbonoAhorros = cuentaAbonoAhorro;

                List<int> listConceptosCesantias = await _cierreAnualRepository.ObtenerCategoriaConceptos("C");
                cierre.ListConceptosCesantias = listConceptosCesantias;

                int cuentaAbonoCesantias = await _cierreAnualRepository.ObtenerCuentaAbono("C");
                cierre.CuentaAbonoCesantias = cuentaAbonoCesantias;
            }

            Task<Response<bool>> responseValidacion = ValidarExisteCierre(cierre);

            if (responseValidacion.Result.IsSuccess == true)
            {
                ProgramacionCierreAnualCuentasDto result = _mapper.Map<ProgramacionCierreAnualCuentasDto>(await _cierreAnualRepository.
                    ProgramarCierreAnual(_mapper.Map<ParametrosCierreAnualCuentas>(request), _mapper.Map<CierreAnualCuentas>(cierre)));

                Response<RespuestaCierreAnualCuentasDto> respuestaCierreAnualDto = _mapper.Map<Response<RespuestaCierreAnualCuentasDto>>(await _iCierreAnualRequestProvider.
                    CierreAnualCuentas(_mapper.Map<ProgramacionCierreAnualCuentas>(result)));

                ProgramacionCierreAnualCuentasDto res = _mapper.Map<ProgramacionCierreAnualCuentasDto>(await _cierreAnualRepository.
                    ActualizaEstadoCierreAnualCuentas(respuestaCierreAnualDto.Data.Id, respuestaCierreAnualDto.Data.Estado));

                ProgramacionCierreAnualCuentasDto resultProgramacion = _mapper.Map<ProgramacionCierreAnualCuentasDto>(await _cierreAnualRepository.ActualizaEstadoCierreAnualCuentas(respuestaCierreAnualDto.Data.Id, respuestaCierreAnualDto.Data.Estado));

                return new Response<ProgramacionCierreAnualCuentasDto> { Data = resultProgramacion };
            }
            else
            {
                return new Response<ProgramacionCierreAnualCuentasDto> { IsSuccess = false, ReturnMessage = responseValidacion.Result.ReturnMessage };
            }

        }

        /// <summary>
        /// ObtenerIPCMes
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IPCDto>> ObtenerIPCMes()
        {
            return new Response<IPCDto>
            {
                Data = _mapper.Map<IPCDto>(await _cierreAnualRepository.ObtenerIPCMes())
            };
        }

        /// <summary>
        /// ActualizaEstadoCierreAnual
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Response<ProgramacionCierreAnualCuentasDto>> ActualizaEstadoCierreAnualCuentas(ActualizaCierreAnualCuentasDto data)
        {

            ProgramacionCierreAnualCuentasDto result = _mapper.Map<ProgramacionCierreAnualCuentasDto>(await _cierreAnualRepository.ActualizaEstadoCierreAnualCuentas(data.Id, data.Estado));
            return new Response<ProgramacionCierreAnualCuentasDto> { Data = result };

        }

        /// <summary>
        /// ActualizarGA2Async
        /// </summary>
        /// <param name="respuestaprogramacionCierresDto"></param>
        /// <returns></returns>
        public async Task ActualizarGA2Async(Response<IEnumerable<ProgramacionCierreAnualCuentasDto>> respuestaprogramacionCierresDto)
        {
            foreach (ProgramacionCierreAnualCuentasDto datos in respuestaprogramacionCierresDto.Data)
            {
                ProgramacionCierreAnualCuentasDto res = _mapper.Map<ProgramacionCierreAnualCuentasDto>(await _cierreAnualRepository.
                    ActualizaEstadoCierreAnualCuentas(datos.Id, datos.Estado));
            }
        }

        /// <summary>
        /// ConsultarProgramacionCierreAnualGA2
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualGA2()
        {
            List<ProgramacionCierreAnualCuentasDto> list = new List<ProgramacionCierreAnualCuentasDto>();

            IEnumerable<ProgramacionCierreAnualCuentasDto> programacionCierresDto = _mapper.Map<IEnumerable<ProgramacionCierreAnualCuentasDto>>(await _cierreAnualRepository.
                ConsultarProgramacionCierreAnualCuentas());

            foreach (var programacion in programacionCierresDto)
            {
                list.Add(programacion);
            }

            return new Response<IEnumerable<ProgramacionCierreAnualCuentasDto>> { Data = list };
        }

        /// <summary>
        /// ConsultarProgramacionCierreAnual
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentas()
        {

            Response<IEnumerable<ProgramacionCierreAnualCuentasDto>> respuestaprogramacionCierresDto = _mapper.Map<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>>(await this._iCierreAnualRequestProvider.
                ConsultarProgramacionCierreAnualCuentas());

            await ActualizarGA2Async(respuestaprogramacionCierresDto);

            Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> response = ConsultarProgramacionCierreAnualGA2();

            return await response;

        }

        /// <summary>
        /// ConsultarProgramacionCierreAnualBUA
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentasBUA()
        {
            Response<IEnumerable<ProgramacionCierreAnualCuentasDto>> respuestaprogramacionCierresDto = _mapper.Map<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>>(await this._iCierreAnualRequestProvider.
                ConsultarProgramacionCierreAnualCuentas());

            return respuestaprogramacionCierresDto;
        }

        /// <summary>
        /// ValidarExisteCierreAsync
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        private async Task<bool> ValidarExisteCierreAsync(int anio, int mes)
        {
            bool encontroProgramacion = await _cierreAnualRepository.ValidarExisteCierreMensual(anio, mes);
            return encontroProgramacion;
        }

    }
}
