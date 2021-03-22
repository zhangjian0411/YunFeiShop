using System;
using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions
{
    public interface IIntegrationEventService
    {
        Task PublishEventsAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}