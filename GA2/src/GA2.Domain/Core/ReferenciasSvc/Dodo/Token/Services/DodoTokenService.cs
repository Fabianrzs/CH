using GA2.Domain.Core.Dodo.Token.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Token.Services
{
    public class DodoTokenService : CommonIntegracionCliente, IDodoTokenService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoTokenService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoTokenService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<GetTokenResponse>> GetTokenAsync(
            GetTokenRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<GetTokenRequest, GetTokenResponse>(
                "api/Token/GetToken",
                request,
                cancellationToken);
    }
}
