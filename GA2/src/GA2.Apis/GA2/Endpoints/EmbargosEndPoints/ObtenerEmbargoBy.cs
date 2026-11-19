using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerEmbargoBy : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<EmbargoByDto>>
    {
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerEmbargoBy(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpGet("api/ObtenerEmbargoById")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EmbargoByDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene un embargo por su Id",
           Description = "Obtiene un embargo por su Id",
           OperationId = "Embargos.ObtenerEmbargoBy",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<EmbargoByDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerEmbargoBy(request);
        }
    }
}
