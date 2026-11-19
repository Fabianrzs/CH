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
    /// ReporteOpcionCompraEndpoint
    /// </summary>
    [Authorize]
    public class ReporteOpcionCompraEndpoint : BaseAsyncEndpoint
        .WithRequest<ReporteOpcionCompraParametersDto>
        .WithResponse<Response<IEnumerable<ReporteOpcionCompraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteOpcionCompraEndpoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteOpcionCompraEndpoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteOpcionCompra
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/OpcionCompra")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteOpcionCompraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar reporte opcion de compra",
            Description = "Generar reporte opcion de compra",
            OperationId = "Reportes.ReporteOpcionCompra",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteOpcionCompraDto>>>> HandleAsync([FromQuery] ReporteOpcionCompraParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteOpcionCompra(request);
        }
    }
}