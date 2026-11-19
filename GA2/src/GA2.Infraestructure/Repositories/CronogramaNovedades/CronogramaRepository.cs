using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CronogramaNovedades;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Cronograma;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Threading.Tasks;


namespace GA2.Infraestructure.Repositories
{
    public class CronogramaRepository : DapperGenericRepository, ICronogramaRepository
    {
        /// <summary>
        /// Constructor del repositorio
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        /// <param name="configuration"></param>
        public CronogramaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Cronograma>> AgregarUnidadEjecutoraCronograma(Cronograma cronograma)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.UEF_ID), cronograma.UEF_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_REPORTE), cronograma.CRO_FECHA_REPORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_INICIO), cronograma.CRO_FECHA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_FIN), cronograma.CRO_FECHA_FIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_PERIODO), cronograma.CRO_PERIODO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.FRT_ID), cronograma.FRT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.MDE_ID), cronograma.MDE_ID);

            return await GetAsyncList<Cronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Cronograma>> EditarFechaReporteCronograma(Cronograma cronograma)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_ID), cronograma.CRO_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_REPORTE), cronograma.CRO_FECHA_REPORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_INICIO), cronograma.CRO_FECHA_INICIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_FECHA_FIN), cronograma.CRO_FECHA_FIN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_PERIODO), cronograma.CRO_PERIODO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.FRT_ID), cronograma.FRT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.MDE_ID), cronograma.MDE_ID);

            return await GetAsyncList<Domain.Entities.Cronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesAfiliado> EditarNovedadAfiliado(NovedadesAfiliado request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@NVA_ID", request.NVA_ID);
            parameters.Add("@CNV_ID", request.CNV_ID);
            parameters.Add("@COD_MILITAR", request.COD_MILITAR);
            parameters.Add("@NVA_FECHA_INICIO", request.NVA_FECHA_INICIO);
            parameters.Add("@NVA_FECHA_FIN", request.NVA_FECHA_FIN);
            parameters.Add("@NVA_PORCENTAJE", request.NVA_PORCENTAJE);
            parameters.Add("@NVA_VALOR", request.NVA_VALOR);
            parameters.Add("@NVA_ES_ALTA", request.NVA_ES_ALTA);
            parameters.Add("@NVA_ES_BAJA", request.NVA_ES_BAJA);
            parameters.Add("@NVA_OBSERVACIONES", request.NVA_OBSERVACIONES);
            parameters.Add("@UEJ_ID", request.UEJ_ID);

            return await GetAsyncFirst<NovedadesAfiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesCronograma> EditarNovedadCronograma(NovedadesCronograma novedad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_ID), novedad.NVD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_CODIGO_ALTA), novedad.NVD_CODIGO_ALTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_CODIGO_BAJA), novedad.NVD_CODIGO_BAJA);

            return await GetAsyncFirst<NovedadesCronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesCronograma> EliminarNovedadAfiliado(int idUnidadEjecutora)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_ID), idUnidadEjecutora);

            return await GetAsyncFirst<NovedadesCronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesCronograma> EliminarNovedadCrucigrama(int idNovedad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_ID), idNovedad);

            return await GetAsyncFirst<Domain.Entities.NovedadesCronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Cronograma>> EliminarUnidadEjecutoraCronograma(int idCronograma)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CRO_ID), idCronograma);

            return await GetAsyncList<Domain.Entities.Cronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GeneracionArchivos>> GeneracionArchivoNovedades(int IdUnidadEjecutora, int IdConceptoNovedad, DateTime fechaInicio, DateTime fechaFin, bool EsAlta, string codigo, bool DHorizontal)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UNIDADEJECUTORA_ID", IdUnidadEjecutora);
            parameters.Add("@CONCEPTONOVEDAD_ID", IdConceptoNovedad);
            parameters.Add("@FECHA_INICIO", fechaInicio);
            parameters.Add("@FECHA_FIN", fechaFin);
            parameters.Add("@ES_ALTA", EsAlta);
            parameters.Add("@NVD_CODIGO_ALTA", codigo);
            parameters.Add("@DHORIZONTAL", DHorizontal);

            return await GetAsyncList<GeneracionArchivos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesAfiliado> InsertarNovedadAfiliado(NovedadesAfiliado request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@TID_ID", request.TID_ID);
            parameters.Add("@CLI_IDENTIFICACION", request.CLI_IDENTIFICACION);
            parameters.Add("@COD_MILITAR", request.COD_MILITAR);
            parameters.Add("@CNV_ID", request.CNV_ID);
            parameters.Add("@NVA_FECHA_INICIO", request.NVA_FECHA_INICIO);
            parameters.Add("@NVA_FECHA_FIN", request.NVA_FECHA_FIN);
            parameters.Add("@NVA_PORCENTAJE", request.NVA_PORCENTAJE);
            parameters.Add("@NVA_VALOR", request.NVA_VALOR);
            parameters.Add("@NVA_ES_ALTA", request.NVA_ES_ALTA);
            parameters.Add("@NVA_ES_BAJA", request.NVA_ES_BAJA);
            parameters.Add("@NVA_OBSERVACIONES", request.NVA_OBSERVACIONES);
            parameters.Add("@TOKEN", request.TOKEN);
            parameters.Add("@UEJ_ID", request.UEJ_ID);

            return await GetAsyncFirst<NovedadesAfiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesCronograma> InsertarNovedadCronograma(NovedadesCronograma novedad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.CNV_ID), novedad.CNV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_CODIGO_ALTA), novedad.NVD_CODIGO_ALTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.NVD_CODIGO_BAJA), novedad.NVD_CODIGO_BAJA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.UEJ_ID), novedad.UEJ_ID);

            return await GetAsyncFirst<NovedadesCronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<NovedadesAfiliado> ObtenerNovedadAfiliadoPorId(int idNovedad)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@NVA_ID", idNovedad);

            return await GetAsyncFirst<NovedadesAfiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NovedadesAfiliado>> ObtenerNovedadesAfiliado(int idTipoIdentificacion, string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@TID_ID", idTipoIdentificacion);
            parameters.Add("@CLI_IDENTIFICACION", identificacion);

            return await GetAsyncList<NovedadesAfiliado>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NovedadesCronograma>> ObtenerNovedadesCronograma(int idUnidadFuerza)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCronograma.UEJ_ID), idUnidadFuerza);

            return await GetAsyncList<Domain.Entities.NovedadesCronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NovedadPorUnidadEjecutora>> ObtenerNovedadesPorUnidadEjecutora(NovedadPorUnidadEjecutora request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UEJ_ID", request.UEJ_ID);
            parameters.Add("@FECHA_INICIO", request.NVA_FECHA_INICIO);
            parameters.Add("@FECHA_FIN", request.NVA_FECHA_FIN.AddHours(23).AddMinutes(59).AddSeconds(59));

            return await GetAsyncList<NovedadPorUnidadEjecutora>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Domain.Entities.Cronograma>> ObtenerUnidadesEjecutorasCronograma()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<Domain.Entities.Cronograma>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DescargaDefinitiva>> InsertarDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva)
        {
            var archivosAnteriores = await ObtenerArchivosDescargaDefinitiva(descargaDefinitiva);
            var archivoAnterior=archivosAnteriores.Where(x=>x.CNV_ID==descargaDefinitiva.CNV_ID).OrderByDescending(x=>x.NVA_DES_FECHA).FirstOrDefault();
            if(descargaDefinitiva.UEJ_ID==1|| descargaDefinitiva.UEJ_ID == 18 || descargaDefinitiva.UEJ_ID == 7 || descargaDefinitiva.UEJ_ID == 11)
            {
                if (archivoAnterior != null)
                {
                    if(archivoAnterior.NVA_DES_ESTADO== "EVALUACIÓN")
                    {
                        return null;
                    }
                    string parteConsecutivo = archivoAnterior.NVA_DES_NOMBRE_ARCHIVO.Substring(archivoAnterior.NVA_DES_NOMBRE_ARCHIVO.Length - 6);
                    var partes = parteConsecutivo.Split('.');
                    int numerico = (Convert.ToInt32(partes[0]) + 1);
                    string consecutivo=numerico.ToString().Length>1?numerico.ToString():string.Format("0{0}",numerico.ToString());
                    descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = string.Format("DESC{0}{1}{2}{3}.dat",archivoAnterior.NVD_CODIGO_ALTA,archivoAnterior.UEJ_SIGLA,descargaDefinitiva.NVA_DES_PERIODO.Replace("-", ""), consecutivo);
                }
                else
                {
                    descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = string.Format("DESC{0}{1}{2}{3}.dat", "0000", "XXX", descargaDefinitiva.NVA_DES_PERIODO.Replace("-",""), "00");

                }

            }
            else
            {
                switch (descargaDefinitiva.UEJ_ID)
                {
                    case 2:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "Caja Promotora de Vivienda Militar y de Policía.xlsx";
                        break;
                    case 3:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "NUEVO_" + descargaDefinitiva.NVA_DES_PERIODO.Replace("-", "") + "_" +descargaDefinitiva.NVD_CODIGO_ALTA + "_CAJAHONOR.txt";
                        break;
                    case 5:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "NUEVO_" + DateTime.Now.ToString("yyyyMM") + "_" + descargaDefinitiva.NVD_CODIGO_ALTA + "_PENSIONADOSPOLICIANACIONAL.txt";
                        break;
                    case 6:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "NUEVO_" + descargaDefinitiva.NVA_DES_PERIODO.Replace("-", "") + "_" + descargaDefinitiva.NVD_CODIGO_ALTA + "_SANIDADPOLICIANACIONAL.txt";
                        break;
                    case 8:
                    case 10:
                    case 19:
                    case 20:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = descargaDefinitiva.NVD_CODIGO_ALTA + ".txt";
                        break;
                    case 9:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "COMANDO GENERAL FFMM.txt";
                        break;
                    case 12:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "planilla_" + descargaDefinitiva.NVD_CODIGO_ALTA + ".xlsx";
                        break;
                    case 13:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "DIRECCIÓN GENERAL SANIDAD MILITAR.pdf";
                        break;
                    case 14:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "DIRECCIÓN GENERAL SANIDAD EJÉRCITO.pdf";
                        break;
                    case 15:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "DIRECCIÓN GENERAL SANIDAD ARMADA.pdf";
                        break;
                    case 16:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA.pdf";
                        break;
                    case 17:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "np.txt";
                        break;
                    case 21:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "UNIDAD ADMINISTRATIVA ESPECIAL DE LA JUSTICIA PENAL MILITAR Y POLICIAL.pdf";
                        break;
                    case 22:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "NUEVO_" + descargaDefinitiva.NVA_DES_PERIODO.Replace("-", "") + "_" + descargaDefinitiva.NVD_CODIGO_ALTA + "_POLICIABIENESTAR.txt";
                        break;
                    case 24:
                        descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO = "DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA.pdf";
                        break;
                }

            }
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.NOMBRE_ARCHIVO), descargaDefinitiva.NVA_DES_NOMBRE_ARCHIVO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.PERIODO), descargaDefinitiva.NVA_DES_PERIODO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.UEJ_ID), descargaDefinitiva.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.CNV_ID), descargaDefinitiva.CNV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.USUARIO), descargaDefinitiva.NVA_DES_USUARIO);
            parameters.Add("@FECHA", DateTime.Now);
            return await GetAsyncList<DescargaDefinitiva>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DescargaDefinitiva>> ActualizarEstadoDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.NVA_DES_ID), descargaDefinitiva.NVA_DES_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.NVA_DES_ESTADO), descargaDefinitiva.NVA_DES_ESTADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.NVA_DESCARGADO), descargaDefinitiva.NVA_DES_DESCARGADO);

            return await GetAsyncList<DescargaDefinitiva>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<DescargaDefinitiva>> ObtenerArchivosDescargaDefinitiva(DescargaDefinitiva descargaDefinitiva)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDescargaDefinitiva.UEJ_ID), descargaDefinitiva.UEJ_ID);

            return await GetAsyncList<DescargaDefinitiva>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

    }
}
