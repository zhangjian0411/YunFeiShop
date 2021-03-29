using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartHasNoItemException : ApplicationException
    {
        public CartHasNoItemException(Guid buyerId)
            : base($"Cart - {{BuyerId: {buyerId}}} has no item.") { }
    }
}