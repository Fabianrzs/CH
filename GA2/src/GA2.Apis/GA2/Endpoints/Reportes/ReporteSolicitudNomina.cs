using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    public class ReporteSolicitudNomina : BaseAsyncEndpoint

        .WithRequest<RequestReporteSolicitudNomina>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesExcelLogic _reporteExcel;

        /// <summary>
        /// Solicitud Nomina
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteSolicitudNomina(IReportesExcelLogic reporteExcel)
        {
            _reporteExcel = reporteExcel ?? throw new ArgumentException(nameof(reporteExcel));
        }


        /// <summary>
        /// Solicitud Nomina
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteExcelSolicitudNomina")]
        [ProducesResponseType(typeof(Response<ReportRequestExcelDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Solicitud Nomina",
            Description = "Generar Reporte Solicitud Nomina",
            OperationId = "Reportes.ReportesExcel",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(RequestReporteSolicitudNomina request, CancellationToken cancellationToken = default)
        {
            return await _reporteExcel.GenerarExcelSolicitudNomina(request); 
        }
    }
}
