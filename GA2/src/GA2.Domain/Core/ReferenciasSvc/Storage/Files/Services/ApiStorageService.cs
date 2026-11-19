using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Domain.Core.ReferenciasSvc.Storage.Files.Models;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{

    public class ApiStorageProvider :
        CommonIntegracionCliente,
        IStorageProvider
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Internal;

        public ApiStorageProvider(
             HttpClient httpClient,
             IConfiguration configuration,
             IEstadoIntegracionProvider integracionProvider,
             IAuditoriaRepository auditoriaRepository,
             ILogger<ApiStorageProvider> logger)
             : base(
                 httpClient,
                 integracionProvider,
                 auditoriaRepository,
                 logger)
        {
            httpClient.BaseAddress = new Uri(configuration["ApiStorageGA2"]);
        }

        public async Task<string> UploadAsync(
            StorageRequest request,
            IFormFile file,
            CancellationToken ct = default)
        {
            using var memoryStream =
                new MemoryStream();

            await file.CopyToAsync(memoryStream, ct);

            var uploadRequest =
                new UploadFileRequest
                {
                    Path = request.Path,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Base64Content =
                        Convert.ToBase64String(
                            memoryStream.ToArray())
                };

            var response =
                await PostAsync<
                    UploadFileRequest,
                    string>(
                    "/api/storage/upload",
                    uploadRequest,
                    ct);

            if (!response.IsSuccess)
                throw new Exception(
                    response.ReturnMessage);

            return response.Data;
        }

        public async Task<string> DownloadTextAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var response =
                await GetXmlAsync(
                    $"api/storage/download-text?path={request.Path}",
                    ct);

            if (!response.IsSuccess)
                throw new Exception(
                    response.ReturnMessage);

            return response.Data;
        }

        public async Task<byte[]> DownloadBytesAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var response =
                await GetAsync<DownloadFileResponse>(
                    $"api/storage/download?path={request.Path}",
                    ct);

            if (!response.IsSuccess)
                throw new Exception(
                    response.ReturnMessage);

            return Convert.FromBase64String(
                response.Data.Base64Content);
        }

        public async Task<bool> ExistsAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var response =
                await GetAsync<ExistsResponse>(
                    $"/api/storage/exists?path={request.Path}",
                    ct);

            if (!response.IsSuccess)
                return false;

            return response.Data.Exists;
        }

        public async Task DeleteAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var response =
                await DeleteAsync(
                    $"/api/storage/delete?path={request.Path}",
                    ct);

            if (!response.IsSuccess)
            {
                throw new Exception(
                    response.ReturnMessage);
            }
        }

       
    }
}
