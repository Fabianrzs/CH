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
    /// ReporteCargaAnalistaEndPoint
    /// </summary>
    [Authorize]
    public class ReporteCargaAnalista : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReporteCargaAnalistaDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCargaAnalistaEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCargaAnalista(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteCargaAnalista
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteCargaAnalista")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCargaAnalistaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Carga Analista",
            Description = "Generar Reporte Carga Analista",
            OperationId = "Reportes.ReporteCargaAnalista",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteCargaAnalistaDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteCargaAnalista();
        }

    }
}
