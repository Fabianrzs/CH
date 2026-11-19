using GA2.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Integraciones
{
    public class EstadoIntegracionRepository : DapperGenericRepository, IEstadoIntegracionProvider
    {
        public EstadoIntegracionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<ParametroIntegracion>> ObtenerParametrosIntegraciones()
        {
            return await GetAsyncList<ParametroIntegracion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

        }

        public async Task<bool> IntegracionActiva(string integracion)
        {
            if (string.IsNullOrWhiteSpace(integracion))
            {
                return false;
            }

            var parametrosIntegraciones =
                await ObtenerParametrosIntegraciones();

            if (parametrosIntegraciones is null ||
                !parametrosIntegraciones.Any())
            {
                return false;
            }

            var parametroEstado = parametrosIntegraciones
                .FirstOrDefault(p =>
                    !string.IsNullOrWhiteSpace(p.INTEGRACION) &&
                    !string.IsNullOrWhiteSpace(p.PARAMETRO) &&
                    p.INTEGRACION.Equals(
                        integracion,
                        StringComparison.OrdinalIgnoreCase) &&
                    p.PARAMETRO.Equals(
                        "ESTADO",
                        StringComparison.OrdinalIgnoreCase));

            if (parametroEstado is null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(parametroEstado.VALOR))
            {
                return false;
            }

            return bool.TryParse(
                parametroEstado.VALOR,
                out bool estado)
                && estado;
        }
    }
}
