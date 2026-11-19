using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
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

namespace GA2.Apis.GA2.Endpoints.Reportes.Extractos
{
    /// <summary>
    /// CancelarProcesoGeneracionReporte
    /// </summary>
    [Authorize]
    public class CancelarProcesoGeneracionReporte : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<ProcesoGeneracionReporteDto>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// CancelarProcesoGeneracionReporte
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public CancelarProcesoGeneracionReporte(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// CancelarProcesoGeneracionReporte
        /// </summary>
        /// <param name="idProceso"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/CancelarProcesoGeneracionReporte")]
        [ProducesResponseType(typeof(Response<ProcesoGeneracionReporteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Cancelar procesos de generacion reporte",
            Description = "Cancelar procesos de generacion reporte",
            OperationId = "Reportes.CancelarProcesoGeneracionReporte",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<ProcesoGeneracionReporteDto>>> HandleAsync(Guid idProceso, CancellationToken cancellationToken = default)
        {
            var result = await _reportesBusinessLogic.CancelarProcesoGeneracionReporte(idProceso);
            return result;
        }
    }
}
