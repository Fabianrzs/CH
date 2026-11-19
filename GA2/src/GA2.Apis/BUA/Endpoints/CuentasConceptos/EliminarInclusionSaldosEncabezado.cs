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

    public class EliminarInclusionSaldosEncabezado : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<InclusionSaldoRequestDto>>
    {

        private readonly IClientesBusinessLogic _clientesBusinessLogic;

        public EliminarInclusionSaldosEncabezado(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientesBusinessLogic = clientesBusinessLogic;
        }

        [HttpDelete("api/EliminarInclusionSaldosEncabezado")]
        [ProducesResponseType(typeof(Response<InclusionSaldoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta un bloquo de una cuenta o un concecto",
           Description = "Inserta un bloquo de una cuenta o un concecto",
           OperationId = "CuentasConceptos.InsertarBoqueoCuentaConcecto",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public override async Task<ActionResult<Response<InclusionSaldoRequestDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._clientesBusinessLogic.EliminarInclusionSaldosEncabezado(request);
        }
    }
}
