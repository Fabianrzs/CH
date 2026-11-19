using Azure;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Domain.Entities.CargueNonima.Document;
using GA2.Domain.Entities.Catalogs;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class DocumentoCargaRepository
        : DapperGenericRepository, IDocumentoCargaRepository
    {
        public DocumentoCargaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Crear Documento de carga 
        /// </summary>
        /// <param name="documentoCarga">Documento de carga a crear</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Documento creado</returns>
        public async Task<Documento> CrearDocumentoCarga(Documento documentoCarga)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESD_ID), documentoCarga.ESD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.EST_ID), documentoCarga.EST_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.TDC_ID), documentoCarga.TDC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.UEJ_ID), documentoCarga.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_NOMBRE), documentoCarga.DCT_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHA_INICIAL), documentoCarga.DCT_FECHA_INICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_CREADOPOR), documentoCarga.DCT_CREADOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHACREACION), documentoCarga.DCT_FECHACREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_REGISTRO), documentoCarga.DCT_REGISTROS);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear archivo de carga 
        /// </summary>
        /// <param name="nombre">archivo de carga a crear</param>
        /// <author>Federico Marin</author>
        /// <date>08/07/2023</date>
        /// <returns>archivo creado</returns>
        public async Task<FileNameReported> CrearNombreArchivo(FileNameReported nombre)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumNameParameters.NAN_TIPO), nombre.NAN_TIPO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNameParameters.NAN_CATEGORIA), nombre.NAN_CATEGORIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNameParameters.NAN_PERIODOS_APORTES), nombre.NAN_PERIODOS_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNameParameters.UEJ_ID), nombre.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNameParameters.NAN_FECHA_ENVIO), nombre.NAN_FECHA_ENVIO.ToString("yyyy-MM-dd"));

            return await GetAsyncFirst<FileNameReported>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear archivo de carga 
        /// </summary>
        /// <param name="encabezado">archivo de carga a crear</param>
        /// <author>Federico Marin</author>
        /// <date>08/07/2023</date>
        /// <returns>archivo creado</returns>
        public async Task<HeaderFileReported> CrearEncabezadoArchivo(HeaderFileReported encabezado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumHeaderParameters.EAN_CONSECUTIVO), encabezado.CONSECUTIVE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHeaderParameters.TAR_ID), encabezado.FILE_TIPO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHeaderParameters.EAN_NOMBRE_UNIDAD_EJECUTORA), encabezado.NOMBRE_NOMINA_UNIDAD_EJECUTORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHeaderParameters.UEJ_ID), encabezado.ID_UNIDAD_EJECUTORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHeaderParameters.EAN_PERIODO_APORTE), encabezado.FECHA_APORTES.ToString("yyyyMM"));

            return await GetAsyncFirst<HeaderFileReported>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crear archivo de carga 
        /// </summary>
        /// <param name="cuerpo">archivo de carga a crear</param>
        /// <author>Federico Marin</author>
        /// <date>08/07/2023</date>
        /// <returns>archivo creado</returns>
        public async Task<IEnumerable<BodyFile>> CrearCuerpoArchivo(IEnumerable<BodyFile> cuerpo)
        {
            DynamicParameters parameters = new DynamicParameters();

            foreach (var item in cuerpo)
            {
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.CONSECUTIVE), item.CONSECUTIVE);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.DIGITO_FUERZA), item.DIGITO_FUERZA);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.UNIDAD_EJECUTORA), item.UNIDAD_EJECUTORA);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.TIPO_IDENTIFICACION), item.TIPO_IDENTIFICACION);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.IDENTIFICACION), item.IDENTIFICACION);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.CODIGO_MILITAR), item.CODIGO_MILITAR);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.PRIMER_NOMBRE), item.PRIMER_NOMBRE);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.SEGUNDO_NOMBRE), item.SEGUNDO_NOMBRE);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.PRIMER_APELLIDO), item.PRIMER_APELLIDO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.SEGUNDO_APELLIDO), item.SEGUNDO_APELLIDO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.EMBARGO), item.EMBARGO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.INGRESO_BASE_CALCULO), item.INGRESO_BASE_CALCULO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.VALOR), item.VALOR);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.PERIODO), item.PERIODO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.GRADO), item.GRADO);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumBodyParameters.UNIDAD_OPERATIVA), item.UNIDAD_OPERATIVA);

                    await GetAsyncList<BodyFile>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
                }

            return null;

        }

        /// <summary>
        /// Crear archivo de carga 
        /// </summary>
        /// <param name="fin">archivo de carga a crear</param>
        /// <author>Federico Marin</author>
        /// <date>08/07/2023</date>
        /// <returns>archivo creado</returns>
        public async Task<EndFileReported> CrearFinArchivo(EndFileReported fin)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEndParameters.FAN_CONSECUTIVO), fin.CONSECUTIVE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEndParameters.FAN_INDICADOR_CONTROL), fin.FILE_CONTROL_INDICATOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEndParameters.FAN_SUMATORIA_APORTE), fin.SUM_CONTRIBUTIONS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEndParameters.FAN_CODIGO_FIN), fin.END_FILE_CODE);

            return await GetAsyncFirst<EndFileReported>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obetener fin archivo de carga 
        /// </summary>
        /// <param name="dct_id">archivo de carga a crear</param>
        /// <author>Michel Parran</author>
        /// <date>30/10/2024</date>
        /// <returns>archivo creado</returns>
        public async Task<EndFileReported> ObtenerFinArchivo(int dct_id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@DCT_ID", dct_id);
            

            return await GetAsyncFirst<EndFileReported>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los documentos cargados, no consiliados
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Lista de documentos cargados</returns>
        public async Task<IEnumerable<Documento>> ObtenerDocumentosCarga(int esdId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESD_ID), esdId);
            return await GetAsyncList<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los estados de los archivos
        /// </summary>
        /// <author>Federico Marin</author>
        /// <date>21/06/2023</date>
        /// <returns>Lista de estados archivo</returns>
        public async Task<IEnumerable<NMN_ESTADOS_ARCHIVOS_NOMINA>> ObtenerEstadosArchivos()
        {
            return await GetAsyncList<NMN_ESTADOS_ARCHIVOS_NOMINA>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalRechazoNomina>> ConsultarCausalRechazoNomina()
        {
            return await GetAsyncList<CausalRechazoNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los documentos cargados, no consiliados
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Lista de documentos cargados</returns>
        public async Task<List<ConsultarDocumentosCarga>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FRZ_ID), consultarDocumentosCargaFilter.Frz_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.UEJ_ID), consultarDocumentosCargaFilter.Uej_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.TDC_ID), consultarDocumentosCargaFilter.Tdc_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESD_ID), consultarDocumentosCargaFilter.Esd_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHACREACION), consultarDocumentosCargaFilter.Dct_FechaCreacion);
            IEnumerable<ConsultarDocumentosCarga> documentosCarga = await GetAsyncList<ConsultarDocumentosCarga>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return documentosCarga.ToList();
        }
        
        /// <summary>
        /// Obtiene los archivos cargados, no consiliados
        /// </summary>
        /// <author>Federico Marin</author>
        /// <date>15/07/2023</date>
        /// <returns>Lista de archivos cargados</returns>
        public async Task<List<ConsultarArchivosCargados>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FRZ_ID), consultarArchivosCargadosFilter.Frz_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.UEJ_ID), consultarArchivosCargadosFilter.Uej_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), consultarArchivosCargadosFilter.Dct_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.TDC_ID), consultarArchivosCargadosFilter.Tdc_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESD_ID), consultarArchivosCargadosFilter.Esd_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHACREACION), consultarArchivosCargadosFilter.Dct_FechaCreacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FECHA_INICIO_CARGA), consultarArchivosCargadosFilter.FechaInicioCarga);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FECHA_FINAL_CARGA), consultarArchivosCargadosFilter.FechaFinalCarga);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.NOMBRE_ARCHIVO), consultarArchivosCargadosFilter.NombreArchivo);
            IEnumerable<ConsultarArchivosCargados> archivosCargados = await GetAsyncList<ConsultarArchivosCargados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return archivosCargados.ToList();
        }

        public async Task<List<GuardarArchivoCorregido>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), guardarArchivoCorregir.Dct_id); 
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.UEJ_ID), guardarArchivoCorregir.Uej_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHACREACION), guardarArchivoCorregir.Dct_FechaCreacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.EAN_CONSECUTIVO), guardarArchivoCorregir.Encabezado_Consecutivo); 
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_CONSECUTIVO), guardarArchivoCorregir.ConsecutivoCuerpo); 
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_IDENTIFICACION), guardarArchivoCorregir.IdentificacionCuerpo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_PRIMER_NOMBRE), guardarArchivoCorregir.PrimerNombreCuerpo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_SEGUNDO_NOMBRE), guardarArchivoCorregir.SegundoNombreCuerpo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_PRIMER_APELLIDO), guardarArchivoCorregir.PrimerApellidoCuerpo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAN_SEGUNDO_APELLIDO), guardarArchivoCorregir.SegundoApellidoCuerpo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FIN_CONSECUTIVO), guardarArchivoCorregir.Fin_Consecutivo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CODIGO_FIN), guardarArchivoCorregir.Codigo_fin_archivo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.VALOR), guardarArchivoCorregir.Corregir);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.FAN_INDICADOR_CONTROL), guardarArchivoCorregir.IndicadorControl);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.TAR_ID), guardarArchivoCorregir.Tipo_Archivo);
            IEnumerable<GuardarArchivoCorregido> archivosCargados = await GetAsyncList<GuardarArchivoCorregido>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return archivosCargados.ToList();
        }

        /// <summary>
        /// Crear Documento de carga 
        /// </summary>
        /// <param name="documentoCarga">Documento de carga a crear</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Documento creado</returns>
        public async Task<Documento> ActualizarDocumentoCarga(Documento documentoCarga)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoCarga.DCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESD_ID), documentoCarga.ESD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.EST_ID), documentoCarga.EST_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHA_FINAL), documentoCarga.DCT_FECHA_FINAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_MODIFICADOPOR), documentoCarga.DCT_MODIFICADOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_FECHAMODIFICACION), documentoCarga.DCT_FECHAMODIFICACION);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener documento por id repository
        /// </summary>
        /// <param name="documentoId">Documento id</param>
        /// <returns>Documento buscado</returns>
        public async Task<Documento> ObtenerDocumentosPorId(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// actualizar el estado del documento
        /// </summary>
        /// <param name="documentoId">Documento id</param>
        public async Task<Documento> CambiarEstadoAProcesar(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// OBtener archivo por nombre
        /// </summary>
        /// <param name="nombre">Obtener archivo por nombre</param>
        /// <returns>Archivo encontrado</returns>
        public async Task<IEnumerable<Documento>> ObtenerDocumentoPorNombre(string nombre)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_NOMBRE), nombre);

            return await GetAsyncList<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        ///  Cambia el estado  archivo cargado al rechazar procesamiento
        /// </summary>
        /// <param name="documentoId">Id del documento a eliminar</param>
        /// <returns>estado de rechazo</returns>
        public async Task<Documento> CambiarEstadoDocumentoCargadoARechazado(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Cambiar estado documento rechazado a cargado
        /// </summary>
        /// <param name="documentoId">documento id</param>
        /// <returns>documento cargado nuevamente</returns>
        public async Task<Documento> CambiarEstadoDocumentoRechazadoACargado(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<List<ArchivosConError>> ConsultarArchivoConError(ConsultarArchivosConErrorDto ConsultarArchivosConErrorDto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), ConsultarArchivosConErrorDto.Dct_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.VALOR), ConsultarArchivosConErrorDto.Valor); 
            IEnumerable<ArchivosConError> archivosCargados = await GetAsyncList<ArchivosConError>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return archivosCargados.ToList();
        }

        public async Task<Documento> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), eliminarArchivoConError.Dct_Id);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<SolicitudNomina> CrearSolicitudNomina(SolicitudNominaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), request.DocumentoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_ESTADO_SOLICITUD), request.EstadoSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_FECHA_INICIAL), request.FechaInicialDocumento);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_FECHA_FINAL), request.FechaFinalDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_OBSERVACION), request.Observacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_AUTORIZADOPOR), request.AutorizadoPor);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_FECHACREACION), request.FechaCreacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_PROCESADOPOR), request.ProcesadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_APROBADOPOR), request.AprobadoPor);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_FECHAMODIFICACION), request.FechaModificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_ESTADO), request.Estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_RADICADO), request.NumeroRadicado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_MONTO), request.Monto);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_PERIODO), request.Periodo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_REGISTROS), request.Registros);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_TIPO_ARCHIVO), request.TipoArchivo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_NOMBRE_ARCHIVO), request.Nombre);

            return await GetAsyncFirst<SolicitudNomina>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudNomina> ObtenerSolicitudNomina(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<SolicitudNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<SolicitudNomina> ActualizaSolicitudNomina(SolicitudNominaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_NUMERO_SOLICITUD), request.NumeroSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_OBSERVACION), request.Observacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.SNMN_RADICADO), request.NumeroRadicado);

            return await GetAsyncFirst<SolicitudNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<List<ArchivosConError>> ObtenerCuerpoNomina(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            //return await GetAsyncFirst<ArchivoConErrorDto>(HelperDBParameters.BuilderFunction(
            //     HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            IEnumerable<ArchivosConError> archivosCargados = await GetAsyncList<ArchivosConError>(HelperDBParameters.BuilderFunction(
            HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return archivosCargados.ToList();

        }       
        public async Task<List<ArchivoNombreDto>> ObtenerNombreArchivoReconstruido(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            IEnumerable<ArchivoNombreDto> resultado = await GetAsyncList<ArchivoNombreDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);

            return resultado.ToList();
        }

        public async Task<List<ArchivoHeaderDto>> ObtenerHeaderArchivoReconstruido(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            IEnumerable<ArchivoHeaderDto> resultado = await GetAsyncList<ArchivoHeaderDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);

            return resultado.ToList();
        }

        public async Task<List<ArchivoBodyDto>> ObtenerBodyArchivoReconstruido(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            IEnumerable<ArchivoBodyDto> resultado = await GetAsyncList<ArchivoBodyDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);

            return resultado.ToList();
        }
        public async Task<List<ArchivoFinDto>> ObtenerFinArchivoReconstruido(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            IEnumerable<ArchivoFinDto> resultado = await GetAsyncList<ArchivoFinDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);

            return resultado.ToList();
        }


        /// <summary>
        /// actualizar el estado del documento A aprobado
        /// </summary>
        /// <param name="documentoId">Documento id</param>
        public async Task<Documento> AprobarCargueNominaSolicitud(int documentoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), documentoId);

            return await GetAsyncFirst<Documento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }



        public async Task<IEnumerable<InformacionAfiliadoCargueNomina>> InsertarInformacionAfiliadoCargueNomina (InformacionAfiliadoCargueNomina informacionAfiliado)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.PrimerNombre), informacionAfiliado.PrimerNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.SegundoNombre), informacionAfiliado.SegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.PrimerApellido), informacionAfiliado.PrimerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.SegundoApellido), informacionAfiliado.SegundoApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_NUMERO_DOCUMENTO), informacionAfiliado.ICN_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_UNIDAD_EJECUTORA), informacionAfiliado.ICN_UNIDAD_EJECUTORA);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_TIPO_CUENTA), informacionAfiliado.ICN_TIPO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_ESTADO_CUENTA), informacionAfiliado.ICN_ESTADO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_AHORROS_OBLIGATORIOS), informacionAfiliado.ICN_AHORROS_OBLIGATORIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_AHORROS_VOLUNTARIOS), informacionAfiliado.ICN_AHORROS_VOLUNTARIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_CESANTIAS), informacionAfiliado.ICN_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_AHORROS_RETROACTIVOS), informacionAfiliado.ICN_AHORROS_OBLIGATORIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_CESANTIAS_RETROACTIVOS), informacionAfiliado.ICN_CESANTIAS_RETROACTIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_INTERESES_APORTES), informacionAfiliado.ICN_INTERESES_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_INTERESES_CESANTIAS), informacionAfiliado.ICN_INTERESES_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_CESANTIAS_DOCEAVAS), informacionAfiliado.ICN_CESANTIAS_DOCEAVAS);
            parameters.Add(HelpersEnums.GetEnumDescription(CamposInformacionAfiliadoCargueNomina.ICN_TOTAL_CONCEPTO), informacionAfiliado.ICN_TOTAL_CONCEPTO);


            return await GetAsyncList<InformacionAfiliadoCargueNomina>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }


        public async Task<bool> RegistrarNotificacionNomina(NotificacionCargueNomina notificacionCargue)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.DCT_ID), notificacionCargue.DTC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.USUARIO_ID), notificacionCargue.USR_USUARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.CAUSAL_RECHAZO_ID), notificacionCargue.CRN_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ESTADO), notificacionCargue.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.UEJ_USUARIO), notificacionCargue.UEJ_USUARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.NRN_TIPO_ARCHIVO), notificacionCargue.NRN_TIPO_ARCHIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.NRN_PERIODO), notificacionCargue.NRN_PERIODO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.ERRORES), notificacionCargue.ERRORES);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<ConsultarNotificacionNomina>> ConsultaNotificacionNomina(Guid usuarioID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDocumentParameters.USUARIO_ID), usuarioID);


            return await GetAsyncList<ConsultarNotificacionNomina>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }



    }
}
