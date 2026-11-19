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
    public class TipoInmueble : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<List<TipoIncmueblesDto>>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public TipoInmueble(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpGet("api/ProyectoConstructora/obtenerTipoInmuebles")]
        [ProducesResponseType(typeof(Response<List<TipoIncmueblesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Obtener los tipo de inmuebles",
                   Description = "Obtener todos los tipo de inmuebles",
                   OperationId = "ProyectoConstructora.obtenerTipoInmuebles",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<List<TipoIncmueblesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.ObtenerTipoInmueble();
        }
    }
}
