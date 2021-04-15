using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Config;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public class CatalogHttpService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;
        
        public CatalogHttpService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(Guid[] ids)
        {
            var url = _urls.CatalogOperations.GetCatalogItems();

            return await _httpClient.GetFromJsonAsync<IEnumerable<CatalogItem>>(url);
        }
    }
}