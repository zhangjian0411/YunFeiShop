using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.Events;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.DomainEventHandlers
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly BuildingBlocks.Integration.IIntegrationEventService _integrationEventService;

        public OrderStartedDomainEventHandler(BuildingBlocks.Integration.IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService;
        }

        public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrEvent = new IntegrationEvents.Events.OrderStartedIntegrationEvent(domainEvent.BuyerId);
            await _integrationEventService.AddAndSaveEventAsync(integrEvent);
        }
    }
}