using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteMovimientoDeCartera
    /// </summary>
    [Authorize]
    public class MovimientoDeCartera : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteMovimientoDeCartera
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public MovimientoDeCartera(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteMovimientoDeCartera
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteMovimientoDeCartera")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Movimiento De Cartera",
            Description = "Generar Reporte Movimiento De Cartera",
            OperationId = "Reportes.ReporteMovimientoDeCartera",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(string identificacion, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.MovimientoDeCartera(identificacion);
        }
    }
}
