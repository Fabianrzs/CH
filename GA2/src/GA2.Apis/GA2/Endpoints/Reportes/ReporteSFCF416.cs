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
    /// ReporteSFCF416
    /// </summary>
    [Authorize]
    public class ReporteSFCF416 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<List<ReporteSFCF416Dto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteSFCF416
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSFCF416(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteSFCF416")]
        [ProducesResponseType(typeof(Response<List<ReporteSFCF416Dto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte SFC F16",
            Description = "Generar Reporte SFC F16",
            OperationId = "Reportes.ReporteSFCF416",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<List<ReporteSFCF416Dto>>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSFCF416(request);
        }
    }
}
