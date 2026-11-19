using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    /// <summary>
    /// EndPoint para actualizar saldos cta y conceptos cta por pago en seven en Crédito y Césantias.
    /// </summary>
    [Authorize]
    public class ActualizarCuentaECT : BaseAsyncEndpoint
        .WithRequest<ActualizarEctCuentasDto>
        .WithResponse<Response<DatosVerificacionDerechoCuentaDto>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public ActualizarCuentaECT(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("api/cuentas/ActualizarEctCuentas")]
        [ProducesResponseType(typeof(Response<DatosVerificacionDerechoCuentaDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los movientos obtenidos del proceso de pagos en seven de césantias y crédito",
           Description = "Actualiza el ECT en cada cuenta.",
           OperationId = "CuentasConceptos.ActualizarEctCuentas",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        public async override Task<ActionResult<Response<DatosVerificacionDerechoCuentaDto>>> HandleAsync(ActualizarEctCuentasDto request, CancellationToken cancellationToken = default)
        {
                        
            return await _cuentasConceptosBusinessLogic.ActualizarECTCuentas(request);
        }
    }
}
