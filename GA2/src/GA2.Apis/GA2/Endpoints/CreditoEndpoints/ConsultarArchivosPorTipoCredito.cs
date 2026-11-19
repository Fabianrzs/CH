using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ConsultarArchivosPorTipoCredito : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ArchivosCredito>>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint consultar documentos de credito
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ConsultarArchivosPorTipoCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Check documentos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/ConsultarArchivosPorTipoCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Documentos Checks",
           Description = "Documentos Checks",
           OperationId = "Credito.ConsultarArchivosPorTipoCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ArchivosCredito>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarArchivosPorTipoCredito(); 
        }
    }
}
