using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
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
    /// Paso 3 crear Proveedor en seven desde proceso de solicitud de crédito.
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearProveedorSevenCredito : BaseAsyncEndpoint
        .WithRequest<RequestCreditoSeven>
        .WithResponse<Response<SyncPoPvdorResponse>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CrearProveedorSevenCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Crear CDP en seven para temas de desembolso de crédito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCDPResponse</returns>
        [HttpPost("api/credito/CrearProveedorSevenCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SyncPoPvdorResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Proveedor en seven para temas de desembolso",
           Description = "Crear Proveedor en seven para temas de desembolso",
           OperationId = "Credito.CrearProveedorSevenCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<SyncPoPvdorResponse>>> HandleAsync(RequestCreditoSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearProveedorSeven(request, cancellationToken);
        }
    }
}
