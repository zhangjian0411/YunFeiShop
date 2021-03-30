using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions;
using ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;
using static ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events.UserCheckoutAcceptedIntegrationEvent;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, bool>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<CheckOutCommandHandler> _logger;

        public CheckOutCommandHandler(ICartRepository cartRepository, IIntegrationEventService integrationEventService, ILogger<CheckOutCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _integrationEventService = integrationEventService;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckOutCommand command, CancellationToken cancellationToken)
        {
            var cartLines = await GetCheckOutCartLines(command.BuyerId);

            var integrationEvent = new UserCheckoutAcceptedIntegrationEvent 
            {
                UserId = command.BuyerId,
                CheckoutLines = cartLines.Select(i => new CheckoutLine { ProductId = i.ProductId, Quantity = i.Quantity }).ToArray()
            };

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);

            return true;
        }

        private async Task<IEnumerable<CartLine>> GetCheckOutCartLines(Guid buyerId)
        {
            var cart = await _cartRepository.GetAsync(buyerId);
            if (cart == null) throw new CartNotFoundException(buyerId);

            var selectedLines = cart.Lines.Where(i => i.Selected);
            if (selectedLines.Count() == 0) throw new CartHasNoSelectedItemException(buyerId);

            return selectedLines;
        }
    }
    
    // Use for Idempotency in Command process
    public class CheckOutIdentifiedCommandHandler : IdentifiedCommandHandler<CheckOutCommand, bool>
    {
        public CheckOutIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}