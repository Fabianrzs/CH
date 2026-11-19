using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    //[Authorize]

    public class ObtenerAlternativosAplicacionPago : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<AlternativosAplicacionPagoDto>>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ObtenerAlternativosAplicacionPago(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ObtenerAlternativosAplicacionPago")]
        [ProducesResponseType(typeof(Response<IEnumerable<AlternativosAplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener los alternativos de aplicacion de pago",
           Description = "Obtener los alternativos de aplicacion de pago",
           OperationId = "credito.ObtenerAlternativosAplicacionPago",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<AlternativosAplicacionPagoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerAlternativosAplicacionPago();       
        }
    }
}
