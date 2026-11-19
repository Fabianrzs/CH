using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{

    /// <summary>
    /// ICierreAnualRequestProvider
    /// </summary>
    public interface ICierreAnualCuentasRequestProvider
    {
        Task<Response<RespuestaCierreAnualCuentasDto>> CierreAnualCuentas(ProgramacionCierreAnualCuentas programacion);

        Task<Response<IEnumerable<ProgramacionCierreAnualCuentasDto>>> ConsultarProgramacionCierreAnualCuentas();
    }
}