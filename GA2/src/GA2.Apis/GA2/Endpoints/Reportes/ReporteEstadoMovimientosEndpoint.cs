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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteEstadoMovimientosEndpoint
    /// </summary>
    [Authorize]
    public class ReporteEstadoMovimientosEndpoint : BaseAsyncEndpoint
        .WithRequest<ReporteEstadoMovimientosParametersDto>
        .WithResponse<Response<IEnumerable<ReporteEstadoMovimientosDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteEstadoMovimientosEndpoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteEstadoMovimientosEndpoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/EstadoMovimientos")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteEstadoMovimientosDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Estado Movimientos",
            Description = "Generar Reporte Estado Movimientos",
            OperationId = "Reportes.ReporteEstadoMovimientos",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteEstadoMovimientosDto>>>> HandleAsync([FromQuery] ReporteEstadoMovimientosParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteEstadoMovimientos(request);
        }
    }
}