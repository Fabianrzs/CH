using GA2.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IEstadoConvocatoriaRepository
    {
        Task<IEnumerable<EstadoConvocatoriaModel>> ObtenerEstadosConvocatoria();
    }
}
