using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.TU;
using GA2.Application.Dto.Haberes;
using GA2.Application.Dto.Prospeccion;
using GA2.Application.Dto.Reportes;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Entities.Cartera.Reportes;
using GA2.Domain.Entities.Reportes;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IReportesBusinessLogic
    {
       
        // Cargar datos en la Pagina
        Task<Response<IEnumerable<CargarListadoReportesDto>>> CargarListadoReportes();

        // PDF Reports
        Task<Response<List<ReporteSFCF416Dto>>> ReporteSFCF416(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReporteSFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);
        
        Task<FileResult> ReportePlanoSFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReportePlanoSFCF416(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<Response<List<Resumen477Dto>>> ReporteFCF477(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<Response<List<Resumen536Dto>>> ReporteSFC536(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<Response<List<ReporteSFCF453Y454>>> ReporteFCF453Y454(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<Response<List<ReporteSFCF414Dto>>> ReporteFCF414(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<Response<List<DocumentoPorPeriodoDto>>> ReporteFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReportePlanoSFCF477(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReportePlanoSFCF536(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReportePlanoSFCF507(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReportePlanoSFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReporteSFCF341(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReporteSFCF453Y454(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReporteSFCF343(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);

        Task<FileResult> ReporteEntidades(EntidadEducativaDto entidad);
        Task<FileResult> CertificadoIngresosRetenciones(string identificacion);
        
        Task<FileResult> CertificadoHaberes(string identificacion);
        
        Task<FileResult> EstadoDeCuenta(string identificacion);
        
        Task<FileResult> MovimientoDeCartera(string identificacion);
        
        Task<FileResult> ObtenerRespuestaEmbargo(Guid idEmbargo);

        Task<FileResult> ObtenerRespuestaSolicitudDePago(ConsultarSolicitudDto request);

        Task<FileResult> ObtenerRespuestaSolicitudInconsistente(ConsultarSolicitudDto request);

        Task<FileResult> ObtenerRespuestaSolicitud(ConsultarSolicitudDto idSolicitud);

        Task<FileResult> TransferenciaCesantiasPONAL(int idAfiliacion);

        Task<FileResult> TransferenciaCesantias(int idAfiliacion);

        Task<FileResult> RespuestaNoProcedente(int idAfiliacion);

        Task<FileResult> PreaprobadoLeasing(string identificacion);

        Task<FileResult> PreaprobadoHipotecario(string identificacion);

        Task<FileResult> PreaprobadoEducativo(string identificacion); 

        Task<FileResult> AfiliacionExtemporaneaProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> AfiliacionAdminCesantiasProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> AfiliacionVoluntariaProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> AfiliacionVoluntariaVeteranosProcedenteCB(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> AfiliacionVoluntariaVeteranosCredito(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> AfiliacionVoluntariaVeteranosProcedenteAD(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> BeneficiariosProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> Improcedentes(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> NuevaVinculacionProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> PensionRetiroProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> DesafiliacionVoluntariaProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> ReincorporacionProcedente(ParametrosReportesAfiliacionesDto parametros);

        Task<FileResult> ReintegroProcedente(ParametrosReportesAfiliacionesDto parametros);

        // Excel Reports
        Task<Response<IEnumerable<ReporteCarteraDto>>> ReporteCartera();
        
        Task<Response<IEnumerable<ReporteAseguradoraDto>>> ReporteAseguradora();
        
        Task<Response<IEnumerable<ReporteAvaluosDto>>> ReporteAvaluos();
        
        Task<Response<IEnumerable<ReporteCargaAnalistaDto>>> ReporteCargaAnalista();
        
        Task<Response<IEnumerable<ReporteCesionesDto>>> ReporteCesiones();
        
        Task<Response<IEnumerable<ReporteConsultasRestrictivasDto>>> ReporteConsultasRestrictivas();
        
        Task<Response<IEnumerable<ReporteEstudioTitulosDto>>> ReporteEstudioTitulos();
        
        Task<Response<IEnumerable<ReporteValorVentaDto>>> ReporteValorVenta();
        
        Task<Response<IEnumerable<ReporteDeMoraDto>>> ReporteDeMora();
        
        Task<Response<IEnumerable<ReporteGeneralCarteraDto>>> ReporteGeneralCartera();
        
        Task<Response<IEnumerable<ReportePreliminarCarteraDto>>> ReportePreliminarCartera();

        Task<Response<IEnumerable<ReporteGeneralGarantiasDto>>> ReporteGeneralGarantias(ReporteGeneralGarantiaParametersDto parametersDto);
        
        Task<Response<IEnumerable<ReporteSeguroHogarDto>>> ReporteSeguroHogar();
        
        Task<Response<IEnumerable<ReporteDescuentosNominaDto>>> ReporteDescuentosNomina();
        
        Task<Response<IEnumerable<ReporteSeguroVidaDto>>> ReporteSeguroVida();
      
        Task<Response<IEnumerable<ReportePreliminarGarantiasDto>>> ReportePreliminarGarantias();

        Task<Response<IEnumerable<ReporteCierreMensualCarteraDto>>> ReporteCierreMensualCartera(ParamReporteCierreMensualCarteraDto request);
        
        Task<Response<IEnumerable<ReporteResultadoIntegracionDto>>> ReporteResultadoIntegracion(RequestReporteResultadoIntegracionDto request);

        Task<Response<IEnumerable<SettingsDto>>> ConsultaReportesGenerales();

        Task<Response<IEnumerable<ReporteGeneralMoralDto>>> ReporteGeneralMora();

        Task<Response<byte[]>> ReporteCorreosErrados(ParamCorreosErrados paramCorreosErrados);
        Task<FileResult> ReporteTransUnion(string NumeroIdentificacion);
        Task<Response<SimulacionCreditoArchivo>> ReporteTransUnionPropiedades(string xml);
        Task<Response<SimulacionCreditoArchivo>> ReporteSimulacionCredito(List<SimulacionDto> data, int tipoDocumento);
        Task<string> GenerarTxt(string Line);
        Task<string> GenerarExcelStyle(ReportRequestExcelDto request);
        Task<string> GenerarExcel(DataTable data, string nameReport);
        Task<Response<SimulacionCreditoArchivo>> ReporteSimulacion60PorCiento(ReporteSimulacion60PorCientoDto data);
        Task<FileResult> ReportePlanillaPagoIndiCesantias(ConsultarSolicitudDto data);
        Task<FileResult> ReportePlanoSFCF414_V2(ParametrosSuperfinancieraReportesDto parametrosAplicacionPago);
        Task<FileResult> ReporteSolicitudTransUnion(ProspeccionTransUnionRequestDto req);

        Task<FileResult> GenerarReporteExtractoMensual(ReporteExtractoParametersDto parametersDto);
        Task<FileResult> ReporteInformeCredito(string documento);
        Task<Response<IEnumerable<InformacionContableDto>>> ObtenerReporteInformacionContable(ReporteInformacionContableParametersDto parametersDto);
        Task<FileResult> GenerarExcelReporteInformacionContable(ReporteInformacionContableParametersDto parametersDto);
        Task<Response<IEnumerable<EquivalenciaCuentasContableSevenDto>>> ObtenerEquivalenciaCuentasContables();
        Task<FileResult> GenerarReporteExtractoAnual(ReporteExtractoParametersDto parametersDto);
        Task<FileResult> ReporteResumenFinanciero(ReporteResumenFinancieroDto reporteResumenFinanciero);
        Task<FileResult> ReporteSFCF454();
        Task<Response<IEnumerable<CarteraReporteGeneralDto>>> GenerarReporteGeneralCartera(ReporteGeneralCarteraParametersDto parametros);
        Task<Response<IEnumerable<CarteraReporteGeneralDto>>> GenerarReporteGeneralCarteraReestructurado(ReporteGeneralCarteraParametersDto parametros);
        Task<Response<FileResult>> GenerarReportePazSalvo(ReportePazSalvoParemetersDto parameters);
        Task<Response<ProcesoGeneracionReporteDto>> CrearProcesoGeneracionReporteExtractoMasivo(ReporteExtractoMasivoParametersDto parametersDto);
        Task<Response<ProcesoGeneracionReporteDto>> CrearProcesoGeneracionReporteAnioGravable(ReporteAnioGravableParametersDto parametersDto);
        Task<FileResult> GenerarReporteExtractoMasivo(ReporteExtractoMasivoParametersDto parametersDto, CancellationToken cancellationToken = default);
        Task<FileResult> GenerarReporteMasivoRetefuente(ReporteAnioGravableParametersDto parameters, CancellationToken cancellationToken = default);
        Task<Response<IEnumerable<ProcesoGeneracionReporteDto>>> ConsultarProcesosGeneracionReportePorUsuario(Guid idUsuario, ReporteProcesoGeneracionReporteParametersDto tipoReporte);
        Task<Response<Stream>> DescargarReporteMasivo(Guid idProceso);
        Task<Response<ProcesoGeneracionReporteDto>> CancelarProcesoGeneracionReporte(Guid idProceso);
        Task<Response<ProcesoGeneracionReporteDto>> EliminarProcesoGeneracionReporte(Guid idProceso);
        Task<Response<ProcesoGeneracionReporteDto>> ConsultarProcesoGeneracionReporte(Guid idUsuario, Guid idProceso);
        Task<Response<ResultadoReportContable>> GenerarReporteCausaciones(ReporteContableParametersDto parametersDto);
        Task<FileResult> GenerarExcelReporteCausaciones(ReporteContableParametersDto parametersDto);
        Task<Response<IEnumerable<CarteraReporteGeneralMoraDto>>> GenerarReporteGeneralMora(ReporteGeneralMoraParametersDto parametros);
        Task<Response<IEnumerable<ReporteContableDto>>> GenerarReporteContablePagos(ReporteContableParametersDto parametersDto);
        Task<FileResult> GenerarExcelReporteContablePagos(ReporteContableParametersDto parametersDto);
        Task<Response<ResultadoReportePagos>> GenerarReportePagos(ReportePagosParametersDto parametersDto);
        Task<FileResult> GenerarExcelReportePagos(ReportePagosParametersDto parametersDto);
        Task<Response<ResumenReporteContable>> GenerarReporteTotalCausaciones(ReporteContableParametersDto parametersDto);
        Task<Response<FileResult>> GenerarReporteRetefuente(ReporteAnioGravableParametersDto parameters);
        Task<Response<IEnumerable<ReporteAPNRCARDto>>> GenerarReporteAPNRCAR(ReporteAPNRCARParametersDto parametersDto);
        Task<Response<string>> GenerarPlanPagosHipotecarioPdfBase64Async(RequestProductoClienteDto modelo);
        Task<Response<ResultadoReportContable>> GenerarReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto);
        Task<FileResult> GenerarExcelReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto);
        Task<Response<IEnumerable<TotalReporteCierreMensualContableDto>>> GenerarReporteTotalCierreMensualContable(ReporteContableCierreMensualParametersDto parametersDto);
        Task<Response<IEnumerable<ReporteEstadoCuentaAfiliadoDto>>> GenerarReporteEstadoCuentaAfiliado(ReporteEstadoCuentaParametersDto parametersDto);
        Task<Response<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>> GenerarReporteEstadoCuentaPlanPagos(Guid IdCredito);
        Task<Response<IEnumerable<ReporteEstadoMovimientosDto>>> GenerarReporteEstadoMovimientos(ReporteEstadoMovimientosParametersDto parametersDto);
        Task<Response<IEnumerable<MovimientoComprobanteEntity>>> GenerarCausacionesComprobante(MovimientoReportesCartera request); 
        Task<Response<IEnumerable<ReporteMensualSegurosDto>>> GenerarReporteMensualSeguros(ReporteMensualSegurosParametersDto parametersDto);
        Task<Response<IEnumerable<ReporteCapitalCorrienteDto>>> GenerarReporteCapitalCorriente(ReporteCapitalCorrienteParametersDto parametersDto);
        Task<Response<IEnumerable<ReporteOpcionCompraDto>>> GenerarReporteOpcionCompra(ReporteOpcionCompraParametersDto parametersDto);
        Task<Response<FileResult>> GenerarReporteRCredito(ReporteRCreditoParametersDto parametersDto);
        Task<FileContentResult> ReporteTransUnionPropiedadesInformacionComercial(DatosPersonalesProspeccionDto request);
        Task<FileResult> ReporteProspeccionReclasificacion(IdentificacionProspeccion data);
        Task<FileResult> ReporteSolicitudCreditoVivienda(PeticionCreditoBasicaDto peticionCredito);
        Task<FileResult> FacturaSevenCredito(Guid SOC_ID);
        Task<Response<object>> GenerarReporteFondoSolidaridad(ReportFondoSolidaridadRequetsDto parametersDto);
        Task<FileResult> ReporteProspeccionCartaRechazo(IdentificacionProspeccion data);
        decimal ValorCompraCartera(decimal valor);
        Task<Response<byte[]>> GenerarPlanesPagoMasivoZipBase64Async(RequestPlanPagoGeneradoDto request);
        Task<Response<byte[]>> GenerarPlanesPagoMasivoAnualZipBase64Async(RequestPlanPagoGeneradoDto request);
        Task<ReporteGeneradoDto> ReporteApruebaActividadTramite(IdentificacionProspeccion data); 
    }
}