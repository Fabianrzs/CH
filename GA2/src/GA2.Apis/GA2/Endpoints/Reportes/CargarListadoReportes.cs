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
    /// CargarListadoReportesEndPoint
    /// </summary>
    [Authorize]
    public class CargarListadoReportes : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<CargarListadoReportesDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// CargarListadoReportesEndPoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public CargarListadoReportes(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// CargarListadoReportes
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/CargarListadoReportes")]
        [ProducesResponseType(typeof(Response<IEnumerable<CargarListadoReportesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Cargar Listado Reportes",
            Description = "Cargar Listado Reportes",
            OperationId = "Reportes.CargarListadoReportes",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CargarListadoReportesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.CargarListadoReportes();
        }

    }
}
