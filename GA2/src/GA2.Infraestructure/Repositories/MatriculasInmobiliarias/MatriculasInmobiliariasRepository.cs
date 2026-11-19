using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.MatriculasInmobilarias;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class MatriculasInmobiliariasRepository : DapperGenericRepository, IMatriculasInmobiliariasRepository
    {
        public MatriculasInmobiliariasRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<PropietariosMatriculasInmobiliarias> AgregarHistorialDePropietario(PropietariosMatriculasInmobiliarias request, Guid user )
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_NUMERO_IDENTIFICACION), request.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_NOMBRE_RAZON_SOCIAL), request.PRP_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_PRIMER_NOMBRE), request.PRP_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_SEGUNDO_NOMBRE), request.PRP_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_PRIMER_APELLIDO), request.PRP_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_SEGUNDO_APELLIDO), request.PRP_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.TID_ID), request.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.CORREO), request.CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.TELEFONO), request.TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.MAI_ID), request.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.DATE_CREATION), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.USERID),user);

            return await GetAsyncFirst<PropietariosMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<CrearMatriculaResponse> CrearMatriculaInmobiliaria(CrearMatricula crearMatricula)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_NUMERO_MATRICULA), crearMatricula.MAI_NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_DIRECCION), crearMatricula.MAI_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.DPD_DEPARTAMENTO), crearMatricula.DPD_DEPARTAMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.DPC_NOMBRE_CIUDAD), crearMatricula.DPC_NOMBRE_CIUDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_TIPO_IDENTIFICACION), crearMatricula.PRP_TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_NUMERO_IDENTIFICACION), crearMatricula.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_NOMBRE_RAZON_SOCIAL), crearMatricula.PRP_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_PRIMER_NOMBRE), crearMatricula.PRP_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_SEGUNDO_NOMBRE), crearMatricula.PRP_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_PRIMER_APELLIDO), crearMatricula.PRP_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_SEGUNDO_APELLIDO), crearMatricula.PRP_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.CORREO), crearMatricula.CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.TELEFONO), crearMatricula.TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.OFC_ID), crearMatricula.OFC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_FECHA_REGISTRO), DateTime.Now);

            return await GetAsyncFirst<CrearMatriculaResponse>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<PropietariosMatriculasInmobiliarias> EditarHistorialPropietarios(PropietariosMatriculasInmobiliarias request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_ID), request.PRP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.MAI_ID), request.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_NOMBRE_RAZON_SOCIAL), request.PRP_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.TID_ID), request.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_NUMERO_IDENTIFICACION), request.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.TELEFONO), request.TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.CORREO), request.CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_PRIMER_NOMBRE), request.PRP_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_SEGUNDO_NOMBRE), request.PRP_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_PRIMER_APELLIDO), request.PRP_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_SEGUNDO_APELLIDO), request.PRP_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.NOMBRE_USUARIO), request.NOMBRE_USUARIO);

            return await GetAsyncFirst<PropietariosMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<MatriculaInmibiliaria> EditarInformacionBasicaMatricula(MatriculaInmibiliaria request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_NUMERO_MATRICULA), request.MAI_NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_FECHA_REGISTRO), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_DIRECCION), request.MAI_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.DPD_DEPARTAMENTO), request.DPD_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.DPC_NOMBRE_CIUDAD), request.DPC_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.OFC_ID), request.OFC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), request.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.NOMBRE_USUARIO), request.NOMBRE_USUARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.NUMERO_DOCUMENTO), request.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.ID_USUARIO), request.ID_USUARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.CORREO), request.MAI_CORREO_ELECTRONICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.TELEFONO), request.MAI_CELULAR);

            return await GetAsyncFirst<MatriculaInmibiliaria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<PropietariosMatriculasInmobiliarias> EliminarHistorialDePropietarios(string request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_ID), request);

            return await GetAsyncFirst<PropietariosMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<MatriculaInmibiliaria> EliminarMatriculaInmobiliaria(Guid idMatricula)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), idMatricula);

            return await GetAsyncFirst<MatriculaInmibiliaria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        } 

        public async Task<IEnumerable<MatriculaInmibiliaria>> FiltroMatriculaInmobiliaria(string Numero_matricula, string numero_identifiucacion, int tipo_documento, DateTime? fecha_creacion, DateTime? fecha_final, int bloqueo, string nom_oficina, string cliId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_NUMERO_MATRICULA), Numero_matricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.PRP_NUMERO_IDENTIFICACION), numero_identifiucacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.TID_ID), tipo_documento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_FECHA_REGISTRO), fecha_creacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_FECHA_FINAL), fecha_final);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_BLOQUEO), bloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.OFC_DESCRIPCION), nom_oficina);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.CLI_ID), cliId);

            return await GetAsyncList<MatriculaInmibiliaria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>> HistorialNovedadesMatriculaInmobiliariaPorId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), id);
            return await GetAsyncList<HistotialNovedadesMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PropietariosMatriculasInmobiliarias>> HistorialPropietariosMatriculaInmobiliariaPorId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), id);
            return await GetAsyncList<PropietariosMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<TramitesMatriculasInmobiliarias>> ConsultarMatriculasSolicitudes(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), id);
            return await GetAsyncList<TramitesMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<HistorialCambiosMatriculas>> ConsultarMatriculasCambios(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), id);
            return await GetAsyncList<HistorialCambiosMatriculas>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<MatriculaInmibiliaria> InformacionBasicaMatriculaInmobiliariaPorId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFiltroMatriculasInmobiliarias.MAI_ID), id);
            return await GetAsyncFirst<MatriculaInmibiliaria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>> InsertarNovedadMatriculaInmobiliaria(HistotialNovedadesMatriculasInmobiliarias novedad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.MAI_ID), novedad.ID_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.TOP_ID), novedad.ID_TIPO_OPERACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.CAN_ID), novedad.CAUSAL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.TSN_ID), novedad.TIPO_SOLICITUD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.NVM_OBSERVACIONES), novedad.OBSERVACIONES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.NVM_CREATEDOPOR), novedad.CREATEDOPOR_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.NOMBRE_USUARIO), novedad.NOMBRE_USUARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.DATE_REGISTRO), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.DATE_NOVEDADES), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.ID_USUARIO), novedad.ID_USUARIO);
            return await GetAsyncList<HistotialNovedadesMatriculasInmobiliarias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalNovedamatricula>> listarCausalNovedaMatriculaPorId(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNovedadMatriculasInmobiliarias.TOP_ID), id);
            return await GetAsyncList<CausalNovedamatricula>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OperacionesMatriculas>> listarOperacionesMatriculas()
        {
            return await GetAsyncList<OperacionesMatriculas>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalMatricula>> ConsultarCausalesMatriculaInmobiliaria()
        {
            return await GetAsyncList<CausalMatricula>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<int> CrearMatriculaPropietario(CrearMatriculaRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.MAI_ID), request.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.PRP_ID), request.PRP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumHistorialPropietarios.DATE_CREATION), request.MAI_CREATEDOPOR);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

    }
}
