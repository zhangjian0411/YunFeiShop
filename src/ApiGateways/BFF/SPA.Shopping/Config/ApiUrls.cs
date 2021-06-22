using System;
using Microsoft.Extensions.Options;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Config
{
    public class ApiUrls
    {
        public class Catalog
        {
            public static string GetCatalogItems() => "/catalog";
        }

        public class Cart
        {
            public static string AddItemToCart() => "Cart/AddItemToCart";
            public static string UpdateOrCreateCartLine() => "Cart/UpdateOrCreateCartLine";
            public static string RemoveCartLines() => "Cart/RemoveCartLines";
            public static string CheckOut() => "Cart/CheckOut";
            public static string GetCart(string buyerId) => $"Cart/{buyerId}";
        }

        public class Order
        {
            public static string PlaceOrder() => "/Order/PlaceOrder";
            public static string GetOrders(string buyerId) => $"/Order?buyerId={buyerId}";
        }
    }
}