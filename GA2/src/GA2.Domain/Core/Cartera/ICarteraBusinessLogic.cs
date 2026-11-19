using GA2.Application.Dto;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto.Cartera.Cierres;
using GA2.Application.Dto.Cartera.Cierres.CierreAnual;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Application.Dto.Constructora;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using System.DirectoryServices.ActiveDirectory;
using GA2.Domain.Entities.Cartera.Pagos;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Entities.Cartera.CreditoCarteraProducto;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera;
using GA2.Domain.Entities.Cartera.Novedades;

namespace GA2.Domain.Core
{
    public interface ICarteraBusinessLogic
    {
        Task<Response<IEnumerable<ProductoClienteDto>>> ObtenerProductoCliente(SolicitudProductoPorClienteDto request);
        Task<Response<IEnumerable<ProductoClienteDto>>> ObtenerProductosCliente(SolicitudProductoPorClienteDto request);
        Task<Response<InformacionCreditoDto>> ObtenerCreditoCartera(ObtenerProductoCartera obtenerProductoCartera);
        Task<Response<AplicacionPagoDto>> AplicarPago(PagoAplicarDto request);
        Task<Response<AplicacionPagoDto>> ReversarPago(RequestReversionPagoDto request);
        Task<Response<ParametrosAplicacionPagosDto>> ObtenerParametrosAplicacionPagos();
        Task<Response<ProductoClienteDto>> EjecutarDesembolso(RequestEjecucionDesembolsoDto request);
        Task<Response<ProductoClienteDto>> PrepararEjecucionDesembolso(SolicitudProductoPorClienteDto request);
        Task<Response<TasaSimuladorDto>> ActualizarTasasSimulacion(TasaSimuladorDto tasas);
        Task<Response<IEnumerable<TasaSimuladorDto>>> ObtenerTasasSimulador();
        Task<Response<IEnumerable<AplicacionPagoDto>>> ObtenerAplicacionPago(Guid request);
        Task<Response<TasaSimuladorDto>> CrearTasasSimulacion(TasaSimuladorDto tasas);
        Task<Response<TasaSimuladorDto>> EliminarRegistroSimuladorPorId(TasaSimuladorDto tasas);
        Task<Response<IEnumerable<TasaSimuladorDto>>> ConsultarTipoActorById(TasaSimuladorDto tasas);
        Task<Response<IEnumerable<TipoActorDto>>> ObtenerTipoActor();
        Task<Response<ParametrosTasaActorDto>> CrearTasaActor(ParametrosTasaActorDto actores);
        Task<Response<ParametrosTasaActorDto>> ActualizarTasaSimuladorActor(ParametrosTasaActorDto actores);
        Task<Response<ParametrosTasaActorDto>> EliminarParametroSimuladorActor(ParametrosTasaActorDto actores);
        Task<Response<ExcepcionTasaActorDto>> ObtenerExcepcionesPorTipoAfiliacion(int request);
        Task<ActionResult> CargaMasivaLeasing(IFormFile request);
        Task<Response<DatosLiquidacionDto>> ObtenerLiquidacionGeneral();
        Task<Response<DatosLiquidacionDto>> ActualizarLiquidacionGeneral(DatosLiquidacionDto request);
        Task<Response<IEnumerable<UnidadesEjecutorasDto>>> ObtenerUnidadesEjecutorasPorId(UnidadesEjecutorasDto unidades);
        Task<Response<UnidadesEjecutorasDto>> CrearUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades);
        Task<Response<IEnumerable<UnidadesEjecutorasDto>>> ConsultarUnidadEjecutora();
        Task<Response<UnidadesEjecutorasDto>> ActualizarUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades);
        Task<Response<UnidadesEjecutorasDto>> EliminarUnidadEjecutoraSimulador(UnidadesEjecutorasDto unidades);
        Task<Response<ParamGeneralesCreditoYCarteraDto>> ObtenerParamGeneralesCreditoYCartera();
        Task<Response<ParamGeneralesCreditoYCarteraDto>> ActualizarParamGeneralesCreditoYCartera(ParamGeneralesCreditoYCarteraDto parametros);
        Task<Response<List<AplicacionPagoDto>>> CargarArchivoAplicacionPagoMasivo(IFormFile request, int unidadEjecutora);
        Task<Response<List<ResultadoAplicacionPagoDto>>> CargarArchivoPagoMasivo(IFormFile request, ParametrosAplicacionPagoDto parametrosAplicacionPago);
        Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerParametrosGeneralesPlazoCredito();
        Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarParametrosGeneralesPlazoCredito(ParamGeneralesExcepcionPlazoDto plazos);
        Task<Response<ParamGeneralesExcepcionPlazoDto>> CrearExcepcionPlazo(ParamGeneralesExcepcionPlazoDto plazoCreate);
        Task<Response<ParamGeneralesExcepcionPlazoDto>> EliminarExcepcionPlazo(ParamGeneralesExcepcionPlazoDto plazoDelete);
        Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarExcepcionesPlazo(ParamGeneralesExcepcionPlazoDto plazos);
        Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerExcepcionesPlazo(ParamGeneralesExcepcionPlazoDto getPlazo);
        Task<Response<IEnumerable<ParamGeneralesExcepcionPlazoDto>>> ObtenerExcepcionPlazoPorUnidadEjecutora(ParamGeneralesExcepcionPlazoDto getPlazo);
        Task<Response<ParamGeneralesExcepcionPlazoDto>> CrearExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazoDto plazoCreate);
        Task<Response<ParamGeneralesExcepcionPlazoDto>> ActualizarExcepcionPlazoUnidadEjecutora(ParamGeneralesExcepcionPlazoDto plazoUpdate);
        Task<Response<IEnumerable<ParamGeneralesSegurosDto>>> ObtenerSeguroGeneral();
        Task<Response<ParamGeneralesSegurosDto>> ActualizarSeguroGeneral(ParamGeneralesSegurosDto seguroUpdate);
        Task<Response<IEnumerable<ParamGeneralesSegurosDto>>> ObtenerExcepSeguroPorMunicipio();
        Task<Response<ParamGeneralesSegurosDto>> CrearExcepcionPorMunicipio(ParamGeneralesSegurosDto excepcionCreate);
        Task<Response<ParamGeneralesSegurosDto>> ActualizarExcepSeguroPorMunicipio(ParamGeneralesSegurosDto excepcionUpdate);
        Task<Response<ParamGeneralesSegurosDto>> EliminarExcepcionSeguroPorMunicipio(ParamGeneralesSegurosDto excepcionDelete);
        Task<Response<IEnumerable<PagoListaDetalleDto>>> GenerarLista(GenerarListaParametersDto listaParametersDto);
        Task<Response<ExcepcionTasaUEJDto>> ObtenerExcepcionesPlazoUEj(int simId, int uEJ);
        Task<Response<IEnumerable<ReporteCierreMensualCarteraDto>>> CierreMensualCartera(ParamCierreMensualCarteraDto param);
        Task<Response<NovedadCarteraDto>> CrearNovedadCartera(NovedadCarteraDto novedad);

        Task<Response<NovedadCarteraDto>> ActualizarNovedadCartera(NovedadCarteraDto novedad);

        Task<Response<IEnumerable<NovedadCarteraDto>>> ObtenerNovedadesCartera(Guid IdCredito);

        Task<Response<IEnumerable<TiposNovedadesCarteraDto>>> ObtenerTiposNovedadesCartera();

        Task<Response<IEnumerable<EstadosNovedadCarteraDto>>> ObtenerEstadosNovedadCartera();

        Task<Response<IEnumerable<AlternativosAplicacionPagoDto>>> ObtenerAlternativosAplicacionPago();

        Task<Response<ValidarAplicacionPagoDto>> ValidarAplicacionPago(ObtenerProductoCartera request);

        Task<Response<ResponsePlanPagoDto>> ObtenerPlanPago(ObtenerProductoCartera obtenerProductoCartera);
        Task<Response<ProductoClienteDto>> CrearProductoCredito(ProductoClienteDto productoCliente);
        Task<Response<IEnumerable<UltimoCierreDto>>> ConsultarUltimosCierres();
        Task<FileResult> GenerarReporteAltasBajasAsync(RequestReporteAltasBajasDto request);
        Task<Response<IEnumerable<CuotaAdelantadaDto>>> ObtenerCuotasAdelantadas(Guid idCredito);
        Task<Response<IEnumerable<responseAdelantadaFechaCierreDto>>> AplicarCuotasAdelantadasByFechaActual(DateTime FechaAplicacion, CancellationToken cancellationToken);
        Task<Response<ComprobanteCarteraDto>> CrearCuotaCartera(RequestCarteraSeven request, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> AdelantoCuotaCartera(RequestCarteraAdelantoCuotaSeven request, CancellationToken cancellationToken);
        Task<Response<ProcesoAplicacionPagosMasivoDto>> InsertarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivoDto procesoAplicacionPagosMasivo);
        Task<Response<ProcesoAplicacionPagosMasivoDto>> ActualizarProcesoAplicacionPagos(ProcesoAplicacionPagosMasivoDto procesoAplicacionPagosMasivo);
        Task<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>> ObtenerProcesoAplicacionPagos(ParametrosObtenerProcesoAplicacionPagosDto procesoAplicacionPagosMasivo);
        Task<Response<IEnumerable<ResultadoAplicacionPagoDto>>> AplicarPagoMasivoDesdeLista(int idProceso, Guid userId, CancellationToken cancellationToken);
        Task<Response<bool>> EliminarProcesoAplicacionPagos(int idProceso);
        Task<Response<bool>> ReversarPagosLista(int request);
        Task<Response<bool>> ReversarPagoUnitarioDeLista(RequestReversionPagoListaDto request);

        Task<Response<ResponsePlanPagoDto>> ObtenerPlanPagosCredito(ObtenerProductoCartera obtenerProductoCartera);
        Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> AnulacionCuotaCartera(RequestAnulacionCarteraSeven request, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> CierreMensualCarteraSeven(ParamCierreMensualCarteraSevenDto param, CancellationToken cancellationToken);
        Task<Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>> ObtenerTiposCausalesAceleracionCartera();
        Task<Response<SaldoObligacionDto>> ObtenerSaldoObligacion(Guid IdCredito);
        Task<Response<AplicacionPagoDto>> AplicarPagoNovedadCartera(RequestPagoNovedadDto request);
        Task<Response<IEnumerable<ReporteCierreAnualCarteraDto>>> CierreAnualCartera(RequestCierreAnualCartera request);
        Task<Response<IEnumerable<ConsultaSaldosResponse>>> ConsultaSaldosYCartera(ConsultaSaldosRequest request);
        Task<Response<InsertarMovContableResponse>> CierreAnualCarteraSeven(RequestCierreAnualCartera request, CancellationToken cancellationToken);
        Task<Response<object>> CrearProdutoClienteCartera(InsertProductoClienteDto request, CancellationToken cancellationToken);
        Task<Response<object>> ActualizarProdutoClienteCartera(RequestActualizacionProducto request, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> AplicarPagoMasivoMovContable(RequestPagoMasivoSevenDto Proceso, CancellationToken cancellationToken);
        Task<Response<List<InsertPlanPagosDto>>> CrearProductoClientePlanPago(InsertProductoClienteDto request, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> AplicarPagoMasivoMovContableTotalizado(DateTime fechaAplicacion, CancellationToken cancellationToken);
        Task<Response<AnularCnMcontResponse>> AnulacionLista(int papId, CancellationToken cancellationToken);
        Task<Response<ComprobanteCarteraDto>> CrearComprobanteCartera(ComprobanteCarteraDto Request, CancellationToken cancellationToken);
        Task<Response<MovimientoComprobanteDto>> CrearMovimientoComprobanteCartera(MovimientoComprobanteDto Request, CancellationToken cancellationToken);
        Task<Response<List<MovimientoComprobanteDto>>> ActualizaMovimientoComprobanteCartera(MovimientosComprobanteRequest request, CancellationToken cancellationToken);
        Task<Response<ComprobanteCarteraDto>> ObtenerComprobanteCartera(ObtenerComprobanteDto Request, CancellationToken cancellationToken);
        Task<Response<object>> GenerarCausacionesComprobante(MovimientoReportesCartera movimientoReportes, CancellationToken cancellationToken = default);
        Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> ObtenerComprobanteCarteraPorProceso(ObtenerComprobantePorProcesoDto Request, CancellationToken cancellationToken);
        Task<Response<List<CausacionDiariaInteresesCorrientesDto>>> ConstruirMovimientosContables(RequestCausacionDiariaInteresesCorrientesDto Request, CancellationToken cancellationToken);
        Task<Response<List<ComprobanteCarteraPorProcesoDTO>>> GenerarComprobanteSevenPorProceso(GenerarComprobanteSevenPorProcesoDto Request, CancellationToken cancellationToken);
        Task<Response<List<MovimientoComprobantePorProcesoDto>>> ObtenerMovimientosContableCartera(MovimientosContableParametersDto Request, CancellationToken cancellationToken);
        Task<FileResult> GenerarExcelComprobanteDetalle(Guid Request, CancellationToken cancellationToken);
        Task<Response<Guid>> CrearRestitucionInmueble(RestitucionInmuebleCarteraDto request);
        Task<Response<IEnumerable<RestitucionInmuebleCarteraDto>>> ObtenerRestitucionInmuebleCartera(Guid idCredito);
        Task<Response<CreditoCarteraProducto>> ObtenerProductoCreditoCartera(string numeroDocumento);
        Task<Response<object>> GenerarBalanceCartera(BalanceCarteraDto request, CancellationToken cancellationToken);
        Task<FileResult> GenerarAuxiliarContableCartera(BalanceCarteraDto Request, CancellationToken cancellationToken);

        Task<Response<List<AbonoHistoricoDto>>> ObtenerEstadoCuotasCredito(string nroCredito, int tipoCredito);
        Task<Response<string>> ActualizarPlanPagosMasivo(ActualizarPlanPagoRequestDto request);
        Task<Response<string>> UpdatePlanPagoHipotecario(ActualizacionPlanPago request, int tipoCredito, DateTime fechaCorte);
        Task<Response<string>> RecalcularPlanPagoHipotecario(ActualizacionPlanPago request, int tipoCredito, DateTime fechaCorte);
        Task<Dictionary<Guid, IEnumerable<NovedadCartera>>> ObtenerNovedadesEnBatch(List<Guid> creditoIds);
        Task<Dictionary<string, List<ProductoClienteDto>>> ObtenerProductosEnBatch(List<string> identificaciones);
        Task<Dictionary<(long NumeroCredito, int TipoCredito), PlanPagoDto>> ObtenerPlanPagoEnBatch(
            List<(long NumeroCredito, int TipoCredito)> creditos,
            DateTime fechaPeriodo);
        Task ProcesarProvisiones( List<ReporteCierreMensualCarteraDto> response,
            Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion);
        Task ProcesarNovedadesYLeasing(
           List<ReporteCierreMensualCarteraDto> response,
           Dictionary<Guid, IEnumerable<NovedadCartera>> novedadesPorCredito,
           Dictionary<string, List<ProductoClienteDto>> productosPorIdentificacion);
        Task<Response<CrearComprobanteReversionDto>> CrearComprobanteReversionYEnviarASevenAsync(
            int ccIdOriginal);
    }
}