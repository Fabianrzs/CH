using GA2.Domain.Core.Dodo.Offices.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Offices.Services
{
    public class DodoOfficesService : CommonIntegracionCliente, IDodoOfficesService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoOfficesService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoOfficesService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<GetOfficesResponse>> GetOfficesAsync(
            GetOfficesRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<GetOfficesRequest, GetOfficesResponse>(
                "api/Offices/GetOffices",
                request,
                cancellationToken);
    }
}
