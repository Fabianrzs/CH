using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Field.Services;
using GA2.Domain.Core.Dodo.Forms.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Domain.Core.ReferenciasSvc.Dodo.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Forms.Services
{
    public class DodoFormsService : DodoCommonIntegrationService, IDodoFormsService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoFormsService(
            IOptions<CHKeysOptions> choptions,
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoFieldService> logger)
            : base(
                choptions,
                httpClient,
                integracionProvider,
                auditoriaRepository,
                logger)
        {
        }

        public Task<Response<FormGetParametersResponse>> GetParametersAsync(
            FormGetParametersRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormGetParametersRequest, FormGetParametersResponse>(
                "api/Forms/Form_GetParameters",
                request,
                cancellationToken);

        public Task<Response<DodoFileOperationResponse>> InsertAsync(
            FormInsertRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormInsertRequest, DodoFileOperationResponse>(
                "api/Forms/Form_Insert",
                request,
                cancellationToken);

        public Task<Response<FormUpdateResponse>> UpdateAsync(
            FormUpdateRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormUpdateRequest, FormUpdateResponse>(
                "api/Forms/Form_Update",
                request,
                cancellationToken);

        public Task<Response<FormDeleteResponse>> DeleteAsync(
            FormDeleteRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormDeleteRequest, FormDeleteResponse>(
                "api/Forms/Form_Delete",
                request,
                cancellationToken);

        public Task<Response<FormGetDataResponse>> GetDataAsync(
            FormGetDataRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormGetDataRequest, FormGetDataResponse>(
                "api/Forms/Form_GetData",
                request,
                cancellationToken);

        public Task<Response<DodoFileOperationResponse>> AttachFilesAsync(
            FormAttachFilesRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormAttachFilesRequest, DodoFileOperationResponse>(
                "api/Forms/Form_AttachFiles",
                request,
                cancellationToken);

        public Task<Response<FormGetFileCodesResponse>> GetFileCodesAsync(
            FormGetFileCodesRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormGetFileCodesRequest, FormGetFileCodesResponse>(
                "api/Forms/Form_GetFileCodes",
                request,
                cancellationToken);

        public Task<Response<FormGetFilesDirectoriesResponse>> GetFilesDirectoriesAsync(
            FormGetFilesDirectoriesRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormGetFilesDirectoriesRequest, FormGetFilesDirectoriesResponse>(
                "api/Forms/Form_GetFilesDirectories",
                request,
                cancellationToken);

        public Task<Response<FormGetBase64StringFileResponse>> GetBase64StringFileAsync(
            FormGetBase64StringFileRequest request,
            CancellationToken cancellationToken = default)
            => PostDodoAsync<FormGetBase64StringFileRequest, FormGetBase64StringFileResponse>(
                "api/Forms/Form_GetBase64StringFile",
                request,
                cancellationToken);
    }
}