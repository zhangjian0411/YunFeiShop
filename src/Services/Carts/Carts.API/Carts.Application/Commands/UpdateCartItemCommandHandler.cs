using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateCartItemCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(command.BuyerId);
            if (cart == null) throw new CartNotFoundException(command.BuyerId);

            var item = cart.Items.SingleOrDefault(i => i.ProductId == command.ProductId);
            if (item == null) throw new CartItemNotFoundException(command.BuyerId, command.ProductId);

            item.Quantity = command.Quantity;
            item.Selected = command.Selected;

            _cartRepository.Update(cart);

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}