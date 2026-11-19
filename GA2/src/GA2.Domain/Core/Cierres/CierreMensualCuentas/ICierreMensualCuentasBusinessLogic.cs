using GA2.Application.Dto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ICierreMensualCuentasBusinessLogic
    {
        Task<Response<IPCDto>> ObtenerIPCMes();

        Task<Response<ProgramacionCierreMensualCuentasDto>> ProgramarCierreMensual(ParametrosCierreMensualCuentasDto request);
        Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensual();
        Task<Response<ProgramacionCierreMensualCuentasDto>> ActualizaEstadoCierreMensual(ActualizaCierreMensualCuentasDto data);
        Task<Response<IEnumerable<ReporteCierreMensualCuentasDto>>> ReporteCierreMensual(Guid Id);
        Task<Response<RespuestaCierreMensualCuentasDto>> CierreMensualCuentas(ParametrosCierreMensualCuentasDto request);
    }
}