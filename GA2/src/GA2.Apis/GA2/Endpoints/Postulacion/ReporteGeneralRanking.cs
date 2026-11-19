using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Domain.Core.Postulacion;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ReporteGeneralRanking : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<FileResult>>
    {
        private readonly IPostulacionBussineLogic _reporteExcel;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteGeneralRanking(IPostulacionBussineLogic reporteExcel)
        {
            _reporteExcel = reporteExcel ?? throw new ArgumentException(nameof(reporteExcel));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReportesGeneralRanking")]
        [ProducesResponseType(typeof(Response<ReportRequestExcelDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Excels Globals",
            Description = "Generar Reporte reporte Excel Globals",
            OperationId = "Reportes.ReportesExcel",
            Tags = new[] { "ReportesEndpoint" })]



        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(int idConvocatoria,CancellationToken cancellationToken = default)
        {
            return await _reporteExcel.GetReporteGenralRanking(idConvocatoria);
        }
    }
}
