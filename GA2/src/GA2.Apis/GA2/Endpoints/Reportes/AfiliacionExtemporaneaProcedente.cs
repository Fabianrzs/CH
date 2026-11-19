using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// AfiliacionExtemporaneaProcedente
    /// </summary>
    [Authorize]
    public class AfiliacionExtemporaneaProcedente : BaseAsyncEndpoint
        .WithRequest<ParametrosReportesAfiliacionesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteAfiliacionExtemporaneaProcedente
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public AfiliacionExtemporaneaProcedente(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// AfiliacionExtemporaneaProcedente
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/AfiliacionExtemporaneaProcedente")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Afiliacion Extemporanea Procedente",
            Description = "Generar Reporte Afiliacion Extemporanea Procedente",
            OperationId = "Reportes.AfiliacionExtemporaneaProcedente",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosReportesAfiliacionesDto param, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.AfiliacionExtemporaneaProcedente(param);
        }
    }
}
