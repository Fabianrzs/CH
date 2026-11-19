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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteF341
    /// </summary>
    //[Authorize]
    public class ReporteFCF341 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<List<DocumentoPorPeriodoDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteFCF341(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes F414 Información
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteFCF341")]
        [ProducesResponseType(typeof(Response<List<DocumentoPorPeriodoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Información Reporte SFC F341",
            Description = "Obtener Información Reporte SFC F341",
            OperationId = "Reportes.ReporteFCF341",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<List<DocumentoPorPeriodoDto>>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteFCF341(request);
        }
    }
}