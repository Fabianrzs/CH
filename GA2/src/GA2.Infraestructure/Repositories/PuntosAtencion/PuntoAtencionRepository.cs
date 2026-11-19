using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.PuntosAtencion;
using GA2.Infraestructure.Data.Escrituras;
using GA2.Infraestructure.Data.PuntosAtencion;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.PuntosAtencion
{
    public class PuntoAtencionRepository : DapperGenericRepository, IPuntoAtencionRepository
    {
        public PuntoAtencionRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<PuntoAtencion> ActualizarPuntosAtencion(PuntoAtencion PuntoAtencionRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearPuntosAtencion.PA_ID), PuntoAtencionRequest.PA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearPuntosAtencion.PA_PUNTO), PuntoAtencionRequest.PA_PUNTO);
            return await GetAsyncFirst<PuntoAtencion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<PuntoAtencion> CrearPuntosAtencion(PuntoAtencion PuntoAtencionRequest)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearPuntosAtencion.PA_PUNTO), PuntoAtencionRequest.PA_PUNTO);
            return await GetAsyncFirst<PuntoAtencion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PuntoAtencion>> ObtenerPuntosAtencion()
        {
            return await GetAsyncList<PuntoAtencion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<PuntoAtencion> ObtenerPuntosAtencionPorId(int IdPuntoAtencion)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCrearPuntosAtencion.PA_ID), IdPuntoAtencion);
            return await GetAsyncFirst<PuntoAtencion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}
