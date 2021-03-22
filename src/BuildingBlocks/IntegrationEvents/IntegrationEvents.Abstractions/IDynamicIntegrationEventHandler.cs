using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
