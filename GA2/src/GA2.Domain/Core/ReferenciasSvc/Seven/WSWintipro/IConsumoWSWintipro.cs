using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro
{
    public interface IConsumoWSWintipro
    {
        Task<GetCuentaContableResponse> GetCuentaContableAsync(GetCuentaContableRequest request, CancellationToken cancellationToken);
        Task<GetCuentaContableTOResponse> GetCuentaContableTOAsync(GetCuentaContableTORequest request, CancellationToken cancellationToken);
    }
}