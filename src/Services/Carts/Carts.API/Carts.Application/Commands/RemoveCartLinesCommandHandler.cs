using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class RemoveCartLinesCommandHandler : IRequestHandler<RemoveCartLinesCommand, bool>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<RemoveCartLinesCommandHandler> _logger;

        public RemoveCartLinesCommandHandler(ICartRepository cartRepository, ILogger<RemoveCartLinesCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveCartLinesCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(request.BuyerId);
            if (cart == null)
            {
                _logger.LogWarning($"----- Cart ({nameof(Cart.BuyerId)}={request.BuyerId}) is not found. No item is removed.");
                return true;
            }

            cart.RemoveLines(request.ProductIds);

            _cartRepository.Update(cart);

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class RemoveCartLinesIdentifiedCommandHandler : IdentifiedCommandHandler<RemoveCartLinesCommand, bool>
    {
        public RemoveCartLinesIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}