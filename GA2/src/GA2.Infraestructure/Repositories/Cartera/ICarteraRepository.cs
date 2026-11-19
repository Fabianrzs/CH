using GA2.Application.Dto;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCarteraPorPeriodoDto;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Application.Dto.Constructora;
using GA2.Application.Dto.Credito;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera;
using GA2.Domain.Entities.Cartera.Balance;
using GA2.Domain.Entities.Cartera.Cierres;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual.Cartera.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Entities.Cartera.Novedades;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Domain.Entities.Cartera.ProductoPorCliente;
using GA2.Domain.Entities.Cartera.Reportes;
using GA2.Domain.Entities.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICarteraRepository
    {
        Task<IEnumerable<ProductoCliente>> ObtenerProductoCliente(int tipoIdentificacion, string identificacion, Guid? idProducto);
        Task<IEnumerable<ProductoCliente>> ObtenerProductosCliente(int tipoIdentificacion, string identificacion);
        Task<InformacionCredito> ObtenerCreditoCartera(ObtenerProductoCartera obtenerProductoCartera);
        Task<double> ObtenerCapitalNeto_AbonosExtra(string Cli_Identificacion);
        Task<InformacionCredito> ActualizarCredito(InformacionCredito updateCredito);
        Task<IEnumerable<AplicacionPago>> AplicarPago(PagoAplicarDto aplicacionPago);
        Task<AplicacionPago> ReversarPago(RequestReversionPagoDto reversionPagoDto);
        Task<bool> ReversarProcesoAplicacionPagos(int idProceso);
        Task<bool> ReversarPagoUnitarioDeLista(RequestReversionPagoListaDto request);
        Task<ParametrosAplicacionPagos> ObtenerParametrosAplicacionPagos();
        Task<AbonoExtra> ObtenerAbonoExtra(string identificacion);
        Task<ProductoCliente> EjecutarDesembolso(ProductoCliente producto);
        Task<ProductoCliente> PrepararEjecucionDesembolso(int tipoIdentificacion, string identificacion);
        Task<TasaSimulador> ActualizarTasasSimulacion(TasaSimulador tasaUpdate);
        Task<IEnumerable<TasaSimulador>> ObtenerTasasSimulador();
        Task<TasaSimulador> CrearTasasSimulacion(TasaSimulador tasaCreate); 
        Task<TasaSimulador> EliminarRegistroSimuladorPorId(TasaSimulador tasaDelete);
        Task<IEnumerable<TasaSimulador>> ConsultarTipoActorPorId( TasaSimulador consultaExcepcion);
        Task<IEnumerable<TipoActor>> ObtenerTipoActor();
        Task<ParametrosTasaActor> CrearTasaActor(ParametrosTasaActor actorCreate);
        Task<ParametrosTasaActor> ActualizarTasaSimuladorActor(ParametrosTasaActor actorUpdate);
        Task<ParametrosTasaActor> EliminarParametroSimuladorActor(ParametrosTasaActor actorDelete);
        Task<IEnumerable<UnidadesEjecutoras>> ObtenerUnidadesEjecutorasPorId(UnidadesEjecutoras consultaUnidadEjecutora);
        Task<UnidadesEjecutoras> CrearUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraCreate);
        Task<IEnumerable<UnidadesEjecutoras>> ConsultarUnidadEjecutora();
        Task<UnidadEjecutora> ConsultarUnidadEjecutoraPorId(int unidadEjecutoraID); 
        Task<UnidadesEjecutoras> ActualizarUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraUpdate);
        Task<UnidadesEjecutoras> EliminarUnidadEjecutoraSimulador(UnidadesEjecutoras unidadEjecutoraDelete);
        Task<ParamGeneralesCreditoYCartera> ObtenerParamGeneralesCreditoYCartera();
        Task<ParamGeneralesCreditoYCartera> ActualizarParamGeneralesCreditoYCartera(ParamGeneralesCreditoYCartera paramCreditoYCarteraUpdate);
        Task<InformacionCredito> ObtenerCreditoCarteraPorIdentificacion(string identificacion);
        Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerParametrosGeneralesPlazoCredito( );
        Task<ParamGeneralesExcepcionPlazo> ActualizarParametrosGeneralesPlazoCredito(ParamGeneralesExcepcionPlazo plazo);
        Task<ParamGeneralesExcepcionPlazo> CrearExcepcionPlazo(ParamGeneralesExcepcionPlazo plazoCreate);
        Task<ParamGeneralesExcepcionPlazo> EliminarExcepcionPlazo(ParamGeneralesExcepcionPlazo plazoDelete);
        Task<ParamGeneralesExcepcionPlazo> ActualizarExcepcionesPlazo(ParamGeneralesExcepcionPlazo excepcionUpdate);
        Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerExcepcionesPlazo(ParamGeneralesExcepcionPlazo getPlazo);
        Task<IEnumerable<ParamGeneralesExcepcionPlazo>> ObtenerExcepcionPlazoPorUnidadEjecutora(ParamGeneralesExcepcionPlazo getPlazo);
        Task<ParamGeneralesExcepcionPlazo> CrearExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazo plazoCreate);
        Task<ParamGeneralesExcepcionPlazo> ActualizarExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazo plazoUpdate);
        Task<IEnumerable<ParamGeneralesSeguros>> ObtenerSeguroGeneral();
        Task<ParamGeneralesSeguros> ActualizarSeguroGeneral(ParamGeneralesSeguros seguroUpdate);
        Task<IEnumerable<ParamGeneralesSeguros>> ObtenerExcepSeguroPorMunicipio();
        Task<ParamGeneralesSeguros> CrearExcepcionPorMunicipio(ParamGeneralesSeguros seguroExcepcionCreate);
        Task<ParamGeneralesSeguros> ActualizarExcepSeguroPorMunicipio(ParamGeneralesSeguros seguroExcepcionUpdate);
        Task<ParamGeneralesSeguros> EliminarExcepcionSeguroPorMunicipio(ParamGeneralesSeguros seguroExcepcionDelete);
        Task<DatosLiquidacion> ObtenerLiquidacionGeneral();
        Task<DatosLiquidacion> ActualizarLiquidacionGeneral(DatosLiquidacion request);
        Task<IEnumerable<AplicacionPago>> ObtenerAplicacionPago(Guid request);
        Task<IEnumerable<PagoListaDetalle>> GenerarLista(GenerarListaParametersDto listaParameters);
        Task<ExcepcionTasaActor> ObtenerExcepcionesPorTipoAfiliacion(int request);
        Task<ExcepcionTasaUEJ> ObtenerExcepcionesTasaUEj(int simId, int uEJ);
        Task<IEnumerable<ReporteCierreMensualCartera>> CierreMensualCartera(ParamCierreMensualCartera param);
        Task<bool> ExisteCierreMensualCartera(int anio, int mes);
        Task<bool> CrearTempTableAplicacionPagos();
        Task<ResultadoAplicacionPagos> ActualizarAplicacionDePagos(PagoMasivoNominaDto request, ParametrosAplicacionPagoDto parametrosAplicacionPago);
        Task<ComprobanteCarteraPorProcesoDTO> UpdateComprobanteContableCartera(ComprobanteCarteraUpdate comprobanteCarteraUpdate);
        Task<bool> UpdateDetallesComprobanteContableCartera(ComprobanteCarteraUpdate comprobanteCarteraUpdate);

        // NOVEDADES

        Task<NovedadCartera> CrearNovedadCartera(NovedadCartera novedad);
        Task<NovedadCartera> ActualizarNovedadCartera(NovedadCartera novedad);
        Task<NovedadCartera> ObtenerNovedadCartera(Guid IdNovedad);
        Task<IEnumerable<NovedadCartera>> ObtenerNovedadesCartera(Guid IdCredito);
      Task<IEnumerable<NovedadCartera>> ObtenerNovedadesCarteraPorCreditos(List<Guid> creditoIds);
        Task<IEnumerable<TiposCausalesAceleracionCartera>> ObtenerTiposCausalesAceleracionCartera();
        Task<SaldoObligacion> ObtenerSaldoObligacion(Guid IdCredito);
        Task<AplicacionPago> AplicarPagoNovedadCartera(RequestPagoNovedadDto request, decimal capital, decimal interesRemanente);
        Task<IEnumerable<TiposNovedadesCartera>> ObtenerTiposNovedadesCartera();
        Task<IEnumerable<EstadosNovedadCartera>> ObtenerEstadosNovedadCartera();
        Task<int> CrearNovedadRediferido(RequestNovedadRediferido request);
        Task<IEnumerable<ResponseNovedadRediferido>> ObtenerNovedadRediferido(Guid creditoId);

        // 

        Task<IEnumerable<AlternativosAplicacionPago>> ObtenerAlternativosAplicacionPago();
        Task<IEnumerable<PlanPago>> ObtenerPlanPago(ObtenerProductoCartera obtenerProductoCartera);
        Task<ProductoCliente> CrearProductoCredito(ProductoCliente productoCliente);
        Task<IEnumerable<AplicacionPago>> ObtenerAplicacionPagoByFechaPago(CierreDiarioDto request);
        Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByTipoCredito(int tcrId);
        Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(Int32 codOperacion);
        Task<IEnumerable<ReporteGeneralGarantias>> ObtenerValorGarantia(ReporteGeneralGarantiaParametersDto request);

        Task<InformacionCredito> ObtenerCreditoCarteraById(Guid idCredito);
        Task<bool> CrearMovContableCierreDiario(MovimientoContable request);
        Task<IEnumerable<MovimientoContableCierreDiario>> ObtenerMovContableCierreDiarioByNumCredito(int numCredito);
        Task<IEnumerable<UltimoCierre>> ConsultarUltimosCierres();
        Task<IEnumerable<ReporteAltasBajas>> GenerarReporteAltasBajas(int idUnidadEjecutora, int tipoReporte, int periodo);
        Task<IEnumerable<ColumnasArchivoCargueMasivo>> ObtenerColumnasArchivoCargueMasivoPorUnidadEjecutora(int unidadEjecutora);
        Task<IEnumerable<CuotaAdelantada>> ObtenerCuotasAdelantadas(Guid idCredito);
        Task<ProcesoAplicacionPagosMasivo> InsertarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo procesoAplicacionPagosMasivo);
        Task<ProcesoAplicacionPagosMasivo> ActualizarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo procesoAplicacionPagosMasivo);
        Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagos(int? idProceso = null, string estado = null);
        Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagosPorUnidadEjecutora(int unidadEjecutoraId, int? tipoCreditoId = null, int? periodo = null, string estado = null);
        Task<IEnumerable<ResultadoAplicacionPagos>> ActualizarAplicacionDePagos(ParametrosAplicacionPagoMasivoDto aplicacionPagos);
        Task<ProcesoAplicacionPagosMasivo> ActualizarEstadoProcesoAplicacionPagos(ProcesoAplicacionPagosMasivo procesoAplicacionPagosMasivo);
        Task<bool> EliminarProcesoAplicacionPagos(int idProceso);
        Task<IEnumerable<PlanPago>> ObtenerPlanPagosCredito(ObtenerProductoCartera obtenerProductoCartera);
        Task<PlanPagoAbonoCuota> ConsultarPlanPago(long numeroCredito, Guid cpgId);
        Task<IEnumerable<PlanPagoAbonoCuota>> ConsultarPlanPagoAdelantoCuota(long numeroCredito, Guid cpgId);
        Task<bool> UpdatePlanPago(UpdatePlanPago updatePlanPago);
        Task<bool> UpdatePlanPagoAdelantoCuota(UpdatePlanPagoAdelantoCuota updatePlanPago);
        Task<bool> UpdatePapAplicacionPagosMasivos(int idProceso, int mcoCont, int mcoNume);
        Task<bool> InsertarCpgDetallesPagosSeven(RequestDetallePago request);
        Task<IEnumerable<RequestDetallePago>> ObtenerDetallesPagosSevenCartera(int comprobanteId);
        Task<bool> UpdateCierresCarteraRealizados(RequestCierreMensual requestCierreMensual);
        Task<IEnumerable<ReporteCierreAnualCartera>> CierreAnualCartera(RequestCierreAnualCartera param);
        Task<IEnumerable<ConsultaSaldos>> ConsultarDataCreditoReportes(ConsultaSaldosRequest param);
        
        Task<IEnumerable<CuotaAdelantadaFechaCierre>> AplicarCuotasAdelantadasByFechaActual(DateTime fechaAplicacion);
        Task<IEnumerable<ProcesoAplicacionPagosMasivo>> ObtenerProcesoAplicacionPagosConDetalle(ParametrosObtenerProcesoAplicacionPagosDto request);
        Task<IEnumerable<GetMoraCredito>> GetMoraCredito(int idProceso);
        Task<IEnumerable<PlanPagoAbonoCuota>> ConsultarPlanPagoPorFecha(DateTime fechaAplicacion);
        //Funcion temporal
        Task<bool> CrearProdutoClienteCartera(CrearProductoCliente productoCliente);
        Task<bool> CrearPlanPagosProductoClienteCartera(List<InsertPlanPagosProductoCartera> insertPlanPagosProductoCartera, int TipoCredito, DbConnection connection = null, DbTransaction transaction = null);
        Task<bool> CrearCpgProductoClienteCartera(AplicacionPago aplicacionPago);
        Task<int> ConsultarPapAplicacionPagosMasivosSeven(int papId);
        Task<IEnumerable<AplicacionListaModel>> ObtenerProcesoAplicacionPagosConDetalleSeven(int papId);
        Task<IEnumerable<AplicacionListaModel>> ConsultarListasPorFecha(DateTime fechaPago);
        Task<ComprobanteCartera> CrearComprobanteCartera(ComprobanteCartera comprobanteCartera);
        Task<MovimientoComprobanteEntity> InsertarMovimientoComprobanteCartera(MovimientoComprobanteEntity movimientoComprobanteEntity);
        Task<ComprobanteCartera> ObtenerComprobanteCarteraByComprobanteId(int? ComprobanteId, int Transaction);
        Task<IEnumerable<ComprobanteCarteraPorProcesoDTO>> ObtenerComprobanteCartera(ObtenerComprobantePorProcesoDto Request, int CodigoOperacion);
        Task<IEnumerable<MovimientoComprobantePorProcesoDto>> ObtenerMovimientosContableCartera(MovimientosContableParametersDto Comprobante);
        //Task<IEnumerable<ObtenerComprobanteTotalizadoPorCuentaDto>> ObtenerComprobanteTotalizadoPorCuenta(ObtenerComprobanteParametersDto Comprobante);
        Task<IEnumerable<ComprobanteCarteraPorProcesoDTO>> ObtenerComprobanteCarteraPorId(int IdComprobanteContable, string CodigoProceso);
        Task<IEnumerable<MovimientoComprobanteEntity>> ObtenerMovimientosComprobanteCartera(int Comprobante);
        Task<IEnumerable<CierreMnesualSevenModel>> ObtenerDesembolsoSevenCierreMensual(DateTime fechaCierre);
        Task<IEnumerable<CierreMnesualSevenModel>> ObtenerCancelacionTotalCierreMensual(DateTime fechaCierreInicial, DateTime fechaCierreFinal);

        Task<bool> InsertarComprobantePagosSeven(RequestDetallePago request);
        Task<bool> InsertarMovimientoComprobante(MovimientoComprobanteEntity request);
        ////
        //Aplicacion de pago reducion cuota o plazo
        ///
        Task<AplicacionPago> AplicaPagoReducirCuota(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago);
        Task<AplicacionPago> AplicaPagoRediferidoPlazo(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago);
        Task<AplicacionPago> AplicaPagoReducirPlazo(RequestPagoAbonoCapital requestPagoAbonoCapital, InformacionCredito infoCredito, PagoAplicarDto aplicacionPago);

        ///
        ///APLICACION LIQUIDACION LEASIONG
        ///
        Task<AplicacionPago> AplicarPagoLiquidacionLeasing(PagoAplicarDto aplicacionPago);

        Task<AplicacionPago> AplicarPagoOpcionCompra(PagoAplicarDto aplicacionPago);

        ///
        /// CREACION DE RESTITUCION INMUBLE DE CARTERA
        ///
        Task<int> CreateRestitucionInmuebleCartera(RestitucionInmuebleCartera request);
        Task<int> ActualizarEstadoCreditoRestituido(string identificacion, int tipoIdentificacion, Guid creditoid);
        Task<IEnumerable<RestitucionInmuebleCartera>> ObtenerRestitucionInmuebleCartera(Guid idCredito);
        Task<Guid> AplicarPagoCancelacionRestitucion(PagoAplicarDto request);
        Task<Guid> AplicarPagoCancelacionRestructuracion(PagoAplicarDto request);

        /// <summary>
        /// TOTALESBALANCE
        /// </summary>
        /// <param name="Comprobante"></param>
        /// <returns></returns>
        Task<IEnumerable<ObtenerComprobanteTotalizadoPorCuentaDto>> ObtenerComprobanteTotalizadoPorCuenta(ObtenerComprobanteParametersDto Comprobante);
        Task<IEnumerable<BalanceCarteraPorPeriodo>> ConsultarBalancePorFecha(ObtenerComprobantePorProcesoDto Request);

        Task<IEnumerable<ObtenerGarantiaActualizada>> ObtenerCreditosActualizanGarantia(requestGarantiasDto parametros);

        Task<ObtenerGarantiaActualizada> ObtenerUltimosAjustesGarantiaPorCedula(string cliIdentificacion);
        Task<bool> ActualizarNumeroCuota(AbonoHistorico dto);

        Task<int> UpdateNuevaTasaEACredito(InsertPlanPagosProductoCartera request, int causal, int tipoCredito);
        Task<int> ActualizarTasaEACredito(Guid creditoId, decimal tasaEA,double cuota, decimal tasa, int causal);
        Task<int> ActualizarCausacionInteresMora(Guid creId, decimal causacionMora, int diasMora, DateTime fechaUltimo, decimal SaldoMora, decimal moraAnterior);
        Task<InformacionCredito> ObtenerCreditoEnMoraCarteraPorIdentificacion(string identificacion);
        Task<bool> InsertarAbonoHistorico(AbonoHistorico dto);
        Task<IEnumerable<AbonoHistorico>> ObtenerAbonoHistorico(string numeroCredito, int tipoCredito);
        Task<List<AplicacionPago>> ObtenerPagoAbonoHistorico(string numeroCredito, int typeCredit);
        Task<int> ActualizarPorcentajeSeguro(float seguroVida, float seguroHogar, long nroCredito, int plazo, int plazoPendiente);
        Task<int> ActualizarPlanPagoMasivos(DataTable dt, int tipoCredito);
        // 1. Obtener parámetros
        Task<object> ObtenerParametro(string nombreParametro);

        // 2. Validaciones de mora
        Task<bool> ValidarMoraEnPeriodo(long numeroCredito, DateTime fechaInicio, DateTime fechaFin, int diasMoraMaxima);

        // 3. Contar reestructuraciones
        Task<int> ContarReestructuraciones(long numeroCredito, DateTime fechaInicio, DateTime fechaFin);

        // 4. Intereses
        Task<decimal> ObtenerInteresesCorrientesPendientes(long numeroCredito, int tipoCredito);
        Task<decimal> ObtenerInteresesMora(long numeroCredito, int tipoCredito);

        // 5. Tasa vigente
        Task<double> ObtenerTasaVigenteReestructuracion(int tipoCredito);

        // 6. Transacciones
        Task RegistrarTransaccion(TransaccionContable transaccion);

        // 7. Bloqueo
        Task AsignarBloqueoCredito(long numeroCredito, int codigoBloqueo, string concepto, DateTime fechaAsignacion);

        // 8. Histórico
        Task InsertarHistoricoReestructuracion(HistoricoReestructuracion historico);
        Task CancelarInteresRemanenteYCapitalPorPagar(long numeroCredito);
        Task<IEnumerable<PagoPagoMora>> ObtenerPagosPorRango(
            Guid creId,
            DateTime fechaInicial,
            DateTime fechaFinal);
        Task<bool> RegistrarPagoEnMora(
            Guid creId,
            DateTime fechaPago,
            DateTime fechaAplicacion,
            decimal monto,
            int diasMoraAlPago,
            decimal causacionMoraAlPago);
        Task<PagoPagoMora> ObtenerPagoDeDia(Guid creId, DateTime fechaAplicacion);
        Task<bool> ActualizarComprobanteCarteraEliminarAfiliado(int ccId,
            decimal totalCredito,
            decimal totalDebito,
            string movContable);
        Task<AplicacionPago> AplicarPagoMora(PagoAplicarDto aplicacionPago);

        Task ActualizarTipoRecurso(Guid idAplicacionPago, string tipoRecurso);

        Task<int> ActualizarMoraDelCredito(
            Guid creId,
            decimal nuevoAcumuladoInteresMora,
            int nuevosDiasMora);
        Task<bool> ActualizarComprobanteGarantiaCartera(int ccId, int ccGarantia);
        Task<int> ObtenerComprobanteGarantiaCartera(int ccId);
        Task<int> ActualizarNumeroComprobanteCpgAplicacionPago(Guid creditoId, int comprobanteId);
        Task<IEnumerable<ResponseCreditosDesembolsadosPorPeriodo>> ObtenerCreditosDesembolsadosPorPeriodo(int anio, int mes, int tipoCredito);
        Task<IEnumerable<CreditosConPagosEnAnio>> ObtenerCreditosConPagosEnAnio(int anio, int tipoCredito);
        Task<IEnumerable<PagosAplicadosCreditoAnio>> ObtenerPagosAplicadosCreditoAnio(int anio, Guid creId);
        Task<long> ObtenerCreditoEnMoraPeriodo(long numeroCredito, int mes, int anio);

    }
}