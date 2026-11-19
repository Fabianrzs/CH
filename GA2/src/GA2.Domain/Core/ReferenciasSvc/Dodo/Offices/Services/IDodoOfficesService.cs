using GA2.Domain.Core.Dodo.Offices.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Offices.Services
{
    public interface IDodoOfficesService
    {
        Task<Response<GetOfficesResponse>> GetOfficesAsync(
            GetOfficesRequest request,
            CancellationToken cancellationToken = default);
    }
}
