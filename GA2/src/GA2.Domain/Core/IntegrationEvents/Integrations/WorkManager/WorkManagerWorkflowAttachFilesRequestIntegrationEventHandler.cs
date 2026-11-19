using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core.IntegrationEvents.Integrations.WorkManager
{
    /// <summary>
    /// Workmanager workflow attach
    /// </summary>
    public class WorkManagerWorkflowAttachFilesRequestIntegrationEventHandler : IIntegrationEventHandler<WorkManagerWorkflowAttachFilesRequestIntegrationEvent>
    {
        /// <summary>
        /// Logger application information
        /// </summary>
        private readonly ILogger<WorkManagerAttachRequestIntegrationEventHandler> _logger;

        /// <summary>
        /// Instancia de consumo WorkManager
        /// </summary>
        private readonly IConsumoWorkManager _consumo;

        public WorkManagerWorkflowAttachFilesRequestIntegrationEventHandler(ILogger<WorkManagerAttachRequestIntegrationEventHandler> logger, IConsumoWorkManager consumo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumo = consumo ?? throw new ArgumentNullException(nameof(consumo));
        }

        /// <summary>
        /// Handler events
        /// </summary>
        /// <param name="event">Event workflow attach</param>
        public async Task Handle(WorkManagerWorkflowAttachFilesRequestIntegrationEvent @event)
        {
            _logger.LogInformation($"Executando evento integrado");
            await _consumo.Workflow_AttachFilesAsync(@event.WorkflowAttachFilesRequest);
        }
    }
}

