using GA2.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace GA2.BuildingBlocks.EventBus.Abstractions
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
