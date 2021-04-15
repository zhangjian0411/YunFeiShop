using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models
{
    public record OrderLine(Guid ProductId, string Name, int Quantity);
}