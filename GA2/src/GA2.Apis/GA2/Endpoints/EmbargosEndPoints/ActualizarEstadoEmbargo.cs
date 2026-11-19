using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]
    public class ActualizarEstadoEmbargo : BaseAsyncEndpoint.WithRequest<ActualizarEstadoEmbargoDto>.WithResponse<Response<EmbargosDto>>
    {

        IEmbargosBusinessLogic _embargosBusinessLogic;

        public ActualizarEstadoEmbargo(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpPost("api/ActualizarEstadoEmbargo")]
        [ProducesResponseType(typeof(Response<EmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualizar el estado de un embargo",
           Description = "Actualizar el estado de un embargo",
           OperationId = "Embargos.ActualizarEstadoEmbargo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<EmbargosDto>>> HandleAsync(ActualizarEstadoEmbargoDto request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ActualizarEstadoEmbargo(request, Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value));
        }
    }
}