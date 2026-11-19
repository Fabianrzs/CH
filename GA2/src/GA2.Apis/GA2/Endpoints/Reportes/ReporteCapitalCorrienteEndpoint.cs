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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteCapitalCorrienteEndpoint
    /// </summary>
    [Authorize]
    public class ReporteCapitalCorrienteEndpoint : BaseAsyncEndpoint
        .WithRequest<ReporteCapitalCorrienteParametersDto>
        .WithResponse<Response<IEnumerable<ReporteCapitalCorrienteDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCapitalCorrienteEndpoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCapitalCorrienteEndpoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteCapitalCorriente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/CapitalCorriente")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCapitalCorrienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte capital corriente",
            Description = "Generar reporte capital corriente",
            OperationId = "Reportes.ReporteCapitalCorriente",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteCapitalCorrienteDto>>>> HandleAsync([FromQuery] ReporteCapitalCorrienteParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteCapitalCorriente(request);
        }
    }
}