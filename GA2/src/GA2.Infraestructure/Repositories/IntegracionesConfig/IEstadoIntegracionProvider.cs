using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Integraciones
{
    public interface IEstadoIntegracionProvider
    {
        Task<IEnumerable<ParametroIntegracion>> ObtenerParametrosIntegraciones();

        Task<bool> IntegracionActiva(string integracion);
    }
}
