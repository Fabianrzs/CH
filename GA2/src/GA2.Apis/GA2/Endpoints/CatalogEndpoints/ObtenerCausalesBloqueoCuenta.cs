using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    [Authorize]
    public class ObtenerCausalesBloqueoCuenta : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<CausalesBloqueoCuentaDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogos;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="catalogos"></param>
        public ObtenerCausalesBloqueoCuenta(ICatalogosBusinessLogic catalogos)
        {
            _catalogos = catalogos;
        }

        /// <summary>
        /// obtener Abono Unico
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/catalogos/ObtenerCausalesBloqueoCuenta")] 
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<CausalesBloqueoCuentaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Obtener causales de bloqueo de cuenta",
            Description = "Obtener causales de bloqueo de cuenta",
            OperationId = "catalogos.ObtenerCausalesBloqueoCuenta",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CausalesBloqueoCuentaDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogos.ObtenerCausalesBloqueoCuenta();
        }
    }
}
