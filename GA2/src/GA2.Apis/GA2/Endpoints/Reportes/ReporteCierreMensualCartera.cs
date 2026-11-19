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
    /// ReporteCierreMensualCarteraEndPoint
    /// </summary>
    [Authorize]
    public class ReporteCierreMensualCartera : BaseAsyncEndpoint
        .WithRequest<ParamReporteCierreMensualCarteraDto>
        .WithResponse<Response<IEnumerable<ReporteCierreMensualCarteraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCierreMensualCarteraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCierreMensualCartera(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteCierreMensualCartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteCierreMensualCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCierreMensualCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte General Cartera",
            Description = "Generar Reporte General Cartera",
            OperationId = "Reportes.ReporteCierreMensualCartera",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteCierreMensualCarteraDto>>>> HandleAsync(ParamReporteCierreMensualCarteraDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteCierreMensualCartera(request);
        }

    }
}
