using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Core.Proyecto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class CrearPolizaProyecto : BaseAsyncEndpoint.WithRequest<List<PolizaProyectoDto>>
        .WithResponse<Response<string>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public CrearPolizaProyecto(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpPost("api/ProyectoConstructora/CrearPoliza")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Crear poliza  de proyecto  constructora",
                   Description = "Crear poliza  de proyecto  constructora",
                   OperationId = "ProyectoConstructora.CrearPoliza",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(List<PolizaProyectoDto> request, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.CrearPolizas(request);
        }
    }
}
