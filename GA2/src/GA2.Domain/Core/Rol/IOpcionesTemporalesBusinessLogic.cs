using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IOpcionesTemporalesBusinessLogic
    {
        Task<Response<OpcionTemporalUsuarioDto>> InsertarOpcionesTemporalesUsuario(OpcionTemporalUsuarioDto opcionTemporalUsuarioDto);
        Task<Response<IEnumerable<OpcionTemporalUsuarioDto>>> ObtenerOpcionesTemporalesUsuario(Guid idUsuario);
        Task<Response<OpcionTemporalUsuarioDto>> EditarOpcionesTemporalesUsuario(OpcionTemporalUsuarioDto opcionTemporalUsuarioDto);
        Task<Response<OpcionTemporalUsuarioDto>> EliminarOpcionesTemporalesUsuario(int idOpcionTemporalUsuario);

    }
}
