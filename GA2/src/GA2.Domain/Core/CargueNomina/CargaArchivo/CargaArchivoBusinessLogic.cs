using GA2.Application.Dto;
using GA2.Domain.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Metodo que crea los componenetes que extrae desde el archivo de carga de nómina
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public class CargaArchivoBusinessLogic : ICargaArchivoBusinessLogic
    {
        /// <summary>
        /// Interface descomposicion de archivo
        /// </summary>
        private readonly IDescomposicionArchivo _descomposicionArchivo;

        /// <summary>
        /// Constructor Carga archivo
        /// </summary>
        /// <param name="descomposicionArchivo">Interface descomposicion de archivos</param>
        public CargaArchivoBusinessLogic(IDescomposicionArchivo descomposicionArchivo)
        {
            _descomposicionArchivo = descomposicionArchivo;
        }

        /// <summary>
        /// Metodo que hace validaciones del archivo y lo descompone en componentes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<ComponentesDTO> ValidacionArchivoNomina(CargaNominaDTO request,
                                                                  Stream streamFile,
                                                                  string fileName,
                                                                  ParamCargueNomDTO parametros,
                                                                  List<ErrorCargueArchivo> exceptions,
                                                                  DocumentoDto documentoId = default)
        {
            var lineasDatosArchivo = await ObtenerLineasDatosArchivoAsync(streamFile);
            NombreArchivoDto filenameDecomposed = await _descomposicionArchivo.DesgloseNombreArchivoAsync(fileName, parametros);
            EncabezadoArchivoDto headerDecomposed = await _descomposicionArchivo.DesgloseEncabezado(request, lineasDatosArchivo, parametros, exceptions);
            FinDeArchivoDto finalIdentificatorDecomposed = await _descomposicionArchivo.DesgloseIdentificadorFinal(lineasDatosArchivo, parametros, exceptions, filenameDecomposed, request);
            IEnumerable<CuerpoArchivoDto> bodyDecomposed = await _descomposicionArchivo.DesgloseBody(request, lineasDatosArchivo, parametros, exceptions);
            return new ComponentesDTO() { DOCUMENTO = documentoId, FILENAME = filenameDecomposed, HEADER = headerDecomposed, BODY = bodyDecomposed, END = finalIdentificatorDecomposed, FilasRegistros = lineasDatosArchivo.Count };
        }

        /// <summary>
        /// Obtener datos del archivo por lineas 
        /// </summary>
        /// <param name="fileStream">Archivo cargado</param>
        /// <returns></returns>
        public async Task<List<string>> ObtenerLineasDatosArchivoAsync(Stream fileStream)
        {
            List<string> lineasDatos = new List<string>();
            using var reader = new StreamReader(fileStream);

            while (reader.Peek() >= 0)
                lineasDatos.Add(await reader.ReadLineAsync());

            reader.Close();
            await Task.CompletedTask;

            return lineasDatos;
        }
    }
}

