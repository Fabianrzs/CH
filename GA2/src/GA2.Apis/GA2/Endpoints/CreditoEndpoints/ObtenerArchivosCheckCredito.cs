using Ardalis.ApiEndpoints;
using System.Collections.Generic;
using System;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ObtenerArchivosCheckCredito : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<CheckArchivo>>>
    {

        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ObtenerArchivosCheckCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Check documentos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>                
        [HttpPost("api/credito/ObtenerCheckDocumentosCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Documentos Checks",
           Description = "Documentos Checks",
           OperationId = "Credito.ObtenerCheckDocumentosCredito",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<CheckArchivo>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerArchivosChecksCredito(request); 
        }
    }
}
