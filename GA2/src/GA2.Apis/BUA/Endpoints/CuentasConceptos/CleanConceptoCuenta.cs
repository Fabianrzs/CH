using Ardalis.ApiEndpoints;
using GA2.Application.Dto.BUA;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class CleanConceptoCuenta : BaseAsyncEndpoint
        .WithRequest<List<ConceptoCuentaCleanDto>>
        .WithResponse<Response<string>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public CleanConceptoCuenta(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/ClealConceptoCuenta")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "limpia los concepto de cuenta seleccionada",
           Description = "limpia los concepto de cuenta seleccionada.",
           OperationId = "CuentasConceptos.ClealConceptoCuenta",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(List<ConceptoCuentaCleanDto> request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ClealConceptoCuenta(request);
        }
    }
}
