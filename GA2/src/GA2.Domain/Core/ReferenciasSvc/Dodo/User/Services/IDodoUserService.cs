using GA2.Domain.Core.Dodo.User.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.User.Services
{
    public interface IDodoUserService
    {
        Task<Response<UserInfoResponse>> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellationToken = default);
    }
}
