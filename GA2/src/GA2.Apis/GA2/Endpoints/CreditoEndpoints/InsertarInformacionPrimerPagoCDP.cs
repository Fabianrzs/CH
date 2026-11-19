using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class InsertarInformacionPrimerPagoCDP
    : BaseAsyncEndpoint.WithRequest<InsertarDatosCDPPrimerPago>.WithResponse<bool>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public InsertarInformacionPrimerPagoCDP(ICreditoBusinessLogic creditoBusinessLogic)
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
        [HttpPost("api/credito/InsertarInformacionPrimerPagoCDP")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta de notificaciones",
           Description = "Consulta de notificaciones",
           OperationId = "Credito.InsertarInformacionPrimerPagoCDP",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<bool>> HandleAsync(InsertarDatosCDPPrimerPago request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarPrimerPagoCDP(request);
        }
    }
}
