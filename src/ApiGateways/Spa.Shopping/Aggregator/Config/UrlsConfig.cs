using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Config
{
    public class UrlsConfig
    {
        public CartOperations CartOperations => new CartOperations(Cart);
        public CatalogOperations CatalogOperations => new CatalogOperations(Catalog);

        public string Cart { get; set; }
        public string Catalog { get; set; }
        public string Order { get; set; }
    }

    public class CartOperations
    {
        private string _host;

        public CartOperations(string host) => _host = host;

        public string AddItemToCart() => $"{_host}/Cart/AddItemToCart";
        public string UpdateOrCreateCartLine() => $"{_host}/Cart/UpdateOrCreateCartLine";
        public string RemoveCartLines() => $"{_host}/Cart/RemoveCartLines";
        public string CheckOut() => $"{_host}/Cart/CheckOut";
        public string GetCart(Guid buyerId) => $"{_host}/Cart/{buyerId}";
    }

    public class CatalogOperations
    {
        private string _host;
        public CatalogOperations(string host) => _host = host;

        public string GetCatalogItems() => $"{_host}/catalog";
    }

    public class OrderOperations
    {
        private string _host;
        public OrderOperations(string host) => _host = host;

        public string PlaceOrder() => $"{_host}/Order/PlaceOrder";
    }
}