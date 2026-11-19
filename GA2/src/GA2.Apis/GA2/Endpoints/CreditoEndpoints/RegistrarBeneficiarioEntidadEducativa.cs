using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class RegistrarBeneficiarioEntidadEducativa
    : BaseAsyncEndpoint.WithRequest<BeneficiarioEntidadEducativaDto>.WithResponse<Response<BeneficiarioEntidadEducativa>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint registrar la informacion de la entidad educativa 
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public RegistrarBeneficiarioEntidadEducativa(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// registrar la informacion de la entidad educativa 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/RegistrarBeneficiarioEntidadEducativa")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DatosPersonalesCreditoEducativo>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "registrar la informacion de la entidad educativa ",
           Description = "registrar la informacion de la entidad educativa ",
           OperationId = "Credito.RegistrarBeneficiarioEntidadEducativa",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<BeneficiarioEntidadEducativa>>> HandleAsync(BeneficiarioEntidadEducativaDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarBeneficiarioEntidadEducativa(request); 
        }
    }
}
