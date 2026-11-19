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

    public class ObtenerConceptosEmbargoById : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<DetalleEmbargosDto>>
    {

        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerConceptosEmbargoById(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpGet("api/ObtenerConceptosEmbargoById")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<DetalleEmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene los detalles de embargo por su id",
           Description = "Obtiene los detalles de embargo por su id",
           OperationId = "Embargos.ObtenerConceptosEmbargoById",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<DetalleEmbargosDto>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {

            return await this._embargosBusinessLogic.ObtenerConceptosEmbargoById(request);
            
        }
    }
}
