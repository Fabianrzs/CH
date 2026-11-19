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
    /// crear CDP en seven primer pago ("Cuenta Individual").
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearCDPSevenPrimerPago : BaseAsyncEndpoint
        .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<CrearCDPResponse>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearCDPSevenPrimerPago(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// crear CDP en seven primer pago ("Cuenta Individual")
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCDPResponse</returns>
        [HttpPost("api/credito/CrearCDPSevenCreditoPrimerPago")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<CrearCDPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear CDP en seven referente al primer pago recursos cuenta individual del afiliado",
           Description = "Crear CDP en seven primer pago",
           OperationId = "Credito.CrearCDPSevenCreditoPrimerPago",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<CrearCDPResponse>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearCDPSevenPrimerPago(request, cancellationToken);
        }
    }
}
