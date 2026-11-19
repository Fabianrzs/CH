using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Dodo.Common
{
    public abstract class DodoCommonIntegrationService : CommonIntegracionCliente
    {
        public string User { get; }

        public string Token { get; }

        protected DodoCommonIntegrationService(
            IOptions<CHKeysOptions> choptions,
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoCommonIntegrationService> logger)
            : base(
                httpClient,
                integracionProvider,
                auditoriaRepository,
                logger)
        {
            User = choptions.Value.User;
            Token = choptions.Value.Token;
        }

        /// <summary>
        /// Agrega automáticamente el User y Token al header del request.
        /// </summary>
        protected void SetAuthHeader<TRequest>(TRequest request)
            where TRequest : IDodoRequest
        {
            request.Header ??= new DodoHeader();

            request.Header.User = User;
            request.Header.Token = Token;
        }

        /// <summary>
        /// Wrapper genérico para requests Dodo.
        /// Inyecta autenticación automáticamente antes del PostAsync.
        /// </summary>
        protected Task<Response<TResponse>> PostDodoAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : IDodoRequest
        {
            SetAuthHeader(request);

            return PostAsync<TRequest, TResponse>(
                endpoint,
                request,
                cancellationToken);
        }
    }
}