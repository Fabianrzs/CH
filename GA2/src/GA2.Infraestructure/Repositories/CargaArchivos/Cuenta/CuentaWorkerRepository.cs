using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Cuenta repositorio
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>20/05/2021</date>
    public class CuentaWorkerRepository : DapperGenericWorkerRepository, ICuentaWorkerRepository
    {
        /// <summary>
        /// Constructor cuenta repository
        /// </summary>
        /// <param name="configuration">Configuracion de aplicacion</param>
        public CuentaWorkerRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// json para la creacion de las cuentas
        /// </summary>
        /// <param name="json"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<IEnumerable<CuentaDto>> ParametrosCreacionCuenta(IEnumerable<CuerpoArchivoDto> cuerpo)
        {
            /// si la cuenta existe actualizar
            /// si la cuenta no existe registrar
            /// y las cuentas tambien tienen tipo de cuentas segun el tipo de archivos 
            /// 
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(HelpersEnums.GetEnumDescription(EnumEjecutorParameters.Json), JsonConvert.SerializeObject(cuerpo));
            return await EnumerableTransactionOperationBUA<CuentaDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameter, CommandType.StoredProcedure, IsolationLevel.ReadUncommitted);
        }

        public async Task<IEnumerable<CuerpoArchivoDto>> CreacionCuentaCargueNomina(EncabezadoArchivoDto encabezado, IEnumerable<CuerpoArchivoDto> cuerpo, FinDeArchivoDto fin, NombreArchivoDto nombreArchivo)
        {
            DynamicParameters parameter = new DynamicParameters();
            DataTable header = GenerarTablaHeader(encabezado);
            DataTable body = GenerarTablaBody(cuerpo);
            DataTable end = GenerarTablaEnd(fin);
            DataTable fileName = GenerarTablaFileName(nombreArchivo);

            parameter.Add("@HEADER", header.AsTableValuedParameter("[HeaderArchivoNomina]"));
            parameter.Add("@BODY", body.AsTableValuedParameter("[BodyArchivoNomina]"));
            parameter.Add("@END", end.AsTableValuedParameter("[EndArchivoNomina]"));
            parameter.Add("@FILENAME", fileName.AsTableValuedParameter("[FileNameArchivoNomina]"));
            return  await EnumerableTransactionOperationBUA<CuerpoArchivoDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameter, CommandType.StoredProcedure, IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// json para la creacion de las cuentas
        /// </summary>
        /// <param name="json"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>24/04/2021</date>
        public async Task<IEnumerable<CuentaDto>> ParametrosCreacionMovimientos(IEnumerable<CuerpoArchivoDto> cuerpo)
        {
            /// todos los movientos son registros nuevos
            /// todos los valores del movimiento segun la categoria del archivo CES censantias, AOB Aportes, FDS Fiducias, tiene un 
            /// condiciones que se deben registrar en los movimientos segundo el tipo de archivo.
            /// se movimientos para obterner el saldo. y el saldo se debe actualizar en la cuenta 
            /// 
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(HelpersEnums.GetEnumDescription(EnumEjecutorParameters.Json), JsonConvert.SerializeObject(cuerpo));
            return await EnumerableTransactionOperationBUA<CuentaDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameter, CommandType.StoredProcedure, IsolationLevel.ReadUncommitted);
        }

        private DataTable GenerarTablaHeader(EncabezadoArchivoDto encabezado)
        {
            DataTable header = new DataTable();
            header.Columns.Add("CONSECUTIVE", typeof(int));
            header.Columns.Add("FILE_TIPO", typeof(int));
            header.Columns.Add("NOMBRE_NOMINA_UNIDAD_EJECUTORA", typeof(string));
            header.Columns.Add("ID_UNIDAD_EJECUTORA", typeof(int));
            header.Columns.Add("FECHA_APORTES", typeof(DateTime));

            header.Rows.Add(encabezado.CONSECUTIVE, encabezado.FILE_TIPO, encabezado.NOMBRE_NOMINA_UNIDAD_EJECUTORA, encabezado.ID_UNIDAD_EJECUTORA, encabezado.FECHA_APORTES);

            return header;
        }

        private DataTable GenerarTablaBody(IEnumerable<CuerpoArchivoDto> cuerpo)
        {
            DataTable body = new DataTable();
            body.Columns.Add("CONSECUTIVE", typeof(int));
            body.Columns.Add("DIGITO_FUERZA", typeof(int));
            body.Columns.Add("UNIDAD_EJECUTORA", typeof(string));
            body.Columns.Add("TIPO_IDENTIFICACION", typeof(int));
            body.Columns.Add("IDENTIFICACION", typeof(string));
            body.Columns.Add("CODIGO_MILITAR", typeof(string));
            body.Columns.Add("PRIMER_NOMBRE", typeof(string));
            body.Columns.Add("SEGUNDO_NOMBRE", typeof(string));
            body.Columns.Add("PRIMER_APELLIDO", typeof(string));
            body.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
            body.Columns.Add("EMBARGO", typeof(int));
            body.Columns.Add("INGRESO_BASE_CALCULO", typeof(decimal));
            body.Columns.Add("VALOR", typeof(decimal));
            body.Columns.Add("PERIODO", typeof(DateTime));
            body.Columns.Add("GRADO", typeof(int));
            body.Columns.Add("UNIDAD_OPERATIVA", typeof(string));

            foreach (CuerpoArchivoDto item in cuerpo)
            {
                body.Rows.Add(item.CONSECUTIVE, item.DIGITO_FUERZA, item.UNIDAD_EJECUTORA,
                              item.TIPO_IDENTIFICACION, item.IDENTIFICACION, item.CODIGO_MILITAR,
                              item.PRIMER_NOMBRE, item.SEGUNDO_NOMBRE, item.PRIMER_APELLIDO,
                              item.SEGUNDO_APELLIDO, item.EMBARGO, item.INGRESO_BASE_CALCULO,
                              item.VALOR, item.PERIODO, item.GRADO, item.UNIDAD_OPERATIVA);
            }

            return body;
        }

        private DataTable GenerarTablaEnd(FinDeArchivoDto fin)
        {
            DataTable end = new DataTable();
            end.Columns.Add("CONSECUTIVE", typeof(int));
            end.Columns.Add("FILE_CONTROL_INDICATOR", typeof(int));
            end.Columns.Add("SUM_CONTRIBUTIONS", typeof(decimal));
            end.Columns.Add("END_FILE_CODE", typeof(int));

            end.Rows.Add(fin.CONSECUTIVE, fin.FILE_CONTROL_INDICATOR, fin.SUM_CONTRIBUTIONS, fin.END_FILE_CODE);

            return end;
        }

        private DataTable GenerarTablaFileName(NombreArchivoDto nombreArchivo)
        {
            DataTable fileName = new DataTable();
            fileName.Columns.Add("FILE_TIPO", typeof(string));
            fileName.Columns.Add("FILE_CATEGORIA", typeof(string));
            fileName.Columns.Add("FILE_PERIODOS_APORTES", typeof(DateTime));
            fileName.Columns.Add("FILE_UNIDAD_EJECUTORA", typeof(int));
            fileName.Columns.Add("FILE_FECHA_ENVIO", typeof(DateTime));

            fileName.Rows.Add(nombreArchivo.FILE_TIPO, nombreArchivo.FILE_CATEGORIA, nombreArchivo.FILE_PERIODOS_APORTES, nombreArchivo.FILE_UNIDAD_EJECUTORA, nombreArchivo.FILE_FECHA_ENVIO);

            return fileName;
        }
    }
}
