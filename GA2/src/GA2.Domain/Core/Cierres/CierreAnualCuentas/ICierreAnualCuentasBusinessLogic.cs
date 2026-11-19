using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ICierreAnualCuentasBusinessLogic
    {
        Task<Response<IPCDto>> ObtenerIPCMes();
        Task<Response<ProgramacionCierreAnualCuentasDto>> ProgramarCierreAnualCuentas(ParametrosCierreAnualDto request);
        Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentas();
        Task<Response<ProgramacionCierreAnualCuentasDto>> ActualizaEstadoCierreAnualCuentas(ActualizaCierreAnualCuentasDto data);
        Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentasBUA();
    }
}