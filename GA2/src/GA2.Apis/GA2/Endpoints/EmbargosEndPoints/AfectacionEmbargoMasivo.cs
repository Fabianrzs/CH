using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
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
    public class AfectacionEmbargoMasivo : BaseAsyncEndpoint.WithRequest<EmbargarMasivoDto>.WithResponse<Response<bool>>
    {

        IEmbargosBusinessLogic _embargosBusinessLogic;

        public AfectacionEmbargoMasivo(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }

        [HttpPost("api/EmbargoMasivo")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "actualiza un embargo masivo",
           Description = "actualiza un embargo",
           OperationId = "Embargos.EmbargoMasivo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(EmbargarMasivoDto request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.AfectacionEmbargoMasivo(request);
        }
    }
}

