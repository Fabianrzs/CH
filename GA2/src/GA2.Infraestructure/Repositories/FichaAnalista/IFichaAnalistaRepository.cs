using GA2.Application.Dto.FichaAnalista;
using GA2.Domain.Entities.FichaAnalista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.FichaAnalista
{
    public interface IFichaAnalistaRepository
    {
        Task<FichaAnalistas> CrearFichaAnalista(FichaAnalistas FARequest);
        Task<IEnumerable<FichaAnalistas>> ObtenerFichaAnalista(ObtenerFichaAnalistaDto FARequest);
    }
}
