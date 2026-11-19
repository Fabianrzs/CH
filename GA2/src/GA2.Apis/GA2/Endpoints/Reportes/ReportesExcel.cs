using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.Reportes
{
    //[Authorize]
    public class ReportesExcel : BaseAsyncEndpoint

        .WithRequest<ReportRequestExcelDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesExcelLogic _reporteExcel;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportesExcel(IReportesExcelLogic reporteExcel)
        {
            _reporteExcel = reporteExcel ?? throw new ArgumentException(nameof(reporteExcel));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportesExcel")]
        [ProducesResponseType(typeof(Response<ReportRequestExcelDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Excels Globals",
            Description = "Generar Reporte reporte Excel Globals",
            OperationId = "Reportes.ReportesExcel",
            Tags = new[] { "ReportesEndpoint" })]



        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ReportRequestExcelDto request, CancellationToken cancellationToken = default)
        {
            return await _reporteExcel.GenerarExcel(request);
        }
    }

}
