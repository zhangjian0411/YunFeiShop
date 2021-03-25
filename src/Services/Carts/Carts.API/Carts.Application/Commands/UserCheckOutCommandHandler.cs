using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;
using static ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events.UserCheckoutAcceptedIntegrationEvent;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class UserCheckOutCommandHandler : IRequestHandler<UserCheckOutCommand, bool>
    {
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ICartRepository _cartRepository;

        public UserCheckOutCommandHandler(IIntegrationEventService integrationEventService, ICartRepository cartRepository)
        {
            _integrationEventService = integrationEventService;
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(UserCheckOutCommand request, CancellationToken cancellationToken)
        {
            var userCheckoutAcceptedIntegrationEvent = new UserCheckoutAcceptedIntegrationEvent 
            { 
                UserId = request.UserId, 
                CheckoutLines = request.OrderLines.Select(ol => new CheckoutLine { ProductId = ol.ProductId, ProductName = ol.Name, Quantity = ol.Quantity }).ToArray()
            };

            await _integrationEventService.AddAndSaveEventAsync(userCheckoutAcceptedIntegrationEvent);

            var cart = new Cart 
            { 
                BuyerId = new Guid("BBBBBBBB-BBBB-BBBB-DDDD-AAAAAAAAAAAA"),
                Lines = new[] 
                { 
                    new CartLine { ProductId = new Guid("AAAAAAAA-AAAA-BBBB-DDDD-AAAAAAAAAAAA"), Quantity = 1, Selected = false }
                } 
            };
            _cartRepository.Add(cart);

            await _cartRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }

    // Use for Idempotency in Command process
    public class UserCheckOutIdentifiedCommandHandler : IdentifiedCommandHandler<UserCheckOutCommand, bool>
    {
        public UserCheckOutIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UserCheckOutCommand, bool>> logger)
            : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for processing order.
        }
    }
}