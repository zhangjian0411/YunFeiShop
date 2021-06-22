using System;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models
{
    public record Order(Guid BuyerId, OrderLine[] OrderLines);
}