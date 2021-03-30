using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.Events;
using static ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.Events.OrderStartedIntegrationEvent;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.DomainEventHandlers
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly BuildingBlocks.IntegrationEvents.Abstractions.IIntegrationEventService _integrationEventService;

        public OrderStartedDomainEventHandler(BuildingBlocks.IntegrationEvents.Abstractions.IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService;
        }

        public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrEvent = new IntegrationEvents.Events.OrderStartedIntegrationEvent
            {
                UserId = domainEvent.BuyerId,
                OrderLines = domainEvent.Order.OrderLines.Select(line => new OrderLine { ProductId = line.ProductId }).ToArray()
            }; 
            System.Console.WriteLine($"OrderStartedIntegrationEvent: {Newtonsoft.Json.JsonConvert.SerializeObject(integrEvent)}");
            await _integrationEventService.AddAndSaveEventAsync(integrEvent);
        }
    }
}