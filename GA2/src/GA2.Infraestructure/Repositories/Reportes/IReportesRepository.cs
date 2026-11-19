using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Reportes;
using GA2.Domain.Entities.Reportes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IReportesRepository
    {
        Task<IEnumerable<ReporteSFCF416>> ReporteSFCF416(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<Resumen477Dto>> ReporteSFCF477(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<Resumen536Dto>> ReporteSFC536(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<ReporteSFCF477>> ReportePlanoSFCF477(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<ReporteSFCF536>> ReportePlanoSFCF536(ParametrosSuperfinancieraReportesDto parametros);
        
        Task<IEnumerable<ReporteSFCF477>> ReportePlanoSFCF507(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<ReporteSFCF414>> ReporteSFCF414(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<ReporteSFCF453Y454>> ReporteSFCF453Y454(ParametrosSuperfinancieraReportesDto parametro);

        Task<IEnumerable<ReporteCartera>> ReporteCartera();

        Task<IEnumerable<InformacionMovimientosCartera>> InformacionMovimientosCartera(string identificacion);

        Task<IEnumerable<InformacionMovimientosCarteraDetalle>> InformacionMovimientosCarteraDetalle(string identificacion);

        Task<IEnumerable<ReporteAseguradora>> ReporteAseguradora();
        
        Task<IEnumerable<ReporteAvaluos>> ReporteAvaluos();
        
        Task<IEnumerable<ReporteCargaAnalista>> ReporteCargaAnalista();
        
        Task<IEnumerable<ReporteCesiones>> ReporteCesiones();
        
        Task<IEnumerable<ReporteConsultasRestrictivas>> ReporteConsultasRestrictivas();
        
        Task<IEnumerable<ReporteEstudioTitulos>> ReporteEstudioTitulos();
        
        Task<IEnumerable<ReporteValorVenta>> ReporteValorVenta();
        
        Task<IEnumerable<ReporteDeMora>> ReporteDeMora();
        
        Task<IEnumerable<ReporteGeneralCartera>> ReporteGeneralCartera();
        
        Task<IEnumerable<ReportePreliminarCartera>> ReportePreliminarCartera();
        
        Task<IEnumerable<ReporteGeneralGarantias>> ReporteGeneralGarantias(ReporteGeneralGarantiaParametersDto parametros);
        
        Task<IEnumerable<ReporteSeguroHogar>> ReporteSeguroHogar();
        
        Task<IEnumerable<ReporteSeguroVida>> ReporteSeguroVida();
        
        Task<IEnumerable<ReporteDescuentosNomina>> ReporteDescuentosNomina();
        
        Task<IEnumerable<ProyeccionAnualExtractoAnual>> InformacionProyeccionExtractoAnual(string identificacion);
        
        Task<IEnumerable<RelacionPagosExtractoAnual>> InformacionRelacionPagosExtractoAnual(string identificacion);
        
        Task<IEnumerable<InformacionCuotasExtractoMensual>> InformacionCuotasExtractoMensual(string identificacion);
        
        Task<IEnumerable<InformacionValoresExtractoMensual>> InformacionValoresExtractoMensual(string identificacion);
        
        Task<IEnumerable<InformacionDiscriminadaExtractoMensual>> InformacionDiscriminadaExtractoMensual(string identificacion);

        Task<IEnumerable<InformacionMontosCertificadoIYR>> InformacionMontosCertificadoIYR(string identificacion);

        Task<IEnumerable<InformacionObligacionEstadoCuenta>> InformacionObligacionEstadoCuenta(string identificacion);
        
        Task<IEnumerable<InformacionDistribucionValoresEstadoCuenta>> InformacionDistribucionValoresEstadoCuenta(string identificacion);

        Task<IEnumerable<ObtenerRespuestaEmbargo>> ObtenerRespuestaEmbargo(Guid idEmbargo);

        Task<IEnumerable<CargarListadoReportes>> CargarListadoReportes();

        Task<IEnumerable<ReportePreliminarGarantias>> ReportePreliminarGarantias();

        Task<IEnumerable<TransferenciaCesantias>> ObtenerDatosOficioRespuesta(int idAfiliacion);

        Task<IEnumerable<OficiosAfiliacion>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros);

        Task<IEnumerable<ReporteCierreMensualCartera>> ReporteCierreMensualCartera(ParamReporteCierreMensualCarteraDto parametros);
        
        Task<IEnumerable<ReporteResultadoIntegracion>> ReporteResultadoIntegracion(RequestReporteResultadoIntegracionDto parametros);

        Task<IEnumerable<ReporteSFCF341>> ReporteSFCF341(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<DocumentoPorPeriodo>> AfiliadosPorPeriodo(ParametrosSuperfinancieraReportesDto parametros);

        Task<IEnumerable<Datos_341>> ReportePlano341(ParametrosReportePlano341 parametros);

        Task<IEnumerable<Settings>> ConsultaReportesGenerales();

        Task<IEnumerable<ReporteGeneralMora>> ConsultaReporteGeneralMora();

        Task<IEnumerable<Unidad_Ejecutora>> ConsultaUnidadEjecutora();
        Task<IEnumerable<ConfiguracionColumna>> ConsultarColumnasPorUnidadEjecutora(int idUnidadEjecutora, int esReporteDeAltas);
        Task<InformacionExtractoMensual> ObtenerInformacionExtractoMensual(ReporteExtractoParametersDto parametros);
        Task<IEnumerable<InformacionContable>> ReporteInformacionContable(ReporteInformacionContableParametersDto parametros);
        Task<IEnumerable<EquivalenciaCuentasContableSeven>> ObtenerEquivalenciaCuentasContables();
        Task<IEnumerable<InformacionProyecionExtractoAnual>> ObtenerInformacionProyeccionExtractoAnual(ReporteExtractoParametersDto parametros);
        Task<IEnumerable<InformacionPagosRealizadosExtractoAnual>> ObtenerInformacionRelacionPagosExtractoAnual(ReporteExtractoParametersDto parametros);
        Task<IEnumerable<ReporteSFCF454>> ReporteSFCF454();
        Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCartera(ReporteGeneralCarteraParametersDto parametros);
        Task<ReportePazSalvo> GenerarReportePazSalvo(ReportePazSalvoParemetersDto parametros);
        Task<IEnumerable<InformacionCredito>> ConsultarCreditosParaGeneracionExtractoAnual(ReporteExtractoMasivoParametersDto parametros);
        Task<IEnumerable<InformacionCredito>> ConsultarCreditosParaGeneracionExtractoMensual(ReporteExtractoMasivoParametersDto parametros);
        Task<ProcesoGeneracionReporte> InsertarProcesoGeneracionReporte(ProcesoGeneracionReporteDto proceso);
        Task<ProcesoGeneracionReporte> ActualizarEstadoProcesoGeneracionReporte(ActualizacionEstadoProcesoGeneracionReporteDto proceso);
        Task<IEnumerable<ProcesoGeneracionReporte>> ConsultarProcesoGeneracionReporte(ConsultaProcesoGeneracionReporteDto proceso);
        Task<IAsyncEnumerable<ReporteContable>> GenerarReporteCausaciones(ReporteContableParametersDto parametros);
        Task<IEnumerable<TotalReporteContable>> GenerarReporteTotalCausaciones(ReporteContableParametersDto parametros);
        Task<IEnumerable<CarteraReporteGeneralMora>> GenerarReporteGeneralMora(ReporteGeneralMoraParametersDto parametros);
        Task<IEnumerable<ReporteAnioGravable>> GenerarReporteRetefuenteAnioGravable(ReporteAnioGravableParametersDto parametros);
        Task<IEnumerable<ReporteContable>> GenerarReporteContablePagos(ReporteContableParametersDto parametros);
        Task<IEnumerable<ReportePago>> GenerarReportePagos(ReportePagosParametersDto parametros);
        Task<IEnumerable<ReporteAPNRCAR>> GenerarReporteAPNRCAR(ReporteAPNRCARParametersDto parametros);
        Task<IEnumerable<ReporteAPNRCAR>> GenerarReporteAPNRCARPorAfiliado(ReporteAPNRCARParametersDto parametros);
        IAsyncEnumerable<ReporteContable> GenerarReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametros);
        Task<IEnumerable<TotalReporteCierreMensualContable>> GenerarReporteTotalCierreMensualContable(ReporteContableCierreMensualParametersDto parametros);
        Task<IEnumerable<ReporteEstadoCuentaAfiliado>> GenerarReporteEstadoCuentaAfiliado(ReporteEstadoCuentaParametersDto parametros);
        Task<IEnumerable<ReporteEstadoCuentaPlanPago>> GenerarReporteEstadoCuentaPlanPagos(Guid IdCredito);
        Task<IEnumerable<ReporteEstadoMovimientos>> GenerarReporteEstadoMovimientos(ReporteEstadoMovimientosParametersDto parametros);

        Task<IEnumerable<ReporteMensualSeguros>> GenerarReporteMensualSeguros(ReporteMensualSegurosParametersDto parametros);
        Task<IEnumerable<ReporteCapitalCorriente>> GenerarReporteCapitalCorriente(ReporteCapitalCorrienteParametersDto parametros);
        Task<IEnumerable<ReporteOpcioCompra>> GenerarReporteOpcionCompra(ReporteOpcionCompraParametersDto parametros);
        Task<IEnumerable<ReporteRCreditoDatosGenerales>> GenerarDatosGeneralesReporteRCredito(ReporteRCreditoParametersDto parametros);
        Task<IEnumerable<ReporteRCreditoInformacionLibranza>> GenerarInformacionLibranzaReporteRCredito(ReporteRCreditoParametersDto parametros);
        Task<EncabezadoArchivosNomina> ConsultarArchivoEncabezadoNomina(int dtc_id);
        Task<IEnumerable<CuerpoArchivosNomina>> ConsultarArchivoCuerpoNomina(int dtc_id);
        Task<FinArchivosNomina> ConsultarArchivoFinNomina(int dtc_id);
        Task<bool> GuardarDocumentoAdicionalCargado(DocumentosAdicionalesCargados documentos);
        Task<DocumentosAdicionalesCargados> ConsultarDocumentosAdicionalesCargados(string documento);
        Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraNew(ReporteGeneralCarteraParametersDto parametros);
        Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraOptimizado(ReporteGeneralCarteraParametersDto parametros);
        Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraReestructurado(ReporteGeneralCarteraParametersDto parametros);
        Task<IEnumerable<DocumentosAdicionalesCargados>> ConsultarDocumentosAdicionalesCargadosReclasificacion(string documento);
    }
}