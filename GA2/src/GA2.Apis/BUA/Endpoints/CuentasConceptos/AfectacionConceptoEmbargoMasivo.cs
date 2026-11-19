using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Transversals.Commons;
using GA2.Application.Dto.BUA;
using iTextSharp.text;
using System.Collections;
using System.Collections.Generic;
using GA2.Application.Dto.Embargos;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class AfectacionConceptoEmbargoMasivo : BaseAsyncEndpoint
        .WithRequest<EmbargarMasivoDto>
        .WithResponse<Response<bool>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public AfectacionConceptoEmbargoMasivo(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/AfectacionCuentasConcepto")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza lo saldo cuenta seleccionada",
           Description = "Actualiza lo saldo cuenta seleccionada.",
           OperationId = "CuentasConceptos.ActualizarSaldoConceptoCuenta",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(EmbargarMasivoDto request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.AfectacionConceptoMasivoEmbargo(request);
        }
    }
}
