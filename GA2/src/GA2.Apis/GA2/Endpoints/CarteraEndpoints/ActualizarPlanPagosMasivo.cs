using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ActualizarPlanPagosMasivo : BaseAsyncEndpoint.WithRequest<ActualizarPlanPagoRequestDto>.WithResponse<Response<string>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// constructor Reversión pago
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ActualizarPlanPagosMasivo(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Endpoint encargado de actualizar de forma masivas los seguro y plan de pago de los creditos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/ActualizarPlanPagosMasivo")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reversar Pagos Lista",
           Description = "Reversar Pago",
           OperationId = "cartera.ReversarPagosLista",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync([FromForm] ActualizarPlanPagoRequestDto request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ActualizarPlanPagosMasivo(request);
        }
    }
}
