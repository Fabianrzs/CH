using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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
    public class InsertarActualizarAvaluoComercialCredito
        : BaseAsyncEndpoint.WithRequest<AvaluoComercialCreditoDto>.WithResponse<Response<AvaluoComercialCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint InsertarActualizarAvaluoComercialCredito
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public InsertarActualizarAvaluoComercialCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpPost("api/credito/InsertarActualizarAvaluoComercialCredito")]
        [ProducesResponseType(typeof(Response<AvaluoComercialCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar o Actualizar Avalúo Comercial",
            Description = "Inserta o actualiza la información del Avalúo Comercial.",
            OperationId = "credito.InsertarActualizarAvaluoComercialCredito",
            Tags = new[] { "AvaluoComercialCreditoEndpoint" })]
        public override async Task<ActionResult<Response<AvaluoComercialCreditoDto>>> HandleAsync(
            AvaluoComercialCreditoDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarActualizarAvaluoComercialCredito(request);
        }
    }
}