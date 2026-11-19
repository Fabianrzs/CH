using GA2.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.EstadoConvocatoria
{
    public class EstadoConvocatoriaRepository :DapperGenericRepository, IEstadoConvocatoriaRepository
    {
        public EstadoConvocatoriaRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<IEnumerable<EstadoConvocatoriaModel>> ObtenerEstadosConvocatoria()
        {
           var result = await GetAsyncList<EstadoConvocatoriaModel>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return result;
        }
    }
}
