using GA2.Application.Dto.AuditoriaNovedades;
using GA2.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure
{
    public interface IAuditoriaNovedadesRepository
    {
        Task<IEnumerable<AuditoriaNovedadesGet>> ConsultarAuditoriaNovedades(int idModulo);
        Task<AuditoriaNovedades> InsertarAuditoriaNovedades(AuditoriaNovedades request);
        Task<ResponseValidaNovedadesFondoSolidaridadDto> ValidaNovedadesFondoSolidaridad(RequestValidaNovedadesFondoSolidaridadDto request);
    }
}
