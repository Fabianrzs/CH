using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    /// <summary>
    /// Endpoint para validar y gestionar OTP temporal (intentos, bloqueo y expiración 24h)
    /// </summary>
    /// <author>Alejandro Lara Miranda</author>
    /// <date>12/11/2025</date>
    [Authorize]
    public class ValidacionOTPTemporalTU :
         BaseAsyncEndpoint.WithRequest<ValidacionOTPTemporalDto>
        .WithResponse<Response<ValidacionOTPTemporal>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del endpoint de Validación OTP Temporal
        /// </summary>
        /// <param name="creditoBusinessLogic">Inyección de la lógica de negocio de crédito</param>
        public ValidacionOTPTemporalTU(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Endpoint para validar un código OTP temporal.
        /// - Inserta nuevo registro si no existe.
        /// - Actualiza intentos si ya existe.
        /// - Elimina el registro si han pasado más de 24 horas.
        /// - Bloquea el acceso después de 3 intentos fallidos.
        /// </summary>
        /// <param name="request">Datos del OTP temporal (Celular, Identificación)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna el estado actual del OTP (activo o bloqueado)</returns>
        [HttpPost("api/credito/validacionotptemporal")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ValidacionOTPTemporal>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validar y gestionar OTP temporal",
            Description = "Controla intentos, bloqueo y expiración de 24h para la validación de OTP.",
            OperationId = "Credito.ValidacionOTPTemporal",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ValidacionOTPTemporal>>> HandleAsync(
            ValidacionOTPTemporalDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ValidacionOTPTemporalTU(request);
        }
    }
}
