using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data.Notification;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Repository de Notificacion
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>17/05/2021</date>
    /// </summary>
    public class NotificacionRepository : DapperGenericRepository, INotificacionRepository
    {
        public NotificacionRepository(IConfiguration configuration) : base(configuration)
        {
        }
        /// <summary>
        /// Metodo Obtener Notificacion
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>17/05/2021</date>
        /// <returns></returns>
        public async Task<IEnumerable<Notificacion>> ObtenerNotificaciones(Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.ID), userId);
            return await GetAsyncList<Notificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Metodo Crear Notificacion mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>17/05/2021</date>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        public async Task<Notificacion> CrearNotificacion(Notificacion notificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.ID), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.MENSAJE), notificacion.MOD_N_MENSAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.RECEPTOR), notificacion.MOD_N_RECEPTOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.TIPO), notificacion.MOD_N_TIPO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.VISTO), notificacion.MOD_N_VISTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.EMISOR), notificacion.MOD_N_EMISOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.FECHACREACION), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.ESTADO), notificacion.MOD_N_ESTADO);
            var resultado = await GetAsyncFirst<Notificacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return resultado;
        }
        /// <summary>
        /// Metodo Actualizar Notificacion mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>17/05/2021</date>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        public async Task<Notificacion> ActualizarNotificacion(Notificacion notificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.ID), notificacion.MOD_N_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.VISTO), notificacion.MOD_N_VISTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNotificacionParameters.ESTADO), notificacion.MOD_N_ESTADO);

            return await GetAsyncFirst<Notificacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
