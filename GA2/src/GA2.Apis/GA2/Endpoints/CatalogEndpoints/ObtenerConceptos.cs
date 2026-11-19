using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    public class ObtenerConceptos :  BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ConceptoDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerConceptos(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerConceptos")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConceptoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Conceptos",
            Description = "Obtener Conceptos",
            OperationId = "catalogos.ObtenerConceptos",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConceptoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerConceptos();
        }
    }
}
