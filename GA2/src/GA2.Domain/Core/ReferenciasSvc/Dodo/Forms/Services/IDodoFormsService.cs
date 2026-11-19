using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Forms.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Forms.Services
{
    public interface IDodoFormsService
    {
        /// <summary>Obtiene la configuración/parámetros de un formulario.</summary>
        Task<Response<FormGetParametersResponse>> GetParametersAsync(
            FormGetParametersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Inserta un nuevo registro (radicado) en un formulario.</summary>
        Task<Response<DodoFileOperationResponse>> InsertAsync(
            FormInsertRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Actualiza registros de un formulario según filtro.</summary>
        Task<Response<FormUpdateResponse>> UpdateAsync(
            FormUpdateRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Elimina registros de un formulario según filtro.</summary>
        Task<Response<FormDeleteResponse>> DeleteAsync(
            FormDeleteRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta datos/registros de un formulario con filtros y paginación.</summary>
        Task<Response<FormGetDataResponse>> GetDataAsync(
            FormGetDataRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Adjunta uno o varios archivos a un radicado existente.</summary>
        Task<Response<DodoFileOperationResponse>> AttachFilesAsync(
            FormAttachFilesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Obtiene los códigos y propiedades de archivos asociados a un radicado.</summary>
        Task<Response<FormGetFileCodesResponse>> GetFileCodesAsync(
            FormGetFileCodesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta los directorios configurados para un formulario.</summary>
        Task<Response<FormGetFilesDirectoriesResponse>> GetFilesDirectoriesAsync(
            FormGetFilesDirectoriesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Descarga un archivo en Base64 por su código.</summary>
        Task<Response<FormGetBase64StringFileResponse>> GetBase64StringFileAsync(
            FormGetBase64StringFileRequest request,
            CancellationToken cancellationToken = default);
    }
}
