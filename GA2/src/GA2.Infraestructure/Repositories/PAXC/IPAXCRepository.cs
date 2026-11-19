using GA2.Application.Dto.CapXc;
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.PAXC
{
    public interface IPAXCRepository
    {
        Task<IEnumerable<ObtenerUserXCiudadDTO>> ObtenerPeritosXCiudad(int Id_Ciudad);
        Task<IEnumerable<ObtenerUserXCiudadDTO>> ObtenerAbogadosXCiudad(int Id_Ciudad);
    }
}
