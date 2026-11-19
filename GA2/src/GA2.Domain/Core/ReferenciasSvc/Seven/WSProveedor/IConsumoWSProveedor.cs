using GA2.Application.Dto.Seven;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor
{
    public interface IConsumoWSProveedor
    {
        Task<ActModalidadCupoResponse> ActModalidadCupoAsync(ActModalidadCupoRequest request, CancellationToken cancellationToken);
        Task<ChangeNitResponse> InsertarCnMcontAsync(ChangeNitRequest request, CancellationToken cancellationToken);
        Task<SyncEmpleadoPvdorResponse> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorRequest request, CancellationToken cancellationToken);
        Task<SyncFaClienResponse> SyncFaClienAsync(SyncFaClienRequest request, CancellationToken cancellationToken);
        Task<SyncFaClienComplexResponse> SyncFaClienComplexAsync(SyncFaClienComplexRequest request, CancellationToken cancellationToken);
        Task<SyncFaContaResponse> SyncFaContaAsync(SyncFaContaRequest request, CancellationToken cancellationToken);
        Task<SyncGnTerceResponse> SyncGnTerceAsync(SyncGnTerceRequest request, CancellationToken cancellationToken);
        Task<SyncPoPvdorResponse> SyncPoPvdorAsync(SyncPoPvdorRequest request, CancellationToken cancellationToken);
    }
}