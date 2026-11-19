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
    /// Crea integración desembolso crédito en seven segundo momento crea proveedor y factura.
    /// </summary>
    [Authorize]
    public class DesEmbolsoCreditoSevenMomento2 : BaseAsyncEndpoint
         .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<PagosSevenDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public DesEmbolsoCreditoSevenMomento2(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// EndPoint para ejecutar la integración momento 2 del desembolso crédito en Seven
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>PagosSevenDto</returns>
        [HttpPost("api/credito/DesEmbolsoCreditoMomento2")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PagosSevenDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Integración Desembolso del crédito en seven Momento 2 crea proveedor y factura.",
           Description = "Integración Desembolso del crédito en seven Momento 2 crea proveedor y factura.",
           OperationId = "Credito.DesEmbolsoCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<PagosSevenDto>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.DesEmbolsoCreditolSevenMomento2(request, cancellationToken);
        }
    }
}
