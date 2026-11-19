using AutoMapper;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class CierreMensualCuentasBusinessLogic : ICierreMensualCuentasBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ICierreMensualRepository _cierreMensualRepository;

        public CierreMensualCuentasBusinessLogic(IMapper mapper, ICierreMensualRepository cierreMensualRepository)
        {
            _mapper = mapper;
            _cierreMensualRepository = cierreMensualRepository;
        }

        private readonly ICierreMensualRequestProvider _iCierreMensualRequestProvider;

        public CierreMensualCuentasBusinessLogic(
             IMapper mapper,
             ICierreMensualRequestProvider iCierreMensualRequestProvider,
             ICierreMensualRepository iCierreMensualRepository
             )
        {
            _mapper = mapper;
            _iCierreMensualRequestProvider = iCierreMensualRequestProvider;
            _cierreMensualRepository = iCierreMensualRepository;
        }

        public async Task<Response<bool>> ValidarExisteCierre(CierreMensualCuentasDto request)
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
        /// ProgramarCierreMensual
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ProgramacionCierreMensualCuentasDto>> ProgramarCierreMensual(ParametrosCierreMensualCuentasDto request)
        {

            CierreMensualCuentasDto cierre = new CierreMensualCuentasDto();

            Response<IPCDto> response = new Response<IPCDto>
            {
                Data = _mapper.Map<IPCDto>(await _cierreMensualRepository.ObtenerIPCMes())
            };

            cierre.IPC = response.Data.ValorIPC;
            cierre.Anio = response.Data.Anio;
            cierre.Mes = response.Data.Mes;
            cierre.GeneraActualizacion = request.GeneraActualizacion;
            cierre.TipoProceso = request.TipoProceso;

            // 1. AHORROS
            if (request.TipoProceso == 1)
            {
                List<int> listConceptosAhorro = await _cierreMensualRepository.ObtenerCategoriaConceptos("A");
                cierre.ListConceptosAhorros = listConceptosAhorro;

                int cuentaAbonoAhorro = await _cierreMensualRepository.ObtenerCuentaAbono("A");
                cierre.CuentaAbonoAhorros = cuentaAbonoAhorro;
            }

            // 2. CESANTIAS
            if (request.TipoProceso == 2)
            {
                List<int> listConceptosCesantias = await _cierreMensualRepository.ObtenerCategoriaConceptos("C");
                cierre.ListConceptosCesantias = listConceptosCesantias;

                int cuentaAbonoCesantias = await _cierreMensualRepository.ObtenerCuentaAbono("C");
                cierre.CuentaAbonoCesantias = cuentaAbonoCesantias;
            }

            // 3. AMBOS
            if (request.TipoProceso == 3)
            {
                List<int> listConceptosAhorro = await _cierreMensualRepository.ObtenerCategoriaConceptos("A");
                cierre.ListConceptosAhorros = listConceptosAhorro;

                int cuentaAbonoAhorro = await _cierreMensualRepository.ObtenerCuentaAbono("A");
                cierre.CuentaAbonoAhorros = cuentaAbonoAhorro;

                List<int> listConceptosCesantias = await _cierreMensualRepository.ObtenerCategoriaConceptos("C");
                cierre.ListConceptosCesantias = listConceptosCesantias;

                int cuentaAbonoCesantias = await _cierreMensualRepository.ObtenerCuentaAbono("C");
                cierre.CuentaAbonoCesantias = cuentaAbonoCesantias;
            }

            Task<Response<bool>> responseValidacion = ValidarExisteCierre(cierre);

            if (responseValidacion.Result.IsSuccess == true)
            {
                ProgramacionCierreMensualCuentasDto result = _mapper.Map<ProgramacionCierreMensualCuentasDto>(await _cierreMensualRepository.
                    ProgramarCierreMensualCuentas(_mapper.Map<ParametrosCierreMensual>(request), _mapper.Map<CierreMensualCuentas>(cierre)));

                Response<RespuestaCierreMensualCuentasDto> respuestaCierreMensualDto = _mapper.Map<Response<RespuestaCierreMensualCuentasDto>>(await _iCierreMensualRequestProvider.
                    CierreMensualCuentas(_mapper.Map<ProgramacionCierreMensualCuentas>(result)));

                ProgramacionCierreMensualCuentasDto res = _mapper.Map<ProgramacionCierreMensualCuentasDto>(await _cierreMensualRepository.
                    ActualizaEstadoCierreMensualCuentas(respuestaCierreMensualDto.Data.Id, respuestaCierreMensualDto.Data.Estado));

                ProgramacionCierreMensualCuentasDto resultProgramacion = _mapper.Map<ProgramacionCierreMensualCuentasDto>(await _cierreMensualRepository.ActualizaEstadoCierreMensualCuentas(respuestaCierreMensualDto.Data.Id, respuestaCierreMensualDto.Data.Estado));

                return new Response<ProgramacionCierreMensualCuentasDto> { Data = resultProgramacion };
            }
            else
            {
                return new Response<ProgramacionCierreMensualCuentasDto> { IsSuccess = false, ReturnMessage = responseValidacion.Result.ReturnMessage };
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
                Data = _mapper.Map<IPCDto>(await _cierreMensualRepository.ObtenerIPCMes())
            };
        }

        /// <summary>
        /// ActualizaEstadoCierreMensual
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Response<ProgramacionCierreMensualCuentasDto>> ActualizaEstadoCierreMensual(ActualizaCierreMensualCuentasDto data)
        {
            ProgramacionCierreMensualCuentasDto result = _mapper.Map<ProgramacionCierreMensualCuentasDto>(await _cierreMensualRepository.ActualizaEstadoCierreMensualCuentas(data.Id, data.Estado));
            return new Response<ProgramacionCierreMensualCuentasDto> { Data = result };
        }

        /// <summary>
        /// ActualizarGA2Async
        /// </summary>
        /// <param name="respuestaprogramacionCierresDto"></param>
        /// <returns></returns>
        public async Task ActualizarGA2Async(Response<IEnumerable<ProgramacionCierreMensualCuentasDto>> respuestaprogramacionCierresDto)
        {
            foreach (ProgramacionCierreMensualCuentasDto datos in respuestaprogramacionCierresDto.Data)
            {
                ProgramacionCierreMensualCuentasDto res = _mapper.Map<ProgramacionCierreMensualCuentasDto>(await _cierreMensualRepository.
                    ActualizaEstadoCierreMensualCuentas(datos.Id, datos.Estado));
            }
        }

        /// <summary>
        /// ConsultarProgramacionCierreMensualGA2
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensualGA2()
        {
            List<ProgramacionCierreMensualCuentasDto> list = new List<ProgramacionCierreMensualCuentasDto>();

            IEnumerable<ProgramacionCierreMensualCuentasDto> programacionCierresDto = _mapper.Map<IEnumerable<ProgramacionCierreMensualCuentasDto>>(await _cierreMensualRepository.
                ConsultarProgramacionCierreMensualCuentas());

            foreach (var programacion in programacionCierresDto)
            {
                list.Add(programacion);
            }

            return new Response<IEnumerable<ProgramacionCierreMensualCuentasDto>> { Data = list };
        }

        /// <summary>
        /// ConsultarProgramacionCierreMensual
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensual()
        {

            Response<IEnumerable<ProgramacionCierreMensualCuentasDto>> respuestaprogramacionCierresDto = _mapper.Map<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>>(await this._iCierreMensualRequestProvider.
                ConsultarProgramacionCierreMensualCuentas());

            await ActualizarGA2Async(respuestaprogramacionCierresDto);

            Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> response = ConsultarProgramacionCierreMensualGA2();

            return await response;

        }

        /// <summary>
        /// ConsultarProgramacionCierreMensualBUA
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensualBUA()
        {
            Response<IEnumerable<ProgramacionCierreMensualCuentasDto>> respuestaprogramacionCierresDto = _mapper.Map<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>>(await this._iCierreMensualRequestProvider.
                ConsultarProgramacionCierreMensualCuentas());

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
            bool encontroProgramacion = await _cierreMensualRepository.ValidarExisteCierre(anio, mes);
            return encontroProgramacion;
        }

        public async Task<Response<IEnumerable<ReporteCierreMensualCuentasDto>>> ReporteCierreMensual(Guid Id)
        {
            Response<IEnumerable<ReporteCierreMensualCuentasDto>> reporteCierreMensualDto = _mapper.Map<Response<IEnumerable<ReporteCierreMensualCuentasDto>>>(await this._iCierreMensualRequestProvider.
                ReporteCierreMensualCuentas(Id));

            return reporteCierreMensualDto;

        }

        public async Task<Response<RespuestaCierreMensualCuentasDto>> CierreMensualCuentas(ParametrosCierreMensualCuentasDto request)
        {
            ProgramacionCierreMensualCuentas programacion = new ProgramacionCierreMensualCuentas()
            {

            };

            Response<RespuestaCierreMensualCuentasDto> respuestaCierreMensualDto = _mapper.Map<Response<RespuestaCierreMensualCuentasDto>>(await this._iCierreMensualRequestProvider.
                CierreMensualCuentas(programacion));

            return respuestaCierreMensualDto;
        }
    }
}
