using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
