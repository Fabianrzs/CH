using GA2.Domain.Core.Dodo.User.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.User.Services
{
    public class DodoUserService : CommonIntegracionCliente, IDodoUserService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoUserService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoUserService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<UserInfoResponse>> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<UserInfoRequest, UserInfoResponse>(
                "api/User/User_Info",
                request,
                cancellationToken);
    }
}
