using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint que lista los causales de cambio de estado del cliente
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class ObtenerCausalEstadoCliente : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<List<EstadoCausalClienteDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Michel Parra</author>
        /// <date>01/05/2024</date>
        public ObtenerCausalEstadoCliente(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Endpoint que lista los causales de cambio de estado del cliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/ObtenerCausalEstadoCliente")]
        [Consumes("application/json")]
        [SwaggerOperation(
           Summary = " lista los causales de cambio de estado del cliente",
           Description = " lista los causales de cambio de estado del cliente",
           OperationId = "Solicitud.ObtenerCausalEstadoCliente",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<List<EstadoCausalClienteDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerCausalEstadoCliente();
        }
    }
}
