using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class AddItemToCartCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
        public Guid ProductId { get; init; }
    }
}