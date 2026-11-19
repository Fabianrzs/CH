using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// EndPoint para actualizar movimientos causados por pago de intereses.
    /// </summary>
    [Authorize]
    public class InsertarMovimientosAbonoIntereses
        : BaseAsyncEndpoint.WithRequest<VerificacionDerechosPeriodo>
        .WithResponse<Response<ParametrosAbonosInteresDto>>
    {
        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public InsertarMovimientosAbonoIntereses(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
        {
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// Update client info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        //[ValidateAntiForgeryToken]
        [HttpPost("api/VerificacionDerechos/InsertarMovimientoAbonosIntereses")]
        [ProducesResponseType(typeof(Response<VerificacionDerechosPeriodo>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los movientos obtenidos del proceso de pagos de intereses",
           Description = "Actualiza tabla movimientos cta-conceptos, saldo en cuenta.",
           OperationId = "VerificacionDerechos.InsertarMovimientoAbonosIntereses",
           Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<ParametrosAbonosInteresDto>>> HandleAsync(VerificacionDerechosPeriodo request, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.AplicarMovimientosInteresesAbonados(request);
        }
    }
}
