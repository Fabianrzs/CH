using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    /// <summary>
    /// Endpoint para Obtener las categorias de las tablas parametros
    /// </summary>
    /// <author>Michel Parra</author>
    /// <date>12/07/2023</date>

    [Authorize]
    public class ObtenerCategoriasTablasParametros : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<CategoriaTablaParametroDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del endpoint
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        public ObtenerCategoriasTablasParametros(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }
        /// <summary>
        /// Obtiene las categorias de las tablas de parametros
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerCategoriasTablasParametros")]
        [ProducesResponseType(typeof(Response<IEnumerable<CategoriaTablaParametroDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerCategoriasTablasParametros",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CategoriaTablaParametroDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.ObtenerCategoriasTablasParametros();
        }
    }
}
