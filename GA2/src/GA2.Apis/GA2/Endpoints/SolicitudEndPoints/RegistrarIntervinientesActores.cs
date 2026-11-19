using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para registrar intervinientes
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class RegistrarIntervinientesActores :
        BaseAsyncEndpoint.WithRequest<IntervinientesActorSolicitudDto>
        .WithResponse<Response<IntervinientesActorSolicitud>>
    {


        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para registrar intervinientes
        /// </summary>        
        public RegistrarIntervinientesActores(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        ///  Registro de intervinientes
        /// </summary>        
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>        
        [HttpPost("api/solicitud/RegistrarIntervinientesActores")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IntervinientesActorSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Registra actores de intervinientes",
           Description = "Registra los actores de intervinientes",
           OperationId = "Solicitud.RegistrarIntervinientesActores",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IntervinientesActorSolicitud>>> HandleAsync(IntervinientesActorSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.RegistrarIntervinientes(request); 
        }
    }
}
