using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura
{
    public interface IConsumoWSFactura
    {
        Task<AnularFacturaResponse> AnularFacturaAsync(AnularFacturaRequest request, CancellationToken cancellationToken);
        Task<AnularPoFactuResponse> AnularPoFactuAsync(AnularPoFactuRequest request, CancellationToken cancellationToken);
        Task<InsertarFacturaPResponse> InsertarFacturaPAsync(InsertarFacturaPRequest request, CancellationToken cancellationToken);
        Task<InsertarPoFactuResponse> InsertarPoFactuAsync(InsertarPoFactuRequest request, CancellationToken cancellationToken);
        Task<InsertarPoFactuPResponse> InsertarPoFactuPAsync(InsertarPoFactuPRequest request, CancellationToken cancellationToken);
    }
}