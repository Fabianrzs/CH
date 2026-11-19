using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.Reportes;
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
    /// ReporteTotalCierreMensualContable
    /// </summary>
    [Authorize]
    public class ReporteTotalCierreMensualContable : BaseAsyncEndpoint
        .WithRequest<ReporteContableCierreMensualParametersDto>
        .WithResponse<Response<IEnumerable<TotalReporteCierreMensualContableDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteTotalCierreMensualContable
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteTotalCierreMensualContable(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteTotalCierreMensualContable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/TotalCierreMensualContable")]
        [ProducesResponseType(typeof(Response<IEnumerable<TotalReporteCierreMensualContableDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte total cierre mensual contable",
            Description = "Generar reporte total cierre mensual contable",
            OperationId = "Reportes.ReporteTotalCierreMensualContable",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<TotalReporteCierreMensualContableDto>>>> HandleAsync([FromQuery] ReporteContableCierreMensualParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteTotalCierreMensualContable(request);
        }
    }
}
