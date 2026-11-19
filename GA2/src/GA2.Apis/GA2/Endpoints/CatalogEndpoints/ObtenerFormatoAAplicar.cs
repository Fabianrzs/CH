using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]

    public class ObtenerFormatoAAplicar : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<FormatoAplicarDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerFormatoAAplicar(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerFormatoAAplicar")]
        [ProducesResponseType(typeof(Response<CategoriaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener formato a aplicar",
            Description = "Obtener formato a aplicar",
            OperationId = "catalogos.ObtenerFormatoAAplicar",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<FormatoAplicarDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogosBusinessLogic.ObtenerFormatoAAplicar();
        }
    }
}
