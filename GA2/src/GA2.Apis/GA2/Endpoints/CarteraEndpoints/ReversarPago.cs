using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// Endpoint para aplicar reversión de un pago
    /// </summary>
    [Authorize]
    public class ReversarPago : BaseAsyncEndpoint.WithRequest<RequestReversionPagoDto>.WithResponse<Response<AplicacionPagoDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// constructor Reversión pago
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ReversarPago(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Endpoint encargado de reversar un pago
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/reversarPago")]
        [ProducesResponseType(typeof(Response<AplicacionPagoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reversar Pago",
           Description = "Reversar Pago",
           OperationId = "credito.ReversarPago",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<AplicacionPagoDto>>> HandleAsync(RequestReversionPagoDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await this._carteraBusinessLogic.ReversarPago(request);
        }
    }
}
