using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Domain.Entities;
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
    public class ObtenerTiposDocumento
        : BaseAsyncEndpoint
            .WithoutRequest
            .WithResponse<Response<IEnumerable<TipoDocumento>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Obtener Tipos de Documento
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        /// <author>Alejandro Lara</author>
        public ObtenerTiposDocumento(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Obtiene el catálogo de tipos de documento.
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("api/credito/ObtenerTiposDocumento")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoDocumento>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener tipos de documento",
            Description = "Obtiene el catálogo de tipos de documento.",
            OperationId = "Credito.ObtenerTiposDocumento",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<TipoDocumento>>>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerTiposDocumento();
        }
    }
}