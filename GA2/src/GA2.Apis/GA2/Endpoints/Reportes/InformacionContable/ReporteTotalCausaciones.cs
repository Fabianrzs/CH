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
    /// ReporteTotalCausaciones
    /// </summary>
    [Authorize]
    public class ReporteTotalCausaciones : BaseAsyncEndpoint
        .WithRequest<ReporteContableParametersDto>
        .WithResponse<Response<ResumenReporteContable>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteTotalCausaciones
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteTotalCausaciones(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteTotalCausaciones
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/TotalCausaciones")]
        [ProducesResponseType(typeof(Response<ResumenReporteContable>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte total causaciones",
            Description = "Generar reporte total causaciones",
            OperationId = "Reportes.ReporteTotalCausaciones",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<ResumenReporteContable>>> HandleAsync([FromQuery] ReporteContableParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteTotalCausaciones(request);
        }
    }
}
