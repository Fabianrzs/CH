using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.AvaluoComercialCredito
{
    public class ConsultarAvaluoComercialCredito
        : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<AvaluoComercialCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint ConsultarAvaluoComercialCredito
        /// </summary>
        public ConsultarAvaluoComercialCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/credito/ConsultarAvaluoComercialCredito/{socId}")]
        [ProducesResponseType(typeof(Response<AvaluoComercialCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Avalúo Comercial",
            Description = "Consultar Avalúo Comercial por Solicitud de Crédito",
            OperationId = "credito.ConsultarAvaluoComercialCredito",
            Tags = new[] { "AvaluoComercialCreditoEndpoint" })]
        public override async Task<ActionResult<Response<AvaluoComercialCreditoDto>>> HandleAsync(
            Guid socId,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarAvaluoComercialCredito(socId);
        }
    }
}