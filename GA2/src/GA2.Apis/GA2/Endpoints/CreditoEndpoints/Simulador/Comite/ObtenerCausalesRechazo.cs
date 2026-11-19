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

namespace GA2.Endpoints.CreditoEndpoints.Simulador.Comite
{
    /// <summary>
    /// Endpoint Obtener Causales Rechazo
    /// </summary>
    [Authorize]
    public class ObtenerCausalesRechazo : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<CausalesRechazoDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerCausalesRechazo(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Aprueba Datos Comite Credito
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerCausalesRechazo")]
        [ProducesResponseType(typeof(IEnumerable<CausalesRechazoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Causales Rechazo",
            Description = "Obtener Causales Rechazo",
            OperationId = "credito.ObtenerCausalesRechazo",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CausalesRechazoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerCausalesRechazo();
        }
    }
}


