using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class CrearSolicCreditoRefererncias
    :
         BaseAsyncEndpoint.WithRequest<SolicitudCreditoReferernciasDto>
        .WithResponse<Response<SolicitudCreditoReferernciasDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;

        public CrearSolicCreditoRefererncias(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CrearSolicCreditoRefererncias")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<SolicitudCreditoReferernciasDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear referencias solicitud de credito",
           Description = "Crear referencias solicitud de credito",
           OperationId = "Credito.CrearSolicCreditoRefererncias",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<SolicitudCreditoReferernciasDto>>> HandleAsync(SolicitudCreditoReferernciasDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearSolicCreditoRefererncias(request);
        }
    }
}
