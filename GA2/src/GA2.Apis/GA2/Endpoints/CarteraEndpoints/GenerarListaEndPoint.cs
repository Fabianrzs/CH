using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{

    /// <summary>
    /// GenerarListaEndPoint
    /// </summary>
    [Authorize]
    public class GenerarListaEndPoint : BaseAsyncEndpoint
         .WithRequest<GenerarListaParametersDto>
         .WithResponse<Response<IEnumerable<PagoListaDetalleDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// Constructor GenerarListaEndPoint
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public GenerarListaEndPoint(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// GenerarLista
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cartera/GenerarLista")]
        [ProducesResponseType(typeof(Response<IEnumerable<PagoListaDetalleDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Lista aplicacion pago masivo",
            Description = "Generar Lista aplicacion pago masivo",
            OperationId = "cartera.GenerarLista",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<PagoListaDetalleDto>>>> HandleAsync(GenerarListaParametersDto request, CancellationToken cancellationToken = default)
        {
           return await _carteraBusinessLogic.GenerarLista(request);
        }
    }
}
