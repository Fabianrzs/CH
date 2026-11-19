using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Proyecto;
using GA2.Domain.Core.Proyecto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Proyecto
{
    public class CrearInformacionSupervicionTecnica : BaseAsyncEndpoint.WithRequest<List<SupervicionTecnicaDto>>
        .WithResponse<Response<string>>
    {
        private readonly IProyectoBusiness _proyectoBusiness;

        public CrearInformacionSupervicionTecnica(IProyectoBusiness proyectoBusiness)
        {
            _proyectoBusiness = proyectoBusiness;
        }

        [HttpPost("api/ProyectoConstructora/GuardarInformacionSupervicionTecnica")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
                   Summary = "Crear informacion de supervcion tecnica",
                   Description = "Crear o guardar informacion de supervcion tecnica",
                   OperationId = "ProyectoConstructora.GuardarInformacionSupervicionTecnica",
                   Tags = new[] { "ProyectoConstructora" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(List<SupervicionTecnicaDto> request, CancellationToken cancellationToken = default)
        {
            return await _proyectoBusiness.GuardaInformacionSupervisorTecnico(request);
        }
    }
}
