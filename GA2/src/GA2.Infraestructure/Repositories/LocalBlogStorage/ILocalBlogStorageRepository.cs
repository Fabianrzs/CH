using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.LocalBlogStorage
{
    public interface ILocalBlogStorageRepository
    {
        Task<Response<string>> CargaDocumentoSolicCredito(IFormFile request); 
        Task<Transversals.Commons.Response<byte[]>> DescargarArchivoLocal(string folderPath, string fileName);
        Task<FileResult> DescargaDocumentoSolicCreditoDesdeCarpeta(string filename);
        Task<Response<string>> CargarXMLLocal(IFormFile request);
        Task<string> DescargarXMLLocal(string filename);
        Task<Response<string>> CargarDocumentosLocalSolicitudCredito(IFormFile request);
        Task<FileResult> DescargarDocumentosLocalSolicitudCredito(string fileName);
        Task<Transversals.Commons.Response<byte[]>> DescargarArchivoLocalRechazo(string folderPath, string fileName);

    }
}
