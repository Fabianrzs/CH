using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.BackgroundTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes.Extractos
{
    /// <summary>
    /// ReporteCreditoExtractoMasivo
    /// </summary>
    [Authorize]
    public class ReporteCreditoExtractoMasivo : BaseAsyncEndpoint
        .WithRequest<ReporteExtractoMasivoParametersDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IBackgroundReportQueue backgroundReportQueue;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCreditoExtractoMasivo
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <param name="backgroundReportQueue"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteCreditoExtractoMasivo(IReportesBusinessLogic reportesBusinessLogic, IBackgroundReportQueue backgroundReportQueue)
        {
            this.backgroundReportQueue = backgroundReportQueue;
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteCreditoExtractoMasivo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ExtractoMasivo")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Extracto Masivo",
            Description = "Generar Reporte Extracto Masivo",
            OperationId = "Reportes.ReporteExtractoMasivo",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ReporteExtractoMasivoParametersDto request, CancellationToken cancellationToken = default)
        {
            request.IdUsuario = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "USR_ID").Value);
            var proceso = await _reportesBusinessLogic.CrearProcesoGeneracionReporteExtractoMasivo(request);

            if (proceso.IsSuccess)
            {
                request.IdProceso = proceso.Data.IdProceso;
                var result = await _reportesBusinessLogic.GenerarReporteExtractoMasivo(request);
                return result;
                //var data = new ReporteExtractoParametersDto()
                //{
                //    TipoCredito = (int)request.TipoCredito,
                //    Anio = request.Anio,
                //    Mes = request.Mes,
                //};
                //if ((int)request.TipoExtracto == 1)
                //{
                //    return await _reportesBusinessLogic.GenerarReporteExtractoMensual(data);
                //}
                //    return await _reportesBusinessLogic.GenerarReporteExtractoAnual(data);
                //await GenerarReporte(proceso.Data.IdProceso, request);

                //return Accepted(new Response<Guid?>()
                //{
                //    Data = proceso.Data.IdProceso,
                //    IsSuccess = true,
                //    ReturnMessage = "Se esta procesando, una vez este listo podra descargar el reporte ..."
                //});
            }

            return BadRequest(new Response<FileResult>()
            {
                IsSuccess = false,
                ReturnMessage = proceso.ReturnMessage
            });
        }

        private async Task GenerarReporte(Guid idProceso, ReporteExtractoMasivoParametersDto request)
        {
            request.IdProceso = idProceso;
            
            await backgroundReportQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, cancellationToken) =>
            {
                var reportesLogic = serviceProvider.GetRequiredService<IReportesBusinessLogic>();
                await reportesLogic.GenerarReporteExtractoMasivo(request);
            });
        }
    }
}
