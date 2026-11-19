using GA2.Application.Dto;
using GA2.Application.Main.TU;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GA2.Domain.Core.DescomposicionArchivo;

namespace GA2.Domain.Core
{
    public class DescomposicionArchivo : IDescomposicionArchivo
    {
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IMensajeRepository _mensajeRepository;

        public DescomposicionArchivo(ICatalogosRepository catalogosRepository, IMensajeRepository mensajeRepository)
        {
            _catalogosRepository = catalogosRepository;
            _mensajeRepository = mensajeRepository;
        }

        /// <summary>
        /// Metodo que 
        /// </summary>
        /// <param name="lineasDatos">Lista de datos cargados de archivo</param>
        /// <param name="param">Parametro de archivo</param>
        /// <returns>Lista de BodyFileReported</returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>26/03/2021</date>
        public async Task<IEnumerable<CuerpoArchivoDto>> DesgloseBody(CargaNominaDTO request, List<string> lineasDatos, ParamCargueNomDTO param, List<ErrorCargueArchivo> exceptions)
        {
            List<CuerpoArchivoDto> cuerpoArchivo = new List<CuerpoArchivoDto>();
            var primeraLinea = lineasDatos.First();
            int lineaContada = default;
            string tipoError = "Cuerpo del Archivo";

            var validacionesTablas = new ValidacionesTablas
            {
                UnidadesEjecutoras = await this._catalogosRepository.ObtenerUnidadesEjecutoras(),
                Grados = await this._catalogosRepository.ObtenerGrados(),
                TiposArchivos = await this._catalogosRepository.ObtenerTiposArchivos(),
                TiposIdentificacion = await this._catalogosRepository.ObtenerTiposIdentificacion(),
                Fuerzas = await this._catalogosRepository.ObtenerFuerzas()
            };

            var mensaje5 = await _mensajeRepository.ObtenerMensaje("30");
            var mensaje6 = await _mensajeRepository.ObtenerMensaje("31");
            var mensaje7 = await _mensajeRepository.ObtenerMensaje("32");

            var mensajeValidacion = new MensajeValidaciones
            {
                Mensaje5 = mensaje5,
                Mensaje6 = mensaje6,
                Mensaje7 = mensaje7
            };

            // Crear una lista para almacenar los números consecutivos
            List<int> numerosConsecutivos = new List<int>();
            List<string> numerosIdentificacion = new List<string>();
            var consecutivos = new HashSet<int>();
            bool hayAnomalia = false;

                foreach (var linea in lineasDatos.Skip(1).Take(lineasDatos.Count - 2))
            {
                ++lineaContada;

                int number;
                var cuerpo = new CuerpoArchivoDto();
                var consecutive = int.TryParse(linea.Substring(param.PAR_CUER_CONSEC_INI, param.PAR_CUER_CONSEC_LEN), out number);
                number = 1 + lineaContada;
                cuerpo.CONSECUTIVE = 1 + lineaContada; // Iniciar el consecutivo en 2 y luego incrementar de uno en uno
                consecutivos.Add(cuerpo.CONSECUTIVE);
                cuerpo.DIGITO_FUERZA = int.Parse(linea.Substring(param.PAR_CUER_DIGITO_FUERZA_INI, param.PAR_CUER_DIGITO_FUERZA_LEN), CultureInfo.InvariantCulture);
                cuerpo.UNIDAD_EJECUTORA = int.Parse(linea.Substring(param.PAR_CUER_UNI_EJECUTORA_INI, param.PAR_CUER_UNI_EJECUTORA_LEN), CultureInfo.InvariantCulture);
                cuerpo.TIPO_IDENTIFICACION = int.Parse(linea.Substring(param.PAR_CUER_TIPO_ID_INI, param.PAR_CUER_TIPO_ID_LEN), CultureInfo.InvariantCulture);
                var identification = linea.Substring(param.PAR_CUER_IDENTIFICACION_INI, param.PAR_CUER_IDENTIFICACION_LEN);
                cuerpo.IDENTIFICACION = identification.TrimStart('0');
                numerosIdentificacion.Add(cuerpo.IDENTIFICACION);
                var codigoMilitar = linea.Substring(param.PAR_CUER_COD_MILITAR_INI, param.PAR_CUER_COD_MILITAR_LEN);
                cuerpo.CODIGO_MILITAR = codigoMilitar.TrimStart('0');
                cuerpo.PRIMER_NOMBRE = linea.Substring(param.PAR_CUER_PRIMER_NOMBRE_INI, param.PAR_CUER_PRIMER_NOMBRE_LEN).Trim();
                cuerpo.SEGUNDO_NOMBRE = linea.Substring(param.PAR_CUER_SEGUNDO_NOMBRE_INI, param.PAR_CUER_SEGUNDO_NOMBRE_LEN).Trim();
                cuerpo.PRIMER_APELLIDO = linea.Substring(param.PAR_CUER_PRIMER_APELLIDO_INI, param.PAR_CUER_PRIMER_APELLIDO_LEN).Trim();
                cuerpo.SEGUNDO_APELLIDO = linea.Substring(param.PAR_CUER_SEGUNDO_APELLIDO_INI, param.PAR_CUER_SEGUNDO_APELLIDO_LEN).Trim();
                cuerpo.EMBARGO = int.Parse(linea.Substring(param.PAR_CUER_EMBARGO_INI, param.PAR_CUER_EMBARGO_LEN), CultureInfo.InvariantCulture);
                cuerpo.INGRESO_BASE_CALCULO = decimal.Parse(linea.Substring(param.PAR_CUER_INGRESO_BASE_INI, param.PAR_CUER_INGRESO_BASE_LEN), CultureInfo.InvariantCulture);
                cuerpo.VALOR = decimal.Parse(linea.Substring(param.PAR_CUER_VALOR_INI, param.PAR_CUER_VALOR_LEN), CultureInfo.InvariantCulture);
                cuerpo.PERIODO = DateTime.ParseExact(linea.Substring(param.PAR_CUER_PERIODO_INI, param.PAR_CUER_PERIODO_LEN), "yyyy/MM/d", CultureInfo.CurrentCulture);
                cuerpo.GRADO = int.Parse(linea.Substring(param.PAR_CUER_GRADO_INI, param.PAR_CUER_GRADO_LEN), CultureInfo.InvariantCulture);
                var unidadString = linea.Substring(param.PAR_CUER_UNIDAD_OPERATIVA_INI, param.PAR_CUER_UNIDAD_OPERATIVA_LEN);
                cuerpo.UNIDAD_OPERATIVA = unidadString.TrimStart('0');


                await this.ValidacionesCuerpoArchivoAsync(validacionesTablas, request, cuerpo, number, mensajeValidacion, exceptions, hayAnomalia);
                cuerpoArchivo.Add(cuerpo);
                numerosConsecutivos.Add(cuerpo.CONSECUTIVE);
            }

            // Ordenar la lista de números consecutivos
            numerosConsecutivos.Sort();

            int numeroAnterior = 1; // Inicializar con el primer número
            bool secuenciaCorrecta = true;

            for (int i = 0; i < numerosConsecutivos.Count; i++)
            {
                int numeroActual = numerosConsecutivos[i];

                // Verificar si el número actual es consecutivo al número anterior
                if (numeroActual != numeroAnterior + 1)
                {
                    // Agregar una excepción a la lista
                    exceptions.Add(new ErrorCargueArchivo()
                    {
                        DCT_ID = 0,
                        NUMERO_LINEA = numeroActual,
                        CODIGO_INCIDENCIA = 0,
                        DESCRIPCION = $"Error en el consecutivo",
                        TIPO_ERROR = tipoError,
                        ESTADO_ERROR = 1
                    });
                }

                // Actualizar el número anterior
                numeroAnterior = numeroAnterior+1;
            }


            // Verificar números consecutivos repetidos
            var numerosRepetidos = numerosIdentificacion.GroupBy(x => x)
                                                         .Where(group => group.Count() > 1)
                                                         .Select(group => new { NumeroIdentificacion = group.Key, Consecutives = consecutivos.Where((c, i) => numerosIdentificacion[i] == group.Key).ToList() });

            foreach (var item in numerosRepetidos)
            {
                foreach (var numeroRepetido in item.Consecutives)
                {
                    exceptions.Add(new ErrorCargueArchivo()
                    {
                        DCT_ID = 0,
                        NUMERO_LINEA = numeroRepetido,
                        CODIGO_INCIDENCIA = 0,
                        DESCRIPCION = $"Error numero de identificacion repetido",
                        TIPO_ERROR = tipoError,
                        ESTADO_ERROR = 1
                    });
                }
            }

            await Task.Yield();
            return cuerpoArchivo;
        }

        /// <summary>
        /// Metodo que lee el nombre del archivo y lo descompone en variables
        /// </summary>
        /// <param name="file">Archivo recibido</param>
        /// <returns>FilenameReported</returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>26/03/2021</date>
public async Task<NombreArchivoDto> DesgloseNombreArchivoAsync(string fileName, ParamCargueNomDTO param)
{
    if (param == null) throw new ArgumentNullException(nameof(param));

    var tipoArchivo = fileName.Substring(param.PAR_NOM_TIPO_ARCHIVO_REPORTADO_INI, param.PAR_NOM_TIPO_ARCHIVO_REPORTADO_LEN);
    var categoria = fileName.Substring(param.PAR_NOM_CLASIF_ARCHIVO_INI, param.PAR_NOM_CLASIF_ARCHIVO_LEN);
    var periodoAportes = fileName.Substring(param.PAR_NOM_PERIODO_APORTES_INI, param.PAR_NOM_PERIODO_APORTES_LEN);
    var unidadEjecutora = fileName.Substring(param.PAR_NOM_COD_UNID_EJECUTORA_INI, param.PAR_NOM_COD_UNID_EJECUTORA_LEN);
    var fechaEnvioStr = fileName.Substring(param.PAR_NOM_FECHA_ENVIO_INI, param.PAR_NOM_FECHA_ENVIO_LEN);

    DateTime fechaEnvio;
            //if (DateTime.TryParseExact(fechaEnvioStr, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaEnvio))
    if (fechaEnvioStr.Length == 8 && fechaEnvioStr.All(char.IsDigit) &&
    DateTime.TryParseExact(fechaEnvioStr, "ddMMyyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fechaEnvio))
    {
                var archivo = new NombreArchivoDto
        {
            FILE_TIPO = tipoArchivo,
            FILE_CATEGORIA = categoria,
            FILE_PERIODOS_APORTES = int.Parse(periodoAportes, CultureInfo.InvariantCulture),
            FILE_UNIDAD_EJECUTORA = int.Parse(unidadEjecutora, CultureInfo.InvariantCulture),
            FILE_FECHA_ENVIO = fechaEnvio
        };

        await Task.Yield();

        return archivo;
    }
    else
    {
        throw new FormatException("La cadena de fecha no tiene el formato válido.");
    }
}

        /// <summary>
        /// Valida el identificador final del archivo y lo compara con los registros
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>26/03/2021</date>
        /// <param name="file">Archivo a evaluar</param>
        /// <param name="para">Parametros obtenidos de base datos</param>
        /// <returns>EndFileReported</returns>
        public async Task<FinDeArchivoDto> DesgloseIdentificadorFinal(List<string> LineasDatos, ParamCargueNomDTO param, List<ErrorCargueArchivo> exceptions, NombreArchivoDto NombreArchivo, CargaNominaDTO request)
        {
            decimal sumatoriaAportes = default;
            int consecutivo = default;
            decimal comparacionValores = default;
            string codigoFinal = default, valorBase = default, indicadorControl = default;
            string tipoError = "Fin del Archivo";

            var lineaFinal = LineasDatos.Last();
            LineasDatos.ForEach(x =>
            {
                if (x != LineasDatos.FirstOrDefault())
                    if (!string.IsNullOrWhiteSpace(x))
                        if (x != lineaFinal)
                        {
                            valorBase = x.Substring(param.PAR_CUER_VALOR_INI, param.PAR_CUER_VALOR_LEN);
                            sumatoriaAportes += decimal.Parse(valorBase, CultureInfo.InvariantCulture);
                        }
            });


            var totalValorlineaFinal = lineaFinal.Substring(param.PAR_FIN_SUM_APORTES_INI, param.PAR_FIN_SUM_APORTES_LEN);
            comparacionValores = decimal.Parse(totalValorlineaFinal, CultureInfo.InvariantCulture);
            var mensaje1 = await _mensajeRepository.ObtenerMensaje("26");
            var mensaje2 = await _mensajeRepository.ObtenerMensaje("27");
            var mensaje3 = await _mensajeRepository.ObtenerMensaje("28");


            if (comparacionValores != Math.Round(sumatoriaAportes, 0, MidpointRounding.AwayFromZero))
                return new FinDeArchivoDto
                {
                    CONSECUTIVE = 0,
                    FILE_CONTROL_INDICATOR = "error no coincide la sumatoria de aportes",
                    END_FILE_CODE = 0,
                    SUM_CONTRIBUTIONS = "0"
                };

            consecutivo = int.Parse(lineaFinal.Substring(param.PAR_FIN_CONSEC_INI, param.PAR_FIN_CONSEC_LEN));
            if (!consecutivo.Equals(LineasDatos.Count())) exceptions.Add(new ErrorCargueArchivo()
            {
                DCT_ID = 0,
                NUMERO_LINEA = LineasDatos.Count(),
                CODIGO_INCIDENCIA = 27,
                DESCRIPCION = mensaje2.MSJ_MENSAJE,
                TIPO_ERROR = tipoError,
                ESTADO_ERROR = 1
            });

            indicadorControl = lineaFinal.Substring(param.PAR_FIN_INDICADOR_CONTROL_INI, param.PAR_FIN_INDICADOR_CONTROL_LEN);
            codigoFinal = lineaFinal.Substring(param.PAR_FIN_COD_FIN_INI, param.PAR_FIN_COD_FIN_LEN);
            var Calculo = ((decimal)NombreArchivo.FILE_PERIODOS_APORTES * (decimal)LineasDatos.Count()) / (int)request.Uej_Id + (int)request.Tdc_Id;
            var valorFormulaIndicadorControl = Math.Round(Calculo, 0, MidpointRounding.AwayFromZero);
            

            // Validacion de control de indicador
            if (!string.IsNullOrWhiteSpace(indicadorControl))
                if (valorFormulaIndicadorControl != Int64.Parse(indicadorControl, CultureInfo.InvariantCulture)) exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = LineasDatos.Count(),
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "error no coincide el valor de indicador de control",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            // Validacion de codigo final
            if (!string.IsNullOrWhiteSpace(codigoFinal))
                if (codigoFinal != "999") exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = LineasDatos.Count(),
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = mensaje3.MSJ_MENSAJE,
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            await Task.Yield();
            return new FinDeArchivoDto
            {
                CONSECUTIVE = consecutivo,
                FILE_CONTROL_INDICATOR = indicadorControl,
                END_FILE_CODE = int.Parse(codigoFinal, CultureInfo.InvariantCulture),
                SUM_CONTRIBUTIONS = sumatoriaAportes.ToString()
            };
        }

        /// <summary>
        /// Metodo que descompone el encabezado dle archivo en sus variables 
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>26/03/2021</date>
        /// <param name="file">Archivo recibido</param>
        /// <returns>HeaderFileReported</returns>
        public async Task<EncabezadoArchivoDto> DesgloseEncabezado(CargaNominaDTO request, List<string> lineasDatos, ParamCargueNomDTO param, List<ErrorCargueArchivo> exceptions)
        {
            string tipoError = "Encabezado del Archivo";
            var primeraLinea = lineasDatos.First();

            var validacionesTablas = new ValidacionesTablas
            {
                UnidadesEjecutoras = await this._catalogosRepository.ObtenerUnidadesEjecutoras(),
                Grados = await this._catalogosRepository.ObtenerGrados(),
                TiposArchivos = await this._catalogosRepository.ObtenerTiposArchivos(),
                TiposIdentificacion = await this._catalogosRepository.ObtenerTiposIdentificacion(),
                Fuerzas = await this._catalogosRepository.ObtenerFuerzas()
            };

            var nombreArchivo = new NombreArchivoDto();

            string consecutivo = primeraLinea.Substring(param.PAR_ENC_CONSEC_INI, param.PAR_ENC_CONSEC_LEN);
            string tipoArchivo = primeraLinea.Substring(param.PAR_ENC_TIPO_ARCHIVO_INI, param.PAR_ENC_TIPO_ARCHIVO_LEN);
            string nombreNominaUnidad = primeraLinea.Substring(param.PAR_ENC_NOMB_NOMINA_UNI_EJECUTORA_INI, param.PAR_ENC_NOMB_NOMINA_UNI_EJECUTORA_LEN);
            string codigoUnidad = primeraLinea.Substring(param.PAR_ENC_COD_UNI_EJECUTORA_INI, param.PAR_ENC_COD_UNI_EJECUTORA_LEN);
            string periodoAportes = primeraLinea.Substring(param.PAR_ENC_PERIODO_APORTES_INI, param.PAR_ENC_PERIODO_APORTES_LEN);

            var mensaje5 = await _mensajeRepository.ObtenerMensaje("30");
            var mensaje6 = await _mensajeRepository.ObtenerMensaje("31");
            var mensaje7 = await _mensajeRepository.ObtenerMensaje("32");

            var mensajeValidacion = new MensajeValidaciones
            {
                Mensaje5 = mensaje5,
                Mensaje6 = mensaje6,
                Mensaje7 = mensaje7
            };
            if(tipoArchivo=="005")
            {
                var clasificacionArchivo = request.Dct_Nombre.Substring(3, 3);
                var tiposArchivos = await this._catalogosRepository.ObtenerTiposArchivos();
                var tipoArchivoHAV= tiposArchivos.FirstOrDefault(x=>x.TAR_CLASIFICACION==clasificacionArchivo);
                tipoArchivo=tipoArchivoHAV==null?tipoArchivo:tipoArchivoHAV.TAR_ID.ToString();
            };
            var conceptos = await this._catalogosRepository.ObtenerConceptos();
            var conceptoHomologados = await this._catalogosRepository.ObtenerConceptosHomologados();
            var conceptoExiste = (from i in conceptos
                                  join h in conceptoHomologados on i.CNC_ID equals h.CNC_ID
                                  where h.CNH_CONCEPTO_CARGA.Contains(tipoArchivo)
                                  select new Concepto
                                  {
                                      CNC_ID = i.CNC_ID,
                                      CAT_TIPO_CONCEPTO = i.CAT_TIPO_CONCEPTO,
                                      CNC_DESCRIPCION = i.CNC_DESCRIPCION,
                                      CNC_FORMULA_CALCULO = i.CNC_FORMULA_CALCULO,
                                      CNC_INTERES = i.CNC_INTERES,
                                      CNC_ORDEN = i.CNC_ORDEN
                                  });

            var mensaje4 = await _mensajeRepository.ObtenerMensaje("29");


            if (conceptoExiste == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = 1,
                    CODIGO_INCIDENCIA = 29,
                    DESCRIPCION = mensaje4.MSJ_MENSAJE + InfoColumna(param.PAR_ENC_TIPO_ARCHIVO_INI, param.PAR_ENC_TIPO_ARCHIVO_LEN),
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            var encabezado = new EncabezadoArchivoDto
            {
                CONSECUTIVE = int.Parse(consecutivo, CultureInfo.InvariantCulture),
                FILE_TIPO = int.Parse(tipoArchivo, CultureInfo.InvariantCulture),
                ID_UNIDAD_EJECUTORA = int.Parse(codigoUnidad, CultureInfo.InvariantCulture),
                NOMBRE_NOMINA_UNIDAD_EJECUTORA = nombreNominaUnidad,
                FECHA_APORTES = DateTime.ParseExact(periodoAportes, "yyyyMM", CultureInfo.CurrentCulture)
            };

            int linea = 1;
            await this.ValidacionesEncabezadoArchivoAsync(validacionesTablas, request, encabezado, linea, mensajeValidacion, exceptions);

            return encabezado;
        }

        private async Task ValidacionesEncabezadoArchivoAsync(ValidacionesTablas validaciones, CargaNominaDTO request, EncabezadoArchivoDto encabezado, int linea, MensajeValidaciones mensajes, List<ErrorCargueArchivo> exceptions)
        {
            string tipoError = "Encabezado del Archivo";

            var tipoArchivoQuery = validaciones.TiposArchivos.Where(x => x.TAR_ID == encabezado.FILE_TIPO);
            if (tipoArchivoQuery == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Tipo archivo no existe",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            var ValidartiposArchivos = encabezado.FILE_TIPO == request.Tdc_Id;
            if (ValidartiposArchivos == false)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Tipo archivo no coincide con el tipo archivo del nombre",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            if (encabezado.CONSECUTIVE != 1)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Error en consecutivo de encabezado",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            var UnidadEjecutoraQuery = validaciones.UnidadesEjecutoras.Where(x => x.UEJ_ID == encabezado.ID_UNIDAD_EJECUTORA);
            if (UnidadEjecutoraQuery == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Unidad ejecutora no existe",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            var ValidarUnidadEjecutora = encabezado.ID_UNIDAD_EJECUTORA == request.Uej_Id;
            if (ValidarUnidadEjecutora == false)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Unidad ejecutora no coincide con la unidad ejecutora del nombre",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            string añoMesAportes = encabezado.FECHA_APORTES.ToString("yyyyMM");
            var ValidarPeriodo = encabezado.ID_UNIDAD_EJECUTORA == request.Uej_Id;
            if (ValidarPeriodo == false)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = $"Periodo no coincide con el periodo del nombre",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            await Task.Yield();
        }

        private async Task ValidacionesCuerpoArchivoAsync(ValidacionesTablas validaciones, CargaNominaDTO request, CuerpoArchivoDto cuerpo, int linea, MensajeValidaciones mensajes, List<ErrorCargueArchivo> exceptions, bool hayAnomalia)
        {
            string tipoError = "Cuerpo del Archivo";
            //var grado = validaciones.Grados.Where(x => x.GRD_CODIGO == cuerpo.GRADO).FirstOrDefault();
            //if (grado == null)
            //    exceptions.Add(new ErrorCargueArchivo()
            //    {
            //        DCT_ID = 0,
            //        NUMERO_LINEA = linea,
            //        CODIGO_INCIDENCIA = 21,
            //        DESCRIPCION = $"Grado no existe en los catalagos",
            //        TIPO_ERROR = tipoError,
            //        ESTADO_ERROR = 1
            //    });
            var unidadEjecutora = validaciones.UnidadesEjecutoras.Where(x => x.UEJ_ID == cuerpo.UNIDAD_EJECUTORA).FirstOrDefault();
            if (unidadEjecutora == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 30,
                    DESCRIPCION = $"{mensajes.Mensaje5.MSJ_MENSAJE}",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            //var ValidarUnidadEjecutora = cuerpo.UNIDAD_EJECUTORA == request.Uej_Id;
            //if (ValidarUnidadEjecutora == false)
            //    exceptions.Add(new ErrorCargueArchivo()
            //    {
            //        DCT_ID = 0,
            //        NUMERO_LINEA = linea,
            //        CODIGO_INCIDENCIA = 0,
            //        DESCRIPCION = $"Unidad ejecutora no coincide con la unidad ejecutora del nombre",
            //        TIPO_ERROR = tipoError,
            //        ESTADO_ERROR = 1
            //    });
            var fuerza = validaciones.Fuerzas.Where(x => x.FRZ_DIGITO == cuerpo.DIGITO_FUERZA).FirstOrDefault();
            if (fuerza == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 31,
                    DESCRIPCION = $"{mensajes.Mensaje6.MSJ_MENSAJE}",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            var tipoIdentificacion = validaciones.TiposIdentificacion.Where(x => x.TID_ID == cuerpo.TIPO_IDENTIFICACION).FirstOrDefault();
            if (tipoIdentificacion == null)
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 32,
                    DESCRIPCION = $"{mensajes.Mensaje7.MSJ_MENSAJE}",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (string.IsNullOrWhiteSpace(cuerpo.PRIMER_NOMBRE))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer nombre esta vacio",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (string.IsNullOrWhiteSpace(cuerpo.PRIMER_APELLIDO))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer apellido esta vacio",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (ContieneMinusculas(cuerpo.PRIMER_NOMBRE))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer nombre contienen caracteres en minuscula",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (ContieneMinusculas(cuerpo.SEGUNDO_NOMBRE))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Segundo nombre contienen caracteres en minuscula",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (ContieneMinusculas(cuerpo.PRIMER_APELLIDO))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer apellido contienen caracteres en minuscula",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (ContieneMinusculas(cuerpo.SEGUNDO_APELLIDO))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Segundo apellido contienen caracteres en minuscula",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (Regex.IsMatch(cuerpo.PRIMER_NOMBRE, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]"))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer nombre contienen caracteres especiales",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (Regex.IsMatch(cuerpo.SEGUNDO_NOMBRE, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]"))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Segundo nombre contienen caracteres especiales",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (Regex.IsMatch(cuerpo.PRIMER_APELLIDO, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]"))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Primer apellido contienen caracteres especiales",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (Regex.IsMatch(cuerpo.SEGUNDO_APELLIDO, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]"))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Segundo apellido contienen caracteres especiales",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (string.IsNullOrWhiteSpace(cuerpo.IDENTIFICACION))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Identificacion esta vacia",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            if (!cuerpo.IDENTIFICACION.All(char.IsLetterOrDigit) || cuerpo.IDENTIFICACION.Any(c => !char.IsLetterOrDigit(c)))
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Identificacion tiene un caracter no alfanumerico",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });

            if ( cuerpo.IDENTIFICACION.Length < 6 || cuerpo.IDENTIFICACION.Length > 14)
            {
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "La identificación no cumple con el formato requerido",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            }


            if (!Regex.IsMatch(cuerpo.EMBARGO.ToString(), "^[01]$"))
            {
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Valor de embargo no es 0 ni 1",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            }

            string añoMesPeriodo = cuerpo.PERIODO.Date.ToString("yyyyMM");

            if (request.Dct_FechaCreacion != añoMesPeriodo)
            {
                exceptions.Add(new ErrorCargueArchivo()
                {
                    DCT_ID = 0,
                    NUMERO_LINEA = linea,
                    CODIGO_INCIDENCIA = 0,
                    DESCRIPCION = "Periodo no coincide con el periodo del nombre",
                    TIPO_ERROR = tipoError,
                    ESTADO_ERROR = 1
                });
            }

            await Task.Yield();
        }

        private bool ContieneMinusculas(string cadena)
        {
            return cadena.Any(char.IsLower);
        }

        private string InfoColumna(int ParFinCodFinIni, int ParFinCodFinLen)
        {
            return ($" (Columna {ParFinCodFinIni} a la {ParFinCodFinIni + ParFinCodFinLen})");
        }

        public class ValidacionesTablas
        {
            public IEnumerable<Entities.Grado> Grados { get; set; }
            public IEnumerable<UnidadEjecutora> UnidadesEjecutoras { get; set; }
            public IEnumerable<TiposArchivos> TiposArchivos { get; set; }
            public IEnumerable<TipoIdentificacion> TiposIdentificacion { get; set; }
            public IEnumerable<Entities.Fuerzas> Fuerzas { get; set; }

        }

        public class MensajeValidaciones
        {
            public Mensaje Mensaje5 { get; set; }
            public Mensaje Mensaje6 { get; set; }
            public Mensaje Mensaje7 { get; set; }

        }
    }
}
