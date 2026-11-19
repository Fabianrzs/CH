using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.Seven;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Application.Dto.Credito.TU;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Cierres;
using GA2.Domain.Entities.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Entities.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Domain.Entities.Catalogs;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.Simulador;
using GA2.Domain.Entities.Credito.Simulador.DocumentosAct;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Entities.Credito.TransUnion;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.Prospeccion;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Cartera.Cierres;
using GA2.Infraestructure.Data.Cartera.EnumBatchCausacionHipotecarioHistoricoTVP;
using GA2.Infraestructure.Data.Cartera.EnumBatchCausacionLeasingHistoricTVP;
using GA2.Infraestructure.Data.Cartera.EnumBatchCausacionLeasingSaldoActualTVP;
using GA2.Infraestructure.Data.Cartera.EnumValidarContinuidadCausacion;
using GA2.Infraestructure.Data.CierreDiario;
using GA2.Infraestructure.Data.Credito;
using GA2.Infraestructure.Data.Credito.Simulador;
using GA2.Infraestructure.Data.Credito.Simulador.Comite;
using GA2.Infraestructure.Data.Credito.Simulador.Documento;
using GA2.Infraestructure.Data.Credito.Simulador.SolicitudCredito;
using GA2.Infraestructure.Data.Credito.TransUnion;
using GA2.Infraestructure.Data.Escrituras;
using GA2.Infraestructure.Data.User;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static GA2.Infraestructure.Data.Prospeccion.EnumProspeccion;

namespace GA2.Infraestructure.Repositories
{
    public class CreditoRepository : DapperGenericRepository, ICreditoRepository
    {
        public CreditoRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Repositorio Actualiza Actividad Tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> ActualizaActividadTramite(ActividadTramiteSolicitud actualizacion)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), actualizacion.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.AFL_ID), actualizacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), actualizacion.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), actualizacion.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_FECHA_MODIFICACION), actualizacion.ACT_FECHA_MODIFICACION);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene Documentos Act Solicitud
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosActSolicitud(DocumentosActSolicitud request)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParameters.AFL_ID), request.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParameters.SOC_ID), request.SOC_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParameters.TCR_ID), request.TCR_ID);

            return await GetAsyncList<DocumentosActRespuesta>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene Documentos flujo
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RespuestaFlujoDocumentos>> ObtenerDocumentosFlujo(PeticionFlujoDocumentos solicitud)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumFlujoDocumentosParameters.SOC_ID), solicitud.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFlujoDocumentosParameters.TCR_ID), solicitud.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFlujoDocumentosParameters.ORDEN), solicitud.ORDEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFlujoDocumentosParameters.PRINCIPAL), solicitud.PRINCIPAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFlujoDocumentosParameters.AFL_ID), solicitud.AFL_ID);

            return await GetAsyncList<RespuestaFlujoDocumentos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene solicitud credito producto
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public async Task<RespuestaCreditoProducto> ObtenerSolicCreditoProducto(PeticionSolicitudCreditoProducto peticion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoProductoParameters.SOC_ID), peticion.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoProductoParameters.SOP_ID), peticion.SOP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoProductoParameters.IND), peticion.IND);

            return await GetAsyncFirst<RespuestaCreditoProducto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene solicitud credito basica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public async Task<RespuestaCreditoBasica> ObtenerSolicCreditoBasica(PeticionCreditoBasica peticion)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.SOC_ID), peticion.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.SOB_ID), peticion.SOB_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.IND), peticion.IND);

            var respuesta = await GetAsyncFirst<RespuestaCreditoBasica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return respuesta;
        }

        /// <summary>
        /// Obtiene solicitud credito financieros
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public async Task<RespuestaCreditoFinanciero> ObtenerSolicCreditoFinancieros(PeticionCreditoFinanciero peticion)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoFinancieroParameters.SOC_ID), peticion.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoFinancieroParameters.SOF_ID), peticion.SOF_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoFinancieroParameters.IND), peticion.IND);

            return await GetAsyncFirst<RespuestaCreditoFinanciero>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene tramite solicitud
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RespuestaSolicitudObtenerTramite>> ObtenerTramiteSolicitud(PeticionSolicitudObtenerTramite solicitud)
        {
#pragma warning disable IDE0090 // Use 'new(...)'
            DynamicParameters parameters = new DynamicParameters();
#pragma warning restore IDE0090 // Use 'new(...)'

            parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.fecha), solicitud.FECHA_SOL != null ? solicitud.FECHA_SOL : null);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.identificacion), solicitud.CLI_IDENTIFICACION ?? null);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.usuario), solicitud.USUARIO_ID);
            if (solicitud.ESTADO_ACT != null) parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.estado), solicitud.ESTADO_ACT);
            if (solicitud.TCR_ID != null) parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.tipoCredito), solicitud.TCR_ID);


            var response = await GetAsyncList<RespuestaSolicitudObtenerTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        /// <summary>
        /// Obtiene observaciones tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RespuestaObservacionTramite>> ObtenerObservacionTramite(PeticionObtenerObservacionTramite request)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumObservacionTramiteParameters.ACT_ID), request.ACT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObservacionTramiteParameters.TRS_ID), request.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObservacionTramiteParameters.IND), request.IND);


            return await GetAsyncList<RespuestaObservacionTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene estado del tramite solicitud de actividad desembolso
        /// </summary>
        /// <author>Reison Arturo Torres Urina</author>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public async Task<RespuestaEstadoTramiteSolicitudDesembolso> ObtenerEstadoTramiteSolicitudDesembolso(string numeroDocumento)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCreditoParams.identificacion), numeroDocumento ?? null);

            var response = await GetAsyncFirst<RespuestaEstadoTramiteSolicitudDesembolso>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        /// <summary>
        /// Obtiene valor del desembolso del credito
        /// </summary>
        /// <author>Reison Arturo Torres Urina</author>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public async Task<RespuestaValorDesembolsoCredito> ObtenerValorDesembolsoCredito(string numeroDocumento)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCreditoParams.identificacion), numeroDocumento ?? null);

            var response = await GetAsyncFirst<RespuestaValorDesembolsoCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }
        public async Task<SocSlsSimulacionCredito> ObtenerSlsSocSolicitudCredito(Guid IdSolicitud)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSlsSocSimulacionCreditoParams.idSolicitudCredito), IdSolicitud);

            var response = await GetAsyncFirst<SocSlsSimulacionCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }
        public async Task<SocSlsSimulacionCredito> ObtenerSlsSocSolicitudCreditoNroDocumento(string nroDocumento)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSlsSocSimulacionCreditoParams.nroDocumento), nroDocumento);

            var response = await GetAsyncFirst<SocSlsSimulacionCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }
        /// <summary>
        /// Envia subsanacion Actividad Tramite
        /// </summary>
        /// <param name="actualizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> EnviaSubsancionActividadTramite(ActividadTramiteSolicitud actualizacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), actualizacion.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.AFL_ID), actualizacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), actualizacion.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), actualizacion.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_FECHA_MODIFICACION), actualizacion.ACT_FECHA_MODIFICACION);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Aprueba Actividad tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> ApruebaActividadTramite(ActividadTramiteSolicitud actualizacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), actualizacion.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.AFL_ID), actualizacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), actualizacion.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), actualizacion.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_FECHA_MODIFICACION), actualizacion.ACT_FECHA_MODIFICACION);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Rechaza Actividad Tramite
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="actualizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> RechazarActividadTramite(ActividadTramiteSolicitud actualizacion)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), actualizacion.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.AFL_ID), actualizacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), actualizacion.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), actualizacion.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_FECHA_MODIFICACION), actualizacion.ACT_FECHA_MODIFICACION);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Desiste Actividad Tramite
        /// </summary>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <param name="actualizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> DesistirActividadTramite(ActividadTramiteSolicitud desistimiento)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), desistimiento.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.AFL_ID), desistimiento.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), desistimiento.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), desistimiento.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_FECHA_MODIFICACION), desistimiento.ACT_FECHA_MODIFICACION);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Devolver Actividad Tramite
        /// </summary>
        /// <param name="devolucion"></param>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <returns></returns>
        public async Task<RespuestaActividadTramite> DevolverActividadTramite(DevolverActividadTramite devolucion)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.TRS_ID), devolucion.TRS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_ID), devolucion.ACT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_OBSERVACION), devolucion.ACT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_MODIFICADO_POR), devolucion.ACT_MODIFICADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudActividadTramiteParameters.ACT_ID_RETORNO), devolucion.ACT_ID_RETORNO);

            return await GetAsyncFirst<RespuestaActividadTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear Solicitud Credito Informacion Tecnica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfTecnica"></param>
        /// <returns></returns>
        public async Task<SolicCreditoInfTecnica> CrearSolicCreditoInfTecnica(SolicCreditoInfTecnica solicCreditoInfTecnica)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.idSolicitudInformacionTecnica), solicCreditoInfTecnica.SIT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.idSolicitudCredito), solicCreditoInfTecnica.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.valorAvaluoComercial), solicCreditoInfTecnica.SIT_VALOR_AVALUO_COMERCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.valorVentaInmueble), solicCreditoInfTecnica.SIT_VALOR_VENTA_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.consideracionesFinales), solicCreditoInfTecnica.SIT_CONSIDERACIONES_FINALES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.declaraCumplimiento), solicCreditoInfTecnica.SIT_DECLARA_CUMPLIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.condicionesSalvedades), solicCreditoInfTecnica.SIT_CONDICIONES_SALVEADES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.conceptoEstTecnico), solicCreditoInfTecnica.SIT_CONCEPTO_EST_TECNICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.estrato), solicCreditoInfTecnica.SIT_ESTRATO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.creadoPor), solicCreditoInfTecnica.SIT_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.fechaCreacion), solicCreditoInfTecnica.SIT_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecnicaParameters.valorAvaluoCatastral), solicCreditoInfTecnica.SIT_VALOR_AVALUO_CATASTRAL);

            return await GetAsyncFirst<SolicCreditoInfTecnica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear Solicitud Credito Informacion Tecnica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfTecInm"></param>
        /// <returns></returns>
        public async Task<SolicCreditoInfTecInm> CrearSolicCreditoInfTecInm(SolicCreditoInfTecInm solicCreditoInfTecInm)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.idSolicitudInfoTecnicaInmueble), solicCreditoInfTecInm.STI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.idSolicitudtecnica), solicCreditoInfTecInm.SIT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.idSolicitudCredito), solicCreditoInfTecInm.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.numeroMatricula), solicCreditoInfTecInm.STI_NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.fechaMatricula), solicCreditoInfTecInm.STI_FECHA_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.numeroChip), solicCreditoInfTecInm.STI_NUMERO_CHIP);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.cedulaCatastral), solicCreditoInfTecInm.STI_CEDULA_CATASTRAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.creadoPor), solicCreditoInfTecInm.STI_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfTecInmParams.fechaCreacion), solicCreditoInfTecInm.STI_FECHA_CREACION);

            return await GetAsyncFirst<SolicCreditoInfTecInm>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear Solicitud Credito Informacion Juridica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfJuridica"></param>
        /// <returns></returns>
        public async Task<SolicCreditoInfJuridica> CrearSolicCreditoInfJuridica(SolicCreditoInfJuridica solicCreditoInfJuridica)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.idSolicitudInformacionJuridica), solicCreditoInfJuridica.SIJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.idSolicitudCredito), solicCreditoInfJuridica.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.linderos), solicCreditoInfJuridica.SIJ_LINDEROS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.tipoParqueadero), solicCreditoInfJuridica.SIJ_TIPO_PARQUEADERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.edadJuridica), solicCreditoInfJuridica.SIJ_EDAD_JURIDICA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.fechaPrimerActo), solicCreditoInfJuridica.SIJ_FECHA_PRIMER_ACTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.tradicionInmueble), solicCreditoInfJuridica.SIJ_TRADICION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.analisisUlt20Años), solicCreditoInfJuridica.SIJ_ANALISIS_ULT_20_AÑOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.analisisSitJuridica), solicCreditoInfJuridica.SIJ_ANALISIS_SIT_JURIDICA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.AnalisisRegPropHorizontal), solicCreditoInfJuridica.SIJ_ANALISIS_REG_PROP_HOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.analisisPazySalvo), solicCreditoInfJuridica.SIJ_ANALISIS_PAZ_Y_SALVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.analisisVendedor), solicCreditoInfJuridica.SIJ_ANALISIS_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.viabilidadJurNegocio), solicCreditoInfJuridica.SIJ_VIABILIDAD_JUR_NEGOCIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.recomendaciones), solicCreditoInfJuridica.SIJ_RECOMENDACIONES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.conceptoJuridicoFin), solicCreditoInfJuridica.SIJ_CONCEPTO_JURIDICO_FIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.conceptoEstJuridicio), solicCreditoInfJuridica.SIJ_CONCEPTO_EST_JURIDICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.ciudad), solicCreditoInfJuridica.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.departamento), solicCreditoInfJuridica.DPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.direccion), solicCreditoInfJuridica.SIJ_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.creadoPor), solicCreditoInfJuridica.SIJ_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.fechaCreacion), solicCreditoInfJuridica.SIJ_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.escritura), solicCreditoInfJuridica.SJI_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.fechaEscritura), solicCreditoInfJuridica.SJI_FECHA_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.notaria), solicCreditoInfJuridica.SJI_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.CiudadNotaria), solicCreditoInfJuridica.SIJ_CIUDAD_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.DepartamentoNotaria), solicCreditoInfJuridica.SIJ_DEPARTAMENTO_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.DescripcionInmueble), solicCreditoInfJuridica.SIJ_DESCRIPCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.OficinaInstrumentosPublicos), solicCreditoInfJuridica.SIJ_OFICINA_INSTRUMENTOS_PUBLICOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJuridicaParams.FoliosMatriculas), solicCreditoInfJuridica.SIJ_FOLIOS_MATRICULAS);

            return await GetAsyncFirst<SolicCreditoInfJuridica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear Olicitud Informacion Juridica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="solicCreditoInfJurInm"></param>
        /// <returns></returns>s
        public async Task<SolicCreditoInfJurInm> CrearSolicCreditoInfJurInm(SolicCreditoInfJurInm solicCreditoInfJurInm)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.IdSolicitudInfJuridicaInmueble), solicCreditoInfJurInm.SJI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.idSolicitudInformacionJuridica), solicCreditoInfJurInm.SIJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.idSolicitudCredito), solicCreditoInfJurInm.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.numeroMatricula), solicCreditoInfJurInm.SJI_NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.cedulaCatastral), solicCreditoInfJurInm.SJI_CEDULA_CATASTRAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.areaPrivada), solicCreditoInfJurInm.SJI_AREA_PRIVADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.avaluoComercial), solicCreditoInfJurInm.SJI_VALOR_AVALUO_COMERCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.avaluoCatastral), solicCreditoInfJurInm.SJI_VALOR_AVALUO_CATASTRAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.creadoPor), solicCreditoInfJurInm.SJI_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.fechaCreacion), solicCreditoInfJurInm.SJI_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.CiudadNotaria), solicCreditoInfJurInm.SJI_CIUDAD_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicCreditoInfJurInmParams.DepartamentoNotaria), solicCreditoInfJurInm.SJI_DEPARMENTO_NOTARIA);


            return await GetAsyncFirst<SolicCreditoInfJurInm>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Solicitud Credito Informacion tecnica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfTecnica"></param>
        /// <returns></returns>
        public async Task<SolicCreditoInfTecnica> ObtenerSolicCreditoInfTecnica(RequestSolicCreditoInfTecnica requestSolicCreditoInfTecnica)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecnicaParams.idSolicitudInformacionJuridica), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecnicaParams.solicitudCredito), requestSolicCreditoInfTecnica.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecnicaParams.indicador), requestSolicCreditoInfTecnica.IND);

            return await GetAsyncFirst<SolicCreditoInfTecnica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener solicitud Credito Informacion Tecnica Inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfTecnica"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolicCreditoInfTecInm>> ObtenerSolicCreditoInfTecInm(RequestSolicCreditoInfTecInm requestSolicCreditoInfTecInm)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecInmParams.idSolicitudInformaciontecnicaInmueble), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecInmParams.idSolicitudInformacionTecnica), requestSolicCreditoInfTecInm.SIT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecInmParams.idSolicitudCredito), requestSolicCreditoInfTecInm.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfTecInmParams.indicador), requestSolicCreditoInfTecInm.IND);

            return await GetAsyncList<SolicCreditoInfTecInm>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Solicitud Credito Informacion juridica
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfTecnica"></param>
        /// <returns></returns>
        public async Task<SolicCreditoInfJuridica> ObtenerSolicCreditoInfJuridica(RequestSolicCreditoInfJuridica requestSolicCreditoInfJuridica)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJuridicaParams.idSolicitudInformacionJuridica), requestSolicCreditoInfJuridica.SIJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJuridicaParams.idSolicitudCredito), requestSolicCreditoInfJuridica.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJuridicaParams.indicador), requestSolicCreditoInfJuridica.IND);

            return await GetAsyncFirst<SolicCreditoInfJuridica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener solicitud credito informacion juridica inmueble
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="requestSolicCreditoInfTecnica"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolicCreditoInfJurInm>> ObtenerSolicCreditoInfJurInm(RequestSolicCreditoInfJurInm requestSolicCreditoInfJurInm)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJurInmParams.idSolicitudInformacionJuridicaInmueble), requestSolicCreditoInfJurInm.SJI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJurInmParams.idSolicitudInformacionJuridica), requestSolicCreditoInfJurInm.SIJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJurInmParams.idSolicitudCredito), requestSolicCreditoInfJurInm.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJurInmParams.indicador), requestSolicCreditoInfJurInm.IND);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestSolicCreditoInfJurInmParams.Identificacion), requestSolicCreditoInfJurInm.IDENTIFICACION);

            return await GetAsyncList<SolicCreditoInfJurInm>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea Documento Solicitud Credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="documentoSolicCredito"></param>
        /// <returns></returns>
        public async Task<ResponseDocumentoSolicCredito> CrearDocumentoSolicCredito(ResponseDocumentoSolicCredito documentoSolicCredito)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.idDocumentoSolicitud), documentoSolicCredito.DCS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.idSolicitudCredito), documentoSolicCredito.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.idTipoDocumento), documentoSolicCredito.TDC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.codigoBarras), documentoSolicCredito.DCS_CODIGO_BARRAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.fechaDocumento), documentoSolicCredito.DCS_FECHA_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.numeroFolios), documentoSolicCredito.DCS_NUMERO_FOLIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.tamaño), documentoSolicCredito.DCS_TAMAÑO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.rutaImagen), documentoSolicCredito.DCS_RUTA_IMAGEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.estado), documentoSolicCredito.DCS_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.creadoPor), documentoSolicCredito.DCS_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.fechaCreacion), documentoSolicCredito.DCS_FECHA_CREACION);

            return await GetAsyncFirst<ResponseDocumentoSolicCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Realiza la consulta de simulacion de cliente
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public async Task<SimulacionCliente> ConsultarSimulacionCliente(string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSimulacionClienteParams.numeroDocumento), numeroDocumento);

            return await GetAsyncFirst<SimulacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene el detalle de la simulacion de un cliente
        /// </summary>
        /// <author>Daniel Fonseca Guerrero</author>
        /// <param name="simulacionId">Guid de la simulación</param>
        /// <returns></returns>
        public async Task<IEnumerable<DetalleSimulacionCliente>> ObtenerDetalleSimulacionCliente(Guid simulacionId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionClienteParameters.simulacionId), simulacionId);

            return await GetAsyncList<DetalleSimulacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los datos de solicitud de comite crédito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="datosSolicitudComite"></param>
        /// <returns></returns>
        public async Task<DatosSolicitudComite> ObtenerDatosSolicComite(Guid guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosSolicComiteParams.idSolicitudCredito), guid);

            return await GetAsyncFirst<DatosSolicitudComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Repositorio actualiza solciitud credito recomendacion
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SolicCreditoRecomendacion> ActualizaSolicCreditoRecomendacion(SolicCreditoRecomendacion request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.idSolicitudCredito), request.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.observacionRecomendacion), request.SOC_OBSERVACION_RECOMENDACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.valorRecomendadoCredito), request.SOC_VALOR_RECOMENDADO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.fechaActualiza), request.SOC_FECHA_ACTUALIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.actualizadoPor), request.SOC_ACTUALIZADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicCreditoRecomendacionParams.tipoAlivio), request.SOC_TIPO_ALIVIO);

            return await GetAsyncFirst<SolicCreditoRecomendacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Consultar Solicitud Credito
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ConsultaSolicitudCredito> ConsultarSolicitudCredito(ConsultaSolicitudCredito request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCreditoParams.idSolicitudCredito), request.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCreditoParams.identificacion), request.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCreditoParams.estado), request.SOC_ESTADO);

            return await GetAsyncFirst<ConsultaSolicitudCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualiza los datos de la solicitud de credito en Seguro
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ConsultaSolicitudCredito> ActualizaSolicCreditoSeguro(DatosActualizaSolicCreditoSeguro request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.idSolicitudCredito), request.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.convenioAseguradora), request.SOC_CONVENIO_ASEGURADORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.convenioAseguradoraHogar), request.SOC_CONVENIO_ASEGURADORA_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.idAseguradora), request.ASE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.idAseguradoraHogar), request.ASE_ID_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.porcentajeExtraprima), request.SOC_PORC_EXTRAPRIMA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.actualizadoPor), request.SOC_ACTUALIZADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.fechaActualiza), request.SOC_FECHA_ACTUALIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.talla), request.SOC_TALLA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.peso), request.SOC_PESO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActuDatosSolicCreditoParams.edad), request.SOC_EDAD);


            return await GetAsyncFirst<ConsultaSolicitudCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea la planilla de simulacion del cliente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanillaSimulacionCredito>> CrearSimulacionCliente(CreacionSimulacionClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.numDocumento), request.documento);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.tipoAbono),request.tipoAbono);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.tipoVivienda), request.tipoVivienda);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.valorVivienda), request.valor);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.subsidio), request.subsidio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.plazo), request.plazo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.valorPrestamo), request.valorPrestamo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSimulacionParams.valorCuota), request.valorCuota);

            return await GetAsyncList<PlanillaSimulacionCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Realiza la verificaciond e seguridad del cierre de actividad
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseVerificarCierreActSeg> VerificarCierreActSeg(RequestVerificarCierreActSeg request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificarCierreActSegParams.AFL_ID), request.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificarCierreActSegParams.TRS_ID), request.TRS_ID);

            return await GetAsyncFirst<ResponseVerificarCierreActSeg>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }



        #region Consecutivo

        /// <summary>
        /// CrearConsecutivo
        /// </summary>
        /// <param name="consecutivo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Consecutivo> CrearConsecutivo(Consecutivo consecutivo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ID), consecutivo.CNS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_DESCRIPCION), consecutivo.CNS_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ULTIMO_NUMERO), consecutivo.CNS_ULTIMO_NUMERO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ESTADO), consecutivo.CNS_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_CREADO_POR), consecutivo.CNS_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ACTUALIZADO_POR), String.Empty);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_FECHA_ACTUALIZA), null);


            return await GetAsyncFirst<Consecutivo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ActualizarConsecutivo
        /// </summary>
        /// <param name="consecutivo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Consecutivo> ActualizarConsecutivo(Consecutivo consecutivo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ID), consecutivo.CNS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_DESCRIPCION), consecutivo.CNS_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ULTIMO_NUMERO), consecutivo.CNS_ULTIMO_NUMERO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ESTADO), consecutivo.CNS_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_CREADO_POR), consecutivo.CNS_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ACTUALIZADO_POR), consecutivo.CNS_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_FECHA_ACTUALIZA), DateTime.Now);


            return await GetAsyncFirst<Consecutivo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerConsecutivoPorId
        /// </summary>
        /// <param name="consecutivoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Consecutivo> ObtenerConsecutivoPorId(string consecutivoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ID), consecutivoid);

            return await GetAsyncFirst<Consecutivo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarConsecutivoPorid
        /// </summary>
        /// <param name="consecutivoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Consecutivo> EliminarConsecutivoPorid(string consecutivoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsecutivoParametros.CNS_ID), consecutivoid);

            return await GetAsyncFirst<Consecutivo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerConsecutivo
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<IEnumerable<Consecutivo>> ObtenerConsecutivo()
        {
            return await GetAsyncList<Consecutivo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region Aseguradora

        /// <summary>
        /// CrearAseguradoras
        /// </summary>
        /// <param name="aseguradoras"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Aseguradoras> CrearAseguradoras(Aseguradoras aseguradoras)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_RAZON_SOCIAL), aseguradoras.ASE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_SITIO_WEB), aseguradoras.ASE_SITIO_WEB);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ESTADO), aseguradoras.ASE_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_CREADO_POR), aseguradoras.ASE_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ACTUALIZADO_POR), aseguradoras.ASE_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_FECHA_ACTUALIZA), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_CONVENIO), aseguradoras.ASE_CONVENIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_RAMO), aseguradoras.ASE_RAMO);

            return await GetAsyncFirst<Aseguradoras>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarAseguradoras
        /// </summary>
        /// <param name="aseguradoras"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Aseguradoras> ActualizarAseguradoras(Aseguradoras aseguradoras)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ID), aseguradoras.ASE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_RAZON_SOCIAL), aseguradoras.ASE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_SITIO_WEB), aseguradoras.ASE_SITIO_WEB);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ESTADO), aseguradoras.ASE_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_CREADO_POR), aseguradoras.ASE_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ACTUALIZADO_POR), aseguradoras.ASE_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_FECHA_ACTUALIZA), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_CONVENIO), aseguradoras.ASE_CONVENIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_RAMO), aseguradoras.ASE_RAMO);


            return await GetAsyncFirst<Aseguradoras>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerAseguradorasPorId
        /// </summary>
        /// <param name="aseguradorasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<Aseguradoras> ObtenerAseguradorasPorId(string aseguradorasid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ID), aseguradorasid);

            return await GetAsyncFirst<Aseguradoras>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarAseguradorasPorid
        /// </summary>
        /// <param name="aseguradorasId"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<Aseguradoras> EliminarAseguradorasPorid(string aseguradorasid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAseguradorasParametros.ASE_ID), aseguradorasid);

            return await GetAsyncFirst<Aseguradoras>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerAseguradoras
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<IEnumerable<Aseguradoras>> ObtenerAseguradoras()
        {
            return await GetAsyncList<Aseguradoras>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        #endregion

        #region Alertas Automaticas


        /// <summary>
        /// CrearAlertaAutomaticas
        /// </summary>
        /// <param name="alertaautomaticas"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<AlertaAutomaticas> CrearAlertaAutomaticas(AlertaAutomaticas alertaautomaticas)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_ID), alertaautomaticas.ALA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_DESCRIPCION), alertaautomaticas.ALA_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_MENSAJE), alertaautomaticas.ALA_MENSAJE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_MODIFICADO_POR), alertaautomaticas.ALA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<AlertaAutomaticas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }


        /// <summary>
        /// ActualizarAlertaAutomaticas
        /// </summary>
        /// <param name="alertaautomaticas"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<AlertaAutomaticas> ActualizarAlertaAutomaticas(AlertaAutomaticas alertaautomaticas)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_ID), alertaautomaticas.ALA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_DESCRIPCION), alertaautomaticas.ALA_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_MENSAJE), alertaautomaticas.ALA_MENSAJE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_MODIFICADO_POR), alertaautomaticas.ALA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<AlertaAutomaticas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerAlertaAutomaticasPorId
        /// </summary>
        /// <param name="alertaautomaticasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<AlertaAutomaticas> ObtenerAlertaAutomaticasPorId(string alertaautomaticasid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_ID), alertaautomaticasid);

            return await GetAsyncFirst<AlertaAutomaticas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarAseguradorasPorid
        /// </summary>
        /// <param name="alertaautomaticasid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<AlertaAutomaticas> EliminarAlertaAutomaticasPorid(string alertaautomaticasid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumAlertaParametros.ALA_ID), alertaautomaticasid);

            return await GetAsyncFirst<AlertaAutomaticas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarAseguradorasPorid
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<AlertaAutomaticas>> ObtenerAlertaAutomaticas()
        {
            return await GetAsyncList<AlertaAutomaticas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region ColorGrilla

        /// <summary>
        /// CrearColorGrilla
        /// </summary>
        /// <param name="colorGrilla"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<ColorGrilla> CrearColorGrilla(ColorGrilla colorgrilla)


        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_DESCRIPCION), colorgrilla.CLG_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ESTILO_COLOR), colorgrilla.CLG_ESTILO_COLOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ESTADO), colorgrilla.CLG_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_CREADO_POR), colorgrilla.CLG_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ACTUALIZADO_POR), colorgrilla.CLG_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_FECHA_ACTUALIZA), DateTime.Now);


            return await GetAsyncFirst<ColorGrilla>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarColorGrilla
        /// </summary>
        /// <param name="colorgrilla"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<ColorGrilla> ActualizarColorGrilla(ColorGrilla colorgrilla)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ID), colorgrilla.CLG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_DESCRIPCION), colorgrilla.CLG_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ESTILO_COLOR), colorgrilla.CLG_ESTILO_COLOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ESTADO), colorgrilla.CLG_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_CREADO_POR), colorgrilla.CLG_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ACTUALIZADO_POR), colorgrilla.CLG_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_FECHA_ACTUALIZA), DateTime.Now);


            return await GetAsyncFirst<ColorGrilla>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerColorGrillaPorId
        /// </summary>
        /// <param name="colorgrillaid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<ColorGrilla> ObtenerColorGrillaPorId(string colorgrillaid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ID), colorgrillaid);

            return await GetAsyncFirst<ColorGrilla>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarColorGrillaPorid
        /// </summary>
        /// <param name="colorgrillaid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<ColorGrilla> EliminarColorGrillaPorid(string colorgrillaid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumColorGrillaParametros.CLG_ID), colorgrillaid);

            return await GetAsyncFirst<ColorGrilla>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerColorGrilla
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<ColorGrilla>> ObtenerColorGrilla()
        {
            return await GetAsyncList<ColorGrilla>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }




        #endregion

        #region Estado Actividad

        /// <summary>
        /// CrearEstadoActividad
        /// </summary>
        /// <param name="estadoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<EstadoActividad> CrearEstadoActividad(EstadoActividad estadoActividad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_DESCRIPCION), estadoActividad.ESA_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_ESTADO), estadoActividad.ESA_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_CREADO_POR), estadoActividad.ESA_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_MODIFICADO_POR), estadoActividad.ESA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<EstadoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarEstadoActividad
        /// </summary>
        /// <param name="estadoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<EstadoActividad> ActualizarEstadoActividad(EstadoActividad estadoActividad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_ID), estadoActividad.ESA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_DESCRIPCION), estadoActividad.ESA_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_ESTADO), estadoActividad.ESA_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_CREADO_POR), estadoActividad.ESA_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_MODIFICADO_POR), estadoActividad.ESA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<EstadoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerEstadoActividadPorId
        /// </summary>
        /// <param name="estadoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>03/05/2021</date>
        /// 

        public async Task<EstadoActividad> ObtenerEstadoActividadPorId(string estadoActividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_ID), estadoActividadid);

            return await GetAsyncFirst<EstadoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarEstadoActividadPorid
        /// </summary>
        /// <param name="estadoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>03/05/2021</date>
        /// 
        public async Task<EstadoActividad> EliminarEstadoActividadPorid(string estadoActividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoActividadParametros.ESA_ID), estadoActividadid);

            return await GetAsyncFirst<EstadoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerEstadoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>03/05/2021</date>
        /// 
        public async Task<IEnumerable<EstadoActividad>> ObtenerEstadoActividad()
        {
            return await GetAsyncList<EstadoActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        #endregion

        #region Estado Solicitud
        /// <summary>
        /// CrearEstadoSolicitud
        /// </summary>
        /// <param name="estadosolicitud"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<EstadoSolicitud> CrearEstadoSolicitud(EstadoSolicitud estadosolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_DESCRIPCION), estadosolicitud.ESS_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_SIGLA), estadosolicitud.ESS_SIGLA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_ESTADO), estadosolicitud.ESS_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_CREADO_POR), estadosolicitud.ESS_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_MODIFICADO_POR), estadosolicitud.ESS_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<EstadoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ActualizarEstadoSolicitud
        /// </summary>
        /// <param name="estadosolicitud"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<EstadoSolicitud> ActualizarEstadoSolicitud(EstadoSolicitud estadosolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_ID), estadosolicitud.ESS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_DESCRIPCION), estadosolicitud.ESS_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_SIGLA), estadosolicitud.ESS_SIGLA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_ESTADO), estadosolicitud.ESS_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_CREADO_POR), estadosolicitud.ESS_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_MODIFICADO_POR), estadosolicitud.ESS_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<EstadoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerEstadoSolicitudPorId
        /// </summary>
        /// <param name="estadodolicitudid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<EstadoSolicitud> ObtenerEstadoSolicitudPorId(string estadosolicitudid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_ID), estadosolicitudid);

            return await GetAsyncFirst<EstadoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarEstadoSolicitudPorid
        /// </summary>
        /// <param name="estadosolicitudid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<EstadoSolicitud> EliminarEstadoSolicitudPorid(string estadosolicitudid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEstadoSolicitudParametros.ESS_ID), estadosolicitudid);

            return await GetAsyncFirst<EstadoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerEstadoSolicitud
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<EstadoSolicitud>> ObtenerEstadoSolicitud()
        {
            return await GetAsyncList<EstadoSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region Tipo Actividad

        /// <summary>
        /// CrearTipoActividad
        /// </summary>
        /// <param name="tipoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<TipoActividad> CrearTipoActividad(TipoActividad tipoActividad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_DESCRIPCION), tipoActividad.TAC_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_ESTADO), tipoActividad.TAC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_CREADO_POR), tipoActividad.TAC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_MODIFICADO_POR), tipoActividad.TAC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<TipoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarTipoActividad
        /// </summary>
        /// <param name="tipoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<TipoActividad> ActualizarTipoActividad(TipoActividad tipoActividad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_ID), tipoActividad.TAC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_DESCRIPCION), tipoActividad.TAC_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_ESTADO), tipoActividad.TAC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_CREADO_POR), tipoActividad.TAC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_MODIFICADO_POR), tipoActividad.TAC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<TipoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerTipoActividadPorId
        /// </summary>
        /// <param name="tipoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<IEnumerable<TipoActividad>> ObtenerTipoActividadPorId(string tipoActividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_ID), tipoActividadid);

            return await GetAsyncList<TipoActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarTipoActividadPorid
        /// </summary>
        /// <param name="tipoActividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<TipoActividad> EliminarTipoActividadPorid(string tipoActividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.TAC_ID), tipoActividadid);

            return await GetAsyncFirst<TipoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerTipoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<TipoActividad>> ObtenerTipoActividad(int AFL_ID)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoActividadParametros.AFL_ID), AFL_ID);

            return await GetAsyncList<TipoActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        #endregion

        #region tipo Documento

        /// <summary>
        /// CrearTipoDocumento
        /// </summary>
        /// <param name="tipodocumento"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<TipoDocumento> CrearTipoDocumento(TipoDocumento tipodocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_DESCRIPCION), tipodocumento.TDC_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ESTADO), tipodocumento.TDC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_CREADO_POR), tipodocumento.TDC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_MODIFICADO_POR), tipodocumento.TDC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<TipoDocumento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarTipoDocumento
        /// </summary>
        /// <param name="tipodocumento"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<TipoDocumento> ActualizarTipoDocumento(TipoDocumento tipodocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ID), tipodocumento.TDC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_DESCRIPCION), tipodocumento.TDC_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ESTADO), tipodocumento.TDC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_CREADO_POR), tipodocumento.TDC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_MODIFICADO_POR), tipodocumento.TDC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<TipoDocumento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerTipoDocumentoPorId
        /// </summary>
        /// <param name="tipodocumentoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<TipoDocumento> ObtenerTipoDocumentoPorId(string tipodocumentoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ID), tipodocumentoid);

            return await GetAsyncFirst<TipoDocumento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarTipoDocumentoPorid
        /// </summary>
        /// <param name="tipodocumentoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<TipoDocumento> EliminarTipoDocumentoPorid(string tipodocumentoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ID), tipodocumentoid);

            return await GetAsyncFirst<TipoDocumento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerTipoDocumento
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<IEnumerable<TipoDocumento>> ObtenerTipoDocumento()
        {
            return await GetAsyncList<TipoDocumento>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NMNTA_TIPO_ARCHIVO>> ObtenerTiposArchivos()
        {
            return await GetAsyncList<NMNTA_TIPO_ARCHIVO>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region Flujo

        /// <summary>
        /// CrearFlujo
        /// </summary>
        /// <param name="flujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<Flujo> CrearFlujo(Flujo flujo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_DESCRIPCION), flujo.FLU_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ESTADO), flujo.FLU_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ORDEN), flujo.FLU_ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_CREADO_POR), flujo.FLU_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_MODIFICADO_POR), flujo.FLU_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<Flujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ActualizarFlujo
        /// </summary>
        /// <param name="flujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<Flujo> ActualizarFlujo(Flujo flujo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ID), flujo.FLU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_DESCRIPCION), flujo.FLU_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ORDEN), flujo.FLU_ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ESTADO), flujo.FLU_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_CREADO_POR), flujo.FLU_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_MODIFICADO_POR), flujo.FLU_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_FECHA_MODIFICACION), DateTime.Now);


            return await GetAsyncFirst<Flujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerFlujoPorId
        /// </summary>
        /// <param name="flujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<Flujo> ObtenerFlujoPorId(string flujoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ID), flujoid);

            return await GetAsyncFirst<Flujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarFlujoPorid
        /// </summary>
        /// <param name="flujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<Flujo> EliminarFlujoPorid(string flujoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoParametros.FLU_ID), flujoid);

            return await GetAsyncFirst<Flujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerFlujoPorId
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<IEnumerable<Flujo>> ObtenerFlujo()
        {
            return await GetAsyncList<Flujo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region FlujoTipoCredito
        /// <summary>
        /// CrearFlujoTipoCredito
        /// </summary>
        /// <param name="flujotipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<FlujoTipoCredito> CrearFlujoTipoCredito(FlujoTipoCredito flujotipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FLU_ID), flujotipoCredito.FLU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.TCR_ID), flujotipoCredito.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_ESTADO), flujotipoCredito.FTC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_CREADO_POR), flujotipoCredito.FTC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_FECHA_CREACION), flujotipoCredito.FTC_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_MODIFICADO_POR), flujotipoCredito.FTC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_FECHA_MODIFICACION), flujotipoCredito.FTC_FECHA_MODIFICACION);


            return await GetAsyncFirst<FlujoTipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ActualizarFlujoTipoCredito
        /// </summary>
        /// <param name="flujotipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<FlujoTipoCredito> ActualizarFlujoTipoCredito(FlujoTipoCredito flujotipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_ID), flujotipoCredito.FTC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FLU_ID), flujotipoCredito.FLU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.TCR_ID), flujotipoCredito.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_ESTADO), flujotipoCredito.FTC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_CREADO_POR), flujotipoCredito.FTC_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_FECHA_CREACION), flujotipoCredito.FTC_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_MODIFICADO_POR), flujotipoCredito.FTC_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_FECHA_MODIFICACION), flujotipoCredito.FTC_FECHA_MODIFICACION);


            return await GetAsyncFirst<FlujoTipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerFlujoTipoCreditoPorId
        /// </summary>
        /// <param name="flujotipoCreditoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<FlujoTipoCredito> ObtenerFlujoTipoCreditoPorId(string flujotipoCreditoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_ID), flujotipoCreditoid);

            return await GetAsyncFirst<FlujoTipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarFlujoTipoCreditoPorid
        /// </summary>
        /// <param name="flujotipoCreditoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<FlujoTipoCredito> EliminarFlujoTipoCreditoPorid(string flujotipoCreditoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFlujoTipoCreditoParametros.FTC_ID), flujotipoCreditoid);

            return await GetAsyncFirst<FlujoTipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerFlujoTipoCredito
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<IEnumerable<FlujoTipoCredito>> ObtenerFlujoTipoCredito()
        {
            return await GetAsyncList<FlujoTipoCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region Actividad Flujo

        /// <summary>
        /// CrearActividadFlujo
        /// </summary>
        /// <param name="actividadflujo"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<ActividadFlujo> CrearActividadFlujo(ActividadFlujo actividadflujo)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.FLU_ID), actividadflujo.FLU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.TAC_ID), actividadflujo.TAC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ORDEN), actividadflujo.AFL_ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_TIEMPO), actividadflujo.AFL_TIEMPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ACTIVIDAD_AUTOMATICA), actividadflujo.AFL_ACTIVIDAD_AUTOMATICA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ACTIVIDAD_PRINCIPAL), actividadflujo.AFL_ACTIVIDAD_PRINCIPAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_PUEDE_DELEGAR), actividadflujo.AFL_PUEDE_DELEGAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CARGA_ARCHIVOS), actividadflujo.AFL_CARGA_ARCHIVOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_VISUALIZA_ARCHIVOS), actividadflujo.AFL_VISUALIZA_ARCHIVOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CAPTURA_DATOS_TEC), actividadflujo.AFL_CAPTURA_DATOS_TEC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CAPTURA_DATOS_JUR), actividadflujo.AFL_CAPTURA_DATOS_JUR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CAPTURA_DATOS_FIN), actividadflujo.AFL_CAPTURA_DATOS_FIN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_VISUALIZA_DATOS_BAS), actividadflujo.AFL_VISUALIZA_DATOS_BAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_VIISUALIZA_DATOS_GAR), actividadflujo.AFL_VIISUALIZA_DATOS_GAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_VISUALIZA_DATOS_FIN), actividadflujo.AFL_VISUALIZA_DATOS_FIN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_GENERA_PDF_RESUMEN), actividadflujo.AFL_GENERA_PDF_RESUMEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CONSULTA_BURO), actividadflujo.AFL_CONSULTA_BURO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ENVIO_NOTIFICACION), actividadflujo.AFL_ENVIO_NOTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ENVIO_NOTIFICACION_VENC), actividadflujo.AFL_ENVIO_NOTIFICACION_VENC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ENVIA_NOTIFICACION_CLIENTE), actividadflujo.AFL_ENVIA_NOTIFICACION_CLIENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_RECHAZA_FLUJO), actividadflujo.AFL_RECHAZA_FLUJO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_DEVUELVE_FLUJO), actividadflujo.AFL_DEVUELVE_FLUJO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_RETORNA_ACTIVIDAD_ID), actividadflujo.AFL_RETORNA_ACTIVIDAD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_DESISTIR_FLUJO), actividadflujo.AFL_DESISTIR_FLUJO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ESTADO), actividadflujo.AFL_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_CREADO_POR), actividadflujo.AFL_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_FECHA_CREACION), actividadflujo.AFL_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_MODIFICADO_POR), actividadflujo.AFL_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_FECHA_MODIFICACION), actividadflujo.AFL_FECHA_MODIFICACION);

            return await GetAsyncFirst<ActividadFlujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerActividadFlujoPorId
        /// </summary>
        /// <param name="actividadflujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<ActividadFlujo> ObtenerActividadFlujoPorId(string actividadflujoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ID), actividadflujoid);

            return await GetAsyncFirst<ActividadFlujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarActividadFlujoPorid
        /// </summary>
        /// <param name="actividadflujoid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<ActividadFlujo> EliminarActividadFlujoPorid(string actividadflujoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumActividadFlujoParametros.AFL_ID), actividadflujoid);

            return await GetAsyncFirst<ActividadFlujo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerActividadFlujo
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<IEnumerable<ActividadFlujo>> ObtenerActividadFlujo()
        {
            return await GetAsyncList<ActividadFlujo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #endregion

        #region Usuario Actividad

        /// <summary>
        /// CrearUsuarioActividad
        /// </summary>
        /// <param name="usuarioActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// <returns></returns>
        public async Task<UsuarioActividad> CrearUsuarioActividad(UsuarioActividad usuarioActividad)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.AFL_ID), usuarioActividad.AFL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.ID_USERS), usuarioActividad.ID_USERS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_ESTADO), usuarioActividad.USA_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_CREADO_POR), usuarioActividad.USA_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_MODIFICADO_POR), usuarioActividad.USA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<UsuarioActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerUsuarioActividadPorId
        /// </summary>
        /// <param name="usuarioactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// <returns></returns>
        public async Task<UsuarioActividad> ObtenerUsuarioActividadPorId(string usuarioactividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_ID), usuarioactividadid);

            return await GetAsyncFirst<UsuarioActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarUsuarioActividadPorid
        /// </summary>
        /// <param name="usuarioactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// <returns></returns>
        public async Task<UsuarioActividad> EliminarUsuarioActividadPorid(string usuarioactividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumUsuarioActividadParametros.USA_ID), usuarioactividadid);

            return await GetAsyncFirst<UsuarioActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerUsuarioActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<IEnumerable<UsuarioActividad>> ObtenerUsuarioActividad()
        {
            return await GetAsyncList<UsuarioActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        #endregion

        #region Documento Actividad

        /// <summary>
        /// CrearDocumentoActividad
        /// </summary>
        /// <param name="documentoactividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 


        public async Task<DocumentoActividad> CrearDocumentoActividad(DocumentoActividad documentoactividad)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.AFL_ID), documentoactividad.AFL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.TDC_ID), documentoactividad.TDC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_ORDEN), documentoactividad.DCA_ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_OBLIGATORIO), documentoactividad.DCA_OBLIGATORIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_CARGA), documentoactividad.DCA_CARGA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_VISUALIZA), documentoactividad.DCA_VISUALIZA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_VISUALIZA_CLIENTE), documentoactividad.DCA_VISUALIZA_CLIENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_ESTADO), documentoactividad.DCA_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_FECHA_CREACION), documentoactividad.DCA_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_CREADO_POR), documentoactividad.DCA_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_MODIFICADO_POR), documentoactividad.DCA_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_FECHA_MODIFICACION), documentoactividad.DCA_FECHA_MODIFICACION);


            return await GetAsyncFirst<DocumentoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerDocumentoActividadPorId
        /// </summary>
        /// <param name="documentoactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<DocumentoActividad> ObtenerDocumentoActividadPorId(string documentoactividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_ID), documentoactividadid);

            return await GetAsyncFirst<DocumentoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarDocumentoActividadPorid
        /// </summary>
        /// <param name="documentoactividadid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 
        public async Task<DocumentoActividad> EliminarDocumentoActividadPorid(string documentoactividadid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoActividadParametros.DCA_ID), documentoactividadid);

            return await GetAsyncFirst<DocumentoActividad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerDocumentoActividad
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>06/05/2021</date>
        /// 

        public async Task<IEnumerable<DocumentoActividad>> ObtenerDocumentoActividad()
        {
            return await GetAsyncList<DocumentoActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }



        #endregion

        /// <summary>
        /// Obtiene los datos de un comite de credito
        /// </summary>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DatosComiteCredito>> ObtenerComiteCredito(DatosComiteCredito datosComiteCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.codigoComite), datosComiteCredito.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.fecha), datosComiteCredito.COC_FECHA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.numeroComite), datosComiteCredito.COC_NUMERO_COMITE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.codigoEstado), datosComiteCredito.COC_ESTADO);

            var respuesta = await GetAsyncList<DatosComiteCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return respuesta;
        }

        /// <summary>
        /// Aprueba el comite de crédito
        /// </summary>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<DatosComiteCredito> AprobarComiteCredito(DatosComiteCredito datosComiteCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.codigoComite), datosComiteCredito.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.fechaActualiza), datosComiteCredito.COC_FECHA_ACTUALIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.actualizadoPor), datosComiteCredito.COC_ACTUALIZADO_POR);

            return await GetAsyncFirst<DatosComiteCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtiene todos los integrantes del comite de credito
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IntegranteComite>> ObtenerIntegranteComite()
        {
            return await GetAsyncList<IntegranteComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene todos los integrantes del comite de credito
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TemaComite>> ObtenerTemasComite()
        {
            return await GetAsyncList<TemaComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los integrantes d eun comité segun el numero de comité
        /// </summary>
        /// <param name="integranteComite"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IntegrantePorComite>> ObtenerComiteCreditoIntegrante(IntegrantePorComite integranteComite)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerComiteCreditoIntegranteParams.idIntegrantePorComite), integranteComite.COI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerComiteCreditoIntegranteParams.idComite), integranteComite.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerComiteCreditoIntegranteParams.indicador), (integranteComite.INDICADOR != 0) ? integranteComite.INDICADOR : 2);

            return await GetAsyncList<IntegrantePorComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea comite credito
        /// </summary>
        /// <param name="datosComiteCredito"></param>
        /// <returns></returns>
        public async Task<DatosComiteCredito> CrearComiteCredito(DatosComiteCredito datosComiteCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.codigoComite), datosComiteCredito.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.fecha), datosComiteCredito.COC_FECHA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.lugar), datosComiteCredito.COC_LUGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.horaInicio), datosComiteCredito.COC_HORA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.horaFinalizacion), datosComiteCredito.COC_HORA_FINALIZACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.desarrollo), datosComiteCredito.COC_DESARROLLO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.temasAprobacion), datosComiteCredito.COC_TEMAS_APROBACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.anotacion), datosComiteCredito.COC_ANOTACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.cargoAnalista), datosComiteCredito.COC_CARGO_ANALISTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.creadoPor), datosComiteCredito.COC_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.fechaCreacion), DateTime.Today);

            return await GetAsyncFirst<DatosComiteCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea untegrante comite credito
        /// </summary>
        /// <param name="integrante"></param>
        /// <returns></returns>
        public async Task<ComiteIntegrante> CrearComiteCreditoIntegrante(ComiteIntegrante integrante)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.idIntegranteComite), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.idComiteCredito), integrante.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.idAsistente), integrante.ICO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.orden), integrante.COI_ORDEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.creadoPor), integrante.COI_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCrearIntegranteParams.fechaCreacion), DateTime.Now);

            return await GetAsyncFirst<ComiteIntegrante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ClienteIdentificacion>> ConsultarClientesCredito()
        {
            DynamicParameters parameters = new DynamicParameters();


            return await GetAsyncList<ClienteIdentificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Crea tema para comite decrédito
        /// </summary>
        /// <param name="temaComiteCredito"></param>
        /// <returns></returns>
        public async Task<TemaComiteCredito> CrearComiteCreditoTemas(TemaComiteCredito temaComiteCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.idTema), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.idComiteCredito), temaComiteCredito.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.codigoTema), temaComiteCredito.TCO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.orden), temaComiteCredito.COT_ORDEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.observacion), temaComiteCredito.COT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.creadoPor), temaComiteCredito.COT_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnmuCrearTemaComiteCreditoParams.fechaCreacion), DateTime.Now);

            return await GetAsyncFirst<TemaComiteCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene la solicitud de comite de credito
        /// </summary>
        /// <param name="solicitudComite"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolicitudComite>> ObtenerComiteCreditoSolicitud(SolicitudComite solicitudComite)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoParams.idComiteCredito), solicitudComite.COC_ID);

            return await GetAsyncList<SolicitudComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear solicitud Credito comité
        /// </summary>
        /// <param name="solicitudComite"></param>
        /// <returns></returns>
        public async Task<SolicitudComiteCreacion> CrearComiteCreditoSolicitud(SolicitudComiteCreacion solicitudComite)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.idComiteSolicitud), solicitudComite.COS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.idSolicitudCredito), solicitudComite.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.idComiteCredito), solicitudComite.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.resultado), solicitudComite.COS_RESULTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.aprobada), solicitudComite.COS_APROBADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.rechazada), solicitudComite.COS_RECHAZADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.creadoPor), solicitudComite.COS_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.fechaCreacion), solicitudComite.COS_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.valorCredito), solicitudComite.COS_VALOR_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.plazoCredito), solicitudComite.COS_PLAZO_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.teaCredito), solicitudComite.COS_TEA_CREDITO);

            return await GetAsyncFirst<SolicitudComiteCreacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Historial Comercial Buro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HistorialCredito> ObtenerHistorialComercialBuro(string request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialCreditioParams.identificacion), request);

            var historialBuro = await GetAsyncFirst<HistorialCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return historialBuro;
        }

        /// <summary>
        /// Obtiene comite de credito temas
        /// </summary>
        /// <param name="solicitudComite"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TemaComiteCredito>> ObtenerComiteCreditoTemas(RequestTemaComiteCredito solicitudComite)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestTemasComiteParams.idTema), solicitudComite.COT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestTemasComiteParams.idComite), solicitudComite.COC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRequestTemasComiteParams.indicador), (solicitudComite.IND != 0) ? solicitudComite.IND : 2);

            return await GetAsyncList<TemaComiteCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualiza Solic Credito Tasas
        /// </summary>
        /// <param name="TasaSolicitudCredito"></param>
        /// <returns></returns>
        public async Task<TasaSolicitudCredito> ActualizaSolicCreditoTasas(TasaSolicitudCredito TasaSolicitudCredito)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSolicitudCreditoParams.IdSolicitudCredito), TasaSolicitudCredito.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSolicitudCreditoParams.TasaFrech), TasaSolicitudCredito.SOC_TASA_FECH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSolicitudCreditoParams.ValorAlivio), TasaSolicitudCredito.SOC_VALOR_ALIVIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSolicitudCreditoParams.ActualizadoPor), TasaSolicitudCredito.SOC_ACTUALIZADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSolicitudCreditoParams.FechaActualizacion), DateTime.Now);

            return await GetAsyncFirst<TasaSolicitudCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Fuentes Recursos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FuenteRecursos>> ObtenerFuentesRecursos()
        {
            return await GetAsyncList<FuenteRecursos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Desembolsos Solicitud
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SolicitudDesembolso>> ObtenerDesembolsosSolicitud(SolicitudDesembolso solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.FechaDesembolso), solicitud.SID_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.DocumentoCliente), solicitud.DOCUMENTO_CLIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.Aplicado), solicitud.APLICADO);

            var retornado = await GetAsyncList<SolicitudDesembolso>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            foreach (var retorno in retornado)
            {
                retorno.SID_APLICADO_POR = Guid.NewGuid();
            }

            return retornado;
        }

        public async Task<SolicitudDesembolso> ObtenerDesembolsosSolicitudCredito(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.IdSolicitudCredito), idSolicitud);


            var retornado = await GetAsyncFirst<SolicitudDesembolso>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


            return retornado;
        }

        public async Task<bool> ActualizarFechaDesembolsoCredito(Guid idSolicitud, DateTime fechaDesembolso)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.IdSolicitudCredito), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParams.FechaDesembolso), fechaDesembolso);

            var retornado = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


            return retornado;
        }



        /// <summary>
        /// Crear Solic Credito Desembolsos
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<SolicitudCreditosDesembolsos> CrearSolicCreditoDesembolsos(SolicitudCreditosDesembolsos solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdSolicitudDesembolso), solicitud.SID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdSolicitudCredito), solicitud.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdFuenteRecursos), solicitud.FRC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdFuenteRecursosDos), solicitud.FRC_ID_DOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdFuenteRecursosTres), solicitud.FRC_ID_TRES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.ValorDesembolso), solicitud.SID_VALOR_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.ValorDesembolsoDos), solicitud.SID_VALOR_DESEMBOLSO_DOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.ValorDesembolsoTres), solicitud.SID_VALOR_DESEMBOLSO_TRES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.FechaDesembolso), solicitud.SID_FECHA_DESEMBOLSO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.CreadoPor), solicitud.CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.FechaCreacion), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.VendedorId), solicitud.IVEN_ID);

            return await GetAsyncFirst<SolicitudCreditosDesembolsos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// AplicarDesembolsoSolicitud
        /// </summary>
        /// <param name="desembolso"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<DesembolsoCreditoSolicitud> AplicarDesembolsoSolicitud(DesembolsoCreditoSolicitud desembolso)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesembolsoCreditoSolicitudParametros.SOC_ID), desembolso.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesembolsoCreditoSolicitudParametros.SID_ID), desembolso.SID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesembolsoCreditoSolicitudParametros.SID_FECHA_APLICACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesembolsoCreditoSolicitudParametros.SID_APLICADO_POR), desembolso.SID_APLICADO_POR);


            return await GetAsyncFirst<DesembolsoCreditoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerCausalDesistimiento
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        public async Task<IEnumerable<ObtenerCausalDesistimiento>> ObtenerCausalDesistimiento()
        {
            return await GetAsyncList<ObtenerCausalDesistimiento>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerDatosReparto
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<DatosReparto>> ObtenerDatosReparto()
        {
            return await GetAsyncList<DatosReparto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<CreditoFinanciero> CrearSolicCreditoFinancieros(CreditoFinanciero solicitud)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.IdSolicitudCredito), solicitud.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ConceptoSalario), solicitud.SOF_CONCEPTO_SALARIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConceptoSalario), solicitud.SOF_VALOR_CONCEPTO_SALARIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto1), solicitud.SOF_CONCEPTO_1);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto1), solicitud.SOF_VALOR_CONCEPTO_1);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto2), solicitud.SOF_CONCEPTO_2);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto2), solicitud.SOF_VALOR_CONCEPTO_2);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto3), solicitud.SOF_CONCEPTO_3);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto3), solicitud.SOF_VALOR_CONCEPTO_3);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto4), solicitud.SOF_CONCEPTO_4);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto4), solicitud.SOF_VALOR_CONCEPTO_4);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto5), solicitud.SOF_CONCEPTO_5);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto5), solicitud.SOF_VALOR_CONCEPTO_5);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.Concepto6), solicitud.SOF_CONCEPTO_6);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorConcepto6), solicitud.SOF_VALOR_CONCEPTO_6);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorTotalIngresos), solicitud.SOF_VALOR_TOTAL_INGRESOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.ValorTotalEgresos), solicitud.SOF_VALOR_TOTAL_EGRESOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.CreadoPor), solicitud.SOF_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoFinancieroParams.FechaCreacion), DateTime.Now);


            return await GetAsyncFirst<CreditoFinanciero>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtiene los datos de reparto
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DatosReparto> ObtenerDatosReparto(Guid request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosRepartoParams.IdSolicitud), request);

            return await GetAsyncFirst<DatosReparto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Desistir Desembolso Solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DesistimientoDesembolso> DesistirDesembolsoSolicitud(DesistimientoDesembolso solicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesistimientoDesembolsoSolicitudParams.IdSolicitudDesistimiento), solicitud.SID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesistimientoDesembolsoSolicitudParams.IdCdd), solicitud.CDD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesistimientoDesembolsoSolicitudParams.ObservacionAnulacion), solicitud.SID_OBSERVACION_ANULACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesistimientoDesembolsoSolicitudParams.AnuladoPor), solicitud.SID_ANULADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDesistimientoDesembolsoSolicitudParams.FechaAnulacion), DateTime.Now.ToString("yyyy-MM-dd"));

            return await GetAsyncFirst<DesistimientoDesembolso>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Validar documentos flujo
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ValidacionDocumentos>> ValidarDocumentosFlujo(ValidacionDocumentos solicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.IdSolicitud), solicitud.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.IdTipoCredito), solicitud.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.Orden), solicitud.ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.Principal), solicitud.PRINCIPAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.DocSeguroVida), solicitud.DOC_SEGURO_VIDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionDocumentosParams.DocSeguroHogar), solicitud.DOC_SEGURO_HOGAR);

            return await GetAsyncList<ValidacionDocumentos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ActualizacionSolicitudCreditoDesemFirma> ActualizaSolicCreditoDesembolso(ActualizacionSolicitudCreditoDesemFirma solicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudDesembolsoParams.IdSolicitudCredito), solicitud.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudDesembolsoParams.ValidaDesembolso), solicitud.SOC_VALIDA_DESEMBOLSO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudDesembolsoParams.ObservacionValidaDesembolso), solicitud.SOC_OBSERVACION_VALIDA_DES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudDesembolsoParams.ActualizadoPor), solicitud.SOC_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudDesembolsoParams.FechaActualiza), solicitud.SOC_FECHA_ACTUALIZA);

            return await GetAsyncFirst<ActualizacionSolicitudCreditoDesemFirma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Actualiza solicitud de firmas
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<ActualizacionSolicitudCreditoDesemFirma> ActualizaSolicCreditoFirmas(ActualizacionSolicitudCreditoDesemFirma solicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudFirmasParams.IdSolicitudCredito), solicitud.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudFirmasParams.ValidaFirmas), solicitud.SOC_VALIDA_FIRMAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudFirmasParams.ObservacionValidaFirmas), solicitud.SOC_OBSERVACION_VALIDA_FIRMA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudFirmasParams.ActualizadoPor), solicitud.SOC_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudFirmasParams.FechaActualiza), solicitud.SOC_FECHA_ACTUALIZA);

            return await GetAsyncFirst<ActualizacionSolicitudCreditoDesemFirma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Actualiza el radicado de una solicitud
        /// </summary>
        /// <param name="sOC_ID"></param>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ConsultaSolicitudCredito> ActualizarRadicadoSolicitud(Guid sOC_ID, string barCode)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaRadicadoParams.idSolicitud), sOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaRadicadoParams.radicado), barCode);


            return await GetAsyncFirst<ConsultaSolicitudCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Obtener datos personales simulacion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SimulacionDatosPersonales> ObtenerSimulacionPersonales(string request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.NumeroDocumento), request);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.Indicador), 3);


            return await GetAsyncFirst<SimulacionDatosPersonales>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Obtiene cuenta por cedula
        /// </summary>
        /// <param name="clienteIdentificacion"></param>
        /// <returns></returns>
        public async Task<Cuenta> ObtenerCuentaById(string clienteIdentificacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaParameters.IdCliente), clienteIdentificacion);


            return await GetAsyncFirst<Cuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosComiteCredito> ActualizarRadicadoComiteCredito(int comiteNumero, string barCode)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.numeroComite), comiteNumero);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosComiteCreditoParams.radicado), barCode);


            return await GetAsyncFirst<DatosComiteCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosFinancierosPersonales> ObtenerDatosFinancierosPersonales(string request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosFinancierosPersonalesParams.identificacion), request);


            return await GetAsyncFirst<DatosFinancierosPersonales>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualiza5r datos personales vivienda
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public async Task<SimulacionDatosPersonales> ActualizarPersonalesVivienda(SimulacionDatosPersonales peticion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.ValorInmueble), peticion.SDP_VALOR_INMUEBLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.TipoVivienda), peticion.TIPO_VIVIENDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.DepartamentoInmueble), peticion.DPD_ID_INMUEBLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.CiudadInmueble), peticion.DPC_ID_INMUEBLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.NumeroDocumento), peticion.SDP_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.Indicador), 3);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.ValorAvaluoInmueble), peticion.SDP_VALOR_AVALUO_INMUEBLE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.UsuarioId), peticion.USR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.OpcionCompraCartera), peticion.SDP_COMPRA_CARTERA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.ValorPrestamoCartera), peticion.SDP_VALOR_PRESTAMO_COMPRA_CARTERA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.NumeroObligacionCompraCartera), peticion.SDP_NUMERO_OBLIGACION_COMPRA_CARTERA);

            return await GetAsyncFirst<SimulacionDatosPersonales>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ActualizacionSolicitudCreditoDesemFirma> ObtenerSolicitudCredito(Guid request, int indicador)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerSolicitudCredito.idSolicitud), indicador == 1 ? request : null);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerSolicitudCredito.idSls), null);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerSolicitudCredito.identificacion), indicador == 3 ? request : null);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerSolicitudCredito.indicador), indicador != 3 ? 1 : 3);

            return await GetAsyncFirst<ActualizacionSolicitudCreditoDesemFirma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SolicitudCreditosDesembolsos>> ObtenerFuentesRecursosSolicitud(Guid IdSolicitud)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdSolicitudCredito), IdSolicitud);

            return await GetAsyncList<SolicitudCreditosDesembolsos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SolicitudCreditosDesembolsos>> EliminarFuentesRecursosSolicitud(SolicitudCreditosDesembolsos solicitudCreditosDesembolsos)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdSolicitudCredito), solicitudCreditosDesembolsos.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDesembolsoParameters.IdSolicitudDesembolso), solicitudCreditosDesembolsos.SID_ID);

            return await GetAsyncList<SolicitudCreditosDesembolsos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EqivalenciaSimulador>> ObtenerEquivalenciasSimuladorCredito()
        {
            return await GetAsyncList<EqivalenciaSimulador>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<SolicitudSimulacion> CrearSolicitudSimulacionCredito(SolicitudSimulacion solicitudSimulacion)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), solicitudSimulacion.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.FechaSolicitud), solicitudSimulacion.SLS_FECHA_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.TcrId), solicitudSimulacion.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.NumeroPreAprobado), solicitudSimulacion.SLS_NUMERO_PREAPROBADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.ProblemaEmail), solicitudSimulacion.SLS_PROBLEMA_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.EnvioNotificacion), solicitudSimulacion.SLS_ENVIO_NOTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.RutaPdf), solicitudSimulacion.SLS_RUTA_PDF_RESUMEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.PorcExtraprima), solicitudSimulacion.SLS_PORC_EXTRAPRIMA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Estado), solicitudSimulacion.SLS_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.UsuarioActualiza), solicitudSimulacion.SLS_USUARIO_ACTUALIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.FechaActualiza), solicitudSimulacion.SLS_FECHA_ACTUALIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Sls_Score), solicitudSimulacion.SLS_SCORE_TU);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.EndeudamientoSCH), solicitudSimulacion.SLS_PORC_ENDEUDAMIENTO_SCH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.EndeudamientoCCH), solicitudSimulacion.SLS_PORCENDEUDAMIENTO_CCH);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SimulacionDetalle> CrearDetalleSimulacion(SimulacionDetalle simulacion)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ID), simulacion.SMD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMC_ID), simulacion.SMC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA), simulacion.SMD_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_FECHA_CORTE), simulacion.SMD_FECHA_CORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_FECHA_PAGO), simulacion.SMD_FECHA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO), simulacion.SMD_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES), simulacion.SMD_INTERESES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL), simulacion.SMD_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_VIDA), simulacion.SMD_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_TERREMOTO), simulacion.SMD_SEGURO_TERREMOTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_TOTAL), simulacion.SMD_CUOTA_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_EXTRA), simulacion.SMD_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_MENSUAL), simulacion.SMD_ABONO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_UNIDAD_PAGADORA), simulacion.SMD_CUOTA_UNIDAD_PAGADORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES_REDUCCION), simulacion.SMD_INTERESES_REDUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_REDUCCION), simulacion.SMD_CUOTA_REDUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_COBRO_FONVIVIENDA), simulacion.SMD_COBRO_FONVIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_LEASING), simulacion.SMD_SALDO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_LEASING), simulacion.SMD_INTERES_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_LEASING), simulacion.SMD_CAPITAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_LEASING), simulacion.SMD_CUOTA_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_LEASING_SUBSIDIO), simulacion.SMD_CUOTA_LEASING_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_TRADICIONAL_SUBSIDIO), simulacion.SMD_INTERES_TRADICIONAL_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_LEASING_SUBSIDIO), simulacion.SMD_INTERES_LEASING_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_VIDA_LEASING), simulacion.SMD_SEGURO_VIDA_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_REDUCIDO_LEASING), simulacion.SMD_SALDO_REDUCIDO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES_PAGADOS_LEASING), simulacion.SMD_INTERESES_PAGADOS_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_PAGADO_LEASING), simulacion.SMD_CAPITAL_PAGADO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_A_PAGAR_LEASING), simulacion.SMD_CAPITAL_A_PAGAR_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_CAPITAL_LEASING), simulacion.SMD_SALDO_CAPITAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_A_PAGAR_LEASING), simulacion.SMD_INTERES_A_PAGAR_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_TOTAL_LEASING), simulacion.SMD_SALDO_TOTAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_AFECTACION_INDIVIDUAL_LEASING), simulacion.SMD_AFECTACION_INDIVIDUAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_UNICO), simulacion.SMD_ABONO_UNICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.AFL_ID), simulacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.NUMERO_DOCUMENTO), simulacion.NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_VALOR_FINANCIAR), simulacion.SMD_VALOR_FINANCIAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGUROS_TOTALES), simulacion.SMD_SEGUROS_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SCORE), simulacion.SMD_SCORE);

            var retorno = await GetAsyncFirst<SimulacionDetalle>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return retorno;
        }

        public async Task<SMCSimulacionCliente> CrearSimulacionClienteSMC(SMCSimulacionCliente request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdSimulacionCliente), request.SMC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdSimulacion), request.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdDatosPersonales), request.SDP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoVivienda), request.SMC_TIPO_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorVivienda), request.SMC_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorViviendaSeguro), request.SMC_VALOR_VIVIENDA_SEGURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoAlivio), request.SMC_TIPO_ALIVIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoAbono), request.SMC_TIPO_ABONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.Documento), request.SMC_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamo), request.SMC_VALOR_PRESTAMO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaEa), request.SMC_VALOR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaMv), request.SMC_VALOR_TASA_MV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaFrech), request.SMC_VALOR_TASA_MV_FRESH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ViviendaVis), request.SMC_VIVIENDA_VIS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.FechaSimulacion), request.SMC_FECHA_SIMULACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.UsaRecursosAcumulados), request.SMC_USA_REC_ACUMULADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TomaSeguro), request.SMC_TOMA_SEGURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamosLeasing), request.SMC_VALOR_PRESTAMO_LSG);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.NumeroPreaprobado), request.SMC_NUMERO_PREAPROBADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TotalCuotas), request.SMC_TOTAL_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.OpcionCompra), request.SMC_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaSeguroHogar), request.SMC_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaSeguroVida), request.SMC_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamoSolicitado), request.SMC_VALOR_PRESTAMO_SOLICITADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.PorcentajeCanonInicial), request.SMC_PORCEN_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.PorcentajeOpcionCompra), request.SMC_PORCEN_OPCION_DE_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaVoluntaria), request.APORTE_VOLUNTARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.Factor), request.SMC_FACTOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.UsuarioId), request.USR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.AflId), request.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.Clc_id), request.CLC_ID);



            return await GetAsyncFirst<SMCSimulacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudSimulacion> CrearPreAprobado(SolicitudSimulacion request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), request.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.TcrId), request.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Pa_Id), request.PA_ID);
            // parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.FechaActualiza), DateTime.Now);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<SolicitudSimulacion> ConsultarPreAprobado(string numeroDocumento)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.numeroDocumento), numeroDocumento);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<SMCSimulacionCliente> ObtenerDatosSolicitudClienteByPreAprobado(SolicitudSimulacion preAprobado)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdSimulacion), preAprobado.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.NumeroPreaprobado), preAprobado.SLS_NUMERO_PREAPROBADO);

            return await GetAsyncFirst<SMCSimulacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudSimulacion> ActualizarRadicadoPreAprobado(SolicitudSimulacion preAprobado)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), preAprobado.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.NumRadicadoGD), preAprobado.SLS_NUMERO_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.RecordIdGD), preAprobado.SLS_RECORD_ID);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudSimulacion> ObtenerSolicitudSimulacionSLS(Guid simulacionId)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), simulacionId);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<SolicitudSimulacion> ObtenerSolicitudInformacionSimulacion(Guid simulacionId)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), simulacionId);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }




        public async Task<SolicitudSimulacion> ObtenerSolicitudCreditoNumeroDocumento(string numeroDocumento)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.numeroDocumento), numeroDocumento);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudSimulacion> ObtenerSolicitudSimulacionCreditoNumeroDocumento(string numeroDocumento)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.numeroDocumento), numeroDocumento);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudCredito> CrearSolicitudCredito(SolicitudCredito solicitudCredito)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ID), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_FECHA_SOLICITUD), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_NUMERO_SOLICITUD), 0);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_PROPIEDAD_HORIZONTAL), solicitudCredito.SOP_PROPIEDAD_HORIZONTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TIPO_DEPOSITO), solicitudCredito.SOP_TIPO_DEPOSITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TIPO_GARAGE), solicitudCredito.SOP_TIPO_GARAGE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TCR_ID), solicitudCredito.TCR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPD_ID), solicitudCredito.DPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPC_ID), solicitudCredito.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SLS_ID), solicitudCredito.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TID_ID), solicitudCredito.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.CLI_IDENTIFICACION), solicitudCredito.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_DECLARACION_ORIGEN_FONDOS), solicitudCredito.SOC_DECLARACION_ORIGEN_FONDOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_AUTORIZA_USO_DATOS), solicitudCredito.SOC_AUTORIZA_USO_DATOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_AUTORIZA_CONSULTA_CENTRALES), solicitudCredito.SOC_AUTORIZA_CONSULTA_CENTRALES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_DECLARACION_ASEGURABILIDAD), solicitudCredito.SOC_DECLARACION_ASEGURABILIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ESTADO), solicitudCredito.SOC_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_CREADO_POR), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_FECHA_CREACION), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_ID), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_ESTADO_INMUEBLE), solicitudCredito.SOP_ESTADO_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TIPO_INMUEBLE), solicitudCredito.SOP_TIPO_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TIENE_GARAJE), solicitudCredito.SOP_TIENE_GARAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TIENE_DEPOSITO), solicitudCredito.SOP_TIENE_DEPOSITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_CONJUNTO_CERRADO), solicitudCredito.SOP_CONJUNTO_CERRADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TVV_ID), solicitudCredito.TVV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_AÑOS_CONSTRUCCION), solicitudCredito.SOP_AÑOS_CONSTRUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_MATRICULA_INMOBILIARIA1), solicitudCredito.SOP_MATRICULA_INMOBILIARIA1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_MATRICULA_INMOBILIARIA2), solicitudCredito.SOP_MATRICULA_INMOBILIARIA2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_MATRICULA_INMOBILIARIA3), solicitudCredito.SOP_MATRICULA_INMOBILIARIA3);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_VALOR_INMUEBLE), solicitudCredito.SOP_VALOR_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_DIRECCION_INMUEBLE), solicitudCredito.SOP_DIRECCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPD_ID_INMUEBLE), solicitudCredito.DPD_ID_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPC_ID_INMUEBLE), solicitudCredito.DPC_ID_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_VALOR_CREDITO), solicitudCredito.SOP_VALOR_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_PORC_FINANCIACION), solicitudCredito.SOP_PORC_FINANCIACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_PLAZO_FINANCIACION), solicitudCredito.SOP_PLAZO_FINANCIACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_FECHA_ENTREGA_INMUEBLE), solicitudCredito.SOP_FECHA_ENTREGA_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TID_ID_VENDEDOR), solicitudCredito.TID_ID_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_NUMERO_DOCUMENTO_VENDEDOR), solicitudCredito.SOP_NUMERO_DOCUMENTO_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_NOMBRE_VENDEDOR), solicitudCredito.SOP_NOMBRE_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_DIRECCION_VENDEDOR), solicitudCredito.SOP_DIRECCION_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPD_ID_VENDEDOR), solicitudCredito.DPD_ID_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DPC_ID_VENDEDOR), solicitudCredito.DPC_ID_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_CORREO_VENDEDOR), solicitudCredito.SOP_CORREO_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_TELEFONO_VENDEDOR), solicitudCredito.SOP_TELEFONO_VENDEDOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_BARRIO_INMUEBLE), solicitudCredito.SOP_BARRIO_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_LOCALIDAD_INMUEBLE), solicitudCredito.SOP_LOCALIDAD_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_POSEE_ASENSOR), solicitudCredito.SOP_POSEE_ASENSOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOP_PISO_INMUEBLE), solicitudCredito.SOP_PISO_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.UsuarioID), solicitudCredito.USR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.DepartamentoNotariaId), solicitudCredito.DPD_DEPARTAMENTO_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.CiudadNotariaId), solicitudCredito.DPC_CIUDAD_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_ACTIVO), solicitudCredito.FID_ACTIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_TIPO_DOCUMENTO), solicitudCredito.FID_TIPO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_NUMERO_DOCUMENTO), solicitudCredito.FID_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_RAZON_SOCIAL), solicitudCredito.FID_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_DIRECCION), solicitudCredito.FID_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_DEPARTAMENTO), solicitudCredito.FID_DEPARTAMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_CIUDAD), solicitudCredito.FID_CIUDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_CORREO), solicitudCredito.FID_CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.FID_TELEFONO), solicitudCredito.FID_TELEFONO);
            return await GetAsyncFirst<SolicitudCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SocSolicitudInfobasica> CrearSolicCreditoBasicos(SocSolicitudInfobasica infoBasica)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOC_ID), infoBasica.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.TID_ID), infoBasica.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_NUMERO_DOCUMENTO), infoBasica.SOB_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_FECHA_EXPEDICION), infoBasica.SOB_FECHA_EXPEDICION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPD_ID_EXP), infoBasica.DPD_ID_EXP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPC_ID_EXP), infoBasica.DPC_ID_EXP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_PRIMER_APELLIDO), infoBasica.SOB_PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_SEGUNDO_APELLIDO), infoBasica.SOB_SEGUNDO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_PRIMER_NOMBRE), infoBasica.SOB_PRIMER_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_SEGUNDO_NOMBRE), infoBasica.SOB_SEGUNDO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_FECHA_NACIMIENTO), infoBasica.SOB_FECHA_NACIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPD_ID_NACIMIENTO), infoBasica.DPD_ID_NACIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPC_ID_NACIMIENTO), infoBasica.DPC_ID_NACIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_SEXO), infoBasica.SOB_SEXO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_ESTADO_CIVIL), infoBasica.SOB_ESTADO_CIVIL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_NIVEL_EDUCACION), infoBasica.SOB_NIVEL_EDUCACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.FRZ_ID), infoBasica.FRZ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.CTG_ID), infoBasica.CTG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.GRD_ID), infoBasica.GRD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPD_ID_RESIDENCIA), infoBasica.DPD_ID_RESIDENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPC_ID_RESIDENCIA), infoBasica.DPC_ID_RESIDENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_DIRECCION_RESIDENCIA), infoBasica.SOB_DIRECCION_RESIDENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_CORREO_PERSONAL), infoBasica.SOB_CORREO_PERSONAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_TELEFONO_RESIDENCIA), infoBasica.SOB_TELEFONO_RESIDENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_CELULAR), infoBasica.SOB_CELULAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPD_ID_OFICINA), infoBasica.DPD_ID_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPC_ID_OFICINA), infoBasica.DPC_ID_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_DIRECCION_OFICINA), infoBasica.SOB_DIRECCION_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_CORREO_INSTITUCIONAL), infoBasica.SOB_CORREO_INSTITUCIONAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_TELEFONO_OFICINA), infoBasica.SOB_TELEFONO_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.ACC_ID), infoBasica.ACC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.PRF_ID), infoBasica.PRF_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_CREADO_POR), infoBasica.SOB_CREADO_POR);

            return await GetAsyncFirst<SocSolicitudInfobasica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<TasaHogarCiudad>> ObtenerTasasHogarCiudad()
        {
            return await GetAsyncList<TasaHogarCiudad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TasaHogarCiudad>> ObtenerTasasHogarUbicacion(int departamentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerTasasHogarUbicacion.DPD_ID), departamentoId);

            return await GetAsyncList<TasaHogarCiudad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<InformacionConyugue> ObtenerSolicCreditoConyugue(PeticionCreditoBasica IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.SOY_ID), IdSolicitud.SOY_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.SOC_ID), IdSolicitud.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCreditoBasicaParameters.IND), IdSolicitud.IND);

            return await GetAsyncFirst<InformacionConyugue>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InformacionConyugue> CrearSolicCreditoConyugue(InformacionConyugue info)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOC_ID), info.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.TID_ID), info.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_NUMERO_DOCUMENTO), info.SOY_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.DPD_ID_EXP), info.DPD_ID_EXP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.DPC_ID_EXP), info.DPC_ID_EXP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_PRIMER_APELLIDO), info.SOY_PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_SEGUNDO_APELLIDO), info.SOY_SEGUNDO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_PRIMER_NOMBRE), info.SOY_PRIMER_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_SEGUNDO_NOMBRE), info.SOY_SEGUNDO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_FECHA_NACIMIENTO), info.SOY_FECHA_NACIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.DPD_ID_NACIMIENTO), info.DPD_ID_NACIMIENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_CELULAR), info.SOY_CELULAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_CORREO_PERSONAL), info.SOY_CORREO_PERSONAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_ACTIVIDAD_LABORAL), info.SOY_ACTIVIDAD_LABORAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_OTRO_ACTIVIDAD), info.SOY_OTRO_ACTIVIDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_EMPRESA_LABORA), info.SOY_EMPRESA_LABORA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_CREADO_POR), info.SOY_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOY_FECHA_EXPEDICION), info.SOY_FECHA_EXPEDICION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.DPC_ID_NACIMIENTO), info.DPC_ID_NACIMIENTO);

            return await GetAsyncFirst<InformacionConyugue>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CierreDiarioCartera>> CierreDiarioCartera(RequestCierreDiarioCartera request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCierreDiarioParams.ActualizaDatos), request.Actualiza);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCierreDiarioParams.FechaCierreDiario), request.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCierreDiarioParams.CreadoPor), request.CreadoPor);

            return await GetAsyncList<CierreDiarioCartera>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausacionDiariaInteresesCorrientes>> CausacionDiariaInteresCorriente(RequestCausacionDiariaInteresesCorrientes request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.ActualizaDatos), request.Actualiza);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.FechaCierreDiario), request.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.CreadoPor), request.CreadoPor);
            try
            {
                return await GetAsyncList<CausacionDiariaInteresesCorrientes>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public async Task<IEnumerable<CausacionDiariaInteresesCorrientes>> RecuperacionCausacionDiariaInteresCorriente(RequestCausacionDiariaInteresesCorrientes request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.ActualizaDatos), request.Actualiza);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.FechaCierreDiario), request.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.CreadoPor), request.CreadoPor);

            return await GetAsyncList<CausacionDiariaInteresesCorrientes>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<CausacionDiariaInteresesCorrientes>> RecuperacionCausacionDiariaInteresesMoratorio(RequestCausacionDiariaInteresesCorrientes request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.ActualizaDatos), request.Actualiza);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.FechaCierreDiario), request.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCausacionDiariaInteresesCorrientes.CreadoPor), request.CreadoPor);

            return await GetAsyncList<CausacionDiariaInteresesCorrientes>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoCierreCartera>> ObtenerTiposDeCierreCartera()
        {
            DynamicParameters parameters = new();
            return await GetAsyncList<TipoCierreCartera>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CierresCausacionesRealizadas>> ObtenerCierresCausacionesRealizadas(RequestCierresCausacionesRealizadas request)
        {
            DynamicParameters parameters = new();
            parameters.Add(EnumCierresCausacionesRealizadas.TiposCierre.GetDescription(),
                request.TiposCierre != null
                ? string.Join(',', request.TiposCierre)
                : null
            );
            parameters.Add(EnumCierresCausacionesRealizadas.FechaCierreInicial.GetDescription(), request.FechaCierreInicial);
            parameters.Add(EnumCierresCausacionesRealizadas.FechaCierreFinal.GetDescription(), request.FechaCierreFinal);

            return await GetAsyncList<CierresCausacionesRealizadas>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure
            );
        }

        public async Task<int> ObtenerSeguroPorUbicacion(int dpcId, int dpdId)
        {
            DynamicParameters parameters = new();
            parameters.Add("@DPC_ID", dpcId);
            parameters.Add("@DPD_ID", dpdId);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<SolicitudCredito> ObtenerSolicitudCreditoTipoNumeroIdentificacion(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TID_ID), tipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.CLI_IDENTIFICACION), numeroDocumento);
            return await GetAsyncFirst<SolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }




        public async Task<bool> RegistroLogTUSimulador(string breakPoint, string request, string response)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@LOG_BREAK_POINT", breakPoint);
            parameters.Add("@LOG_REQUEST", request);
            parameters.Add("@LOG_RESPONSE", response);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<SolicitudCredito> ObtenerInformacionSolicitudCreditoCartera(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TID_ID), tipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.CLI_IDENTIFICACION), numeroDocumento);
            return await GetAsyncFirst<SolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SocSolicitudInfobasica> ActualizaSolicCreditoBasica(SocSolicitudInfobasica infoBasica)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_ID), infoBasica.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOC_ID), infoBasica.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_TIENE_TRANSACCION_ME), infoBasica.SOB_TIENE_TRANSACCION_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_TIPO_TRANSACCION_ME), infoBasica.SOB_TIPO_TRANSACCION_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_TIPO_PRODUCTO_ME), infoBasica.SOB_TIPO_PRODUCTO_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_BANCO_TRANSACCION_ME), infoBasica.SOB_BANCO_TRANSACCION_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_NUMERO_PRODUCTO_ME), infoBasica.SOB_NUMERO_PRODUCTO_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_SALDO_PROMEDIO_CUENTAS_ME), infoBasica.SOB_SALDO_PROMEDIO_CUENTAS_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_MONEDA_TRANSACCION_ME), infoBasica.SOB_MONEDA_TRANSACCION_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPP_ID_ME), infoBasica.DPP_ID_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.DPD_ID_ME), infoBasica.DPD_ID_ME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_ACTUALIZADO_POR), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSocSolicitudInfobasicaParametros.SOB_FECHA_ACTUALIZA), DateTime.Now);


            return await GetAsyncFirst<SocSolicitudInfobasica>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SolicCreditoRefererncias> CrearSolicCreditoRefererncias(SolicCreditoRefererncias data)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOC_ID), data.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_TIPO_REFERENCIA), data.SOR_TIPO_REFERENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_PRIMER_APELLIDO), data.SOR_PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_SEGUNDO_APELLIDO), data.SOR_SEGUNDO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_NOMBRES), data.SOR_NOMBRES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_PARENTESCO), data.SOR_PARENTESCO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_RELACION), data.SOR_RELACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_DIRECCION), data.SOR_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.DPD_ID), data.DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.DPC_ID), data.DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_TELEFONO), data.SOR_TELEFONO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_CELULAR), data.SOR_CELULAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_CREADO_POR), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_FECHA_CREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_REFERENCIA), data.SOR_REFERENCIA);

            return await GetAsyncFirst<SolicCreditoRefererncias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<DocumentoCargarSolicitud>> ObtenerDocumentosRequeridosSolicCredito()
        {
            return await GetAsyncList<DocumentoCargarSolicitud>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<SolicCreditoFinancieros> ActualizaSolicCreditoFinancieros(SolicCreditoFinancieros data)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_ID), data.SOF_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOC_ID), data.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_DIRECCION_VIVIENDA), data.SOF_DIRECCION_VIVIENDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_MATRICULA_VIVIENDA), data.SOF_MATRICULA_VIVIENDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_VALOR_COMERCIAL_VIVIENDA), data.SOF_VALOR_COMERCIAL_VIVIENDA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_MARCA_VEHICULO), data.SOF_MARCA_VEHICULO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_PLACA_VEHICULO), data.SOF_PLACA_VEHICULO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_VALOR_COMERCIAL_VEHICULO), data.SOF_VALOR_COMERCIAL_VEHICULO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_ACTUALIZADO_POR), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOF_FECHA_ACTUALIZA), DateTime.Now);

            return await GetAsyncFirst<SolicCreditoFinancieros>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<SolicCreditoRefererncias>> ObtenerSolicCreditoReferencias(Guid ID_SOC)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOC_ID), ID_SOC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.IND), 2);
            return await GetAsyncList<SolicCreditoRefererncias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);


        }

        public async Task<SolicCreditoRefererncias> ObtenerSolicCreditoReferenciasPorTipo(Guid ID_SOC, string tipoReferencia)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOR_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.SOC_ID), ID_SOC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.IND), 2);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReferenciasParameter.TIPO_REFERENCIA), tipoReferencia);

            return await GetAsyncFirst<SolicCreditoRefererncias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);


        }

        public async Task<SolicitudCredito> ActualizaSolicCreditoAutorizacionDatos(SolicitudCredito solic)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ID), solic.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_DECLARACION_ORIGEN_FONDOS), solic.SOC_DECLARACION_ORIGEN_FONDOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_AUTORIZA_USO_DATOS), solic.SOC_AUTORIZA_USO_DATOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_AUTORIZA_CONSULTA_CENTRALES), solic.SOC_AUTORIZA_CONSULTA_CENTRALES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_DECLARACION_ASEGURABILIDAD), solic.SOC_DECLARACION_ASEGURABILIDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_CONVENIO_ASEGURADORA), solic.SOC_CONVENIO_ASEGURADORA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_CONVENIO_ASEGURADORA_HOGAR), solic.SOC_CONVENIO_ASEGURADORA_HOGAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ASE_ID), solic.ASE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ASE_ID_HOGAR), solic.ASE_ID_HOGAR);

            return await GetAsyncFirst<SolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseDocumentoSolicCredito> ObtenerDocumentosSubidosSolicCredito(RequestDocumentosSubidosProspeccion request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerDocumentosCargadosParams.TipoDocumento), request.TipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerDocumentosCargadosParams.IdSolicitud), request.IdSolicitud);

            return await GetAsyncFirst<ResponseDocumentoSolicCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<EstadoSolicCredito> ObtenerEstadoSolicCredito(string documento)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SMC_NUMERO_DOCUMENTO), documento);
            return await GetAsyncFirst<EstadoSolicCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task LimpiarSimulacionCliente(Guid simulacionId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@SMC_ID", simulacionId);
            await GetAsyncFirst<object>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ConsultaSolicitudCredito> CrearNoSolicitudCredito(ConsultaSolicitudCredito solic)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ID), solic.SOC_ID);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_SOCRE), solic.SOC_SCORE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ESTADO), solic.SOC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_ACTUALIZADO_POR), solic.SOC_ACTUALIZADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.SOC_FECHA_ACTUALIZA), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.UsuarioID), solic.USR_ID);

            return await GetAsyncFirst<ConsultaSolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<RespuestaSolicitudObtenerTramite> ObtenerTramiteSolicitudCli(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumTramiteSolicitudParameters.identificacion), identificacion);
            return await GetAsyncFirst<RespuestaSolicitudObtenerTramite>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> ObtenerPeritos()
        {
            return await GetAsyncList<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ResponseDocumentoSolicCredito>> ObtenerDocumentosSolicCredito(Guid sOC_ID)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSolicCreditoParams.idSolicitudCredito), sOC_ID);
            return await GetAsyncList<ResponseDocumentoSolicCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<TramiteSolicitud> ActualizaTramitePerito(RequestTramiteSolicitud request)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TRS_ID), request.TRS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ACT_ID), request.ACT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TRS_OBSERVACION), request.TRS_OBSERVACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ID_USUARIO_PERITO), request.ID_USUARIO_PERITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TRS_MODIFICADO_POR), request.TRS_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TRS_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<TramiteSolicitud>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseActividadTramite> SubsanarActividadTramite(RequestActividadTramite request)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.TRS_ID), request.TRS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ACT_ID), request.ACT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ACT_OBSERVACION), request.ACT_OBSERVACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ACT_FECHA_MODIFICACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.ACT_MODIFICADO_POR), request.ACT_MODIFICADO_POR);

            return await GetAsyncFirst<ResponseActividadTramite>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParametroIntegracion>> ObtenerParametrosIntegraciones()
        {
            return await GetAsyncList<ParametroIntegracion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

        }

        public async Task<RespuestaVigiaParam> ConsultarResponseVigiaDesactivado()
        {
            return await GetAsyncFirst<RespuestaVigiaParam>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<InsertarNotificacionFuncionarioExternoResp>> InsertarNotificacionFuncionarioExterno(InsertarNotificacionFuncionarioExterno request)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.Perfil), request.Perfil);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.Mensaje), request.Mensaje);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.Modulo), request.Modulo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCreacionSolicitudCreditoParams.UsuarioEnvia), request.UsuarioEnvia);



            return await GetAsyncList<InsertarNotificacionFuncionarioExternoResp>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<PreaprobadoCliente>> SimulacionCliente(string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSimulacionClienteParams.numeroDocumento), numeroDocumento);

            return await GetAsyncList<PreaprobadoCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InformacionConyugue> EliminarInformacionSolicConyuge(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoConyugueParametros.SOC_ID), solicitudId);

            return await GetAsyncFirst<InformacionConyugue>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Preaprobaciones>> ObtenerPreaprobaciones(string? NumeroDocumento, string? estado, DateTime? fecha, DateTime? fechaf)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPreapobadosParameters.SMC_NUMERO_DOCUMENTO), NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPreapobadosParameters.SLS_ESTADO), estado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPreapobadosParameters.SMC_FECHA_SIMULACION), fecha);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPreapobadosParameters.FECHAFIN), fechaf);
            return await GetAsyncList<Preaprobaciones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseDatosSimulacionParaEstudio> ObtenerDatosSimulacionParaEstudio(string numeroDocumento)
        {

            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPreapobadosParameters.SMC_NUMERO_DOCUMENTO), numeroDocumento);

            return await GetAsyncFirst<ResponseDatosSimulacionParaEstudio>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerParametrosSeguroUbicacion>> ObtenerParametrosSeguroUbicacion()
        {
            return await GetAsyncList<ObtenerParametrosSeguroUbicacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ParametroSeguroRegion>> ObtenerParametrosporSeguroRegion()
        {
            return await GetAsyncList<ParametroSeguroRegion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParametroSeguroRegionGui>> ActualizarSeguroPorRegion(int? id, double? seguro)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSeguroRegionParametros.REG_ID), id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSeguroRegionParametros.REG_SEGURO), seguro);
            return await GetAsyncList<ParametroSeguroRegionGui>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResultadoIntegracionDto> InsertarResultadoIntegracion(ResultadoIntegracionDto resultado)
        {
            DynamicParameters parameters = new DynamicParameters();

            // parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.PIN_ID), resultado.Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.TID_ID), resultado.TipoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.PIN_NRO_IDENTIFICACION), resultado.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.PIN_PARAM_ID), resultado.ParamId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.PIN_FECHA_EJECUCION), resultado.FechaEjecucion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.PIN_RESULTADO_INTEGRACION), resultado.Resultado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumResultadoIntegracionParametros.USR_ID), resultado.CreadoPor);

            return await GetAsyncFirst<ResultadoIntegracionDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        async Task<ProyeccionCliente> ICreditoRepository.ObtenerProyeccionCliente(string Id)
        {
            DynamicParameters parametros = new();
            parametros.Add("@GRD_ID", Id);
            parametros.Add("@GRD_ANIO", DateTime.Now.Year);
            return await GetAsyncFirst<ProyeccionCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ProyeccionCliente> ObtenerProyeccionCliente(string Id, int year)
        {
            DynamicParameters parametros = new();
            parametros.Add("@GRD_ID", Id);
            parametros.Add("@GRD_ANIO", year);
            return await GetAsyncFirst<ProyeccionCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SmlvAnualDto> ObtenerSalarioMinimoAnual(ObtenerSmlvAnualDto anio)
        {
            DynamicParameters parametros = new();
            parametros.Add("@SMLV_ANIO", anio.SMLV_ANIO);
            parametros.Add("@CTG_ID", anio.CTG_ID);
            return await GetAsyncFirst<SmlvAnualDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProyeccionCliente>> ObtenerSimCargo(string request)
        {
            DynamicParameters parametros = new();
            parametros.Add("@CRG_ID", request);
            parametros.Add("@CRG_ANIO", 2026);
            return await GetAsyncList<ProyeccionCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<MatriculaCreditoDto> CrearMatriculaCredito(MatriculaCreditoDto resultado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.IdMatricula), resultado.IdMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.NumeroMatricula), resultado.NumeroMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.FechaMatricula), resultado.FechaMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.Chip), resultado.Chip);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.CedulaCatastral), resultado.CedulaCatastral);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaPrivadaConstruida), resultado.AreaPrivadaConstruida);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaAvaluoComercial), resultado.AreaAvaluoComercial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaAvaluoCatastral), resultado.AreaAvaluoCatastral);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.SocId), resultado.SocId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.Tipo_Matricula), resultado.Tipo_Matricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaConstruidaDescripcion), resultado.AreaConstruida);

            return await GetAsyncFirst<MatriculaCreditoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MatriculaCreditoDto>> ObtenerMatriculasCredito()
        {
            return await GetAsyncList<MatriculaCreditoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<MatriculaCreditoDto> EliminarMatriculasCreditoSolicitud(Guid? solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.IdMatricula), solicitudId);

            return await GetAsyncFirst<MatriculaCreditoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalesRechazo>> ObtenerCausalesRechazo()
        {
            return await GetAsyncList<CausalesRechazo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ActividadTramiteSolicitud>> ObtenerActividadTramite()
        {
            return await GetAsyncList<ActividadTramiteSolicitud>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadoTramites>> ObtenerEstadosTramite()
        {
            return await GetAsyncList<EstadoTramites>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TramitesPendientes>> ObtenerTramitesPendientes()
        {
            return await GetAsyncList<TramitesPendientes>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        async Task<IEnumerable<MatriculaCreditoDto>> ICreditoRepository.ActualizarMatriculasCredito(MatriculaCreditoDto resultado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.IdMatricula), resultado.IdMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.NumeroMatricula), resultado.NumeroMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.FechaMatricula), resultado.FechaMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.Chip), resultado.Chip);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.CedulaCatastral), resultado.CedulaCatastral);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaPrivadaConstruida), resultado.AreaPrivadaConstruida);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaAvaluoComercial), resultado.AreaAvaluoComercial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.AreaAvaluoCatastral), resultado.AreaAvaluoCatastral);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.SocId), resultado.SocId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaCreditoParametros.Tipo_Matricula), resultado.Tipo_Matricula);

            return await GetAsyncList<MatriculaCreditoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MatriculaCreditoDto>> ObtenerMatriculasCreditoporSolicitud(Guid? solicitudId)
        {
            DynamicParameters parametros = new();
            parametros.Add("@SocId", solicitudId);
            return await GetAsyncList<MatriculaCreditoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> ActualizaMPRCONTSolicitudCredito(Guid idSolicitud, int mpr_cont, int tipo_cdp)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SOC_ID", idSolicitud);
            parameters.Add("@MPR_CONT", mpr_cont);
            parameters.Add("@TIPO_CDP", tipo_cdp);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizaFACCONTSolicitudCredito(RequestAcualizarFacContDto request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SOC_ID", request.idSolicitud);
            parameters.Add("@FAC_CONT", request.fac_cont);
            parameters.Add("@FAC_NUME", request.fac_nume);
            parameters.Add("@TIPO_PAGO", request.tipo_pago);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizarEstadoDesembolso(Guid idSolicitud, decimal saldoDesembolsado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SOC_ID", idSolicitud);
            parameters.Add("@SALDO_DESEMBOLSO", saldoDesembolsado);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizarCodProveedorSevenSolicitudCredito(Guid idSolicitud, int ProveCod, int tipo_pv)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SOC_ID", idSolicitud);
            parameters.Add("@PVD_CODD", ProveCod);
            parameters.Add("@TIPO_PV", tipo_pv);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldos()
        {
            return await GetAsyncList<ObtenerOrdenDescuentos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, typeCommand: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldosCredito()
        {
            return await GetAsyncList<ObtenerOrdenDescuentos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, typeCommand: CommandType.StoredProcedure);
        }
        public async Task<User> ConsultaUsuarioActaComite(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaUsuarioActaComite.idUser), user.USR_ID);
            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IntegranteComite> CrearIntegranteComiteCredito(IntegranteComite Integrante)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Id), Integrante.ICO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Nombre), Integrante.ICO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Cargo), Integrante.ICO_CARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Tipo), Integrante.ICO_TIPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Creado_Por), Integrante.ICO_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Estado), Integrante.ICO_ESTADO);
            return await GetAsyncFirst<IntegranteComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IntegranteComite> ActualizarIntegranteComiteCredito(IntegranteComite Integrante)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Id), Integrante.ICO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Nombre), Integrante.ICO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Cargo), Integrante.ICO_CARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Tipo), Integrante.ICO_TIPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Actualizado_Por), Integrante.ICO_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Estado), Integrante.ICO_ESTADO);
            return await GetAsyncFirst<IntegranteComite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarIntegranteComiteCredito(long Id_Integrante)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegranteComiteCredito.Ico_Id), Id_Integrante);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosActSimulacion(DocumentosActSimulacion request)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParametersSimulacion.AFL_ID), request.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParametersSimulacion.SLS_ID), request.SLS_ID);

            return await GetAsyncList<DocumentosActRespuesta>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResponseDocumentoSimulacion> CrearDocumentoSimulacion(ResponseDocumentoSimulacion documentoSolicCredito)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.idDocumentoSimulacion), documentoSolicCredito.DSC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.idSimulacion), documentoSolicCredito.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.idTipoDocumento), documentoSolicCredito.TDC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.codigoBarras), documentoSolicCredito.DSC_CODIGO_BARRAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.fechaDocumento), documentoSolicCredito.DSC_FECHA_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.numeroFolios), documentoSolicCredito.DSC_NUMERO_FOLIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.tamaño), documentoSolicCredito.DSC_TAMAÑO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.rutaImagen), documentoSolicCredito.DSC_RUTA_IMAGEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.estado), documentoSolicCredito.DSC_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.creadoPor), documentoSolicCredito.DSC_CREADO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentoSimulacionParams.fechaCreacion), documentoSolicCredito.DSC_FECHA_CREACION);

            return await GetAsyncFirst<ResponseDocumentoSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<UpdateSlsTu> ActualizarSlsReporteTU(UpdateSlsTu requestUpdateSlsTuDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumReporteTuSlsParametros.SLS_ID), requestUpdateSlsTuDto.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumReporteTuSlsParametros.XML), requestUpdateSlsTuDto.SLS_XML);

            return await GetAsyncFirst<UpdateSlsTu>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<int> ConsultarVigenciaSimulacion(string Request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVigenciaSimulacion.NUMERO_IDENTIFICACION), Request);


            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ValoresProyectados> GuardarValoresProyectados(ValoresProyectados valoresProyectados)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_Proyectado), valoresProyectados.SVP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Simulacion_ID), valoresProyectados.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_Sueldo), valoresProyectados.SVP_SUELDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_Ahorros), valoresProyectados.SVP_AHORROS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_Cesantias), valoresProyectados.SVP_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_Subsidio), valoresProyectados.SVP_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Sim_Valores_EA), valoresProyectados.SVP_EA);

            return await GetAsyncFirst<ValoresProyectados>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ValoresProyectados> ObtenerValoresProyectados(Guid RequestSLS_ID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresProyectados.Simulacion_ID), RequestSLS_ID);

            var Response = await GetAsyncFirst<ValoresProyectados>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            if (Response == null)
            {
                Response = new ValoresProyectados();
            }
            return Response;
        }


        public async Task<SimulacionDatosPersonales> ActualizarDatosPersonalesSolicitud(SimulacionDatosPersonales peticion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_ID), peticion.SDP_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.CTG_ID), peticion.CTG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.GRD_ID), peticion.GRD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_NOMBRES_APELLIDOS), peticion.SDP_NOMBRES_APELLIDOS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_EDAD), peticion.SDP_EDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.DPD_ID), peticion.DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.DPC_ID), peticion.DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_DIRECCION), peticion.SDP_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_TELEFONO_CELULAR), peticion.SDP_TELEFONO_CELULAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_TELEFONO_FIJO), peticion.SDP_TELEFONO_FIJO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_E_MAIL), peticion.SDP_E_MAIL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_CUOTAS), peticion.SDP_CUOTAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_REGIMEN), peticion.SDP_REGIMEN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_USUARIO_ACTUALIZA), peticion.SDP_USUARIO_ACTUALIZA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_FECHA_ACTUALIZA), peticion.SDP_FECHA_ACTUALIZA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudDatosPersonalesParams.SDP_NUMERO_DOCUMENTO), peticion.SDP_NUMERO_DOCUMENTO);

            return await GetAsyncFirst<SimulacionDatosPersonales>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CheckArchivo> CheckArchivosCredito(Guid idSolicitud, string nombreArchivo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoArchivos.SolicitudCreditoId), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoArchivos.NombreArchivo), nombreArchivo);

            return await GetAsyncFirst<CheckArchivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<bool> EliminarCheckArchivosCredito(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoArchivos.SolicitudCreditoId), idSolicitud);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<CheckArchivo>> ObtenerArchivosCheckCredito(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoArchivos.SolicitudCreditoId), idSolicitud);


            return await GetAsyncList<CheckArchivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ArchivosCredito>> ConsultarArchivosPorTipoCredito()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<ArchivosCredito>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }


        public async Task<IEnumerable<MediosCreditoVivienda>> ConsultarMediosCreditosVivienda()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<MediosCreditoVivienda>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<MediosCreditoVivienda> ObtenerMediosCreditosViviendaById(Guid idMedioCreditoVivienda)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMedioCreditoVivienda.MCV_ID), idMedioCreditoVivienda);

            return await GetAsyncFirst<MediosCreditoVivienda>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<MediosRecepcion> ObtenerMediosRecepcionById(int idMedioRecepcion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMedioRecepcion.MR_ID), idMedioRecepcion);

            return await GetAsyncFirst<MediosRecepcion>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<DatosAdicionalesSimulacion> ActualizarDatosAdicionalesSimulacion(DatosAdicionalesSimulacion datosSimulacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), datosSimulacion.SMC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.NumerosFolios), datosSimulacion.NUM_FOLIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.MediosCreditoVivienda), datosSimulacion.MEDIOS_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.MedioRecepcion), datosSimulacion.MR_ID);

            DatosAdicionalesSimulacion response = new DatosAdicionalesSimulacion();
            response = await GetAsyncFirst<DatosAdicionalesSimulacion>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<InformacionProspeccion> ConsultarProspeccionPorNumeroDocumento(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.tipoDocumento), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<InformacionProspeccion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ValoresProspeccion> ConsultarTasasGeneralesPorAfiliado(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.tipoDocumento), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<ValoresProspeccion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarValoresSimulacionAfiliado(ValoresSimulacionAfiliadoDto valoresSimulacionAfiliado)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.RiesgoMinimo), valoresSimulacionAfiliado.RiesgoMinimo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.ValorFinanciar), valoresSimulacionAfiliado.ValorFinanciar);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.Sms_id), valoresSimulacionAfiliado.SMS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.NumeroDocumento), valoresSimulacionAfiliado.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.FechaRegistro), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.ValorSectorFinanciero), valoresSimulacionAfiliado.ValorSectorFinanciero);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.ValorSectorReal), valoresSimulacionAfiliado.ValorSectorReal);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumGuardarValoresSimulacion.Factor), valoresSimulacionAfiliado.Factor);


            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<DatosProspeccionEndudamiento> ConsultarProspeccionEndudamiento(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.tipoDocumento), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<DatosProspeccionEndudamiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<LineaCreditoObligacion>> InsertarLineaCreditoObligacion(LineasCreditoObligacionDto lineasCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.LineaCredito), lineasCredito.OLN_LINEA_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.SaldoObligacion), lineasCredito.OLN_SALDO_OBLIGACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.ValorCuota), lineasCredito.OLN_VALOR_CUOTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.EstadoLineaCredito), lineasCredito.OLN_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.CuotasPactadas), lineasCredito.OLN_CUOTAS_PACTADAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.CuotasCanceladas), lineasCredito.OLN_CUOTAS_CANCELADAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.CuotasRestantes), lineasCredito.OLN_CUOTAS_RESTANTES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.NumeroDocumento), lineasCredito.OLN_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.Calidad), lineasCredito.OLN_CALIDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.NumeroObligacion), lineasCredito.OLN_NUMERO_OBLIGACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarLineasCredito.TipoObligacion), lineasCredito.OLN_TIPO_OBLIGACION);
            return await GetAsyncList<LineaCreditoObligacion>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public async Task<IEnumerable<LineaCreditoObligacion>> ConsultarLineaCreditoObligacion(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.tipoDocumento), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncList<LineaCreditoObligacion>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<bool> LimpiarLineasCreditoObligacion(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.tipoDocumento), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarPropeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SimulacionPlanCliente> ObtenerDatosSimulacionClienteSPC(DatosSimulacionUsuarioDto datosSimulacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarSimulacionAfiliado.Afl_id), datosSimulacion.Afl_id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarSimulacionAfiliado.NumeroDocumento), datosSimulacion.Numero_Documento);

            return await GetAsyncFirst<SimulacionPlanCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SimulacionDetallePlanCliente>> ObtenerDetalleSimulacionClienteSPD(DatosSimulacionUsuarioDto datosSimulacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarSimulacionAfiliado.Afl_id), datosSimulacion.Afl_id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConsultarSimulacionAfiliado.NumeroDocumento), datosSimulacion.Numero_Documento);

            return await GetAsyncList<SimulacionDetallePlanCliente>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SMCSimulacionCliente> RestaurarSimulacionCliente(SMCSimulacionCliente request)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdSimulacionCliente), request.SMC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdSimulacion), request.SLS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.IdDatosPersonales), request.SDP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoVivienda), request.SMC_TIPO_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorVivienda), request.SMC_VALOR_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorViviendaSeguro), request.SMC_VALOR_VIVIENDA_SEGURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoAlivio), request.SMC_TIPO_ALIVIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TipoAbono), request.SMC_TIPO_ABONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.Documento), request.SMC_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamo), request.SMC_VALOR_PRESTAMO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaEa), request.SMC_VALOR_TASA_EA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaMv), request.SMC_VALOR_TASA_MV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaFrech), request.SMC_VALOR_TASA_MV_FRESH);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ViviendaVis), request.SMC_VIVIENDA_VIS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.FechaSimulacion), request.SMC_FECHA_SIMULACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.UsaRecursosAcumulados), request.SMC_USA_REC_ACUMULADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TomaSeguro), request.SMC_TOMA_SEGURO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamosLeasing), request.SMC_VALOR_PRESTAMO_LSG);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.NumeroPreaprobado), request.SMC_NUMERO_PREAPROBADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TotalCuotas), request.SMC_TOTAL_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.OpcionCompra), request.SMC_OPCION_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaSeguroHogar), request.SMC_TASA_SEGURO_HOGAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaSeguroVida), request.SMC_TASA_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.ValorPrestamoSolicitado), request.SMC_VALOR_PRESTAMO_SOLICITADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.PorcentajeCanonInicial), request.SMC_PORCEN_CANON_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.PorcentajeOpcionCompra), request.SMC_PORCEN_OPCION_DE_COMPRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.TasaVoluntaria), request.APORTE_VOLUNTARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.Factor), request.SMC_FACTOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.UsuarioId), request.USR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSMCParameters.AflId), request.AFL_ID);



            return await GetAsyncFirst<SMCSimulacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SimulacionDetalle> RestaurarDetallePlanCliente(SimulacionDetalle simulacion)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ID), simulacion.SMD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMC_ID), simulacion.SMC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA), simulacion.SMD_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_FECHA_CORTE), simulacion.SMD_FECHA_CORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_FECHA_PAGO), simulacion.SMD_FECHA_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO), simulacion.SMD_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES), simulacion.SMD_INTERESES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL), simulacion.SMD_CAPITAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_VIDA), simulacion.SMD_SEGURO_VIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_TERREMOTO), simulacion.SMD_SEGURO_TERREMOTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_TOTAL), simulacion.SMD_CUOTA_TOTAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_EXTRA), simulacion.SMD_ABONO_EXTRA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_MENSUAL), simulacion.SMD_ABONO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_UNIDAD_PAGADORA), simulacion.SMD_CUOTA_UNIDAD_PAGADORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES_REDUCCION), simulacion.SMD_INTERESES_REDUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_REDUCCION), simulacion.SMD_CUOTA_REDUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_COBRO_FONVIVIENDA), simulacion.SMD_COBRO_FONVIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_LEASING), simulacion.SMD_SALDO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_LEASING), simulacion.SMD_INTERES_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_LEASING), simulacion.SMD_CAPITAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_LEASING), simulacion.SMD_CUOTA_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CUOTA_LEASING_SUBSIDIO), simulacion.SMD_CUOTA_LEASING_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_TRADICIONAL_SUBSIDIO), simulacion.SMD_INTERES_TRADICIONAL_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_LEASING_SUBSIDIO), simulacion.SMD_INTERES_LEASING_SUBSIDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SEGURO_VIDA_LEASING), simulacion.SMD_SEGURO_VIDA_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_REDUCIDO_LEASING), simulacion.SMD_SALDO_REDUCIDO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERESES_PAGADOS_LEASING), simulacion.SMD_INTERESES_PAGADOS_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_PAGADO_LEASING), simulacion.SMD_CAPITAL_PAGADO_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_CAPITAL_A_PAGAR_LEASING), simulacion.SMD_CAPITAL_A_PAGAR_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_CAPITAL_LEASING), simulacion.SMD_SALDO_CAPITAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_INTERES_A_PAGAR_LEASING), simulacion.SMD_INTERES_A_PAGAR_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_SALDO_TOTAL_LEASING), simulacion.SMD_SALDO_TOTAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_AFECTACION_INDIVIDUAL_LEASING), simulacion.SMD_AFECTACION_INDIVIDUAL_LEASING);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.SMD_ABONO_UNICO), simulacion.SMD_ABONO_UNICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.AFL_ID), simulacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetalleSimulacionParameters.NUMERO_DOCUMENTO), simulacion.NUMERO_DOCUMENTO);

            var retorno = await GetAsyncFirst<SimulacionDetalle>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return retorno;
        }


        public async Task<LineaCreditoObligacion> RecalcularLineaCreditoObligacion(DatosLineaCreditoObligacionDto datosLinea)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumLineasCreditoRestaurar.LlineaCreditoId), datosLinea.OLN_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumLineasCreditoRestaurar.LineaEstado), datosLinea.OLN_ESTADO_RECALCULAR);

            return await GetAsyncFirst<LineaCreditoObligacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<PeritoDetalle> InsertarPeritoDetalle(PeritoDetalleDto perito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumPeritoDetalle.PeritoId), perito.PER_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPeritoDetalle.Observacion), perito.Descripcion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPeritoDetalle.SolicitudCredito), perito.SOC_ID);

            return await GetAsyncFirst<PeritoDetalle>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<PeritoDetalle> ConsultarPeritoDetalle(string solicitudCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumPeritoDetalle.SolicitudCredito), solicitudCredito);

            return await GetAsyncFirst<PeritoDetalle>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene el detalle de los pagos en seven
        /// </summary>
        /// <author>Juan Moncayo</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>> ObtenerDetallePagoSeven(RequestDetalleSeven request)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.CUENTA), request.cuenta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.ACT_ID), request.identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.NO_COMPROBANTE), request.NumeroComprobante);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.FECHA_INICIAL), request.fechaInicio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.FECHA_FIN), request.fechaFin);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDetallePagoSeven.TCR_ID), request.tipoCredito);
            //jm

            return await GetAsyncList<ConsultarDetalleAprobadosNoDesembolsadosDTO>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<TipoIdentificacion> ObtenerTipoIdentificacionPorId(int tipo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TID_ID), tipo);

            return await GetAsyncFirst<TipoIdentificacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<DatosEndeudamientoProspeccion> InsertarDatosEndeudamientoProspeccion(DatosEndeudamientoProspeccionDto datosEndeudamiento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.NumeroDocumento), datosEndeudamiento.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.SalarioTotal), datosEndeudamiento.SalarioUltimoMes);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.Deducciones), datosEndeudamiento.Descuentos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.SalarioNeto), datosEndeudamiento.SalarioNeto);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.CapacidadPago), datosEndeudamiento.CapacidadPago);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.CanonMensual), datosEndeudamiento.CanonMensual);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.SectorFinanciero), datosEndeudamiento.SectorFinanciero);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.SectorReal), datosEndeudamiento.SectorReal);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.Score), datosEndeudamiento.Score);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.DisponibleLibranza), datosEndeudamiento.DisponibleLibranza);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.EndeudamientoGlobalCuota), datosEndeudamiento.EndeudamientoGlobalCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.ControlNivelEndeudamientoCuota), datosEndeudamiento.ControlNivelEndeudamientoGlobalCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.EndeudamientoGlobalSinCuota), datosEndeudamiento.EndeudamientoGlobalSinCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.ControlNivelEndeudamientoSinCuota), datosEndeudamiento.ControlNivelEndeudamientoGlobalSinCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.PorcentajeEndeudamientoCuota), datosEndeudamiento.PorcentajeEndeudamientoGlobalCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.PorcentajeEndeudamientoSinCuota), datosEndeudamiento.PorcentajeEndeudamientoGlobalSinCuota);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.TotalCanonDeducciones), datosEndeudamiento.TotalCanonDeducciones);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.DecisionHomologada), datosEndeudamiento.DecisionHomologada);

            return await GetAsyncFirst<DatosEndeudamientoProspeccion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosEndeudamientoProspeccion> ConsultarDatosEndeudamientoProspeccion(string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosEndeudamientoProspeccion.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<DatosEndeudamientoProspeccion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> GuardarValorCDP(InsertarValoresCDPSeven valoresCDPSevenDto)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.SOC_ID), valoresCDPSevenDto.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.NUMERO_DOC), valoresCDPSevenDto.NUMERO_DOC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.NOMBRE_AFILIADO), valoresCDPSevenDto.NOMBRE_AFILIADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.VALOR_PRESTAMO), valoresCDPSevenDto.VALOR_PRESTAMO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.SOC_DESEMBOLSO_APLICADO), valoresCDPSevenDto.SOC_DESEMBOLSO_APLICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.MPR_CONT), valoresCDPSevenDto.MPR_CONT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.TCR_ID), valoresCDPSevenDto.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.TID_ID), valoresCDPSevenDto.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Emp_codi), valoresCDPSevenDto.Emp_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Top_codi), valoresCDPSevenDto.Top_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Arb_csuc), valoresCDPSevenDto.Arb_csuc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.DPD_ID), valoresCDPSevenDto.DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.DPC_ID), valoresCDPSevenDto.DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Arb_codc), valoresCDPSevenDto.ARB_CODC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Dmp_valo), valoresCDPSevenDto.DMP_VALO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.Pro_codi), valoresCDPSevenDto.PRO_CODI);

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.FECHA_CREACION_CDP), valoresCDPSevenDto.FECHA_CREACION_CDP);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ObtenerValoresCDPSevenDto>> ObtenerValorCDP(RequestValoresCDPSeven request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.SOC_ID), request.idSolicitudCredito);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarCDPSeven.NUMERO_DOC), request.NumeroDocumentoCliente);

            return await GetAsyncList<ObtenerValoresCDPSevenDto>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SegurosDatosSolicitudCredito> RegistrarSeguroSolicitudCredito(SegurosDatosSolicitudCreditoDto segurosDatos)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.ConvenioSeguroVida), segurosDatos.soC_CONVENIO_ASEGURADORA);
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.SeguroVida), segurosDatos.asE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.ConvenioSeguroHogar), segurosDatos.soC_CONVENIO_ASEGURADORA_HOGAR);
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.SeguroHogar), segurosDatos.asE_ID_HOGAR);
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.NumeroDocumento), segurosDatos.numeroDocumento);

            return await GetAsyncFirst<SegurosDatosSolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConsultaSegurosCredito>> ConsultarSegurosSolicitudCredito(ConsultaSegurosCreditoDto consultaSegurosCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.NumeroDocumento), consultaSegurosCredito.numeroDocumento);

            return await GetAsyncList<ConsultaSegurosCredito>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<DocumentosActRespuestaDto> ActualizarDocLegalizacion(DocumentosActEstadoRequestDto request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.SOC_ID), request.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.DCS_RUTA_IMAGEN), request.DCS_RUTA_IMAGEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.DCS_APROBACION), request.DCS_APROBACION);

            return await GetAsyncFirst<DocumentosActRespuestaDto>(HelperDBParameters.BuilderFunction(
          HelperDBParameters.EnumSchemas.DBO), parameters, System.Data.CommandType.StoredProcedure);
        }

        public async Task<SegurosDatosSolicitudCredito> ConsultarSeguroSimulacionCredito(string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionSegurosSolicitudCredito.NumeroDocumento), numeroDocumento);

            return await GetAsyncFirst<SegurosDatosSolicitudCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> GuardarValorFactura(InsertarValoresFacturaSeven valoresFacturaSeven)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.SOC_ID), valoresFacturaSeven.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.IDENTIFICACION), valoresFacturaSeven.IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.PRIMER_NOMBRE), valoresFacturaSeven.PRIMER_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.SEGUNDO_NOMBRE), valoresFacturaSeven.SEGUNDO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.PRIMER_APELLIDO), valoresFacturaSeven.PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.SEGUNDO_APELLIDO), valoresFacturaSeven.SEGUNDO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.DIRECCION), valoresFacturaSeven.DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.FAC_DESC), valoresFacturaSeven.FAC_DESC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.TOP_CODI), valoresFacturaSeven.TOP_CODI);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.NUMERO_RADICADO), valoresFacturaSeven.NUMERO_RADICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.Emp_codi), valoresFacturaSeven.Emp_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.MPR_CONT), valoresFacturaSeven.MPR_CONT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.TCR_ID), valoresFacturaSeven.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.PVD_CODA), valoresFacturaSeven.PVD_CODA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.ARB_CODA), valoresFacturaSeven.ARB_CODA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.ARB_CODC), valoresFacturaSeven.ARB_CODC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.ARB_CODP), valoresFacturaSeven.ARB_CODP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.DFA_DEST), valoresFacturaSeven.DFA_DEST);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.DFA_VALO), valoresFacturaSeven.DFA_VALO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.PRO_CODI), valoresFacturaSeven.PRO_CODI);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.AFL_ID), valoresFacturaSeven.AFL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.DPD_ID), valoresFacturaSeven.DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.DPC_ID), valoresFacturaSeven.DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.FAC_CONT), valoresFacturaSeven.FAC_CONT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.FAC_NUME), valoresFacturaSeven.FAC_NUME);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.FECHA_CREACION_FACTURA), valoresFacturaSeven.FECHA_CREACION_FACTURA);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<ObtenerDetalleFacturaSevenDto>> ObtenerValorFactura(RequestValoresFactura request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.IDENTIFICACION), request.IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarFacturaSeven.NUMERO_RADICADO), request.NUMERO_RADICADO);
            return await GetAsyncList<ObtenerDetalleFacturaSevenDto>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerComprobantesDto>> ObtenerComprobantesCredito(RequestObtenerComprobantesCredito request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerComprobantesCredito.FECHA_INICIO), request.FECHA_INICIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerComprobantesCredito.FECHA_FIN), request.FECHA_FIN);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerComprobantesCredito.TCC_CODIGO), request.TCC_CODIGO);
            return await GetAsyncList<ObtenerComprobantesDto>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        //TasaSimulador
        public async Task<TasaSimulador> ObtenerTasasSimuladorByTipoCredito(string descripcion, int tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionTipoDeAfiliacion.DescripcionAfiliacion), descripcion);
            parametros.Add(HelpersEnums.GetEnumDescription(DecripcionTipoDeAfiliacion.TipoCredito), tipoCredito);
            return await GetAsyncFirst<TasaSimulador>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> GuardarAprobacionDesembolso(InsertarAprobadosDesembolso aprobadosDesembolso)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Soc_id), aprobadosDesembolso.Soc_id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Identificacion), aprobadosDesembolso.Identificacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Emp_codi), aprobadosDesembolso.Emp_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Top_codi), aprobadosDesembolso.Top_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Mco_nume), aprobadosDesembolso.Mco_nume);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Mco_cont), aprobadosDesembolso.Mco_cont);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Mco_fech), aprobadosDesembolso.Mco_fech);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Mod_codi), aprobadosDesembolso.Mod_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Arb_csuc), aprobadosDesembolso.Arb_csuc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Mco_desc), aprobadosDesembolso.Mco_desc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Cue_codi), aprobadosDesembolso.Cue_codi);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_desc), aprobadosDesembolso.Dmc_desc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_cant), aprobadosDesembolso.Dmc_cant);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_acti), aprobadosDesembolso.Dmc_acti);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_refe), aprobadosDesembolso.Dmc_refe);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_vadb), aprobadosDesembolso.Dmc_vadb);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_vacr), aprobadosDesembolso.Dmc_vacr);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Dmc_vaba), aprobadosDesembolso.Dmc_vaba);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Ter_coda), aprobadosDesembolso.Ter_coda);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Arb_codc), aprobadosDesembolso.Arb_codc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Arb_coda), aprobadosDesembolso.Arb_coda);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Arb_codp), aprobadosDesembolso.Arb_codp);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Arb_cods), aprobadosDesembolso.Arb_cods);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Ter_codm), aprobadosDesembolso.Ter_codm);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarAprobadosDesembolso.Fecha_creacion), aprobadosDesembolso.Fecha_creacion);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public async Task<bool> InsertarRecursosCongelados(CongelamientoRecursosProspeccion congelamientoRecursos)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.Descripcion), congelamientoRecursos.CGR_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.Valor), congelamientoRecursos.CGR_VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.SimulacionId), congelamientoRecursos.CGR_SIMULACION_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.Orden), congelamientoRecursos.CGR_ORDEN);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.NumeroDocumento), congelamientoRecursos.CGR_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.TipoCredito), congelamientoRecursos.CGR_TIPO_CREDITO);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public async Task<User> ConsultarUsuarioPorId(Guid? usuarioId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(ConsultarUsuarios.UsuarioID), usuarioId);
            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> LimpiarRecursosCongelados(Guid? simulacionId, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.SimulacionId), simulacionId);
            parametros.Add(HelpersEnums.GetEnumDescription(CongelamientoRecursos.NumeroDocumento), numeroDocumento);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> InsertarNotificacionesSolicitudes(NotificacionesSolicitudes notificacionesSolicitudes)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.SimulacionId), notificacionesSolicitudes.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.CDP), notificacionesSolicitudes.NOT_CDP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.Fecha), DateTime.Now);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<NotificacionesSolicitudes> ConsultarNotificacionesSolicitudes(Guid simulacionId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.SimulacionId), simulacionId);
            return await GetAsyncFirst<NotificacionesSolicitudes>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<NotificacionesSolicitudes>> ConsultarCantidadNotificacionesSolicitudes()
        {
            DynamicParameters parametros = new DynamicParameters();
            return await GetAsyncList<NotificacionesSolicitudes>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<AprobacionCreditoAsesor>> ConsultarAprobacionAsesor(Guid trsID)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.Trs_id), trsID);

            return await GetAsyncList<AprobacionCreditoAsesor>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosAsegurabilidadSolicitudCredito> ConsultarDatosAsegurabilidadCredito(Guid solicitudCreditoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumNotificacionesSolicitudes.Soc_Id), solicitudCreditoId);
            return await GetAsyncFirst<DatosAsegurabilidadSolicitudCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> LimpiarConceptosCongelados(Guid? simulacionId, string numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.SimulacionId), simulacionId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.NumeroDocumento), numeroDocumento);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarConceptosCongelados(ConceptosCongeladosProspeccion conceptosCongelados)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.Descripcion), conceptosCongelados.CON_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.Valor), conceptosCongelados.CON_VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.SimulacionId), conceptosCongelados.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.NumeroDocumento), conceptosCongelados.CON_NUMERO_DOCUMENTO);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ConceptosCongeladosProspeccion>> ConsultarRecursosCongelados(ConceptosCongeladosProspeccion conceptosCongelados)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.SimulacionId), conceptosCongelados.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptosCongeladosProspeccion.NumeroDocumento), conceptosCongelados.CON_NUMERO_DOCUMENTO);
            return await GetAsyncList<ConceptosCongeladosProspeccion>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DescuentoTipoVinculacion>> ConsultarTipoVinculacionCredito(string descripcionVinculacion, string tipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoVinculacionCredito.Descripcion), descripcionVinculacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoVinculacionCredito.TipoCredito), tipoCredito);
            return await GetAsyncList<DescuentoTipoVinculacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<bool> InsertarPrimerPagoCDP(InsertarDatosCDPPrimerPago conceptosCongelados)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarPrimerPago.SimulacionId), conceptosCongelados.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarPrimerPago.Cdp), conceptosCongelados.PPC_CDP);
            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public async Task<PrimerPagoCDPCredito> ConsultarPrimerPagoCDP(Guid simulacionID)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInsertarPrimerPago.SimulacionId), simulacionID);
            return await GetAsyncFirst<PrimerPagoCDPCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<DatosPersonalesCreditoEducativo> ActualizarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_TIPO_DOCUMENTO), datosPersonales.DPE_TIPO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_DOCUMENTO), datosPersonales.DPE_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PRIMER_NOMBRE_ESTUDIANTE), datosPersonales.DPE_PRIMER_NOMBRE_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_SEGUNDO_NOMBRE_ESTUDIANTE), datosPersonales.DPE_SEGUNDO_NOMBRE_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PRIMER_APELLIDO_ESTUDIANTE), datosPersonales.DPE_PRIMER_APELLIDO_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_SEGUNDO_APELLIDO_ESTUDIANTE), datosPersonales.DPE_SEGUNDO_APELLIDO_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_DIRECCION_ESTUDIANTE), datosPersonales.DPE_DIRECCION_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_VALOR_CREDITO), datosPersonales.DPE_VALOR_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PERIODO_LECTIVO), datosPersonales.DPE_PERIODO_LECTIVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.FK_ENE_ID), datosPersonales.FK_ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.ENE_DPD_ID), datosPersonales.ENE_DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.ENE_DPC_ID), datosPersonales.ENE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_IDENTIFICACION_TITULAR), datosPersonales.DPE_NUMERO_IDENTIFICACION_TITULAR);


            return await GetAsyncFirst<DatosPersonalesCreditoEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosPersonalesCreditoEducativo> InsertarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_TIPO_DOCUMENTO), datosPersonales.DPE_TIPO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_DOCUMENTO), datosPersonales.DPE_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PRIMER_NOMBRE_ESTUDIANTE), datosPersonales.DPE_PRIMER_NOMBRE_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_SEGUNDO_NOMBRE_ESTUDIANTE), datosPersonales.DPE_SEGUNDO_NOMBRE_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PRIMER_APELLIDO_ESTUDIANTE), datosPersonales.DPE_PRIMER_APELLIDO_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_SEGUNDO_APELLIDO_ESTUDIANTE), datosPersonales.DPE_SEGUNDO_APELLIDO_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_DIRECCION_ESTUDIANTE), datosPersonales.DPE_DIRECCION_ESTUDIANTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_VALOR_CREDITO), datosPersonales.DPE_VALOR_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_PERIODO_LECTIVO), datosPersonales.DPE_PERIODO_LECTIVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.FK_ENE_ID), datosPersonales.FK_ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.ENE_DPD_ID), datosPersonales.ENE_DPD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.ENE_DPC_ID), datosPersonales.ENE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_IDENTIFICACION_TITULAR), datosPersonales.DPE_NUMERO_IDENTIFICACION_TITULAR);

            return await GetAsyncFirst<DatosPersonalesCreditoEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosPersonalesCreditoEducativo> EliminarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_DOCUMENTO), datosPersonales.DPE_NUMERO_DOCUMENTO);

            return await GetAsyncFirst<DatosPersonalesCreditoEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosPersonalesCreditoEducativo> ConsultarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatosPersonalesEducativo.DPE_NUMERO_IDENTIFICACION_TITULAR), datosPersonales.DPE_NUMERO_IDENTIFICACION_TITULAR);

            return await GetAsyncFirst<DatosPersonalesCreditoEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BeneficiarioEntidadEducativa> InsertarBeneficiarioEntidadEducativa(BeneficiarioEntidadEducativa datosPersonales)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_ID), Guid.NewGuid());
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_TIPO_DOCUMENTO), datosPersonales.BEN_TIPO_DOCUENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_NIT), datosPersonales.BEN_NIT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_RAZON_SOCIAL_ENTIDAD), datosPersonales.BEN_RAZON_SOCIAL_ENTIDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_DIRECCION), datosPersonales.BEN_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_DEPARTAMENTO), datosPersonales.BEN_DEPARTAMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_CIUDAD), datosPersonales.BEN_CIUDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_CORREO), datosPersonales.BEN_CORREO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_TELEFONO), datosPersonales.BEN_TELEFONO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_TIPO_CUENTA), datosPersonales.BEN_TIPO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_NUMERO_CUENTA), datosPersonales.BEN_NUMERO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_BANCO), datosPersonales.BEN_BANCO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.BEN_FECHA_LIMITE), datosPersonales.BEN_FECHA_LIMITE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.SLS_ID), datosPersonales.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.DPE_NUMERO_TITULAR), datosPersonales.DPE_NUMERO_TITULAR);


            return await GetAsyncFirst<BeneficiarioEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BeneficiarioEntidadEducativa> ConsultarBeneficiarioPorNumeroTitular(string numeroTitular)
        {
            var parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.DPE_NUMERO_TITULAR), numeroTitular);


            return await GetAsyncFirst<BeneficiarioEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<UbicacionInmuebleCredito> ObtenerUbicacionViviendaCredito(Guid sls_id)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativa.SLS_ID), sls_id);
            return await GetAsyncFirst<UbicacionInmuebleCredito>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReportesSolicitudCredito>> ReportesSolicitudCreditoGeneral(ReportesSolicitudCreditoDto datos)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.FECHA_INICIO), datos.FechaInicio);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.FECHA_FINAL), datos.FechaFinal);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.CONSECUTIVO), datos.ConsecutivoSolicitud);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.TAC_ID), datos.TipoTarea);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.NUMERO_DOCUMENTO), datos.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.EstadoCredito), datos.EstadoCredito);


            return await GetAsyncList<ReportesSolicitudCredito>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReportesSolicitudCredito>> ReportesSolicitudCreditoGeneralUsuuarioExterno(ReportesSolicitudCreditoDto datos)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.FECHA_INICIO), datos.FechaInicio);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.FECHA_FINAL), datos.FechaFinal);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.CONSECUTIVO), datos.ConsecutivoSolicitud);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.TAC_ID), datos.TipoTarea);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.NUMERO_DOCUMENTO), datos.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.EstadoCredito), datos.EstadoCredito);
            parametros.Add(HelpersEnums.GetEnumDescription(ReporteCreditos.UsuarioId), datos.UsuarioId);


            return await GetAsyncList<ReportesSolicitudCredito>(HelperDBParameters.BuilderFunction(
              HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> LimpiarDatosPersonalesSimulacion(string request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSimulacionDatosPersonalesParams.NumeroDocumento), request);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarDatosFacturaSevenCredito(FacturaSevenCredito facturaSeven)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.TipoDocumento), facturaSeven.FAC_TIPO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.IdSolicitudCredito), facturaSeven.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NumeroDocumento), facturaSeven.FAC_NUMERO_DOCUMENTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NombreAfiliado), facturaSeven.FAC_NOMBRE_AFILIADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.SegundoNombreAfiliado), facturaSeven.FAC_SEGUNDO_NOMBRE_AFILIADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.ApellidoAfiliado), facturaSeven.FAC_APELLIDO_AFILIADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.SegundoApellidoAfiliado), facturaSeven.FAC_SEGUNDO_APELLIDO_AFILIADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.FechaTercero), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.Flujo), facturaSeven.FLUJO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.TipoDocumentoVendedor), facturaSeven.FAC_TIPO_DOCUMENTO_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NumeroDocumentoVendedor), facturaSeven.FAC_NUMERO_DOCUMENTO_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NombreCompletoVendedor), facturaSeven.FAC_NOMBRE_COMPLETO_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.TipoCuentaVendedor), facturaSeven.FAC_TIPO_CUENTA_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NumeroCuentaVendedor), facturaSeven.FAC_NUMERO_CUENTA_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.BancoVendedor), facturaSeven.FAC_BANCO_VENDEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.FechaVendedor), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.CDP), facturaSeven.FAC_CDP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.TipoCredito), facturaSeven.FAC_TIPO_CREDITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.ValorDesembolso), facturaSeven.FAC_VALOR_DESEMBOLSO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.Radicado), facturaSeven.FAC_RADICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.AreaNegocio), facturaSeven.FAC_AREA_NEGOCIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.FechaCDP), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.NumeroFactura), facturaSeven.FAC_NUMERO_FACTURA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.FechaFactura), DateTime.Now);



            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<FacturaSevenCredito> ConsultarDatosFactura(Guid SOC_ID)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFacturaSevenCredito.IdSolicitudCredito), SOC_ID);
            return await GetAsyncFirst<FacturaSevenCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<RespuestaObservacionTramite> ConsultarActividadPorTramiteID(Guid tramiteID)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObservacionTramiteParameters.TRS_ID), tramiteID);


            var response = await GetAsyncFirst<RespuestaObservacionTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<bool> RegistrarBloqueoCuentaCredito(BloqueoRecursosCredito bloqueoRecursos)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.TipoDocumento), bloqueoRecursos.BRC_TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.NumeroDocumento), bloqueoRecursos.BRC_NUMERO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.NumeroCuenta), bloqueoRecursos.BRC_NUMERO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.EstadoCuenta), bloqueoRecursos.BRC_ESTADO);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BloqueoRecursosCredito>> ConsultarBloqueoCuentaCredito(BloqueoRecursosCreditoDto bloqueoRecursos)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.TipoDocumento), bloqueoRecursos.TipoIdentificacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoRecursosCredito.NumeroDocumento), bloqueoRecursos.NumeroIdentificacion);


            var response = await GetAsyncList<BloqueoRecursosCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }

        public async Task<ConsultarCreditoProspeccion> ConsultarNumeroTipoCreditoProspeccion(int tipoDocumento, string numeroDocumento)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(ConsultaCreditoProspeccion.TipoIdentificacion), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(ConsultaCreditoProspeccion.NumeroIdentificacion), numeroDocumento);

            return await GetAsyncFirst<ConsultarCreditoProspeccion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Celular), validacion.Celular);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Identificacion), validacion.Identificacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.FechaRegistro), System.DateTime.Now);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ValidacionOTPTemporal> ConsultarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Celular), validacion.Celular);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Identificacion), validacion.Identificacion);

            return await GetAsyncFirst<ValidacionOTPTemporal>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Celular), validacion.Celular);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValidacionOTPTemporal.Identificacion), validacion.Identificacion);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public Task InsertarDocumentoSimulacion(ResponseDocumentoSimulacion documento)
        {
            throw new NotImplementedException();
        }


        public async Task<CompraCarteraCredito> InsertarValoresCompraCartera(CompraCarteraCredito compraCartera)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.CCC_VALOR_COMPRA_CARTERA), compraCartera.CCC_VALOR_COMPRA_CARTERA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.CCC_CUOTA), compraCartera.CCC_CUOTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.CCC_PLAZO), compraCartera.CCC_PLAZO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.CCC_VALOR_FINAL_PRESTAMO_CARTERA), compraCartera.CCC_VALOR_FINAL_PRESTAMO_CARTERA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.SLS_ID), compraCartera.SLS_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.CCC_NUMERO_OBLIGACION_CARTERA), compraCartera.CCC_NUMERO_OBLIGACION_CARTERA);

            return await GetAsyncFirst<CompraCarteraCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CompraCarteraCredito> ConsultarValoresCompraCartera(CompraCarteraCredito compraCartera)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCompraCarteraCredito.SLS_ID), compraCartera.SLS_ID);

            return await GetAsyncFirst<CompraCarteraCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<TasaSimulador> ConsultarTasaPorTipoCredito(string descripcionTipoSimulacion)
        {
            var parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumTasaSimuladorParams.Descripcion), descripcionTipoSimulacion);

            return await GetAsyncFirst<TasaSimulador>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Valida que el día anterior ya fue causado
        /// Previene que se cause el 11 sin haber causado el 10
        /// Usa patrón Dapper con GetAsyncList
        /// </summary>
        /// <returns>
        /// 0 = Validación OK, puede proceder
        /// 1 = Validación FALLÓ, falta día anterior
        /// </returns>
        public async Task<(int Resultado, string Mensaje)> ValidarContinuidadCausacionDiaria(
            DateTime fechaCausacion, int tcrId)
        {
            var parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.FechaCausacion), fechaCausacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.TcrId), tcrId);

            // OUTPUT parameters: inicializar con valores default
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.Resultado),
                value: 0,
                dbType: DbType.Int32,
                direction: ParameterDirection.Output);
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.MensajeError),
                value: string.Empty,
                dbType: DbType.String,
                direction: ParameterDirection.Output,
                size: 500);

            // Ejecutar usando el método helper existente
            await ExecuteAsync(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO, "ValidarContinuidadCausacionDiaria"),
                parameters,
                CommandType.StoredProcedure);

            // Extraer OUTPUT parameters después de ejecutar
            var resultado = parameters.Get<int>(HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.Resultado));
            var mensaje = parameters.Get<string>(HelpersEnums.GetEnumDescription(EnumValidarContinuidadCausacion.MensajeError));

            return (resultado, mensaje ?? "Error desconocido");
        }


        // ════════════════════════════════════════════════
        // BATCH HISTÓRICO — verifica resultado SP
        // ════════════════════════════════════════════════
        public async Task InsertarCausacionDiariaLeasingHistoricoEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            if (causaciones == null || causaciones.Count == 0) return;

            var leasingData = causaciones.Where(c => c.IdTipoCredito == 1).ToList();
            if (leasingData.Count == 0) return;

            var dt = CrearDataTableCausacionLeasingHistorico(leasingData, FechaCierre);

            var parameters = new DynamicParameters();
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumBatchCausacionLeasingHistoricTVP.Data),
                dt.AsTableValuedParameter("dbo.TVP_CausacionDiariaLeasingHistorico"));

            var result = await GetAsyncList<ResultadoValidacionContinuidadDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                    "InsertarCausacionDiariaLeasingHistoricoBatch"),
                parameters,
                CommandType.StoredProcedure);

            // ← Verificar resultado del SP
            var resultadoSP = result?.FirstOrDefault();
            if (resultadoSP?.Mensaje == "ERROR")
                throw new Exception($"Error insertando histórico leasing: {resultadoSP.Mensaje}");
        }

        // ════════════════════════════════════════════════
        // BATCH SALDO ACTUAL — verifica resultado SP
        // ════════════════════════════════════════════════
        public async Task ActualizarCausacionDiariaLeasingSaldoActualEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            if (causaciones == null || causaciones.Count == 0) return;

            var leasingData = causaciones.Where(c => c.IdTipoCredito == 1).ToList();
            if (leasingData.Count == 0) return;

            var dt = CrearDataTableCausacionLeasingSaldoActual(leasingData, FechaCierre);

            var parameters = new DynamicParameters();
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumBatchCausacionLeasingSaldoActualTVP.Data),
                dt.AsTableValuedParameter("dbo.TVP_CausacionDiariaLeasingSaldoActual"));

            var result = await GetAsyncList<ResultadoValidacionContinuidadDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                    "ActualizarCausacionDiariaLeasingSaldoActualBatch"),
                parameters,
                CommandType.StoredProcedure);

            // ← Verificar resultado del SP
            var resultadoSP = result?.FirstOrDefault();
            if (resultadoSP?.Mensaje == "ERROR")
                throw new Exception($"Error actualizando saldo actual leasing: {resultadoSP.Mensaje}");
        }

        // ═════════════════════════════════════════════════════════════════════════════
        // BATCH HIPOTECARIOS HISTÓRICO CON TVP
        // ═════════════════════════════════════════════════════════════════════════════

        // ════════════════════════════════════════════════
        // DataTable HISTÓRICO hipotecario —
        // Solo agregar verificación de resultado
        // ════════════════════════════════════════════════
        public async Task InsertarCausacionDiariaHipotecarioHistoricoEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            if (causaciones == null || causaciones.Count == 0) return;

            var hipotecarioData = causaciones.Where(c => c.IdTipoCredito != 1).ToList();
            if (hipotecarioData.Count == 0) return;

            var dt = CrearDataTableCausacionHipotecarioHistorico(hipotecarioData, FechaCierre);

            var parameters = new DynamicParameters();
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumBatchCausacionHipotecarioHistoricoTVP.Data),
                dt.AsTableValuedParameter("dbo.TVP_CausacionDiariaHipotecarioHistorico"));

            var result = await GetAsyncList<ResultadoValidacionContinuidadDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                    "InsertarCausacionDiariaHipotecarioHistoricoBatch"),
                parameters,
                CommandType.StoredProcedure);

            // ← Verificar resultado
            var resultadoSP = result?.FirstOrDefault();
            if (resultadoSP?.Mensaje == "ERROR")
                throw new Exception($"Error insertando histórico hipotecario: {resultadoSP.Mensaje}");
        }


        // ════════════════════════════════════════════════
        // BATCH SALDO ACTUAL hipotecario — verificar resultado
        // ════════════════════════════════════════════════
        public async Task ActualizarCausacionDiariaHipotecarioSaldoActualEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            if (causaciones == null || causaciones.Count == 0) return;

            var hipotecarioData = causaciones.Where(c => c.IdTipoCredito != 1).ToList();
            if (hipotecarioData.Count == 0) return;

            var dt = CrearDataTableCausacionHipotecarioSaldoActual(hipotecarioData, FechaCierre);

            var parameters = new DynamicParameters();
            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumBatchCausacionHipotecarioSaldoActualTVP.Data),
                dt.AsTableValuedParameter("dbo.TVP_CausacionDiariaHipotecarioSaldoActual"));

            var result = await GetAsyncList<ResultadoValidacionContinuidadDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                    "ActualizarCausacionDiariaHipotecarioSaldoActualBatch"),
                parameters,
                CommandType.StoredProcedure);

            // ← Verificar resultado
            var resultadoSP = result?.FirstOrDefault();
            if (resultadoSP?.Mensaje == "ERROR")
                throw new Exception($"Error actualizando saldo actual hipotecario: {resultadoSP.Mensaje}");
        }

        /// <summary>
        /// Obtiene solo el SALDO ACTUAL sin histórico (más rápido)
        /// Útil para consultas de saldo en tiempo real
        /// </summary>
        public async Task<List<CausacionSaldoActual>> ObtenerSaldoActualCreditoAsync(
            long numeroCredito,
            int idTipoCredito)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NUMERO_CREDITO", numeroCredito);
            parameters.Add("@ID_TIPO_CREDITO", idTipoCredito);
            parameters.Add("@DIAS_HISTORICO", 0);  // No traer histórico


            var response = await GetAsyncList<CausacionSaldoActual>(
               HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                        "ObtenerCausacionPorCredito"),
               parameters,
               CommandType.StoredProcedure);
            return response.ToList();
        }

        public async Task<List<CausacionHistorico>> ObtenerHistorialCausacionInteresCorrienteAsync(
    long numeroCredito, int idTipoCredito, DateTime fechaInicial, DateTime fechaFinal)

        {
            var parameters = new DynamicParameters();
            parameters.Add("@NUMERO_CREDITO", numeroCredito);
            parameters.Add("@ID_TIPO_CREDITO", idTipoCredito);
            parameters.Add("@FECHA_INICIAL", fechaInicial);
            parameters.Add("@FECHA_FINAL", fechaFinal);

            var response = await GetAsyncList<CausacionHistorico>(
               HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO,
                        "ObtenerHistorialCausacionInteresCorriente"),
               parameters,
               CommandType.StoredProcedure);

            return response.ToList();
        }

        // ════════════════════════════════════════════════
        // DataTable HISTÓRICO
        // ════════════════════════════════════════════════
        private DataTable CrearDataTableCausacionLeasingHistorico(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            var dt = new DataTable();
            dt.Columns.Add("CRE_NRO_CREDITO", typeof(long));
            dt.Columns.Add("CAUSACION_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CAUSACION_INTERES_REMANENTE", typeof(decimal));
            dt.Columns.Add("CAUSACION_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CAUSACION_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("ACUMULADO_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("ACUMULADO_INTERES_REMANENTE", typeof(decimal));
            dt.Columns.Add("ACUMULADO_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("ACUMULADO_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("DEUDA_REMANENTE", typeof(decimal));
            dt.Columns.Add("DIAS_CAUSADOS", typeof(int));
            dt.Columns.Add("CLI_IDENTIFICACION", typeof(string));
            dt.Columns.Add("FECHA_CAUSACION", typeof(DateTime));
            dt.Columns.Add("SALDO_SIN_PAGO_CUOTA", typeof(decimal));  // ← nuevo
            dt.Columns.Add("SALDO_CON_PAGO_CUOTA", typeof(decimal));  // ← nuevo

            foreach (var c in causaciones)
            {
                // Calcular saldos
                var totalAcumulado = c.CausacionInteresesCorriente
                                   + (c.CausacionInteresesRemanente)
                                   + c.CausacionSeguroHogar
                                   + c.CausacionSeguroVida;

                var saldoSinPago = c.SaldoCapital + totalAcumulado;         // ← fórmula
                var saldoConPago = saldoSinPago - c.ValorCuota;             // ← fórmula

                dt.Rows.Add(
                    c.NumeroCredito,
                    c.CausacionInteresesCorriente,
                    c.CausacionInteresesRemanente,
                    c.CausacionSeguroVida,
                    c.CausacionSeguroHogar,
                    c.AcumuladoInteresesCorrientes,
                    0,
                    c.AcumuladoSeguroVida,
                    c.AcumuladoSeguroHogar,
                    c.ValorDeudaRemanente,
                    c.DiasCausados,
                    c.Identificacion,
                    FechaCierre,
                    saldoSinPago,    // ← nuevo
                    saldoConPago     // ← nuevo
                );
            }
            return dt;
        }

        // ════════════════════════════════════════════════
        // LEASING Saldo Actual
        // ════════════════════════════════════════════════
        private DataTable CrearDataTableCausacionLeasingSaldoActual(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            var dt = new DataTable();
            dt.Columns.Add("NUMERO_CREDITO", typeof(long));
            dt.Columns.Add("CLI_IDENTIFICACION", typeof(string));
            dt.Columns.Add("CDLSA_FECHA_CAUSACION", typeof(DateTime));
            dt.Columns.Add("CDLSA_SALDO_CAPITAL", typeof(decimal));
            dt.Columns.Add("CDLSA_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CDLSA_INTERES_REMANENTE", typeof(decimal));
            dt.Columns.Add("CDLSA_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CDLSA_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("CDLSA_ACUMULADO_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CDLSA_ACUMULADO_INTERES_REMANENTE", typeof(decimal));
            dt.Columns.Add("CDLSA_ACUMULADO_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CDLSA_ACUMULADO_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("CDLSA_VALOR_DEUDA_REMANENTE", typeof(decimal));
            dt.Columns.Add("CDLSA_SALDO_SIN_PAGO_CUOTA", typeof(decimal));  // ← nuevo
            dt.Columns.Add("CDLSA_SALDO_CON_PAGO_CUOTA", typeof(decimal));  // ← nuevo

            foreach (var c in causaciones)
            {
                var totalAcumulado = c.CausacionSeguroVida
                                   + (c.CausacionInteresesCorriente)
                                   + c.CausacionInteresesRemanente
                                   + c.CausacionSeguroHogar;

                var saldoSinPago = c.SaldoCapital + totalAcumulado;
                var saldoConPago = saldoSinPago - c.ValorCuota;

                dt.Rows.Add(
                    c.NumeroCredito,
                    c.Identificacion,
                    FechaCierre,
                    c.SaldoCapital,
                    c.CausacionInteresesCorriente,
                    c.CausacionInteresesRemanente,
                    c.CausacionSeguroVida,
                    c.CausacionSeguroHogar,
                    c.AcumuladoInteresesCorrientes,
                     0,  // ← corregido (antes era 0 hardcodeado)
                    c.AcumuladoSeguroVida,
                    c.AcumuladoSeguroHogar,
                    c.ValorDeudaRemanente,
                    saldoSinPago,    // ← nuevo
                    saldoConPago     // ← nuevo
                );
            }
            return dt;
        }

        // ════════════════════════════════════════════════
        // HIPOTECARIO Saldo Actual — sin remanente
        // ════════════════════════════════════════════════
        private DataTable CrearDataTableCausacionHipotecarioSaldoActual(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            var dt = new DataTable();
            dt.Columns.Add("NUMERO_CREDITO", typeof(long));
            dt.Columns.Add("CLI_IDENTIFICACION", typeof(string));
            dt.Columns.Add("CDHA_FECHA_CAUSACION", typeof(DateTime));
            dt.Columns.Add("CDHA_SALDO_CAPITAL", typeof(decimal));
            dt.Columns.Add("CDHA_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CDHA_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CDHA_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("CDHA_ACUMULADO_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CDHA_ACUMULADO_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CDHA_ACUMULADO_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("CDHA_SALDO_SIN_PAGO_CUOTA", typeof(decimal));
            dt.Columns.Add("CDHA_SALDO_CON_PAGO_CUOTA", typeof(decimal));

            foreach (var c in causaciones)
            {
                // Hipotecario: SaldoSinPago NO incluye remanente
                var totalAcumulado = c.CausacionInteresesCorriente
                                   + c.CausacionSeguroVida
                                   + c.CausacionSeguroHogar;

                var saldoSinPago = c.SaldoCapital + totalAcumulado;
                var saldoConPago = saldoSinPago - c.ValorCuota;

                dt.Rows.Add(
                    c.NumeroCredito,
                    c.Identificacion,
                    FechaCierre,
                    c.SaldoCapital,
                    c.CausacionInteresesCorriente,
                    c.CausacionSeguroVida,
                    c.CausacionSeguroHogar,
                    c.AcumuladoInteresesCorrientes,
                    c.AcumuladoSeguroVida,
                    c.AcumuladoSeguroHogar,
                    saldoSinPago,
                    saldoConPago
                );
            }
            return dt;
        }

        /// <summary>
        private DataTable CrearDataTableCausacionHipotecarioHistorico(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre)
        {
            var dt = new DataTable();
            dt.Columns.Add("CRE_NRO_CREDITO", typeof(long));
            dt.Columns.Add("CAUSACION_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("CAUSACION_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("CAUSACION_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("ACUMULADO_INTERES_CORRIENTE", typeof(decimal));
            dt.Columns.Add("ACUMULADO_SEGURO_VIDA", typeof(decimal));
            dt.Columns.Add("ACUMULADO_SEGURO_HOGAR", typeof(decimal));
            dt.Columns.Add("SALDO_CAPITAL", typeof(decimal));  // ← sin remanente
            dt.Columns.Add("DIAS_CAUSADOS", typeof(int));
            dt.Columns.Add("CLI_IDENTIFICACION", typeof(string));
            dt.Columns.Add("FECHA_CAUSACION", typeof(DateTime));
            dt.Columns.Add("SALDO_SIN_PAGO_CUOTA", typeof(decimal));
            dt.Columns.Add("SALDO_CON_PAGO_CUOTA", typeof(decimal));

            foreach (var c in causaciones)
            {
                // Hipotecario: SaldoSinPago NO incluye remanente
                var totalAcumulado = c.CausacionInteresesCorriente
                                   + c.CausacionSeguroVida
                                   + c.CausacionSeguroHogar;

                var saldoSinPago = c.SaldoCapital + totalAcumulado;
                var saldoConPago = saldoSinPago - c.ValorCuota;

                dt.Rows.Add(
                    c.NumeroCredito,
                    c.CausacionInteresesCorriente,
                    c.CausacionSeguroVida,
                    c.CausacionSeguroHogar,
                    c.AcumuladoInteresesCorrientes,
                    c.AcumuladoSeguroVida,
                    c.AcumuladoSeguroHogar,
                    c.SaldoCapital,       // ← saldo capital real
                    c.DiasCausados,
                    c.Identificacion,
                    FechaCierre,
                    saldoSinPago,
                    saldoConPago
                );
            }
            return dt;
        }



        public async Task<SolicitudSimulacion> ActualizarRadicadoPreAprobadoFlujo83(Guid idSimulacion, string Radicado83)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.Id), idSimulacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.NumRadicado83), Radicado83);

            return await GetAsyncFirst<SolicitudSimulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> ActualizarTipoSimulador(string numeroDocumento, string simuladorDescripcion)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.numeroDocumento), numeroDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSimulacionParameters.SimuladorDescripcion), simuladorDescripcion);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertValoresSimulacionCredito(
     ValoresSimulacionCreditoReclasificacion valores)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.NUMERO_DOCUMENTO),
                valores.NUMERO_DOCUMENTO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.TASA_CREDITO),
                valores.TASA_CREDITO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.TASA_NOMINAL),
                valores.TASA_NOMINAL);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.SEGURO_VIDA),
                valores.SEGURO_VIDA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.SEGURO_HOGAR),
                valores.SEGURO_HOGAR);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.SALARIO_MINIMO),
                valores.SALARIO_MINIMO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.SEGURO_VIDA_EXTRAPRIMA),
                valores.SEGURO_VIDA_EXTRAPRIMA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.TASA_MIXTA),
                valores.TASA_MIXTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.TASA_MIXTA_EXTRAPRIMA),
                valores.TASA_MIXTA_EXTRAPRIMA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.FACTOR),
                valores.FACTOR);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.FACTOR_EXTRA_PRIMA),
                valores.FACTOR_EXTRA_PRIMA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.PLAZO),
                valores.PLAZO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.VALOR_PRESTAMO),
                valores.VALOR_PRESTAMO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.CUOTA_SIN_SEGURO_HOGAR),
                valores.CUOTA_SIN_SEGURO_HOGAR);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA),
                valores.CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.CUOTA_SEGURO_HOGAR),
                valores.CUOTA_SEGURO_HOGAR);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA),
                valores.CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.VALOR_INMUEBLE),
                valores.VALOR_INMUEBLE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.VALOR_AVALUO),
                valores.VALOR_AVALUO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.CAPACIDAD_PAGO),
                valores.CAPACIDAD_PAGO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.VALOR_PRESTAMO_SEGUN_CUOTA),
                valores.VALOR_PRESTAMO_SEGUN_CUOTA);

            return await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<ValoresSimulacionCreditoReclasificacion> ConsultaValoresSimulacionCredito(string numeroDocumento)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValoresSimulacionCreditoProspeccion.NUMERO_DOCUMENTO), numeroDocumento);

            return await GetAsyncFirst<ValoresSimulacionCreditoReclasificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarDocumentoCargadoDodo(DocumentoCargadoDodo documento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.DCD_DESCRIPCION),
                documento.DCD_DESCRIPCION);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.DCD_CARGADO),
                documento.DCD_CARGADO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.SLS_ID),
                documento.SLS_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.RUTA),
                documento.RUTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.TDC_ID),
                documento.TDC_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.DCD_CARGUE_361),
                documento.DCD_CARGUE_361);

            parameters.Add(
    HelpersEnums.GetEnumDescription(
        EnumInsertarDocumentoCargadoDodo.DCD_CARGUE_83),
    documento.DCD_CARGUE_83);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<DocumentoCargadoDodo> ConsultaDocumentoCargadoDodo(Guid slsId, string descripcionDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumConsultaDocumentoCargadoDodo.SLS_ID),
                slsId);
            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarDocumentoCargadoDodo.DCD_DESCRIPCION),
                descripcionDocumento);

            return await GetAsyncFirst<DocumentoCargadoDodo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionPerito>> ConsultarSolicitudesTramitePerito(string usuarioPerito)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInformacionPeritos.UsuarioPerito), usuarioPerito);

            return await GetAsyncList<InformacionPerito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AliadosEstrategicos>> ConsultarAliadosEstrategicos()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<AliadosEstrategicos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        public async Task<AvaluoComercialCredito> ConsultarAvaluoComercialCredito(Guid soc_id)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreacionComiteSolicitudCreditoParams.idSolicitudCredito), soc_id);

            return await GetAsyncFirst<AvaluoComercialCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AvaluoComercialCredito> InsertarActualizarAvaluoComercialCredito(AvaluoComercialCredito avaluoComercialCredito)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.SOC_ID), avaluoComercialCredito.SOC_ID);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_CLIENTE), avaluoComercialCredito.AVC_CLIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_NUMERO_DOCUMENTO), avaluoComercialCredito.AVC_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DIRECCION), avaluoComercialCredito.AVC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_SECTOR), avaluoComercialCredito.AVC_SECTOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_BARRIO), avaluoComercialCredito.AVC_BARRIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_ID_DPD), avaluoComercialCredito.AVC_ID_DPD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_ID_DPC), avaluoComercialCredito.AVC_ID_DPC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_FECHA_INFORME), avaluoComercialCredito.AVC_FECHA_INFORME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_VETUSTEZ), avaluoComercialCredito.AVC_VETUSTEZ);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_FECHA_PAGO_AVALUO), avaluoComercialCredito.AVC_FECHA_PAGO_AVALUO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_FECHA_VISITA), avaluoComercialCredito.AVC_FECHA_VISITA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_PROPIETARIO), avaluoComercialCredito.AVC_PROPIETARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_ESCRITURA), avaluoComercialCredito.AVC_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_NOTARIA), avaluoComercialCredito.AVC_NOTARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_FECHA_ESCRITURA), avaluoComercialCredito.AVC_FECHA_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DPD_ID_ESCRITURA), avaluoComercialCredito.AVC_DPD_ID_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DPC_ID_ESCRITURA), avaluoComercialCredito.AVC_DPC_ID_ESCRITURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_CEDULA_CATASTRAL), avaluoComercialCredito.AVC_CEDULA_CATASTRAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_LICENCIA_CONSTRUCCION), avaluoComercialCredito.AVC_LICENCIA_CONSTRUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_NOMBRE_CONJUNTO), avaluoComercialCredito.AVC_NOMBRE_CONJUNTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_COEFICIENTE_COPROPIEDAD), avaluoComercialCredito.AVC_COEFICIENTE_COPROPIEDAD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DISTRIBUCION_INMUEBLE), avaluoComercialCredito.AVC_DISTRIBUCION_INMUEBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_CARACTERISTICAS_ADICIONALES), avaluoComercialCredito.AVC_CARACTERISTICAS_ADICIONALES);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DESCRIPCION_DEPENDENCIA), avaluoComercialCredito.AVC_DESCRIPCION_DEPENDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_GARAJE), avaluoComercialCredito.AVC_GARAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_DESCRIPCION_GARAJE), avaluoComercialCredito.AVC_DESCRIPCION_GARAJE);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_SALA_AREA_CONSTRUIDA), avaluoComercialCredito.AVC_SALA_AREA_CONSTRUIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_AREA_PRIVADA), avaluoComercialCredito.AVC_AREA_PRIVADA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAvaluoComercialCreditoParams.AVC_CUADRO_VALORACION_INMUEBLE), avaluoComercialCredito.AVC_CUADRO_VALORACION_INMUEBLE);

            return await GetAsyncFirst<AvaluoComercialCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ValidacionCreditoProspeccion> ConsultarValidacionCreditoProspeccion(string numeroDocumento)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValidacionCreditoProspeccionParams.VCP_NUMERO_DOCUMENTO),
                numeroDocumento);

            return await GetAsyncFirst<ValidacionCreditoProspeccion>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<ValidacionCreditoProspeccion> InsertarActualizarValidacionCreditoProspeccion(ValidacionCreditoProspeccion validacionCreditoProspeccion)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValidacionCreditoProspeccionParams.VCP_NUMERO_DOCUMENTO),
                validacionCreditoProspeccion.VCP_NUMERO_DOCUMENTO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumValidacionCreditoProspeccionParams.VCP_ESTADO),
                validacionCreditoProspeccion.VCP_ESTADO);

            return await GetAsyncFirst<ValidacionCreditoProspeccion>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UsuarioCreditoTramite>> ConsultarUsuarioCredito(int indicador)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInformacionPeritos.Indicador), indicador);

            return await GetAsyncList<UsuarioCreditoTramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene Documentos Act Solicitud por TacID
        /// </summary>
        /// <author>Alejandro Lara Miranda</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosPorTAC(DocumentosActSolicitud request)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentosActRespuestaParameters.TAC_ID), request.TAC_ID);

            return await GetAsyncList<DocumentosActRespuesta>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<ValidacionCreditoProspeccion> InsertarDocumentoActividadCredito(DocumentosCredito request)
        {
            DynamicParameters parameters = new();

            parameters.Add(
            HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.TAC_ID),
            request.TAC_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.TDC_ID),
                request.TDC_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.DCA_OBLIGATORIO),
                request.DCA_OBLIGATORIO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.DCA_CARGA),
                request.DCA_CARGA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.DCA_VISUALIZA),
                request.DCA_VISUALIZA);


            return await GetAsyncFirst<ValidacionCreditoProspeccion>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<TipoDocumento>> ObtenerTiposDocumento()
        {

            return await GetAsyncList<TipoDocumento>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        public async Task<bool> EliminarDocumentoActividadCredito(DocumentosCredito request)
        {
            DynamicParameters parameters = new();

            parameters.Add(
            HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.TAC_ID),
            request.TAC_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumDocumentoActividadCreditoParameters.TDC_ID),
                request.TDC_ID);

            return await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }
        public async Task<bool> InsertarDocumentoCargado(DocumentosCargadoDodo request)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_DESCRIPCION),
                request.DCD_DESCRIPCION);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_FECHA_CARGUE),
                request.DCD_FECHA_CARGUE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_CARGADO),
                request.DCD_CARGADO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.SLS_ID),
                request.SLS_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.RUTA),
                request.RUTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.TDC_ID),
                request.TDC_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_CARGUE_361),
                request.DCD_CARGUE_361);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_CARGUE_83),
                request.DCD_CARGUE_83);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.DCD_CREADO_POR),
                request.DCD_CREADO_POR);

            return await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<DocumentosCargadoDodo> ObtenerDocumentoCargado(DocumentosCargadoDodo request)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.SLS_ID),
                request.SLS_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumCargueArchivos.TDC_ID),
                request.TDC_ID);

            return await GetAsyncFirst<DocumentosCargadoDodo>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarReporteTransUnion(ReporteProspeccionPreaprobado request)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_FECHA_CONSULTA),
                request.RTU_FECHA_CONSULTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_NUMERO_IDENTIFICACION),
                request.RTU_NUMERO_IDENTIFICACION);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_NOMBRE),
                request.RTU_NOMBRE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_DEPARTAMENTO),
                request.RTU_DEPARTAMENTO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_CIUDAD),
                request.RTU_CIUDAD);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_TIPO_CREDITO),
                request.RTU_TIPO_CREDITO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_VALOR_INMUEBLE),
                request.RTU_VALOR_INMUEBLE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_ESTADO),
                request.RTU_ESTADO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_MOTIVO),
                request.RTU_MOTIVO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_CREADO_POR),
                request.RTU_CREADO_POR);

            return await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteProspeccionPreaprobado>> ObtenerReporteTransUnion(
    string numeroIdentificacion,
    DateTime? fechaDesde,
    DateTime? fechaHasta,
    int? estado)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_NUMERO_IDENTIFICACION),
                numeroIdentificacion);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.FECHA_DESDE),
                fechaDesde);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.FECHA_HASTA),
                fechaHasta);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumReporteTransUnionParameters.RTU_ESTADO),
                estado);

            return await GetAsyncList<ReporteProspeccionPreaprobado>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }
    }
}
