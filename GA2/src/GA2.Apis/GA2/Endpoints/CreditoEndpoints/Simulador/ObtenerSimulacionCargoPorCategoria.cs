using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador
{
    public class ObtenerSimulacionCargoPorCategoria : BaseAsyncEndpoint.WithRequest<SimulacionCargoCategoriaDto>.WithResponse<Response<CargoCategoriaDto>>
    {
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;

        public ObtenerSimulacionCargoPorCategoria(ICatalogosBusinessLogic catalogosBusinessLogic)
        {
            _catalogosBusinessLogic = catalogosBusinessLogic;
        }

        [Authorize]

        /// <summary>
        /// Obtiene todos seguros por ubicacion
        /// <param name="request"></param>
        /// <author>Michel Parra</author>
        /// <date>16/07/2024</date>
        /// <returns>Listado de los cargos para la proyeccion</returns>
        [HttpPost("api/ObtenerSimulacionCargoPorCategoria")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProyeccionClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Listado de los cargos para la proyeccion",
           Description = "Listado de los cargos para la proyeccion",
           OperationId = "credito.ObtenerSimulacionCargoPorCategoria",
           Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<CargoCategoriaDto>>> HandleAsync(SimulacionCargoCategoriaDto request, CancellationToken cancellationToken = default)
        {
            return await this._catalogosBusinessLogic.ObtenerSimulacionCargoPorCategoria(request.CategoriaId,request.CargoId);
        }
    }
}
