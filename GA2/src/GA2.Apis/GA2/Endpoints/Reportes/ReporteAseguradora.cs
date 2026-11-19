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
    /// ReporteAseguradoraEndPoint
    /// </summary>
    [Authorize]
    public class ReporteAseguradora : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<ReporteAseguradoraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteAseguradoraEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteAseguradora(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteAseguradora
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteAseguradora")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteAseguradoraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Cargar Listado Reporte Aseguradora",
            Description = "Cargar Listado Reporte Aseguradora",
            OperationId = "Reportes.ReporteAseguradora",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteAseguradoraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteAseguradora();
        }

    }
}
