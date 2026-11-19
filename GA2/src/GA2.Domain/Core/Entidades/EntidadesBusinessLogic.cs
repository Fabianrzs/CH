using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using GA2.Application.Dto.Entidades;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities.InfoVendedor;
using GA2.Domain.Entities.Entidades;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Catalogs Core Negocio
    /// <author>Edgar Andrés Riaño</author>
    /// <date>8/03/2021</date>
    /// </summary>
    public class EntidadesBusinessLogic : IEntidadesBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly IEntidadesRepository _entidadesRepository;
        private readonly IOptions<BlobStorageOptions> _options;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IGestorDocumentalBusinessLogic _WMBusinessLogic;
        public EntidadesBusinessLogic(
            IMapper mapper,
            IEntidadesRepository entidadesRepository,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            IOptions<BlobStorageOptions> options,
            IGestorDocumentalBusinessLogic WMBusinessLogic
            )
        {
            _mapper = mapper;
            _entidadesRepository = entidadesRepository;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _WMBusinessLogic = WMBusinessLogic;
        }

        #region EntidadEducativa

        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativa()
        {
            var arespuestaEntidades = await this._entidadesRepository.ObtenerEntidadEducativa();
            IEnumerable<EntidadEducativaDto> entidadEducativaTypes = this._mapper.Map<IEnumerable<EntidadEducativaDto>>(arespuestaEntidades);
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidadEducativaTypes };
        }
        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativaFideicomiso()
        {
            var arespuestaEntidades = await this._entidadesRepository.ObtenerEntidadEducativa();
            IEnumerable<EntidadEducativaDto> entidadEducativaTypes = this._mapper.Map<IEnumerable<EntidadEducativaDto>>(arespuestaEntidades);
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidadEducativaTypes };
        }
        /// <summary>
        /// Obtener entidades educativas
        /// </summary>
        /// <author>Hanson Restrepo</author>
        /// <date26/01/2022</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativaDto entidadEducativa)
        {
            var entidadEnt = _mapper.Map<EntidadEducativa>(entidadEducativa);
            IEnumerable <EntidadEducativaDto> entidad = _mapper.Map<IEnumerable<EntidadEducativaDto>>(await _entidadesRepository.ObtenerEntidadEducativaPorNombreNit(entidadEnt));
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidad };
        }

        /// <summary>
        /// CrearEntidadEducativa
        /// </summary>
        /// <param name="entidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<EntidadEducativaDto>> CrearEntidadEducativa(EntidadEducativaDto entidadEducativa)
        {

            var validateExistEntity = await _entidadesRepository.ObtenerEntidadEducativaPorNit(entidadEducativa.Nit);
            if (validateExistEntity == 0)
            {
                return new Response<EntidadEducativaDto>
                {
                    Data = _mapper.Map<EntidadEducativaDto>(
                 await _entidadesRepository.CrearEntidadEducativa(
             _mapper.Map<EntidadEducativa>(entidadEducativa)))
                };
            }
            return new Response<EntidadEducativaDto>
            {
                Data = null,
                ReturnMessage = "Ya se encuentra registrada una entidad con ese nit"
            };
        }
        public async Task<Response<IEnumerable<FidecomisoDto>>> CrearFideicomisoEntidadEducativa(FidecomisoDto fidecomiso)
        {
            return new Response<IEnumerable<FidecomisoDto>>
            {
                Data = _mapper.Map<IEnumerable<FidecomisoDto>>( await _entidadesRepository.InsertarFideicomisoEntidad(_mapper.Map<Fideicomiso>(fidecomiso)))
            };
        }

        public async Task<Response<IEnumerable<FidecomisoDto>>> ActualizarFideicomisoEntidadEducativa(FidecomisoDto fidecomiso)
        {
            Fideicomiso Fideicomiso = new()
            {
                ENE_FIDEICOMISO_ID = fidecomiso.ENE_FIDEICOMISO_ID,
                ENE_TIPO_IDENTIFICACION = fidecomiso.idTipoIdentificacion.ToString(),
                ENE_NUMERO_IDENTIFICACION = fidecomiso.nroIdentificacion,
                ENE_RAZON_SOCIAL_FIDEICOMISO = fidecomiso.nombreRazonSocial,
                ENEC_USER = fidecomiso.usr_id
            };

            return new Response<IEnumerable<FidecomisoDto>>
            {
                Data = _mapper.Map<IEnumerable<FidecomisoDto>>(await _entidadesRepository.ActualizarFideicomisoEntidadEducativa(Fideicomiso))
            };
        }

        public async Task<Response<IEnumerable<FidecomisoDto>>> EliminarFideicomiso(IdentificadoresEntidadEducativa fidecomiso)
        {
            return new Response<IEnumerable<FidecomisoDto>>
            {
                Data = _mapper.Map<IEnumerable<FidecomisoDto>>(await _entidadesRepository.EliminarFideicomiso(fidecomiso))
            };
        }


        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ConsultarFideicomisosEntidad(EntidadEducativaDto data)
        {
            var fidecomiso = _mapper.Map<Fideicomiso>(data);
            IEnumerable<EntidadEducativaDto> entidad = _mapper.Map<IEnumerable<EntidadEducativaDto>>(await _entidadesRepository.ConsultarFideicomisosEntidad(fidecomiso));
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidad };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<EntidadEducativaDto>> ActualizarEntidadEducativa(EntidadEducativaDto entidadEducativa)
        {
            return new Response<EntidadEducativaDto>
            {
                Data = _mapper.Map<EntidadEducativaDto>(
                    await _entidadesRepository.ActualizarEntidadEducativa(
                         _mapper.Map<EntidadEducativa>(entidadEducativa)))
            };
        }

        /// <summary>
        /// EliminarEntidadEducativaPorId
        /// </summary>
        /// <param name="IdEntidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<EntidadEducativaDto>> EliminarEntidadEducativaPorId(string idEntidadEducativa)
        {
            return new Response<EntidadEducativaDto>
            {
                Data = _mapper.Map<EntidadEducativaDto>(
                    await this._entidadesRepository.EliminarEntidadEducativaPorId(idEntidadEducativa))
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEntidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<SedeEntidadEducativaDto>>> ObtenerSedesPorEntidadEducativa(Guid idEntidadEducativa)
        {
            IEnumerable<SedeEntidadEducativaDto> sedes = _mapper.Map<IEnumerable<SedeEntidadEducativaDto>>(await _entidadesRepository.ObtenerSedesPorEntidadEducativa(idEntidadEducativa));
            return new Response<IEnumerable<SedeEntidadEducativaDto>> { Data = sedes };
        }

        public async Task<Response<SedeEntidadEducativaDto>> CrearSedeEntidadEducativa(SedeEntidadEducativaDto sedeEntidadEducativa)
        {
            return new Response<SedeEntidadEducativaDto>
            {
                Data = _mapper.Map<SedeEntidadEducativaDto>(
                    await _entidadesRepository.CrearSedeEntidadEducativa(
                         _mapper.Map<SedeEntidadEducativa>(sedeEntidadEducativa)))
            };
        }

        public async Task<Response<CrearAnotacionDTO>> CrearAnotacion(CrearAnotacionDTO crearAnotacion)
        {
            return new Response<CrearAnotacionDTO>
            {
                Data = _mapper.Map<CrearAnotacionDTO>(
                    await _entidadesRepository.CrearAnotacion(
                         _mapper.Map<crearAnotacion>(crearAnotacion)))
            };
        }

        public async Task<Response<IEnumerable<RespuestaAnotacionDTO>>> ConsultarAnotaciones(Guid entidadId)
        {
            var anotaciones = await _entidadesRepository.ConsultarAnotacion(entidadId); 
            return new Response<IEnumerable<RespuestaAnotacionDTO>>
            {
                Data = _mapper.Map<IEnumerable<RespuestaAnotacionDTO>>(anotaciones)
            };
        }

        public async Task<Response<SedeEntidadEducativaDto>> ActualizarSedeEntidadEducativa(SedeEntidadEducativaDto sedeEntidadEducativa)
        {
            return new Response<SedeEntidadEducativaDto>
            {
                Data = _mapper.Map<SedeEntidadEducativaDto>(
                    await _entidadesRepository.ActualizarSedeEntidadEducativa(
                         _mapper.Map<SedeEntidadEducativa>(sedeEntidadEducativa)))
            };
        }

        public async Task<Response<SedeEntidadEducativaDto>> EliminarSedesEntidadEducativaPorId(IdentificadoresEntidadEducativa sedeEntidadEducativa)
        {
            return new Response<SedeEntidadEducativaDto>
            {
                Data = _mapper.Map<SedeEntidadEducativaDto>(
                    await this._entidadesRepository.EliminarSedeEntidadEducativaPorId(sedeEntidadEducativa))
            };
        }
    //    var arespuestaEntidades = await this._entidadesRepository.ObtenerEntidadEducativa();
    //    IEnumerable<EntidadEducativaDto> entidadEducativaTypes = this._mapper.Map<IEnumerable<EntidadEducativaDto>>(arespuestaEntidades);
    //        return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidadEducativaTypes
    //};

    public async Task<Response<BloqueoEntidadEducativaDto>> BloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativa)
        {
            return new Response<BloqueoEntidadEducativaDto>{Data = _mapper.Map<BloqueoEntidadEducativaDto>(await _entidadesRepository.BloqueoEntidadEducativa(_mapper.Map<BloqueoEntidadEducativa>(bloqueoEntidadEducativa)))};
        }

        public async Task<Response<BloqueoEntidadEducativaDto>> DesbloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativa)
        {
            return new Response<BloqueoEntidadEducativaDto>
            {
                Data = _mapper.Map<BloqueoEntidadEducativaDto>(
                    await _entidadesRepository.DesbloqueoEntidadEducativa(
                         _mapper.Map<BloqueoEntidadEducativa>(bloqueoEntidadEducativa)))
            };
        }

        public async Task<Response<IEnumerable<BloqueoEntidadEducativaDto>>> ObtenerBloqueoEntidadEducativa(string idEntidadEducativa)
        {
            IEnumerable<BloqueoEntidadEducativaDto> bloqueo = _mapper.Map<IEnumerable<BloqueoEntidadEducativaDto>>(await _entidadesRepository.ObtenerBloqueoEntidadEducativa(idEntidadEducativa));
            return new Response<IEnumerable<BloqueoEntidadEducativaDto>> { Data = bloqueo };
        }

        public async Task<Response<CuentaBancariaEntidadEducativaDto>> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativa)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await _entidadesRepository.CrearCuentaBancariaEntidadEducativa(
                         _mapper.Map<CuentaBancariaEntidadEducativa>(cuentaBancariaEntidadEducativa)))
            };
        }

        public async Task<Response<CuentaBancariaEntidadEducativaDto>> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativa)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await _entidadesRepository.ActualizarCuentaBancariaEntidadEducativa(
                         _mapper.Map<CuentaBancariaEntidadEducativa>(cuentaBancariaEntidadEducativa)))
            };
        }

        public async Task<Response<IEnumerable<FidecomisoDto>>> ObtenerCuentaBancariaPorEntidad(string idEntidad)
        {
            IEnumerable<FidecomisoDto> cuenta = _mapper.Map<IEnumerable<FidecomisoDto>>(await _entidadesRepository.ObtenerCuentaBancariaPorEntidad(idEntidad));
            return new Response<IEnumerable<FidecomisoDto>> { Data = cuenta };
        }
        public async Task<Response<CuentaBancariaEntidadEducativaDto>> EliminarCuentaBancariaEntidadEducativa(Guid idCuenta)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await this._entidadesRepository.EliminarCuentaBancariaEntidadEducativa(idCuenta))
            };
        }


        #endregion EntidadEducativa

        /// <summary>
        /// Obtener programas educativos
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <returns>Lista de programas educativos</returns>
        public async Task<Response<IEnumerable<ProgramaEducativoDto>>> ObtenerProgramaEducativo(Guid idEntidad)
        {
            return new Response<IEnumerable<ProgramaEducativoDto>> { Data = this._mapper.Map<IEnumerable<ProgramaEducativoDto>>(await this._entidadesRepository.ObtenerProgramaEducativo(idEntidad)) };
        }

        /// <summary>
        /// Obtener niveles educativos
        /// </summary>
        /// <param name="idPrograma"></param>
        /// <returns>Lista de niveles educativos</returns>
        public async Task<Response<IEnumerable<NivelEducativoDto>>> ObtenerNivelEducativoCesantias(Guid idPrograma)
        {
            return new Response<IEnumerable<NivelEducativoDto>> { Data = this._mapper.Map<IEnumerable<NivelEducativoDto>>(await this._entidadesRepository.ObtenerNivelEducativoCesantias(idPrograma)) };
        }
        public async Task<Response<IEnumerable<ListarProgramaEducativoDto>>> ObtenerProgramaEducativoPorEntidad(Guid idEntidad)
        {
            return new Response<IEnumerable<ListarProgramaEducativoDto>> { Data = this._mapper.Map<IEnumerable<ListarProgramaEducativoDto>>(await _entidadesRepository.ObtenerProgramaEducativoPorEntidad(idEntidad)) };
        }
        public async Task<Response<ListarProgramaEducativoDto>> CrearProgramaEducativo(ProgramaEducativoDto programaEducativo)
        {
            return new Response<ListarProgramaEducativoDto>
            {
                Data = _mapper.Map<ListarProgramaEducativoDto>(
             await _entidadesRepository.CrearProgramaEducativo(
                  _mapper.Map<ProgramaEducativo>(programaEducativo)))
            };
        }
        public async Task<Response<ProgramaEducativoDto>> EliminarProgramaEducativoPorId(IdentificadoresEntidadEducativa entidadEducativa)
        {
            return new Response<ProgramaEducativoDto>
            {
                Data = _mapper.Map<ProgramaEducativoDto>(
                    await this._entidadesRepository.EliminarProgramaEducativoPorId(entidadEducativa))
            };
        }
        public async Task<Response<ProgramaEducativoDto>> ActualizarProgramaEducativo(ProgramaEducativoDto programaEducativo)
        {
            return new Response<ProgramaEducativoDto>
            {
                Data = _mapper.Map<ProgramaEducativoDto>(
                    await _entidadesRepository.ActualizarProgramaEducativo(
                         _mapper.Map<ProgramaEducativo>(programaEducativo)))
            };
        }

        public async Task<Response<bool>> CargarDocumentosEntidad(List<CargarDocumentosEntidadDto> cargarDocumentosEntidad, Guid idEntidad)
        {
            try
            {
                foreach (var x in cargarDocumentosEntidad)
                {
                    FileStreamResult fileStreamResult = new(x.archivoCargado.OpenReadStream(), x.archivoCargado.ContentType);
                    FileInfo fi = new(x.archivoCargado.FileName.Split("--")[1]);

                    if (fi.Extension == ".pdf" || fi.Extension == ".PDF")
                    {
                        var newName = Path.GetRandomFileName() + fi.Extension.ToLower();
                        var result = await _blobStorageGenericRepository.SubirArchivoBlobStorage("entidades", newName.ToString(), fileStreamResult);

                        InsertarArchivoEntidad insertarArchivoEntidad = new()
                        {
                            ENE_ID = idEntidad,
                            ENE_PAM_ID = x.archivoParametrizadoId,
                            ENE_NOMBRE_ARCHIVO = fi.Name,
                            ENE_RUTA_STORAGE = newName,
                            ENE_EXTENSION = fi.Extension,
                            ENE_PAM_NUMERO_FOLIO = x.numeroFolio,
                            ENE_PAM_CREATEDOPOR = new Guid()
                        };

                        await _entidadesRepository.InsertarArchivoPorEntidad(insertarArchivoEntidad);
                    }
                    else
                    {
                        return new Response<bool>
                        {
                            Data = false,
                            IsSuccess = false,
                            ReturnMessage = "Algunos documentos no fueron almacenados. Solo puede cargar documentos PDF"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = e.Message };
            }

            return new Response<bool> { Data = true };
        }

        public async Task<Response<IEnumerable<ConsultarArchivosPorEntidadDto>>> ConsultarArchivosPorEntidad(Guid idEntidad)
        {
            /*
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<IEnumerable<ConsultarArchivosPorEntidadDto>>(
                    await _catalogsRepository.CrearCuentaBancariaEntidadEducativa(
                         _mapper.Map<CuentaBancariaEntidadEducativa>(cuentaBancariaEntidadEducativa)))
            };*/
            IEnumerable<ConsultarArchivosPorEntidadDto> archivo = _mapper.Map<IEnumerable<ConsultarArchivosPorEntidadDto>>(await this._entidadesRepository.ConsultarArchivosPorEntidad(idEntidad));
            return new Response<IEnumerable<ConsultarArchivosPorEntidadDto>> { Data = archivo, IsSuccess = true, ReturnMessage = "" };
        }

        public async Task<FileResult> DescargarArchivoPorId(string rutaStorage)
        {

            BlobContainerClient container = new(_options.Value.CadenaOne, "entidades");
            BlobClient blob = container.GetBlobClient(rutaStorage);
            BlobDownloadInfo download = await blob.DownloadAsync();

            var descargado = new FileStreamResult(download.Content, download.ContentType)
            {
                FileDownloadName = rutaStorage
            };
            await descargado.FileStream.FlushAsync();

            return descargado;
        }

        public async Task<Response<EntidadEducativaDto>> ObtenerEntidadEducativaDetalle(Guid idEntidad)
        {
            EntidadEducativaDto entidad = _mapper.Map<EntidadEducativaDto>(await _entidadesRepository.ObtenerEntidadEducativaDetalle(idEntidad));
            return new Response<EntidadEducativaDto> { Data = entidad };
        }
        public async Task<Response<EntidadEducativaDto>> ObtenerEntidadEducativaFideicomiso(Guid idEntidad)
        {
            EntidadEducativaDto entidad = _mapper.Map<EntidadEducativaDto>(await _entidadesRepository.ObtenerEntidadEducativaDetalle(idEntidad));
            return new Response<EntidadEducativaDto> { Data = entidad };
        }
        public async Task<Response<bool>> EliminarArchivoEntidad(EliminarArchivoEntidadDto eliminarArchivoEntidadDto)
        {
            Response<bool> respuesta = new()
            {
                Data = false,
                IsSuccess = false
            };

            //elimina del blobstorage
            try
            {
                var result = await _blobStorageGenericRepository.EliminarArchivoStorage("entidades", eliminarArchivoEntidadDto.rutaStorage);
                //elimina de la bd
                EliminarArchivoEntidad eliminarArchivo = new()
                {
                    ENE_ID = eliminarArchivoEntidadDto.idArchivo,
                    ENE_RUTA_STORAGE = eliminarArchivoEntidadDto.rutaStorage
                };

                respuesta.Data = await _entidadesRepository.EliminarArchivoEntidad(eliminarArchivo);
                respuesta.IsSuccess = true;
            }
            catch (Exception e)
            {
                // File error or not exist
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = e.Message };
            }
            return respuesta;
        }
        /// <summary>
        /// Carga los documentos para la solicitud
        /// </summary>
        /// <author>Camilo Brito</author>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<string>> CargaDocumentoEntidades(IFormFile request)
        {
            try
            {
                var _request = request;
                FileInfo fi = new(_request.FileName);
                BlobContainerClient container = new(_options.Value.CadenaOne, "entidades");
                await container.CreateIfNotExistsAsync();
                var newName = Guid.NewGuid() + fi.Extension;
                BlobClient blob = container.GetBlobClient(newName);
                var result = await blob.UploadAsync(request.OpenReadStream());
                return new Response<string> { Data = newName };
            }
            catch (Exception)
            {
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardado documento en el Blob Storage" };
            }


        }
        public async Task<Response<IEnumerable<TramiteEntidadesDto>>> ConsultarDataTramiteEntidades(Guid idEntidad)
        {
            IEnumerable<TramiteEntidadesDto> entidad = _mapper.Map<IEnumerable<TramiteEntidadesDto>>(await _entidadesRepository.DataTramitesEntidades(idEntidad));
            return new Response<IEnumerable<TramiteEntidadesDto>> { Data = entidad };
        }

        public async Task<Response<string>> EliminarSolicitudes(string Identificacion)
        {
            await _entidadesRepository.EliminarSolicitudes(Identificacion);

            return new Response<string>
            {
                Data = "La solicitud se eliminó correctamente",
                IsSuccess = true
            };
        }

        /// <summary>
        /// Metodo Crear Documento Entidades
        /// </summary>
        /// <author>Camilo Brito</author>
        /// <param name="documentoSolicitud"></param>
        /// <returns></returns>
        public async Task<Response<ResponseDocumentoSolicitudDto>> CrearDocumentoEntidades(RequestDocumentoEntidadesDto requestDocumentoEntidades)
        {
            Application.Dto.AttachRequestDto attachRequestDto = new()
            {
                UserOperation = "web",
                Files = new List<Application.Dto.FileDto>()
            };
            string FilePath = requestDocumentoEntidades.rutaImagen;

            
            attachRequestDto.Radicado = requestDocumentoEntidades.numeroRadicado;

            var fileExtensions = new List<string> { "pdf", "tif" };
            int folios;
            var bdinfo = await _blobStorageGenericRepository.DescargarArchivoStorage("entidades", $"{requestDocumentoEntidades.rutaImagen}");
            string archivoBase64 = string.Empty;
            byte[] conten;
            var result = await _entidadesRepository.ConsultarArchivosEntidadPorId(requestDocumentoEntidades.idArchivo);

            if (fileExtensions.Contains(FilePath.Split(".")[1]))
            {
                using (MemoryStream memoryStream = new())
                {
                    await bdinfo.Content.CopyToAsync(memoryStream);
                    conten = memoryStream.ToArray();
                }
                string contenidoTexto = Encoding.UTF8.GetString(conten);

                folios = new Regex(@"/Type\s*/Page[^s]").Matches(contenidoTexto).Count;

                archivoBase64 = Convert.ToBase64String(conten);
            }
            else folios = 0;

            var filenamewhitoutext = Guid.Parse(FilePath.Split(".")[0]);
            var fileExtension = FilePath.Split(".")[1];
            var documento = new ResponseDocumentoSolicCredito
            {
                DCS_ID = filenamewhitoutext,
                SOC_ID = requestDocumentoEntidades.idEntidades,
                TDC_ID = 0,
                DCS_CODIGO_BARRAS = requestDocumentoEntidades.numeroRadicado,
                DCS_FECHA_DOCUMENTO = DateTime.Now,
                DCS_NUMERO_FOLIOS = folios,
                DCS_TAMAÑO = 0,
                DCS_RUTA_IMAGEN = requestDocumentoEntidades.rutaImagen,
                DCS_ESTADO = "1",
                DCS_CREADO_POR = result.ENE_PAM_CREATEDOPOR!= Guid.Empty ? result.ENE_PAM_CREATEDOPOR: Guid.NewGuid(),
                DCS_FECHA_CREACION = DateTime.Now
            };

            TipoDocumentoEntidadesDto infoTipoDocument = new()
            {
                DirectoryCode = "00",
                DocumentName = result.ENE_PAM_NOMBRE_ARCHIVO,
                DocumentTypeCode = result.PAM_CVL_TIPO_PERSONA_ID,
                Descripcion = result.ENE_PAM_DESCRIPCION_ARCHIVO
            };

            attachRequestDto.Files.Add(CrearObjetoWorkmanager(archivoBase64, infoTipoDocument, fileExtension));

            if (attachRequestDto.Files.Count > 0)
            {
                await _WMBusinessLogic.Form_AttachFilesAsync(attachRequestDto);

            }
            return new Response<ResponseDocumentoSolicitudDto> { Data = this._mapper.Map<ResponseDocumentoSolicitudDto>(documento) };

        }
       
        private static Application.Dto.FileDto CrearObjetoWorkmanager(string fileBase64, TipoDocumentoEntidadesDto infoDoc, string fileExtension)
        {
            return new Application.Dto.FileDto(fileBase64, infoDoc.DocumentName, infoDoc.DirectoryCode.ToString(), infoDoc.DocumentTypeCode.ToString(), fileExtension, "");
        }

        public async Task<Response<IEnumerable<HistorialCambiosDto>>> ObtenerHistorialCambios(Guid idEntidad)
        {
            var cambios = _mapper.Map<IEnumerable<HistorialCambiosDto>>(await _entidadesRepository.ObtenerHistorialCambios(idEntidad));
            return new Response<IEnumerable<HistorialCambiosDto>> { Data = cambios };
        }

        public async Task<Response<IEnumerable<HistorialCambiosIntervinienteDto>>> ObtenerHistorialCambiosIntervinientes(Guid idIntervinientes)
        {
            var cambios = _mapper.Map<IEnumerable<HistorialCambiosIntervinienteDto>>(await _entidadesRepository.ObtenerHistorialCambiosIntervinientes(idIntervinientes));
            return new Response<IEnumerable<HistorialCambiosIntervinienteDto>> { Data = cambios };
        }

        public async Task<Response<string>> CrearNovedadesEntidadEducativa(NovedadesEntidadEducativaRequestDto request)
        {
            var mapRequest = _mapper.Map<NovedadesEntidadEducativaRequest>(request);
            var response = await _entidadesRepository.CrearNovedadesEntidadesEducativas(mapRequest);
            if (response > 0)
            {
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false,ReturnMessage ="No se logro guardar la novedad"};
        }
        public async Task<Response<string>> CrearHistorialEntidad(HistorialCambiosDto request)
        {
            var mapRequest = _mapper.Map<HistorialCambios>(request);
            var response = await _entidadesRepository.CrearHistorialEntidad(mapRequest);
            if (response > 0)
            {
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se logro guardar la novedad" };
        }

        /// <summary>
        /// Mensaje
        /// </summary>
        /// <param name="codigoMensaje"></param>
        ///<author>Camilo Brito</author>
        /// <date>23/10/2023</date>


    }
}
