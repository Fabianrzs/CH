using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    public class GenerarPlanPagoMasivoAnual : BaseAsyncEndpoint
        .WithRequest<RequestPlanPagoGeneradoDto>
        .WithResponse<byte[]>   // >>> CAMBIO: byte[] en vez de FileContentResult
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        public GenerarPlanPagoMasivoAnual(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        [HttpPost("api/Reportes/GenerarPlanesPagoMasivoAnualZipBase64Async")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Anual informacion plan pago",
            Description = "Generar Reporte Anual informacion plan pago",
            OperationId = "Reportes.ReporteInformacionPlanPago",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<byte[]>> HandleAsync([FromBody] RequestPlanPagoGeneradoDto request, CancellationToken cancellationToken = default)
        {
            var resultado = await _reportesBusinessLogic.GenerarPlanesPagoMasivoAnualZipBase64Async(request);

            if (!resultado.IsSuccess)
            {
                return BadRequest(new { resultado.ReturnMessage });
            }

            var nombreArchivo = $"Planes Pago {request.Periodo}.zip";

            Response.Headers.Add("X-Resultado-Mensaje", Uri.EscapeDataString(resultado.ReturnMessage));
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Resultado-Mensaje");

            return new FileContentResult(resultado.Data, "application/zip")
            {
                FileDownloadName = nombreArchivo
            };
        }
    }
}
