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
    public class ObtenerSalarioMinimoAnual : BaseAsyncEndpoint
        .WithRequest<ObtenerSmlvAnualDto>
        .WithResponse<Response<SmlvAnualDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerSalarioMinimoAnual(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [Authorize]
        [HttpGet("api/ObtenerSalarioMinimoAnual")]
        [ProducesResponseType(typeof(Response<SmlvAnualDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el salario mínimo anual",
            Description = "Obtiene el salario mínimo anual basado en los parámetros proporcionados.",
            OperationId = "credito.ObtenerSalarioMinimoAnual",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<SmlvAnualDto>>> HandleAsync(
            [FromQuery] ObtenerSmlvAnualDto request, 
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerSalarioMinimoAnual(request);
        }
    }
}
