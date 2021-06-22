using System;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models
{
    public record CatalogItem(Guid Id, string Name, decimal UnitPrice)
    {
        public string Name { get; init; } = Name ?? string.Empty;
    }
}