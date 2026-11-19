using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IVerificacionDerechosRepository
    {

        /// <summary>
        /// Metodo para obtener todos los IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>02/05/2023</date>
        /// <param name="ParametrosVerificacionDerecho"></param>
        /// <returns></returns>
        Task<IEnumerable<ParametrosVerificacionDerecho>> ObtenerTodoVerificacionDerecho();

        /// <summary>
        /// Metodo para obtener todos los IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>02/05/2023</date>
        /// <param name="InfoVerificacionDerecho"></param>
        /// <returns></returns>
        Task<IEnumerable<InfoVerificacionDerecho>> ObtenerTodoVerificacionDerechoInfoById(int ID);

        /// <summary>
        /// Metodo para obtener todos los IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>02/05/2023</date>
        /// <param name="InfoVerificacionDerecho"></param>
        /// <returns></returns>
        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerVerificacionDerechoResultById(int ID);
     

        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoDatosVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID);

        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerVerificacionDerechoResultByPeriodo(int periodoAnio, int PeriodoMes);

        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionDerechoCuentas();

        /// <summary>
        /// Metodo para crear un IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>04/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>

        Task<ParametrosVerificacionDerecho> CrearPeriodoVerificacionDerecho(ParametrosVerificacionDerecho parametroVerificacionDrecho);

        /// <summary>
        /// Metodo para crear un IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>04/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>

        Task<ParametrosVerificacionDerecho> ActualizarPeriodoVerificacionDerecho(ParametrosVerificacionDerecho parametroVerificacionDrecho);

        /// <summary>
        /// Actualizar parametros verificación derechos
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>04/05/2023</date>
        /// <param name="ParametrosVerificacionDerecho"></param>
        /// <returns></returns>

        Task<ParametrosVerificacionDerecho> ActualizarParametrosVerificacionDerechos(ParametrosVerificacionDerecho parametroVerificacionDrecho);


        /// <summary>
        /// Actualizar parametros verificación derechos
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>04/05/2023</date>
        /// <param name="ParametrosVerificacionDerecho"></param>
        /// <returns></returns>

        Task<ResultVerificacionDerecho> ActualizarVerificacionDerechosResult(ResultVerificacionDerecho resultVerificacionDerechoDto);



        Task<ResultVerificacionDerecho> CrearVerificacionDerechoResult(ResultVerificacionDerecho resultVerificacionDerechoDto);

        Task<ParametrosVerificacionDerecho> ObtenerTodoVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<ParametrosVerificacionDerecho>> ObtenerTodoVerificacionDerechosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID);

        Task<IPC> ObtenerIPCByPeriodo(VerificacionDerechosPeriodo ID);

        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionDerechosResultByAnioCuenta(int CTA_ID, int ANIO);

        Task<ResultVerificacionDerecho> ObtenerValorMesAnteriorCuenta(int CTA_CUENTA, int ANIO, int MES);
                
         Task<DatosCuentasAdministracionCesantiasResult> CrearDatosCuentasAdministracionCesantiasResult(DatosCuentasAdministracionCesantiasResult datosCuentasAdministracionCesantiasResultDto);

        Task<DatosCuentasAdministracionCesantiasResult> ObtenerValorMesAnteriorCuentaCesantias(int CTA_CUENTA, int ANIO, int MES);




        Task<DatosCuentasAVResult> CrearDatosCuentasAVResult(DatosCuentasAVResult datosCuentasAVResult);

        Task<ObtenerCuentasAVACResult> InsertarDatosCuentasAVAC(ObtenerCuentasAVACResult datosCuentasAVACResult);
        Task<ObtenerCuentasAVAVResult> InsertarDatosCuentasAVAV(ObtenerCuentasAVAVResult datosCuentasAVAVResult);
        Task<ObtenerCuentasAVCResult> InsertarDatosCuentasAVC(ObtenerCuentasAVCResult datosCuentasAVCResult);



        Task<DatosCuentasAVResult> ObtenerValorMesAnteriorDatosCuentasAVResult(int CTA_CUENTA, int ANIO, int MES);
        Task<ObtenerCuentasAVACResult> ObtenerValorMesAnteriorDatosCuentasAVACResult(int CTA_CUENTA, int ANIO, int MES);
        Task<ObtenerCuentasAVAVResult> ObtenerValorMesAnteriorDatosCuentasAVAVResult(int CTA_CUENTA, int ANIO, int MES);
        Task<ObtenerCuentasAVCResult> ObtenerValorMesAnteriorDatosCuentasAVCResult(int CTA_CUENTA, int ANIO, int MES);

        Task<ParametrosAbonoInteres> CrearPeriodoValoresInteresAbonados(ParametrosAbonoInteres parametrosAbonoInteres);
        Task<IEnumerable<ParametrosAbonoInteres>> ObtenerTodoValoresInteresAbonados();
        Task<IEnumerable<ParametrosAbonoInteres>> ObtenerTodoValoresInteresAbonadossFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID);



        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoDatosCuentasAVResultFilterPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoDatosCuentasAVACResultFilterPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoDatosCuentasAVCResultFilterPeriodo(VerificacionDerechosPeriodo ID);
        Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(VerificacionDerechosPeriodo ID);



        Task EliminarParamValoresInteresAbonados(VerificacionDerechosPeriodo ID);
        Task EliminarParamVerificacionDerechosResult(VerificacionDerechosPeriodo ID);
        Task EliminarVerificacionCuentasAdministracionCesantias(VerificacionDerechosPeriodo ID);
        Task EliminarVerificacionCuentasAV(VerificacionDerechosPeriodo ID);
        Task EliminarVerificacionDerechosSolucionVivienda(VerificacionDerechosPeriodo ID);

        Task<ParametrosVerificacionDerechoDto> ObtenerTodoVerificacionDerechoByPeriodo(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<ParametrosVerificacionDerechoDto> ObtenerTodoVerificacionDerechoBySolId(Guid? id);

        Task<ParametrosAbonoInteres> ActualizarParametrosInteresAbonadosTotal(ParametrosAbonoInteres parametrosAbonoInteres);

        Task<ParametrosAbonoInteres> ObtenerTodoValoresInteresAbonadosByPeriodo(VerificacionDerechosPeriodo verificacionDerechosPeriodo);


        Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoVerificacionAdministracionCesantiasByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoVerificacionCuentasAVByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionSolucionViviendaByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);

        Task<IEnumerable<SolicitudConsecutivo>> ObtenerTodoSolicitudConsecutivo();

        Task<SolicitudConsecutivo> ActualizarSolicitudConsecutivo(SolicitudConsecutivoDto solicitudConsecutivoDto);


        
        Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoDatosCuentasAVResultFilterIdentificacion(int IDENTIFICACION);
        Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoCuentasSolucionViviendaResultFilterIdentificacion(int IDENTIFICACION);
        Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoCuentasAdministracionCesantiasResultFilterIdentificacion(int IDENTIFICACION);

        Task<ParametrosAbonoInteres> ActualizarParametrosInteresAbonadosMovimientos(ParametrosAbonoInteres parametrosAbonoInteres);



        Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoDatosCuentasAVACResultFilterIdentificacion(int IDENTIFICACION);
        Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoDatosCuentasAVAVResultFilterIdentificacion(int IDENTIFICACION);
        Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoDatosCuentasAVCResultFilterIdentificacion(int IDENTIFICACION);

        Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoVerificacionCuentasAVACByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoVerificacionCuentasAVAVByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoVerificacionCuentasAVCByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo);
        Task<NotificacionTarea> ObtenerUsuarioVerificacionDerechos(string CTA_CUENTA);
        Task<NotificacionTarea> InsertarNotificacionTarea(NotificacionTarea notificacionTarea);
        Task<VerificacionDerechoSolicitudTarea> CrearVerificacionDerechoSolicitudTarea(VerificacionDerechoSolicitudTarea verificacionDerechoSolicitudTarea);
        Task<VerificacionDerechoSolicitudTarea> ActualizarSolicitudTareaVerificacionDerechos(ActualizarSolicitudTarea actualizarSolicitudTarea);

        Task<ParamValoresIPCNotificacionAutomatica> ObtenerValoresIPCNotificacionAutomatica(int ANIO, int MES);

        Task ActualizarNotificacionTareaVerificacionYAbonos(Guid tareaSiguiente, string Link);
        Task<int> ICT_InsertarCuentaTemporal(List<CuentaTemp> cuentas);
        Task<int> Sp_CrearDatosCuentasAVResult(IPC request, long consecutivo);

        Task<List<DatosCuentasAVDto>> ICT_ConsultarCuentasTemporales();
        Task<int> InsertarDatosCedulasCesantias(List<DatosCedulasCesantias> request, double ipc, int mes, int anio);
        Task<int> InsertarDatosInfoCesantia(DatosInfoCensatiaByCuentaPerioso request, int cuentaId);
        Task<int> InsertarDatosVerificacionDerechoCuentaTemp(List<DatosVerificacionDerechoCuenta> request);
        Task<int> Insertar_ObtenerCuentasAVAC(List<ObtenerCuentasAVAC> request);
        Task<int> InsertarTempCliente(ClienteDto request);
        Task<int> InsertarTempCuentaCliente(List<CuentaDto> request);
        Task<int> InsertarTempMovimiento(List<MovimientoDTO> request);
        Task<int> InsertarDatosConcetoCuenta(List<ConceptoCuentaDto> request);
        Task<int> InsertarDatosMovimientoConcetoCuenta(List<MovimientosCuentaDto> request);
        Task<int> InsertarTOCAVC_Temp_ObtenerCuentaAVC(List<ObtenerCuentaAVAVResponse> request);
        Task<int> SP_TOCAVAV_InsertObtenerCuentaAVAV(List<ObtenerCuentaAVAVResponse> request);

        Task<List<DatosCedulasCesantias>> ConsultarDatosCedulasCesantias(int mes, int anio);
        Task<DatosInfoCensatiaByCuentaPerioso> ConsultarDatosInfoCesantia(ObtenerInfoCedulaRequestDto request);
        Task<List<ObtenerCuentasAVAC>> ConsultarCuentasAVAC();
        Task<ClienteDto> ConsultarInformacionCliente(string identificacion);
        Task<List<CuentaDto>> ConsultarInformacioCuentaCliente(int clientId);
        Task<List<ConceptoCuentaDto>> ConsultarInformacioConceptoCuentaCliente(int cuentaId);
        Task<List<MovimientoDTO>> ConsultarInformacioMovimientoCuentaCliente(int cuentaId);
        Task<List<ObtenerCuentaAVAVResponseDto>> ConsultarDaraCuentaAVAV();
        Task<List<ObtenerCuentaAVAVResponseDto>> ConsultarCuentaAVC();
        Task<int> InsertarDatosVerificacionDerechoCuenta(List<DatosVerificacionDerechoCuentaDto> request);
        Task<List<DatosVerificacionDerechoCuentaDto>> ConsultaDatosVerificacionDerechoCuenta();
        Task<ParametrosVerificacionDerechoDto> ObtenerAbonoInteresBySolId(Guid? id);
    }


}
