using Ardalis.ApiEndpoints;
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
    /// Endpoint para consultar la validación OTP temporal del portal
    /// </summary>
    /// <author>Alejandro Lara Miranda</author>
    /// <date>12/11/2025</date>
    public class ConsultaValidacionOTPTemporalPortal :
         BaseAsyncEndpoint.WithRequest<ValidacionOTPTemporalDto>
        .WithResponse<Response<ValidacionOTPTemporal>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del endpoint de consulta OTP temporal
        /// </summary>
        /// <param name="creditoBusinessLogic">Inyección de lógica de negocio</param>
        public ConsultaValidacionOTPTemporalPortal(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Consulta el estado actual de la validación del OTP temporal.
        /// No modifica nada, solo retorna la información del registro encontrado.
        /// </summary>
        /// <param name="request">Datos para consultar (celular, identificación)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Información del OTP temporal registrado</returns>
        [HttpPost("api/credito/consultavalidacionotptemporalportal")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ValidacionOTPTemporal>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar validación OTP temporal del portal",
            Description = "Obtiene el registro actual de la validación OTP temporal sin modificarlo.",
            OperationId = "Credito.ConsultaValidacionOTPTemporalPortal",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ValidacionOTPTemporal>>> HandleAsync(
            ValidacionOTPTemporalDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarValidacionOTPTemporalTU(request);
        }
    }
}
