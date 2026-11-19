using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Solicitud;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Logica
    /// <author>Erwin Pantoja España</author>
    /// <date>20/10/2021</date>
    /// </summary>
    ///
    public class SolicitudRepository : DapperGenericRepository, ISolicitudRepository
    {
        public SolicitudRepository(IConfiguration configuration) : base(configuration) { }


        /// <summary>
        /// Obtiene tramite solicitud
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <param name="crearSolicitudDto"></param>
        /// <returns></returns>
        public async Task<ObtenerTramiteSolicitud> CrearSolicitud(CrearSolicitud crearSolicitudDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.clienteId), crearSolicitudDto.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.creadoPor), crearSolicitudDto.SOL_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.cuentaId), crearSolicitudDto.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.tipoSubModalidadId), crearSolicitudDto.TPS_SUB_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.tareaId), crearSolicitudDto.TRA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.solEstadoSolicitud), crearSolicitudDto.SOL_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.stlEstadoSolicitud), crearSolicitudDto.STL_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Sol_fechaCreacion), crearSolicitudDto.SOL_FECHACREACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Sol_Num_cedula), crearSolicitudDto.SOL_CEDULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Sol_Nombre_Completo), crearSolicitudDto.SOL_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Sol_radicado), crearSolicitudDto.SOL_RADICADO);

            ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizaNumeroRadicadoSolicitud(ObtenerTramiteSolicitudDto solicitudTareaDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudId), solicitudTareaDto.solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.RadicadoSolicitudSG), solicitudTareaDto.NumeroRadicadoSG);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizaMPRCONTSolicitud(ParametrosObtenerCesantiasDto solicitud, int mpr_cont)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudId), solicitud.solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Mpr_Cont), mpr_cont);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizaFACTSolicitud(Guid solicitud, string factCont, string factNume)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudId), solicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFactSolicitud.factCont), factCont);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumFactSolicitud.factNume), factNume);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudTerceroBeneficiario> InsertarSolicitudTerceroBeneficiario(SolicitudTerceroBeneficiario solicitudTerceroBeneficiario)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.SolicitudId), solicitudTerceroBeneficiario.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoIdentificacion), solicitudTerceroBeneficiario.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.NumeroDocumento), solicitudTerceroBeneficiario.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.RazonSocial), solicitudTerceroBeneficiario.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.Entidad), solicitudTerceroBeneficiario.ENT_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoCuenta), solicitudTerceroBeneficiario.TER_TIPO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.NumeroCuenta), solicitudTerceroBeneficiario.TER_NUMERO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.CreadoPor), solicitudTerceroBeneficiario.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoTercero), solicitudTerceroBeneficiario.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.ValorRetirarDos), solicitudTerceroBeneficiario.TER_VALOR_RETIRAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.FechaCreacionTer), solicitudTerceroBeneficiario.TER_FECHA_CREACION);

            SolicitudTerceroBeneficiario response = new SolicitudTerceroBeneficiario();
            response = await GetAsyncFirst<SolicitudTerceroBeneficiario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<ObtenerTramiteSolicitud> ConsultarExisteSolicitud(ConsultarSolicitud consultareSolicitudDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudParameters.clienteId), consultareSolicitudDto.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudParameters.tipoSubModalidadId), consultareSolicitudDto.TPS_SUB_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudParameters.solEstadoAnulado), consultareSolicitudDto.SOL_ESTADO_ANULADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudParameters.CLI_IDENTIFICACION), consultareSolicitudDto.CLI_IDENTIFICACION);

            ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<SolicitudAcreditacionWorker>> ObtenerSolicitudAcreditacion()
        {

            var response = await GetAsyncList<SolicitudAcreditacionWorker>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response.ToList();
        }
        public async Task<List<ObtenerTramiteSolicitud>> ObtenerSolicitudEnTramiteDePagoAprobadas()
        {

            var response = await GetAsyncList<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response.ToList();
        }





        /// <summary>
        /// Obtiene la solicitud por Id solicitud.
        /// </summary>
        /// <param name="solId"></param>
        /// <returns>ObtenerSolicitud</returns>
        public async Task<ObtenerSolicitud> ConsultaSolicitud(Guid solId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudId), solId);

            ObtenerSolicitud response = new ObtenerSolicitud();
            response = await GetAsyncFirst<ObtenerSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<ObtenerTramiteSolicitud> ConsultarSolicitudPorActividad(ConsultarSolicitudPorActividad consultareSolicitudDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudPorActividadParameters.sltId), consultareSolicitudDto.SLT_ID);

            ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<RespuestaTerceroApoderado> ConsultarTerceroRazonSocial(string numeroDocumento)
        {
            RespuestaTerceroApoderado solicitudTerceroApoderado = new RespuestaTerceroApoderado();
            solicitudTerceroApoderado.TER_NUMERO_DOCUMENTO = numeroDocumento;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.NumeroDocumento), solicitudTerceroApoderado.TER_NUMERO_DOCUMENTO);

            RespuestaTerceroApoderado response = new RespuestaTerceroApoderado();
            response = await GetAsyncFirst<RespuestaTerceroApoderado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        // Consultar propietarios entre solicitudes para integridad de datos, provisional mientras se fusiona tabla props con terceros.
        public async Task<SolicitudPropietario> ConsultarPropietarioIntegridad(string numeroIdentificacion)
        {
            SolicitudPropietario solicitudPropietario = new SolicitudPropietario();
            solicitudPropietario.PRP_NUMERO_IDENTIFICACION = numeroIdentificacion;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.NumeroIdentificacion), solicitudPropietario.PRP_NUMERO_IDENTIFICACION);

            SolicitudPropietario response = new SolicitudPropietario();
            response = await GetAsyncFirst<SolicitudPropietario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudMatriculaInmobiliaria> ConsultarMatriculaInmobiliaria(string numeroMatricula)
        {
            SolicitudMatriculaInmobiliaria solicitudMatricula = new SolicitudMatriculaInmobiliaria();
            solicitudMatricula.MAI_NUMERO_MATRICULA = numeroMatricula;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaMatriculaParameters.MaiNumeroMatricula), solicitudMatricula.MAI_NUMERO_MATRICULA);

            SolicitudMatriculaInmobiliaria response = new SolicitudMatriculaInmobiliaria();
            response = await GetAsyncFirst<SolicitudMatriculaInmobiliaria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudMatriculaInmobiliaria> ConsultarMatriculaInmobiliariaPorNumeroMatricula(string numeroMatricula, int ofc_id)
        {
            SolicitudMatriculaInmobiliaria solicitudMatricula = new SolicitudMatriculaInmobiliaria();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaMatriculaPorNumeroMatriculaParameters.MaiNumMatricula), numeroMatricula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaMatriculaPorNumeroMatriculaParameters.Ofc_id_matricula), ofc_id);


            SolicitudMatriculaInmobiliaria response = new SolicitudMatriculaInmobiliaria();
            response = await GetAsyncFirst<SolicitudMatriculaInmobiliaria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudMatriculaInmobiliaria> ParamBloqueoMatricula(Guid matriculaId)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaMatriculaPorNumeroMatriculaParameters.MatriculaId), matriculaId);


            SolicitudMatriculaInmobiliaria response = new SolicitudMatriculaInmobiliaria();
            response = await GetAsyncFirst<SolicitudMatriculaInmobiliaria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear la matrícula de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudMatricula"></param>
        /// <returns></returns>
        public async Task<SolicitudMatricula> InsertarSolicitudMatricula(SolicitudMatricula solicitudMatricula)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.SolicitudId), solicitudMatricula.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.NumeroMatricula), solicitudMatricula.MAI_NUMERO_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.FechaRegistro), solicitudMatricula.MAI_FECHA_REGISTRO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.Direccion), solicitudMatricula.MAI_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.CiudadId), solicitudMatricula.DPC_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.Principal), solicitudMatricula.MAI_PRINCIPAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.CreadoPor), solicitudMatricula.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.DepartamentoId), solicitudMatricula.DPD_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.OficinaRegistroId), solicitudMatricula.OFC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.Num_matricula), solicitudMatricula.MAI_NUM_MATRICULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.ConyugeActivo), solicitudMatricula.MAI_CONYUGE_ACTIVO);


            SolicitudMatricula response = new SolicitudMatricula();
            response = await GetAsyncFirst<SolicitudMatricula>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizarSolicitudMatricula(SolicitudMatricula solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.SolicitudId), solicitud.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaInmobiliaria.MAI_ID), solicitud.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.BloqueoCreado), solicitud.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.ConyugeActivo), solicitud.MAI_CONYUGE_ACTIVO);



            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizarObservacionSolicitud(CrearSolicitudTareaDto solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), solicitud.SolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Observacion), solicitud.observacion);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarActualizarDatoSolicitud(CrearSolicitudTareaDto solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudMatriculaParameters.SolicitudId), solicitud.SolicitudId);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el propietario de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudPropietario"></param>
        /// <returns></returns>
        public async Task<SolicitudPropietario> InsertarSolicitudPropietario(SolicitudPropietario solicitudPropietario)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.SolicitudId), solicitudPropietario.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.TipoIdentificacion), solicitudPropietario.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.RazonSocial), solicitudPropietario.PRP_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.NumeroIdentificacion), solicitudPropietario.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.CreadoPor), solicitudPropietario.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.PrimerNombre), solicitudPropietario.PrimerNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.SegundoNombre), solicitudPropietario.SegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.PrimerApellido), solicitudPropietario.PrimerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.SegundoApellido), solicitudPropietario.SegundoApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioParameters.FechaCreacion), solicitudPropietario.PRP_FECHACREACION);


            SolicitudPropietario response = new SolicitudPropietario();
            response = await GetAsyncFirst<SolicitudPropietario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el apoderado de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudTerceroApoderado"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroApoderado> InsertarSolicitudTerceroApoderado(SolicitudTerceroApoderado solicitudTerceroApoderado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SolicitudId), solicitudTerceroApoderado.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoIdentificacion), solicitudTerceroApoderado.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.NumeroDocumento), solicitudTerceroApoderado.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.RazonSocial), solicitudTerceroApoderado.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.CreadoPor), solicitudTerceroApoderado.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoTercero), solicitudTerceroApoderado.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.EsAutorizado), solicitudTerceroApoderado.TER_ESAUTORIZADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.Parentesco), solicitudTerceroApoderado.TER_PARENTESCO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.PrimerNombre), solicitudTerceroApoderado.TER_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SegundoNombre), solicitudTerceroApoderado.TER_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.PrimerApellido), solicitudTerceroApoderado.TER_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SegundoApellido), solicitudTerceroApoderado.TER_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.FechaRegistro), solicitudTerceroApoderado.TER_FECHA_REGISTRO);

            SolicitudTerceroApoderado response = new SolicitudTerceroApoderado();
            response = await GetAsyncFirst<SolicitudTerceroApoderado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        public async Task<SolicitudBeneficiario> SolicitudBeneficiario(SolicitudBeneficiario solicitudTerceroApoderado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SolicitudId), solicitudTerceroApoderado.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoIdentificacion), solicitudTerceroApoderado.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.NumeroDocumento), solicitudTerceroApoderado.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.CreadoPor), solicitudTerceroApoderado.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoTercero), solicitudTerceroApoderado.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.Parentesco), solicitudTerceroApoderado.TER_PARENTESCO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.PrimerNombre), solicitudTerceroApoderado.TER_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SegundoNombre), solicitudTerceroApoderado.TER_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.PrimerApellido), solicitudTerceroApoderado.TER_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SegundoApellido), solicitudTerceroApoderado.TER_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.FechaRegistro), solicitudTerceroApoderado.TER_FECHA_REGISTRO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.ValorRetirarDos), solicitudTerceroApoderado.TER_VALOR_RETIRAR);

            SolicitudBeneficiario response = new SolicitudBeneficiario();
            response = await GetAsyncFirst<SolicitudBeneficiario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el beneficiario de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudTerceroBeneficiario"></param>
        /// <returns></returns>

        public async Task<bool> ActualizarCodProvSevenTerceroBeneficiario(ParametrosObtenerCesantiasDto solicitud, int TipoTerceroBeneficiarioId, int codProveeSeven)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.SolicitudId), solicitud.solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoTercero), TipoTerceroBeneficiarioId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.CodProveeSeven), codProveeSeven);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el constructor de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudTerceroConstructor"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroConstructor> InsertarSolicitudTerceroConstructor(SolicitudTerceroConstructor solicitudTerceroConstructor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.SolicitudId), solicitudTerceroConstructor.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.TipoIdentificacion), solicitudTerceroConstructor.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.NumeroDocumento), solicitudTerceroConstructor.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.RazonSocial), solicitudTerceroConstructor.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.NumeroLicencia), solicitudTerceroConstructor.TER_NUMERO_LICENCIA_CONSTRUCCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.CreadoPor), solicitudTerceroConstructor.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroConstructorParameters.TipoTercero), solicitudTerceroConstructor.TER_TIPO_TERCERO_FK);

            SolicitudTerceroConstructor response = new SolicitudTerceroConstructor();
            response = await GetAsyncFirst<SolicitudTerceroConstructor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el constructor de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudTerceroVendedor"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroVendedor> InsertarSolicitudTerceroVendedor(SolicitudTerceroVendedor solicitudTerceroVendedor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.SolicitudId), solicitudTerceroVendedor.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.TipoIdentificacion), solicitudTerceroVendedor.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.NumeroDocumento), solicitudTerceroVendedor.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.RazonSocial), solicitudTerceroVendedor.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.Direccion), solicitudTerceroVendedor.TER_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.Ciudad), solicitudTerceroVendedor.DPC_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.CorreoElectronico), solicitudTerceroVendedor.TER_CORREO_ELECTRONICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.Telefono), solicitudTerceroVendedor.TER_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.CreadoPor), solicitudTerceroVendedor.CREATEDO_POR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.TipoTercero), solicitudTerceroVendedor.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroVendedorParameters.FechaCreacionTer), solicitudTerceroVendedor.TER_FECHA_CREACION);

            SolicitudTerceroVendedor response = new SolicitudTerceroVendedor();
            response = await GetAsyncFirst<SolicitudTerceroVendedor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de insertar archivos de solicitud
        /// </summary>
        /// <param name="insertarArchivo"></param>
        /// <returns></returns>
        public async Task<InsertarArchivo> InsertarArchivoPorSolicitudTarea(InsertarArchivo insertarArchivo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.NombreArchivo), insertarArchivo.AST_NOMBRE_ARCHIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.RutaArchivo), insertarArchivo.AST_RUTA_STORAGE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.ExtensionArchivo), insertarArchivo.AST_EXTENSION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.Solicitud), insertarArchivo.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.Parametrizacion), insertarArchivo.PAM_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.CreadoPor), insertarArchivo.AST_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.NumeroFolio), insertarArchivo.AST_NUMERO_FOLIO);

            InsertarArchivo response = new InsertarArchivo();
            response = await GetAsyncFirst<InsertarArchivo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el autorizado de solicitud de vivienda
        /// </summary>
        /// <param name="solicitudTerceroAutorizado"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroAutorizado> InsertarSolicitudTerceroAutorizado(SolicitudTerceroAutorizado solicitudTerceroAutorizado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroAutorizadoParameters.SolicitudId), solicitudTerceroAutorizado.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroAutorizadoParameters.TipoIdentificacion), solicitudTerceroAutorizado.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroAutorizadoParameters.NumeroDocumento), solicitudTerceroAutorizado.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroAutorizadoParameters.RazonSocial), solicitudTerceroAutorizado.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroAutorizadoParameters.CreadoPor), solicitudTerceroAutorizado.CREATEDO_POR);

            SolicitudTerceroAutorizado response = new SolicitudTerceroAutorizado();
            response = await GetAsyncFirst<SolicitudTerceroAutorizado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;

        }

        public async Task<bool> ActualizarSolicitudTarea(ActualizarSolicitudTarea actualizarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), actualizarSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoId), actualizarSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Observacion), actualizarSolicitudTarea.SLT_OBSERVACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NOMBRE_COMPLETO), actualizarSolicitudTarea.NOMBRE_COMPLETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NUM_CEDULA), actualizarSolicitudTarea.NUM_CEDULA);


            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizarEstadoInconsistente(Guid solicitudId, string Radicado378)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("SOL_ID", solicitudId);
            parameters.Add("Radicado378", Radicado378);


            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaMatricula>> ConsultarSolicitudMatricula(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudMatriculaParameters.SolicitudId), solicitudId);

            IEnumerable<RespuestaMatricula> response;
            response = await GetAsyncList<RespuestaMatricula>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<Procesos> ObtenerProcesoPorSubmodalidad(Guid sudModalidadId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.tipoSubModalidadId), sudModalidadId);


            return await GetAsyncFirst<Procesos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Procesos> ObtenerProcesoPorSolicitudTarea(Guid solicitudTareaId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaSolicitudPorActividadParameters.sltId), solicitudTareaId);


            return await GetAsyncFirst<Procesos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<RespuestaConyugue> ConsultarConyugue(int cliId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.clienteId), cliId);

            RespuestaConyugue response;
            response = await GetAsyncFirst<RespuestaConyugue>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        public async Task<bool> GuardarConyugue(RespuestaConyugue conyugue)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CliId), conyugue.CliId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CliId), conyugue.CliId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnyTipo), conyugue.CnyTipo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnyIdentificacion), conyugue.CnyIdentificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnyPrimerNombre), conyugue.CnyPrimerNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnySegundoNombre), conyugue.CnySegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnyPrimerApellido), conyugue.CnyPrimerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConyugueParameters.CnySegundoApellido), conyugue.CnySegundoApellido);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaPropietario>> ConsultarSolicitudPropietario(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudMatriculaParameters.SolicitudId), solicitudId);

            IEnumerable<RespuestaPropietario> response;
            response = await GetAsyncList<RespuestaPropietario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudAdjudicacion> ConsultarSolicitudAdjudicacion(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.SOL_ID), solicitudId);

            SolicitudAdjudicacion response;
            response = await GetAsyncFirst<SolicitudAdjudicacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudEscrituracion> ConsultarSolicitudEscrituracion(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.SOL_ID), solicitudId);

            SolicitudEscrituracion response;
            response = await GetAsyncFirst<SolicitudEscrituracion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaTerceroVendedor>> ConsultarTerceroVendedor(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoTercero), tipoTercero);

            IEnumerable<RespuestaTerceroVendedor> response;
            response = await GetAsyncList<RespuestaTerceroVendedor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaTerceroBeneficiario>> ConsultarTerceroBeneficiario(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoTercero), tipoTercero);

            IEnumerable<RespuestaTerceroBeneficiario> response;
            response = await GetAsyncList<RespuestaTerceroBeneficiario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaTerceroApoderado>> ConsultarTerceroApoderado(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoTercero), tipoTercero);

            IEnumerable<RespuestaTerceroApoderado> response;
            response = await GetAsyncList<RespuestaTerceroApoderado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<SolicitudBeneficiario>> ConsultarSolicitudBeneficiario(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroApoderadoParameters.TipoTercero), tipoTercero);

            IEnumerable<SolicitudBeneficiario> response;
            response = await GetAsyncList<SolicitudBeneficiario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaTerceroBeneficiarioEstudio>> ConsultarTerceroBeneficiarioEstudio(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.TipoTerceroBeneficiarioEstudioId), tipoTercero);

            IEnumerable<RespuestaTerceroBeneficiarioEstudio> response;
            response = await GetAsyncList<RespuestaTerceroBeneficiarioEstudio>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<RespuestaTerceroEntidadEducativa>> ConsultarTerceroEntidadEducativa(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TipoTerceroEntidadEducativa), tipoTercero);

            IEnumerable<RespuestaTerceroEntidadEducativa> response;
            response = await GetAsyncList<RespuestaTerceroEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<RespuestaTerceroEntidadSeguroEducativo> ConsultarTerceroEntidadSeguroEducativo(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.TipoTerceroEntidadSeguroId), tipoTercero);

            RespuestaTerceroEntidadSeguroEducativo response;
            response = await GetAsyncFirst<RespuestaTerceroEntidadSeguroEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<RespuestaTerceroTenedorAcciones> ConsultarTerceroTenedorAcciones(Guid solicitudId, int tipoTercero)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.TipoTercero), tipoTercero);

            RespuestaTerceroTenedorAcciones response;
            response = await GetAsyncFirst<RespuestaTerceroTenedorAcciones>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <param name="solicitudPropietarioMatricula"></param>
        /// <returns></returns>
        public async Task<SolicitudPropietarioMatricula> InsertarSolicitudPropietarioMatricula(SolicitudPropietarioMatricula solicitudPropietarioMatricula)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioMatriculaParameters.PropietarioId), solicitudPropietarioMatricula.PRP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioMatriculaParameters.MatriculaId), solicitudPropietarioMatricula.MAI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioMatriculaParameters.CreadoPor), solicitudPropietarioMatricula.PMA_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudPropietarioMatriculaParameters.FechaPropietario), DateTime.Now);

            SolicitudPropietarioMatricula response = new SolicitudPropietarioMatricula();
            response = await GetAsyncFirst<SolicitudPropietarioMatricula>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <param name="solicitudTerceroEntidadEducativa"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroEntidadEducativa> InsertarSolicitudTerceroEntidadEducativa(SolicitudTerceroEntidadEducativa solicitudTerceroEntidadEducativa)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.EntidadId), solicitudTerceroEntidadEducativa.ENE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.Nit), solicitudTerceroEntidadEducativa.ENE_NIT);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.RazonSocial), solicitudTerceroEntidadEducativa.ENE_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.NivelId), solicitudTerceroEntidadEducativa.PRN_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.ReciboPago), solicitudTerceroEntidadEducativa.TER_NUM_RECIBO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.FechaPago), solicitudTerceroEntidadEducativa.TER_FECHA_LIMITE_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.CreadoPor), solicitudTerceroEntidadEducativa.ENE_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TipoTerceroEntidadEducativa), solicitudTerceroEntidadEducativa.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.SolicitudId), solicitudTerceroEntidadEducativa.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TipoIdentificacionNit), solicitudTerceroEntidadEducativa.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_DEPARTAMENTO), solicitudTerceroEntidadEducativa.TER_ENT_DEPARTAMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_CIUDAD), solicitudTerceroEntidadEducativa.TER_ENT_CIUDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_MODALIDAD_ESTUDIO), solicitudTerceroEntidadEducativa.TER_ENT_MODALIDAD_ESTUDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_PERIODO_ACADEMICO), solicitudTerceroEntidadEducativa.TER_ENT_PERIODO_ACADEMICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_HORAS), solicitudTerceroEntidadEducativa.TER_ENT_HORAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_FECHA_INICIO), solicitudTerceroEntidadEducativa.TER_ENT_FECHA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_FECHA_FIN), solicitudTerceroEntidadEducativa.TER_ENT_FECHA_FIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_ACREDITADA), solicitudTerceroEntidadEducativa.TER_ENT_ACREDITADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_VALOR_RECIBO), solicitudTerceroEntidadEducativa.TER_ENT_VALOR_RECIBO);

            SolicitudTerceroEntidadEducativa response = new SolicitudTerceroEntidadEducativa();
            response = await GetAsyncFirst<SolicitudTerceroEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudTerceroEntidadEducativa> ActualizarSolicitudTerceroEntidadEducativa(SolicitudTerceroEntidadEducativa solicitudTerceroEntidadEducativa)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.ID), solicitudTerceroEntidadEducativa.TER_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.EntidadId), solicitudTerceroEntidadEducativa.ENE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.Nit), solicitudTerceroEntidadEducativa.ENE_NIT);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.RazonSocial), solicitudTerceroEntidadEducativa.ENE_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.NivelId), solicitudTerceroEntidadEducativa.PRN_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.ReciboPago), solicitudTerceroEntidadEducativa.TER_NUM_RECIBO_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.FechaPago), solicitudTerceroEntidadEducativa.TER_FECHA_LIMITE_PAGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.CreadoPor), solicitudTerceroEntidadEducativa.ENE_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TipoTerceroEntidadEducativa), solicitudTerceroEntidadEducativa.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.SolicitudId), solicitudTerceroEntidadEducativa.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TipoIdentificacionNit), solicitudTerceroEntidadEducativa.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_DEPARTAMENTO), solicitudTerceroEntidadEducativa.TER_ENT_DEPARTAMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_CIUDAD), solicitudTerceroEntidadEducativa.TER_ENT_CIUDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_MODALIDAD_ESTUDIO), solicitudTerceroEntidadEducativa.TER_ENT_MODALIDAD_ESTUDIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_PERIODO_ACADEMICO), solicitudTerceroEntidadEducativa.TER_ENT_PERIODO_ACADEMICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_HORAS), solicitudTerceroEntidadEducativa.TER_ENT_HORAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_FECHA_INICIO), solicitudTerceroEntidadEducativa.TER_ENT_FECHA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_FECHA_FIN), solicitudTerceroEntidadEducativa.TER_ENT_FECHA_FIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_ACREDITADA), solicitudTerceroEntidadEducativa.TER_ENT_ACREDITADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEntidadEducativaParameters.TER_ENT_VALOR_RECIBO), solicitudTerceroEntidadEducativa.TER_ENT_VALOR_RECIBO);

            SolicitudTerceroEntidadEducativa response = new SolicitudTerceroEntidadEducativa();
            response = await GetAsyncFirst<SolicitudTerceroEntidadEducativa>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Método encargado de crear el beneficiario estudio para entidad educativa
        /// </summary>
        /// <param name="solicitudTerceroBeneficiarioEstudio"></param>
        /// <returns></returns>
        public async Task<SolicitudTerceroBeneficiarioEstudio> InsertarSolicitudTerceroBeneficiarioEstudio(SolicitudTerceroBeneficiarioEstudio solicitudTerceroBeneficiarioEstudio)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.numDoc), solicitudTerceroBeneficiarioEstudio.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.razonSocialEstudiante), solicitudTerceroBeneficiarioEstudio.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.parentescoId), solicitudTerceroBeneficiarioEstudio.TER_PARENTESCO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.CreadoPor), solicitudTerceroBeneficiarioEstudio.CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.TipoTerceroBeneficiarioEstudioId), solicitudTerceroBeneficiarioEstudio.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.SolicitudId), solicitudTerceroBeneficiarioEstudio.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioEstudioParameters.tipoDocId), solicitudTerceroBeneficiarioEstudio.TID_ID_FK);

            SolicitudTerceroBeneficiarioEstudio response = new SolicitudTerceroBeneficiarioEstudio();
            response = await GetAsyncFirst<SolicitudTerceroBeneficiarioEstudio>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        // <summary>
        /// Método encargado de crear la entidad de seguro o poliza educativa
        /// </summary>
        public async Task<SolicitudTerceroEntidadSeguroEducativo> InsertarSolicitudTerceroEntidadSeguroEducativo(SolicitudTerceroEntidadSeguroEducativo solicitudTerceroEntidadSeguroEducativo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.numDoc), solicitudTerceroEntidadSeguroEducativo.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.razonSocialAseguradora), solicitudTerceroEntidadSeguroEducativo.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.CreadoPor), solicitudTerceroEntidadSeguroEducativo.TER_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.TipoTerceroEntidadSeguroId), solicitudTerceroEntidadSeguroEducativo.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.SolicitudId), solicitudTerceroEntidadSeguroEducativo.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.tipoDocId), solicitudTerceroEntidadSeguroEducativo.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.entidadSeguroEducativoId), solicitudTerceroEntidadSeguroEducativo.ESE_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroEntidadSeguroEducativoParameters.numeroPoliza), solicitudTerceroEntidadSeguroEducativo.TER_NUM_POLIZA);


            SolicitudTerceroEntidadSeguroEducativo response = new SolicitudTerceroEntidadSeguroEducativo();
            response = await GetAsyncFirst<SolicitudTerceroEntidadSeguroEducativo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        // <summary>
        /// Método encargado de crear el tercero tenedor de acciones
        /// </summary>
        public async Task<SolicitudTerceroTenedorAcciones> InsertarSolicitudTerceroTenedorAcciones(SolicitudTerceroTenedorAcciones solicitudTerceroTenedorAcciones)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.SolicitudId), solicitudTerceroTenedorAcciones.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.TipoIdentificacion), solicitudTerceroTenedorAcciones.TID_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.NumeroDocumento), solicitudTerceroTenedorAcciones.TER_NUMERO_DOCUMENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.RazonSocial), solicitudTerceroTenedorAcciones.TER_NOMBRE_RAZON_SOCIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.Direccion), solicitudTerceroTenedorAcciones.TER_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.Ciudad), solicitudTerceroTenedorAcciones.DPC_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.CorreoElectronico), solicitudTerceroTenedorAcciones.TER_CORREO_ELECTRONICO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.Telefono), solicitudTerceroTenedorAcciones.TER_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.CreadoPor), solicitudTerceroTenedorAcciones.TER_CREATEDOPOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.TipoTercero), solicitudTerceroTenedorAcciones.TER_TIPO_TERCERO_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroTenedorAccionesParameters.RazonSocialEmisor), solicitudTerceroTenedorAcciones.TER_EMISOR_NOMBRE_RAZON_SOCIAL);


            SolicitudTerceroTenedorAcciones response = new SolicitudTerceroTenedorAcciones();
            response = await GetAsyncFirst<SolicitudTerceroTenedorAcciones>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarTerceroSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), solicitudId);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarMatriculaPropietarioSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), solicitudId);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarPropietarioSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), solicitudId);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarMatriculaInmobiliaria(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaInmobiliaria.MAI_ID), solicitudId);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<ObtenerTramiteSolicitud> AprobarSolicitudTarea(AprobarSolicitudTarea aprobarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), aprobarSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoId), aprobarSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoNuevoId), aprobarSolicitudTarea.SLT_ESTADO_SOLICITUD_NUEVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoCliente), aprobarSolicitudTarea.SLT_ESTADO_CLIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudEstadCcuenta), aprobarSolicitudTarea.SLT_ESTADO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.FRZ_ID), aprobarSolicitudTarea.FRZ_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NOMBRE_COMPLETO), aprobarSolicitudTarea.NOMBRE_COMPLETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NUM_CEDULA), aprobarSolicitudTarea.NUM_CEDULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.IdGrado), aprobarSolicitudTarea.IdGrado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Fecha_tarea), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Prorrateo), aprobarSolicitudTarea.Prorrateo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.ReglaRegimenCuota), aprobarSolicitudTarea.ReglaRegimenCuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolRechazada), aprobarSolicitudTarea.SOL_RECHAZADA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.EmbargoBloqueo), aprobarSolicitudTarea.EmbargoBloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NivelRiesgo), aprobarSolicitudTarea.NivelRiesgo);


            ObtenerTramiteSolicitud response;
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<ObtenerTramiteSolicitud> AprobarSolicitudTareaAcreditacion(AprobarSolicitudTarea aprobarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), aprobarSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoId), aprobarSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoNuevoId), aprobarSolicitudTarea.SLT_ESTADO_SOLICITUD_NUEVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoCliente), aprobarSolicitudTarea.SLT_ESTADO_CLIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudEstadCcuenta), aprobarSolicitudTarea.SLT_ESTADO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.FRZ_ID), aprobarSolicitudTarea.FRZ_CODIGO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NOMBRE_COMPLETO), aprobarSolicitudTarea.NOMBRE_COMPLETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NUM_CEDULA), aprobarSolicitudTarea.NUM_CEDULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.IdGrado), aprobarSolicitudTarea.IdGrado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Fecha_tarea), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.Prorrateo), aprobarSolicitudTarea.Prorrateo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.ReglaRegimenCuota), aprobarSolicitudTarea.ReglaRegimenCuota);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolRechazada), aprobarSolicitudTarea.SOL_RECHAZADA);


            ObtenerTramiteSolicitud response;
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aprobarSolicitudTarea"></param>
        /// <returns></returns>
        public async Task<Guid> ConsultarSiguienteTareaSolicitud(AprobarSolicitudTarea aprobarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), aprobarSolicitudTarea.SLT_ID);

            ObtenerTramiteSolicitud response;
            response = await GetAsyncFirst<ObtenerTramiteSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response.SLT_ID;
        }

        public async Task<bool> RechazarSolicitudTarea(RechazarSolicitudTarea rechazarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), rechazarSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoId), rechazarSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoNuevoId), rechazarSolicitudTarea.SLT_ESTADO_SOLICITUD_NUEVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudEstadoId), rechazarSolicitudTarea.SOL_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NOMBRE_COMPLETO), rechazarSolicitudTarea.NOMBRE_COMPLETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NUM_CEDULA), rechazarSolicitudTarea.NUM_CEDULA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudId), rechazarSolicitudTarea.SOL_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.FechaRegistro), rechazarSolicitudTarea.SOL_FECHA_REGISTRO);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        /// <summary>
        /// Anular la solicitud y la tarea de la actividad
        /// Create: David Pineda
        /// </summary>
        /// <param name="anularSolicitudTarea"></param>
        /// <returns></returns>
        public async Task<bool> AnularSolicitudTarea(AnularSolicitudTarea anularSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaId), anularSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudTareaEstadoId), anularSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.SolicitudEstadoId), anularSolicitudTarea.SOL_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NOMBRE_COMPLETO), anularSolicitudTarea.NOMBRE_COMPLETO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.NUM_CEDULA), anularSolicitudTarea.NUM_CEDULA);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Buscar los documentos que han sido cargados por una solicitud
        /// Modificado: Anderson Páez: Se modifica para mostrar el número de folio.
        /// </summary>
        /// <param name="idSubModalidad"></param>
        /// <param name="idSolicitudtarea"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RespuestaDocumentosPorSubModalidad>> ConsultarDocumentosPorSubModalidad(Guid idSubModalidad, Guid idSolicitudtarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumGeneralParameters.SubModalidad), idSubModalidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumGeneralParameters.SolicitudTarea), idSolicitudtarea);

            IEnumerable<RespuestaDocumentosPorSubModalidad> response;
            response = await GetAsyncList<RespuestaDocumentosPorSubModalidad>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ConsultarArchivoPorSolicitud>> ConsultarArchivoPorSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.Solicitud), solicitudId);

            IEnumerable<ConsultarArchivoPorSolicitud> response;
            response = await GetAsyncList<ConsultarArchivoPorSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitud eliminarArchivoPorSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.ArchivoSolicitudId), eliminarArchivoPorSolicitud.AST_ID);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizarSolicitud(ActualizarSolicitud actualizarSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), actualizarSolicitud.SOL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.valorRetirar), actualizarSolicitud.SOL_VALOR_A_RETIRAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Posee_Avac), actualizarSolicitud.Posee_Avac);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Numero_Folio), actualizarSolicitud.NUMERO_FOLIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Fecha_firma_escritura), actualizarSolicitud.FECHA_FIRMA_ESCRITURA);
            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }



        /// <summary>
        /// InsertarInconsistenciaSolicitud
        /// </summary>
        /// <param name="insertarInconsistenciaSolicitud"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>01/06/2022</date>
        public async Task<InconsistenciaSolicitud> InsertarInconsistenciaSolicitud(InconsistenciaSolicitud insertarInconsistenciaSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.ListaInconsistenciaId), insertarInconsistenciaSolicitud.LIN_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Novedad), insertarInconsistenciaSolicitud.NVD_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.GrupoInconsistencia), insertarInconsistenciaSolicitud.GIN_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.HerramientaId), insertarInconsistenciaSolicitud.HER_ID_FK == Guid.Empty ? null : insertarInconsistenciaSolicitud.HER_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.PuntoAtencionId), insertarInconsistenciaSolicitud.PTA_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), insertarInconsistenciaSolicitud.SOL_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.TecnicoId), insertarInconsistenciaSolicitud.INC_CREATEDOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.ObjetoEstudio), insertarInconsistenciaSolicitud.INC_OBJETO_ESTUDIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Analisis), insertarInconsistenciaSolicitud.INC_ANALISIS_REALIZADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Observacion), insertarInconsistenciaSolicitud.INC_OBSERVACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Estado), insertarInconsistenciaSolicitud.INC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.CreadoPor), insertarInconsistenciaSolicitud.INC_CREATEDOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.CreadoPor), insertarInconsistenciaSolicitud.INC_CREATEDOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.IdCausalInterrupcion), insertarInconsistenciaSolicitud.CIN_ID_FK);

            return await GetAsyncFirst<InconsistenciaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// ActualizarInconsistenciaSolicitud
        /// </summary>
        /// <param name="actualizarInconsistenciaSolicitud"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>01/06/2022</date>
        public async Task<InconsistenciaSolicitud> ActualizarInconsistenciaSolicitud(InconsistenciaSolicitud actualizarInconsistenciaSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.InconsistenciaId), actualizarInconsistenciaSolicitud.INC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Novedad), actualizarInconsistenciaSolicitud.NVD_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.GrupoInconsistencia), actualizarInconsistenciaSolicitud.GIN_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.ListaInconsistenciaId), actualizarInconsistenciaSolicitud.LIN_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.HerramientaId), actualizarInconsistenciaSolicitud.HER_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.PuntoAtencionId), actualizarInconsistenciaSolicitud.PTA_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), actualizarInconsistenciaSolicitud.SOL_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.TecnicoId), actualizarInconsistenciaSolicitud.USR_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.ObjetoEstudio), actualizarInconsistenciaSolicitud.INC_OBJETO_ESTUDIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Analisis), actualizarInconsistenciaSolicitud.INC_ANALISIS_REALIZADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Observacion), actualizarInconsistenciaSolicitud.INC_OBSERVACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Estado), actualizarInconsistenciaSolicitud.INC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.ModificadoPor), actualizarInconsistenciaSolicitud.INC_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.FechaModificacion), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.IdCausalInterrupcion), actualizarInconsistenciaSolicitud.CIN_ID_FK);

            return await GetAsyncFirst<InconsistenciaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InconsistenciaSolicitud>> ConsultarInconsistenciaSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), solicitudId);

            IEnumerable<InconsistenciaSolicitud> response;
            response = await GetAsyncList<InconsistenciaSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        /// <summary>
        /// Consulta solicitudes de cesantías
        /// </summary>
        /// <author>Hanson Restrepo</author>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RespuestaConsultarSolicitudCesantias>> ConsultarSolicitudCesantias(PeticionConsultarSolicitudCesantias solicitud, int PageSize, int PageNumber)
        {
            DynamicParameters parameters = new DynamicParameters();

            if (solicitud.NumRadicado == "") { solicitud.NumRadicado = null; }
            if (solicitud.Identificacion == "") { solicitud.Identificacion = null; }
            if (solicitud.EstadoTarea == null) { solicitud.EstadoTarea = 0; }


            // parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.Identificacion), solicitud.Identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.EstadoSolicitud), solicitud.EstadoTarea);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.NumRadicado), solicitud.NumRadicado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.FechaIni), solicitud.FechaIni);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.FechaFin), solicitud.FechaFin);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.Tipo), solicitud.TipoSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.Modalidad), solicitud.Modalidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.SubModalidad), solicitud.SubModalidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.NumeroSolicitud), solicitud.NumeroSolictud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.UsuarioIngreso), solicitud.UsuarioIngreso);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.Solicitante), solicitud.Solicitante);
            // parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarSolicitudCesantiasParameters.Usuario), solicitud.idUsuario);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPaginacion.PageNumber), PageNumber);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsPaginacion.PageSize), PageSize);

            var response = await GetAsyncList<RespuestaConsultarSolicitudCesantias>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return response;
        }

        /// <summary>
        /// Eliminar inconsistencia de una solicitud
        /// </summary>
        /// <author>Andres Riaño</author>
        /// <param name="inconsistenciaId"></param>
        /// <returns></returns>
        public async Task<InconsistenciaSolicitud> EliminarInconsistenciaSolicitud(Guid inconsistenciaId)
        {
            {
                DynamicParameters parametros = new DynamicParameters();

                parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.InconsistenciaId), inconsistenciaId);

                return await GetAsyncFirst<InconsistenciaSolicitud>(HelperDBParameters.BuilderFunction(
                     HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<ConsultarNumeroFolioSolicitud> ConsultarInformacionSolicitud(int numeroSolicitud)
        {
            {
                DynamicParameters parametros = new DynamicParameters();

                parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Sol_Numero_Solicitud), numeroSolicitud);

                return await GetAsyncFirst<ConsultarNumeroFolioSolicitud>(HelperDBParameters.BuilderFunction(
                     HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<ConsultarNumeroFolioSolicitud> ConsultarSolicitudTareaPorId(Guid SLT_ID)
        {
            {
                DynamicParameters parametros = new DynamicParameters();

                parametros.Add(HelpersEnums.GetEnumDescription(EnumGeneralParameters.SolicitudTarea), SLT_ID);

                return await GetAsyncFirst<ConsultarNumeroFolioSolicitud>(HelperDBParameters.BuilderFunction(
                     HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<ObtenerFormularioSolicitud>> ObtenerFomulariosSolicitud(string timCodigo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerFomulariosSolicitudParameters.TimCodigo), timCodigo);
            IEnumerable<ObtenerFormularioSolicitud> response = await GetAsyncList<ObtenerFormularioSolicitud>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<Fuerzas>> ObtenerFuerzaPorId(int frzId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumActualizaSolicitudTareaParameters.FRZ_ID), frzId);
            IEnumerable<Fuerzas> response = await GetAsyncList<Fuerzas>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<ComunicacionSolicitud>> ObtenerComunicacionesSolicitud(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), IdSolicitud);

            return await GetAsyncList<ComunicacionSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<ComunicacionSolicitud> InsertarComunicacionSolicitud(ComunicacionSolicitud request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), request.SOL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.IdCausalComunicacion), request.CSC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.TextoIncluir), request.CMN_TEXTO_INCLUIR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.FirmaDigital), request.CMN_FIRMA_DIGITAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.Radicado), request.CMN_RADICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.RadicadoRelacionado), request.CMN_RADICADO_RELACIONADO);

            return await GetAsyncFirst<ComunicacionSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<ComunicacionSolicitud> ActualizarComunicacionesSolicitud(ComunicacionSolicitud request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.IdComunicacion), request.CMN_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.SolicitudId), request.SOL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.IdCausalComunicacion), request.CSC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.TextoIncluir), request.CMN_TEXTO_INCLUIR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSolicitudParameters.FirmaDigital), request.CMN_FIRMA_DIGITAL);

            return await GetAsyncFirst<ComunicacionSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<ObservacionSolicitud> ActualizarObservacionesSolicitud(ObservacionSolicitud request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumGeneralParameters.SolicitudTarea), request.SLT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Observacion), request.SOL_OBSERVACIONES);

            return await GetAsyncFirst<ObservacionSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TrazabilidadSolicitud>> ObtenerTrazabilidadSolicitud(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.IdSolicitud), IdSolicitud);

            return await GetAsyncList<TrazabilidadSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionTareas(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }


        public async Task<IEnumerable<InconsistenciasDto>> SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@IdSolicitud", IdSolicitud);

            var a = await GetAsyncList<InconsistenciasDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }


        public async Task<TareaSolicitud> AsignacionDeTareas(TareaSolicitud tarea)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@AST_MENSAJE", tarea.AST_MENSAJE);
            parametros.Add("@AST_LINK", tarea.AST_LINK);
            parametros.Add("@AST_ESTADO", tarea.AST_ESTADO);
            parametros.Add("@USR_ID", tarea.USR_ID);
            parametros.Add("@FLJ_ID", tarea.FLJ_ID);
            parametros.Add("@AST_FECHA_REGISTRO", DateTime.Now);
            parametros.Add("@AST_FECHA_FINALIZACION", DateTime.Now);

            return await GetAsyncFirst<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadosCliente>> ObtenerEstadosCliente()
        {
            DynamicParameters parametros = new DynamicParameters();

            return await GetAsyncList<EstadosCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EstadoCausalCliente>> ObtenerCausalEstadoCliente()
        {
            DynamicParameters parametros = new DynamicParameters();

            return await GetAsyncList<EstadoCausalCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Sucursal>> ObtenerSucursales(string sucursales = null)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitIdFk), sucursales);

            return await GetAsyncList<Sucursal>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Turno>> ObtenerTurnos(string turno = null)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.TitIdFk), turno);

            return await GetAsyncList<Turno>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Recepcion>> ObtenerRecepcion(string recepcion = null)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.RitIdFk), recepcion);

            return await GetAsyncList<Recepcion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<InformacionTurno> ObterInformacionTurnoSolicitud(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.IdSolicitud), IdSolicitud);

            return await GetAsyncFirst<InformacionTurno>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        public async Task<SolicitudInfo> ConsultarModadidaYSubmodalidadXSolicitud(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.IdSolicitud), IdSolicitud);

            return await GetAsyncFirst<SolicitudInfo>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }
        public async Task<InformacionTurno> CrearInformacionTurnoSolicitud(InformacionTurno informacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.RitIdFk), informacion.RIT_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitIdFk), informacion.SIT_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.TitIdFk), informacion.TIT_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitPreTramite), informacion.SIT_PRE_TRAMITE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitTurnoNumero), informacion.SIT_TURNO_NUMERO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitFechaComision), informacion.SIT_FECHA_COMISION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SitNumeroComision), informacion.SIT_NUMERO_COMISION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionTurno.SolIdFk), informacion.SOL_ID_FK);

            // Agrega más atributos según la estructura de tu modelo InformacionTurno

            return await GetAsyncFirst<InformacionTurno>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarCheckArchivos(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), idSolicitud);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<CheckArchivo> CheckArchivosSolicitud(Guid idSolicitud, string nombreArchivo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), idSolicitud);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarArchivoParameters.NombreArchivoCheck), nombreArchivo);


            return await GetAsyncFirst<CheckArchivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<CheckArchivo>> ObtenerArchivosCheck(Guid idSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), idSolicitud);

            return await GetAsyncList<CheckArchivo>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<bool> InsertarMatriculaSolicitud(Guid solicitudId, Guid matriculaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.SolicitudId), solicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMatriculaInmobiliaria.MAI_ID_FK), matriculaId);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<InformacionAdjudicacion> InsertarSolicitudInformacionAdjudicacion(InformacionAdjudicacion informacionAdjudicacion, Guid SOL_ID_FK, Guid CreadoPor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.SOL_ID_FK), SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.TipoVivienda), informacionAdjudicacion.TipoVivienda);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.Direccion), informacionAdjudicacion.Direccion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.MatriculaInmobiliaria), informacionAdjudicacion.MatriculaInmobiliaria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.ValorSubsidio), informacionAdjudicacion.ValorSubsidio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.SIA_CIUDAD), informacionAdjudicacion.SIA_CIUDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionAdjudicacion.SIE_CREATEDOPOR), CreadoPor);

            InformacionAdjudicacion response = new InformacionAdjudicacion();
            response = await GetAsyncFirst<InformacionAdjudicacion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        public async Task<InformacionEscrituracion> InsertarSolicitudInformacionEscrituracion(InformacionEscrituracion informacionEscrituracion, Guid SOL_ID_FK, Guid CreadoPor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.SOL_ID_FK), SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.FechaEscrituracion), informacionEscrituracion.FechaEscrituracion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.Notaria), informacionEscrituracion.Notaria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.NumeroEscritura), informacionEscrituracion.NumeroEscritura);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.SIE_CREATEDOPOR), CreadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInsertarSolicitudInformacionEscrituracion.FechaEcrituraRegistro), informacionEscrituracion.FechaEscrituracionRegistro);

            InformacionEscrituracion response = new InformacionEscrituracion();
            response = await GetAsyncFirst<InformacionEscrituracion>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondo(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }
        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoArchivo(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }
        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoEntrevista(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }
        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionVerificationFondo(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }
        public async Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoCalificacion(Guid IdSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@USR_USERNAME", IdSolicitud);

            var a = await GetAsyncList<TareaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }

        public async Task<IEnumerable<ConsultarNovedad>> ConsultarNovedadSolicitud()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<ConsultarNovedad>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ConsultarTipoNovedad>> ConsultarTipoNovedadSolicitud(string nov_descripcion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NOV_DESCRIPCION", nov_descripcion);

            var a = await GetAsyncList<ConsultarTipoNovedad>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }

        public async Task<IEnumerable<ConsultarCausalInterrupcionSolicitud>> ConsultarCausalInterrupcionSolicitud(string nov_descripcion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NOV_DESCRIPCION", nov_descripcion);

            var a = await GetAsyncList<ConsultarCausalInterrupcionSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }

        public async Task<IEnumerable<ConsultarPagoSucesivoSolicitud>> ConsultarPagoSucesivoSolicitud(string nov_descripcion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NOV_DESCRIPCION", nov_descripcion);

            var a = await GetAsyncList<ConsultarPagoSucesivoSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }

        public async Task<IEnumerable<ConsultarInformeInconsistenciaSolicitud>> ConsultarInformeInconsistenciaSolicitud(string nov_descripcion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NOV_DESCRIPCION", nov_descripcion);

            var a = await GetAsyncList<ConsultarInformeInconsistenciaSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
            return a;
        }

        public async Task<InconsistenciaSeguridadSolicitud> InsertarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud insertarInconsistenciaSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Inconsistencia), insertarInconsistenciaSolicitud.INC_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Estado), insertarInconsistenciaSolicitud.INC_ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Grupo), insertarInconsistenciaSolicitud.INC_GRUPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Subsanado), insertarInconsistenciaSolicitud.INC_SUBSANADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudId), insertarInconsistenciaSolicitud.SOL_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.UserId), insertarInconsistenciaSolicitud.USR_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.TipoNovedadId), insertarInconsistenciaSolicitud.TPO_NOV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.CausalInterrupcionId), insertarInconsistenciaSolicitud.CAU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.InformeInconsistenciaId), insertarInconsistenciaSolicitud.INF_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Fecha_Inconsistencia), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudTareaId), insertarInconsistenciaSolicitud.SLT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.FECHA_REGISTRO), insertarInconsistenciaSolicitud.FECHA_REGISTRO);

            return await GetAsyncFirst<InconsistenciaSeguridadSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InconsistenciaSeguridadSolicitud>> ConsultarInconsistenciasSeguridadSolicitud(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudId), solicitudId);

            IEnumerable<InconsistenciaSeguridadSolicitud> response;
            response = await GetAsyncList<InconsistenciaSeguridadSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<InconsistenciaSeguridadSolicitud> ActualizarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud actualizarInconsistenciaSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Inconsistencia), actualizarInconsistenciaSolicitud.INC_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Subsanado), actualizarInconsistenciaSolicitud.INC_SUBSANADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.TipoNovedadId), actualizarInconsistenciaSolicitud.TPO_NOV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.CausalInterrupcionId), actualizarInconsistenciaSolicitud.CAU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.InformeInconsistenciaId), actualizarInconsistenciaSolicitud.INF_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.NovedadNuevaId), actualizarInconsistenciaSolicitud.TPO_NOV_ID_NUEVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.CausalNuevaId), actualizarInconsistenciaSolicitud.CAU_ID_NUEVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Informe114NuevaId), actualizarInconsistenciaSolicitud.INF_ID_NUEVO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Grupo), actualizarInconsistenciaSolicitud.INC_GRUPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Fecha_Subsanacion), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudTareaId), actualizarInconsistenciaSolicitud.SLT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudId), actualizarInconsistenciaSolicitud.SOL_ID_FK);


            return await GetAsyncFirst<InconsistenciaSeguridadSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<InconsistenciaSeguridadSolicitud> EliminarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud eliminarInconsistenciaSolicitud)
        {
            DynamicParameters parametros = new DynamicParameters();


            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Inconsistencia), eliminarInconsistenciaSolicitud.INC_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.TipoNovedadId), eliminarInconsistenciaSolicitud.TPO_NOV_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.CausalInterrupcionId), eliminarInconsistenciaSolicitud.CAU_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.InformeInconsistenciaId), Guid.Parse(eliminarInconsistenciaSolicitud.INF_ID));
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudId), eliminarInconsistenciaSolicitud.SOL_ID_FK);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.Grupo), eliminarInconsistenciaSolicitud.INC_GRUPO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInconsistenciaSeguridadSolicitudParameters.SolicitudTareaId), eliminarInconsistenciaSolicitud.SLT_ID);

            return await GetAsyncFirst<InconsistenciaSeguridadSolicitud>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptosAfectadosSolicitud>> ConsultarTipoConcepto(string tipoCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.TipoCuenta), tipoCuenta);

            IEnumerable<ConceptosAfectadosSolicitud> response;
            response = await GetAsyncList<ConceptosAfectadosSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<CuentasAfectadasSolicitud> InsertarConceptoAfectadoCliente(CuentasAfectadasSolicitud cuentasAfectadasSolicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.ConceptoID), cuentasAfectadasSolicitud.CTC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.ConceptoDescripcion), cuentasAfectadasSolicitud.CTC_DESCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.SolicitudID), cuentasAfectadasSolicitud.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.ClienteID), cuentasAfectadasSolicitud.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.IdentificacionCliente), cuentasAfectadasSolicitud.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.SaldoCuenta), cuentasAfectadasSolicitud.CAC_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.ValorRetirar), cuentasAfectadasSolicitud.CAC_VALOR_A_RETIRAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.ValorDescontadoConcepto), cuentasAfectadasSolicitud.CAC_VALOR_DESCONTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.NumeroCuenta), cuentasAfectadasSolicitud.NUMERO_CUENTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.TipoCuenta), cuentasAfectadasSolicitud.DESCRIPCION_CUENTA);


            CuentasAfectadasSolicitud response = new CuentasAfectadasSolicitud();
            response = await GetAsyncFirst<CuentasAfectadasSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<CuentasAfectadasSolicitud>> ObtenerConceptosAfectados(ObtenerConceptosAfectadosClientes cuentasAfectadas)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.SolicitudID), cuentasAfectadas.SolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.IdentificacionCliente), cuentasAfectadas.Identificacion);

            IEnumerable<CuentasAfectadasSolicitud> response;
            response = await GetAsyncList<CuentasAfectadasSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        public async Task<List<CuentasAfectadasSolicitud>> ObtenerConceptosAfectadosSolicitud(string cuentasAfectadas)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.IdentificacionCliente), cuentasAfectadas);
            List<CuentasAfectadasSolicitud> response;
            response = (List<CuentasAfectadasSolicitud>)await GetAsyncList<CuentasAfectadasSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<List<ReporteAfiliadosCesantiasItem>> GenerarReportePagoDeCesantias(ReporteAfiliadosCesantiasRequestDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CTA", request.TipoCuenta);
            parameters.Add("@FechaRadicacionInicial", request.FechaRadicacionInicial);
            parameters.Add("@FechaRadicacionFinal", request.FechaRadicacionFinal);

            return (await GetAsyncList<ReporteAfiliadosCesantiasItem>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure)).ToList();
        }

        public async Task<IEnumerable<CuentasAfectadasSolicitud>> ObtenerCuentasAfectadas(ObtenerConceptosAfectadosClientes cuentasAfectadas)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.SolicitudID), cuentasAfectadas.SolicitudId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptoSolicitud.IdentificacionCliente), cuentasAfectadas.Identificacion);

            IEnumerable<CuentasAfectadasSolicitud> response;
            response = await GetAsyncList<CuentasAfectadasSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<TiposDeCuentaSolicitud>> ConsultarTipoCuentas()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<TiposDeCuentaSolicitud>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ObtenerInformacionSolicitudCliente>> ObtenerInformacionSolicitudCliente(string cedula)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInformacionClienteSolicitud.CedulaCliente), cedula);

            return await GetAsyncList<ObtenerInformacionSolicitudCliente>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<SolicitudMatriculaInmobiliaria> DesbloqueoMatriculaSolicitud(Guid MatriculaId)
        {
            SolicitudMatriculaInmobiliaria solicitudMatricula = new SolicitudMatriculaInmobiliaria();
            solicitudMatricula.MAI_ID = MatriculaId;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultaMatriculaParameters.MaiId), solicitudMatricula.MAI_ID);

            SolicitudMatriculaInmobiliaria response = new SolicitudMatriculaInmobiliaria();
            response = await GetAsyncFirst<SolicitudMatriculaInmobiliaria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ConsultarSubmodalidadPrimerPago>> ConsultarSubmodalidadPrimerPago()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<ConsultarSubmodalidadPrimerPago>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<RespuestaConsultarRadicadoSolicitud> ConsultarRadicadoSolicitud(Guid SolicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.IdSolicitud), SolicitudId);


            RespuestaConsultarRadicadoSolicitud response;
            response = await GetAsyncFirst<RespuestaConsultarRadicadoSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }



        public async Task<RespuestaConsultarRadicadoSolicitud> InsertarDescripcionInconsistenciaSolicitud(RespuestaConsultarRadicadoSolicitud consultarRadicadoSolicitudDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.IdSolicitud), consultarRadicadoSolicitudDto.SOL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudParameters.Inc_descripcion), consultarRadicadoSolicitudDto.INC_DESCRIPCION);


            RespuestaConsultarRadicadoSolicitud response;
            response = await GetAsyncFirst<RespuestaConsultarRadicadoSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SubmodalidadSolicitud> ObtenerSubModalidadPrimerPago(Guid submodalidadId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudSubModalidadesParameters.SubmodalidadId), submodalidadId);

            SubmodalidadSolicitud response;
            response = await GetAsyncFirst<SubmodalidadSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        public async Task<bool> ActualizarAsignacionSubsidioSolicitud(string numeroMatricula)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudReglaAsignacionSubsidio.NumeroMatricula), numeroMatricula);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<UsuarioIngresoSolicitud>> ConsultarUsuarioIngresoSolicitud(RespuestaConsultarUsuarioIngresoSolicitud usuarioIngreso)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudBuscarUsuario.NombresUsuario), usuarioIngreso.Nombres_usuario);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudBuscarUsuario.ApellidoUsuario), usuarioIngreso.Apellidos_usuario);

            return await GetAsyncList<UsuarioIngresoSolicitud>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>> ConsultarSolicitanteApoderadoSolicitud(SolicitanteBeneficiarioTerceroSolicitudDto solicitante)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudBuscarSolicitante.NombresSolicitante), solicitante.Nombres_solicitante);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudBuscarSolicitante.ApellidSolicitante), solicitante.Apellidos_solicitante);

            return await GetAsyncList<SolicitanteBeneficiarioTerceroSolicitud>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<IntervinientesActor>> ObtenerIntervinienteActor()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<IntervinientesActor>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IntervinientesActorSolicitud> RegistrarIntervinientesActor(IntervinientesActorSolicitudDto intervinientesActor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoPersona), intervinientesActor.PersonaJuridicaNatural);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoDocumento), intervinientesActor.TipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumento), intervinientesActor.NumeroDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NombreRazonSocial), intervinientesActor.Nombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.SegundoNombre), intervinientesActor.SegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.PrimerApellido), intervinientesActor.PrimerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.SegundoApellido), intervinientesActor.SegundoApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Celular), intervinientesActor.Celular);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.CorreoElectronico), intervinientesActor.CorreoElectronico);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.DepartamentoId), intervinientesActor.Departamento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.CiudadId), intervinientesActor.Ciudad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Direccion), intervinientesActor.Direccion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Barrio), intervinientesActor.Barrio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Telefono), intervinientesActor.Telefono);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.ActorId), intervinientesActor.TipoActor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoCuenta), intervinientesActor.TipoCuenta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroCuenta), intervinientesActor.NumeroCuentaBancaria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.EntidadBancaria), intervinientesActor.EntidadBancaria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.FechaExpedicion), intervinientesActor.FechaExpedicionCedula);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Constructora), intervinientesActor.Constructora);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Convenio), intervinientesActor.ConvenioEntidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.SolId), intervinientesActor.SolId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoDocumentoAfiliado), intervinientesActor.TipoDocumentoAfiliado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumentoAfiliado), intervinientesActor.NumeroDocumentoAfiliado);


            IntervinientesActorSolicitud response;
            response = await GetAsyncFirst<IntervinientesActorSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;

        }

        public async Task<IntervinientesActorSolicitud> ObtenerIntervinientePorId(Guid idInterviniente)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.IdInterviniente), idInterviniente);

            IntervinientesActorSolicitud response;
            response = await GetAsyncFirst<IntervinientesActorSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<List<IntervinientesActorSolicitud>> ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(int tipoDocumento, string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoDocumentoAfiliado),
                tipoDocumento);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumentoAfiliado),
                numeroDocumento);

            var result = await GetAsyncList<IntervinientesActorSolicitud>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);

            return result?.ToList() ?? new List<IntervinientesActorSolicitud>();
        }
        public async Task<List<Tramite>> TramitesInterviniente(ConsultaTramitesIntervinientesActorSolicitud interviniente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoDocumento), interviniente.TipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumento), interviniente.NumeroDocumento);

            IEnumerable<Tramite> response;
            response = await GetAsyncList<Tramite>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return (List<Tramite>)response;
        }


        public async Task<IEnumerable<IntervinientesActorSolicitud>> BuscarIntervinientesActores(BuscarIntervinientesDto interviniente)
        {
            DynamicParameters parameters = new DynamicParameters();
            interviniente.ActorId = interviniente.ActorId == Guid.Empty ? null : interviniente.ActorId;
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.TipoDocumento), interviniente.TipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumento), interviniente.NumeroDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NombreRazonSocial), interviniente.Nombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.ActorId), interviniente.ActorId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Radicado), interviniente.Radicado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumSolicitud), interviniente.NumSolicitud);


            return await GetAsyncList<IntervinientesActorSolicitud>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IntervinientesActorSolicitud> ActualizarIntervinienteActor(ActualizarIntervinientesActorSolicitudDto intervinientes)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.IdInterviniente), intervinientes.IntervinientesId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NombreRazonSocial), intervinientes.Nombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.SegundoNombre), intervinientes.SegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.PrimerApellido), intervinientes.primerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.SegundoApellido), intervinientes.SegundoApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Celular), intervinientes.Celular);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.CorreoElectronico), intervinientes.CorreoElectronico);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.DepartamentoId), intervinientes.Departamento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.CiudadId), intervinientes.Ciudad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Direccion), intervinientes.Direccion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Barrio), intervinientes.Barrio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Telefono), intervinientes.Telefono);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.ModificadoPor), intervinientes.ModificadoPor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.Modulo), intervinientes.Modulo);

            IntervinientesActorSolicitud response;
            response = await GetAsyncFirst<IntervinientesActorSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ObtenerIntervinienteActorPorNumeroDocumento(string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.NumeroDocumento), numeroDocumento);

            bool response;
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }


        public async Task<IEnumerable<SolicitudTerceroApoderado>> ConsultarTercerosPorSolicitud(Guid solicitudTareaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroGeneral.SolicitudTareaId), solicitudTareaId);

            return await GetAsyncList<SolicitudTerceroApoderado>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<EstadosCatalogosValorSolicitudes> ConsultarEstadosPorDescripcion(string nombreEstado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudEstados.DescripcionEstado), nombreEstado);

            EstadosCatalogosValorSolicitudes response;
            response = await GetAsyncFirst<EstadosCatalogosValorSolicitudes>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudPropietario> ConsultarPropietariosPorSolicitud(Guid solicitudTareaId, string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroGeneral.SolicitudTareaId), solicitudTareaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.NumeroDocumento), numeroDocumento);


            return await GetAsyncFirst<SolicitudPropietario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<SolicitudPropietario> ActualizarPropietarioMatricula(SolicitudPropietario propietario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.NumeroDocumento), propietario.PRP_NUMERO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.SolicitudId), propietario.SOL_ID_FK);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.PrimerNombrePropietario), propietario.PrimerNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.PrimerApellidoPropietario), propietario.PrimerApellido);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.SegundoNombrePropietario), propietario.SegundoNombre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumPropietarioMatriculaSolicitud.SegundoApellidoPropietario), propietario.SegundoApellido);

            return await GetAsyncFirst<SolicitudPropietario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
        public async Task<IntervinientesActorSolicitud> EliminarIntervinienteActor(EliminarIntervinientesActorSolicitudDto intervinientes)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntervinientesActoresSolicitud.IdInterviniente), intervinientes.IntervinientesId);
            IntervinientesActorSolicitud response;
            response = await GetAsyncFirst<IntervinientesActorSolicitud>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> ActualizarSolicitudAcreditacion(ActualizacionSolicitudAcreditacion solicitud)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("ACUERDO", solicitud.Acuerdo);
            parameters.Add("ACCIONREALIZAR", solicitud.AccionRealizar);
            parameters.Add("CAUSAL", solicitud.Causal);
            parameters.Add("CUMPLEREQUISITO", solicitud.CumpleRequisito);
            parameters.Add("SOL_ID_FK", solicitud.SolicitudId);
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> GuardarMisCuentas(SolicitudCuenta solicitudCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                 HelpersEnums.GetEnumDescription(EnumSolicitudCuenta.SolicitudId), solicitudCuenta.SOL_ID);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumSolicitudCuenta.SaldoCuenta), solicitudCuenta.SALDO_CUENTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumSolicitudCuenta.Cuenta), solicitudCuenta.CUENTA);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumSolicitudCuenta.TrasladoAutorizado), solicitudCuenta.TRASLADO_AUTORIZADO);

            bool response = await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<SolicitudCuenta> ConsultarMisCuentas(Guid solicitudId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudCuenta.SolicitudId), solicitudId);

            SolicitudCuenta response = await GetAsyncFirst<SolicitudCuenta>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizarTerceroBeneficiario(
            Guid solId,
            Guid TER_ID,
            int tipoDoc,
            string numDoc,
            string razonSocial,
            int tipoCuenta,
            Guid entidad,
            string numeroCuenta,
            decimal valor)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.SolicitudId), solId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TER_ID), TER_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoIdentificacion), tipoDoc);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.NumeroDocumento), numDoc);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.RazonSocial), razonSocial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.TipoCuenta), tipoCuenta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.Entidad), entidad);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.NumeroCuenta), numeroCuenta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudTerceroBeneficiarioParameters.ValorRetirarDos), valor);

            bool response = await GetAsyncFirst<bool>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
    }
}
