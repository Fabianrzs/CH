using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;

namespace GA2.Apis.GA2.Endpoints.Reportes.PazSalvo
{
    /// <summary>
    /// PazSalvoEndPoint
    /// </summary>
    //[Authorize]
    public class ReportePazSalvoEndPoint : BaseAsyncEndpoint
            .WithRequest<ReportePazSalvoParemetersDto>
            .WithResponse<object>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePazSalvoEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePazSalvoEndPoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReportePazSalvoEndPoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/PazSalvo")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Paz y Salvo",
            Description = "Generar Reporte Paz y Salvo",
            OperationId = "Reportes.ReportePazSalvoEndPoint",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<object>> HandleAsync([FromQuery] ReportePazSalvoParemetersDto request, CancellationToken cancellationToken = default)
        {
            var result = await _reportesBusinessLogic.GenerarReportePazSalvo(request);

            if (result.IsSuccess)
                return result.Data;

            return BadRequest(result);
        }
    }
}
