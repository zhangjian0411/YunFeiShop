using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public record RemoveCartLinesCommand(Guid BuyerId, Guid[] ProductIds) : IRequest<bool>;
}