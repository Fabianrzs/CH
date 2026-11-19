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

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Comprobantes
{
    public class ObtenerComprobantePorCodigoProceso: BaseAsyncEndpoint.WithRequest<ObtenerComprobantePorProcesoDto>.WithResponse<Response<List<ComprobanteCarteraPorProcesoDTO>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        public ObtenerComprobantePorCodigoProceso(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }
        [HttpPost("api/cartera/ObtenerComprobantePorCodigoProceso")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(typeof(Response<List<ComprobanteCarteraPorProcesoDTO>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Comprobante Cartera Por Proceso Contable",
            Description = "Obtener Comprobante Cartera Por Proceso Contable",
            OperationId = "cartera.ObtenerComprobantePorCodigoProceso",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<List<ComprobanteCarteraPorProcesoDTO>>>> HandleAsync(ObtenerComprobantePorProcesoDto request, CancellationToken cancellationToken = default)
        {           
            return await _carteraBusinessLogic.ObtenerComprobanteCarteraPorProceso(request, cancellationToken);
        }
    }
}
