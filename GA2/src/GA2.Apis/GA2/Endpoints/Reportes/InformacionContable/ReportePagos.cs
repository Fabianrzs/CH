using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
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

namespace GA2.Apis.GA2.Endpoints.Reportes.InformacionContable
{
    /// <summary>
    /// ReportePagos
    /// </summary>
    [Authorize]
    public class ReportePagos : BaseAsyncEndpoint
        .WithRequest<ReportePagosParametersDto>
        .WithResponse<Response<ResultadoReportePagos>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePagos
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReportePagos(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReportePagos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/Pagos")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReportePagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte pagos",
            Description = "Generar reporte pagos",
            OperationId = "Reportes.ReportePagos",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<ResultadoReportePagos>>> HandleAsync([FromQuery] ReportePagosParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReportePagos(request);
        }
    }
}
