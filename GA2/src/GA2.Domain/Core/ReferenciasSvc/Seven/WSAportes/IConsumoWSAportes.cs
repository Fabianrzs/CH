using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes
{
    public interface IConsumoWSAportes
    {
        Task<GetAporteResponse> GetAporteAsync(GetAporteRequest request, CancellationToken cancellationToken);
    }
}