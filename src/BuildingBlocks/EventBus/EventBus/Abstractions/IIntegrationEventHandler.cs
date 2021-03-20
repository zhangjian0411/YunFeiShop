using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
