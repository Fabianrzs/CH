using GA2.Domain.Entities.PuntosAtencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.PuntosAtencion
{
    public interface IPuntoAtencionRepository
    {
        Task<PuntoAtencion> CrearPuntosAtencion(PuntoAtencion EscrituraRequest);
        Task<PuntoAtencion> ActualizarPuntosAtencion(PuntoAtencion EscrituraRequest);
        Task<IEnumerable<PuntoAtencion>> ObtenerPuntosAtencion();
        Task<PuntoAtencion> ObtenerPuntosAtencionPorId(int IdPuntoAtencion);

    }
}
