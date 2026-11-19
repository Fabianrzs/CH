using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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
    /// ReporteGeneralCarteraEndPoint
    /// </summary>
    [Authorize]
    public class GenerarReporteGeneralCarteraReestructurado : BaseAsyncEndpoint
        .WithRequest<ReporteGeneralCarteraParametersDto>
        .WithResponse<Response<IEnumerable<CarteraReporteGeneralDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteGeneralCarteraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public GenerarReporteGeneralCarteraReestructurado(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteGeneralCartera
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/GenerarReporteGeneralCarteraReestructurado")]
        [ProducesResponseType(typeof(Response<IEnumerable<CarteraReporteGeneralDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte General Cartera Reestructurado",
            Description = "Generar Reporte General Cartera Reestructurado",
            OperationId = "Reportes.ReporteGeneralCartera Reestructurado",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CarteraReporteGeneralDto>>>> HandleAsync([FromQuery] ReporteGeneralCarteraParametersDto parametros, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteGeneralCarteraReestructurado(parametros);
        }
    }
}
