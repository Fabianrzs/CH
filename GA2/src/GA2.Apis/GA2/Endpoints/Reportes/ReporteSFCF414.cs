using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteSFCF414
    /// </summary>
    [Authorize]
    public class ReporteSFCF414 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteSFCF414
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSFCF414(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteSFCF414")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte SFC F16",
            Description = "Generar Reporte SFC F16",
            OperationId = "Reportes.ReporteSFCF414",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSFCF414(request);
        }
    }
}
