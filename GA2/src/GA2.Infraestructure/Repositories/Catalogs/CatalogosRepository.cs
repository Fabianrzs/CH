using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Catalogs;
using GA2.Domain.Entities.Catalogs.ComprobantesCartera;
using GA2.Domain.Entities.InfoVendedor;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Catalog;
using GA2.Infraestructure.Data.InfoVendedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Azure.Core;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http;
using System.Net.Sockets;

namespace GA2.Infraestructure.Repositories
{
    public class CatalogosRepository : DapperGenericRepository, ICatalogosRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        public CatalogosRepository(IConfiguration configuration,IHttpContextAccessor httpContext) : base(configuration) 
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// Obtener Catalogos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Catalogo>> ObtenerCatalogos()
        {
            return await GetAsyncList<Catalogo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Valores Catalogos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CatalogoValor>> ObtenerValoresCatalogos()
        {
            return await GetAsyncList<CatalogoValor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CatalogoValor>> ObtenerValoresCatalogosPorIdCatalogo(int catalogoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Id), catalogoId);


            return await GetAsyncList<CatalogoValor>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CatalogoValor>> ObtenerValoresCatalogoPorNombre(String nombreCatalogo)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCatalogoParams.NombreCatalogo), nombreCatalogo);

            return await GetAsyncList<CatalogoValor>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerCategorias
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Categoria>> ObtenerCategorias()
        {
            return await GetAsyncList<Categoria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public string ObtenerIpLocal()
        {           
            {
                try
                {                    
                    string ip = RequestIp.GetRequestIP(true,_httpContext);
                    string hostName= RequestIp.GetRequestHost(_httpContext,ip);
                    return string.Format("{0}**{1}",ip,hostName);
                }
                catch (Exception ex)
                {
                    return (ex.Message);
                }
            }
        }
       
        /// <summary>
        /// Obtener Ciudades
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Ciudad>> ObtenerCiudades()
        {
            return await GetAsyncList<Ciudad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtener Ciudades Por Departamentos
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Ciudad>> ObtenerCiudadesPorDepartamento(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCiudadParameters.DPD_ID), Id);

            return await GetAsyncList<Ciudad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<Ciudad> ObtenerCiudadPorDepartamentoCiudad(int idDpto, int idCiudad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCiudadParameters.DPD_ID), idDpto);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCiudadParameters.DPC_CODIGO), idCiudad);

            return await GetAsyncFirst<Ciudad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<Departamento>ObteneDepartamentoPorCodigoCiudad(string nombreCiudad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCiudadParameters.DPC_NOMBRE), nombreCiudad);

            return await GetAsyncFirst<Departamento>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Obtener Paises
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Pais>> ObtenerPaises()
        {
            return await GetAsyncList<Pais>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtener Paises Por Id
        /// </summary>
        /// <returns></returns>
        public async Task<Pais> ObtenerPaisesPorId(int idPais)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumPaisParameters.DPP_ID), idPais);

            return await GetAsyncFirst<Pais>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Departamentos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Departamento>> ObtenerDepartamentos()
        {
            return await GetAsyncList<Departamento>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Unidades Ejecutoras
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UnidadEjecutora>> ObtenerUnidadesEjecutoras(Guid? IdUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.USR_ID), IdUsuario);
            return await GetAsyncList<UnidadEjecutora>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Fuerzas
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Fuerzas>> ObtenerFuerzas()
        {
            return await GetAsyncList<Fuerzas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoActor>> obtenerTipoActor()
        {
            return await GetAsyncList<TipoActor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TiposArchivos>> ObtenerTiposArchivos()
        {
            return await GetAsyncList<TiposArchivos>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Grados
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Grado>> ObtenerGrados()
        {
            return await GetAsyncList<Grado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Grados por Fuerza y Categoria
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Grado>> ObtenerGradosPorFuerzaCategoria(Grado grado)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGradoParameters.FRZ_ID), grado.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGradoParameters.CTG_ID), grado.CTG_ID);

            return await GetAsyncList<Grado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Grados por Fuerza y Categoria
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ConceptoNomina>> ObtenerConceptoNominaCat(ConceptoNomina concepto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(Data.EnumConceptoNomina.CTG_ID), concepto.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(Data.EnumConceptoNomina.CNN_SALARIO), concepto.CNN_SALARIO);

            return await GetAsyncList<ConceptoNomina>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerTiposIdentificacion
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoIdentificacion>> ObtenerTiposIdentificacion()
        {
            return await GetAsyncList<TipoIdentificacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtener tipo de sexo
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CatalogoValor>> ObtenerTipoDeSexo()
        {
            return await GetAsyncList<CatalogoValor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Estados Civiles
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CatalogoValor>> ObtenerEstadosCiviles()
        {
            return await GetAsyncList<CatalogoValor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Estado Cuentas Clientes
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EstadoCivil>> ObtenerEstadoCuentasCliente()
        {
            return await GetAsyncList<EstadoCivil>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actividad Econimica 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ActividadEconomica>> ObtenerActividadesEconomicas()
        {
            return await GetAsyncList<ActividadEconomica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Actividad Econimica Por ID
        /// </summary>
        /// <returns></returns>
        public async Task<ActividadEconomica> ObtenerActividadEconomica(int idActidadEconomica)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActidadesEconomicas.IdActidadEconomica), idActidadEconomica);

            return await GetAsyncFirst<ActividadEconomica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actividad Econimica 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Profesion>> ObtenerProfesiones()
        {
            return await GetAsyncList<Profesion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener direcciones abecedarios 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Abecedario>> ObtenerDireccionesAbecedario()
        {
            return await GetAsyncList<Abecedario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener direcciones cardinal 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Cardinalidad>> ObtenerDireccionesCardinal()
        {
            return await GetAsyncList<Cardinalidad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener direcciones bis
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Bis>> ObtenerDireccionesBis()
        {
            return await GetAsyncList<Bis>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtener direcciones tipos  
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoCalle>> ObtenerDireccionesTipos()
        {
            return await GetAsyncList<TipoCalle>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener tipos de Credito
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoCredito>> ObtenerTipoCredito()
        {
            return await GetAsyncList<TipoCredito>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener tipos de vivienda
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TiVivienda>> ObtenerTipoVivienda()
        {
            return await GetAsyncList<TiVivienda>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener estados de vivienda
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EstadoVivienda>> ObtenerEstadoVivienda()
        {
            return await GetAsyncList<EstadoVivienda>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Estado de Actividad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EstadoActividad>> ObtenerEstadoActividad()
        {
            return await GetAsyncList<EstadoActividad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Obtener Tipos de actividad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoCorreo>> ObtenerTiposCorreo()
        {
            return await GetAsyncList<TipoCorreo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Tipos de telefono
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoTelefono>> ObtenerTiposTelefono()
        {
            return await GetAsyncList<TipoTelefono>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        /// <summary>
        /// ObtenerTiposMoneda
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<TipoMoneda>> ObtenerTiposMoneda()
        {
            return await GetAsyncList<TipoMoneda>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// CrearTipoVivienda
        /// </summary>
        /// <param name="TipoVivienda"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<TipoVivienda> CrearTipoVivienda(TipoVivienda TipoVivienda)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_ID), TipoVivienda.TVV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_DESCRIPCION), TipoVivienda.TVV_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_PROMOCIONADA), TipoVivienda.TVV_PROMOCIONADA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_ACTIVA), TipoVivienda.TVV_ACTIVA);

            return await GetAsyncFirst<TipoVivienda>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarTipoVivienda
        /// </summary>
        /// <param name="TipoVivienda"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<TipoVivienda> ActualizarTipoVivienda(TipoVivienda TipoVivienda)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_ID), TipoVivienda.TVV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_DESCRIPCION), TipoVivienda.TVV_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_PROMOCIONADA), TipoVivienda.TVV_PROMOCIONADA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_ACTIVA), TipoVivienda.TVV_ACTIVA);

            return await GetAsyncFirst<TipoVivienda>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerTiposDeVivienda
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 
        public async Task<IEnumerable<TipoVivienda>> ObtenerTiposDeVivienda()
        {
            return await GetAsyncList<TipoVivienda>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarTipoActividadPorid
        /// </summary>
        /// <param name="TipoViviendaid"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        /// 

        /// <summary>
        /// CrearTipoCredito
        /// </summary>
        /// <param name="TipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<TipoVivienda> EliminarTipoViviendaPorid(string TipoViviendaid)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoViviendaParametros.TVV_ID), TipoViviendaid);

            return await GetAsyncFirst<TipoVivienda>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        public async Task<TipoCredito> CrearTipoCredito(TipoCredito TipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_DESCRIPCION), TipoCredito.TCR_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ID_PADRE), TipoCredito.TCR_ID_PADRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_NIVEL), TipoCredito.TCR_NIVEL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ESTADO), TipoCredito.TCR_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_CREADO_POR), TipoCredito.TCR_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_FECHA_CREACION), TipoCredito.TCR_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_MODIFICADO_POR), TipoCredito.TCR_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_FECHA_MODIFICACION), TipoCredito.TCR_FECHA_MODIFICACION);

            return await GetAsyncFirst<TipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<bool> RegistrarSelFuerzaUnidadUsuario(SLFZUN_FUERZA_UNIDAD_USUARIO FuerzaUnidadUsuario)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.FRZ_ID), FuerzaUnidadUsuario.FRZ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.UEJ_ID), FuerzaUnidadUsuario.UEJ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.USR_ID), FuerzaUnidadUsuario.USR_ID);

            await GetAsyncFirst<SLFZUN_FUERZA_UNIDAD_USUARIO>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> ActualizarSelFuerzaUnidadUsuario(SLFZUN_FUERZA_UNIDAD_USUARIO FuerzaUnidadUsuario)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.FRZ_ID), FuerzaUnidadUsuario.FRZ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.UEJ_ID), FuerzaUnidadUsuario.UEJ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.USR_ID), FuerzaUnidadUsuario.USR_ID);

            await GetAsyncFirst<SLFZUN_FUERZA_UNIDAD_USUARIO>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);

            return true;
        }

        public async Task<SLFZUN_FUERZA_UNIDAD_USUARIO> ObtenerSelFuerzaUnidadUsuario(string usuarioId)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.USR_ID), usuarioId);

            return await GetAsyncFirst<SLFZUN_FUERZA_UNIDAD_USUARIO>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarTipoCredito
        /// </summary>
        /// <param name="TipoCredito"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<TipoCredito> ActualizarTipoCredito(TipoCredito TipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ID), TipoCredito.TCR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_DESCRIPCION), TipoCredito.TCR_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ID_PADRE), TipoCredito.TCR_ID_PADRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_NIVEL), TipoCredito.TCR_NIVEL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ESTADO), TipoCredito.TCR_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_CREADO_POR), TipoCredito.TCR_CREADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_FECHA_CREACION), TipoCredito.TCR_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_MODIFICADO_POR), TipoCredito.TCR_MODIFICADO_POR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_FECHA_MODIFICACION), TipoCredito.TCR_FECHA_MODIFICACION);

            return await GetAsyncFirst<TipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// EliminarTipoActividadPorid
        /// </summary>
        /// <param name="TipoActividad"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<TipoCredito> EliminarTipoCreditoPorid(string TipoCreditoid)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoCreditoParametros.TCR_ID), TipoCreditoid);

            return await GetAsyncFirst<TipoCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// OBtener tipos de conceptos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Concepto>> ObtenerConceptos()
        {
            return await GetAsyncList<Concepto>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ClasificacionArchivo>> ObtenerClasificacionesArchivo()
        {
            return await GetAsyncList<ClasificacionArchivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoHomologado>> ObtenerConceptosHomologados()
        {
            return await GetAsyncList<ConceptoHomologado>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// OBtener dominios correo
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DominiosCorreo>> ObtenerDominiosCorreo()
        {
            return await GetAsyncList<DominiosCorreo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoDireccion>> ObtenerTiposDireccion()
        {
            return await GetAsyncList<TipoDireccion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los tipos de abonos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoAbono>> ObtenerTipoAbono()
        {
            return await GetAsyncList<TipoAbono>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los tipos de subsidio
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoSubsidio>> ObtenerTipoSubsidio()
        {
            return await GetAsyncList<TipoSubsidio>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AbonoUnico>> ObtenerAbonoUnico()
        {
            var response = await GetAsyncList<AbonoUnico>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }


        /// <summary>
        /// Obtiene los tipos de subsidio
        /// <paramref name="IdTipoActor">Filtro tipo actor</paramref>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoSolicitud>> ObtenerTipoSolicitud(int IdTipoActor)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoSolicitudParametros.TPA_ID), IdTipoActor);

            return await GetAsyncList<TipoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtiene los tipos de subsidio
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoModalidad>> ObtenerTipoModalidad(string IdTipoSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoModalidadParametros.TIS_ID), IdTipoSolicitud);

            return await GetAsyncList<TipoModalidad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Obtiene los tipos de subsidio
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoSubModalidad>> ObtenerTipoSubModalidad(string IdTipoModalidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoSubModalidadParametros.TIM_ID), IdTipoModalidad);

            return await GetAsyncList<TipoSubModalidad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadoCartera>> ObtenerEstadoCartera()
        {
            var response = await GetAsyncList<EstadoCartera>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<GA2.Domain.Entities.Catalogs.EstadoCuenta>> ObtenerEctEstadoCuenta()
        {
            var response = await GetAsyncList<GA2.Domain.Entities.Catalogs.EstadoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<CategoriaTablaParametro>> ObtenerCategoriasTablasParametros()
        {
            var response = await GetAsyncList<CategoriaTablaParametro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<dynamic>> ObtenerDatosTablasParametros(string nombreTabla)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatoTablaParametro.NombreTabla), nombreTabla);
            var response = await GetAsyncList<dynamic>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<EsquemaTablaParametro>> ObtenerEsquemasTablasParametros(string nombreTabla)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDatoTablaParametro.NombreTabla), nombreTabla);
            var response = await GetAsyncList<EsquemaTablaParametro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return response;
        }
        public async Task<Departamento> ObtenerDepartamentoPorId(int dpd)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDepartamentoParams.IdDepartamento), dpd);

            var response = await GetAsyncFirst<Departamento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return response;
        }
        public async Task<Ciudad> ObtenerCiudadPorId(int dpc)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCiudadParams.IdCiudad), dpc);

            var response = await GetAsyncFirst<Ciudad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return response;
        }
        public async Task<Ciudad> ObtenerCiudadPorIdDPCyIdDPD(int dpc, int dpd)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCiudadParams.IdCiudad), dpc);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDepartamentoParams.IdDepartamento), dpd);
            var response = await GetAsyncFirst<Ciudad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<TipoCuenta>> ObtenerTipoCuenta()
        {
            var response = await GetAsyncList<TipoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<EntidadBancaria>> ObtenerEntidadBancaria()
        {
            var response = await GetAsyncList<EntidadBancaria>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<Formato>> ObtenerFormato()
        {
            var response = await GetAsyncList<Formato>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<MedioDeEnvio>> ObtenerMediosDeEnvio()
        {
            var response = await GetAsyncList<MedioDeEnvio>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        /// <summary>
        /// Obtiene las Entidades Educativas
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativa()
        {
            return await GetAsyncList<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene las Entidades Educativas por NIT y Rzon Social
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativa entidad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_TIPO_IDENTIFICACION), entidad.ENE_TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NIT), entidad.ENE_NIT);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_NOMBRE_RAZON_SOCIAL), entidad.ENE_NOMBRE_RAZON_SOCIAL);

            return await GetAsyncList<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ProgramaEducativo>> ObtenerProgramaEducativo(Guid idEntidad)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.ENE_ID), idEntidad);

            return await GetAsyncList<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ProgramaEducativo> CrearProgramaEducativo(ProgramaEducativo programaEducativo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_DESCRIPCION), programaEducativo.PGE_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ESTADO), programaEducativo.PGE_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_CREATEDOPOR), programaEducativo.PGE_CREATEDOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_FECHACREACION), programaEducativo.PGE_FECHACREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_MODIFICADOPOR), programaEducativo.PGE_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_FECHAMODIFICACION), programaEducativo.PGE_FECHAMODIFICACION);


            return await GetAsyncFirst<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        [Obsolete("ObtenerNivelEducativoCesantias is deprecated, please use ObtenerNivelEducativoPorPrograma instead.")]
        public async Task<IEnumerable<NivelEducativoObsolete>> ObtenerNivelEducativoCesantias(Guid idPrograma)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumNivelEducativoParametros.PGN_ID), idPrograma);

            return await GetAsyncList<NivelEducativoObsolete>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NivelEducativo>> ObtenerNivelEducativo()
        {
            DynamicParameters parametros = new DynamicParameters();
            return await GetAsyncList<NivelEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NivelEducativoTemp>> ObtenerNivelEducativoCatalogoValor()
        {
            DynamicParameters parametros = new DynamicParameters();
            return await GetAsyncList<NivelEducativoTemp>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EntidadSeguroEducativo>> ObtenerEntidadSeguroEducativo()
        {
            var response = await GetAsyncList<EntidadSeguroEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<EntidadSeguroEducativo> InsertarEntidadSeguroEducativo(EntidadSeguroEducativo request)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.TipoIdentificacion), request.TID_ID_FK);

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.NumeroIdentificacion), request.ESE_NUMERO_IDENTIFICACION);

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.Nit), request.ESE_NIT?.Trim());

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.Razon), request.ESE_NOMBRE_RAZON_SOCIAL);

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.CreadoPor), request.ESE_CREATEDOPOR);


            var response = await GetAsyncFirst<EntidadSeguroEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }

        /// <summary>
        /// Obtiene tipos de documento
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoIdentificacion>> ObtenerDocumentoTipo()
        {
            return await GetAsyncList<TipoIdentificacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<TipoIdentificacion> CrearDocumentoTipo(TipoIdentificacion data)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Abreviatura), data.TID_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Descripcion), data.TID_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Empresarial), data.TID_EMPRESARIAL);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Activo), data.TID_ACTIVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.ERP), data.TID_ERP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Auditoria), data.TID_VIGIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Embargo), data.TID_EMBARGO);

            return await GetAsyncFirst<TipoIdentificacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualizar Documentos Tipo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TipoIdentificacion> ActualizarDocumentoTipo(TipoIdentificacion data)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.IdTipoDocumento), data.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Abreviatura), data.TID_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Descripcion), data.TID_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Empresarial), data.TID_EMPRESARIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Activo), data.TID_ACTIVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.ERP), data.TID_ERP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Auditoria), data.TID_VIGIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDocumentoTipoParams.Embargo), data.TID_EMBARGO);

            return await GetAsyncFirst<TipoIdentificacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea Parametro Fuerza
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Fuerzas> CrearFuerza(Fuerzas data)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Codigo), data.FRZ_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Descripcion), data.FRZ_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Soldado), data.FRZ_SOLDADO);

            return await GetAsyncFirst<Fuerzas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Fuerzas> ActualizarFuerza(Fuerzas data)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Id), data.FRZ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Codigo), data.FRZ_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Digito), data.FRZ_DIGITO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Descripcion), data.FRZ_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Soldado), data.FRZ_SOLDADO);

            return await GetAsyncFirst<Fuerzas>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CatalogoValor> CrearSexo(CatalogoValor data)
        {
            DynamicParameters parametros = new DynamicParameters();

            //parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.IdCatalogo), data.CVL_ID);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Id), data.CAT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Codigo), data.CVL_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Descripcion), data.CVL_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Estado), data.CVL_ACTIVO);

            return await GetAsyncFirst<CatalogoValor>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<CatalogoValor> ActualizarSexo(CatalogoValor data)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.IdCatalogo), data.CVL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Id), data.CAT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Codigo), data.CVL_CODIGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Descripcion), data.CVL_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.Estado), data.CVL_ACTIVO);

            return await GetAsyncFirst<CatalogoValor>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Crea una nueva Entidad Educativa
        /// </summary>
        /// <param name="entidadEducativa"></param>
        /// <returns></returns>
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
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEntidadEducativaParameters.ENE_ESTADO), entidadEducativa.ENE_ESTADO);

            return await GetAsyncFirst<EntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
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

        public async Task<IEnumerable<SedeEntidadEducativa>> ObtenerSedesPorEntidadEducativa(string entidadEducativaId)
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
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_ESTADO), sedeEntidadEducativa.SNE_SEDE_ESTADO);

            return await GetAsyncFirst<SedeEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<SedeEntidadEducativa> EliminarSedeEntidadEducativaPorId(string sedeEntidadEducativaId)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSedeEntidadEducativaParameters.SNE_SEDE_ID), sedeEntidadEducativaId);

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
        public async Task<ProgramaEducativo> EliminarProgramaEducativoPorId(string idProgramaEducativo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ID), idProgramaEducativo);

            return await GetAsyncFirst<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<ProgramaEducativo> ActualizarProgramaEducativo(ProgramaEducativo programaEducativo)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_ID), programaEducativo.PGE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_DESCRIPCION), programaEducativo.PGE_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_MODIFICADOPOR), programaEducativo.PGE_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumProgramaEducativoParametros.PGE_FECHAMODIFICACION), programaEducativo.PGE_FECHAMODIFICACION);

            return await GetAsyncFirst<ProgramaEducativo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<BloqueoEntidadEducativa> BloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), bloqueoEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_TIPO_OPERACION), bloqueoEntidadEducativa.ENE_TIPO_OPERACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_CAUSAL_BLOQUEO), bloqueoEntidadEducativa.ENE_CAUSAL_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FECHA_BLOQUEO), bloqueoEntidadEducativa.ENE_FECHA_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FUNCIONARIO_BLOQUEO), bloqueoEntidadEducativa.ENE_FUNCIONARIO_BLOQUEO);

            return await GetAsyncFirst<BloqueoEntidadEducativa>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<BloqueoEntidadEducativa> DesbloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), bloqueoEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_TIPO_OPERACION), bloqueoEntidadEducativa.ENE_TIPO_OPERACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_CAUSAL_BLOQUEO), bloqueoEntidadEducativa.ENE_CAUSAL_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FECHA_BLOQUEO), bloqueoEntidadEducativa.ENE_FECHA_BLOQUEO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_FUNCIONARIO_BLOQUEO), bloqueoEntidadEducativa.ENE_FUNCIONARIO_BLOQUEO);

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
        public async Task<IEnumerable<CuentaBancariaEntidadEducativa>> ObtenerCuentaBancariaPorEntidad(Guid idEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumBloqueoEntidadEducativaParameters.ENE_ID), idEntidadEducativa);

            return await GetAsyncList<CuentaBancariaEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<CuentaBancariaEntidadEducativa> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_BANCARIA), cuentaBancariaEntidadEducativa.ENE_CTA_BANCARIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ID), cuentaBancariaEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ENT_ID), cuentaBancariaEntidadEducativa.ENE_ENT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_NUMERO_CTA), cuentaBancariaEntidadEducativa.ENE_NUMERO_CTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_ESTADO), cuentaBancariaEntidadEducativa.ENE_CTA_ESTADO);

            return await GetAsyncFirst<CuentaBancariaEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<CuentaBancariaEntidadEducativa> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_CTA_BANCARIA), cuentaBancariaEntidadEducativa.ENE_CTA_BANCARIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ID), cuentaBancariaEntidadEducativa.ENE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCuentaBancariaEntidadParametros.ENE_ENT_ID), cuentaBancariaEntidadEducativa.ENE_ENT_ID);
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

        public async Task<Categoria> CrearCategoria(Categoria data)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCategoriaParams.CTG_DESCRIPTION), data.CTG_DESCRIPCION);

            return await GetAsyncFirst<Categoria>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<Categoria> ActualizarCategoria(Categoria data)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCategoriaParams.CTG_ID), data.CTG_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCategoriaParams.CTG_DESCRIPTION), data.CTG_DESCRIPCION);

            return await GetAsyncFirst<Categoria>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoAfiliacion>> ObtenerTiposAfiliacion()
        {
            return await GetAsyncList<TipoAfiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadoAfiliacion>> ObtenerEstadosAfiliacion()
        {
            return await GetAsyncList<EstadoAfiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CampoCalidad>> ObtenerCampoCalidadAfiliacion()
        {
            return await GetAsyncList<CampoCalidad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoAfiliacionProcedente>> ObtenerTiposAfiliacionProcedente()
        {
            return await GetAsyncList<TipoAfiliacionProcedente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PorcentajeDescuento>> ObtenerPorcentajesDescuento()
        {
            return await GetAsyncList<PorcentajeDescuento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<PorcentajeDescuento> InsertarPorcentajeDescuento(PorcentajeDescuento porcentajeDescuento)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.PRC_PRODUCTO), porcentajeDescuento.PRC_PRODUCTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.PRC_DESCRIPCION), porcentajeDescuento.PRC_DESCRIPCION);

            return await GetAsyncFirst<PorcentajeDescuento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoCuentaC>> ObtenerTipoCuentaC()
        {
            return await GetAsyncList<TipoCuentaC>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PersonasCargo>> ObtenerPersonasCargo()
        {
            return await GetAsyncList<PersonasCargo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoContrato>> ObtenerTipoContrato()
        {
            return await GetAsyncList<TipoContrato>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoViviendaPropia>> ObtenerTipoViviendaPropia()
        {
            return await GetAsyncList<TipoViviendaPropia>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Estrato>> ObtenerEstrato()
        {
            return await GetAsyncList<Estrato>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GrupoInconsistencia>> ListarGruposInconsistencia()
        {
            return await GetAsyncList<GrupoInconsistencia>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoInconsistencia>> ObtenerInconsistencia(int GrupoInconsistencia)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumTipoInconsistenciaParametros.GIN_GRUPO), GrupoInconsistencia);

            return await GetAsyncList<TipoInconsistencia>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<HerramientaInconsistencia>> ObtenerHerramientaInconsistencia()
        {
            return await GetAsyncList<HerramientaInconsistencia>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PuntosAtencionInconsistencia>> ObtenerPuntosAtencionInconsistencia()
        {
            return await GetAsyncList<PuntosAtencionInconsistencia>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NivelAcademico>> ObtenerNivelAcademico()
        {
            return await GetAsyncList<NivelAcademico>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoNomina>> ObtenerConceptoNomina()
        {
            return await GetAsyncList<ConceptoNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<bool> InsertarConceptoNomina(ConceptoNomina conceptoNomina)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_DESCRIPCION), conceptoNomina.CNN_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_SIGLA), conceptoNomina.CNN_SIGLA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_ESTADO), conceptoNomina.CNN_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_CREADO_POR), conceptoNomina.CNN_CREADO_POR);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoNomina>> ActualizarConceptoNomina(ConceptoNomina conceptoNomina)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_ID), conceptoNomina.CNN_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_DESCRIPCION), conceptoNomina.CNN_DESCRIPCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_SIGLA), conceptoNomina.CNN_SIGLA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_ESTADO), conceptoNomina.CNN_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConceptoNominaCredito.CNN_MODIFICADO_POR), conceptoNomina.CNN_MODIFICADO_POR);

            return await GetAsyncList<ConceptoNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalBloqueo>> ObtenerCausalBloqueo()
        {
            return await GetAsyncList<CausalBloqueo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoSolicitudNovedadMatricula>> ObtenerTipoSolicitudNovedadMatricula()
        {
            return await GetAsyncList<TipoSolicitudNovedadMatricula>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener Estados de las tareas de una Solicitud
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EstadoTareaSolicitud>> ObtenerEstadoTareaSolicitud()
        {
            return await GetAsyncList<EstadoTareaSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OficinaDeRegistro>> ObtenerOficinasDeRegistro()
        {
            return await GetAsyncList<OficinaDeRegistro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<CargoCategoria> ObtenerSimulacionCargoPorCategoria(int categoriaId, int Cargo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumCargoCategoriaParams.IdCategoria), categoriaId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCargoCategoriaParams.IdCargo), Cargo);

            return await GetAsyncFirst<CargoCategoria>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NivelAcademicoCatalogo>> ObtenerNivelEducativoPostulante()
        {
            return await GetAsyncList<NivelAcademicoCatalogo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Ocupacion>> ObtenerOcupacionPostulante()
        {
            return await GetAsyncList<Ocupacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ModalidadAfiliacion>> ObtenerModalidadAfiliacion()
        {
            return await GetAsyncList<ModalidadAfiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene los estados de los documentos
        /// </summary>
        /// <returns>Lista de estados documentos</returns>
        public async Task<IEnumerable<ESD_DOCUMENTO>> ObtenerEstadosDocumento()
        {
            return await GetAsyncList<ESD_DOCUMENTO>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoSolicitud>> ObtenerTipoSolicitudDesafiliacion()
        {
            return await GetAsyncList<TipoSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FormatoAplicar>> ObtenerFormatoAAplicar()
        {
            return await GetAsyncList<FormatoAplicar>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalesBloqueoCuenta>> ObtenerCausalesBloqueoCuenta()
        {
            var a = await GetAsyncList<CausalesBloqueoCuenta>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return a;
        }

        public async Task<IEnumerable<ConceptoNovedad>> ObtenerConceptosNovedades()
        {
            return await GetAsyncList<ConceptoNovedad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarTiposIdentificacion(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumTipoDocumentoParametros.TDC_ID), Id);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarTipoDeSexo(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCatalogoValorParams.IdCatalogo), Id);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarFuerzas(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumFuerzaParams.Id), Id);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarCategorias(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCategoriaParams.CTG_ID), Id);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TipoActorEmbargo>> ObtenerTipoActorEmbargo()
        {
            return await GetAsyncList<TipoActorEmbargo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoActorEmbargo>> EliminarTipoActorEmbargo(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCategoriaParams.TPR_ID), Id);

            return await GetAsyncList<TipoActorEmbargo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoUnidadEjecutoraReporte>> ObtenerUnidadEjecutoraReporte()
        {
            return await GetAsyncList<TipoUnidadEjecutoraReporte>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoReporteGA2>> ObtenerTipoReporteGA2()
        {
            return await GetAsyncList<TipoReporteGA2>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<LineaCredito>> ObtenerLineasCredito(int IdTipoCredito)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumLineaCreditoParametros.TCR_ID), IdTipoCredito);

            return await GetAsyncList<LineaCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MediosRecepcion>> ObtenerMediosRecepcion()
        {
            return await GetAsyncList<MediosRecepcion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<MedioAtencionCredito> ConsultarMediosAtencionPorId(Guid IdMedioAtencion)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@MCV_ID", IdMedioAtencion);
            return await GetAsyncFirst<MedioAtencionCredito>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadoTareaSolicitud>> ObtenerParametrizacionHangFire()
        {
            return await GetAsyncList<EstadoTareaSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<int> InsertarLogHangFire(string mensaje, string request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("HL_MENSAJE", mensaje);
            parametros.Add("HL_REQUEST_FAIL", request);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UnidadEjecutora>> ObtenerUnidadesEjecutorasPorUsuario(Guid IdUsuario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumRegistrarSelFuerzaUnidadUsuario.USR_ID), IdUsuario);
            return await GetAsyncList<UnidadEjecutora>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UnidadEjecutora>> ObtenerUnidadesEjecutoras()
        {
            return await GetAsyncList<UnidadEjecutora>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoNomina>> ObtenerConceptoNominaParam()
        {
            return await GetAsyncList<ConceptoNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        #region Comprobante, Concepto, Beneficiario Cartera
        public async Task<IEnumerable<TipoBeneficiario>> ObtenerBeneficiarioCartera()
        {
            return await GetAsyncList<TipoBeneficiario>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoConcepto>> ObtenerConceptosCartera()
        {
            return await GetAsyncList<TipoConcepto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoComprobante>> ObtenerTipoComprobanteCartera()
        {
            return await GetAsyncList<TipoComprobante>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

      
        #endregion
    }
    public static class RequestIp
    {
        public static string GetRequestIP(bool tryUseXForwardHeader = true, IHttpContextAccessor _httpContextAccessor=null)
        {
            string ip = null;
           
            if (tryUseXForwardHeader)
                ip =GetHeaderValueAs<string>("X-Forwarded-For", _httpContextAccessor).SplitCsv().FirstOrDefault();

            
            if (ip.IsNullOrWhitespace() && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR", _httpContextAccessor);           

            if (ip.IsNullOrWhitespace())
                ip="No se ha podido determinar la IP remota.";

            return ip;
        }

        public static string GetRequestHost(IHttpContextAccessor _httpContextAccessor = null,string ip=null)
        {
            string host = null;
            
            if (host.IsNullOrWhitespace())
                host = GetHeaderValueAs<string>("REMOTE_HOST", _httpContextAccessor);
            if(ip==null)
            {
                return "No se ha podido determinar la IP remota";
            }
            if (host.IsNullOrWhitespace())
            {
                try
                {
                    IPHostEntry hostInfo = new IPHostEntry();
                    hostInfo = Dns.GetHostEntry(ip);
                    host = hostInfo.HostName;
                }
                catch (SocketException ex)
                {
                    host = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                catch (Exception ex)
                {
                    host = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }

            }
            return host;
        }
       

    public static T GetHeaderValueAs<T>(string headerName, IHttpContextAccessor _httpContextAccessor)
        {
            StringValues values="";

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        
    }
}
