using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarInformacionPrimerPagoCDP
    : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<PrimerPagoCDPCredito>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarInformacionPrimerPagoCDP(ICreditoBusinessLogic creditoBusinessLogic)
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
        [HttpPost("api/credito/ConsultarInformacionPrimerPagoCDP")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PrimerPagoCDPCredito>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta de notificaciones",
           Description = "Consulta de notificaciones",
           OperationId = "Credito.ConsultarInformacionPrimerPagoCDP",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<PrimerPagoCDPCredito>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarPrimerPagoCDP(request); 
        }
    }
}
