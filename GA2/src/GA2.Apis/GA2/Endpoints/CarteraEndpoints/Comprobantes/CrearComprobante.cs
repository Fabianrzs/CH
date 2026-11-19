using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class CrearComprobante : BaseAsyncEndpoint.WithRequest<ComprobanteCarteraDto>.WithResponse<Response<ComprobanteCarteraDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public CrearComprobante(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/CrearComprobanteCartera")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<ComprobanteCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea Comprobante Cartera",
            Description = "Crea Comprobante Cartera",
            OperationId = "cartera.CrearComprobanteCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<ComprobanteCarteraDto>>> HandleAsync(ComprobanteCarteraDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.CrearComprobanteCartera(request, cancellationToken);
        }
    }
}
