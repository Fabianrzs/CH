using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Crea integración desembolso crédito en seven primer momento valida tercero y crea CDP
    /// </summary>
    [Authorize]
    public class DesEmbolsoCreditoSevenMomento1 : BaseAsyncEndpoint
         .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<PagosSevenDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public DesEmbolsoCreditoSevenMomento1(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// EndPoint para ejecutar la integración momento 1 del desembolso crédito en Seven
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>PagosSevenDto</returns>
        [HttpPost("api/credito/DesEmbolsoCreditoMomento1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PagosSevenDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Integración Desembolso del crédito en seven Momento 1 Valida Tercero y crea CDP.",
           Description = "Integración Desembolso del crédito en seven Momento 1 Valida Tercero y crea CDP.",
           OperationId = "Credito.DesEmbolsoCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<PagosSevenDto>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.DesEmbolsoCreditolSevenMomento1(request, cancellationToken);
        }
    }
}
