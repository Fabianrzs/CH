using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class CrearDocumentoSolicitud : BaseAsyncEndpoint.WithRequest<RequestDocumentoSolicitudDto>.WithResponse<Response<ResponseDocumentoSolicitudes>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public CrearDocumentoSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/crearDocumentoSolicitud")]
        [ProducesResponseType(typeof(Response<ResponseDocumentoSolicitudes>), StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Crear Documento Solicitud",
            Description = "Obtener Solicitud Credito Informacion Tecnica",
            OperationId = "solicitud.crearDocumentoSolicitud",
            Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<ResponseDocumentoSolicitudes>>> HandleAsync([FromBody] RequestDocumentoSolicitudDto request, CancellationToken cancellationToken = default)
        {
            //var file = Request.Form.Files[0];
            return await this._solicitudBusinessLogic.CrearDocumentoSolicitud(request);
        }
    }
}
