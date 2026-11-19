using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public sealed class LocalStorageProvider : IStorageProvider
    {
        private readonly LocalBlogStorageConfig _config;
        private readonly ILogger<LocalStorageProvider> _logger;


        public LocalStorageProvider(
            IOptions<LocalBlogStorageConfig> config,
            ILogger<LocalStorageProvider> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        private string BuildPhysicalPath(string relativePath)
        {
            relativePath = relativePath
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());

            return Path.Combine(
                _config.CarpetaServidorStorage,
                relativePath);
        }

        public async Task<string> UploadAsync(
            StorageRequest request,
            IFormFile file,
            CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(
                    "Archivo inválido");

            string fileName = request.SameName
                ? Path.GetFileName(file.FileName)
                : $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            string relativePath =
                Path.Combine(
                    request.Path,
                    fileName);

            string physicalPath =
                BuildPhysicalPath(relativePath);

            string? folder =
                Path.GetDirectoryName(physicalPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder!);

            await using var stream =
                new FileStream(
                    physicalPath,
                    FileMode.Create);

            await file.CopyToAsync(stream, ct);

            _logger.LogInformation(
                "Archivo almacenado: {Path}",
                physicalPath);

            return relativePath;
        }

        public async Task<string> DownloadTextAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            string physicalPath =
                BuildPhysicalPath(request.Path);

            if (!File.Exists(physicalPath))
            {
                throw new FileNotFoundException(
                    physicalPath);
            }

            return await File.ReadAllTextAsync(
                physicalPath,
                ct);
        }

        public async Task<byte[]> DownloadBytesAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            string physicalPath =
                BuildPhysicalPath(request.Path);

            if (!File.Exists(physicalPath))
            {
                throw new FileNotFoundException(
                    physicalPath);
            }

            return await File.ReadAllBytesAsync(
                physicalPath,
                ct);
        }

        public Task<bool> ExistsAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            string physicalPath =
                BuildPhysicalPath(request.Path);

            return Task.FromResult(
                File.Exists(physicalPath));
        }

        public Task DeleteAsync(
            StorageRequest request,
            CancellationToken ct = default)
        {
            string physicalPath =
                BuildPhysicalPath(request.Path);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            return Task.CompletedTask;
        }
    }
}
