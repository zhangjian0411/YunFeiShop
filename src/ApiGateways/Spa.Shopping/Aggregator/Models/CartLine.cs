using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models
{
    public record CartLine(Guid ProductId, int Quantity, bool Selected);
}