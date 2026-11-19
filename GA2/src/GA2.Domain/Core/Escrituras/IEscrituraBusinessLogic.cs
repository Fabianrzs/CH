using GA2.Application.Dto;
using GA2.Application.Dto.Escrituras;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Escrituras
{
    public interface IEscrituraBusinessLogic
    {
        Task<Response<CreacionEscritura>> CrearEscritura(CreacionEscritura escritura);

        Task<Response<ObtenerEscrituras>> ObtenerEscritura(RequestObtenerEscrituras escritura);
    }
}
