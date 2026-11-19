using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Login G2 Users
    /// </summary>
    public class LoginGA2Repository : DapperGenericRepository, ILoginGA2Repository
    {
        /// <summary>
        /// Constructor Login GA2 https://chordify.net/
        /// </summary>
        /// <param name="configuration">Configuracion aplicacion</param>
        /// <author>Oscar  Julian Rojas</author>
        /// <date>25/03/2021</date>
        public LoginGA2Repository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Login User 
        /// </summary>
        /// <param name="user">User submite login</param>
        /// <returns>User searched </returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>25/03/2021</date>
        public async Task<User> AccesoUsuario(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_EMAIL), user.USR_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_USERNAME), user.USR_USERNAME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREDENTIAL), user.USR_PASSWORD);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction( HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Login User 
        /// </summary>
        /// <param name="user">User submite login</param>
        /// <returns>User searched </returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>25/03/2021</date>
        public async Task<User> LoginUserUEJ(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_EMAIL), user.USR_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_USERNAME), user.USR_USERNAME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREDENTIAL), user.USR_PASSWORD);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<User> InicioSesionParaUsuario(Cliente cliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), cliente.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), cliente.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_EXPEDICION), cliente.CLI_FECHA_EXPEDICION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), cliente.DTL_TELEFONO);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Menu>> ObtenerMenu()
        {
            return await GetAsyncList<Menu>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SubMenu>> ObtenerSubmenu()
        {
            return await GetAsyncList<SubMenu>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        public async Task<User> RecuperaContrasena(string tipo,string numero)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TID_ID), tipo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_IDENTIFICACION), numero);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<User> ActualizarTokenUsuario(Guid idUser, string token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUser);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TOKEN), token);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<User> ValidaTokenRestablecimiento(string token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TOKEN), token);
            return await GetAsyncFirst<User>("SELECT * FROM USR_USERS WHERE TOKEN=@TOKEN", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<GrupoOpciones>> ObtenerOpcionesUsuariosPorId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), id);
            return await GetAsyncList<GrupoOpciones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<LoginSession> CheckSessionLogin(Guid idUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUsuario);
            return await GetAsyncFirst<LoginSession>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task CreateSessionLogin(Guid idUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUsuario);
            await GetAsyncList<LoginSession>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateSession(Guid idUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUsuario);
            var active = await GetAsyncFirst<LoginSession>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return active.LSS_ACTIVA;
        }

        public async Task Logout(Guid idUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUsuario);
            await GetAsyncList<LoginSession>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
