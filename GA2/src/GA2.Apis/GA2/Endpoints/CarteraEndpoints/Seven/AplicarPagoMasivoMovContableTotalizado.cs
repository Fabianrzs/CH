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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Seven
{
    public class AplicarPagoMasivoMovContableTotalizado : BaseAsyncEndpoint
        .WithRequest<DateTime>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public AplicarPagoMasivoMovContableTotalizado(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic ?? throw new ArgumentException(nameof(carteraBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProceso"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/AplicarPagoMasivoMovContableTotalizado")]
        [ProducesResponseType(typeof(Response<IEnumerable<AplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aplicacion de pagos totalizados",
            Description = "Aplicacion de pagos totalizados",
            OperationId = "cartera.AplicarPagoMasivoMovContableTotalizado",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(DateTime Proceso, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.AplicarPagoMasivoMovContableTotalizado(Proceso, cancellationToken);
        }
    }
}
