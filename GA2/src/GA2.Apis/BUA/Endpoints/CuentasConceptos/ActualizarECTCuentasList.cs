using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ActualizarECTCuentasList : BaseAsyncEndpoint
        .WithRequest<List<ActualizarEctCuentasDto>>
        .WithResponse<Response<List<DatosVerificacionDerechoCuentaDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public ActualizarECTCuentasList(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/ActualizarECTCuentasList")]
        [ProducesResponseType(typeof(Response<DatosVerificacionDerechoCuentaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los movientos obtenidos del proceso de pagos en seven de césantias y crédito",
           Description = "Actualiza el ECT en cada cuenta.",
           OperationId = "CuentasConceptos.ActualizarEctCuentas",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<List<DatosVerificacionDerechoCuentaDto>>>> HandleAsync(List<ActualizarEctCuentasDto> request, CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ActualizarECTCuentasList(request);
        }
    }
}
