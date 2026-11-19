using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ProductoPorCliente;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class CrearPlanesPagoProdutoCliente : BaseAsyncEndpoint.WithRequest<InsertProductoClienteDto>.WithResponse<Response<List<InsertPlanPagosDto>>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public CrearPlanesPagoProdutoCliente(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        [HttpPost("api/cartera/CrearProductoClientePlanPago")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<InsertPlanPagosDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crea Plan Pagos Productos Por Cliente",
            Description = "Crea Plan Pagos Productos Por Cliente",
            OperationId = "cartera.CrearProductoClientePlanPago",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<List<InsertPlanPagosDto>>>> HandleAsync([FromForm] InsertProductoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.CrearProductoClientePlanPago(request, cancellationToken);
        }
    }
}
