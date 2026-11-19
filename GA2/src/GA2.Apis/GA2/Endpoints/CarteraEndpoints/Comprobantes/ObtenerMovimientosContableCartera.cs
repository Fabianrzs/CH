using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using System;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class ObtenerMovimientosContableCartera : BaseAsyncEndpoint.WithRequest<MovimientosContableParametersDto>.WithResponse<Response<List<MovimientoComprobantePorProcesoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public ObtenerMovimientosContableCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }
        [HttpPost("api/cartera/ObtenerMovimientosContableCartera")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<MovimientoComprobantePorProcesoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Detalle Comprobante Cartera Por ID",
            Description = "Obtener Detalle Comprobante Cartera Por ID",
            OperationId = "cartera.ObtenerMovimientosContableCartera",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<List<MovimientoComprobantePorProcesoDto>>>> HandleAsync(MovimientosContableParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerMovimientosContableCartera(request, cancellationToken);
        }
    }
}