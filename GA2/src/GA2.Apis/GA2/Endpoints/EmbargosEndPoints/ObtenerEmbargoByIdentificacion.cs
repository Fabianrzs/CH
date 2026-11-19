using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerEmbargoByIdentificacion : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<EmbargoByDto>>>
    {
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerEmbargoByIdentificacion(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpGet("api/ObtenerEmbargoByIdentificacion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<EmbargoByDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene un embargo por Id cliente",
           Description = "Obtiene un embargo por Id cliente",
           OperationId = "Embargos.ObtenerEmbargoBy",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<EmbargoByDto>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerEmbargoByIdentificacion(request);
        }
    }
}
