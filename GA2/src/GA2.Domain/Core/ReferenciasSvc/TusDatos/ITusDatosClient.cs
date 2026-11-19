using GA2.Domain.Core.ReferenciasSvc.TusDatos.Dto;
using GA2.Domain.Core.ReferenciasSvc.TusDatos.Models;
using GA2.Transversals.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc
{
    public interface ITusDatosClient
    {
        public Task<Response<DatosClienteResponse>> ConsultarAsync(
            string identification,
            string documentType,
            bool checkHistory,
            CancellationToken cancellationToken);

         public Task<Response<DatosClienteJobResponse>>  ConsultarJobAsync(
            string jobId,
            string identification,
            CancellationToken cancellationToken);
        public Task<Response<VigiaResponse>> ConsultarVigiaAsync(string identification, string documentType, CancellationToken cancellationToken);
    }
}
