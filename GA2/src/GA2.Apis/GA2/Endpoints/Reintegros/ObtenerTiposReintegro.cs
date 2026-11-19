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
    /// ObtenerTiposReintegro
    /// </summary>
    /// [Authorize]
    public class ObtenerTiposReintegro : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<TipoReintegrosDto>>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerTiposReintegro
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ObtenerTiposReintegro(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegro
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ObtenerTiposReintegro")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoReintegrosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Tipos Reintegro",
           Description = "Obtener Tipos Reintegro",
           OperationId = "Reintegros.ObtenerTiposReintegro",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<TipoReintegrosDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.ObtenerTipoReintegros();
        }
    }
}