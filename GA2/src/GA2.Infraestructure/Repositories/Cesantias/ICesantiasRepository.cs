using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICesantiasRepository
    {
        Task<ObtenerTramiteCesantias> ObtenerTramiteCesantias(ParametrosObtenerCesantias solicitud);
        Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldos();
        Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(Int32 codOperacion);
        Task<obtenerTipoSolicitud> ObtenerTipoSolicitudPorSolIdySubModalidad(Guid Sol_Id, Guid Sub_Modalidad);
    }
}
