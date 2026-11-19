using GA2.Domain.Core.Dodo.File.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.File.Services
{
    public class DodoFileService : CommonIntegracionCliente, IDodoFileService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoFileService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoFileService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<FileSimpleResponse>> CopyToEPAsync(
            FileCopyToEPRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<FileCopyToEPRequest, FileSimpleResponse>(
                "api/File/File_CopyToEP",
                request,
                cancellationToken);

        public Task<Response<FileSimpleResponse>> CopyToFatherAsync(
            FileCopyToFatherRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<FileCopyToFatherRequest, FileSimpleResponse>(
                "api/File/File_CopyToFather",
                request,
                cancellationToken);

        public Task<Response<FileSimpleResponse>> DeleteFileAsync(
            FileDeleteRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<FileDeleteRequest, FileSimpleResponse>(
                "api/File/File_Delete",
                request,
                cancellationToken);
    }
}
