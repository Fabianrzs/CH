using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Proyecto;
using GA2.Application.Dto;
using GA2.Domain.Core.Proyecto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class ObtenerProyecto : BaseAsyncEndpoint.WithRequest<RequestConstructoraProyectoDto>
        .WithResponse<Response<List<ProyectoConstructoraResponseDto>>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public ObtenerProyecto(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpGet("api/ProyectoConstructora/obtenerProyecto")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Obtener todos proyecto de constructora",
                   Description = "Obtener todos de constructora",
                   OperationId = "ProyectoConstructora.obtenerProyecto",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<List<ProyectoConstructoraResponseDto>>>> HandleAsync([FromQuery] RequestConstructoraProyectoDto request,CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.ObtenerProyectos(request);
        }
    }
}
