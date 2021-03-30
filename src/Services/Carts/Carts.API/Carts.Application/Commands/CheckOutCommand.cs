using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class CheckOutCommand : IRequest<bool>
    {
        public Guid BuyerId { get; init; }
    }
}