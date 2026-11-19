using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class CesantiasRepository : DapperGenericRepository, ICesantiasRepository
    {
        public CesantiasRepository(IConfiguration configuration) : base(configuration) { }


        /// <summary>
        /// Obtiene tramite solicitud
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<ObtenerTramiteCesantias> ObtenerTramiteCesantias(ParametrosObtenerCesantias solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCesantiasParameters.idCliente), solicitud.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCesantiasParameters.solicitudId), solicitud.SOL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCesantiasParameters.subModalidadId), solicitud.TPS_SUB_ID);



            ObtenerTramiteCesantias response = new ObtenerTramiteCesantias();
            response = await GetAsyncFirst<ObtenerTramiteCesantias>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldos()
        {
            return await GetAsyncList<ObtenerOrdenDescuentos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, typeCommand: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<EquivalenciaCuentasContablesSeven>> ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(Int32 codOperacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCreditoCarteraParams.codOperacion), codOperacion);

            return await GetAsyncList<EquivalenciaCuentasContablesSeven>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
        public async Task<obtenerTipoSolicitud> ObtenerTipoSolicitudPorSolIdySubModalidad(Guid Sol_Id, Guid Sub_Modalidad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TPS_SUB_ID", Sub_Modalidad);
            parameters.Add("@SOL_ID", Sol_Id);

            return await GetAsyncFirst<obtenerTipoSolicitud>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters, CommandType.StoredProcedure);
        }
    }
}
