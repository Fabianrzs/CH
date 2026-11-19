using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Integraciones;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.NewFolder;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class AuditoriaRepository : DapperGenericRepository, IAuditoriaRepository
    {
        public AuditoriaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Auditoria>> ObtenerlogAuditoria(ConsultaAuditoria parametros) 
        {
            string fechaInicio = null;
            if (parametros.AUR_FECHA_FILTRO_INICIO.HasValue)
            {
                fechaInicio = string.Format("{0}-{1}-{2} 00:00", parametros.AUR_FECHA_FILTRO_INICIO.Value.Year,
                    parametros.AUR_FECHA_FILTRO_INICIO.Value.Month < 10 ? "0" + parametros.AUR_FECHA_FILTRO_INICIO.Value.Month : parametros.AUR_FECHA_FILTRO_INICIO.Value.Month,
                    parametros.AUR_FECHA_FILTRO_INICIO.Value.Day);
                    //, parametros.AUR_FECHA_FILTRO_INICIO.Value.Hour, 
                    //parametros.AUR_FECHA_FILTRO_INICIO.Value.Minute < 10 ? "0"+ parametros.AUR_FECHA_FILTRO_INICIO.Value.Minute: parametros.AUR_FECHA_FILTRO_INICIO.Value.Minute);
                    
            }
            string fechaFin = null;
            if (parametros.AUR_FECHA_FILTRO_FIN.HasValue)
            {
                fechaFin = string.Format("{0}-{1}-{2} 23:59", parametros.AUR_FECHA_FILTRO_FIN.Value.Year,
                    parametros.AUR_FECHA_FILTRO_FIN.Value.Month < 10 ? "0" + parametros.AUR_FECHA_FILTRO_FIN.Value.Month : parametros.AUR_FECHA_FILTRO_FIN.Value.Month,
                    parametros.AUR_FECHA_FILTRO_FIN.Value.Day);
                   //, parametros.AUR_FECHA_FILTRO_FIN.Value.Hour, 
                   // parametros.AUR_FECHA_FILTRO_FIN.Value.Minute < 10 ? "0" + parametros.AUR_FECHA_FILTRO_FIN.Value.Minute: parametros.AUR_FECHA_FILTRO_FIN.Value.Minute);

            }
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID_TIPO_ACCION", parametros.ID_TIPO_ACCION);
            param.Add("@USUARIO", parametros.USUARIO);
            param.Add("@FECHA_INICIAL", fechaInicio);
            param.Add("@FECHA_FINAL", fechaFin);
            param.Add("@DESCRIPCION", parametros.DESCRIPCION);
            param.Add("@MODULO", parametros.MODULO);
            return await GetAsyncList<Auditoria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), param, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<TablasAuditoria>> ObtenerTablasAuditoria()
        {
            return await GetAsyncList<TablasAuditoria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),null, CommandType.StoredProcedure);
        }

        public async Task<int> ObtenerNumeroRegistrologAuditoria(ConsultaAuditoria parametros)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_USR_ID), parametros.AUR_USR_ID);
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_NOM_TABLE), parametros.AUR_NOM_TABLE);
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_VER_DETALLE), parametros.AUR_VER_DETALLE);
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_FECHA_FILTRO_FIN), parametros.AUR_FECHA_FILTRO_FIN);
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_FECHA_FILTRO_INICIO), parametros.AUR_FECHA_FILTRO_INICIO);
            param.Add(HelpersEnums.GetEnumDescription(EnumAuditoriaParameter.AUR_AGRUPADOR), parametros.AUR_AGRUPADOR);
  

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), param, CommandType.StoredProcedure);
        }


        public async Task<bool> InsertReportIntegracionesRest(ReportIntegracion parametros)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.IDINTEGRACIONES),
                parametros.ID_INTEGRACIONES);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.TIPOSERVICIO),
                parametros.TIPOSERVICIO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.METODOHTTP),
                parametros.METODOHTTP);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.ENDPOINT),
                parametros.ENDPOINT);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.REQUEST),
                parametros.REQUEST);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.RESPONSE),
                parametros.RESPONSE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.STATUSCODE),
                parametros.STATUSCODE);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.SUCCESS),
                parametros.SUCCESS);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.ERROR),
                parametros.ERROR);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.FECHAINICIO),
                parametros.FECHAINICIO);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.FECHAFIN),
                parametros.FECHAFIN);

            parameters.Add(
                HelpersEnums.GetEnumDescription(EnumIntegraciones.DURACIONMS),
                parametros.DURACIONMS);

            bool response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> InsertReportIntegraciones(ReportIntegracion parametros)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegraciones.IDINTEGRACIONES), parametros.ID_INTEGRACIONES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegraciones.REQUEST), parametros.REQUEST);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegraciones.TIPOSERVICIO), parametros.TIPOSERVICIO);


            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> UpdateReportIntegraciones(ReportIntegracion parametros)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegraciones.IDINTEGRACIONES), parametros.ID_INTEGRACIONES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIntegraciones.RESPONSE), parametros.RESPONSE);


            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<Auditoria> InsertarLogAuditoria(AuditoriaDto request)
        {            
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID_TIPO_ACCION", request.IdTipoAccion);
            param.Add("@USUARIO", request.NombreUsuario);
            param.Add("@TIPO_ACCION", request.TipoAccion);
            param.Add("@USR_ID", request.UsurioId);
            param.Add("@IP", request.Ip);
            param.Add("@NAVEGADOR", request.Navegador);
            param.Add("@SISTEMA_OPERATIVO", request.SistemaOperativo);
            param.Add("@DESCRIPCION", request.Descripcion);
            param.Add("@HOSTNAME", request.Hostname);
            param.Add("@MODULO", request.Modulo);
            param.Add("@FECHA",DateTime.Now);
            param.Add("@NOMBRE_COLUMNA", request.NombreColumna);
            param.Add("@VALOR_ANTERIOR", request.ValorAnterior);
            param.Add("@VALOR_NUEVO", request.ValorNuevo);
            return await GetAsyncFirst<Auditoria>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), param, CommandType.StoredProcedure);
        }
        public async Task<int>InsertarAuditoriaDetalle(AuditLog request)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@LAD_TRANSACION_ID", request.TranstationId);
            param.Add("@LAD_CAMPOS_ACTUALIZADOS", request.ChangedFields);
            param.Add("@LAD_USER_ID", request.UserName);
            param.Add("@LAD_FECHA_CREACION", request.Timestamp);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), param, CommandType.StoredProcedure);
        }
        public async Task<int> ObtenerTipoAcionAuditoria(int request)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TRANSACION_ID", request);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), param, CommandType.StoredProcedure);
        }


    }
}
