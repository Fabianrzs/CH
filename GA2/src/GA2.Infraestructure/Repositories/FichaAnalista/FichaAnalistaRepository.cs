using Dapper;
using GA2.Application.Dto.FichaAnalista;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.FichaAnalista;
using GA2.Infraestructure.Data.Escrituras;
using GA2.Infraestructure.Data.FichaAnalista;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.FichaAnalista
{
    public class FichaAnalistaRepository : DapperGenericRepository, IFichaAnalistaRepository
    {
        public FichaAnalistaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<FichaAnalistas> CrearFichaAnalista(FichaAnalistas FARequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.FA_ID), FARequest.FA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.SOC_ID), FARequest.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.FA_NOMBRE), FARequest.FA_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.FA_CARGO), FARequest.FA_CARGO);
            return await GetAsyncFirst<FichaAnalistas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FichaAnalistas>> ObtenerFichaAnalista(ObtenerFichaAnalistaDto FARequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.FA_ID), FARequest.Fa_Id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFichaAnalista.SOC_ID), FARequest.Soc_id);
            return await GetAsyncList<FichaAnalistas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}

