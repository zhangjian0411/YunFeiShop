using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record AddItemToCartCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
        public Guid ProductId { get; init; }
    }
}