using Dapper;
using GA2.Application.Dto.CapXc;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.PAXCs;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.PAXC
{
    public class PAXCRepository : DapperGenericRepository, IPAXCRepository
    {
        public PAXCRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<ObtenerUserXCiudadDTO>> ObtenerAbogadosXCiudad(int Id_Ciudad)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPAXCParametros.DPC_ID), Id_Ciudad);

            return await GetAsyncList<ObtenerUserXCiudadDTO>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerUserXCiudadDTO>> ObtenerPeritosXCiudad(int Id_Ciudad)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPAXCParametros.DPC_ID), Id_Ciudad);

            return await GetAsyncList<ObtenerUserXCiudadDTO>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
