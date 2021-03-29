using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record CheckOutCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
    }
}