using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record RemoveCartItemsCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
        public Guid[] ProductIds { get; init; }
    }
}