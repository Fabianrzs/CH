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

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerPagoEmbargo : BaseAsyncEndpoint.WithRequest<DateTime>.WithResponse<Response<PagoEmbargoDto>>
    {
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerPagoEmbargo(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpGet("api/ObtenerPagoEmbargo")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<PagoEmbargoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene los los pagos de embargo por fecha",
           Description = "Obtiene los pagos de embargo por fecha",
           OperationId = "Embargos.ObtenerPagoEmbargo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<PagoEmbargoDto>>> HandleAsync(DateTime request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerPagoEmbargo(request);
        }
    }
}
