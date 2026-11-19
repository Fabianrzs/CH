using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para obtener interviniente por id
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class ObtenerIntervinientesPorId :
        BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IntervinientesActorSolicitud>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para obtener interviniente por id
        /// </summary>        
        public ObtenerIntervinientesPorId(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Obtener interviniente por id
        /// </summary>        
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>        
        [HttpPost("api/solicitud/ObtenerIntervinientesPorId")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IntervinientesActorSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener interviniente por id",
           Description = "Obtener interviniente por id",
           OperationId = "Solicitud.ObtenerIntervinientesPorId",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IntervinientesActorSolicitud>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ConsultarIntervinientesPorId(request); 
        }
    }
}
