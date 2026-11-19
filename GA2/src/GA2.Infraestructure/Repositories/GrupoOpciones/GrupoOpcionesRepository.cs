using Dapper;
using GA2.Application.Dto.GrupoUsuarios;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data.GrupoUsuarios;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class GrupoOpcionesRepository : DapperGenericRepository, IGrupoOpcionesRepository
    {
        public GrupoOpcionesRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<GrupoOpciones> ActualizaGrupoOpciones(GrupoOpciones grupo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), grupo.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), grupo.CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.NOMBRE), grupo.NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.DESCRIPCION), grupo.DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ESTADO), grupo.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.USR_MODIFICADOR), grupo.USR_MODIFICADOR);

            return await GetAsyncFirst<GrupoOpciones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<GrupoOpciones> CrearGrupoOpciones(GrupoOpciones grupo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), grupo.CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.NOMBRE), grupo.NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.DESCRIPCION), grupo.DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ESTADO), grupo.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.USR_CREADOR), grupo.USR_CREADOR);

            return await GetAsyncFirst<GrupoOpciones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<GrupoOpciones> EliminarGruposOpciones(int id)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), id);

            return await GetAsyncFirst<GrupoOpciones>("UPDATE SEG_GRUPO_OPCIONES SET SEG_INHABILITADO = 1 WHERE ID=@ID", parameters, CommandType.Text);
        
        }

        public async Task<GrupoOpciones> EliminarGruposOpcionesGrupoById(int grupoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), grupoId);

            return await GetAsyncFirst<GrupoOpciones>("DELETE FROM SEG_GRUPO_X_OPCIONES  WHERE GRP_ID=@ID", parameters, CommandType.Text);
        }

        public async Task<GrupoOpciones> InsertGruposOpcionesGrupoById(int grupoId,int opId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.OPC_ID), opId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), grupoId);

            return await GetAsyncFirst<GrupoOpciones>("INSERT INTO SEG_GRUPO_X_OPCIONES (OPC_ID,GRP_ID) VALUES(@OPC_ID,@GRP_ID)", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<GrupoOpciones>> GetGrupoOpcionesGrupoById(int grupoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), grupoId);

            return await GetAsyncList<GrupoOpciones>("SELECT GOP.* FROM SEG_GRUPO_X_OPCIONES GXP INNER JOIN SEG_GRUPO_OPCIONES GOP ON GOP.ID=GXP.OPC_ID WHERE GXP.GRP_ID=@GRP_ID", parameters, CommandType.Text);

        }

        public async Task<IEnumerable<GrupoOpciones>> ObtenerGrupoOpciones()
        {
           return await GetAsyncList<GrupoOpciones>(HelperDBParameters.BuilderFunction(
           HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<GrupoOpciones> ObtenerGrupoOpcionesById(string codigo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), codigo);


            return await GetAsyncFirst<GrupoOpciones>("SELECT * FROM SEG_GRUPO_OPCIONES WHERE CODIGO=@CODIGO AND SEG_INHABILITADO = 0", parameters, CommandType.Text);
            
        }

        public async Task<IEnumerable<GrupoOpciones>> GetOpcionesGrupoById(int grupoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), grupoId);

            return await GetAsyncList<GrupoOpciones>("SELECT OP.* FROM OPC_SEG_OPCIONES OP INNER JOIN SEG_GRUPOOPCIONES_OPC_OPCIONES GRO ON GRO.OPC_ID = OP.ID  WHERE GRO.GRP_ID = @GRP_ID", parameters, CommandType.Text);

        }

        public async Task<IEnumerable<GrupoOpciones>> GetAllOpciones()
        {
            return await GetAsyncList<GrupoOpciones>("SELECT * FROM OPC_SEG_OPCIONES", null, CommandType.Text);
        }

        public async Task<GrupoOpciones> EliminarOpcionesGrupoById(int grupoId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ID), grupoId);

            return await GetAsyncFirst<GrupoOpciones>("DELETE FROM SEG_GRUPOOPCIONES_OPC_OPCIONES WHERE GRP_ID=@ID DELETE FROM SEG_GRUPOOPCIONES_SUBOPCIONES WHERE GRP_ID=@ID", parameters, CommandType.Text);
        }


        public async Task<GrupoOpciones> InsertOpcionesGrupoById(int grupoId, int opId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.OPC_ID), opId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), grupoId);

            return await GetAsyncFirst<GrupoOpciones>("INSERT INTO SEG_GRUPOOPCIONES_OPC_OPCIONES (OPC_ID,GRP_ID) VALUES(@OPC_ID,@GRP_ID)", parameters, CommandType.Text);
        }

        public async Task<GrupoOpciones> CrearOpciones(GrupoOpciones Opcion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.CODIGO), Opcion.CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.NOMBRE), Opcion.NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.DESCRIPCION), Opcion.DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.ESTADO), Opcion.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposUsuarios.USR_CREADOR), Opcion.USR_CREADOR);

            return await GetAsyncFirst<GrupoOpciones>("insert into opc_seg_opciones (CODIGO,NOMBRE,DESCRIPCION,ESTADO, USR_CREADOR) values (@CODIGO,@NOMBRE,@DESCRIPCION,1,@USR_CREADOR)", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<SubOpciones>> ObtenerSubOpcinesPorIdOpcion(int idOpcion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSubOpcionesPorOpciones.IdOpcion), idOpcion);

            return await GetAsyncList<SubOpciones>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SubOpciones> InsertSubOpciones(int idOpcion, int idGrupo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.OPC_ID), idOpcion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), idGrupo);

            return await GetAsyncFirst<SubOpciones>("INSERT INTO SEG_GRUPOOPCIONES_SUBOPCIONES (SOP_ID,GRP_ID) VALUES(@OPC_ID,@GRP_ID)", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<SubOpciones>> ObtenerSubOpcionesByGrupoId(int idGrupo, int idOpcion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.GRP_ID), idGrupo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGruposPorOpciones.OPC_ID), idOpcion);

            return await GetAsyncList<SubOpciones>("SELECT* FROM SEG_GRUPOOPCIONES_SUBOPCIONES A INNER JOIN OPC_SEG_SUB_OPCIONES B ON A.SOP_ID = B.SOP_ID INNER JOIN OPC_SEG_OPCIONES C ON B.OPC_SEG_OPCIONES_ID = C.ID WHERE A.GRP_ID = @GRP_ID AND C.ID = @OPC_ID", parameters, CommandType.Text);
        }
        public async Task<IEnumerable<ResponseGruposUsuarioPorId>> ObtenerGruposUsuarioPorId(Guid userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@USR_ID", userId);

            return await GetAsyncList<ResponseGruposUsuarioPorId>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
