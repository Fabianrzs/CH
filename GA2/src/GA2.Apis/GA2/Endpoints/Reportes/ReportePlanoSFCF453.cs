using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    [Authorize]
    public class ReportePlanoSFCF453 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteSFCF453
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePlanoSFCF453(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportePlanoSFCF453")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte SFC F453",
            Description = "Generar Reporte SFC F453",
            OperationId = "Reportes.ReporteSFCF453",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteSFCF453Y454(request);
        }
    }
}
