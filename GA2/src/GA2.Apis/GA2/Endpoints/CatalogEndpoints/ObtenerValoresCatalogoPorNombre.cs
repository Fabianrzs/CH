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
	/// Endpoint para Obtener Catalogos por nombre
	/// </summary>
	/// <author>David Alejandro Pineda Diaz</author>
	/// <date>06/003/2023</date>
    [Authorize]
    public class ObtenerValoresCatalogoPorNombre :
        BaseAsyncEndpoint.WithRequest<String>.WithResponse<Response<IEnumerable<CatalogoValorDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;


        public ObtenerValoresCatalogoPorNombre(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }


        [HttpGet("api/catalogos/ObtenerValoresCatalogoPorNombre")]
        [ProducesResponseType(typeof(Response<IEnumerable<CatalogoValorDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene valores por nombre de catalogo",
            OperationId = "catalogos.ObtenerValoresCatalogoPorNombre",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CatalogoValorDto>>>> HandleAsync(string nombreCatalogo, CancellationToken cancellationToken = default)
        {
            return await this._catalogsBusinessLogic.ObtenerValoresCatalogoPorNombre(nombreCatalogo);
        }
    }
}
