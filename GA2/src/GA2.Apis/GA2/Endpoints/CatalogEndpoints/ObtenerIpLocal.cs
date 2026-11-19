using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    public class ObtenerIpLocal : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<string>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        
        public ObtenerIpLocal(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }
        [HttpGet("api/Catalogos/ObtenerIpLocal")]
        [ProducesResponseType(typeof(Response<IEnumerable<CategoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene ip local",
           Description = "Obtiene ip local",
           OperationId = "catalogos.ObtenerIpLocal",
           Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerIpLocal();
        }
    }
}
