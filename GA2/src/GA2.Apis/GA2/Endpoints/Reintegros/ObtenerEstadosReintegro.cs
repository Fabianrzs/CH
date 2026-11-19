using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    /// <summary>
    /// ObtenerEstadosReintegro
    /// </summary>
    /// [Authorize]
    public class ObtenerEstadosReintegro : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<EstadoReintegrosDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerEstadosReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ObtenerEstadosReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerEstadosReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ObtenerEstadosReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EstadoReintegrosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Estados Reintegro",
           Description = "Obtener Estados Reintegro",
           OperationId = "Reintegros.ObtenerEstadosReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<EstadoReintegrosDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.ObtenerEstadoReintegros();
        }
    }
}