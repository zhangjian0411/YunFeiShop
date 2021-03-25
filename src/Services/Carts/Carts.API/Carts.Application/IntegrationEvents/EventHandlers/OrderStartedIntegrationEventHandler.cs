using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

        public OrderStartedIntegrationEventHandler(ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            _logger.LogWarning($"---- Begin to handle integration event {@event}");

            return Task.CompletedTask;
        }
    }
}