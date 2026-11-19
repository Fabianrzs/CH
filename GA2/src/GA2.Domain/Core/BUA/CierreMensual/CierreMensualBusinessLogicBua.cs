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
    public class CierreMensualBusinessLogicBua : ICierreMensualBusinessLogicBua
    {

        private readonly IMapper _mapper;
        private readonly ICierreMensualRepositoryBua _cierreMensualRepositoryBua;

        public CierreMensualBusinessLogicBua(
            ICierreMensualRepositoryBua cierreMensualRepositoryBua,
            IMapper mapper)
        {
            _cierreMensualRepositoryBua = cierreMensualRepositoryBua;
            _mapper = mapper;

        }

        public async Task<Response<RespuestaCierreMensualCuentasDto>> CierreMensualCuentas(ProgramacionCierreMensualCuentasDto request)
        {

            ProgramacionCierreMensualCuentas cierre = new ProgramacionCierreMensualCuentas();
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
            cierre.CCT_MENSUAL_CREADOPOR = request.CreadoPor;
            cierre.CCT_MENSUAL_FECHA_CREACION = request.FechaCreacion;
            cierre.CCT_MENSUAL_MODIFICADOPOR = request.ModificadoPor;
            cierre.CCT_MENSUAL_FECHA_MODIFICACION = request.FechaModificacion;

            RespuestaCierreMensualCuentasDto cierreRespuesta = _mapper.Map<RespuestaCierreMensualCuentasDto>(await _cierreMensualRepositoryBua.
                CierreMensual(cierre));

            return new Response<RespuestaCierreMensualCuentasDto>
            {
                Data = cierreRespuesta,
            };

        }

        public async Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensualCuentas()
        {
            List<ProgramacionCierreMensualCuentasDto> list = new List<ProgramacionCierreMensualCuentasDto>();

            IEnumerable<ProgramacionCierreMensualCuentasDto> respuestaprogramacionCierresDto = _mapper.Map<IEnumerable<ProgramacionCierreMensualCuentasDto>>(await this._cierreMensualRepositoryBua.
                ConsultarProgramacionCierreMensual());

            foreach (var programacion in respuestaprogramacionCierresDto)
            {
                list.Add(programacion);
            }

            return new Response<IEnumerable<ProgramacionCierreMensualCuentasDto>> { Data = list };

        }

        public async Task<Response<IEnumerable<ReporteCierreMensualCuentasDto>>> ReporteCierreMensualCuentas(Guid Id)
        {
            List<ReporteCierreMensualCuentasDto> list = new List<ReporteCierreMensualCuentasDto>();

            IEnumerable<ReporteCierreMensualCuentasDto> respuestareporteCierresDto = _mapper.Map<IEnumerable<ReporteCierreMensualCuentasDto>>(await this._cierreMensualRepositoryBua.
                ReporteCierreMensual(Id));

            foreach (var programacion in respuestareporteCierresDto)
            {
                list.Add(programacion);
            }

            return new Response<IEnumerable<ReporteCierreMensualCuentasDto>> { Data = list };

        }
    }
}
