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
    /// ReporteSFCF477
    /// </summary>
    //[Authorize]
    public class ReporteFCF477 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<List<Resumen477Dto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteSFCF477
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteFCF477(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteFCF477")]
        [ProducesResponseType(typeof(Response<List<Resumen477Dto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte SFC F477",
            Description = "Generar Reporte SFC F477",
            OperationId = "Reportes.ReporteSFCF477",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<List<Resumen477Dto>>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteFCF477(request);
        }
    }
}