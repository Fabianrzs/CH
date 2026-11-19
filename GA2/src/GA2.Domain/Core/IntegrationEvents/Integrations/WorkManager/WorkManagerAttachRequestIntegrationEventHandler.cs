using GA2.BuildingBlocks.EventBus.Abstractions;
using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core.IntegrationEvents.Integrations.WorkManager
{
    /// <summary>
    /// Workmanager integration event
    /// </summary>
    public class WorkManagerAttachRequestIntegrationEventHandler : IIntegrationEventHandler<WorkManagerAttachRequestIntegrationEvent>
    {
        /// <summary>
        /// Logger application information
        /// </summary>
        private readonly ILogger<WorkManagerAttachRequestIntegrationEventHandler> _logger;

        /// <summary>
        /// Instancia de consumo WorkManager
        /// </summary>
        private readonly IConsumoWorkManager _consumo;

        public WorkManagerAttachRequestIntegrationEventHandler(ILogger<WorkManagerAttachRequestIntegrationEventHandler> logger, IConsumoWorkManager consumo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumo = consumo ?? throw new ArgumentNullException(nameof(consumo));
        }

        public async Task Handle(WorkManagerAttachRequestIntegrationEvent @event)
        {
            _logger.LogInformation($"Executando evento integrado Attach-WorkManager");
            await _consumo.Form_AttachFilesAsync(@event.AttachRequest);
        }
    }
}

