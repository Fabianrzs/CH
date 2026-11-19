using Ardalis.ApiEndpoints;
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
    /// Endpoint para consultar los actores de intervinientes
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class ConsultarIntervinientesActores
        : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<IntervinientesActor>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint donde se consultan los actores de intervinientes
        /// </summary>        
        public ConsultarIntervinientesActores(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        ///  Consulta los actores de intervinientes
        /// </summary>        
        /// <param name="cancellationToken"></param>        
        [HttpGet("api/solicitud/ConsultarIntervinientesActores")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<IntervinientesActor>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultalos actores de intervinientes",
           Description = "Consulta los actores de intervinientes",
           OperationId = "Solicitud.ConsultarIntervinientesActores",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<IntervinientesActor>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.IntervinientesActor(); 
        }
    }
}
