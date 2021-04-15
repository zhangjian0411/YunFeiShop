using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models
{
    public record CatalogItem(Guid Id, string Name, decimal UnitPrice)
    {
        public string Name { get; init; } = Name ?? string.Empty;
    }
}