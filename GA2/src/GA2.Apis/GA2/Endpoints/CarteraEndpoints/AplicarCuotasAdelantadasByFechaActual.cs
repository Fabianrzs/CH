

using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AplicarCuotasAdelantadasByFechaActual : BaseAsyncEndpoint.WithRequest<DateTime>.WithResponse<Response<IEnumerable<responseAdelantadaFechaCierreDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AplicarCuotasAdelantadasByFechaActual(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FechaAplicacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/AplicarCuotasAdelantadasByFechaActual")]
        [ProducesResponseType(typeof(Response<IEnumerable<responseAdelantadaFechaCierreDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Aplicar Cuotas Adelantadas por FechaActual",
           Description = "Aplicar Cuotas Adelantadas por FechaActual",
           OperationId = "cartera.AplicarCuotasAdelantadasByFechaActual",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<responseAdelantadaFechaCierreDto>>>> HandleAsync(DateTime FechaAplicacion, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.AplicarCuotasAdelantadasByFechaActual(FechaAplicacion, cancellationToken);
        }
    }

}
