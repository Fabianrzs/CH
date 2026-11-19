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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    [Authorize]
    public class ActualizaTramitePerito : BaseAsyncEndpoint.WithRequest<RequestTramiteSolicitudDto>.WithResponse<Response<TramiteSolicitudDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ActualizaTramitePerito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/ActualizaTramitePerito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<TramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza tramite perito",
           Description = "Actualiza tramite perito",
           OperationId = "Credito.ActualizaTramitePerito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<TramiteSolicitudDto>>> HandleAsync(RequestTramiteSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizaTramitePerito(request);
        }
    }
}
