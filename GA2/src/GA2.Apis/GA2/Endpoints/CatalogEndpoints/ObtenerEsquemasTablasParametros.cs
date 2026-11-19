using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    /// <summary>
    /// Endpoint para Obtener esquemas de las tablas parametros
    /// </summary>
    /// <author>Michel Parra</author>
    /// <date>22/07/2023</date>

    [Authorize]
    public class ObtenerEsquemasTablasParametros :
         BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<EsquemaTablaParametroDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del endpoint
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        public ObtenerEsquemasTablasParametros(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }
        /// <summary>
        /// Obtiene el esquema de las tablas de parametros
        /// </summary>
        /// <param name="nombreTabla"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerEsquemasTablasParametros")]
        [ProducesResponseType(typeof(Response<IEnumerable<EsquemaTablaParametroDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerEsquemasTablasParametros",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EsquemaTablaParametroDto>>>> HandleAsync(string nombreTabla, CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.ObtenerEsquemasTablasParametros(nombreTabla);
        }
    }
}
