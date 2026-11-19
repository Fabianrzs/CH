using GA2.Application.Dto.Proyecto;
using GA2.Application.Dto;
using GA2.Domain.Core.Proyecto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using System;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class ObtenerProyectoById :BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<ProyectoConstructoraResponseDto>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

    public ObtenerProyectoById(IProyectoBusiness proyectoBusiness)
    {
        _proyectoBusiness = proyectoBusiness;
    }

    [HttpGet("api/ProyectoConstructora/obtenerProyectoById")]
    [ProducesResponseType(typeof(Response<IEnumerable<EstadoConvocatoriaDto>>), StatusCodes.Status200OK)]
    [SwaggerOperation(
               Summary = "Obtener  proyecto de constructora por codigo",
               Description = "Obtener todos de constructora",
               OperationId = "ProyectoConstructora.obtenerProyectoById",
               Tags = new[] { "ProyectoConstructora" })]
    public async override Task<ActionResult<Response<ProyectoConstructoraResponseDto>>> HandleAsync(Guid Id,CancellationToken cancellationToken = default)
    {
        return await _proyectoBusiness.ObtenerProyectosById(Id);
    }
}
}
