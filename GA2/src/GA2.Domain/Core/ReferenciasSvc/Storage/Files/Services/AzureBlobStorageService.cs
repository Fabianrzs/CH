using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GA2.Domain.Core.ReferenciasSvc.Storage.Files;
using GA2.Domain.Core.ReferenciasSvc.Storage.Files.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public class AzureStorageProvider : IStorageProvider
    {
        private readonly AzureStorageConfig _config;
        private readonly ILogger<AzureStorageProvider> _logger;

        public AzureStorageProvider(
            IOptions<AzureStorageConfig> config,
            ILogger<AzureStorageProvider> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        private BlobContainerClient GetContainer()
        {
            return new BlobContainerClient(
                _config.ConnectionString,
                _config.ContainerName);
        }

        public async Task<string> UploadAsync(
            StorageRequest request,
            IFormFile file,
            CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo inválido");

            var container = GetContainer();

            await container.CreateIfNotExistsAsync(
                PublicAccessType.None,
                cancellationToken: ct);

            string extension =
                Path.GetExtension(file.FileName);

            string blobName =
                string.IsNullOrWhiteSpace(request.Path)
                    ? $"{Guid.NewGuid()}{extension}"
                    : request.Path;

            var blobClient =
                container.GetBlobClient(blobName);

            using var stream =
                file.OpenReadStream();

            await blobClient.UploadAsync(
                stream,
                overwrite: true,
                cancellationToken: ct);

            _logger.LogInformation(
                "Archivo almacenado en Azure: {Blob}",
                blobName);

            return blobName;
        }

        public async Task<string> DownloadTextAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var blobClient =
                GetContainer().GetBlobClient(request.Path);

            if (!await blobClient.ExistsAsync(ct))
            {
                throw new FileNotFoundException(
                    $"Blob no encontrado: {request.Path}");
            }

            var response =
                await blobClient.DownloadContentAsync(ct);

            return response.Value.Content.ToString();
        }

        public async Task<byte[]> DownloadBytesAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var blobClient =
                GetContainer().GetBlobClient(request.Path);

            if (!await blobClient.ExistsAsync(ct))
            {
                throw new FileNotFoundException(
                    $"Blob no encontrado: {request.Path}");
            }

            var response =
                await blobClient.DownloadContentAsync(ct);

            return response.Value.Content.ToArray();
        }

        public async Task<bool> ExistsAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var blobClient =
                GetContainer().GetBlobClient(request.Path);

            return await blobClient.ExistsAsync(ct);
        }

        public async Task DeleteAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            var blobClient =
                GetContainer().GetBlobClient(request.Path);

            await blobClient.DeleteIfExistsAsync(
                cancellationToken: ct);

            _logger.LogInformation(
                "Archivo eliminado de Azure: {Blob}",
                request.Path);
        }
    }
}