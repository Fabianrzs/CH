using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Domain.Entities.CargueNonima.Document;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ErrorCargueArchivoRepository : DapperGenericRepository, IErrorCargueArchivoRepository
    {
        public ErrorCargueArchivoRepository(IConfiguration configuration) : base(configuration) { }        

        public async Task<List<ErrorCargueArchivo>> CrearErroresCargueArchivo(List<ErrorCargueArchivo> errorCargueArchivo)
        {
            DynamicParameters parametros = new DynamicParameters();
            DataTable Lst_Errores = new DataTable();
            Lst_Errores.Columns.Add("NMN_ID", typeof(Guid));
            Lst_Errores.Columns.Add("DCT_ID", typeof(int));
            Lst_Errores.Columns.Add("NUMERO_LINEA", typeof(int));
            Lst_Errores.Columns.Add("CODIGO_INCIDENCIA", typeof(int));
            Lst_Errores.Columns.Add("DESCRIPCION", typeof(string));
            Lst_Errores.Columns.Add("TIPO_ERROR", typeof(string));

            foreach (ErrorCargueArchivo error in errorCargueArchivo)
            {
                Lst_Errores.Rows.Add(null, error.DCT_ID, error.NUMERO_LINEA, error.CODIGO_INCIDENCIA, error.DESCRIPCION, error.TIPO_ERROR);
            }

            parametros.Add("@LST_ERRORES", Lst_Errores.AsTableValuedParameter("[ErroresCargueArchivoNomina]"));
            return (List<ErrorCargueArchivo>)await GetAsyncList<ErrorCargueArchivo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);            
        }

        public async Task<List<ErrorCargueArchivo>> ConsultarErroresCargueArchivo()
        {
            return (List<ErrorCargueArchivo>) await GetAsyncList<ErrorCargueArchivo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), new DynamicParameters() ,CommandType.StoredProcedure);
        }

        public async Task<List<ErrorCargueArchivo>> ConsultarErroresPorCargueArchivo(int documentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            return (List<ErrorCargueArchivo>)await GetAsyncList<ErrorCargueArchivo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<FileNameReported> ObtenerNombreArchivoPorId(int documentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            return await GetAsyncFirst<FileNameReported>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BodyFile> ObtenerUltimoConsecutivoCuerpo(int documentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            return await GetAsyncFirst<BodyFile>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseCorreccionMasiva> ActualizarConsecutivos(int documentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            return await GetAsyncFirst<ResponseCorreccionMasiva>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseCorreccionMasiva> GuardarEncabezadoMasivo(int documentoId, int unidadEjecutora, string periodo, string tipoArchivo, string categoria)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            parametros.Add("@UEJ_ID", unidadEjecutora);
            parametros.Add("@EAN_PERIODO_APORTE", periodo);
            parametros.Add("@TAR_TIPO", tipoArchivo);
            parametros.Add("@TAR_CLASIFICACION", categoria);
            return await GetAsyncFirst<ResponseCorreccionMasiva>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ResponseCorreccionMasiva> GuardarCuerpoMasivo(int documentoId, int unidadEjecutoral, string periodo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            parametros.Add("@UEJ_ID", unidadEjecutoral);
            parametros.Add("@CAN_PERIODO", periodo);
            var hola =  await GetAsyncFirst<ResponseCorreccionMasiva>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return hola;
        }

        public async Task<ResponseCorreccionMasiva> GuardarFinMasivo(int documentoId, int valorIndicadorControl)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            parametros.Add("@FAN_INDICADOR_CONTROL", valorIndicadorControl);
            return await GetAsyncFirst<ResponseCorreccionMasiva>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<List<ErrorCargueArchivoDto>> ActualizarEstadoErrorArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", guardarArchivoCorregir.Dct_id);
            if(guardarArchivoCorregir.ConsecutivoCuerpo != 0)
            {
                parametros.Add("@NUMERO_LINEA", guardarArchivoCorregir.ConsecutivoCuerpo);

            }else if(guardarArchivoCorregir.Fin_Consecutivo != 0 )
            {
                parametros.Add("@NUMERO_LINEA", guardarArchivoCorregir.Fin_Consecutivo);

            }else if(guardarArchivoCorregir.Encabezado_Consecutivo != 0 )
            {
                parametros.Add("@NUMERO_LINEA", guardarArchivoCorregir.Encabezado_Consecutivo);

            }
            parametros.Add("@TIPO_ERROR", guardarArchivoCorregir.Tipo_error);
            return (List<ErrorCargueArchivoDto>)await GetAsyncList<ErrorCargueArchivoDto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ConsultarArchivosCargados> ObtenerTotalErroresPorArchivo(int documentoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            return await GetAsyncFirst<ConsultarArchivosCargados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<ConsultarArchivosCargados> ActualizarEstadoDocumento(int documentoId, int estadoCargueDocumentoId, int estadoCargueArchivoId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@DCT_ID", documentoId);
            parametros.Add("@ESD_ID", estadoCargueDocumentoId);
            parametros.Add("@EST_ID", estadoCargueArchivoId);
            return await GetAsyncFirst<ConsultarArchivosCargados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

    }
}
