
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
    /// Obtiene el detalle de las facturas
    /// </summary>
    /// <author>Juan Moncayo</author>
    [Authorize]
    public class ObtenerValorFactura : BaseAsyncEndpoint
        .WithRequest<RequestValoresFactura>
        .WithResponse<Response<IEnumerable<ObtenerDetalleFacturaSevenDto>>>
    {
        //IDetallePagoSevenLogic _detallePagoSevenLogic;
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerValorFactura(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Obtiene el detalle de las facturas
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerValorFactura")]
        [ProducesResponseType(typeof(Response<IEnumerable<ObtenerDetalleFacturaSevenDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el detalle de las facturas (\"ObtenerValorFactura\").",
            Description = "proceso obtener el detalle de las facturas enviadas  a seven",
            OperationId = "credito.ObtenerValorFactura",
            Tags = new[] { "CreditoEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ObtenerDetalleFacturaSevenDto>>>> HandleAsync(RequestValoresFactura request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerValorFactura(request);//jm
        }
    }
}
