using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarValidacionCreditoProspeccion
        : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ValidacionCreditoProspeccionDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint ConsultarValidacionCreditoProspeccion
        /// </summary>
        public ConsultarValidacionCreditoProspeccion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/credito/ConsultarValidacionCreditoProspeccion/{numeroDocumento}")]
        [ProducesResponseType(typeof(Response<ValidacionCreditoProspeccionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Validación Crédito Prospección",
            Description = "Consulta la validación de crédito por número de documento.",
            OperationId = "credito.ConsultarValidacionCreditoProspeccion",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionCreditoProspeccionDto>>> HandleAsync(
            string numeroDocumento,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarValidacionCreditoProspeccion(numeroDocumento);
        }
    }
}