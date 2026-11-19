using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
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
    /// Crea Cuenta por pagar en seven para primer pago ("Cuentas Individuales").
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearFacturaSevenPrimerPago : BaseAsyncEndpoint
        .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<InsertarFacturaPResponse>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearFacturaSevenPrimerPago(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Crea Cuenta por pagar en seven para primer pago ("Cuentas Individuales").
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CrearFacturaSevenPrimerPago")]
        [ProducesResponseType(typeof(Response<InsertarFacturaPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea Cuenta por pagar en seven para primer pago (\"Cuentas Individuales\").",
            Description = "proceso primer pago seven crédito",
            OperationId = "credito.CrearFacturaSevenPrimerPago",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<InsertarFacturaPResponse>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearFacturaProveSevenPrimerPago(request, cancellationToken);
        }
    }
}
