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
    /// AfiliacionVoluntariaVeteranosProcedenteAD
    /// </summary>
    [Authorize]
    public class AfiliacionVoluntariaVeteranosProcedenteAD : BaseAsyncEndpoint
        .WithRequest<ParametrosReportesAfiliacionesDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteAfiliacionVoluntariaVeteranosProcedenteAD
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public AfiliacionVoluntariaVeteranosProcedenteAD(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// AFILIACION VOLUNTARIA VETERANOS_PROCEDENTE_Línea de Ahorro por Activación De Descuento
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/AfiliacionVoluntariaVeteranosProcedenteAD")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Afiliacion Voluntaria Veteranos Procedente AD",
            Description = "Generar Reporte Afiliacion Admin Cesantias Procedente",
            OperationId = "Reportes.AfiliacionVoluntariaVeteranosProcedenteAD",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ParametrosReportesAfiliacionesDto param, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.AfiliacionVoluntariaVeteranosProcedenteAD(param);
        }
    }
}
