using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
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

namespace GA2.Apis.GA2.Endpoints.Reportes.parametrizacionReports
{
    /// <summary>
    /// Metodo ConsultaReportes
    /// </summary>
    [Authorize]
    [ApiController]
    public class ConsultaReportesGenerales : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<SettingsDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteGeneralCarteraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ConsultaReportesGenerales(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// Metodo Get
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ConsultaReportesGenerales")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteGeneralCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Reportes Generales",
            Description = "Consultar Reportes Generales",
            OperationId = "Consultar Reportes Generales",
            Tags = new[] { "GetReports" })]
        public async override Task<ActionResult<Response<IEnumerable<SettingsDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ConsultaReportesGenerales();
        }
    }
}
