using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;


namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint para actualizar intervinientes
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class EliminarIntervinienteActorSolicitud : BaseAsyncEndpoint.WithRequest<EliminarIntervinientesActorSolicitudDto>.WithResponse<Response<IntervinientesActorSolicitud>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para eliminar intervinientes
        /// </summary> 
        public EliminarIntervinienteActorSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        ///  Eliminar intervinientes
        /// </summary>        
        /// <param name="cancellationToken"></param>   
        /// <param name="request"></param>   
        [HttpPost("api/solicitud/EliminarIntervinientePorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IntervinientesActorSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Eliminar interviniente",
         Description = "Elimina interviniente por Id",
         OperationId = "Solicitud.EliminarIntervinientePorId",
          Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IntervinientesActorSolicitud>>> HandleAsync(EliminarIntervinientesActorSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.EliminarIntervinienteActor(request);
        }
    }
}
