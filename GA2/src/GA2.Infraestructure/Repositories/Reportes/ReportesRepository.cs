
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Comprobantes;
using GA2.Domain.Entities.Cartera.Reportes;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Reportes;
using GA2.Infraestructure.Data.Reportes.Cartera;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ReportesRepository : DapperGenericRepository, IReportesRepository
    {
        public ReportesRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<ReporteCartera>> ReporteCartera()
        {
            return await GetAsyncList<ReporteCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF416>> ReporteSFCF416(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<ReporteSFCF416>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Resumen477Dto>> ReporteSFCF477(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FECHA_APLICACION", parametros.FechaAplicacion);

            return await GetAsyncList<Resumen477Dto>(
                "ReporteSFC477",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF477>> ReportePlanoSFCF477(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FECHA_APLICACION", parametros.FechaAplicacion);

            return await GetAsyncList<ReporteSFCF477>(
                "ReporteTXTSFC477",
                parameters,
                CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ReporteSFCF536>> ReportePlanoSFCF536(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FECHA_APLICACION", parametros.FechaAplicacion);

            return await GetAsyncList<ReporteSFCF536>(
                "ReporteTXTSFC536",
                parameters,
                CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Resumen536Dto>> ReporteSFC536(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FECHA_APLICACION", parametros.FechaAplicacion);

            return await GetAsyncList<Resumen536Dto>(
                "ReporteSFC536",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF477>> ReportePlanoSFCF507(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FECHA_APLICACION", parametros.FechaAplicacion);

            return await GetAsyncList<ReporteSFCF477>(
                "ReporteSFC507",
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF414>> ReporteSFCF414(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<ReporteSFCF414>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ReporteAseguradora>> ReporteAseguradora()
        {
            return await GetAsyncList<ReporteAseguradora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteAvaluos>> ReporteAvaluos()
        {
            return await GetAsyncList<ReporteAvaluos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCargaAnalista>> ReporteCargaAnalista()
        {
            return await GetAsyncList<ReporteCargaAnalista>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCesiones>> ReporteCesiones()
        {
            return await GetAsyncList<ReporteCesiones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteConsultasRestrictivas>> ReporteConsultasRestrictivas()
        {
            return await GetAsyncList<ReporteConsultasRestrictivas>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteEstudioTitulos>> ReporteEstudioTitulos()
        {
            return await GetAsyncList<ReporteEstudioTitulos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteValorVenta>> ReporteValorVenta()
        {
            return await GetAsyncList<ReporteValorVenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteDeMora>> ReporteDeMora()
        {
            return await GetAsyncList<ReporteDeMora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteGeneralCartera>> ReporteGeneralCartera()
        {
            return await GetAsyncList<ReporteGeneralCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteGeneralGarantias>> ReporteGeneralGarantias(ReporteGeneralGarantiaParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteGeneralGarantia.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteGeneralGarantia.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteGeneralGarantia.TipoCredito.GetDescription(), parametros.TipoCredito);
            parameters.Add("CLI_IDENTIFICACION", parametros.Identificacion);
            parameters.Add("NRO_CREDITO", parametros.NumeroCredito);

            return await GetAsyncList<ReporteGeneralGarantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteRCreditoDatosGenerales>> GenerarDatosGeneralesReporteRCredito(ReporteRCreditoParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteRCredito.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteRCredito.Mes.GetDescription(), parametros.Mes);

            return await GetAsyncList<ReporteRCreditoDatosGenerales>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteRCreditoInformacionLibranza>> GenerarInformacionLibranzaReporteRCredito(ReporteRCreditoParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteRCredito.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteRCredito.Mes.GetDescription(), parametros.Mes);

            return await GetAsyncList<ReporteRCreditoInformacionLibranza>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteMensualSeguros>> GenerarReporteMensualSeguros(ReporteMensualSegurosParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteMensualSeguros.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteMensualSeguros.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteMensualSeguros.TipoCredito.GetDescription(), parametros.TipoCredito);

            return await GetAsyncList<ReporteMensualSeguros>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCapitalCorriente>> GenerarReporteCapitalCorriente(ReporteCapitalCorrienteParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteCapitalCorriente.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteCapitalCorriente.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteCapitalCorriente.TipoCredito.GetDescription(), parametros.TipoCredito);

            return await GetAsyncList<ReporteCapitalCorriente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteOpcioCompra>> GenerarReporteOpcionCompra(ReporteOpcionCompraParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteOpcionCompra.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteOpcionCompra.Mes.GetDescription(), parametros.Mes);

            return await GetAsyncList<ReporteOpcioCompra>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReportePreliminarCartera>> ReportePreliminarCartera()
        {
            return await GetAsyncList<ReportePreliminarCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSeguroHogar>> ReporteSeguroHogar()
        {
            return await GetAsyncList<ReporteSeguroHogar>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSeguroVida>> ReporteSeguroVida()
        {
            return await GetAsyncList<ReporteSeguroVida>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteDescuentosNomina>> ReporteDescuentosNomina()
        {
            return await GetAsyncList<ReporteDescuentosNomina>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionFinancieraExtractoAnual>> InformacionFinancieraExtractoAnual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionFinancieraExtractoAnual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProyeccionAnualExtractoAnual>> InformacionProyeccionExtractoAnual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<ProyeccionAnualExtractoAnual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RelacionPagosExtractoAnual>> InformacionRelacionPagosExtractoAnual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<RelacionPagosExtractoAnual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionCuotasExtractoMensual>> InformacionCuotasExtractoMensual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionCuotasExtractoMensual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<InformacionExtractoMensual> ObtenerInformacionExtractoMensual(ReporteExtractoParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Mes), parametros.Mes ?? 1);

            return await GetAsyncFirst<InformacionExtractoMensual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionProyecionExtractoAnual>> ObtenerInformacionProyeccionExtractoAnual(ReporteExtractoParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Anio), parametros.Anio);

            return await GetAsyncList<InformacionProyecionExtractoAnual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionCredito>> ConsultarCreditosParaGeneracionExtractoMensual(ReporteExtractoMasivoParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.UnidadEjecutora), parametros.UnidadEjecutora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Mes), parametros.Mes);

            return await GetAsyncList<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionCredito>> ConsultarCreditosParaGeneracionExtractoAnual(ReporteExtractoMasivoParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.UnidadEjecutora), parametros.UnidadEjecutora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Anio), parametros.Anio);

            return await GetAsyncList<InformacionCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionPagosRealizadosExtractoAnual>> ObtenerInformacionRelacionPagosExtractoAnual(ReporteExtractoParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsObtenerInformacionExtracto.Anio), parametros.Anio);

            return await GetAsyncList<InformacionPagosRealizadosExtractoAnual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<ProcesoGeneracionReporte> InsertarProcesoGeneracionReporte(ProcesoGeneracionReporteDto proceso)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsProcesoGeneracionReporte.IdProceso.GetDescription(), proceso.IdProceso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.TipoReporte.GetDescription(), proceso.TipoReporte);
            parameters.Add(EnumParamsProcesoGeneracionReporte.NombreReporte.GetDescription(), proceso.NombreReporte);
            parameters.Add(EnumParamsProcesoGeneracionReporte.Estado.GetDescription(), proceso.Estado);
            parameters.Add(EnumParamsProcesoGeneracionReporte.Progreso.GetDescription(), proceso.Progreso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.BlobContenedor.GetDescription(), proceso.BlobContenedor);
            parameters.Add(EnumParamsProcesoGeneracionReporte.BlobRutaAcceso.GetDescription(), proceso.BlobRutaAcceso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.CreadoPor.GetDescription(), proceso.CreadoPor);
            parameters.Add(EnumParamsProcesoGeneracionReporte.DetalleProceso.GetDescription(), proceso.DetalleProceso);

            return await GetAsyncFirst<ProcesoGeneracionReporte>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<ProcesoGeneracionReporte> ActualizarEstadoProcesoGeneracionReporte(ActualizacionEstadoProcesoGeneracionReporteDto proceso)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsProcesoGeneracionReporte.IdProceso.GetDescription(), proceso.IdProceso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.Estado.GetDescription(), proceso.Estado);
            parameters.Add(EnumParamsProcesoGeneracionReporte.Progreso.GetDescription(), proceso.Progreso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.DetalleProceso.GetDescription(), proceso.DetalleProceso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.ConcatenarDetalle.GetDescription(), proceso.ConcatenarDetalle);

            return await GetAsyncFirst<ProcesoGeneracionReporte>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProcesoGeneracionReporte>> ConsultarProcesoGeneracionReporte(ConsultaProcesoGeneracionReporteDto proceso)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsProcesoGeneracionReporte.IdProceso.GetDescription(), proceso.IdProceso);
            parameters.Add(EnumParamsProcesoGeneracionReporte.TipoReporte.GetDescription(), proceso.TipoReporte);
            parameters.Add(EnumParamsProcesoGeneracionReporte.NombreReporte.GetDescription(), proceso.NombreReporte);
            parameters.Add(EnumParamsProcesoGeneracionReporte.Estado.GetDescription(), proceso.Estado);
            parameters.Add(EnumParamsProcesoGeneracionReporte.CreadoPor.GetDescription(), proceso.CreadoPor);

            return await GetAsyncList<ProcesoGeneracionReporte>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<InformacionContable>> ReporteInformacionContable(ReporteInformacionContableParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteInformacionContable.FechaInicial), parametros.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteInformacionContable.FechaFinal), parametros.FechaFinal);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteInformacionContable.CuentaContable), 
                string.IsNullOrWhiteSpace(parametros.CuentaContable) ? null : parametros.CuentaContable);

            return await GetAsyncList<InformacionContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IAsyncEnumerable<ReporteContable>> GenerarReporteCausaciones(ReporteContableParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaInicial), parametros.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaFinal), parametros.FechaFinal);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.CantidadRegistros), parametros.RegistrosPorPagina);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.NumeroPagina), parametros.NumeroPagina);

            return GetAsyncSequence<ReporteContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TotalReporteContable>> GenerarReporteTotalCausaciones(ReporteContableParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaInicial), parametros.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaFinal), parametros.FechaFinal);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.TipoCredito), parametros.TipoCredito);

            return await GetAsyncList<TotalReporteContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public IAsyncEnumerable<ReporteContable> GenerarReporteCierreMensualContable(ReporteContableCierreMensualParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.CantidadRegistros), parametros.RegistrosPorPagina);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.NumeroPagina), parametros.NumeroPagina);

            return GetAsyncSequence<ReporteContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TotalReporteCierreMensualContable>> GenerarReporteTotalCierreMensualContable(ReporteContableCierreMensualParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.TipoCredito), parametros.TipoCredito);

            return await GetAsyncList<TotalReporteCierreMensualContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteContable>> GenerarReporteContablePagos(ReporteContableParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaInicial), parametros.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.FechaFinal), parametros.FechaFinal);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteContable.TipoCredito), parametros.TipoCredito);

            return await GetAsyncList<ReporteContable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReportePago>> GenerarReportePagos(ReportePagosParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReportePago.TipoCredito.GetDescription(), parametros.TipoCredito);
            parameters.Add(EnumParamsReportePago.UnidadEjecutora.GetDescription(), parametros.UnidadEjecutora);

            if(parametros.TipoFecha is TipoFecha.FechaAplicacion)
            {
                parameters.Add(EnumParamsReportePago.FechaAplicacionInicial.GetDescription(), parametros.FechaInicial);
                parameters.Add(EnumParamsReportePago.FechaAplicacionFinal.GetDescription(), parametros.FechaFinal);
            }
            else
            {
                parameters.Add(EnumParamsReportePago.FechaPagoInicial.GetDescription(), parametros.FechaInicial);
                parameters.Add(EnumParamsReportePago.FechaPagoFinal.GetDescription(), parametros.FechaFinal);
            }

            return await GetAsyncList<ReportePago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteAPNRCAR>> GenerarReporteAPNRCAR(ReporteAPNRCARParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteAPNRCAR.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteAPNRCAR.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteAPNRCAR.TipoCredito.GetDescription(), parametros.TipoCredito);

            return await GetAsyncList<ReporteAPNRCAR>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ReporteAPNRCAR>> GenerarReporteAPNRCARPorAfiliado(ReporteAPNRCARParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteAPNRCAR.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsReporteAPNRCAR.Mes.GetDescription(), parametros.Mes);
            parameters.Add(EnumParamsReporteAPNRCAR.TipoCredito.GetDescription(), parametros.TipoCredito);
            parameters.Add(EnumParamsReporteAPNRCAR.Identificacion.GetDescription(), parametros.Identificacion);

            return await GetAsyncList<ReporteAPNRCAR>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteEstadoCuentaAfiliado>> GenerarReporteEstadoCuentaAfiliado(ReporteEstadoCuentaParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteEstadoCuenta.TipoCredito.GetDescription(), parametros.TipoCredito);
            parameters.Add(EnumParamsReporteEstadoCuenta.NumeroCredito.GetDescription(), parametros.NumeroCredito);
            parameters.Add(EnumParamsReporteEstadoCuenta.NumeroIdentificacion.GetDescription(), parametros.NumeroIdentificacion);

            return await GetAsyncList<ReporteEstadoCuentaAfiliado>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteEstadoCuentaPlanPago>> GenerarReporteEstadoCuentaPlanPagos(Guid IdCredito)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteEstadoCuenta.IdCredito.GetDescription(), IdCredito);

            return await GetAsyncList<ReporteEstadoCuentaPlanPago>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteEstadoMovimientos>> GenerarReporteEstadoMovimientos(ReporteEstadoMovimientosParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsReporteEstadoMovimientos.IdCredito.GetDescription(), parametros.IdCredito);
            parameters.Add(EnumParamsReporteEstadoMovimientos.FechaInicial.GetDescription(), parametros.FechaInicial);
            parameters.Add(EnumParamsReporteEstadoMovimientos.FechaFinal.GetDescription(), parametros.FechaFinal);

            return await GetAsyncList<ReporteEstadoMovimientos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EquivalenciaCuentasContableSeven>> ObtenerEquivalenciaCuentasContables()
        {
            var parameters = new DynamicParameters();

            return await GetAsyncList<EquivalenciaCuentasContableSeven>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionValoresExtractoMensual>> InformacionValoresExtractoMensual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionValoresExtractoMensual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionDiscriminadaExtractoMensual>> InformacionDiscriminadaExtractoMensual(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionDiscriminadaExtractoMensual>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionMontosCertificadoIYR>> InformacionMontosCertificadoIYR(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionMontosCertificadoIYR>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionBasicaCertificadoHaberes>> InformacionBasicaCertificadoHaberes(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionBasicaCertificadoHaberes>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionBasicaEstadoCuenta>> InformacionBasicaEstadoCuenta(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionBasicaEstadoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionObligacionEstadoCuenta>> InformacionObligacionEstadoCuenta(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionObligacionEstadoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionDistribucionValoresEstadoCuenta>> InformacionDistribucionValoresEstadoCuenta(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionDistribucionValoresEstadoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerRespuestaEmbargo>> ObtenerRespuestaEmbargo(Guid idEmbargo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.IdEmbargo), idEmbargo);

            return await GetAsyncList<ObtenerRespuestaEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CargarListadoReportes>> CargarListadoReportes()
        {
            return await GetAsyncList<CargarListadoReportes>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
               null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionMovimientosCartera>> InformacionMovimientosCartera(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionMovimientosCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionMovimientosCarteraDetalle>> InformacionMovimientosCarteraDetalle(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), identificacion);

            return await GetAsyncList<InformacionMovimientosCarteraDetalle>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReportePreliminarGarantias>> ReportePreliminarGarantias()
        {
            return await GetAsyncList<ReportePreliminarGarantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }
       
        public async Task<IEnumerable<TransferenciaCesantias>> ObtenerDatosOficioRespuesta(int idAfiliacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.IdAfiliacion), idAfiliacion);

            return await GetAsyncList<TransferenciaCesantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OficiosAfiliacion>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), parametros.IdAfiliacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.OpcionReporte), parametros.OpcionReporte);

            return await GetAsyncList<OficiosAfiliacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCierreMensualCartera>> ReporteCierreMensualCartera(ParamReporteCierreMensualCarteraDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.AnioCierre), parametros.AnioCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.MesCierre), parametros.MesCierre);

            return await GetAsyncList<ReporteCierreMensualCartera>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteResultadoIntegracion>> ReporteResultadoIntegracion( RequestReporteResultadoIntegracionDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesResultadoIntegracionParams.PIN_NRO_IDENTIFICACION), parametros.NumeroDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesResultadoIntegracionParams.FECHAINI), parametros.FechaFinal);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesResultadoIntegracionParams.FECHAFIN), parametros.FechaFinal);

            return await GetAsyncList<ReporteResultadoIntegracion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF341>> ReporteSFCF341(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<ReporteSFCF341>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<DocumentoPorPeriodo>> AfiliadosPorPeriodo(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<DocumentoPorPeriodo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Datos_341>> ReportePlano341(ParametrosReportePlano341 parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.Identificacion), parametros.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<Datos_341>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Domain.Entities.Reportes.Settings>> ConsultaReportesGenerales()
        {
            return await GetAsyncList<Domain.Entities.Reportes.Settings>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteGeneralMora>> ConsultaReporteGeneralMora()
        {
            return await GetAsyncList<ReporteGeneralMora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Unidad_Ejecutora>> ConsultaUnidadEjecutora()
        {
            return await GetAsyncList<Unidad_Ejecutora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure);
        }        
        
        public async Task<IEnumerable<ConfiguracionColumna>> ConsultarColumnasPorUnidadEjecutora(int idUnidadEjecutora, int esReporteDeAltas)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsConfiguracionColumnas.IdUnidadEjecutora), idUnidadEjecutora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsConfiguracionColumnas.EsReporteDeAltas), esReporteDeAltas);
            return await GetAsyncList<ConfiguracionColumna>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF454>> ReporteSFCF454()
        {
            return await GetAsyncList<ReporteSFCF454>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                null, CommandType.StoredProcedure); 
        }


        public async Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCartera(ReporteGeneralCarteraParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.PeriodoActual), parametros.PeriodoActual);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.NumeroCredito), parametros.NumeroCredito);

            return await GetAsyncList<CarteraReporteGeneral>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraNew(ReporteGeneralCarteraParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.PeriodoActual), parametros.PeriodoActual);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.NumeroCredito), parametros.NumeroCredito);

            return await GetAsyncList<CarteraReporteGeneral>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraOptimizado(ReporteGeneralCarteraParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.PeriodoActual), parametros.PeriodoActual);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.NumeroCredito), parametros.NumeroCredito);

            return await GetAsyncList<CarteraReporteGeneral>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CarteraReporteGeneral>> GenerarReporteGeneralCarteraReestructurado(ReporteGeneralCarteraParametersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.PeriodoActual), parametros.PeriodoActual);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.TipoCredito), parametros.TipoCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralCartera.NumeroCredito), parametros.NumeroCredito);

            return await GetAsyncList<CarteraReporteGeneral>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CarteraReporteGeneralMora>> GenerarReporteGeneralMora(ReporteGeneralMoraParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralMora.Anio), parametros.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralMora.Mes), parametros.Mes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralMora.TipoCredito), parametros.TipoCredito);

            return await GetAsyncList<CarteraReporteGeneralMora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        //public async Task<IEnumerable<CarteraReporteGeneralMora>> GenerarReporteGarantias(ReporteGarantiasParametersDto parametros)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralMora.Anio), parametros.Anio);
        //    parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReporteGeneralMora.Mes), parametros.Mes);

        //    return await GetAsyncList<CarteraReporteGeneralMora>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
        //        parameters, CommandType.StoredProcedure);
        //}

        public async Task<ReportePazSalvo> GenerarReportePazSalvo(ReportePazSalvoParemetersDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReportePazSalvo.NumeroCredito), parametros.NumeroCredito);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumParamsReportePazSalvo.TipoCredito), parametros.TipoCredito);

            return await GetAsyncFirst<ReportePazSalvo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteAnioGravable>> GenerarReporteRetefuenteAnioGravable(ReporteAnioGravableParametersDto parametros)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumParamsRetefuenteAnioGravable.Anio.GetDescription(), parametros.Anio);
            parameters.Add(EnumParamsRetefuenteAnioGravable.TipoCredito.GetDescription(), parametros.TipoCredito);
            parameters.Add(EnumParamsRetefuenteAnioGravable.NumeroCredito.GetDescription(), parametros.NumeroCredito);

            return await GetAsyncList<ReporteAnioGravable>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteSFCF453Y454>> ReporteSFCF453Y454(ParametrosSuperfinancieraReportesDto parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReportesParams.FechaAplicacion), parametros.FechaAplicacion);
            return await GetAsyncList<ReporteSFCF453Y454>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);

        }


        public async Task<EncabezadoArchivosNomina> ConsultarArchivoEncabezadoNomina(int dtc_id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumSolicitudNomina.DocumentoID.GetDescription(), dtc_id);

            return await GetAsyncFirst<EncabezadoArchivosNomina>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CuerpoArchivosNomina>> ConsultarArchivoCuerpoNomina(int dtc_id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumSolicitudNomina.DocumentoID.GetDescription(), dtc_id);

            return await GetAsyncList<CuerpoArchivosNomina>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<FinArchivosNomina> ConsultarArchivoFinNomina(int dtc_id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumSolicitudNomina.DocumentoID.GetDescription(), dtc_id);

            return await GetAsyncFirst<FinArchivosNomina>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> GuardarDocumentoAdicionalCargado(DocumentosAdicionalesCargados documentos)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumDocumentoAdicionalesCargados.Documento.GetDescription(), documentos.DSA_DOCUMENTO);
            parameters.Add(EnumDocumentoAdicionalesCargados.Ruta.GetDescription(), documentos.DSA_RUTA);
            parameters.Add(EnumDocumentoAdicionalesCargados.Carpeta.GetDescription(), documentos.DSA_CARPETA);
            parameters.Add(EnumDocumentoAdicionalesCargados.CargadoDodo.GetDescription(), documentos.DSA_CARGADO_DODO);
            parameters.Add(EnumDocumentoAdicionalesCargados.Modulo.GetDescription(), documentos.DSA_MODULO);
            parameters.Add(EnumDocumentoAdicionalesCargados.NumeroDocumento.GetDescription(), documentos.DSA_NUMERO_DOCUMENTO);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }


        public async Task<DocumentosAdicionalesCargados> ConsultarDocumentosAdicionalesCargados(string documento)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumDocumentoAdicionalesCargados.NumeroDocumento.GetDescription(), documento);

            return await GetAsyncFirst<DocumentosAdicionalesCargados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DocumentosAdicionalesCargados>> ConsultarDocumentosAdicionalesCargadosReclasificacion(string documento)
        {
            var parameters = new DynamicParameters();
            parameters.Add(EnumDocumentoAdicionalesCargados.NumeroDocumento.GetDescription(), documento);

            return await GetAsyncList<DocumentosAdicionalesCargados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }



    }
}
 