using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record UpdateCartItemCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public bool Selected { get; init; }
    }
}