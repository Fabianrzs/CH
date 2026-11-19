using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    public class ObtenerConceptosNovedades : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<ConceptoNovedadDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerConceptosNovedades(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerConceptosNovedades")]
        [ProducesResponseType(typeof(Response<CategoriaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "ObtenerConceptosNovedades",
            Description = "ObtenerConceptosNovedades",
            OperationId = "catalogos.ObtenerConceptosNovedades",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConceptoNovedadDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerConceptosNovedades();
        }
    }
}
