using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
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
    /// Obtiene el detalle del pago registrado en seven
    /// </summary>
    /// <author>Juan Moncayo</author>
    [Authorize]
    public class ObtenerDetallePagoSeven : BaseAsyncEndpoint
        .WithRequest<RequestDetalleSeven>
        .WithResponse<Response<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>>>
    {
        //IDetallePagoSevenLogic _detallePagoSevenLogic;
        ICreditoBusinessLogic _creditoBusinessLogic;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerDetallePagoSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }
        /// <summary>
        /// Obtiene el detalle del pago registrado en seven
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerDetallePagoSeven")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el detalle del pago registrado en seven (\"ObtenerDetallePagoSeven\").",
            Description = "proceso obtener detalle de pago en seven",
            OperationId = "credito.ObtenerDetallePagoSeven",
            Tags = new[] { "CreditoEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>>>> HandleAsync(RequestDetalleSeven request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerDetallePagoSeven(request);//jm
        }
    }
}
