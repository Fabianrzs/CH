using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// Controller para generar el reporte jefe
    /// </summary>
    [Authorize]
    public class ReporteJefe : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReporteGeneralCarteraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

    /// <summary>
    /// ReporteGeneralCarteraEndPoint
    /// </summary>
    /// <param name="reportesBusinessLogic"></param>
    /// <exception cref="ArgumentException"></exception>
    public ReporteJefe(IReportesBusinessLogic reportesBusinessLogic)
    {
        _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
    }

        /// <summary>
        /// ReporteGeneralCartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReporteJefe")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteGeneralCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "ReporteJefe",
            Description = "Generar Reporte Jefe",
            OperationId = "Reportes.ReporteJefe",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteGeneralCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteGeneralCartera();
        }
    }
}
