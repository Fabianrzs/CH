using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons; // Asegúrate de incluir este namespace
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
    /// ReportePlanoSFCF477
    /// </summary>
    //[Authorize]
    public class ReportePlanoSFCF507 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<FileResult>> // Ajustado para usar el wrapper Response
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportePlanoSFCF507(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// Generar Archivo Plano Reporte SFC F477
        /// </summary>
        [HttpPost("api/Reportes/ReportePlanoSFCF507")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Archivo Plano Reporte SFC F507",
            Description = "Generar Archivo Plano Reporte SFC F507",
            OperationId = "Reportes.ReportePlanoSFCF507",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReportePlanoSFCF507(request);
        }
    }
}