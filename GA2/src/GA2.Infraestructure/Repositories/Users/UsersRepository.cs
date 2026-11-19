using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.GruposUsuarios;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.GrupoUsuarios;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Clase repositorio User
    /// <author>Oscar Julian Rojas Garces</author>
    /// <date>24/02/2021</date>  
    /// </summary>
    public class UsersRepository : DapperGenericRepository, IUsersRepository
    {
        /// <summary>
        /// Inyeccion de Configuracion
        /// </summary>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>24/02/2021</date>
        /// <param name="configuration"></param>
        public UsersRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user">Modelo user</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>24/02/2021</date>
        /// <returns>User Creado</returns>
        public async Task<User> CreateUser(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), Guid.NewGuid());
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_NOMBRE), user.USR_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_SEGUNDONOMBRE), user.USR_SEGUNDONOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_APELLIDO), user.USR_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_SEGUNDOAPELLIDO), user.USR_SEGUNDOAPELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_EMAIL), user.USR_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_USERNAME), user.USR_USERNAME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREDENTIAL), user.USR_PASSWORD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_IDENTIFICACION), user.USR_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TID_ID), user.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREADOPOR), user.USR_CREADOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.ACTIVE_DIRECTORY), user.ACTIVE_DIRECTORY);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ESTADO), user.USR_ESTADO);


            return await GetAsyncFirst<User>(UserQuery.QueryInsertUser, parameters, CommandType.Text);
        }

        /// <summary>
        /// Actualizar Usuario
        /// </summary>
        /// <param name="user">Modelo usuario</param>
        /// <author>Oscar Julian Rojas </author>
        /// <date>24/02/2021</date>
        /// <returns>Usuario Actualizado</returns>
        public async Task<User> UpdateUser(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), user.USR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_NOMBRE), user.USR_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_SEGUNDONOMBRE), user.USR_SEGUNDONOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_APELLIDO), user.USR_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_SEGUNDOAPELLIDO), user.USR_SEGUNDOAPELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_EMAIL), user.USR_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_USERNAME), user.USR_USERNAME);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_IDENTIFICACION), user.USR_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TID_ID), user.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ESTADO), user.USR_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.ACTIVE_DIRECTORY), user.ACTIVE_DIRECTORY);

            var result= await GetAsyncFirst<User>(UserQuery.QueryUpdateUser, parameters, CommandType.Text);

            return result;

        }

        /// <summary>
        /// Actualizar Usuario
        /// </summary>
        /// <param name="user">Modelo usuario</param>
        /// <author>Oscar Julian Rojas </author>
        /// <date>24/02/2021</date>
        /// <returns>Usuario Actualizado</returns>
        public async Task<User> ActualizarContrasenaUsuario(string token,string password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TOKEN), token);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREDENTIAL), password);
            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualizar Usuario
        /// </summary>
        /// <param name="user">Modelo usuario</param>
        /// <author>Oscar Julian Rojas </author>
        /// <date>24/02/2021</date>
        /// <returns>Usuario Actualizado</returns>
        public async Task<User> ActualizarContrasenaUsuarioById(Guid idUsuario, string password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), idUsuario);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_CREDENTIAL), password);
            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        /// <summary>
        /// Eliminar Usuario
        /// </summary>
        /// <param name="Id">Id de usuario modelo</param>
        /// <author>Oscar Julian Rojas </author>
        /// <date>24/02/2021</date>
        /// <returns>Usuario Eliminado</returns>
        public async Task<User> DeleteUser(Guid Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), Id);

           
            return await GetAsyncFirst<User>(UserQuery.QueryDelete, parameters, CommandType.Text);
        }

        /// <summary>
        /// Obtener Usuarios
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>24/02/2021</date>
        /// <returns>Lista de usuarios</returns>
        public async Task<IEnumerable<User>> ObtenerUsuariosDos()
        {
           var result=await GetAsyncList<User>(UserQuery.QueryGetAllUser, null, CommandType.Text);

            return result;
        }        

        /// <summary>
        /// Obtener usuario por id
        /// </summary>
        /// <param name="userId">usuario Id</param>
        /// <returns>Usurio por id</returns>
        /// <date>26/05/2021</date>
        /// <author>Oscar Julian Rojas</author>
        public async Task<User> ObtenerUsuarioPorId(Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), userId);

            return await GetAsyncFirst<User>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener usuario por id
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="tipoIdentificationId"></param>
        /// <date>26/05/2021</date>
        /// <author>Oscar Julian Rojas</author>
        /// <returns>Usurio por id</returns>
        public async Task<User> ObtenerUsuarioPorIdentificacion(string identificacion,int tipoIdentificationId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_IDENTIFICACION), identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.TID_ID), tipoIdentificationId);

            return await GetAsyncFirst<User>(UserQuery.QueryGetIdentiUser,parameters, CommandType.Text);
        }

        public async Task<UsuariosPorGrupos> InsertGruposPorUsuarios(Guid userId, int grupoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorUsuarios.USR_ID), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorUsuarios.GRP_ID), grupoId);

            var grupo= await GetAsyncFirst<UsuariosPorGrupos>("SELECT USR_ID,GRP_ID FROM SEG_USER_X_GRUPO WHERE   USR_ID=@USR_ID AND GRP_ID=@GRP_ID", parameters, CommandType.Text);

            if (grupo != null)
            {
                return grupo;
               
            }
            else {
                return await GetAsyncFirst<UsuariosPorGrupos>("INSERT INTO SEG_USER_X_GRUPO(USR_ID,GRP_ID) VALUES (@USR_ID,@GRP_ID);", parameters, CommandType.Text);

            }

        }


        
        public async Task<IEnumerable<GrupoUsuario>> GetGrupoUserById(Guid userId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorUsuarios.USR_ID), userId);

            return await GetAsyncList<GrupoUsuario>("SELECT DISTINCT GRP.* FROM SEG_USER_X_GRUPO UG INNER JOIN SEG_GRUPO_USUARIOS GRP ON GRP.ID=UG.GRP_ID WHERE UG.USR_ID=@USR_ID", parameters, CommandType.Text);
            
        }

        public async Task<GrupoUsuario> DeleteGrupoUsuarioUserById(Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUsersParameters.USR_ID), userId);
            return await GetAsyncFirst<GrupoUsuario>("DELETE FROM SEG_USER_X_GRUPO  WHERE USR_ID=@USR_ID", parameters, CommandType.Text);
           
        }
    }
}
