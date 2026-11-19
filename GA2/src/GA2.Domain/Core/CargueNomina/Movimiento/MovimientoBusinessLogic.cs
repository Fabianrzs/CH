using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using GA2.Application.Main.TU;
using System.ComponentModel;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Application.Dto.CargaNomina;
using SixLabors.ImageSharp.Memory;
using System.Net.Mime;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using GA2.Domain.Entities.ServiciosDinamicosEnt;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Logica de negocio para el movimiento de nomina
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public class MovimientoBusinessLogic : IMovimientoBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IParamCargueNominaRepository _paramCargueNominaRepository;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IDocumentoBusinessLogic _documentoBusinessLogic;
        private readonly IMensajeRepository _mensajeRepository;
        private readonly IErrorCargueArchivoBusinessLogic _errorCargueArchivoBusinessLogic;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IServiciosDinamicosRepository _serviciosDinamicosRepository;
        private readonly IDocumentoCargaRepository _documentoCargaRepository;

        private const int version = 1;
        private const int ZeroPosition = 0;
        private const int LastPosition = 27;
        private const int EstadoCargado = 1;
        private const int EstadoCargadoArchivo = 2;
        private const int EstadoPorProcesar = 6;
        private const int CargadoConErrores = 8;
        private const int CargadoConErroresEstadoArchivo = 1;

        /// <summary>
        /// Movimiento de logica de negocio carga de archivos
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="paramCargueNominaRepository"></param>
        /// <param name="blobStorageGenericRepository"></param>
        /// <param name="documentoBusinessLogic"></param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>22/05/2021</date>
            public MovimientoBusinessLogic(IMapper mapper,
                                       IParamCargueNominaRepository paramCargueNominaRepository,
                                       IBlobStorageGenericRepository blobStorageGenericRepository,
                                       IDocumentoBusinessLogic documentoBusinessLogic,
                                       IClientRequestProvider clientRequestProvider,
                                       IMensajeRepository mensajeRepository,
                                       IErrorCargueArchivoBusinessLogic errorCargueArchivoBusinessLogic,
                                       IClientRequestProvider iClientRequestProvider,
                                       IServiciosDinamicosRepository serviciosDinamicosRepository,
                                       IDocumentoCargaRepository documentoCargaRepository
                )
        {
            _mapper = mapper;
            _paramCargueNominaRepository = paramCargueNominaRepository;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _documentoBusinessLogic = documentoBusinessLogic;
            _mensajeRepository = mensajeRepository;
            _errorCargueArchivoBusinessLogic = errorCargueArchivoBusinessLogic;
            _iClientRequestProvider = iClientRequestProvider;
            _serviciosDinamicosRepository = serviciosDinamicosRepository;
            _documentoCargaRepository = documentoCargaRepository;
        }

        public async Task<Response<IEnumerable<DocumentoDto>>> CargarArchivoNominaTemp(CargaNominaDTO request, Guid userId)
        {
            var file = request.File;
            if (file == null) throw new ArgumentNullException(nameof(file));

            var parametros = this._mapper.Map<ParamCargueNomDTO>(
                await this._paramCargueNominaRepository.ObtenerParametrosCargueNomina(version));

            string nameFile = file.FileName.Substring(ZeroPosition, LastPosition);
            //Variable de los errores de procesamiento
            List<ErrorCargueArchivo> exceptions = new();
            var componentes = await _documentoBusinessLogic.ValidarArchivoNomina(request,
                                                                                 file.OpenReadStream(),
                                                                                 nameFile,
                                                                                 parametros,
                                                                                 exceptions);

            // Valida si la sumatoria de aportes y el valor en la seccion fin coinciden
            if (componentes.END.SUM_CONTRIBUTIONS == "error no coincide la sumatoria de aportes") throw new Exception(CargueNominaConstans.ErrorCargaArchivoSumaAportes);

            // Logica para generar archivo descargable con FileResult buscar ejemplo en el código
            var fileJson = JsonConvert.SerializeObject(componentes);
            byte[] buffer = Encoding.ASCII.GetBytes(fileJson);
            MemoryStream ms = new(buffer);

            var documentsExist = await _documentoBusinessLogic.ObtenerDocumentoPorNombre(nameFile.ToLower());
            foreach (var documentExist in documentsExist)
            {
                var finArchivo = await this._documentoBusinessLogic.ObtenerFinArchivo(documentExist.DocumentoId);
                if (finArchivo.SUM_CONTRIBUTIONS == componentes.END.SUM_CONTRIBUTIONS&& documentExist.EstadoDocumento==EstadoCargado)
                    throw new Exception(CargueNominaConstans.ErrorCargaArchivoExistente);
            }
            
            CuentaStorage blob = new CuentaStorage();
            blob.BLOBNAME = nameFile.ToLower();

            if (!exceptions.Any())
            {
                blob = await _blobStorageGenericRepository.SubirArchivoStorage(new CuentaStorage
                {
                    FILE = ms,
                    BLOBNAME = nameFile.ToLower(),
                    MODULO = CargueNominaConstans.BlobName,
                    CONTAINERNAME = CargueNominaConstans.BlobName
                });
            }

            if (blob != null)
            {
                //if (documentExist is not null)
                //    componentes.DOCUMENTO = await this._documentoBusinessLogic.CambiarEstadoRechazoACargado(documentExist.DocumentoId);
                if (exceptions.Any())
                {
                    componentes.DOCUMENTO = await this._documentoBusinessLogic.RegistroDocumento(componentes, userId, blob, CargadoConErrores, CargadoConErroresEstadoArchivo);
                    componentes.FILENAME = await this._documentoBusinessLogic.RegistroNombreArchivo(componentes.FILENAME);
                    componentes.HEADER = await this._documentoBusinessLogic.RegistroEnzabezadoArchivo(componentes.HEADER);
                    componentes.BODY = await this._documentoBusinessLogic.RegistroCuerpoArchivo(componentes.BODY);
                    componentes.END = await this._documentoBusinessLogic.RegistroFinArchivo(componentes.END);
                    await CrearErroresCargueArchivo(componentes.DOCUMENTO.DocumentoId, exceptions);
                    var documentosCargados = await ObtenerDocumentosCarga(CargadoConErrores);
                    documentosCargados.ReturnMessage = CargueNominaConstans.ErrorCargaArchivo;
                    return documentosCargados;
                }
                else
                {
                    componentes.DOCUMENTO = await this._documentoBusinessLogic.RegistroDocumento(componentes, userId, blob, EstadoCargado, EstadoCargadoArchivo);
                    componentes.FILENAME = await this._documentoBusinessLogic.RegistroNombreArchivo(componentes.FILENAME);
                    componentes.HEADER = await this._documentoBusinessLogic.RegistroEnzabezadoArchivo(componentes.HEADER);
                    componentes.BODY = await this._documentoBusinessLogic.RegistroCuerpoArchivo(componentes.BODY);
                    componentes.END = await this._documentoBusinessLogic.RegistroFinArchivo(componentes.END);

                    var documentosCargados = await ObtenerDocumentosCarga(EstadoCargado);
                    documentosCargados.ReturnMessage = CargueNominaConstans.ArchivoCargado;
                    return documentosCargados;
                }
            }

            return new Response<IEnumerable<DocumentoDto>> { Data = null, ReturnMessage = CargueNominaConstans.ErrorCargaArchivo };
        }

        /// <summary>
        /// Metodo para relaizar la validacion de la nomina cargada
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        public async Task<Response<IEnumerable<ArchivoDto>>> CargarArchivoNomina(CargaNominaDTO request, Guid userId)
        {
            var file = request.File;
            if (file == null) throw new ArgumentNullException(nameof(file));

            var parametros = this._mapper.Map<ParamCargueNomDTO>(await this._paramCargueNominaRepository.ObtenerParametrosCargueNomina(version));

            string nameFile = file.FileName;
            //string nameFile = file.FileName.Substring(ZeroPosition, LastPosition);
            //Variable de los errores de procesamiento
            List<ErrorCargueArchivo> exceptions = new();

            var componentes = await _documentoBusinessLogic.ValidarArchivoNomina(request,
                                                                                 file.OpenReadStream(),
                                                                                 nameFile,
                                                                                 parametros,
                                                                                 exceptions);
            // Valida si la sumatoria de aportes y el valor en la seccion fin coinciden
            if (componentes.END.FILE_CONTROL_INDICATOR == "error no coincide la sumatoria de aportes") throw new Exception(CargueNominaConstans.ErrorCargaArchivoSumaAportes);

            // Logica para generar archivo descargable con FileResult buscar ejemplo en el código
            //var fileJson = JsonConvert.SerializeObject(componentes);
            //byte[] buffer = Encoding.ASCII.GetBytes(file.to);
            MemoryStream ms =new MemoryStream();
            await file.CopyToAsync(ms);

            var documentName = nameFile.Split('.');

            var documentsExist = await _documentoBusinessLogic.ObtenerDocumentoPorNombre(documentName[0].ToLower());

            foreach(var documentExist in documentsExist)
            {
                var finArchivo= await this._documentoBusinessLogic.ObtenerFinArchivo(documentExist.DocumentoId);
                if (finArchivo != null)
                {
                    if (finArchivo.SUM_CONTRIBUTIONS==componentes.END.SUM_CONTRIBUTIONS)
                    throw new Exception(CargueNominaConstans.ErrorCargaArchivoExistente);
                }
            }
            int documents= documentsExist.Count();
            if (documents>0)
            {
                nameFile = string.Format("{0}({1}).{2}", documentName[0], documents, documentName[1]);
            }

            ServiciosDinamicos blogStorageEstado = await _serviciosDinamicosRepository.ConsultarEstadoServicio();
            CuentaStorage blob = new CuentaStorage();
            blob.BLOBNAME = nameFile.ToLower();
            if (blogStorageEstado.SRD_ESTADO)
            {
                if (!exceptions.Any())
                {
                    blob = await _blobStorageGenericRepository.SubirArchivoStorage(new CuentaStorage
                    {
                        FILE = ms,
                        BLOBNAME = nameFile.ToLower(),
                        MODULO = CargueNominaConstans.BlobName,
                        CONTAINERNAME = CargueNominaConstans.BlobName
                    });
                }
            }
            else
            {
                if (!exceptions.Any())
                {
                    blob = await _blobStorageGenericRepository.GuardarArchivoCarpetaLocal(new CuentaStorage
                    {
                        FILE = ms,
                        BLOBNAME = nameFile.ToLower(),
                        MODULO = CargueNominaConstans.BlobName,
                        CONTAINERNAME = CargueNominaConstans.BlobName
                    });
                }
            }

            
               
            
            



            if (blob != null)
            {
                //if (documentExist is not null)
                //    componentes.DOCUMENTO = await this._documentoBusinessLogic.CambiarEstadoRechazoACargado(documentExist.DocumentoId);
                if (exceptions.Any())
                {
                    componentes.DOCUMENTO = await this._documentoBusinessLogic.RegistroDocumento(componentes, userId, blob, CargadoConErrores, CargadoConErroresEstadoArchivo);
                    componentes.FILENAME = await this._documentoBusinessLogic.RegistroNombreArchivo(componentes.FILENAME);
                    componentes.HEADER = await this._documentoBusinessLogic.RegistroEnzabezadoArchivo(componentes.HEADER);
                    componentes.BODY = await this._documentoBusinessLogic.RegistroCuerpoArchivo(componentes.BODY);
                    componentes.END = await this._documentoBusinessLogic.RegistroFinArchivo(componentes.END);
                    await CrearErroresCargueArchivo(componentes.DOCUMENTO.DocumentoId, exceptions);

                    if(componentes.DOCUMENTO.TipoDocumentoId!=request.Tdc_Id)
                    {
                        request.Dct_Id = componentes.DOCUMENTO.DocumentoId;
                        await this._documentoBusinessLogic.EliminarArchivosCargadosConErrores(request);
                        throw new Exception("Verifique el tipo de archivo.");
                    }
                    var filtro = new ConsultarArchivosCargadosFilterDTO
                    {
                        Dct_Id = componentes.DOCUMENTO.DocumentoId
                    };
                    var documentosCargados2 = await ConsultarArchivosCargados(filtro);

                    var erroresArchivo = await ConsultarErroresNominaPorId(new ConsultarArchivosIdDto
                    {
                        Dct_Id = componentes.DOCUMENTO.DocumentoId
                    });

                    var documentoConErrores = documentosCargados2.Data.FirstOrDefault(doc => doc.IdDocumento == filtro.Dct_Id);

                    if (documentoConErrores != null)
                    {
                        documentoConErrores.Errores = erroresArchivo.Data;
                    }

                    NotificacionCargueNomina notificacionCargue = new NotificacionCargueNomina();

                    notificacionCargue.DTC_ID = Convert.ToInt32(componentes.DOCUMENTO.DocumentoId);
                    notificacionCargue.USR_USUARIO = userId;
                    notificacionCargue.CRN_ID = Guid.NewGuid();
                    notificacionCargue.ESTADO = documentoConErrores.Errores.Count == 0? "CORREGIDO" : "CARGADO";
                    notificacionCargue.UEJ_USUARIO = Convert.ToInt32(request.Uej_Id);
                    notificacionCargue.NRN_TIPO_ARCHIVO = Convert.ToInt32(request.Tdc_Id);
                    notificacionCargue.NRN_PERIODO = request.Dct_FechaCreacion;
                    notificacionCargue.ERRORES = documentoConErrores.Errores.Count;


                    await _documentoCargaRepository.RegistrarNotificacionNomina(notificacionCargue);

                    //MemoryStream mst = new MemoryStream();
                    //blob.FILE.CopyTo(mst);
                    //documentosCargados2.Data[0].File = mst.ToArray();
                    return new Response<IEnumerable<ArchivoDto>> { Data = documentosCargados2.Data, ReturnMessage = CargueNominaConstans.ErrorCargaArchivo };
                }
                else
                {
                    componentes.DOCUMENTO = await this._documentoBusinessLogic.RegistroDocumento(componentes, userId, blob, EstadoCargado, EstadoCargadoArchivo);
                    componentes.FILENAME = await this._documentoBusinessLogic.RegistroNombreArchivo(componentes.FILENAME);
                    componentes.HEADER = await this._documentoBusinessLogic.RegistroEnzabezadoArchivo(componentes.HEADER);
                    componentes.BODY = await this._documentoBusinessLogic.RegistroCuerpoArchivo(componentes.BODY);
                    componentes.END = await this._documentoBusinessLogic.RegistroFinArchivo(componentes.END);
                    if(componentes.DOCUMENTO.TipoDocumentoId != request.Tdc_Id)
                    {
                        request.Dct_Id = componentes.DOCUMENTO.DocumentoId;
                        await this._documentoBusinessLogic.EliminarArchivosCargadosConErrores(request);
                        throw new Exception("Verifique el tipo de archivo.");
                    }

                    var filtro = new ConsultarArchivosCargadosFilterDTO { Dct_Id = componentes.DOCUMENTO.DocumentoId };
                    var documentosCargado = await ConsultarArchivosCargados(filtro);
                    if (documentosCargado.Data.Count > 0)
                    {
                        documentosCargado.Data[0].Errores = new List<ErrorCargueArchivoDto>();
                    }
                    var erroresArchivo = await ConsultarErroresNominaPorId(new ConsultarArchivosIdDto { Dct_Id = componentes.DOCUMENTO.DocumentoId });
                    var documentoConErrores = documentosCargado.Data.FirstOrDefault(doc => doc.IdDocumento == filtro.Dct_Id);
                    if (documentoConErrores != null)
                    {
                        documentoConErrores.Errores = erroresArchivo.Data;
                    }
                    //MemoryStream mst = new MemoryStream();
                    //blob.FILE.CopyTo(mst);
                    //documentosCargado.Data[0].File= mst.ToArray();
                    NotificacionCargueNomina notificacionCargue = new NotificacionCargueNomina();

                    notificacionCargue.DTC_ID = Convert.ToInt32(componentes.DOCUMENTO.DocumentoId);
                    notificacionCargue.USR_USUARIO = userId;
                    notificacionCargue.CRN_ID = Guid.NewGuid();
                    notificacionCargue.ESTADO = documentoConErrores.Errores.Count == 0 ? "CORREGIDO" : "CARGADO";
                    notificacionCargue.UEJ_USUARIO = Convert.ToInt32(request.Uej_Id);
                    notificacionCargue.NRN_TIPO_ARCHIVO = Convert.ToInt32(request.Tdc_Id);
                    notificacionCargue.NRN_PERIODO = request.Dct_FechaCreacion;
                    notificacionCargue.ERRORES = documentoConErrores.Errores.Count;


                    await _documentoCargaRepository.RegistrarNotificacionNomina(notificacionCargue);

                    return new Response<IEnumerable<ArchivoDto>> { Data = documentosCargado.Data, ReturnMessage = CargueNominaConstans.ArchivoCargado };
                }
            }


           

            return new Response<IEnumerable<ArchivoDto>> { Data = null, ReturnMessage = CargueNominaConstans.ArchivoCargado };
        }
        private async Task<List<ErrorCargueArchivo>> CrearErroresCargueArchivo(int documentoId, List<ErrorCargueArchivo> exceptions)
        {
            exceptions.ForEach(c => c.DCT_ID = documentoId);
            return await _errorCargueArchivoBusinessLogic.CrearErroresCargueArchivo(exceptions);
        }

        /// <summary>
        /// Metodo para rechazar la nomina cargada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        public async Task<Response<IEnumerable<DocumentoDto>>> RechazarCargueNomina(IEnumerable<DocumentoDto> documentos)
        {
            ServiciosDinamicos blobStorageActivo = await _serviciosDinamicosRepository.ConsultarEstadoServicio();

            if (documentos == null) throw new ArgumentNullException(nameof(documentos));
            foreach (var item in documentos)
            {
                await _documentoBusinessLogic.RechazarArchivoCargueNomina(item.DocumentoId);

                NotificacionCargueNomina notificacionCargue = new NotificacionCargueNomina();

                notificacionCargue.DTC_ID = Convert.ToInt32(item.DocumentoId);
                notificacionCargue.USR_USUARIO = item.UsuarioID;
                notificacionCargue.CRN_ID = item.CausalRechazoID;
                notificacionCargue.ESTADO = "RECHAZADO";
                notificacionCargue.UEJ_USUARIO = Convert.ToInt32(item.UnidadEjecutora);
                notificacionCargue.NRN_TIPO_ARCHIVO = Convert.ToInt32(item.TipoDocumentoId);
                notificacionCargue.NRN_PERIODO = item.FechaInicialDocumento.ToString();
                await _documentoCargaRepository.RegistrarNotificacionNomina(notificacionCargue); 

                if (blobStorageActivo.SRD_ESTADO)
                {
                    await _blobStorageGenericRepository.EliminarArchivoStorage("carguenomina", item.Nombre);
                }
                else
                {
                    await _blobStorageGenericRepository.EliminarArchivoLocalAsync(item.Nombre);
                }
            }

            var mensaje = await _mensajeRepository.ObtenerMensaje("32");
            var listaDocumento = await this._documentoBusinessLogic.ObtenerDocumentosCarga(1);

            return new Response<IEnumerable<DocumentoDto>> { Data = listaDocumento?.Data, ReturnMessage = mensaje.MSJ_MENSAJE };
        }

        /// <summary>
        /// Metodo para rechazar la nomina cargada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        public async Task<Response<IEnumerable<ArchivoDto>>> AprobarCargueNomina(IEnumerable<DocumentoDto> documentos)
        {
            if (documentos == null) throw new ArgumentNullException(nameof(documentos));
            foreach (var item in documentos)
            {
               var nueva = await _documentoBusinessLogic.CambiarEstadoAProcesar(item.DocumentoId);
            }

            var filtro = new ConsultarArchivosCargadosFilterDTO { Dct_Id = documentos.First().DocumentoId };
            var documentosCargado = await ConsultarArchivosCargados(filtro);

            var mensaje = await _mensajeRepository.ObtenerMensaje("25");
            var listaDocumento = await this._documentoBusinessLogic.ObtenerDocumentosCarga(1);

            return new Response<IEnumerable<ArchivoDto>> { Data = documentosCargado?.Data, ReturnMessage = mensaje.MSJ_MENSAJE };
        }

        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="estadoDocumento">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        public async Task<Response<IEnumerable<DocumentoDto>>> ObtenerDocumentosCarga(int estadoDocumento = EstadoPorProcesar)
        {
            return await this._documentoBusinessLogic.ObtenerDocumentosCarga(estadoDocumento);
        }

        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="consultarDocumentosCargaFilter">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        public async Task<Response<List<DocumentoDto>>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter)
        {
            List<DocumentoDto> documentos = await this._documentoBusinessLogic.ConsultarDocumentosCarga(consultarDocumentosCargaFilter);
            List<ErrorCargueArchivoDto> erroresCargueArchivo = this._mapper.Map<List<ErrorCargueArchivoDto>>(await _errorCargueArchivoBusinessLogic.ConsultarErroresCargueArchivo());
            documentos.ForEach(d => d.Errores = (erroresCargueArchivo.Where(e => e.DctId == d.DocumentoId).Select(x => x).ToList()));
            return new Response<List<DocumentoDto>> { Data = documentos };
        }

        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="consultarArchivosCargadosFilter">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        public async Task<Response<List<ArchivoDto>>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter)
        {
            List<ArchivoDto> archivos = await this._documentoBusinessLogic.ConsultarArchivosCargados(consultarArchivosCargadosFilter);           
            return new Response<List<ArchivoDto>> { Data = archivos };
        }

        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="guardarArchivoCorregir">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        public async Task<Response<List<ArchivoConErrorDto>>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir)
        {

            Response<List<ArchivoConErrorDto>> ConsultaArchivoConError = null; 

            if (guardarArchivoCorregir.Corregir == 4)
            {
                ConsultarArchivosConErrorDto consultarArchivosConErrorDto = new ConsultarArchivosConErrorDto
                {
                    Dct_Id = guardarArchivoCorregir.Dct_id,
                    Valor = 5
                };
                ConsultaArchivoConError = await this.ConsultarArchivoConError(consultarArchivosConErrorDto); 
            }

            var nombreArchivo = await this._errorCargueArchivoBusinessLogic.ObtenerNombreArchivoPorId((int)guardarArchivoCorregir.Dct_id);

            var validarCargueArchivo = await _errorCargueArchivoBusinessLogic.ValidarCargueArchivo(guardarArchivoCorregir, nombreArchivo, ConsultaArchivoConError);


            List<ArchivoConErrorDto> archivos = await this._documentoBusinessLogic.GuardarArchivosCorregido(guardarArchivoCorregir);
            List<ErrorCargueArchivoDto> actualizarEstado = await _errorCargueArchivoBusinessLogic.ActualizarEstadoErrorArchivo(guardarArchivoCorregir);

            var archivoCorregido = await this._errorCargueArchivoBusinessLogic.ObtenerTotalErroresPorArchivo((int)guardarArchivoCorregir.Dct_id);

            if (archivoCorregido.TotalEstadoError == 0)
            {
                await _errorCargueArchivoBusinessLogic.ActualizarEstadoDocumento((int)guardarArchivoCorregir.Dct_id, EstadoCargado, EstadoCargadoArchivo);
            }

            return new Response<List<ArchivoConErrorDto>> { Data = archivos };
        }

        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="guardarArchivoCorregir">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        public async Task<ResponseCorreccionMasiva> GuardarCorreccionMasiva(CorreccionMasivaArchivoDto guardarCorreccionMasiva)
        {
            ResponseCorreccionMasiva mensaje = new ResponseCorreccionMasiva();

            try
            {
                Response<List<ArchivoConErrorDto>> ConsultaArchivoConError = null;


                if (guardarCorreccionMasiva.Valor == 4)
                {
                    ConsultarArchivosConErrorDto consultarArchivosConErrorDto = new ConsultarArchivosConErrorDto
                    {
                        Dct_Id = guardarCorreccionMasiva.Dct_id,
                        Valor = 5
                    };
                    ConsultaArchivoConError = await this.ConsultarArchivoConError(consultarArchivosConErrorDto);
                }
                var nombreArchivo = await this._errorCargueArchivoBusinessLogic.ObtenerNombreArchivoPorId((int)guardarCorreccionMasiva.Dct_id);

                await _errorCargueArchivoBusinessLogic.GuardarDatosCorreccionMasiva(guardarCorreccionMasiva, nombreArchivo, ConsultaArchivoConError);
                var archivoCorregido = await this._errorCargueArchivoBusinessLogic.ObtenerTotalErroresPorArchivo((int)guardarCorreccionMasiva.Dct_id);

                if (archivoCorregido.TotalEstadoError == 0)
                {

                    NotificacionCargueNomina notificacionCargue = new NotificacionCargueNomina();

                    notificacionCargue.DTC_ID = Convert.ToInt32(guardarCorreccionMasiva.Dct_id);
                    notificacionCargue.USR_USUARIO = Guid.Parse("16EE93D8-D06D-40D9-BF5F-500656929EC5");
                    notificacionCargue.ESTADO = "CORREGIDO";
                    notificacionCargue.CRN_ID = Guid.NewGuid();
                    notificacionCargue.UEJ_USUARIO = Convert.ToInt32(guardarCorreccionMasiva.Uej_Id);
                    notificacionCargue.NRN_TIPO_ARCHIVO = guardarCorreccionMasiva.idTipoArchivo;
                    notificacionCargue.NRN_PERIODO = guardarCorreccionMasiva.Dct_Periodo.ToString();


                    await _documentoCargaRepository.RegistrarNotificacionNomina(notificacionCargue);

                    await _errorCargueArchivoBusinessLogic.ActualizarEstadoDocumento((int)guardarCorreccionMasiva.Dct_id, EstadoCargado, EstadoCargadoArchivo);
                    var archivoReconstruido = await _documentoBusinessLogic.ReconstruirArchivoNomina((int)guardarCorreccionMasiva.Dct_id);



                }

                mensaje.Success = true;
                mensaje.Message = "La corrección masiva se guardó correctamente.";
            }
            catch (Exception ex)
            {
                mensaje.Success = false;
                mensaje.Message = $"Error al guardar la corrección masiva. Detalles: {ex.Message}";
            }

            return mensaje;
        }

        /// <summary>
        /// Obtener estados archivo
        /// </summary>
        /// <returns>Lista de estados archivo</returns>
        public async Task<Response<IEnumerable<EstadoArchivoDto>>> ObtenerEstadosArchivos()
        {
            return await this._documentoBusinessLogic.ObtenerEstadosArchivos();
        }

        public async Task<FileResult> DescargarArchivoErroresCargueNomina(int documentoId)
        {
            var listErrores = await _errorCargueArchivoBusinessLogic.ConsultarErroresPorCargueArchivo(documentoId);
            StringBuilder builder1 = new StringBuilder();
            builder1.Append($"<html xmlns:o='urn:schemas-microsoft-com:office:office '");
            builder1.Append(@"xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'> ");
            builder1.Append($"<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/><body>");
            builder1.AppendLine("<table><tr><td>Linea</td><td>Código de incidencia</td><td>Descripción</td><td>Tipo error</td></tr>");
            foreach (var error in listErrores)
            {
                builder1.Append($"<tr><td>{error.NUMERO_LINEA}</td><td>{error.CODIGO_INCIDENCIA}</td><td>{error.DESCRIPCION}</td><td>{error.TIPO_ERROR}</td></tr>");
            }
            builder1.AppendLine("</table></body></html5>");

            byte[] buffer = Encoding.UTF8.GetBytes(builder1.ToString());
            MemoryStream ms = new MemoryStream(buffer);
            var file = new FileStreamResult(ms, "application/vnd.ms-excel");
            file.FileDownloadName = Guid.NewGuid().ToString() + ".xls";
            await file.FileStream.FlushAsync();

            return file;
        }
        public async Task<FileResult> DescargarArchivoNomina(int documentoId)
        {
            var documento = await _documentoBusinessLogic.ObtenerDocumentosPorId(documentoId);
            var archivo =await _blobStorageGenericRepository.DescargarArchivoStorageStream(CargueNominaConstans.BlobName, documento.Data.Nombre);
            //var a = new MemoryStream(archivo.Details.ContentHash);
            var file = new FileStreamResult(archivo,MediaTypeNames.Text.Plain);
            file.FileDownloadName = documento.Data.Nombre;
            await file.FileStream.FlushAsync();

            return file;
        }


        public async Task<Response<List<ArchivoConErrorDto>>> ConsultarArchivoConError(ConsultarArchivosConErrorDto consultarArchivosConError)
        {
            List<ArchivoConErrorDto> archivosConError = await this._documentoBusinessLogic.ConsultarArchivoConError(consultarArchivosConError);
           
            return new Response<List<ArchivoConErrorDto>> { Data = archivosConError };
        }

        public async Task<Response<DocumentoDto>> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError)
        {
            DocumentoDto archivosConError = await this._documentoBusinessLogic.EliminarArchivosCargadosConErrores(eliminarArchivoConError);
            return new Response<DocumentoDto> { Data = archivosConError };
        }

        public async Task<Response<List<ErrorCargueArchivoDto>>> ConsultarErroresNominaPorId(ConsultarArchivosIdDto documentoId)
        {
            int dctId = documentoId.Dct_Id ?? 0; // Convierte int? a int, usando 0 como valor predeterminado si es nulo

            var listErrores = await _errorCargueArchivoBusinessLogic.ConsultarErroresPorCargueArchivo(dctId);

            
            var listErroresDto = listErrores.Select(error =>
                new ErrorCargueArchivoDto
                {
                    NmnId = error.NMN_ID,
                    DctId = error.DCT_ID,
                    NumeroLinea = error.NUMERO_LINEA,
                    InconsistenciasNombre = error.INCONSISTENCIAS_NOMBRE,
                    InconsistenciasEncabezado = error.INCONSISTENCIAS_ENCABEZADO,
                    InconsistenciasCuerpo = error.INCONSISTENCIAS_CUERPO,
                    InconsistenciasFin = error.INCONSISTENCIAS_FIN,
                    CodigoIncidencia = error.CODIGO_INCIDENCIA,
                    Descripcion = error.DESCRIPCION,
                    TipoError = error.TIPO_ERROR,
                    FechaCreacion = error.FECHA_CREACION,
                    EstadoError = error.ESTADO_ERROR
                }).ToList();

            return new Response<List<ErrorCargueArchivoDto>> { Data = listErroresDto };
        }

        public async Task<Response<SolicitudNominaDto>> CrearSolicitudNomina(SolicitudNominaDto request)
        {
            SolicitudNominaDto solNomina = await this._documentoBusinessLogic.CrearSolicitudNomina(request);
            return new Response<SolicitudNominaDto> { Data = solNomina };
        } 
        public async Task<Response<SolicitudNominaDto>> ActualizaSolicitudNomina(SolicitudNominaDto request)
        {
            SolicitudNominaDto solNomina = await this._documentoBusinessLogic.ActualizaSolicitudNomina(request);
            return new Response<SolicitudNominaDto> { Data = solNomina };
        }

        public async Task<Response<SolicitudNominaDto>> ObtenerSolicitudNomina(int documentoId )
        {
            return await this._documentoBusinessLogic.ObtenerSolicitudNomina(documentoId);
        }

        public async Task<Response<List<ErrorCargueArchivoDto>>> ValidarNominaBua(ConsultarArchivosIdDto request)
        {
            int dctId = request.Dct_Id ?? 0;
            var cuerpoNominaResponse = await this._documentoBusinessLogic.ObtenerCuerpoNomina(dctId);


            foreach (var item in cuerpoNominaResponse.Data)
            {

                var clienteResponse = await _iClientRequestProvider.ObtenerClienteNomina(item.TipoDocumento, item.Identificacion);
                if (item.IdUnidadEjecutora != clienteResponse.Data.IdUnidadEjecutora)
                {
                    Console.WriteLine($"La unidad ejecutora es diferente para {item.Identificacion}");
                }  
                if (item.digitoFuerza != clienteResponse.Data.IdFuerza)
                {

                    Console.WriteLine($"La fuerza es diferente para {item.Identificacion}");
                }
            }

            var listErrores = await _errorCargueArchivoBusinessLogic.ConsultarErroresPorCargueArchivo(dctId);

            var listErroresDto = listErrores.Select(error =>
                new ErrorCargueArchivoDto
                {
                    NmnId = error.NMN_ID,
                    DctId = error.DCT_ID,
                    NumeroLinea = error.NUMERO_LINEA,
                    InconsistenciasNombre = error.INCONSISTENCIAS_NOMBRE,
                    InconsistenciasEncabezado = error.INCONSISTENCIAS_ENCABEZADO,
                    InconsistenciasCuerpo = error.INCONSISTENCIAS_CUERPO,
                    InconsistenciasFin = error.INCONSISTENCIAS_FIN,
                    CodigoIncidencia = error.CODIGO_INCIDENCIA,
                    Descripcion = error.DESCRIPCION,
                    TipoError = error.TIPO_ERROR,
                    FechaCreacion = error.FECHA_CREACION,
                    EstadoError = error.ESTADO_ERROR
                }).ToList();

            return new Response<List<ErrorCargueArchivoDto>> { Data = listErroresDto };
        }


        public async Task<Response<IEnumerable<ConsultarNotificacionNomina>>> ConsultarNotificacionNominaSolicitud(Guid usuarioID)
        {
            var response = new Response<IEnumerable<ConsultarNotificacionNomina>>();

            try
            {
                var notificaciones = await _documentoCargaRepository.ConsultaNotificacionNomina(usuarioID);

                response.IsSuccess = true;
                response.Data = notificaciones;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = $"Error en la lógica de negocio al consultar notificaciones de nómina: {ex.Message}";
            }

            return response;
        }

    }
}
