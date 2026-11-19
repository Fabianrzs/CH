namespace GA2.Apis.GA2.Endpoints.Integraciones
{
    using global::GA2.Domain.Core.ReferenciasSvc.Storage.Files;
    using global::GA2.Domain.Core.ReferenciasSvc.Storage;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;
    using System.ComponentModel;

    namespace GA2.Services.Controllers
    {
        [ApiController]
        [Route("api/storage")]
        public class StorageController : ControllerBase
        {
            private readonly IStorageFactory _storageFactory;

            public StorageController(
                IStorageFactory storageFactory)
            {
                _storageFactory = storageFactory;
            }

            [HttpPost("upload")]
            public async Task<IActionResult> Upload(
                [FromForm] UploadStorageRequest uploadStorageRequest,
                CancellationToken ct)
            {
                var provider =
                    await _storageFactory.GetProviderAsync();

                var result =
                    await provider.UploadAsync(
                        new StorageRequest
                        {
                            Path = uploadStorageRequest.Path,
                            SameName = uploadStorageRequest.SameName
                        },
                        uploadStorageRequest.File,
                        ct) ;

                return Ok(new
                {
                    Path = result
                });
            }

            [HttpGet("download-text")]
            public async Task<IActionResult> DownloadText(
                [FromQuery] string path,
                CancellationToken ct)
            {
                var provider =
                    await _storageFactory.GetProviderAsync();

                var content =
                    await provider.DownloadTextAsync(
                        new StorageRequest
                        {
                            Path = path
                        },
                        ct);

                return Ok(content);
            }

            [HttpGet("download-bytes")]
            public async Task<IActionResult> DownloadBytes(
                [FromQuery] string path,
                CancellationToken ct)
            {
                var provider =
                    await _storageFactory.GetProviderAsync();

                var bytes =
                    await provider.DownloadBytesAsync(
                        new StorageRequest
                        {
                            Path = path
                        },
                        ct);

                return File(
                    bytes,
                    "application/octet-stream");
            }

            [HttpGet("exists")]
            public async Task<IActionResult> Exists(
                [FromQuery] string path,
                CancellationToken ct)
            {
                var provider =
                    await _storageFactory.GetProviderAsync();

                var exists =
                    await provider.ExistsAsync(
                        new StorageRequest
                        {
                            Path = path
                        },
                        ct);

                return Ok(new
                {
                    Exists = exists
                });
            }

            [HttpDelete]
            public async Task<IActionResult> Delete(
                [FromQuery] string path,
                CancellationToken ct)
            {
                var provider =
                    await _storageFactory.GetProviderAsync();

                await provider.DeleteAsync(
                    new StorageRequest
                    {
                        Path = path
                    },
                    ct);

                return NoContent();
            }
        }
    }
    public class UploadStorageRequest
    {
        public string Path { get; set; } = string.Empty;

        public bool SameName { get; set; } = false;

        public IFormFile File { get; set; } = default;
    }
}
