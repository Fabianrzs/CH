using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reintegros
{
    /// <summary>
    /// ObtenerTiposReintegroById
    /// </summary>
    /// [Authorize]
    public class ObtenerTiposReintegroById : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<TipoReintegrosDto>>
    {

        private readonly IReintegrosBusinessLogic _reintegrosBusinessLogic;

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="reintegrosBusinessLogic"></param>
        public ObtenerTiposReintegroById(IReintegrosBusinessLogic reintegrosBusinessLogic)
        {
            _reintegrosBusinessLogic = reintegrosBusinessLogic;
        }

        /// <summary>
        /// ObtenerTiposReintegroById
        /// </summary>
        /// <param name="idTipo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reintegros/ObtenerTiposReintegroById")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TipoReintegrosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Tipos Reintegro",
           Description = "Obtener Tipos Reintegro",
           OperationId = "Reintegros.ObtenerTiposReintegroById",
           Tags = new[] { "ReintegrosEndpoint" })]
        public override async Task<ActionResult<Response<TipoReintegrosDto>>> HandleAsync(int idTipo, CancellationToken cancellationToken = default)
        {
            return await _reintegrosBusinessLogic.ObtenerTipoReintegroById(idTipo);
        }
    }
}