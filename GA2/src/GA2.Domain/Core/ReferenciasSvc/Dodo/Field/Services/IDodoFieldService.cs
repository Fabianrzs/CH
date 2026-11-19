using GA2.Domain.Core.Dodo.Field.Models;
using GA2.Domain.Core.Dodo.Lists.Models;
using GA2.Transversals.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Field.Services
{
    public interface IDodoFieldService
    {
        Task<Response<FieldGetValuesResponse>> GetFieldValuesAsync(
            FieldGetValuesRequest request,
            CancellationToken cancellationToken = default);

        Task<Response<ListGetItemsResponse>> GetListItemsAsync(
            ListGetItemsRequest request,
            CancellationToken cancellationToken = default);
    }
}
