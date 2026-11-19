using Dapper;
using GA2.Domain.Entities.GruposUsuarios;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.GrupoUsuarios;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class GruposUsuariosRepository : DapperGenericRepository, IGruposUsuariosRepository
    {


    
        public GruposUsuariosRepository(IConfiguration configuration) : base(configuration)
        {



        }

        public async Task<GrupoUsuario> CrearGrupoUsuario(GrupoUsuario grupo)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO),grupo.CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.NOMBRE), grupo.NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.DESCRIPCION), grupo.DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ESTADO), grupo.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.USR_CREADOR), grupo.USR_CREADOR);

            return await GetAsyncFirst<GrupoUsuario>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            
        }

        public async Task<UsuariosPorGrupos> InsertGruposPorUsuarios(Guid userId,int grupoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorUsuarios.USR_ID), userId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorUsuarios.GRP_ID), grupoId);

            return await GetAsyncFirst<UsuariosPorGrupos>("INSERT INTO SEG_USER_X_GRUPO(USR_ID,GRP_ID) VALUES (@USR_ID,@GRP_ID);", parameters, CommandType.Text);

        }

        public async Task<IEnumerable<GrupoUsuario>> ObtenerGruposUsuarios()
        {
            return await GetAsyncList<GrupoUsuario>(HelperDBParameters.BuilderFunction(
            HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);      
        }


        public async Task<GruposPorOpciones> InsertGrupoOpciones(int opcionId, int grupoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.OPC_ID), opcionId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), grupoId);

            var grupo = await GetAsyncFirst<GruposPorOpciones>("SELECT ID,OPC_ID, GRP_ID FROM SEG_GRUPO_X_OPCIONES WHERE OPC_ID=@OPC_ID AND GRP_ID =@GRP_ID ", parameters, CommandType.Text);

            if (grupo != null)
            {
                return grupo;

            }
            else
            {
                return await GetAsyncFirst<GruposPorOpciones>("INSERT INTO SEG_GRUPO_X_OPCIONES (OPC_ID,GRP_ID) VALUES (@OPC_ID,@GRP_ID);", parameters, CommandType.Text);

            }

        }


        /// <summary>
        /// Obtener usuario por id
        /// </summary>
        /// <param name="userId">usuario Id</param>
        /// <returns>Usurio por id</returns>
        /// <date>26/05/2021</date>
        /// <author>Oscar Julian Rojas</author>
        public async Task<GrupoUsuario> ObtenerGrupoUsuarioById(string codigo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), codigo);
          

            return await GetAsyncFirst<GrupoUsuario>("SELECT * FROM SEG_GRUPO_USUARIOS WHERE CODIGO=@CODIGO AND SEG_INHABILITADO = 0", parameters, CommandType.Text);
        }

        public async Task<GrupoUsuario> DeleteGrupoUsuarioById(int grupoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), grupoId);

            await GetAsyncFirst<GrupoUsuario>("UPDATE SEG_GRUPO_X_OPCIONES SET SEG_INHABILITADO = 1 WHERE GRP_ID=@ID", parameters, CommandType.Text);

            return await GetAsyncFirst<GrupoUsuario>("UPDATE SEG_GRUPO_USUARIOS SET SEG_INHABILITADO = 1 WHERE ID=@ID", parameters, CommandType.Text);

        }

        public async Task<GrupoUsuario> ActualizarGrupoUsuario(GrupoUsuario grupo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), grupo.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), grupo.CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.NOMBRE), grupo.NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.DESCRIPCION), grupo.DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ESTADO), grupo.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.USR_MODIFICADOR), grupo.USR_MODIFICADOR);


            return  await GetAsyncFirst<GrupoUsuario>("UPDATE SEG_GRUPO_USUARIOS SET CODIGO = @CODIGO, NOMBRE=@NOMBRE,DESCRIPCION = @DESCRIPCION, ESTADO = @ESTADO, USR_MODIFICADOR = @USR_MODIFICADOR WHERE ID = @ID", parameters, CommandType.Text);

        }
    }
}
