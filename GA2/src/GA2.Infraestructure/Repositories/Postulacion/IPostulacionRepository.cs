using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Postulacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IPostulacionRepository
    {
        Task<IEnumerable<TipoSolictudPostulante>> Consultar_Tipo_Solicitud_Postulacion();
        Task<IEnumerable<TipoModalidadPostulacion>> Consultar_Tipo_Modalidad_Postulacion(int idTipoSolicitud_Postulacion);
        Task<IEnumerable<TipoSubModalidadPostulacion>> Consultar_Spo_Submodalidad_Postulacion(int idTipoSubModalidad_);
        Task<SolicitudPostulante> Spo_Crear_Solicitud_Postulante(SolicitudPostulante solicitud);
        Task<IEnumerable<Postulante>> POS_CREAR_POSTULANTES(Postulante postulante);
        Task<IEnumerable<EstadoPostulacion>> ESP_OBTENER_ESTADO_SOLICITUD();
        Task<SolicitudPostulacionResponse> Consultar_Solicitud_Postulacion_By_Id(long identificacion);
        Task<SolicitudPostulacionResponse> ConsultarSolicitudPostulacionBySolId(Guid IdSolicitud);
        Task<IEnumerable<PostulanteResponse>> Consultar_Postulacion_By_Id(Guid id);
        Task<int> DeleteSolicitudFondoSolidaridad(Guid id);
        Task<SolicitudPostulacionResponse> Update_Status_Aproved(Guid idSolicitud, int estadoName);
        Task<IEnumerable<PostulanteResponse>> Obtener_Postulante_By_Id_Solicitud(Guid idSolicitud);
        Task<SolicitudPostulante> Update_Solicitud_Postulante(SolicitudPostulante solicitudPostulante);
        Task<IEnumerable<Postulante>> Update_Postulante(Postulante postulante);
        Task<Postulante> ObtenerPostulanteByNumeroDocumento(int numeroDocumento);
        Task<int> DeletePostulanteByIdSolicitud(Guid? idSolicitud);
        Task<IEnumerable<ActividadFondoSolidaridad>> ObtenerActividadFondoSolidaridad();
        Task<IEnumerable<TipoListaArchivo>> ConsultarDocumentosPorModalidadPostulacion(DocumentoModalidadPostulacion documento);
        Task<CargeArchivosPostulacion> InsertarArchivoPorSolicitudPostulacion(CargeArchivosPostulacion cargeArchivos);
        Task<bool> EliminarArchivoFSPorSolicitud(EliminarArchivoFSPorSolicitud eliminarArchivo);
        Task<IEnumerable<ArchivoSolicitud>> ConsultarArchivoPorSolicitudFs(Guid idSolicitud);
        Task<IEnumerable<int>> ActualizarEstadoArchivoPostulacion(Guid idSolicitud,int estado,int actividad);
        Task<SolicitudPostulacionResponse> ObtenerDatosArchivoById(Guid IdSolicitud);
        Task<SolicitudPostulante> ObtenerSolicitudPostulacionBySolicitudId(Guid? IdSolicitud);
        Task<int> CrearEntrenvistaFs(Entrevista entrevista);
        Task<EntrevistaResponse> ConsultarEntrevistaBySolId(Guid id);
        Task<int> UpdateDataEntrevistaFs(Entrevista entrevista);
        Task<int> UpdateStatusEntrevista(Guid idSolicitud, int estado);
        Task<SolicitudPostulacionResponse> ObtenerDatosEntrevitsById(Guid IdSolicitud);
        Task<Clasificacion> CrearCalificacionFs(Clasificacion clasificacion);
        Task<CalificacionResponse> ConsultarCalificacionBySolId(Guid idSolicitud);
        Task<Clasificacion> UpdateCalificacionFs(Clasificacion clasificacion);
        Task<SolicitudPostulacionResponse> ObtenerDatosCalificacionById(Guid IdSolicitud);
        Task<int> UpdateStatusCalificacionFs(Guid id, int estadoId);
        Task<int> UpdateObservationArchivoFs(Guid id, string observacion);
        Task<ObservacionArchivo> ConsultarObservationArchivoFsBySolId(Guid id);
        Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarSolicitudPostulacion(SolicitudPostulacionRequest IdSolicitud);
        Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarArchivoFs(SolicitudPostulacionRequest IdSolicitud);
        Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarEntrevistaFs(SolicitudPostulacionRequest IdSolicitud);
        Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarCalicacionFs(SolicitudPostulacionRequest IdSolicitud);
        Task<int> CrearCargueArchivo(Guid idSolicitud, int estado, int actividad,Guid userId);
        Task<int> CrearEntrevista(Guid idSolicitud, int estado, int actividad, Guid userId);
        Task<int> CrearCalificacion(Guid idSolicitud, int estado, int actividad, Guid userId);
        Task<int> ActualizarRadicadoAprobado(Guid idSolicitud, string radicado);
        Task<IEnumerable<TipoCatalogoPostulacion>>ObtenerTipoIntegrante();
        Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoParentescoPostulacion();
        Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoAyudaTecnica();
        Task<IEnumerable<TipoCatalogoPostulacion>> ObtenerTipoDiscapacidaPostulacion();
        Task<int> CreateVerification(Guid idSolicitud,int etapa,Guid userId);
        Task<int> UpdateDataVericatePostulacion(Guid idSolicitud, string observacion,int etapaid);
        Task<int> UpdateStatusVerificate(Guid idSolicitud, int status);
        Task<int> UpdateStatusVerificateAoR(Guid idSolicitud, int status,int etapaId);
        Task<IEnumerable<VerificatePostulacionResponse>> GetDataVerificatePostulacion(Guid idSolicitud);
        Task<VerificatePostulacionResponse> GetDataVerificate(Guid idSolicitud);
        Task<IEnumerable<SolicitudPostulacionResponse>> ConsultarDataVerificatePostulacinoFs(SolicitudPostulacionRequest IdSolicitud);
        Task<SolicitudPostulacionResponse> ObtenerDatoVrificatePostulacionById(Guid IdSolicitud);
        Task<StagePostulation>GetSolicitudPostulacionById (Guid IdSolicitud);
        Task<IEnumerable<ReportSolicitud>> GetAllSolicitud(int idConvocatoria);
        Task<int> ActualizarN_FolioArchivoPostulacion(Guid idSolicitud, int folios);
        Task<ModulosPostulacion> ObtenerModalidadPostulacionById(int Mpo_id);
        Task<SolicitudPostulacionResponse> ObtenerDatosTrazabilidadCalificacionById(Guid IdSolicitud);
        Task<int> CreateTrazabilidadFondo(Guid idSolicitud, Guid userName, string observation,
          int actividad, int estado);
        Task<IEnumerable<StagePostulation>> ObtenerTrazabilidadFondo(Guid idSolicitud);
        Task<EstadoSolicitudFondo> ObtenerEstadoFondoByRadicado(string noRadicado);
        Task<SolicitudPostulacionResponse> ObtenerInformacionSolicitudByRadicado(string radicado);
        Task<int> CrearInconsistenciaSolictudFondo(Guid IdSolicitud, string description);
        Task<int> ActualizarEtapaVerificacion(Guid idSolicitud, int statudId, int etapaId);
        Task<List<InconsistenciaFondoSolidaridad>> ObtemerIncosistenciaSolicitudFondo(Guid IdSolicitud);
        Task<IEnumerable<SolicitudPostulacionResponse>> ObtenerDataReportModelFondoSolidaridad(ReportModelFondoSolidaria request);
        Task<IEnumerable<SolicitudPostulacionResponse>> ObtenerDataReportEstadoFondoSolidaridad(ReportStatusFondoSolidaridad request);
    }
}
