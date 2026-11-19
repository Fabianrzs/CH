using Dapper;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Domain.Entities.ValidacionTelAfiliado;
using GA2.Infraestructure.Data.DatosAliadoEstrategico;
using GA2.Infraestructure.Data.ValidacionTelAfiliado;
using GA2.Infraestructure.Repositories.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ValidacionTelAfiliado
{
    public class ValidacionTelAfiliadoRepository : DapperGenericRepository, IValidacionTelAfiliadoRepository
    {
        public ValidacionTelAfiliadoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ValidacionTelAfiliados> CrearValTelAfiliado(ValidacionTelAfiliados ValTelAfiliado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.SOC_ID), ValTelAfiliado.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_CONF_VAL_INM), ValTelAfiliado.VAL_CONF_VAL_INM);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_ANIOS_FUERZA), ValTelAfiliado.VAL_ANIOS_FUERZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_LUGAR_TRABAJO), ValTelAfiliado.VAL_LUGAR_TRABAJO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_CARGO_ACTUAL), ValTelAfiliado.VAL_CARGO_ACTUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_VALOR_ARRIENDO), ValTelAfiliado.VAL_VALOR_ARRIENDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_VALOR_ACTIVOS), ValTelAfiliado.VAL_VALOR_ACTIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_VALOR_PASIVOS), ValTelAfiliado.VAL_VALOR_PASIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_MONTOS_RECURSOS_PROPIOS), ValTelAfiliado.VAL_MONTOS_RECURSOS_PROPIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.VAL_OBSERVACIONES), ValTelAfiliado.VAL_OBSERVACIONES);
            return await GetAsyncFirst<ValidacionTelAfiliados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ValidacionTelAfiliados> ObtenerValTelAfiliado(Guid Soc_Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumValidacionTelAfiliado.SOC_ID), Soc_Id);
            return await GetAsyncFirst<ValidacionTelAfiliados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}