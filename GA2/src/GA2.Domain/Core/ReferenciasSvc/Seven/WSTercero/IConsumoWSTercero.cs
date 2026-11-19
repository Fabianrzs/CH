using GA2.Application.Dto.Seven;
using GA2.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero
{
    public interface IConsumoWSTercero
    {
        Task<InsertarGnTerceResponse> InsertarGnTerceAsync(InsertarGnTerceRequest request, CancellationToken cancellationToken);
    }
}