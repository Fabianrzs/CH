using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Postulacion.Tipo;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Postulacion
{
    public interface IPostulacionBussineLogic
    {
        Task<Response<IEnumerable<TipoSolicitudPostulacionDto>>> ConsultarSolicitud();
        Task<Response<IEnumerable<TipoModaliadPostulacionDto>>> ConsultarModalidad(int id);
        Task<Response<IEnumerable<TipoSubModalidadPostulacionDto>>> ConsultarSubModalidad(int id);
        Task<Response<SolicitudPostulacionDto>> CrearSolicitudPostante(SolicitudPostulacionDto solicitudPostulacion);
        Task<Response<SolicituPostulancionResponseDto>> ValidarSolicitud(long identificacion);
        Task<Response<SolicituPostulancionResponseDto>> UpdateStatusRecline(Guid idSolicitud,Guid userId);
        Task<Response<SolicituPostulancionResponseDto>> UpdateStatusAproved(Guid idSolicitud, Guid userId);
        Task<Response<SolicitudPostulacionDto>> UpdateSolicitud(SolicitudPostulacionDto solicitud);
        Task<Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>>> TipoEstadoSolicitudPostulacion();
        Task<Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>>> ObtenerListaArchivoPorModalidad(DocumentoModalidadPostulacionDto documento);
        Task<Response<bool>> CargarDocumentosSolicitud(List<CargarDocumentosSolicitudDto> CargarDocumentosSolicitudDto, Guid IdSolicitud,Guid userId);
        Task<FileResult> DescargarArchivoPorSolicitud(string rutaStorage);
        Task<Response<bool>> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitudDto eliminarArchivoPorSolicitudDto);
        Task<Response<IEnumerable<ArchivoSolicitudDto>>> ConsultarArchivoPorSolicitudPostulacion(Guid idSolicitud);
        Task<Response<SolicituPostulancionResponseDto>> AprobarCargoDocumento(AprobarCargueDocumentoDto request, Guid userId );
        Task<Response<SolicituPostulancionResponseDto>> RechazarCargueDocumento(Guid IdSolicitud);
        Task<Response<SolicituPostulancionResponseDto>> AnularCargoDocumento(Guid IdSolicitud);
        Task<Response<IEnumerable<PostulanteDto>>> CrearPostulanteFondoSolidaridad(IEnumerable<PostulanteDto> solicitudPostulacion);
        Task<Response<string>> CrearEntrevista(EntrevistaDto entrevista);
        Task<Response<EntresvistaResponseDto>> ObtenerEntrevitsById(Guid solId);
        Task<Response<IEnumerable<PostulanteResponseDto>>> ObtenerPostulanteById(Guid solId);
        Task<Response<string>> UpdateEntrevista(EntrevistaDto entrevista);
        Task<Response<SolicituPostulancionResponseDto>> AprobarEntrevista(Guid solId,Guid userId);
        Task<Response<SolicituPostulancionResponseDto>> AnularEntrevista(Guid solId,Guid UserId);
        Task<Response<SolicituPostulancionResponseDto>> RechazarEntrevista(Guid IdSolicitud,Guid userId);
        Task<Response<ClasficacionDto>> CrearClasificacion(ClasficacionDto clasficacion);
        Task<Response<CalificacionResponseDto>> ConsultarCalificacionByIdSolcitud(Guid solId);
        Task<Response<ClasficacionDto>> UpdateClasificacion(ClasficacionDto clasficacion);
        Task<Response<SolicituPostulancionResponseDto>> AprobarCalificacion(Guid solId,Guid userId);
        Task<Response<SolicituPostulancionResponseDto>> AnularCalificacion(Guid solId, Guid userId);
        Task<Response<SolicituPostulancionResponseDto>> RechazarCalificacion(Guid IdSolicitud, Guid userId);
        Task<Response<string>> ActualizarObservacionArchivos(ActualizarObservacionDto observacion);
        Task<Response<ObservacionArchivoDto>> ConsultarObservacionArchivos(Guid IdSolicitud);
        Task<Response<IEnumerable<SolicituPostulancionResponseDto>>> ConsultarSolicitudPostultacion(SolicitudPostulacionRequestDto requestDto);
        Task<Response<IEnumerable<EtapaPostulacionDto>>> ObtenerEtapaPostulacion();
        Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoIntegrante();
        Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoParentescoPostulacion();
        Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoAyudaTecnica();
        Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoDiscapacidaPostulacion();
        Task<Response<string>> UpdateDateVerificatePostulacion(VeficatePostulacionDto request);
        Task<Response<string>> AprovedVerificateNovedadesFs(VeficateFondoDto verificate, Guid userId);
        Task<Response<string>> RechazarVerificatePostulacion(Guid idSolicitud, Guid userId);
        Task<Response<string>> AnularVerificatePostulacion(Guid idSolicitud, Guid userId);
        Task<Response<IEnumerable<VerificatePostulacionDto>>> GetDataVerificatePostulacion(Guid idSolicitud);
        Task<Response<List<StagePostulationDto>>> GetStagePostulacion(Guid idSolicitud);
        Task<Response<IEnumerable<ReportRankingDto>>> GetReporteRanking(int idConvocatoria);
        Task<Response<string>> AprovedVerificateEmbargo(VeficateFondoDto verificate, Guid userId);
        Task<Response<string>> RechazarVerificateEmbargosPostulacion(VeficateFondoDto veficate, Guid userId);
        Task<Response<string>> AnularVerificateEmbargosPostulacion(VeficateFondoDto veficate, Guid userId);
        Task<Response<string>> AprovedVerificateCuentasIndividuales(VeficateFondoDto verificate, Guid userId);
        Task<Response<string>> RechazarVerificateCuentaIndividuales(VeficateFondoDto veficate, Guid userId);
        Task<Response<string>> AnularVerificateCuentasIndividuales(VeficateFondoDto veficate, Guid userId);
        Task<Response<string>> ActualizarFolio(Guid IdSolicitud, int numeroFolios);
        Task<FileResult> GetReporteGenralRanking(int idConvocatoria);
        Task<Response<SolicituPostulancionResponseDto>> UpdateStatusRequestStage(UpdateRequestStageDto request,Guid userId);
        Task<Response<SolicituPostulancionResponseDto>> AnularEtapaFondoSolidaridad(UpdateRequestStageDto request, Guid userId);
        Task<Response<EstadoSolicitudFondoDto>> ObtenerEstadoSolicitudFondo(string noRadicado);
        Task<Response<SolicituPostulancionResponseDto>> ObtenerInformcacionPostulacion(string radicado);
        Task<Response<string>> GuardarIncosistenciaSolicitudFondo(IncosistenciaFondoRequest request);
        Task<Response<List<InconsistenciaFondoSolidaridadDTO>>> ObtemerIncosistenciaSolicitudFondo(Guid idSolicitud);
        Task<List<ReportModelFondoSolidariaResponse>> ObtenerDataReportModelFondoSolidaridad(ReportModelFondoSolidaria request);
        Task<List<ReportStatusFondoSolidariaResponse>> ObtenerDataReporlFondoSolidaridadPorEstadoTramite(ReportStatusFondoSolidaridad request);
    }
}
