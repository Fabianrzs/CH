using GA2.Application.Dto;
using GA2.Domain.Entities;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IClienteWorkRepository
    {
        Task<CuerpoArchivoDto> InsertarClienteNomina(CuerpoArchivoDto idCuenta);
        Task<Cliente> ObtenerClientePorTipoIdentificationYNumero(int identificationType, string identificationNumber);
    }
}