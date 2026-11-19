using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Credito.Simulador;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador
{
    public class GuardarValoresProyectados : BaseAsyncEndpoint.WithRequest<ValoresProyectadosDto>.WithResponse<Response<ValoresProyectadosDto>>
    {
        private readonly ICreditoBusinessLogic _iCreditoBusinessLogic;

        public GuardarValoresProyectados(ICreditoBusinessLogic iCreditoBusinessLogic)
        {
            _iCreditoBusinessLogic = iCreditoBusinessLogic;
        }

        [HttpPost("/api/credito/GuardarValoresProyectados")]
        [ProducesResponseType(typeof(Response<ValoresProyectadosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation("GuardarValoresProyectados",
            Description = "GuardarValoresProyectados",
            OperationId = "credito.GuardarValoresProyectados",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ValoresProyectadosDto>>> HandleAsync(ValoresProyectadosDto request, CancellationToken cancellationToken = default)
        {
            return await _iCreditoBusinessLogic.GuardarValoresProyectados(request);
        }

    }
}
