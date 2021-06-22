using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(Guid[] ids);
    }
}