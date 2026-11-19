using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{   
    public class ActualizarInformacionPersonalCreditoEducativo: BaseAsyncEndpoint.WithRequest<DatosPersonalesCreditoEducativoDto>.WithResponse<Response<DatosPersonalesCreditoEducativo>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint actualizar datos personales de credito educativo
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ActualizarInformacionPersonalCreditoEducativo(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// actualizar datos personales de credito educativo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/ActualizarInformacionPersonalCreditoEducativo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DatosPersonalesCreditoEducativo>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar datos datos personales de credito educativo",
           Description = "Actualizar datos datos personales de credito educativo",
           OperationId = "Credito.ActualizarInformacionPersonalCreditoEducativo",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<DatosPersonalesCreditoEducativo>>> HandleAsync(DatosPersonalesCreditoEducativoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ActualizarDatosPersonalesEducativo(request);
        }
    }
}
