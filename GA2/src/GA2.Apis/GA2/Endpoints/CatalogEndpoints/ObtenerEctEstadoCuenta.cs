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
    /// Endpoint para Obtener los estados de las cuentas
    /// </summary>
    /// <author>Michel Parra</author>
    /// <date>12/07/2023</date>

    [Authorize]
    public class ObtenerEctEstadoCuenta : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<EstadoCuentaDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        /// <summary>
        /// Constructor del endpoint
        /// </summary>
        /// <param name="catalogosBusinessLogic"></param>
        public ObtenerEctEstadoCuenta(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }
        /// <summary>
        /// Obtiene los estados del usuario en cartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerEctEstadoCuenta")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoCuentaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerEctEstadoCuenta",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EstadoCuentaDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.ObtenerEctEstadoCuenta();
        }
    }
}
