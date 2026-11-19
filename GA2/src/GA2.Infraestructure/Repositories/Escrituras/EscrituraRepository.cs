using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Escritura;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Escrituras;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Escrituras
{
    public class EscrituraRepository : DapperGenericRepository,IEscriturasRepository
    {
        public EscrituraRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Escritura> CrearEscritura(Escritura EscrituraRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.SOC_NRO_SOLICITUD), EscrituraRequest.SOC_ID_SOLICITUD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.CLI_IDENTIFICACION), EscrituraRequest.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.ESC_FECHA), EscrituraRequest.ESC_FECHA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.ESC_ESCRITURA), EscrituraRequest.ESC_ESCRITURA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.ESC_NOTARIA), EscrituraRequest.ESC_NOTARIA);
            return await GetAsyncFirst<Escritura>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Escritura> ObtenerEscritura(Escritura EscrituraRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.SOC_NRO_SOLICITUD), EscrituraRequest.SOC_ID_SOLICITUD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearEscrituras.CLI_IDENTIFICACION), EscrituraRequest.CLI_IDENTIFICACION);
            return await GetAsyncFirst<Escritura>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}
