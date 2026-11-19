using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Parametrization;
using GA2.Infraestructure.Data.Verificacion;
using GA2.Transversals.Commons;
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
    public class VerificacionDerechosRepository : DapperGenericRepository, IVerificacionDerechosRepository
    {
        public VerificacionDerechosRepository(IConfiguration configuration) : base(configuration)
        {
        }



        public async Task<IEnumerable<ParametrosVerificacionDerecho>> ObtenerTodoVerificacionDerecho()
        {
            return await GetAsyncList<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionDerechoCuentas()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InfoVerificacionDerecho>> ObtenerTodoVerificacionDerechoInfoById(int PARAM_VD_ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoInfo.PARAM_VD_ID), PARAM_VD_ID);


            return await GetAsyncList<InfoVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerVerificacionDerechoResultById(int ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoInfo.PARAM_VD_ID), ID);



            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<ParametrosVerificacionDerecho> CrearPeriodoVerificacionDerecho(ParametrosVerificacionDerecho parametroVerificacionDerecho)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ID), parametroVerificacionDerecho.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.NUMERO_SOLICITUD), parametroVerificacionDerecho.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ANIO), parametroVerificacionDerecho.ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.MES), parametroVerificacionDerecho.MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.FECHA_OPERACION), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.FECHA_APROBACION), parametroVerificacionDerecho.FECHA_APROBACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.TOTAL_CUENTAS_VERIFICADAS), parametroVerificacionDerecho.TOTAL_CUENTAS_VERIFICADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.TOTAL_CAMBIOS_ESTADO), parametroVerificacionDerecho.TOTAL_CAMBIOS_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.USUARIO_ID), parametroVerificacionDerecho.USUARIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.USUARIO_NOMBRE), parametroVerificacionDerecho.USUARIO_NOMBRE);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ESTADO), parametroVerificacionDerecho.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.NOMBRE_ESTADO), parametroVerificacionDerecho.NOMBRE_ESTADO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.SLT_ID), parametroVerificacionDerecho.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.SLT_ID_APROBAR), parametroVerificacionDerecho.SLT_ID_APROBAR);

            return await GetAsyncFirst<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResultVerificacionDerecho> CrearVerificacionDerechoResult(ResultVerificacionDerecho resultVerificacionDerecho)
        {
            DateTime fechaMinimaSql = new DateTime(1753, 1, 1);
            DateTime fechaMaximaSql = new DateTime(9999, 12, 31);

            // Si la fecha es menor que la mínima permitida, se reemplaza con la mínima permitida
            if (resultVerificacionDerecho.FECHA_ULTIMO_MOVIMIENTOS_CREDITO > fechaMinimaSql && resultVerificacionDerecho.CTA_FECHA_PRIMER_APORTE > fechaMinimaSql
                && resultVerificacionDerecho.FECHA_ULTIMO_MOVIMIENTOS_CREDITO < fechaMaximaSql && resultVerificacionDerecho.CTA_FECHA_PRIMER_APORTE < fechaMaximaSql)
            {

                DynamicParameters parameters = new DynamicParameters();
                try
                {
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ID), resultVerificacionDerecho.ID);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID), resultVerificacionDerecho.CTA_ID);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_NUMERO), resultVerificacionDerecho.CTA_NUMERO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FECHA_ULTIMO_MOVIMIENTOS_CREDITO), resultVerificacionDerecho.FECHA_ULTIMO_MOVIMIENTOS_CREDITO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_FECHA_PRIMER_APORTE), resultVerificacionDerecho.CTA_FECHA_PRIMER_APORTE);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_CUOTAS), resultVerificacionDerecho.CTA_CUOTAS);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CUOTAS_PENDIENTES), resultVerificacionDerecho.CUOTAS_PENDIENTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_INICIAL), resultVerificacionDerecho.ECT_INICIAL);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_FINAL), resultVerificacionDerecho.ECT_FINAL);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CAMBIO_ESTADO), resultVerificacionDerecho.CAMBIO_ESTADO);


                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO), resultVerificacionDerecho.PERIODO_ANIO);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_MES), resultVerificacionDerecho.PERIODO_MES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FLAG), resultVerificacionDerecho.FLAG);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), resultVerificacionDerecho.IDENTIFICACION);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), resultVerificacionDerecho.CTA_SALDO);


                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANUAL_APORTES), resultVerificacionDerecho.SALDO_ANUAL_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANUAL_CESANTIAS), resultVerificacionDerecho.SALDO_ANUAL_CESANTIAS);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.MOVIMIENTOS_MENSUAL_APORTES), resultVerificacionDerecho.MOVIMIENTOS_MENSUAL_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.MOVIMIENTOS_MENSUAL_CESANTIAS), resultVerificacionDerecho.MOVIMIENTOS_MENSUAL_CESANTIAS);


                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANTERIOR_APORTES), resultVerificacionDerecho.SALDO_ANTERIOR_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANTERIOR_CESANTIAS), resultVerificacionDerecho.SALDO_ANTERIOR_CESANTIAS);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_FINAL_APORTES), resultVerificacionDerecho.SALDO_FINAL_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_FINAL_CESANTIAS), resultVerificacionDerecho.SALDO_FINAL_CESANTIAS);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.UNIDAD_EJECUTORA), resultVerificacionDerecho.UNIDAD_EJECUTORA);

                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.NUMERO_SOLICITUD), resultVerificacionDerecho.NUMERO_SOLICITUD);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.TIPO_SOLICITUD), resultVerificacionDerecho.TIPO_SOLICITUD);

                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERES_APORTES), resultVerificacionDerecho.INTERES_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERES_CESANTIAS), resultVerificacionDerecho.INTERES_CESANTIAS);

                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES), resultVerificacionDerecho.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES);
                    parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS), resultVerificacionDerecho.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS);

                    var ver = parameters;

                    return await GetAsyncFirst<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

                }
                catch (Exception E)
                {

                    throw;
                }
            }
            return null;

        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerVerificacionDerechoResultByPeriodo(int periodoAnio, int PeriodoMes)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO), periodoAnio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_MES), PeriodoMes);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoDatosVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ParametrosVerificacionDerecho> ObtenerTodoVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncFirst<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ParametrosVerificacionDerecho>> ObtenerTodoVerificacionDerechosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.DESDE_ANIO), ID.DesdeAnio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.DESDE_MES), ID.DesdeMes);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.HASTA_ANIO), ID.HastaAnio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.HASTA_MES), ID.HastaMes);

            return await GetAsyncList<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ParametrosVerificacionDerecho> ActualizarParametrosVerificacionDerechos(ParametrosVerificacionDerecho parametroVerificacionDrecho)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ID), parametroVerificacionDrecho.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.FECHA_APROBACION), parametroVerificacionDrecho.FECHA_APROBACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.APROBADO), parametroVerificacionDrecho.APROBADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.USUARIO_ID), parametroVerificacionDrecho.USUARIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.USUARIO_NOMBRE), parametroVerificacionDrecho.USUARIO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.NUMERO_RADICADO), parametroVerificacionDrecho.NUMERO_RADICADO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ESTADO), parametroVerificacionDrecho.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.NOMBRE_ESTADO), parametroVerificacionDrecho.NOMBRE_ESTADO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.SLT_ID_APROBAR), parametroVerificacionDrecho.SLT_ID_APROBAR);

            var param = parameters;

            return await GetAsyncFirst<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IPC> ObtenerIPCByPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIPCParameters.ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumIPCParameters.MES), ID.Mes);

            return await GetAsyncFirst<IPC>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResultVerificacionDerecho> ActualizarVerificacionDerechosResult(ResultVerificacionDerecho resultVerificacionDerechoDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ID), resultVerificacionDerechoDto.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANTERIOR_APORTES), resultVerificacionDerechoDto.SALDO_ANTERIOR_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_ANTERIOR_CESANTIAS), resultVerificacionDerechoDto.SALDO_ANTERIOR_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERESES_CAUSADO_APORTES), resultVerificacionDerechoDto.INTERESES_CAUSADO_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERESES_CAUSADO_CESANTIAS), resultVerificacionDerechoDto.INTERESES_CAUSADO_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERESES_NO_CAUSADO_APORTES), resultVerificacionDerechoDto.INTERESES_NO_CAUSADO_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.INTERESES_NO_CAUSADO_CESANTIAS), resultVerificacionDerechoDto.INTERESES_NO_CAUSADO_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_FINAL_APORTES), resultVerificacionDerechoDto.SALDO_FINAL_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.SALDO_FINAL_CESANTIAS), resultVerificacionDerechoDto.SALDO_FINAL_CESANTIAS);




            return await GetAsyncFirst<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResultVerificacionDerecho> CrearVerificacionDerechoResultBatch(List<ResultVerificacionDerecho> batchEntities)
        {
            DynamicParameters parameters = new DynamicParameters();

            var dataTable = new DataTable();

            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ID), typeof(long));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_NUMERO), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CLI_ID), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FECHA_ULTIMO_MOVIMIENTOS_CREDITO), typeof(DateTime));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_FECHA_PRIMER_APORTE), typeof(DateTime));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_CUOTAS), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CUOTAS_PENDIENTES), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_INICIAL), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_FINAL), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CAMBIO_ESTADO), typeof(string));


            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_MES), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID_INTEGRACION), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.TCT_ID), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_FECHA_CREACION), typeof(DateTime));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CANTIDAD_MOVIMIENTOS_CREDITO), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ULTIMO_ANIO_PERIODO), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ULTIMO_MES_PERIODO), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.AHORROS_OBLIGATORIOS), typeof(int));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FLAG), typeof(string));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), typeof(string));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));

            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));
            dataTable.Columns.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO), typeof(decimal));

            // Agrega las filas al DataTable
            foreach (var entity in batchEntities)
            {
                var row = dataTable.NewRow();
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ID)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_NUMERO)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CLI_ID)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FECHA_ULTIMO_MOVIMIENTOS_CREDITO)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_FECHA_PRIMER_APORTE)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_CUOTAS)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CUOTAS_PENDIENTES)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_INICIAL)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ECT_FINAL)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_MES)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID_INTEGRACION)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.TCT_ID)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_FECHA_CREACION)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CANTIDAD_MOVIMIENTOS_CREDITO)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ULTIMO_ANIO_PERIODO)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.ULTIMO_MES_PERIODO)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.AHORROS_OBLIGATORIOS)] = entity.ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.FLAG)] = entity.CTA_ID;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION)] = entity.CTA_NUMERO;
                row[HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_SALDO)] = entity.CTA_NUMERO;



                dataTable.Rows.Add(row);
            }



            return await GetAsyncFirst<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionDerechosResultByAnioCuenta(int CTA_CUENTA, int ANIO)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO), ANIO);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<ResultVerificacionDerecho> ObtenerValorMesAnteriorCuenta(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.PERIODO_MES), MES);

            return await GetAsyncFirst<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosCuentasAdministracionCesantiasResult> CrearDatosCuentasAdministracionCesantiasResult(DatosCuentasAdministracionCesantiasResult datosCuentasAdministracionCesantiasResultDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.ID), datosCuentasAdministracionCesantiasResultDto.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.CTA_ID), datosCuentasAdministracionCesantiasResultDto.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.CTA_NUMERO), datosCuentasAdministracionCesantiasResultDto.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.CLI_ID), datosCuentasAdministracionCesantiasResultDto.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.TCT_ID), datosCuentasAdministracionCesantiasResultDto.TCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.CTA_SALDO), datosCuentasAdministracionCesantiasResultDto.CTA_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.FECHA_ULTIMO_MOVIMIENTOS_CREDITO), datosCuentasAdministracionCesantiasResultDto.FECHA_ULTIMO_MOVIMIENTOS_CREDITO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.IDENTIFICACION), datosCuentasAdministracionCesantiasResultDto.IDENTIFICACION);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.PERIODO_ANIO), datosCuentasAdministracionCesantiasResultDto.PERIODO_ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.PERIODO_MES), datosCuentasAdministracionCesantiasResultDto.PERIODO_MES);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.SALDO_ANUAL_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.SALDO_ANUAL_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.MOVIMIENTOS_MENSUAL_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.MOVIMIENTOS_MENSUAL_CESANTIAS);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.INTERESES_NO_CAUSADO_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.INTERESES_NO_CAUSADO_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.INTERESES_CAUSADO_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.INTERESES_CAUSADO_CESANTIAS);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.SALDO_ANTERIOR_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.SALDO_ANTERIOR_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.SALDO_FINAL_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.SALDO_FINAL_CESANTIAS);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.UNIDAD_EJECUTORA), datosCuentasAdministracionCesantiasResultDto.UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.NUMERO_SOLICITUD), datosCuentasAdministracionCesantiasResultDto.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.TIPO_SOLICITUD), datosCuentasAdministracionCesantiasResultDto.TIPO_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.INTERES_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.INTERES_CESANTIAS);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS), datosCuentasAdministracionCesantiasResultDto.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS);

            return await GetAsyncFirst<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }



        public async Task<DatosCuentasAdministracionCesantiasResult> ObtenerValorMesAnteriorCuentaCesantias(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.PERIODO_MES), MES);

            return await GetAsyncFirst<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<DatosCuentasAVResult> CrearDatosCuentasAVResult(DatosCuentasAVResult datosCuentasAVResult)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.ID), datosCuentasAVResult.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.CTA_ID), datosCuentasAVResult.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.CTA_NUMERO), datosCuentasAVResult.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.PERIODO_ANIO), datosCuentasAVResult.PERIODO_ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.PERIODO_MES), datosCuentasAVResult.PERIODO_MES);


            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.VALOR_AVAC), datosCuentasAVResult.VALOR_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANUAL_AVAC), datosCuentasAVResult.SALDO_ANUAL_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.MOVIMIENTOS_MENSUAL_AVAC), datosCuentasAVResult.MOVIMIENTOS_MENSUAL_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_NO_CAUSADO_AVAC), datosCuentasAVResult.INTERESES_NO_CAUSADO_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_CAUSADO_AVAC), datosCuentasAVResult.INTERESES_CAUSADO_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANTERIOR_AVAC), datosCuentasAVResult.SALDO_ANTERIOR_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_FINAL_AVAC), datosCuentasAVResult.SALDO_FINAL_AVAC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.VALOR_AVAV), datosCuentasAVResult.VALOR_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANUAL_AVAV), datosCuentasAVResult.SALDO_ANUAL_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.MOVIMIENTOS_MENSUAL_AVAV), datosCuentasAVResult.MOVIMIENTOS_MENSUAL_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_NO_CAUSADO_AVAV), datosCuentasAVResult.INTERESES_NO_CAUSADO_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_CAUSADO_AVAV), datosCuentasAVResult.INTERESES_CAUSADO_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANTERIOR_AVAV), datosCuentasAVResult.SALDO_ANTERIOR_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_FINAL_AVAV), datosCuentasAVResult.SALDO_FINAL_AVAV);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.VALOR_AVC), datosCuentasAVResult.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANUAL_AVC), datosCuentasAVResult.SALDO_ANUAL_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.MOVIMIENTOS_MENSUAL_AVC), datosCuentasAVResult.MOVIMIENTOS_MENSUAL_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_NO_CAUSADO_AVC), datosCuentasAVResult.INTERESES_NO_CAUSADO_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.INTERESES_CAUSADO_AVC), datosCuentasAVResult.INTERESES_CAUSADO_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_ANTERIOR_AVC), datosCuentasAVResult.SALDO_ANTERIOR_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.SALDO_FINAL_AVC), datosCuentasAVResult.SALDO_FINAL_AVC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.IDENTIFICACION), datosCuentasAVResult.IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.UNIDAD_EJECUTORA), datosCuentasAVResult.UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.NUMERO_SOLICITUD), datosCuentasAVResult.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.TIPO_SOLICITUD), datosCuentasAVResult.TIPO_SOLICITUD);






            return await GetAsyncFirst<DatosCuentasAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<DatosCuentasAVResult> ObtenerValorMesAnteriorDatosCuentasAVResult(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.PERIODO_MES), MES);

            return await GetAsyncFirst<DatosCuentasAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<ParametrosAbonoInteres> CrearPeriodoValoresInteresAbonados(ParametrosAbonoInteres parametrosAbonoInteres)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ID), parametrosAbonoInteres.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.NUMERO_SOLICITUD), parametrosAbonoInteres.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ANIO), parametrosAbonoInteres.ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.MES), parametrosAbonoInteres.MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.TOTAL_INTERESES_ABONADOS), parametrosAbonoInteres.TOTAL_INTERESES_ABONADOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_ID), null);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_NOMBRE), null);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.FECHA_APROBACION), parametrosAbonoInteres.FECHA_APROBACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.FECHA_RECEPCION), parametrosAbonoInteres.FECHA_RECEPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.VALOR_IPC), parametrosAbonoInteres.VALOR_IPC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.PORCENTAJE_IPC), parametrosAbonoInteres.PORCENTAJE_IPC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_SOLUCION_VIVIENDA_APORTES), parametrosAbonoInteres.COMPROBANTE_SOLUCION_VIVIENDA_APORTES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS), parametrosAbonoInteres.COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS), parametrosAbonoInteres.COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_AVAC), parametrosAbonoInteres.COMPROBANTE_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_AVAV), parametrosAbonoInteres.COMPROBANTE_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.COMPROBANTE_AVC), parametrosAbonoInteres.COMPROBANTE_AVC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ESTADO), parametrosAbonoInteres.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.NOMBRE_ESTADO), parametrosAbonoInteres.NOMBRE_ESTADO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.SLT_ID), parametrosAbonoInteres.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.SLT_ID_APROBAR), parametrosAbonoInteres.SLT_ID_APROBAR);

            return await GetAsyncFirst<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParametrosAbonoInteres>> ObtenerTodoValoresInteresAbonados()
        {
            return await GetAsyncList<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ParametrosAbonoInteres>> ObtenerTodoValoresInteresAbonadossFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.DESDE_ANIO), ID.DesdeAnio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.DESDE_MES), ID.DesdeMes);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.HASTA_ANIO), ID.HastaAnio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultFlitroByPeriodo.HASTA_MES), ID.HastaMes);

            return await GetAsyncList<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }




        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public async Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoDatosCuentasAVResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<DatosCuentasAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            var datos = await GetAsyncList<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return await GetAsyncList<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task EliminarParamValoresInteresAbonados(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);


            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                   HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


        }

        public async Task EliminarParamVerificacionDerechosResult(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);


            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                   HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


        }

        public async Task EliminarVerificacionCuentasAdministracionCesantias(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);


            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                   HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


        }

        public async Task EliminarVerificacionCuentasAV(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);


            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                   HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


        }

        public async Task EliminarVerificacionDerechosSolucionVivienda(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);


            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                   HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


        }

        public async Task<ParametrosVerificacionDerechoDto> ObtenerTodoVerificacionDerechoByPeriodo(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), verificacionDerechosPeriodo.Mes);

            return await GetAsyncFirst<ParametrosVerificacionDerechoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
        public async Task<ParametrosVerificacionDerechoDto> ObtenerTodoVerificacionDerechoBySolId(Guid? id)
        {
            DynamicParameters parameters = new();
            parameters.Add("@Sol_Id", id);

            return await GetAsyncFirst<ParametrosVerificacionDerechoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }
        public async Task<ParametrosVerificacionDerechoDto> ObtenerAbonoInteresBySolId(Guid? id)
        {
            DynamicParameters parameters = new();
            parameters.Add("@Sol_Id", id);

            return await GetAsyncFirst<ParametrosVerificacionDerechoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<ParametrosVerificacionDerecho> ActualizarPeriodoVerificacionDerecho(ParametrosVerificacionDerecho parametroVerificacionDrecho)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.ANIO), parametroVerificacionDrecho.ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.MES), parametroVerificacionDrecho.MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.TOTAL_CUENTAS_VERIFICADAS), parametroVerificacionDrecho.TOTAL_CUENTAS_VERIFICADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoParameters.TOTAL_CAMBIOS_ESTADO), parametroVerificacionDrecho.TOTAL_CAMBIOS_ESTADO);

            return await GetAsyncFirst<ParametrosVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }



        public async Task<ParametrosAbonoInteres> ActualizarParametrosInteresAbonadosTotal(ParametrosAbonoInteres parametrosAbonoInteres)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ANIO), parametrosAbonoInteres.ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.MES), parametrosAbonoInteres.MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.TOTAL_INTERESES_ABONADOS), parametrosAbonoInteres.TOTAL_INTERESES_ABONADOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.NUMERO_RADICADO), parametrosAbonoInteres.NUMERO_RADICADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.SLT_ID), parametrosAbonoInteres.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.SLT_ID_APROBAR), parametrosAbonoInteres.SLT_ID_APROBAR);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_ID), parametrosAbonoInteres.USUARIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_NOMBRE), parametrosAbonoInteres.USUARIO_NOMBRE);

            return await GetAsyncFirst<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParametrosAbonoInteres> ObtenerTodoValoresInteresAbonadosByPeriodo(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), verificacionDerechosPeriodo.Mes);

            var result = await GetAsyncFirst<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }




        public async Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoVerificacionAdministracionCesantiasByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);


            return await GetAsyncList<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoVerificacionCuentasAVByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);

            return await GetAsyncList<DatosCuentasAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoVerificacionCuentasAVACByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);

            return await GetAsyncList<ObtenerCuentasAVACResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoVerificacionCuentasAVAVByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);

            return await GetAsyncList<ObtenerCuentasAVAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoVerificacionCuentasAVCByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);

            return await GetAsyncList<ObtenerCuentasAVCResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoVerificacionSolucionViviendaByAnio(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), verificacionDerechosPeriodo.Anio);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<SolicitudConsecutivo>> ObtenerTodoSolicitudConsecutivo()
        {
            return await GetAsyncList<SolicitudConsecutivo>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }


        public async Task<SolicitudConsecutivo> ActualizarSolicitudConsecutivo(SolicitudConsecutivoDto solicitudConsecutivoDto)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudConsecutivo.CNS_ID), solicitudConsecutivoDto.CnsId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumSolicitudConsecutivo.CNS_ULTIMO_NUMERO), solicitudConsecutivoDto.CnsUltimoNumero);

            return await GetAsyncFirst<SolicitudConsecutivo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<DatosCuentasAVResult>> ObtenerTodoDatosCuentasAVResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<DatosCuentasAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ResultVerificacionDerecho>> ObtenerTodoCuentasSolucionViviendaResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAV.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<ResultVerificacionDerecho>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<DatosCuentasAdministracionCesantiasResult>> ObtenerTodoCuentasAdministracionCesantiasResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasCesantiasResult.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<DatosCuentasAdministracionCesantiasResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        public async Task<ParametrosAbonoInteres> ActualizarParametrosInteresAbonadosMovimientos(ParametrosAbonoInteres parametrosAbonoInteres)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ANIO), parametrosAbonoInteres.ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.MES), parametrosAbonoInteres.MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ID), parametrosAbonoInteres.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.FECHA_APROBACION), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.APROBADO), parametrosAbonoInteres.APROBADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_ID), parametrosAbonoInteres.USUARIO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.USUARIO_NOMBRE), parametrosAbonoInteres.USUARIO_NOMBRE);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.ESTADO), parametrosAbonoInteres.ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.NOMBRE_ESTADO), parametrosAbonoInteres.NOMBRE_ESTADO);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionAbonoIntereses.SLT_ID_APROBAR), parametrosAbonoInteres.SLT_ID_APROBAR);

            return await GetAsyncFirst<ParametrosAbonoInteres>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }





        public async Task<ObtenerCuentasAVACResult> InsertarDatosCuentasAVAC(ObtenerCuentasAVACResult datosCuentasAVACResult)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.ID), datosCuentasAVACResult.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.CTA_ID), datosCuentasAVACResult.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.CTA_NUMERO), datosCuentasAVACResult.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.ECT_ID), datosCuentasAVACResult.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.PERIODO_ANIO), datosCuentasAVACResult.PERIODO_ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.PERIODO_MES), datosCuentasAVACResult.PERIODO_MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.CTA_SALDO), datosCuentasAVACResult.CTA_SALDO);


            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.VALOR_AVAC), datosCuentasAVACResult.VALOR_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.SALDO_ANUAL_AVAC), datosCuentasAVACResult.SALDO_ANUAL_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.MOVIMIENTOS_MENSUAL_AVAC), datosCuentasAVACResult.MOVIMIENTOS_MENSUAL_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.INTERESES_AVAC), datosCuentasAVACResult.INTERES_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.INTERESES_NO_CAUSADO_AVAC), datosCuentasAVACResult.INTERESES_NO_CAUSADO_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.INTERESES_CAUSADO_AVAC), datosCuentasAVACResult.INTERESES_CAUSADO_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.SALDO_ANTERIOR_AVAC), datosCuentasAVACResult.SALDO_ANTERIOR_AVAC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.SALDO_FINAL_AVAC), datosCuentasAVACResult.SALDO_FINAL_AVAC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.CLI_ID), datosCuentasAVACResult.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.IDENTIFICACION), datosCuentasAVACResult.IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.UNIDAD_EJECUTORA), datosCuentasAVACResult.UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.NUMERO_SOLICITUD), datosCuentasAVACResult.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.TIPO_SOLICITUD), datosCuentasAVACResult.TIPO_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC), datosCuentasAVACResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC);


            return await GetAsyncFirst<ObtenerCuentasAVACResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ObtenerCuentasAVAVResult> InsertarDatosCuentasAVAV(ObtenerCuentasAVAVResult datosCuentasAVAVResult)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.ID), datosCuentasAVAVResult.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.CTA_ID), datosCuentasAVAVResult.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.CTA_NUMERO), datosCuentasAVAVResult.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.ECT_ID), datosCuentasAVAVResult.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.PERIODO_ANIO), datosCuentasAVAVResult.PERIODO_ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.PERIODO_MES), datosCuentasAVAVResult.PERIODO_MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.CTA_SALDO), datosCuentasAVAVResult.CTA_SALDO);


            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.VALOR_AVAV), datosCuentasAVAVResult.VALOR_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.SALDO_ANUAL_AVAV), datosCuentasAVAVResult.SALDO_ANUAL_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.MOVIMIENTOS_MENSUAL_AVAV), datosCuentasAVAVResult.MOVIMIENTOS_MENSUAL_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.INTERESES_AVAV), datosCuentasAVAVResult.INTERES_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.INTERESES_NO_CAUSADO_AVAV), datosCuentasAVAVResult.INTERESES_NO_CAUSADO_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.INTERESES_CAUSADO_AVAV), datosCuentasAVAVResult.INTERESES_CAUSADO_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.SALDO_ANTERIOR_AVAV), datosCuentasAVAVResult.SALDO_ANTERIOR_AVAV);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.SALDO_FINAL_AVAV), datosCuentasAVAVResult.SALDO_FINAL_AVAV);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.CLI_ID), datosCuentasAVAVResult.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.IDENTIFICACION), datosCuentasAVAVResult.IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.UNIDAD_EJECUTORA), datosCuentasAVAVResult.UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.NUMERO_SOLICITUD), datosCuentasAVAVResult.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.TIPO_SOLICITUD), datosCuentasAVAVResult.TIPO_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV), datosCuentasAVAVResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV);


            return await GetAsyncFirst<ObtenerCuentasAVAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ObtenerCuentasAVCResult> InsertarDatosCuentasAVC(ObtenerCuentasAVCResult datosCuentasAVCResult)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.ID), datosCuentasAVCResult.ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.CTA_ID), datosCuentasAVCResult.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.CTA_NUMERO), datosCuentasAVCResult.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.ECT_ID), datosCuentasAVCResult.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.PERIODO_ANIO), datosCuentasAVCResult.PERIODO_ANIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.PERIODO_MES), datosCuentasAVCResult.PERIODO_MES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.CTA_SALDO), datosCuentasAVCResult.CTA_SALDO);


            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.VALOR_AVC), datosCuentasAVCResult.VALOR_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.SALDO_ANUAL_AVC), datosCuentasAVCResult.SALDO_ANUAL_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.MOVIMIENTOS_MENSUAL_AVC), datosCuentasAVCResult.MOVIMIENTOS_MENSUAL_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.INTERESES_AVC), datosCuentasAVCResult.INTERES_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.INTERESES_NO_CAUSADO_AVC), datosCuentasAVCResult.INTERESES_NO_CAUSADO_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.INTERESES_CAUSADO_AVC), datosCuentasAVCResult.INTERESES_CAUSADO_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.SALDO_ANTERIOR_AVC), datosCuentasAVCResult.SALDO_ANTERIOR_AVC);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.SALDO_FINAL_AVC), datosCuentasAVCResult.SALDO_FINAL_AVC);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.CLI_ID), datosCuentasAVCResult.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.IDENTIFICACION), datosCuentasAVCResult.IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.UNIDAD_EJECUTORA), datosCuentasAVCResult.UNIDAD_EJECUTORA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.NUMERO_SOLICITUD), datosCuentasAVCResult.NUMERO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.TIPO_SOLICITUD), datosCuentasAVCResult.TIPO_SOLICITUD);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC), datosCuentasAVCResult.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC);


            return await GetAsyncFirst<ObtenerCuentasAVCResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ObtenerCuentasAVACResult> ObtenerValorMesAnteriorDatosCuentasAVACResult(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAC.PERIODO_MES), MES);

            return await GetAsyncFirst<ObtenerCuentasAVACResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ObtenerCuentasAVAVResult> ObtenerValorMesAnteriorDatosCuentasAVAVResult(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVAV.PERIODO_MES), MES);

            return await GetAsyncFirst<ObtenerCuentasAVAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ObtenerCuentasAVCResult> ObtenerValorMesAnteriorDatosCuentasAVCResult(int CTA_CUENTA, int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.CTA_ID), CTA_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.PERIODO_ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionCuentasAVC.PERIODO_MES), MES);

            return await GetAsyncFirst<ObtenerCuentasAVCResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoDatosCuentasAVACResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<ObtenerCuentasAVACResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<ObtenerCuentasAVAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoDatosCuentasAVCResultFilterPeriodo(VerificacionDerechosPeriodo ID)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_ANIO), ID.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResultByPeriodo.PERIODO_MES), ID.Mes);

            return await GetAsyncList<ObtenerCuentasAVCResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVACResult>> ObtenerTodoDatosCuentasAVACResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<ObtenerCuentasAVACResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVAVResult>> ObtenerTodoDatosCuentasAVAVResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<ObtenerCuentasAVAVResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVCResult>> ObtenerTodoDatosCuentasAVCResultFilterIdentificacion(int IDENTIFICACION)
        {
            DynamicParameters parameters = new();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoResult.IDENTIFICACION), IDENTIFICACION);

            return await GetAsyncList<ObtenerCuentasAVCResult>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NotificacionTarea> ObtenerUsuarioVerificacionDerechos(string CTA_CUENTA)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.CodigoPerfil), CTA_CUENTA);

            return await GetAsyncFirst<NotificacionTarea>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<NotificacionTarea> InsertarNotificacionTarea(NotificacionTarea notificacionTarea)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.Usuario), notificacionTarea.Usuario);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.Notificacion), notificacionTarea.Notificacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.NumRadicado), notificacionTarea.NumRadicado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.Modulo), notificacionTarea.Modulo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.NUM_CEDULA), notificacionTarea.NUM_CEDULA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.TareaSiguiente), notificacionTarea.TareaSiguiente);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.UsuarioEnvia), notificacionTarea.UsuarioEnvia);


            return await GetAsyncFirst<NotificacionTarea>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<VerificacionDerechoSolicitudTarea> CrearVerificacionDerechoSolicitudTarea(VerificacionDerechoSolicitudTarea verificacionDerechoSolicitudTarea)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.SLT_ID), verificacionDerechoSolicitudTarea.SLT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.TRA_ID), verificacionDerechoSolicitudTarea.TRA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.SOL_ID), verificacionDerechoSolicitudTarea.SOL_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.USR_ID), verificacionDerechoSolicitudTarea.USR_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.OBSERVACION), verificacionDerechoSolicitudTarea.OBSERVACION);


            return await GetAsyncFirst<VerificacionDerechoSolicitudTarea>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task<VerificacionDerechoSolicitudTarea> ActualizarSolicitudTareaVerificacionDerechos(ActualizarSolicitudTarea actualizarSolicitudTarea)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.SLT_ID), actualizarSolicitudTarea.SLT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.SLT_ESTADO_SOLICITUD), actualizarSolicitudTarea.SLT_ESTADO_SOLICITUD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.OBSERVACION), actualizarSolicitudTarea.SLT_OBSERVACION);

            return await GetAsyncFirst<VerificacionDerechoSolicitudTarea>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ParamValoresIPCNotificacionAutomatica> ObtenerValoresIPCNotificacionAutomatica(int ANIO, int MES)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamValoresIPCNotificacionAutomatica.ANIO), ANIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumParamValoresIPCNotificacionAutomatica.MES), MES);

            return await GetAsyncFirst<ParamValoresIPCNotificacionAutomatica>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public async Task ActualizarNotificacionTareaVerificacionYAbonos(Guid tareaSiguiente, string link)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.TareaSiguiente), tareaSiguiente);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumObtenerUsuarioVerificacionDerechos.Link), link);

            var response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }


        public static DataTable CrearDataTableCuentas(List<CuentaTemp> cuentas)
        {
            var table = new DataTable();

            table.Columns.Add("TTC_CtaId", typeof(int));
            table.Columns.Add("TTC_CtaNumero", typeof(int));
            table.Columns.Add("TTC_CliId", typeof(int));
            table.Columns.Add("TTC_TctId", typeof(int));
            table.Columns.Add("TTC_EctId", typeof(int));
            table.Columns.Add("TTC_CtaFechaCreacion", typeof(DateTime));
            table.Columns.Add("TTC_CtaSaldo", typeof(decimal));
            table.Columns.Add("TTC_AVAC", typeof(DateTime));
            table.Columns.Add("TTC_ValorAVAC", typeof(decimal));
            table.Columns.Add("TTC_SaldoAnualAVAC", typeof(decimal));
            table.Columns.Add("TTC_MovimientosMensualAVAC", typeof(decimal));
            table.Columns.Add("TTC_AVAV", typeof(DateTime));
            table.Columns.Add("TTC_ValorAVAV", typeof(decimal));
            table.Columns.Add("TTC_SaldoAnualAVAV", typeof(decimal));
            table.Columns.Add("TTC_MovimientosMensualAVAV", typeof(decimal));
            table.Columns.Add("TTC_AVC", typeof(DateTime));
            table.Columns.Add("TTC_ValorAVC", typeof(decimal));
            table.Columns.Add("TTC_SaldoAnualAVC", typeof(decimal));
            table.Columns.Add("TTC_MovimientosMensualAVC", typeof(decimal));
            table.Columns.Add("TTC_Identificacion", typeof(string));
            table.Columns.Add("TTC_UnidadEjecutora", typeof(int));

            foreach (var cuenta in cuentas)
            {
                table.Rows.Add(
                    cuenta.CtaId,
                    cuenta.CtaNumero,
                    cuenta.CliId,
                    cuenta.TctId,
                    cuenta.EctId,
                    cuenta.CtaFechaCreacion,
                    cuenta.CtaSaldo ?? (object)DBNull.Value,
                    cuenta.AVAC ?? (object)DBNull.Value,
                    cuenta.ValorAVAC ?? (object)DBNull.Value,
                    cuenta.SaldoAnualAVAC ?? (object)DBNull.Value,
                    cuenta.MovimientosMensualAVAC ?? (object)DBNull.Value,
                    cuenta.AVAV ?? (object)DBNull.Value,
                    cuenta.ValorAVAV ?? (object)DBNull.Value,
                    cuenta.SaldoAnualAVAV ?? (object)DBNull.Value,
                    cuenta.MovimientosMensualAVAV ?? (object)DBNull.Value,
                    cuenta.AVC ?? (object)DBNull.Value,
                    cuenta.ValorAVC ?? (object)DBNull.Value,
                    cuenta.SaldoAnualAVC ?? (object)DBNull.Value,
                    cuenta.MovimientosMensualAVC ?? (object)DBNull.Value,
                    cuenta.Identificacion,
                    cuenta.UnidadEjecutora
                );
            }

            return table;
        }
        public async Task<int> ICT_InsertarCuentaTemporal(List<CuentaTemp> cuentas)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableCuentas(cuentas);
            parametros.Add(name: "@Cuentas", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<int> Sp_CrearDatosCuentasAVResult(IPC request, long consecutivo)
        {
            DynamicParameters parametros = new();
            parametros.Add("@Anio", request.ANIO);
            parametros.Add("@Mes", request.MES);
            parametros.Add("@ValorIPC", request.VALOR_IPC);
            parametros.Add("@PorcentajeIPC", request.PORCENTAJE_IPC);
            parametros.Add("@Consecutivo", consecutivo);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }

        public async Task<int> InsertarDatosCedulasCesantias(List<DatosCedulasCesantias> request,double ipc,int mes,int anio)
        {
            DynamicParameters parametros = new();
            var data = CreateTableAdmiCesantias(request, ipc,mes,anio);
            parametros.Add(name: "@DatosCedulas", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public static DataTable CreateTableAdmiCesantias(List<DatosCedulasCesantias> cuentas, double ipc, int mes, int anio)
        {
            var table = new DataTable();

            table.Columns.Add("DTC_Identificacion", typeof(string));
            table.Columns.Add("DTC_IdCliente", typeof(int));
            table.Columns.Add("DTC_CuentaNumero", typeof(int));
            table.Columns.Add("DTC_ConceptoId", typeof(int));
            table.Columns.Add("DTC_CuentaId", typeof(int));
            table.Columns.Add("DTC_ValorIPC", typeof(double));
            table.Columns.Add("DTC_Mes", typeof(int));
            table.Columns.Add("DTC_Anio", typeof(int));

            foreach (var cuenta in cuentas)
            {
                table.Rows.Add(
                    cuenta.Identificacion ?? (object)DBNull.Value,
                    cuenta.IdCliente,
                    cuenta.CuentaNumero,
                    cuenta.ConceptoId,
                    cuenta.CuentaId,
                    ipc, 
                    mes,
                    anio
                );
            }

            return table;
        }

        public async Task<List<DatosCuentasAVDto>> ICT_ConsultarCuentasTemporales()
        {

            var response = await GetAsyncList<DatosCuentasAVDto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response.ToList();
        }
        public async Task<int> InsertarDatosInfoCesantia(DatosInfoCensatiaByCuentaPerioso request,int cuentaId)
        {
            DynamicParameters parametros = new();
            parametros.Add("@TIC_CuentaId",cuentaId);
            parametros.Add("TIC_ValorIngresos",request.ValorIngresos); 
            parametros.Add("@TIC_ValorEgresos",request.ValorIngresos);
            parametros.Add("@TIC_SaldoCuenta",request.SaldoCuenta);
            parametros.Add("@TIC_SaldoDisponible",request.SaldoDisponible);
            parametros.Add("@TIC_SaldoAnual",request.SaldoAnual);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<int> InsertarDatosVerificacionDerechoCuentaTemp(List<DatosVerificacionDerechoCuenta> request)
        {
            DynamicParameters parametros = new();
            var data = PrepararDatosAdministracionVivienda(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<int> Insertar_ObtenerCuentasAVAC(List<ObtenerCuentasAVAC> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableAVAC(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return response;
        }
        public async Task<int> InsertarTempCliente(ClienteDto request)
        {
            DynamicParameters parametros = new();
            parametros.Add("TC_IdCliente", request.IdCliente);
            parametros.Add("TC_CLV_ID", request.CLV_ID);
            parametros.Add("TC_IdTipoActor", request.IdTipoActor);
            parametros.Add("TC_IdTipoIdentificacion", request.IdTipoIdentificacion);
            parametros.Add("TC_Identificacion", request.Identificacion);
            parametros.Add("TC_FechaExpedicionDocumento", DateTime.Now);
            parametros.Add("TC_IdPaisExpedicionDocumento", request.IdPaisExpedicionDocumento);
            parametros.Add("TC_IdDepartamentoExpedicionDocumento", request.IdDepartamentoExpedicionDocumento);
            parametros.Add("TC_IdCiudadExpedicionDocumento", request.IdCiudadExpedicionDocumento);
            parametros.Add("TC_LugarExpedicionExtranjero", request.LugarExpedicionExtranjero);
            parametros.Add("TC_CodigoMilitar", request.CodigoMilitar);
            parametros.Add("TC_SituacionActual", request.SituacionActual);
            parametros.Add("TC_TipoVinculacion", request.TipoVinculacion);
            parametros.Add("TC_PrimerNombre", request.PrimerNombre);
            parametros.Add("TC_SegundoNombre", request.SegundoNombre);
            parametros.Add("TC_PrimerApellido", request.PrimerApellido);
            parametros.Add("TC_SegundoApellido", request.SegundoApellido);
            parametros.Add("TC_FechaNacimiento", request.FechaNacimiento  = request.FechaNacimiento == DateTime.MinValue ? DateTime.Now : request.FechaNacimiento);
            parametros.Add("TC_IdPaisNacimiento", request.IdPaisNacimiento);
            parametros.Add("TC_IdDepartamentoNacimiento", request.IdDepartamentoNacimiento);
            parametros.Add("TC_IdCiudadNacimiento", request.IdCiudadNacimiento);
            parametros.Add("TC_LugarNacimientoExtranjero", request.LugarNacimientoExtranjero);
            parametros.Add("TC_Sexo", request.Sexo);
            parametros.Add("TC_EstadoCivil", request.EstadoCivil);
            parametros.Add("TC_IdEstadoCivil", request.IdEstadoCivil);
            parametros.Add("TC_IdProfesion", request.IdProfesion);
            parametros.Add("TC_Unidad", request.Unidad);
            parametros.Add("TC_IdCategoria", request.IdCategoria);
            parametros.Add("TC_NombreTipoIdentificacion", request.NombreTipoIdentificacion);
            parametros.Add("TC_IdFuerza", request.IdFuerza);
            parametros.Add("TC_IdGrado", request.IdGrado);
            parametros.Add("TC_IdUnidadEjecutora", request.IdUnidadEjecutora);
            parametros.Add("TC_FechaActualizaDatos", DateTime.Now);
            parametros.Add("TC_FechaActualizaDatosFinancieros", DateTime.Now);
            parametros.Add("TC_FechaActualizaDatosContacto", DateTime.Now);
            parametros.Add("TC_FechaInscripcion", request.FechaInscripcion);
            parametros.Add("TC_FechaAlta", request.FechaAlta = request.FechaAlta = request.FechaAlta == DateTime.MinValue ? DateTime.Now : request.FechaAlta);
            parametros.Add("TC_FechaPrimeraCuota", request.FechaPrimeraCuota = request.FechaPrimeraCuota == DateTime.MinValue ? DateTime.Now : request.FechaPrimeraCuota);
            parametros.Add("TC_FechaUltimaCuota", request.FechaUltimaCuota = request.FechaUltimaCuota == DateTime.MinValue ? DateTime.Now : request.FechaUltimaCuota);
            parametros.Add("TC_Regimen", request.Regimen);
            parametros.Add("TC_FechaAltaCalculada", request.FechaAltaCalculada = request.FechaAltaCalculada == DateTime.MinValue || request.FechaAltaCalculada == null ? DateTime.Now : request.FechaAltaCalculada);
            parametros.Add("TC_FechaReintegro", DateTime.Now);
            parametros.Add("TC_EstadoCliente", request.EstadoCliente);
            parametros.Add("TC_IdEstadoCliente", request.IdEstadoCliente);
            parametros.Add("TC_NumeroValidacion", request.NumeroValidacion);
            parametros.Add("TC_IdPatrocinador", request.IdPatrocinador);
            parametros.Add("TC_Participacion", request.Participacion);
            parametros.Add("TC_SaldoDisponible", request.SaldoDisponible);
            parametros.Add("TC_NumeroResolucion", request.NumeroResolucion);
            parametros.Add("TC_ValidacionBiometrica", request.ValidacionBiometrica);
            parametros.Add("TC_EsBeneficiario", request.EsBeneficiario);
            parametros.Add("TC_EstadoCargueNomina", request.EstadoCargueNomina);
            parametros.Add("TC_CuentasHabilitadas", request.CuentasHabilitadas);
            parametros.Add("TC_TasaAportes", request.TasaAportes);
            parametros.Add("TC_TasaVoluntariaAportes", request.TasaVoluntariaAportes);
            parametros.Add("TC_TasaSalarioBase", request.TasaSalarioBase);
            parametros.Add("TC_NumeroCuotas", request.NumeroCuotas);
            parametros.Add("TC_Payload", request.Payload);
            parametros.Add("TC_ValorSubsidio", request.ValorSubsidio);
            parametros.Add("TC_Actualiza", request.Actualiza);
            parametros.Add("TC_Usuario", request.Usuario);


            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<int> InsertarTempCuentaCliente(List<CuentaDto> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableCuentaCliente(request);
            parametros.Add(name: "@DatosCuenta", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

         
        }
        public async Task<int> InsertarTempMovimiento(List<MovimientoDTO> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableMovimiento(request);
            parametros.Add(name: "@DatosMovimiento", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<int> InsertarDatosConcetoCuenta(List<ConceptoCuentaDto> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableConceptoCuenta(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

          
        }
        public async Task<int> InsertarDatosMovimientoConcetoCuenta(List<MovimientosCuentaDto> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableMovimientoConceptoCuenta(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e )
            {

                throw;
            }

        }
        public async Task<int> SP_TOCAVAV_InsertObtenerCuentaAVAV(List<ObtenerCuentaAVAVResponse> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableObtenerCuentasAVAV(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<int> InsertarTOCAVC_Temp_ObtenerCuentaAVC(List<ObtenerCuentaAVAVResponse> request)
        {
            DynamicParameters parametros = new();
            var data = CrearDataTableObtenerCuentaAVC(request);
            parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

            try
            {
                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<List<DatosCedulasCesantias>> ConsultarDatosCedulasCesantias(int mes, int anio)
        {
            DynamicParameters parametros = new();
            parametros.Add("@Mes", mes);
            parametros.Add("@Anio", anio);

            try
            {
                var response = await GetAsyncList<DatosCedulasCesantias>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<DatosInfoCensatiaByCuentaPerioso> ConsultarDatosInfoCesantia(ObtenerInfoCedulaRequestDto request)
        {
            DynamicParameters parametros = new();
            parametros.Add("@CuentaId", request.CuentaId);

            try
            {
                var response = await GetAsyncFirst<DatosInfoCensatiaByCuentaPerioso>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<ObtenerCuentasAVAC>> ConsultarCuentasAVAC()
        {

            try
            {
                var response = await GetAsyncList<ObtenerCuentasAVAC>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<ClienteDto> ConsultarInformacionCliente(string identificacion)
        {

            try
            {

                DynamicParameters parametros = new();
                parametros.Add("@Identificacion", identificacion);


                var response = await GetAsyncFirst<ClienteDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<CuentaDto>> ConsultarInformacioCuentaCliente(int clientId)
        {

            try
            {
                DynamicParameters parametros = new();
                parametros.Add("@IdCliente", clientId);

                var response = await GetAsyncList<CuentaDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<ConceptoCuentaDto>> ConsultarInformacioConceptoCuentaCliente(int cuentaId)
        {

            try
            {
                DynamicParameters parametros = new();
                parametros.Add("@CuentaId", cuentaId);

                var response = await GetAsyncList<ConceptoCuentaDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<MovimientoDTO>> ConsultarInformacioMovimientoCuentaCliente(int cuentaId)
        {

            try
            {
                DynamicParameters parametros = new();
                parametros.Add("@CuentaId", cuentaId);

                var response = await GetAsyncList<MovimientoDTO>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<ObtenerCuentaAVAVResponseDto>> ConsultarDaraCuentaAVAV()
        {

            try
            {

                var response = await GetAsyncList<ObtenerCuentaAVAVResponseDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<ObtenerCuentaAVAVResponseDto>> ConsultarCuentaAVC()
            {

            try
            {
                

                var response = await GetAsyncList<ObtenerCuentaAVAVResponseDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
                return response.ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<int> InsertarDatosVerificacionDerechoCuenta(List<DatosVerificacionDerechoCuentaDto> request)
        {

            try
            {
                DynamicParameters parametros = new();
                var data = CrearDataTableSolucionVivienda(request);
                parametros.Add(name: "@Datos", value: data, dbType: DbType.Object, direction: ParameterDirection.Input);

                var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO),parametros, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<List<DatosVerificacionDerechoCuentaDto>> ConsultaDatosVerificacionDerechoCuenta()
        {

            try
            {
                
                var response = await GetAsyncList<DatosVerificacionDerechoCuentaDto>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
                return response.ToList();

            }
            catch (Exception e)
            {

                throw;
            }
        }

        #region preparacion de tipo de tablas
        public static DataTable PrepararDatosAdministracionVivienda(List<DatosVerificacionDerechoCuenta> request)
        {
            // Crear el DataTable
            var table = new DataTable();
            table.Columns.Add("DVT_CtaId", typeof(int));
            table.Columns.Add("DVT_CtaIdIntegracion", typeof(int));
            table.Columns.Add("DVT_CtaNumero", typeof(int));
            table.Columns.Add("DVT_CliId", typeof(int));
            table.Columns.Add("DVT_TctId", typeof(int));
            table.Columns.Add("DVT_EctId", typeof(int));
            table.Columns.Add("DVT_CtaFechaCreacion", typeof(DateTime));
            table.Columns.Add("DVT_CtaFechaPrimerAporte", typeof(DateTime));
            table.Columns.Add("DVT_CtaCuotas", typeof(int));
            table.Columns.Add("DVT_CtaSaldo", typeof(decimal));
            table.Columns.Add("DVT_CuotasEsperadas", typeof(int));
            table.Columns.Add("DVT_CantidadMovimientoCredito", typeof(int));
            table.Columns.Add("DVT_FechaUltimoMovimientoAporte", typeof(DateTime));
            table.Columns.Add("DVT_UltimoAnioPeriodo", typeof(int));
            table.Columns.Add("DVT_UltimoMesPeriodo", typeof(int));
            table.Columns.Add("DVT_AhorrosObligarotios", typeof(int));
            table.Columns.Add("DVT_Identificacion", typeof(string));
            table.Columns.Add("DVT_Flag", typeof(string));
            table.Columns.Add("DVT_SaldoAnualAportes", typeof(decimal));
            table.Columns.Add("DVT_SaldoAnualCesantias", typeof(decimal));
            table.Columns.Add("DVT_MovimientosMensualAportes", typeof(decimal));
            table.Columns.Add("DVT_MovimientosMensualCesantias", typeof(decimal));
            table.Columns.Add("DVT_UnidadEjecutora", typeof(int));
            table.Columns.Add("DVT_InteresesAportes", typeof(decimal));
            table.Columns.Add("DVT_InteresesCesantias", typeof(decimal));
            table.Columns.Add("DVT_TotalCuentas", typeof(int));

            // Llenar el DataTable
            foreach (var item in request)
            {
                table.Rows.Add(
                    item.CTA_ID,
                    item.CTA_ID_INTEGRACION,
                    item.CTA_NUMERO,
                    item.CLI_ID,
                    item.TCT_ID,
                    item.ECT_ID,
                    item.CTA_FECHA_CREACION,
                    item.CTA_FECHA_PRIMER_APORTE,
                    item.CTA_CUOTAS,
                    item.CTA_SALDO,
                    item.CUOTAS_ESPERADAS,
                    item.CANTIDAD_MOVIMIENTOS_CREDITO,
                    item.FECHA_ULTIMO_MOVIMIENTOS_CREDITO,
                    item.ULTIMO_ANIO_PERIODO,
                    item.ULTIMO_MES_PERIODO,
                    item.AHORROS_OBLIGATORIOS,
                    item.IDENTIFICACION,
                    item.FLAG,
                    item.SALDO_ANUAL_APORTES,
                    item.SALDO_ANUAL_CESANTIAS,
                    item.MOVIMIENTOS_MENSUAL_APORTES,
                    item.MOVIMIENTOS_MENSUAL_CESANTIAS,
                    item.UNIDAD_EJECUTORA,
                    item.INTERES_APORTES,
                    item.INTERES_CESANTIAS,
                    0
                );
            }
            return table;
        }
        public static DataTable CrearDataTableAVAC(List<ObtenerCuentasAVAC> request)
        {
            // Crear el DataTable
            var table = new DataTable();
            table.Columns.Add("TAVAC_Id", typeof(long));
            table.Columns.Add("TAVAC_CtaId", typeof(int));
            table.Columns.Add("TAVAC_CtaNumero", typeof(int));
            table.Columns.Add("TAVAC_CliId", typeof(int));
            table.Columns.Add("TAVAC_TctId", typeof(int));
            table.Columns.Add("TAVAC_EctId", typeof(int));
            table.Columns.Add("TAVAC_CtaFechaCreacion", typeof(DateTime));
            table.Columns.Add("TAVAC_CtaSaldo", typeof(decimal));
            table.Columns.Add("TAVAC_AVAC", typeof(DateTime));
            table.Columns.Add("TAVAC_ValorAVAC", typeof(decimal));
            table.Columns.Add("TAVAC_SaldoAnualAVAC", typeof(decimal));
            table.Columns.Add("TAVAC_InteresesAVAC", typeof(decimal));
            table.Columns.Add("TAVAC_MovimientosMensualAVAC", typeof(decimal));
            table.Columns.Add("TAVAC_Identificacion", typeof(string));
            table.Columns.Add("TAVAC_UnidadEjecutora", typeof(int));

            // Llenar el DataTable
            foreach (var item in request)
            {
                table.Rows.Add(
                               item.ID,
            item.CTA_ID,
            item.CTA_NUMERO,
            item.CLI_ID,
            item.TCT_ID,
            item.ECT_ID,
            item.CTA_FECHA_CREACION,
            item.CTA_SALDO,
            (object?)item.AVAC ?? DBNull.Value,
            item.VALOR_AVAC,
            item.SALDO_ANUAL_AVAC,
            item.INTERESES_AVAC,
            item.MOVIMIENTOS_MENSUAL_AVAC,
            item.IDENTIFICACION,
            item.UNIDAD_EJECUTORA
             );
            }
            return table;
        }

        public static DataTable InsertarClientesAsync(ClienteDto c)
        {
            var dt = new DataTable();

            dt.Columns.Add("TC_IdCliente", typeof(int));
            dt.Columns.Add("TC_CLV_ID", typeof(int));
            dt.Columns.Add("TC_IdTipoActor", typeof(int));
            dt.Columns.Add("TC_IdTipoIdentificacion", typeof(int));
            dt.Columns.Add("TC_Identificacion", typeof(string));
            dt.Columns.Add("TC_FechaExpedicionDocumento", typeof(DateTime));
            dt.Columns.Add("TC_IdPaisExpedicionDocumento", typeof(int));
            dt.Columns.Add("TC_IdDepartamentoExpedicionDocumento", typeof(int));
            dt.Columns.Add("TC_IdCiudadExpedicionDocumento", typeof(int));
            dt.Columns.Add("TC_LugarExpedicionExtranjero", typeof(string));
            dt.Columns.Add("TC_CodigoMilitar", typeof(string));
            dt.Columns.Add("TC_SituacionActual", typeof(string));
            dt.Columns.Add("TC_TipoVinculacion", typeof(string));
            dt.Columns.Add("TC_PrimerNombre", typeof(string));
            dt.Columns.Add("TC_SegundoNombre", typeof(string));
            dt.Columns.Add("TC_PrimerApellido", typeof(string));
            dt.Columns.Add("TC_SegundoApellido", typeof(string));
            dt.Columns.Add("TC_FechaNacimiento", typeof(DateTime));
            dt.Columns.Add("TC_IdPaisNacimiento", typeof(int));
            dt.Columns.Add("TC_IdDepartamentoNacimiento", typeof(int));
            dt.Columns.Add("TC_IdCiudadNacimiento", typeof(int));
            dt.Columns.Add("TC_LugarNacimientoExtranjero", typeof(string));
            dt.Columns.Add("TC_Sexo", typeof(string));
            dt.Columns.Add("TC_EstadoCivil", typeof(string));
            dt.Columns.Add("TC_IdEstadoCivil", typeof(string));
            dt.Columns.Add("TC_IdProfesion", typeof(int));
            dt.Columns.Add("TC_Unidad", typeof(string));
            dt.Columns.Add("TC_IdCategoria", typeof(int));
            dt.Columns.Add("TC_NombreTipoIdentificacion", typeof(string));
            dt.Columns.Add("TC_IdFuerza", typeof(int));
            dt.Columns.Add("TC_IdGrado", typeof(int));
            dt.Columns.Add("TC_IdUnidadEjecutora", typeof(int));
            dt.Columns.Add("TC_FechaActualizaDatos", typeof(DateTime));
            dt.Columns.Add("TC_FechaActualizaDatosFinancieros", typeof(DateTime));
            dt.Columns.Add("TC_FechaActualizaDatosContacto", typeof(DateTime));
            dt.Columns.Add("TC_FechaInscripcion", typeof(DateTime));
            dt.Columns.Add("TC_FechaAlta", typeof(DateTime));
            dt.Columns.Add("TC_FechaPrimeraCuota", typeof(DateTime));
            dt.Columns.Add("TC_FechaUltimaCuota", typeof(DateTime));
            dt.Columns.Add("TC_Regimen", typeof(int));
            dt.Columns.Add("TC_FechaAltaCalculada", typeof(DateTime));
            dt.Columns.Add("TC_FechaReintegro", typeof(DateTime));
            dt.Columns.Add("TC_EstadoCliente", typeof(string));
            dt.Columns.Add("TC_IdEstadoCliente", typeof(int));
            dt.Columns.Add("TC_NumeroValidacion", typeof(string));
            dt.Columns.Add("TC_IdPatrocinador", typeof(int));
            dt.Columns.Add("TC_Participacion", typeof(decimal));
            dt.Columns.Add("TC_SaldoDisponible", typeof(decimal));
            dt.Columns.Add("TC_NumeroResolucion", typeof(string));
            dt.Columns.Add("TC_ValidacionBiometrica", typeof(bool));
            dt.Columns.Add("TC_EsBeneficiario", typeof(bool));
            dt.Columns.Add("TC_EstadoCargueNomina", typeof(int));
            dt.Columns.Add("TC_CuentasHabilitadas", typeof(int));
            dt.Columns.Add("TC_TasaAportes", typeof(decimal));
            dt.Columns.Add("TC_TasaVoluntariaAportes", typeof(decimal));
            dt.Columns.Add("TC_TasaSalarioBase", typeof(decimal));
            dt.Columns.Add("TC_NumeroCuotas", typeof(int));
            dt.Columns.Add("TC_Payload", typeof(string));
            dt.Columns.Add("TC_ValorSubsidio", typeof(decimal));
            dt.Columns.Add("TC_Actualiza", typeof(int));
            dt.Columns.Add("TC_Usuario", typeof(string));

            dt.Rows.Add(
                c.IdCliente, c.CLV_ID, c.IdTipoActor, c.IdTipoIdentificacion, c.Identificacion,
                c.FechaExpedicionDocumento, c.IdPaisExpedicionDocumento, c.IdDepartamentoExpedicionDocumento,
                c.IdCiudadExpedicionDocumento, c.LugarExpedicionExtranjero, c.CodigoMilitar, c.SituacionActual,
                c.TipoVinculacion, c.PrimerNombre, c.SegundoNombre, c.PrimerApellido, c.SegundoApellido,
                c.FechaNacimiento, c.IdPaisNacimiento, c.IdDepartamentoNacimiento, c.IdCiudadNacimiento,
                c.LugarNacimientoExtranjero, c.Sexo, c.EstadoCivil, c.IdEstadoCivil, c.IdProfesion,
                c.Unidad, c.IdCategoria, c.NombreTipoIdentificacion, c.IdFuerza, c.IdGrado,
                c.IdUnidadEjecutora, c.FechaActualizaDatos, c.FechaActualizaDatosFinancieros,
                c.FechaActualizaDatosContacto, c.FechaInscripcion, c.FechaAlta, c.FechaPrimeraCuota,
                c.FechaUltimaCuota, c.Regimen, c.FechaAltaCalculada, c.FechaReintegro, c.EstadoCliente,
                c.IdEstadoCliente, c.NumeroValidacion, c.IdPatrocinador, c.Participacion,
                c.SaldoDisponible, c.NumeroResolucion, c.ValidacionBiometrica, c.EsBeneficiario,
                c.EstadoCargueNomina, c.CuentasHabilitadas, c.TasaAportes, c.TasaVoluntariaAportes,
                c.TasaSalarioBase, c.NumeroCuotas, c.Payload, c.ValorSubsidio, c.Actualiza, c.Usuario
            );
            
            return dt;
        }

        public DataTable CrearDataTableCuentaCliente(List<CuentaDto> cuentas)
        {
            var table = new DataTable();
            table.Columns.Add("TTCC_IdCuenta", typeof(int));
            table.Columns.Add("TTCC_IdCuentaIntegracion", typeof(int));
            table.Columns.Add("TTCC_NumeroCuenta", typeof(int));
            table.Columns.Add("TTCC_NumeroCuentaCompleta", typeof(string));
            table.Columns.Add("TTCC_Subcuenta", typeof(int));
            table.Columns.Add("TTCC_IdTct", typeof(int));
            table.Columns.Add("TTCC_TipoCuenta", typeof(string));
            table.Columns.Add("TTCC_Saldo", typeof(decimal));
            table.Columns.Add("TTCC_FechaCreacion", typeof(DateTime));
            table.Columns.Add("TTCC_IdEct", typeof(int));
            table.Columns.Add("TTCC_EstadoCuenta", typeof(string));
            table.Columns.Add("TTCC_EstadoCliente", typeof(string));
            table.Columns.Add("TTCC_IdCcn", typeof(int));
            table.Columns.Add("TTCC_IdDocumento", typeof(int));
            table.Columns.Add("TTCC_IdTpa", typeof(int));
            table.Columns.Add("TTCC_Cuotas", typeof(int));
            table.Columns.Add("TTCC_FechaCierre", typeof(DateTime));
            table.Columns.Add("TTCC_IdCuentaHabiente", typeof(int));
            table.Columns.Add("TTCC_CuotasPendientes", typeof(int));
            table.Columns.Add("TTCC_FechaPrimerAporte", typeof(DateTime));
            table.Columns.Add("TTCC_SaldoCanje", typeof(decimal));
            table.Columns.Add("TTCC_SaldoDisponible", typeof(decimal));
            table.Columns.Add("TTCC_SaldoDisponibleEmbargos", typeof(decimal));
            table.Columns.Add("TTCC_TotalEmbargos", typeof(decimal));
            table.Columns.Add("TTCC_TotalBloqueos", typeof(decimal));
            table.Columns.Add("TTCC_TotalReintegros", typeof(decimal));
            table.Columns.Add("TTCC_Principal", typeof(int));
            table.Columns.Add("TTCC_IdCliente", typeof(int));
            table.Columns.Add("TTCC_conceptoId", typeof(int));
            table.Columns.Add("TTCC_CausalEstado", typeof(string));

            foreach (var item in cuentas)
            {
                table.Rows.Add(
                    item.IdCuenta,
                    item.IdCuentaIntegracion,
                    item.NumeroCuenta,
                    item.NumeroCuentaCompleta,
                    item.Subcuenta,
                    item.IdTct,
                    item.TipoCuenta,
                    item.Saldo,
                    item.FechaCreacion = item.FechaCreacion == DateTime.MinValue ? DateTime.Now : item.FechaCreacion,
                    item.IdEct,
                    item.EstadoCuenta,
                    item.EstadoCliente,
                    item.IdCcn,
                    item.IdDocumento,
                    item.IdTpa,
                    item.Cuotas,
                    DateTime.Now,
                    item.IdCuentaHabiente,
                    item.CuotasPendientes,
                    item.FechaPrimerAporte = item.FechaPrimerAporte == DateTime.MinValue ? DateTime.Now : item.FechaPrimerAporte,
                    item.SaldoCanje,
                    item.SaldoDisponible,
                    item.SaldoDisponibleEmbargos,
                    item.TotalEmbargos,
                    item.TotalReintegros,
                    item.Principal,
                    item.IdCliente,
                    item.conceptoId,
                    item.CausalEstado
                    
                );

            }
            return table;
        }
        public DataTable CrearDataTableMovimiento(List<MovimientoDTO> movimientos)
        {
            var table = new DataTable();
            table.Columns.Add("TTM_MVT_ID", typeof(int));
            table.Columns.Add("TTM_CTA_ID", typeof(int));
            table.Columns.Add("TTM_CNC_ID", typeof(int));
            table.Columns.Add("TTM_MVT_VALOR", typeof(decimal));
            table.Columns.Add("TTM_CAT_TIPO_MOVIMIENTO", typeof(string));
            table.Columns.Add("TTM_MVT_FECHA_PROCESO", typeof(DateTime));
            table.Columns.Add("TTM_DCT_ID", typeof(int));
            table.Columns.Add("TTM_MVT_ANO_PERIODO", typeof(int));
            table.Columns.Add("TTM_MVT_MES_PERIODO", typeof(string));
            table.Columns.Add("TTM_MVT_FECHA_MOVIMIENTO", typeof(DateTime));
            table.Columns.Add("TTM_CNC_CONCEPTO", typeof(string));

            foreach (var item in movimientos)
            {
                table.Rows.Add(
                    item.MVT_ID,
                    item.CTA_ID,
                    item.CNC_ID,
                    item.MVT_VALOR,
                    item.CAT_TIPO_MOVIMIENTO,
                    item.MVT_FECHA_PROCESO = item.MVT_FECHA_PROCESO == DateTime.MinValue ? DateTime.Now : item.MVT_FECHA_PROCESO,
                    item.DCT_ID,
                    item.MVT_ANO_PERIODO,
                    item.MVT_MES_PERIODO,
                    item.MVT_FECHA_MOVIMIENTO = item.MVT_FECHA_MOVIMIENTO == DateTime.MinValue ? DateTime.Now : item.MVT_FECHA_MOVIMIENTO,
                    item.CNC_CONCEPTO

                );
            }
            return table;
        }
        public DataTable CrearDataTableConceptoCuenta(List<ConceptoCuentaDto> movimientos)
        {
            var table = new DataTable();
            table.Columns.Add("TCT_Id", typeof(int));
            table.Columns.Add("TCT_CuentaId", typeof(int));
            table.Columns.Add("TCT_ConceptoId", typeof(int));
            table.Columns.Add("TCT_ConceptoDescripcion", typeof(string));
            table.Columns.Add("TCT_saldo", typeof(decimal));
            table.Columns.Add("TCT_FechaUltimoAporte", typeof(DateTime));
            table.Columns.Add("TCT_Ingresos", typeof(int));
            table.Columns.Add("TCT_Sva", typeof(decimal));
            table.Columns.Add("TCT_DescripcionConcepto", typeof(string));
            table.Columns.Add("TCT_ValorEmbargo", typeof(decimal));
            table.Columns.Add("TCT_ValorBloqueoEmbargo", typeof(decimal));
            table.Columns.Add("TCT_ValorDevolver", typeof(decimal));
            table.Columns.Add("TCT_ValorNewSaldo", typeof(decimal));

            foreach (var item in movimientos)
            {
                table.Rows.Add(
                    item.Id,
                    item.CuentaId,
                    item.ConceptoId,
                    item.ConceptoDescripcion,
                    item.saldo,
                    item.FechaUltimoAporte = item.FechaUltimoAporte == DateTime.MinValue ? DateTime.Now : item.FechaUltimoAporte,
                    item.Ingresos,
                    item.Sva,
                    item.DescripcionConcepto,
                    item.ValorEmbargo,
                    item.ValorBloqueoEmbargo,
                    item.ValorDevolver,
                    item.ValorNewSaldo
                );
            }
            return table;
        }

        public DataTable CrearDataTableMovimientoConceptoCuenta(List<MovimientosCuentaDto> movimientos)
        {
            var table = new DataTable();
            table.Columns.Add("TMCT_MovimientoId", typeof(int));
            table.Columns.Add("TMCT_Cuentaid", typeof(int));
            table.Columns.Add("TMCT_ConceptoId", typeof(int));
            table.Columns.Add("TMCT_Valor", typeof(decimal));
            table.Columns.Add("TMCT_TipoMovimiento", typeof(string));
            table.Columns.Add("TMCT_FechaProceso", typeof(DateTime));
            table.Columns.Add("TMCT_DocumentoId", typeof(int));
            table.Columns.Add("TMCT_PeriodoAno", typeof(string));
            table.Columns.Add("TMCT_FechaMovimiento", typeof(DateTime));

            foreach (var item in movimientos)
            {
                table.Rows.Add(
                    item.MovimientoId,
                    item.Cuentaid,
                    item.ConceptoId,
                    item.Valor,
                    item.TipoMovimiento ,
                    item.FechaProceso = item.FechaProceso == DateTime.MinValue ? DateTime.Now : item.FechaProceso,
                    item.DocumentoId,
                    item.PeriodoAno,
                    item.FechaMovimiento = item.FechaMovimiento == DateTime.MinValue ? DateTime.Now : item.FechaMovimiento
                );
            }
            return table;
        }

        public DataTable CrearDataTableObtenerCuentasAVAV(IEnumerable<ObtenerCuentaAVAVResponse> cuentas)
        {
            var table = new DataTable();
            table.Columns.Add("TOCAVAVT_Identificacion", typeof(long));
            table.Columns.Add("TOCAVAVT_IdCliente", typeof(int));
            table.Columns.Add("TOCAVAVT_CuentaId", typeof(int));
            table.Columns.Add("TOCAVAVT_CuentaNumero", typeof(int));
            table.Columns.Add("TOCAVAVT_ConceptoId", typeof(int));

            foreach (var c in cuentas)
            {
                table.Rows.Add(
                    c.Identificacion,
                    c.IdCliente,
                    c.CuentaId,
                    c.CuentaNumero,
                    c.ConceptoId
                );
            }

            return table;
        }
        public DataTable CrearDataTableObtenerCuentaAVC(IEnumerable<ObtenerCuentaAVAVResponse> cuentas)
        {
            var table = new DataTable();
            table.Columns.Add("TOCAVCT_Identificacion", typeof(string));
            table.Columns.Add("TOCAVCT_IdCliente", typeof(int));
            table.Columns.Add("TOCAVCT_CuentaId", typeof(int));
            table.Columns.Add("TOCAVCT_CuentaNumero", typeof(int));
            table.Columns.Add("TOCAVCT_ConceptoId", typeof(int));

            foreach (var c in cuentas)
            {
                table.Rows.Add(
                    c.Identificacion,
                    c.IdCliente,
                    c.CuentaId,
                    c.CuentaNumero,
                    c.ConceptoId
                );
            }

            return table;
        }

        public DataTable CrearDataTableSolucionVivienda(List<DatosVerificacionDerechoCuentaDto> datos)
        {
            var table = new DataTable();
            table.Columns.Add("DVDC_CtaId", typeof(int));
            table.Columns.Add("DVDC_CtaIdIntegracion", typeof(int));
            table.Columns.Add("DVDC_CtaNumero", typeof(int));
            table.Columns.Add("DVDC_CliId", typeof(int));
            table.Columns.Add("DVDC_TctId", typeof(int));
            table.Columns.Add("DVDC_EctId", typeof(int));
            table.Columns.Add("DVDC_CtaFechaCreacion", typeof(DateTime));
            table.Columns.Add("DVDC_CtaFechaPrimerAporte", typeof(DateTime));
            table.Columns.Add("DVDC_CtaCuotas", typeof(int));
            table.Columns.Add("DVDC_CtaSaldo", typeof(decimal));
            table.Columns.Add("DVDC_CuotasEsperadas", typeof(int));
            table.Columns.Add("DVDC_CantidadMovimientoCredito", typeof(int));
            table.Columns.Add("DVDC_FechaUltimoMovimientoAporte", typeof(DateTime));
            table.Columns.Add("DVDC_UltimoAnioPeriodo", typeof(int));
            table.Columns.Add("DVDC_UltimoMesPeriodo", typeof(int));
            table.Columns.Add("DVDC_AhorrosObligatorios", typeof(int));
            table.Columns.Add("DVDC_Identificacion", typeof(string));
            table.Columns.Add("DVDC_Flag", typeof(string));
            table.Columns.Add("DVDC_SaldoAnualAportes", typeof(decimal));
            table.Columns.Add("DVDC_SaldoAnualCesantias", typeof(decimal));
            table.Columns.Add("DVDC_MovimientosMensualAportes", typeof(decimal));
            table.Columns.Add("DVDC_MovimientosMensualCesantias", typeof(decimal));
            table.Columns.Add("DVDC_UnidadEjecutora", typeof(int));
            table.Columns.Add("DVDC_InteresesAportes", typeof(decimal));
            table.Columns.Add("DVDC_InteresesCesantias", typeof(decimal));
            table.Columns.Add("DVDC_TotalCuentas", typeof(int));

            foreach (var item in datos)
            {
                table.Rows.Add(
                    item.CtaId,
                    item.CtaIdIntegracion,
                    item.CtaNumero,
                    item.CliId,
                    item.TctId,
                    item.EctId,
                    item.CtaFechaCreacion = item.CtaFechaCreacion == DateTime.MinValue ? DateTime.Now : item.CtaFechaCreacion,  
                    item.CtaFechaPrimerAporte = item.CtaFechaPrimerAporte == DateTime.MinValue ? DateTime.Now : item.CtaFechaPrimerAporte,
                    item.CtaCuotas ?? (object)DBNull.Value,
                    item.CtaSaldo ?? (object)DBNull.Value,
                    item.CuotasEsperadas ?? (object)DBNull.Value,
                    item.CantidadMovimientoCredito ?? (object)DBNull.Value,
                    item.FechaUltimoMovimientoAporte = item.FechaUltimoMovimientoAporte == DateTime.MinValue ? DateTime.Now : item.FechaUltimoMovimientoAporte,
                    item.UltimoAnioPeriodo ?? (object)DBNull.Value,
                    item.UltimoMesPeriodo ?? (object)DBNull.Value,
                    item.AhorrosObligarotios ?? (object)DBNull.Value,
                    item.Identificacion ?? (object)DBNull.Value,
                    item.Flag ?? (object)DBNull.Value,
                    item.SaldoAnualAportes ?? (object)DBNull.Value,
                    item.SaldoAnualCesantias ?? (object)DBNull.Value,
                    item.MovimientosMensualAportes ?? (object)DBNull.Value,
                    item.MovimientosMensualCesantias ?? (object)DBNull.Value,
                    item.UnidadEjecutora,
                    item.InteresesAportes,
                    item.InteresesCesantias,
                    item.TotalCuentas
                );
            }
            return table;
        }
        #endregion
    }
} 
