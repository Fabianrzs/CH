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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint para obtener tramites de intervinientes por tipo y número de documento
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class TramiteIntervimiente :
        BaseAsyncEndpoint.WithRequest<ConsultaTramitesIntervinientesActorSolicitud>
        .WithResponse<Response<List<Tramite>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor de la clase del endpoint para obtener lista de tramites de intervinientes por tipo y número de documento
        /// </summary>        
        public TramiteIntervimiente(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Obtener tramites de intervinientes por tipo y número de documento
        /// </summary>        
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>        
        [HttpPost("api/solicitud/TramiteIntervimiente")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<Tramite>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener tramites de un interviniente por tipo y número de documento",
           Description = "Obtener tramites de un interviniente por tipo y número de documento",
           OperationId = "Solicitud.TramiteIntervimiente",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<List<Tramite>>>> HandleAsync(ConsultaTramitesIntervinientesActorSolicitud request, CancellationToken cancellationToken = default)
        {
            var result = await _solicitudBusinessLogic.TramiteIntervimiente(request);
            return Ok(result);
        }
    }
}
