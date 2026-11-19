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
    /// ReportePlanoSFCF414_V2
    /// </summary>
    //[Authorize]
    public class ReportePlanoSFCF414_V2 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePlanoSFCF414_V2
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePlanoSFCF414_V2(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportePlanoSFCF414_V2")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte SFC F16",
            Description = "Generar Reporte SFC F16",
            OperationId = "Reportes.ReportePlanoSFCF414_V2",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReportePlanoSFCF414_V2(request);
        }
    }
}
