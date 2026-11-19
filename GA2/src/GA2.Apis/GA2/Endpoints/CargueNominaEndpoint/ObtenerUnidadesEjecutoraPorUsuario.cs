using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CargueNominaEndpoint
{
    public class ObtenerUnidadesEjecutoraPorUsuario
    : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<UnidadEjecutoraDto>>>
    {
        private readonly ICatalogosBusinessLogic _catalogsBusinessLogic;

        /// <summary>
        /// Constructor de Endpoint Unidades Ejecutoras
        /// </summary>
        /// <param name="catalogsBusinessLogic"></param>
        /// <author>Cristian Gonzalez</author>
        /// <date>18/05/2021</date>
        public ObtenerUnidadesEjecutoraPorUsuario(ICatalogosBusinessLogic catalogsBusinessLogic)
        {
            _catalogsBusinessLogic = catalogsBusinessLogic;
        }


        /// <summary>
        /// Obtiene all unidades ejecutoras
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Catalogos/ObtenerUnidadesEjecutoraPorUsuario")]
        [ProducesResponseType(typeof(Response<IEnumerable<UnidadEjecutoraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene catalogo",
            Description = "Obtiene catalogo",
            OperationId = "catalogos.ObtenerUnidadesEjecutoraPorUsuario",
            Tags = new[] { "CatalogosEndpoint" })]
  
        public override async Task<ActionResult<Response<IEnumerable<UnidadEjecutoraDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _catalogsBusinessLogic.ObtenerUnidadesEjecutorasPorUsuario(request);

        }
    }
}
