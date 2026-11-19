using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Main;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.BackgroundTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// ReporteAnioGravable
    /// </summary>
    [Authorize]
    public class ReporteRetefuenteAnioGravable : BaseAsyncEndpoint
        .WithRequest<ReporteAnioGravableParametersDto>
        .WithResponse<Response<FileResult>>
    {
        private readonly IBackgroundReportQueue backgroundReportQueue;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;
        private readonly IHubContext<HubNotificaciones> _hubContext;

        /// <summary>
        /// ReporteAnioGravable
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteRetefuenteAnioGravable(IReportesBusinessLogic reportesBusinessLogic,
            IBackgroundReportQueue backgroundReportQueue,

            IHubContext<HubNotificaciones> hubContext)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
            this.backgroundReportQueue = backgroundReportQueue;
            _hubContext = hubContext;
        }

        /// <summary>
        /// ReporteAnioGravable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/RetefuenteAnioGravable")]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Año Gravable",
            Description = "Generar Reporte Año Gravable",
            OperationId = "Reportes.ReporteAnioGravable",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<FileResult>>> HandleAsync([FromQuery] ReporteAnioGravableParametersDto request, CancellationToken cancellationToken = default)
        {
            request.IdUsuario = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "USR_ID").Value);
            var proceso = await _reportesBusinessLogic.CrearProcesoGeneracionReporteAnioGravable(request);

            if (proceso.IsSuccess)
             {
                request.IdProceso = proceso.Data.IdProceso;

                var response =  await _reportesBusinessLogic.GenerarReporteMasivoRetefuente(request);
                return response;
                //await backgroundReportQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, cancellationToken) =>
                //{
                //    try
                //    {
                //        request.IdProceso = proceso.Data.IdProceso;
                //        await _reportesBusinessLogic.GenerarReporteMasivoRetefuente(request);
                //        await _hubContext.Clients.All // o request.ConnectionId si lo manejas
                //            .SendAsync("GeneracionReporteMasivo", new
                //            {
                //                request.IdUsuario,
                //                estado = "Reporte finalizada con éxito"
                //            });
                //    }
                //    catch (Exception ex)
                //    {
                //        throw;
                //    }
                //});

                //return Accepted(new Response<Guid?>()
                //{
                //    Data = proceso.Data.IdProceso,
                //    IsSuccess = true,
                //    ReturnMessage = "Se esta procesando, una vez este listo podra descargar el reporte ..."
                //});

            }

            return BadRequest(new Response<Guid?>()
            {
                IsSuccess = false,
                ReturnMessage = proceso.ReturnMessage
            });
        }

        private async Task GenerarReporte(Guid idProceso, ReporteAnioGravableParametersDto request)
        {
            request.IdProceso = idProceso;

            await backgroundReportQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, cancellationToken) =>
            {
                var reportesLogic = serviceProvider.GetRequiredService<IReportesBusinessLogic>();
                await reportesLogic.GenerarReporteMasivoRetefuente(request);
            });
        }
    }
}
