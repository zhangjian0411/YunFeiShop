using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class UpdateOrCreateCartItemCommandHandler : IRequestHandler<UpdateOrCreateCartItemCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateOrCreateCartItemCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(UpdateOrCreateCartItemCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(command.BuyerId);
            if (cart == null) cart = new Cart(command.BuyerId);

            var item = cart.Items.SingleOrDefault(i => i.ProductId == command.ProductId);
            if (item == null) item = cart.AddItem(command.ProductId);

            item.Quantity = command.Quantity;
            item.Selected = command.Selected;

            _cartRepository.Update(cart);

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class UpdateOrCreateCartItemIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateOrCreateCartItemCommand, bool>
    {
        public UpdateOrCreateCartItemIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}