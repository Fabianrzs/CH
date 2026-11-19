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
    /// <summary>
    /// clase del Endpoint para consultar los flujos del usuario en dodo
    /// </summary>
    public class ObtenerFlujoDodo :
         BaseAsyncEndpoint.WithRequest<ObtenerFlujosDodoDto>
        .WithResponse<Response<ObtenerFlujosDodo>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar los flujos del usuario en dodo
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        public ObtenerFlujoDodo(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// EndPoint para consultar los flujos del usuario en dodo
        /// </summary>
        /// <param name="obtenerFlujosDodoDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ObtenerFlujoDodo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerFlujosDodo>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "consultar los flujos del usuario en dodo",
           Description = "consultar los flujos del usuario en dodo",
           OperationId = "Solicitud.ObtenerFlujoDodo",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<ObtenerFlujosDodo>>> HandleAsync(ObtenerFlujosDodoDto obtenerFlujosDodoDto, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ObtenerFlujosDodo(obtenerFlujosDodoDto); 
        }
    }
}
