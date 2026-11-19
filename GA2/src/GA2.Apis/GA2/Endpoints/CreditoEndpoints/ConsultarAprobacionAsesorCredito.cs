using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarAprobacionAsesorCredito : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<AprobacionCreditoAsesor>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarAprobacionAsesorCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/credito/ConsultarAprobacionAsesorCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<AprobacionCreditoAsesor>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta de notificaciones",
           Description = "Consulta de notificaciones",
           OperationId = "Credito.ConsultarAprobacionAsesorCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<AprobacionCreditoAsesor>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarAprobacionesCreditoAsesor(request);
        }
    }
}
