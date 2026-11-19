using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using static GA2.Domain.Core.ReportesBusinessLogic;
using GA2.Application.Main;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using Microsoft.Extensions.Logging;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Descompone la informacion y crea el documento
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public class DocumentoBusinessLogic : IDocumentoBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentoBusinessLogic> _logger;
        private readonly IDocumentoCargaRepository _documentoCargaRepository;
        private readonly ICargaArchivoBusinessLogic cargaArchivoBusinessLogic;
        private readonly IErrorCargueArchivoBusinessLogic _errorCargueArchivoBusinessLogic;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IReportesRepository _reportesRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ICarteraRepository _carteraRepository;

        public DocumentoBusinessLogic(IMapper mapper,
                                      ILogger<DocumentoBusinessLogic> logger,
                                      IDocumentoCargaRepository documentoCargaRepository,
                                      ICargaArchivoBusinessLogic cargaArchivoBusinessLogic,
                                      IErrorCargueArchivoBusinessLogic errorCargueArchivoBusinessLogic,
                                      IBlobStorageGenericRepository blobStorageGenericRepository,
                                      IReportesRepository reportesRepository,
                                      IClientRequestProvider iClientRequestProvider,
                                      ICarteraRepository carteraRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _documentoCargaRepository = documentoCargaRepository;
            this.cargaArchivoBusinessLogic = cargaArchivoBusinessLogic;
            _errorCargueArchivoBusinessLogic = errorCargueArchivoBusinessLogic;
            _blobStorageGenericRepository=blobStorageGenericRepository;
            _reportesRepository = reportesRepository;
            _iClientRequestProvider = iClientRequestProvider;
            _carteraRepository = carteraRepository;
        }

        /// <summary>
        /// Metodo que carga la informacion del documento a los componentes
        /// </summary>
        /// <param name="componentesDto">Componenes de cargua (header, body, finarchivo)</param>
        /// <param name="nombreArchivo">Nombre archivo</param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        public async Task<DocumentoDto> RegistroDocumento(ComponentesDTO componentesDto, Guid userId, CuentaStorage cuentaStorage, int estadoCargueDocumentoId, int estadoCargueArchivoId)
        {
            DocumentoDto documentoDtoo = new DocumentoDto()
            {
                DocumentoId = cuentaStorage.ID,
                Nombre = cuentaStorage.BLOBNAME,
                TipoDocumentoId = componentesDto.HEADER.FILE_TIPO,
                FechaInicialDocumento = DateTime.Now,
                EstadoDocumento = estadoCargueDocumentoId,
                EstadoArchivo = estadoCargueArchivoId,
                CreadoPor = userId,
                UnidadEjecutora = componentesDto.FILENAME.FILE_UNIDAD_EJECUTORA,
                FechaCreacion = DateTime.Now,
                Registros= componentesDto.FilasRegistros
            };

            return this._mapper.Map<DocumentoDto>(await this._documentoCargaRepository.CrearDocumentoCarga(this._mapper.Map<Documento>(documentoDtoo)));
        }

        public async Task<NombreArchivoDto> RegistroNombreArchivo(NombreArchivoDto Name)
        {
            return this._mapper.Map<NombreArchivoDto>(await this._documentoCargaRepository.CrearNombreArchivo(this._mapper.Map<FileNameReported>(Name)));
        }
        
        public async Task<EncabezadoArchivoDto> RegistroEnzabezadoArchivo(EncabezadoArchivoDto Header)
        {
            return this._mapper.Map<EncabezadoArchivoDto>(await this._documentoCargaRepository.CrearEncabezadoArchivo(this._mapper.Map<HeaderFileReported>(Header)));
        }

        public async Task<IEnumerable<CuerpoArchivoDto>> RegistroCuerpoArchivo(IEnumerable<CuerpoArchivoDto> Body)
        {
            return this._mapper.Map<IEnumerable<CuerpoArchivoDto>>(await this._documentoCargaRepository.CrearCuerpoArchivo(_mapper.Map<IEnumerable<BodyFile>>(Body)));
        }

        public async Task<FinDeArchivoDto> RegistroFinArchivo(FinDeArchivoDto Fin)
        {
            return this._mapper.Map<FinDeArchivoDto>(await this._documentoCargaRepository.CrearFinArchivo(this._mapper.Map<EndFileReported>(Fin)));
        }
        public async Task<FinDeArchivoDto> ObtenerFinArchivo(int dct_id)
        {
            return this._mapper.Map<FinDeArchivoDto>(await this._documentoCargaRepository.ObtenerFinArchivo(dct_id));
        }

        public async Task<ReconstruccionArchivoDto> ReconstruirArchivoNomina(int dct_id)
        {
            // 1. Consultar cada parte
            var nombreArchivoList = await this._documentoCargaRepository.ObtenerNombreArchivoReconstruido(dct_id);
            var headerList = await this._documentoCargaRepository.ObtenerHeaderArchivoReconstruido(dct_id);
            var bodyList = await this._documentoCargaRepository.ObtenerBodyArchivoReconstruido(dct_id);
            var finList = await this._documentoCargaRepository.ObtenerFinArchivoReconstruido(dct_id);

            // 2. Armar el contenido del archivo
            var nombreArchivo = nombreArchivoList.FirstOrDefault()?.NAN_NOMBRE_LOGICO ?? $"archivo_nomina_{dct_id}";
            var header = headerList.FirstOrDefault()?.HEADER_RECONSTRUIDO ?? "";
            var body = bodyList.Select(x => x.LINEA_BODY).ToList();
            var fin = finList.FirstOrDefault()?.LINEA_FIN_ARCHIVO ?? "";

            var lineas = new List<string>();
            if (!string.IsNullOrWhiteSpace(header)) lineas.Add(header);
            if (body != null && body.Any()) lineas.AddRange(body);
            if (!string.IsNullOrWhiteSpace(fin)) lineas.Add(fin);

            // 3. Generar archivo en memoria
            var contenidoBytes = System.Text.Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, lineas));
            var ms = new MemoryStream(contenidoBytes);
            var nombreArchivoConExtension = nombreArchivo.ToLower() + ".txt";

            // 4. Guardar archivo (ajusta blogStorageEstado y excepciones según tu contexto)
            CuentaStorage blob = null;
            var exceptions = new List<Exception>(); // si tienes una colección global de errores


                blob = await _blobStorageGenericRepository.GuardarArchivoCarpetaLocal(new CuentaStorage
                {
                    FILE = ms,
                    BLOBNAME = nombreArchivoConExtension,
                    MODULO = CargueNominaConstans.BlobName,
                    CONTAINERNAME = CargueNominaConstans.BlobName
                });
            

            // 5. Devolver resultado (puedes incluir más info si lo necesitas)
            var resultado = new ReconstruccionArchivoDto
            {
                NombreArchivo = nombreArchivo,
                Header = header,
                Body = body,
                FinArchivo = fin,
                //RutaArchivo = blob?.RUTA // si defines esta propiedad en el DTO
            };

            return resultado;
        }




        /// <summary>
        /// Obtiene todos los documentos cargados por estado documento
        /// </summary>
        /// <param name="estadoDocumento">Estado Documento</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Lista de documento por estado</returns>
        public async Task<Response<IEnumerable<DocumentoDto>>> ObtenerDocumentosCarga(int estadoDocumento)
        {
            return new Response<IEnumerable<DocumentoDto>>
            {
                Data = this._mapper.Map<IEnumerable<DocumentoDto>>(
                    await this._documentoCargaRepository.ObtenerDocumentosCarga(estadoDocumento))
            };
        }

        /// <summary>
        /// Obtiene los estados de los archivos
        /// </summary>
        /// <returns>Lista de estados documentos</returns>
        public async Task<Response<IEnumerable<EstadoArchivoDto>>> ObtenerEstadosArchivos()
        {
            return new Response<IEnumerable<EstadoArchivoDto>>
            {
                Data = this._mapper.Map<IEnumerable<EstadoArchivoDto>>(await this._documentoCargaRepository.ObtenerEstadosArchivos())
            };
        }


        public async Task<Response<IEnumerable<CausalRechazoNomina>>> CausalRechazoNomina()
        {

            IEnumerable<CausalRechazoNomina>  listaCausalesRechazos=await this._documentoCargaRepository.ConsultarCausalRechazoNomina();

            return new Response<IEnumerable<CausalRechazoNomina>>
            {
                Data = listaCausalesRechazos, 
                ReturnMessage = "Lista de causales de rechazo de nomina", 
                IsSuccess=true
            };
        }

        /// <summary>
        /// Obtiene todos los documentos cargados por estado documento
        /// </summary>
        /// <param name="estadoDocumento">Estado Documento</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Lista de documento por estado</returns>
        public async Task<List<DocumentoDto>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter)
        {
            return this._mapper.Map<List<DocumentoDto>>(
                await this._documentoCargaRepository.ConsultarDocumentosCarga(consultarDocumentosCargaFilter));
        }
        
        /// <summary>
        /// Obtiene todos los archivos cargados por estado
        /// </summary>
        /// <param name="estadoDocumento">Estado Documento</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>18/05/2021</date>
        /// <returns>Lista de documento por estado</returns>
        public async Task<List<ArchivoDto>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter)
        {
            var listaArchivos = this._mapper.Map<List<ArchivoDto>>(
                await this._documentoCargaRepository.ConsultarArchivosCargados(consultarArchivosCargadosFilter));           
            return listaArchivos;
        }

        public async Task<List<ArchivoConErrorDto>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir)
        {
            return this._mapper.Map<List<ArchivoConErrorDto>>(
                await this._documentoCargaRepository.GuardarArchivosCorregido(guardarArchivoCorregir));
        }

        public async Task<ResponseCorreccionMasiva> GuardarCorreccionMasiva(CorreccionMasivaArchivoDto guardarCorreccionMasiva, GuardarArchivoCorregirDto asignarDatosCorreccionMasiva)
        {
            return this._mapper.Map<ResponseCorreccionMasiva>(
                await this._documentoCargaRepository.GuardarArchivosCorregido(asignarDatosCorreccionMasiva));
        }

        /// <summary>
        /// Validar archivo de carga
        /// </summary>
        /// <param name="stream">Stream del archivo cargado</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="parametros">parametros para validar el archivo</param>
        /// <returns></returns>
        public async Task<ComponentesDTO> ValidarArchivoNomina(CargaNominaDTO request, Stream stream, string nombreArchivo, ParamCargueNomDTO parametros, List<ErrorCargueArchivo> exceptions, DocumentoDto documento= default) 
        {
            ComponentesDTO componentes = await cargaArchivoBusinessLogic.ValidacionArchivoNomina(request,
                                                                                                 stream,
                                                                                                 nombreArchivo,
                                                                                                 parametros,
                                                                                                 exceptions,
                                                                                                 documento);
            return componentes;
        }

        /// <summary>
        /// Obtiene los documentos por id
        /// </summary>
        /// <param name="documentoId">documentoId</param>
        /// <returns>Documento buscado por id</returns>
        /// <author>Oscar Julian Rojas</author>
        public async Task<Response<DocumentoDto>> ObtenerDocumentosPorId(int documentoId)
        {
            return new Response<DocumentoDto>
            {
                Data = this._mapper.Map<DocumentoDto>(
                   await this._documentoCargaRepository.ObtenerDocumentosPorId(documentoId))
            };
        }

        /// <summary>
        /// Cambiar estado a procesar documento cargue de nomina
        /// </summary>
        /// <param name="documentoId">id documento</param>
        /// <returns>Documento con cambio de estado</returns>
        /// <auhtor>Oscar Julian Rojas</auhtor>
        /// <date>30/09/2021</date>
        public async Task<Response<DocumentoDto>> CambiarEstadoAProcesar(int documentoId)
        {
            return new Response<DocumentoDto>
            {
                Data = this._mapper.Map<DocumentoDto>(
                   await this._documentoCargaRepository.CambiarEstadoAProcesar(documentoId))
            };
        }

        /// <summary>
        /// Obtener document cargado por nombre archivo
        /// </summary>
        /// <param name="nombre">Nombre archivo</param>
        /// <returns>Documento cargado</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<DocumentoDto>> ObtenerDocumentoPorNombre(string nombre)
        {
            return _mapper.Map<IEnumerable<DocumentoDto>>(
                   await this._documentoCargaRepository.ObtenerDocumentoPorNombre(nombre));
        }

        /// <summary>
        /// Eliminar archivo cargado al rechazar 
        /// </summary>
        /// <param name="documentoId">documentoId a eliminar</param>
        /// <returns>Estado de eliminacion</returns>
        public async Task<DocumentoDto> RechazarArchivoCargueNomina(int documentoId)
        {
            return _mapper.Map<DocumentoDto>(
                await _documentoCargaRepository.CambiarEstadoDocumentoCargadoARechazado(documentoId));
        }

        /// <summary>
        /// Eliminar archivo cargado al rechazar 
        /// </summary>
        /// <param name="documentoId">documentoId a eliminar</param>
        /// <returns>Estado de eliminacion</returns>
        public async Task<DocumentoDto> CambiarEstadoDocumentoRechazadoACargado(int documentoId)
        {
            return _mapper.Map<DocumentoDto>(
                await _documentoCargaRepository.CambiarEstadoDocumentoRechazadoACargado(documentoId));
        }

        /// <summary>
        /// Cambiar estado rechazo a cargado
        /// </summary>
        /// <param name="documentoId"></param>
        /// <returns></returns>
        public async Task<DocumentoDto> CambiarEstadoRechazoACargado(int documentoId)
        {
            return _mapper.Map<DocumentoDto>(
              await _documentoCargaRepository.CambiarEstadoDocumentoRechazadoACargado(documentoId));
        }

        public async Task<List<ArchivoConErrorDto>> ConsultarArchivoConError(ConsultarArchivosConErrorDto ConsultarArchivosConError)
        {
            return this._mapper.Map<List<ArchivoConErrorDto>>(
                await this._documentoCargaRepository.ConsultarArchivoConError(ConsultarArchivosConError));
        }

        public async Task<DocumentoDto> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError)
        {
            return this._mapper.Map<DocumentoDto>(
                await this._documentoCargaRepository.EliminarArchivosCargadosConErrores(eliminarArchivoConError));
        }
        public async Task<SolicitudNominaDto> CrearSolicitudNomina(SolicitudNominaDto request)
        {
            // Mapear la solicitud de nómina DTO a la entidad SolicitudNomina
            var solicitudNomina = _mapper.Map<SolicitudNominaDto>(request);

            try
            {
                // Llamar al método CrearSolicitudNomina del repositorio para crear la solicitud de nómina
                var solicitudCreada = await _documentoCargaRepository.CrearSolicitudNomina(solicitudNomina);

                // Mapear la solicitud de nómina creada de vuelta a DTO
                var solicitudCreadaDto = _mapper.Map<SolicitudNominaDto>(solicitudCreada);

                // Devolver la solicitud de nómina DTO en un objeto Response
                return solicitudCreadaDto;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la creación de la solicitud de nómina
                // Aquí puedes agregar el manejo de errores según sea necesario, como registrar el error o lanzar una excepción personalizada
                // Por ejemplo:
                _logger.LogError(ex, "Error al crear la solicitud de nómin" );
                throw;
            }
        }

        public async Task<Response<SolicitudNominaDto>> ObtenerSolicitudNomina(int documentoId)
        {
            return new Response<SolicitudNominaDto>
            {
                Data = this._mapper.Map<SolicitudNominaDto>(
                   await this._documentoCargaRepository.ObtenerSolicitudNomina(documentoId))
            };
        }  

        public async Task<SolicitudNominaDto> ActualizaSolicitudNomina(SolicitudNominaDto request)

        {
            var solicitudNomina = _mapper.Map<SolicitudNominaDto>(request);
            var solicitudCreada = await _documentoCargaRepository.ActualizaSolicitudNomina(solicitudNomina);
            var solicitudCreadaDto = _mapper.Map<SolicitudNominaDto>(solicitudCreada);
            return solicitudCreadaDto;

        }

        //public async Task<Response<List<ArchivoConErrorDto>>> ObtenerCuerpoNomina(int documentoId)
        //{
        //    return new Response<List<ArchivoConErrorDto>>
        //    {
        //        Data = mapper.Map<List<ArchivoConErrorDto>>(
        //           await this._documentoCargaRepository.ObtenerCuerpoNomina(documentoId))
        //    };
        //}

        //public async Task<Response<List<ArchivoConErrorDto>>> ObtenerCuerpoNomina(int documentoId)
        //{
        //    //return this._mapper.Map<Response<List<ArchivoConErrorDto>>> (
        //    //    await this._documentoCargaRepository.ObtenerCuerpoNomina(documentoId));

        //    return new Response<List<ArchivoConErrorDto>>
        //    {
        //        Data = this._mapper.Map<List<ArchivoConErrorDto>>(
        //           await this._documentoCargaRepository.ObtenerCuerpoNomina(documentoId))
        //    };
        //}

        public async Task<Response<List<ArchivoConErrorDto>>> ObtenerCuerpoNomina(int documentoId)
        {
            // Se obtienen los datos del repositorio y se mapean a objetos ArchivoConErrorDto
            var cuerpoNomina = await this._documentoCargaRepository.ObtenerCuerpoNomina(documentoId);
            var cuerpoNominaDto = this._mapper.Map<List<ArchivoConErrorDto>>(cuerpoNomina);

            // Se envuelve la lista mapeada en un objeto Response
            var response = new Response<List<ArchivoConErrorDto>>
            {
                Data = cuerpoNominaDto  // Se asignan los datos mapeados al campo Data del objeto Response
            };

            return response; // Se retorna el objeto Response con los datos mapeados
        }


        /// <summary>
        /// Cambiar estado a aprobar documento cargue de nomina
        /// </summary>
        /// <param name="documentoId">id documento</param>
        /// <returns>Documento con cambio de estado aprobado</returns>
        /// <auhtor>Alejandro Lara Miranda</auhtor>
        public async Task<Response<DocumentoDto>> AprobarCargueNominaSolicitud(int documentoId)
        {
            return new Response<DocumentoDto>
            {
                Data = this._mapper.Map<DocumentoDto>(
                   await this._documentoCargaRepository.AprobarCargueNominaSolicitud(documentoId))
            };
        }


        public async Task<Response<DescargarArchivoCorregidoSolicitudNomina>> ArchivoCorregidoSolicitudNomina(int documentoID)
        {
            try
            {
                var encabezado = await _reportesRepository.ConsultarArchivoEncabezadoNomina(documentoID);
                if (encabezado == null)
                {
                    return new Response<DescargarArchivoCorregidoSolicitudNomina>
                    {
                        ReturnMessage = "No se encontró información del encabezado para el documento proporcionado.",
                        Data = null,
                        IsSuccess = false
                    };
                }

                var cuerpo = await _reportesRepository.ConsultarArchivoCuerpoNomina(documentoID);
                if (cuerpo == null || !cuerpo.Any())
                {
                    return new Response<DescargarArchivoCorregidoSolicitudNomina>
                    {
                        ReturnMessage = "No se encontraron datos del cuerpo del archivo.",
                        Data = null,
                        IsSuccess = false
                    };
                }

                var fin = await _reportesRepository.ConsultarArchivoFinNomina(documentoID);
                if (fin == null)
                {
                    return new Response<DescargarArchivoCorregidoSolicitudNomina>
                    {
                        Data = null,
                        ReturnMessage = "No se encontró información del fin del archivo.",
                        IsSuccess = false
                    };
                }

                var resultado = new DescargarArchivoCorregidoSolicitudNomina
                {
                    EncabezadoArchivosNomina = encabezado,
                    CuerpoArchivosNomina = cuerpo,
                    FinArchivosNomina = fin
                };

                return new Response<DescargarArchivoCorregidoSolicitudNomina>
                {
                    Data = resultado,
                    ReturnMessage = "Mensaje corregido con exito",
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                return new Response<DescargarArchivoCorregidoSolicitudNomina>
                {
                    ReturnMessage = $"Error al generar archivo corregido: {ex.Message}", 
                    Data = null,
                    IsSuccess = false
                };
            }
        }

        public async Task<Response<string>> InsertarInformacionAfiliadoCargueNomina(DatosInformacionCliente datosClientes)
        {
            var errores = new List<string>();

            foreach (var numeroDocumento in datosClientes.cedulas)
            {
                try
                {
                    
                    // Simulamos tipo de documento fijo o puedes parametrizarlo si lo necesitas
                    int tipoDocumento = 1;

                    Response<ClienteDto> cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(tipoDocumento, numeroDocumento);

                    if (!cliente.IsSuccess || cliente.Data == null)
                    {
                        errores.Add($"No se encontró cliente con documento: {numeroDocumento}");
                        continue;
                    }

                    UnidadEjecutora unidadEjecutora = await _carteraRepository.ConsultarUnidadEjecutoraPorId(cliente.Data.IdUnidadEjecutora);

                    InformacionAfiliadoCargueNomina afiliadoInformacion = new InformacionAfiliadoCargueNomina();
                    List<CuentaDto> listaCuentas = cliente.Data.Cuentas;

                    if (datosClientes.unidadEjecutoraID == cliente.Data.IdUnidadEjecutora)
                    {
                        afiliadoInformacion.ICN_AFILIADO = $"{cliente.Data.PrimerNombre} {cliente.Data.SegundoNombre} {cliente.Data.PrimerApellido} {cliente.Data.SegundoApellido}";
                        afiliadoInformacion.PrimerNombre = cliente.Data.PrimerNombre;
                        afiliadoInformacion.SegundoNombre = cliente.Data.SegundoNombre;
                        afiliadoInformacion.PrimerApellido = cliente.Data.PrimerApellido;
                        afiliadoInformacion.SegundoApellido = cliente.Data.SegundoApellido; 
                        afiliadoInformacion.ICN_UNIDAD_EJECUTORA = unidadEjecutora.UEJ_NOMBRE;
                        afiliadoInformacion.ICN_NUMERO_DOCUMENTO = cliente.Data.Identificacion;


                        foreach (var item in listaCuentas)
                        {
                            decimal totalConcepto = 0;

                            afiliadoInformacion.ICN_TIPO_CUENTA = item.TipoCuenta;
                            afiliadoInformacion.ICN_ESTADO_CUENTA = item.EstadoCuenta;


                            if (item.TipoCuenta.Equals("CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS") && item.EstadoCuenta.Equals("VIGENTE"))
                            {

                                foreach (var concepto in item.Conceptos)
                                {
                                    decimal saldo = Convert.ToDecimal(concepto.saldo);
                                    switch (concepto.ConceptoDescripcion)
                                    {
                                        case "AHORROS OBLIGATORIOS":
                                            afiliadoInformacion.ICN_AHORROS_OBLIGATORIOS = saldo;
                                            break;
                                        case "AHORROS VOLUNTARIOS":
                                            afiliadoInformacion.ICN_AHORROS_VOLUNTARIOS = saldo;
                                            break;
                                        case "CESANTÍAS":
                                            afiliadoInformacion.ICN_CESANTIAS = saldo;
                                            break;
                                        case "CESANTÍAS RETROACTIVOS":
                                            afiliadoInformacion.ICN_CESANTIAS_RETROACTIVOS = saldo;
                                            break;
                                        case "INTERESES APORTES":
                                            afiliadoInformacion.ICN_INTERESES_APORTES = saldo;
                                            break;
                                        case "INTERESES CESANTÍAS":
                                            afiliadoInformacion.ICN_INTERESES_CESANTIAS = saldo;
                                            break;
                                        case "CESANTÍAS DOCEAVAS":
                                            afiliadoInformacion.ICN_CESANTIAS_DOCEAVAS = saldo;
                                            break;
                                        case "AHORROS RETROACTIVOS":
                                            afiliadoInformacion.ICN_AHORROS_RETROACTIVOS = saldo;
                                            break;
                                    }

                                    totalConcepto += saldo;
                                }

                                afiliadoInformacion.ICN_TOTAL_CONCEPTO = totalConcepto;

                                await _documentoCargaRepository.InsertarInformacionAfiliadoCargueNomina(afiliadoInformacion);

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Error al procesar la cédula {numeroDocumento}: {ex.Message}");
                }
            }

            if (errores.Count == 0)
            {
                return new Response<string>
                {
                    IsSuccess = true,
                    ReturnMessage = "Todos los afiliados fueron procesados exitosamente.",
                    Data = "OK"
                };
            }
            else
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Se presentaron errores al procesar algunos afiliados.",
                    Data = string.Join("; ", errores)
                };
            }
        }



    }
}
