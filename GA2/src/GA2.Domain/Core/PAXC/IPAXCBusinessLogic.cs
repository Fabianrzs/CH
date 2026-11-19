using GA2.Application.Dto.CapXc;
using GA2.Application.Dto.Escrituras;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.PAXC
{
    public interface IPAXCBusinessLogic
    {
        Task<Response<ObtenerPAXCDto>> ObtenerPAXC(int IdCiudad);
    }
}
