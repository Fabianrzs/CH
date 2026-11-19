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
    /// Endpoint para filtrar intervinientes
    /// </summary>
    /// <author>Alejandro Lara</author>
    [Authorize]
    public class BuscarIntervinientesActoresSolicitud :
        BaseAsyncEndpoint.WithRequest<BuscarIntervinientesDto>
        .WithResponse<Response<IEnumerable<IntervinientesActorSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Constructor de la clase del endpoint para filtrar intervinientes
        /// </summary>        
        public BuscarIntervinientesActoresSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        ///  Buscador para filtrar intervinientes
        /// </summary>        
        /// <param name="cancellationToken"></param>   
        /// <param name="request"></param>   
        [HttpPost("api/solicitud/BuscarIntervinientesActores")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<IntervinientesActorSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Buscador para filtrar intervinientes",
           Description = "Buscador para filtrar intervinientes",
           OperationId = "Solicitud.BuscarIntervinientesActores",
            Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<IntervinientesActorSolicitud>>>> HandleAsync(BuscarIntervinientesDto request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.FiltrarIntervinientes(request);
        }
    }
}
