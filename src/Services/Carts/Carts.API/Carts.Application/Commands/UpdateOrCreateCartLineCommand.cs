using System;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record UpdateOrCreateCartLineCommand(Guid BuyerId, Guid ProductId, int Quantity, bool Selected) : IRequest<bool>;
}