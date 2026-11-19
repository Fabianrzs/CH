using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    [Authorize]
    public class ConsultarAliadosEstrategicosCredito : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<AliadosEstrategicos>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarAliadosEstrategicosCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/Credito/ConsultarAliadosEstrategicos")]
        [ProducesResponseType(typeof(Response<IEnumerable<AliadosEstrategicos>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar aliados estratégicos",
            Description = "Permite consultar el listado de aliados estratégicos.",
            OperationId = "credito.ConsultarAliadosEstrategicos",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<AliadosEstrategicos>>>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var dataResponse = await _creditoBusinessLogic.ConsultarAliadosEstrategicos();

            return Ok(dataResponse);
        }
    }
}