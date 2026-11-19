using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core;
using GA2.Application.Dto.Cartera.ProductoPorCliente;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class CrearProductoCliente : BaseAsyncEndpoint.WithRequest<InsertProductoClienteDto>.WithResponse<Response<object>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public CrearProductoCliente(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/CrearProductoCliente")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea Productos Por Cliente",
            Description = "Crea Productos Por Cliente",
            OperationId = "cartera.ObtenerProductoCliente",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<object>>> HandleAsync([FromForm] InsertProductoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.CrearProdutoClienteCartera(request, cancellationToken);
        }
    }
}
