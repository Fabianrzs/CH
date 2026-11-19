using GA2.BuildingBlocks.EventBus.Events;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;

namespace GA2.Domain.Core.IntegrationEvents.Events.WorkManager
{
    public record WorkManagerInsertRequestIntegrationEvent : IntegrationEvent
    {
        public Form_InsertRequest InsertRequest { get; set; }
    }

    public record WorkManagerUpdateRequestIntegrationEvent: IntegrationEvent
    {
        public Form_UpdateRequest UpdateRequest { get; set; }
    }

    public record WorkManagerDeleteRequestIntegrationEvent : IntegrationEvent
    {
        public Form_DeleteRequest  DeleteRequest{ get; set; }
    }

    public record WorkManagerWorkflowAttachFilesRequestIntegrationEvent : IntegrationEvent
    {
        public Workflow_AttachFilesRequest WorkflowAttachFilesRequest { get; set; }
    }

    public record WorkManagerAttachRequestIntegrationEvent : IntegrationEvent
    {
        public Form_AttachFilesRequest AttachRequest { get; set; }
    }

}
