using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes.MovimientoComprobante
{
    public class CrearMovimientoComprobante : BaseAsyncEndpoint.WithRequest<MovimientoComprobanteDto>.WithResponse<Response<MovimientoComprobanteDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public CrearMovimientoComprobante(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/CrearMovimientoComprobanteCartera")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<MovimientoComprobanteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea Movimiento Comprobante Cartera",
            Description = "Crea Movimiento Comprobante Cartera",
            OperationId = "cartera.CrearMovimientoComprobanteCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<MovimientoComprobanteDto>>> HandleAsync(MovimientoComprobanteDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.CrearMovimientoComprobanteCartera(request, cancellationToken);
        }
    }
}
