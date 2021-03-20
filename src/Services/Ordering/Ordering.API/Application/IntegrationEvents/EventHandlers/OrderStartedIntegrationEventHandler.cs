using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Abstractions;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.Events;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.EventHandlers
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
            _logger.LogWarning($"Fake Handling event {@event}");

            return Task.CompletedTask;
        }
    }
}