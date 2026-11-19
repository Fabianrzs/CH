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
    /// AfiliacionVoluntariaProcedente
    /// </summary>
    [Authorize]
    public class AfiliacionVoluntariaProcedente : BaseAsyncEndpoint
        .WithRequest<ParametrosReportesAfiliacionesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteAfiliacionVoluntariaProcedente
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public AfiliacionVoluntariaProcedente(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// AfiliacionVoluntariaProcedente
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/AfiliacionVoluntariaProcedente")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Afiliacion Voluntaria Procedente",
            Description = "Generar Reporte Afiliacion Voluntaria Procedente",
            OperationId = "Reportes.AfiliacionVoluntariaProcedente",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosReportesAfiliacionesDto param, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.AfiliacionVoluntariaProcedente(param);
        }
    }
}
