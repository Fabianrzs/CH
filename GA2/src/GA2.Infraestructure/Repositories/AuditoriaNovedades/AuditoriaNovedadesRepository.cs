using Azure.Core;
using Dapper;
using GA2.Application.Dto.AuditoriaNovedades;
using GA2.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class AuditoriaNovedadesRepository : DapperGenericRepository, IAuditoriaNovedadesRepository
    {

        public AuditoriaNovedadesRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<AuditoriaNovedadesGet>> ConsultarAuditoriaNovedades(int idModulo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID_MODULO", idModulo);
            return await GetAsyncList<AuditoriaNovedadesGet>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AuditoriaNovedades> InsertarAuditoriaNovedades(AuditoriaNovedades request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AUN_ID_TIPO_IDENTIFICACION", request.TID_DESCRIPCION);
            parameters.Add("@AUN_IDENTIFICACION", request.AUN_IDENTIFICACION);
            parameters.Add("@AUN_MODIFICACION", request.AUN_MODIFICACION);
            parameters.Add("@FECHA_INICIO", request.FECHA_INICIO);
            parameters.Add("@FECHA_FIN", request.FECHA_FIN);
            parameters.Add("@AUN_FECHA_MODIFICACION", request.AUN_FECHA_MODIFICACION);
            parameters.Add("@AUN_USUARIO_MODIFICACION", request.AUN_USUARIO_MODIFICACION);            
            parameters.Add("@ID_MODULO", request.ID_MODULO);
            parameters.Add("@AUN_ACCION", request.AUN_ACCION);
            parameters.Add("@UNIDAD_EJECUTORA", request.UNIDAD_EJECUTORA);

            return await GetAsyncFirst<AuditoriaNovedades>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResponseValidaNovedadesFondoSolidaridadDto> ValidaNovedadesFondoSolidaridad(RequestValidaNovedadesFondoSolidaridadDto request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@TipoIdentificacion", request.TipoIdentificacion);
            parameters.Add("@NumeroIdentificacion", request.NumeroIdentificacion);
            parameters.Add("@DescripcionConceptoNovedad", request.DescripcionConceptoNovedad);
            parameters.Add("@IdConceptoNovedad", request.IdConceptoNovedad);
            parameters.Add("@FechaInicio", request.FechaInicio);
            parameters.Add("@FechaFin", request.FechaFin);

            return await GetAsyncFirst<ResponseValidaNovedadesFondoSolidaridadDto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
