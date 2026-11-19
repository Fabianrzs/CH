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
    public class ConsultarBloqueoCuentaCredito : BaseAsyncEndpoint.WithRequest<BloqueoRecursosCreditoDto>.WithResponse<Response<List<BloqueoRecursosCreditoRespuesta>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ConsultarBloqueoCuentaCredito(ICreditoBusinessLogic creditoBusinessLogic)
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
        [HttpPost("api/credito/ConsultarBloqueoCuentaCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<BloqueoRecursosCreditoRespuesta>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar el bloqueo de la cuenta del afiliado",
            Description = "Consulta el bloqueo de la cuenta del afiliado",
            OperationId = "Credito.ConsultarBloqueoCuentaCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<List<BloqueoRecursosCreditoRespuesta>>>> HandleAsync(BloqueoRecursosCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarBloqueoCuentaCredito(request); 
        }
    }
}
