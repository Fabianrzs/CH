using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP
{
    public interface IConsumoCDP
    {
        Task<AnularCDPResponse> AnularCDPAsync(AnularCDPRequest request, CancellationToken cancellationToken);
        Task<AplicarCDPResponse> AplicarCDPAsync(AplicarCDPRequest request, CancellationToken cancellationToken);
        Task<CrearCDPResponse> CrearCDPAsync(CrearCDPRequest request, CancellationToken cancellationToken);
        Task<InsertarCDPResponse> InsertarCDPAsync(InsertarCDPRequest request, CancellationToken cancellationToken);
        Task<InsertarDetCDPResponse> InsertarDetCDPAsync(InsertarDetCDPRequest request, CancellationToken cancellationToken);
    }
}