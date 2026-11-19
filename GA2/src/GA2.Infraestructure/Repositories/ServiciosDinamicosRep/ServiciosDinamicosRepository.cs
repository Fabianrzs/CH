using Azure;
using Dapper;
using GA2.Domain.Entities.ServiciosDinamicosEnt;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ServiciosDinamicosRep
{
    public class ServiciosDinamicosRepository : DapperGenericRepository, IServiciosDinamicosRepository
    {
        public ServiciosDinamicosRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ServiciosDinamicos> ConsultarEstadoServicio()
        {
            DynamicParameters parameters = new();

            return await GetAsyncFirst<ServiciosDinamicos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    
    }
}
