using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ConstruirMovimientosContables: BaseAsyncEndpoint.WithRequest<RequestCausacionDiariaInteresesCorrientesDto>.WithResponse<Response<List<CausacionDiariaInteresesCorrientesDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public ConstruirMovimientosContables(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }
        [HttpPost("api/cartera/ConstruirMovimientosContables")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<CausacionDiariaInteresesCorrientesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Construir movimientos contables",
           Description = "Construir movimientos contables de causacion diaria",
           OperationId = "cartera.construirMovimientosContables",
           Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<List<CausacionDiariaInteresesCorrientesDto>>>> HandleAsync(RequestCausacionDiariaInteresesCorrientesDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ConstruirMovimientosContables(request, cancellationToken);
        }

    }
}
