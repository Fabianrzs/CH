using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerPlanPago : BaseAsyncEndpoint.WithRequest<ObtenerProductoCartera>.WithResponse<Response<ResponsePlanPagoDto>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ObtenerPlanPago(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ObtenerPlanPago")]
        [ProducesResponseType(typeof(Response<ResponsePlanPagoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene el plan de pago de un credito por el numero",
           Description = "Obtiene el plan de pago de un credito por el numero",
           OperationId = "credito.ObtenerPlanPago",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<ResponsePlanPagoDto>>> HandleAsync(ObtenerProductoCartera obtenerProductoCartera, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerPlanPagosCredito(obtenerProductoCartera);
           
        }
    }
}
