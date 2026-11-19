using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Entidades;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class EntidadesRepository : DapperGenericRepository, IEntidadesRepository
    {
        public EntidadesRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativa()
        {
            return await GetAsyncList<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativaFideicomiso()
        {
            return await GetAsyncList<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativa entidad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), entidad.ENE_TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NIT), entidad.ENE_NIT);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_RAZON_SOCIAL), entidad.ENE_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.RENOVAR_ARCHIVO), entidad.RENOVAR_ARCHIVO);

            return await GetAsyncList<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<int> ObtenerEntidadEducativaPorNit(string entidad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NIT), entidad);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ProgramaEducativo>> ObtenerProgramaEducativo(Guid idEntidad)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.ENE_ID), idEntidad);

            return await GetAsyncList<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<ListarProgramaEducativo> CrearProgramaEducativo(ProgramaEducativo programaEducativo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.ENE_ID), programaEducativo.PGE_ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_NVE_ID), programaEducativo.PGE_NVE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_CTL_ID), programaEducativo.PGE_CTL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_DESCRIPCION), programaEducativo.PGE_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_CREATEDOPOR), programaEducativo.PGE_CREATEDOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_FECHACREACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ESTADO), programaEducativo.PGE_ESTADO);
            ListarProgramaEducativo s = await GetAsyncFirst<ListarProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return s;
        }
        public async Task<IEnumerable<NivelEducativo>> ObtenerNivelEducativoCesantias(Guid idPrograma)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumNivelEducativoParametros.PGN_ID), idPrograma);

            return await GetAsyncList<NivelEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<EntidadSeguroEducativo>> ObtenerEntidadSeguroEducativo()
        {
            var response = await GetAsyncList<EntidadSeguroEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<EntidadEducativa> CrearEntidadEducativa(EntidadEducativa entidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), entidadEducativa.ENE_TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NIT), entidadEducativa.ENE_NIT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_RAZON_SOCIAL), entidadEducativa.ENE_NOMBRE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CORREO_ELECTRONICO), entidadEducativa.ENE_CORREO_ELECTRONICO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_DPC_ID), entidadEducativa.ENE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_DIRECCION), entidadEducativa.ENE_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TELEFONO), entidadEducativa.ENE_TELEFONO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_CONTACTO), entidadEducativa.ENE_NOMBRE_CONTACTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CVL_ACREDITADA_ID), entidadEducativa.ENE_CVL_ACREDITADA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CVL_TIPO_PERSONA_ID), entidadEducativa.ENE_CVL_TIPO_PERSONA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_ESTADO), entidadEducativa.ENE_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CREADOPOR), entidadEducativa.ENE_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FECHA_CREACION), DateTime.Now);

            return await GetAsyncFirst<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<Fideicomiso> ConsultarFideicomisosEntidad(Fideicomiso fidecomiso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FIDEICOMISO_ID), fidecomiso.ENE_FIDEICOMISO_ID);


            return await GetAsyncFirst<Fideicomiso>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Fideicomiso>> InsertarFideicomisoEntidad(Fideicomiso fidecomiso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_ID), fidecomiso.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), fidecomiso.ENE_TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NUMERO_IDENTIFICACION), fidecomiso.ENE_NUMERO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_RAZON_SOCIAL_FIDEICOMISO), fidecomiso.ENE_RAZON_SOCIAL_FIDEICOMISO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FEC_CRE),DateTime.Now);


            var response = await GetAsyncList<Fideicomiso>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<Fideicomiso>> ActualizarFideicomisoEntidadEducativa(Fideicomiso fidecomiso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FIDEICOMISO_ID), fidecomiso.ENE_FIDEICOMISO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), fidecomiso.ENE_TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NUMERO_IDENTIFICACION), fidecomiso.ENE_NUMERO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_RAZON_SOCIAL_FIDEICOMISO), fidecomiso.ENE_RAZON_SOCIAL_FIDEICOMISO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENEC_USER), fidecomiso.ENEC_USER);


            var response = await GetAsyncList<Fideicomiso>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<Fideicomiso>> EliminarFideicomiso(IdentificadoresEntidadEducativa fidecomiso)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FIDEICOMISO_ID), fidecomiso.idCuentaBancaria);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_MODIFICADOPOR), fidecomiso.ModificadoPor);
            var response = await GetAsyncList<Fideicomiso>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<EntidadEducativa> ActualizarEntidadEducativa(EntidadEducativa entidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_ID), entidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), entidadEducativa.ENE_TIPO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NIT), entidadEducativa.ENE_NIT);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_RAZON_SOCIAL), entidadEducativa.ENE_NOMBRE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CORREO_ELECTRONICO), entidadEducativa.ENE_CORREO_ELECTRONICO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_DPC_ID), entidadEducativa.ENE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_DIRECCION), entidadEducativa.ENE_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TELEFONO), entidadEducativa.ENE_TELEFONO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_CONTACTO), entidadEducativa.ENE_NOMBRE_CONTACTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CVL_ACREDITADA_ID), entidadEducativa.ENE_CVL_ACREDITADA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_CVL_TIPO_PERSONA_ID), entidadEducativa.ENE_CVL_TIPO_PERSONA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_MODIFICADOPOR), entidadEducativa.ENE_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<EntidadEducativa> EliminarEntidadEducativaPorId(string EntidadEducativaId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_ID), EntidadEducativaId);

            return await GetAsyncFirst<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SedeEntidadEducativa>> ObtenerSedesPorEntidadEducativa(Guid entidadEducativaId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_ID), entidadEducativaId);

            return await GetAsyncList<SedeEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SedeEntidadEducativa> CrearSedeEntidadEducativa(SedeEntidadEducativa sedeEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_ID), sedeEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_NOMBRE_RAZON_SOCIAL), sedeEntidadEducativa.SNE_SEDE_NOMBRE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_DPC_ID), sedeEntidadEducativa.SNE_SEDE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_CREADOPOR), sedeEntidadEducativa.SNE_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_FECHA_CREACION), DateTime.Now);

            return await GetAsyncFirst<SedeEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<crearAnotacion> CrearAnotacion(crearAnotacion crearAnotacion)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_ID_FK), crearAnotacion.ENE_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_ANOTACION), crearAnotacion.ENE_ANOTACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_CREADOPOR), crearAnotacion.ENE_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_FECHA_REGISTRO), crearAnotacion.ENE_FECHA_REGISTRO);

            return await GetAsyncFirst<crearAnotacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<RespuestaAnotacion>> ConsultarAnotacion(Guid entidadId)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.ENE_ID_FK), entidadId);
            return await GetAsyncList<RespuestaAnotacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<SedeEntidadEducativa> ActualizarSedeEntidadEducativa(SedeEntidadEducativa sedeEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_ID), sedeEntidadEducativa.SNE_SEDE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_NOMBRE_RAZON_SOCIAL), sedeEntidadEducativa.SNE_SEDE_NOMBRE_RAZON_SOCIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_DPC_ID), sedeEntidadEducativa.SNE_SEDE_DPC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_ESTADO), sedeEntidadEducativa.SNE_SEDE_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_MODIFICADOPOR), sedeEntidadEducativa.SNE_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_FECHA_MODIFICACION), DateTime.Now);

            return await GetAsyncFirst<SedeEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<SedeEntidadEducativa> EliminarSedeEntidadEducativaPorId(IdentificadoresEntidadEducativa entidad)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_ID), entidad.idSede);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_MODIFICADOPOR), entidad.ModificadoPor);

            return await GetAsyncFirst<SedeEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ListarProgramaEducativo>> ObtenerProgramaEducativoPorEntidad(Guid idEntidad)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.ENE_ID), idEntidad);

            return await GetAsyncList<ListarProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ProgramaEducativo> EliminarProgramaEducativoPorId(IdentificadoresEntidadEducativa entidadEducativa)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ID), entidadEducativa.idProgramaEducativo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_MODIFICADOPOR), entidadEducativa.ModificadoPor);

            return await GetAsyncFirst<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ProgramaEducativo> ActualizarProgramaEducativo(ProgramaEducativo programaEducativo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ID), programaEducativo.PGE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_DESCRIPCION), programaEducativo.PGE_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_MODIFICADOPOR), programaEducativo.PGE_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_FECHAMODIFICACION), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ESTADO), programaEducativo.PGE_ESTADO);

            return await GetAsyncFirst<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BloqueoEntidadEducativa> BloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), bloqueoEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_CAUSAL_BLOQUEO), bloqueoEntidadEducativa.ENE_CAUSAL_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FECHA_BLOQUEO), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FUNCIONARIO_BLOQUEO), bloqueoEntidadEducativa.ENE_FUNCIONARIO_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_OBSERVACIONES), bloqueoEntidadEducativa.ENE_OBSERVACIONES);

            return await GetAsyncFirst<BloqueoEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BloqueoEntidadEducativa> DesbloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), bloqueoEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_CAUSAL_BLOQUEO), bloqueoEntidadEducativa.ENE_CAUSAL_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FECHA_BLOQUEO), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FUNCIONARIO_BLOQUEO), bloqueoEntidadEducativa.ENE_FUNCIONARIO_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_OBSERVACIONES), bloqueoEntidadEducativa.ENE_OBSERVACIONES);

            return await GetAsyncFirst<BloqueoEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BloqueoEntidadEducativa>> ObtenerBloqueoEntidadEducativa(string idEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), idEntidadEducativa);

            return await GetAsyncList<BloqueoEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Fideicomiso>> ObtenerCuentaBancariaPorEntidad(string idEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), idEntidadEducativa);

            return await GetAsyncList<Fideicomiso>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CuentaBancariaEntidadEducativa> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ID), cuentaBancariaEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ENT_ID), cuentaBancariaEntidadEducativa.ENE_ENT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CVL_ID), cuentaBancariaEntidadEducativa.ENE_CVL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_NUMERO_CTA), cuentaBancariaEntidadEducativa.ENE_NUMERO_CTA);

            return await GetAsyncFirst<CuentaBancariaEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CuentaBancariaEntidadEducativa> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_BANCARIA), cuentaBancariaEntidadEducativa.ENE_CTA_BANCARIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ENT_ID), cuentaBancariaEntidadEducativa.ENE_ENT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CVL_ID), cuentaBancariaEntidadEducativa.ENE_CVL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_NUMERO_CTA), cuentaBancariaEntidadEducativa.ENE_NUMERO_CTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_ESTADO), cuentaBancariaEntidadEducativa.ENE_CTA_ESTADO);

            return await GetAsyncFirst<CuentaBancariaEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CuentaBancariaEntidadEducativa> EliminarCuentaBancariaEntidadEducativa(Guid idCuenta)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_BANCARIA), idCuenta);

            return await GetAsyncFirst<CuentaBancariaEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<InsertarArchivoEntidad> InsertarArchivoPorEntidad(InsertarArchivoEntidad insertarArchivoEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.Entidad), insertarArchivoEntidad.ENE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.Parametrizacion), insertarArchivoEntidad.ENE_PAM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.NombreArchivo), insertarArchivoEntidad.ENE_NOMBRE_ARCHIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.RutaArchivo), insertarArchivoEntidad.ENE_RUTA_STORAGE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.ExtensionArchivo), insertarArchivoEntidad.ENE_EXTENSION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.NumeroFolio), insertarArchivoEntidad.ENE_PAM_NUMERO_FOLIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.CreadoPor), insertarArchivoEntidad.ENE_PAM_CREATEDOPOR);

            return await GetAsyncFirst<InsertarArchivoEntidad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConsultarArchivosPorEntidad>> ConsultarArchivosPorEntidad(Guid IdEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.Entidad), IdEntidad);

            IEnumerable<ConsultarArchivosPorEntidad> response;
            response = await GetAsyncList<ConsultarArchivosPorEntidad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<EntidadEducativa> ObtenerEntidadEducativaDetalle(Guid IdEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.Entidad), IdEntidad);

            return await GetAsyncFirst<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarArchivoEntidad(EliminarArchivoEntidad eliminarArchivoEntidad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoEntidadParameters.ArchivoEntidadId), eliminarArchivoEntidad.ENE_ID);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<ConsultarArchivosPorId> ConsultarArchivosEntidadPorId(Guid IdEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ENE_PAM_ID", IdEntidad);

            return await GetAsyncFirst<ConsultarArchivosPorId>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TramiteEntidades>> DataTramitesEntidades(Guid idEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ENE_ID", idEntidad);
            return await GetAsyncList<TramiteEntidades>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<int> EliminarSolicitudes(string identificacion)
        {
            DynamicParameters parameters = new();
            parameters.Add("@ENE_ID", identificacion);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure); 
        }
        public async Task<IEnumerable<HistorialCambios>> ObtenerHistorialCambios(Guid idEntidad)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ENE_ID", idEntidad);
            return await GetAsyncList<HistorialCambios>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<HistorialCambiosInterviniente>> ObtenerHistorialCambiosIntervinientes(Guid idIntervinientes)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ITRC_ID", idIntervinientes);
            return await GetAsyncList<HistorialCambiosInterviniente>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<int> CrearHistorialEntidad(HistorialCambios request)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ENE_ID", request.ENE_ID);
            parameters.Add("@ENEC_PESTANA", request.ENEC_PESTANA);
            parameters.Add("@ENEC_CAMPO", request.ENEC_CAMPO);
            parameters.Add("@ENEC_VLR_ANTERIOR", request.ENEC_VLR_ANTERIOR);
            parameters.Add("@ENEC_VLR_NUEVO", request.ENEC_VLR_NUEVO);
            parameters.Add("@USERID", request.ENEC_USER);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<int> CrearNovedadesEntidadesEducativas(NovedadesEntidadEducativaRequest request)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ENV_OBSERVACION", request.Observacion);
            parameters.Add("@ENV_FECHA_CREACION", request.FechaCreacion);
            parameters.Add("@ENV_CODIGO_USUARIO", request.CodigoUsuario);
            parameters.Add("@ENV_ID_ENTIDAD", request.IdEntidad);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
