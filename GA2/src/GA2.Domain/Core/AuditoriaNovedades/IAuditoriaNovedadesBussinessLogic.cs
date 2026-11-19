using GA2.Application.Dto;
using GA2.Application.Dto.AuditoriaNovedades;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IAuditoriaNovedadesBussinessLogic
    {
        Task<Response<IEnumerable<AuditoriaNovedadesGetDto>>> ConsultaAuditoriaNovedades(int idModulo);
        Task<Response<AuditoriaNovedadesDto>> InsertarAuditoriaNovedades(AuditoriaNovedadesDto request);
        Task<Response<ResponseValidaNovedadesFondoSolidaridadDto>> ValidaNovedadesFondoSolidaridad(RequestValidaNovedadesFondoSolidaridadDto request);
        Task<Response<GeneracionArchivosResponse>> GenerarExcelNovedaes(int idModulo);
    }
}
