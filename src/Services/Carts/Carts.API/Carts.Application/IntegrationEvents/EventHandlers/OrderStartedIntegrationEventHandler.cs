using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.Services.Carts.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

        public OrderStartedIntegrationEventHandler(IMediator mediator, ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            var buyerId = @event.UserId;
            var productIds = @event.OrderLines.Select(ol => ol.ProductId).ToArray();

            var removeCartItemsCommand = new RemoveCartLinesCommand { BuyerId = buyerId, ProductIds = productIds };

            return _mediator.Send(removeCartItemsCommand);
        }
    }
}