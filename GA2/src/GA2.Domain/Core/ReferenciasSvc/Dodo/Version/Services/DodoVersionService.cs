using GA2.Domain.Core.Dodo.User.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Version.Services
{
    public class DodoVersionService : CommonIntegracionCliente, IDodoVersionService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoVersionService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoVersionService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<string>> GetVersionInfoAsync(
            CancellationToken cancellationToken = default)
            => GetAsync<string>("api/version", cancellationToken);
    }
}
