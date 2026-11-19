using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.FichaAnalista;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.FichaAnalista
{
    public interface IFichaAnalistaBusinessLogic
    {
        Task<Response<FichaAnalistaDto>> CreaFichaAnalista(FichaAnalistaDto fichaAnalista);
        Task<Response<IEnumerable<FichaAnalistaDto>>> ObtenerFichaAnalista(ObtenerFichaAnalistaDto fichaAnalista);
    }
}
