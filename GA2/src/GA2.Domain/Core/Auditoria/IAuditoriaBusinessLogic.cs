using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IAuditoriaBusinessLogic
    {
        Task<Response<listaAuditorias>> ObtenerlogAuditoria(ConsultaAuditoriaDto parametros);
        Task<Response<AuditoriaDto>> InsertarLogAuditoria(AuditoriaDto request);
        Task<Response<IEnumerable<TablasAuditoriaDto>>> ObtenerTablasAuditoria();
        Task LogAuditAsync<T>(int transId, T oldEntity, T newEntity, string userName) where T : class;
        string Ip();
        UserAgentInfo ParseUserAgent(string userAgent);
    }
}
