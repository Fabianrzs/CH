using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    [Authorize]

    public class InsertarInclusionSaldos : BaseAsyncEndpoint.WithRequest<InclusionSaldoRequestDto>.WithResponse<Response<InclusionSaldoRequestDto>>
    {

        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        public InsertarInclusionSaldos(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientsBusinessLogic = clientesBusinessLogic;
        }

        [HttpPost("api/InsertarInclusionSaldos")]
        [ProducesResponseType(typeof(Response<EmbargosDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta un bloquo de una cuenta o un concecto",
           Description = "Inserta un bloquo de una cuenta o un concecto",
           OperationId = "CuentasConceptos.InsertarBoqueoCuentaConcecto",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public override async Task<ActionResult<Response<InclusionSaldoRequestDto>>> HandleAsync(InclusionSaldoRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._clientsBusinessLogic.InsertarInclusionSaldos(request);
        }
    }
}
