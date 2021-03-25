using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.Services.Carts.Domain.Events;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.DomainEventHandlers
{
    public class CartCreatedDomainEventHandler : INotificationHandler<CartCreatedDomainEvent>
    {
        private readonly ILogger<CartCreatedDomainEventHandler> _logger;

        public CartCreatedDomainEventHandler(ILogger<CartCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CartCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("---- Begin to handle CartCreatedDomainEvent");

            // throw new System.NotImplementedException();

            return Task.CompletedTask;
        }
    }
}