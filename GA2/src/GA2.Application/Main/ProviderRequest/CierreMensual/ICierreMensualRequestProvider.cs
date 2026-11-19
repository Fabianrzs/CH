using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main
{

    /// <summary>
    /// ICierreMensualRequestProvider
    /// </summary>
    public interface ICierreMensualRequestProvider
    {
        Task<Response<RespuestaCierreMensualCuentasDto>> CierreMensualCuentas(ProgramacionCierreMensualCuentas programacion);
        Task<Response<IEnumerable<ProgramacionCierreMensualCuentasDto>>> ConsultarProgramacionCierreMensualCuentas();
        Task<Response<IEnumerable<ReporteCierreMensualCuentasDto>>> ReporteCierreMensualCuentas(Guid Id);

        Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerTodoDatosVerificacionDerechos(VerificacionDerechosByFechaDto request);

        Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request);
        Task<Response<List<DatosVerificacionDerechoCuentaDto>>> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request);
        Task<Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<DatosCuentasAVDto>>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request);

        Task<Response<IEnumerable<ObtenerCuentasAVACDto>>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<ObtenerCuentasAVAVDto>>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request);
        Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtemerCedulaCuentaAVA();
        Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtemerCedulaCuentaAVC();
        Task<Response<IEnumerable<DatosCedulasCesantiasDto>>> ObtenerCedulaCuentaCesantias();
        Task<Response<DatosInfoCensatiaByCuentaPeriodoDto>> ObtenerInfoCuentaByPeriodo(ObtenerInfoCedulaRequestDto request);
        Task<Response<IEnumerable<ObtenerCuentasAVCDto>>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request);
    }
}