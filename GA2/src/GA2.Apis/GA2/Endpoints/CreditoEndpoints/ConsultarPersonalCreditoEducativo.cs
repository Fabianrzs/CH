using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarPersonalCreditoEducativo : BaseAsyncEndpoint
        .WithRequest<DatosPersonalesCreditoEducativoDto>
        .WithResponse<Response<DatosPersonalesCreditoEducativo>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarPersonalCreditoEducativo(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpPost("api/credito/ConsultarPersonalCreditoEducativo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(DatosPersonalesCreditoEducativo), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar datos personales de crédito educativo",
           Description = "Consulta la información personal asociada a un crédito educativo con base en el número de documento.",
           OperationId = "Credito.ConsultarPersonalCreditoEducativo",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<DatosPersonalesCreditoEducativo>>> HandleAsync(DatosPersonalesCreditoEducativoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarDatosPersonalesEducativo(request); 
        }
    }
}