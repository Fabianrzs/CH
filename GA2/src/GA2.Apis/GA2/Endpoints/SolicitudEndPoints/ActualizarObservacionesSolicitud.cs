using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Solicitud;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class ActualizarObservacionesSolicitud :
         BaseAsyncEndpoint.WithRequest<ObservacionSolicitudDto>
        .WithResponse<Response<ObservacionSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ActualizarObservacionesSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/ActualizarObservacionesSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObservacionSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar comunicaciones solicitud",
           Description = "Actualizar comunicaciones solicitud",
           OperationId = "Solicitud.ActualizarObservacionesSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<ObservacionSolicitudDto>>> HandleAsync(ObservacionSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ActualizarObservacionesSolicitud(request);
        }
    }
}
