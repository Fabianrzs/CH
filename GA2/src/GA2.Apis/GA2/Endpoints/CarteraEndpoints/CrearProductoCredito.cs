using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    public class CrearProductoCredito : BaseAsyncEndpoint.WithRequest<ProductoClienteDto>.WithResponse<Response<ProductoClienteDto>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public CrearProductoCredito(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/CrearProductoCredito")]
        [ProducesResponseType(typeof(Response<ProductoClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Crear Producto Credito",
           Description = "Crear Producto Credito",
           OperationId = "credito.CrearProductoCredito",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<ProductoClienteDto>>> HandleAsync(ProductoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CrearProductoCredito(request);       
        }
    }
}
