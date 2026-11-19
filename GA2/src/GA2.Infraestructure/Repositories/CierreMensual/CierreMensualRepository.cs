using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class CierreMensualRepository
        : DapperGenericRepository, ICierreMensualRepository
    {
        public CierreMensualRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<bool> ValidarExisteCierre(int anio, int mes)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.ANIO), anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.MES), mes);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<int> ObtenerCuentaAbono(string concepto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CONCEPTOS), concepto);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<float> ObtenerIPC()
        {
            return await GetAsyncFirst<float>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<List<int>> ObtenerCategoriaConceptos(string concepto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CONCEPTOS), concepto);


            var resultado = await GetAsyncList<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return resultado.AsList();

        }

        public async Task<ProgramacionCierreMensualCuentas> ProgramarCierreMensualCuentas(ParametrosCierreMensual programacionCierre, CierreMensualCuentas cierre)
        {
            var conceptosAhorros = "";
            var conceptosCesantias = "";


            if (cierre.ListConceptosAhorros.Count > 0)
            {
                conceptosAhorros = String.Join(", ", cierre.ListConceptosAhorros);
            }

            if (cierre.ListConceptosCesantias.Count > 0)
            {
                conceptosCesantias = String.Join(", ", cierre.ListConceptosCesantias);
            }

            DynamicParameters parametros = new();

            // PARAMETROS ENTRADA
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.USUARIO), programacionCierre.Usuario);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.GENERA_ACTUALIZACION), programacionCierre.GeneraActualizacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.TIPO_PROCESO), programacionCierre.TipoProceso);
            // PARAMETROS OBTENIDOS

            //AHORRO
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CUENTA_ABONO_AHORRO), cierre.CuentaAbonoAhorros);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CONCEPTOS_AHORRO), conceptosAhorros);

            //CESANTIAS
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CUENTA_ABONO_CESANTIAS), cierre.CuentaAbonoCesantias);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CONCEPTOS_CESANTIAS), conceptosCesantias);

            //GENERAL
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.IPC), cierre.IPC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.ANIO), cierre.Anio);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.MES), cierre.Mes);

            return (ProgramacionCierreMensualCuentas)await GetAsyncFirst<ProgramacionCierreMensualCuentas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IPC> ObtenerIPCMes()
        {
            return await GetAsyncFirst<IPC>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<ProgramacionCierreMensualCuentas> ActualizaEstadoCierreMensualCuentas(Guid id, int estado)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CCT_ID), id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.CCT_ESTADO), estado);
            return await GetAsyncFirst<ProgramacionCierreMensualCuentas>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProgramacionCierreMensualCuentas>> ConsultarProgramacionCierreMensualCuentas()
        {
            return await GetAsyncList<ProgramacionCierreMensualCuentas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCierreMensualCuentas>> ReporteCierreMensualCuentas(Guid Id)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCierreMensualParams.ID_PROGRAMACION), Id);
            return await GetAsyncList<ReporteCierreMensualCuentas>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

    }
}
