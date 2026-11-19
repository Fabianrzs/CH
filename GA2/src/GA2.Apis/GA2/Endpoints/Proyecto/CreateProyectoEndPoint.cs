using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Core.Proyecto;
using System;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class CreateProyectoEndPoint : BaseAsyncEndpoint.WithRequest<ProyectoConstructoraDto>
        .WithResponse<Response<Guid?>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public CreateProyectoEndPoint(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpPost("api/ProyectoConstructora/CrearProyecto")]
        [ProducesResponseType(typeof(Response<Guid?>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Crear proyecto de constructora",
                   Description = "Crear proyecto de constructora",
                   OperationId = "ProyectoConstructora.CrearProyecto",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<Guid?>>> HandleAsync(ProyectoConstructoraDto request, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.CrearProyecto(request);
        }
    }
}
