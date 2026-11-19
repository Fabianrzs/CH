using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.DatosAliadoEstrategico
{
    public class DatosAliadoEstrategicoRepository : DapperGenericRepository, IDatosAliadoEstrategicoRepository
    {
        public DatosAliadoEstrategicoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<DatosAliadoEstrategicos> ActualizarDatosAliadoEstrategico(DatosAliadoEstrategicos aliadoEstrategico)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_ID), aliadoEstrategico.DAT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.SOC_ID), aliadoEstrategico.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_NOMBRE_ALIADO), aliadoEstrategico.DAT_NOMBRE_ALIADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_FECHA_PAGO_ESTUDIO_TITULO), aliadoEstrategico.DAT_FECHA_PAGO_ESTUDIO_TITULO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_FECHA_RECEPCION_ESTUDIO_TITULO), aliadoEstrategico.DAT_FECHA_RECEPCION_ESTUDIO_TITULO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_CONCEPTO), aliadoEstrategico.DAT_CONCEPTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_TIPO_PARQUEADERO), aliadoEstrategico.DAT_TIPO_PARQUEADERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_EXTRAPRIMA), aliadoEstrategico.DAT_PORCENTAJE_EXTRAPRIMA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_DEAL_INSURANCE), aliadoEstrategico.DAT_DEAL_INSURANCE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_DEAL_INSURANCE_HOME), aliadoEstrategico.DAT_DEAL_INSURANCE_HOME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_IMC), aliadoEstrategico.DAT_IMC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_INSURANCE_ID), aliadoEstrategico.DAT_INSURANCE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_HOME_INSURANCE_ID), aliadoEstrategico.DAT_HOME_INSURANCE_ID);

            return await GetAsyncFirst<DatosAliadoEstrategicos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<DatosAliadoEstrategicos> CrearDatosAliadoEstrategico(DatosAliadoEstrategicos aliadoEstrategico)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_ID), aliadoEstrategico.DAT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.SOC_ID), aliadoEstrategico.SOC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_NOMBRE_ALIADO), aliadoEstrategico.DAT_NOMBRE_ALIADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_FECHA_PAGO_ESTUDIO_TITULO), aliadoEstrategico.DAT_FECHA_PAGO_ESTUDIO_TITULO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_FECHA_RECEPCION_ESTUDIO_TITULO), aliadoEstrategico.DAT_FECHA_RECEPCION_ESTUDIO_TITULO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_CONCEPTO), aliadoEstrategico.DAT_CONCEPTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_TIPO_PARQUEADERO), aliadoEstrategico.DAT_TIPO_PARQUEADERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_DEAL_INSURANCE), aliadoEstrategico.DAT_DEAL_INSURANCE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_DEAL_INSURANCE_HOME), aliadoEstrategico.DAT_DEAL_INSURANCE_HOME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_IMC), aliadoEstrategico.DAT_IMC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_INSURANCE_ID), aliadoEstrategico.DAT_INSURANCE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.DAT_HOME_INSURANCE_ID), aliadoEstrategico.DAT_HOME_INSURANCE_ID);

            return await GetAsyncFirst<DatosAliadoEstrategicos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<DatosAliadoEstrategicos> ObtenerDatosAliadoEstrategico(Guid aliadoEstrategico)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDatosAliadoEstrategicoParams.SOC_ID), aliadoEstrategico);
            return await GetAsyncFirst<DatosAliadoEstrategicos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}