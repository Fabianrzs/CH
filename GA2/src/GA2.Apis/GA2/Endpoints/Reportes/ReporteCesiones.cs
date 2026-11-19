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
    /// ReporteCesionesEndPoint
    /// </summary>
    [Authorize]
    public class ReporteCesiones : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReporteCesionesDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCesionesEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCesiones(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteCesiones
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteCesiones")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCesionesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Cesiones",
            Description = "Generar Reporte Cesiones",
            OperationId = "Reportes.ReporteCesiones",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteCesionesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteCesiones();
        }

    }
}
