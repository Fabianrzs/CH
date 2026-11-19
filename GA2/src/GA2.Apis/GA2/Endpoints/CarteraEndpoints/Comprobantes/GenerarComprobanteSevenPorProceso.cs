using GA2.Application.Dto.Cartera.Comprobantes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;
using System.Linq;
using System;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class GenerarComprobanteSevenPorProceso: BaseAsyncEndpoint.WithRequest<GenerarComprobanteSevenPorProcesoDto>.WithResponse<Response<List<ComprobanteCarteraPorProcesoDTO>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public GenerarComprobanteSevenPorProceso(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }
        [HttpPost("api/cartera/GenerarComprobanteSevenPorProceso")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<ComprobanteCarteraPorProcesoDTO>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Comprobante Seven Cartera Por Proceso Contable",
            Description = "Generar Comprobante Seven Cartera Por Proceso Contable",
            OperationId = "cartera.GenerarComprobanteSevenPorProceso",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<List<ComprobanteCarteraPorProcesoDTO>>>> HandleAsync(GenerarComprobanteSevenPorProcesoDto request, CancellationToken cancellationToken = default)
        {
            //request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _carteraBusinessLogic.GenerarComprobanteSevenPorProceso(request, cancellationToken);
        }
    }
}
