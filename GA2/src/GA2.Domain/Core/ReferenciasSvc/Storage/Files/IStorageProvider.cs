using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public interface IStorageProvider
    {
        Task<string> DownloadTextAsync(
            StorageRequest request,
            CancellationToken ct = default);

        Task<byte[]> DownloadBytesAsync(
            StorageRequest request,
            CancellationToken ct = default);

        Task<string> UploadAsync(
            StorageRequest request,
            IFormFile file,
            CancellationToken ct = default);

        Task DeleteAsync(
            StorageRequest request,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            StorageRequest request,
            CancellationToken ct = default);
    }
}
