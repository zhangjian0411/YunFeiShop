using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Config;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services.Implements
{
    public class CatalogHttpClient : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(Guid[] ids)
        {
            var url = ApiUrls.Catalog.GetCatalogItems();
            
            return await _httpClient.GetFromJsonAsync<IEnumerable<CatalogItem>>(url);

        }
    }
}