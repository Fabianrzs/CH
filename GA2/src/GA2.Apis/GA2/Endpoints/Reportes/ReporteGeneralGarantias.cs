using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
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
    /// ReporteGeneralGarantiasEndPoint
    /// </summary>
    //[Authorize]
    public class ReporteGeneralGarantias : BaseAsyncEndpoint
        .WithRequest<ReporteGeneralGarantiaParametersDto>
        .WithResponse<Response<IEnumerable<ReporteGeneralGarantiasDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteGeneralGarantiasEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteGeneralGarantias(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteGeneralGarantias
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReporteGeneralGarantias")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteGeneralGarantiasDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reportes General Garantias",
            Description = "Generar Reportes General Garantias",
            OperationId = "Reportes.ReporteGeneralGarantias",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteGeneralGarantiasDto>>>> HandleAsync([FromQuery]ReporteGeneralGarantiaParametersDto request,CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteGeneralGarantias(request);
        }

    }
}
