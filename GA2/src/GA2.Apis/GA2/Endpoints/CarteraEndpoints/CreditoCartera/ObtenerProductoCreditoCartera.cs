using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.CreditoCarteraProducto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.CreditoCartera
{
    [Authorize]
    public class ObtenerProductoCreditoCartera : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<CreditoCarteraProducto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerProductoCreditoCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cartera/ObtenerProductoCreditoCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<CreditoCarteraProducto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar producto de la integracion entre credito y cartera",
            Description = "Consultar producto de la integracion entre credito y cartera",
            OperationId = "credito.ObtenerProductoCreditoCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public override async Task<ActionResult<Response<CreditoCarteraProducto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerProductoCreditoCartera(request); 
        }
    }
}
