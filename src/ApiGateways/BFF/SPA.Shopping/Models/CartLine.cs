using System;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models
{
    public record CartLine(Guid ProductId, int Quantity, bool Selected);
}