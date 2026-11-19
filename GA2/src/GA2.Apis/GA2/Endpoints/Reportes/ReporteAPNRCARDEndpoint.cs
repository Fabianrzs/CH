using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.Reportes;
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
    /// ReporteAPNRCARDEndpoint
    /// </summary>
    [Authorize]
    public class ReporteAPNRCARDEndpoint : BaseAsyncEndpoint
        .WithRequest<ReporteAPNRCARParametersDto>
        .WithResponse<Response<IEnumerable<ReporteAPNRCARDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteAPNRCARDEndpoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteAPNRCARDEndpoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReporteAPNRCAR")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteAPNRCARDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte APNRCAR",
            Description = "Generar Reporte APNRCAR",
            OperationId = "Reportes.ReporteCertificadoIngresosRetenciones",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteAPNRCARDto>>>> HandleAsync([FromQuery]ReporteAPNRCARParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteAPNRCAR(request);
        }
    }
}