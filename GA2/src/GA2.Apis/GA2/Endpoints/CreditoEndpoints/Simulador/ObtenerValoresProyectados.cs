using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito.Simulador;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador
{
    public class ObtenerValoresProyectados : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<ValoresProyectadosDto>>
    {
        private readonly ICreditoBusinessLogic _iCreditoBusinessLogic;
        public ObtenerValoresProyectados(ICreditoBusinessLogic iCreditoBusinessLogic)
        {
            _iCreditoBusinessLogic = iCreditoBusinessLogic;
        }

        [HttpPost("/api/credito/ObtenerValoresProyectados")]
        [ProducesResponseType(typeof(Response<ValoresProyectadosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation("Obtener Valores Proyectados",
            Description = "Obtener Valores Proyectados",
            OperationId = "credito.ObtenerValoresProyectados",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ValoresProyectadosDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _iCreditoBusinessLogic.ObtenerValoresProyectados(request);
        }
    }
}
