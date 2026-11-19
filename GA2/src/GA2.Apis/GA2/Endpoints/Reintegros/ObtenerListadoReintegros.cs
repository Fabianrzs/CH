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
    /// ObtenerListadoReintegros
    /// </summary>
    /// [Authorize]
    public class ObtenerListadoReintegros : BaseAsyncEndpoint
        .WithRequest<ReintegrosParamsDto>
        .WithResponse<Response<IEnumerable<ListaReintegrosDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerListadoReintegros
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ObtenerListadoReintegros(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerListadoReintegros
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ObtenerListadoReintegros")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ListaReintegrosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Tipos Reintegro",
           Description = "Obtener Tipos Reintegro",
           OperationId = "Reintegros.ObtenerListadoReintegros",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ListaReintegrosDto>>>> HandleAsync([FromQuery] ReintegrosParamsDto request, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.ObtenerListadoReintegros(request);
        }
    }
}