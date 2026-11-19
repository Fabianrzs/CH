using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core.IntegrationEvents.Integrations.WorkManager
{
    /// <summary>
    /// Workmanager insert request integration event
    /// </summary>
    public class WorkManagerInsertRequestIntegrationEventHandler : IIntegrationEventHandler<WorkManagerInsertRequestIntegrationEvent>
    {

        /// <summary>
        /// Logger application information
        /// </summary>
        private readonly ILogger<WorkManagerAttachRequestIntegrationEventHandler> _logger;

        /// <summary>
        /// Instancia de consumo WorkManager
        /// </summary>
        private readonly IConsumoWorkManager _consumo;

        /// <summary>
        /// Workmanager integration event
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="consumo"></param>
        /// <exception cref="ArgumentNullException">Null exception dependecy injection</exception>
        public WorkManagerInsertRequestIntegrationEventHandler(ILogger<WorkManagerAttachRequestIntegrationEventHandler> logger,
                                                               IConsumoWorkManager consumo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumo = consumo ?? throw new ArgumentNullException(nameof(consumo));
        }

        public async Task Handle(WorkManagerInsertRequestIntegrationEvent @event)
        {
            _logger.LogInformation($"Executando evento integrado Insert Workmanager");
            var result = await _consumo.Form_InsertAsync(@event.InsertRequest);
            if (result.Form_InsertResult.Success)
            {
                var numeroSolicitud = @event.InsertRequest.request.Data.FirstOrDefault(s => s.Field.Equals("C0098"));
                //await _solicitudRepository.UpdateSoli(numeroSolicitud.Value, result.Form_InsertResult.BarCode);
            }
        }
    }


}

