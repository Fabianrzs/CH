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

namespace GA2.Endpoints.Credito.Simulador
{
    /// <summary>
    /// ObtenerEstadosTramite
    /// </summary>
    [Authorize]
    public class ObtenerEstadosTramite : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<EstadoTramitesDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerEstadosTramite(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// ObtenerEstadosTramite
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerEstadosTramite")]
        [ProducesResponseType(typeof(IEnumerable<EstadoTramitesDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Estados Tramite",
            Description = "Obtener Estados Tramite",
            OperationId = "credito.ObtenerEstadosTramite",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EstadoTramitesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerEstadosTramite();
        }
    }
}
