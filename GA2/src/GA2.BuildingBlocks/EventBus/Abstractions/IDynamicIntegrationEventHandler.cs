using System.Threading.Tasks;

namespace GA2.BuildingBlocks.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic @event);
    }
}
