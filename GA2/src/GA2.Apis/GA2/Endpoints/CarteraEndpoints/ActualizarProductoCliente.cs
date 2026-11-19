using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ActualizarProductoCliente : BaseAsyncEndpoint.WithRequest<RequestActualizacionProducto>.WithResponse<Response<object>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public ActualizarProductoCliente(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/ActualizarProductoCliente")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza Productos Por Cliente",
            Description = "Actualiza Productos Por Cliente",
            OperationId = "cartera.ActualizarProductoCliente",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<object>>> HandleAsync([FromForm] RequestActualizacionProducto request, CancellationToken cancellationToken = default)
        {
            InsertProductoClienteDto insertProductoClienteDto = new();
            return await _carteraBusinessLogic.CrearProdutoClienteCartera(insertProductoClienteDto, cancellationToken);
        }
    }
}
