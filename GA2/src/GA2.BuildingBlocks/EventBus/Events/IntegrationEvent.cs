
using System;

namespace GA2.BuildingBlocks.EventBus.Events
{
    public record IntegrationEvent
    {
        public Guid Id { get; private init; }
        public DateTime CreatedOn { get; set; }

        public IntegrationEvent()
        {
           Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }

        public IntegrationEvent(Guid id, DateTime createOn)
        {
            Id = id;
            CreatedOn = createOn;
        }
    }
}
