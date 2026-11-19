using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class GenerarComprobanteCausaciones : BaseAsyncEndpoint
        .WithRequest<MovimientoReportesCartera>
        .WithResponse<Response<object>>
    {

        private readonly ICarteraBusinessLogic _carterabusinesslogic; 
        public GenerarComprobanteCausaciones(ICarteraBusinessLogic carterabusinesslogic)
        {
            _carterabusinesslogic = carterabusinesslogic;
        }


        [HttpPost("api/cartera/GenerarComprobanteCausaciones")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<MovimientoComprobanteEntity>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar comprobante causaciones",
            Description = "Generar comprobante causaciones",
            OperationId = "cartera.GenerarComprobanteCausaciones",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<object>>> HandleAsync(MovimientoReportesCartera request, CancellationToken cancellationToken = default)
        {
            return await _carterabusinesslogic.GenerarCausacionesComprobante(request); 
        }
    }
}
