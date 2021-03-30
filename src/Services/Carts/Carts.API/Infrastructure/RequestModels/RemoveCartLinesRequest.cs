using System;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.RequestModels
{
    public class RemoveCartLinesRequest
    {
        public Guid[] ProductIds { get; init; }
    }
}