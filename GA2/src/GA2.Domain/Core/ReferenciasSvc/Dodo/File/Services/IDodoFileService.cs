using GA2.Domain.Core.Dodo.File.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.File.Services
{
    public interface IDodoFileService
    {
        /// <summary>Copia un archivo a un directorio de la estructura archivística.</summary>
        Task<Response<FileSimpleResponse>> CopyToEPAsync(
            FileCopyToEPRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Copia un archivo del formulario hijo al formulario padre.</summary>
        Task<Response<FileSimpleResponse>> CopyToFatherAsync(
            FileCopyToFatherRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Elimina un archivo almacenado en la aplicación.</summary>
        Task<Response<FileSimpleResponse>> DeleteFileAsync(
            FileDeleteRequest request,
            CancellationToken cancellationToken = default);
    }
}
