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

    public class ValidarAplicacionPago : BaseAsyncEndpoint.WithRequest<ObtenerProductoCartera>.WithResponse<Response<ValidarAplicacionPagoDto>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ValidarAplicacionPago(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ValidarAplicacionPago")]
        [ProducesResponseType(typeof(Response<IEnumerable<ValidarAplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Valida la aplicacion de pago",
           Description = "Valida la aplicacion de pago",
           OperationId = "credito.ValidarAplicacionPago",
           Tags = new[] { "CarteraEndpoint" })]
        public  override async Task<ActionResult<Response<ValidarAplicacionPagoDto>>> HandleAsync(ObtenerProductoCartera request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ValidarAplicacionPago(request); 
        }
    }
}
