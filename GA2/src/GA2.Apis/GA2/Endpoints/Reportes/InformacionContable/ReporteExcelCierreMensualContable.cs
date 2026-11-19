using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes.InformacionContable
{
    /// <summary>
    /// ReporteExcelCierreMensualContable
    /// </summary>
    [Authorize]
    public class ReporteExcelCierreMensualContable : BaseAsyncEndpoint
        .WithRequest<ReporteContableCierreMensualParametersDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExcelCierreMensualContable
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteExcelCierreMensualContable(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteExcelCierreMensualContable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/GenerarExcelCierreMensualContable")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte cierre mensual contable",
            Description = "Generar reporte cierre mensual contable",
            OperationId = "Reportes.ReporteExcelCierreMensualContable",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync([FromQuery] ReporteContableCierreMensualParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarExcelReporteCierreMensualContable(request);
        }
    }
}
