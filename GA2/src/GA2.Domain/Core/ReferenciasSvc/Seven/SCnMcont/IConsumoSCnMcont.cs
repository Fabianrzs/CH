using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont
{
    public interface IConsumoSCnMcont
    {
        Task<AnularCnMcontResponse> AnularCnMcontAsync(AnularCnMcontRequest request, CancellationToken cancellationToken);
        Task<AplicarCnMcontResponse> AplicarCnMcontAsync(AplicarCnMcontRequest request, CancellationToken cancellationToken);
        Task<InsertarCnDmconResponse> InsertarCnDmconAsync(InsertarCnDmconRequest request, CancellationToken cancellationToken);
        Task<InsertarCnMcontResponse> InsertarCnMcontAsync(InsertarCnMcontRequest request, CancellationToken cancellationToken);
        Task<InsertarMovContableResponse> InsertarMovContableAsync(InsertarMovContableRequest request, CancellationToken cancellationToken);
        Task<RevertirCnMcontResponse> RevertirCnMcontAsync(RevertirCnMcontRequest request, CancellationToken cancellationToken);
        Task<SetContabToperResponse> SetContabToperAsync(SetContabToperRequest request, CancellationToken cancellationToken);
    }
}