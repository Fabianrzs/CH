using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Paso 1 Integración Credito - Creación Tercero
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearTerceroSevenCredito : BaseAsyncEndpoint
        .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<InsertarGnTerceResponse>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearTerceroSevenCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Crear Tercero en seven para temas de desembolso de crédito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CrearTerceroSevenCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarGnTerceResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Tercero en seven para temas de desembolso",
           Description = "Crear Tercero en seven para temas de desembolso",
           OperationId = "Credito.CrearTerceroSevenCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<InsertarGnTerceResponse>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.InsertTerceroSeven(request, cancellationToken);
        }
    }
}
