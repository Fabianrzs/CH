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
    /// Crea cuenta por pagar en seven para segundo pago ("Recursos Caja")
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearFacturaSevenSegundoPago : BaseAsyncEndpoint
        .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<InsertarFacturaPResponse>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearFacturaSevenSegundoPago(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Crea cuenta por pagar en seven para segundo pago ("Recursos Caja")
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CrearFacturaSevenSegundoPago")]
        [ProducesResponseType(typeof(Response<InsertarFacturaPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea cuenta por pagar en seven para segundo pago (\"Recursos Caja\").",
            Description = "proceso segundo pago seven crédito",
            OperationId = "credito.CrearFacturaSevenSegundoPago",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<InsertarFacturaPResponse>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearFacturaProveSevenSegundoPago(request, cancellationToken);
        }
    }
}