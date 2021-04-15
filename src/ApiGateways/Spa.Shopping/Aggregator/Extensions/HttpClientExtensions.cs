using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator
{
    public static class HttpClientExtensions
    {
        public static async Task PutWithIdAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value)
        {
            await httpClient.SendWithIdAsync(HttpMethod.Put, requestUri, value);
        }

        public static async Task PostWithIdAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value)
        {
            await httpClient.SendWithIdAsync(HttpMethod.Post, requestUri, value);
        }

        private static async Task SendWithIdAsync<TValue>(this HttpClient httpClient, HttpMethod method, string requestUri, TValue value)
        {
            var request = new HttpRequestMessage(method, requestUri);

            request.Content = new StringContent(JsonSerializer.Serialize(value, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            request.Headers.Add("x-requestid", Guid.NewGuid().ToString());

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}