using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Proyecto;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Proyecto;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Proyectos
{
    public class ProyectoRepository : DapperGenericRepository, IProyectoRepository
    {
        public ProyectoRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<int> CREATE_BENEFICIARIO_PROYECTOS(BeneficiarioProyectoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_TIPO_DOCUMENTO), model.BP_TIPO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_NUMERO_DOCUMENTO), model.BP_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_DIGITO_VEFICACION), model.BP_DIGITO_VEFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_RAZON_SOCIAL), model.BP_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_TIPO_CUENTA), model.BP_TIPO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_TIPO_ENTIDAD), model.BP_TIPO_ENTIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_NUMERO_CUENTA), model.BP_NUMERO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_ACTIVIDAD_CUENTA), model.BP_ACTIVIDAD_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.BP_DIRECCION), model.BP_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLBeneficiarioProyecto.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CREATE_SUPERVISOR_PROYECTOS(SupervisorProyecoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_TIPO_DOCUMENTO), model.SP_TIPO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_NUMERO_DOCUMENTO), model.SP_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_NOMBRE_APELLIDO), model.SP_NOMBRE_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_CARGO), model.SP_CARGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_EMAIL), model.SP_EMAIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.SP_TELEFONO), model.SP_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLSupervisorProyecto.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> LICENCIA_CONSTRUCCION_PROYECTO(LicenciaUrbanismoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_RESOLUCION), model.LUP_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_CURADURIA), model.LUP_CURADURIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DEPARTAMENTO_ID), model.LUP_DEPARTAMENTO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_MUNICIPIO_ID), model.LUP_MUNICIPIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DATE_VIGENCIA), model.LUP_DATE_VIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_TITULAR), model.LUP_TITULAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DATE_RESOLUCION), model.LUP_DATE_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_MISMA_LICENCI), model.LUP_MISMA_LICENCI);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> CREAR_LICENCIA_URBANISMO_PROYECTO(LicenciaUrbanismoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_RESOLUCION), model.LUP_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_CURADURIA), model.LUP_CURADURIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DEPARTAMENTO_ID), model.LUP_DEPARTAMENTO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_MUNICIPIO_ID), model.LUP_MUNICIPIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DATE_VIGENCIA), model.LUP_DATE_VIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_TITULAR), model.LUP_TITULAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_DATE_RESOLUCION), model.LUP_DATE_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.LUP_MISMA_LICENCI), model.LUP_MISMA_LICENCI);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsLicenciaUrbanismoConstruccion.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<Guid> CREAR_PROYECTO_CONSTRUCTORA(ProyectoConstructoraModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_CONTRUCTORA_ID), model.PC_CONTRUCTORA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_NOMBRE), model.PC_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_DIRECCION), model.PC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_DEPARTAMENTO_ID), model.PC_DEPARTAMENTO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_MUNICIPIO_ID), model.PC_MUNICIPIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_TELEFONO), model.PC_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_VALOR), model.PC_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_CUPOS), model.PC_CUPOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_LATITUD), model.PC_LATITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_LONGITUD), model.PC_LONGITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_ESTADO_ID), model.PC_ESTADO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_CREACION), model.PC_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_VIGENCIA), model.PC_FECHA_VIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_ACTA_INICIO), model.PC_FECHA_ACTA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_ENTREGA), model.PC_FECHA_ENTREGA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_OBJETO), model.PC_OBJETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_TIPO_CONTRATO_ID), model.PC_TIPO_CONTRATO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_NUMERO_REGISTRO), model.PC_NUMERO_REGISTRO);
            parameters.Add("@PC_TIPO_CONTRATO", model.PC_TIPO_PROYECTO);

            var response = await GetAsyncFirst<Guid>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<ProyectoConstructoraResponse>> ObtenerProyecto(RequestConstructoraProyecto request)
        {
            DynamicParameters parameters = new DynamicParameters();


            parameters.Add("@IdEstado", request.IdEstado);
            parameters.Add("@IdCiudad", request.IdCiudad);
            parameters.Add("@IdDepartamento", request.IdDepartamento);

            if(request.ProyectoId is not null)
            {
                var id = Guid.Parse(request.ProyectoId);
                parameters.Add("@ProyectoId", id);
            }
            else
            {
                parameters.Add("@ProyectoId", null);

            }
            var response = await GetAsyncList<ProyectoConstructoraResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<int> CREATE_PP_POLIZA_PROYECTO(PolizaProyectoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_CONCEPTOPOLIZA_ID), model.PP_CONCEPTOPOLIZA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_RADICADOPOLIZA), model.PP_RADICADOPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_FECHAVIGENCIA), model.PP_FECHAVIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_FECHAINICIO), model.PP_FECHAINICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_NUMEROPOLIZA), model.PP_NUMEROPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_ASEGURADORA), model.PP_ASEGURADORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_VALORPOLIZA), model.PP_VALORPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_PORCENTAJE), model.PP_PORCENTAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_AJUSTADOR), model.PP_AJUSTADOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_DESCRIPCION), model.PP_DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<LicenciaUrbanismoResponse> ObtenerLicenciaConduccionByProyectoId(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncFirst<LicenciaUrbanismoResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<LicenciaUrbanismoResponse> ObtenerLicenciaUrbanimosByProyectoId(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncFirst<LicenciaUrbanismoResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<ProyectoConstructoraResponse> ObtenerProyectoById(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncFirst<ProyectoConstructoraResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<List<SupervisorResponse>> ObtenerSupervisorByProyectoId(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncList<SupervisorResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<List<BeneficiarioResponse>> ObtenerBeneficiarioByProyectoId(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncList<BeneficiarioResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }

        public async Task<List<PolizaProyectoResponse>> ObtenerPolizaByProyectoId(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), proyectoId);

            var response = await GetAsyncList<PolizaProyectoResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<int> ActualizarPolizaProyecto(PolizaProyectoModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_CONCEPTOPOLIZA_ID), model.PP_CONCEPTOPOLIZA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_RADICADOPOLIZA), model.PP_RADICADOPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_FECHAVIGENCIA), model.PP_FECHAVIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_FECHAVIGENCIA), model.PP_FECHAVIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_NUMEROPOLIZA), model.PP_NUMEROPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_ASEGURADORA), model.PP_ASEGURADORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_VALORPOLIZA), model.PP_VALORPOLIZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_PORCENTAJE), model.PP_PORCENTAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_AJUSTADOR), model.PP_AJUSTADOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PP_DESCRIPCION), model.PP_DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> ActualizarProyectoConstructora(ProyectoConstructoraModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_CONTRUCTORA_ID), model.PC_CONTRUCTORA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_NOMBRE), model.PC_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_DIRECCION), model.PC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_DEPARTAMENTO_ID), model.PC_DEPARTAMENTO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_MUNICIPIO_ID), model.PC_MUNICIPIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_TELEFONO), model.PC_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_VALOR), model.PC_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_CUPOS), model.PC_CUPOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_LATITUD), model.PC_LATITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_LONGITUD), model.PC_LONGITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_ESTADO_ID), model.PC_ESTADO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_CREACION), model.PC_FECHA_CREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_VIGENCIA), model.PC_FECHA_VIGENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_ACTA_INICIO), model.PC_FECHA_ACTA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_FECHA_ENTREGA), model.PC_FECHA_ENTREGA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_OBJETO), model.PC_OBJETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_TIPO_CONTRATO_ID), model.PC_TIPO_CONTRATO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateProyectoConstruccion.PC_NUMERO_REGISTRO), model.PC_NUMERO_REGISTRO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPolizaProyecto.PC_PROYECTO_ID), model.PC_PROYECTO_ID);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<TipoIncmuebles>> ObtenerTipoInmuble()
        {

            var response = await GetAsyncList<TipoIncmuebles>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<List<TipoIncmuebles>> ObtenerTipoViviendaProyecto(int codigo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Codigo", codigo);

            var response = await GetAsyncList<TipoIncmuebles>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<int> CrearTipoInmuebleProyecto(InmublesProyecto inmuebles)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CodigoTipoInmueble", inmuebles.CodigoTipoInmueble);
            parameters.Add("@Valor", inmuebles.Valor);
            parameters.Add("@AreaPrivada", inmuebles.AreaPrivada);
            parameters.Add("@AreaConstruida", inmuebles.AreaConstruida);
            parameters.Add("@CodigoTipoVivienda", inmuebles.CodigoTipoVivienda);
            parameters.Add("@CodigoClaseVivienda", inmuebles.CodigoClaseVivienda);
            parameters.Add("@Cantidad", inmuebles.Cantidad);
            parameters.Add("@Descripcion", inmuebles.Descripcion);
            parameters.Add("@ProyectoId", inmuebles.ProyectoId);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> GuardaInformacionSupervisorTecnico(SupervicionTecnica request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@STP_ID_PROYECTO", request.IdProyecto);
            parameters.Add("@STP_PERSONA_ATENDIO_VISITA", request.PersonaAtendioVisita);
            parameters.Add("@STP_CARGO", request.Cargo);
            parameters.Add("@STP_SUPERVISOR_TECNICO_TIPO_DOCUMENTO", request.SupervisoTecnicoTipoDocumento);
            parameters.Add("@STP_NOMBRE_SUPERVISOR_TECNICO", request.NombreSupervisorTecnico);
            parameters.Add("@STP_FECHA_VISITA", request.FechaVisita);
            parameters.Add("@STP_DESARROLLO_PROYECTO", request.DesarrolloProyecto);
            parameters.Add("@STP_OBSERVACION_GENERALES", request.ObservacionGeneral);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<IEnumerable<SupervicionTecnica>> ObtenerGuardaInformacionSupervisorTecnico(Guid request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@STP_ID_PROYECTO", request);

            var response = await GetAsyncList<SupervicionTecnica>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearClaseInmueble(ClaseInmueble clases)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", clases.ProyectoId);
            parameters.Add("@ClaseVivienda", clases.ClaseVivienda);
            parameters.Add("@NumeroVivienda", clases.NumeroVivienda);
            parameters.Add("@DireccionInmueble", clases.DireccionInmueble);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearListaAvance(ListaAvance clases)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", clases.ProyectoId);
            parameters.Add("@ItemAvance", clases.ItemAvance);
            parameters.Add("@Porcentaje", clases.Porcentaje);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<InmublesProyectoResponse>> ObtenerTipoInmuebleProyecto(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", proyectoId);

            var response = await GetAsyncList<InmublesProyectoResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<List<ClaseInmuebleResponse>> ObtenerClaseInmueble(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", proyectoId);

            var response = await GetAsyncList<ClaseInmuebleResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<List<ListaAvanceResponse>> ObtenerListaAvance(Guid proyectoId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", proyectoId);

            var response = await GetAsyncList<ListaAvanceResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<int> ActualizarEstadoProyectoContrato(Guid proyectoId,int codigoEstado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ProyectoId", proyectoId);
            parameters.Add("@CodigoEstado", codigoEstado);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<Guid> CrearAdjudicacionProyecto(AdjudicacionProyecto adjudicacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AFP_ESTADO_ADJUDICACION", adjudicacion.EstadoAdjudicacion);
            parameters.Add("@AFP_RADICACION_ACEPTACION", adjudicacion.RadicacionAceptacion);
            parameters.Add("@AFP_ID_PROYECTO", adjudicacion.IdProyecto);
            parameters.Add("@AFP_TIPO_INMUEBLE", adjudicacion.TipoVivienda);
            parameters.Add("@AFP_TIPO_VIVIENDA", adjudicacion.ValorVivienda);
            parameters.Add("@AFP_CLASE_VIVIENDA", adjudicacion.ClaseVivienda);
            parameters.Add("@AFP_VALOR_VIVIENDA", adjudicacion.ValorVivienda);
            parameters.Add("@AFP_SOLICITUD_ID", adjudicacion.IdSolictud);

            var response = await GetAsyncFirst<Guid>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearLogisticaAdjudicacion(LogisticaAdjudicacion logistica)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@LA_TIEMPO_DESPLAZAMINENTO", logistica.TimpoDesplazamiento);
            parameters.Add("@LA_DISTANCIA", logistica.Distancia);
            parameters.Add("@LA_ACOMPAÑANTE",logistica.Acompañante);
            parameters.Add("@LA_TIPO_IDENTIFICACION", logistica.TipoIdentificacion);
            parameters.Add("@LA_NUMERO_IDENTIFICACION",logistica.NumeroDocumento);
            parameters.Add("@LA_NOMBRES", logistica.NombreCompleto);
            parameters.Add("@AFP_ADJUFICACION_ID", logistica.AdjudicacionProyecto);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> CrearEntregaViviendaAdjudicacion(EntregaViviendaAdjudicacion entrega)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@EVA_FECHA_CREACION", entrega.FechaCreacion);
            parameters.Add("@EVA_ENTREGADA", entrega.Entregada);
            parameters.Add("@EVA_OBSERVACION", entrega.Observacion);
            parameters.Add("@AFP_ADJUFICACION_ID", entrega.AdjudicacionProyecto);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
    }
}
