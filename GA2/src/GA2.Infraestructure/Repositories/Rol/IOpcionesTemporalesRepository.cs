using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IOpcionesTemporalesRepository
    {
        Task<OpcionTemporalUsuario> InsertarOpcionesTemporalesUsuario(OpcionTemporalUsuario opcionTemporalUsuario);
        Task<IEnumerable<OpcionTemporalUsuario>> ObtenerOpcionesTemporalesUsuario(Guid idUsuario);
        Task<OpcionTemporalUsuario> EditarOpcionesTemporalesUsuario(OpcionTemporalUsuario opcionTemporalUsuario);
        Task<OpcionTemporalUsuario> EliminarOpcionesTemporalesUsuario(int idOpcionTemporalUsuario);
    }
}
