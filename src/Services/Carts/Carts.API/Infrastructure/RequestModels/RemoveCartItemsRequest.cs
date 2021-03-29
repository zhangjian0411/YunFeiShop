using System;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.RequestModels
{
    public class RemoveCartItemsRequest
    {
        public Guid[] ProductIds { get; init; }
    }
}