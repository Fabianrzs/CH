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
    /// ReportePreliminarCarteraEndPoint
    /// </summary>
    [Authorize]
    public class ReportePreliminarCartera : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReportePreliminarCarteraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePreliminarCarteraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePreliminarCartera(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReportePreliminarCartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReportePreliminarCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReportePreliminarCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reportes Preliminar Cartera",
            Description = "Generar Reportes Preliminar Cartera",
            OperationId = "Reportes.ReportePreliminarCartera",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReportePreliminarCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReportePreliminarCartera();
        }

    }
}
