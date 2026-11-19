using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.LocalBlogStorage
{
    public class LocalBlogStorageRepository : ILocalBlogStorageRepository
    {

        private readonly LocalBlogStorageConfig _config;
        private readonly ITransUnionRepository _transUnionRepository;
        private readonly ILogger<LocalBlogStorageRepository> _logger;
        private readonly ICreditoRepository _creditoRepository; 


        public LocalBlogStorageRepository(IOptions<LocalBlogStorageConfig> config, ITransUnionRepository transUnionRepository, ILogger<LocalBlogStorageRepository> logger, ICreditoRepository creditoRepository)
        {
            _config = config.Value;
            _transUnionRepository = transUnionRepository;
            _logger= logger;
            _creditoRepository = creditoRepository; 
        }

        public async Task<Response<string>> CargaDocumentoSolicCredito(IFormFile request)
        {
            try
            {
                if (request == null || request.Length == 0)
                {
                    return new Response<string>
                    {
                        Data = string.Empty,
                        IsSuccess = false,
                        ReturnMessage = "El archivo proporcionado no es válido."
                    };
                }
                FileInfo fi = new FileInfo(request.FileName);

                var newName = Guid.NewGuid() + fi.Extension;

                string folderPath = _config.CarpetaServidorStorage + "\\credito";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, newName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CopyToAsync(stream);
                }

                return new Response<string>
                {
                    Data = newName,
                    IsSuccess = true,
                    ReturnMessage = "Archivo guardado exitosamente."
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Data = string.Empty,
                    IsSuccess = false,
                    ReturnMessage = $"Error al guardar el documento: {ex.Message}"
                };
            }
        }


        public async Task<Transversals.Commons.Response<byte[]>> DescargarArchivoLocal(string folderPath, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException("La ruta de la carpeta o el nombre del archivo no pueden estar vacíos.");
                }

                string filePath = Path.Combine(_config.CarpetaServidorStorage + "\\" + folderPath, fileName);

                if (!File.Exists(filePath))
                {
                    return new Transversals.Commons.Response<byte[]>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "El archivo no existe en la ubicación especificada."
                    };
                }

                byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                return new Transversals.Commons.Response<byte[]>
                {
                    Data = fileContent,
                    IsSuccess = true,
                    ReturnMessage = "Archivo descargado exitosamente."
                };
            }
            catch (Exception ex)
            {
                return new Transversals.Commons.Response<byte[]>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error al descargar el archivo: {ex.Message}"
                };
            }
        }

        public async Task<FileResult> DescargaDocumentoSolicCreditoDesdeCarpeta(string filename)
        {
            string carpetaPrincipal = Path.Combine(
                _config.CarpetaServidorStorage,
                "credito");

            string carpetaSecundaria = Path.Combine(
                _config.CarpetaServidorStorage,
                "reportes-sfc");

            string path = Path.Combine(carpetaPrincipal, filename);

            // Buscar primero en la carpeta principal
            if (!System.IO.File.Exists(path))
            {
                // Si no existe, buscar en la carpeta secundaria
                path = Path.Combine(carpetaSecundaria, filename);
            }

            // Si no existe en ninguna de las dos
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"El archivo {filename} no se encontró en ninguna de las carpetas.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(path);

            var fileStreamResult = new FileContentResult(
                fileBytes,
                "application/octet-stream")
            {
                FileDownloadName = filename
            };

            return fileStreamResult;
        }

        public async Task<Response<string>> CargarXMLLocal(IFormFile request)
        {
            try
            {
                await _creditoRepository.RegistroLogTUSimulador("CargarXMLLocal", "metodo que carga el archivo", request.ToString());

                if (request == null || request.Length == 0)
                {
                    await _transUnionRepository.CrearLogTu(new SolicitudCreditoProductoGral()
                    {
                        VLI_ID = Guid.NewGuid(),
                        VLT_ID = Guid.NewGuid(),
                        VLT_SERVICIO = "TU",
                        VLT_PASO_TRANSACCION = "CARGA_XML_LOCAL",
                        VLT_XML_SOLICITUD = null,
                        VLT_XML_RESPUESTA = "Archivo XML inválido o vacío",
                        VLT_ID_APICACION = null,
                        VLT_CODIGO_RESPUESTA = "false",
                        VLT_TIPO_RESPUESTA = "false",
                        VLT_DESCRIPCION_RESPUESTA = "El archivo recibido es nulo o no contiene información",
                        VLT_FECHA_ACTUALIZA = DateTime.Now
                    });

                    return new Response<string>
                    {
                        IsSuccess = false,
                        ReturnMessage = "El archivo XML es inválido o está vacío"
                    };
                }

                // Ruta de almacenamiento local
                string localPath = Path.Combine(_config.CarpetaServidorStorage, "trans-union");

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                string extension = Path.GetExtension(request.FileName);
                string newName = $"TU_{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(localPath, newName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CopyToAsync(stream);
                }

                // LOG TU - ÉXITO
                await _transUnionRepository.CrearLogTu(new SolicitudCreditoProductoGral()
                {
                    VLI_ID = Guid.NewGuid(),
                    VLT_ID = Guid.NewGuid(),
                    VLT_SERVICIO = "TU",
                    VLT_PASO_TRANSACCION = "CARGA_XML_LOCAL",
                    VLT_XML_SOLICITUD = request.FileName,
                    VLT_XML_RESPUESTA = newName,
                    VLT_ID_APICACION = null,
                    VLT_CODIGO_RESPUESTA = "true",
                    VLT_TIPO_RESPUESTA = "true",
                    VLT_DESCRIPCION_RESPUESTA = "Archivo XML TransUnion almacenado correctamente en servidor local",
                    VLT_FECHA_ACTUALIZA = DateTime.Now
                });
                await _creditoRepository.RegistroLogTUSimulador("ARCHIVO CARGADO CORRECTAMENTE", newName, localPath.ToString());

                return new Response<string>
                {
                    IsSuccess = true,
                    Data = newName
                };


            }
            catch (Exception ex)
            {
                // LOG TU - ERROR
                await _transUnionRepository.CrearLogTu(new SolicitudCreditoProductoGral()
                {
                    VLI_ID = Guid.NewGuid(),
                    VLT_ID = Guid.NewGuid(),
                    VLT_SERVICIO = "TU",
                    VLT_PASO_TRANSACCION = "CARGA_XML_LOCAL",
                    VLT_XML_SOLICITUD = request?.FileName,
                    VLT_XML_RESPUESTA = ex.Message,
                    VLT_ID_APICACION = null,
                    VLT_CODIGO_RESPUESTA = "false",
                    VLT_TIPO_RESPUESTA = "false",
                    VLT_DESCRIPCION_RESPUESTA = "Error al guardar el archivo XML TransUnion en almacenamiento local",
                    VLT_FECHA_ACTUALIZA = DateTime.Now
                });

                await _creditoRepository.RegistroLogTUSimulador("ERROR CARGANDO EL ARCHIVO ", "CATCH DE CargarXMLLocal",ex.ToString());


                return new Response<string>
                {
                    IsSuccess = false,
                    Data = string.Empty,
                    ReturnMessage = "Error guardando el documento en almacenamiento local"
                };
            }
        }


        public async Task<string> DescargarXMLLocal(string filename)
        {
            try
            {
                _logger.LogInformation("(DescargarXMLLocal) Nombre del archivo: " + filename); 
                // Ruta de almacenamiento local
                string localPath = _config.CarpetaServidorStorage + "\\trans-union";


                _logger.LogInformation("(DescargarXMLLocal)Ruta de descarga: " + localPath);



                // Construir la ruta completa del archivo
                string filePath = Path.Combine(localPath, filename);

                _logger.LogInformation("(DescargarXMLLocal)Ruta de descarga con el archivo: " + filePath);

                if (!File.Exists(filePath))
                {
                    _logger.LogError("El archivo no se encuentra en el almacenamiento local." + filePath); 
                    throw new FileNotFoundException("El archivo no se encuentra en el almacenamiento local.");
                }

                // Leer el contenido del archivo
                string xml = await File.ReadAllTextAsync(filePath);

                return xml;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error descargando el documento desde almacenamiento local." + filename);

                throw new Exception("Error descargando el documento desde almacenamiento local", ex);
            }
        }

        public async Task<FileResult> DescargarDocumentosLocalSolicitudCredito(string fileName)
        {
            try
            {
                string folderPath = Path.Combine(_config.CarpetaServidorStorage + "\\reportes-sfc");

                string filePath = Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo solicitado no existe.");
                }

                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var contentType = "application/octet-stream";
                var result = new FileStreamResult(stream, contentType)
                {
                    FileDownloadName = fileName
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al descargar el archivo: " + ex.Message);
            }
        }

        public async Task<Response<string>> CargarDocumentosLocalSolicitudCredito(IFormFile request)
        {
            try
            {
                string folderPath = Path.Combine(_config.CarpetaServidorStorage, "credito");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(request.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CopyToAsync(stream);
                }

                return new Response<string> { Data = fileName };
            }
            catch (Exception ex)
            {
                return new Response<string> { Data = string.Empty, IsSuccess = false, ReturnMessage = "Error guardando el documento en almacenamiento local" };
            }
        }

        public async Task<Transversals.Commons.Response<byte[]>> DescargarArchivoLocalRechazo(string folderPath, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException("La ruta de la carpeta o el nombre del archivo no pueden estar vacíos.");
                }

                string filePath = Path.Combine(_config.CarpetaServidorStorage + "\\reportes-sfc", fileName);

                if (!File.Exists(filePath))
                {
                    return new Transversals.Commons.Response<byte[]>
                    {
                        Data = null,
                        IsSuccess = false,
                        ReturnMessage = "El archivo no existe en la ubicación especificada."
                    };
                }

                byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                return new Transversals.Commons.Response<byte[]>
                {
                    Data = fileContent,
                    IsSuccess = true,
                    ReturnMessage = "Archivo descargado exitosamente."
                };
            }
            catch (Exception ex)
            {
                return new Transversals.Commons.Response<byte[]>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = $"Error al descargar el archivo: {ex.Message}"
                };
            }
        }






    }
}
