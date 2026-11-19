using GA2.Domain.Core.Dodo.Token.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Token.Services
{
    public interface IDodoTokenService
    {
        Task<Response<GetTokenResponse>> GetTokenAsync(
            GetTokenRequest request,
            CancellationToken cancellationToken = default);
    }
}
