using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Solicitud.WorkManager
{
    public interface IGestorDocumentSolicitudes
    {
        Task<Response<Form_InsertResponse>> RadicarSolicitud(ObtenerTramiteSolicitudDto obtenerTramiteSolicitudDto);
        Task<Response<Form_UpdateResponse>> ActualizarRadicadoSolicitud(ActualizarRadicadoSolicitudTareaDto obtenerTramiteSolicitudDto);
        Task<Response<FileResponseDto>> ObtenerDocumentosPorRadicado(string numeroRadicado);
        Task<Response<string>> ObtenerEnlaceDodo(string numeroRadicado);
        Task<Response<Form_GetDataResponse>> ObtenerFormulariosDodo(string IdentificacionCliente, string FormCode);
        Task<Response<Form_InsertResponse>> RadicarSolicitud378or113(CrearRadicadoCartaIncosistencias request );
    }
}