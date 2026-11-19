using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Credito
{
    public class ConsultaValoresSimulacionCreditoReclasificiacion
        : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ValoresSimulacionCreditoReclasificacion>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint ConsultaValoresSimulacionCreditoReclasificiacion
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ConsultaValoresSimulacionCreditoReclasificiacion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/credito/ConsultaValoresSimulacionCreditoReclasificiacion/{numeroDocumento}")]
        [ProducesResponseType(typeof(Response<ValoresSimulacionCreditoReclasificacion>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consulta Valores Simulación Crédito Reclasificación",
            Description = "Consulta los valores de la simulación de crédito para reclasificación por número de documento.",
            OperationId = "credito.ConsultaValoresSimulacionCreditoReclasificiacion",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<ValoresSimulacionCreditoReclasificacion>>> HandleAsync(
            string numeroDocumento,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultaValoresSimulacionCredito(numeroDocumento);
        }
    }
}