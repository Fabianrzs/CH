using GA2.Domain.Entities.Constructora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IConstructoraRepository
    {
        Task<Constructora> AgregaConstructora(Constructora constructora);

        Task<IEnumerable<Constructora>> ConsultarConstructora();

        Task<Constructora> ConsultarConstructoraById(Guid constructora);
        Task<int> EliminarConstructora(Guid constructora);
        Task<int> ActualizarConstructora(Constructora constructora);
    }
}
