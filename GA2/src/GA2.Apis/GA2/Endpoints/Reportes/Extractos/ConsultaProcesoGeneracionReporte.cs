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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes.Extractos
{
    /// <summary>
    /// ConsultaProcesoGeneracionReporte
    /// </summary>
    [Authorize]
    public class ConsultaProcesoGeneracionReporte : BaseAsyncEndpoint
        .WithRequest<ReporteProcesoGeneracionReporteParametersDto>
        .WithResponse<Response<IEnumerable<ProcesoGeneracionReporteDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ConsultaProcesoGeneracionReporte
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ConsultaProcesoGeneracionReporte(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ConsultaProcesoGeneracionReporte
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ConsultaProcesoGeneracionReporte")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProcesoGeneracionReporteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar procesos de generacion reporte",
            Description = "Consultar procesos de generacion reporte",
            OperationId = "Reportes.ConsultaProcesoGeneracionReporte",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ProcesoGeneracionReporteDto>>>> HandleAsync(ReporteProcesoGeneracionReporteParametersDto tipoReporte, CancellationToken cancellationToken = default)
        {
            var idUsuario = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "USR_ID").Value);
            var result = await _reportesBusinessLogic.ConsultarProcesosGeneracionReportePorUsuario(idUsuario, tipoReporte);
            return result;
        }
    }
}
