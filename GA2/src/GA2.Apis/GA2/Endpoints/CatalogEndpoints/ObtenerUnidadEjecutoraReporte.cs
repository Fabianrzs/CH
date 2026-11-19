using Ardalis.ApiEndpoints;
using Azure;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CatalogEndpoints
{
    /// <summary>
    /// Obtiene los tipos UE
    /// </summary>
    [Authorize]
    public class ObtenerUnidadEjecutoraReporte : 
        BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TipoUnidadEjecutoraReporteDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;
        /// <summary>
        /// Constructor de Endpoint catalogo UE
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>10/11/2023</date>
        /// 

        public ObtenerUnidadEjecutoraReporte(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }
        /// <summary>
        /// Obtiene Unidad Ejecutora
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/catalogos/ObtenerUnidadEjecutoraReporte")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoUnidadEjecutoraReporteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerUnidadEjecutoraReporte",
            Tags = new[] { "CatalogosEndpoint" })]
  
        public override async Task<ActionResult<Response<IEnumerable<TipoUnidadEjecutoraReporteDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _catalogsBusinessLogic.ObtenerUnidadEjecutoraReporte();

            return response != null ? (ActionResult<Response<IEnumerable<TipoUnidadEjecutoraReporteDto>>>)Ok(response) : NotFound();
        }
    }
}
