using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebMVC.Infrastructure;
using WebMVC.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        // private CustomHttpClient _apiClient;
        private readonly IHttpClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private ILogger<CatalogService> _logger;
        private string _remoteServiceBaseUrl;

        public CatalogService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CatalogService> logger)
        {
            _settings = settings;
            _apiClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/catalog/";
        }
        public async Task<Catalog> GetCatalogItems(int pageIndex, int pageSize, int? type)
        {
            var allcatalogItemsUri = ApiPaths.Catalog.GetAllCatalogItems(
                _remoteServiceBaseUrl,
                pageIndex,
                pageSize,
                type);
            var dataString = await _apiClient.GetStringAsync(allcatalogItemsUri);
            var response = JsonSerializer.Deserialize<Catalog>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var getTypesUri = ApiPaths.Catalog.GetAllTypes(_remoteServiceBaseUrl);
            var dataString = await _apiClient.GetStringAsync(getTypesUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var types = JsonDocument.Parse(dataString);
            var brands = types.RootElement.EnumerateArray();

            foreach (var brand in brands)
            {
                var a = brand.GetProperty("id");
                items.Add(new SelectListItem()
                {
                    Value = brand.GetProperty("id").ToString(),
                    Text = brand.GetProperty("type").ToString()
                });
            }

            return items;
        }
    }
}