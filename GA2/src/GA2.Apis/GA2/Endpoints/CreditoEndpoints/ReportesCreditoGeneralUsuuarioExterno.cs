using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ReportesSolicitudCreditoGeneralUsuuarioExterno :
        BaseAsyncEndpoint.WithRequest<ReportesSolicitudCreditoDto>
        .WithResponse<Response<IEnumerable<ReportesSolicitudCredito>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="businessLogic"></param>
        public ReportesSolicitudCreditoGeneralUsuuarioExterno(ICreditoBusinessLogic businessLogic)
        {
            _creditoBusinessLogic = businessLogic;
        }

        /// <summary>
        /// Obtiene el reporte general de solicitudes de crédito para usuario externo.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ReportesSolicitudCreditoGeneralUsuarioExterno")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReportesSolicitudCredito>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Reporte General de Solicitudes de Crédito - Usuario Externo",
            Description = "Obtiene el reporte general de solicitudes de crédito filtrado para usuario externo.",
            OperationId = "credito.ReportesSolicitudCreditoGeneralUsuuarioExterno",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReportesSolicitudCredito>>>> HandleAsync(
            ReportesSolicitudCreditoDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ReportesSolicitudCreditoGeneralUsuuarioExterno(request);
        }
    }
}