using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Postulacion;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class PostulacionRepository : DapperGenericRepository, IPostulacionRepository
    {
        public PostulacionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<int>> ActualizarEstadoArchivoPostulacion(Guid idSolicitud, int estado, int actividad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Estado), estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Actividad), actividad);

            var response = await GetAsyncList<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> ActualizarN_FolioArchivoPostulacion(Guid idSolicitud, int folios)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.N_Folio), folios);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ArchivoSolicitud>> ConsultarArchivoPorSolicitudFs(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsConsultarArchivoPostulacionById.Sol_Id), idSolicitud);

            var response = await GetAsyncList<ArchivoSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoListaArchivo>> ConsultarDocumentosPorModalidadPostulacion(DocumentoModalidadPostulacion documento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarListaArchivoPostulanteByModaladad.Id), documento.IdModalidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarListaArchivoPostulanteByModaladad.IdSolicitud), documento.IdSolicitud);

            var response = await GetAsyncList<TipoListaArchivo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<EntrevistaResponse> ConsultarEntrevistaBySolId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), id);

            var response = await GetAsyncFirst<EntrevistaResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarSolicitudPostulacion(SolicitudPostulacionRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.Identificacion), request.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.TipoSolicitudId), request.TipoSolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.EtapasId), request.EtapasId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoConvocatoria), request.NoConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoRadicado), request.NoRadicado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.UserId), request.UsrId);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<PostulanteResponse>> Consultar_Postulacion_By_Id(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacionById.Id), id);

            var response = await GetAsyncList<PostulanteResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> DeleteSolicitudFondoSolidaridad(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacionById.Id), id);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<SolicitudPostulacionResponse> Consultar_Solicitud_Postulacion_By_Id(long identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacionById.Identificacion), identificacion);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<SolicitudPostulacionResponse> ConsultarSolicitudPostulacionBySolId(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SolId", IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<SolicitudPostulacionResponse> ObtenerInformacionSolicitudByRadicado(string radicado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("NoRadicado", radicado);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoSubModalidadPostulacion>> Consultar_Spo_Submodalidad_Postulacion(int idTipoSubModalidad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarTipoSubModalidadPostulante.IdSolicitud), idTipoSubModalidad);

            var response = await GetAsyncList<TipoSubModalidadPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoModalidadPostulacion>> Consultar_Tipo_Modalidad_Postulacion(int idTipoSolicitud_Postulacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarTipoSolicitudPostulante.IdSolicitud), idTipoSolicitud_Postulacion);

            var response = await GetAsyncList<TipoModalidadPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoSolictudPostulante>> Consultar_Tipo_Solicitud_Postulacion()
        {
            var response = await GetAsyncList<TipoSolictudPostulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<Clasificacion> CrearCalificacionFs(Clasificacion clasificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Sol_Id), clasificacion.SolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.TipoImputabilidad), clasificacion.TipoImputabilidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.ImputabilidadAlServicio), clasificacion.ImputabilidadAlServicio, DbType.Decimal,precision:5,scale:2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.PorcentajePostulacion), clasificacion.PorcentajePostulacion, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Hecho), clasificacion.Hecho,DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Puntaje), clasificacion.Puntaje, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.DisfrutePension), clasificacion.DisfrutePension);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.DatosResolucion), clasificacion.DatosResolucion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Estado), clasificacion.EstadoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Actividad), clasificacion.Actividad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Observacion), clasificacion.Observacion);
            var response = await GetAsyncFirst<Clasificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<CalificacionResponse> ConsultarCalificacionBySolId(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Sol_Id), idSolicitud);

            var response = await GetAsyncFirst<CalificacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearEntrenvistaFs(Entrevista entrevista)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.IdSolicitud), entrevista.EFS_ID_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.ViviendaPropia), entrevista.EFS_VIVIENDA_PROPIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.TipoVivienda), entrevista.EFS_TIPO_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.NoHabitante), entrevista.EFS_HABITANTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.InforAdmi), entrevista.EFS_INFORMATIVO_ADMIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.ConceptoPsicosocial), entrevista.EFS_CONCEPTO_PSICOSOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.Estado), entrevista.EFS_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.Actividad), entrevista.EFS_ACTIVIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.Observacion), entrevista.EFS_OBSERVACION);
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<int> DeletePostulanteByIdSolicitud(Guid? idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateStatusAproved.Id), idSolicitud);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarArchivoFSPorSolicitud(EliminarArchivoFSPorSolicitud eliminarArchivo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsEliminarArchivoPorSolicitud.Asp_Id), eliminarArchivo.Asp_Id);

            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<IEnumerable<EstadoPostulacion>> ESP_OBTENER_ESTADO_SOLICITUD()
        {
            var response = await GetAsyncList<EstadoPostulacion>(HelperDBParameters.BuilderFunction(
     HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<CargeArchivosPostulacion> InsertarArchivoPorSolicitudPostulacion(CargeArchivosPostulacion cargeArchivos)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Nombre_Archivo), cargeArchivos.Asp_Nombre_Archivo);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Ruta_Storage), cargeArchivos.Asp_Ruta_Storage);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Extension), cargeArchivos.Asp_Extension);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Solicitud_Id), cargeArchivos.Asp_Solicitud_Id);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Numero_Folio), cargeArchivos.Asp_Numero_Folio);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.Archivo_Id), cargeArchivos.Asp_Id_Archivo_Fk);
            parameters.Add(HelpersEnums.GetEnumDescription(InsertarArchivoPostulacion.UserId), cargeArchivos.UserId);

            var response = await GetAsyncFirst<CargeArchivosPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ActividadFondoSolidaridad>> ObtenerActividadFondoSolidaridad()
        {
            var response = await GetAsyncList<ActividadFondoSolidaridad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<SolicitudPostulacionResponse> ObtenerDatosArchivoById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPostulacionResponse> ObtenerDatosEntrevitsById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<Postulante> ObtenerPostulanteByNumeroDocumento(int numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerPostulateByNumeroDocumento.NumeroDocumento), numeroDocumento);

            var response = await GetAsyncFirst<Postulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPostulante> ObtenerSolicitudPostulacionBySolicitudId(Guid? IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<PostulanteResponse>> Obtener_Postulante_By_Id_Solicitud(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateStatusAproved.Id), idSolicitud);

            var response = await GetAsyncList<PostulanteResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<Postulante>> POS_CREAR_POSTULANTES(Postulante postulante)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.IdSolicitud), postulante.POS_ID_SOLICIUD);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.Nombre), postulante.POS_NOMBRE);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.Apellido), postulante.POS_APELLIDO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TipoDocumentoId), postulante.POS_ID_TIPO_DOCUMENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.NumeroDocumento), postulante.POS_NUMERO_DOCUMENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.DepamentoExpedicion), postulante.POS_DEPARMENTO_EXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.CiduadExpedicion), postulante.POS_CIUDAD_EXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.Parentesco), postulante.POS_PARENTESCO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.Sexo), postulante.POS_SEXO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.FechaNacimiento), postulante.POS_FECHA_NACIMIENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.EstadoCivil), postulante.POS_ESTADO_CIVIL);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.Ocupacion), postulante.POS_OCUPACION);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.NivelEducativo), postulante.POS_NIVEL_EDUCATIVO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.AfectacionFisica), postulante.POS_AFECTACION_FISICA);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.AfectacionSensorial), postulante.POS_AFECTACIONSENSORIAL);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.EnfermedadesEspecificas), postulante.POS_ENFERMEDADESPECIFICA);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TratamientoMedico), postulante.POS_TRATAMIENTOMEDICO);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.DificualtaEscaleras), postulante.POS_DIFICULTADESCALERAS);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.MedicamentoPermanente), postulante.POS_MEDICAMENTOPERMANENTE);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.AyudaSensorial), postulante.POS_AYUDATECNICASENSORIAL);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.CondicionSalud), postulante.POS_CONDICIONSALUD);
            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TipoIntegranteId), postulante.TipoIntegranteId);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TipoParentescoId), postulante.TipoParentescoId);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TipAyudaTecnicaId), postulante.TipAyudaTecnicaId);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.TipoDiscapacidadId), postulante.TipoDiscapacidadId);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.PorcentajePostulacion), postulante.PorcentajePostulacion);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.PorcentajeAportes), postulante.PorcentajeAportes);

            parameters.Add(HelpersEnums.GetEnumDescription(CrearPostulante.PorcentajeCesantias), postulante.PorcentajeCesantias);

            var response = await GetAsyncList<Postulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPostulante> Spo_Crear_Solicitud_Postulante(SolicitudPostulante solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.TipoDocumentoId), solicitud.SPO_TIPO_DOCUMENTO_ID);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NumeroDocumento), solicitud.SPO_NUMERO_DOCUMENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.IdTipoSolicitud), solicitud.SPO_ID_TIPO_DE_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.IdMdalidad), solicitud.SPO_ID_MODALIDAD);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.IdSubModalidad), solicitud.SPO_ID_SUBMODALIDAD);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ManiestacionAutorizaciones), solicitud.SPO_MANIFESTACION_AUTORIZACIONES);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Deparmaento1), solicitud.SPO_DEPARTAMENTO_PREFERENCIA1);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Ciudad1), solicitud.SPO_CIUDAD_PREFERENCIA1);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Deparmaento2), solicitud.SPO_DEPARTAMENTO_PREFERENCIA2);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Ciudad2), solicitud.SPO_CIUDAD_PREFERENCIA2);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Deparmaento3), solicitud.SPO_DEPARTAMENTO_PREFERENCIA3);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Ciudad3), solicitud.SPO_CIUDAD_PREFERENCIA3);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.FirmaSolicitud), solicitud.SPO_TIPO_DOCUMENTO_FIRMA_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NumeroDocumentoFirma), solicitud.SPO_NUMERO_DOCUMENTO_FIRMA_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.FechaExpedicion), solicitud.SPO_FECHAEXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NumeroCelular), solicitud.SPO_NUMERO_CELULAR);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NumeroConvocatoria), solicitud.SPO_NUMERO_CONVOCATORIA);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Estado), solicitud.SPO_ESTADO);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Actividad), solicitud.ACTIVIDA_POSTULACION);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NombreFirma), solicitud.SPO_NOMBRES_FIRMA);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ApellidoFirma), solicitud.SPO_APELLIDOS_FIRMA);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Observacion), solicitud.SPO_OBSERVACION);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.RepresentanteLegal), solicitud.SPO_REPRESENTANTE_LEGAL);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.FechaCreacion), solicitud.SPO_FECHA_CREACION);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.SPO_DISFRUTE_PENSION), solicitud.SPO_DISFRUTE_PENSION);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.SPO_UNIDAD_EJECUTORA), solicitud.SPO_UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.SPO_CATEGORIA), solicitud.SPO_CATEGORIA);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.SPO_NOMBRE_AFILIADO), solicitud.SPO_NOMBRE_AFILIADO_PRINCIPAL);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ModalidadSolucionVivienda), solicitud.ModalidadSolucionVivienda);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.AfectacionCuenta), solicitud.AfectacionCuenta);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ValorAfectacion), solicitud.ValorAfectacion ,DbType.Decimal, precision: 18, scale: 6);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.SPO_USERID), solicitud.SPO_USERID);

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.Fuerza), solicitud.Fuerza);

            parameters.Add("SPO_FECHA_ASIGNACION_INVALIDEZ", solicitud.SPO_FECHA_ASIGNACION_INVALIDEZ);

            parameters.Add("SPO_FECHA_RETIRO_FUERZA", solicitud.SPO_FECHA_RETIRO_FUERZA);
            parameters.Add("SPO_FECHA_RETIRO", solicitud.SPO_FECHA_RETIRO);
            parameters.Add("SPO_MISMO_AFILIADO", solicitud.SPO_MISMO_AFILIADO);
            parameters.Add("SPO_PRINCIPAL", solicitud.SPO_PRINCIPAL);
            parameters.Add("SPO_ID_SOLICITUD_PRINCIPAL", solicitud.SPO_ID_SOLICITUD_PRINCIPAL);
            parameters.Add("SPO_FECHA_PRIMER_APORTE", solicitud.SPO_FECHA_PRIMER_APORTE);

            parameters.Add("SPO_FECHA_ULTIMO_APORTE", solicitud.SPO_FECHA_ULTIMO_APORTE);


            try
            {
                parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.IdConvocatoria), solicitud.IdConvocatoria);
                var response = await GetAsyncFirst<SolicitudPostulante>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

                return response;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public async Task<int> UpdateDataEntrevistaFs(Entrevista entrevista)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.Id), entrevista.EFS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.ViviendaPropia), entrevista.EFS_VIVIENDA_PROPIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.TipoVivienda), entrevista.EFS_TIPO_VIVIENDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.NoHabitante), entrevista.EFS_HABITANTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.InforAdmi), entrevista.EFS_INFORMATIVO_ADMIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.ConceptoPsicosocial), entrevista.EFS_CONCEPTO_PSICOSOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.Observacion), entrevista.EFS_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearEntrevista.UserId), entrevista.UserId);
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<int> UpdateStatusEntrevista(Guid idSolicitud, int estado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Estado), estado);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<Postulante>> Update_Postulante(Postulante postulante)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Id), postulante.POS_ID_SOLICIUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Nombre), postulante.POS_NOMBRE);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Apellido), postulante.POS_APELLIDO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.TipoDocumento), postulante.POS_ID_TIPO_DOCUMENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.NumeroDocumento), postulante.POS_NUMERO_DOCUMENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Deparmaento), postulante.POS_DEPARMENTO_EXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Ciudad), postulante.POS_CIUDAD_EXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Parenteso), postulante.POS_PARENTESCO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Sexo), postulante.POS_SEXO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.FechaNacimiento), postulante.POS_FECHA_NACIMIENTO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.EstadoCivil), postulante.POS_ESTADO_CIVIL);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Ocupacion), postulante.POS_OCUPACION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.NivelEducativo), postulante.POS_NIVEL_EDUCATIVO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.AfectacionFisica), postulante.POS_AFECTACION_FISICA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.AfectacionSensorial), postulante.POS_AFECTACIONSENSORIAL);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.EnfermeadEspecifica), postulante.POS_ENFERMEDADESPECIFICA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.TratamientoMedico), postulante.POS_TRATAMIENTOMEDICO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.DificultaEscaleras), postulante.POS_DIFICULTADESCALERAS);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.MedicamentoPermanente), postulante.POS_MEDICAMENTOPERMANENTE);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.AyudaTecnica), postulante.POS_AYUDATECNICASENSORIAL);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.CondicionSalud), postulante.POS_CONDICIONSALUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdatePostulante.Ocupacion), postulante.POS_OCUPACION);


            var response = await GetAsyncList<Postulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPostulante> Update_Solicitud_Postulante(SolicitudPostulante solicitudPostulante)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Id), solicitudPostulante.SPO_ID_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.ManifesracionAutorizacion), solicitudPostulante.SPO_MANIFESTACION_AUTORIZACIONES);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Deparmaento1), solicitudPostulante.SPO_DEPARTAMENTO_PREFERENCIA1);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Ciudad1), solicitudPostulante.SPO_CIUDAD_PREFERENCIA1);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Deparmaento2), solicitudPostulante.SPO_DEPARTAMENTO_PREFERENCIA2);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Ciudad2), solicitudPostulante.SPO_CIUDAD_PREFERENCIA2);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Deparmaento3), solicitudPostulante.SPO_DEPARTAMENTO_PREFERENCIA3);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Ciudad3), solicitudPostulante.SPO_CIUDAD_PREFERENCIA3);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.TipoDocumentoFimar), solicitudPostulante.SPO_TIPO_DOCUMENTO_FIRMA_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.NumeroDocumentoFimar), solicitudPostulante.SPO_NUMERO_DOCUMENTO_FIRMA_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.FechaExpeidcion), solicitudPostulante.SPO_FECHAEXPEDICION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.NumeroCelular), solicitudPostulante.SPO_NUMERO_CELULAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Observacion), solicitudPostulante.SPO_OBSERVACION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.NumeroCelular), solicitudPostulante.SPO_NUMERO_CELULAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateSolicitudPostulacion.Observacion), solicitudPostulante.SPO_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.NombreFirma), solicitudPostulante.SPO_NOMBRES_FIRMA);
            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ApellidoFirma), solicitudPostulante.SPO_APELLIDOS_FIRMA);
            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.RepresentanteLegal), solicitudPostulante.SPO_REPRESENTANTE_LEGAL);
            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.ValorAfectacion), solicitudPostulante.ValorAfectacion);
            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.AfectacionCuenta), solicitudPostulante.AfectacionCuenta);
            parameters.Add("SPO_MISMO_AFILIADO", solicitudPostulante.SPO_MISMO_AFILIADO);

            var response = await GetAsyncFirst<SolicitudPostulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPostulacionResponse> Update_Status_Aproved(Guid idSolicitud, int estadoName)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateStatusAproved.Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateStatusAproved.Nombre), estadoName);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<Clasificacion> UpdateCalificacionFs(Clasificacion clasificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.IdCalificacion), clasificacion.CalificacionId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.TipoImputabilidad), clasificacion.TipoImputabilidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.ImputabilidadAlServicio), clasificacion.ImputabilidadAlServicio, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.PorcentajePostulacion), clasificacion.PorcentajePostulacion, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Hecho), clasificacion.Hecho, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Puntaje), clasificacion.Puntaje, DbType.Decimal, precision: 5, scale: 2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.DisfrutePension), clasificacion.DisfrutePension);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.DatosResolucion), clasificacion.DatosResolucion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Observacion), clasificacion.Observacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.HechoDescripcion), clasificacion.HechoDescripcion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.PorcentajeDisminucion), clasificacion.PorcentajeDisminucion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.UserId), clasificacion.UserId);
            parameters.Add("@ImputabilidadServicioDescripcion", clasificacion.ImputabilidadServicioDescripcion);
            parameters.Add("@Descripcion", clasificacion.ImputabilidadServicioDescripcion);
            var response = await GetAsyncFirst<Clasificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<SolicitudPostulacionResponse> ObtenerDatosCalificacionById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<SolicitudPostulacionResponse> ObtenerDatosTrazabilidadCalificacionById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> UpdateStatusCalificacionFs(Guid id,int estadoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Sol_Id), id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCrearClasificacionFs.Estado), estadoId);
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> UpdateObservationArchivoFs(Guid id, string observacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarObservacion.Sol_Id), id);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarObservacion.Observacion), observacion);
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<ObservacionArchivo> ConsultarObservationArchivoFsBySolId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            var salida = string.Empty;
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarObservacion.Sol_Id), id);
            var response = await GetAsyncFirst<ObservacionArchivo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarArchivoFs(SolicitudPostulacionRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.Identificacion), request.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.TipoSolicitudId), request.TipoSolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.EtapasId), request.EtapasId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoConvocatoria), request.NoConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoRadicado), request.NoRadicado);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarEntrevistaFs(SolicitudPostulacionRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.Identificacion), request.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.TipoSolicitudId), request.TipoSolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.EtapasId), request.EtapasId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoConvocatoria), request.NoConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoRadicado), request.NoRadicado);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarCalicacionFs(SolicitudPostulacionRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.Identificacion), request.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.TipoSolicitudId), request.TipoSolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.EtapasId), request.EtapasId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoConvocatoria), request.NoConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.NoRadicado), request.NoRadicado);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> CrearCargueArchivo(Guid idSolicitud, int estado, int actividad,Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Estado), estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Actividad), actividad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.UserId), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.DateCreate), DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearEntrevista(Guid idSolicitud, int estado, int actividad, Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Estado), estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Actividad), actividad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.UserId), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.DateCreate), DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearCalificacion(Guid idSolicitud, int estado, int actividad, Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Estado), estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Actividad), actividad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.UserId), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.DateCreate), DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> ActualizarRadicadoAprobado(Guid idSolicitud, string radicado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoRadicado.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoRadicado.Radicado), radicado);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoIntegrante()
        {
            var response = await GetAsyncList<TipoCatalogoPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoParentescoPostulacion()
        {
            var response = await GetAsyncList<TipoCatalogoPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoAyudaTecnica()
        {
            var response = await GetAsyncList<TipoCatalogoPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoDiscapacidaPostulacion()
        {
            var response = await GetAsyncList<TipoCatalogoPostulacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<int> CreateVerification(Guid idSolicitud, int etapa,Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateVerificatePostulacion.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateVerificatePostulacion.EtapaId), etapa);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateVerificatePostulacion.UserId), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateVerificatePostulacion.DateCreate), DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> UpdateDataVericatePostulacion(Guid idSolicitud, string observacion,int etapaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateVerificatePostulacion.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateVerificatePostulacion.Observacion), observacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateVerificatePostulacion.EtapaId), etapaId);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> UpdateStatusVerificate(Guid idSolicitud, int statudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.StatusId), statudId);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> ActualizarEtapaVerificacion(Guid idSolicitud, int statudId,int etapaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.StatusId), statudId);
            parameters.Add("@EtapaId", etapaId);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> UpdateStatusVerificateAoR(Guid idSolicitud, int status, int etapaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.StatusId), status);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.EtapaId), etapaId);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<VerificatePostulacionResponse>> GetDataVerificatePostulacion(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.Sol_Id), idSolicitud);

            var response = await GetAsyncList<VerificatePostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<VerificatePostulacionResponse> GetDataVerificate(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateStatusPostulacion.Sol_Id), idSolicitud);

            var response = await GetAsyncFirst<VerificatePostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarDataVerificatePostulacinoFs(SolicitudPostulacionRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.Identificacion), request.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.TipoSolicitudId), request.TipoSolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudPostulacion.EtapasId), request.EtapasId);
            parameters.Add("@NoRadicado", request.NoRadicado);
            parameters.Add("@NoConvocatoria", request.NoConvocatoria);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<SolicitudPostulacionResponse> ObtenerDatoVrificatePostulacionById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<StagePostulation> GetSolicitudPostulacionById(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsActualizarEstadoYActividadArchivo.Sol_Id), IdSolicitud);

            var response = await GetAsyncFirst<StagePostulation>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<ReportSolicitud>> GetAllSolicitud(int idConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(SpoCrearSolicitudPostulante.IdConvocatoria), idConvocatoria);

            var response = await GetAsyncList<ReportSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }
        public async Task<ModulosPostulacion> ObtenerModalidadPostulacionById(int Mpo_id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarTipoModalidadPostulante.Mpo_Id), Mpo_id);

            var response = await GetAsyncFirst<ModulosPostulacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CreateTrazabilidadFondo(Guid idSolicitud,Guid userName,string observation,
           int actividad , int estado )
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.Sol_Id), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.UserName), userName);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.Observation), observation);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.ActivityId), actividad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.EstadoId), estado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.DateCreate), DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<StagePostulation>> ObtenerTrazabilidadFondo(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateTrazabilidadFondo.Sol_Id), idSolicitud);

            var response = await GetAsyncList<StagePostulation>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<EstadoSolicitudFondo> ObtenerEstadoFondoByRadicado(string noRadicado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("noRadicado", noRadicado);

            var response = await GetAsyncFirst<EstadoSolicitudFondo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearInconsistenciaSolictudFondo(Guid IdSolicitud,string description)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("IFS_ID_SOLICITUD", IdSolicitud);
            parameters.Add("IFS_OBSERVACION", description);
            parameters.Add("IFS_FECHA_CREACION", DateTime.Now);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<InconsistenciaFondoSolidaridad>> ObtemerIncosistenciaSolicitudFondo(Guid IdSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("IFS_ID_SOLICITUD", IdSolicitud);

            var response = await GetAsyncList<InconsistenciaFondoSolidaridad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<IEnumerable<SolicitudPostulacionResponse>> ObtenerDataReportModelFondoSolidaridad(ReportModelFondoSolidaria request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Modelo", request.Modelo);
            parameters.Add("Fuerza", request.Fuerza);
            parameters.Add("UnidadEjecutora", request.UnidadEjecutora);
            parameters.Add("FechaPostulacionInicial", request.FechaPostulacionInicial);
            parameters.Add("FechaPostulacionFinal", request.FechaPostulacionFinal);
            parameters.Add("fechaInicialPrimerAporteDesde", request.FechaInicialPrimerAporteDesde);
            parameters.Add("fechaInicialPrimerAporteHasta", request.FechaInicialPrimerAporteHasta);
            parameters.Add("fechaInicialUltimoAporteDesde", request.FchaUltimoAporteDesde);
            parameters.Add("fechaInicialUltimoAporteHasta", request.FchaUltimoporteHasta);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<SolicitudPostulacionResponse>> ObtenerDataReportEstadoFondoSolidaridad(ReportStatusFondoSolidaridad request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Radicado", request.Radicado);
            parameters.Add("@TipoIdentificacion", request.TipoDocumento);
            parameters.Add("@NumeroIdentificacion", request.NumeroDocumento);
            parameters.Add("@FechaDesde", request.FechaSolicitud);
            parameters.Add("@Fuerza", request.Fuerza);
            parameters.Add("@UnidadEjecutora", request.UnidadEjecutora);
            parameters.Add("@Categoria", request.Categoria); 
            parameters.Add("@Estado", request.Estado);

            var response = await GetAsyncList<SolicitudPostulacionResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
    }
    
}
