using Ardalis.ApiEndpoints;
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

    /// <summary>
    /// Clase del Endpoint para Eliminar una inconsistencia de solicitud
    /// </summary>
    public class EliminarInconsistenciasSeguridadSolicitud :
         BaseAsyncEndpoint.WithRequest<InconsistenciaSeguridadSolicitud>
        .WithResponse<Response<InconsistenciaSeguridadDto>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Eliminar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public EliminarInconsistenciasSeguridadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// EndPoint para Eliminar una inconsistencia de solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPatch("api/solicitud/EliminarInconsistenciasSeguridadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InconsistenciaSeguridadDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Eliminar Inconsistencia",
           Description = "Eliminar Inconsistencia",
           OperationId = "Solicitud.EliminarInconsistenciasSeguridadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<InconsistenciaSeguridadDto>>> HandleAsync(InconsistenciaSeguridadSolicitud request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.EliminarInconsistenciaSeguridadSolicitud(request); 
        }
    }
}
