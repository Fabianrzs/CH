using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// Generar reporte sin tener en cuenta la referencia cultural
    /// </summary>
    [Authorize]
    public class ReportesExcelInvariantCulture : BaseAsyncEndpoint
        .WithRequest<ReportRequestExcelFormatDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesExcelInvariantCultureLogic _reporteExcel;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reporteExcel"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportesExcelInvariantCulture(IReportesExcelInvariantCultureLogic reporteExcel)
        {
            _reporteExcel = reporteExcel ?? throw new ArgumentException(null, nameof(reporteExcel));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportesExcelInvariantCulture")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Excels Globals",
            Description = "Generar Reporte reporte Excel Globals",
            OperationId = "Reportes.ReportesExcel",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ReportRequestExcelFormatDto request, CancellationToken cancellationToken = default)
        {
            return await _reporteExcel.GenerarExcel(request);
        }
    }

}
