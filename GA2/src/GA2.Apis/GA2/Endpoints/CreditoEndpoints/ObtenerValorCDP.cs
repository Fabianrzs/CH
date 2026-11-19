using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Obtiene el valor del cdp
    /// </summary>
    /// <author>Juan Moncayo</author>
    [Authorize]
    public class ObtenerValorCDP : BaseAsyncEndpoint
        .WithRequest<RequestValoresCDPSeven>
        .WithResponse<Response<IEnumerable<ObtenerValoresCDPSevenDto>>>
    {
        //IDetallePagoSevenLogic _detallePagoSevenLogic;
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerValorCDP(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Obtiene el valor del cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerValorCdp")]
        [ProducesResponseType(typeof(Response<IEnumerable<ObtenerValoresCDPSevenDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el valor del cdp (\"ObtenerValorCdp\").",
            Description = "proceso obtener valor del cdp enviado a seven",
            OperationId = "credito.ObtenerValorCdp",
            Tags = new[] { "CreditoEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ObtenerValoresCDPSevenDto>>>> HandleAsync(RequestValoresCDPSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerValorCDP(request);//jm
        }
    }
}
