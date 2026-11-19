using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class OpcionesTemporalesRepository : DapperGenericRepository, IOpcionesTemporalesRepository
    {

        public OpcionesTemporalesRepository(IConfiguration configuration) : base(configuration) { }
        public async Task<OpcionTemporalUsuario> EditarOpcionesTemporalesUsuario(OpcionTemporalUsuario opcionTemporalUsuarioDto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_ID), opcionTemporalUsuarioDto.UOT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.OPC_ID), opcionTemporalUsuarioDto.OPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_FECHA_DESDE), opcionTemporalUsuarioDto.UOT_FECHA_DESDE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_FECHA_HASTA), opcionTemporalUsuarioDto.UOT_FECHA_HASTA);

            return await GetAsyncFirst<OpcionTemporalUsuario>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<OpcionTemporalUsuario> EliminarOpcionesTemporalesUsuario(int idOpcionTemporalUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_ID), idOpcionTemporalUsuario);

            return await GetAsyncFirst<OpcionTemporalUsuario>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<OpcionTemporalUsuario> InsertarOpcionesTemporalesUsuario(OpcionTemporalUsuario opcionTemporalUsuarioDto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.USR_ID), opcionTemporalUsuarioDto.USR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.OPC_ID), opcionTemporalUsuarioDto.OPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_FECHA_DESDE), opcionTemporalUsuarioDto.UOT_FECHA_DESDE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.UOT_FECHA_HASTA), opcionTemporalUsuarioDto.UOT_FECHA_HASTA);

            return await GetAsyncFirst<OpcionTemporalUsuario>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OpcionTemporalUsuario>> ObtenerOpcionesTemporalesUsuario(Guid idUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOpcionesTemporales.USR_ID), idUsuario);

            return await GetAsyncList<OpcionTemporalUsuario>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
