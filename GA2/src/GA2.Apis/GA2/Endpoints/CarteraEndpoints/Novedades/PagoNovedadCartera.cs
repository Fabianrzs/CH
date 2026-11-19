using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Novedades
{
    /// <summary>
    /// </summary>
    [Authorize]
    public class PagoNovedadCartera :
        BaseAsyncEndpoint.WithRequest<RequestPagoNovedadDto>.WithResponse<Response<AplicacionPagoDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public PagoNovedadCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Novedades/AplicarPagoNovedadCartera")]
        [ProducesResponseType(typeof(Response<AplicacionPagoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Realizar pago a una novedad de cartera",
            Description = "Realizar pago a una novedad de cartera",
            OperationId = "credito.AplicarPagoNovedadCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<AplicacionPagoDto>>> HandleAsync(RequestPagoNovedadDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _carteraBusinessLogic.AplicarPagoNovedadCartera(request);
        }
    }
}
