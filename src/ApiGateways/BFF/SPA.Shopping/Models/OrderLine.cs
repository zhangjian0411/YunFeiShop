using System;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models
{
    public record OrderLine(Guid ProductId, string Name, int Quantity);
}