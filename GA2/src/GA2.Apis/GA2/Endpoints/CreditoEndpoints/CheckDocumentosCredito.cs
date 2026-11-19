using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class CheckDocumentosCredito : BaseAsyncEndpoint.WithRequest<CheckDocumentosCreditoDto>.WithResponse<Response<bool>>
    {


        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public CheckDocumentosCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Check documentos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/CheckDocumentosCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Documentos Checks",
           Description = "Documentos Checks",
           OperationId = "Credito.CheckDocumentosCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(CheckDocumentosCreditoDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.CheckDocumentosCredito(request); 
        }
    }
}
