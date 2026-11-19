using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.PuntosAtencion
{
    public interface IPuntoAtencionBusinessLogic
    {
        Task<Response<PuntosAtencionDto>> CrearPuntoAtencion(PuntosAtencionDto PuntoAtencion);
        Task<Response<PuntosAtencionDto>> ActualizarPuntoAtencion(PuntosAtencionDto PuntoAtencion);

        Task<Response<IEnumerable<PuntosAtencionDto>>> ObtenerPuntoAtencion();
        Task<Response<PuntosAtencionDto>> ObtenerPuntoAtencionPorId(int IdPuntoAtencion);
    }
}
