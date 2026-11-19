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
    /// ReporteContablePagos
    /// </summary>
    [Authorize]
    public class ReporteContablePagos : BaseAsyncEndpoint
        .WithRequest<ReporteContableParametersDto>
        .WithResponse<Response<IEnumerable<ReporteContableDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteContablePagos
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteContablePagos(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteContablePagos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReporteContablePagos")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteContableDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte contable pagos",
            Description = "Generar reporte contable pagos",
            OperationId = "Reportes.ReporteContablePagos",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteContableDto>>>> HandleAsync([FromQuery] ReporteContableParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteContablePagos(request);
        }
    }
}
