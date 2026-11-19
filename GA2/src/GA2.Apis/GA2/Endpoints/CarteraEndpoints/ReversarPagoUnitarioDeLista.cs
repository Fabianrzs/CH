using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// Endpoint para aplicar reversión de un pago unitario desde una lista
    /// </summary>
    [Authorize]
    public class ReversarPagoUnitarioDeLista : BaseAsyncEndpoint.WithRequest<RequestReversionPagoListaDto>.WithResponse<Response<bool>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// Constructor del endpoint de reversión de pago unitario
        /// </summary>
        /// <param name="carteraBusinessLogic">Lógica de negocio de cartera</param>
        public ReversarPagoUnitarioDeLista(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Endpoint encargado de reversar un pago unitario de una lista
        /// </summary>
        /// <param name="request">Datos del pago a reversar</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta de la operación</returns>
        [HttpPost("api/cartera/ReversarPagoUnitarioDeLista")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reversar pago unitario de lista",
           Description = "Reversa un pago unitario seleccionado de una lista de pagos",
           OperationId = "cartera.ReversarPagoUnitarioDeLista",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<bool>>> HandleAsync(RequestReversionPagoListaDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _carteraBusinessLogic.ReversarPagoUnitarioDeLista(request);
        }
    }
}
