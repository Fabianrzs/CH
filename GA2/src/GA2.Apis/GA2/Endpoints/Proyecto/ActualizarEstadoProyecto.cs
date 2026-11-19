using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Core.Proyecto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class ActualizarEstadoProyecto : BaseAsyncEndpoint.WithRequest<ActualizarProyectoDto>
        .WithResponse<Response<string>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public ActualizarEstadoProyecto(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpPost("api/ProyectoConstructora/ActualizarEstadoProyecto")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Actualizar estado inmueble  de proyecto  constructora",
                   Description = "Actualizar estado inmueble  de proyecto  constructora",
                   OperationId = "ProyectoConstructora.ActualizarEstadoProyecto",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(ActualizarProyectoDto request, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.ActualizarEstadoProyecto(request);
        }
    }
}
