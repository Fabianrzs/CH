using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Catalogs;
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

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    /// <summary>
    /// OBtener estados documento endpoint
    /// </summary>
    [Authorize]
    public class ObtenerEstadosDocumento : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<EstadoDocumentoDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Instancia endpoint
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObtenerEstadosDocumento(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic ?? throw new ArgumentNullException(nameof(catalogsBusinessLogic));
        }

        /// <summary>
        /// Obtener estados documentos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/catalogos/ObtenerEstadosDocumento")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoDocumentoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerEstadosDocumento",
            Tags = new[] { "CatalogosEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<EstadoDocumentoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _catalogsBusinessLogic.ObtenerEstadosDocumento();
        }
    }
}
