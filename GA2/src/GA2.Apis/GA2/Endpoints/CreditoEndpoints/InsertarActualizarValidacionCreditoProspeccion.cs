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
    public class InsertarActualizarValidacionCreditoProspeccion
        : BaseAsyncEndpoint.WithRequest<ValidacionCreditoProspeccionDto>.WithResponse<Response<ValidacionCreditoProspeccionDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint InsertarActualizarValidacionCreditoProspeccion
        /// </summary>
        public InsertarActualizarValidacionCreditoProspeccion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpPost("api/credito/InsertarActualizarValidacionCreditoProspeccion")]
        [ProducesResponseType(typeof(Response<ValidacionCreditoProspeccionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar o Actualizar Validación Crédito Prospección",
            Description = "Inserta o actualiza la validación de crédito de prospección.",
            OperationId = "credito.InsertarActualizarValidacionCreditoProspeccion",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ValidacionCreditoProspeccionDto>>> HandleAsync(
            ValidacionCreditoProspeccionDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarActualizarValidacionCreditoProspeccion(request);
        }
    }
}