using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class ObtenerComprobante : BaseAsyncEndpoint.WithRequest<ObtenerComprobanteDto>.WithResponse<Response<ComprobanteCarteraDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public ObtenerComprobante(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/ObtenerComprobanteCartera")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<ComprobanteCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Comprobante Cartera",
            Description = "Obtener Comprobante Cartera",
            OperationId = "cartera.ObtenerComprobanteCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<ComprobanteCarteraDto>>> HandleAsync(ObtenerComprobanteDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerComprobanteCartera(request, cancellationToken);
        }
    }
}
