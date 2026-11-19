using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    public class RegistrarBloqueoCuentaCredito : BaseAsyncEndpoint.WithRequest<BloqueoRecursosCreditoDto>.WithResponse<Response<bool>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public RegistrarBloqueoCuentaCredito(ICreditoBusinessLogic creditoBusinessLogic)
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
        [HttpPost("api/credito/RegistrarBloqueoCuentaCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Registrar el bloqueo de la cuenta del afiliado en la actividad bloqueo de recursos",
            Description = "Registrar el bloqueo de la cuenta del afiliado en la actividad bloqueo de recursos",
            OperationId = "Credito.RegistrarBloqueoCuentaCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(BloqueoRecursosCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.RegistrarBloqueoCuentaCredito(request);
        }
    }
}
