using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// Generar reporte de altas y bajas por unidad ejecutora
    /// </summary>
    public class GenerarReporteAltasBajas : BaseAsyncEndpoint
        .WithRequest<RequestReporteAltasBajasDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public GenerarReporteAltasBajas(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Descargar Reporte Altas y Bajas
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/cartera/descargarReporteAltasBajas")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Descargar reporte csv altas o bajas",
            Description = "Descargar reporte altas o bajas",
            OperationId = "cartera.DescargarReporteAltasBajas",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync([FromQuery] RequestReporteAltasBajasDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.GenerarReporteAltasBajasAsync(request);
        }
    }
}
