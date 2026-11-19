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
    public class ActualizarAbonoInteresMovimientos
        : BaseAsyncEndpoint.WithRequest<ParametrosAbonosInteresDto>
        .WithResponse<Response<ParametrosAbonosInteresDto>>
    {
        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;
        /// <summary>
        /// CTR.
        /// </summary>
        /// <param name="clientesBusinessLogic"></param>
        public ActualizarAbonoInteresMovimientos(IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic)
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
        [HttpPost("api/VerificacionDerechos/ActualizarAbonoInteresMovimientos")]
        [ProducesResponseType(typeof(Response<ParametrosAbonosInteresDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Actualiza los parametors del abono de interes e inserta los movientos obtenidos del proceso de pagos de intereses",
           Description = "Actualiza tabla movimientos cta-conceptos, saldo en cuenta.",
           OperationId = "VerificacionDerechos.ActualizarAbonoInteresMovimientos",
           Tags = new[] { "VerificacionDerechosEndPoints" })]
        public async override Task<ActionResult<Response<ParametrosAbonosInteresDto>>> HandleAsync(ParametrosAbonosInteresDto request, CancellationToken cancellationToken = default)
        {
            return await _verificacionDerechosBusinessLogic.ActualizarParametrosInteresAbonadosMovimientos(request);
        }
    }
}
