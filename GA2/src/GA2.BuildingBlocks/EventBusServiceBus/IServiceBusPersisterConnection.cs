using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;

namespace GA2.BuildingBlocks.EventBusServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusClient TopicClient { get; }
        ServiceBusAdministrationClient AdministrationClient { get; }
    }
}
