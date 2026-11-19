using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos;

namespace GA2.Domain.Core
{
    public interface IVerificacionDerechosBusinessLogic
    {

        Task<Response<IEnumerable<ParametrosVerificacionDerechoDto>>> ObtenerTodoVerificacionDerecho();

        Task<Response<IEnumerable<InfoVerificacionDerechoDto>>> ObtenerTodoVerificacionDerechoInfoById(int ID);
      
        Task<Response<IEnumerable<ResultVerificacionDerechoDto>>> ObtenerVerificacionDerechoResultById(int ID);

        Task<Response<ParametrosVerificacionDerechoDto>> CrearPeriodoVerificacionDerecho(IPCDto parametroIPC, long Consecutivo);

        Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerTodoDatosVerificacionDerecho();

        Task<Response<ParametrosVerificacionDerechoDto>> CrearParametrosVerificacionDerecho(ParametrosVerificacionDerechoDto parametrizacionIPC);

        
        Task<Response<IEnumerable<ResultVerificacionDerechoDto>>> ObtenerTodoDatosVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID);
        
       Task<Response<ParametrosVerificacionDerechoDto>> ObtenerTodoVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID);

       Task<Response<IEnumerable<ParametrosVerificacionDerechoDto>>> ObtenerTodoVerificacionDerechosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID);

        Task<Response<InfoVerificacionDerechoDto>> ObtenerTodoVerificacionDerechosInfoByPeriodo(VerificacionDerechosPeriodo ID);

        Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request);

        Task<Response<ParametrosVerificacionDerechoDto>> ActualizarTodoECTCuentas(VerificacionDerechosPeriodo request);

        Task<Response<InteresNoCausadoDto>> ObtenerTodoVerificacionDerechosResultByAnioCuenta(InteresNoCausadoDto ID);
        
        Task<Response<IEnumerable<LiquidacionCuentasSolucionViviendaDTO>>> ObtenerInteresesCuentasSolucionVivienda(InteresNoCausadoDto request);

        Task CrearDatosCuentasAdministracionCesantiasResult(IPCDto parametroIPC, long Consecutivo);
        Task<Response<IEnumerable<ParametrosAbonosInteresDto>>> ObtenerTodoValoresInteresAbonados();

        Task<Response<IEnumerable<ParametrosAbonosInteresDto>>> ObtenerTodoValoresInteresAbonadosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID);
        Task<Response<IEnumerable<InformacionConsolidadoBaseCalculo>>> ObtenerConsolidadoFiltroByPeriodo(VerificacionDerechosPeriodo ID);
        Task<Response<ParametrosAbonosInteresDto>> CrearPeriodoValoresInteresAbonados(IPCDto parametroIPC, long Consecutivo);

        Task CrearDatosCuentasAVResult(IPCDto parametroIPC, long Consecutivo);
        Task CrearDatosCuentasAVACResult(IPCDto parametroIPC, long Consecutivo);
        Task CrearDatosCuentasAVAVResult(IPCDto parametroIPC, long Consecutivo);
        Task CrearDatosCuentasAVCResult(IPCDto parametroIPC, long Consecutivo);


        Task<Response<IEnumerable<InformacionAbonoInteresUnidadEjecutora>>> ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodo(VerificacionDerechosPeriodo ID);

        Task<Response<CrearAbonosInteresDatos>> CrearAbonosInteresesDatos(IPCDto parametroIPC);

        Task EliminarActualizarIpc(IPCDto parametroIPC);

        Task<Response<ParametrosVerificacionDerechoDto>> ObtenerTodoVerificacionDerechoByPeriodo(IPCDto parametroIPC);
        Task CrearDatosCuentasSolucionViviendaResult(IPCDto parametroIPC, long Consecutivo);

        Task<Response<ParametrosAbonosInteresDto>> ObtenerTodoValoresInteresAbonadosByPeriodo(IPCDto parametroIPC);

        Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacion(int IDENTIFICACION);

        Task<Response<ParametrosAbonosInteresDto>> AplicarMovimientosInteresesAbonados(VerificacionDerechosPeriodo verificacionDerechosPeriodo);

        Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacionYAnio(InteresNoCausadoDto request);
        Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacionYAnioYConcepto(InteresNoCausadoDto request);

        Task<Response<ParametrosAbonosInteresDto>> ActualizarParametrosInteresAbonadosMovimientos(ParametrosAbonosInteresDto request);

        Task SincronizacionCuentasAV(IPCDto parametroIPC);
        Task SincronizacionCuentasAdmiCesantias(IPCDto parametroIPC);
        Task SincronizacionCuentaAdmiVivienda(IPCDto parametroIPC);
        Task SincronizacionCuentaAVAC(IPCDto parametroIPC);
        Task SincronizacionCuentaAVAV(IPCDto parametroIPC);
        Task SincronizacionCuentaAVC(IPCDto parametroIPC);

    }
}
