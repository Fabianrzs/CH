using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class RecalcularPlanPagoHipotecario : BaseAsyncEndpoint.WithRequest<RecalcularPlanPagoRequest>.WithResponse<Response<string>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// constructor Reversión pago
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public RecalcularPlanPagoHipotecario(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Endpoint encargado de actualizar de forma masivas los seguro y plan de pago de los creditos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/RecalcularPlanPagoHipotecario")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Recalcular Plan Pago Hipotecario",
           Description = "Recalcular Plan Pago Hipotecario",
           OperationId = "cartera.RecalcularPlanPagoHipotecario",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync([FromForm] RecalcularPlanPagoRequest request, CancellationToken cancellationToken = default)
        {
            var dto = new ActualizacionPlanPago
            {
                NumeroCredito = request.NumeroCredito,
                ValorSeguroHogar = request.ValorSeguroHogar,
                PorcentajeSeguroVida = request.PorcentajeSeguroVida

            };
            if (request.ValorSeguroHogar == 0 && request.PorcentajeSeguroVida == 0)
            {
                return await this._carteraBusinessLogic.RecalcularPlanPagoHipotecario(dto, 2, request.FechaCorte);
            }
            return await this._carteraBusinessLogic.UpdatePlanPagoHipotecario(dto, 2,request.FechaCorte);
        }
    }
}
