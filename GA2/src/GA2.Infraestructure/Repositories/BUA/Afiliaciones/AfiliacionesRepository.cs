using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.BUA;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class AfiliacionesRepository : DapperGenericRepository, IAfiliacionesRepository
    {
        public AfiliacionesRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Afiliacion> ActualizarAfiliacion(Afiliacion consultaAfiliacionesRequestDTO)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ID), consultaAfiliacionesRequestDTO.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), consultaAfiliacionesRequestDTO.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPF_ID), consultaAfiliacionesRequestDTO.TPF_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_RADICADO), consultaAfiliacionesRequestDTO.AFL_NUMERO_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_RADICADO), consultaAfiliacionesRequestDTO.AFL_FECHA_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_ELABORACION), consultaAfiliacionesRequestDTO.AFL_FECHA_ELABORACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLD_ID), consultaAfiliacionesRequestDTO.CLD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPP_ID), consultaAfiliacionesRequestDTO.TPP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_TRAMITE_AFILIACION), consultaAfiliacionesRequestDTO.AFL_TRAMITE_AFILIACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CONSERVA_ANTIGUEDAD), consultaAfiliacionesRequestDTO.AFL_CONSERVA_ANTIGUEDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_RATIFICACION_CUOTAS), consultaAfiliacionesRequestDTO.AFL_RATIFICACION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_OBSERVACIONES_JUSTIFICACION), consultaAfiliacionesRequestDTO.AFL_OBSERVACIONES_JUSTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION), consultaAfiliacionesRequestDTO.AFL_FECHA_NOTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CAMPO_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_CAMPO_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ACTIVACION_DESCUENTO), consultaAfiliacionesRequestDTO.AFL_ACTIVACION_DESCUENTO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD), consultaAfiliacionesRequestDTO.AFL_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_REINTEGRO_APORTES), consultaAfiliacionesRequestDTO.AFL_REINTEGRO_APORTES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PERIODO_APORTES_DESDE), consultaAfiliacionesRequestDTO.AFL_PERIODO_APORTES_DESDE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PERIODO_APORTES_HASTA), consultaAfiliacionesRequestDTO.AFL_PERIODO_APORTES_HASTA);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_DERECHO_LIQUIDACION_CUOTAS), consultaAfiliacionesRequestDTO.AFL_DERECHO_LIQUIDACION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_OBSERVACIONES_CONCEPTUALIZACION), consultaAfiliacionesRequestDTO.AFL_OBSERVACIONES_CONCEPTUALIZACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.PRC_ID), consultaAfiliacionesRequestDTO.PRC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.MDL_ID), consultaAfiliacionesRequestDTO.MDL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_AHORRO_CONSIGNACION_BANCARIA), consultaAfiliacionesRequestDTO.AFL_AHORRO_CONSIGNACION_BANCARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_SOLICITUD_TRASLADO_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_SOLICITUD_TRASLADO_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CONSERVA_ANTIGUEDAD_DEPENDIENTE), consultaAfiliacionesRequestDTO.AFL_CONSERVA_ANTIGUEDAD_DEPENDIENTE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL), consultaAfiliacionesRequestDTO.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_DERECHO_LIQUIDACION_CUOTAS_DEPENDIENTE), consultaAfiliacionesRequestDTO.AFL_DERECHO_LIQUIDACION_CUOTAS_DEPENDIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPC_ID), consultaAfiliacionesRequestDTO.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPD_ID), consultaAfiliacionesRequestDTO.DPD_ID);
            //INFORMACION CLIENTES
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_ALTA), consultaAfiliacionesRequestDTO.CLI_FECHA_ALTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTG_ID), consultaAfiliacionesRequestDTO.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_NACIMIENTO), consultaAfiliacionesRequestDTO.CLI_FECHA_NACIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), consultaAfiliacionesRequestDTO.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), consultaAfiliacionesRequestDTO.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CIE_INGRESO_MENSUAL), consultaAfiliacionesRequestDTO.CIE_INGRESO_MENSUAL);



            return await GetAsyncFirst<Afiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AportesCategoriaAfiliaciones> ActualizarAportes(AportesCategoriaAfiliaciones aporte)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), aporte.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CTG_ID), aporte.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APC_CUOTAS_ACUMULADAS), aporte.APC_CUOTAS_ACUMULADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APC_FECHA_ULTIMA_CUOTA), aporte.APC_FECHA_ULTIMA_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APC_FECHA_PRIMERA_CUOTA), aporte.APC_FECHA_PRIMERA_CUOTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_PERIODO_DESDE), aporte.APF_PERIODO_DESDE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_PERIODO_HASTA), aporte.APF_PERIODO_HASTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_RATIFICACION_CUOTAS), aporte.APF_RATIFICACION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD), aporte.APF_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_REINTEGRO_APORTES), aporte.APF_REINTEGRO_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_DERECHO_LIQUIDAICION_CUOTAS), aporte.APF_DERECHO_LIQUIDAICION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.APF_VALOR_DERECHO_LIQUIDACION_CUOTAS), aporte.APF_VALOR_DERECHO_LIQUIDACION_CUOTAS);

            return await GetAsyncFirst<AportesCategoriaAfiliaciones>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<BeneficiariosAfiliaciones> ActualizarBeneficiario(BeneficiariosAfiliaciones beneficiario)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPT_ID), beneficiario.DPT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), beneficiario.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TID_ID), beneficiario.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPT_IDENTIFICACION), beneficiario.DPT_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPT_NOMBRE), beneficiario.DPT_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPT_PARTICIPACION), beneficiario.DPT_PARTICIPACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_ID), beneficiario.DPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_DIRECCION), beneficiario.DPA_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_CORREO), beneficiario.DPA_CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_TELEFONO), beneficiario.DPA_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_CONSERVA_ANTIGUEDAD), beneficiario.DPA_CONSERVA_ANTIGUEDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL), beneficiario.DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PERSONAL), beneficiario.DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PERSONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES), beneficiario.DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), beneficiario.DPA_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), beneficiario.DPA_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS), beneficiario.DPA_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_DERECHO_LIQUIDACION_CUOTAS_BENEFICIARIOS), beneficiario.DPA_DERECHO_LIQUIDACION_CUOTAS_BENEFICIARIOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO), beneficiario.DPA_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO);

            return await GetAsyncFirst<BeneficiariosAfiliaciones>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Afiliacion> EliminarAfiliacion(int request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ID), request);

            return await GetAsyncFirst<Afiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Afiliacion> InsertarAfiliacion(Afiliacion consultaAfiliacionesRequestDTO)
        {
            DateTime defaultDate = DateTime.Parse("01-01-1900");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), consultaAfiliacionesRequestDTO.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPF_ID), consultaAfiliacionesRequestDTO.TPF_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_RADICADO), consultaAfiliacionesRequestDTO.AFL_NUMERO_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_RADICADO), consultaAfiliacionesRequestDTO.AFL_FECHA_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_ELABORACION), consultaAfiliacionesRequestDTO.AFL_FECHA_ELABORACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLD_ID), consultaAfiliacionesRequestDTO.CLD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPP_ID), consultaAfiliacionesRequestDTO.TPP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_TRAMITE_AFILIACION), consultaAfiliacionesRequestDTO.AFL_TRAMITE_AFILIACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CONSERVA_ANTIGUEDAD), consultaAfiliacionesRequestDTO.AFL_CONSERVA_ANTIGUEDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_RATIFICACION_CUOTAS), consultaAfiliacionesRequestDTO.AFL_RATIFICACION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_OBSERVACIONES_JUSTIFICACION), consultaAfiliacionesRequestDTO.AFL_OBSERVACIONES_JUSTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION), consultaAfiliacionesRequestDTO.AFL_FECHA_NOTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CAMPO_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_CAMPO_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ACTIVACION_DESCUENTO), consultaAfiliacionesRequestDTO.AFL_ACTIVACION_DESCUENTO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD), consultaAfiliacionesRequestDTO.AFL_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_REINTEGRO_APORTES), consultaAfiliacionesRequestDTO.AFL_REINTEGRO_APORTES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PERIODO_APORTES_DESDE), consultaAfiliacionesRequestDTO.AFL_PERIODO_APORTES_DESDE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PERIODO_APORTES_HASTA), consultaAfiliacionesRequestDTO.AFL_PERIODO_APORTES_HASTA);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_DERECHO_LIQUIDACION_CUOTAS), consultaAfiliacionesRequestDTO.AFL_DERECHO_LIQUIDACION_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_OBSERVACIONES_CONCEPTUALIZACION), consultaAfiliacionesRequestDTO.AFL_OBSERVACIONES_CONCEPTUALIZACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.PRC_ID), consultaAfiliacionesRequestDTO.PRC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.MDL_ID), consultaAfiliacionesRequestDTO.MDL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_AHORRO_CONSIGNACION_BANCARIA), consultaAfiliacionesRequestDTO.AFL_AHORRO_CONSIGNACION_BANCARIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_SOLICITUD_TRASLADO_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_SOLICITUD_TRASLADO_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_CONSERVA_ANTIGUEDAD_DEPENDIENTE), consultaAfiliacionesRequestDTO.AFL_CONSERVA_ANTIGUEDAD_DEPENDIENTE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL), consultaAfiliacionesRequestDTO.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), consultaAfiliacionesRequestDTO.AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_DERECHO_LIQUIDACION_CUOTAS_DEPENDIENTE), consultaAfiliacionesRequestDTO.AFL_DERECHO_LIQUIDACION_CUOTAS_DEPENDIENTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPC_ID), consultaAfiliacionesRequestDTO.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.DPD_ID), consultaAfiliacionesRequestDTO.DPD_ID);
            //INFORMACION CLIENTES
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_ALTA), consultaAfiliacionesRequestDTO.CLI_FECHA_ALTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTG_ID), consultaAfiliacionesRequestDTO.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_NACIMIENTO), consultaAfiliacionesRequestDTO.CLI_FECHA_NACIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), consultaAfiliacionesRequestDTO.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), consultaAfiliacionesRequestDTO.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CIE_INGRESO_MENSUAL), consultaAfiliacionesRequestDTO.CIE_INGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_OFICIO_SOLICITUD_CESANTIAS), consultaAfiliacionesRequestDTO.AFL_OFICIO_SOLICITUD_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_MES_INICIO_ACTIVACION_DESCUENTO), consultaAfiliacionesRequestDTO.AFL_MES_INICIO_ACTIVACION_DESCUENTO.Year < 1900 ? defaultDate : consultaAfiliacionesRequestDTO.AFL_MES_INICIO_ACTIVACION_DESCUENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_MES_FIN_ACTIVACION_DESCUENTO), consultaAfiliacionesRequestDTO.AFL_MES_FIN_ACTIVACION_DESCUENTO.Year < 1900 ? defaultDate : consultaAfiliacionesRequestDTO.AFL_MES_FIN_ACTIVACION_DESCUENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_AHORRO_CONSIGNACION_BANCARIA_OPCION), consultaAfiliacionesRequestDTO.AFL_AHORRO_CONSIGNACION_BANCARIA_OPCION);

            return await GetAsyncFirst<Afiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<AfiliacionDto> InsertarAfiliacionGestorDocumental(InsertarAfiliacionGestorDocumental consultaAfiliacionesRequestDTO)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), consultaAfiliacionesRequestDTO.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPF_ID), consultaAfiliacionesRequestDTO.TipoAfiliacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_NUMERO_RADICADO), consultaAfiliacionesRequestDTO.NumeroRadicado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_RADICADO), DateTime.Now);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_ELABORACION), DateTime.Now);

            return await GetAsyncFirst<AfiliacionDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Afiliacion>> ObtenerAfiliacionByIdentificacion(ConsultaAfiliacion consultaAfiliacionesRequestDTO)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TID_ID), consultaAfiliacionesRequestDTO.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_IDENTIFICACION), consultaAfiliacionesRequestDTO.CLI_IDENTIFICACION);

            return await GetAsyncList<Afiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Afiliacion>> ObtenerAfiliacionesRangoFiltros(ObtenerAfiliacionesRangoFiltros consultaAfiliacionesRequestDTO)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLD_ID), consultaAfiliacionesRequestDTO.CLD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TPF_ID), consultaAfiliacionesRequestDTO.TPF_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_RADICADO), consultaAfiliacionesRequestDTO.AFL_NUMERO_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_ELABORACION), consultaAfiliacionesRequestDTO.AFL_FECHA_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_FECHA_ELABORACION), consultaAfiliacionesRequestDTO.AFL_FECHA_ELABORACION);

            return await GetAsyncList<Afiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AportesCategoriaAfiliaciones>> ObtenerAportesAfiliacion(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), idCliente);

            return await GetAsyncList<AportesCategoriaAfiliaciones>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BeneficiariosAfiliaciones>> ObtenerBeneficiariosAfiliacion(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_ID), idCliente);

            return await GetAsyncList<BeneficiariosAfiliaciones>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionCliente>> ReporteInformacionCliente(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_IDENTIFICACION), identificacion);

            return await GetAsyncList<InformacionCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ReporteInformacionClienteDto> ReporteObtenerInformacionCliente(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_IDENTIFICACION), identificacion);

            return await GetAsyncFirst<ReporteInformacionClienteDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TransferenciaCesantias>> ObtenerDatosOficioRespuesta(int idAfiliacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.ID_AFILIACION), idAfiliacion);

            return await GetAsyncList<TransferenciaCesantias>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OficioImprocedente>> ObtenerDatosOficioImprocedente(int idAfiliacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.ID_AFILIACION), idAfiliacion);

            return await GetAsyncList<OficioImprocedente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TramiteDesafiliacion>> ActualizarTramiteDesafiliacion(TramiteDesafiliacion tramiteDesafiliacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TMD_ID), tramiteDesafiliacion.TMD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ID), tramiteDesafiliacion.AFL_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.PCS_ID), tramiteDesafiliacion.PCS_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TIM_ID), tramiteDesafiliacion.TIM_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.TMD_VALOR), tramiteDesafiliacion.TMD_VALOR );

            return await GetAsyncList<TramiteDesafiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TramiteDesafiliacion>> ObtenerTramitesDesafiliacion(int idAfiliacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.AFL_ID), idAfiliacion);

            return await GetAsyncList<TramiteDesafiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OficiosAfiliacion>> GenerarReportesAfiliacion(ParametrosReportesAfiliaciones parametros)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.ID_AFILIACION), parametros.IdAfiliacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.OPCION_REPORTE), parametros.OpcionReporte);

            return await GetAsyncList<OficiosAfiliacion>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        
        public async Task<Afiliado> ObtenerNombresCliente(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAfiliacionesParameters.CLI_IDENTIFICACION), identificacion);

            return await GetAsyncFirst<Afiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<List<Afiliado>> ObtenerNombresClientesPorDocumentos(List<string> documentos)
        {
            DynamicParameters parameters = new DynamicParameters();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("DOCUMENTOS", typeof(string));
            foreach (string documento in documentos)
            {
                dataTable.Rows.Add(documento);
            }

            parameters.Add("@DOCUMENTOSCLIENTES", dataTable.AsTableValuedParameter("[DocumentosClientes]"));

            var responseSP = await GetAsyncList<Afiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return responseSP.ToList();
        }

        public async Task<IEnumerable<ReporteClienteCorreosErrados>> ObtenerClientesCorreoErrados()
        {
            return await GetAsyncList<ReporteClienteCorreosErrados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
    }
}
