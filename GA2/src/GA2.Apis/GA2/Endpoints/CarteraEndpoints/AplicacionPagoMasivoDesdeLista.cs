using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AplicarPagoMasivoDesdeLista : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<IEnumerable<ResultadoAplicacionPagoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AplicarPagoMasivoDesdeLista(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic ?? throw new ArgumentException(nameof(carteraBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProceso"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/AplicarPagoMasivoDesdeLista")]
        [ProducesResponseType(typeof(Response<IEnumerable<AplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aplicar pago masivo desde lista",
            Description = "Aplicar pago masivo desde lista",
            OperationId = "cartera.AplicarPagoMasivoDesdeLista",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ResultadoAplicacionPagoDto>>>> HandleAsync(int idProceso, CancellationToken cancellationToken = default)
        {
            var userId = User.Claims.Where(c => c.Type.Equals("USR_ID")).FirstOrDefault().Value;
            return await _carteraBusinessLogic.AplicarPagoMasivoDesdeLista(idProceso, Guid.Parse(userId), cancellationToken);
        }
    }
}
