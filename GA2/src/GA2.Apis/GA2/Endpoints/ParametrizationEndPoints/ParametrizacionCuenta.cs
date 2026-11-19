using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System;
using Microsoft.AspNetCore.SignalR;
using GA2.Application.Main;
using System.Diagnostics;
using GA2.Transversals.Commons.BackgroundTask;
using System.Linq;

namespace GA2.Apis.GA2.Endpoints.ParametrizationEndPoints
{
    public class ParametrizacionCuenta : BaseAsyncEndpoint.WithRequest<IPCDto>
        .WithoutResponse
    {
        private readonly IParametrizacionBusinessLogic _parametrizationBusinessLogic;
        private readonly IHubContext<HubNotificaciones> _hubContext;
        private readonly IBackgroundReportQueue _backgroundReportQueue;

        /// <summary>
        /// Constructor de Endpoint Crear Valor IPC
        /// </summary>
        /// <param name="parametrizationBusinessLogic"></param>
        /// <author>Cesar Hernandez</author>
        /// <date>18/05/2021</date>
        public ParametrizacionCuenta(IParametrizacionBusinessLogic parametrizationBusinessLogic, IHubContext<HubNotificaciones> hubContext, IBackgroundReportQueue backgroundReportQueue)
        {
            _parametrizationBusinessLogic = parametrizationBusinessLogic;
            _hubContext = hubContext;  
            _backgroundReportQueue = backgroundReportQueue; 

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/parametrizacion/ParametrizacionCuenta")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IPCDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = " crear parametrizacion legal Gmf",
            Description = "crear parametrizacion legal Gmf",
            OperationId = "parametrizacion.IdIPC",
            Tags = new[] { "ParametrizationEndPoint" })]
        public override  async Task<ActionResult> HandleAsync(IPCDto request, CancellationToken cancellationToken = default)
        {

            await  _backgroundReportQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, cancellationToken) =>
            {
                try
                {
                    await _parametrizationBusinessLogic.SincronizacionCuentas(request);

                    await _hubContext.Clients.All // o request.ConnectionId si lo manejas
                        .SendAsync("SincronizacionLiquidacionInteres", new
                        {
                            request.UsuarioId,
                            estado = "Sincronización finalizada con éxito"
                        });
                }
                catch (Exception ex)
                {
                    throw;
                }
            });

            return await Task.FromResult<ActionResult>(Accepted(new
            {
                mensaje = "Tarea de sincronizacion de cuentas ejecutandose en segundo plano"
            }));
        }
    }
}
