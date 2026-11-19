using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
	/// Endpoint para Obtener lista de niveles educativos
	/// </summary>
	/// <author>David Pineda/author>
	/// <date>03/03/2023</date>
    [Authorize]
    public class ObtenerNivelEducativo :
        BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<NivelEducativoDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        public ObtenerNivelEducativo(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }

        /// <summary>
        /// Obtiene todos los niveles educativos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/catalogos/ObtenerNivelEducativo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<NivelEducativoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo de nivel educativo",
            OperationId = "catalogos.ObtenerNivelEducativo",
            Tags = new[] { "CatalogosEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<NivelEducativoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerNivelEducativo();
        }
    }
}
