using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    [Authorize]

    public class ObtenerSolicCreditoReferencias : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<SolicCreditoReferernciasDto>>>
    {

        ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerSolicCreditoReferencias(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        

        /// <summary>
        /// Obtiene all ciudades
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerSolicCreditoReferencias")]
        [ProducesResponseType(typeof(Response<SolicCreditoReferernciasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiener las referencias de solicitud de credito",
            Description = "Obtiener las referencias de solicitud de credito",
            OperationId = "credito.ObtenerSolicCreditoReferencias",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<SolicCreditoReferernciasDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerSolicCreditoReferencias(request);    
        }
    }
}
