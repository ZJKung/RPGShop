using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Net;

namespace WebMVC.Infrastructure
{
    public class CustomHttpClient 
    {
        private readonly IHttpClientFactory _clientFactory;
        private ILogger<CustomHttpClient> _logger;

        public CustomHttpClient(IHttpClientFactory clientFactory, ILogger<CustomHttpClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await client.SendAsync(request);
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            var requestMessage = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(item)
                    , Encoding.UTF8, "application/json")
            };
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }
    }
}