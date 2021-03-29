using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        public CheckOutCommandHandler(ICartRepository cartRepository, IIntegrationEventService integrationEventService)
        {
            _cartRepository = cartRepository;
            _integrationEventService = integrationEventService;
        }

        public async Task<bool> Handle(CheckOutCommand command, CancellationToken cancellationToken)
        {
            var cartItems = await GetCheckOutCartItems(command.BuyerId);

            var integrationEvent = new UserCheckoutAcceptedIntegrationEvent 
            {
                UserId = command.BuyerId,
                CheckoutLines = cartItems.Select(i => new CheckoutLine { ProductId = i.ProductId, Quantity = i.Quantity }).ToArray()
            };

            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);

            return true;
        }

        private async Task<IEnumerable<CartItem>> GetCheckOutCartItems(Guid buyerId)
        {
            var cart = await _cartRepository.GetAsync(buyerId);
            if (cart == null) throw new CartNotFoundException(buyerId);

            var selectedItems = cart.Items.Where(i => i.Selected);
            if (selectedItems.Count() == 0) throw new CartHasNoSelectedItemException(buyerId);

            return selectedItems;
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