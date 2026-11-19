using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class ReversarComprobante : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<CrearComprobanteReversionDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ReversarComprobante(ICarteraBusinessLogic carteraBusinessLogic)
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
        [HttpPost("api/cartera/ReversarComprobante")]
        [ProducesResponseType(typeof(Response<CrearComprobanteReversionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reversar Comprobante Cartera",
           Description = "Reversar Comprobante Cartera",
           OperationId = "cartera.ReversarComprobante",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<CrearComprobanteReversionDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CrearComprobanteReversionYEnviarASevenAsync(request);
        }
    }
}
