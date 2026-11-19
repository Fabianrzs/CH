using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities.InfoVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.InfoVendedor
{
    public interface IInformacionVendedorRepository
    {
        Task<InformacionVendedor> CrearInformacionVendedor(InformacionVendedor informacionVendedor);
        Task<IEnumerable<InformacionVendedor>> ObtenerInformacionVendedor(consultarVendedorDto request);
        Task<IEnumerable<InformacionVendedor>> EliminarInformacionVendedor(InformacionVendedor informacionVendedor);
        Task<InformacionVendedor> ObtenerInformacionVendedorPorIdSolicCredito(Guid soc_id); 

    }
}
