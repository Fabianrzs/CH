using Dapper;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.ValoresCategoria;
using GA2.Infraestructure.Data.Escrituras;
using GA2.Infraestructure.Data.ValoresCategoria;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ValoresCategoria
{
    public class ValoresCategoriaRepository : DapperGenericRepository, IValoresCategoriaRepository
    {
        public ValoresCategoriaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ValoresCategorias> ActualizarValoresCategoria(ValoresCategorias ValoresCatRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRG_ID), ValoresCatRequest.CRG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CTG_ID), ValoresCatRequest.CTG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRG_FACTOR_SUBS), ValoresCatRequest.CRG_FACTOR_SUBS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRG_ANIOS_ASCENSO), ValoresCatRequest.CRG_ANIOS_ASCENSO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_SALARIO_BASICO), ValoresCatRequest.CRA_SALARIO_BASICO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_SALARIO_TOTAL), ValoresCatRequest.CRA_SALARIO_TOTAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_ANIO), ValoresCatRequest.CRA_ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRG_PROYECTA_SM), ValoresCatRequest.CRG_PROYECTA_SM);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRG_PROYECTA_SB), ValoresCatRequest.CRG_PROYECTA_SUBSIDIO);
            return await GetAsyncFirst<ValoresCategorias>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ValoresCategorias>> CrearValoresCategoria(CrearValoresDto ValoresRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_ANIO), ValoresRequest.Cra_Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_PORC_AUM), ValoresRequest.Porc_Aumento);
            return await GetAsyncList<ValoresCategorias>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ValoresCategorias>> ObtenerValoresCategoria(int Anio)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumValoresCategoria.CRA_ANIO), Anio);
            return await GetAsyncList<ValoresCategorias>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}
