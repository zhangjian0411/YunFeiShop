using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartHasNoSelectedItemException : ApplicationException
    {
        public CartHasNoSelectedItemException(Guid buyerId)
            : base($"Cart - {{BuyerId: {buyerId}}} has not select a item.") { }
    }
}