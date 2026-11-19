using Ardalis.ApiEndpoints;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ProcesarSaldoConceptoCuenta : BaseAsyncEndpoint
        .WithRequest<List<ConceptoUpdate>>
        .WithResponse<Response<string>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ProcesarSaldoConceptoCuenta(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/ProcesarSaldoConceptoCuenta")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza lo saldo cuenta seleccionada",
           Description = "Actualiza lo saldo cuenta seleccionada.",
           OperationId = "CuentasConceptos.ProcesarSaldoConceptoCuenta",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(List<ConceptoUpdate> request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ProcesarSaldoConceptoCuenta(request);
        }
    }
}
