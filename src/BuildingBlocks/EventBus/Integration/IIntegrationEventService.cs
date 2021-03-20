using System;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Events;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration
{
    public interface IIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}