using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.Events;
using static ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands.CreateOrderCommand;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.EventHandlers
{
    public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

        public UserCheckoutAcceptedIntegrationEventHandler(IMediator mediator, ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
        {
            _logger.LogInformation($"Begin to handle integration event {JsonConvert.SerializeObject(@event)}");

            var command = new CreateOrderCommand(@event.UserId, @event.CheckoutLines.Select(cl => new OrderLine(cl.ProductId, cl.ProductName, cl.Quantity)));
            
            await _mediator.Send(command);
        }
    }
}