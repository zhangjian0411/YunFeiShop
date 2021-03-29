using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class RemoveCartItemsCommandHandler : IRequestHandler<RemoveCartItemsCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartItemsCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(RemoveCartItemsCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetAsync(request.BuyerId);
            if (cart == null) throw new CartNotFoundException(request.BuyerId);

            cart.RemoveItems(request.ProductIds);

            _cartRepository.Update(cart);

            return await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}