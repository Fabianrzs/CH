using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
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
    /// Obtiene comprobantes de credito (cdp y factura)
    /// </summary>
    /// <author>Juan Moncayo</author>
    [Authorize]
    public class ObtenerComprobantesCredito : BaseAsyncEndpoint
        .WithRequest<RequestObtenerComprobantesCredito>
        .WithResponse<Response<IEnumerable<ObtenerComprobantesDto>>>
    {
        //IDetallePagoSevenLogic _detallePagoSevenLogic;
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerComprobantesCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Obtiene comprobantes de credito (cdp y factura)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerComprobantesCredito")]
        [ProducesResponseType(typeof(Response<IEnumerable<ObtenerComprobantesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene comprobantes de credito (\"ObtenerComprobantesCredito\").",
            Description = "proceso obtener los comprobantes por fecha de cdp y factura",
            OperationId = "credito.ObtenerComprobantesCredito",
            Tags = new[] { "CreditoEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ObtenerComprobantesDto>>>> HandleAsync(RequestObtenerComprobantesCredito request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerComprobantesCredito(request);//jm
        }
    }
}
