using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarInformacionCreditoEducativo : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<InformacionGeneralCreditoEducativo>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarInformacionCreditoEducativo(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/credito/ConsultarInformacionCreditoEducativo")]
        [ProducesResponseType(typeof(Response<InformacionGeneralCreditoEducativo>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar información de crédito educativo",
            Description = "Obtiene la información de crédito educativo de un estudiante con base en su número de documento",
            OperationId = "credito.ConsultarInformacionCreditoEducativo",
            Tags = new[] { "CreditoEndpoint" })]

        public override async Task<ActionResult<Response<InformacionGeneralCreditoEducativo>>> HandleAsync(string numeroDocumento, CancellationToken cancellationToken = default)
        {        
            return await _creditoBusinessLogic.ConsultarInformacionCreditoEducativo(numeroDocumento);

        }
    }
}
