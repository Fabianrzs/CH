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
    /// ReporteFCF414
    /// </summary>
    //[Authorize]
    public class ReporteFCF414 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<List<ReporteSFCF414Dto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteFCF414(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes F414 Información
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteFCF414")]
        [ProducesResponseType(typeof(Response<List<ReporteSFCF414Dto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Información Reporte SFC F414",
            Description = "Obtener Información Reporte SFC F414",
            OperationId = "Reportes.ReporteFCF414",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<List<ReporteSFCF414Dto>>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteFCF414(request);
        }
    }
}