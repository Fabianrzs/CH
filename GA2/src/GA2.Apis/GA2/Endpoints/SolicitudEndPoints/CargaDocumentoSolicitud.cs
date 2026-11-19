using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Http;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class CargaDocumentoSolicitud : BaseAsyncEndpoint.WithRequest<IFormFile>.WithResponse<Response<string>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public CargaDocumentoSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Carga Archivo al blob storage para solicitud credito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/CargaDocumentoSolicitud")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation("Carga Archivo al blob storage para solicitud, Crear Documento Solicitud",
            Description = "Carga Archivo al blob storage para solicitud",
            OperationId = "solicitud.CargaDocumentoSolicitud",
            Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(IFormFile request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.CargaDocumentoSolicitud(request);
        }
    }
}
