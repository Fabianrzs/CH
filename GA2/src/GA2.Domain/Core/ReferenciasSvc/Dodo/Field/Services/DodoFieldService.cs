using GA2.Domain.Core.Dodo.Field.Models;
using GA2.Domain.Core.Dodo.Lists.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Domain.Core.ReferenciasSvc.Dodo.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Field.Services
{
    public class DodoFieldService : DodoCommonIntegrationService, IDodoFieldService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoFieldService(
            IOptions<CHKeysOptions> choptions,
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoFieldService> logger)
            : base(choptions, httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<FieldGetValuesResponse>> GetFieldValuesAsync(
            FieldGetValuesRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<FieldGetValuesRequest, FieldGetValuesResponse>(
                "api/Field/Field_GetValues",
                request,
                cancellationToken);

        public Task<Response<ListGetItemsResponse>> GetListItemsAsync(
            ListGetItemsRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<ListGetItemsRequest, ListGetItemsResponse>(
                "api/Lists/List_GetItems",
                request,
                cancellationToken);
    }
}
