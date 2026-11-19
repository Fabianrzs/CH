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

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class UpdateSaldoConcepto : BaseAsyncEndpoint
        .WithRequest<List<CuentaConceptoDto>>
        .WithResponse<Response<IEnumerable<ConceptoCuentaDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public UpdateSaldoConcepto(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/ActualizarSaldoConceptoCuenta")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza lo saldo cuenta seleccionada",
           Description = "Actualiza lo saldo cuenta seleccionada.",
           OperationId = "CuentasConceptos.ActualizarSaldoConceptoCuenta",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<IEnumerable<ConceptoCuentaDto>>>> HandleAsync(List<CuentaConceptoDto> request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ActualizarSaldoConceptoCuenta(request);    
        }
    }
}
