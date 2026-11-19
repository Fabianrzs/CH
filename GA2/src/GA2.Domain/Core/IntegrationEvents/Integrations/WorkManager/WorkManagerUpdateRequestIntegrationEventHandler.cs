using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core.IntegrationEvents.Integrations.WorkManager
{
    /// <summary>
    /// Workmanager update integration event
    /// </summary>
    public class WorkManagerUpdateRequestIntegrationEventHandler : IIntegrationEventHandler<WorkManagerUpdateRequestIntegrationEvent>
    {
        /// <summary>
        /// Logger application information
        /// </summary>
        private readonly ILogger<WorkManagerAttachRequestIntegrationEventHandler> _logger;

        /// <summary>
        /// Instancia de consumo WorkManager
        /// </summary>
        private readonly IConsumoWorkManager _consumo;

        public WorkManagerUpdateRequestIntegrationEventHandler(ILogger<WorkManagerAttachRequestIntegrationEventHandler> logger, IConsumoWorkManager consumo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumo = consumo ?? throw new ArgumentNullException(nameof(consumo));
        }

        public async Task Handle(WorkManagerUpdateRequestIntegrationEvent @event)
        {
            _logger.LogInformation($"Executando evento integrado update form WorkManager");
            await _consumo.Form_UpdateAsync(@event.UpdateRequest);
        }
    }
}

