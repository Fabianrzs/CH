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
    /// Generar reporte excel
    /// </summary>
    [Authorize]
    public class ReportesExcelEndpoint : BaseAsyncEndpoint
        .WithRequest<ExcelReportDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesExcelInvariantCultureLogic _reporteExcel;

        /// <summary>
        /// ReportesExcelEndpoint
        /// </summary>
        /// <param name="reporteExcel"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportesExcelEndpoint(IReportesExcelInvariantCultureLogic reporteExcel)
        {
            _reporteExcel = reporteExcel ?? throw new ArgumentException(null, nameof(reporteExcel));
        }


        /// <summary>
        /// ReportesExcelEndpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/GenerarExcel")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte excel",
            Description = "Generar reporte excel",
            OperationId = "Reportes.GenerarReporteExcel",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ExcelReportDto request, CancellationToken cancellationToken = default)
        {
            return await _reporteExcel.GenerarExcel(request);
        }
    }

}
