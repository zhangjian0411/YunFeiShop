using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class UpdateOrCreateCartLineCommandHandler : IRequestHandler<UpdateOrCreateCartLineCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateOrCreateCartLineCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(UpdateOrCreateCartLineCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(command.BuyerId);
            cart = cart ?? new Cart(command.BuyerId);

            var line = cart.Lines.SingleOrDefault(i => i.ProductId == command.ProductId);
            line = line ?? cart.AddItem(command.ProductId);

            line.Quantity = command.Quantity;
            line.Selected = command.Selected;

            _cartRepository.Update(cart);

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    // Use for Idempotency in Command process
    public class UpdateOrCreateCartLineIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateOrCreateCartLineCommand, bool>
    {
        public UpdateOrCreateCartLineIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}