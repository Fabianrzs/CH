using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReportePreliminarGarantiasEndPoint
    /// </summary>
    [Authorize]
    public class ReportePreliminarGarantias : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReportePreliminarGarantiasDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePreliminarGarantiasEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePreliminarGarantias(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReportePreliminarGarantias
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportePreliminarGarantias")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReportePreliminarGarantiasDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reportes Superfinanciera",
            Description = "Generar Reportes Superfinanciera",
            OperationId = "Reportes.ReportePreliminarGarantias",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReportePreliminarGarantiasDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReportePreliminarGarantias();
        }

    }
}
