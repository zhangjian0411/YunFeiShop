using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartItemNotFoundException : ApplicationException
    {
        public CartItemNotFoundException(Guid buyerId, Guid productId)
            : base($"Cart - {{BuyerId: {buyerId}}} doesn't have a item - {{ProductId: {productId}}}.") { }
    }
}