using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<AddItemToCartCommandHandler> _logger;

        public AddItemToCartCommandHandler(ICartRepository cartRepository, ILogger<AddItemToCartCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(request.BuyerId);

            if (cart == null)
            {
                cart = new Cart(request.BuyerId);
                cart.AddItem(request.ProductId);
                _cartRepository.Add(cart);
            }
            else
            {
                cart.AddItem(request.ProductId);
                _cartRepository.Update(cart);
            }

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class AddItemToCartIdentifiedCommandHandler : IdentifiedCommandHandler<AddItemToCartCommand, bool>
    {
        public AddItemToCartIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}