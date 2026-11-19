using GA2.Domain.Core.Dodo.User.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Version.Services
{
    public interface IDodoVersionService
    {
        Task<Response<string>> GetVersionInfoAsync(
            CancellationToken cancellationToken = default);
    }
}
