using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(Guid[] ids);
    }
}