using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class CierreAnualCuentasBusinessLogicBua : ICierreAnualCuentasBusinessLogicBua
    {

        private readonly IMapper _mapper;
        private readonly ICierreAnualCuentasRepositoryBua _cierreAnualRepositoryBua;

        public CierreAnualCuentasBusinessLogicBua(
            ICierreAnualCuentasRepositoryBua cierreAnualRepositoryBua,
            IMapper mapper)
        {
            _cierreAnualRepositoryBua = cierreAnualRepositoryBua;
            _mapper = mapper;

        }

        /// <summary>
        /// Método para realizar el ciere anual
        /// </summary>
        /// <param name="request"> Objeto con los datos de operación </param>
        /// <returns></returns>
        public async Task<Response<RespuestaCierreAnualCuentasDto>> CierreAnualCuentas(ProgramacionCierreAnualCuentasDto request)
        {

            ProgramacionCierreAnualCuentas cierre = new ProgramacionCierreAnualCuentas();
            cierre.ID = request.Id;
            cierre.ANIO = request.Anio;
            cierre.MES = request.Mes;
            cierre.IPC = request.IPC;
            cierre.TIPO_PROCESO = request.TipoProceso;
            cierre.CONCEPTOS_AHORRO = request.ConceptosAhorro;
            cierre.CONCEPTOS_CESANTIAS = request.ConceptosCesantias;
            cierre.CUENTA_ABONO_AHORRO = request.CuentaAbonoAhorro;
            cierre.CUENTA_ABONO_CESANTIAS = request.CuentaAbonoCesantias;
            cierre.ESTADO = request.Estado;
            cierre.FECHA_CARGUE = request.FechaCargue;
            cierre.FECHA_PROCESAMIENTO = request.FechaProcesamiento;
            cierre.GENERA_ACTUALIZACION = request.GeneraActualizacion;
            cierre.USUARIO_PROCESAMIENTO = request.UsuarioProcesamiento;

            RespuestaCierreAnualCuentasDto cierreRespuesta = _mapper.Map<RespuestaCierreAnualCuentasDto>(await _cierreAnualRepositoryBua.
                CierreAnualCuentas(cierre));

            return new Response<RespuestaCierreAnualCuentasDto>
            {
                Data = cierreRespuesta,
            };

        }

        public async Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentas()
        {
            List<ProgramacionCierreAnualCuentasDto> list = new List<ProgramacionCierreAnualCuentasDto>();

            IEnumerable<ProgramacionCierreAnualCuentasDto> respuestaprogramacionCierresDto = _mapper.Map<IEnumerable<ProgramacionCierreAnualCuentasDto>>(await this._cierreAnualRepositoryBua.
                ConsultarProgramacionCierreAnualCuentas());

            foreach (var programacion in respuestaprogramacionCierresDto)
            {
                list.Add(programacion);
            }

            return new Response<IEnumerable<ProgramacionCierreAnualCuentasDto>> { Data = list };

        }
    }
}
