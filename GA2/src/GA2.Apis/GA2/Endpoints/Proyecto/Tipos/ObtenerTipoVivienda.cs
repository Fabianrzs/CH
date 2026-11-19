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
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Proyecto.Tipos
{
    public class ObtenerTipoVivienda : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<List<TipoIncmueblesDto>>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public ObtenerTipoVivienda(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpGet("api/ProyectoConstructora/ObtenerTipoVivienda")]
        [ProducesResponseType(typeof(Response<List<TipoIncmueblesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Obtener los tipo vivienda por codigo",
                   Description = "Obtener todos los tipo vivienda por codigo",
                   OperationId = "ProyectoConstructora.obtenerProyectoById",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<List<TipoIncmueblesDto>>>> HandleAsync(int codigo, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.ObtenerTipoVivienda(codigo);
        }
    }
}
