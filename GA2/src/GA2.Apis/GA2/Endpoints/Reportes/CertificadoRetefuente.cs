using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// ReporteRetefuenteCredito
    /// </summary>
    [Authorize]
    public class CertificadoRetefuente : BaseAsyncEndpoint
        .WithRequest<ReporteAnioGravableParametersDto>
        .WithResponse<object>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteRetefuenteCredito
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public CertificadoRetefuente(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteRetefuenteCredito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/CertificadoRetefuente")]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Año Gravable",
            Description = "Generar Reporte Año Gravable",
            OperationId = "Reportes.ReporteAnioGravable",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<object>> HandleAsync([FromQuery] ReporteAnioGravableParametersDto request, CancellationToken cancellationToken = default)
        {
            var result = await _reportesBusinessLogic.GenerarReporteRetefuente(request);

            if (result.IsSuccess)
                return result.Data;

            return BadRequest(result);
        }

    }
}
