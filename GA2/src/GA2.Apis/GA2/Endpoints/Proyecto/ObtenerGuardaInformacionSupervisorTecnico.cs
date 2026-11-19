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

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class ObtenerGuardaInformacionSupervisorTecnico : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<List<SupervicionTecnicaDto>>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public ObtenerGuardaInformacionSupervisorTecnico(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpGet("api/ProyectoConstructora/ObtenerGuardaInformacionSupervisorTecnico")]
        [ProducesResponseType(typeof(Response<List<SupervicionTecnicaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Obtener informacion de supervisor tecnico",
                   Description = "Obtener informacion de supervisor tecnico",
                   OperationId = "ProyectoConstructora.ObtenerGuardaInformacionSupervisorTecnico",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<List<SupervicionTecnicaDto>>>> HandleAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.ObtenerGuardaInformacionSupervisorTecnico(Id);
        }
    }
}
