using GA2.Domain.Entities;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{

    /// <summary>
    /// Interfaz del repositorio de Procesos Especiales
    /// </summary>
    /// <author>Cesar Hernandez</author>
    /// <date>14/03/2023</date>
    public interface IProcesosEspecialesRepository
    {
        Task<Producto> CrearProducto(Producto producto);
    }
}
