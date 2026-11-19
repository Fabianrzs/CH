using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    /// <summary>
    /// Endpoint para consultar el tipo de crédito en simulación
    /// </summary>
    public class ConsultarTipoCreditoSimulacion :
        BaseAsyncEndpoint
        .WithRequest<ConsultarCreditoSimulacionDto>
        .WithResponse<Response<TipoCreditoSimulado>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsultarTipoCreditoSimulacion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        /// <summary>
        /// Consulta el tipo de crédito según el número de documento
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ConsultarTipoCreditoSimulacion")]
        [ProducesResponseType(typeof(Response<TipoCreditoSimulado>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar tipo de crédito simulación",
            Description = "Determina el tipo de crédito (Leasing, Cesantías, Veteranos) basado en reglas de negocio",
            OperationId = "Credito.ConsultarTipoCreditoSimulacion",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<TipoCreditoSimulado>>> HandleAsync(
            ConsultarCreditoSimulacionDto request,
            CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarTipoCredito(request); 
        }
    }
}