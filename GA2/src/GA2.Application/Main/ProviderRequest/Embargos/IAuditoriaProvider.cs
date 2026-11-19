using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest
{
    public interface IAuditoriaProvider
    {
        Task<Response<bool>> GuardarAuditoria(AuditoriaDto request);
    }
}