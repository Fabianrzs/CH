using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.BackgroundTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// CierreMensualCartera
    /// </summary>
    [Authorize]
    public class CierreMensualCartera : BaseAsyncEndpoint
        .WithRequest<ParamCierreMensualCarteraDto>
        .WithResponse<Response<IEnumerable<ReporteCierreMensualCarteraDto>>>
    {
        /// <summary>
        /// CierreMensualCartera
        /// </summary>

        private readonly IBackgroundReportQueue backgroundReportQueue;
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        private readonly IHubContext<HubNotificaciones> _hubContext;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CierreMensualCartera(ICarteraBusinessLogic carteraBusinessLogic, IHubContext<HubNotificaciones> hubContext, IBackgroundReportQueue backgroundReportQueue)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
            this.backgroundReportQueue = backgroundReportQueue;
            _hubContext = hubContext;
        }

        /// <summary>
        /// CierreMensualCartera
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CierreMensualCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCierreMensualCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Ejecutar Cierre Mensual Cartera",
           Description = "Ejecutar Cierre Mensual Cartera",
           OperationId = "Credito.CierreMensualCartera",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteCierreMensualCarteraDto>>>> HandleAsync(ParamCierreMensualCarteraDto request, CancellationToken cancellationToken = default)
        {


            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            //await backgroundReportQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, cancellationToken) =>
            //{

            //    try
            //    {
            //        // Notificar inicio
            //        await _hubContext.Clients.All.SendAsync("ProgresoReporte", new
            //        {
            //            request.CreadoPor,
            //            Estado = "Iniciado"
            //        });

            //        await this._carteraBusinessLogic.CierreMensualCartera(request);

            //        // Notificar éxito
            //        await _hubContext.Clients.All.SendAsync("ProgresoReporte", new
            //        {
            //            request.CreadoPor,
            //            Estado = "Finalizado con éxito"
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        // Notificar error
            //        await _hubContext.Clients.All.SendAsync("ProgresoReporte", new
            //        {
            //            request.CreadoPor,
            //            Estado = "Error",
            //            Mensaje = ex.Message
            //        });
            //    }
            //});


            return await this._carteraBusinessLogic.CierreMensualCartera(request);
        }
    }
}