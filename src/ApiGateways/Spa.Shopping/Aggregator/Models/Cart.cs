using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models
{
    public record Cart(Guid BuyerId, CartLine[] Lines)
    {
        public CartLine[] Lines { get; init; } = Lines ?? new CartLine[0];
    }
}