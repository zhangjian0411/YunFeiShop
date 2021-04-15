using System;
using System.Collections.Generic;
using static ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers.DraftOrder;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers
{
    public record UpdateCartLineRequest(Guid ProductId, int Quantity, bool Selected);
    public record UpdateCartLineResponse(decimal TotalPrice);

    public record RemoveCartLineResponse(decimal TotalPrice);

    public record DraftOrder(Guid BuyerId, DraftOrderLine[] Lines)
    {
        public record DraftOrderLine(Guid ProductId, string Name, int Quantity, decimal UnitPrice);
    }
}