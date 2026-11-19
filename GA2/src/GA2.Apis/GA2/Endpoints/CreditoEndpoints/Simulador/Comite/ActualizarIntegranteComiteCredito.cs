using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Credito.Simulador.Comite;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.Comite
{
    [Authorize]
    public class ActualizarIntegranteComiteCredito : BaseAsyncEndpoint.WithRequest<CrearIntegranteComiteCreditoDto>.WithResponse<Response<IntegranteComiteCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ActualizarIntegranteComiteCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpPost("api/credito/ActualizarIntegranteComiteCredito")]
        [ProducesResponseType(typeof(Response<IntegranteComiteCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar Integrante Comite Credito",
            Description = "Actualizar Integrante Comite Credito",
            OperationId = "credito.ActualizarIntegranteComiteCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IntegranteComiteCreditoDto>>> HandleAsync(CrearIntegranteComiteCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ActualizarIntegranteComiteCredito(request);
        }
    }
}
