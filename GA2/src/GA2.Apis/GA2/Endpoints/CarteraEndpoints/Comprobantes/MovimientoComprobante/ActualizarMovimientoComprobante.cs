using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes.MovimientoComprobante
{
    public class ActualizarMovimientoComprobante : BaseAsyncEndpoint.WithRequest<MovimientosComprobanteRequest>.WithResponse<Response<List<MovimientoComprobanteDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ActualizarMovimientoComprobante(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPut("api/cartera/ActualizarMovimientoComprobanteCartera")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<MovimientoComprobanteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza Movimiento Comprobante Cartera",
            Description = "Actualiza un conjunto de Movimientos de Comprobante Cartera",
            OperationId = "cartera.ActualizarMovimientoComprobanteCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<List<MovimientoComprobanteDto>>>> HandleAsync(MovimientosComprobanteRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _carteraBusinessLogic.ActualizaMovimientoComprobanteCartera(request, cancellationToken);
            return Ok(result);
        }
    }
}
